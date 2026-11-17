import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useDeletePlanningAssignment = () => {
  const [succeeded, setSucceeded] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState("");

  const deletePlanningAssignment = async (teacherPlanId, assignmentId) => {
    try {
      setSucceeded(false);
      setIsLoading(true);
      const endpoint = Settings.getArchiveQuizOfPlanningAPI(
        teacherPlanId,
        assignmentId
      );
      const response = await axios.post(endpoint);
      const result = response.status === 200;
      setIsLoading(false);
      setMessage(response.data.Messages[0]);
      setSucceeded(result);
      return Promise.resolve(result);
    } catch (error) {
      setIsLoading(false);
      setSucceeded(false);

      return false;
    }
  };

  return {
    deletePlanningAssignment,
    succeeded,
    message,
    isLoading,
  };
};

export default useDeletePlanningAssignment;
