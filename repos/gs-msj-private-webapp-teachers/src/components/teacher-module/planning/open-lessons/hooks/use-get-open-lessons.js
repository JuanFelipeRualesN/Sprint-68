import { useState } from "react";
import * as lessons from "../services/open-lessons.service";

const useGetOpenLessons = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getCriterias = (criterias, category) => {
    return criterias?.filter(
      (c) => c.EducationalCriteria?.Type === category
    ).map((criteria) => ({
      value: criteria.EducationalCriteriaId,
      label: criteria.EducationalCriteria.Description,
    }));
  };

  const getOpenLessonCriterias = (criterias) => {
    const topics = getCriterias(criterias, 0);
    const standards = getCriterias(criterias, 1);
    const expectations = getCriterias(criterias, 2);
    const strategies = getCriterias(criterias, 3);
    const goals = getCriterias(criterias, 4);
    const units = getCriterias(criterias, 5);
    const initial = getCriterias(criterias, 6);
    const development = getCriterias(criterias, 7);
    const close = getCriterias(criterias, 8);

    return {
      topics: topics.map(t => t.value),
      standards,
      expectations,
      strategies,
      goals,
      units,
      initial,
      development,
      close
    };
  };

  const getOpenLessons = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await lessons.getOpenLessons(planningId);
      const results = response.map((lesson) => {
        const criterias = getOpenLessonCriterias(lesson.TeacherPlanCustomLessonEducationalCriterias);

        return {
          id: lesson.Id,
          planningId: lesson.TeacherPlanId,
          title: lesson.Title,
          criterias: lesson.TeacherPlanCustomLessonEducationalCriterias.map(
            (criteria) => criteria.EducationalCriteriaId
          ),
          implementationDates: lesson.TeacherPlanCustomLessonDates.map(
            (i) => new Date(i.ImplementationDate)
          ),
          ...criterias
        };
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getOpenLessons,
    isLoading,
    succeeded,
    lessons: data,
  };
};

export default useGetOpenLessons;
