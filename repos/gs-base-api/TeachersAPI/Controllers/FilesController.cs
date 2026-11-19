using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TeachersAPI.CustomClasses;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Generals;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;
using TeachersAPI.Utils;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// To handle files such files upload, files' rules, etc.
    /// </summary>
    [ApiController]
    [Route("teachersapi/files")]
    public class FilesController : ControllerBase
    {
        [HttpGet("admin/host/ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok();
        }

        [HttpPost("admin/host/ping")]
        public async Task<IActionResult> PingPost()
        {
            return Ok();
        }

        [HttpGet("get/{fileName}/{extension}")]
        public async Task<IActionResult> GetFile(string fileName, string extension)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "UploadFiles");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                string fileNewName = fileName + "." + extension;
                string fileUrl = AzureAssets.GetFileFromAzureStorage(fileNewName);
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Uploads a file via multipart/form-data.
        /// </summary>
        /// <remarks>
        /// Use an API client/app that sends multipart/form-data (not application/json).
        /// </remarks>
        [HttpPost("upload")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        public async Task<IActionResult> UploadFile()
            => await HandleUploadAsync(onSaved: null, langCode: null, authorizationToken: null);

        /// <summary>
        /// Uploads a file associated with a quiz via multipart/form-data.
        /// </summary>
        /// <param name="authorizationToken">Quiz authorization token.</param>
        /// <param name="langCode">Language code for messages.</param>
        [HttpPost("quiz/upload/{authorizationToken}/{langCode}")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        public async Task<IActionResult> UploadFileQuiz(string authorizationToken, string langCode)
            => await HandleUploadAsync(SaveQuizFileAsync, langCode, authorizationToken);

        // ------------------------- Shared core -------------------------
        private async Task<IActionResult> HandleUploadAsync(
            Func<string, string, Task>? onSaved,
            string? langCode,
            string? authorizationToken)
        {
            // 1) Session check
            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "UploadFiles");
            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            // 2) Content-Type and file presence
            if (!Request.HasFormContentType)
                return Unsupported("Unsupported media type. Use multipart/form-data.");

            var form = await Request.ReadFormAsync();
            if (form.Files.Count == 0)
                return BadRequestResp("No files were provided.");

            var file = form.Files[0];
            var fileName = file.FileName ?? "upload";
            var dot = fileName.LastIndexOf('.');
            if (dot < 0 || dot == fileName.Length - 1)
                return BadRequestResp("Archivo no válido. No se pudo determinar la extensión.");

            var ext = fileName[(dot + 1)..];

            // 3) Rules
            var rules = new SupportedFileDAO().Read(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                new SupportedFileModel(), false, false);

            var rule = rules.Find(x => string.Equals(x.Format, ext, StringComparison.OrdinalIgnoreCase));
            if (rule is null)
                return BadRequestResp($"Archivo no válido. Extensión no permitida: .{ext}; Nombre: {file.FileName}");

            if (!long.TryParse(rule.MaxSize, out var maxBytes))
                return BadRequestResp("Configuración de tamaño máximo inválida.");

            if (file.Length > maxBytes)
            {
                var lang = langCode ?? RequestHandler.GetLanguage(HttpContext);
                var msg = new MessageManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], lang)
                    .GetMessageDetails(AppConfiguration.Configuration["AppSettings:gt_message_uploaded_file_exceeds_max_size"]);
                return BadRequestResp(msg);
            }

            // 4) Upload
            var newName = $"{Guid.NewGuid()}.{ext}";
            using var stream = file.OpenReadStream();
            var urlObj = await AzureAssets.UploadFileToAzureStorage(stream, newName);

            // 5) Optional side-effect (e.g., tie to quiz)
            if (onSaved != null)
                await onSaved(newName, authorizationToken!);

            return StatusCode(StatusCodes.Status201Created, OkResp(urlObj?.ToString() ?? string.Empty, newName));

            // ---- local helpers ----
            IActionResult Unsupported(string msg) => StatusCode(StatusCodes.Status415UnsupportedMediaType, FailResp(msg));
            IActionResult BadRequestResp(string msg) => BadRequest(FailResp(msg));
            Domain.Models.Generals.Responses.ResponseModel FailResp(string msg) => new()
            {
                SessionValid = true,
                TransactionValid = false,
                Messages = new List<string> { msg }
            };
            Domain.Models.Generals.Responses.ResponseModel OkResp(string url, string name) => new()
            {
                SessionValid = true,
                TransactionValid = true,
                Messages = new List<string> { url, name }
            };
        }

        // Persist quiz-file linkage
        private async Task SaveQuizFileAsync(string newFileName, string authorizationToken)
        {
            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "UploadFiles");

            var mgr = new UserQuizManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                RequestHandler.GetLanguage(HttpContext));

            var auth = mgr.GetUserQuizAuthorizationCodeModels(new List<UserQuizAuthorizationCodeModel>
        {
            new() { UserId = session.UserId, AuthorizationToken = authorizationToken }
        });

            if (auth.Count > 0)
            {
                mgr.CreateUserQuizFileModel(new UserQuizFileModel
                {
                    ModifiedBy = session.UserId,
                    UserId = session.UserId,
                    QuizId = auth[0].CustomQuizId,
                    FileUrl = newFileName
                });
            }
        }

        /*
        /// <summary>
        /// To upload files that are related to a homework. Be aware that you should send the header and body that are generated by the APIs, 
        /// or programs like Fiddler, to upload files. For example, Fiddler creates the content-type as multipart/formdata, and the request body included the details 
        /// of the internal files. So do not test this endpoint directly, unless you are using an API or application to upload files. This Content-Type is 
        /// different from the other calls that do not upload files, which are usually application/json but this one is autogenerated by those APIs.
        /// </summary>
        /// <returns>Retuns a HTTPResponse with files details.</returns>
        [HttpPost, Route("api/files/quiz/upload/{authorizationToken}/{langCode}/")]
        public async Task<IActionResult> UploadFileQuiz(string authorizationToken, string langCode)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "UploadFiles");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Get the file rules details 
            var supportedFileModels = new
                SupportedFileDAO().Read(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                new SupportedFileModel(), false, false);

            // 2) Ensure multipart/form-data
            if (!Request.HasFormContentType)
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType,
                    new Domain.Models.Generals.Responses.ResponseModel
                    {
                        SessionValid = true,
                        TransactionValid = false,
                        Messages = new List<string> { "Unsupported media type. Use multipart/form-data." }
                    });
            }

            // Now we need to identify the format to specify the root folder 
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var stream = await file.ReadAsStreamAsync();

                // Check the extension
                var extensionFile =
                    filename.Substring(filename.LastIndexOf('.')); // Get format 
                extensionFile = extensionFile.Substring(1);

                // Find supported file
                var supportedFileModel =
                    supportedFileModels.Find(x => x.Format.ToLower() == extensionFile.ToLower());

                // Check if extension file is valid 
                if (supportedFileModel == null)
                {
                    HttpResponseMessage httpResponseMessage = Request.CreateResponse(
                        HttpStatusCode.UnsupportedMediaType,
                        new TeacherAPI.Domain.Models.Generals.Responses.ResponseModel()
                        {
                            SessionValid = true,
                            TransactionValid = false,
                        }, Configuration.Formatters.JsonFormatter);
                    throw new HttpResponseException(httpResponseMessage);
                }

                if (stream.Length > int.Parse(supportedFileModel.MaxSize))
                {
                    // Too heavy 
                    return Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        new TeacherAPI.Domain.Models.Generals.Responses.ResponseModel()
                        {
                            SessionValid = true,
                            TransactionValid = false,
                            Messages = new List<string>()
                            {
                                new TeacherAPI.Services.Generals.MessageManager(
                                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                    langCode).GetMessageDetails(
                                    AppConfiguration.Configuration["AppSettings:gt_message_uploaded_file_exceeds_max_size"])
                            }
                        },
                        Configuration.Formatters.JsonFormatter);
                }

                string newFileName = Guid.NewGuid().ToString() + "." + extensionFile;

                // Now, create the file
                object fileUrl = await AzureAssets.UploadFileToAzureStorage(stream, newFileName);


                //string pathName = ConfigurationManager.AppSettings["webservice_url"] + ConfigurationManager.AppSettings["gt_files_folder"] + newFileName;
                var userQuizAuthorizationCodeModelList =
                    new List<UserQuizAuthorizationCodeModel>();

                // Insert the path name to authorization code in a NEW TABLE 

                // Get quiz Id by token and authorization 
                var userQuizManager = new UserQuizManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    langCode);
                userQuizAuthorizationCodeModelList = userQuizManager.GetUserQuizAuthorizationCodeModels(
                    new List<UserQuizAuthorizationCodeModel>()
                    {
                        new UserQuizAuthorizationCodeModel()
                        {
                            UserId = responseSession.UserId,
                            AuthorizationToken = authorizationToken,
                        }
                    });

                if (userQuizAuthorizationCodeModelList.Count > 0)
                {
                    userQuizManager.CreateUserQuizFileModel(new UserQuizFileModel()
                    {
                        ModifiedBy = responseSession.UserId,
                        UserId = responseSession.UserId,
                        QuizId = userQuizAuthorizationCodeModelList[0].CustomQuizId,
                        FileUrl = newFileName
                    });
                }

                return StatusCode(StatusCodes.Status201Created,
                  new Domain.Models.Generals.Responses.ResponseModel
                  {
                      SessionValid = true,
                      TransactionValid = true,
                      Messages = new List<string>
                      {
                        fileUrl.ToString(),
                        newFileName
                      }
                  });
            }

            return BadRequest(new Domain.Models.Generals.Responses.ResponseModel
            {
                SessionValid = true,
                TransactionValid = false,
                Messages = new List<string> { "Archivo no válido. No se pudo determinar la extensión." }
            });
        }


        // Note: File upload endpoints require more work to adapt to ASP.NET Core's IFormFile, etc.
        // For brevity, only the basic structure is shown here. You should use IFormFile for file uploads in ASP.NET Core.
        /// <summary>
        /// Upload files via multipart/form-data. 
        /// Use an API client (e.g., app or Fiddler/Postman) that builds the multipart body.
        /// </summary>
        /// <remarks>
        /// Content-Type must be multipart/form-data. 
        /// </remarks>
        /// <returns>HTTP response with file details.</returns>
        [HttpPost("upload")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        public async Task<ActionResult<Domain.Models.Generals.Responses.ResponseModel>> UploadFile()
        {
            string pathNameE = string.Empty;
            string urlE = string.Empty;

            try
            {
                // 1) Check access/session (adapted from your original helper)
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "UploadFiles");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // 2) Ensure multipart/form-data
                if (!Request.HasFormContentType)
                {
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType,
                        new Domain.Models.Generals.Responses.ResponseModel
                        {
                            SessionValid = true,
                            TransactionValid = false,
                            Messages = new List<string> { "Unsupported media type. Use multipart/form-data." }
                        });
                }

                var form = await Request.ReadFormAsync();
                if (form.Files.Count == 0)
                {
                    return BadRequest(new Domain.Models.Generals.Responses.ResponseModel
                    {
                        SessionValid = true,
                        TransactionValid = false,
                        Messages = new List<string> { "No files were provided." }
                    });
                }

                // 3) Load supported file rules (kept as-is, just .NET 8 style)
                var supportedFileModels = new SupportedFileDAO().Read(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    new SupportedFileModel(), false, false);

                // Process the first file (same behavior as your original, which returns inside the loop)
                IFormFile file = form.Files[0];
                var fileName = file.FileName ?? "upload";
                var lastDot = fileName.LastIndexOf('.');
                if (lastDot < 0 || lastDot == fileName.Length - 1)
                {
                    return BadRequest(new Domain.Models.Generals.Responses.ResponseModel
                    {
                        SessionValid = true,
                        TransactionValid = false,
                        Messages = new List<string> { "Archivo no válido. No se pudo determinar la extensión." }
                    });
                }

                var extensionFile = fileName[(lastDot + 1)..]; // without dot
                var supported = supportedFileModels.Find(x =>
                    string.Equals(x.Format, extensionFile, StringComparison.OrdinalIgnoreCase));

                if (supported is null)
                {
                    return BadRequest(new Domain.Models.Generals.Responses.ResponseModel
                    {
                        SessionValid = true,
                        TransactionValid = false,
                        Messages = new List<string>
                    {
                        $"Archivo no válido. Extensión no permitida: .{extensionFile}; Nombre: {file.FileName}"
                    }
                    });
                }

                // 4) Size check (SupportedFileModel.MaxSize assumed to be bytes)
                if (!long.TryParse(supported.MaxSize, out var maxBytes))
                {
                    // Fallback or treat as invalid rule
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Domain.Models.Generals.Responses.ResponseModel
                        {
                            SessionValid = true,
                            TransactionValid = false,
                            Messages = new List<string> { "Configuración de tamaño máximo inválida." }
                        });
                }

                if (file.Length > maxBytes)
                {
                    var lang = RequestHandler.GetLanguage(HttpContext);
                    var message = new MessageManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                        lang).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gt_message_uploaded_file_exceeds_max_size"]);

                    return BadRequest(new Domain.Models.Generals.Responses.ResponseModel
                    {
                        SessionValid = true,
                        TransactionValid = false,
                        Messages = new List<string> { message }
                    });
                }

                // 5) Upload to Azure
                var newFileName = $"{Guid.NewGuid()}.{extensionFile}";
                using var stream = file.OpenReadStream();
                var fileUrlObj = await AzureAssets.UploadFileToAzureStorage(stream, newFileName);

                return StatusCode(StatusCodes.Status201Created,
                    new Domain.Models.Generals.Responses.ResponseModel
                    {
                        SessionValid = true,
                        TransactionValid = true,
                        Messages = new List<string>
                        {
                        fileUrlObj?.ToString() ?? string.Empty,
                        newFileName
                        }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex} PATH {pathNameE} URL {urlE}");
            }
        }
        */

        [HttpGet("file-rules")]
        public async Task<IActionResult> GetFileRules()
        {
            try
            {
                CustomClasses.CacheHandler.FilesCache filesCache = new CustomClasses.CacheHandler.FilesCache();
                List<SupportedFileModel> attachmentRuleModels = filesCache.GetFileAttachmentRules();
                if (attachmentRuleModels != null && attachmentRuleModels.Count > 0)
                {
                    return Ok(new SupportedFilesResponseModel
                    {
                        TransactionValid = true,
                        SessionValid = true,
                        SupportedFileModelList = attachmentRuleModels
                    });
                }
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "QuizTypesRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                List<SupportedFileModel> supportedFiles = new SupportedFileDAO().Read(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], new SupportedFileModel(), false, false);
                filesCache.SetFileAttachmentRules(supportedFiles);
                SupportedFilesResponseModel supportedFilesResponseModel = new SupportedFilesResponseModel
                {
                    TransactionValid = true,
                    SessionValid = true,
                    SupportedFileModelList = supportedFiles
                };
                return Ok(supportedFilesResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteFile([FromBody] SupportedFileModel fileExampleModel)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "UploadFiles");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                await AzureAssets.DeleteFileFromAzureStorage(fileExampleModel.FileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
