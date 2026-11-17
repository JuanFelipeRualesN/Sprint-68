import { useState } from "react";
import * as quizzes from "../../quizzes/services/quizzes.service";
import { format } from "../../shared/utils";

const useGetQuizzesForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getQuizzes = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await quizzes.getQuizzes(planningId);
      const results = response.QuizzesItems.flatMap((lesson) => {
        return lesson.TeacherPlanLessonQuizModels.map((quiz) => {
          return {
            id: quiz.Id,
            quizId: quiz.QuizId,
            date: format(quiz.DueDate),
            title: lesson.TeacherPlanLessonModel.LessonTitle,
            description: quiz.QuizTitle,
            time: format(quiz.DueDate, "hh:mm A"),
          };
        });
      });

      setSucceeded(true);
      setData(results);
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

export default useGetQuizzesForPreview;
