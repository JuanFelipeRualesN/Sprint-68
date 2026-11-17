import { useState } from "react";
import * as observations from "../services/observations.service";

const useDeleteObservations = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteObservation = async (observationId) => {
    setIsLoading(true);

    try {
      const response = await observations.deleteObservation(observationId);

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
    deleteObservation,
    isLoading,
    succeeded,
  };
};

export default useDeleteObservations;
