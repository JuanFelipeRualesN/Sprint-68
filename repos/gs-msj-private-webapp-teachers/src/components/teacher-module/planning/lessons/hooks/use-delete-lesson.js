import { useState } from "react";
import * as lessons from "../services/lessons.service";

const useDeleteLesson = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const deleteLesson = async (planningId, id) => {
    setIsLoading(true);

    try {
      const response = await lessons.deleteLesson(planningId, id);
     
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
    deleteLesson,
    succeeded,
    isLoading,
  };
};

export default useDeleteLesson;
