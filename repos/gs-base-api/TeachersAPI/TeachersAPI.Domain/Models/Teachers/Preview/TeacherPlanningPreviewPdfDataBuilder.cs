using System;
using System.Collections.Generic;
using System.Linq;
using GSPlanning.Domain;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.Domain.Models.Teachers.Preview;
using GSPlanning.Models;

namespace TeachersAPI.TeachersAPI.Services.PlanningPreview
{
    /// <summary>
    /// 
    /// </summary>
    public class TeacherPlanningPreviewPdfDataBuilder
    {
        private TeacherPlanningDetailResponseModel teacherPlanningDetail;
        private List<LessonResponseModel> lessonResponseModels;
        private List<TeacherPlanLessonFullDetailModel> teacherPlanLessonFullDetailModels;
        private List<ForumModel> forumModels;
        private List<TeacherPlanCustomLesson> customLessons;
        private List<TeacherPlanAccommodation> accommodations;
        private List<TeacherPlanEvaluation> evaluations;
        private List<TeacherPlanInitiativeProjectInnovation> initativeProjectInnovations;
        private List<TeacherPlanMaterial> materials;
        private List<TeacherPlanOtherSubjectIntegration> otherSubjectIntegrations;
        private List<TeacherPlanPraxisReflection> praxisReflections;
        private List<TeacherPlanStrategy> strategies;
        private List<TeacherPlanDifferentiationStrategy> differentiationStrategies;
        private List<TeacherPlanObservation> observations;
        private List<TeacherPlanCommonExperience> commonExperiences;
        private List<TeacherPlanActiveResponse> activeResponses;

