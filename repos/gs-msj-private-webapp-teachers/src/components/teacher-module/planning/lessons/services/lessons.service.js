import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";
import moment from "moment";

// const getLessons = async (planningId) => {
//   try {
//     const endpoint = Settings.getLessonsTeacher(planningId, 0, 100);
//     const response = await axios.get(endpoint);
//     return response.status === 200 ? response.data.LessonModelList : [];
//   } catch (error) {
//     return Promise.resolve([]);
//   }
// };

const deleteLesson = async (planningId, id) => {
  const endpoint = Settings.getArchiveLessonOfPlanningAPI(planningId, id);
  const response = await axios.post(endpoint);
  return Promise.resolve(response.status === 200);
};

const createLesson = async (lesson) => {
  const data = {
    ...lesson,
    TeacherPlanLessonQuizModelList: lesson.TeacherPlanLessonQuizModelList.map((quiz) => {
      return {
        QuizId: quiz.quizId,
        DueDate: `${moment(quiz.date).format("YYYY/MM/DD")} ${quiz.time}`,
        DueDateTimezone: quiz.timezone,
        Duration: quiz.duration,
      };
    }),
  };
  const endpoint = Settings.getAttachLessonToPlanAPI();
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return response.data;
};

const updateLesson = async (lesson) => {
  const data = {
    ...lesson,
    ForGrading: lesson.ForGrading ? lesson.ForGrading : "0",
    TeacherPlanLessonQuizModelList: lesson.TeacherPlanLessonQuizModelList.map((quiz) => {
      return {
        Id: quiz.id,
        QuizId: quiz.quizId,
        DueDate: `${moment(quiz.date).format("YYYY/MM/DD")} ${quiz.time}`,
        DueDateTimezone: quiz.timezone,
        Duration: quiz.duration,
        PlanLessonId: quiz.planLessonId,
      };
    }),
  };
  const endpoint = Settings.getModifyTeacherLessonAPI();
  const response = await axios.post(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

export { deleteLesson, createLesson, updateLesson };
