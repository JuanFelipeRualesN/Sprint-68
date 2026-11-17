import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetAvailableSubjectsAndGrades = () => {
  const [availableSubjectsAndGrades, setAvailableSubjectsAndGrades] = useState(
    []
  );

  const getAvailableSubjectsAndGrades = async () => {
    try {
      const endpoint = Settings.getUserSubscriptionProductsAPI();
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : [];

      setAvailableSubjectsAndGrades(data);
    } catch (error) {}
  };

  return {
    getAvailableSubjectsAndGrades,
    availableSubjectsAndGrades,
  };
};

export default useGetAvailableSubjectsAndGrades;
