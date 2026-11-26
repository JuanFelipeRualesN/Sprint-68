using GSPlanning.Domain;
using GSPlanning.Models;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanObservationServiceFactory
    {
        public static TeacherPlanObservationService CreateService(MySqlConnection dbConn)
        {

            Repository<TeacherPlanObservation> teacherPlanObservationRepository =
                new Repository<TeacherPlanObservation>(dbConn);
            Repository<TeacherPlanObservationDate> teacherPlanObservationDateRepository =
                new Repository<TeacherPlanObservationDate>(dbConn);

            return new TeacherPlanObservationService(teacherPlanObservationRepository, teacherPlanObservationDateRepository);
        }
    }

}
