import axios from "axios";
import Settings from "../custom-classes/settings";
import SessionManager from "../custom-classes/session-manager";
import LanguageManager from "../custom-classes/language-manager";

export const getReportLesson = ({ teacherPlanId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate }) => {
  const url = Settings.getReportsLessonsPlan({ teacherPlanId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate });
  return axios({
    method: "GET",
    url,
    headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), 'Accept-Language': LanguageManager.getLanguage() },
  })
}

export const getReportAssignments = ({ teacherPlanId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate }) => {
  const url = Settings.getReportsAssignmentsPlan({ teacherPlanId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate });
  return axios({
    method: "GET",
    url,
    headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), 'Accept-Language': LanguageManager.getLanguage() },
  })
}

export const getReportsGroupAverage = (teacherPlanId, isAssignment) => {
  const url = Settings.getReportsGroupAveragePlan(teacherPlanId, isAssignment);
  return axios({
    method: "GET",
    url,
    headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), 'Accept-Language': LanguageManager.getLanguage() },
  })
}