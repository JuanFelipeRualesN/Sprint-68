import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getManyLessonDetail = async (lessonsId) => {
  try {
    const responses = await Promise.all(
      lessonsId.map((lessonId) => {
        const endpoint = Settings.getLessonDetailsAPI(lessonId);
        return axios.post(endpoint, {
          lessonId: lessonId
        });
      })
    );

    return responses.map((response) => response.data);
  } catch (error) {
    return Promise.resolve([]);
  }
};

const getPreviewPdf = async (planningId, signal) => {
  try {
    const endpoint = Settings.getPreviewPdfAPI(planningId);
    const response = await axios.get(endpoint, { signal });
    return response.data;
  } catch (error) {
    return Promise.resolve({});
  }
};

export { getManyLessonDetail, getPreviewPdf };
