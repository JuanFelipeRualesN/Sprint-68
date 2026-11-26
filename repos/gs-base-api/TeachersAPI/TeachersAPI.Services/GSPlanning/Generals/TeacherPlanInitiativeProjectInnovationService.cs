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
    public class TeacherPlanInitiativeProjectInnovationService : Service<InitiativeProjectInnovationType>
    {
        private readonly Repository<TeacherPlanInitiativeProjectInnovation> _teacherPlanInitiativeProjectInnovationRepository;
        private readonly Repository<TeacherPlanInitiativeProjectInnovationType> _teacherPlanInitiativeProjectInnovationTypeRepository;
        private readonly Repository<TeacherPlanInitiativeProjectInnovationDate> _teacherPlanInitiativeProjectInnovationDateRepository;
        private readonly Repository<InitiativeProjectInnovationTypeTranslation> _initiativeProjectInnovationTypeTranslationRepository;

        public TeacherPlanInitiativeProjectInnovationService(
            Repository<InitiativeProjectInnovationType> initiativeProjectInnovationTypeRepository, 
            Repository<TeacherPlanInitiativeProjectInnovation> teacherPlanInitiativeProjectInnovationRepository, 
            Repository<TeacherPlanInitiativeProjectInnovationType> teacherPlanInitiativeProjectInnovationTypeRepository, 
            Repository<TeacherPlanInitiativeProjectInnovationDate> teacherPlanInitiativeProjectInnovationDateRepository,
            Repository<InitiativeProjectInnovationTypeTranslation> initiativeProjectInnovationTypeTranslationRepository
            ) 
            : base(initiativeProjectInnovationTypeRepository)
        {
            _teacherPlanInitiativeProjectInnovationRepository = teacherPlanInitiativeProjectInnovationRepository;
            _teacherPlanInitiativeProjectInnovationTypeRepository = teacherPlanInitiativeProjectInnovationTypeRepository;
            _teacherPlanInitiativeProjectInnovationDateRepository = teacherPlanInitiativeProjectInnovationDateRepository;
            _initiativeProjectInnovationTypeTranslationRepository = initiativeProjectInnovationTypeTranslationRepository;
        }

        public async Task<List<InitiativeProjectInnovationType>> GetInitiativeProjectInnovationTypes()
        {
           return await GetTypes();
        }

        public async Task<List<InitiativeProjectInnovationType>> GetInitiativeProjectInnovationTypeTranslations(string langCode = "es")
        {
            var initiativeProjectInnovationTypes = await GetTypes();
            var translations = (await _initiativeProjectInnovationTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var initiativeProjectInnovationType in initiativeProjectInnovationTypes)
            {
                var translation = translations.FirstOrDefault(x => x.InitiativeProjectInnovationTypeId == initiativeProjectInnovationType.Id);
                if (translation != null)
                {
                    initiativeProjectInnovationType.Description = translation.Description;
                }
            }
            return initiativeProjectInnovationTypes;
        }

        public async Task<InitiativeProjectInnovationType> CreateInitiativeProjectInnovationType(InitiativeProjectInnovationType initiativeProjectInnovationType)
        {
            if(initiativeProjectInnovationType == null)
            {
                throw new ArgumentNullException(nameof(initiativeProjectInnovationType), "The initiative project innovation type cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(initiativeProjectInnovationType.Description))
            {
                throw new ArgumentException("The initiative project innovation type description cannot be null or empty.", nameof(initiativeProjectInnovationType.Description));
            }

            if (initiativeProjectInnovationType.Type == InitiativeProjectInnovationType.Default)
            {
                throw new ArgumentException(
                    $"Invalid type {nameof(initiativeProjectInnovationType.Type)} {initiativeProjectInnovationType.Type}");
            }
            
            var initiativeProjectInnovationTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, initiativeProjectInnovationType.Description))
                .Where(x => x.Type == initiativeProjectInnovationType.Type)
                .ToList();
            
            if (initiativeProjectInnovationTypes.Any())
            {
                return initiativeProjectInnovationTypes.FirstOrDefault();
            }
            
            var id = await TypeRepository.AddAsync(new InitiativeProjectInnovationType()
            {
                Description =  initiativeProjectInnovationType.Description,
                ModifiedBy = initiativeProjectInnovationType.ModifiedBy,
                Type = InitiativeProjectInnovationType.Custom
            });

            return await TypeRepository.GetByIdAsync(id);
        }

        public async Task<List<TeacherPlanInitiativeProjectInnovation>> GetTeacherPlanInitiativeProjectInnovationByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanInitiativeProjectInnovations =
                (await _teacherPlanInitiativeProjectInnovationRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();
            
            if (!teacherPlanInitiativeProjectInnovations.Any())
            {
                return new List<TeacherPlanInitiativeProjectInnovation>();
            }
            
            var teacherPLanInitiativeProjectInnovationIds = teacherPlanInitiativeProjectInnovations.Select(x => x.Id).ToArray();
            
            var teacherPlanInitiativeProjectInnovationDates = (await _teacherPlanInitiativeProjectInnovationDateRepository.GetAllInByAsync(x => x.TeacherPlanInitiativeProjectInnovationId,
                teacherPLanInitiativeProjectInnovationIds)).ToList();

            var teacherPlanInitiativeProjectInnovationTypes = (await _teacherPlanInitiativeProjectInnovationTypeRepository.GetAllInByAsync(x => x.TeacherPlanInitiativeProjectInnovationId,
                teacherPLanInitiativeProjectInnovationIds)).ToList();

            var types = (await GetInitiativeProjectInnovationTypeTranslations(langCode))
                .ToDictionary(x => x.Id);
            
            foreach (var teacherPlanInitiativeProjectInnovation in teacherPlanInitiativeProjectInnovations)
            {
                teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates = teacherPlanInitiativeProjectInnovationDates
                    .Where(x => x.TeacherPlanInitiativeProjectInnovationId == teacherPlanInitiativeProjectInnovation.Id).ToList();

                 var initiativeProjectInnovationTypes = teacherPlanInitiativeProjectInnovationTypes
                    .Where(x => x.TeacherPlanInitiativeProjectInnovationId == teacherPlanInitiativeProjectInnovation.Id).ToList();

                 initiativeProjectInnovationTypes.ForEach(x => x.InitiativeProjectInnovationType = types[x.InitiativeProjectInnovationTypeId]);
                 teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes =
                     initiativeProjectInnovationTypes;
            }

            return teacherPlanInitiativeProjectInnovations;
        }

        public async Task CreateTeacherPlanInitiativeProjectInnovation(TeacherPlanInitiativeProjectInnovation teacherPlanInitiativeProjectInnovation)
        {
            if (teacherPlanInitiativeProjectInnovation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanInitiativeProjectInnovation), "The teacher plan initiative project innovation cannot be null.");
            }

            var id = await _teacherPlanInitiativeProjectInnovationRepository.AddAsync(teacherPlanInitiativeProjectInnovation);
            
            if (teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes.Count > 0)
            {
                await SaveTeacherPlanInitiativeProjectInnovationTypes(teacherPlanInitiativeProjectInnovation, id);
            }

            if (teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates.Count > 0)
            {
                await SaveTeacherPlanInitiativeProjectInnovationDates(teacherPlanInitiativeProjectInnovation, id);
            }
        }
        
        public async Task UpdateTeacherPlanInitiativeProjectInnovation(TeacherPlanInitiativeProjectInnovation teacherPlanInitiativeProjectInnovation)
        {
            if (teacherPlanInitiativeProjectInnovation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanInitiativeProjectInnovation), "The teacher plan initiative project innovation cannot be null.");
            }

            if (teacherPlanInitiativeProjectInnovation.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanInitiativeProjectInnovation)}.{nameof(teacherPlanInitiativeProjectInnovation.Id)} property.");
            }
            
            if (teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes.Count > 0)
            {
                await _teacherPlanInitiativeProjectInnovationTypeRepository.DeleteByAsync(
                    x => x.TeacherPlanInitiativeProjectInnovationId, teacherPlanInitiativeProjectInnovation.Id);
                await SaveTeacherPlanInitiativeProjectInnovationTypes(teacherPlanInitiativeProjectInnovation, teacherPlanInitiativeProjectInnovation.Id);
            }

            if (teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates.Count > 0)
            {
                await _teacherPlanInitiativeProjectInnovationDateRepository.DeleteByAsync(
                    x => x.TeacherPlanInitiativeProjectInnovationId, teacherPlanInitiativeProjectInnovation.Id);
                await SaveTeacherPlanInitiativeProjectInnovationDates(teacherPlanInitiativeProjectInnovation, teacherPlanInitiativeProjectInnovation.Id);
            }
        }

        private async Task SaveTeacherPlanInitiativeProjectInnovationDates(
            TeacherPlanInitiativeProjectInnovation teacherPlanInitiativeProjectInnovation, int id)
        {
            if(teacherPlanInitiativeProjectInnovation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanInitiativeProjectInnovation), "The teacher plan initiative project innovation cannot be null.");
            }

            // If id is 0...
            if (id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanInitiativeProjectInnovation)}.{nameof(teacherPlanInitiativeProjectInnovation.Id)} property.");
            }
            
            if (teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates.Count > 0)
            {
                foreach (var teacherPlanInitiativeProjectInnovationDate in teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates)
                {
                    if(teacherPlanInitiativeProjectInnovationDate == null)
                    {
                        continue;
                    }
                    DateUtil.ValidateMySqlDate(teacherPlanInitiativeProjectInnovationDate.ImplementationDate);
                    teacherPlanInitiativeProjectInnovationDate.TeacherPlanInitiativeProjectInnovationId = id;
                }

                await _teacherPlanInitiativeProjectInnovationDateRepository.AddAsync(teacherPlanInitiativeProjectInnovation
                    .TeacherPlanInitiativeProjectInnovationDates);
            }
        }

        private async Task SaveTeacherPlanInitiativeProjectInnovationTypes(
            TeacherPlanInitiativeProjectInnovation teacherPlanInitiativeProjectInnovation, int id)
        {
            if (teacherPlanInitiativeProjectInnovation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanInitiativeProjectInnovation), "The teacher plan initiative project innovation cannot be null.");
            }

            // If id is 0...
            if (id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanInitiativeProjectInnovation)}.{nameof(teacherPlanInitiativeProjectInnovation.Id)} property.");
            }

            foreach (var teacherPlanInitiativeProjectInnovationType in teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes)
            {
                teacherPlanInitiativeProjectInnovationType.TeacherPlanInitiativeProjectInnovationId = id;
            }

            await _teacherPlanInitiativeProjectInnovationTypeRepository.AddAsync(teacherPlanInitiativeProjectInnovation
                .TeacherPlanInitiativeProjectInnovationTypes);
        
        }
        
        public async Task RemoveTeacherPlanInitiativeProjectInnovation(int teacherPlanInitiativeProjectInnovationId)
        {
            // If id is 0...
            if (teacherPlanInitiativeProjectInnovationId == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(teacherPlanInitiativeProjectInnovationId)} property.");
            }

            await _teacherPlanInitiativeProjectInnovationDateRepository.DeleteByAsync(
                x => x.TeacherPlanInitiativeProjectInnovationId, teacherPlanInitiativeProjectInnovationId);
            
            await _teacherPlanInitiativeProjectInnovationTypeRepository.DeleteByAsync(
                x => x.TeacherPlanInitiativeProjectInnovationId, teacherPlanInitiativeProjectInnovationId);
            
            await _teacherPlanInitiativeProjectInnovationRepository.DeleteAsync(teacherPlanInitiativeProjectInnovationId);
        }

        /// <summary>
        /// To copy the Initiative Project Innovation from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanInitiativeProjectInnovation(TeacherPlan oldTeacherPlan,
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

            var initiativeProjectInnovationsToCopy = await GetTeacherPlanInitiativeProjectInnovationByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanInitiativeProjectInnovation in initiativeProjectInnovationsToCopy)
            {
                var newDates = teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var initiativeProjectInnovation = new TeacherPlanInitiativeProjectInnovation
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanInitiativeProjectInnovationDates = newDates.
                        Select(date => new TeacherPlanInitiativeProjectInnovationDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanInitiativeProjectInnovationTypes = teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes
                        .Select(x => new TeacherPlanInitiativeProjectInnovationType
                        {
                            InitiativeProjectInnovationTypeId = x.InitiativeProjectInnovationTypeId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };
                
                await CreateTeacherPlanInitiativeProjectInnovation(initiativeProjectInnovation);
            }
        }
    }
}