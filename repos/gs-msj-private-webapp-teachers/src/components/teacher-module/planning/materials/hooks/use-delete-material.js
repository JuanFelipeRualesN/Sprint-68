import { useState } from "react";
import * as materials from "../services/materials.service";

const useDeleteMaterial = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteMaterial = async (materialId) => {
    setIsLoading(true);

    try {
      const response = await materials.deleteMaterial(materialId);

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
    deleteMaterial,
    isLoading,
    succeeded,
  };
};

export default useDeleteMaterial;
