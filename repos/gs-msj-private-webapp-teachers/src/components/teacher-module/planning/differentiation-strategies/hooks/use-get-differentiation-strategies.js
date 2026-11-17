import { useState } from "react";
import * as diffStrategies from "../services/differentiation-strategies.service";

const useGetDifferentiationStrategies = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getDifferentiationStrategyTypes = (differentiationStrategy, type) => {
    return differentiationStrategy.TeacherPlanDifferentiationStrategyTypes?.filter(
      (p) => p.DifferentiationStrategyType?.Type === type
    );
  };

  const getDifferentiationStrategyTitle = (types) => {
    return types?.map((type) => type.DifferentiationStrategyType?.Description)?.join(", ");
  };

  const getDifferentiationStrategyCategory = (categories) => {
    return categories?.map((category) => category.DifferentiationStrategyCategory?.Name)?.join(", ")
  };

  const getDifferentiationStrategyCategoryId = (categories) => {
    return categories?.map((category) => category.DifferentiationStrategyCategory?.Id)?.join(", ")
  }

  const getDifferentiationStrategies = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await diffStrategies.getDifferentiationStrategies(planningId);
      const results = response.map((differentiationStrategy) => {
        const defaults = getDifferentiationStrategyTypes(differentiationStrategy, 0);
        const customs = getDifferentiationStrategyTypes(differentiationStrategy, 1);

        return {
          id: differentiationStrategy.Id,
          category:
            getDifferentiationStrategyCategory(differentiationStrategy.TeacherPlanDifferentiationStrategyCategories),
          categoryId:
            getDifferentiationStrategyCategoryId(differentiationStrategy.TeacherPlanDifferentiationStrategyCategories),
          title: getDifferentiationStrategyTitle(defaults),
          subtitle: getDifferentiationStrategyTitle(customs),
          defaults: defaults?.map((type) => type.DifferentiationStrategyTypesId),
          customs: customs?.map((type) => ({
            value: type.DifferentiationStrategyTypesId,
            label: type.DifferentiationStrategyType?.Description,
          })),          
          types: differentiationStrategy.TeacherPlanDifferentiationStrategyTypes.map(
            (type) => type.DifferentiationStrategyTypesId
          ),
          implementationDates: differentiationStrategy.TeacherPlanDifferentiationStrategyDates?.map(
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
    getDifferentiationStrategies,
    isLoading,
    succeeded,
    differentiationStrategies: data,
  };
};

export default useGetDifferentiationStrategies;
