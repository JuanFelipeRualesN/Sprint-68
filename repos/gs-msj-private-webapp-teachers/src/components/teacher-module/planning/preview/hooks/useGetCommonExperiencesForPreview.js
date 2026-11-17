import { useState } from "react";
import * as comExperience from "../../common-experiences/services/common-experiences.service";
import { format, translate } from "../../shared/utils";

const useGetCommonExperienceForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getCommonExperience = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await comExperience.getCommonExperiences(planningId);
      const results = response.flatMap((commonExperience) => {

        return commonExperience.TeacherPlanCommonExperienceDates.map(
          (date) => ({
            id: commonExperience.Id,
            type: "default",
            title: translate("preview.table-common-experiences"),
            description: commonExperience.Detail,
            date: format(date.ImplementationDate),
          })
        );
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getCommonExperience,
    isLoading,
    succeeded,
    commonExperiences: data,
  };
};

export default useGetCommonExperienceForPreview;
