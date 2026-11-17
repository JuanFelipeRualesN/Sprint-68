using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Generals.Responses.Reports;
using TeachersAPI.Utils;
using TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses.HTMLSanitizer;
using System.Web;
using System.Net;
using HtmlRendererCore.PdfSharp;
using Microsoft.AspNetCore.StaticFiles;

namespace TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses
{
    /// <summary>
    /// Handles the PDF Results.
    /// </summary>
    public static class PDFResultGenerator
    {
        /// <summary>
        /// Downloads an image from a given URL and saves it to a temporary directory.
        /// The file extension of the saved image matches the extension of the original image.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to download.</param>
        /// <returns>The path of the downloaded image in the temporary directory.</returns>
        public async static Task<string> DownloadImageToTempDir(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                var uri = new Uri(imageUrl);
                var extension = Path.GetExtension(uri.AbsolutePath);
                var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
                File.WriteAllBytes(tempPath, imageBytes);
                return tempPath;
            }
        }

        /// <summary>
        /// Generates a PDF of the user responses.
        /// The PDF includes the user's responses to a quiz, as well as additional information such as the quiz title, total score, attempt number, and depth of knowledge.
        /// Images included in the HTML string are downloaded and saved to a temporary directory, and their URLs are replaced with the local file paths in the HTML string.
        /// The HTML string is then converted to a PDF, which is saved to Azure storage.
        /// </summary>
        /// <param name="profileReportsResponseModel">Profile result details.</param>
        /// <returns>Returns the URL of the file.</returns>
        public static async Task<string> GeneratePDF(ReportPlanLessonQuizResponseModel profileReportsResponseModel)
        {
            try
            {
                // Prepare HTML
                string htmlString = "";
                htmlString += "<p><h5>Nombre / Name: <b>" + profileReportsResponseModel.StudentName + "</b><br/>";
                htmlString += "Prueba / Quiz: <b>" + profileReportsResponseModel.UserQuizFullDetails.QuizTitle + "</b><br/>";
                htmlString += "Total: <b>" + profileReportsResponseModel.UserQuizFullDetails.TotalPercent + "%</b><br/>";
                htmlString += "Nivel de pensamiento / Depth of Knowledge: <b>" + profileReportsResponseModel.UserQuizFullDetails.DoK + "</b></p></h5>";
                htmlString += "<hr>";

                htmlString += "<div style='text-align: center'>";
                htmlString += "<h5> Instrucciones / Instructions </h5>";
                htmlString += profileReportsResponseModel.UserQuizFullDetails.Instructions;
                htmlString += "</div>";

                if (!string.IsNullOrWhiteSpace(profileReportsResponseModel.UserQuizFullDetails.ExtraInfo))
                {
                    htmlString += "<p style='text-align:center'> Información extra / Extra info </p>";
                    htmlString += profileReportsResponseModel.UserQuizFullDetails.ExtraInfo;
                }

                int questionNo = 1;
                foreach (var question in profileReportsResponseModel.UserQuizFullDetails.QuestionQuizList)
                {
                    htmlString += "<p>Premisa / Premise " + questionNo + ": <b>" + question.Question + "</b></p>";
                    foreach (var response in question.QuestionResponses)
                    {
                        htmlString += "<p>Respuesta del estudiante / User Response: <b>" + response.Response + "</b></p>";
                        if (response.Correct == 1)
                            htmlString += "<p style='color: green'>Correct / Correcto</p>";
                        else
                            htmlString += "<p style='color: red'>Incorrect / Incorrecto</p>";
                    }
                    questionNo++;
                    htmlString += "<hr>";
                }

                // Replace <img src="http..."> with local temp files
                var matches = Regex.Matches(htmlString, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        string imageUrl = match.Groups[1].Value;
                        string localImagePath = await DownloadImageToTempDir(imageUrl);
                        htmlString = htmlString.Replace(imageUrl, localImagePath);
                    }
                }

                // Convert HTML to PDF (HtmlRendererCore.PdfSharp + PdfSharpCore)
                var config = new PdfGenerateConfig
                {
                    PageSize = PdfSharpCore.PageSize.Letter,
                    MarginTop = 10,
                    MarginRight = 20,
                    MarginBottom = 10,
                    MarginLeft = 20
                };

                var pdf = HtmlRendererCore.PdfSharp.PdfGenerator.GeneratePdf(Replacer.FixFontFamily(htmlString), config);

                await using var stream = new MemoryStream();
                pdf.Save(stream);
                stream.Position = 0;

                string fileName = Guid.NewGuid() + ".pdf";
                string filePath = $"{AwsAssets.FolderPath}{fileName}";

                // Replace MimeMapping with ASP.NET Core provider
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out var contentType))
                    contentType = "application/pdf";

                var resp = await AwsAssets.WritingObjectToS3BucketAsync(filePath, stream, contentType);

                if (resp.HttpStatusCode == HttpStatusCode.OK)
                    return AwsAssets.GetSignedUrlFronCloudFront(filePath);

                throw new Exception("S3 upload failed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}