import { useState } from "react";
import * as materials from "../services/materials.service";

const useGetMaterialTypes = () => {
  const [data, setData] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getMaterialTypes = async () => {
    setIsLoading(true);

    try {
      const response = await materials.getTypes();
      const results = response.map((type) => {
        return {
          id: type.Id,
          name: type.Name,
          description: type.Description,
          type: type.Type,
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
    getMaterialTypes,
    isLoading,
    succeeded,
    types: data,
  };
};

export default useGetMaterialTypes;
