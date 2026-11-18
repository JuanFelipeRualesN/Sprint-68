using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanOtherSubjectIntegrationServiceFactory
    {
        public static TeacherPlanOtherSubjectIntegrationService CreateService(MySqlConnection dbConn)
        {
            Repository<TeacherPlanOtherSubjectIntegration> teacherPlanOtherSubjectIntegrationRepository =
                new Repository<TeacherPlanOtherSubjectIntegration>(dbConn);
            Repository<TeacherPlanOtherSubjectIntegrationDate> teacherPlanOtherSubjectIntegrationDateRepository =
                new Repository<TeacherPlanOtherSubjectIntegrationDate>(dbConn);
            Repository<TeacherPlanOtherSubjectIntegrationSubject> teacherPlanOtherSubjectIntegrationSubjectReposito =
                new Repository<TeacherPlanOtherSubjectIntegrationSubject>(dbConn);

            return new TeacherPlanOtherSubjectIntegrationService(teacherPlanOtherSubjectIntegrationRepository,
                teacherPlanOtherSubjectIntegrationDateRepository, teacherPlanOtherSubjectIntegrationSubjectReposito);
        }
    }
}