using GSPlanning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanAccommodationService : Service<AccommodationType>
    {
        private readonly Repository<AccommodationTypeTranslation> _accommodationTypeTranslationRepository;
        private readonly Repository<TeacherPlanAccommodation> _teacherPlanAccommodationRepository;
        private readonly Repository<TeacherPlanAccommodationType> _teacherPlanAccommodationTypeRepository;
        private readonly Repository<TeacherPlanAccommodationDate> _teacherPlanAccommodationDateRepository;

        public TeacherPlanAccommodationService(
            Repository<AccommodationType> accommodationTypeRepository, 
            Repository<TeacherPlanAccommodationType> teacherPlanAccommodationTypeRepository, 
            Repository<TeacherPlanAccommodation> teacherPlanAccommodationRepository, 
            Repository<TeacherPlanAccommodationDate> teacherPlanAccommodationDateRepository,
            Repository<AccommodationTypeTranslation> accommodationTypeTranslationRepository
            )
            : base(accommodationTypeRepository)
        {
            _teacherPlanAccommodationTypeRepository = teacherPlanAccommodationTypeRepository;
            _teacherPlanAccommodationRepository = teacherPlanAccommodationRepository;
            _teacherPlanAccommodationDateRepository = teacherPlanAccommodationDateRepository;
            _accommodationTypeTranslationRepository = accommodationTypeTranslationRepository;
        }

        public async Task<List<AccommodationType>> GetAccommodationTypes()
        {
           return await GetTypes();
        }

        public async Task<List<AccommodationType>> GetAccommodationTypeTranslations(string langCode = "es")
        {
            var accomodationTypes = await GetTypes();
            var translations = (await _accommodationTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var accomodationType in accomodationTypes)
            {
                var translation = translations.FirstOrDefault(x => x.AccommodationTypeId == accomodationType.Id);
                if (translation != null)
                {
                    accomodationType.Description = translation.Description;
                    accomodationType.Name = translation.Name;
                }
            }
            return accomodationTypes;
        }

        public async Task<AccommodationType> CreateAccommodationType(AccommodationType accommodationType)
        {
            if (accommodationType == null)
                throw new ArgumentNullException(nameof(accommodationType), "AccommodationType cannot be null.");

            if (string.IsNullOrWhiteSpace(accommodationType.Description))
                throw new ArgumentException("Description is required.", nameof(accommodationType.Description));

            var accommodationTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, accommodationType.Description)).ToList();
            
            if (accommodationTypes.Any())
            {
                return accommodationTypes.FirstOrDefault();
            }
            
            var id = await TypeRepository.AddAsync(new AccommodationType()
            {
                Description =  accommodationType.Description,
                Name = accommodationType.Description,
                ModifiedBy = accommodationType.ModifiedBy,
                Type = AccommodationType.IsCustom,
            });

            return await TypeRepository.GetByIdAsync(id);
        }

        public async Task<List<TeacherPlanAccommodation>> GetTeacherPlanAccommodationsByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanAccommodations =
                (await _teacherPlanAccommodationRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanAccommodations.Any())
            {
                return new List<TeacherPlanAccommodation>();
            }
            
            var teacherPLanAccommodationsIds = teacherPlanAccommodations.Select(x => x.Id).ToArray();
            
            var teacherPlanAccommodationDates = (await _teacherPlanAccommodationDateRepository.GetAllInByAsync(x => x.TeacherPlanAccommodationId,
                teacherPLanAccommodationsIds)).ToList();

            var teacherPlanAccommodationTypes = (await _teacherPlanAccommodationTypeRepository.GetAllInByAsync(x => x.TeacherPlanAccommodationId,
                teacherPLanAccommodationsIds)).ToList();

            var types = (await GetAccommodationTypeTranslations(langCode))
                .ToDictionary(x => x.Id);
            
            foreach (var teacherPlanAccommodation in teacherPlanAccommodations)
            {
                teacherPlanAccommodation.TeacherPlanAccommodationDates = teacherPlanAccommodationDates
                    .Where(x => x.TeacherPlanAccommodationId == teacherPlanAccommodation.Id).ToList();

                var accomodationTypes = teacherPlanAccommodationTypes
                    .Where(x => x.TeacherPlanAccommodationId == teacherPlanAccommodation.Id)
                    .ToList();
                accomodationTypes.ForEach(x => x.AccommodationType = types[x.AccommodationTypeId]);
                
                teacherPlanAccommodation.TeacherPlanAccommodationTypes = accomodationTypes;

            }

            return teacherPlanAccommodations;
        }

        public async Task CreateTeacherPlanAccommodation(TeacherPlanAccommodation teacherPlanAccommodation)
        {
            if (teacherPlanAccommodation == null)
                throw new ArgumentNullException(nameof(teacherPlanAccommodation), "TeacherPlanAccommodation cannot be null.");

            var id = await _teacherPlanAccommodationRepository.AddAsync(teacherPlanAccommodation);
            
            if (teacherPlanAccommodation.TeacherPlanAccommodationTypes.Count > 0)
            {
                await SaveTeacherPlanAccommodationTypes(teacherPlanAccommodation, id);
            }

            if (teacherPlanAccommodation.TeacherPlanAccommodationDates.Count > 0)
            {
                await SaveTeacherPlanAccommodationDates(teacherPlanAccommodation, id);
            }
        }
        
        public async Task UpdateTeacherPlanAccommodation(TeacherPlanAccommodation teacherPlanAccommodation)
        {
            if (teacherPlanAccommodation == null)
                throw new ArgumentNullException(nameof(teacherPlanAccommodation), "TeacherPlanAccommodation cannot be null.");

            if (teacherPlanAccommodation.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanAccommodation)}.{nameof(teacherPlanAccommodation.Id)} property.");
            }
            
            if (teacherPlanAccommodation.TeacherPlanAccommodationTypes.Count > 0)
            {
                await _teacherPlanAccommodationTypeRepository.DeleteByAsync(
                    x => x.TeacherPlanAccommodationId, teacherPlanAccommodation.Id);
                await SaveTeacherPlanAccommodationTypes(teacherPlanAccommodation, teacherPlanAccommodation.Id);
            }

            if (teacherPlanAccommodation.TeacherPlanAccommodationDates.Count > 0)
            {
                await _teacherPlanAccommodationDateRepository.DeleteByAsync(x => x.TeacherPlanAccommodationId,
                    teacherPlanAccommodation.Id);
                await SaveTeacherPlanAccommodationDates(teacherPlanAccommodation, teacherPlanAccommodation.Id);
            }
        }

        private async Task SaveTeacherPlanAccommodationDates(
            TeacherPlanAccommodation teacherPlanAccommodation, int id)
        {
            if (teacherPlanAccommodation == null)
                throw new ArgumentNullException(nameof(teacherPlanAccommodation), "TeacherPlanAccommodation cannot be null.");

            if (teacherPlanAccommodation.TeacherPlanAccommodationDates.Count > 0)
            {
                foreach (var teacherPlanAccommodationDate in teacherPlanAccommodation.TeacherPlanAccommodationDates)
                {
                    if (teacherPlanAccommodationDate == null)
                        continue; 

                    teacherPlanAccommodationDate.TeacherPlanAccommodationId = id;
                    DateUtil.ValidateMySqlDate(teacherPlanAccommodationDate.ImplementationDate);
                }

                await _teacherPlanAccommodationDateRepository.AddAsync(teacherPlanAccommodation
                    .TeacherPlanAccommodationDates);
            }
        }

        private async Task SaveTeacherPlanAccommodationTypes(
            TeacherPlanAccommodation teacherPlanAccommodation, int id)
        {
            if (teacherPlanAccommodation == null)
                throw new ArgumentNullException(nameof(teacherPlanAccommodation), "TeacherPlanAccommodation cannot be null.");

            if (id <= 0)
                throw new ArgumentException("Invalid id.", nameof(id));

            foreach (var teacherPlanAccommodationType in teacherPlanAccommodation.TeacherPlanAccommodationTypes)
            {
                teacherPlanAccommodationType.TeacherPlanAccommodationId = id;
            }

            await _teacherPlanAccommodationTypeRepository.AddAsync(teacherPlanAccommodation
                .TeacherPlanAccommodationTypes);
        
        }
        
        public async Task RemoveTeacherPlanAccommodation(int teacherPlanAccommodationId)
        {
            if (teacherPlanAccommodationId <= 0)
                throw new ArgumentException("Invalid id.", nameof(teacherPlanAccommodationId));

            await _teacherPlanAccommodationDateRepository.DeleteByAsync(
                x => x.TeacherPlanAccommodationId, teacherPlanAccommodationId);
            
            await _teacherPlanAccommodationTypeRepository.DeleteByAsync(
                x => x.TeacherPlanAccommodationId, teacherPlanAccommodationId);
            
            await _teacherPlanAccommodationRepository.DeleteAsync(teacherPlanAccommodationId);
        }

        /// <summary>
        /// To copy the accommodations from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanAccommodation(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            if (oldTeacherPlan == null)
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old teacher plan cannot be null.");

            if (newTeacherPlan == null)
                throw new ArgumentNullException(nameof(newTeacherPlan), "New teacher plan cannot be null.");

            var accommodationsToCopy = await GetTeacherPlanAccommodationsByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanAccommodation in accommodationsToCopy)
            {
                var newDates = teacherPlanAccommodation.TeacherPlanAccommodationDates
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

                var accommodation = new TeacherPlanAccommodation
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanAccommodationDates = newDates.
                        Select(date => new TeacherPlanAccommodationDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanAccommodationTypes = teacherPlanAccommodation.TeacherPlanAccommodationTypes
                        .Select(x => new TeacherPlanAccommodationType
                        {
                            AccommodationTypeId = x.AccommodationTypeId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };
                
                await CreateTeacherPlanAccommodation(accommodation);
            }
        }
    }
}
