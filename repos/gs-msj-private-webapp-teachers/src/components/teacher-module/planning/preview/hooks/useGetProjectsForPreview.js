import { useState } from "react";
import * as projects from "../../projects/services/projects.service";
import { format, translate } from "../../shared/utils";

const useGetProjectsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getProjects = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await projects.getProjects(planningId);
      const results = response.flatMap((project) => {
        return project.TeacherPlanInitiativeProjectInnovationDates.map(
          (date) => ({
            id: project.Id,
            type: "default",
            title: translate("preview.table-projects"),
            description: project.Description,
            date: format(date.ImplementationDate),
          })
        );
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getProjects,
    isLoading,
    succeeded,
    projects: data,
  };
};

export default useGetProjectsForPreview;
