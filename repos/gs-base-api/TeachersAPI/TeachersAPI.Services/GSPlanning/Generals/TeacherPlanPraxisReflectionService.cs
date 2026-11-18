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
    public class TeacherPlanPraxisReflectionService : Service<PraxisReflectionCompletedReason>
    {
        private readonly Repository<TeacherPlanPraxisReflection> _teacherPlanPraxisReflectionRepository;
        private readonly Repository<TeacherPlanPraxisReflectionCompletedReason> _teacherPlanPraxisReflectionCompletedReason;
        private readonly Repository<TeacherPlanPraxisReflectionDate> _teacherPlanPraxisReflectionDateRepository;
        private readonly Repository<PraxisReflectionCompletedReasonTranslation> _praxisReflectionCompletedReasonTranslationRepository;

        public TeacherPlanPraxisReflectionService(
            Repository<PraxisReflectionCompletedReason> praxisReflectionCompletedReasonRepository,
            Repository<TeacherPlanPraxisReflectionCompletedReason> teacherPlanPraxisReflectionCompletedReason, 
            Repository<TeacherPlanPraxisReflection> teacherPlanPraxisReflectionRepository, 
            Repository<TeacherPlanPraxisReflectionDate> teacherPlanPraxisReflectionDateRepository,
            Repository<PraxisReflectionCompletedReasonTranslation> praxisReflectionCompletedReasonTranslationRepository
            )
            : base(praxisReflectionCompletedReasonRepository)
        {
            _teacherPlanPraxisReflectionCompletedReason = teacherPlanPraxisReflectionCompletedReason;
            _teacherPlanPraxisReflectionRepository = teacherPlanPraxisReflectionRepository;
            _teacherPlanPraxisReflectionDateRepository = teacherPlanPraxisReflectionDateRepository;
            _praxisReflectionCompletedReasonTranslationRepository = praxisReflectionCompletedReasonTranslationRepository;
        }

        public async Task<List<PraxisReflectionCompletedReason>> GetPraxisReflectionTypes()
        {
            return await GetTypes();
        }

        public async Task<List<PraxisReflectionCompletedReason>> GetPraxisReflectionTypeTranslations(string langCode = "es")
        {
            var praxisReflectionTypes = await GetTypes();
            var translations = (await _praxisReflectionCompletedReasonTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var praxisReflectionType in praxisReflectionTypes)
            {
                var translation = translations.FirstOrDefault(x => x.PraxisReflectionCompletedReasonId == praxisReflectionType.Id);
                if (translation != null)
                {
                    praxisReflectionType.Description = translation.Description;
                    praxisReflectionType.Name = translation.Name;
                }
            }
            return praxisReflectionTypes;
        }


        public async Task<List<TeacherPlanPraxisReflection>> GetTeacherPlanPraxisReflectionsByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanPraxisReflections =
                (await _teacherPlanPraxisReflectionRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanPraxisReflections.Any())
            {
                return new List<TeacherPlanPraxisReflection>();
            }

            var teacherPLanPraxisReflectionsIds = teacherPlanPraxisReflections.Select(x => x.Id).ToArray();

            var teacherPlanPraxisReflectionDates = (await _teacherPlanPraxisReflectionDateRepository.GetAllInByAsync(x => x.TeacherPlanPraxisReflectionId,
                teacherPLanPraxisReflectionsIds)).ToList();

            var teacherPlanPraxisReflectionCompletedReasons = (await _teacherPlanPraxisReflectionCompletedReason.GetAllInByAsync(x => x.TeacherPlanPraxisReflectionId,
                teacherPLanPraxisReflectionsIds)).ToList();

            var types = (await GetPraxisReflectionTypeTranslations(langCode)).ToDictionary(x => x.Id);

            foreach (var teacherPlanPraxisReflection in teacherPlanPraxisReflections)
            {
                teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates = teacherPlanPraxisReflectionDates
                    .Where(x => x.TeacherPlanPraxisReflectionId == teacherPlanPraxisReflection.Id).ToList();

                var praxisReflectionCompletedReasons = teacherPlanPraxisReflectionCompletedReasons
                    .Where(x => x.TeacherPlanPraxisReflectionId == teacherPlanPraxisReflection.Id)
                    .ToList();
                praxisReflectionCompletedReasons.ForEach(x => x.PraxisReflectionCompletedReason = types[x.PraxisReflectionCompletedReasonId]);

                teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons = praxisReflectionCompletedReasons;

            }

            return teacherPlanPraxisReflections;
        }

        public async Task CreateTeacherPlanPraxisReflection(TeacherPlanPraxisReflection teacherPlanPraxisReflection)
        {

            if (teacherPlanPraxisReflection == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanPraxisReflection), "The teacher plan praxis reflection cannot be null.");
            }

            var id = await _teacherPlanPraxisReflectionRepository.AddAsync(teacherPlanPraxisReflection);

            if (teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons.Count > 0)
            {
                await SaveTeacherPlanPraxisReflectionTypes(teacherPlanPraxisReflection, id);
            }

            if (teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates.Count > 0)
            {
                await SaveTeacherPlanPraxisReflectionDates(teacherPlanPraxisReflection, id);
            }
        }

        public async Task UpdateTeacherPlanPraxisReflection(TeacherPlanPraxisReflection teacherPlanPraxisReflection)
        {
            if (teacherPlanPraxisReflection == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanPraxisReflection), "The teacher plan praxis reflection cannot be null.");
            }

            if (teacherPlanPraxisReflection.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanPraxisReflection)}.{nameof(teacherPlanPraxisReflection.Id)} property.");
            }

            await _teacherPlanPraxisReflectionRepository.UpdateAsync(teacherPlanPraxisReflection);

            await _teacherPlanPraxisReflectionCompletedReason.DeleteByAsync(
                x => x.TeacherPlanPraxisReflectionId, teacherPlanPraxisReflection.Id);

            if (teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons.Count > 0)
            {
                await SaveTeacherPlanPraxisReflectionTypes(teacherPlanPraxisReflection, teacherPlanPraxisReflection.Id);
            }

            await _teacherPlanPraxisReflectionDateRepository.DeleteByAsync(x => x.TeacherPlanPraxisReflectionId,
                teacherPlanPraxisReflection.Id);

            if (teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates.Count > 0)
            {
                await SaveTeacherPlanPraxisReflectionDates(teacherPlanPraxisReflection, teacherPlanPraxisReflection.Id);
            }
        }

        private async Task SaveTeacherPlanPraxisReflectionDates(
            TeacherPlanPraxisReflection teacherPlanPraxisReflection, int id)
        {

            if (teacherPlanPraxisReflection == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanPraxisReflection), "The teacher plan praxis reflection cannot be null.");
            }

            // If id is 0
            if (id == 0)
            {
                throw new ArgumentException(
                                         $"Invalid update. Is missing the {nameof(TeacherPlanPraxisReflection)}.{nameof(teacherPlanPraxisReflection.Id)} property.");
            }


            foreach (var teacherPlanPraxisReflectionDate in teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates)
            {
                teacherPlanPraxisReflectionDate.TeacherPlanPraxisReflectionId = id;
            }

            await _teacherPlanPraxisReflectionDateRepository.AddAsync(teacherPlanPraxisReflection
                .TeacherPlanPraxisReflectionDates);
        }

        private async Task SaveTeacherPlanPraxisReflectionTypes(
            TeacherPlanPraxisReflection teacherPlanPraxisReflection, int id)
        {

            if (teacherPlanPraxisReflection == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanPraxisReflection), "The teacher plan praxis reflection cannot be null.");
            }

            // If id is 0
            if (id == 0)
            {
                throw new ArgumentException( $"Invalid update. Is missing the {nameof(TeacherPlanPraxisReflection)}.{nameof(teacherPlanPraxisReflection.Id)} property.");
            }

            foreach (var teacherPlanPraxisReflectionType in teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons)
            {
                teacherPlanPraxisReflectionType.TeacherPlanPraxisReflectionId = id;
            }

            await _teacherPlanPraxisReflectionCompletedReason.AddAsync(teacherPlanPraxisReflection
                .TeacherPlanPraxisReflectionCompletedReasons);

        }

        public async Task RemoveTeacherPlanPraxisReflection(int teacherPlanPraxisReflectionId)
        {
            // If id is 0 throw an exception
            if (teacherPlanPraxisReflectionId == 0)
            {
                throw new ArgumentException(
                                       $"Invalid delete. Is missing the {nameof(TeacherPlanPraxisReflection)}.{nameof(teacherPlanPraxisReflectionId)} property.");
            }

            await _teacherPlanPraxisReflectionDateRepository.DeleteByAsync(
                x => x.TeacherPlanPraxisReflectionId, teacherPlanPraxisReflectionId);

            await _teacherPlanPraxisReflectionCompletedReason.DeleteByAsync(
                x => x.TeacherPlanPraxisReflectionId, teacherPlanPraxisReflectionId);

            await _teacherPlanPraxisReflectionRepository.DeleteAsync(teacherPlanPraxisReflectionId);
        }

        /// <summary>
        /// To copy the Plan Praxis from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanPraxisReflection(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            if (oldTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(oldTeacherPlan), "The old teacher plan cannot be null.");
            }

            if (newTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(newTeacherPlan), "The new teacher plan cannot be null.");
            }

            var praxisReflectionsToCopy = await GetTeacherPlanPraxisReflectionsByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanPraxisReflection in praxisReflectionsToCopy)
            {
                var newDates =
                    teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var praxisReflection = new TeacherPlanPraxisReflection
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    Detail = teacherPlanPraxisReflection.Detail,
                    TeacherPlanPraxisReflectionDates = newDates.
                        Select(date => new TeacherPlanPraxisReflectionDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanPraxisReflectionCompletedReasons = teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons
                        .Select(x => new TeacherPlanPraxisReflectionCompletedReason
                        {
                            PraxisReflectionCompletedReasonId = x.PraxisReflectionCompletedReasonId,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };

                await CreateTeacherPlanPraxisReflection(praxisReflection);
            }
        }
    }
}
