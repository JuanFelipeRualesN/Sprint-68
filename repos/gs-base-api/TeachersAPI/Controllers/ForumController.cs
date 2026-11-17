using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Requests;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/forums")]
    public class ForumController : ControllerBase
    {
        [HttpGet("teacher")]
        public async Task<IActionResult> GetTeacherForums(string teacherPlanId = null, int fromPage = 0, int limiter = 10, string title = null, DateTime? createdAt = null)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var result = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).ReadTeacherForums(token: responseSession.Token,
                ipAddress: responseSession.IpAddress, createdAt: createdAt, title: title, teacherPlanId: teacherPlanId,
                userIdMakingRequest: responseSession.UserId, fromPage: fromPage,
                limiter: limiter);
            return Ok(result);
        }

        [HttpGet("student")]
        public async Task<IActionResult> GetStudentForums(int page = 0, int pageSize = 10, string subjectCode = null, string levelCode = null)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var result = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
                GetStudentForums(responseSession.Token, responseSession.IpAddress, responseSession.UserId, responseSession.LangCode, page, pageSize, subjectCode, levelCode);
            return Ok(result);
        }

        [HttpDelete("{forumId}/post/{postId}")]
        public async Task<IActionResult> ArchivePostForum(string forumId, string postId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumPostHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var teacherForum = new ForumManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).ArchivePost(postId, userSessionId: responseSession.UserId);
            if (!teacherForum.Valid) return BadRequest(teacherForum);
            return Ok(teacherForum);
        }

        [HttpGet("{forumId}")]
        public async Task<IActionResult> GetForumDetails(string forumId, int fromPage = 0, int limiter = 10, string forumParentId = null)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var forumManager = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                     AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                     responseSession.LangCode);
                var result = forumManager.ReadForumPosts(responseSession.Token,
                            responseSession.IpAddress,
                            responseSession.LangCode,
                            forumId,
                            fromPage,
                            limiter,
                            responseSession.UserId,
                            true,
                            forumParentId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateForum([FromBody] Domain.Models.Teachers.ForumModel forumModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var forumResponse = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
                CreateForum(forumModel, responseSession.UserId);
            if (!forumResponse.Valid) return BadRequest(forumResponse);
            return Created(string.Empty, forumResponse);
        }

        [HttpDelete("{forumId}")]
        public async Task<IActionResult> ArchiveForumUpdated(string forumId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var teacherForum = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode);
            var forumResponse = teacherForum.ArchiveForum(forumId, responseSession.UserId, responseSession.IsStaff);
            if (!forumResponse.Valid) return BadRequest(forumResponse);
            return Ok(forumResponse);
        }

        [HttpPut("{forumId}")]
        public async Task<IActionResult> ModifyForum(string forumId, [FromBody] Domain.Models.Teachers.ForumModel forumModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            forumModel.Id = forumId;
            var forumResponse = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).ModifyForum(forumModel, responseSession.UserId);
            if (!forumResponse.Valid) return BadRequest(forumResponse);
            return Ok(forumResponse);
        }

        [HttpPost("{forumId}/post")]
        public async Task<IActionResult> CreatePostForum(string forumId, [FromBody] ForumPostModel forumPostModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumPostHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            forumPostModel.ForumId = forumId;
            var forumManager = new ForumManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode);
            var createdForum = forumManager.CreatePost(forumPostModel, responseSession.UserId, responseSession.FullName);
            if (!createdForum.Valid) return BadRequest(createdForum);
            return Created(string.Empty, createdForum);
        }

        [HttpPut("{forumId}/post/{postId}")]
        public async Task<IActionResult> ModifyPostForum(string forumId, string postId, [FromBody] ForumPostModel forumPostModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumPostHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var teacherForum = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                 responseSession.LangCode);
            forumPostModel.ForumId = forumId;
            forumPostModel.Id = postId;
            var forumResponse = teacherForum.ModifyPost(forumPostModel, responseSession.UserId,
                responseSession.FullName);
            if (!forumResponse.Valid) return BadRequest(forumResponse);
            return Ok(forumResponse);
        }
    }
}
