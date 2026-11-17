using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PdfSharp.Pdf;
using System;
using System.Configuration; // if you're using ConfigurationManager for logo_url
using System.IO;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

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
        /// <param name="profileReportsResponseModel">Profile result details.</param>
        /// <param name="generatePdfForCloud">To generate the PDF for Cloud.</param>
        /// <param name="httpContextAccessor">Optional: inject to build absolute URL (ASP.NET Core).</param>
        /// <returns>Returns the URL of the file (or local path if no HTTP context).</returns>
        public static async Task<string> GeneratePDF(
            ProfileReportsResponseModel profileReportsResponseModel,
            bool generatePdfForCloud,
            IHttpContextAccessor? httpContextAccessor = null)
        {
            // Build HTML
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

            htmlString += @"
            <table class='header-table'>
                <tr class='header-row'>
                    <td class='label'>Nombre / Name:</td>
                    <td class='value'>" + profileReportsResponseModel.StudentName + @"</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Prueba / Quiz:</td>
                    <td class='value'>" + profileReportsResponseModel.UserQuizFullDetails.QuizTitle + @"</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Total:</td>
                    <td class='value'>" + profileReportsResponseModel.UserQuizFullDetails.TotalPercent + @"%</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Intento / Attempt:</td>
                    <td class='value'>" + profileReportsResponseModel.UserQuizFullDetails.TryNo + @"</td>
                </tr>
                <tr class='header-row'>
                    <td class='label'>Nivel de pensamiento / Depth of Knowledge:</td>
                    <td class='value'>" + profileReportsResponseModel.UserQuizFullDetails.DoK + @"</td>
                </tr>
            </table>
        </div>";

            // Instructions
            htmlString += @"
        <div class='section-content'>
            <h2 class='section-title text-center'>Instrucciones / Instructions</h2>
            <div class='mt-10 mb-10'>" + profileReportsResponseModel.UserQuizFullDetails.Instructions + @"</div>";

            // Extra Info
            if (!string.IsNullOrWhiteSpace(profileReportsResponseModel.UserQuizFullDetails.ExtraInfo))
            {
                htmlString += @"
            <h2 class='section-title text-center'>Información extra / Extra Info</h2>
            <div class='mt-10 mb-10'>" + profileReportsResponseModel.UserQuizFullDetails.ExtraInfo + @"</div>";
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

            // PDF generation
            var config = new PdfGenerateConfig
            {
                PageSize = PdfSharp.PageSize.A4,
                MarginTop = 20,
                MarginRight = 20,
                MarginBottom = 20,
                MarginLeft = 20
            };

            PdfDocument pdf = PdfGenerator.GeneratePdf(htmlString, config);

            using var stream = new MemoryStream();
            pdf.Save(stream, false);
            stream.Position = 0;

            string fileName = Guid.NewGuid() + ".pdf";
            string contentType = "application/pdf";

            if (!generatePdfForCloud)
            {
                // 1) Save the actual PDF bytes locally
                string localReportsDirectory = Path.Combine(Path.GetTempPath(), "LocalPDFReports");
                Directory.CreateDirectory(localReportsDirectory);

                string localFilePath = Path.Combine(localReportsDirectory, fileName);
                stream.Position = 0;
                using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                // 2) Build a public URL if we have an HTTP context; otherwise return local path
                var request = httpContextAccessor?.HttpContext?.Request;
                if (request != null)
                {
                    string scheme = request.Scheme;         // "http" or "https"
                    string host = request.Host.Value;       // "localhost:5000"
                                                            // Adjust the path segment ("reports") to match your static-file mapping if needed
                    string url = $"{scheme}://{host}/reports/{fileName}";
                    return url;
                }

                return localFilePath;
            }
            else
            {
                /* TODO: Substitue this for Azure Blob Storage 
                // Upload to S3/CloudFront
                string fullFilePath = $"{AwsAssets.FolderPath}{fileName}";
                stream.Position = 0;
                await AwsAssets.WritingObjectToS3BucketAsync(fullFilePath, stream, contentType);

                string signedUrl = AwsAssets.GetSignedUrlFronCloudFront(fullFilePath);

                return signedUrl;*/
                return nameof(NotImplementedException);
            }
        }
    }
}