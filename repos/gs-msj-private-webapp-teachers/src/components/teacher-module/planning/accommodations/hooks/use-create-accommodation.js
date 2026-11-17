import { useState } from "react";
import * as accommodations from "../services/accommodations.service";

const useCreateAccommodation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createAccommodation = async (accommodation) => {
    setIsLoading(true);

    try {
      const response = await accommodations.createAccommodation(accommodation);

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
    createAccommodation,
    isLoading,
    succeeded,
  };
};

export default useCreateAccommodation;
