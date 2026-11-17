import { useState } from "react";
import * as integrations from "../services/integrations.service";

const useCreateIntegration = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createIntegration = async (integration) => {
    setIsLoading(true);

    try {
      const response = await integrations.createIntegration(integration);

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
    createIntegration,
    isLoading,
    succeeded,
  };
};

export default useCreateIntegration;
