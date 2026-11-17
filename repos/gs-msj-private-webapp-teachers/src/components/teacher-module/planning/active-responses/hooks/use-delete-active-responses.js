import { useState } from "react";
import * as actResponse from "../services/active-responses.service";

const useDeleteActiveResponse = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteActiveResponse = async (activeResponseId) => {
    setIsLoading(true);

    try {
      const response = await actResponse.deleteActiveResponse(activeResponseId);

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
    deleteActiveResponse,
    isLoading,
    succeeded,
  };
};

export default useDeleteActiveResponse;
