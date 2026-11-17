import { useState } from "react";
import * as diffStrategies from "../services/differentiation-strategies.service";

const useGetDifferentiationStrategyCategories = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getDifferentiationStrategyCategories = async () => {
    setIsLoading(true);

    try {
      const response = await diffStrategies.getCategories();
      const results = response.map((type) => {
        return {
          value: type.Id.toString(),
          label: type.Name,
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
    getDifferentiationStrategyCategories,
    isLoading,
    succeeded,
    categories: data,
  };
};

export default useGetDifferentiationStrategyCategories;
