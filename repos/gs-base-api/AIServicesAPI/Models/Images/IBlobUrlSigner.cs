namespace GSAIServicesAPI.Services.Images
{
    /// <summary>
    /// Translates between the canonical, unsigned form of a blob URL stored in the database
    /// and the signed form handed to clients.
    /// </summary>
    public interface IBlobUrlSigner
    {
        /// <summary>
        /// Determines whether a URL points at this application's container and, if so,
        /// extracts the blob name with any SAS query string removed.
        /// </summary>
        /// <returns><c>true</c> when the URL is ours; <c>false</c> for external URLs.</returns>
        bool TryGetBlobName(string url, out string? blobName);

        /// <summary>Returns the unsigned absolute URL for a blob name.</summary>
        string ToCanonicalUrl(string blobName);

        /// <summary>Returns a time-limited read URL for a blob name.</summary>
        string ToSignedUrl(string blobName);
    }
}