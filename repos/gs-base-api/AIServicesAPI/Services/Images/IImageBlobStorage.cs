namespace GSAIServicesAPI.Services.Images
{
    /// <summary>
    /// Stores image payloads supplied as Base64 text and returns URIs by which they can be read back.
    /// </summary>
    /// <remarks>
    /// Implementations must be safe for concurrent use by multiple callers, since the batch overload
    /// invokes the single-image path in parallel and consumers are expected to resolve a single
    /// instance for the lifetime of the application. Implementations choose the blob name; callers
    /// cannot influence it and must not derive one from the returned URI, as the naming scheme is not
    /// part of this contract. The lifetime of a returned URI is also implementation-defined: it may
    /// carry a time-limited credential and expire, so callers should treat it as a value to hand on
    /// promptly rather than persist indefinitely.
    /// </remarks>
    public interface IImageBlobStorage
    {
        /// <summary>Decodes a Base64 payload and uploads it, returning the blob URI.</summary>
        /// <param name="base64">
        /// The image bytes encoded as Base64. Whether a data-URI prefix such as
        /// <c>data:image/png;base64,</c> is tolerated is implementation-defined; callers should pass
        /// the bare encoding.
        /// </param>
        /// <param name="cancellationToken">Token used to abandon the upload.</param>
        /// <returns>
        /// An absolute URI addressing the stored image. Never <see langword="null"/> or empty on
        /// successful completion.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="base64"/> is not valid Base64 text.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was signalled before the upload completed.
        /// </exception>
        /// <remarks>
        /// Implementations are not required to be idempotent: uploading the same payload twice may
        /// produce two blobs and two distinct URIs.
        /// </remarks>
        Task<string> UploadBase64Async(string base64, CancellationToken cancellationToken = default);

        /// <summary>Uploads several Base64 payloads in parallel, preserving input order.</summary>
        /// <param name="base64Images">
        /// The payloads to upload, each encoded as described for the single-image overload. The
        /// sequence is enumerated once.
        /// </param>
        /// <param name="cancellationToken">Token used to abandon the remaining uploads.</param>
        /// <returns>
        /// The URIs of the stored images. The result has the same count as
        /// <paramref name="base64Images"/> and corresponds to it positionally: element <c>i</c> of
        /// the result is the URI of element <c>i</c> of the input, regardless of the order in which
        /// the uploads actually completed.
        /// </returns>
        /// <exception cref="FormatException">
        /// At least one element of <paramref name="base64Images"/> is not valid Base64 text.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was signalled before all uploads completed.
        /// </exception>
        /// <remarks>
        /// Failure is not atomic. If one payload fails, payloads that already succeeded remain
        /// stored, and their URIs are lost to the caller because the exception carries no partial
        /// result — implementations that need cleanup must perform it themselves. The degree of
        /// parallelism is implementation-defined and unbounded as far as this contract is concerned,
        /// so callers should not pass sequences of arbitrary size.
        /// </remarks>
        Task<IReadOnlyList<string>> UploadBase64Async(
            IEnumerable<string> base64Images,
            CancellationToken cancellationToken = default);
    }
}