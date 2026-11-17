import { useState } from "react";
import * as diffStrategies from "../../differentiation-strategies/services/differentiation-strategies.service";
import { format, translate } from "../../shared/utils";

const useGetDifferentiationStrategiesForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getDifferentiationStrategies = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await diffStrategies.getDifferentiationStrategies(planningId);
      const results = response.flatMap((differentiationStrategy) => {
        return differentiationStrategy.TeacherPlanDifferentiationStrategyDates.map((date) => ({
          id: differentiationStrategy.Id,
          type: "default",
          title: translate("preview.table-differentiation-strategies"),
          description: differentiationStrategy.Description,
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
    getDifferentiationStrategies,
    isLoading,
    succeeded,
    differentiationStrategies: data,
  };
};

export default useGetDifferentiationStrategiesForPreview;
