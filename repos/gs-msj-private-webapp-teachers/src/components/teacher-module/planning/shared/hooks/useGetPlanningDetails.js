import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetPlanningDetails = () => {
  const [planning, setPlanning] = useState({});
  const [isLoading, setIsLoading] = useState(false);

  const getPlanning = async (id) => {
    setIsLoading(true);
    try {
      const endpoint = Settings.getTeacherPlanningDetailsAPI(id);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : null;
      setIsLoading(false);

      if (data === null) return;

      const {
        TeacherPlanModel,
        AcademicPeriodFullDetail,
        GroupsAttachedList,
        LessonAttachedModelList,
        QuizzesAttachedModelList,
      } = data;
      const academicPeriod = AcademicPeriodFullDetail.AcademicPeriods[0];
      const academicDetail = AcademicPeriodFullDetail.AcademicDetail;

      const planning = {
        id: TeacherPlanModel.Id,
        name: TeacherPlanModel.Name,
        closure: academicPeriod.PeriodTo,
        openDate: TeacherPlanModel.OpenDate,
        closeDate: TeacherPlanModel.CloseDate,
        weekNumber: TeacherPlanModel.WeekNumber,
        timeframe: TeacherPlanModel.Timeframe,
        subjectAndGrade: TeacherPlanModel.SubjectName + " " + TeacherPlanModel.LevelCode,
        studentsGroup: GroupsAttachedList[0]?.GroupName,
        periodNumber: academicPeriod.PeriodNumber,
        periodFrom: academicPeriod.PeriodFrom,
        periodTo: academicPeriod.PeriodTo,
        academic: {
          fromDate: academicDetail.FullTermStartDate,
          toDate: academicDetail.FullTermEndDate,
        },
        planningGroups: GroupsAttachedList?.map((x) => ({
          GroupId: x.GroupId,
          GroupName: x.GroupName,
        })),
        periodId: TeacherPlanModel.PeriodId,
        subjectCode: TeacherPlanModel.SubjectCode,
        levelCode: TeacherPlanModel.LevelCode,
        lessons: LessonAttachedModelList?.map((lesson) => {
          return {
            id: lesson.Id,
            title: lesson.Title,
            lessonId: lesson.ItemId,
            dueDate: lesson.DueDate,
            dates: lesson.TeacherPlanLessonDateModels.map((date) => date.ImplementationDate),
          };
        }),
        assignments: QuizzesAttachedModelList?.map((assignment) => {
          return {
            id: assignment.Id,
            title: assignment.Title,
            quizId: assignment.ItemId,
            dueDate: assignment.DueDate,
            forGrading: assignment.ForGrading === "1",
          };
        }),
      };

      setPlanning(planning);
    } catch (error) { }
  };

  return {
    getPlanning,
    planning,
    isLoading,
  };
};

export default useGetPlanningDetails;
