import { getLessonsTeachers } from "../../api/teachers";
import loading from "./loading";

export const getLessonsSuccess = (lessons) => {
  return {
    type: "GET_LESSONS_SUCCESS",
    payload: lessons,
  };
};

export const getLessonsBySubjectsAndGradesThuk = (subjectCode, levelCode) => {
  return async (dispatch, getState) => {
    try {
      dispatch(loading(true));
      const res = await getLessonsTeachers(subjectCode, levelCode);
      const lessons = res.data;
      dispatch(getLessonsSuccess(lessons));
    } catch {
    } finally {
      dispatch(loading(false));
    }
  };
};
