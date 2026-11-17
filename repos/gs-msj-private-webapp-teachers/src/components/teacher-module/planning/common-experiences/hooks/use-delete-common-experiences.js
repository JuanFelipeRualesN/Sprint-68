import { useState } from "react";
import * as commonExperiences from "../services/common-experiences.service";

const useDeleteCommonExperiences = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteCommonExperience = async (commonExperienceId) => {
    setIsLoading(true);

    try {
      const response = await commonExperiences.deleteCommonExperience(commonExperienceId);

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
    deleteCommonExperience,
    isLoading,
    succeeded,
  };
};

export default useDeleteCommonExperiences;
