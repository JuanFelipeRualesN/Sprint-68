import axios from "../../../../../api/interceptors";
import { useState } from "react";
import Settings from "../../../../../custom-classes/settings";

const useGetStudentsGroup = () => {
  const [studentsGroup, setStudentsGroup] = useState([]);

  const getStudentsGroup = async () => {
    try {
      const endpoint = Settings.getSubGroupStudentsAPI();
      const response = await axios.get(endpoint);

      if (response.status !== 200) return [];

      const data = response.data.TeacherGroupModelList?.map((subject) => {
        return {
          id: subject.Id,
          name: subject.Name,
        };
      });

      setStudentsGroup(data);
    } catch (error) {}
  };

  return {
    getStudentsGroup,
    studentsGroup,
  };
};

export default useGetStudentsGroup;
