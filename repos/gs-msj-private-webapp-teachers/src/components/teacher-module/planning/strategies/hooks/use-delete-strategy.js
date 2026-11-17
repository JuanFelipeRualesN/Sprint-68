import { useState } from "react";
import * as strategies from "../services/strategies.service";

const useDeleteStrategy = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteStrategy = async (strategyId) => {
    setIsLoading(true);

    try {
      const response = await strategies.deleteStrategy(strategyId);

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
    deleteStrategy,
    isLoading,
    succeeded,
  };
};

export default useDeleteStrategy;
