import { useState } from "react";
import * as quizzes from "../services/quizzes.service";

const useUpdateQuiz = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateQuiz = async (request) => {
    setIsLoading(true);

    try {
      const response = await quizzes.updateQuiz(request);

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
    updateQuiz,
    isLoading,
    succeeded,
  };
};

export default useUpdateQuiz;
