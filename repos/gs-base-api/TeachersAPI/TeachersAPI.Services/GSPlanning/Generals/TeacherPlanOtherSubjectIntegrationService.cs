using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanOtherSubjectIntegrationService
    {
        private readonly Repository<TeacherPlanOtherSubjectIntegration> _teacherPlanOtherSubjectIntegrationRepository;
        private readonly Repository<TeacherPlanOtherSubjectIntegrationDate> _teacherPlanOtherSubjectIntegrationDateRepository;
        private readonly Repository<TeacherPlanOtherSubjectIntegrationSubject> _teacherPlanOtherSubjectIntegrationSubjectRepository;

        public TeacherPlanOtherSubjectIntegrationService(Repository<TeacherPlanOtherSubjectIntegration> teacherPlanOtherSubjectIntegrationRepository, Repository<TeacherPlanOtherSubjectIntegrationDate> teacherPlanOtherSubjectIntegrationDateRepository, Repository<TeacherPlanOtherSubjectIntegrationSubject> teacherPlanOtherSubjectIntegrationSubjectRepository)
        {
            _teacherPlanOtherSubjectIntegrationRepository = teacherPlanOtherSubjectIntegrationRepository;
            _teacherPlanOtherSubjectIntegrationDateRepository = teacherPlanOtherSubjectIntegrationDateRepository;
            _teacherPlanOtherSubjectIntegrationSubjectRepository = teacherPlanOtherSubjectIntegrationSubjectRepository;
        }

        public async Task CreateTeacherPlanOtherSubjectIntegration(
            TeacherPlanOtherSubjectIntegration teacherPlanOtherSubjectIntegration)
        {

            if (teacherPlanOtherSubjectIntegration == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanOtherSubjectIntegration), "TeacherPlanOtherSubjectIntegration cannot be null.");
            }

            var id = await _teacherPlanOtherSubjectIntegrationRepository.AddAsync(teacherPlanOtherSubjectIntegration);
            
            if (teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects.Count > 0)
            {
                await SaveTeacherPlanOtherSubjectIntegrationSubjects(teacherPlanOtherSubjectIntegration, id);
            }

            if (teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates.Count > 0)
            {
                await SaveTeacherPlanOtherSubjectIntegrationDates(teacherPlanOtherSubjectIntegration, id);
            }
        }
        
        public async Task UpdateTeacherPlanOtherSubjectIntegration(
            TeacherPlanOtherSubjectIntegration teacherPlanOtherSubjectIntegration)
        {
            if (teacherPlanOtherSubjectIntegration == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanOtherSubjectIntegration), "TeacherPlanOtherSubjectIntegration cannot be null.");
            }

            if (teacherPlanOtherSubjectIntegration.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanOtherSubjectIntegration)}.{nameof(teacherPlanOtherSubjectIntegration.Id)} property.");
            }
            
            if (teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects.Count > 0)
            {
                await _teacherPlanOtherSubjectIntegrationSubjectRepository.DeleteByAsync(
                    x => x.TeacherPlanOtherSubjectIntegrationId, teacherPlanOtherSubjectIntegration.Id);

                await SaveTeacherPlanOtherSubjectIntegrationSubjects(teacherPlanOtherSubjectIntegration, teacherPlanOtherSubjectIntegration.Id);
            }

            if (teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates.Count > 0)
            {
                await _teacherPlanOtherSubjectIntegrationDateRepository.DeleteByAsync(
                    x => x.TeacherPlanOtherSubjectIntegrationId, teacherPlanOtherSubjectIntegration.Id);
                
                await SaveTeacherPlanOtherSubjectIntegrationDates(teacherPlanOtherSubjectIntegration, teacherPlanOtherSubjectIntegration.Id);
            }
            
        }

        private async Task SaveTeacherPlanOtherSubjectIntegrationDates(
            TeacherPlanOtherSubjectIntegration teacherPlanOtherSubjectIntegration, int id)
        {
            if (teacherPlanOtherSubjectIntegration == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanOtherSubjectIntegration), "TeacherPlanOtherSubjectIntegration cannot be null.");
            }

            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID for TeacherPlanOtherSubjectIntegration.", nameof(id));
            }

            if (teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates.Count > 0)
            {
                foreach (var teacherPlanOtherSubjectIntegrationDate in teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates)
                {
                    teacherPlanOtherSubjectIntegrationDate.TeacherPlanOtherSubjectIntegrationId = id;
                }

                await _teacherPlanOtherSubjectIntegrationDateRepository.AddAsync(teacherPlanOtherSubjectIntegration
                    .TeacherPlanOtherSubjectIntegrationDates);
            }
        }

        private async Task SaveTeacherPlanOtherSubjectIntegrationSubjects(
            TeacherPlanOtherSubjectIntegration teacherPlanOtherSubjectIntegration, int id)
        {
            if (teacherPlanOtherSubjectIntegration == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanOtherSubjectIntegration), "TeacherPlanOtherSubjectIntegration cannot be null.");
            }

            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID for TeacherPlanOtherSubjectIntegration.", nameof(id));
            }

            foreach (var teacherPlanOtherSubjectIntegrationSubject in teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects)
            {
                teacherPlanOtherSubjectIntegrationSubject.TeacherPlanOtherSubjectIntegrationId = id;
            }

            await _teacherPlanOtherSubjectIntegrationSubjectRepository.AddAsync(teacherPlanOtherSubjectIntegration
                .TeacherPlanOtherSubjectInegrationSubjects);
        
        }

        public async Task<List<TeacherPlanOtherSubjectIntegration>> GetTeacherPlanOtherSubjectIntegrationByTeacherPlanId(
            int teacherPlanId)
        {
            var teacherPlanOtherSubjectIntegrations = (await _teacherPlanOtherSubjectIntegrationRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanOtherSubjectIntegrations.Any())
            {
                return new List<TeacherPlanOtherSubjectIntegration>();
            }

            var teacherPlanOtherSubjectIntegrationDates = (await _teacherPlanOtherSubjectIntegrationDateRepository.GetAllInByAsync(
                x => x.TeacherPlanOtherSubjectIntegrationId, teacherPlanOtherSubjectIntegrations.Select(x => x.Id).ToArray()))
                .ToList();
            
            var teacherPlanOtherSubjectIntegrationSubjects = (await _teacherPlanOtherSubjectIntegrationSubjectRepository.GetAllInByAsync(
                    x => x.TeacherPlanOtherSubjectIntegrationId, teacherPlanOtherSubjectIntegrations.Select(x => x.Id).ToArray()))
                .ToList();

            foreach (var teacherPlanOtherSubjectIntegration in teacherPlanOtherSubjectIntegrations)
            {
                teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates =
                    teacherPlanOtherSubjectIntegrationDates.Where(x =>
                        x.TeacherPlanOtherSubjectIntegrationId == teacherPlanOtherSubjectIntegration.Id)
                        .ToList();
                
                teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects =
                    teacherPlanOtherSubjectIntegrationSubjects.Where(x =>
                            x.TeacherPlanOtherSubjectIntegrationId == teacherPlanOtherSubjectIntegration.Id)
                        .ToList();
            }

            return teacherPlanOtherSubjectIntegrations;
        }

        public async Task RemoveTeacherPlanOtherSubjectIntegration(int teacherPlanOtherSubjectIntegrationId)
        {
            if (teacherPlanOtherSubjectIntegrationId <= 0)
            {
                throw new ArgumentException("Invalid ID for TeacherPlanOtherSubjectIntegration.", nameof(teacherPlanOtherSubjectIntegrationId));
            }

            await _teacherPlanOtherSubjectIntegrationDateRepository.DeleteByAsync(
                x => x.TeacherPlanOtherSubjectIntegrationId, teacherPlanOtherSubjectIntegrationId);
            
            await _teacherPlanOtherSubjectIntegrationSubjectRepository.DeleteByAsync(
                x => x.TeacherPlanOtherSubjectIntegrationId, teacherPlanOtherSubjectIntegrationId);
            
            await _teacherPlanOtherSubjectIntegrationRepository.DeleteAsync(teacherPlanOtherSubjectIntegrationId);
        }

        /// <summary>
        /// To copy the Plan Other Subjects from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanOtherSubjectIntegration(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            if (oldTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old TeacherPlan cannot be null.");
            }

            if (newTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(newTeacherPlan), "New TeacherPlan cannot be null.");
            }

            var otherSubjectIntegrationsToCopy = await GetTeacherPlanOtherSubjectIntegrationByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanOtherSubjectIntegration in otherSubjectIntegrationsToCopy)
            {
                var newDates = teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var otherSubjectIntegration = new TeacherPlanOtherSubjectIntegration
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanOtherSubjectIntegrationDates = newDates.
                        Select(date => new TeacherPlanOtherSubjectIntegrationDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanOtherSubjectInegrationSubjects = teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects
                        .Select(x => new TeacherPlanOtherSubjectIntegrationSubject
                        {
                            SubjectId = x.SubjectId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };
                
                await CreateTeacherPlanOtherSubjectIntegration(otherSubjectIntegration);
            }
        }
    }
}