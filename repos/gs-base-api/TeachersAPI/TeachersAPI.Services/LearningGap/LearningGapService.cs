using TeachersAPI.TeachersAPI.Infrastructure.Repositories.LearningGap;
using TeachersAPI.Domain.Models.LearningGap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Responses;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.LearningGap
{
    /// <summary>
    /// To handle Learning Gap module.
    /// </summary>
    public class LearningGapService
    {
        private readonly LearningGapValidations _learningGapValidations;
        private readonly string _connectionStringRead, _connectionStringWrite;
        private readonly string _langCode;

        /// <summary>
        /// The constructor of the learning gap service.
        /// </summary>
        /// <param name="connectionStringRead">Connection string to read.</param>
        /// <param name="connectionStringWrite">Connection string to write.</param>
        /// <param name="langCode">The language code.</param>
        public LearningGapService(string connectionStringRead, string connectionStringWrite, string langCode)
        {
            _learningGapValidations = new LearningGapValidations(connectionStringRead, langCode);
            this._connectionStringRead = connectionStringRead;
            this._connectionStringWrite = connectionStringWrite;
            this._langCode = langCode;
        }

        #region student

        /// <summary>
        /// To get learning gap initial data settings for the teacher by the student.
        /// If there are results, then the teacher will not be asked to enter the initial data.
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns the learning gap initial data setting model.</returns>
        public LearningGapInitialDataSettingModel ReadLearningGapInitialDataSettings(string teacherId, string studentId)
        {
            var learningSetting = new LearningGapInitialDataSettingDAO().Read(_connectionStringRead,
                new LearningGapInitialDataSettingModel()
                {
                    TeacherId = teacherId,
                    StudentId = studentId
                }, true, true);

            if (learningSetting == null || learningSetting.Count == 0)
            {
                return new LearningGapInitialDataSettingModel();
            }

            return learningSetting[0];
        }

        /// <summary>
        /// To post a learning initial data setting. When created, the user will not be asked to 
        /// enter the initial data.
        /// </summary>
        /// <param name="teacherId">The id of the teacher.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the created or the existing initial data setting model.</returns>
        public LearningGapInitialDataSettingModel PostLearningGapInitialDataSettings(string teacherId, string studentId)
        {
            var learningSetting = new LearningGapInitialDataSettingDAO().Read(_connectionStringRead,
                new LearningGapInitialDataSettingModel()
                {
                    TeacherId = teacherId,
                    StudentId = studentId
                }, true, true);

            // If exists, update it.
            if (learningSetting == null || learningSetting.Count == 0)
            {
                // Create a new one.
                learningSetting = new LearningGapInitialDataSettingDAO().Create(_connectionStringWrite, new LearningGapInitialDataSettingModel()
                {
                    TeacherId = teacherId,
                    StudentId = studentId,
                    ModifiedBy = studentId
                });
            }

            learningSetting[0].Valid = true;
            return learningSetting[0];
        }

        /// <summary>
        /// To read the pending lessons assignments for gap.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="token">The token.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <returns>Returns the learning gap assignled lessons list.</returns>
        public PaginatedResult<LearningGapAssignedLessonModel> ReadPendingLessonAssignmentsGap(string studentId, int fromPage, int limiter, string token)
        {
            var activeAcademicYears = this.GetStudentActiveAcademicYears(token, studentId);
            // Read the pending lessons.
            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();

            // Prepare the learning gap assigned lessons to find. 
            var learningGapAssignedLessonsToFind = new List<LearningGapAssignedLessonModel>();
            if(activeAcademicYears != null)
            {
                foreach(var academicYear in activeAcademicYears)
                {
                    learningGapAssignedLessonsToFind.Add(new LearningGapAssignedLessonModel()
                    {
                        AcademicYearId = academicYear,
                        StudentId = studentId,
                        Completed = "0"
                    });
                }
            }
            else
            {
                // Return emtpy
                return new PaginatedResult<LearningGapAssignedLessonModel>(0, new List<LearningGapAssignedLessonModel>());
            }

            var lessons = learningGapAssignedLessonDAO.Read(_connectionStringRead, learningGapAssignedLessonsToFind);

            // If there are no lessons, return an empty list.
            if (lessons == null || lessons.Count == 0)
            {
                return new PaginatedResult<LearningGapAssignedLessonModel>(0, new List<LearningGapAssignedLessonModel>());
            }

            // Order by the limit date.
            lessons = lessons.OrderBy(x => DateTime.Parse(x.LimitDate)).ToList();

            // Prepare the result, calculating the total pages according to the limiter and results.
            var totalPages = lessons.Count / limiter;
            if (lessons.Count % limiter > 0)
            {
                totalPages++;
            }

            // Paginate the results.
            var paginatedResults = lessons.Skip(fromPage * limiter).Take(limiter).ToList();

            // Call the lessons in Athenas. 
            var athenasLessonsToRead = new List<LessonModel>();
            foreach (var lesson in paginatedResults)
            {
                athenasLessonsToRead.Add(new LessonModel()
                {
                    Id = lesson.LessonId
                });
            }

            // Read the lessons.
            var athenasLessons = FeignAPI.AthenasHandler.GetAthenasLessons(athenasLessonsToRead, true);

            // Merge the results.
            foreach (var lesson in paginatedResults)
            {
                var athenasLesson = athenasLessons.LessonModelList.FirstOrDefault(x => x.Id == lesson.LessonId);
                if (athenasLesson != null)
                {
                    lesson.SubjectName = athenasLesson.SubjectCode;
                    lesson.LevelName = athenasLesson.LevelCode;
                    lesson.LessonTitle = athenasLesson.LessonTitle;
                }
            }

            // Prepare the response.
            var response = new List<LearningGapAssignedLessonModel>();
            foreach (var lesson in paginatedResults)
            {
                response.Add(new LearningGapAssignedLessonModel()
                {
                    Id = lesson.Id,
                    LessonId = lesson.LessonId,
                    SubjectName = lesson.SubjectName,
                    LevelName = lesson.LevelName,
                    LessonTitle = lesson.LessonTitle,
                    LimitDate = lesson.LimitDate,
                    MaxTries = lesson.MaxTries
                });
            }

            return new PaginatedResult<LearningGapAssignedLessonModel>(totalPages, response);
        }

        /// <summary>
        /// To get the overall score for student dashboard.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns the overall score of the student.</returns>
        public List<LearningGapStudentOverallScoreModel> GetOverallScore(string studentId, string token)
        {
            // Get the active academic years. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);
            // If there are no active academic years, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new List<LearningGapStudentOverallScoreModel>();
            }

            // For each academic year, prepare the read of the overall score.
            var overallScoreToRead = new List<LearningGapAssignedLessonModel>();
            foreach (var academicYear in academicYears)
            {
                overallScoreToRead.Add(new LearningGapAssignedLessonModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear
                });
            }

            // Get the overall score of students. 
            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();
            var lessonsStudentResults = learningGapAssignedLessonDAO.Read(_connectionStringRead, overallScoreToRead);

            // If there are no results, return an empty list.
            if (lessonsStudentResults == null || lessonsStudentResults.Count == 0)
            {
                return new List<LearningGapStudentOverallScoreModel>();
            }

            // For those lesson student results, read the learning gap lesson quiz results.

            var learningGapLessonQuizResultDAO = new LearningGapLessonQuizResultDAO();
            // Prepare the lesson quiz result according to the learning gap assigned lesson model. 
            var lessonQuizResultsToRead = new List<LearningGapLessonQuizResultModel>();
            foreach (var lessonStudentResult in lessonsStudentResults)
            {
                lessonQuizResultsToRead.Add(new LearningGapLessonQuizResultModel()
                {
                    LearningGapAssignedLessonId = lessonStudentResult.Id
                });
            }

            // Read the lesson quiz results.
            var lessonQuizResults = learningGapLessonQuizResultDAO.Read(_connectionStringRead, lessonQuizResultsToRead);

            // If empty, return an empty list.
            if (lessonQuizResults == null || lessonQuizResults.Count == 0)
            {
                return new List<LearningGapStudentOverallScoreModel>();
            }

            // Get the ids of the quizzes. 
            var quizIds = lessonQuizResults.Select(x => x.QuizId).ToList();
            // Call athenas quizzes. 
            var athenasQuizzes = FeignAPI.AthenasHandler.GetQuizAthenasModelList(quizIds);

            // Filter only the exams.
            var examQuizzes = athenasQuizzes.Where(x => x.IsExam == "1").ToList();

            // Filter lessonQuizResults to only include results from exams.
            var examQuizIds = examQuizzes.Select(x => x.Id).ToHashSet();


            // Filter only the results that are exams.
            lessonQuizResults = lessonQuizResults
            .Where(x => examQuizIds.Contains(x.QuizId)) // Ensure only exams are considered
            .ToList();

            // Now, for each subject code in learning gap assigned lesson model, get the overall score according to the learning gap lesson quiz results.

            var averageScoresBySubject = lessonQuizResults
            .Join(lessonsStudentResults,
                  quiz => quiz.LearningGapAssignedLessonId,
                  lesson => lesson.Id,
                  (quiz, lesson) => new { lesson.SubjectCode, quiz.Score })
            .GroupBy(x => x.SubjectCode)
            .Select(group => new
            {
                SubjectCode = group.Key,
                AverageScore = Math.Truncate(group.Average(x => double.Parse(x.Score)) * 100) / 100
            })
            .ToList();

            // Get Athenas Subjects.
            var athenasSubjects = FeignAPI.AthenasHandler.GetAthenasSubjects(_langCode);

            // Merge the results.
            var overallScores = new List<LearningGapStudentOverallScoreModel>();
            foreach (var averageScore in averageScoresBySubject)
            {
                var athenasSubject = athenasSubjects.FirstOrDefault(x => x.Code.ToLower() == averageScore.SubjectCode.ToLower());
                if (athenasSubject != null)
                {
                    overallScores.Add(new LearningGapStudentOverallScoreModel()
                    {
                        SubjectCode = averageScore.SubjectCode,
                        SubjectName = athenasSubject.Name,
                        Score = averageScore.AverageScore.ToString()
                    });
                }
            }

            return overallScores;

        }

        /// <summary>
        /// To check ig student has the quizzes available for the lessons.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="token">The user that is making the request token.</param>
        /// <returns>Returns the quiz athenas model list.</returns>
        public List<QuizAthenasModel> GetAvailableQuizzesForStudent(string studentId, string lessonId, string token)
        {
            // Get the Athenas exams. 
            var athenasExamsQuizzess = AthenasHandler.GetLessonQuizzes(lessonId);
            // Filter out practices. 
            athenasExamsQuizzess.Quizzes = athenasExamsQuizzess.Quizzes.Where(q => q.IsExam != "0").ToList();

            // Get the academic years of the student. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);
            // If there are no active academic years, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                // Quiz Athenas 
                var quizAthenas = new List<QuizAthenasModel>();
                // Prepare the response as exams not available. 
                foreach (var quiz in athenasExamsQuizzess.Quizzes)
                {
                    quizAthenas.Add(new QuizAthenasModel()
                    {
                        Available = false,
                        Id = quiz.Id,
                        CustomValue = quiz.CustomValue,
                        Instruction = quiz.Instruction,
                        IsExam = quiz.IsExam,
                        LimitDate = null,
                        Title = quiz.Title,
                        LessonId = lessonId,
                        LevelOfKnowledge = quiz.LevelOfKnowledge
                    });
                }

                return quizAthenas;
            }

            // Call the assigned lessons 
            var assignedLessonsToCheck = new List<LearningGapAssignedLessonModel>();
            foreach (var academicYear in academicYears)
            {
                assignedLessonsToCheck.Add(new LearningGapAssignedLessonModel()
                {
                    LessonId = lessonId,
                    AcademicYearId = academicYear,
                    StudentId = studentId
                });
            }

            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();

            var assignedLessonsResults = learningGapAssignedLessonDAO.Read(this._connectionStringRead,
                assignedLessonsToCheck);

            if (assignedLessonsResults == null || assignedLessonsResults.Count == 0)
            {
                // Do not allow the exam.
                // Quiz Athenas 
                var quizAthenas = new List<QuizAthenasModel>();
                // Prepare the response as exams not available. 
                foreach (var quiz in athenasExamsQuizzess.Quizzes)
                {
                    quizAthenas.Add(new QuizAthenasModel()
                    {
                        Available = false,
                        Id = quiz.Id,
                        CustomValue = quiz.CustomValue,
                        Instruction = quiz.Instruction,
                        IsExam = quiz.IsExam,
                        LimitDate = null,
                        Title = quiz.Title,
                        LessonId = lessonId,
                        LevelOfKnowledge = quiz.LevelOfKnowledge,
                        QuizTypeId = quiz.QuizTypeId,
                    });
                }

                return quizAthenas;
            }

            // If there are results, then check the quizzes list. 
            var quizzesResultsToCheck = new List<LearningGapLessonQuizResultModel>();
            var learningGapLessonQuizResultDAO = new LearningGapLessonQuizResultDAO();

            foreach (var assignedLesson in assignedLessonsResults)
            {
                quizzesResultsToCheck.Add(new LearningGapLessonQuizResultModel()
                {
                    LearningGapAssignedLessonId = assignedLesson.Id,
                });
            }

            var quizzesResults = learningGapLessonQuizResultDAO.Read(this._connectionStringRead,
                quizzesResultsToCheck);

            // Now, for each Athenas quiz, check if there are results, if the actual try is less than 
            // assigned lesson max tries. 
            var athenasQuizList = new List<QuizAthenasModel>();
            foreach (var athenasQuiz in athenasExamsQuizzess.Quizzes)
            {
                // Check all the quizzes results. 
                var quizzesResultsFound = quizzesResults.FindAll(x => x.QuizId == athenasQuiz.Id);

                // If there are no results, then, allow the user to take the exam. 
                if (quizzesResultsFound == null)
                {
                    // Check if limit date still available. 
                    var lessons = assignedLessonsResults.FindAll(x => x.LessonId == athenasQuiz.LessonId);

                    // Check the limit date.
                    var dateAvailable = false;

                    // Get the current UTC date (just the date, no time)
                    var currentUtcDate = DateTime.UtcNow.Date;

                    var maxLimitDate = new DateTime();
                    foreach (var lesson in lessons)
                    {
                        // Parse the lesson's LimitDate and normalize it to just the date
                        DateTime lessonLimitDate = DateTime.Parse(lesson.LimitDate).Date;
                        DateTime originalLimitDate = DateTime.Parse(lesson.LimitDate).Date;

                        // Extend the lessonLimitDate to account for the latest global timezone (UTC-12)
                        lessonLimitDate = lessonLimitDate.AddHours(23).AddMinutes(59).AddSeconds(59).AddHours(12); // Ensuring full global availability

                        // Check if the lesson's LimitDate is today or in the past
                        if (currentUtcDate <= lessonLimitDate)
                        {
                            dateAvailable = true;
                            maxLimitDate = originalLimitDate;
                            break;
                        }
                    }

                    // Add the quiz availability to the list
                    athenasQuizList.Add(new QuizAthenasModel()
                    {
                        Available = dateAvailable,
                        Id = athenasQuiz.Id,
                        CustomValue = athenasQuiz.CustomValue,
                        Instruction = athenasQuiz.Instruction,
                        IsExam = athenasQuiz.IsExam,
                        LimitDate = maxLimitDate.ToShortDateString(),
                        Title = athenasQuiz.Title,
                        LessonId = athenasQuiz.LessonId,
                        LevelOfKnowledge = athenasQuiz.LevelOfKnowledge,
                        QuizTypeId = athenasQuiz.QuizTypeId,
                    });
                }
                else
                {
                    var lesson = assignedLessonsResults.FirstOrDefault(x => x.LessonId == athenasQuiz.LessonId);
                    // Handle the case where quiz results are found
                    // Check the max tries of quiz results against the max tries allowed for the lesson
                    var maxTriesAllowed = 0;
                    // Pre and pos tests are allowed only once.
                    if (athenasQuiz.IsExam == "2" || athenasQuiz.IsExam == "3")
                    {
                        maxTriesAllowed = 1;
                    }
                    // Exams are allowed as max tries for the lesson. 
                    else if (athenasQuiz.IsExam == "1" && lesson != null && !string.IsNullOrEmpty(lesson.MaxTries))
                    {
                        int.TryParse(lesson.MaxTries, out maxTriesAllowed);
                    }

                    var allowMaxTry = false;
                    var actualPercentage = "";

                    foreach (var quiz in quizzesResultsFound)
                    {
                        if (int.Parse(quiz.ActualTry) < maxTriesAllowed)
                        {
                            allowMaxTry = true;
                            actualPercentage = quiz.Percentage;
                            break;
                        }
                        else
                        {
                            actualPercentage = quiz.Percentage;
                            allowMaxTry = false;
                        }
                    }
                    if (quizzesResultsFound == null || quizzesResultsFound.Count == 0)
                    {
                        allowMaxTry = true;
                    }

                    var dateAvailable = false;

                    // Get the current UTC date (just the date, no time)
                    var currentUtcDate = DateTime.UtcNow.Date;

                    // Check if limit date is still available
                    var lessons = assignedLessonsResults.FindAll(x => x.LessonId == athenasQuiz.LessonId);
                    var maxLimitDate = new DateTime();
                    foreach (var lessonToCheck in lessons)
                    {
                        DateTime lessonLimitDate = DateTime.Parse(lessonToCheck.LimitDate).Date;
                        DateTime originalLimitDate = DateTime.Parse(lessonToCheck.LimitDate).Date;
                        // Extend the lessonLimitDate to account for the latest global timezone (UTC-12)
                        lessonLimitDate = lessonLimitDate.AddHours(23).AddMinutes(59).AddSeconds(59).AddHours(12); // Ensuring full global availability

                        if (currentUtcDate <= lessonLimitDate)
                        {
                            dateAvailable = true;
                            maxLimitDate = originalLimitDate;
                            break;
                        }
                    }

                    // Add the quiz to the list only if there are remaining attempts and the date is available
                    if (allowMaxTry && dateAvailable)
                    {
                        athenasQuizList.Add(new QuizAthenasModel()
                        {
                            Available = true,
                            Id = athenasQuiz.Id,
                            CustomValue = athenasQuiz.CustomValue,
                            Instruction = athenasQuiz.Instruction,
                            IsExam = athenasQuiz.IsExam,
                            LimitDate = maxLimitDate.ToShortDateString(),
                            Title = athenasQuiz.Title,
                            LessonId = athenasQuiz.LessonId,
                            LevelOfKnowledge = athenasQuiz.LevelOfKnowledge,
                            QuizTypeId = athenasQuiz.QuizTypeId,
                            ActualPercentage = actualPercentage
                        });
                    }
                    else
                    {
                        // Add the quiz as unavailable if no attempts are remaining or the date is not valid
                        athenasQuizList.Add(new QuizAthenasModel()
                        {
                            Available = false,
                            Id = athenasQuiz.Id,
                            CustomValue = athenasQuiz.CustomValue,
                            Instruction = athenasQuiz.Instruction,
                            IsExam = athenasQuiz.IsExam,
                            LimitDate = null,
                            Title = athenasQuiz.Title,
                            LessonId = athenasQuiz.LessonId,
                            LevelOfKnowledge = athenasQuiz.LevelOfKnowledge,
                            QuizTypeId = athenasQuiz.QuizTypeId,
                            ActualPercentage = actualPercentage
                        });
                    }
                }
            }
            return athenasQuizList;
        }

        /// <summary>
        /// To create or update the student lesson quiz result.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="quizValue">The value of the quiz.</param>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="usquiQuizResultId">The id of the user quiz result.</param>
        /// <param name="score">The score of the student.</param>
        /// <param name="token">Token.</param>
        /// <param name="learningSheetIds">The learning sheet id list to insert.</param>
        public void CreateOrUpdateStudentLessonQuizResult(string studentId,
            string lessonId,
            string quizValue,
            string quizId,
            string usquiQuizResultId,
            string score,
            string token,
            List<string> learningSheetIds)
        {
            // Get active academic years
            var academicYears = GetStudentActiveAcademicYears(token, studentId);
            if (academicYears == null || !academicYears.Any()) return;

            // Prepare assigned lesson models
            var assignedLessons = new LearningGapAssignedLessonDAO().Read(
                _connectionStringRead,
                academicYears.Select(year => new LearningGapAssignedLessonModel
                {
                    LessonId = lessonId,
                    StudentId = studentId,
                    AcademicYearId = year
                }).ToList()
            );
            if (assignedLessons == null || !assignedLessons.Any()) return;

            // Fetch existing quiz results
            var lessonQuizResults = new LearningGapLessonQuizResultDAO().Read(
                _connectionStringRead,
                assignedLessons.Select(lesson => new LearningGapLessonQuizResultModel
                {
                    LearningGapAssignedLessonId = lesson.Id,
                    QuizId = quizId
                }).ToList()
            );

            double quizMaxScore = double.Parse(quizValue);
            var quizResultToCreate = new List<LearningGapLessonQuizResultModel>();
            var quizResultToUpdate = new List<LearningGapLessonQuizResultModel>();
            var quizResultToUpdateWhere = new List<LearningGapLessonQuizResultModel>();

            foreach (var lesson in assignedLessons)
            {
                var existingResult = lessonQuizResults.FirstOrDefault(x => x.LearningGapAssignedLessonId == lesson.Id);
                double percentage = (double.Parse(score) / quizMaxScore * 100);

                if (existingResult == null)
                {
                    quizResultToCreate.Add(new LearningGapLessonQuizResultModel
                    {
                        LearningGapAssignedLessonId = lesson.Id,
                        QuizId = quizId,
                        Score = score,
                        GswebUsquiResultsId = usquiQuizResultId,
                        ModifiedBy = studentId,
                        Percentage = percentage.ToString("0.##"),
                        ActualTry = "1"
                    });
                }
                else if (int.Parse(existingResult.ActualTry) < int.Parse(lesson.MaxTries))
                {
                    // If the new score is higher, update the existing result
                    if (double.Parse(score) > double.Parse(existingResult.Score))
                    {
                        quizResultToUpdate.Add(new LearningGapLessonQuizResultModel
                        {
                            Id = existingResult.Id,
                            LearningGapAssignedLessonId = lesson.Id,
                            QuizId = quizId,
                            Score = score,
                            GswebUsquiResultsId = usquiQuizResultId,
                            Percentage = percentage.ToString("0.##"),
                            ActualTry = (int.Parse(existingResult.ActualTry) + 1).ToString()
                        });
                        quizResultToUpdateWhere.Add(new LearningGapLessonQuizResultModel { Id = existingResult.Id });
                    }
                    else
                    {
                        // Else, just update the try count, but DO NOT overwrite the score, neither the result's id.
                        quizResultToUpdate.Add(new LearningGapLessonQuizResultModel
                        {
                            LearningGapAssignedLessonId = lesson.Id,
                            ActualTry = (int.Parse(existingResult.ActualTry) + 1).ToString()
                        });
                        quizResultToUpdateWhere.Add(new LearningGapLessonQuizResultModel { Id = existingResult.Id });
                    }
                }
            }

            var quizDAO = new LearningGapLessonQuizResultDAO();
            if (quizResultToCreate.Any()) quizDAO.Create(_connectionStringWrite, quizResultToCreate);
            if (quizResultToUpdate.Any()) quizDAO.Update(_connectionStringWrite, quizResultToUpdate, quizResultToUpdateWhere);

            // Insert the learning sheets if any. 
            var learningGapStudentLearningSheetDAO = new LearningGapStudentLearningSheetDAO();
            var learningSheetLearningSheetList = new List<LearningGapStudentLearningSheetModel>();
            foreach (var academicYear in academicYears)
            {
                foreach (var learningSheet in learningSheetIds)
                {
                    learningSheetLearningSheetList.Add(new LearningGapStudentLearningSheetModel()
                    {
                        StudentId = studentId,
                        AcademicYearId = academicYear,
                        LearningSheetId = learningSheet,
                        QuizId = quizId,
                        Available = "1",
                        ModifiedBy = studentId
                    });
                }
            }

            if (learningSheetLearningSheetList.Any())
            {
                learningGapStudentLearningSheetDAO.Create(_connectionStringWrite, learningSheetLearningSheetList);
            }

            if (quizResultToCreate.Any() || quizResultToUpdate.Any())
            {
                UpdateLessonAsCompleted(studentId, lessonId, token);
            }
        }

        /// <summary>
        /// To read the pending diagnostic tests for the student.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns the pedning diagnostics assignments paginated.</returns>
        public PaginatedResult<LearningGapDiagnosticYearScoreModel> ReadPendingDiagnosticScores(
            string studentId,
            int fromPage, int limiter, string token)
        {
            // Read the academic years of the student.
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new PaginatedResult<LearningGapDiagnosticYearScoreModel>(0, new List<LearningGapDiagnosticYearScoreModel>());
            }

            // Call the DAO to read the diagnostic year scores.
            var studentAcademicDiagnosticYearScores = new List<LearningGapDiagnosticYearScoreModel>();
            foreach (var academicYear in academicYears)
            {
                studentAcademicDiagnosticYearScores.Add(new LearningGapDiagnosticYearScoreModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear,
                });
            }

            var learningGapDiagnostics = new LearningGapDiagnosticYearScoreDAO().Read(this._connectionStringRead,
                studentAcademicDiagnosticYearScores);

            // Remove the ones that has data in the final and initial diagnostic score.
            learningGapDiagnostics = learningGapDiagnostics.FindAll(x => string.IsNullOrEmpty(x.InitialDiagnosticScore) || string.IsNullOrEmpty(x.FinalDiagnosticScore));
            if (learningGapDiagnostics.Count == 0)
            {
                return new PaginatedResult<LearningGapDiagnosticYearScoreModel>(0, new List<LearningGapDiagnosticYearScoreModel>());
            }

            // Call the diagnostic tests. 
            var diagnosticIdsToFind = learningGapDiagnostics.Select(x => x.InitialDiagnosticResultId).ToList();
            diagnosticIdsToFind.AddRange(learningGapDiagnostics.Select(x => x.FinalDiagnosticResultId).ToList());

            var diagnosticTests = AthenasHandler.GetAthenasDiagnosticTest(diagnosticIdsToFind);

            // Call Athenas subjects.
            var athenasSubjects = AthenasHandler.GetAthenasSubjects(_langCode);

            // Attach the subject name to the model.
            foreach (var diagnostic in learningGapDiagnostics)
            {
                diagnostic.SubjectName = athenasSubjects?.Find(x => x.Code.ToLower() == diagnostic.SubjectCode.ToLower())?.Name;
                diagnostic.DiagnosticTitle = diagnosticTests?.Find(x => x.Id == diagnostic.InitialDiagnosticResultId)?.Title;

                if (string.IsNullOrEmpty(diagnostic.DiagnosticTitle))
                {
                    if (_langCode == "es")
                    {
                        diagnostic.DiagnosticTitle = "Diagnóstica " + diagnostic.SubjectName + " " + diagnostic.LevelCode;
                    }
                    else
                    {
                        diagnostic.DiagnosticTitle = "Diagnostic " + diagnostic.SubjectName + " " + diagnostic.LevelCode;
                    }
                }

                // Set to 11:59:59
                diagnostic.FinalDiagnosticDateLimit = DateTime.Parse(diagnostic.FinalDiagnosticDateLimit)
                    .AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");
                diagnostic.InitialDiagnosticDateLimit = DateTime.Parse(diagnostic.InitialDiagnosticDateLimit)
                    .AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");
            }

            // Order by most recent diagnostic date. 

            learningGapDiagnostics = learningGapDiagnostics.OrderByDescending(x => DateTime.Parse(x.InitialDiagnosticDateLimit)).
                ThenByDescending(x => DateTime.Parse(x.FinalDiagnosticDateLimit)).ToList();

            // Prepare the result, calculating the total pages according to the limiter and results.
            var totalPages = learningGapDiagnostics.Count / limiter;
            if (learningGapDiagnostics.Count % limiter > 0)
            {
                totalPages++;
            }

            // Paginate the results.
            var paginatedResults = learningGapDiagnostics.Skip(fromPage * limiter).Take(limiter).ToList();

            return new PaginatedResult<LearningGapDiagnosticYearScoreModel>(totalPages, paginatedResults);
        }

        /// <summary>
        /// To read the diagnostic details.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="token">The user's token.</param>
        /// <param name="removeIsCorrect">True if remove IsCorrect.</param>
        /// <returns>eturns the diagnostic details.</returns>
        public AthenasDiagnosticTestModel ReadDiagnosticDetail(string subjectCode, string levelCode, string token, string studentId, bool removeIsCorrect)
        {
            // Read it from Athenas. 
            var diagnosticDetail = AthenasHandler.GetAthenasDiagnosticDetails(subjectCode, levelCode);

            // Check if available for the student. 
            if (diagnosticDetail == null)
            {
                return new AthenasDiagnosticTestModel();
            }

            // Check if student has active academic years. 
            // Read the academic years of the student.
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                return diagnosticDetail;
            }

            // Call the DAO to read the diagnostic year scores.
            var studentAcademicDiagnosticYearScores = new List<LearningGapDiagnosticYearScoreModel>();
            foreach (var academicYear in academicYears)
            {
                studentAcademicDiagnosticYearScores.Add(new LearningGapDiagnosticYearScoreModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear,
                    SubjectCode = subjectCode,
                    LevelCode = levelCode
                });
            }

            var learningGapDiagnostics = new LearningGapDiagnosticYearScoreDAO().Read(this._connectionStringRead,
                studentAcademicDiagnosticYearScores);

            if (learningGapDiagnostics == null || learningGapDiagnostics.Count == 0)
            {
                return diagnosticDetail;
            }

            // For each result, check if the diagnostic has not been taken and has a available limit date. 
            foreach (var diagnostic in learningGapDiagnostics)
            {
                if (string.IsNullOrWhiteSpace(diagnostic.InitialDiagnosticScore) &&
                    DateTime.Parse(diagnostic.InitialDiagnosticDateLimit).AddHours(23).AddMinutes(59).AddSeconds(59).AddHours(12) >= DateTime.UtcNow)
                {
                    diagnosticDetail.AvailableForStudent = true;
                    return diagnosticDetail;
                }

                if (string.IsNullOrWhiteSpace(diagnostic.FinalDiagnosticScore) &&
                    DateTime.Parse(diagnostic.FinalDiagnosticDateLimit).AddHours(23).AddMinutes(59).AddSeconds(59).AddHours(12) >= DateTime.UtcNow)
                {
                    diagnosticDetail.AvailableForStudent = true;
                    return diagnosticDetail;
                }
            }

            // If removeIsCorrect is true, remove the IsCorrect from the responses.
            if (removeIsCorrect)
            {
                foreach (var diagnosticTest in diagnosticDetail.DiagnosticTests)
                {
                    foreach (var question in diagnosticTest.Questions)
                    {
                        foreach (var response in question.Responses)
                        {
                            response.IsCorrect = 0;
                        }
                    }
                }
            }

            return diagnosticDetail;
        }


        /// <summary>
        /// To submit the diagnostic test by the student.
        /// </summary>
        /// <param name="diagnosticTest">The diagnostic test details.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="token">The user's token.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the response. </returns>
        public ResponseModel SubmitDiagnosticByStudent(DiagnosticTest diagnosticTest, string subjectCode, string levelCode, string token, string studentId)
        {
            // if one of the values is null, return error. 
            if (diagnosticTest == null || string.IsNullOrWhiteSpace(subjectCode) || string.IsNullOrWhiteSpace(levelCode) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(studentId))
            {
                return new ResponseModel()
                {
                    SessionValid = true,
                    TransactionValid = false
                };
            }

            subjectCode = subjectCode.ToLower();
            levelCode = levelCode.ToLower();

            // Call the diagnostics in Athenas. 
            var athenasDiagnosticTests = this.ReadDiagnosticDetail(subjectCode, levelCode, token, studentId, false);

            // If there are no diagnostics, return an error.
            if (athenasDiagnosticTests == null || !athenasDiagnosticTests.AvailableForStudent)
            {
                return new ResponseModel()
                {
                    SessionValid = true,
                    TransactionValid = false
                };
            }

            // Get the academic years of the student.
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new ResponseModel()
                {
                    SessionValid = true,
                    TransactionValid = false
                };
            }

            // Prpare the learning gap test result responses. 
            var diagnosticTestResultsToInsert = new List<LearningGapDiagnosticTestResultResponseModel>();

            // Compare the responses of the student with the diagnostic test.
            var diagnosticTestQuestionResponses = new List<DiagnosticTestQuestionResponse>();
            var totalCorrect = 0;
            foreach (var question in diagnosticTest.Questions)
            {
                var athenasQuestion = athenasDiagnosticTests.DiagnosticTests[0].Questions.FirstOrDefault(x => x.Id == question.Id);
                if (athenasQuestion == null)
                {
                    continue;
                }

                // Find the response of the student.
                var response = question.Responses.FirstOrDefault(x => x.SelectedByStudent);

                // Find the response in Athenas question.
                var athenasResponse = athenasQuestion.Responses.FirstOrDefault(x => x.Id == response.Id);

                // If the response is correct, add to the total correct.
                if (athenasResponse != null && athenasResponse.IsCorrect == 1)
                {
                    totalCorrect++;
                }

                diagnosticTestResultsToInsert.Add(new LearningGapDiagnosticTestResultResponseModel()
                {
                    ModifiedBy = studentId,
                    QuestionId = question.Id.ToString(),
                    ResponseId = response.Id.ToString(),
                    Correct = athenasResponse != null && athenasResponse.IsCorrect == 1 ? "1" : "0"
                });
            }

            // Prepare the diagnostic test results. 
            var learningGapDiagnosticTestResultModel = new LearningGapDiagnosticTestResultModel()
            {
                StudentId = studentId,
                SubjectCode = subjectCode,
                LevelCode = levelCode,
                QuizId = athenasDiagnosticTests.Id,
                TotalCorrect = totalCorrect.ToString(),
                TotalQuestions = athenasDiagnosticTests.DiagnosticTests[0].Questions.Count.ToString(),
                ModifiedBy = studentId,
                TotalPercent = Math.Round((double)totalCorrect / athenasDiagnosticTests.DiagnosticTests[0].Questions.Count * 100, 2).ToString(),
                TotalScore = Math.Round((double)totalCorrect / athenasDiagnosticTests.DiagnosticTests[0].Questions.Count * 100, 2).ToString()
            };

            // Insert the results.
            var learningGapDiagnosticTestResultDAO = new LearningGapDiagnosticTestResultDAO();
            var learningGapDiagnosticTestResultInserted = learningGapDiagnosticTestResultDAO.Create(_connectionStringWrite, learningGapDiagnosticTestResultModel);

            // Assign the inserted id to the responses.
            foreach (var response in diagnosticTestResultsToInsert)
            {
                response.LearningGapDiagnosticTestResultId = learningGapDiagnosticTestResultInserted[0].Id;
            }

            // Insert the test results.
            var learningGapDiagnosticTestResultResponseDAO = new LearningGapDiagnosticTestResultResponseDAO();
            learningGapDiagnosticTestResultResponseDAO.Create(_connectionStringWrite, diagnosticTestResultsToInsert);

            // Create the diagnostic year score foreach academic year. 
            var learningGapDiagnosticYearScoreDAO = new LearningGapDiagnosticYearScoreDAO();

            foreach (var academicYearId in academicYears)
            {
                // Check if exists the diagnostic year score.
                var learningGapDiagnosticYearScoreExists = learningGapDiagnosticYearScoreDAO.Read(_connectionStringRead, new LearningGapDiagnosticYearScoreModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYearId,
                    SubjectCode = subjectCode,
                    LevelCode = levelCode
                }, true, true);

                if (learningGapDiagnosticYearScoreExists != null)
                {
                    // Check if initial is null, if so, update the initial.
                    // If limit date is more than today, adding the time to 11:59:59 PM allow. 
                    var initialDiagnosticDateLimit = DateTime.Parse(learningGapDiagnosticYearScoreExists[0].InitialDiagnosticDateLimit).AddHours(23).AddMinutes(59).AddSeconds(59);
                    if (initialDiagnosticDateLimit > DateTime.Now && string.IsNullOrWhiteSpace(learningGapDiagnosticYearScoreExists[0].InitialDiagnosticScore))
                    {
                        learningGapDiagnosticYearScoreDAO.Update(_connectionStringWrite,
                        new LearningGapDiagnosticYearScoreModel()
                        {
                            InitialDiagnosticScore = learningGapDiagnosticTestResultInserted[0].TotalScore,
                            InitialDiagnosticResultId = learningGapDiagnosticTestResultInserted[0].Id,
                        },
                        new LearningGapDiagnosticYearScoreModel()
                        {
                            Id = learningGapDiagnosticYearScoreExists[0].Id
                        });
                    }
                    else if (DateTime.Parse(learningGapDiagnosticYearScoreExists[0].FinalDiagnosticDateLimit).AddHours(23).AddMinutes(59).AddSeconds(59) > DateTime.Now && string.IsNullOrWhiteSpace(learningGapDiagnosticYearScoreExists[0].FinalDiagnosticScore))
                    {

                        // If limit date is more than today, adding the time to 11:59:59 PM allow. 
                        learningGapDiagnosticYearScoreDAO.Update(_connectionStringWrite,
                             new LearningGapDiagnosticYearScoreModel()
                             {
                                 FinalDiagnosticScore = learningGapDiagnosticTestResultInserted[0].TotalScore,
                                 FinalDiagnosticResultId = learningGapDiagnosticTestResultInserted[0].Id,
                             },
                             new LearningGapDiagnosticYearScoreModel()
                             {
                                 Id = learningGapDiagnosticYearScoreExists[0].Id
                             });
                    }
                }
            }

            return new ResponseModel()
            {
                SessionValid = true,
                TransactionValid = true
            };
        }

        #endregion student

        #region teacher
        /// <summary>
        /// To read the learning gap lessons by level code.
        /// </summary>
        /// <param name="levelCode">The level code of the lessons to call.</param>
        /// <returns>Returns the lesson model list.</returns>
        public List<LessonModel> ReadLearningGapLessons(string levelCode)
        {
            // Read the lessons.
            var lessonModelsList = new List<LessonModel>()
            {
                new LessonModel()
                {
                    LevelCode = levelCode
                }
            };

            var lessons = FeignAPI.AthenasHandler.GetAthenasLessons(lessonModelsList, true);
            return lessons.LessonModelList;
        }

        /// <summary>
        /// To read the lesson status of the student.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="token">The connected user's token.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the assignled lessons with the status.</returns>
        public List<LearningGapAssignedLessonModel> ReadStudentLessonStatus(string studentId, string token, bool assignedByTeacherOnly, string teacherId)
        {
            // Check the active academic years for the student.
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                // No results.
                return new List<LearningGapAssignedLessonModel>();
            }

            // Prepare the read of every active academic year. 
            var lessonsToRead = new List<LearningGapAssignedLessonModel>();
            foreach (var academicYear in academicYears)
            {
                var lesson = new LearningGapAssignedLessonModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear,
                };

                if (assignedByTeacherOnly)
                {
                    lesson.ModifiedBy = teacherId;
                }

                lessonsToRead.Add(lesson);
            }

            // Read the lessons assigned to the student.
            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();
            var lessons = learningGapAssignedLessonDAO.Read(
                _connectionStringRead, lessonsToRead);

            // If there are no lessons, return an empty list.
            if (lessons == null || lessons.Count == 0)
            {
                return new List<LearningGapAssignedLessonModel>();
            }

            var athenasLessonsToRead = new List<LessonModel>();
            foreach (var lesson in lessons)
            {
                athenasLessonsToRead.Add(new LessonModel()
                {
                    Id = lesson.LessonId
                });
            }

            // In Athenas, read the lessons to get the subject and level name and lesson title.
            var athenasLessons = FeignAPI.AthenasHandler.GetAthenasLessons(athenasLessonsToRead, true);

            var athenasSubjects = FeignAPI.AthenasHandler.GetAthenasSubjects(_langCode);

            // Merge the results.
            foreach (var lesson in lessons)
            {
                var athenasLesson = athenasLessons.LessonModelList.FirstOrDefault(x => x.Id == lesson.LessonId);

                // Find the subject. 
                var athenasSubject = athenasSubjects.FirstOrDefault(x => x.Code.ToLower() == athenasLesson.SubjectCode.ToLower());
                if (athenasLesson != null)
                {
                    lesson.SubjectName = athenasSubject.Name;
                    lesson.LevelName = athenasLesson.LevelCode;
                    lesson.LessonTitle = athenasLesson.LessonTitle;
                }
            }

            return lessons;
        }

        /// <summary>
        /// To read the student lesson quiz results.
        /// </summary>
        /// <param name="assingedLessonId">The assigned lesson id.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <returns>Returns the list of quizzes.</returns>
        public List<LearningGapLessonQuizResultModel> ReadStudentLessonQuizResultList(string assingedLessonId, bool assignedByTeacherOnly, string teacherId)
        {

            // First, retrieve the lesson to call the quizzes of the lesson. 
            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();

            var learningGapAssignedLesson = new LearningGapAssignedLessonModel()
            {
                Id = assingedLessonId,
            };

            // Check if the lesson is assigned by the teacher.
            if (assignedByTeacherOnly)
            {
                learningGapAssignedLesson.ModifiedBy = teacherId;
            }

            var assignedLesson = learningGapAssignedLessonDAO.Read(_connectionStringRead,
                learningGapAssignedLesson, false, false);

            // If the lesson was not found, return an empty list.
            if (assignedLesson == null || assignedLesson.Count == 0)
            {
                return new List<LearningGapLessonQuizResultModel>();
            }

            // Now call from Athenas the lesson. 
            var athenasLessonQuizzes = FeignAPI.AthenasHandler.GetLessonQuizzes(assignedLesson[0].LessonId);

            // If there are no quizzes, return an empty list.
            if (athenasLessonQuizzes == null || athenasLessonQuizzes.Quizzes == null || athenasLessonQuizzes.Quizzes.Count == 0)
            {
                return new List<LearningGapLessonQuizResultModel>();
            }

            // Read the quiz results.
            var learningGapLessonQuizResultDAO = new LearningGapLessonQuizResultDAO();
            var quizResults = learningGapLessonQuizResultDAO.Read(_connectionStringRead, new LearningGapLessonQuizResultModel()
            {
                LearningGapAssignedLessonId = assingedLessonId
            }, false, false);

            var lessonGapLessonQuizToResult = new List<LearningGapLessonQuizResultModel>();

            foreach (var quiz in athenasLessonQuizzes.Quizzes)
            {
                if (quiz.IsExam == "0")
                {
                    continue;
                }
                // Check if the quiz result exists.
                var quizResult = quizResults.FirstOrDefault(x => x.QuizId == quiz.Id);
                if (quizResult == null)
                {
                    lessonGapLessonQuizToResult.Add(new LearningGapLessonQuizResultModel()
                    {
                        LearningGapAssignedLessonId = assingedLessonId,
                        QuizId = quiz.Id,
                        Score = "0",
                        Percentage = "0",
                        ActualTry = "0",
                        ModifiedBy = "0",
                        QuizTitle = quiz.Title,
                    });
                }
                else
                {
                    lessonGapLessonQuizToResult.Add(new LearningGapLessonQuizResultModel()
                    {
                        LearningGapAssignedLessonId = assingedLessonId,
                        QuizId = quiz.Id,
                        QuizTitle = quiz.Title,
                        CreatedAt = quiz.CreatedAt,
                        ActualTry = quizResult.ActualTry,
                        Percentage = quizResult.Percentage,
                        GswebUsquiResultsId = quizResult.GswebUsquiResultsId,
                        Score = quizResult.Score,
                        Id = quizResult.Id
                    });
                }
            }

            return lessonGapLessonQuizToResult;
        }

        /// <summary>
        /// To retrieve the student annotations.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="token">The token of the user making the request.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the paginated results. </returns>
        public PaginatedResult<LearningGapAnnotationModel> ReadStudentAnnotations(string studentId, int fromPage, int limiter, string token, bool assignedByTeacherOnly, string teacherId)
        {
            // First, read the current academic years. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                // No results.
                return new PaginatedResult<LearningGapAnnotationModel>(0, new List<LearningGapAnnotationModel>());
            }

            // Now retrieve the student annotations.
            var annotationsToFind = new List<LearningGapAnnotationModel>();
            foreach (var academicYear in academicYears)
            {
                var annotation = new LearningGapAnnotationModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear
                };

                if (assignedByTeacherOnly)
                {
                    annotation.ModifiedBy = teacherId;
                }

                annotationsToFind.Add(annotation);
            }

            var learningGapAnnotationDAO = new LearningGapAnnotationDAO();
            var annotations = learningGapAnnotationDAO.Read(_connectionStringRead, annotationsToFind);

            // Stop the process if there are no annotations.
            if (annotations == null || annotations.Count == 0)
            {
                return new PaginatedResult<LearningGapAnnotationModel>(0, new List<LearningGapAnnotationModel>());
            }

            // Order by date
            annotations = annotations.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).ToList();

            // Prepare the result, calculating the total pages according to the limiter and results.
            var totalPages = annotations.Count / limiter;
            if (annotations.Count % limiter > 0)
            {
                totalPages++;
            }

            // Paginate the results.
            var paginatedResults = annotations.Skip(fromPage * limiter).Take(limiter).ToList();

            return new PaginatedResult<LearningGapAnnotationModel>(totalPages, paginatedResults);

        }

        /// <summary>
        /// To retrieve the learning sheets of the student.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="token">The token of the user making the request.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the paginated results.</returns>
        public PaginatedResult<LearningSheetModel> ReadStudentLearningSheets(string studentId, int fromPage, int limiter, string token, bool assignedByTeacherOnly, string teacherId)
        {
            // First, read the current academic years. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                // No results.
                return new PaginatedResult<LearningSheetModel>(0, new List<LearningSheetModel>());
            }

            // Prepare the academic years to read the learning sheets.
            var learningSheetToFindList = new List<LearningGapStudentLearningSheetModel>();
            foreach (var academicYear in academicYears)
            {
                learningSheetToFindList.Add(new LearningGapStudentLearningSheetModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear
                });
            }

            // Read the learning sheets.
            var learningGapStudentLearningSheetDAO = new LearningGapStudentLearningSheetDAO();
            var learningSheets = learningGapStudentLearningSheetDAO.Read(_connectionStringRead, learningSheetToFindList);

            if (learningSheets == null || learningSheets.Count == 0)
            {
                return new PaginatedResult<LearningSheetModel>(0, new List<LearningSheetModel>());
            }

            if (assignedByTeacherOnly)
            {
                // Get the quiz ids to read the learning sheets.
                var quizIdsToRead = learningSheets.Select(x => x.QuizId).Distinct().ToList();

                var learningGapLessonQuizResultDAO = new LearningGapLessonQuizResultDAO();
                // Read the quiz results to identify the learning gap assigned lesson id. 
                var learningGapLessonQuizResultsToFind = new List<LearningGapLessonQuizResultModel>();
                foreach (var quizId in quizIdsToRead)
                {
                    learningGapLessonQuizResultsToFind.Add(new LearningGapLessonQuizResultModel()
                    {
                        QuizId = quizId
                    });
                }

                var learningGapLessonQuizResults = learningGapLessonQuizResultDAO.Read(_connectionStringRead, learningGapLessonQuizResultsToFind);

                if (learningGapLessonQuizResults == null || learningGapLessonQuizResults.Count == 0)
                {
                    return new PaginatedResult<LearningSheetModel>(0, new List<LearningSheetModel>());
                }

                // Now identify if the teacher has assigned the learning gap lesson quiz by reading the learning_gap_assigned_lessons
                var learningGapAssignedLessonToFind = new List<LearningGapAssignedLessonModel>();
                foreach (var learningGapLessonQuizResult in learningGapLessonQuizResults)
                {
                    var learningGapAssignedLesson = new LearningGapAssignedLessonModel()
                    {
                        Id = learningGapLessonQuizResult.LearningGapAssignedLessonId,
                        ModifiedBy = teacherId
                    };

                    learningGapAssignedLessonToFind.Add(learningGapAssignedLesson);
                }

                var learningGapAssignedLessons = new LearningGapAssignedLessonDAO().Read(_connectionStringRead, learningGapAssignedLessonToFind);
                if (learningGapAssignedLessons == null || learningGapAssignedLessons.Count == 0)
                {
                    return new PaginatedResult<LearningSheetModel>(0, new List<LearningSheetModel>());
                }

                // Get the IDs of the assigned lessons that were actually assigned by the teacher.
                var assignedLessonIdsByTeacher = new HashSet<string>(
                    learningGapAssignedLessons
                        .Where(x => x.ModifiedBy == teacherId)
                        .Select(x => x.Id)
                );

                // Filter the quiz results to keep only those assigned by the teacher.
                var filteredQuizResults = learningGapLessonQuizResults
                    .Where(x => assignedLessonIdsByTeacher.Contains(x.LearningGapAssignedLessonId))
                    .ToList();

                // Get the quiz IDs from the filtered results
                var filteredQuizIds = filteredQuizResults
                    .Select(x => x.QuizId)
                    .Distinct()
                    .ToHashSet();

                // Finally, filter the learning sheets using the filtered quiz IDs
                learningSheets = learningSheets
                    .Where(sheet => filteredQuizIds.Contains(sheet.QuizId))
                    .ToList();

                // If there are no learning sheets, return an empty list.
                if (learningSheets == null || learningSheets.Count == 0)
                {
                    return new PaginatedResult<LearningSheetModel>(0, new List<LearningSheetModel>());
                }
            }


            // Order by last created date.
            learningSheets = learningSheets.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).ToList();

            // Filter by available == 1 (only available learning sheets).
            learningSheets = learningSheets.FindAll(x => x.Available == "1");

            // Prepare the result, calculating the total pages according to the limiter and results.
            var totalPages = learningSheets.Count / limiter;
            if (learningSheets.Count % limiter > 0)
            {
                totalPages++;
            }

            // Paginate the results.
            var paginatedResults = learningSheets.Skip(fromPage * limiter).Take(limiter).ToList();

            // Call the learning sheets in Athenas.
            var learningSheetsIds = new List<string>();
            foreach (var learningSheet in paginatedResults)
            {
                learningSheetsIds.Add(learningSheet.LearningSheetId);
            }

            // Read the learning sheets.
            var athenasLearningSheets = FeignAPI.AthenasHandler.GetLearningSheetByList(learningSheetsIds);

            var learningSheetsToReturn = new List<LearningSheetModel>();
            foreach (var learningSheetStudent in paginatedResults)
            {
                var athenasLearningSheet = athenasLearningSheets.FirstOrDefault(x => x.Id
                == learningSheetStudent.LearningSheetId);
                if (athenasLearningSheet != null)
                {
                    learningSheetsToReturn.Add(new LearningSheetModel()
                    {
                        Id = learningSheetStudent.LearningSheetId,
                        KeyWord = athenasLearningSheet.KeyWord,
                        TextDescription = athenasLearningSheet.TextDescription,
                        TextImportance = athenasLearningSheet.TextImportance,
                        CreatedAt = learningSheetStudent.CreatedAt,
                        ModifiedBy = learningSheetStudent.ModifiedBy,
                        LessonTitle = athenasLearningSheet.LessonTitle
                    });
                }
                else
                {
                    // Add it as not found.
                    learningSheetsToReturn.Add(new LearningSheetModel()
                    {
                        Id = learningSheetStudent.LearningSheetId,
                        KeyWord = null,
                        TextDescription = null,
                        TextImportance = null,
                        CreatedAt = learningSheetStudent.CreatedAt,
                        ModifiedBy = learningSheetStudent.ModifiedBy,
                        LessonTitle = null
                    });
                }
            }

            return new PaginatedResult<LearningSheetModel>(totalPages, learningSheetsToReturn);
        }

        /// <summary>
        /// To read diagnostic details for the teacher.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <returns>Returns the diagnostic details.</returns>
        public AthenasDiagnosticTestModel ReadDiagnosticDetailTeacher(string subjectCode, string levelCode)
        {
            // Read it from Athenas. 
            var diagnosticDetail = AthenasHandler.GetAthenasDiagnosticDetails(subjectCode, levelCode);

            // Check if available for the student. 
            if (diagnosticDetail == null)
            {
                return new AthenasDiagnosticTestModel();
            }

            return diagnosticDetail;
        }

        /// <summary>
        /// To print the learning sheet.
        /// </summary>
        /// <param name="learningSheetId">The learning sheet id.</param>
        /// <returns>Returns the url where the learning sheet is.</returns>
        public async Task<string> PrintLearningSheet(string learningSheetId)
        {
            if (string.IsNullOrWhiteSpace(learningSheetId))
            {
                return null;
            }

            // Read the learning sheets.
            var athenasLearningSheets = FeignAPI.AthenasHandler.GetLearningSheetByList(new List<string>() { learningSheetId });
            if (athenasLearningSheets == null || athenasLearningSheets.Count == 0)
            {
                return null;
            }

            return await PdfGenerator.GenerateLearningSheetPDF(_langCode, athenasLearningSheets.FirstOrDefault());
        }

        /// <summary>
        /// To retrieve the diagnostic test results and the academic year initial and final grade.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="token">The token of the user making the request.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the list of academic year score for the student.</returns>
        public List<LearningGapAcademicYearScoreModel> ReadStudentAcademicYearScoreModel(
            string studentId, string token, bool assignedByTeacherOnly, string teacherId)
        {
            // Get the active academic years. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);
            // To remove the scores that the teacher cannot handle.
            var yearsScoresToRemove = new List<LearningGapAcademicYearScoreModel>();

            // If empty, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new List<LearningGapAcademicYearScoreModel>();
            }

            // Prepare the read of the academic year scores.
            var academicYearScoresToRead = new List<LearningGapAcademicYearScoreModel>();
            foreach (var academicYear in academicYears)
            {
                academicYearScoresToRead.Add(new LearningGapAcademicYearScoreModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear
                });
            }

            var teacherSubjects = new List<string>();
            if (assignedByTeacherOnly)
            {
                teacherSubjects = this.GetTeacherAllowedSubjects(token);
            }

            // Read the academic year scores.
            var learningGapAcademicYearScoreDAO = new LearningGapAcademicYearScoreDAO();
            var academicYearScores = learningGapAcademicYearScoreDAO.Read(_connectionStringRead, academicYearScoresToRead);

            if (academicYearScores == null || academicYearScores.Count == 0)
            {
                return new List<LearningGapAcademicYearScoreModel>();
            }

            // Read the subjects in Athenas. 
            var athenasSubjects = AthenasHandler.GetAthenasSubjects(_langCode);

            // Get the student actual score.
            var studentActualScore = this.GetOverallScore(studentId, token);

            // Relate subjects with the scores. 
            foreach (var academicYear in academicYearScores)
            {
                // Find the subject 
                var subject = athenasSubjects.Find(x => x.Code.ToLower() == academicYear.SubjectCode.ToLower());
                if (subject != null)
                {
                    academicYear.SubjectName = subject.Name;
                }

                // If assignedByTeacherOnly is true, check if the subject is allowed for the teacher.
                if (assignedByTeacherOnly && !string.IsNullOrWhiteSpace(academicYear.SubjectCode))
                {
                    var allowedSubject = teacherSubjects.Find(x => x.ToLower() == academicYear.SubjectCode.ToLower());
                    if (allowedSubject == null)
                    {
                        // If the subject is not allowed, remove it from the list.
                        yearsScoresToRemove.Add(academicYear);
                    }
                }

                // Find the student actual score by subject.
                var studentScore = studentActualScore.Find(x => x.SubjectCode.ToLower() == academicYear.SubjectCode.ToLower());
                if (studentScore != null)
                {
                    academicYear.ActualScore = studentScore.Score;
                }
            }

            // Remove the scores that the teacher cannot handle.
            if (yearsScoresToRemove != null && yearsScoresToRemove.Count > 0)
            {
                foreach (var score in yearsScoresToRemove)
                {
                    academicYearScores.Remove(score);
                }
            }

            return academicYearScores;
        }

        /// <summary>
        /// To create or update a grade score manually.
        /// </summary>
        /// <param name="model">The model to update.</param>
        /// <param name="token">The user token to retrieve the student id.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the learning gap academic model created/updated.</returns>
        public LearningGapAcademicYearScoreModel CreateOrUpdateGradeScoreManually(LearningGapAcademicYearScoreModel model, string token, bool assignedByTeacherOnly, string teacherId)
        {
            // Read the active academic years for the student. 
            var academicYears = this.GetStudentActiveAcademicYears(token, model.StudentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                model.Valid = false;
                model.Messages = new List<string>()
                {
                    new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_no_active_academic_years"])
                };
                return model;
            }

            // Check if the teacher is allowed to update the grade score.
            if (assignedByTeacherOnly)
            {
                var teacherSubjects = this.GetTeacherAllowedSubjects(token);
                var allowedSubject = teacherSubjects.Find(x => x.ToLower() == model.SubjectCode.ToLower());
                if (allowedSubject == null)
                {
                    model.Valid = false;
                    model.Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_teacher_not_allowed"])
                    };
                    return model;
                }
            }


            var learningGapAcademicYearScoreDAO = new LearningGapAcademicYearScoreDAO();

            // for each academic year, check if the student has a grade score for the subject and level.
            // NOTE: Since it is not common to the user have more than one academic year active, this can be done 
            // if a loop (the reads). However, if the user has several academic years active,
            // could be necessary to change the logic to avoid multiple reads.
            foreach (var academicYear in academicYears)
            {
                model.AcademicYearId = academicYear;
                // Validations
                var validatedModel = _learningGapValidations.ValidateGapAcademicYearScore(model);
                if (!validatedModel.Valid)
                {
                    return validatedModel;
                }

                var learningGapAcademicYearScore = learningGapAcademicYearScoreDAO.Read(_connectionStringRead, new LearningGapAcademicYearScoreModel()
                {
                    StudentId = validatedModel.StudentId,
                    AcademicYearId = academicYear,
                    SubjectCode = validatedModel.SubjectCode,
                }, true, true);

                // If exists, update
                if (learningGapAcademicYearScore != null && learningGapAcademicYearScore.Count > 0)
                {
                    learningGapAcademicYearScoreDAO.Update(_connectionStringWrite, new LearningGapAcademicYearScoreModel()
                    {
                        InitialGradeScore = validatedModel.InitialGradeScore,
                        FinalGradeScore = validatedModel.FinalGradeScore,
                        ModifiedBy = validatedModel.ModifiedBy,
                    }, new LearningGapAcademicYearScoreModel()
                    {
                        Id = learningGapAcademicYearScore[0].Id,
                    });
                    return validatedModel;
                }

                // Else, create
                learningGapAcademicYearScoreDAO.Create(_connectionStringWrite, new LearningGapAcademicYearScoreModel()
                {
                    StudentId = validatedModel.StudentId,
                    AcademicYearId = academicYear,
                    SubjectCode = validatedModel.SubjectCode,
                    InitialGradeScore = validatedModel.InitialGradeScore,
                    FinalGradeScore = validatedModel.FinalGradeScore,
                    ModifiedBy = validatedModel.ModifiedBy,
                });
            }

            model.Valid = true;

            return model;
        }

        /// <summary>
        /// To update the diagnostic score manually.
        /// </summary>
        /// <param name="learningGapDiagnosticYearScoreModel">The diagnostic model to update.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="token">Token to make external requests.</param>
        /// <returns>Returns the updated diagnostic sco.</returns>
        public LearningGapDiagnosticYearScoreModel UpdateDiagnosticScoreManually(LearningGapDiagnosticYearScoreModel learningGapDiagnosticYearScoreModel, bool assignedByTeacherOnly, string teacherId, string token)
        {
            // Validate the model.
            var validatedModel = _learningGapValidations.ValidateManualDiagnosticScore(learningGapDiagnosticYearScoreModel);

            if (!validatedModel.Valid)
            {
                return validatedModel;
            }

            var learningGapDiagnosticYearScoreDAO = new LearningGapDiagnosticYearScoreDAO();

            // Check if the teacher is allowed to update the grade score.
            if (assignedByTeacherOnly)
            {
                // Get the subject code of the diagnostic score.
                if (string.IsNullOrWhiteSpace(validatedModel.SubjectCode))
                {
                    validatedModel.SubjectCode = learningGapDiagnosticYearScoreDAO.Read(_connectionStringRead, new LearningGapDiagnosticYearScoreModel()
                    {
                        Id = validatedModel.Id
                    }, true, true)[0]?.SubjectCode;
                }

                var teacherSubjects = this.GetTeacherAllowedSubjects(token);
                var allowedSubject = teacherSubjects.Find(x => x.ToLower() == validatedModel.SubjectCode.ToLower());
                if (allowedSubject == null)
                {
                    validatedModel.Valid = false;
                    validatedModel.Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_teacher_not_allowed"])
                    };
                    return validatedModel;
                }
            }


            // Update the diagnostic score.

            learningGapDiagnosticYearScoreDAO.Update(_connectionStringWrite, new LearningGapDiagnosticYearScoreModel()
            {
                InitialDiagnosticScore = validatedModel.InitialDiagnosticScore,
                FinalDiagnosticScore = validatedModel.FinalDiagnosticScore,
                ModifiedBy = validatedModel.ModifiedBy
            }, new LearningGapDiagnosticYearScoreModel()
            {
                Id = validatedModel.Id
            });

            return validatedModel;
        }

        /// <summary>
        /// To create or update the diagnostic score when the user finishes the diagnostic.
        /// </summary>
        /// <param name="studentId">The student id that is making the quiz.</param>
        /// <param name="learningGapDiagnosticTestResult">The learning gat diagnostic test result.</param>
        /// <param name="learningGapDiagnosticTestResultResponseModels">The responses</param>
        /// <param name="token">The user token to make external requests.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the learning gap academic year scores.</returns>
        public LearningGapDiagnosticYearScoreModel CreateOrUpdateDiagnosticScore(
            string studentId,
            LearningGapDiagnosticTestResultModel learningGapDiagnosticTestResult,
            List<LearningGapDiagnosticTestResultResponseModel> learningGapDiagnosticTestResultResponseModels,
            string token,
             bool assignedByTeacherOnly, string teacherId)
        {
            // Read the active academic years for the student. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new LearningGapDiagnosticYearScoreModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_no_active_academic_years"])
                    }
                };
            }

            // Check if the teacher is allowed to update the grade score.
            if (assignedByTeacherOnly)
            {
                var teacherSubjects = this.GetTeacherAllowedSubjects(token);
                var allowedSubject = teacherSubjects.Find(x => x.ToLower() == learningGapDiagnosticTestResult.SubjectCode.ToLower());
                if (allowedSubject == null)
                {
                    return new LearningGapDiagnosticYearScoreModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_teacher_not_allowed"])
                        }
                    };
                }
            }

            // Also, check the subject and level code of the diagnostic test. 
            var athenasDiagnosticTest = FeignAPI.AthenasHandler.GetAthenasDiagnosticTest(new List<string> { learningGapDiagnosticTestResult.QuizId })[0];

            // Read the learning gap academic year scores. 
            var learningGapDiagnosticYearScoreDAO = new LearningGapDiagnosticYearScoreDAO();

            // Foreach academic year, check if the student has a grade score for the subject and level.
            var learningGapAcademicYearToCreate = new List<LearningGapDiagnosticYearScoreModel>();
            var learningGapAcademicYearToUpdateDiagnosticFinalScore = new List<LearningGapDiagnosticYearScoreModel>();
            var learningGapAcademicYearToUpdateDiagnosticFinalScoreWhere = new List<LearningGapDiagnosticYearScoreModel>(); ;
            var learningGapAcademicYearToUpdateDiagnosticInitialScore = new List<LearningGapDiagnosticYearScoreModel>();
            var learningGapAcademicYearToUpdateDiagnosticInitialScoreWhere = new List<LearningGapDiagnosticYearScoreModel>();

            foreach (var academicYear in academicYears)
            {
                var learningGapAcademicYearScores = learningGapDiagnosticYearScoreDAO.Read(_connectionStringRead, new LearningGapDiagnosticYearScoreModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear,
                    SubjectCode = athenasDiagnosticTest.SubjectCode,
                    LevelCode = athenasDiagnosticTest.LevelCode
                }, true, true);

                // If not found, prepare the diagnostic test restuls and responses. 
                if (learningGapAcademicYearScores == null || learningGapAcademicYearScores.Count == 0)
                {
                    learningGapAcademicYearToCreate.Add(new LearningGapDiagnosticYearScoreModel()
                    {
                        StudentId = learningGapDiagnosticTestResult.StudentId,
                        AcademicYearId = academicYear,
                        SubjectCode = athenasDiagnosticTest.SubjectCode,
                        LevelCode = athenasDiagnosticTest.LevelCode,
                        ModifiedBy = learningGapDiagnosticTestResult.ModifiedBy
                    });
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(learningGapAcademicYearScores[0].InitialDiagnosticScore))
                    {
                        learningGapAcademicYearToUpdateDiagnosticInitialScore.Add(new
                            LearningGapDiagnosticYearScoreModel()
                        {
                            Id = learningGapAcademicYearScores[0].Id,
                            AcademicYearId = learningGapAcademicYearScores[0].AcademicYearId,
                            ModifiedBy = learningGapDiagnosticTestResult.ModifiedBy
                        });
                    }
                    else if (string.IsNullOrWhiteSpace(learningGapAcademicYearScores[0].FinalDiagnosticScore))
                    {
                        learningGapAcademicYearToUpdateDiagnosticFinalScore.Add(
                            new LearningGapDiagnosticYearScoreModel()
                            {
                                Id = learningGapAcademicYearScores[0].Id,
                                AcademicYearId = learningGapAcademicYearScores[0].AcademicYearId,
                                ModifiedBy = learningGapDiagnosticTestResult.ModifiedBy
                            });
                    }

                    // Else, ignore, because it has both required scores.   
                }
            }


            if (learningGapAcademicYearToCreate.Count > 0 ||
                learningGapAcademicYearToUpdateDiagnosticFinalScore.Count > 0 ||
                learningGapAcademicYearToUpdateDiagnosticInitialScore.Count > 0)
            {
                // Create the diagnostic test results and responses.
                var learningGapDiagnosticTestResultDAO = new LearningGapDiagnosticTestResultDAO();
                var learningGapDiagnosticTestResponseDAO = new LearningGapDiagnosticTestResultResponseDAO();

                // Validate...
                var learningGapDiagnosticTestResultValidated = _learningGapValidations.ValidateLearningGapDiagnosticTestResultModel(learningGapDiagnosticTestResult);
                if (!learningGapDiagnosticTestResultValidated.Valid)
                {
                    return new LearningGapDiagnosticYearScoreModel()
                    {
                        Valid = false,
                        Messages = new List<string>() {
                            learningGapDiagnosticTestResultValidated.Messages[0]
                        }
                    };
                }

                // Insert the diagnostic test result.
                learningGapDiagnosticTestResult = learningGapDiagnosticTestResultDAO.Create(_connectionStringWrite, learningGapDiagnosticTestResult)[0];

                // Insert the diagnostic test responses.
                // Prepare the responses.
                foreach (var response in learningGapDiagnosticTestResultResponseModels)
                {
                    response.ModifiedBy = learningGapDiagnosticTestResult.ModifiedBy;
                    response.LearningGapDiagnosticTestResultId = learningGapDiagnosticTestResult.Id;
                }

                // Create them
                learningGapDiagnosticTestResultResponseModels = learningGapDiagnosticTestResponseDAO.Create(
                    _connectionStringRead, learningGapDiagnosticTestResultResponseModels);


                if (learningGapAcademicYearToCreate.Count > 0)
                {
                    foreach (var academicYear in academicYears)
                    {
                        // Set the diagnostic test result id to create.
                        foreach (var learningGapToCreate in learningGapAcademicYearToCreate)
                        {
                            learningGapToCreate.InitialDiagnosticResultId = learningGapDiagnosticTestResult.Id;
                            learningGapToCreate.StudentId = learningGapDiagnosticTestResult.StudentId;
                            learningGapToCreate.SubjectCode = athenasDiagnosticTest.SubjectCode;
                            learningGapToCreate.LevelCode = athenasDiagnosticTest.LevelCode;
                            learningGapToCreate.ModifiedBy = learningGapDiagnosticTestResult.ModifiedBy;
                            learningGapToCreate.AcademicYearId = academicYear;
                            learningGapToCreate.InitialDiagnosticScore = learningGapDiagnosticTestResult.TotalScore;
                            learningGapToCreate.FinalDiagnosticScore = null;
                        }

                        // Set the diagnostic test result id to update the initial diagnostic score.
                        foreach (var learningGapToUpdate in learningGapAcademicYearToUpdateDiagnosticInitialScore)
                        {
                            // Find by academic year.
                            if (learningGapToUpdate.AcademicYearId == academicYear)
                            {
                                learningGapToUpdate.InitialDiagnosticResultId = learningGapDiagnosticTestResult.Id;
                                learningGapToUpdate.InitialDiagnosticScore = learningGapDiagnosticTestResult.TotalScore;
                                learningGapAcademicYearToUpdateDiagnosticFinalScoreWhere.Add(new LearningGapDiagnosticYearScoreModel()
                                {
                                    Id = learningGapToUpdate.Id
                                });
                            }
                        }

                        // Same for the final score.
                        foreach (var learningGapToUpdate in learningGapAcademicYearToUpdateDiagnosticFinalScore)
                        {
                            // Find by academic year.
                            if (learningGapToUpdate.AcademicYearId == academicYear)
                            {
                                learningGapToUpdate.FinalDiagnosticResultId = learningGapDiagnosticTestResult.Id;
                                learningGapToUpdate.FinalDiagnosticScore = learningGapDiagnosticTestResult.TotalScore;
                                learningGapAcademicYearToUpdateDiagnosticFinalScoreWhere.Add(new LearningGapDiagnosticYearScoreModel()
                                {
                                    Id = learningGapToUpdate.Id
                                });
                            }
                        }

                    }

                    if (learningGapAcademicYearToCreate.Count > 0)
                    {
                        learningGapDiagnosticYearScoreDAO.Create(_connectionStringWrite, learningGapAcademicYearToCreate);
                    }

                    if (learningGapAcademicYearToUpdateDiagnosticInitialScore.Count > 0)
                    {
                        learningGapDiagnosticYearScoreDAO.Update(_connectionStringWrite, learningGapAcademicYearToUpdateDiagnosticInitialScore, learningGapAcademicYearToUpdateDiagnosticInitialScoreWhere);
                    }

                    if (learningGapAcademicYearToUpdateDiagnosticFinalScore.Count > 0)
                    {
                        learningGapDiagnosticYearScoreDAO.Update(_connectionStringWrite, learningGapAcademicYearToUpdateDiagnosticFinalScore, learningGapAcademicYearToUpdateDiagnosticFinalScoreWhere);
                    }
                }
            }

            return new LearningGapDiagnosticYearScoreModel()
            {
                Valid = true
            };
        }

        /// <summary>
        /// To create annotation model.
        /// </summary>
        /// <param name="model">The annotation model to create.</param>
        /// <param name="token">Token of the user making the request.</param>
        /// <returns>Returns the created annotation model if success with property valid as true.</returns>
        public LearningGapAnnotationModel CreateAnnotationModel(LearningGapAnnotationModel model, string token)
        {
            try
            {
                // Get the academic years of the student.
                var academicYears = this.GetStudentActiveAcademicYears(token, model.StudentId);

                // If there are no active academic years, return an error.
                if (academicYears == null || academicYears.Count == 0)
                {
                    return new LearningGapAnnotationModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_no_active_academic_years"])
                    }
                    };
                }

                var annotationsToInsertPerAcademicYears = new List<LearningGapAnnotationModel>();
                foreach (var academicYear in academicYears)
                {

                    var annotation = new LearningGapAnnotationModel()
                    {
                        StudentId = model.StudentId,
                        TeacherId = model.TeacherId,
                        Text = model.Text,
                        AcademicYearId = academicYear,
                        ModifiedBy = model.ModifiedBy,
                    };

                    // Validate
                    annotation = _learningGapValidations.ValidateAnnotation(annotation);
                    if (!annotation.Valid)
                    {
                        return annotation;
                    }

                    annotationsToInsertPerAcademicYears.Add(annotation);
                }

                if (annotationsToInsertPerAcademicYears != null && annotationsToInsertPerAcademicYears.Count > 0)
                {
                    new LearningGapAnnotationDAO().Create(_connectionStringWrite, annotationsToInsertPerAcademicYears);
                    model.Valid = true;
                    return model;
                }
                model.Valid = true;
                return model;
            }
            catch (Exception)
            {
                model.Valid = false;
                return model;
            }
        }

        /// <summary>
        /// To update an annotation model.
        /// </summary>
        /// <param name="model">The model to update.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <param name="onlyAssignedByTeacher">True to call only the assigned by the teacher.</param>
        /// <returns>Returns the updated annotation model with proerty valid as true if updated successfully.</returns>
        public LearningGapAnnotationModel UpdateAnnotationModel(LearningGapAnnotationModel model, bool onlyAssignedByTeacher, string teacherId)
        {
            try
            {
                model = _learningGapValidations.ValidateAnnotation(model);
                if (!model.Valid)
                {
                    return model;
                }

                var learningGapAnnotationDAO = new LearningGapAnnotationDAO();

                // Set the teacher id if only assigned by teacher.
                var modelWhere = new LearningGapAnnotationModel()
                {
                    Id = model.Id,
                };
                if (onlyAssignedByTeacher)
                {
                    modelWhere.ModifiedBy = teacherId;
                }

                return learningGapAnnotationDAO.Update(_connectionStringWrite, new LearningGapAnnotationModel()
                {
                    Text = model.Text,
                    ModifiedBy = model.ModifiedBy,
                }, modelWhere)[0];
            }
            catch (Exception)
            {
                model.Valid = false;
                return model;
            }
        }

        /// <summary>
        /// To assign a learning gap lesson to a student.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="teacherId">The id of the teacher.</param>
        /// <param name="limitDate">The limit date.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="token">The users' token.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <returns>Returns the learning gap assigned lesson.</returns>
        public LearningGapAssignedLessonModel AssignLearningGapLessonToStudent(string lessonId, string teacherId, string limitDate, string studentId, string subjectCode, string token,
             bool assignedByTeacherOnly)
        {
            // Check the active academic years for the student.
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);

            // If there are no active academic years, return an error.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new LearningGapAssignedLessonModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_no_active_academic_years"])
                    }
                };
            }

            // Check if the teacher is allowed to assign the lesson.
            if (assignedByTeacherOnly)
            {
                var teacherSubjects = this.GetTeacherAllowedSubjects(token);
                var allowedSubject = teacherSubjects.Find(x => x.ToLower() == subjectCode.ToLower());
                if (allowedSubject == null)
                {
                    return new LearningGapAssignedLessonModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_teacher_not_allowed"])
                        }
                    };
                }
            }

            // Check if the lesson was assigned before. 
            var lessonAssignedsToCheckList = new List<LearningGapAssignedLessonModel>();
            foreach (var academicYear in academicYears)
            {
                lessonAssignedsToCheckList.Add(new LearningGapAssignedLessonModel()
                {
                    AcademicYearId = academicYear,
                    LessonId = lessonId,
                    StudentId = studentId
                });
            }

            // Check if the lesson was assigned before.
            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();
            var lessonAssignedsToCheck = learningGapAssignedLessonDAO.Read(_connectionStringRead, lessonAssignedsToCheckList);

            if (lessonAssignedsToCheck != null && lessonAssignedsToCheck.Count > 0)
            {
                return new LearningGapAssignedLessonModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_lesson_already_assigned"])
                    }
                };
            }

            // Assign the lesson.
            var lessonAssignedsToCreate = new List<LearningGapAssignedLessonModel>();
            foreach (var academicYear in academicYears)
            {
                var lessonToAssign = new LearningGapAssignedLessonModel()
                {
                    AcademicYearId = academicYear,
                    LessonId = lessonId,
                    SubjectCode = subjectCode.ToLower(),
                    StudentId = studentId,
                    MaxTries = AppConfiguration.Configuration["AppSettings:lgap_default_max_tries"],
                    Completed = "0",
                    TeacherId = teacherId,
                    ModifiedBy = teacherId,
                    LimitDate = limitDate
                };

                // First, validate. 
                var validation = _learningGapValidations.ValidateLearningGapAssignedLessonModel(lessonToAssign);
                if (!validation.Valid)
                {
                    return validation;
                }
                lessonAssignedsToCreate.Add(lessonToAssign);
            }

            // Create the lesson.
            var createdLesson = learningGapAssignedLessonDAO.Create(_connectionStringWrite, lessonAssignedsToCreate)[0];
            createdLesson.Valid = true;
            return createdLesson;

        }

        /// <summary>
        /// To update assigned lesson.
        /// </summary>
        /// <param name="learningGapAssignedLessonModel">The learning gap lesson to update.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the learning gap assigned lesson updated.</returns>
        public LearningGapAssignedLessonModel UpdateAssignedLessonToStudent(LearningGapAssignedLessonModel learningGapAssignedLessonModel,
             bool assignedByTeacherOnly, string teacherId)
        {

            if (string.IsNullOrWhiteSpace(learningGapAssignedLessonModel.LimitDate))
            {
                var validation = _learningGapValidations.ValidateLearningGapAssignedLessonModelJustDateAndId(learningGapAssignedLessonModel);
                if (!validation.Valid)
                {
                    return validation;
                }
            }

            // Update the lesson.
            var learningGapAssignedLessonDAO = new LearningGapAssignedLessonDAO();

            var updateLessonWhere = new LearningGapAssignedLessonModel()
            {
                Id = learningGapAssignedLessonModel.Id
            };

            // if aready assigned by teacher, set the teacher id.
            if (assignedByTeacherOnly)
            {
                updateLessonWhere.ModifiedBy = teacherId;
            }

            var updatedLesson = learningGapAssignedLessonDAO.Update(_connectionStringWrite, new LearningGapAssignedLessonModel()
            {
                Completed = learningGapAssignedLessonModel.Completed,
                LimitDate = learningGapAssignedLessonModel.LimitDate,
                ModifiedBy = learningGapAssignedLessonModel.ModifiedBy
            }, updateLessonWhere)[0];

            updatedLesson.Valid = true;
            return updatedLesson;
        }

        /// <summary>
        /// To assign diagnostic test dates.
        /// </summary>
        /// <param name="studentUserIds">The student user ids to add the diagnostic.</param>
        /// <param name="teacherGroupIds">Optional (send null if optional). The teacher groups.</param>
        /// <param name="subjectCode">The subject code.</param>
        /// <param name="levelCode">The level code.</param>
        /// <param name="initialDiagnosticTestDate">The first diagnostic.</param>
        /// <param name="finalDiagnosticTestDate">The last diagnostic.</param>
        /// <param name="modifiedBy">The user that modified the row.</param>
        /// <param name="token">The token of the user.</param>
        /// <param name="teacherId">The teacher's id.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned subjects to teacher.</param>
        /// <returns>Returns the response with valid or not.</returns>
        public LearningGapDiagnosticYearScoreModel AssignDiagnosticTestDates(
            List<string> studentUserIds,
            List<string> teacherGroupIds,
            string subjectCode,
            string levelCode,
            string initialDiagnosticTestDate,
            string finalDiagnosticTestDate,
            string modifiedBy,
            string token,
            bool assignedByTeacherOnly, string teacherId)
        {

            // Validate. 
            var validateDates = _learningGapValidations.ValidateAssignDiagnosticDates(new LearningGapDiagnosticYearScoreModel()
            {
                InitialDiagnosticDateLimit = initialDiagnosticTestDate,
                FinalDiagnosticDateLimit = finalDiagnosticTestDate
            });

            if (!validateDates.Valid)
            {
                return validateDates;
            }

            // Lower case the subject code.
            subjectCode = subjectCode.ToLower();
            // Lower case the level code.
            levelCode = levelCode.ToLower();

            // Check if the teacher is allowed to assign the lesson.
            if (assignedByTeacherOnly)
            {
                var teacherSubjects = this.GetTeacherAllowedSubjects(token);
                var allowedSubject = teacherSubjects.Find(x => x.ToLower() == subjectCode.ToLower());
                if (allowedSubject == null)
                {
                    return new LearningGapDiagnosticYearScoreModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_teacher_not_allowed"])
                        }
                    };
                }
            }

            // If there are groups, find the students in the groups
            if (teacherGroupIds != null && teacherGroupIds.Count > 0)
            {
                var teacherGroupStudentsToFind = new List<Domain.Models.Teachers.TeacherGroupStudentModel>();
                foreach (var groupId in teacherGroupIds)
                {
                    teacherGroupStudentsToFind.Add(new Domain.Models.Teachers.TeacherGroupStudentModel()
                    {
                        TeacherGroupId = groupId
                    });
                }

                // Get the students in the groups.
                var studentsInGroups = new TeacherGroupStudentDAO().Read(
                    this._connectionStringRead,
                    teacherGroupStudentsToFind);

                // Add the students to the list.
                foreach (var studentInGroup in studentsInGroups)
                {
                    studentUserIds.Add(studentInGroup.StudentId);
                }
            }

            if (studentUserIds == null || studentUserIds.Count == 0)
            {
                // Return error. 
                return new LearningGapDiagnosticYearScoreModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_model_lack"])
                    }
                };
            }

            foreach (string studentUserId in studentUserIds)
            {
                // Read the active academic years for the student. 
                var academicYears = this.GetStudentActiveAcademicYears(token, studentUserId);

                // If there are no active academic years, return an error.
                if (academicYears == null || academicYears.Count == 0)
                {
                    continue;
                }

                // Prepare the read of the academic year scores.
                var academicYearScoresToRead = new List<LearningGapDiagnosticYearScoreModel>();
                foreach (var academicYear in academicYears)
                {

                    academicYearScoresToRead.Add(new LearningGapDiagnosticYearScoreModel()
                    {
                        StudentId = studentUserId,
                        AcademicYearId = academicYear
                    });
                }

                // Read the academic year scores.
                var learningGapDiagnosticYearScoreDAO = new LearningGapDiagnosticYearScoreDAO();
                var academicYearScores = learningGapDiagnosticYearScoreDAO.Read(_connectionStringRead, academicYearScoresToRead);

                // Set the ones to update. 
                var academicYearToUpdate = new List<LearningGapDiagnosticYearScoreModel>();
                var academicYearToUpdateWhere = new List<LearningGapDiagnosticYearScoreModel>();

                // Set the ones to create 
                var academicYearToCreate = new List<LearningGapDiagnosticYearScoreModel>();

                foreach (var academicYear in academicYears)
                {
                    var academicYearScore = academicYearScores.FirstOrDefault(
                        x => x.AcademicYearId == academicYear &&
                        x.StudentId == studentUserId &&
                        x.SubjectCode.ToLower() == subjectCode.ToLower() &&
                        x.LevelCode.ToLower() == levelCode.ToLower());
                    if (academicYearScore != null)
                    {
                        academicYearToUpdate.Add(new LearningGapDiagnosticYearScoreModel()
                        {
                            Id = academicYearScore.Id,
                            InitialDiagnosticDateLimit = initialDiagnosticTestDate,
                            FinalDiagnosticDateLimit = finalDiagnosticTestDate,
                            ModifiedBy = modifiedBy
                        });
                        academicYearToUpdateWhere.Add(new LearningGapDiagnosticYearScoreModel()
                        {
                            Id = academicYearScore.Id
                        });
                    }
                    else
                    {
                        academicYearToCreate.Add(new LearningGapDiagnosticYearScoreModel()
                        {
                            StudentId = studentUserId,
                            AcademicYearId = academicYear,
                            InitialDiagnosticDateLimit = initialDiagnosticTestDate,
                            FinalDiagnosticDateLimit = finalDiagnosticTestDate,
                            SubjectCode = subjectCode,
                            LevelCode = levelCode,
                            ModifiedBy = modifiedBy
                        });
                    }
                }

                // Update the academic year scores if they exist.
                if (academicYearToUpdate.Count > 0)
                {
                    learningGapDiagnosticYearScoreDAO.Update(_connectionStringWrite, academicYearToUpdate, academicYearToUpdateWhere);
                }

                // Create the academic year scores if any. 
                if (academicYearToCreate.Count > 0)
                {
                    learningGapDiagnosticYearScoreDAO.Create(_connectionStringWrite, academicYearToCreate);
                }
            }

            return new LearningGapDiagnosticYearScoreModel()
            {
                Valid = true
            };
        }

        /// <summary>
        /// To delete the learning gap lesson.
        /// </summary>
        /// <param name="id">The id of the learning gap lesson.</param>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <returns>Returns the response.</returns>
        public LearningGapAssignedLessonModel DeleteLearninngGapLesson(string id, bool assignedByTeacherOnly, string teacherId)
        {
            // If id is null, return error. 
            if (string.IsNullOrWhiteSpace(id))
            {
                return new LearningGapAssignedLessonModel();
            }

            var learningGapWhere = new LearningGapAssignedLessonModel()
            {
                Id = id
            };

            // Check if the teacher is allowed to delete the lesson.
            if (assignedByTeacherOnly)
            {
                learningGapWhere.ModifiedBy = teacherId;
            }

            new LearningGapAssignedLessonDAO().Delete(this._connectionStringWrite, learningGapWhere);
            return new LearningGapAssignedLessonModel() { Valid = true };
        }

        /// <summary>
        /// To get the required subjects for learning gap.
        /// </summary>
        /// <param name="assignedByTeacherOnly">True to call only the assigned by the teacher.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <param name="token">Token to make external requests.</param>
        /// <returns>Returns the list of string of codes.</returns>
        public List<LearningGapRequiredSubjectModel> GetRequiredSubjectsForLearningGap(bool assignedByTeacherOnly, string teacherId, string token)
        {
            var subjectsRequired = new LearningGapRequiredSubjectDAO().Read(this._connectionStringRead, new LearningGapRequiredSubjectModel(), false, false);

            if (assignedByTeacherOnly)
            {
                // Check if the teacher is allowed to assign the lesson.
                var teacherSubjects = this.GetTeacherAllowedSubjects(token);
                subjectsRequired = subjectsRequired.Where(x => teacherSubjects.Contains(x.SubjectCode.ToLower())).ToList();
            }

            // Read subjects from Athenas. 
            var athenasSubjects = AthenasHandler.GetAthenasSubjects(_langCode);

            foreach (var subjectRequired in subjectsRequired)
            {
                subjectRequired.SubjectName = athenasSubjects?.Find(x => x.Code.ToLower() == subjectRequired.SubjectCode.ToLower())?.Name;
            }
            return subjectsRequired;
        }

        /// <summary>
        /// To insert the quiz result manually.
        /// </summary>
        /// <param name="learningGapAssignedLessonId">The learning gap assigned lesson id.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="score">The score.</param>
        /// <param name="modifiedBy">The user that modified the row.</param>
        /// <param name="token">The user's token of the user making the request.</param>
        /// <returns>Returns the learning gap lesson quiz result with valid.</returns>
        public LearningGapLessonQuizResultModel CreateOrUpdateQuizResultManually(string learningGapAssignedLessonId,
            string studentId, string quizId, decimal score, string modifiedBy, string token)
        {
            var learningGapLessonQuizResultModelToInsert = new LearningGapLessonQuizResultModel()
            {
                LearningGapAssignedLessonId = learningGapAssignedLessonId,
                QuizId = quizId,
                Score = score.ToString(),
                ModifiedBy = modifiedBy
            };

            var validated = _learningGapValidations.ValidateLearningGapLessonQuizResultModel(learningGapLessonQuizResultModelToInsert);

            if (!validated.Valid)
            {
                return validated;
            }

            // Get the quiz detals. 
            var quizDetails = AthenasHandler.GetQuizAthenasModelList(new List<string>() { quizId });
            // Set the Title. 
            learningGapLessonQuizResultModelToInsert.QuizTitle = quizDetails[0].Title;

            // Set the percentage according to quiz value. 
            learningGapLessonQuizResultModelToInsert.Percentage =
            ((Decimal.Parse(learningGapLessonQuizResultModelToInsert.Score) / Decimal.Parse(quizDetails[0].CustomValue)) * 100).ToString("0.##");

            var learningGapLessonQuizResultDAO = new LearningGapLessonQuizResultDAO();

            // Check if exists already. 
            var resultExists = learningGapLessonQuizResultDAO.Read(_connectionStringRead,
                new LearningGapLessonQuizResultModel()
                {
                    LearningGapAssignedLessonId = learningGapAssignedLessonId,
                    QuizId = quizId,
                }, true, true);

            if (resultExists.Count == 0)
            {
                learningGapLessonQuizResultModelToInsert.ActualTry = "1";
            }
            else
            {
                var actualTry = 0;
                if (string.IsNullOrWhiteSpace(resultExists[0].ActualTry))
                {
                    actualTry = 1;
                }
                else
                {
                    actualTry += int.Parse(resultExists[0].ActualTry);
                }
                learningGapLessonQuizResultModelToInsert.ActualTry = actualTry.ToString();
            }

            // Insert if new, else, update. 
            if (resultExists.Count == 0)
            {
                // Create
                learningGapLessonQuizResultModelToInsert = learningGapLessonQuizResultDAO.Create(this._connectionStringWrite,
                    learningGapLessonQuizResultModelToInsert)[0];
                learningGapLessonQuizResultModelToInsert.Valid = true;
            }
            else
            {
                // Update 
                learningGapLessonQuizResultModelToInsert = learningGapLessonQuizResultDAO.Update(this._connectionStringWrite,
                    learningGapLessonQuizResultModelToInsert, new LearningGapLessonQuizResultModel()
                    {
                        Id = resultExists[0].Id
                    })[0];
                learningGapLessonQuizResultModelToInsert.Valid = true;
            }

            // Check if lesson is completed by getting Athenas Quizzes.
            UpdateLessonAsCompleted(studentId, quizDetails[0].LessonId, token);

            return learningGapLessonQuizResultModelToInsert;
        }

        /// <summary>
        /// To handle the diagnostic year scores.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="token">User's token.</param>
        /// <param name="teacherId">Teacher's id.</param>
        /// <param name="assignedByTeacherOnly">True to set the editable fields by the teacher.</param>
        /// <param name="levelCode">Optional: The level code.</param>
        /// <param name="subjectCode">Optional: The subject code.</param>
        /// <returns>Returns the diagnostic year scores.</returns>
        public List<LearningGapDiagnosticYearScoreModel> ReadDiagnosticYearScores(string studentId, string token, bool assignedByTeacherOnly, string teacherId, string subjectCode = null, string levelCode = null)
        {
            // Read the academic years of the student.
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);
            // A list to remote what teacher cannot handle.
            var diagnosticsToRemove = new List<LearningGapDiagnosticYearScoreModel>();

            // If there are no active academic years, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                return new List<LearningGapDiagnosticYearScoreModel>();
            }

            // Call the DAO to read the diagnostic year scores.
            var studentAcademicDiagnosticYearScores = new List<LearningGapDiagnosticYearScoreModel>();
            foreach (var academicYear in academicYears)
            {
                studentAcademicDiagnosticYearScores.Add(new LearningGapDiagnosticYearScoreModel()
                {
                    StudentId = studentId,
                    AcademicYearId = academicYear,
                    SubjectCode = subjectCode,
                    LevelCode = levelCode
                });
            }

            var learningGapDiagnostics = new LearningGapDiagnosticYearScoreDAO().Read(this._connectionStringRead,
                studentAcademicDiagnosticYearScores);

            // Call the teacher subjects. 
            var teacherSubjects = new List<string>();
            if (assignedByTeacherOnly)
            {
                teacherSubjects = this.GetTeacherAllowedSubjects(token);
            }

            if (learningGapDiagnostics.Count > 0)
            {
                // Call Athenas subjects. 
                var athenasSubjects = AthenasHandler.GetAthenasSubjects(_langCode);

                // Attach the subject name to the model.
                foreach (var diagnostic in learningGapDiagnostics)
                {
                    diagnostic.SubjectName = athenasSubjects?.Find(x => x.Code.ToLower() == diagnostic.SubjectCode.ToLower())?.Name;
                    // Check if the subject can be edited by the teacher.
                    if (assignedByTeacherOnly)
                    {
                        // If the subject is not in the list, remove it.
                        if (!teacherSubjects.Contains(diagnostic.SubjectCode.ToLower()))
                        {
                            diagnosticsToRemove.Add(diagnostic);
                        }
                    }
                }
            }

            // Remove after iterating
            foreach (var diagnostic in diagnosticsToRemove)
            {
                learningGapDiagnostics.Remove(diagnostic);
            }

            return learningGapDiagnostics;
        }

        /// Get the unread notifications from the teacher for a specific student
        /// </summary>
        /// <param name="teacherId">The teacher id</param>
        /// <param name="studentId">The student id</param>
        /// <param name="fromPage">The starting page (starting at 0)</param>
        /// <param name="limiter">Items per page</param>
        /// <returns>Returns the paginated model with the notifications</returns>
        public PaginatedResult<LearningGapNotificationModel> GetTeacherNotifications(string teacherId, string studentId, int fromPage, int limiter)
        {
            // Create an instance of LearningGapTeacherNotifications
            var learningGapTeacherNotifications = new LearningGapTeacherNotifications(_connectionStringRead);

            // Get unread notifications for the teacher
            var notifications = learningGapTeacherNotifications.GetUnreadNotificationsByTeacherId(teacherId, studentId);

            if (notifications == null || notifications.Count == 0)
            {
                return new PaginatedResult<LearningGapNotificationModel>(0, new List<LearningGapNotificationModel>());
            }

            var totalPages = notifications.Count / limiter;
            if (notifications.Count % limiter > 0)
            {
                totalPages++;
            }

            var paginatedResults = notifications.Skip(fromPage * limiter).Take(limiter).ToList();
            return new PaginatedResult<LearningGapNotificationModel>(totalPages, paginatedResults);
        }

        /// <summary>
        /// Get linked notification to the lesson or diagnostic test, then mark notification as read
        /// </summary>
        /// <param name="assignedLessonId">Id referenced in assigned lessons table</param>
        /// <param name="diagnosticTestId">Id referenced in diagnostic tests table</param>
        public void CheckAndUpdateNotificationForLessonOrDiagnosticTest(string assignedLessonId = null, string diagnosticTestId = null)
        {
            // Create an instance of LearningGapTeacherNotifications
            var learningGapTeacherNotifications = new LearningGapTeacherNotifications(_connectionStringRead);
            var notification = learningGapTeacherNotifications.GetNotificationByLessonOrDiagnosticId(assignedLessonId, diagnosticTestId);
            
            if (notification != null)
            {
                var learningGapTeacherNotificationsWrite = new LearningGapTeacherNotifications(_connectionStringWrite);
                learningGapTeacherNotificationsWrite.MarkNotificationAsRead(notification.Id);
            }
        }

        /// <summary>
        /// Delete the notification linked to the lesson when deleting the lesson
        /// </summary>
        /// <param name="assignedLessonId">Id referenced in assigned lessons table</param>
        public void DeleteNotificationsForAssignedLesson(string assignedLessonId)
        {
            // Create an instance of LearningGapTeacherNotifications
            var learningGapTeacherNotifications = new LearningGapTeacherNotifications(_connectionStringWrite);
            learningGapTeacherNotifications.DeleteNotificationByAssignedLessonId(assignedLessonId);
        }

        /// <summary>
        /// Check if teacher has any active notifications
        /// </summary>
        /// <param name="teacherId">The teacher id</param>
        /// <returns>true or false</returns>
        public bool CheckIfTeacherHasNotifications(string teacherId)
        {
            // Create an instance of LearningGapTeacherNotifications
            var learningGapTeacherNotifications = new LearningGapTeacherNotifications(_connectionStringRead);
            return learningGapTeacherNotifications.HasUnreadNotificationsByTeacherId(teacherId);
        }

        /// <summary>
        /// Check if the student has active notifications
        /// </summary>
        /// <param name="teacherId">The teacher's id</param>
        /// <returns>A list with the student id and a flag to show if the student has notifications</returns>
        public List<LearningGapStudentHasNotificationsModel> CheckIfStudentHasNotifications(string teacherId)
        {
            // Create an instance of LearningGapTeacherNotifications
            var learningGapTeacherNotifications = new LearningGapTeacherNotifications(_connectionStringRead);
            return learningGapTeacherNotifications.StudentHasNotifications(teacherId);
        }

        /// <summary>
        /// To update the lesson as completed.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="token">The user's token that is making the request.</param>
        private void UpdateLessonAsCompleted(string studentId, string lessonId, string token)
        {
            // Get the academic years of the student. 
            var academicYears = this.GetStudentActiveAcademicYears(token, studentId);
            // If there are no active academic years, return an empty list.
            if (academicYears == null || academicYears.Count == 0)
            {
                return;
            }

            // Read the student learning gap assigned lessons. 
            var learningGapAssignedLessonsDAO = new LearningGapAssignedLessonDAO();

            // Prepare the ones to read. 
            var learningGapAssignedLessonsToCheck = new List<LearningGapAssignedLessonModel>();
            foreach (var academicYear in academicYears)
            {
                learningGapAssignedLessonsToCheck.Add(new LearningGapAssignedLessonModel()
                {
                    AcademicYearId = academicYear,
                    StudentId = studentId,
                    LessonId = lessonId,
                    Completed = "0"
                });
            }

            var assignedLessonsToCheck = learningGapAssignedLessonsDAO.Read(_connectionStringRead, learningGapAssignedLessonsToCheck);
            if (assignedLessonsToCheck.Count == 0)
            {
                return;
            }

            // Else, check if lesson was completed. 
            // Get all the quizzes of the lesson. 
            var athenasQuizzes = AthenasHandler.GetLessonQuizzes(lessonId);
            // Filter out practices. 
            athenasQuizzes.Quizzes = athenasQuizzes.Quizzes.Where(q => q.IsExam != "0").ToList();

            // Read the learning gp results.
            var quizResultsToCheck = new List<LearningGapLessonQuizResultModel>();
            foreach (var assignedLesson in assignedLessonsToCheck)
            {
                quizResultsToCheck.Add(new LearningGapLessonQuizResultModel()
                {
                    LearningGapAssignedLessonId = assignedLesson.Id,
                });
            }

            var learningGapLessonQuizResultDAO = new LearningGapLessonQuizResultDAO();
            // Read the results. 
            var resultsLearningGapQuizzes = learningGapLessonQuizResultDAO.Read(this._connectionStringRead,
                quizResultsToCheck);

            // If no results, then it is not completed. 
            if (resultsLearningGapQuizzes.Count == 0)
            {
                return;
            }

            // Else, compare if all quizzes are present.
            var missingQuizzes = athenasQuizzes.Quizzes
                .Where(athenaQuiz => !resultsLearningGapQuizzes.Any(result => result.QuizId == athenaQuiz.Id))
                .ToList();

            if (missingQuizzes.Any())
            {
                // There are quizzes missing in the results.
                // Handle the incomplete case here.
                return;
            }

            // Since all quizzes all present, update all the learningGapLessons as completed. 
            var assignedLessonToUpdate = new List<LearningGapAssignedLessonModel>();
            var assignedLessonToUpdateWhere = new List<LearningGapAssignedLessonModel>();
            foreach (var assignedLesson in assignedLessonsToCheck)
            {
                assignedLessonToUpdate.Add(new LearningGapAssignedLessonModel()
                {
                    Completed = "1"
                });
                assignedLessonToUpdateWhere.Add(new LearningGapAssignedLessonModel()
                {
                    Id = assignedLesson.Id,
                });
            }

            if (assignedLessonToUpdate.Count > 0)
            {
                // Update
                learningGapAssignedLessonsDAO.Update(_connectionStringWrite, assignedLessonToUpdate, assignedLessonToUpdateWhere);
                return;
            }
        }

        /// <summary>
        /// To get the student active academic years.
        /// </summary>
        /// <param name="token">The user that is making the request token.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns the ids of the active academic years.</returns>
        private List<string> GetStudentActiveAcademicYears(string token, string studentId)
        {
            // First, get the students' schools
            var schoolIds = FeignAPI.GetUserDetails.GetSchoolIdsOfUser(token, studentId);

            if (schoolIds == null || schoolIds.Count == 0)
            {
                return new List<string>();
            }

            // Then get the academic years of the schools.
            return FeignAPI.SchoolsDetailsHandler.GetActiveAcademicYearsIdsBySchoolIds(token, schoolIds);
        }

        /// <summary>
        /// To get the teacher allowed subjects.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Return the list string of subject codes.</returns>
        private List<string> GetTeacherAllowedSubjects(string token)
        {
            // Get the teacher's allowed subjects.
            var courses = SubscriptionHandler.GetUserCourses(token);
            if (courses == null || courses.UserSubscribedLevelsSubjectsModelList == null || courses.UserSubscribedLevelsSubjectsModelList.Count == 0)
            {
                return new List<string>();
            }

            var subjects = new List<string>();
            foreach (var course in courses.UserSubscribedLevelsSubjectsModelList)
            {
                if (!string.IsNullOrWhiteSpace(course.Subject))
                {
                    subjects.Add(course.Subject.ToLower());
                }
            }

            return subjects;
        }
    }

    #endregion teacher
}