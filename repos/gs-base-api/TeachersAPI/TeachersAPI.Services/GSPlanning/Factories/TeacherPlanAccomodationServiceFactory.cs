using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Generals;
using MySqlConnector;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanAccommodationServiceFactory
    {
        public static TeacherPlanAccommodationService CreateService(MySqlConnection dbConn)
        {
            Repository<AccommodationType> accommodationTypeRepository =
                new Repository<AccommodationType>(dbConn);
            Repository<TeacherPlanAccommodationType> teacherPlanAccommodationTypeRepository =
                new Repository<TeacherPlanAccommodationType>(dbConn);
            Repository<TeacherPlanAccommodationDate> teacherPlanAccommodationDateRepository =
                new Repository<TeacherPlanAccommodationDate>(dbConn);
            Repository<TeacherPlanAccommodation> teacherPlanAccommodationRepository =
                new Repository<TeacherPlanAccommodation>(dbConn);
            Repository<AccommodationTypeTranslation> accommodationTypeTranslationRepository =
                new Repository<AccommodationTypeTranslation>(dbConn);


            return new TeacherPlanAccommodationService(accommodationTypeRepository, teacherPlanAccommodationTypeRepository,
                teacherPlanAccommodationRepository, teacherPlanAccommodationDateRepository, accommodationTypeTranslationRepository);
        }
    }
}