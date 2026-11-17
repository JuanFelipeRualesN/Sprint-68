import { useState } from "react";
import * as materials from "../services/materials.service";

const useCreateMaterial = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createMaterial = async (material) => {
    setIsLoading(true);

    try {
      const response = await materials.createMaterial(material);

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
    createMaterial,
    isLoading,
    succeeded,
  };
};

export default useCreateMaterial;
