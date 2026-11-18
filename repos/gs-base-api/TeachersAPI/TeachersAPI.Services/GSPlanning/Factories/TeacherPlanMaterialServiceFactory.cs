using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanMaterialServiceFactory
    {
        public static TeacherPlanMaterialService CreateService(MySqlConnection dbConn)
        {
            Repository<MaterialType> materialTypeRepository =
                new Repository<MaterialType>(dbConn);
            Repository<TeacherPlanMaterialType> teacherPlanMaterialTypeRepository =
                new Repository<TeacherPlanMaterialType>(dbConn);
            Repository<TeacherPlanMaterialDate> teacherPlanMaterialDateRepository =
                new Repository<TeacherPlanMaterialDate>(dbConn);
            Repository<TeacherPlanMaterial> teacherPlanMaterialRepository =
                new Repository<TeacherPlanMaterial>(dbConn);
            Repository<MaterialTypeTranslation> materialTypeTranslationRepository =
                new Repository<MaterialTypeTranslation>(dbConn);

            return new TeacherPlanMaterialService(materialTypeRepository,
                teacherPlanMaterialRepository, teacherPlanMaterialTypeRepository, teacherPlanMaterialDateRepository, materialTypeTranslationRepository);
        }
    }
}