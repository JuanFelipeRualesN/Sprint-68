import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetLessonDetails = () => {
  const [lessonDetail, setLessonDetail] = useState();

  const getLessonHtmlDetail = async (lessonId) => {
    try {
      const endpoint = Settings.getLessonPreviewHtmlAPI(lessonId);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : null;

      setLessonDetail(data);
    } catch (error) {}
  };

  return {
    getLessonHtmlDetail,
    lessonDetail,
  };
};

export default useGetLessonDetails;
