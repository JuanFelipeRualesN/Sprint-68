import { useState } from "react";
import * as forums from "../services/forums.service";

const useUpdateForum = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const updateForum = async (forum) => {
    setIsLoading(true);

    try {
      const response = await forums.updateForum(forum);

      setSucceeded(response);
      setIsLoading(false);
      return Promise.resolve(response);
    } catch (error) {
      const data = error.response?.data;
      if (data) {
        setMessage(data.Messages[0]);
      }
      setSucceeded(false);
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  return {
    updateForum,
    isLoading,
    succeeded,
    message,
  };
};

export default useUpdateForum;
