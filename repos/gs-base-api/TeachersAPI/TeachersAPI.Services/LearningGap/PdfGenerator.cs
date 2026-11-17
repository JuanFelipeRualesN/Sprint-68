using CacheHandlerLibrary;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Utils;
using HtmlRendererCore.PdfSharp;
using Microsoft.AspNetCore.StaticFiles; 
using PdfSharpCore.Drawing;             
using PdfSharpCore.Pdf;                 
using System;
using System.IO;
using System.Threading.Tasks;

namespace TeachersAPI.TeachersAPI.Services.LearningGap
{
    /// <summary>
    /// To generate a pdf file.
    /// </summary>
    public class PdfGenerator
    {
        /// <summary>
        /// TO generate a pdf file for learning sheets.
        /// </summary>
        /// <param name="langCode">The language's code.</param>
        /// <param name="athenasLearningSheet">The learning sheet.</param>
        /// <returns>Returns the url where the file is stored.</returns>
        public static async Task<string> GenerateLearningSheetPDF(string langCode, LearningSheetModel athenasLearningSheet)
        {
            string competencyText = "";
            string competencyExplanationPart1 = "";
            string competencyExplanationPart2 = "";
            string whyIsItImportant = "";

            if (langCode == "es")
            {
                competencyText = "Competencia: ";
                competencyExplanationPart1 = @"Según los resultados de las evaluaciones, 
                    el estudiante debe reforzar conceptos relacionados a: ";
                competencyExplanationPart2 = "Le recomendamos lo siguiente para mejorar su dominio.";
                whyIsItImportant = "¿Por qué es importante?";
            }
            else
            {
                competencyText = "Competency: ";
                competencyExplanationPart1 = @"According to the evaluation results, 
                    the student must reinforce concepts related to: ";
                competencyExplanationPart2 = "The following is recommended to improve mastery.";
                whyIsItImportant = "Why is it important?";
            }

            string htmlToGenerate = $@"
                <html>
                  <body style=""font-family: sans-serif;"">
                    <img src=""{AppConfiguration.Configuration["AppSettings:img_src_learning_gap_banner"]}"" style=""width:100%;height:100px;""/>
                   <table style=""margin-top:15px;width:100%;border:solid;"">
                      <tr style=""border:solid;"">
                        <td style=""border:none;background-color: #c5dded; height: 45px;padding: 10px"">
                            <b>{competencyText} 
                            {athenasLearningSheet?.KeyWord}
                            </b></td>
                      </tr>
                      <tr>
                        <td style=""border:none;"">{competencyExplanationPart1} 
                        <b>{athenasLearningSheet?.KeyWord}</b>. 
                        {competencyExplanationPart2} 
                        </td>
                      </tr>
                    </table>
                   <div style=""margin-left:5%;margin-top:15px;width:90%;"">
                      {athenasLearningSheet?.TextDescription}
                    </div>
                    <table style=""margin-top:15px;width:100%;border:solid;"">
                      <tr style=""border:solid;"">
                        <td style=""border:none;background-color: #c5dded; height:45px;padding: 10px""><b>{whyIsItImportant}</b></td>
                      </tr>
                      <tr>
                        <td style=""border:none;"">
                         {athenasLearningSheet?.TextImportance}</td>
                      </tr>
                    </table>
                  </body>
                </html>";

            var config = new PdfGenerateConfig
            {
                PageSize = PdfSharpCore.PageSize.A4,
                MarginTop = 10,
                MarginRight = 20,
                MarginBottom = 10,
                MarginLeft = 20
            };

            // Generate PDF using HtmlRendererCore.PdfSharp + PdfSharpCore
            PdfDocument pdf = HtmlRendererCore.PdfSharp.PdfGenerator.GeneratePdf(htmlToGenerate, config);

            // Add footer to each page
            foreach (PdfPage page in pdf.Pages)
            {
                using var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 8, XFontStyle.Regular);
                var brush = XBrushes.Black;
                double x = page.Width / 2.0;     // Centered
                double y = page.Height - 20.0;   // Near bottom

                gfx.DrawString("Estudia y Avanza " + DateTime.Now.Year, font, brush, new XPoint(x, y), XStringFormats.Center);
            }

            // Save and upload
            await using var stream = new MemoryStream();
            pdf.Save(stream);
            stream.Position = 0;

            string fileName = athenasLearningSheet.Id + langCode + ".pdf";

            // Replace MimeMapping with ASP.NET Core provider
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
                contentType = "application/pdf";

            string fullFilePath = $"{AwsAssets.FolderPath}{fileName}";

            await AwsAssets.WritingObjectToS3BucketAsync(fullFilePath, stream, contentType);

            string signedUrl = AwsAssets.GetSignedUrlFronCloudFront(fullFilePath);
            return signedUrl;
        }
    }
}
