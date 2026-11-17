import { useState } from "react";
import * as projects from "../services/projects.service";

const useDeleteProject = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteProject = async (projectId) => {
    setIsLoading(true);

    try {
      const response = await projects.deleteProject(projectId);

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
    deleteProject,
    isLoading,
    succeeded,
  };
};

export default useDeleteProject;
