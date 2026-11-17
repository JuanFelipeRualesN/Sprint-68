import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetPlannings = () => {
  const [plannings, setPlannings] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  const getPlannings = async (filters) => {
    try {
      setIsLoading(true);
      const endpoint = Settings.getTeacherPlanningAPI();
      const response = await axios.get(endpoint, { params: filters });
      setIsLoading(false);

      if (response.status !== 200) return [];

      const results = response.data.map((planning) => {
        return {
          id: planning.PlanId,
          name: planning.PlanName,
          closure:
            planning.AcademicPeriodFullDetail?.AcademicPeriods[0].PeriodTo,
          openDate: planning.OpenDate,
          closeDate: planning.CloseDate,
          subjectAndGrade: planning.SubjectName,
          studentsGroups: planning.TeacherGroups,
          subjectCode: planning.SubjectCode,
          levelCode: planning.LevelCode
        };
      });

      setPlannings(results);
    } catch (error) {}
  };

  return {
    getPlannings,
    plannings,
    isLoading,
  };
};

export default useGetPlannings;
