import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useDeletePlanning = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deletePlanning = async (id) => {
    setIsLoading(true);

    try {
      const endpoint = Settings.getArchivePlanningAPI(id);
      const response = await axios.post(endpoint);
      const result = response.status === 200;

      setSucceeded(result);
      setIsLoading(false);
      return Promise.resolve(result);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  return {
    deletePlanning,
    succeeded,
    isLoading,
  };
};

export default useDeletePlanning;
