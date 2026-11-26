using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using UglyToad.PdfPig;
using System.Text;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Handles file upload and text extraction for PDF, DOCX, and TXT files.
    /// </summary>
    [Route("aiservicesapi/[controller]")]
    public class ExtractTextController : BaseController
    {
        private const string Module = "AIExtractText";

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            "pdf", "docx", "txt"
        };

        public ExtractTextController(IConfiguration config) : base(config) { }

        /// <summary>
        /// Accepts a file upload and returns its extracted plain text content.
        /// </summary>
        /// <remarks>
        /// <b>POST</b> /api/extracttext
        /// <br/><br/>
        /// Supported types: PDF, DOCX, TXT.
        /// Returns a JSON object with filename, size in bytes, character count, and extracted text.
        /// </remarks>
        [HttpPost]
        [RequestSizeLimit(20 * 1024 * 1024)] // 20 MB cap
        public async Task<IActionResult> Post(IFormFile file)
        {
            var (isValid, _) = await GetAuthenticatedUserAsync(Module);
            if (!isValid)
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });

            if (file == null || file.Length == 0)
                return BadRequest(new { error = GetLocalizedMessage("NoFileUploaded") });

            var ext = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
            var mime = file.ContentType?.ToLowerInvariant() ?? string.Empty;

            if (!AllowedExtensions.Contains(ext))
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType,
                    new { error = string.Format(GetLocalizedMessage("UnsupportedFileType"), ext) });
            }

            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var buffer = ms.ToArray();

                string text = ext switch
                {
                    "pdf" => ExtractFromPdf(buffer),
                    "docx" => ExtractFromDocx(buffer),
                    "txt" => Encoding.UTF8.GetString(buffer),
                    _ => throw new InvalidOperationException("Unreachable")
                };

                text = NormalizeWhitespace(text);

                return Ok(new
                {
                    filename = file.FileName,
                    bytes = file.Length,
                    chars = text.Length,
                    text
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = string.Format(GetLocalizedMessage("ExtractionFailed"), ex.Message) });
            }
        }

        // ── Private helpers ──────────────────────────────────────────────────

        private static string ExtractFromPdf(byte[] buffer)
        {
            using var doc = PdfDocument.Open(buffer);
            var sb = new StringBuilder();
            foreach (var page in doc.GetPages())
                sb.AppendLine(page.Text);
            return sb.ToString();
        }

        private static string ExtractFromDocx(byte[] buffer)
        {
            using var ms = new MemoryStream(buffer);
            using var wordDoc = WordprocessingDocument.Open(ms, false);
            var body = wordDoc.MainDocumentPart?.Document?.Body;
            if (body == null) return string.Empty;

            var sb = new StringBuilder();
            foreach (var para in body.Elements<Paragraph>())
            {
                sb.AppendLine(para.InnerText);
            }
            return sb.ToString();
        }

        private static string NormalizeWhitespace(string input) =>
            System.Text.RegularExpressions.Regex
                .Replace(input.Replace("\r", ""), @"\n{3,}", "\n\n")
                .Trim();
    }
}