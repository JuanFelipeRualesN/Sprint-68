import { useState } from "react";
import * as lessons from "../services/open-lessons.service";

const useCreateOpenLesson = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createOpenLesson = async (lesson) => {
    setIsLoading(true);

    try {
      const response = await lessons.createOpenLesson(lesson);

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
    createOpenLesson,
    isLoading,
    succeeded,
  };
};

export default useCreateOpenLesson;
