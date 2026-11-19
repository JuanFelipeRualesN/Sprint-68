namespace GSAIServicesAPI.Models.Images
{
    /// <summary>Outcome of a generate-then-persist operation.</summary>
    /// <param name="UpstreamSucceeded">
    /// <see langword="true"/> when the generation provider returned a response body, whether or not
    /// it contained any images; <see langword="false"/> when it returned nothing.
    /// </param>
    /// <param name="ImageUrls">
    /// The URIs of the stored images, corresponding positionally to the payloads returned by the
    /// provider: element <c>i</c> addresses the <c>i</c>th generated image. Empty when nothing was
    /// generated and also when storage failed, in which case any images that were already written
    /// are not reported here.
    /// </param>
    /// <param name="StorageFailed">
    /// <see langword="true"/> when generation succeeded but persisting the payloads did not. Always
    /// <see langword="false"/> when <paramref name="UpstreamSucceeded"/> is <see langword="false"/>,
    /// since storage is not attempted in that case.
    /// </param>
    /// <param name="GeneratedCount">
    /// How many images the provider actually produced, independent of how many were stored, 
    /// so a storage failure still charges for what was generated.
    /// </param>
    /// <remarks>
    /// Immutable and safe to share across threads, subject to the caller not mutating the list passed
    /// as <paramref name="ImageUrls"/>: the record captures the reference rather than copying, and
    /// <see cref="System.Collections.Generic.IReadOnlyList{T}"/> does not guarantee the underlying
    /// instance is itself immutable.
    /// <para>
    /// The two flags are not independent and only four combinations are produced. Callers should
    /// branch on both rather than on <paramref name="ImageUrls"/> being empty, since an empty list
    /// alone does not distinguish a provider that generated nothing from a storage failure.
    /// </para>
    /// </remarks>
    public sealed record GeneratedImagesResult(
        bool UpstreamSucceeded,
        IReadOnlyList<string> ImageUrls,
        bool StorageFailed,
        int GeneratedCount = 0);
}