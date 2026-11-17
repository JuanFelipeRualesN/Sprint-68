import { useState } from "react";
import * as strategies from "../services/strategies.service";

const useGetStrategies = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getStrategyTypes = (strategy, type) => {
    return strategy.TeacherPlanStrategyTypes?.filter(
      (p) => p.StrategyType?.Type === type
    );
  };

  const getStrategyTitle = (types) => {
    return types?.map((type) => type.StrategyType?.Description)?.join(", ");
  };

  const getStrategies = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await strategies.getStrategies(planningId);
      const results = response.map((strategy) => {
        const defaults = getStrategyTypes(strategy, 0);
        const customs = getStrategyTypes(strategy, 1);

        return {
          id: strategy.Id,
          category:
            strategy.TeacherPlanStrategyCategories[0]?.StrategyCategory?.Name,
          categoryId:
          strategy.TeacherPlanStrategyCategories[0]?.StrategyCategory?.Id.toString(),
          title: getStrategyTitle(defaults),
          subtitle: getStrategyTitle(customs),
          defaults: defaults?.map((type) => type.StrategyTypeId),
          customs: customs?.map((type) => ({
            value: type.StrategyTypeId,
            label: type.StrategyType?.Description,
          })),          
          types: strategy.TeacherPlanStrategyTypes.map(
            (type) => type.StrategyTypeId
          ),
          implementationDates: strategy.TeacherPlanStrategyDates?.map(
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
    getStrategies,
    isLoading,
    succeeded,
    strategies: data,
  };
};

export default useGetStrategies;
