using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using GSPlanning.Models.Translations;
using MySqlConnector;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanInitiativeProjectInnovationServiceFactory
    {
        public static TeacherPlanInitiativeProjectInnovationService CreateService(MySqlConnection dbConn)
        {
            Repository<InitiativeProjectInnovationType> initiativeProjectInnovationTypeRepository =
                new Repository<InitiativeProjectInnovationType>(dbConn);
            Repository<TeacherPlanInitiativeProjectInnovationType> teacherPlanInitiativeProjectInnovationTypeRepository =
                new Repository<TeacherPlanInitiativeProjectInnovationType>(dbConn);
            Repository<TeacherPlanInitiativeProjectInnovationDate> teacherPlanInitiativeProjectInnovationDateRepository =
                new Repository<TeacherPlanInitiativeProjectInnovationDate>(dbConn);
            Repository<TeacherPlanInitiativeProjectInnovation> teacherPlanInitiativeProjectInnovationRepository =
                new Repository<TeacherPlanInitiativeProjectInnovation>(dbConn);
            Repository<InitiativeProjectInnovationTypeTranslation> initiativeProjectInnovationTypeTranslationRepository =
                new Repository<InitiativeProjectInnovationTypeTranslation>(dbConn);

            return new TeacherPlanInitiativeProjectInnovationService(initiativeProjectInnovationTypeRepository,
                teacherPlanInitiativeProjectInnovationRepository,
                teacherPlanInitiativeProjectInnovationTypeRepository,
                teacherPlanInitiativeProjectInnovationDateRepository,
                initiativeProjectInnovationTypeTranslationRepository);
        }
    }
}