import { useState } from "react";
import * as reflections from "../../reflections/services/reflections.service";
import { format, translate } from "../../shared/utils";

const useGetReflectionsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getReflections = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await reflections.getReflections(planningId);
      const results = response.flatMap((reflection) => {

        // Use custom description if available
        let description = reflection.Detail;
        
        if (!description) {         
          //Join reflections
          description = reflection.TeacherPlanPraxisReflectionCompletedReasons
            .filter(reason => reason.PraxisReflectionCompletedReason && 
                   reason.PraxisReflectionCompletedReason.Description)
            .map(reason => reason.PraxisReflectionCompletedReason.Description)
            .join(", ");
        }

        return reflection.TeacherPlanPraxisReflectionDates.map(
          (date) => ({
            id: reflection.Id,
            type: "default",
            title: translate("preview.table-reflections"),
            description,
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
    getReflections,
    isLoading,
    succeeded,
    reflections: data,
  };
};

export default useGetReflectionsForPreview;
