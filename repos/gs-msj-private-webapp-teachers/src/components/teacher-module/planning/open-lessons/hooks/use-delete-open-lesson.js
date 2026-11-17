import { useState } from "react";
import * as lessons from "../services/open-lessons.service";

const useDeleteOpenLesson = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteOpenLesson = async (lessonId) => {
    setIsLoading(true);

    try {
      const response = await lessons.deleteOpenLesson(lessonId);

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
    deleteOpenLesson,
    isLoading,
    succeeded,
  };
};

export default useDeleteOpenLesson;
