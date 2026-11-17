import { useState } from "react";
import * as lessons from "../services/open-lessons.service";

const useGetOpenLessonCriterias = () => {
  const [data, setData] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getOpenLessonCriterias = async () => {
    setIsLoading(true);

    try {
      const response = await lessons.getCriterias();
      const results = response.map((criteria) => {
        return {
          id: criteria.Id,
          name: criteria.Description,
          description: criteria.Description,
          type: criteria.Type,
        };
      });
      const defaults = results.filter((r) => r.type === 0);
      const customs = results.filter((r) => r.type === 1);

      setSucceeded(true);
      setData({ defaults, customs });
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getOpenLessonCriterias,
    isLoading,
    succeeded,
    criterias: data,
  };
};

export default useGetOpenLessonCriterias;
