import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";
import { getMinAndMaxDates } from "../../shared/utils";
import moment from 'moment';

const useGetPlanningLessons = () => {
  const [planningLesson, setPlanningLessons] = useState({ TotalLessons: 0, planningLessons: [] });
  const [isLoading, setIsLoading] = useState(false);

  const getPlanningLessons = async (teachPlanId, limit, pageNumber) => {
    try {
      setIsLoading(true);
      const endpoint = Settings.getTeacherPlanningDetailsPaginationAPI(teachPlanId, limit, pageNumber);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : {};
      setIsLoading(false);
      const { TotalLessons } = data;
      const planningLessons = data.LessonAttachedModelList?.map(
        (lessonAttached) => {
          const { minDate, maxDate } = getMinAndMaxDates(
            lessonAttached.TeacherPlanLessonDateModels.map((x) => new Date(x.ImplementationDate))
          );
          return {
            Id: lessonAttached.Id,
            LessonName: lessonAttached.Title,
            DueDate: moment(lessonAttached.DueDate).format("YYYY-MM-DD 00:00:00"),
            LessonId: lessonAttached.ItemId,
            StartDate: minDate,
            EndDate: maxDate,
            PrePostRequired: lessonAttached.PrePostRequired,
            ForGrading: lessonAttached.ForGrading,
            TeacherPlanLessonDateModels: lessonAttached.TeacherPlanLessonDateModels?.map(
              (x) => ({
                Date: moment(x.Date || x).format("YYYY-MM-DD"),
                ImplementationDate: moment(x.ImplementationDate || x).format("YYYY-MM-DD")
              })
            ),
            TeacherPlanLessonDateModelList: lessonAttached.TeacherPlanLessonDateModels?.map(
              (x) => ({
                Date: moment(x.Date || x).format("YYYY-MM-DD"),
                ImplementationDate: moment(x.ImplementationDate || x).format("YYYY-MM-DD")
              })
            ),
            TeacherPlanLessonQuizModels: lessonAttached.TeacherPlanLessonQuizModels?.map((quiz) => {
              // Try to parse with multiple formats
              const dueDate = moment(quiz.DueDate, ["DD/MM/YYYY hh:mm:ss A", "M/D/YYYY h:mm:ss A", "YYYY-MM-DD HH:mm:ss", moment.ISO_8601], true);
              return {
                id: quiz.Id,
                quizId: quiz.QuizId,
                name: quiz.QuizTitle,
                date: dueDate.isValid() ? dueDate.format("YYYY/MM/DD") : "",
                time: dueDate.isValid() ? dueDate.format("hh:mm A") : "",
                timezone: quiz.DueDateTimezone,
                countForGrade: quiz.ForGrading === "1",
                duration: quiz.Duration,
                planLessonId: quiz.PlanLessonId,
                lessonDueDate: lessonAttached.DueDate,
                lessonId: lessonAttached.ItemId,
              };
            }),

            implementationDates: lessonAttached.TeacherPlanLessonDateModels?.map(
              (i) => new Date(i.ImplementationDate)
            ),
          };
        }
      );
      setPlanningLessons({ TotalLessons, planningLessons: planningLessons ?? [] });
    } catch (error) {
      setPlanningLessons({ TotalLessons: 0, planningLessons: [] });
    }
  };

  return {
    getPlanningLessons,
    planningLesson,
    isLoading,
  };
};

export default useGetPlanningLessons;
