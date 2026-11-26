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
    public class TeacherPlanActiveResponseService : Service<ActiveResponseType>
    {
        private readonly Repository<TeacherPlanActiveResponseType> _teacherPlanActiveResponseTypeRepository;
        private readonly Repository<TeacherPlanActiveResponseDate> _teacherPlanActiveResponseDateRepository;
        private readonly Repository<TeacherPlanActiveResponse> _teacherPlanActiveResponseRepository;
        private readonly Repository<TeacherPlanActiveResponseInclude> _teacherPlanActiveResponseIncludeRepository;
        private readonly Repository<ActiveResponseTypeTranslation> _activeResponseTypeTranslationRepository;

        public TeacherPlanActiveResponseService(
            Repository<ActiveResponseType> activeResponseTypeRepository,
            Repository<TeacherPlanActiveResponseType> teacherPlanActiveResponseTypeRepository,
            Repository<TeacherPlanActiveResponseDate> teacherPlanActiveResponseDateRepository,
            Repository<TeacherPlanActiveResponse> teacherPlanActiveResponseRepository,
            Repository<TeacherPlanActiveResponseInclude> teacherPlanActiveResponseIncludeRepository,
            Repository<ActiveResponseTypeTranslation> activeResponseTypeTranslationRepository
        ) : base(activeResponseTypeRepository)
        {
            _teacherPlanActiveResponseTypeRepository = teacherPlanActiveResponseTypeRepository;
            _teacherPlanActiveResponseDateRepository = teacherPlanActiveResponseDateRepository;
            _teacherPlanActiveResponseRepository = teacherPlanActiveResponseRepository;
            _teacherPlanActiveResponseIncludeRepository = teacherPlanActiveResponseIncludeRepository;
            _activeResponseTypeTranslationRepository = activeResponseTypeTranslationRepository;
        }

        public async Task<List<ActiveResponseType>> GetActiveResponseTypes()
        {
            return await GetTypes();
        }

        public async Task<List<ActiveResponseType>> GetActiveResponseTypeTranslations(string langCode = "es")
        {
            var activeResponseTypes = await GetTypes();
            var translations = (await _activeResponseTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var activeResponseType in activeResponseTypes)
            {
                var translation = translations.FirstOrDefault(x => x.ActiveResponseTypeId == activeResponseType.Id);
                if (translation != null)
                {
                    activeResponseType.Description = translation.Description;
                    activeResponseType.Name = translation.Name;
                }
            }
            return activeResponseTypes;
        }


        public async Task<List<TeacherPlanActiveResponseInclude>> GetTeacherPlanActiveResponseIncludes()
        {
            return (await _teacherPlanActiveResponseIncludeRepository.GetAllAsync()).ToList();
        }

        public async Task<ActiveResponseType> CreateActiveResponseType(ActiveResponseType activeResponseType)
        {
            if (activeResponseType == null)
                throw new ArgumentNullException(nameof(activeResponseType), "ActiveResponseType cannot be null.");

            if (string.IsNullOrWhiteSpace(activeResponseType.Description))
                throw new ArgumentException("Description is required.", nameof(activeResponseType.Description));

            var differentiationStrategiesTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, activeResponseType.Description)).ToList();

            if (differentiationStrategiesTypes.Any())
            {
                return differentiationStrategiesTypes.FirstOrDefault();
            }

            var id = await TypeRepository.AddAsync(new ActiveResponseType()
            {
                Name = activeResponseType.Description,
                Description = activeResponseType.Description,
                ModifiedBy = activeResponseType.ModifiedBy,
            });

            return await TypeRepository.GetByIdAsync(id);
        }

        public async Task<List<TeacherPlanActiveResponse>> GetTeacherPlanActiveResponsesByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanActiveResponses =
                (await _teacherPlanActiveResponseRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanActiveResponses.Any())
            {
                return new List<TeacherPlanActiveResponse>();
            }

            var teacherPLanActiveResponsesIds = teacherPlanActiveResponses.Select(x => x.Id).ToArray();

            var teacherPlanActiveResponseDates = (await _teacherPlanActiveResponseDateRepository.GetAllInByAsync(x => x.TeacherPlanActiveResponseId,
                teacherPLanActiveResponsesIds)).ToList();

            var teacherPlanActiveResponseTypes = (await _teacherPlanActiveResponseTypeRepository.GetAllInByAsync(x => x.TeacherPlanActiveResponseId,
                teacherPLanActiveResponsesIds)).ToList();

            var teacherPlanActiveResponseIncludes = (await _teacherPlanActiveResponseIncludeRepository.GetAllInByAsync(
                x => x.TeacherPlanActiveResponseId, teacherPLanActiveResponsesIds)).ToList();

            var types = (await GetActiveResponseTypeTranslations(langCode)).ToDictionary(x => x.Id);

            var includesInLessons = (await GetTeacherPlanActiveResponseIncludes()).ToDictionary(x => x.Id);

            foreach (var teacherPlanActiveResponse in teacherPlanActiveResponses)
            {
                teacherPlanActiveResponse.TeacherPlanActiveResponseDates = teacherPlanActiveResponseDates
                    .Where(x => x.TeacherPlanActiveResponseId == teacherPlanActiveResponse.Id).ToList();

                var activeResponseTypes = teacherPlanActiveResponseTypes
                    .Where(x => x.TeacherPlanActiveResponseId == teacherPlanActiveResponse.Id)
                    .ToList();
                activeResponseTypes.ForEach(x => x.ActiveResponseType = types[x.ActiveResponseTypesId]);

                teacherPlanActiveResponse.TeacherPlanActiveResponseTypes = activeResponseTypes;

                var activeResponseIncludes = teacherPlanActiveResponseIncludes
                    .Where(x => x.TeacherPlanActiveResponseId == teacherPlanActiveResponse.Id)
                    .ToList();

                teacherPlanActiveResponse.TeacherPlanActiveResponseIncludes = activeResponseIncludes;

            }

            return teacherPlanActiveResponses;
        }

        public async Task CreateTeacherPlanActiveResponse(TeacherPlanActiveResponse teacherPlanActiveResponse)
        {
            if (teacherPlanActiveResponse == null)
                throw new ArgumentNullException(nameof(teacherPlanActiveResponse), "TeacherPlanActiveResponse cannot be null.");

            var id = await _teacherPlanActiveResponseRepository.AddAsync(teacherPlanActiveResponse);

            if (teacherPlanActiveResponse.TeacherPlanActiveResponseTypes.Count > 0)
            {
                await SaveTeacherPlanActiveResponseTypes(teacherPlanActiveResponse, id);
            }

            if (teacherPlanActiveResponse.TeacherPlanActiveResponseDates.Count > 0)
            {
                await SaveTeacherPlanActiveResponseDates(teacherPlanActiveResponse, id);
            }
            if (teacherPlanActiveResponse.TeacherPlanActiveResponseIncludes.Count > 0)
            {
                await SaveTeacherPlanActiveResponseIncludes(teacherPlanActiveResponse, id);
            }
        }

        public async Task UpdateTeacherPlanActiveResponse(TeacherPlanActiveResponse teacherPlanActiveResponse)
        {
            if (teacherPlanActiveResponse == null)
                throw new ArgumentNullException(nameof(teacherPlanActiveResponse), "TeacherPlanActiveResponse cannot be null.");

            if (teacherPlanActiveResponse.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanActiveResponse)}.{nameof(teacherPlanActiveResponse.Id)} property.");
            }

            if (teacherPlanActiveResponse.TeacherPlanActiveResponseTypes.Count > 0)
            {
                await _teacherPlanActiveResponseTypeRepository.DeleteByAsync(
                    x => x.TeacherPlanActiveResponseId, teacherPlanActiveResponse.Id);
                await SaveTeacherPlanActiveResponseTypes(teacherPlanActiveResponse, teacherPlanActiveResponse.Id);
            }

            if (teacherPlanActiveResponse.TeacherPlanActiveResponseDates.Count > 0)
            {
                await _teacherPlanActiveResponseDateRepository.DeleteByAsync(x => x.TeacherPlanActiveResponseId,
                    teacherPlanActiveResponse.Id);
                await SaveTeacherPlanActiveResponseDates(teacherPlanActiveResponse, teacherPlanActiveResponse.Id);
            }
            if (teacherPlanActiveResponse.TeacherPlanActiveResponseIncludes.Count > 0)
            {
                await _teacherPlanActiveResponseIncludeRepository.DeleteByAsync(x => x.TeacherPlanActiveResponseId,
                    teacherPlanActiveResponse.Id);
                await SaveTeacherPlanActiveResponseIncludes(teacherPlanActiveResponse, teacherPlanActiveResponse.Id);
            }
        }

        private async Task SaveTeacherPlanActiveResponseDates(
            TeacherPlanActiveResponse teacherPlanActiveResponse, int id)
        {
            if (teacherPlanActiveResponse == null)
                throw new ArgumentNullException(nameof(teacherPlanActiveResponse), "TeacherPlanActiveResponse cannot be null.");

            if (teacherPlanActiveResponse.TeacherPlanActiveResponseDates.Count > 0)
            {
                foreach (var teacherPlanActiveResponseDate in teacherPlanActiveResponse.TeacherPlanActiveResponseDates)
                {
                    if (teacherPlanActiveResponseDate == null)
                        continue;

                    teacherPlanActiveResponseDate.TeacherPlanActiveResponseId = id;
                    DateUtil.ValidateMySqlDate(teacherPlanActiveResponseDate.ImplementationDate);
                }

                await _teacherPlanActiveResponseDateRepository.AddAsync(teacherPlanActiveResponse
                    .TeacherPlanActiveResponseDates);
            }
        }

        private async Task SaveTeacherPlanActiveResponseIncludes(
            TeacherPlanActiveResponse teacherDifferentiationPlanStrategy, int id)
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

            if (teacherDifferentiationPlanStrategy.TeacherPlanActiveResponseIncludes.Count > 0)
            {
                foreach (var teacherPlanStrategyInclude in teacherDifferentiationPlanStrategy.TeacherPlanActiveResponseIncludes)
                {
                    teacherPlanStrategyInclude.TeacherPlanActiveResponseId = id;
                }

                await _teacherPlanActiveResponseIncludeRepository.AddAsync(teacherDifferentiationPlanStrategy
                    .TeacherPlanActiveResponseIncludes);
            }
        }

        private async Task SaveTeacherPlanActiveResponseTypes(
            TeacherPlanActiveResponse teacherPlanActiveResponse, int id)
        {
            if (teacherPlanActiveResponse == null)
                throw new ArgumentNullException(nameof(teacherPlanActiveResponse), "TeacherPlanActiveResponse cannot be null.");

            if (id <= 0)
                throw new ArgumentException("Invalid id.", nameof(id));

            foreach (var teacherPlanActiveResponseType in teacherPlanActiveResponse.TeacherPlanActiveResponseTypes)
            {
                teacherPlanActiveResponseType.TeacherPlanActiveResponseId = id;
            }

            await _teacherPlanActiveResponseTypeRepository.AddAsync(teacherPlanActiveResponse
                .TeacherPlanActiveResponseTypes);

        }

        public async Task RemoveTeacherPlanActiveResponse(int teacherPlanActiveResponseId)
        {
            if (teacherPlanActiveResponseId <= 0)
                throw new ArgumentException("Invalid id.", nameof(teacherPlanActiveResponseId));

            await _teacherPlanActiveResponseDateRepository.DeleteByAsync(
                x => x.TeacherPlanActiveResponseId, teacherPlanActiveResponseId);

            await _teacherPlanActiveResponseTypeRepository.DeleteByAsync(
                x => x.TeacherPlanActiveResponseId, teacherPlanActiveResponseId);

            await _teacherPlanActiveResponseRepository.DeleteAsync(teacherPlanActiveResponseId);

            await _teacherPlanActiveResponseIncludeRepository.DeleteAsync(teacherPlanActiveResponseId);
        }

        /// <summary>
        /// To copy the activeResponses from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanActiveResponse(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            if (oldTeacherPlan == null)
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old teacher plan cannot be null.");

            if (newTeacherPlan == null)
                throw new ArgumentNullException(nameof(newTeacherPlan), "New teacher plan cannot be null.");

            var activeResponsesToCopy = await GetTeacherPlanActiveResponsesByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanActiveResponse in activeResponsesToCopy)
            {
                var newDates = teacherPlanActiveResponse.TeacherPlanActiveResponseDates
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

                var activeResponse = new TeacherPlanActiveResponse
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanActiveResponseDates = newDates.
                        Select(date => new TeacherPlanActiveResponseDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanActiveResponseTypes = teacherPlanActiveResponse.TeacherPlanActiveResponseTypes
                        .Select(x => new TeacherPlanActiveResponseType
                        {
                            ActiveResponseTypesId = x.ActiveResponseTypesId,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanActiveResponseIncludes = teacherPlanActiveResponse.TeacherPlanActiveResponseIncludes
                        .Select(x => new TeacherPlanActiveResponseInclude
                        {
                            TeacherPlanActiveResponseId = x.TeacherPlanActiveResponseId,
                            IncludesStudentActiveResponse = x.IncludesStudentActiveResponse,
                            IncludesLearningCheckActivity = x.IncludesLearningCheckActivity,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };

                await CreateTeacherPlanActiveResponse(activeResponse);
            }
        }
    }
}
