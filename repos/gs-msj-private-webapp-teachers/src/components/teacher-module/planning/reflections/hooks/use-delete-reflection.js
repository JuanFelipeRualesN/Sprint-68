import { useState } from "react";
import * as reflections from "../services/reflections.service";

const useDeleteReflection = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteReflection = async (reflectionId) => {
    setIsLoading(true);

    try {
      const response = await reflections.deleteReflection(reflectionId);

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
    deleteReflection,
    isLoading,
    succeeded,
  };
};

export default useDeleteReflection;
