using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanPraxisReflectionServiceFactory
    {
        public static TeacherPlanPraxisReflectionService CreateService(MySqlConnection dbConn)
        {
            Repository<PraxisReflectionCompletedReason> praxisReflectionTypeRepository =
                new Repository<PraxisReflectionCompletedReason>(dbConn);
            Repository<TeacherPlanPraxisReflectionCompletedReason> teacherPlanPraxisReflectionCompletedReasonRepository =
                new Repository<TeacherPlanPraxisReflectionCompletedReason>(dbConn);
            Repository<TeacherPlanPraxisReflectionDate> teacherPlanPraxisReflectionDateRepository =
                new Repository<TeacherPlanPraxisReflectionDate>(dbConn);
            Repository<TeacherPlanPraxisReflection> teacherPlanPraxisReflectionRepository =
                new Repository<TeacherPlanPraxisReflection>(dbConn);
            Repository<PraxisReflectionCompletedReasonTranslation> praxisReflectionCompletedReasonTranslationRepository =
                new Repository<PraxisReflectionCompletedReasonTranslation>(dbConn);

            return new TeacherPlanPraxisReflectionService(praxisReflectionTypeRepository, teacherPlanPraxisReflectionCompletedReasonRepository,
                teacherPlanPraxisReflectionRepository, teacherPlanPraxisReflectionDateRepository, praxisReflectionCompletedReasonTranslationRepository);
        }
    }
}