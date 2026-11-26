using GSPlanning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanCustomLessonService : Service<EducationalCriteria>
    {
        private readonly Repository<EducationalCriteria> _educationalCriteriaRepository;
        private readonly Repository<TeacherPlanCustomLesson> _teacherPlanCustomLessonRepository;
        private readonly Repository<TeacherPlanCustomLessonEducationalCriteria> _teacherPlanCustomLessonEducationalCriteriaRepository;
        private readonly Repository<TeacherPlanCustomLessonDate> _teacherPlanCustomLessonDateRepository;
        private readonly Repository<EducationalCriteriaTranslation> _educationalCriteriaTranslationRepository;

        public TeacherPlanCustomLessonService(
            Repository<EducationalCriteria> educationalCriteriaRepository, 
            Repository<TeacherPlanCustomLessonEducationalCriteria> teacherPlanCustomLessonEducationalCriteriaRepository, 
            Repository<TeacherPlanCustomLesson> teacherPlanCustomLessonRepository, 
            Repository<TeacherPlanCustomLessonDate> teacherPlanCustomLessonDateRepository,
            Repository<EducationalCriteriaTranslation> educationalCriteriaTranslationRepository
            ) 
            : base(educationalCriteriaRepository)
        {
            _educationalCriteriaRepository = educationalCriteriaRepository;
            _teacherPlanCustomLessonEducationalCriteriaRepository = teacherPlanCustomLessonEducationalCriteriaRepository;
            _teacherPlanCustomLessonRepository = teacherPlanCustomLessonRepository;
            _teacherPlanCustomLessonDateRepository = teacherPlanCustomLessonDateRepository;
            _educationalCriteriaTranslationRepository = educationalCriteriaTranslationRepository;
        }

        public async Task<List<EducationalCriteria>> GetEducationCriteria()
        {
           return await GetTypes(x => x.Type, EducationalCriteria.Transversal);
        }

        public async Task<List<EducationalCriteria>> GetEducationCriteriaTranslations(string langCode = "es")
        {
            var educationalCriterias = await GetTypes();
            var translations = (await _educationalCriteriaTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var educationalCriteria in educationalCriterias)
            {
                var translation = translations.FirstOrDefault(x => x.EducationalCriteriaId == educationalCriteria.Id);
                if (translation != null)
                {
                    educationalCriteria.Description = translation.Description;
                }
            }
            return educationalCriterias;
        }

        public async Task<EducationalCriteria> CreateEducationCriteria(EducationalCriteria educationalCriteria)
        {
            // Null check
            if (educationalCriteria == null)
                throw new ArgumentNullException(nameof(educationalCriteria), "EducationalCriteria cannot be null.");

            // Required fields validation
            if (string.IsNullOrWhiteSpace(educationalCriteria.Description))
                throw new ArgumentException("Description is required.", nameof(educationalCriteria.Description));

            if (educationalCriteria.Type == EducationalCriteria.Transversal)
            {
                throw new ArgumentException(
                    $"Invalid type {nameof(educationalCriteria.Type)} {educationalCriteria.Type}");
            }
            
            var id = await _educationalCriteriaRepository.AddAsync(new EducationalCriteria()
            {
                Description =  educationalCriteria.Description,
                ModifiedBy = educationalCriteria.ModifiedBy,
                Type = educationalCriteria.Type
            });

            return await _educationalCriteriaRepository.GetByIdAsync(id);
        }

        public async Task RemoveEducationCriteria(int teacherPlanCustomLessonId, int educationalCriteriaId)
        {
            // Validate input IDs
            if (teacherPlanCustomLessonId <= 0)
                throw new ArgumentException("Invalid TeacherPlanCustomLessonId.", nameof(teacherPlanCustomLessonId));

            if (educationalCriteriaId <= 0)
                throw new ArgumentException("Invalid EducationalCriteriaId.", nameof(educationalCriteriaId));

            var criteriaIds =
                (await _teacherPlanCustomLessonEducationalCriteriaRepository.GetAllByAsync(
                    x => x.TeacherPlanCustomLessonId, teacherPlanCustomLessonId))
                .Where(x => x.EducationalCriteriaId == educationalCriteriaId)
                .Select(x => x.Id)
                .ToArray();
            
            if (criteriaIds.Any())
            {
                await _teacherPlanCustomLessonEducationalCriteriaRepository.DeleteByInAsync(x => x.Id,
                    criteriaIds);
            }
            
            await _educationalCriteriaRepository.DeleteAsync(educationalCriteriaId);
        }

        public async Task<List<TeacherPlanCustomLesson>> GetTeacherPlanCustomLessonsByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {

            var teacherPlanCustomLessons =
                (await _teacherPlanCustomLessonRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanCustomLessons.Any())
            {
                return new List<TeacherPlanCustomLesson>();
            }
            
            var teacherPLanCustomLessonsIds = teacherPlanCustomLessons.Select(x => x.Id).ToArray();
            
            var teacherPlanCustomLessonDates = (await _teacherPlanCustomLessonDateRepository.GetAllInByAsync(x => x.TeacherPlanCustomLessonId,
                teacherPLanCustomLessonsIds)).ToList();

            var teacherPlanCustomLessonEducationCriteria = (await _teacherPlanCustomLessonEducationalCriteriaRepository.GetAllInByAsync(x => x.TeacherPlanCustomLessonId,
                teacherPLanCustomLessonsIds)).ToList();

            var types = (await GetEducationCriteriaTranslations(langCode)).ToDictionary(x => x.Id);
            
            foreach (var teacherPlanCustomLesson in teacherPlanCustomLessons)
            {
                teacherPlanCustomLesson.TeacherPlanCustomLessonDates = teacherPlanCustomLessonDates
                    .Where(x => x.TeacherPlanCustomLessonId == teacherPlanCustomLesson.Id).ToList();

                var educationalCriterias = teacherPlanCustomLessonEducationCriteria
                    .Where(x => x.TeacherPlanCustomLessonId == teacherPlanCustomLesson.Id)
                    .ToList();
                
                educationalCriterias.ForEach(x => x.EducationalCriteria = types[x.EducationalCriteriaId]);
                
                teacherPlanCustomLesson.TeacherPlanCustomLessonEducationalCriterias = educationalCriterias;

            }

            return teacherPlanCustomLessons;
        }

        public async Task CreateTeacherPlanCustomLesson(TeacherPlanCustomLesson teacherPlanCustomLesson)
        {
            // Null check
            if (teacherPlanCustomLesson == null)
                throw new ArgumentNullException(nameof(teacherPlanCustomLesson), "TeacherPlanCustomLesson cannot be null.");

            var id = await _teacherPlanCustomLessonRepository.AddAsync(teacherPlanCustomLesson);
            
            if (teacherPlanCustomLesson.TeacherPlanCustomLessonEducationalCriterias.Count > 0)
            {
                await SaveTeacherPlanCustomLessonTypes(teacherPlanCustomLesson, id);
            }

            if (teacherPlanCustomLesson.TeacherPlanCustomLessonDates.Count > 0)
            {
                await SaveTeacherPlanCustomLessonDates(teacherPlanCustomLesson, id);
            }
        }
        
        public async Task UpdateTeacherPlanCustomLesson(TeacherPlanCustomLesson teacherPlanCustomLesson)
        {
            if (teacherPlanCustomLesson == null)
                throw new ArgumentNullException(nameof(teacherPlanCustomLesson), "TeacherPlanCustomLesson cannot be null.");

            if (teacherPlanCustomLesson.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanCustomLesson)}.{nameof(teacherPlanCustomLesson.Id)} property.");
            }

            await _teacherPlanCustomLessonRepository.UpdateAsync(teacherPlanCustomLesson);
            
            if (teacherPlanCustomLesson.TeacherPlanCustomLessonEducationalCriterias.Count > 0)
            {
                await _teacherPlanCustomLessonEducationalCriteriaRepository.DeleteByAsync(
                    x => x.TeacherPlanCustomLessonId, teacherPlanCustomLesson.Id);
                await SaveTeacherPlanCustomLessonTypes(teacherPlanCustomLesson, teacherPlanCustomLesson.Id);
            }

            if (teacherPlanCustomLesson.TeacherPlanCustomLessonDates.Count > 0)
            {
                await _teacherPlanCustomLessonDateRepository.DeleteByAsync(x => x.TeacherPlanCustomLessonId,
                    teacherPlanCustomLesson.Id);
                await SaveTeacherPlanCustomLessonDates(teacherPlanCustomLesson, teacherPlanCustomLesson.Id);
            }
        }

        private async Task SaveTeacherPlanCustomLessonDates(
            TeacherPlanCustomLesson teacherPlanCustomLesson, int id)
        {
            if (teacherPlanCustomLesson == null)
                throw new ArgumentNullException(nameof(teacherPlanCustomLesson), "TeacherPlanCustomLesson cannot be null.");

            if (id <= 0)
                throw new ArgumentException("Invalid TeacherPlanCustomLessonId.", nameof(id));


            if (teacherPlanCustomLesson.TeacherPlanCustomLessonDates.Count > 0)
            {
                foreach (var teacherPlanCustomLessonDate in teacherPlanCustomLesson.TeacherPlanCustomLessonDates)
                {
                    if(teacherPlanCustomLessonDate == null) 
                        continue; // Skip null dates

                    // Validate date
                    DateUtil.ValidateMySqlDate(teacherPlanCustomLessonDate.ImplementationDate);

                    teacherPlanCustomLessonDate.TeacherPlanCustomLessonId = id;
                }

                await _teacherPlanCustomLessonDateRepository.AddAsync(teacherPlanCustomLesson
                    .TeacherPlanCustomLessonDates);
            }
        }

        private async Task SaveTeacherPlanCustomLessonTypes(
            TeacherPlanCustomLesson teacherPlanCustomLesson, int id)
        {
            if (teacherPlanCustomLesson == null)
                throw new ArgumentNullException(nameof(teacherPlanCustomLesson), "TeacherPlanCustomLesson cannot be null.");

            if (id <= 0)
                throw new ArgumentException("Invalid TeacherPlanCustomLessonId.", nameof(id));


            foreach (var teacherPlanCustomLessonType in teacherPlanCustomLesson.TeacherPlanCustomLessonEducationalCriterias)
            {
                if (teacherPlanCustomLessonType == null)
                    continue; 

                teacherPlanCustomLessonType.TeacherPlanCustomLessonId = id;
            }

            if (teacherPlanCustomLesson
                .TeacherPlanCustomLessonEducationalCriterias != null && teacherPlanCustomLesson
                .TeacherPlanCustomLessonEducationalCriterias.Count > 0)
            {
                await _teacherPlanCustomLessonEducationalCriteriaRepository.AddAsync(teacherPlanCustomLesson
                    .TeacherPlanCustomLessonEducationalCriterias);
            }
        
        }

        public async Task RemoveTeacherPlanCustomLesson(int teacherPlanCustomLessonId)
        {
            if (teacherPlanCustomLessonId <= 0)
                throw new ArgumentException("Invalid teacherPlanCustomLessonId");

            // Step 1: Delete related dates (ignore if none found)
            await _teacherPlanCustomLessonDateRepository.DeleteByAsync(
                x => x.TeacherPlanCustomLessonId, teacherPlanCustomLessonId);

            // Step 2: Get associated educational criterias
            var teacherPlanCustomLessonEducationalCriterias =
                await _teacherPlanCustomLessonEducationalCriteriaRepository.GetAllByAsync(
                    x => x.TeacherPlanCustomLessonId, teacherPlanCustomLessonId);

            if (teacherPlanCustomLessonEducationalCriterias != null && teacherPlanCustomLessonEducationalCriterias.Any())
            {       
                await _teacherPlanCustomLessonEducationalCriteriaRepository.DeleteByAsync(
                    x => x.TeacherPlanCustomLessonId, teacherPlanCustomLessonId);
            }

            // Step 3: Delete the main custom lesson
            await _teacherPlanCustomLessonRepository.DeleteAsync(teacherPlanCustomLessonId);
        }

        /// <summary>
        /// To copy the custom lessons from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanCustomLessons(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            // === Basic validations ===
            if (oldTeacherPlan == null)
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old teacher plan cannot be null.");

            if (newTeacherPlan == null)
                throw new ArgumentNullException(nameof(newTeacherPlan), "New teacher plan cannot be null.");


            var customLessonsToCopy = await GetTeacherPlanCustomLessonsByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanLessonCustom in customLessonsToCopy)
            {
                if (teacherPlanLessonCustom == null)
                    continue;

                var newDates = teacherPlanLessonCustom.TeacherPlanCustomLessonDates
                      .Select(x => x.ImplementationDate)
                      .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate), 
                        DateTime.Parse(suggestedCloseDate));
                }  
                
                var customLesson = new TeacherPlanCustomLesson
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    Title = teacherPlanLessonCustom.Title,
                    TeacherPlanCustomLessonDates = newDates.
                        Select(date => new TeacherPlanCustomLessonDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanCustomLessonEducationalCriterias = teacherPlanLessonCustom.TeacherPlanCustomLessonEducationalCriterias
                        .Select(x => new TeacherPlanCustomLessonEducationalCriteria
                        {
                            EducationalCriteriaId = x.EducationalCriteriaId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };
                
                await CreateTeacherPlanCustomLesson(customLesson);
            }
        }
    }
}
