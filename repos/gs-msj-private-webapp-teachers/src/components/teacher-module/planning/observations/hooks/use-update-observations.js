import { useState } from "react";
import * as observations from "../services/observations.service";

const useUpdateObservations = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateObservation = async (observation) => {
    setIsLoading(true);

    try {
      const response = await observations.updateObservation(observation);
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
    updateObservation,
    isLoading,
    succeeded,
  };
};

export default useUpdateObservations;
