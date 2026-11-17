import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetPlanningAssignments = () => {
  const [planningAssignments, setPlanningAssignments] = useState({TotalAssignments: 0, Assignments: []});
  const [isLoading, setIsLoading] = useState(false);
  const getPlanningAssignments = async (planningId, limit, pageNumber) => {
    try {
      setIsLoading(true);
      const endpoint = Settings.getTeacherPlanningDetailsPaginationAPI(planningId, limit, pageNumber);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : [];
      const totalAssignments = data.TotalAssignments || 0;
      setIsLoading(false);
      const assignments = data.QuizzesAttachedModelList.map((assignment) => ({
        Id: assignment.Id,
        AssignmentName: assignment.Title,
        QuizId: assignment.ItemId,
        DueDate: assignment.DueDate,
        ForGrading: assignment.ForGrading === "1",
        PlanningId: planningId,
      }));
      setPlanningAssignments({TotalAssignments: totalAssignments, Assignments: assignments});
    } catch (error) {
      setPlanningAssignments({TotalAssignments: 0, Assignments: []});
      setIsLoading(false);
      console.error("Error fetching planning assignments:", error);
    }
  };

  return {
    getPlanningAssignments,
    planningAssignments,
    isLoading,
  };
};

export default useGetPlanningAssignments;
