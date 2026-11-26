using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Models;
using GSPlanning.Services.Generals;
using MySqlConnector;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanDifferentiationStrategyServiceFactory
    {
        public static TeacherPlanDifferentiationStrategyService CreateService(MySqlConnection dbConn)
        {
            Repository<DifferentiationStrategyCategory> differentiationStrategyCategoryRepository =
                new Repository<DifferentiationStrategyCategory>(dbConn);
            Repository<DifferentiationStrategyType> differentiationStrategyTypeRepository =
                new Repository<DifferentiationStrategyType>(dbConn);
            Repository<TeacherPlanDifferentiationStrategyType> teacherPlanDifferentiationStrategyTypeRepository =
                new Repository<TeacherPlanDifferentiationStrategyType>(dbConn);
            Repository<TeacherPlanDifferentiationStrategyDate> teacherPlanDifferentiationStrategyDateRepository =
                new Repository<TeacherPlanDifferentiationStrategyDate>(dbConn);
            Repository<TeacherPlanDifferentiationStrategy> teacherPlanDifferentiationStrategyRepository =
                new Repository<TeacherPlanDifferentiationStrategy>(dbConn);
            Repository< TeacherPlanDifferentiationStrategyCategory> teacherPlanDifferentiationStrategyCategoryRepository =
                new Repository<TeacherPlanDifferentiationStrategyCategory>(dbConn);
            Repository<DifferentiationStrategyTypeTranslation> differentiationStrategyTypeTranslationRepository =
                new Repository<DifferentiationStrategyTypeTranslation>(dbConn);
            Repository<DifferentiationStrategyCategoryTranslation> differentiationStrategyCategoryTranslationRepository =
                new Repository<DifferentiationStrategyCategoryTranslation>(dbConn);

            return new TeacherPlanDifferentiationStrategyService(differentiationStrategyCategoryRepository, differentiationStrategyTypeRepository, teacherPlanDifferentiationStrategyTypeRepository,
                teacherPlanDifferentiationStrategyDateRepository, teacherPlanDifferentiationStrategyRepository, teacherPlanDifferentiationStrategyCategoryRepository, differentiationStrategyTypeTranslationRepository,
                differentiationStrategyCategoryTranslationRepository);
        }
    }
}
