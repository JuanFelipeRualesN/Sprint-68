import { useState } from "react";
import * as evaluations from "../services/evaluations.service";

const useUpdateEvaluation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateEvaluation = async (evaluation) => {
    setIsLoading(true);

    try {
      const response = await evaluations.updateEvaluation(evaluation);

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
    updateEvaluation,
    isLoading,
    succeeded,
  };
};

export default useUpdateEvaluation;
