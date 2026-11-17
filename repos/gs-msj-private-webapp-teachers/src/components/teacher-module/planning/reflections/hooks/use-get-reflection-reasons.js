import { useState } from "react";
import * as reflections from "../services/reflections.service";

const useGetReflectionsNotAchievingReasons = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getReflectionsNotAchievingReasons = async () => {
    setIsLoading(true);

    try {
      const response = await reflections.getNotAchievingReasons();
      const results = response.map((type) => {
        return {
          value: type.Id,
          label: type.Name,
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
    getReflectionsNotAchievingReasons,
    isLoading,
    succeeded,
    reasons: data,
  };
};

export default useGetReflectionsNotAchievingReasons;
