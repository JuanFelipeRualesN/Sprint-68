import { useState } from "react";
import * as lessons from "../services/open-lessons.service";

const useUpdateOpenLesson = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateOpenLesson = async (lesson) => {
    setIsLoading(true);

    try {
      const response = await lessons.updateOpenLesson(lesson);

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
    updateOpenLesson,
    isLoading,
    succeeded,
  };
};

export default useUpdateOpenLesson;
