import { useState } from "react";
import * as diffStrategies from "../services/differentiation-strategies.service";

const useDeleteDifferentiationStrategy = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteDifferentiationStrategy = async (differentiationStrategyId) => {
    setIsLoading(true);

    try {
      const response = await diffStrategies.deleteDifferentiationStrategy(differentiationStrategyId);

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
    deleteDifferentiationStrategy,
    isLoading,
    succeeded,
  };
};

export default useDeleteDifferentiationStrategy;
