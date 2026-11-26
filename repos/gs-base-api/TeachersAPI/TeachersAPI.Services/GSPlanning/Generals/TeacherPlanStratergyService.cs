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
    public class TeacherPlanStrategyService : Service<StrategyType>
    {
        private readonly Repository<StrategyCategory> _strategyCategoryRepository;
        private readonly Repository<TeacherPlanStrategy> _teacherPlanStrategyRepository;
        private readonly Repository<TeacherPlanStrategyType> _teacherPlanStrategyTypeRepository;
        private readonly Repository<TeacherPlanStrategyDate> _teacherPlanStrategyDateRepository;
        private readonly Repository<TeacherPlanStrategyCategory> _teacherPlanStrategyCategoryRepository;
        private readonly Repository<StrategyTypeTranslation> _strategyTypeTranslationRepository;
        private readonly Repository<StrategyCategoryTranslation> _strategyCategoryTranslationRepository;

        public TeacherPlanStrategyService(
            Repository<StrategyType> strategyTypeRepository, 
            Repository<TeacherPlanStrategyType> teacherPlanStrategyTypeRepository, 
            Repository<TeacherPlanStrategy> teacherPlanStrategyRepository, 
            Repository<TeacherPlanStrategyDate> teacherPlanStrategyDateRepository, 
            Repository<StrategyCategory> strategyCategoryRepository, 
            Repository<TeacherPlanStrategyCategory> teacherPlanStrategyCategoryRepository,
            Repository<StrategyTypeTranslation> strategyTypeTranslationRepository,
            Repository<StrategyCategoryTranslation> strategyCategoryTranslationRepository
            ) 
            : base(strategyTypeRepository)
        {
            _teacherPlanStrategyTypeRepository = teacherPlanStrategyTypeRepository;
            _teacherPlanStrategyRepository = teacherPlanStrategyRepository;
            _teacherPlanStrategyDateRepository = teacherPlanStrategyDateRepository;
            _strategyCategoryRepository = strategyCategoryRepository;
            _teacherPlanStrategyCategoryRepository = teacherPlanStrategyCategoryRepository;
            _strategyTypeTranslationRepository = strategyTypeTranslationRepository;
            _strategyCategoryTranslationRepository = strategyCategoryTranslationRepository;
        }

        public async Task<List<StrategyType>> GetStrategyTypes()
        {
           return await GetTypes();
        }

        public async Task<List<StrategyType>> GetStrategyTypeTranslations(string langCode = "es")
        {
            var strategyTypes = await GetTypes();
            var translations = (await _strategyTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var strategyType in strategyTypes)
            {
                var translation = translations.FirstOrDefault(x => x.StrategyTypeId == strategyType.Id);
                if (translation != null)
                {
                    strategyType.Description = translation.Description;
                    strategyType.Name = translation.Name;
                }
            }
            return strategyTypes;
        }

        public async Task<List<StrategyCategory>> GetStrategyCategories()
        {
            return (await _strategyCategoryRepository.GetAllAsync()).ToList();
        }

        public async Task<List<StrategyCategory>> GetStrategyCategoryTranslations(string langCode = "es")
        {
            var strategyCategories = (await _strategyCategoryRepository.GetAllAsync()).ToList();
            var translations = (await _strategyCategoryTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var strategyCategory in strategyCategories)
            {
                var translation = translations.FirstOrDefault(x => x.StrategyCategoryId == strategyCategory.Id);
                if (translation != null)
                {
                    strategyCategory.Description = translation.Description;
                    strategyCategory.Name = translation.Name;
                }
            }
            return strategyCategories;

        }

        public async Task<StrategyType> CreateStrategyType(StrategyType strategyType)
        {
            // Validate if not null
            if (strategyType == null)
            {
                throw new ArgumentNullException(nameof(strategyType), "Strategy type cannot be null.");
            }

            var strategyTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, strategyType.Description)).ToList();
            
            if (strategyTypes.Any())
            {
                return strategyTypes.FirstOrDefault();
            }
            
            var id = await TypeRepository.AddAsync(new StrategyType()
            {
                Name = strategyType.Description,
                Description =  strategyType.Description,
                ModifiedBy = strategyType.ModifiedBy,
                Type = MaterialType.IsCustom,
                StrategyCategoryId = strategyType.StrategyCategoryId
            });

            return await TypeRepository.GetByIdAsync(id);
        }
        
        public async Task<List<TeacherPlanStrategy>> GetTeacherPlanStrategyByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanStrategys =
                (await _teacherPlanStrategyRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanStrategys.Any())
            {
                return new List<TeacherPlanStrategy>();
            }
            
            var teacherPLanStrategysIds = teacherPlanStrategys.Select(x => x.Id).ToArray();
            
            var teacherPlanStrategyDates = (await _teacherPlanStrategyDateRepository.GetAllInByAsync(x => x.TeacherPlanStrategyId,
                teacherPLanStrategysIds)).ToList();

            var teacherPlanStrategyTypes = (await _teacherPlanStrategyTypeRepository.GetAllInByAsync(x => x.TeacherPlanStrategyId,
                teacherPLanStrategysIds)).ToList();

            var teacherPlanStrategyCatogories =
                (await _teacherPlanStrategyCategoryRepository.GetAllInByAsync(x => x.TeacherPlanStrategyId,
                    teacherPLanStrategysIds)).ToList();

            var types = (await GetStrategyTypeTranslations(langCode))
                .ToDictionary(x => x.Id);
            
            var categories = (await GetStrategyCategoryTranslations(langCode)).ToDictionary(x => x.Id);
            
            foreach (var teacherPlanStrategy in teacherPlanStrategys)
            {
                teacherPlanStrategy.TeacherPlanStrategyDates = teacherPlanStrategyDates
                    .Where(x => x.TeacherPlanStrategyId == teacherPlanStrategy.Id)
                    .ToList();
                
                var strategyTypes = teacherPlanStrategyTypes
                    .Where(x => x.TeacherPlanStrategyId == teacherPlanStrategy.Id)
                    .ToList();
                strategyTypes.ForEach(x => x.StrategyType = types[x.StrategyTypeId]);
                
                teacherPlanStrategy.TeacherPlanStrategyTypes = strategyTypes;

                var strategyCategories = teacherPlanStrategyCatogories
                    .Where(x => x.TeacherPlanStrategyId == teacherPlanStrategy.Id)
                    .ToList();
                strategyCategories.ForEach(x => x.StrategyCategory = categories[x.StrategyCategoryId]);

                teacherPlanStrategy.TeacherPlanStrategyCategories = strategyCategories;
            }

            return teacherPlanStrategys;
        }

        public async Task CreateTeacherPlanStrategy(TeacherPlanStrategy teacherPlanStrategy)
        {
            if (teacherPlanStrategy == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanStrategy), "Teacher plan strategy cannot be null.");
            }

            var id = await _teacherPlanStrategyRepository.AddAsync(teacherPlanStrategy);
            
            if (teacherPlanStrategy.TeacherPlanStrategyTypes.Count > 0)
            {
                await SaveTeacherPlanStrategyTypes(teacherPlanStrategy, id);
            }

            if (teacherPlanStrategy.TeacherPlanStrategyDates.Count > 0)
            {
                await SaveTeacherPlanStrategyDates(teacherPlanStrategy, id);
            }

            if (teacherPlanStrategy.TeacherPlanStrategyCategories.Count > 0)
            {
                await SaveTeacherPlanStrategyCategories(teacherPlanStrategy, id);
            }
        }
        
        public async Task UpdateTeacherPlanStrategy(TeacherPlanStrategy teacherPlanStrategy)
        {
            if(teacherPlanStrategy == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanStrategy), "Teacher plan strategy cannot be null.");
            }

            if (teacherPlanStrategy.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanStrategy)}.{nameof(teacherPlanStrategy.Id)} property.");
            }
            
            if (teacherPlanStrategy.TeacherPlanStrategyTypes.Count > 0)
            {
                await _teacherPlanStrategyTypeRepository.DeleteByAsync(
                    x => x.TeacherPlanStrategyId, teacherPlanStrategy.Id);
                await SaveTeacherPlanStrategyTypes(teacherPlanStrategy, teacherPlanStrategy.Id);
            }

            if (teacherPlanStrategy.TeacherPlanStrategyDates.Count > 0)
            {
                await _teacherPlanStrategyDateRepository.DeleteByAsync(x => x.TeacherPlanStrategyId,
                    teacherPlanStrategy.Id);
                await SaveTeacherPlanStrategyDates(teacherPlanStrategy, teacherPlanStrategy.Id);
            }
            
            if (teacherPlanStrategy.TeacherPlanStrategyCategories.Count > 0)
            {
                await _teacherPlanStrategyCategoryRepository.DeleteByAsync(x => x.TeacherPlanStrategyId,
                    teacherPlanStrategy.Id);
                await SaveTeacherPlanStrategyCategories(teacherPlanStrategy, teacherPlanStrategy.Id);
            }
        }

        private async Task SaveTeacherPlanStrategyDates(
            TeacherPlanStrategy teacherPlanStrategy, int id)
        {
            if (teacherPlanStrategy.TeacherPlanStrategyDates.Count > 0)
            {
                foreach (var teacherPlanStrategyDate in teacherPlanStrategy.TeacherPlanStrategyDates)
                {
                    teacherPlanStrategyDate.TeacherPlanStrategyId = id;
                }

                await _teacherPlanStrategyDateRepository.AddAsync(teacherPlanStrategy
                    .TeacherPlanStrategyDates);
            }
        }
        
        private async Task SaveTeacherPlanStrategyCategories(
            TeacherPlanStrategy teacherPlanStrategy, int id)
        {
            // Validate if not null
            if (teacherPlanStrategy == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanStrategy), "Teacher plan strategy cannot be null.");
            }

            // Validate if id not 0. 
            if (id == 0)
            {
                throw new ArgumentException("Invalid id for TeacherPlanStrategy.");
            }

            if (teacherPlanStrategy.TeacherPlanStrategyCategories.Count > 0)
            {
                foreach (var teacherPlanStrategyCategory in teacherPlanStrategy.TeacherPlanStrategyCategories)
                {
                    teacherPlanStrategyCategory.TeacherPlanStrategyId = id;
                }

                await _teacherPlanStrategyCategoryRepository.AddAsync(teacherPlanStrategy
                    .TeacherPlanStrategyCategories);
            }
        }

        private async Task SaveTeacherPlanStrategyTypes(
            TeacherPlanStrategy teacherPlanStrategy, int id)
        {
            // Validate if not null
            if (teacherPlanStrategy == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanStrategy), "Teacher plan strategy cannot be null.");
            }

            // Validate if id not 0. 
            if (id == 0)
            {
                throw new ArgumentException("Invalid id for TeacherPlanStrategy.");
            }

            foreach (var teacherPlanStrategyType in teacherPlanStrategy.TeacherPlanStrategyTypes)
            {
                teacherPlanStrategyType.TeacherPlanStrategyId = id;
            }

            await _teacherPlanStrategyTypeRepository.AddAsync(teacherPlanStrategy
                .TeacherPlanStrategyTypes);
        
        }
        
        public async Task RemoveTeacherPlanStrategy(int teacherPlanStrategyId)
        {
            // Validate if id not 0. 
            if (teacherPlanStrategyId == 0)
            {
                throw new ArgumentException("Invalid id for teacherPlanStrategyId.");
            }

            await _teacherPlanStrategyDateRepository.DeleteByAsync(
                x => x.TeacherPlanStrategyId, teacherPlanStrategyId);
            
            await _teacherPlanStrategyTypeRepository.DeleteByAsync(
                x => x.TeacherPlanStrategyId, teacherPlanStrategyId);
            
            await _teacherPlanStrategyCategoryRepository.DeleteByAsync(x => x.TeacherPlanStrategyId,
                teacherPlanStrategyId);
            
            await _teacherPlanStrategyRepository.DeleteAsync(teacherPlanStrategyId);
        }

        /// <summary>
        /// To copy the Plan Strategy from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanStrategy(TeacherPlan oldTeacherPlan,
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

            var strategiesToCopy = await GetTeacherPlanStrategyByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanStrategy in strategiesToCopy)
            {
                var newDates = teacherPlanStrategy.TeacherPlanStrategyDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var strategy = new TeacherPlanStrategy
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanStrategyDates = newDates.
                        Select(date => new TeacherPlanStrategyDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanStrategyTypes = teacherPlanStrategy.TeacherPlanStrategyTypes
                        .Select(x => new TeacherPlanStrategyType
                        {
                            StrategyTypeId = x.StrategyTypeId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanStrategyCategories = teacherPlanStrategy.TeacherPlanStrategyCategories
                        .Select(x => new TeacherPlanStrategyCategory
                        {
                            StrategyCategoryId = x.StrategyCategoryId,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        }).ToList()
                };
                
                await CreateTeacherPlanStrategy(strategy);
            }
        }
    }
}
