import { useState } from "react";
import * as quizzes from "../services/quizzes.service";

const useDeleteQuiz = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteQuiz = async (planningId, quizId) => {
    setIsLoading(true);

    try {
      const response = await quizzes.deleteQuiz(planningId, quizId);

      setSucceeded(response);
      setIsLoading(false);
      return Promise.resolve(response);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  return {
    deleteQuiz,
    isLoading,
    succeeded,
  };
};

export default useDeleteQuiz;
