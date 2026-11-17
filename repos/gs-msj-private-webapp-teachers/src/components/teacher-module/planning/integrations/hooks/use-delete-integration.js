import { useState } from "react";
import * as integrations from "../services/integrations.service";

const useDeleteintegration = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteIntegration = async (integrationId) => {
    setIsLoading(true);

    try {
      const response = await integrations.deleteIntegration(integrationId);

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
    deleteIntegration,
    isLoading,
    succeeded,
  };
};

export default useDeleteintegration;
