using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;

namespace GSWebAPI.GSWebAPI.Services.Community
{
    /// <summary>
    /// Which files can be uploaded as community resources, and a check that the content matches the extension.
    /// <list type="bullet">
    /// <item>Text documents: Word (doc, docx), OpenDocument / LibreOffice / OpenOffice (odt), rtf, txt and pdf.</item>
    /// <item>Presentations: PowerPoint (ppt, pptx, pps, ppsx) and OpenDocument (odp).</item>
    /// <item>Spreadsheets: Excel (xls, xlsx) and OpenDocument (ods).</item>
    /// <item>Media: mp4 (video) and mp3 (audio).</item>
    /// </list>
    /// The stored content type comes from the extension, never from the client. The first bytes must match the format
    /// (for example <c>%PDF</c> for pdf, a ZIP header for docx/pptx/odt, the OLE header for doc/ppt), so a renamed
    /// executable is rejected.
    /// </summary>
    public static class CommunityResourceFiles
    {
        /// <summary>Maximum size of an uploaded resource file: 100 MB (videos).</summary>
        public const long MaxFileBytes = 100L * 1024 * 1024;

        /// <summary>Bytes read from the start of the file to check its format.</summary>
        internal const int HeaderLength = 512;

        private enum Format
        {
            Pdf,
            Zip,
            Ole,
            Rtf,
            Text,
            Mp3,
            Mp4,
        }

        private sealed record FileKind(string ContentType, CommunityResourceType ResourceType, Format Format);

        private static readonly Dictionary<string, FileKind> Kinds = new(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = new("application/pdf", CommunityResourceType.Document, Format.Pdf),
            [".doc"] = new("application/msword", CommunityResourceType.Document, Format.Ole),
            [".docx"] = new("application/vnd.openxmlformats-officedocument.wordprocessingml.document", CommunityResourceType.Document, Format.Zip),
            [".odt"] = new("application/vnd.oasis.opendocument.text", CommunityResourceType.Document, Format.Zip),
            [".rtf"] = new("application/rtf", CommunityResourceType.Document, Format.Rtf),
            [".txt"] = new("text/plain", CommunityResourceType.Document, Format.Text),
            [".ppt"] = new("application/vnd.ms-powerpoint", CommunityResourceType.Document, Format.Ole),
            [".pps"] = new("application/vnd.ms-powerpoint", CommunityResourceType.Document, Format.Ole),
            [".pptx"] = new("application/vnd.openxmlformats-officedocument.presentationml.presentation", CommunityResourceType.Document, Format.Zip),
            [".ppsx"] = new("application/vnd.openxmlformats-officedocument.presentationml.slideshow", CommunityResourceType.Document, Format.Zip),
            [".odp"] = new("application/vnd.oasis.opendocument.presentation", CommunityResourceType.Document, Format.Zip),
            [".xls"] = new("application/vnd.ms-excel", CommunityResourceType.Document, Format.Ole),
            [".xlsx"] = new("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CommunityResourceType.Document, Format.Zip),
            [".ods"] = new("application/vnd.oasis.opendocument.spreadsheet", CommunityResourceType.Document, Format.Zip),
            [".mp4"] = new("video/mp4", CommunityResourceType.Video, Format.Mp4),
            [".mp3"] = new("audio/mpeg", CommunityResourceType.Audio, Format.Mp3),
        };

        /// <summary>The allowed extensions (with the dot), for documentation and error messages.</summary>
        public static IReadOnlyCollection<string> AllowedExtensions => Kinds.Keys;

        /// <summary>Content type and resource type of an allowed extension; false when the extension is not allowed.</summary>
        /// <param name="extension">Extension with the dot, e.g. <c>.pdf</c>.</param>
        /// <param name="contentType">Content type to store with the blob.</param>
        /// <param name="resourceType">Resource type to store (document, video or audio).</param>
        /// <returns>True when the extension is allowed.</returns>
        public static bool TryGetType(string? extension, out string contentType, out CommunityResourceType resourceType)
        {
            if (!string.IsNullOrEmpty(extension) && Kinds.TryGetValue(extension, out var kind))
            {
                contentType = kind.ContentType;
                resourceType = kind.ResourceType;
                return true;
            }

            contentType = string.Empty;
            resourceType = CommunityResourceType.Link;
            return false;
        }

        /// <summary>True when the first bytes of the file match the format of its (allowed) extension.</summary>
        /// <param name="extension">Extension with the dot.</param>
        /// <param name="header">The first bytes of the file (up to <see cref="HeaderLength"/>).</param>
        /// <returns>False for unknown extensions or content that does not match.</returns>
        public static bool MatchesContent(string extension, ReadOnlySpan<byte> header)
        {
            if (!Kinds.TryGetValue(extension, out var kind))
            {
                return false;
            }

            return kind.Format switch
            {
                Format.Pdf => header.StartsWith("%PDF-"u8),
                Format.Zip => header.StartsWith("PK\u0003\u0004"u8),
                Format.Ole => header.StartsWith(new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }),
                Format.Rtf => header.StartsWith(@"{\rtf"u8),
                Format.Mp4 => header.Length >= 8 && header.Slice(4, 4).SequenceEqual("ftyp"u8),
                Format.Mp3 => header.StartsWith("ID3"u8) || (header.Length >= 2 && header[0] == 0xFF && (header[1] & 0xE0) == 0xE0),
                Format.Text => header.Length > 0 && !header.Contains((byte)0),
                _ => false,
            };
        }
    }
}
