import { useState } from "react";
import * as observations from "../services/observations.service";

const useGetObservations = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getObservations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await observations.getObservations(planningId);
      const results = response.map((observation) => {
        return {
          id: observation.Id,
          detail: observation.Detail,
          achieved: observation.IsCompleted,
          implementationDates: observation.TeacherPlanObservationDates.map(
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
    getObservations,
    isLoading,
    succeeded,
    observations: data,
  };
};

export default useGetObservations;