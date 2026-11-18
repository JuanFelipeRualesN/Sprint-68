using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Services.Utils;
using Microsoft.AspNetCore.Http;

// WkHtmlToPdfDotNet (cross-platform: Windows + Linux)
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace GSWebAPI.CustomClasses
{
    /// <summary>
    /// To generate the PDF result of the user responses.
    /// </summary>
    public static class PDFResultGenerator
    {
        /// <summary>
        /// Generates a PDF of the user responses.
        /// </summary>
        public static async Task<string> GeneratePDF(
            ProfileReportsResponseModel profileReportsResponseModel,
            bool generatePdfForCloud,
            IHttpContextAccessor? httpContextAccessor = null)
        {
            // --- 1. BUILD HTML ---
            string htmlString = BuildHtmlContent(profileReportsResponseModel);

            // --- 2. GENERATE PDF (same code for Windows & Linux) ---
            byte[] pdfBytes = GeneratePdf_WkHtml(htmlString);

            // --- 3. COMMON FILE HANDLING ---
            using var stream = new MemoryStream(pdfBytes);
            stream.Position = 0;

            string fileName = Guid.NewGuid() + ".pdf";

            if (!generatePdfForCloud)
            {
                // Save locally (temp folder)
                string localReportsDirectory = Path.Combine(Path.GetTempPath(), "LocalPDFReports");
                Directory.CreateDirectory(localReportsDirectory);

                string localFilePath = Path.Combine(localReportsDirectory, fileName);
                stream.Position = 0;
                using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                // Try to build a public URL if we have an HTTP context
                var request = httpContextAccessor?.HttpContext?.Request;
                if (request != null)
                {
                    string scheme = request.Scheme;
                    string host = request.Host.Value;
                    string url = $"{scheme}://{host}/reports/{fileName}";
                    return url;
                }

                return localFilePath;
            }
            else
            {
                // Upload to Azure (existing implementation)
                return await AzureAssets.UploadFileToAzureStorage(stream, fileName);
            }
        }

        /// <summary>
        /// Generates PDF using WkHtmlToPdfDotNet (Windows & Linux).
        /// Requires wkhtmltopdf binary installed in the environment.
        /// </summary>
        private static byte[] GeneratePdf_WkHtml(string htmlString)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    ColorMode = ColorMode.Color,
                    Margins = new MarginSettings {
                        Top = 20,
                        Right = 20,
                        Bottom = 20,
                        Left = 20
                    },
                    Out = null
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = htmlString,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return WkHtmlConverterSingleton.Instance.Convert(doc);
        }

        /// <summary>
        /// Centralized method for building the common HTML content.
        /// </summary>
        private static string BuildHtmlContent(ProfileReportsResponseModel profileReportsResponseModel)
        {
            string htmlString = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Quiz Report</title>
    <style>
        body { font-family: 'Arial', sans-serif; font-size: 12px; line-height: 1.6; color: #333; margin: 0; padding: 0; }
        .container { width: 100%; max-width: 700px; margin: 0 auto; padding: 20px; box-sizing: border-box; }
        .header-section { text-align: center; margin-bottom: 30px; border-bottom: 1px solid #eee; padding-bottom: 20px; }
        .header-section img { max-width: 150px; height: auto; margin-bottom: 15px; }
        .header-table { width: 100%; border-collapse: collapse; margin-top: 15px; border: 1px solid #ddd; }
        .header-table td { padding: 8px 10px; vertical-align: top; text-align: left; border-bottom: 1px solid #eee; }
        .header-table tr:nth-child(odd) { background-color: #f9f9f9; }
        .header-table tr:nth-child(even) { background-color: #f0f0f0; }
        .header-table tr:last-child td { border-bottom: none; }
        .header-table .label { font-weight: bold; color: #555; width: 40%; }
        .header-table .value { color: #333; width: 60%; }
        .section-title { font-size: 16px; font-weight: bold; color: #444; margin-top: 25px; margin-bottom: 10px; border-bottom: 1px solid #eee; padding-bottom: 5px; }
        .question-block { margin-bottom: 20px; padding-bottom: 15px; border-bottom: 1px dashed #eee; }
        .question-block:last-child { border-bottom: none; }
        .question-premise { font-size: 14px; font-weight: bold; color: #333; margin-bottom: 8px; }
        .user-response { font-size: 12px; margin-bottom: 5px; padding-left: 15px; }
        .correct-response { color: #28a745; font-weight: bold; }
        .incorrect-response { color: #dc3545; font-weight: bold; }
        .text-center { text-align: center; }
        .mt-10 { margin-top: 10px; }
        .mb-10 { margin-bottom: 10px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header-section'>";

            // Logo
            string? logoUrl = AppConfiguration.Configuration["AppSettings:logo_url"];
            if (!string.IsNullOrWhiteSpace(logoUrl))
            {
                htmlString += $"<img src='{logoUrl}' alt='Logo'>";
            }

            htmlString += $@"
            <table class='header-table'>
                <tr class='header-row'>
                    <td class='label'>Nombre / Name:</td>
                    <td class='value'>{profileReportsResponseModel.StudentName}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Prueba / Quiz:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.QuizTitle}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Lección / Lesson:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.LessonTitle}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Materia / Subject:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.Subject}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Grado / Grade:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.Grade}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Fecha / Date Taken:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.CreatedAt}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Total:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.TotalPercent}%</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Intento / Attempt:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.TryNo}</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Nivel de pensamiento / Depth of Knowledge:</td>
                    <td class='value'>{profileReportsResponseModel.UserQuizFullDetails.DoK}</td>
                </tr>
            </table>
        </div>";

            // Instructions
            htmlString += $@"
        <div class='section-content'>
            <h2 class='section-title text-center'>Instrucciones / Instructions</h2>
            <div class='mt-10 mb-10'>{profileReportsResponseModel.UserQuizFullDetails.Instructions}</div>";

            // Extra Info
            if (!string.IsNullOrWhiteSpace(profileReportsResponseModel.UserQuizFullDetails.ExtraInfo))
            {
                htmlString += $@"
            <h2 class='section-title text-center'>Información extra / Extra Info</h2>
            <div class='mt-10 mb-10'>{profileReportsResponseModel.UserQuizFullDetails.ExtraInfo}</div>";
            }

            // Q&A
            htmlString += "<h2 class='section-title text-center'>Preguntas y Respuestas / Questions & Answers</h2>";
            int questionNo = 1;
            foreach (var question in profileReportsResponseModel.UserQuizFullDetails.QuestionQuizList)
            {
                htmlString += $@"
            <div class='question-block'>
                <p class='question-premise'>Pregunta / Question {questionNo}: {question.Question}</p>";

                foreach (var response in question.QuestionResponses)
                {
                    string responseClass = response.Correct ? "correct-response" : "incorrect-response";
                    string statusText = response.Correct ? "Correct / Correcto" : "Incorrect / Incorrecto";
                    htmlString += $@"
                <p class='user-response'>Respuesta del estudiante / User Response: <b>{response.Response}</b></p>
                <p class='user-response {responseClass}'>{statusText}</p>";
                }
                htmlString += "</div>";
                questionNo++;
            }

            htmlString += @"
        </div>
    </div>
</body>
</html>";

            return htmlString;
        }
    }
}