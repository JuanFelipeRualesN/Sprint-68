import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetAssignments = () => {
  const [assignments, setAssignments] = useState([]);

  const getAssignments = async () => {
    try {
      const endpoint = Settings.getTeacherQuizzesAPI(true);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : [];
      const assignments = data.QuizModelList.map((assignment) => ({
        Id: assignment.Id,
        AssignmentName: assignment.Title,
      }));
      setAssignments(assignments);
    } catch (error) {
      setAssignments(Promise.resolve([]));
    }
  };

  return {
    getAssignments,
    assignments,
  };
};

export default useGetAssignments;
