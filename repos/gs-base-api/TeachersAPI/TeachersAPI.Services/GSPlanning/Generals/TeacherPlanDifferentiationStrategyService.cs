using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Models;
using GSPlanning.Services.Utilities;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanDifferentiationStrategyService : Service<DifferentiationStrategyType>
    {
        private readonly Repository<DifferentiationStrategyCategory> _differentiationStrategyCategoryRepository;
        private readonly Repository<TeacherPlanDifferentiationStrategyType> _teacherPlanDifferentiationStrategyTypeRepository;
        private readonly Repository<TeacherPlanDifferentiationStrategyDate> _teacherPlanDifferentiationStrategyDateRepository;
        private readonly Repository<TeacherPlanDifferentiationStrategy> _teacherPlanDifferentiationStrategyRepository;
        private readonly Repository<TeacherPlanDifferentiationStrategyCategory> _teacherPlanDifferentiationStrategyCategoryRepository;
        private readonly Repository<DifferentiationStrategyTypeTranslation> _differentiationStrategyTypeTranslationRepository;
        private readonly Repository<DifferentiationStrategyCategoryTranslation> _differentiationStrategyCategoryTranslationRepository;

        public TeacherPlanDifferentiationStrategyService(
            Repository<DifferentiationStrategyCategory> differentiationStrategyCategoryRepository,
            Repository<DifferentiationStrategyType> differentiationStrategyTypeRepository,
            Repository<TeacherPlanDifferentiationStrategyType> teacherPlanDifferentiationStrategyTypeRepository,
            Repository<TeacherPlanDifferentiationStrategyDate> teacherPlanDifferentiationStrategyDateRepository,
            Repository<TeacherPlanDifferentiationStrategy> teacherPlanDifferentiationStrategyRepository,
            Repository<TeacherPlanDifferentiationStrategyCategory> teacherPlanDifferentiationStrategyCategoryRepository,
            Repository<DifferentiationStrategyTypeTranslation> differentiationStrategyTypeTranslationRepository,
            Repository<DifferentiationStrategyCategoryTranslation> differentiationStrategyCategoryTranslationRepository
        ) : base(differentiationStrategyTypeRepository)
        {
            _differentiationStrategyCategoryRepository = differentiationStrategyCategoryRepository;
            _teacherPlanDifferentiationStrategyTypeRepository = teacherPlanDifferentiationStrategyTypeRepository;
            _teacherPlanDifferentiationStrategyDateRepository = teacherPlanDifferentiationStrategyDateRepository;
            _teacherPlanDifferentiationStrategyRepository = teacherPlanDifferentiationStrategyRepository;
            _teacherPlanDifferentiationStrategyCategoryRepository = teacherPlanDifferentiationStrategyCategoryRepository;
            _differentiationStrategyTypeTranslationRepository = differentiationStrategyTypeTranslationRepository;
            _differentiationStrategyCategoryTranslationRepository = differentiationStrategyCategoryTranslationRepository;
        }

        public async Task<List<DifferentiationStrategyType>> GetDifferentiationStrategyTypes()
        {
            return await GetTypes();
        }

        public async Task<List<DifferentiationStrategyType>> GetDifferentiationStrategyTypeTranslations(string langCode = "es")
        {
            var differentiationStrategyTypes = await GetTypes();
            var translations = (await _differentiationStrategyTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var differentiationStrategyType in differentiationStrategyTypes)
            {
                var translation = translations.FirstOrDefault(x => x.DifferentiationStrategyTypeId == differentiationStrategyType.Id);
                if (translation != null)
                {
                    differentiationStrategyType.Description = translation.Description;
                    differentiationStrategyType.Name = translation.Name;
                }
            }
            return differentiationStrategyTypes;
        }

        public async Task<List<DifferentiationStrategyCategory>> GetDifferentiationStrategyCategories()
        {
            return (await _differentiationStrategyCategoryRepository.GetAllAsync()).ToList();
        }

        public async Task<List<DifferentiationStrategyCategory>> GetDifferentiationStrategyCategoryTranslations(string langCode = "es")
        {
            var differentiationStrategyCategories = (await _differentiationStrategyCategoryRepository.GetAllAsync()).ToList();
            var translations = (await _differentiationStrategyCategoryTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var differentiationStrategyCategory in differentiationStrategyCategories)
            {
                var translation = translations.FirstOrDefault(x => x.DifferentiationStrategyCategoryId == differentiationStrategyCategory.Id);
                if (translation != null)
                {
                    differentiationStrategyCategory.Description = translation.Description;
                    differentiationStrategyCategory.Name = translation.Name;
                }
            }
            return differentiationStrategyCategories;

        }

        public async Task<DifferentiationStrategyType> CreateDifferentiationStrategyType(DifferentiationStrategyType differentiationStrategyType)
        {
            if (differentiationStrategyType == null)
                throw new ArgumentNullException(nameof(differentiationStrategyType), "DifferentiationStrategyType cannot be null.");

            if (string.IsNullOrWhiteSpace(differentiationStrategyType.Description))
                throw new ArgumentException("Description is required.", nameof(differentiationStrategyType.Description));

            var differentiationStrategiesTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, differentiationStrategyType.Description)).ToList();

            if (differentiationStrategiesTypes.Any())
            {
                return differentiationStrategiesTypes.FirstOrDefault();
            }

            var id = await TypeRepository.AddAsync(new DifferentiationStrategyType()
            {
                Name = differentiationStrategyType.Description,
                Description = differentiationStrategyType.Description,
                ModifiedBy = differentiationStrategyType.ModifiedBy,
                Type = differentiationStrategyType.Type,
                DifferentiationStrategyCategoryId = 5
            });

            return await TypeRepository.GetByIdAsync(id);
        }

        public async Task<List<TeacherPlanDifferentiationStrategy>> GetTeacherPlanDifferentiationStrategiesByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanDifferentiationStrategies =
                (await _teacherPlanDifferentiationStrategyRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanDifferentiationStrategies.Any())
            {
                return new List<TeacherPlanDifferentiationStrategy>();
            }

            var teacherPLanDifferentiationStrategiesIds = teacherPlanDifferentiationStrategies.Select(x => x.Id).ToArray();

            var teacherPlanDifferentiationStrategyDates = (await _teacherPlanDifferentiationStrategyDateRepository.GetAllInByAsync(x => x.TeacherPlanDifferentiationStrategyId,
                teacherPLanDifferentiationStrategiesIds)).ToList();

            var teacherPlanDifferentiationStrategyTypes = (await _teacherPlanDifferentiationStrategyTypeRepository.GetAllInByAsync(x => x.TeacherPlanDifferentiationStrategyId,
                teacherPLanDifferentiationStrategiesIds)).ToList();

            var teacherPlanDifferentiationStrategyCatogories = (await _teacherPlanDifferentiationStrategyCategoryRepository.GetAllInByAsync(
                x => x.TeacherPlanDifferentiationStrategyId, teacherPLanDifferentiationStrategiesIds)).ToList();

            var types = (await GetDifferentiationStrategyTypeTranslations(langCode)).ToDictionary(x => x.Id);

            var categories = (await GetDifferentiationStrategyCategoryTranslations(langCode)).ToDictionary(x => x.Id);

            foreach (var teacherPlanDifferentiationStrategy in teacherPlanDifferentiationStrategies)
            {
                teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates = teacherPlanDifferentiationStrategyDates
                    .Where(x => x.TeacherPlanDifferentiationStrategyId == teacherPlanDifferentiationStrategy.Id).ToList();

                var differentiationStrategyTypes = teacherPlanDifferentiationStrategyTypes
                    .Where(x => x.TeacherPlanDifferentiationStrategyId == teacherPlanDifferentiationStrategy.Id)
                    .ToList();
                differentiationStrategyTypes.ForEach(x => x.DifferentiationStrategyType = types[x.DifferentiationStrategyTypesId]);

                teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes = differentiationStrategyTypes;

                var differentiationStrategyCategories = teacherPlanDifferentiationStrategyCatogories
                    .Where(x => x.TeacherPlanDifferentiationStrategyId == teacherPlanDifferentiationStrategy.Id)
                    .ToList();
                differentiationStrategyCategories.ForEach(x => x.DifferentiationStrategyCategory = categories[x.DifferentiationStrategyCategoryId]);

                teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyCategories = differentiationStrategyCategories;

            }

            return teacherPlanDifferentiationStrategies;
        }

        public async Task CreateTeacherPlanDifferentiationStrategy(TeacherPlanDifferentiationStrategy teacherPlanDifferentiationStrategy)
        {
            if (teacherPlanDifferentiationStrategy == null)
                throw new ArgumentNullException(nameof(teacherPlanDifferentiationStrategy), "TeacherPlanDifferentiationStrategy cannot be null.");

            var id = await _teacherPlanDifferentiationStrategyRepository.AddAsync(teacherPlanDifferentiationStrategy);

            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes.Count > 0)
            {
                await SaveTeacherPlanDifferentiationStrategyTypes(teacherPlanDifferentiationStrategy, id);
            }

            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates.Count > 0)
            {
                await SaveTeacherPlanDifferentiationStrategyDates(teacherPlanDifferentiationStrategy, id);
            }
            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyCategories.Count > 0)
            {
                await SaveTeacherPlanDifferentiationStrategyCategories(teacherPlanDifferentiationStrategy, id);
            }
        }

        public async Task UpdateTeacherPlanDifferentiationStrategy(TeacherPlanDifferentiationStrategy teacherPlanDifferentiationStrategy)
        {
            if (teacherPlanDifferentiationStrategy == null)
                throw new ArgumentNullException(nameof(teacherPlanDifferentiationStrategy), "TeacherPlanDifferentiationStrategy cannot be null.");

            if (teacherPlanDifferentiationStrategy.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanDifferentiationStrategy)}.{nameof(teacherPlanDifferentiationStrategy.Id)} property.");
            }

            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes.Count > 0)
            {
                await _teacherPlanDifferentiationStrategyTypeRepository.DeleteByAsync(
                    x => x.TeacherPlanDifferentiationStrategyId, teacherPlanDifferentiationStrategy.Id);
                await SaveTeacherPlanDifferentiationStrategyTypes(teacherPlanDifferentiationStrategy, teacherPlanDifferentiationStrategy.Id);
            }

            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates.Count > 0)
            {
                await _teacherPlanDifferentiationStrategyDateRepository.DeleteByAsync(x => x.TeacherPlanDifferentiationStrategyId,
                    teacherPlanDifferentiationStrategy.Id);
                await SaveTeacherPlanDifferentiationStrategyDates(teacherPlanDifferentiationStrategy, teacherPlanDifferentiationStrategy.Id);
            }
            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyCategories.Count > 0)
            {
                await _teacherPlanDifferentiationStrategyCategoryRepository.DeleteByAsync(x => x.TeacherPlanDifferentiationStrategyId,
                    teacherPlanDifferentiationStrategy.Id);
                await SaveTeacherPlanDifferentiationStrategyCategories(teacherPlanDifferentiationStrategy, teacherPlanDifferentiationStrategy.Id);
            }
        }

        private async Task SaveTeacherPlanDifferentiationStrategyDates(
            TeacherPlanDifferentiationStrategy teacherPlanDifferentiationStrategy, int id)
        {
            if (teacherPlanDifferentiationStrategy == null)
                throw new ArgumentNullException(nameof(teacherPlanDifferentiationStrategy), "TeacherPlanDifferentiationStrategy cannot be null.");

            if (teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates.Count > 0)
            {
                foreach (var teacherPlanDifferentiationStrategyDate in teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates)
                {
                    if (teacherPlanDifferentiationStrategyDate == null)
                        continue;

                    teacherPlanDifferentiationStrategyDate.TeacherPlanDifferentiationStrategyId = id;
                    DateUtil.ValidateMySqlDate(teacherPlanDifferentiationStrategyDate.ImplementationDate);
                }

                await _teacherPlanDifferentiationStrategyDateRepository.AddAsync(teacherPlanDifferentiationStrategy
                    .TeacherPlanDifferentiationStrategyDates);
            }
        }

        private async Task SaveTeacherPlanDifferentiationStrategyCategories(
            TeacherPlanDifferentiationStrategy teacherDifferentiationPlanStrategy, int id)
        {
            // Validate if not null
            if (teacherDifferentiationPlanStrategy == null)
            {
                throw new ArgumentNullException(nameof(teacherDifferentiationPlanStrategy), "Teacher plan strategy cannot be null.");
            }

            // Validate if id not 0. 
            if (id == 0)
            {
                throw new ArgumentException("Invalid id for TeacherPlanStrategy.");
            }

            if (teacherDifferentiationPlanStrategy.TeacherPlanDifferentiationStrategyCategories.Count > 0)
            {
                foreach (var teacherPlanStrategyCategory in teacherDifferentiationPlanStrategy.TeacherPlanDifferentiationStrategyCategories)
                {
                    teacherPlanStrategyCategory.TeacherPlanDifferentiationStrategyId = id;
                }

                await _teacherPlanDifferentiationStrategyCategoryRepository.AddAsync(teacherDifferentiationPlanStrategy
                    .TeacherPlanDifferentiationStrategyCategories);
            }
        }

        private async Task SaveTeacherPlanDifferentiationStrategyTypes(
            TeacherPlanDifferentiationStrategy teacherPlanDifferentiationStrategy, int id)
        {
            if (teacherPlanDifferentiationStrategy == null)
                throw new ArgumentNullException(nameof(teacherPlanDifferentiationStrategy), "TeacherPlanDifferentiationStrategy cannot be null.");

            if (id <= 0)
                throw new ArgumentException("Invalid id.", nameof(id));

            foreach (var teacherPlanDifferentiationStrategyType in teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes)
            {
                teacherPlanDifferentiationStrategyType.TeacherPlanDifferentiationStrategyId = id;
            }

            await _teacherPlanDifferentiationStrategyTypeRepository.AddAsync(teacherPlanDifferentiationStrategy
                .TeacherPlanDifferentiationStrategyTypes);

        }

        public async Task RemoveTeacherPlanDifferentiationStrategy(int teacherPlanDifferentiationStrategyId)
        {
            if (teacherPlanDifferentiationStrategyId <= 0)
                throw new ArgumentException("Invalid id.", nameof(teacherPlanDifferentiationStrategyId));

            await _teacherPlanDifferentiationStrategyDateRepository.DeleteByAsync(
                x => x.TeacherPlanDifferentiationStrategyId, teacherPlanDifferentiationStrategyId);

            await _teacherPlanDifferentiationStrategyTypeRepository.DeleteByAsync(
                x => x.TeacherPlanDifferentiationStrategyId, teacherPlanDifferentiationStrategyId);

            await _teacherPlanDifferentiationStrategyRepository.DeleteAsync(teacherPlanDifferentiationStrategyId);
        }

        /// <summary>
        /// To copy the accommodations from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanDifferentiationStrategy(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            if (oldTeacherPlan == null)
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old teacher plan cannot be null.");

            if (newTeacherPlan == null)
                throw new ArgumentNullException(nameof(newTeacherPlan), "New teacher plan cannot be null.");

            var differentiationStrategiesToCopy = await GetTeacherPlanDifferentiationStrategiesByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanDifferentiationStrategy in differentiationStrategiesToCopy)
            {
                var newDates = teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));

                    // Validate the new dates for MySQL compatibility
                    foreach (var date in newDates)
                    {
                        DateUtil.ValidateMySqlDate(date);
                    }
                }

                var differentiationStrategy = new TeacherPlanDifferentiationStrategy
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanDifferentiationStrategyDates = newDates.
                        Select(date => new TeacherPlanDifferentiationStrategyDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanDifferentiationStrategyTypes = teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes
                        .Select(x => new TeacherPlanDifferentiationStrategyType
                        {
                            DifferentiationStrategyTypesId = x.DifferentiationStrategyTypesId,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanDifferentiationStrategyCategories = teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyCategories
                        .Select(x => new TeacherPlanDifferentiationStrategyCategory
                        {
                            TeacherPlanDifferentiationStrategyId = x.TeacherPlanDifferentiationStrategyId,
                            DifferentiationStrategyCategoryId = x.DifferentiationStrategyCategoryId,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };

                await CreateTeacherPlanDifferentiationStrategy(differentiationStrategy);
            }
        }
    }
}
