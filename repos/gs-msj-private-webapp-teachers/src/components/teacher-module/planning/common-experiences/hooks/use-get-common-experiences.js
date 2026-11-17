import { useState } from "react";
import * as commonExperiences from "../services/common-experiences.service";

const useGetCommonExperiences = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getCommonExperiences = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await commonExperiences.getCommonExperiences(planningId);
      const results = response.map((commonExperience) => {
        return {
          id: commonExperience.Id,
          detail: commonExperience.Detail,
          achieved: commonExperience.IsCompleted,
          implementationDates: commonExperience.TeacherPlanCommonExperienceDates.map(
            (i) => new Date(i.ImplementationDate)
          )
        };
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getCommonExperiences,
    isLoading,
    succeeded,
    commonExperiences: data,
  };
};

export default useGetCommonExperiences;