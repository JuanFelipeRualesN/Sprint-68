namespace GSAIServicesAPI.Models.Images
{
    /// <summary>
    /// Configuration for the blob storage backend that holds generated images.
    /// </summary>
    /// <remarks>
    /// Bound from configuration at startup and consumed as a singleton, so instances are
    /// treated as immutable after binding; mutating a property at runtime is not observed
    /// by components that have already captured the value.
    /// </remarks>
    public class BlobStorageOptions
    {
        /// <summary>
        /// Credential-bearing connection string for the storage account. Empty by default,
        /// which means the storage client cannot be constructed.
        /// </summary>
        /// <remarks>
        /// Holds an account key or SAS token, so it belongs in a secret store rather than in
        /// checked-in configuration. <see cref="UseSasUri"/> requires a connection string that
        /// carries an account key; a connection string authenticated by other means cannot sign
        /// a SAS.
        /// </remarks>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Name of the container that generated images are written to. Defaults to
        /// <c>generated-images</c>.
        /// </summary>
        public string ContainerName { get; set; } = "generated-images";

        /// <summary>When true, returns a time-limited SAS URI instead of the raw blob URI.</summary>
        /// <remarks>
        /// Defaults to <see langword="true"/>. When false, callers receive the raw blob URI,
        /// which is only usable if the container permits anonymous read.
        /// </remarks>
        public bool UseSasUri { get; set; } = true;

        /// <summary>
        /// Lifetime of an issued SAS URI, in hours. Defaults to <c>24</c>.
        /// </summary>
        /// <remarks>
        /// Ignored when <see cref="UseSasUri"/> is false. The expiry is fixed at the moment the
        /// URI is generated, so a URI handed to a client outlives neither this window nor the
        /// rotation of the account key that signed it.
        /// </remarks>
        public int SasValidForHours { get; set; } = 24;

        /// <summary>
        /// Hosts whose URLs are treated as belonging to this application's storage.
        /// Include the blob endpoint and any CDN or custom domain in front of it.
        /// </summary>
        public List<string> KnownHosts { get; set; } = new();
    }
}