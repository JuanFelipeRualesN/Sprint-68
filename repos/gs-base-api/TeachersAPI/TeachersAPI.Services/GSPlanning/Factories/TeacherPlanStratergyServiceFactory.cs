using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Models.Translations;
using GSPlanning.Services.Generals;
using MySqlConnector;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanStratergyServiceFactory
    {
        public static TeacherPlanStrategyService CreateService(MySqlConnection dbConn)
        {
            Repository<StrategyType> strategyTypeRepository =
                new Repository<StrategyType>(dbConn);
            Repository<StrategyCategory> strategyCategoryRepository =
                new Repository<StrategyCategory>(dbConn);
            Repository<TeacherPlanStrategyType> teacherPlanStrategyTypeRepository =
                new Repository<TeacherPlanStrategyType>(dbConn);
            Repository<TeacherPlanStrategyDate> teacherPlanStrategyDateRepository =
                new Repository<TeacherPlanStrategyDate>(dbConn);
            Repository<TeacherPlanStrategy> teacherPlanStrategyRepository =
                new Repository<TeacherPlanStrategy>(dbConn);
            Repository<TeacherPlanStrategyCategory> teacherPlanStrategyCategoryRepository =
                new Repository<TeacherPlanStrategyCategory>(dbConn);
            Repository<StrategyTypeTranslation> strategyTypeTranslationRepository =
                new Repository<StrategyTypeTranslation>(dbConn);
            Repository<StrategyCategoryTranslation> strategyCategoryTranslationRepository =
                new Repository<StrategyCategoryTranslation>(dbConn);


            return new TeacherPlanStrategyService(strategyTypeRepository, teacherPlanStrategyTypeRepository,
                teacherPlanStrategyRepository, teacherPlanStrategyDateRepository, strategyCategoryRepository, teacherPlanStrategyCategoryRepository,
                strategyTypeTranslationRepository, strategyCategoryTranslationRepository);
        }
    }
}