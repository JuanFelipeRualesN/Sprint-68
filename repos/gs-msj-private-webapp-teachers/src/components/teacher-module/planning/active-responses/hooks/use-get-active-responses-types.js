import { useState } from "react";
import * as actResponse from "../services/active-responses.service";

const useGetActiveResponseTypes = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getActiveResponseTypes = async () => {
    setIsLoading(true);

    try {
      const response = await actResponse.getTypes();
      const results = response.map((type) => {
        return {
          id: type.Id,
          name: type.Name,
          description: type.Description,
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
    getActiveResponseTypes,
    isLoading,
    succeeded,
    types: data,
  };
};

export default useGetActiveResponseTypes;