import { useState } from "react";
import * as commonExperiences from "../services/common-experiences.service";

const useUpdateCommonExperiences = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateCommonExperience = async (commonExperience) => {
    setIsLoading(true);

    try {
      const response = await commonExperiences.updateCommonExperience(commonExperience);
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
    updateCommonExperience,
    isLoading,
    succeeded,
  };
};

export default useUpdateCommonExperiences;
