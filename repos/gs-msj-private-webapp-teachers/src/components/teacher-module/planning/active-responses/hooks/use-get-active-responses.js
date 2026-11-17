import { useState } from "react";
import * as actResponse from "../services/active-responses.service";

const useGetActiveResponses = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getActiveResponseTypes = (activeResponse, type) => {
    return activeResponse.TeacherPlanActiveResponseTypes?.filter(
      (p) => p.ActiveResponseType?.Type === type
    );
  };

  const getActiveResponseTitle = (types) => {
    return types?.map((type) => type.ActiveResponseType?.Description)?.join(", ");
  };

  const getActiveResponseCategory = (categories) => {
    return categories?.map((category) => category.ActiveResponseCategory?.Name)?.join(", ")
  };

  const getActiveResponseCategoryId = (categories) => {
    return categories?.map((category) => category.ActiveResponseCategory?.Id)?.join(", ")
  }

  const getActiveResponses = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await actResponse.getActiveResponses(planningId);
      const results = response.map((activeResponse) => {
        // const defaults = getActiveResponseTypes(activeResponse, 0);
        // const customs = getActiveResponseTypes(activeResponse, 1);

        return {
          id: activeResponse.Id,
          includesStudentActiveResponse: activeResponse.TeacherPlanActiveResponseIncludes?.[0]?.IncludesStudentActiveResponse || 0,
          includesLearningCheckActivity: activeResponse.TeacherPlanActiveResponseIncludes?.[0]?.IncludesLearningCheckActivity || 0,
          title: getActiveResponseTitle(activeResponse.TeacherPlanActiveResponseTypes),         
          activeResponseTypeIds: activeResponse.TeacherPlanActiveResponseTypes.map(
            (type) => type.ActiveResponseTypesId
          ),
          implementationDates: activeResponse.TeacherPlanActiveResponseDates?.map(
            (i) => new Date(i.ImplementationDate)
          ),
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
    getActiveResponses,
    isLoading,
    succeeded,
    activeResponses: data,
  };
};

export default useGetActiveResponses;
