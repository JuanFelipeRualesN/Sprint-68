import { useState } from "react";
import * as diffStrategies from "../services/differentiation-strategies.service";

const useCreateDifferentiationStrategy = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createDifferentiationStrategy = async (differentiationStrategy) => {
    setIsLoading(true);

    try {
      const response = await diffStrategies.createDifferentiationStrategy(differentiationStrategy);
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
    createDifferentiationStrategy,
    isLoading,
    succeeded,
  };
};

export default useCreateDifferentiationStrategy;
