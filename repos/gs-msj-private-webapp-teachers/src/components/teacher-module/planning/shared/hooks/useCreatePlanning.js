import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useCreatePlanning = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [planningId, setPlanningId] = useState("0");
  const [isLoading, setIsLoading] = useState(false);
  const [expiredDateResults, setExpiredDateResults] = useState([]);

  const createPlanning = async (planning) => {
    setIsLoading(true);

    try {
      const endpoint = Settings.getCreatePlanningAPI(planning.Id);
      const response = await axios.post(endpoint, planning);
      const result = response.status === 201;

      setPlanningId(response.data.Id);
      setSucceeded(false); // will only be set true manually
      setIsLoading(false);

      if (response.data.ExpiredDateResults && response.data.ExpiredDateResults.length > 0) {
        setExpiredDateResults(response.data.ExpiredDateResults);
      } else {
        setExpiredDateResults([]);
      }

      return response;
    } catch (error) {
      setSucceeded(false);
      setIsLoading(false);
      return null;
    }
  };

  return {
    createPlanning,
    succeeded,
    setSucceeded,
    planningId,
    isLoading,
    expiredDateResults,
    setExpiredDateResults, // optional, but useful
  };
};

export default useCreatePlanning;
