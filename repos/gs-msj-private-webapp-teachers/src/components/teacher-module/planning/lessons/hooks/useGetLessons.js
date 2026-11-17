import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetLessons = () => {
  const [lessons, setLessons] = useState([]);

  const getLessons = async (subjectCode, level) => {
    try {
      const endpoint = Settings.getLessonTeachersAPI(subjectCode, level);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : [];

      setLessons(data);
    } catch (error) {}
  };

  return {
    getLessons,
    lessons,
  };
};

export default useGetLessons;
