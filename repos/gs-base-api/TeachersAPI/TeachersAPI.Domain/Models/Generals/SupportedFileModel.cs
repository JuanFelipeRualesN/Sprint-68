namespace TeachersAPI.Domain.Models.Generals
{
    /// <summary>
    /// To handle supported files.
    /// </summary>
    public class SupportedFileModel
    {
        /// <summary>
        /// Gets or sets the supported file.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the classifcation (if image, audio, etc).
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the max width.
        /// </summary>
        public string MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the max height.
        /// </summary>
        public string MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the max size.
        /// </summary>
        public string MaxSize { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the user id who modified the file.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created at date.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at date.
        /// </summary>
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Extra: Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

    }
}