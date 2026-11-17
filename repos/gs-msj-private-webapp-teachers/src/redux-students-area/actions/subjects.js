import { getSubscriptionCourses } from "../../api/subscriptions";
import { removeDuplicates } from "../../utils/utils";

export const getSubjectsSuccess = (subjects) => {
  return {
    type: "GET_SUBJECTS_SUCCESS",
    payload: subjects,
  };
};

export const subscriptionType = (type) => {
  return {
    type: "SUBSCRIPTION_TYPE",
    payload: type,
  };
};

export const getSubjectsAndCoursesThuk = () => {
  return async (dispatch, getState) => {
    try {
      const res = await getSubscriptionCourses();
      const courses = res.data;
      dispatch(getSubjectsSuccess(removeDuplicates(courses)));
      dispatch(subscriptionType(type));
    } catch {}
  };
};
