import { useState } from "react";
import * as forums from "../services/forums.service";

const useGetForums = () => {
  const [data, setData] = useState({TotalItems: 0, Items: []});
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getForums = async (planningId, limit, pageNumber) => {
    setIsLoading(true);

    try {
      const response = await forums.getForums(planningId, limit, pageNumber);
      const { TotalItems } = response;
      const results = response.ForumModelList.map((forum) => {
        return {
          id: forum.Id,
          planningId,
          title: forum.Title,
          description: forum.Description,
          implementationDates: forum.ImplementationDates.map(
            (i) => new Date(i.ImplementationDate)
          ),
        };
      });

      setSucceeded(true);
      setData({TotalItems: TotalItems, Items: results});
    } catch (error) {
      setSucceeded(false);
      setData({TotalItems: 0, Items: []});
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

export default useGetForums;
