using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeachersAPI.CustomClasses;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Generals;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Utils;
using TeachersAPI;

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
                string fileUrl = AwsAssets.GetSignedUrlFronCloudFront(fileNewName);
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // Note: File upload endpoints require more work to adapt to ASP.NET Core's IFormFile, etc.
        // For brevity, only the basic structure is shown here. You should use IFormFile for file uploads in ASP.NET Core.

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
                await AwsAssets.DeleteObjectIntoS3BucketAsync(fileExampleModel.FileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
