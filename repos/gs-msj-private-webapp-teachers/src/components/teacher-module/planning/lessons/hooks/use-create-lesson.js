import { useState } from "react";
import * as lessons from "../services/lessons.service";

const useCreateLesson = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const createLesson = async (lesson) => {
    setIsLoading(true);

    try {
      const response = await lessons.createLesson(lesson);

      setSucceeded(true);
      setIsLoading(false);
      return response; // Return the response data
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
    return null; // Return null in case of an error
  };

  return {
    createLesson,
    succeeded,
    isLoading,
  };
};

export default useCreateLesson;
