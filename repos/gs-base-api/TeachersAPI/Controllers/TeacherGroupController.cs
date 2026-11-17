using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeachersAPI.CustomClasses;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Requests.APIs;
using System.Diagnostics;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/teacher-groups")]
    public class TeacherGroupController : ControllerBase
    {
        [HttpGet("{fromPage}/{limit}")]
        public async Task<IActionResult> GetTeacherGroups(int fromPage, int limit)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherGroupStudentModelList = TeacherGroups.GetTeacherGroups(responseSession.UserId);
                var teacherGroupResponse = new Domain.Models.Generals.Responses.TeacherGroupResponseModel
                {
                    TeacherGroupModelList = new List<TeacherGroupExtraDetailModel>(),
                };
                foreach (var teacherGroup in teacherGroupStudentModelList)
                {
                    if (teacherGroupResponse.TeacherGroupModelList.Find(x => x.Id == teacherGroup.GroupId) == null)
                    {
                        var studentsInGroup = teacherGroupStudentModelList.FindAll(x => x.GroupId == teacherGroup.GroupId);
                        int totalStudentsInGroup = 0;
                        if (studentsInGroup != null && studentsInGroup.Count > 0)
                        {
                            if (studentsInGroup.Count == 1 && studentsInGroup[0].StudentId == null)
                            {
                                totalStudentsInGroup = 0;
                            }
                            else
                            {
                                totalStudentsInGroup = studentsInGroup.Count;
                            }
                        }
                        teacherGroupResponse.TeacherGroupModelList.Add(new TeacherGroupExtraDetailModel
                        {
                            Available = teacherGroup.Available ? "1" : "0",
                            SubjectCode = teacherGroup.SubjectCode,
                            TeacherId = teacherGroup.TeacherId,
                            Name = teacherGroup.GroupName,
                            LevelCode = teacherGroup.LevelCode,
                            Id = teacherGroup.GroupId,
                            TotalStudents = totalStudentsInGroup,
                        });
                    }
                }
                teacherGroupResponse.TotalPages = int.Parse(Math.Ceiling((double)teacherGroupResponse.TeacherGroupModelList.Count() / limit).ToString());
                teacherGroupResponse.TeacherGroupModelList = teacherGroupResponse.TeacherGroupModelList != null && teacherGroupResponse.TeacherGroupModelList.Count() > 0 ? teacherGroupResponse.TeacherGroupModelList.Skip(fromPage * limit).Take(limit).ToList() : teacherGroupResponse.TeacherGroupModelList.ToList();
                return Ok(teacherGroupResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherGroupId(string id)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherGroupStudentModelList = TeacherGroups.GetTeacherGroups(responseSession.UserId, id);
                if (teacherGroupStudentModelList == null || teacherGroupStudentModelList.Count == 0)
                {
                    return Unauthorized();
                }
                var teacherGroupResponse = new Domain.Models.Generals.Responses.TeacherGroupResponseModel
                {
                    TeacherGroupModelList = new List<TeacherGroupExtraDetailModel>
                    {
                        new TeacherGroupExtraDetailModel
                        {
                            Available = "1",
                            SubjectCode = teacherGroupStudentModelList[0].SubjectCode,
                            LevelCode = teacherGroupStudentModelList[0].LevelCode,
                            Name = teacherGroupStudentModelList[0].GroupName,
                            TotalStudents = teacherGroupStudentModelList.Count(),
                            TeacherId = teacherGroupStudentModelList[0].TeacherId.ToString(),
                            Id = teacherGroupStudentModelList[0].GroupId.ToString()
                        }
                    },
                    StudentDetailModelList = new List<StudentDetail>()
                };
                var userIdsToCall = new List<string>();
                foreach (var teacherGroup in teacherGroupStudentModelList)
                {
                    if (teacherGroup.StudentId != null)
                    {
                        userIdsToCall.Add(teacherGroup.StudentId.ToString());
                    }
                }
                if (userIdsToCall.Count > 0)
                {
                    var profileConnectionModels = GetUserDetails.GetStudentsThatCanBeHandled(new UserRequestModel
                    {
                        UserIds = userIdsToCall,
                        SameSchoolRequired = true,
                        RoleId = "3"
                    }, responseSession.Token);
                    foreach (var teacherGroup in teacherGroupStudentModelList)
                    {
                        var userResponse = profileConnectionModels?.UserInfo.Find(x => x.UserId == teacherGroup.StudentId);
                        if (userResponse != null)
                        {
                            teacherGroupResponse.StudentDetailModelList.Add(new StudentDetail
                            {
                                UserSchoolRecord = userResponse.UserSchoolRecord,
                                FirstName = userResponse.GivenName,
                                InvitationCode = userResponse.Token,
                                LastName = userResponse.Surname,
                                UserId = userResponse.UserId,
                                Email = userResponse.Email
                            });
                        }
                    }
                }
                return Ok(teacherGroupResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("get-students-overall/{fromPage}/{limit}")]
        public async Task<IActionResult> GetTeacherStudents(int fromPage, int limit)
        {
            try
            {
                var profileConnectionModels = GetUserDetails.GetStudentsThatCanBeHandled(new UserRequestModel
                {
                    PageFrom = fromPage,
                    Limiter = limit,
                    SameSchoolRequired = true,
                    RoleId = "3"
                }, RequestHandler.GetToken(HttpContext));
                if (profileConnectionModels == null || profileConnectionModels.UserInfo == null || profileConnectionModels.UserInfo.Count == 0)
                {
                    return Ok(new Domain.Models.Generals.Responses.TeacherGroupResponseModel());
                }
                var teacherGroupResponseModel = new Domain.Models.Generals.Responses.TeacherGroupResponseModel
                {
                    StudentDetailModelList = new List<StudentDetail>(),
                    TotalPages = profileConnectionModels.PageTotals,
                };
                foreach (var userSharedDetail in profileConnectionModels.UserInfo)
                {
                    teacherGroupResponseModel.StudentDetailModelList.Add(new StudentDetail
                    {
                        FirstName = userSharedDetail.GivenName,
                        LastName = userSharedDetail.Surname,
                        UserId = userSharedDetail.UserId,
                        InvitationCode = userSharedDetail.Token,
                        UserSchoolRecord = userSharedDetail.UserSchoolRecord
                    });
                }
                return Ok(teacherGroupResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("create-group")]
        public async Task<IActionResult> CreateGroup([FromBody] TeacherGroupModel teacherGroupRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherGroupModifierManager = new TeacherGroupModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var response = teacherGroupModifierManager.CreateGroup(new TeacherGroupModel
                {
                    Name = teacherGroupRequestModel.Name,
                    SubjectCode = teacherGroupRequestModel.SubjectCode ?? teacherGroupRequestModel.SubjectCode.ToLower(),
                    LevelCode = teacherGroupRequestModel.LevelCode ?? teacherGroupRequestModel.LevelCode,
                    SubscriptionId = teacherGroupRequestModel.SubscriptionId,
                    TeacherId = responseSession.UserId,
                }, responseSession.UserId);
                if (!response.Valid)
                {
                    return BadRequest(response);
                }
                return Ok(new Domain.Models.Generals.Responses.ResponseModel
                {
                    Messages = response.Messages,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("archive/{groupId}")]
        public async Task<IActionResult> ArchiveGroup(string groupId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherGroupModifierManager = new TeacherGroupModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var response = teacherGroupModifierManager.ArchiveGroup(new TeacherGroupModel
                {
                    Id = groupId,
                    TeacherId = responseSession.UserId,
                }, responseSession.UserId);
                if (!response.Valid)
                {
                    return BadRequest(response);
                }
                try
                {
                    await teacherGroupModifierManager.RemoveAllEventCalendarFromGroup(groupId, responseSession);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                }
                return Ok(new Domain.Models.Generals.Responses.ResponseModel
                {
                    Messages = response.Messages,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("add-to-group/{groupId}/{studentId}")]
        public async Task<IActionResult> AddStudentToGroup(string groupId, string studentId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherGroupModifierManager = new TeacherGroupModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var response = await teacherGroupModifierManager.AddStudentToGroup(new Domain.Models.Teachers.TeacherGroupStudentModel
                {
                    TeacherGroupId = groupId,
                    StudentId = studentId,
                }, true, responseSession, Environment.GetEnvironmentVariable("tm_apply_duplication_evaluation_group_subject_level_logic") == "1");
                if (!response.Valid)
                {
                    return BadRequest(response);
                }
                return Ok(new Domain.Models.Generals.Responses.ResponseModel
                {
                    TransactionValid = true,
                    Messages = response.Messages,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("delete-from-group/{groupId}/{studentId}")]
        public async Task<IActionResult> DeleteStudentFromGroup(string groupId, string studentId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherGroupModifierManager = new TeacherGroupModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var response = await teacherGroupModifierManager.DeleteStudentFromGroup(new Domain.Models.Teachers.TeacherGroupStudentModel
                {
                    TeacherGroupId = groupId,
                    StudentId = studentId,
                }, true, responseSession);
                if (!response.Valid)
                {
                    return BadRequest(response);
                }
                return Ok(new Domain.Models.Generals.Responses.ResponseModel
                {
                    Messages = response.Messages,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("auto-subgroups")]
        public async Task<IActionResult> AutoSubGroupCreation([FromBody] List<Domain.Models.Generals.Dapper.TeacherGroupStudentModel> teacherGroups)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            new TeacherGroupModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).AutoTeacherGroupCreation(teacherGroups, responseSession.UserId);
            return Ok();
        }
        [HttpPatch("{groupId}/name")]
        public async Task<IActionResult> UpdateGroupName(int groupId, [FromBody] TeacherGroupModel teacherGroupModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherGroup");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            var response = new TeacherGroupModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).UpdateGroupName(groupId, teacherGroupModel.Name, responseSession.UserId);
            if (response.Valid)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
