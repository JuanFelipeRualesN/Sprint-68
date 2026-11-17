import { useState } from "react";
import * as strategies from "../services/differentiation-strategies.service";

const useUpdateDifferentiationStrategy = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateDifferentiationStrategy = async (differentiationStrategy) => {
    setIsLoading(true);

    try {
      const response = await strategies.updateDifferentiationStrategy(differentiationStrategy);
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
    updateDifferentiationStrategy,
    isLoading,
    succeeded,
  };
};

export default useUpdateDifferentiationStrategy;
