import { useState } from "react";
import * as projects from "../services/projects.service";

const useCreateProject = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const createProject = async (project) => {
    setIsLoading(true);

    try {
      const response = await projects.createProject(project);

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
    createProject,
    isLoading,
    succeeded,
  };
};

export default useCreateProject;