        /// <summary>
        /// 
        /// </summary>
        private TeacherPlanningPreviewPdfDataBuilder()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public static TeacherPlanningPreviewPdfDataBuilder Builder
        {
            get
            {
                return new TeacherPlanningPreviewPdfDataBuilder();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherPlanningDetail"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddTeacherPlanningDetail(TeacherPlanningDetailResponseModel teacherPlanningDetail)
        {
            this.teacherPlanningDetail = teacherPlanningDetail;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lessonResponseModels"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddLessonModels(List<LessonResponseModel> lessonResponseModels)
        {
            this.lessonResponseModels = lessonResponseModels;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherPlanLessonFullDetailModels"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TeacherPlanningPreviewPdfDataBuilder AddTeacherPlanLessonFullDetail(List<TeacherPlanLessonFullDetailModel> teacherPlanLessonFullDetailModels)
        {
            this.teacherPlanLessonFullDetailModels = teacherPlanLessonFullDetailModels;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forumModels"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddForums(List<ForumModel> forumModels)
        {
            this.forumModels = forumModels;
            return this;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customLessons"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddCustomLessons(List<TeacherPlanCustomLesson> customLessons)
        {
            this.customLessons = customLessons;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accommodations"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddAccommodations(List<TeacherPlanAccommodation> accommodations)
        {
            this.accommodations = accommodations;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="evaluations"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddEvaluations(List<TeacherPlanEvaluation> evaluations)
        {
            this.evaluations = evaluations;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initativeProjectInnovations"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddInitiativeProjectInnovations(List<TeacherPlanInitiativeProjectInnovation> initativeProjectInnovations)
        {
            this.initativeProjectInnovations = initativeProjectInnovations;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="materials"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddMaterials(List<TeacherPlanMaterial> materials)
        {
            this.materials = materials;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherSubjectIntegrations"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddOtherSubjectIntegrations(List<TeacherPlanOtherSubjectIntegration> otherSubjectIntegrations)
        {
            this.otherSubjectIntegrations = otherSubjectIntegrations;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="praxisReflections"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddPraxisReflections(List<TeacherPlanPraxisReflection> praxisReflections)
        {
            this.praxisReflections = praxisReflections;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strategies"></param>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfDataBuilder AddStratergies(List<TeacherPlanStrategy> strategies)
        {
            this.strategies = strategies;
            return this;
        }

        public TeacherPlanningPreviewPdfDataBuilder AddDifferentiationStrategies(List<TeacherPlanDifferentiationStrategy> differentiationStrategies)
        {
            this.differentiationStrategies = differentiationStrategies;
            return this;
        }

        public TeacherPlanningPreviewPdfDataBuilder AddObservations(List<TeacherPlanObservation> observations)
        {
            this.observations = observations;
            return this;
        }

        public TeacherPlanningPreviewPdfDataBuilder AddCommonExperiences(List<TeacherPlanCommonExperience> commonExperiences)
        {
            this.commonExperiences = commonExperiences;
            return this;
        }


        public TeacherPlanningPreviewPdfDataBuilder AddActiveResponses(List<TeacherPlanActiveResponse> activeResponses)
        {
            this.activeResponses = activeResponses;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TeacherPlanningPreviewPdfData Build(string langCode ="es")
        {
            #region Build Labels


            var buildLabels = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {

                ["es"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["LessonContent"] = "Contenido genial",
                    ["FreeContent"] = "Contenido libre",
                    ["Assignments"] = "Asignaciones",
                    ["Subjects"] = "Integración con otras materias",
                    ["Projects"] = "Iniciativa o proyecto innovador",
                    ["Evaluations"] = "Evaluación y avalúos",
                    ["Accommodations"] = "Acomodos razonables",
                    ["Materials"] = "Materiales",
                    ["Reflections"] = "Reflexión de praxis",
                    ["DifferentiationStrategy"] = "Estrategias de instrucción diferenciada",
                    ["Observations"] = "Observaciones",
                    ["CommonExperience"] = "Plan de mejoramiento experiencia común (2–3 min)",
                    ["ActiveStudentResponse"] = "Plan de mejoramiento respuesta activa del estudiante",
                    ["GoalAchieved"] = "Se logró el objetivo",
                    ["GoalNotAchieved"] = "No se logró el objetivo"
                },
                ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["LessonContent"] = "Genial content",
                    ["FreeContent"] = "Open content",
                    ["Assignments"] = "Assignments",
                    ["Subjects"] = "Integration with other subjects",
                    ["Projects"] = "Innovative initiative or project",
                    ["Evaluations"] = "Evaluations (Assessments)",
                    ["Accommodations"] = "Reasonable accommodations",
                    ["Materials"] = "Materials",
                    ["Reflections"] = "Praxis reflection",
                    ["DifferentiationStrategy"] = "Differentiated instruction strategies",
                    ["Observations"] = "Observations",
                    ["CommonExperience"] = "Improvement plan common experience (2–3 min)",
                    ["ActiveStudentResponse"] = "Improvement plan student active response",
                    ["GoalAchieved"] = "Goal achieved",
                    ["GoalNotAchieved"] = "Goal not achieved"
                }
            };

            #endregion

            if (!buildLabels.TryGetValue(langCode ?? "es", out var labels))
                labels = buildLabels["es"];

            string T(string key) => labels.TryGetValue(key, out var v) ? v : key;

            DateTime openDate = DateTime.Parse(teacherPlanningDetail.TeacherPlanModel.OpenDate);
            DateTime closeDate = DateTime.Parse(teacherPlanningDetail.TeacherPlanModel.CloseDate);
            var pdfData = new TeacherPlanningPreviewPdfData(openDate, closeDate);
            
            pdfData.WeekNumber = !string.IsNullOrWhiteSpace(teacherPlanningDetail.TeacherPlanModel.WeekNumber) ? int.Parse(teacherPlanningDetail.TeacherPlanModel.WeekNumber) : 0;
            pdfData.SubjectAndGrade = $"{teacherPlanningDetail.TeacherPlanModel} {teacherPlanningDetail.TeacherPlanModel.LevelCode}";
            pdfData.TeacherFullName = $"{teacherPlanningDetail.TeacherPlanModel.TeacherFullName}";
            pdfData.SubjectAndGrade = $"{teacherPlanningDetail.TeacherPlanModel.SubjectName} {teacherPlanningDetail.TeacherPlanModel.LevelCode}";

            // Get unique students groups
            HashSet<string> uniqueGroups = new HashSet<string>(
                teacherPlanningDetail.GroupsAttachedList.Select(x => x.GroupName)
            );
            pdfData.GroupStudent = string.Join(", ", uniqueGroups);

            // Create dictionary for unique standards and expectations by lesson
            var uniqueStandardsByDay = new Dictionary<DateTime, HashSet<(string Title, string Description)>>();
            var uniqueExpectationsByDay = new Dictionary<DateTime, HashSet<(string Title, string Description)>>();

            foreach (var lesson in teacherPlanningDetail.LessonAttachedModelList)
            {
                foreach (var date in lesson.TeacherPlanLessonDateModels)
                {
                    var implementationDate = DateTime.Parse(date.ImplementationDate);

                    pdfData.AddDataLessonToDay(implementationDate, new DataInfo { Title = T("LessonContent"), Description = lesson.Title, Type = T("LessonContent") });

                    if (!uniqueStandardsByDay.ContainsKey(implementationDate))
                        uniqueStandardsByDay[implementationDate] = new HashSet<(string, string)>();

                    if (!uniqueExpectationsByDay.ContainsKey(implementationDate))
                        uniqueExpectationsByDay[implementationDate] = new HashSet<(string, string)>();

                    var standards = lessonResponseModels
                    .Where(x => x.LessonModifierRequestModel.LessonModel.Id == lesson.ItemId)
                    .SelectMany(x => x.LessonModifierRequestModel.ConstructedCodeHandleModelList)
                    .SelectMany(x => x.ConstructedCodeCodeModels.Where(y => y.CodeTypeId == 1))
                    .Select(x => new DataInfo { Description = x.CodeDescription, Title = lesson.Title })
                    .ToList();

                    var expectations = lessonResponseModels
                    .Where(x => x.LessonModifierRequestModel.LessonModel.Id == lesson.ItemId)
                    .SelectMany(x => x.LessonModifierRequestModel.ConstructedCodeHandleModelList)
                    .SelectMany(x => x.ConstructedCodeCodeModels.Where(y => y.CodeTypeId == 7))
                    .Select(x => new DataInfo { Description = x.CodeDescription, Title = lesson.Title })
                    .ToList();

                    var strategies = lessonResponseModels
                    .Where(x => x.LessonModifierRequestModel.LessonModel.Id == lesson.ItemId)
                    .SelectMany(x => x.LessonModifierRequestModel.LessonStrategyModelList)
                    .Select(x => new DataInfo { Description = x.Desc, Title = lesson.Title })
                    .ToList();

                    var objectives = lessonResponseModels
                    .Where(x => x.LessonModifierRequestModel.LessonModel.Id == lesson.ItemId)
                    .SelectMany(x => x.LessonModifierRequestModel.LessonObjectiveModelList)
                    .Select(x => new DataInfo { Description = x.Desc, Title = lesson.Title })
                    .ToList();

                    var transversalThemes = lessonResponseModels
                    .Where(x => x.LessonModifierRequestModel.LessonModel.Id == lesson.ItemId)
                    .SelectMany(x => x.LessonModifierRequestModel.LessonTransversalThemeModelList)
                    .Select(x => new DataInfo { Description = x.Desc, Title = lesson.Title })
                    .ToList();

                    var units = lessonResponseModels
                    .Where(x => x.LessonModifierRequestModel.LessonModel.Id == lesson.ItemId)
                    .SelectMany(x => x.LessonModifierRequestModel.LessonPerformanceTaskModelList)
                    .Select(x => new DataInfo { Description = x.Desc, Title = lesson.Title })
                    .ToList();

                    // Add unique standards
                    foreach (var standard in standards)
                    {
                        var standardKey = (standard.Title, standard.Description);

                        if (uniqueStandardsByDay[implementationDate].Add(standardKey))
                        {
                            pdfData.AddDataStandardsToDay(implementationDate, standard);
                        }
                    }

                    // Add unique expectations
                    foreach (var expectation in expectations)
                    {
                        var key = (expectation.Title, expectation.Description);
                        if (uniqueExpectationsByDay[implementationDate].Add(key))
                        {
                            pdfData.AddDataExpectationsToDay(implementationDate, expectation);
                        }
                    }

                    strategies.ToList().ForEach(x => pdfData.AddDataStratergiesToDay(DateTime.Parse(date.ImplementationDate), x));
                    objectives.ToList().ForEach(x => pdfData.AddDataObjetivesToDay(DateTime.Parse(date.ImplementationDate), x));
                    transversalThemes.ToList().ForEach(x => pdfData.AddDataTransversalThemesToDay(DateTime.Parse(date.ImplementationDate), x));
                    units.ToList().ForEach(x => pdfData.AddDataUnitsToDay(DateTime.Parse(date.ImplementationDate), x));
                }
            }

            foreach (dynamic assignment in teacherPlanningDetail.QuizzesAttachedModelList)
            {
                var dueDate = assignment.DueDate.ToString().Split(' ')[0].ToString();
                pdfData.AddDataAssignmentsToDay(DateTime.Parse(dueDate), new DataInfo { Title = T("Assignments"), Description = assignment.Title });
            }

            foreach (var lessons in teacherPlanLessonFullDetailModels)
            {
                foreach (var quizModel in lessons.TeacherPlanLessonQuizModels)
                {

                    var dueDate = quizModel.DueDate.Split(' ')[0];
                    var dueDateComplete = DateTime.Parse(quizModel.DueDate);
                    pdfData.AddDataEvaluationsToDay(DateTime.Parse(dueDate), new DataInfo { Title = lessons.TeacherPlanLessonModel.LessonTitle, Description = quizModel.QuizTitle, SecundaryDescription = dueDateComplete.ToString("hh:mm tt") });
                }
            }

            foreach (var forum in forumModels)
            {
                foreach (var implementationDate in forum.ImplementationDates)
                {
                    var dueDate = implementationDate.ImplementationDate.Split(' ')[0];
                    pdfData.AddDataEvaluationsToDay(DateTime.Parse(dueDate), new DataInfo { Title = forum.Title, Description = forum.Description});
                }
            }
            
            foreach (var otherSubjectIntegration in otherSubjectIntegrations)
            {
                foreach (var implementationDate in otherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates)
                {
                    pdfData.AddDataOtherSubjectIntegrationsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Subjects"), Description = otherSubjectIntegration.Description, Type = DataInfo.CustomLessonType});
                }
            }
            
            foreach (var initiativeProjectInnovation in initativeProjectInnovations)
            {
                foreach (var implementationDate in initiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates)
                {
                    pdfData.AddDataInitiativeProjectInnovationsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Projects"), Description = initiativeProjectInnovation.Description, Type = DataInfo.CustomLessonType});
                }
            }
            
            foreach (var evaluation in evaluations)
            {
                foreach (var implementationDate in evaluation.TeacherPlanEvaluationsDates)
                {
                    pdfData.AddDataEvaluationsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Evaluations"), Description = evaluation.GetDescriptionByLang(langCode)});
                }
            }

            foreach (var accommodation in accommodations)
            {
                foreach (var implementationDate in accommodation.TeacherPlanAccommodationDates)
                {
                    pdfData.AddDataAccommodationsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Accommodations"), Description = accommodation.Description, Type = DataInfo.CustomLessonType});
                }
            }
            
            foreach (var material in materials)
            {
                foreach (var implementationDate in material.TeacherPlanMaterialDates)
                {
                    pdfData.AddDataMaterialsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Materials"), Description = material.Description, Type = DataInfo.CustomLessonType});
                }
            }

            foreach (var strategy in strategies)
            {
                foreach (var implementationDate in strategy.TeacherPlanStrategyDates)
                {
                    pdfData.AddDataCustomStrategiesToDay(implementationDate.ImplementationDate, new DataInfo { Title = strategy.Description ,Description = strategy.Strategies, Type = DataInfo.CustomLessonType });
                }
            }

            foreach (var praxisReflection in praxisReflections)
            {
                foreach (var implementationDate in praxisReflection.TeacherPlanPraxisReflectionDates)
                {
                    string description;

                    string completedMessage = praxisReflection.IsCompleted == 1
                        ? T("GoalAchieved")
                        : T("GoalNotAchieved");

                    string details = null;

                    // Use custom reflections
                    if (praxisReflection.IsCompleted == 1)
                    {
                        details = praxisReflection.Detail;
                    }
                    else
                    {
                        if (praxisReflection.TeacherPlanPraxisReflectionCompletedReasons?.Any() == true)
                        {
                            // Join reflections descriptions
                            var reasonDescriptions = praxisReflection.TeacherPlanPraxisReflectionCompletedReasons
                                .Where(reason => reason.PraxisReflectionCompletedReason != null)
                                .Select(reason => reason.PraxisReflectionCompletedReason.Description)
                                .Where(desc => !string.IsNullOrEmpty(desc));

                            if (reasonDescriptions.Any())
                            {
                                details = string.Join(", ", reasonDescriptions);
                            }
                        }
                    }

                    description = !string.IsNullOrEmpty(details)
                        ? $"{completedMessage}: {details}" : completedMessage;

                    pdfData.AddDataPraxisReflectionsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Reflections"), Description = description, Type = DataInfo.CustomLessonType});
                }
            }

            // Handle custom/free lessons
            foreach (var customLesson in customLessons)
            {
                foreach (var implementationDate in customLesson.TeacherPlanCustomLessonDates)
                {
                    string lessonTitle = customLesson.Title;

                    pdfData.AddDataLessonToDay(implementationDate.ImplementationDate, new DataInfo
                    {
                        Title = T("FreeContent"),
                        Description = lessonTitle,
                        Type = DataInfo.CustomLessonType
                    });

                    if (customLesson.TeacherPlanCustomLessonEducationalCriterias != null)
                    {
                        // Type 0: Transversal Themes
                        var transversalThemes = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 0)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        transversalThemes.ToList().ForEach(x => pdfData.AddDataTransversalThemesToDay(implementationDate.ImplementationDate, x));

                        // Type 1: Standards
                        var standards = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 1)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType
                            })
                            .ToList();

                        standards.ToList().ForEach(x => pdfData.AddDataStandardsToDay(implementationDate.ImplementationDate, x));

                        // Type 2: Expectations
                        var expectations = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 2)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        expectations.ToList().ForEach(x => pdfData.AddDataExpectationsToDay(implementationDate.ImplementationDate, x));

                        // Type 3: Strategies
                        var strategies = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 3)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        strategies.ToList().ForEach(x => pdfData.AddDataStratergiesToDay(implementationDate.ImplementationDate, x));

                        // Type 4: Objectives
                        var objectives = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 4)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        objectives.ToList().ForEach(x => pdfData.AddDataObjetivesToDay(implementationDate.ImplementationDate, x));

                        // Type 5: Units
                        var units = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 5)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        units.ToList().ForEach(x => pdfData.AddDataUnitsToDay(implementationDate.ImplementationDate, x));

                        // Type 6: Introductory activity
                        var introductoryActivity = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 6)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        introductoryActivity.ToList().ForEach(x => pdfData.AddDataIntroductoryActivityToDay(implementationDate.ImplementationDate, x));

                        // Type 7: Development activity
                        var developmentActivity = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 7)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        developmentActivity.ToList().ForEach(x => pdfData.AddDataDevelopmentActivityToDay(implementationDate.ImplementationDate, x));

                        // Type 8: Closing activity
                        var closingActivity = customLesson.TeacherPlanCustomLessonEducationalCriterias
                            .Where(ec => ec.EducationalCriteria.Type == 8)
                            .Select(ec => new DataInfo
                            {
                                Description = ec.EducationalCriteria.Description,
                                Title = lessonTitle,
                                Type = DataInfo.CustomLessonType

                            })
                            .ToList();

                        closingActivity.ToList().ForEach(x => pdfData.AddDataClosingActivityToDay(implementationDate.ImplementationDate, x));
                    }
                }
            }

            foreach (var differentiationStrategy in differentiationStrategies)
            {
                foreach (var implementationDate in differentiationStrategy.TeacherPlanDifferentiationStrategyDates)
                {
                    pdfData.AddDataDifferentiationStrategiesToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("DifferentiationStrategy"), Description = differentiationStrategy.Description });
                }

            }

            foreach (var observation in observations)
            {
                foreach (var implementationDate in observation.TeacherPlanObservationDates)
                {
                    pdfData.AddDataObservationsToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("Observations"), Description = observation.Detail });
                }
            }

            foreach (var commonExperience in commonExperiences)
            {
                foreach (var implementationDate in commonExperience.TeacherPlanCommonExperienceDates)
                {
                    pdfData.AddDataCommonExperiencesToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("CommonExperience"), Description = commonExperience.Detail });
                }
            }

            foreach (var activeResponse in activeResponses)
            {
                foreach (var implementationDate in activeResponse.TeacherPlanActiveResponseDates)
                {
                    pdfData.AddDataActiveResponsesToDay(implementationDate.ImplementationDate, new DataInfo { Title = T("ActiveStudentResponse"), Description = activeResponse.Description });
                }
            }


            return pdfData;
        }
    }
}