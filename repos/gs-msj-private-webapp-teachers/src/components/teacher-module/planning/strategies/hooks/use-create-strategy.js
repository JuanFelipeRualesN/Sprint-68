import { useState } from "react";
import * as strategies from "../services/strategies.service";

const useCreateStrategy = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createStrategy = async (strategy) => {
    setIsLoading(true);

    try {
      const response = await strategies.createStrategy(strategy);

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
    createStrategy,
    isLoading,
    succeeded,
  };
};

export default useCreateStrategy;
