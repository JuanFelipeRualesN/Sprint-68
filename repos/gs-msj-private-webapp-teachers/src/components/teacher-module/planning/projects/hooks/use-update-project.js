import { useState } from "react";
import * as projects from "../services/projects.service";

const useUpdateProject = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const updateProject = async (project) => {
    setIsLoading(true);

    try {
      const response = await projects.updateProject(project);

      setSucceeded(response);
      setIsLoading(false);
      return Promise.resolve(response);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  return {
    updateProject,
    isLoading,
    succeeded,
  };
};

export default useUpdateProject;
