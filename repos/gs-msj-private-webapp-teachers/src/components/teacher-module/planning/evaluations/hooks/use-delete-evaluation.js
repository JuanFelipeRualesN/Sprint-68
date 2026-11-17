import { useState } from "react";
import * as evaluations from "../services/evaluations.service";

const useDeleteEvaluation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteEvaluation = async (evaluationId) => {
    setIsLoading(true);

    try {
      const response = await evaluations.deleteEvaluation(evaluationId);

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
    deleteEvaluation,
    isLoading,
    succeeded,
  };
};

export default useDeleteEvaluation;
