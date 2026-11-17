import { useState } from "react";
import * as reflections from "../services/reflections.service";

const useUpdateReflection = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateReflection = async (reflection) => {
    setIsLoading(true);

    try {
      const response = await reflections.updateReflection(reflection);

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
    updateReflection,
    isLoading,
    succeeded,
  };
};

export default useUpdateReflection;
