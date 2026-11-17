using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.Generals.Requests;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.UserQuizzes.CustomClasses;
using TeachersAPI.Domain.Models.Generals.Responses.Reports;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons;
using System.Reflection;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeacherPlanLessonQuizModel = TeachersAPI.Domain.Models.Teachers.TeacherPlanLessonQuizModel;
using TeacherPlanQuizModel = TeachersAPI.Domain.Models.Teachers.TeacherPlanQuizModel;
using System.Numerics;
using TeachersAPI;
using TeachersAPI.CustomClasses;

namespace TeachersAPI.TeachersAPI.Services.UserQuizzes
{
    /// <summary>
    /// To handle the student assignments.
    /// </summary>
    public static class StudentAssignmentsAndResults
    {
        /// <summary>
        /// Method to retrieve the student pending lessons.
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="fromPage">From Page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="subjectCode">(optional) The subject code to call.</param>
        /// <param name="levelCode">(optional) The level code to call.</param>
        /// <param name="testTitle">Custom test title.</param>
        /// <param name="planId">(optional) The id of the plan.</param>
        /// <returns>Returns the student homework response.</returns>
        public static StudentAssignmentsResponseModel GetStudentPendingLessons(string studentId, int fromPage,
            int limiter, string testTitle,
            string subjectCode = null, string levelCode = null, string planId = null)
        {

            var homeworkResponseModel = new StudentAssignmentsResponseModel()
            {
                PendingLessonsModelList = new List<StudentLessonAssignmentDetail>(),
            };

            // First, get the student open plans. 
            var studentOpenPlans = TeacherPlans.GetStudentOpenPlans(studentId, subjectCode, levelCode, planId);
            if (studentOpenPlans.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Now, retrieve the plans lessons if any. 
            var planLessons = TeacherPlans.GetPlanLessonsByPlanIds(studentOpenPlans.Select(x => x.PlanId).ToList());
            if (planLessons.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Now, retrieve the plan lesson quizzes if any.
            var planLessonQuizzes =
                TeacherPlans.GetPlanLessonQuizzesByPlanIds(planLessons.Select(x => int.Parse(x.Id)).ToList());

            // If there are plan lesson quizzes, call the results.
            var userResults = new List<PlanLessonQuizUserResultModel>();
            if (planLessonQuizzes.Count > 0)
            {
                // Read the user results to check if the student has completed the exam.
                userResults = UserResults.GetPlanLessonQuizStudentResults(studentId,
                    planLessonQuizzes.Select(x
                        => int.Parse(x.PlanLessonId)).ToList());
            }

            // If there are pre/post results, call them.
            // Check if there is at least one plan lesson that requires a pre/post exam.
            var planPrePostUserResultModelList = new List<PlanPrePostUserResultModel>();
            var planLessonRequiresPrePost = planLessonQuizzes.Any(x => x.Required == "1");
            if (planLessonRequiresPrePost)
            {
                planPrePostUserResultModelList = UserResults.GetPlanPrePostUserResultModelsByPlanIds(studentId,
                    planLessons.Select(x
                        => int.Parse(x.Id)).ToList());
            }

            var pendingResults = new List<StudentLessonAssignmentDetail>();

            // Now, for each plan lesson, we check if the user completed the exam.
            foreach (var planLesson in planLessons)
            {
                var planLessonPlanId = int.Parse(planLesson.PlanId);
                var planDetails = studentOpenPlans.Find(x => x.PlanId == planLessonPlanId);
                var studentLessonAssignment = new StudentLessonAssignmentDetail()
                {
                    LessonId = planLesson.LessonId,
                    Subject = planDetails?.Subject,
                    LevelCode = planDetails?.LevelCode,
                    PlanId = planDetails.PlanId,
                    TeacherId = planDetails.TeacherId,
                    TriesQuantity = 1,
                    TeacherPlanLessonId = planLesson.Id,
                    StudentExamModels = new List<StudentExamModel>()
                };

                // Check if there are any exams.
                var planLessonQuizzesForLesson = planLessonQuizzes.FindAll(x => x.PlanLessonId == planLesson.Id);
                // If there are no exams, but the pre and post are required and the student has not completed them, then add it as pending.
                // Else, continue.
                if (planLessonQuizzesForLesson.Count == 0)
                {
                    // Check if pre/post is required.
                    if (planLesson.PrePostRequired == "1")
                    {
                        var studentPrePost = planPrePostUserResultModelList.FindAll(x => x.PlanLessonId == planLesson.Id);
                        if (studentPrePost.Count == 2)
                        {
                            // Both pre and post are completed.
                            continue;
                        }

                        // Add it as pending.
                        pendingResults.Add(studentLessonAssignment);
                        continue;
                    }

                    // If not pre/post are not required, and was assigned, and there are no exams, then add it as pending.
                    pendingResults.Add(studentLessonAssignment);
                    continue;
                }

                // If there are exams, prepare the details.
                var examsDetailsToAdd = new List<StudentExamModel>();
                foreach (var planLessonQuiz in planLessonQuizzesForLesson)
                {
                    var studentResult = userResults.Find(x => x.QuizId == planLessonQuiz.QuizId);
                    var studentExam = new StudentExamModel()
                    {
                        Title = testTitle,
                        Score = "0",
                        Completed = 0,
                        UsquiUserResultId = null,
                        LessonId = planLesson.LessonId,
                        LessonTitle = "",
                        QuizId = planLessonQuiz.QuizId,
                        Required = planLessonQuiz.Required,
                        DueDate = planLessonQuiz.DueDate,
                        DueDateTimezone = planLessonQuiz.DueDateTimezone,
                        Duration = planLessonQuiz.Duration,
                        ForGrading = int.Parse(planLessonQuiz.ForGrading),
                        PlanLessonId = planLesson.Id,
                    };

                    if (studentResult != null)
                    {
                        studentExam.UsquiUserResultId = studentResult.UsquiUserResultId;
                        studentExam.Score = studentResult.Score;
                        studentExam.Completed = 1;
                    }
                    examsDetailsToAdd.Add(studentExam);
                }

                studentLessonAssignment.StudentExamModels = examsDetailsToAdd;

                // Check if the pre/post is required and if the student has completed them.
                var prePostPending = false;
                if (planLesson.PrePostRequired == "1")
                {
                    var studentPrePost = planPrePostUserResultModelList.FindAll(x => x.PlanLessonId == planLesson.Id);
                    if (studentPrePost.Count == 2)
                    {
                        // If ids are different, then the student has completed the pre and post.
                        if (studentPrePost[0].QuizId != studentPrePost[1].QuizId)
                        {
                            prePostPending = false;
                        }
                        else
                        {
                            prePostPending = true;
                        }
                    }
                    else
                    {
                        prePostPending = true;
                    }
                }

                // Check if the student has completed the exams.
                var studentLessonResults = userResults.FindAll(x => x.PlanLessonId == planLesson.Id);
                if (studentLessonResults.Count == 0)
                {
                    // Add the lesson as pending.
                    pendingResults.Add(studentLessonAssignment);
                    continue;
                }

                // Check if the student has completed all the exams by checking the id.
                var completedAllExams = true;
                foreach (var planLessonQuizToFindResult in planLessonQuizzesForLesson)
                {
                    var studentResultQuiz = studentLessonResults.Find(x => x.QuizId == planLessonQuizToFindResult.QuizId && planLessonQuizToFindResult.Required == "1");
                    if (studentResultQuiz == null)
                    {
                        completedAllExams = false;
                        break;
                    }
                }


                if (!completedAllExams || prePostPending)
                {
                    // Add the lesson as pending.
                    pendingResults.Add(studentLessonAssignment);
                }
            }

            homeworkResponseModel.PendingLessonsModelList = pendingResults;

            // After evaluating all the pending lessons, order them by due date.
            try
            {
                foreach (var pendingLesson in homeworkResponseModel.PendingLessonsModelList)
                {
                    pendingLesson.StudentExamModels = pendingLesson.StudentExamModels?.OrderBy(se => se.DueDate).ToList();
                }

                // Sort the PendingLessonsModelList based on the first DueDate of StudentExamModels
                homeworkResponseModel.PendingLessonsModelList = homeworkResponseModel.PendingLessonsModelList?.OrderBy(pl => pl.StudentExamModels?.FirstOrDefault()?.DueDate).ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            // Set the total 
            homeworkResponseModel.TotalPages = int.Parse(Math
                .Ceiling((decimal)homeworkResponseModel.PendingLessonsModelList.Count() / limiter).ToString());
            // Set the limit
            homeworkResponseModel.PendingLessonsModelList = homeworkResponseModel.PendingLessonsModelList
                .Skip(fromPage * limiter).Take(limiter).ToList();

            var userIdsToCall = new List<string>();
            var lessonModelsToCallAthenas = new List<LessonModel>();
            foreach (var pendingLessonsModel in homeworkResponseModel.PendingLessonsModelList)
            {
                userIdsToCall.Add(pendingLessonsModel.TeacherId);
                lessonModelsToCallAthenas.Add(new LessonModel()
                {
                    Id = pendingLessonsModel.LessonId,
                });
            }

            var lessonsAthenas = new LessonsResponseModel();
            if (lessonModelsToCallAthenas.Count > 0)
            {
                lessonsAthenas = AthenasHandler.GetAthenasLessons(lessonModelsToCallAthenas, false);
            }

            foreach (var pendingLessonModel in homeworkResponseModel.PendingLessonsModelList)
            {
                var lesson = lessonsAthenas?.LessonModelList?.Find(x => x.Id == pendingLessonModel.LessonId);
                if (lesson != null)
                {
                    pendingLessonModel.Title = lesson.LessonTitle;
                    pendingLessonModel.Subject = lesson.SubjectCode;
                    pendingLessonModel.LevelCode = lesson.LevelCode;
                }

                int examNo = 1;
                foreach (var exam in pendingLessonModel.StudentExamModels)
                {
                    exam.Title = testTitle + " " + examNo.ToString();
                    examNo++;
                }
            }

            return homeworkResponseModel;
        }

        /// <summary>
        /// To get the acumulated total by subject and level from a specific period and student. 
        /// </summary>
        /// <param name="periodId">The period's id.</param>
        /// <param name="studentId">The student's id.</param>
        /// <param name="planId">The plan's id. (Optional)</param>
        /// <param name="teacherId">The teacher's id (Optional).</param>
        /// <returns>Returns a list with the user averages by subject and level.</returns>
        public static List<StudentPeriodAverageResponseModel> GetAccumulatedUserResultsByPeriod(string periodId, string studentId, string planId = null, string teacherId = null, string subjectCode = null, string levelCode = null, bool includeDetails = false, string langCode = "es")
        {
            // Step 1: Get the plans
            List<TeacherPlanGroupStudentModel> teacherPlansStudents = TeacherPlans.GetTeacherPlanWithGroupDetails(!string.IsNullOrWhiteSpace(planId) ? new List<string>() { planId } : null, new List<string>() { studentId }, teacherId, periodId, subjectCode, levelCode);

            List<string> teacherPlanIds = new List<string>();
            foreach (TeacherPlanGroupStudentModel teacherPlanGroup in teacherPlansStudents)
            {
                teacherPlanIds.Add(teacherPlanGroup.PlanId);
            }

            if (teacherPlanIds.Count == 0)
            {
                return new List<StudentPeriodAverageResponseModel>();
            }
            List<SubjectModel> subjects = AthenasHandler.GetAthenasSubjects(langCode);

            // Step 2: Get the plan-lessons
            List<TeacherPlanLessonModel> teacherPlanLessonModels = TeacherPlans.GetTeacherPlanLessonSimple(null, teacherPlanIds);
            teacherPlanLessonModels = teacherPlanLessonModels.FindAll(x => x.ForGrading == "1").ToList();

            List<PlanLessonUserModel> planLessonUserResultModels = new List<PlanLessonUserModel>();
            if (teacherPlanLessonModels != null)
            {
                // Get the lesson results 
                List<string> teacherPlanLessonsIds = new List<string>();
                foreach (TeacherPlanLessonModel teacherPlanLesson in teacherPlanLessonModels)
                {
                    teacherPlanLessonsIds.Add(teacherPlanLesson.Id);
                }
                planLessonUserResultModels = UserResults.GetUserLessonsAverages(new List<string> { studentId }, null, teacherPlanLessonsIds);
            }

            // Step 4: Call the custom homework of the plans 
            List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> teacherPlanHomeworkModelsList = TeacherPlans.GetTeacherPlanCustomHomework(teacherPlanIds);
            // Filter the ones that are only for grading
            teacherPlanHomeworkModelsList = teacherPlanHomeworkModelsList.FindAll(x => x.ForGrading == "1").ToList();

            // Step 5: Call the the student homework results 
            List<string> teacherHomeworkPlanQuizIds = new List<string>();
            foreach (Domain.Models.Generals.Dapper.TeacherPlanQuizModel teacherPlanQuizModel in teacherPlanHomeworkModelsList)
            {
                teacherHomeworkPlanQuizIds.Add(teacherPlanQuizModel.Id);
            }

            List<TeacherPlanHomeworkResultModel> teacherPlanHomeworkResultModels = new List<TeacherPlanHomeworkResultModel>();
            if (teacherHomeworkPlanQuizIds.Count != 0)
            {
                // Call the results 
                teacherPlanHomeworkResultModels = UserResults.GetUserHomeworkPlanResultsSimpleByStudent(teacherHomeworkPlanQuizIds, new List<string>() { studentId });
                // Filter just the completed ones 
                teacherPlanHomeworkResultModels = teacherPlanHomeworkResultModels.FindAll(x => x.Status == "C");
            }

            // Now, calculate the totals
            // First, identify the distinct subjects and levels
            List<StudentPeriodAverageResponseModel> studentPeriodAverageResponseModelList = new List<StudentPeriodAverageResponseModel>();
            var teacherPlansStudentsSubjects = teacherPlansStudents.GroupBy(x => (x.SubjectCode, x.LevelCode)).ToList();

            // Create the subjects and levels according to this. 
            foreach (var teacherPlan in teacherPlansStudentsSubjects)
            {
                studentPeriodAverageResponseModelList.Add(new StudentPeriodAverageResponseModel()
                {
                    SubjectCode = teacherPlan.Key.SubjectCode,
                    LevelCode = teacherPlan.Key.LevelCode,
                    SubjectName = subjects?.Find(x => x.Code == teacherPlan.Key.SubjectCode)?.Name
                });
            }

            if (teacherPlanLessonModels.Count == 0 && teacherPlanHomeworkModelsList.Count == 0)
            {
                return studentPeriodAverageResponseModelList;
            }

            LessonsResponseModel lessonsResponseModel = new LessonsResponseModel();
            if (includeDetails)
            {
                List<LessonModel> lesonModelsIdsToCall = new List<LessonModel>();
                foreach (TeacherPlanLessonModel teacherPlanLesson in teacherPlanLessonModels)
                {
                    lesonModelsIdsToCall.Add(new LessonModel()
                    {
                        Id = teacherPlanLesson.LessonId,
                    });
                }

                // Call Athenas 
                if (lesonModelsIdsToCall.Count > 0)
                {
                    lessonsResponseModel = AthenasHandler.GetAthenasLessons(lesonModelsIdsToCall, false);
                }
            }

            // Step 6: Attach the results of the lesson. 
            var academicGrades = GradingSystem.GetAcademicGrades();
            foreach (StudentPeriodAverageResponseModel studentPeriodAverageResponse in studentPeriodAverageResponseModelList)
            {
                List<TeacherPlanGroupStudentModel> teacherPlanGroupStudents = teacherPlansStudents.FindAll(x => x.SubjectCode == studentPeriodAverageResponse.SubjectCode && x.LevelCode == studentPeriodAverageResponse.LevelCode);

                foreach (TeacherPlanGroupStudentModel teacherPlan in teacherPlanGroupStudents)
                {
                    // First, process the plan lessons 
                    List<PlanLessonUserModel> teacherPlanLessonUsers = planLessonUserResultModels.FindAll(x => x.PlanId == teacherPlan.PlanId);
                    foreach (PlanLessonUserModel teacherPlanUser in teacherPlanLessonUsers)
                    {
                        var lessonExamScore = decimal.Parse(teacherPlanUser.OverallScore.ToString());
                        if (includeDetails)
                        {
                            // Find the lesson title 
                            TeacherPlanLessonModel teacherPlanLessonModel = teacherPlanLessonModels.Find(x => x.Id == teacherPlanUser.PlanLessonId);
                            studentPeriodAverageResponse.Details.Add(new StudentPeriodAverageResponseDetail()
                            {
                                Type = "L",
                                Average = lessonExamScore,
                                Title = lessonsResponseModel.LessonModelList.Find(x => x.Id == teacherPlanLessonModel.LessonId)?.LessonTitle,
                                AcademicGrade = academicGrades.Find(x => lessonExamScore >= x.MinValue && lessonExamScore < x.BelowValue).Grade
                            });
                        }

                        studentPeriodAverageResponse.Scores.Add(lessonExamScore);
                        studentPeriodAverageResponse.Average = AverageCalculator.FixTwoDecimalsAfterPoint(studentPeriodAverageResponse.Scores.Average());
                        studentPeriodAverageResponse.AcademicGrade = academicGrades.Find(x => studentPeriodAverageResponse.Average >= x.MinValue && studentPeriodAverageResponse.Average < x.BelowValue).Grade;
                    }

                    // Now process the custom assignments 
                    // Find the plans 
                    List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> teacherPlanQuizzes = teacherPlanHomeworkModelsList.FindAll(x => x.PlanId == teacherPlan.PlanId);
                    // Search the results for those plan quizzes 
                    foreach (var teacherPlanQuiz in teacherPlanQuizzes)
                    {
                        var teacherPlanQuizResults = teacherPlanHomeworkResultModels.FindAll(x => x.PlanQuizId == teacherPlanQuiz.Id);
                        foreach (var teacherPlanQuizResult in teacherPlanQuizResults)
                        {
                            decimal score = decimal.Parse(teacherPlanQuizResult.TotalPercent);
                            if (includeDetails)
                            {
                                studentPeriodAverageResponse.Details.Add(new StudentPeriodAverageResponseDetail()
                                {
                                    UsquiId = teacherPlanQuizResult.UsquiUserResultId,
                                    Average = score,
                                    Title = teacherPlanQuiz.Title,
                                    Type = "H",
                                    AcademicGrade = academicGrades.Find(x => score >= x.MinValue && score < x.BelowValue).Grade
                                });
                            }
                            studentPeriodAverageResponse.Scores.Add(score);
                            studentPeriodAverageResponse.Average = AverageCalculator.FixTwoDecimalsAfterPoint(studentPeriodAverageResponse.Scores.Average());
                            studentPeriodAverageResponse.AcademicGrade = academicGrades.Find(x => studentPeriodAverageResponse.Average >= x.MinValue && studentPeriodAverageResponse.Average < x.BelowValue).Grade;

                        }
                    }
                }
            }

            return studentPeriodAverageResponseModelList;
        }

        /// <summary>
        /// Method to retrieve the student pending lessons.
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="fromPage">From Page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="token">The user's token (the one that is making the request).</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <returns>Returns the student homework response.</returns>
        public static StudentAssignmentsResponseModel GetStudentPendingLessons(string studentId, int fromPage, int limiter, string token, string ipAddress)
        {
            StudentAssignmentsResponseModel homeworkResponseModel = new StudentAssignmentsResponseModel()
            {
                PendingLessonsModelList = new List<StudentLessonAssignmentDetail>(),
            };

            homeworkResponseModel.PendingLessonsModelList = TeacherPlans.GetStudentPendingLessons(studentId);

            // Now prepare the other details
            if (homeworkResponseModel.PendingLessonsModelList == null || homeworkResponseModel.PendingLessonsModelList.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Order by dates 
            homeworkResponseModel.PendingLessonsModelList = homeworkResponseModel.PendingLessonsModelList.OrderBy(x => x.StudentExamModels.Min(y => DateTime.Parse(y.DueDate))).ToList();

            // Set the total 
            homeworkResponseModel.TotalPages = int.Parse(Math.Ceiling((double)homeworkResponseModel.PendingLessonsModelList.Count() / limiter).ToString());
            // Set the limit
            homeworkResponseModel.PendingLessonsModelList = homeworkResponseModel.PendingLessonsModelList.Skip(fromPage * limiter).Take(limiter).ToList();

            List<string> userIdsToCall = new List<string>();
            List<LessonModel> lessonModelsToCallAthenas = new List<LessonModel>();
            List<string> quizzesAthenasToCall = new List<string>();
            foreach (StudentLessonAssignmentDetail pendingLessonsModel in homeworkResponseModel.PendingLessonsModelList)
            {
                userIdsToCall.Add(pendingLessonsModel.TeacherId);
                lessonModelsToCallAthenas.Add(new LessonModel()
                {
                    Id = pendingLessonsModel.LessonId,
                });
                foreach(var pendingQuiz in pendingLessonsModel.StudentExamModels)
                {
                    quizzesAthenasToCall.Add(pendingQuiz.QuizId);
                }
            }

            LessonsResponseModel lessonsAthenas = new LessonsResponseModel();
            if (lessonModelsToCallAthenas.Count > 0)
            {
                lessonsAthenas = AthenasHandler.GetAthenasLessons(lessonModelsToCallAthenas, false);
            }

            // Call the users 
            // Get the teacher and student information.
            List<UserResponse> userSharedDetails = new List<UserResponse>();
            if (userIdsToCall.Count > 0)
            {
                try
                {
                    // userSharedDetails = await GetUserDetails.GetUsersSharedDetails(token, ipAddress, userIdsToCall);
                }
                catch (Exception)
                {
                    // Continue without the names. This is in another API and could affect the performance.
                }
            }

            // Get the quizzes title 
            var quizzessAthenas = AthenasHandler.GetQuizAthenasModelList(quizzesAthenasToCall);

            foreach (StudentLessonAssignmentDetail pendingLessonModel in homeworkResponseModel.PendingLessonsModelList)
            {
                LessonModel lesson = lessonsAthenas?.LessonModelList?.Find(x => x.Id == pendingLessonModel.LessonId);
                if (lesson != null)
                {
                    pendingLessonModel.Title = lesson.LessonTitle;
                    pendingLessonModel.Subject = lesson.SubjectCode;
                    pendingLessonModel.LevelCode = lesson.LevelCode;
                }

                UserResponse userResponse = userSharedDetails?.Find(x => x.UserId == pendingLessonModel.TeacherId);
                if (userResponse != null)
                {
                    pendingLessonModel.SentBy = userResponse.GivenName + " " + userResponse.Surname;
                }

                foreach(var pendingExam in pendingLessonModel.StudentExamModels)
                {
                    try
                    {
                        pendingExam.Title = quizzessAthenas.Find(x => x.Id == pendingExam.QuizId)?.Title;
                    }
                    catch(Exception)
                    {

                    }
                }
            }

            return homeworkResponseModel;
        }

        /// <summary>
        /// Method to retrieve the student pending homework (homework are created by teacher).
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="fromPage">From Page.</param>
        /// <param name="limiter">Limiter (how many items per page).</param>
        /// <param name="token">The user's token (the user that is making the request).</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="pendingOrComplete">C for complete, P for pending.</param>
        /// <returns>Returns the student homework response.</returns>
        public static StudentAssignmentsResponseModel GetStudentsHomework(string studentId, int fromPage, int limiter, string token, string ipAddress, string subjectCode, string levelCode, string pendingOrComplete, string planId = null)
        {
            StudentAssignmentsResponseModel homeworkResponseModel = new StudentAssignmentsResponseModel()
            {
                PendingLessonsModelList = new List<StudentLessonAssignmentDetail>(),
            };

            homeworkResponseModel.HomeworkModelList = TeacherPlans.GetStudentHomework(studentId, subjectCode, levelCode, pendingOrComplete, planId);

            // Now prepare the other details
            if (homeworkResponseModel.HomeworkModelList == null || homeworkResponseModel.HomeworkModelList.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Order by dates 
            homeworkResponseModel.HomeworkModelList = homeworkResponseModel.HomeworkModelList.OrderBy(x => DateTime.Parse(x.DueDate)).ToList();

            // Set the total 
            homeworkResponseModel.TotalPages = int.Parse(Math.Ceiling((double)homeworkResponseModel.HomeworkModelList.Count() / limiter).ToString());
            // Set the limit
            homeworkResponseModel.HomeworkModelList = homeworkResponseModel.HomeworkModelList.Skip(fromPage * limiter).Take(limiter).ToList();

            List<string> userIdsToCall = new List<string>();

            foreach (StudentHomeworkDetail pendingLessonsModel in homeworkResponseModel.HomeworkModelList)
            {
                userIdsToCall.Add(pendingLessonsModel.TeacherId);
            }

            // Call the users 
            // Get the teacher and student information.
            List<UserResponse> userSharedDetails = new List<UserResponse>();
            if (userIdsToCall.Count > 0)
            {
                try
                {
                    userSharedDetails = GetUserDetails.GetUsersSharedDetails(token, ipAddress, userIdsToCall);
                }
                catch (Exception)
                {
                    // Continue without the names. This is in another API and could affect the performance.
                }
            }

            foreach (StudentHomeworkDetail pendingHomework in homeworkResponseModel.HomeworkModelList)
            {
                UserResponse userResponse = userSharedDetails?.Find(x => x.UserId == pendingHomework.TeacherId);
                if (userResponse != null)
                {
                    pendingHomework.SentBy = userResponse.GivenName + " " + userResponse.Surname;
                }
            }

            return homeworkResponseModel;
        }

        /// <summary>
        /// Method to retrieve the student pending homework (homework are created by teacher).
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="fromPage">From Page.</param>
        /// <param name="limiter">Limiter (how many items per page).</param>
        /// <param name="token">The user's token (the user that is making the request).</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="pendingOrComplete">C for complete, P for pending.</param>
        /// <returns>Returns the student homework response.</returns>
        public static StudentAssignmentsResponseModel GetStudentsHomework(string studentId, int fromPage, int limiter,
            string token, string ipAddress, string subjectCode, string levelCode, string pendingOrComplete,
            bool filterOutPastDates, string planId = null)
        {
            var homeworkResponseModel = new StudentAssignmentsResponseModel()
            {
                PendingLessonsModelList = new List<StudentLessonAssignmentDetail>(),
            };

            homeworkResponseModel.HomeworkModelList = TeacherPlans.GetStudentHomework(studentId, subjectCode, levelCode,
                pendingOrComplete, filterOutPastDates, planId);

            // Now prepare the other details
            if (homeworkResponseModel.HomeworkModelList == null || homeworkResponseModel.HomeworkModelList.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Order by dates 
            homeworkResponseModel.HomeworkModelList = homeworkResponseModel.HomeworkModelList
                .OrderBy(x => DateTime.Parse(x.DueDate)).ToList();

            // Set the total 
            homeworkResponseModel.TotalPages =
                int.Parse(Math.Ceiling((double)homeworkResponseModel.HomeworkModelList.Count() / limiter).ToString());
            // Set the limit
            homeworkResponseModel.HomeworkModelList = homeworkResponseModel.HomeworkModelList.Skip(fromPage * limiter)
                .Take(limiter).ToList();

            List<string> userIdsToCall = new List<string>();

            foreach (StudentHomeworkDetail pendingLessonsModel in homeworkResponseModel.HomeworkModelList)
            {
                userIdsToCall.Add(pendingLessonsModel.TeacherId);
            }

            // Call the users 
            // Get the teacher and student information.

            var userSharedDetails = new List<UserFullNameModel>();
            if (userIdsToCall.Count > 0)
            {
                try
                {
                    userSharedDetails = GetUserDetails.GetUserFullNamesSimple(token, ipAddress, userIdsToCall);
                }
                catch (Exception)
                {
                    // Continue without the names. This is in another API and could affect the performance.
                }
            }

            foreach (StudentHomeworkDetail pendingHomework in homeworkResponseModel.HomeworkModelList)
            {
                var userResponse = userSharedDetails?.Find(x => x.UserId == pendingHomework.TeacherId);
                if (userResponse != null)
                {
                    pendingHomework.SentBy = userResponse.GivenName + " " + userResponse.Surname;
                }
            }

            return homeworkResponseModel;
        }

        /// <summary>
        /// To get the user's pending quizzes.
        /// </summary>
        /// <param name="studentId">The student id to find.</param>
        /// <param name="fromPage">From Page.</param>
        /// <param name="limiter">Limiter (how many items per page).</param>
        /// <param name="token">The user's token.</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <param name="quizId">The quiz id (optional).</param>
        /// <param name="planId">The plan id (optional).</param>
        /// <returns>Returns a list of student assignments.</returns>
        public static StudentAssignmentsResponseModel GetStudentPendingQuizzes(string studentId, int fromPage, int limiter, string token, string ipAddress, string quizId = null, string planId = null)
        {
            StudentAssignmentsResponseModel homeworkResponseModel = new StudentAssignmentsResponseModel()
            {
                LessonQuizModelList = new List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>(),
            };

            homeworkResponseModel.LessonQuizModelList = TeacherPlans.GetStudentPendingLessonQuizzes(studentId, quizId, planId);
            if (homeworkResponseModel.LessonQuizModelList == null || homeworkResponseModel.LessonQuizModelList.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Order by date
            homeworkResponseModel.LessonQuizModelList = homeworkResponseModel.LessonQuizModelList.OrderBy(x => DateTime.Parse(x.DueDate)).ToList();

            // Set the total 
            homeworkResponseModel.TotalPages = int.Parse(Math.Ceiling((double)homeworkResponseModel.LessonQuizModelList.Count() / limiter).ToString());

            // Set the limit
            homeworkResponseModel.LessonQuizModelList = homeworkResponseModel.LessonQuizModelList.Skip(fromPage * limiter).Take(limiter).ToList();

            List<string> userIdsToCall = new List<string>();
            List<LessonModel> lessonModelsToCall = new List<LessonModel>();
            foreach (Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel pendingQuiz in homeworkResponseModel.LessonQuizModelList)
            {
                userIdsToCall.Add(pendingQuiz.TeacherId);
                lessonModelsToCall.Add(new LessonModel() { Id = pendingQuiz.LessonId });
            }

            // Call the users 
            // Get the teacher and student information.
            List<UserResponse> userSharedDetails = new List<UserResponse>();
            if (userIdsToCall.Count > 0)
            {
                try
                {
                    userSharedDetails = GetUserDetails.GetUsersSharedDetails(token, ipAddress, userIdsToCall);
                }
                catch (Exception)
                {
                    // Continue without the names. This is in another API and could affect the performance.
                }
            }

            // Call the lessons 
            LessonsResponseModel lessonsResponseModel = new LessonsResponseModel();
            if (lessonModelsToCall.Count > 0)
            {
                try
                {
                    lessonsResponseModel = AthenasHandler.GetAthenasLessons(lessonModelsToCall, false);
                }
                catch (Exception)
                {

                }
            }

            foreach (Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel pendingQuiz in homeworkResponseModel.LessonQuizModelList)
            {
                UserResponse userResponse = userSharedDetails?.Find(x => x.UserId == pendingQuiz.TeacherId);
                if (userResponse != null)
                {
                    pendingQuiz.SentBy = userResponse.GivenName + " " + userResponse.Surname;
                }
                pendingQuiz.LessonTitle = lessonsResponseModel?.LessonModelList?.Find(x => x.Id == pendingQuiz.LessonId).LessonTitle;
            }

            return homeworkResponseModel;
        }

        /// <summary>
        /// To call the lesson, assignments or both results.
        /// </summary>
        /// <param name="detailsRequest">Details requests that include the limiter and page from.</param>
        /// <param name="token">The token to consume other APIs.</param>
        /// <param name="userIds">The users' ids (students) to call.</param>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="typePendingOrComplete">1 for call pending, 2 for call completed, else to call pending and completed homework.</param>
        /// <param name="planningId">The id of the plan if any (optional).</param>
        /// <returns></returns>
        public static LessonAssignmentResponseModel GetStudentLessonResults(DetailsRequestModel detailsRequest, string token, List<string> userIds, string ipAddress, int typePendingOrComplete, string planningId, string perioId = null)
        {
            LessonAssignmentResponseModel homeworkResponseModel = new LessonAssignmentResponseModel()
            {
                PlanDetails = new List<PlanDetailLessonModel>(),
            };

            // Get the plans by students. Planning Id and User Ids are optional. Can be sent as null.
            List<TeacherPlanGroupStudentModel> planStudentsDetails = TeacherPlans.GetTeacherPlanWithGroupDetails(!string.IsNullOrWhiteSpace(planningId) ? new List<string> { planningId } : null, userIds, null, perioId);
            if (planStudentsDetails.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Now, for each plan, call the details 
            List<string> planningsToCall = new List<string>();
            foreach (TeacherPlanGroupStudentModel teacherStudentPlan in planStudentsDetails)
            {
                planningsToCall.Add(teacherStudentPlan.PlanId);
            }
            List<TeacherPlanFullDetailsModel> planDetails = TeacherPlans.GetTeacherPlanFullDetails(planningsToCall, detailsRequest != null ? detailsRequest?.SubjectCode : null, detailsRequest != null ? detailsRequest?.LevelCode : null);
            if (planDetails.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Get the assignments results 
            List<string> userIdsToCall = new List<string>();
            userIdsToCall.AddRange(userIds); // Add the students already

            List<string> lessonIds = new List<string>();

            foreach (TeacherPlanFullDetailsModel teacherPlan in planDetails)
            {
                foreach (LessonAssignmentModel assignmentPlan in teacherPlan.LessonAssignmentModels)
                {
                    lessonIds.Add(assignmentPlan.LessonId);
                    userIdsToCall.Add(teacherPlan.TeacherPlanModel.TeacherId);
                }
            }

            // If there is no lessons to call and we just need the lessons, then return an empty object.
            if (lessonIds.Count == 0)
            {
                return homeworkResponseModel;
            }


            // Get the teacher and student information.
            List<UserResponse> userSharedDetails = new List<UserResponse>();
            try
            {
                userSharedDetails = GetUserDetails.GetUsersSharedDetails(token, ipAddress, userIdsToCall);
            }
            catch (Exception)
            {
                // Continue without the names. This is in another API and could affect the performance.
            }

            // Now get the details 
            return CalculateLessonAverages(userIds, userSharedDetails, planDetails, typePendingOrComplete, UserResults.GetUserLessonsAverages(userIds, lessonIds), detailsRequest.FromPage, detailsRequest.Limiter);
        }

        /// <summary>
        /// To get the full planning lesson details with student results.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="planningId">The id of the teacher plan.</param>
        /// <param name="token">The user's token to retrieve details.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <param name="lessonId">The lesson's id (optional).</param>
        /// <returns>Returns the lesson assignment with full details.</returns>
        public static LessonAssignmentResponseModel GetStudentLessonsResultsForPlanning(string studentId,
            string planningId, string token, string ipAddress, string lessonId = null)
        {
            var homeworkResponseModel = new LessonAssignmentResponseModel()
            {
                PlanDetails = new List<PlanDetailLessonModel>(),
            };

            // First, get the plan details
            var teacherPlans = new TeacherPlanDAO().Read(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                new Domain.Models.Teachers.TeacherPlanModel() { Id = planningId, Available = "1" }, true, true);

            if (teacherPlans.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Retrieve the plan lessons
            var teacherPlanLessons = new TeacherPlanLessonDAO().Read(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                new Domain.Models.Teachers.TeacherPlanLessonModel()
                {
                    PlanId = planningId,
                    Available = "1",
                    LessonId = lessonId
                }, false,
                false);

            if (teacherPlanLessons.Count == 0)
            {
                return homeworkResponseModel;
            }

            var teacherPlanLessonQuizzesToCall = new List<TeacherPlanLessonQuizModel>();
            foreach (var teacherPlanLesson in teacherPlanLessons)
            {
                teacherPlanLessonQuizzesToCall.Add(new TeacherPlanLessonQuizModel()
                {
                    PlanLessonId = teacherPlanLesson.Id,
                    Required = "1",
                    ForGrading = "1"
                });
            }

            // Retrieve the plan lesson quizzes
            var teacherPlanLessonQuizzes = new TeacherPlanLessonQuizDAO().Read(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                teacherPlanLessonQuizzesToCall);

            if (teacherPlanLessonQuizzes.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Call Athena's quizzes
            var quizzesAthenas = AthenasHandler.GetQuizAthenasModelList(teacherPlanLessonQuizzes.Select(x => x.QuizId).ToList());

            // Now, get the student results for the planning-exams 
            var studentResults = UserResults.GetStudentPlanningLessonsResults(studentId, planningId);

            // Call the teacher and student details.
            var userIdsToCall = new List<string> { studentId };

            // To call the lessons in Athenas
            var lessonModels = new List<LessonModel>();
            foreach (var teacherPlan in teacherPlanLessons)
            {
                lessonModels.Add(new LessonModel() { Id = teacherPlan.LessonId });
                userIdsToCall.Add(teacherPlans[0].UserId);
            }

            if (lessonModels.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Call Athenas details 
            var lessonsAthenas = AthenasHandler.GetAthenasLessons(lessonModels, false);

            // Call the users
            var users = GetUserDetails.GetUsersSharedDetails(token, ipAddress, userIdsToCall);


            // Now, for each lesson in the planning, calculate the average.
            var teacherName = users.Find(x => x.UserId == teacherPlans[0].UserId)?.GivenName + " " +
                              users.Find(x => x.UserId == teacherPlans[0].UserId)?.Surname;
            var lessonAssigment = new LessonAssignmentResponseModel()
            {
                PlanDetails = new List<PlanDetailLessonModel>()
                {
                    new PlanDetailLessonModel()
                    {
                        PlanId = teacherPlans[0].Id,
                        PlanName = teacherPlans[0].Name,
                        LevelCode = teacherPlans[0].LevelCode,
                        SubjectCode = teacherPlans[0].SubjectCode,
                        PeriodId = teacherPlans[0].PeriodId,
                        TeacherId = teacherPlans[0].UserId,
                        TeacherName = teacherName,
                        AssignmentDetails = new List<StudentLessonAssignmentDetail>()
                    }
                }
            };

            var studentName = users.Find(x => x.UserId == studentId)?.GivenName + " " +
                              users.Find(x => x.UserId == studentId)?.Surname;

            foreach (var planLesson in teacherPlanLessons)
            {
                var lessonAthenas = lessonsAthenas.LessonModelList.Find(x => x.Id == planLesson.LessonId);
                var assignmentDetail = new StudentLessonAssignmentDetail()
                {
                    TeacherPlanLessonId = planLesson.Id,
                    LessonId = planLesson.LessonId,
                    Subject = lessonAssigment.PlanDetails[0].SubjectCode,
                    LevelCode = lessonAssigment.PlanDetails[0].LevelCode,
                    StudentName = studentName,
                    SentBy = teacherName,
                    Title = lessonAthenas?.LessonTitle,
                    TotalPercent = 0,
                    Status = "P",
                    StudentExamModels = new List<StudentExamModel>(),
                };

                // Identify the quizzes of that lesson.
                var lessonQuizzesAssignedByTeacher = teacherPlanLessonQuizzes.FindAll(x => x.PlanLessonId
                    == planLesson.Id);

                List<decimal> scores = new List<decimal>();
                bool allQuizzesCompleted = true;
                foreach (var lessonQuiz in lessonQuizzesAssignedByTeacher)
                {
                    var studentResult = studentResults.Find(x => x.QuizId == lessonQuiz.QuizId);
                    if (studentResult != null)
                    {
                        scores.Add(decimal.Parse(studentResult.OverallScore));
                    }
                    else
                    {
                        allQuizzesCompleted = false;
                    }
                }

                if (allQuizzesCompleted && lessonQuizzesAssignedByTeacher.Count > 0)
                {
                    assignmentDetail.Status = "C";
                    assignmentDetail.TotalPercent = AverageCalculator.CalculateAverage(scores.ToArray());
                }

                if (allQuizzesCompleted && lessonQuizzesAssignedByTeacher.Count == 0 && planLesson.PrePostRequired == "1")
                {
                    assignmentDetail.Status = "CPP";
                    assignmentDetail.TotalPercent = 0;
                }

                if (allQuizzesCompleted && lessonQuizzesAssignedByTeacher.Count == 0 && planLesson.PrePostRequired == "0")
                {
                    assignmentDetail.Status = "N/A";
                    assignmentDetail.TotalPercent = 0;
                }

                // Find the quizzes of the plan lesson.
                var quizzesOfPlanLesson = teacherPlanLessonQuizzes.FindAll(x => x.PlanLessonId == planLesson.Id);
                foreach (var quiz in quizzesOfPlanLesson)
                {
                    var quizDetails = quizzesAthenas.Find(x => x.Id == quiz.QuizId);

                    var studentExam = new StudentExamModel()
                    {
                        Title = quizDetails?.Title,
                        Score = "0",
                        Completed = 0,
                        ForGrading = int.Parse(quiz.ForGrading),
                        LessonId = planLesson.LessonId,
                        LessonTitle = lessonAthenas?.LessonTitle,
                        QuizId = quizDetails?.Id,
                        Required = quiz.Required,
                        DueDate = quiz.DueDate,
                        DueDateTimezone = quiz.DueDateTimezone,
                        Duration = quiz.Duration,
                        PlanLessonId = planLesson.Id,
                    };

                    var studentResult = studentResults.Find(x => x.QuizId == quiz.QuizId);
                    if (studentResult != null)
                    {

                        studentExam.UsquiUserResultId = studentResult.UsquiUserResultId;
                        studentExam.Score = decimal.Parse(studentResult.OverallScore).ToString("0.##");
                        studentExam.Completed = 1;
                    }

                    assignmentDetail.StudentExamModels.Add(studentExam);
                }

                // Add the assignment detail
                lessonAssigment.PlanDetails[0].AssignmentDetails.Add(assignmentDetail);
            }

            // Now calculate the average for the planning.
            // Calculate the average for the planning by TotalPercent and Completed lessons.
            var overallScores = lessonAssigment.PlanDetails[0].AssignmentDetails
                .Where(x => x.Status == "C") // Consider only completed lessons
                .Select(x => x.TotalPercent)
                .ToArray();
            lessonAssigment.PlanDetails[0].OverallAveragePlan = AverageCalculator.CalculateAverage(overallScores).ToString("0.##");
            lessonAssigment.OverallAverage = lessonAssigment.PlanDetails[0].OverallAveragePlan;

            // Return the response
            return lessonAssigment;
        }

        /// <summary>
        /// To get the homework results (the homework created by the teachers).
        /// </summary>
        /// <param name="detailsRequest">General request details that includes filters as Subject, Level, etc.</param>
        /// <param name="token">The user's token.</param>
        /// <param name="userIds">The users ids to find an specific user if any.</param>
        /// <param name="ipAddress">IP address.</param>
        /// <param name="typePendingOrComplete">1 for call pending, 2 for call completed, else to call pending and completed homework.</param>
        /// <param name="planningId">The planning id if filtering by planning.</param>
        /// <param name="perioId">The period's id.</param>
        /// <returns>Return the homework response with all the details.</returns>
        public static HomeworkResponseModel GetStudentHomeworkResults(DetailsRequestModel detailsRequest, string token, List<string> userIds, string ipAddress, int typePendingOrComplete, string planningId, string perioId = null)
        {
            HomeworkResponseModel homeworkResponseModel = new HomeworkResponseModel()
            {
                PlanDetails = new List<PlanDetail>()
            };

            // Get the plans by students. Planning Id and User Ids are optional. Can be sent as null.
            List<TeacherPlanGroupStudentModel> planStudentsDetails = TeacherPlans.GetTeacherPlanWithGroupDetails(!string.IsNullOrWhiteSpace(planningId) ? new List<string> { planningId } : null, userIds, null, perioId);
            if (planStudentsDetails.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Now, for each plan, call the details 
            List<string> planningsToCall = new List<string>();
            foreach (TeacherPlanGroupStudentModel teacherStudentPlan in planStudentsDetails)
            {
                planningsToCall.Add(teacherStudentPlan.PlanId);
            }
            List<TeacherPlanFullDetailsModel> planDetails = TeacherPlans.GetTeacherPlanFullDetails(planningsToCall, detailsRequest != null ? detailsRequest?.SubjectCode : null, detailsRequest != null ? detailsRequest?.LevelCode : null);
            if (planDetails.Count == 0)
            {
                return homeworkResponseModel;
            }

            // Get the assignments results 
            List<string> userIdsToCall = new List<string>();
            userIdsToCall.AddRange(userIds); // Add the students already


            List<string> quizzessIds = new List<string>();
            List<string> planQuizzesIds = new List<string>();

            // Get the assignments results                
            foreach (TeacherPlanFullDetailsModel teacherPlan in planDetails)
            {
                foreach (HomeworkAssignmentModel assignmentPlan in teacherPlan.HomeworkAssignmentModels)
                {
                    planQuizzesIds.Add(assignmentPlan.Id);
                    quizzessIds.Add(assignmentPlan.QuizId);
                    userIdsToCall.Add(teacherPlan.TeacherPlanModel.TeacherId);
                }
            }
            // If there is no assingments to call and we just need the assignments, then return an empty object.
            if (quizzessIds.Count == 0)
            {
                return homeworkResponseModel;
            }


            // Get the teacher and student information.
            List<UserResponse> userSharedDetails = new List<UserResponse>();
            try
            {
                userSharedDetails = GetUserDetails.GetUsersSharedDetails(token, ipAddress, userIdsToCall);
            }
            catch (Exception)
            {
                // Continue without the names. This is in another API and could affect the performance.
            }

            // Now get the details 
            return CalculateAssingmentsAverage(userIds, userSharedDetails, planDetails, typePendingOrComplete, UserResults.GetUserQuizResults(userIds, quizzessIds, planQuizzesIds), detailsRequest.FromPage, detailsRequest.Limiter);

        }

        /// <summary>
        /// Get the groups Average Quizes from plan. 
        /// </summary>
        /// <param name="planId">The Plan's Id</param>
        /// <param name="isAssignment">Indicate for find lessons or assignments.</param>
        /// <returns>Object with List of quiz average.</returns>
        public static object GetGroupAverage(string planId, bool isAssignment)
        {
            // Get the planning details exams with the results
            List<ReportQuizModel> studentQuizResultsPerPlanList = null;

            if (isAssignment)
            {
                studentQuizResultsPerPlanList = UserResults.GetStudentsTeacherCustomQuizzesResultsByGroup(planId);
            }
            else
            {
                studentQuizResultsPerPlanList = UserResults.GetStudentsLessonExamsResultsByGroup(planId);
            }

            // For each exam, calculate the average 
            // Calculate average score per QuizId
            IEnumerable<object> averageScores = studentQuizResultsPerPlanList
                .Where(q => q.Score != null)
                .GroupBy(q => new { q.QuizId, q.DueDate, q.DueDateTimeZone, LessonId = isAssignment ? null : q.LessonId })
                .Select(group => new
                {
                    group.Key.QuizId,
                    AverageScore = group.Average(q => decimal.Parse(q.Score)).ToString(),
                }).ToList();

            return new
            {
                ReportByGroupingResultsAverageModels = averageScores,
            };
        }

        /// <summary>
        /// To get the students-groups' teachers created assignments results.
        /// </summary>
        /// <param name="token">The user requester's token.</param>
        /// <param name="ipAddress">The user's making the request IP.</param>
        /// <param name="planId">The id of the plan (required).</param>
        /// <param name="groupCode">The group's code.</param>
        /// <param name="teacherGroupId">The teacher's group.</param>
        /// <param name="fromDate">From date to call the results.</param>
        /// <param name="quizFromLimiter">From which item to call.</param>
        /// <param name="quizLimiter">How many items to call.</param>
        /// <param name="studentLimiter">How many students per call.</param>
        /// <param name="studentFromLimiter">student start limit.</param>
        /// <param name="toDate">To date to call the results. </param>
        public static ReportScoreGroupOverallModel GetGroupResultsTeacherAssignments(string token, string ipAddress,
            string planId,
            int quizLimiter = 10, int quizFromLimiter = 0, int studentLimiter = 10, int studentFromLimiter = 0,
            string groupCode = null, string teacherGroupId = null, string fromDate = null, string toDate = null)
        {
            var reportScoreGroupToReturn = new ReportScoreGroupOverallModel();

            // Get the lessons quizzes related to the plan, so we can calculate everything based on this.
            var planCustomQuizzes = TeacherPlans.GetTeacherPlanCustomHomework(new List<string>() { planId });
            if (planCustomQuizzes.Count == 0)
            {
                return reportScoreGroupToReturn;
            }

            // Filter out by due dates
            DateTime fromDateDT, toDateDT;
            if (fromDate != null)
            {
                if (!DateTime.TryParse(fromDate, out fromDateDT))
                {
                    fromDateDT = DateTime.Now.AddDays(-14);
                }

                planCustomQuizzes = planCustomQuizzes.Where(x => DateTime.Parse(x.DueDate) >= fromDateDT).ToList();
            }

            if (toDate != null)
            {
                if (!DateTime.TryParse(toDate, out toDateDT))
                {
                    toDateDT = DateTime.Now;
                }

                planCustomQuizzes = planCustomQuizzes.Where(x => DateTime.Parse(x.DueDate) <= toDateDT).ToList();
            }

            planCustomQuizzes = planCustomQuizzes.OrderBy(x => DateTime.Parse(x.DueDate)).ToList();
            planCustomQuizzes = planCustomQuizzes
                .Skip(quizFromLimiter) // Skip the first N items based on quizFromLimiter
                .Take(quizLimiter) // Take only the next N items based on quizLimiter
                .ToList();

            if (planCustomQuizzes == null || planCustomQuizzes.Count == 0)
            {
                return reportScoreGroupToReturn;
            }

            // We get the planning details exams with the results (the results are in a left join)
            var studentQuizResultsPerPlanList =
                UserResults.GetStudentsTeacherCustomQuizzesResultsByGroup(planId, fromDate, toDate, teacherGroupId);

            // Now, we retrieve the users related to that plan id 
            var usersRelatedToPlan = TeacherGroups.GetTeacherPlanStudents(planId, teacherGroupId);

            // Retrieve the groups to filter out. 
            List<string> userIdsToCallGroups = usersRelatedToPlan.Select(q => q.StudentId).ToList();
            var userGroups = GetUserDetails.GetUsersMainGroups(token, ipAddress, userIdsToCallGroups);

            List<UserMainGroupModel> userGroupsFiltered = new List<UserMainGroupModel>();
            if (!string.IsNullOrWhiteSpace(groupCode))
            {
                userGroupsFiltered = userGroups.Where(m => m.GroupCode == groupCode).ToList();
                // Filter the studentQuizResultsPerPlanList by userGroupsFiltered
                studentQuizResultsPerPlanList = studentQuizResultsPerPlanList
                    .Where(x => userGroupsFiltered.Any(y => y.UserId == x.UserId)).ToList();

                // Filter the usersRelatedToPlan by userGroupsFiltered
                usersRelatedToPlan = usersRelatedToPlan.Where(x => userGroupsFiltered.Any(y => y.UserId == x.StudentId))
                    .ToList();
            }
            else
            {
                userGroupsFiltered = userGroups;
            }

            // Call the user names after filtering out. 
            List<string> userIdsToCallNames = usersRelatedToPlan.Select(q => q.StudentId).ToList();

            // Get the names 
            List<UserFullNameModel> userFullNames = new List<UserFullNameModel>();
            if (userIdsToCallNames.Count > 0)
            {
                userFullNames = GetUserDetails.GetProfileConnectionsFullNames(token, ipAddress, userIdsToCallNames);
            }

            userFullNames = userFullNames.OrderBy(x => x.Surname).ThenBy(x => x.GivenName).ToList();

            // For each exam, calculate the average 
            // Calculate average score per QuizId
            var averageScores = studentQuizResultsPerPlanList
                .Where(q => q.Score != null)
                .GroupBy(q => new { q.QuizId, q.DueDate, q.DueDateTimeZone })
                .Select(group => new
                {
                    QuizId = group.Key.QuizId,
                    DueDate = group.Key.DueDate,
                    DueDateTimeZone = group.Key.DueDateTimeZone,
                    AverageScore = group.Average(q => Decimal.Parse(q.Score))
                });

            // Now 'averageScores' contains the average score per QuizId
            List<ReportByGroupingResultsModel> reportsGrouping = new List<ReportByGroupingResultsModel>();
            foreach (var planCustomQuiz in planCustomQuizzes)
            {
                // Find the average score
                var averageScore = averageScores.FirstOrDefault(x => x.QuizId == planCustomQuiz.QuizId);
                string averageText = "-";
                if (averageScore != null)
                {
                    averageText = averageScore.AverageScore.ToString();
                }

                reportsGrouping.Add(new ReportByGroupingResultsModel()
                {
                    GroupAverage = averageText,
                    QuizId = planCustomQuiz.QuizId,
                    QuizTitle = planCustomQuiz.Title,
                    DueDate = planCustomQuiz.DueDate,
                    ForGrading = int.Parse(planCustomQuiz.ForGrading)
                });
            }

            reportsGrouping = reportsGrouping.OrderBy(x => DateTime.Parse(x.DueDate)).ToList();
            reportScoreGroupToReturn.ReportByGroupingResultsModels = reportsGrouping;

            // Now, prepare the horizontal list, which is the name of the user, the group details, and the results per exam 
            List<ReportGroupStudentResultsModel> reportStudentDetails = new List<ReportGroupStudentResultsModel>();
            foreach (var student in usersRelatedToPlan)
            {
                var reportGroupStudentResult = new ReportGroupStudentResultsModel()
                {
                    UserId = student.StudentId,
                    TeacherGroups = new List<string>(),
                    ReportQuizModels = new List<ReportQuizModel>(),
                };

                // Find the student name
                var studentName = userFullNames.Find(x => x.UserId == student.StudentId);
                if (studentName != null)
                {
                    reportGroupStudentResult.GivenName = studentName.GivenName;
                    reportGroupStudentResult.Surname = studentName.Surname;
                }

                // Find the main group code if any 
                if (userGroupsFiltered != null && userGroupsFiltered.Count > 0)
                {
                    var mainGroup = userGroupsFiltered?.Find(x => x.UserId == student.StudentId);
                    if (mainGroup != null)
                    {
                        reportGroupStudentResult.MainGroupCode = mainGroup.GroupCode;
                    }
                }

                // Get the teacher groups.
                if (usersRelatedToPlan != null && usersRelatedToPlan.Count > 0)
                {
                    var userTeacherGroups = usersRelatedToPlan?.FindAll(x => x.StudentId == student.StudentId);
                    if (userTeacherGroups != null && userTeacherGroups.Count > 0)
                    {
                        foreach (var userTeacherGroup in userTeacherGroups)
                        {
                            reportGroupStudentResult.TeacherGroups.Add(userTeacherGroup.GroupName);
                        }
                    }
                }

                // Now, evaluate if the student made every quiz
                foreach (var quizLesson in planCustomQuizzes)
                {
                    if (quizLesson != null)
                    {
                        // Check if the student has completed it. 
                        var userQuizResult = studentQuizResultsPerPlanList.Find(x => x.UserId == student.StudentId
                            && x.QuizId == quizLesson.QuizId
                            && x.PlanId == planId);

                        var reportDetailsStudent = new ReportQuizModel()
                        {
                            QuizId = quizLesson.QuizId,
                            PlanId = planId,
                            DueDate = quizLesson.DueDate,
                            UserId = student.StudentId,
                            Premises = userQuizResult?.Premises,
                            TotalMaxScore = quizLesson.TotalMaxScore.ToString(),
                            TeacherPlanQuizId = quizLesson.Id,
                        };

                        if (userQuizResult != null)
                        {
                            reportDetailsStudent.Score = userQuizResult.Score;
                            reportDetailsStudent.UsquiUserResultId = userQuizResult.UsquiUserResultId;
                            if (string.IsNullOrEmpty(reportDetailsStudent.Score))
                            {
                                reportDetailsStudent.Score = "P"; // Pending to correct, since the student already
                                // submitted the assignment, but the teacher has not graded it yet.
                            }
                        }
                        else
                        {
                            reportDetailsStudent.Score = "-";
                        }

                        reportGroupStudentResult.ReportQuizModels.Add(reportDetailsStudent);
                    }
                }

                reportStudentDetails.Add(reportGroupStudentResult);
            }

            reportScoreGroupToReturn.ReportGroupStudentResultsModels = reportStudentDetails;
            // Order by Student Name
            reportScoreGroupToReturn.ReportGroupStudentResultsModels = reportScoreGroupToReturn
                .ReportGroupStudentResultsModels.OrderBy(x => x.Surname).ThenBy(x => x.GivenName).ToList();

            // Limit the students
            reportScoreGroupToReturn.ReportGroupStudentResultsModels = reportScoreGroupToReturn
                .ReportGroupStudentResultsModels.Skip(studentFromLimiter)
                .Take(studentLimiter) // Take only the next N items based on student limiter
                .ToList(); // Skip the first N items based on quizFromLimiter

            return reportScoreGroupToReturn;
        }

        /// <summary>
        /// To calculate the assignments averages.
        /// </summary>
        /// <param name="userIds">The users' to calculate the averages per plan.</param>
        /// <param name="userSharedDetails">The user shared detials, that includes the users' names, etc.</param>
        /// <param name="planDetails">The details of the plan.</param>
        /// <param name="typePendingOrComplete">1 for call pending, 2 for call completed, else to call pending and completed homework.</param>
        /// <param name="userQuizzessResults">The users' results to calculate the average.</param>
        /// <param name="fromPage">To skip the results according to this number.</param>
        /// <param name="limiter">How many items should we called.</param>
        /// <returns>Returns the homework response model with assignments averages.</returns>
        private static HomeworkResponseModel CalculateAssingmentsAverage(List<string> userIds, List<UserResponse> userSharedDetails, List<TeacherPlanFullDetailsModel> planDetails, int typePendingOrComplete, List<Domain.Models.Generals.Dapper.UserQuizResultModel> userQuizzessResults, int fromPage, int limiter)
        {
            HomeworkResponseModel homeworkResponseModel = new HomeworkResponseModel()
            {
                PlanDetails = new List<PlanDetail>()
            };

            // To add the to average
            var averageDoubles = new List<decimal>();
            foreach (string userId in userIds)
            {
                // Get the student info 
                UserResponse studentInfo = userSharedDetails?.Find(x => x.UserId == userId);

                foreach (TeacherPlanFullDetailsModel teacherPlan in planDetails)
                {

                    // Find the teacher info.                  
                    UserResponse teacherDetails = userSharedDetails?.Find(x => x.UserId == teacherPlan.TeacherPlanModel.TeacherId);

                    // Stting the plan detail.
                    PlanDetailHomeworkModel planDetail = new PlanDetailHomeworkModel()
                    {
                        SubjectCode = teacherPlan.TeacherPlanModel.SubjectCode,
                        LevelCode = teacherPlan.TeacherPlanModel.LevelCode,
                        SubjectName = teacherPlan.TeacherPlanModel.SubjectName,
                        AssignmentDetails = new List<StudentHomeworkDetail>(),
                        PeriodId = teacherPlan.TeacherPlanModel.PeriodId,
                        TeacherId = teacherPlan.TeacherPlanModel.TeacherId,
                        TeacherName = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                        PlanId = teacherPlan.TeacherPlanModel.PlanId,
                        PlanName = teacherPlan.TeacherPlanModel.PlanName,
                    };

                    // To calculate the average of the plan. 
                    var averagePlan = new List<decimal>();

                    foreach (HomeworkAssignmentModel assignmentPlan in teacherPlan.HomeworkAssignmentModels)
                    {
                        // Pending logic
                        if (typePendingOrComplete == 1)
                        {
                            if (userQuizzessResults.Find(x => x.QuizId == assignmentPlan.QuizId && x.UserId == userId) == null && planDetail.AssignmentDetails.Find(x => x.TeacherPlanQuizId == assignmentPlan.Id && x.StudentId == userId) == null)
                            {
                                planDetail.AssignmentDetails.Add(new HomeworkDetail()
                                {
                                    TriesQuantity = assignmentPlan.TriesQuantity,
                                    CurrentAvailableTries = assignmentPlan.TriesQuantity,
                                    SentBy = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                                    Status = "P",
                                    DueDate = assignmentPlan.DueDate,
                                    ForGrading = assignmentPlan.ForGrading ? "1" : "0",
                                    SubmittedDate = "-",
                                    Title = assignmentPlan.Title,
                                    QuizId = assignmentPlan.QuizId,
                                    TeacherPlanQuizId = assignmentPlan.Id,
                                    StudentId = userId,
                                    StudentName = studentInfo?.GivenName + " " + studentInfo?.Surname,
                                    MaxScore = assignmentPlan.TotalMaxScore
                                });
                            }
                        }
                        // Complete logic
                        else if (typePendingOrComplete == 2)
                        {
                            var userQuizzessFound = userQuizzessResults.FindAll(x => x.QuizId == assignmentPlan.QuizId && x.UserId == userId);

                            if (userQuizzessFound != null && userQuizzessFound.Count > 0 && !string.IsNullOrWhiteSpace(userQuizzessFound[0].TotalPercent) && planDetail.AssignmentDetails.Find(x => x.TeacherPlanQuizId == assignmentPlan.Id && x.StudentId == userId) == null)
                            {

                                var highestScore = userQuizzessFound.OrderByDescending(x => double.Parse(x.TotalPercent)).ToList()[0];
                                if (highestScore != null && !string.IsNullOrWhiteSpace(highestScore.TotalPercent) && assignmentPlan.ForGrading)
                                {
                                    averageDoubles.Add(decimal.Parse(highestScore?.TotalPercent));
                                    averagePlan.Add(decimal.Parse(highestScore?.TotalPercent));
                                }

                                planDetail.AssignmentDetails.Add(new HomeworkDetail()
                                {
                                    TriesQuantity = assignmentPlan.TriesQuantity,
                                    CurrentAvailableTries = assignmentPlan.TriesQuantity - userQuizzessFound.Count,
                                    SentBy = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                                    Status = "C",
                                    DueDate = assignmentPlan.DueDate,
                                    ForGrading = assignmentPlan.ForGrading ? "1" : "0",
                                    SubmittedDate = highestScore?.CreatedAt,
                                    TotalPercent = highestScore?.TotalPercent,
                                    TotalCorrect = highestScore?.TotalCorrect,
                                    TotalQuestions = highestScore?.TotalQuestions,
                                    Title = assignmentPlan.Title,
                                    QuizId = assignmentPlan.QuizId,
                                    TeacherPlanQuizId = assignmentPlan.Id,
                                    UserQuizResultId = highestScore?.Id,
                                    StudentId = userId,
                                    StudentName = studentInfo?.GivenName + " " + studentInfo?.Surname,
                                    MaxScore = assignmentPlan.TotalMaxScore,
                                    Score = highestScore?.Score,
                                    ManuallyCorrectedByTeacher = highestScore?.ManuallyCorrectedByTeacher == "1"
                                });
                            }
                        }
                        else // Mix
                        {
                            var userQuizzessFound = userQuizzessResults.FindAll(x => x.QuizId == assignmentPlan.QuizId && x.UserId == userId);

                            if (planDetail.AssignmentDetails.Find(x => x.TeacherPlanQuizId == assignmentPlan.Id && x.StudentId == userId) == null)
                            {
                                Domain.Models.Generals.Dapper.UserQuizResultModel highestScore = null;
                                if (userQuizzessFound != null && userQuizzessFound.Count > 0)
                                {
                                    highestScore = userQuizzessFound.OrderByDescending(x => decimal.Parse(x.TotalPercent != null ? x.TotalPercent : "0".ToString())).ToList()?[0];
                                }
                                if (highestScore != null && !string.IsNullOrWhiteSpace(highestScore.TotalPercent) && assignmentPlan.ForGrading)
                                {
                                    averageDoubles.Add(decimal.Parse(highestScore?.TotalPercent));
                                    averagePlan.Add(decimal.Parse(highestScore?.TotalPercent));
                                }

                                planDetail.AssignmentDetails.Add(new HomeworkDetail()
                                {
                                    TriesQuantity = assignmentPlan.TriesQuantity,
                                    CurrentAvailableTries = assignmentPlan.TriesQuantity - userQuizzessFound.Count,
                                    SentBy = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                                    Status = string.IsNullOrWhiteSpace(highestScore?.TotalPercent) ? "P" : "C",
                                    DueDate = assignmentPlan.DueDate,
                                    ForGrading = assignmentPlan.ForGrading ? "1" : "0",
                                    SubmittedDate = highestScore != null ? highestScore.CreatedAt : "-",
                                    TotalPercent = highestScore?.TotalPercent,
                                    TotalCorrect = highestScore?.TotalCorrect,
                                    TotalQuestions = highestScore?.TotalQuestions,
                                    Title = assignmentPlan.Title,
                                    QuizId = assignmentPlan.QuizId,
                                    TeacherPlanQuizId = assignmentPlan.Id,
                                    UserQuizResultId = highestScore?.Id,
                                    StudentId = userId,
                                    StudentName = studentInfo?.GivenName + " " + studentInfo?.Surname,
                                    MaxScore = assignmentPlan.TotalMaxScore,
                                    Score = highestScore?.Score,
                                });
                            }
                        }
                    }

                    planDetail.OverallAveragePlan = averagePlan.Count == 0 ? "0" : AverageCalculator.CalculateAverage(averagePlan.ToArray(), true).ToString();

                    // Calculate limit pages 
                    planDetail.TotalPages = int.Parse(Math.Ceiling((double)planDetail.AssignmentDetails.Count() / limiter).ToString());
                    planDetail.AssignmentDetails = planDetail.AssignmentDetails.Skip(fromPage * limiter).Take(limiter).ToList();

                    // Add to plan details
                    homeworkResponseModel.PlanDetails.Add(planDetail);
                }
            }

            homeworkResponseModel.OverallAverage = averageDoubles.Count == 0 ? "0" : AverageCalculator.CalculateAverage(averageDoubles.ToArray(), true).ToString();
            homeworkResponseModel.PlanDetails = homeworkResponseModel.PlanDetails.Skip(fromPage * limiter).Take(limiter).ToList();

            return homeworkResponseModel;
        }

        /// <summary>
        /// To calculate the lesson averages.
        /// </summary>
        /// <param name="userIds">The users' to calculate the averages per plan.</param>
        /// <param name="userSharedDetails">The user shared detials, that includes the users' names, etc.</param>
        /// <param name="planDetails">The details of the plan.</param>
        /// <param name="typePendingOrComplete">1 for call pending, 2 for call completed, else to call pending and completed homework.</param>
        /// <param name="planLessonUserAverages">The users' plan lesson averages to calculate the average.</param>
        /// <param name="fromPage">To skip the results according to this number.</param>
        /// <param name="limiter">How many items should we called.</param>
        /// <returns>Returns the homework response model with assignments averages.</returns>
        private static LessonAssignmentResponseModel CalculateLessonAverages(List<string> userIds, List<UserResponse> userSharedDetails, List<TeacherPlanFullDetailsModel> planDetails, int typePendingOrComplete, List<PlanLessonUserModel> planLessonUserAverages, int fromPage, int limiter)
        {
            LessonAssignmentResponseModel homeworkResponseModel = new LessonAssignmentResponseModel()
            {
                PlanDetails = new List<PlanDetailLessonModel>(),
            };

            // To add the to average
            var averageDoubles = new List<decimal>();

            List<LessonModel> lessonModelsToCallAthenas = new List<LessonModel>();
            foreach (string userId in userIds)
            {
                // Get the student info 
                UserResponse studentInfo = userSharedDetails.Find(x => x.UserId == userId);

                foreach (TeacherPlanFullDetailsModel teacherPlan in planDetails)
                {

                    // Find the teacher info.                  
                    UserResponse teacherDetails = userSharedDetails.Find(x => x.UserId == teacherPlan.TeacherPlanModel.TeacherId);

                    // Stting the plan detail.
                    PlanDetailLessonModel planDetail = new PlanDetailLessonModel()
                    {
                        SubjectCode = teacherPlan.TeacherPlanModel.SubjectCode,
                        LevelCode = teacherPlan.TeacherPlanModel.LevelCode,
                        SubjectName = teacherPlan.TeacherPlanModel.SubjectName,
                        AssignmentDetails = new List<StudentLessonAssignmentDetail>(),
                        PeriodId = teacherPlan.TeacherPlanModel.PeriodId,
                        TeacherId = teacherPlan.TeacherPlanModel.TeacherId,
                        TeacherName = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                        PlanId = teacherPlan.TeacherPlanModel.PlanId,
                        PlanName = teacherPlan.TeacherPlanModel.PlanName,
                    };

                    // To calculate the average of the plan. 
                    var averagePlan = new List<decimal>();

                    foreach (LessonAssignmentModel assignmentPlan in teacherPlan.LessonAssignmentModels)
                    {
                        // Pending logic
                        if (typePendingOrComplete == 1)
                        {
                            if ((planLessonUserAverages == null || planLessonUserAverages.Count == 0 || planLessonUserAverages.Find(x => x.LessonId == assignmentPlan.LessonId && x.Completed != "1" && x.UserId == userId) == null) && planDetail.AssignmentDetails.Find(x => x.LessonId == assignmentPlan.LessonId && x.StudentId == userId) == null)
                            {
                                planDetail.AssignmentDetails.Add(new StudentLessonAssignmentDetail()
                                {
                                    TriesQuantity = assignmentPlan.TriesQuantity,
                                    CurrentAvailableTries = assignmentPlan.TriesQuantity,
                                    SentBy = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                                    Status = "P",
                                    DueDate = assignmentPlan.DueDate,
                                    ForGrading = assignmentPlan.ForGrading ? "1" : "0",
                                    SubmittedDate = "-",
                                    Title = assignmentPlan.Title,
                                    LessonId = assignmentPlan.LessonId,
                                    TeacherPlanLessonId = assignmentPlan.Id,
                                    StudentId = userId,
                                    StudentName = studentInfo?.GivenName + " " + studentInfo?.Surname,
                                });
                            }
                        }
                        // Complete logic
                        else if (typePendingOrComplete == 2)
                        {
                            PlanLessonUserModel averageUserLesson = planLessonUserAverages?.Find(x => x.LessonId == assignmentPlan.LessonId && x.Completed == "1" && x.UserId == userId);
                            if (averageUserLesson != null && assignmentPlan.ForGrading)
                            {
                                averagePlan.Add(decimal.Parse(averageUserLesson.OverallScore.ToString()));
                                averageDoubles.Add(decimal.Parse(averageUserLesson.OverallScore.ToString()));
                            }
                            if (planLessonUserAverages != null && planLessonUserAverages.Count == 0 && averageUserLesson != null && planDetail.AssignmentDetails.Find(x => x.LessonId == assignmentPlan.LessonId && x.StudentId == userId) == null)
                            {
                                planDetail.AssignmentDetails.Add(new StudentLessonAssignmentDetail()
                                {
                                    TriesQuantity = assignmentPlan.TriesQuantity,
                                    SentBy = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                                    Status = "C",
                                    DueDate = assignmentPlan.DueDate,
                                    ForGrading = assignmentPlan.ForGrading ? "1" : "0",
                                    SubmittedDate = averageUserLesson.UpdatedAt,
                                    Title = assignmentPlan.Title,
                                    LessonId = assignmentPlan.LessonId,
                                    TeacherPlanLessonId = assignmentPlan.Id,
                                    StudentId = userId,
                                    StudentName = studentInfo?.GivenName + " " + studentInfo?.Surname,
                                    TotalPercent = decimal.Parse(averageUserLesson.OverallScore),
                                });
                            }
                        }
                        else // Mix
                        {
                            PlanLessonUserModel averageUserLesson = planLessonUserAverages?.Find(x => x.LessonId == assignmentPlan.LessonId && x.Completed == "1" && x.UserId == userId);
                            if (averageUserLesson != null && assignmentPlan.ForGrading)
                            {
                                averagePlan.Add(decimal.Parse(averageUserLesson.OverallScore.ToString()));
                                averageDoubles.Add(decimal.Parse(averageUserLesson.OverallScore.ToString()));
                            }

                            if (planDetail.AssignmentDetails.Find(x => x.LessonId == assignmentPlan.LessonId && x.StudentId == userId) == null)
                            {
                                planDetail.AssignmentDetails.Add(new StudentLessonAssignmentDetail()
                                {
                                    TriesQuantity = assignmentPlan.TriesQuantity,
                                    SentBy = teacherDetails?.GivenName + " " + teacherDetails?.Surname,
                                    Status = averageUserLesson?.Completed == "1" ? "C" : "P",
                                    DueDate = assignmentPlan.DueDate,
                                    ForGrading = assignmentPlan.ForGrading ? "1" : "0",
                                    SubmittedDate = averageUserLesson?.Completed == "1" ? averageUserLesson.UpdatedAt : "-",
                                    Title = assignmentPlan.Title,
                                    LessonId = assignmentPlan.LessonId,
                                    TeacherPlanLessonId = assignmentPlan.Id,
                                    StudentId = userId,
                                    StudentName = studentInfo?.GivenName + " " + studentInfo?.Surname,
                                    TotalPercent = averageUserLesson != null ? decimal.Parse(averageUserLesson.OverallScore) : 0,
                                });
                            }
                        }
                    }

                    planDetail.OverallAveragePlan = averagePlan.Count == 0 ? "0" : AverageCalculator.CalculateAverage(averagePlan.ToArray(), true).ToString();

                    // Calculate limit pages 
                    planDetail.TotalPages = int.Parse(Math.Ceiling((double)planDetail.AssignmentDetails.Count() / limiter).ToString());
                    planDetail.AssignmentDetails = planDetail.AssignmentDetails.Skip(fromPage * limiter).Take(limiter).ToList();

                    // Set the lessons to call 
                    foreach (StudentLessonAssignmentDetail homework in planDetail.AssignmentDetails)
                    {
                        lessonModelsToCallAthenas.Add(new LessonModel()
                        {
                            Id = homework.LessonId
                        });
                    }

                    // Add to plan details
                    homeworkResponseModel.PlanDetails.Add(planDetail);
                }
            }

            // Set Athenas details
            LessonsResponseModel lessonsAthenas = AthenasHandler.GetAthenasLessons(lessonModelsToCallAthenas, false);

            foreach (PlanDetailLessonModel planDetail in homeworkResponseModel.PlanDetails)
            {
                foreach (StudentLessonAssignmentDetail homeworkDetail in planDetail.AssignmentDetails)
                {
                    homeworkDetail.Title = lessonsAthenas?.LessonModelList.Find(x => x.Id == homeworkDetail.LessonId)?.LessonTitle;
                }
            }

            homeworkResponseModel.OverallAverage = averageDoubles.Count == 0 ? "0" : AverageCalculator.CalculateAverage(averageDoubles.ToArray(), true).ToString();
            homeworkResponseModel.PlanDetails = homeworkResponseModel.PlanDetails.Skip(fromPage * limiter).Take(limiter).ToList();
            return homeworkResponseModel;
        }
    }
}