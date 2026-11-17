import { useState } from "react";
import * as diffStrategies from "../services/differentiation-strategies.service";

const useGetDifferentiationStrategyTypes = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getDifferentiationStrategyTypes = async () => {
    setIsLoading(true);

    try {
      const response = await diffStrategies.getTypes();
      const results = response.map((type) => {
        return {
          id: type.Id,
          name: type.Name,
          description: type.Description,
          type: type.Type,
          categoryId: type.DifferentiationStrategyCategoryId.toString(),
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
    getDifferentiationStrategyTypes,
    isLoading,
    succeeded,
    types: data,
  };
};

export default useGetDifferentiationStrategyTypes;