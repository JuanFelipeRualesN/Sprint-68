import { useState } from "react";
import * as activeResponses from "../../active-responses/services/active-responses.service";
import { format, translate } from "../../shared/utils";

const useGetActiveResponsesForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getActiveResponses = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await activeResponses.getActiveResponses(planningId);
      const results = response.flatMap((activeResponse) => {
        return activeResponse.TeacherPlanActiveResponseDates.map((date) => ({
          id: activeResponse.Id,
          type: "default",
          title: translate("preview.table-active-responses"),
          description: activeResponse.Description,
          date: format(date.ImplementationDate),
        }));
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

export default useGetActiveResponsesForPreview;
