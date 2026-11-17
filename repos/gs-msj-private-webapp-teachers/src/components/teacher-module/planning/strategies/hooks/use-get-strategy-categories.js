import { useState } from "react";
import * as strategies from "../services/strategies.service";

const useGetStrategyCategories = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getStrategyCategories = async () => {
    setIsLoading(true);

    try {
      const response = await strategies.getCategories();
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
    getStrategyCategories,
    isLoading,
    succeeded,
    categories: data,
  };
};

export default useGetStrategyCategories;
