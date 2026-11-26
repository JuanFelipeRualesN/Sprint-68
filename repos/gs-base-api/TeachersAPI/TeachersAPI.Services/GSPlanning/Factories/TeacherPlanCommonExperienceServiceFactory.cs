using GSPlanning.Domain;
using GSPlanning.Models;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanCommonExperienceServiceFactory
    {
        public static TeacherPlanCommonExperienceService CreateService(MySqlConnection dbConn)
        {

            Repository<TeacherPlanCommonExperience> teacherPlanCommonExperienceRepository =
                new Repository<TeacherPlanCommonExperience>(dbConn);
            Repository<TeacherPlanCommonExperienceDate> teacherPlanCommonExperienceDateRepository =
                new Repository<TeacherPlanCommonExperienceDate>(dbConn);

            return new TeacherPlanCommonExperienceService(teacherPlanCommonExperienceRepository, teacherPlanCommonExperienceDateRepository);
        }
    }
}
