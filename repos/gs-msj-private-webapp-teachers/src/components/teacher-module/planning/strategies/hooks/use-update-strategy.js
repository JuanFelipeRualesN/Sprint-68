import { useState } from "react";
import * as strategies from "../services/strategies.service";

const useUpdateStrategy = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateStrategy = async (strategy) => {
    setIsLoading(true);

    try {
      const response = await strategies.updateStrategy(strategy);

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
    updateStrategy,
    isLoading,
    succeeded,
  };
};

export default useUpdateStrategy;
