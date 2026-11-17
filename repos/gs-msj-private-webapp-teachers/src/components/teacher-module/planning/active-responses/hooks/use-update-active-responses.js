import { useState } from "react";
import * as actResponse from "../services/active-responses.service";

const useUpdateActiveResponse = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateActiveResponse = async (activeResponse) => {
    setIsLoading(true);

    try {
      const response = await actResponse.updateActiveResponse(activeResponse);
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
    updateActiveResponse,
    isLoading,
    succeeded,
  };
};

export default useUpdateActiveResponse;
