import { useState } from "react";
import * as quizzes from "../services/quizzes.service";
import moment from "moment";

const useGetQuizzes = () => {
  const [data, setData] = useState( {TotalQuizzes: 0, quizzesLessons: [] });
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getQuizzes = async (planningId, limit, pageNumber) => {
    setIsLoading(true);

    try {
      const response = await quizzes.getQuizzes(planningId, limit, pageNumber);
      const { TotalQuizzes } = response;
      const results = response.QuizzesItems.flatMap((lesson) => {
        return lesson.TeacherPlanLessonQuizModels.map((quiz) => {
            return {
            id: quiz.Id,
            quizId: quiz.QuizId,
            planLessonId: lesson.TeacherPlanLessonModel.Id,
            lessonId: lesson.TeacherPlanLessonModel.LessonId,
            lesson: lesson.TeacherPlanLessonModel.LessonTitle,
            lessonDueDate: lesson.TeacherPlanLessonModel.DueDate,
            name: quiz.QuizTitle,
            date: moment(quiz.DueDate).format("YYYY/MM/DD"),
            time: moment(quiz.DueDate).format("hh:mm A"),
            timezone: quiz.DueDateTimezone,
            countForGrade: quiz.ForGrading === "1",
            duration: quiz.Duration,
          };
        });
      });
      
      setSucceeded(true);
      setData({
        TotalQuizzes: TotalQuizzes,
        quizzesLessons: results,
      });
    } catch (error) {
      setSucceeded(false);
      setMessage("An error occur while getting quizzes");
    }

    setIsLoading(false);
  };

  return {
    getQuizzes,
    isLoading,
    succeeded,
    message,
    quizzes: data,
  };
};

export default useGetQuizzes;
