import axios from "../../../../../api/interceptors";
import moment from "moment";
import Settings from "../../../../../custom-classes/settings";

const getQuizzes = async (planningId, limit, pageNumber) => {
  try {
    const endpoint = Settings.getTeacherLessonsAPI(planningId, limit, pageNumber);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteQuiz = async (planningId, quizId) => {
  const endpoint = Settings.getTeacherLessonsDeleteAPI(planningId, quizId);
  const response = await axios.delete(endpoint);
  return Promise.resolve(response.status === 200);
};

const createQuiz = async (request) => {
  // Uncomment the following line to exclude inactive quizzes when allowing users to select quizzes for planning.
  // const filtered = request.quizzes.filter((t) => t.isActive);
  const filtered = request.quizzes // For MSJ select all quizzes for planning
  const data = {
    LessonId: request.lessonId,
    PlanningId: request.planningId,
    DueDate: moment(request.dueDate).format("YYYY/MM/DD"),
    TeacherPlanLessonQuizModelList: filtered.map((quiz) => {
      return {
        PlanLessonId: quiz.planLessonId,
        ForGrading: quiz.countForGrade ? "1" : "0",
        QuizId: quiz.quizId,
        DueDate: `${moment(quiz.date).format("YYYY/MM/DD")} ${quiz.time}`,
        DueDateTimezone: quiz.timezone,
        Duration: quiz.duration,
      };
    }),
  };

  const endpoint = Settings.getTeacherLessonsCreationAPI();
  const response = await axios.post(endpoint, data);

  return Promise.resolve(response.status === 201);
};

const updateQuiz = async (request) => {
  // Uncomment the following line to exclude inactive quizzes when allowing users to select quizzes for planning.
  // const filtered = request.quizzes.filter((t) => t.isActive);
  const filtered = request.quizzes // For MSJ select all quizzes for planning
  const data = {
    LessonId: request.lessonId,
    PlanningId: request.planningId,
    DueDate: moment(request.dueDate).format("YYYY/MM/DD"),
    TeacherPlanLessonQuizModelList: filtered.map((quiz) => {
      return {
        Id: quiz.id,
        PlanLessonId: quiz.planLessonId,
        ForGrading: quiz.countForGrade ? "1" : "0",
        QuizId: quiz.quizId,
        DueDate: `${moment(quiz.date).format("YYYY/MM/DD")} ${quiz.time}`,
        DueDateTimezone: quiz.timezone,
        Duration: quiz.duration,
      };
    }),
  };

  const endpoint = Settings.getTeacherLessonsModifyAPI();
  const response = await axios.post(endpoint, data);

  return Promise.resolve(response.status === 200);
};

const getLessonQuizzes = async (lessonId) => {
  try {
    const endpoint = Settings.getLessonExamsTeachersAPI(lessonId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const getQuizPreview = async (quizId, lessonId) => {
  try {
    const endpoint = Settings.getLessonPreviewHtmlAPI(lessonId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getQuizzes,
  getLessonQuizzes,
  deleteQuiz,
  createQuiz,
  updateQuiz,
  getQuizPreview,
};
