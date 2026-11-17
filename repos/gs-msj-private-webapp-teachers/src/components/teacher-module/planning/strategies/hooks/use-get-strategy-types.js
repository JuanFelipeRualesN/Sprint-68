import { useState } from "react";
import * as strategies from "../services/strategies.service";

const useGetStrategyTypes = () => {
  const [data, setData] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getStrategyTypes = async () => {
    setIsLoading(true);

    try {
      const response = await strategies.getTypes();
      const results = response.map((type) => {
        return {
          id: type.Id,
          name: type.Name,
          description: type.Description,
          type: type.Type,
          categoryId: type.StrategyCategoryId.toString(),
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
    getStrategyTypes,
    isLoading,
    succeeded,
    types: data,
  };
};

export default useGetStrategyTypes;
