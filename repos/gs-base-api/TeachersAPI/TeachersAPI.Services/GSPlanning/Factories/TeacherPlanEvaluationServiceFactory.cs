using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanEvaluationServiceFactory
    {
        public static TeacherPlanEvaluationService CreateService(MySqlConnection dbConn)
        {
            Repository<EvaluationType> evaluationTypeRepository =
                new Repository<EvaluationType>(dbConn);
            Repository<TeacherPlanEvaluationType> teacherPlanEvaluationTypeRepository =
                new Repository<TeacherPlanEvaluationType>(dbConn);
            Repository<TeacherPlanEvaluationsDate> teacherPlanEvaluationDateRepository =
                new Repository<TeacherPlanEvaluationsDate>(dbConn);
            Repository<TeacherPlanEvaluation> teacherPlanEvaluationRepository =
                new Repository<TeacherPlanEvaluation>(dbConn);
            Repository<EvaluationTypeTranslation> evaluationTypeTranslationRepository =
                new Repository<EvaluationTypeTranslation>(dbConn);

            return new TeacherPlanEvaluationService(evaluationTypeRepository,
                teacherPlanEvaluationRepository, teacherPlanEvaluationTypeRepository, teacherPlanEvaluationDateRepository,
                evaluationTypeTranslationRepository);
        }
    }
}