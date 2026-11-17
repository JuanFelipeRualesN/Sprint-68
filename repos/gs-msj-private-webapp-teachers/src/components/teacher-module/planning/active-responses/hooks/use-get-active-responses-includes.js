import { useState } from "react";
import * as actResponse from "../services/active-responses.service";

const useGetActiveResponseIncludes = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getActiveResponseIncludes = async () => {
    setIsLoading(true);

    try {
      const response = await actResponse.getIncludes();
      const results = response.map((include) => {
        return {
          value: include.Id.toString(),
          label: include.Name,
          includesStudentActiveResponse: include.IncludesStudentActiveResponse,
          includesLearningCheckActivity: include.IncludesLearningCheckActivity
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
    getActiveResponseIncludes,
    isLoading,
    succeeded,
    includes: data,
  };
};

export default useGetActiveResponseIncludes;
