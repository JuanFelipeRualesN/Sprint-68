using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.LearningGap;
using TeachersAPI.Domain.Models.LearningGap;
using TeachersAPI.Domain.Models.LearningGap.Request;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/learning-gap")]
    public class LearningGapController : ControllerBase
    {
        private string ConnectionString => AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"];
        private string ConnectionStringReader => AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"];

        #region student
        [HttpGet("student/pending-lessons")]
        public async Task<IActionResult> GetStudentPendingLessons(int fromPage = 0, int limiter = 10)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent"); 
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }
            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode)
                    .ReadPendingLessonAssignmentsGap(responseSession.UserId, fromPage, limiter, responseSession.Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("student/overall-score")]
        public async Task<IActionResult> GetStudentOverallScore()
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent"); 
            
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }
            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode)
                    .GetOverallScore(responseSession.UserId, responseSession.Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("student/lesson/{lessonId}/exams")]
        public async Task<IActionResult> GetStudentAvailableExams(string lessonId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }
            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode)
                    .GetAvailableQuizzesForStudent(responseSession.UserId, lessonId, responseSession.Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To submit the student's quiz result.
        /// </summary>
        /// <param name="learningGapQuizSubmitRequestModel">The learning gap submit request model with details.</param>

        /// <returns>Returns a response if created, bad request, etc.</returns>
        [HttpPost("student/results")]
        public async Task<IActionResult> PostStudentAutomaticResult([FromBody] LearningGapQuizSubmitRequestModel learningGapQuizSubmitRequestModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                learningGapQuizSubmitRequestModel.StudentId = responseSession.UserId;
                new LearningGapService(ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode).CreateOrUpdateStudentLessonQuizResult(
                    responseSession.UserId,
                    learningGapQuizSubmitRequestModel.LessonId,
                    learningGapQuizSubmitRequestModel.QuizValue,
                    learningGapQuizSubmitRequestModel.QuizId,
                    learningGapQuizSubmitRequestModel.UsquiQuizResultId,
                    learningGapQuizSubmitRequestModel.Score,
                    responseSession.Token,
                    learningGapQuizSubmitRequestModel.LearningSheetIds);
                return Created("", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read the pending diagnostics.
        /// </summary>
        /// <param name="fromPage">The from page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <returns>Returns the list of pending diagnostics.</returns>
        [HttpGet("student/diagnostic/pending")]
        public async Task<IActionResult> ReadStudentPendingDiagnostics(int fromPage = 0, int limiter = 5)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode)
                    .ReadPendingDiagnosticScores(responseSession.UserId, fromPage, limiter, responseSession.Token);

                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read the diagnostic by subject and level.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <returns>Returns the diagnostic details.</returns>
        [HttpGet("student/diagnostic/{subjectCode}/{levelCode}")]
        public async Task<IActionResult> ReadDiagnosticDetail(string subjectCode, string levelCode)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode)
                    .ReadDiagnosticDetail(subjectCode, levelCode, responseSession.Token, responseSession.UserId, true);

                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To submit the diagnostic test by student.
        /// </summary>
        /// <param name="diagnosticTestSubmittion">To handle the submittion of diagnostic test model.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <returns>Returns the correction. </returns>
        [HttpPost("student/diagnostic/{subjectCode}/{levelCode}/submit")]
        public async Task<IActionResult> SubmitDiagnosticTest([FromBody] DiagnosticTest diagnosticTestSubmittion, string subjectCode, string levelCode)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapStudent");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).
                    SubmitDiagnosticByStudent(diagnosticTestSubmittion, 
                    subjectCode, 
                    levelCode,
                    responseSession.Token, 
                    responseSession.UserId);

                if (result.TransactionValid)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion student

        /// <summary>
        /// To get learning gap initial data settings for the teacher by the student.
        /// If there are results, then the teacher will not be asked to enter the initial data.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the learning gap initial data setting for teacher and student.</returns>
        [HttpGet("initial-data-setting/{studentId}")]
        public async Task<IActionResult> GetLearningGapInitialDataSetting(string studentId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var service = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode);

                var result = service.ReadLearningGapInitialDataSettings(responseSession.UserId, studentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// To post a learning initial data setting. When created, the user will not be asked to 
        /// enter the initial data.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the created or the existing initial data setting model.</returns>
        [HttpPost("initial-data-setting/{studentId}")]
        public async Task<IActionResult> PostLearningGapInitialDataSetting(string studentId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode).PostLearningGapInitialDataSettings(responseSession.UserId,
                    studentId);
                if (result.Valid)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To get the learning sheet by id.
        /// </summary>
        /// <param name="id">The id to get.</param>
        /// <returns>Returns the url of the learning sheet if anything. Else, it will return null.</returns>
        [HttpGet("learning-sheet/{id}/pdf")]
        public async Task<IActionResult> GetLearningSheetPDF(string id)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var pdfUrl = await new LearningGapService(ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode).PrintLearningSheet(id);
                return Ok(pdfUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To get the diagnostic preview for teacher.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <returns>Returns the diagnostic details.</returns>
        [HttpGet("diagnostic/preview/{subjectCode}/{levelCode}")]
        public async Task<IActionResult> GetDiagnosticPreviewForTeacher(string subjectCode, string levelCode)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                return Ok(new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).ReadDiagnosticDetailTeacher(subjectCode, levelCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read the student lesson status.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the lesson list with if completed or finished.</returns>
        [HttpGet("student/{studentId}/lessons-status")]
        public async Task<IActionResult> ReadStudentLessonStatus(string studentId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).ReadStudentLessonStatus(studentId, responseSession.Token, true, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read the student learning sheets.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <returns>Returns the learning sheets of the students.</returns>
        [HttpGet("student/{studentId}/learning-sheets")]
        public async Task<IActionResult> ReadStudentLearningSheets(string studentId, int fromPage = 0, int limiter = 10)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).ReadStudentLearningSheets(studentId, fromPage, limiter, responseSession.Token, true, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To assign a lesson.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="assignedLessonModel">The model details. The required fields are LessonId and LimitDate.</param>
        /// <returns>Returns a HTTP Response.</returns>
        [HttpPost("student/{studentId}/assign-lesson")]
        public async Task<IActionResult> AssignLesson(string studentId, [FromBody] LearningGapAssignedLessonModel assignedLessonModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher"); 
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            // Create an instance of PlanningModifierManager
            PlanningModifierManager planningModifierManager = new PlanningModifierManager(
                    ConnectionString,
                    ConnectionStringReader,
                    responseSession.LangCode);

            // Get available courses from Subscriptions API
            var availableCourses = SubscriptionHandler.GetUserCourses(responseSession.Token);

            var lessonValidation =
               await planningModifierManager.IsLessonInTeacherCourses(assignedLessonModel.LessonId, availableCourses);

            if (!lessonValidation.Valid)
            {
                LearningGapAssignedLessonModel responseModel = new LearningGapAssignedLessonModel()
                {
                    Valid = false,
                    Messages = lessonValidation.Messages
                };

                return BadRequest(responseModel);
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode).AssignLearningGapLessonToStudent(assignedLessonModel.LessonId, responseSession.UserId, 
                    assignedLessonModel.LimitDate, studentId, assignedLessonModel.SubjectCode, responseSession.Token, true);
                if(result.Valid)
                {
                    return Created("", result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To delete the assigned lesson.
        /// </summary>
        /// <param name="assignedLessonId">The id of the assigned lesson.</param>
        /// <returns>Returns the response.</returns>
        [HttpDelete("assign-lesson/{assignedLessonId}")]
        public async Task<IActionResult> DeleteAssignLesson(string assignedLessonId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");

            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode);
                
                var result = learningGapService.DeleteLearninngGapLesson(assignedLessonId, true, responseSession.UserId);

                if (result.Valid)
                {
                    // Delete the notification linked to the the lesson if the lesson has one
                    learningGapService.DeleteNotificationsForAssignedLesson(assignedLessonId);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To edit the assigned lesson.
        /// </summary>
        /// <param name="assignedLessonId">The assigned lesson id.</param>
        /// <param name="learningGapAssignedLessonModel">The assigned lesson to edit.</param>
        /// <returns>Returns the assigned lesson. </returns>
        [HttpPut("assign-lesson/{assignedLessonId}")]
        public async Task<IActionResult> EditAssignLesson(string assignedLessonId, [FromBody] LearningGapAssignedLessonModel learningGapAssignedLessonModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");

            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                learningGapAssignedLessonModel.Id = assignedLessonId;

                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode);
                var result = learningGapService.UpdateAssignedLessonToStudent(learningGapAssignedLessonModel, true, responseSession.UserId);

                if (result.Valid)
                {
                    // Update the read status of the notification linked to the lesson when updating the limit date
                    learningGapService.CheckAndUpdateNotificationForLessonOrDiagnosticTest(assignedLessonId: learningGapAssignedLessonModel.Id);                


                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read all the student score info.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the list of student scores of currently active years.</returns>
        [HttpGet("student/{studentId}/scores")]
        public async Task<IActionResult> ReadStudentAcademicYearScoreModel(string studentId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).ReadStudentAcademicYearScoreModel(studentId, responseSession.Token, true, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read the student lesson quiz results.
        /// </summary>
        /// <param name="assignedLessonId">The assigned lesson's id.</param>
        /// <returns>Returns the student lesson quiz result list.</returns>
        [HttpGet("assigned-lessons/{assignedLessonId}/results")]
        public async Task<IActionResult> ReadStudentLessonQuizResultList(string assignedLessonId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).ReadStudentLessonQuizResultList(assignedLessonId, true, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To read the diagnostic year scores.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="levelCode">The level code.</param>
        /// <param name="subjectCode">The subject code.</param>
        /// <returns>Returns the list of diagnostic scores.</returns>
        [HttpGet("diagnostic/{studentId}/scores")]
        public async Task<IActionResult> ReadDiagnosticYearScores(string studentId, string subjectCode = null, string levelCode = null)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).
                                                          ReadDiagnosticYearScores(studentId, responseSession.Token, true, responseSession.UserId, subjectCode, levelCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To update the diagnostic score manually.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="learningGapDiagnosticYearScoreModel">The learning gap diagnostic year score to insert.</param>
        /// <returns>Returns the list of diagnostic scores.</returns>
        [HttpPost("diagnostic/{studentId}/scores/manual")]
        public async Task<IActionResult> UpdateDiagnosticYearScoresManually(string studentId, [FromBody] LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                // Initialize the service once instead of multiple times
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );
                learningGapDiagnosticYearScoreModel.StudentId = studentId;
                var response = learningGapService.UpdateDiagnosticScoreManually(learningGapDiagnosticYearScoreModel, true, responseSession.UserId, responseSession.Token);

                if (response.Valid)
                {
                    // Update the read status of the notification linked to the diagnostic test when updating the score
                    learningGapService.CheckAndUpdateNotificationForLessonOrDiagnosticTest(diagnosticTestId: learningGapDiagnosticYearScoreModel.Id);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To assign the diagnostic dates.
        /// </summary>
        /// <param name="learningGapDiagnosticYearScoreModel">The learning gap diagnostic year score to insert dates model.</param>
        /// <returns>Returns the assigned model.</returns>
        [HttpPost("diagnostic/assign")]
        public async Task<IActionResult> AssignDiagnosticTestDates([FromBody] LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                // Initialize the service once instead of multiple times
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );
                
                var response = learningGapService.AssignDiagnosticTestDates(
                   learningGapDiagnosticYearScoreModel.StudentIds,
                   learningGapDiagnosticYearScoreModel.TeacherGroupIds,
                   learningGapDiagnosticYearScoreModel.SubjectCode,
                   learningGapDiagnosticYearScoreModel.LevelCode,
                   learningGapDiagnosticYearScoreModel.InitialDiagnosticDateLimit,
                   learningGapDiagnosticYearScoreModel.FinalDiagnosticDateLimit,
                   responseSession.UserId,
                   responseSession.Token,
                   true, responseSession.UserId);

                if (response.Valid)
                {
                    // Update the read status of the notification linked to the diagnostic test when updating the limit dates
                    learningGapService.CheckAndUpdateNotificationForLessonOrDiagnosticTest(diagnosticTestId: learningGapDiagnosticYearScoreModel.Id);
                }
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        /// <summary>
        /// To read the required subjects for learning gap (to stablish the initial score).
        /// </summary>
        /// <returns>Returns the list of strings of codes.</returns>
        [HttpGet("required-subjects")]
        public async Task<IActionResult> ReadRequiredSubjectsForLearningGap()
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).GetRequiredSubjectsForLearningGap(true, responseSession.UserId, responseSession.Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To create the learning gap score.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="academicYearScoreModels">The academic year scores.</param>
        /// <returns>Returns the transaction details.</returns>
        [HttpPost("{studentId}/annual-score")]
        public async Task<IActionResult> CreateOrUpdateGradeScoreManually(string studentId, [FromBody] List<LearningGapAcademicYearScoreModel> academicYearScoreModels)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                // Initialize the service once instead of multiple times
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );

                // Prepare variables for processing results
                LearningGapAcademicYearScoreModel firstInvalidEntry = null;
                LearningGapAcademicYearScoreModel lastValidEntry = null;
                bool hasInvalidEntry = false;

                // Process each academic year score
                foreach (var academicYearScore in academicYearScoreModels)
                {
                    academicYearScore.StudentId = studentId;
                    academicYearScore.ModifiedBy = responseSession.UserId;

                    var processResult = learningGapService.CreateOrUpdateGradeScoreManually(academicYearScore, responseSession.Token, true, responseSession.UserId);

                    if (!processResult.Valid)
                    {
                        // Track the first invalid entry
                        hasInvalidEntry = true;
                        if (firstInvalidEntry == null)
                        {
                            firstInvalidEntry = processResult;
                        }
                    }
                    else
                    {
                        // Track the last valid entry
                        lastValidEntry = processResult;
                    }
                }

                // Return response based on validation results
                if (hasInvalidEntry)
                {
                    return BadRequest(firstInvalidEntry);
                }

                return Ok(lastValidEntry);
            }
            catch (Exception ex)
            {
                // Log exception (recommended for production environments)
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To update a quiz score.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="learningGapLessonQuizResultModel">The learning gap lesson quiz result model to insert.</param>
        /// <returns>Returns the transaction details.</returns>
        [HttpPost("{studentId}/quiz-score")]
        public async Task<IActionResult> CreateOrUpdateGradeQuizScoreManually(string studentId, [FromBody] LearningGapLessonQuizResultModel learningGapLessonQuizResultModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                // Initialize the service once instead of multiple times
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );

                var processResult = learningGapService.CreateOrUpdateQuizResultManually(learningGapLessonQuizResultModel.LearningGapAssignedLessonId,
                    studentId, learningGapLessonQuizResultModel.QuizId, 
                    Decimal.Parse(learningGapLessonQuizResultModel.Score), 
                    responseSession.UserId,
                    responseSession.Token);

   
                // Return response based on validation results
                if (!processResult.Valid)
                {
                    return BadRequest(processResult);
                }

                // Update the read status of the notification linked to the lesson when updating the quiz score of the lesson
                learningGapService.CheckAndUpdateNotificationForLessonOrDiagnosticTest(assignedLessonId: learningGapLessonQuizResultModel.LearningGapAssignedLessonId);

                return Ok(processResult);
            }
            catch (Exception ex)
            {
                // Log exception (recommended for production environments)
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// To read the annotations.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="fromPage">From page.</param>
        /// <param name="limiter">Limiter.</param>
        /// <returns>Returns the annotations.</returns>
        [HttpGet("student/{studentId}/annotations")]
        public async Task<IActionResult> ReadStudentAnnotations(string studentId, int fromPage = 0, int limiter = 10)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).ReadStudentAnnotations(studentId, fromPage, limiter, responseSession.Token, true, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To create the annotation for the student.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="annotationModel">The annotation details.</param>
        /// <returns>Returns the transaction details.</returns>
        [HttpPost("student/{studentId}/annotations")]
        public async Task<IActionResult> CreateAnnotation(string studentId, [FromBody] LearningGapAnnotationModel annotationModel)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                annotationModel.StudentId = studentId;
                annotationModel.TeacherId = responseSession.UserId;
                annotationModel.ModifiedBy = responseSession.UserId;

                var result = new LearningGapService(ConnectionStringReader, ConnectionString, responseSession.LangCode).CreateAnnotationModel(annotationModel, responseSession.Token);
                if (result.Valid)
                {
                    return Created("", result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get the teacher's notifications
        /// </summary>
        /// <param name="studentId">The student id</param>
        /// <param name="fromPage">The starting page (starting at 0)</param>
        /// <param name="limiter">Items per page.</param>
        /// <returns>A list with the notifications</returns>
        [HttpGet("{studentId}/notifications")]
        public async Task<IActionResult> GetLearningGapNotifications(string studentId, int fromPage = 0, int limiter = 10)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }

            try
            {
                var teacherId = responseSession.UserId;

                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );

                // Call the service to get the notifications
                var notifications = learningGapService.GetTeacherNotifications(teacherId, studentId, fromPage, limiter);

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Check if teacher has any active notifications
        /// </summary>
        /// <returns>true or false</returns>
        [HttpGet("teacher/has-notifications")]
        public async Task<IActionResult> CheckIfTeacherHasNotifications()
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }
            try
            {
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );
                string teacherId = responseSession.UserId;

                // Call the service
                bool hasNotifications = learningGapService.CheckIfTeacherHasNotifications(teacherId);

                return Ok(hasNotifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Check if the student has active notifications
        /// </summary>
        /// <returns>A list with the student id and a flag to show if the student has notifications</returns>
        [HttpGet("student-has-notifications")]
        public async Task<IActionResult> CheckIfStudentHasNotifications()
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LearningGapTeacher");
            if (responseSession == null || !responseSession.SessionValid)
            {
                return Unauthorized();
            }
            try
            {
                var learningGapService = new LearningGapService(
                    ConnectionStringReader,
                    ConnectionString,
                    responseSession.LangCode
                );

                var teacherId = responseSession.UserId;

                //Call the service
                List<LearningGapStudentHasNotificationsModel> hasNotifications = learningGapService.CheckIfStudentHasNotifications(teacherId);

                return Ok(hasNotifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
