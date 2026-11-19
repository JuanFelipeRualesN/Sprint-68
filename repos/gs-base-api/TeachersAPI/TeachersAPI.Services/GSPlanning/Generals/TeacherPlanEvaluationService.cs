using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanEvaluationService : Service<EvaluationType>
    {
        private readonly Repository<TeacherPlanEvaluation> _teacherPlanEvaluationRepository;
        private readonly Repository<TeacherPlanEvaluationType> _teacherPlanEvaluationTypeRepository;
        private readonly Repository<TeacherPlanEvaluationsDate> _teacherPlanEvaluationsDateRepository;
        private readonly Repository<EvaluationTypeTranslation> _evaluationTypeTranslationRepository;

        public TeacherPlanEvaluationService(
            Repository<EvaluationType> evaluationTypeRepository, 
            Repository<TeacherPlanEvaluation> teacherPlanEvaluationRepository, 
            Repository<TeacherPlanEvaluationType> teacherPlanEvaluationTypeRepository, 
            Repository<TeacherPlanEvaluationsDate> teacherPlanEvaluationsDateRepository,
            Repository<EvaluationTypeTranslation> evaluationTypeTranslationRepository
            ) 
            : base(evaluationTypeRepository)
        {
            _teacherPlanEvaluationRepository = teacherPlanEvaluationRepository;
            _teacherPlanEvaluationTypeRepository = teacherPlanEvaluationTypeRepository;
            _teacherPlanEvaluationsDateRepository = teacherPlanEvaluationsDateRepository;
            _evaluationTypeTranslationRepository = evaluationTypeTranslationRepository;
        }

        public async Task<List<EvaluationType>> GetEvaluationTypes()
        {
           return await GetTypes();
        }

        public async Task<List<EvaluationType>> GetEvaluationTypeTranslations(string langCode = "es")
        {
            var evaluationTypes = await GetTypes();
            var translations = (await _evaluationTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var evaluationType in evaluationTypes)
            {
                var translation = translations.FirstOrDefault(x => x.EvaluationTypeId == evaluationType.Id);
                if (translation != null)
                {
                    evaluationType.Description = translation.Description;
                }
            }
            return evaluationTypes;
        }

        public async Task<EvaluationType> CreateEvaluationType(EvaluationType evaluationType)
        {
            if (evaluationType == null)
            {
                throw new ArgumentNullException(nameof(evaluationType), "Evaluation type cannot be null.");
            }

            // If description is null
            if (string.IsNullOrWhiteSpace(evaluationType.Description))
            {
                throw new ArgumentException("Evaluation type description cannot be null or empty.", nameof(evaluationType.Description));
            }

            var evaluationTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, evaluationType.Description)).ToList();
            
            if (evaluationTypes.Any())
            {
                return evaluationTypes.FirstOrDefault();
            }
            
            var id = await TypeRepository.AddAsync(new EvaluationType()
            {
                Description =  evaluationType.Description,
                ModifiedBy = evaluationType.ModifiedBy,
                Type = evaluationType.Type,
                Custom = EvaluationType.IsCustom
            });

            return await TypeRepository.GetByIdAsync(id);
        }
        
        public async Task<List<TeacherPlanEvaluation>> GetTeacherPlanAEvaluationByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanEvaluations =
                (await _teacherPlanEvaluationRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanEvaluations.Any())
            {
                return new List<TeacherPlanEvaluation>();
            }

            var teacherPLanEvaluationIds = teacherPlanEvaluations.Select(x => x.Id).ToArray();
            
            var teacherPlanEvaluationDates = (await _teacherPlanEvaluationsDateRepository.GetAllInByAsync(x => x.TeacherPlanEvaluationsId,
                teacherPLanEvaluationIds)).ToList();

            var teacherPlanEvaluationTypes = (await _teacherPlanEvaluationTypeRepository.GetAllInByAsync(x => x.TeacherPlanEvaluationId,
                teacherPLanEvaluationIds)).ToList();

            var types =
                (await GetEvaluationTypeTranslations(langCode))
                .ToDictionary(x => x.Id);
            
            foreach (var teacherPlanEvaluation in teacherPlanEvaluations)
            {
                teacherPlanEvaluation.TeacherPlanEvaluationsDates = teacherPlanEvaluationDates
                    .Where(x => x.TeacherPlanEvaluationsId == teacherPlanEvaluation.Id).ToList();

                 var evaluationTypes= teacherPlanEvaluationTypes
                    .Where(x => x.TeacherPlanEvaluationId == teacherPlanEvaluation.Id).ToList();
                 evaluationTypes.ForEach(x => x.EvaluationType = types[x.EvaluationTypeId]);
                 teacherPlanEvaluation.TeacherPlanEvaluationTypes = evaluationTypes;
            }

            return teacherPlanEvaluations;
        }

        public async Task CreateTeacherPlanEvaluation(TeacherPlanEvaluation teacherPlanEvaluation)
        {
            if (teacherPlanEvaluation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanEvaluation), "Teacher plan evaluation cannot be null.");
            }

            var id = await _teacherPlanEvaluationRepository.AddAsync(teacherPlanEvaluation);
            
            if (teacherPlanEvaluation.TeacherPlanEvaluationTypes.Count > 0)
            {
                await SaveTeacherPlanEvaluationTypes(teacherPlanEvaluation, id);
            }

            if (teacherPlanEvaluation.TeacherPlanEvaluationsDates.Count > 0)
            {
                await SaveTeacherPlanEvaluationDates(teacherPlanEvaluation, id);
            }
        }
        
        public async Task UpdateTeacherPlanEvaluation(TeacherPlanEvaluation teacherPlanEvaluation)
        {
            if (teacherPlanEvaluation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanEvaluation), "Teacher plan evaluation cannot be null.");
            }

            if (teacherPlanEvaluation.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanEvaluation)}.{nameof(teacherPlanEvaluation.Id)} property.");
            }
            
            if (teacherPlanEvaluation.TeacherPlanEvaluationTypes.Count > 0)
            {
                await _teacherPlanEvaluationTypeRepository.DeleteByAsync(x => x.TeacherPlanEvaluationId,
                    teacherPlanEvaluation.Id);
                await SaveTeacherPlanEvaluationTypes(teacherPlanEvaluation, teacherPlanEvaluation.Id);
            }

            if (teacherPlanEvaluation.TeacherPlanEvaluationsDates.Count > 0)
            {
                await _teacherPlanEvaluationsDateRepository.DeleteByAsync(x => x.TeacherPlanEvaluationsId,
                    teacherPlanEvaluation.Id);
                await SaveTeacherPlanEvaluationDates(teacherPlanEvaluation, teacherPlanEvaluation.Id);
            }
        }

        private async Task SaveTeacherPlanEvaluationDates(
            TeacherPlanEvaluation teacherPlanEvaluation, int id)
        {
            if(teacherPlanEvaluation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanEvaluation), "Teacher plan evaluation cannot be null.");
            }

            if(teacherPlanEvaluation.TeacherPlanEvaluationsDates == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanEvaluation.TeacherPlanEvaluationsDates), "Teacher plan evaluation dates cannot be null.");
            }
            // If id is 0, it means that the evaluation is not saved yet.
            if (id == 0)
            {
                throw new ArgumentException("Teacher plan evaluation ID cannot be zero.", nameof(id));
            }

            if (teacherPlanEvaluation.TeacherPlanEvaluationsDates.Count > 0)
            {
                foreach (var teacherPlanEvaluationDate in teacherPlanEvaluation.TeacherPlanEvaluationsDates)
                {
                    teacherPlanEvaluationDate.TeacherPlanEvaluationsId = id;
                }

                await _teacherPlanEvaluationsDateRepository.AddAsync(teacherPlanEvaluation
                    .TeacherPlanEvaluationsDates);
            }
        }

        private async Task SaveTeacherPlanEvaluationTypes(
            TeacherPlanEvaluation teacherPlanEvaluation, int id)
        {
            if (teacherPlanEvaluation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanEvaluation), "Teacher plan evaluation cannot be null.");
            }
            if (id == 0)
            {
                throw new ArgumentException("Teacher plan evaluation ID cannot be zero.", nameof(id));
            }
            if (teacherPlanEvaluation.TeacherPlanEvaluationTypes == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanEvaluation.TeacherPlanEvaluationTypes), "Teacher plan evaluation types cannot be null.");
            }

            foreach (var teacherPlanEvaluationType in teacherPlanEvaluation.TeacherPlanEvaluationTypes)
            {
                teacherPlanEvaluationType.TeacherPlanEvaluationId = id;
            }

            await _teacherPlanEvaluationTypeRepository.AddAsync(teacherPlanEvaluation
                .TeacherPlanEvaluationTypes);
        
        }
        
        public async Task RemoveTeacherPlanEvaluation(int teacherPlanEvaluationId)
        {
            if (teacherPlanEvaluationId <= 0)
            {
                throw new ArgumentException("Teacher plan evaluation ID must be greater than zero.", nameof(teacherPlanEvaluationId));
            }

            await _teacherPlanEvaluationsDateRepository.DeleteByAsync(
                x => x.TeacherPlanEvaluationsId, teacherPlanEvaluationId);
            
            await _teacherPlanEvaluationTypeRepository.DeleteByAsync(
                x => x.TeacherPlanEvaluationId, teacherPlanEvaluationId);
            
            await _teacherPlanEvaluationRepository.DeleteAsync(teacherPlanEvaluationId);
        }

        /// <summary>
        /// To copy the evaluations from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanEvaluation(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {

            if (oldTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old teacher plan cannot be null.");
            }
            if (newTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(newTeacherPlan), "New teacher plan cannot be null.");
            }

            var evaluationsToCopy = await GetTeacherPlanAEvaluationByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanEvaluation in evaluationsToCopy)
            {
                var newDates = teacherPlanEvaluation.TeacherPlanEvaluationsDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();


                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var evaluation = new TeacherPlanEvaluation
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanEvaluationsDates = newDates.
                        Select(date => new TeacherPlanEvaluationsDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanEvaluationTypes = teacherPlanEvaluation.TeacherPlanEvaluationTypes
                        .Select(x => new TeacherPlanEvaluationType
                        {
                            EvaluationTypeId = x.EvaluationTypeId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };
                
                await CreateTeacherPlanEvaluation(evaluation);
            }
        }
    }
}