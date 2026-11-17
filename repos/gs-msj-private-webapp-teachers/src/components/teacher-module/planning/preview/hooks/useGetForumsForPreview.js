import { useState } from "react";
import * as forums from "../../forums/services/forums.service";
import { format } from "../../shared/utils";

const useGetForumsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getForums = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await forums.getForums(planningId);
      const results = response.ForumModelList.flatMap((forum) => {
        return forum.ImplementationDates.map((date) => ({
          id: forum.Id,
          type: "default",
          title: forum.Title,
          description: forum.Description,
          date: format(date.ImplementationDate),
        }));
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
      setMessage("An error occur while getting forums");
    }

    setIsLoading(false);
  };

  return {
    getForums,
    isLoading,
    succeeded,
    message,
    forums: data,
  };
};

export default useGetForumsForPreview;
