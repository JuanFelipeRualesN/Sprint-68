import { useState } from "react";
import * as forums from "../services/forums.service";

const useDeleteForum = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const deleteForum = async (forumId) => {
    setIsLoading(true);

    try {
      const response = await forums.deleteForum(forumId);

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
    deleteForum,
    isLoading,
    succeeded,
  };
};

export default useDeleteForum;
