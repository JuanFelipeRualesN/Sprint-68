import { useState } from "react";
import * as observations from "../services/observations.service";

const useCreateObservations = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createObservation = async (observation) => {
    setIsLoading(true);

    try {
      const response = await observations.createObservation(observation);

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
    createObservation,
    isLoading,
    succeeded,
  };
};

export default useCreateObservations;
