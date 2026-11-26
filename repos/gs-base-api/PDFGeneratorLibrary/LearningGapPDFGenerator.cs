using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;

namespace PDFGeneratorLibrary
{
    public class LearningGapPdfGenerator
    {
        // Single converter instance, works on Windows + Linux
        private static readonly Lazy<SynchronizedConverter> _lazyConverter =
            new Lazy<SynchronizedConverter>(() => new SynchronizedConverter(new PdfTools()));

        private static SynchronizedConverter HtmlToPdfConverter => _lazyConverter.Value;

        /// <summary>
        /// TO generate a pdf file for learning sheets.
        /// </summary>
        /// <param name="langCode">The language's code.</param>
        /// <param name="athenasLearningSheet">The learning sheet.</param>
        /// <param name="imgUrlBanner">The URL of the image banner.</param>
        /// <returns>Returns the url where the file is stored.</returns>
        public static async Task<byte[]> GenerateLearningSheetPDF(string langCode, LearningSheetModel athenasLearningSheet, string imgUrlBanner)
        {
            // --- 1. BUILD HTML & LOCALIZED TEXT (Same for all OS) ---

            string competencyText;
            string competencyExplanationPart1;
            string competencyExplanationPart2;
            string whyIsItImportant;

            if (langCode == "es")
            {
                competencyText = "Competencia: ";
                competencyExplanationPart1 = @"Según los resultados de las evaluaciones, el estudiante debe reforzar conceptos relacionados a: ";
                competencyExplanationPart2 = "Le recomendamos lo siguiente para mejorar su dominio.";
                whyIsItImportant = "¿Por qué es importante?";
            }
            else
            {
                competencyText = "Competency: ";
                competencyExplanationPart1 = @"According to the evaluation results, the student must reinforce concepts related to: ";
                competencyExplanationPart2 = "The following is recommended to improve mastery.";
                whyIsItImportant = "Why is it important?";
            }

            var currentYear = DateTime.Now.Year;

            string htmlToGenerate = $@"
<html>
  <head>
    <meta charset='UTF-8' />
    <style>
      body {{
        font-family: sans-serif;
        font-size: 12px;
        margin: 0;
        padding: 0;
      }}
      .content-wrapper {{
        padding: 20px;
      }}
      .footer {{
        position: fixed;
        bottom: 10px;
        left: 0;
        right: 0;
        text-align: center;
        font-size: 10px;
        color: #555;
      }}
    </style>
  </head>
  <body>
    <div class='content-wrapper'>
      <img src=""{imgUrlBanner}"" style=""width:100%;height:100px;""/>
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
            {athenasLearningSheet?.TextImportance}
          </td>
        </tr>
      </table>
    </div>

    <div class='footer'>
      Estudia y Avanza {currentYear}
    </div>
  </body>
</html>";

            // --- 2. GENERATE PDF (same for Windows & Linux using WkHtmlToPdfDotNet) ---

            string fileName = athenasLearningSheet.Id + langCode + ".pdf";
            byte[] pdfBytes = GeneratePdf_FromHtml(htmlToGenerate);

            return pdfBytes;
            // --- 3. COMMON FILE HANDLING ---

           // await using var stream = new MemoryStream(pdfBytes);
           // stream.Position = 0;

           // await AzureAssets.UploadFileToAzureStorage(stream, fileName);

           // string signedUrl = AzureAssets.GetFileFromAzureStorage(fileName);
           // return signedUrl;
        }

        /// <summary>
        /// Generates PDF using WkHtmlToPdfDotNet (cross-platform).
        /// Requires wkhtmltopdf binary installed in the environment.
        /// </summary>
        private static byte[] GeneratePdf_FromHtml(string htmlToGenerate)
        {
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings
                    {
                        Top = 10,
                        Right = 20,
                        Bottom = 10,
                        Left = 20
                    },
                    Out = null
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = htmlToGenerate,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            return HtmlToPdfConverter.Convert(doc);
        }
    }
}
