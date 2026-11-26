using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Models;
using GSPlanning.Services.Generals;
using MySqlConnector;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanActiveResponseServiceFactory
    {
        public static TeacherPlanActiveResponseService CreateService(MySqlConnection dbConn)
        {

            Repository<ActiveResponseType> activeResponseTypeRepository =
                new Repository<ActiveResponseType>(dbConn);
            Repository<TeacherPlanActiveResponseType> teacherPlanActiveResponseTypeRepository =
                new Repository<TeacherPlanActiveResponseType>(dbConn);
            Repository<TeacherPlanActiveResponseDate> teacherPlanActiveResponseDateRepository =
                new Repository<TeacherPlanActiveResponseDate>(dbConn);
            Repository<TeacherPlanActiveResponse> teacherPlanActiveResponseRepository =
                new Repository<TeacherPlanActiveResponse>(dbConn);
            Repository<TeacherPlanActiveResponseInclude> teacherPlanActiveResponseIncludeRepository =
                new Repository<TeacherPlanActiveResponseInclude>(dbConn);
            Repository<ActiveResponseTypeTranslation> activeResponseTypeTranslationRepository =
                new Repository<ActiveResponseTypeTranslation>(dbConn);

            return new TeacherPlanActiveResponseService(activeResponseTypeRepository, teacherPlanActiveResponseTypeRepository,
                teacherPlanActiveResponseDateRepository, teacherPlanActiveResponseRepository, teacherPlanActiveResponseIncludeRepository,
                activeResponseTypeTranslationRepository);
        }
    }
}
