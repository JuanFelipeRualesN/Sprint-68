import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetSubjectsAndGrades = () => {
  const [subjectsAndGrades, setSubjectsAndGrades] = useState([]);

  const getSubjectsAndGrades = async () => {
    try {
      const endpoint = Settings.getSubjectsAPI();
      const response = await axios.get(endpoint);

      if (response.status !== 200) return [];

      const data = response.data.map((subject) => {
        return {
          id: subject.Id,
          name: subject.Name,
          code: subject.Code,
        };
      });

      setSubjectsAndGrades(data);
    } catch (error) {}
  };

  return {
    getSubjectsAndGrades,
    subjectsAndGrades,
  };
};

export default useGetSubjectsAndGrades;
