using HtmlRendererCore.PdfSharp;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Handles PDF generation from HTML.
    /// </summary>
    public static class PdfHtmlGenerator
    {
        /// <summary>
        /// Generate a PDF from HTML/CSS and upload it to Azure Storage.
        /// </summary>
        /// <param name="htmlToGenerate">HTML markup.</param>
        /// <param name="cssData">Optional CSS stylesheet text.</param>
        /// <param name="fileName">File name without extension.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static async Task<string> GeneratePdfAsync(
            string htmlToGenerate,
            string? cssData,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(htmlToGenerate))
                throw new ArgumentException("HTML content cannot be null or empty.", nameof(htmlToGenerate));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("A file name is required.", nameof(fileName));

            var config = new PdfGenerateConfig
            {
                PageSize = PageSize.A4,
                MarginTop = 10,
                MarginRight = 20,
                MarginBottom = 10,
                MarginLeft = 20
            };

            // Parse stylesheet only if provided
            var stylesheet = !string.IsNullOrWhiteSpace(cssData)
                ? PdfGenerator.ParseStyleSheet(cssData!, true)
                : null;

            using PdfDocument pdf = stylesheet is null
                ? PdfGenerator.GeneratePdf(htmlToGenerate, config)
                : PdfGenerator.GeneratePdf(htmlToGenerate, config, stylesheet);

            using var stream = new MemoryStream();
            pdf.Save(stream, closeStream: false);
            stream.Position = 0; // rewind before upload

            // Your existing uploader (ensure it reads from current position)
            return await AzureAssets
                .UploadFileToAzureStorage(stream, $"{fileName}.pdf")
                .ConfigureAwait(false);
        }
    }
}
