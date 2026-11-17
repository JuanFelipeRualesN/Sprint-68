import { useState } from "react";
import * as evaluations from "../services/evaluations.service";

const useCreateEvaluation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createEvaluation = async (evaluation) => {
    setIsLoading(true);

    try {
      const response = await evaluations.createEvaluation(evaluation);
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
    createEvaluation,
    isLoading,
    succeeded,
  };
};

export default useCreateEvaluation;
