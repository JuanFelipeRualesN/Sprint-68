import { useState } from "react";
import * as reflections from "../services/reflections.service";

const useGetReflections = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getReflections = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await reflections.getReflections(planningId);
      const results = response.map((reflection) => {
        return {
          id: reflection.Id,
          detail: reflection.Detail,
          achieved: reflection.IsCompleted == 1,
          implementationDates: reflection.TeacherPlanPraxisReflectionDates.map(
            (i) => new Date(i.ImplementationDate)
          ),
          reasons: reflection.TeacherPlanPraxisReflectionCompletedReasons?.map(
            (reason) => reason.PraxisReflectionCompletedReasonId
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
    getReflections,
    isLoading,
    succeeded,
    reflections: data,
  };
};

export default useGetReflections;
