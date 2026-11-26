using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the supported files in Teachers API.
    /// </summary>
    public class SupportedFile
    {
        public int Id { get; set; }

        /// <summary>
        /// The file classification: image, file, etc.
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// The file&apos;s format: jpg, png, mp3, etc.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The allowed max width for the file (if it is an image.).
        /// </summary>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// The allowed max height for the file (if it is an image.).
        /// </summary>
        public int? MaxHeight { get; set; }

        /// <summary>
        /// The allowed max size for the file.
        /// </summary>
        public int? MaxSize { get; set; }

        /// <summary>
        /// The directory where the file is uploaded.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
