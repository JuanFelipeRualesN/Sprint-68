import { useState } from "react";
import * as integrations from "../services/integrations.service";

const useUpdateIntegration = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateIntegration = async (integration) => {
    setIsLoading(true);

    try {
      const response = await integrations.updateIntegration(integration);

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
    updateIntegration,
    isLoading,
    succeeded,
  };
};

export default useUpdateIntegration;
