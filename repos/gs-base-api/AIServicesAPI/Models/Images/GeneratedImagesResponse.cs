namespace GSAIServicesAPI.Models.Images
{
    /// <summary>
    /// Response body returned to API clients after a generate-then-persist request, carrying the
    /// locations of the stored images.
    /// </summary>
    /// <remarks>
    /// A serialisation contract: shaped for the wire and bound by JSON property name, so renaming a
    /// member is a breaking change for clients. Mutable and not thread-safe; construct one per
    /// response and do not share instances.
    /// </remarks>
    public class GeneratedImagesResponse
    {
        /// <summary>
        /// The URIs of the stored images, in the order the provider generated them. Defaults to an
        /// empty list, which is what clients receive when nothing was generated and also when
        /// generation succeeded but storage did not.
        /// </summary>
        /// <remarks>
        /// The setter allows the list to be replaced with <see langword="null"/>, which serialises as
        /// a JSON null rather than an empty array; callers assigning this property should assign an
        /// empty list instead. Individual URIs may carry a time-limited credential and expire, so
        /// clients should fetch them promptly rather than store them.
        /// </remarks>
        public List<string> ImageUrls { get; set; } = new();
    }
}