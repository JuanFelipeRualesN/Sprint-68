import { useState } from "react";
import * as accommodations from "../services/accommodations.service";

const useUpdateAccommodation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateAccommodation = async (accommodation) => {
    setIsLoading(true);

    try {
      const response = await accommodations.updateAccommodation(accommodation);

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
    updateAccommodation,
    isLoading,
    succeeded,
  };
};

export default useUpdateAccommodation;
