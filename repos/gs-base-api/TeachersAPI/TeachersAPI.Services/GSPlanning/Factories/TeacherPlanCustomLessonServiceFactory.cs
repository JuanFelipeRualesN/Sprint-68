using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Models.Translations;
using GSPlanning.Services.Generals;
using MySqlConnector;

namespace GSPlanning.Services.Factories
{
    public class TeacherPlanCustomLessonServiceFactory
    {
        public static TeacherPlanCustomLessonService CreateService(MySqlConnection dbConn)
        {
            Repository<EducationalCriteria> educationalCriteriaRepository =
                new Repository<EducationalCriteria>(dbConn);
            Repository<TeacherPlanCustomLessonEducationalCriteria> teacherPlanCustomLessonEducationalCriteriaRepository =
                new Repository<TeacherPlanCustomLessonEducationalCriteria>(dbConn);
            Repository<TeacherPlanCustomLessonDate> teacherPlanCustomLessonDateRepository =
                new Repository<TeacherPlanCustomLessonDate>(dbConn);
            Repository<TeacherPlanCustomLesson> teacherPlanCustomLessonRepository =
                new Repository<TeacherPlanCustomLesson>(dbConn);
            Repository<EducationalCriteriaTranslation> educationalCriteriaTranslationRepository =
                new Repository<EducationalCriteriaTranslation>(dbConn);

            return new TeacherPlanCustomLessonService(educationalCriteriaRepository, teacherPlanCustomLessonEducationalCriteriaRepository,
                teacherPlanCustomLessonRepository, teacherPlanCustomLessonDateRepository, educationalCriteriaTranslationRepository);
        }
    }
}