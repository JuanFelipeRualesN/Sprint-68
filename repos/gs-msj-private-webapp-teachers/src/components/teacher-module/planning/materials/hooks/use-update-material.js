import { useState } from "react";
import * as materials from "../services/materials.service";

const useUpdateMaterial = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateMaterial = async (material) => {
    setIsLoading(true);

    try {
      const response = await materials.updateMaterial(material);

      setSucceeded(response);
      setIsLoading(false);
      return Promise.resolve(response);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  return {
    updateMaterial,
    isLoading,
    succeeded,
  };
};

export default useUpdateMaterial;
