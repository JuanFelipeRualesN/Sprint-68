import { useState } from "react";
import * as lessons from "../services/lessons.service";

const useUpdateLesson = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const updateLesson = async (lesson) => {
    setIsLoading(true);

    try {
      const response = await lessons.updateLesson(lesson);

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
    updateLesson,
    succeeded,
    isLoading,
  };
};

export default useUpdateLesson;
