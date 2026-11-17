import { useState } from "react";
import * as quizzes from "../services/quizzes.service";

const useGetLessonQuizzes = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getLessonQuizzes = async (lessonId) => {
    if (!lessonId) {
      setData([]);
      return;
    }

    setIsLoading(true);

    try {
      const response = await quizzes.getLessonQuizzes(lessonId);
      const results = response.map((quiz) => {
        return {
          quizId: quiz.Id,
          lessonId,
          name: quiz.Title,
          countForGrade: true,
        };
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
    getLessonQuizzes,
    isLoading,
    succeeded,
    message,
    quizzes: data,
  };
};

export default useGetLessonQuizzes;
