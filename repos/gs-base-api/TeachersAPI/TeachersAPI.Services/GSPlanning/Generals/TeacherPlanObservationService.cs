using GSPlanning.Domain;
using GSPlanning.Models;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanObservationService : Service<TeacherPlanObservation>
    {
        private readonly Repository<TeacherPlanObservation> _teacherPlantObservationRepository;
        private readonly Repository<TeacherPlanObservationDate> _teacherPlanObservationDateRepository;

        public TeacherPlanObservationService(Repository<TeacherPlanObservation> teacherPlantObservationRepository, Repository<TeacherPlanObservationDate> teacherPlanObservationDateRepository)
            : base(teacherPlantObservationRepository)
        {
            _teacherPlantObservationRepository = teacherPlantObservationRepository;
            _teacherPlanObservationDateRepository = teacherPlanObservationDateRepository;
        }

        public async Task<List<TeacherPlanObservation>> GetTeacherPlanObservationTypes()
        {
            return await GetTypes();
        }

        public async Task<List<TeacherPlanObservation>> GetTeacherPlanObservationByTeacherPlanId(int teacherPlanId)
        {
            var teacherPlanObservations =
                (await _teacherPlantObservationRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanObservations.Any())
            {
                return new List<TeacherPlanObservation>();
            }

            var teacherPlanObservationsIds = teacherPlanObservations.Select(x => x.Id).ToArray();

            var teacherPlanObservationsDates = (await _teacherPlanObservationDateRepository.GetAllInByAsync(x => x.TeacherPlanObservationId,
                teacherPlanObservationsIds)).ToList();


            var types = (await GetTypes()).ToDictionary(x => x.Id);

            foreach (var teacherPlanObservation in teacherPlanObservations)
            {
                teacherPlanObservation.TeacherPlanObservationDates = teacherPlanObservationsDates
                    .Where(x => x.TeacherPlanObservationId == teacherPlanObservation.Id).ToList();
            }

            return teacherPlanObservations;
        }

        public async Task CreateTeacherPlanObservation(TeacherPlanObservation teacherPlanObservation)
        {

            if (teacherPlanObservation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanObservation), "The teacher plan praxis reflection cannot be null.");
            }

            var id = await _teacherPlantObservationRepository.AddAsync(teacherPlanObservation);


            if (teacherPlanObservation.TeacherPlanObservationDates.Count > 0)
            {
                await SaveTeacherPlanObservationDates(teacherPlanObservation, id);
            }
        }

        public async Task UpdateTeacherPlanObservation(TeacherPlanObservation teacherPlanObservation)
        {
            if (teacherPlanObservation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanObservation), "The teacher plan praxis reflection cannot be null.");
            }

            if (teacherPlanObservation.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanObservation)}.{nameof(teacherPlanObservation.Id)} property.");
            }

            await _teacherPlantObservationRepository.UpdateAsync(teacherPlanObservation);


            await _teacherPlanObservationDateRepository.DeleteByAsync(x => x.TeacherPlanObservationId,
                teacherPlanObservation.Id);

            if (teacherPlanObservation.TeacherPlanObservationDates.Count > 0)
            {
                await SaveTeacherPlanObservationDates(teacherPlanObservation, teacherPlanObservation.Id);
            }
        }

        private async Task SaveTeacherPlanObservationDates(
            TeacherPlanObservation teacherPlanObservation, int id)
        {

            if (teacherPlanObservation == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanObservation), "The teacher plan praxis reflection cannot be null.");
            }

            // If id is 0
            if (id == 0)
            {
                throw new ArgumentException(
                                         $"Invalid update. Is missing the {nameof(TeacherPlanObservation)}.{nameof(teacherPlanObservation.Id)} property.");
            }


            foreach (var teacherPlanObservationDate in teacherPlanObservation.TeacherPlanObservationDates)
            {
                teacherPlanObservationDate.TeacherPlanObservationId = id;
            }

            await _teacherPlanObservationDateRepository.AddAsync(teacherPlanObservation
                .TeacherPlanObservationDates);
        }


        public async Task RemoveTeacherPlanObservation(int teacherPlanObservationId)
        {
            // If id is 0 throw an exception
            if (teacherPlanObservationId == 0)
            {
                throw new ArgumentException(
                                       $"Invalid delete. Is missing the {nameof(TeacherPlanObservation)}.{nameof(teacherPlanObservationId)} property.");
            }

            await _teacherPlanObservationDateRepository.DeleteByAsync(
                x => x.TeacherPlanObservationId, teacherPlanObservationId);

            await _teacherPlantObservationRepository.DeleteAsync(teacherPlanObservationId);
        }

        /// <summary>
        /// To copy the Plan Praxis from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanObservation(TeacherPlan oldTeacherPlan,
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

            var teacherPlanObservationToCopy = await GetTeacherPlanObservationByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanObservation in teacherPlanObservationToCopy)
            {
                var newDates =
                    teacherPlanObservation.TeacherPlanObservationDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var observation = new TeacherPlanObservation
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    Detail = teacherPlanObservation.Detail,
                    TeacherPlanObservationDates = newDates.
                        Select(date => new TeacherPlanObservationDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };

                await CreateTeacherPlanObservation(observation);
            }
        }
    }
}
