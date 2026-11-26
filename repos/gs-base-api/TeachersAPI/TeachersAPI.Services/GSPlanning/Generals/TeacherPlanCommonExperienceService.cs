using GSPlanning.Domain;
using GSPlanning.Models;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanCommonExperienceService : Service<TeacherPlanCommonExperience>
    {
        private readonly Repository<TeacherPlanCommonExperience> _teacherPlantCommonExperienceRepository;
        private readonly Repository<TeacherPlanCommonExperienceDate> _teacherPlanCommonExperienceDateRepository;

        public TeacherPlanCommonExperienceService(Repository<TeacherPlanCommonExperience> teacherPlantCommonExperienceRepository, Repository<TeacherPlanCommonExperienceDate> teacherPlanCommonExperienceDateRepository)
            : base(teacherPlantCommonExperienceRepository)
        {
            _teacherPlantCommonExperienceRepository = teacherPlantCommonExperienceRepository;
            _teacherPlanCommonExperienceDateRepository = teacherPlanCommonExperienceDateRepository;
        }

        public async Task<List<TeacherPlanCommonExperience>> GetTeacherPlanCommonExperienceTypes()
        {
            return await GetTypes();
        }

        public async Task<List<TeacherPlanCommonExperience>> GetTeacherPlanCommonExperienceByTeacherPlanId(int teacherPlanId)
        {
            var teacherPlanCommonExperiences =
                (await _teacherPlantCommonExperienceRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanCommonExperiences.Any())
            {
                return new List<TeacherPlanCommonExperience>();
            }

            var teacherPlanCommonExperiencesIds = teacherPlanCommonExperiences.Select(x => x.Id).ToArray();

            var teacherPlanCommonExperiencesDates = (await _teacherPlanCommonExperienceDateRepository.GetAllInByAsync(x => x.TeacherPlanCommonExperienceId,
                teacherPlanCommonExperiencesIds)).ToList();


            var types = (await GetTypes()).ToDictionary(x => x.Id);

            foreach (var teacherPlanCommonExperience in teacherPlanCommonExperiences)
            {
                teacherPlanCommonExperience.TeacherPlanCommonExperienceDates = teacherPlanCommonExperiencesDates
                    .Where(x => x.TeacherPlanCommonExperienceId == teacherPlanCommonExperience.Id).ToList();
            }

            return teacherPlanCommonExperiences;
        }

        public async Task CreateTeacherPlanCommonExperience(TeacherPlanCommonExperience teacherPlanCommonExperience)
        {

            if (teacherPlanCommonExperience == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanCommonExperience), "The teacher plan praxis reflection cannot be null.");
            }

            var id = await _teacherPlantCommonExperienceRepository.AddAsync(teacherPlanCommonExperience);


            if (teacherPlanCommonExperience.TeacherPlanCommonExperienceDates.Count > 0)
            {
                await SaveTeacherPlanCommonExperienceDates(teacherPlanCommonExperience, id);
            }
        }

        public async Task UpdateTeacherPlanCommonExperience(TeacherPlanCommonExperience teacherPlanCommonExperience)
        {
            if (teacherPlanCommonExperience == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanCommonExperience), "The teacher plan praxis reflection cannot be null.");
            }

            if (teacherPlanCommonExperience.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanCommonExperience)}.{nameof(teacherPlanCommonExperience.Id)} property.");
            }

            await _teacherPlantCommonExperienceRepository.UpdateAsync(teacherPlanCommonExperience);


            await _teacherPlanCommonExperienceDateRepository.DeleteByAsync(x => x.TeacherPlanCommonExperienceId,
                teacherPlanCommonExperience.Id);

            if (teacherPlanCommonExperience.TeacherPlanCommonExperienceDates.Count > 0)
            {
                await SaveTeacherPlanCommonExperienceDates(teacherPlanCommonExperience, teacherPlanCommonExperience.Id);
            }
        }

        private async Task SaveTeacherPlanCommonExperienceDates(
            TeacherPlanCommonExperience teacherPlanCommonExperience, int id)
        {

            if (teacherPlanCommonExperience == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanCommonExperience), "The teacher plan praxis reflection cannot be null.");
            }

            // If id is 0
            if (id == 0)
            {
                throw new ArgumentException(
                                         $"Invalid update. Is missing the {nameof(TeacherPlanCommonExperience)}.{nameof(teacherPlanCommonExperience.Id)} property.");
            }


            foreach (var teacherPlanCommonExperienceDate in teacherPlanCommonExperience.TeacherPlanCommonExperienceDates)
            {
                teacherPlanCommonExperienceDate.TeacherPlanCommonExperienceId = id;
            }

            await _teacherPlanCommonExperienceDateRepository.AddAsync(teacherPlanCommonExperience
                .TeacherPlanCommonExperienceDates);
        }


        public async Task RemoveTeacherPlanCommonExperience(int teacherPlanCommonExperienceId)
        {
            // If id is 0 throw an exception
            if (teacherPlanCommonExperienceId == 0)
            {
                throw new ArgumentException(
                                       $"Invalid delete. Is missing the {nameof(TeacherPlanCommonExperience)}.{nameof(teacherPlanCommonExperienceId)} property.");
            }

            await _teacherPlanCommonExperienceDateRepository.DeleteByAsync(
                x => x.TeacherPlanCommonExperienceId, teacherPlanCommonExperienceId);

            await _teacherPlantCommonExperienceRepository.DeleteAsync(teacherPlanCommonExperienceId);
        }

        /// <summary>
        /// To copy the Plan Praxis from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanCommonExperience(TeacherPlan oldTeacherPlan,
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

            var teacherPlanCommonExperienceToCopy = await GetTeacherPlanCommonExperienceByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanCommonExperience in teacherPlanCommonExperienceToCopy)
            {
                var newDates =
                    teacherPlanCommonExperience.TeacherPlanCommonExperienceDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var observation = new TeacherPlanCommonExperience
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    Detail = teacherPlanCommonExperience.Detail,
                    TeacherPlanCommonExperienceDates = newDates.
                        Select(date => new TeacherPlanCommonExperienceDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };

                await CreateTeacherPlanCommonExperience(observation);
            }
        }
    }
}
