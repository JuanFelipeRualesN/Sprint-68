import { useState } from "react";
import * as observations from "../../observations/services/observations.service";
import { format, translate } from "../../shared/utils";

const useGetObservationsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getObservations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await observations.getObservations(planningId);
      const results = response.flatMap((observation) => {    

        return observation.TeacherPlanObservationDates.map(
          (date) => ({
            id: observation.Id,
            type: "default",
            title: translate("preview.table-observations"),
            description: observation.Detail,
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
    getObservations,
    isLoading,
    succeeded,
    observations: data,
  };
};

export default useGetObservationsForPreview;
