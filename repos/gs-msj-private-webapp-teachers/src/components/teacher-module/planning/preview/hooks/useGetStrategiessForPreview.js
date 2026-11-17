import { useState } from "react";
import * as strategies from "../../strategies/services/strategies.service";
import { format } from "../../shared/utils";

const useGetStrategiesForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getStrategies = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await strategies.getStrategies(planningId);
      const results = response.flatMap((strategy) => {
        return strategy.TeacherPlanStrategyDates.map((date) => ({
          id: strategy.Id,
          type: "default",
          title:
            strategy.TeacherPlanStrategyCategories[0]?.StrategyCategory?.Name,
          description: strategy.TeacherPlanStrategyTypes.map(
            (type) => type.StrategyType?.Name
          ).join(", "),
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
    getStrategies,
    isLoading,
    succeeded,
    strategies: data,
  };
};

export default useGetStrategiesForPreview;
