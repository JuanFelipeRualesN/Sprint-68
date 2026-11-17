import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useCreatePlanningAssignment = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [planningAssignmentId, setPlanningAssignmentId] = useState("0");

  const createPlanningAssignment = async (planningAssignment) => {
    try {
      setIsLoading(true);
      setSucceeded(false);
      const endpoint = Settings.getAttachQuizToPlanAPI();
      const response = await axios.post(endpoint, {
        ...planningAssignment,
        ForGrading: planningAssignment.ForGrading ? 1 : 0,
      });
      const result = response.status === 201;

      setPlanningAssignmentId(result ? response.data.Id : "");
      setMessage(response.data.Messages[0]);
      setSucceeded(result);
      setIsLoading(false);
      return true;
    } catch (error) {
      setIsLoading(false);
      setSucceeded(false);

      return false;
    }
  };

  return {
    createPlanningAssignment,
    succeeded,
    planningAssignmentId,
    message,
    isLoading,
  };
};

export default useCreatePlanningAssignment;
