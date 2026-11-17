import axios from "./interceptors";
import Settings from "../custom-classes/settings";

export const getSubjectsAndCourses = () => {
  return axios.get(Settings.getCoursesAPI());
};