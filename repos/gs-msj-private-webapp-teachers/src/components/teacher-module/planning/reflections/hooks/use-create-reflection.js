import { useState } from "react";
import * as reflections from "../services/reflections.service";

const useCreateReflection = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createReflection = async (reflection) => {
    setIsLoading(true);

    try {
      const response = await reflections.createReflection(reflection);

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
    createReflection,
    isLoading,
    succeeded,
  };
};

export default useCreateReflection;
