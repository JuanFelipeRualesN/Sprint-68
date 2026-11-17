import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useModifyPlanningAssignment = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState("");

  const modifyPlanningAssignment = async (planningAssignment) => {
    try {
      setSucceeded(false);
      setIsLoading(true);
      const endpoint = Settings.getModifyTeacherQuizAPI();
      const response = await axios.post(endpoint, {
        ...planningAssignment,
        ForGrading: planningAssignment.ForGrading ? 1 : 0,
      });
      const result = response.status === 200;
      setMessage(response.data.Messages[0]);
      setSucceeded(result);
      setIsLoading(false);

      return true;
    } catch (error) {
      setSucceeded(false);
      setIsLoading(false);
      return false;
    }
  };

  return {
    modifyPlanningAssignment,
    succeeded,
    message,
    isLoading,
  };
};

export default useModifyPlanningAssignment;
