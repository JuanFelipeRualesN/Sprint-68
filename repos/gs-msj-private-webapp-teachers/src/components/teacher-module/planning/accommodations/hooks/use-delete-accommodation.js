import { useState } from "react";
import * as accommodations from "../services/accommodations.service";

const useDeleteAccommodation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteAccommodation = async (accommodationId) => {
    setIsLoading(true);

    try {
      const response = await accommodations.deleteAccommodation(
        accommodationId
      );

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
    deleteAccommodation,
    isLoading,
    succeeded,
  };
};

export default useDeleteAccommodation;
