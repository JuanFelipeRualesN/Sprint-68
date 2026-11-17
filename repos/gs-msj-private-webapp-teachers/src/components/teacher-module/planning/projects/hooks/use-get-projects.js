import { useState } from "react";
import * as projects from "../services/projects.service";

const useGetProjects = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getProjectTypes = (project, type) => {
    return project.TeacherPlanInitiativeProjectInnovationTypes?.filter(
      (p) => p.InitiativeProjectInnovationType?.Type === type
    );
  };

  const getProjectTitle = (types) => {
    return types
      ?.map((type) => type.InitiativeProjectInnovationType?.Description)
      ?.join(", ");
  };

  const getProjects = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await projects.getProjects(planningId);
      const results = response.map((project) => {
        const defaults = getProjectTypes(project, 0);
        const customs = getProjectTypes(project, 1);

        return {
          id: project.Id,
          title: getProjectTitle(defaults),
          subtitle: getProjectTitle(customs),
          defaults: defaults?.map(
            (type) => type.InitiativeProjectInnovationTypeId
          ),
          customs: customs?.map((type) => ({
            value: type.InitiativeProjectInnovationTypeId,
            label: type.InitiativeProjectInnovationType?.Description,
          })),
          implementationDates: project.TeacherPlanInitiativeProjectInnovationDates.map(
            (i) => new Date(i.ImplementationDate)
          ),
        };
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

export default useGetProjects;
