import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getOpenLessons = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanOpenLessonsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteOpenLesson = async (lessonId) => {
  const endpoint = Settings.getOpenLessonsDeleteAPI(lessonId);
  const response = await axios.delete(endpoint);
  return Promise.resolve(response.status === 200);
};

const createOpenLessonCriterias = async (lesson) => {
  const standards = await createCriterias(lesson.standards, 1);
  const expectations = await createCriterias(lesson.expectations, 2);
  const strategies = await createCriterias(lesson.strategies, 3);
  const goals = await createCriterias(lesson.goals, 4);
  const units = await createCriterias(lesson.units, 5);
  const initial = await createCriterias(lesson.initial, 6);
  const development = await createCriterias(lesson.development, 7);
  const close = await createCriterias(lesson.close, 8);

  return [
    ...lesson.topics ?? [],
    ...standards,
    ...expectations,
    ...strategies,
    ...goals,
    ...units,
    ...initial,
    ...development,
    ...close
  ];
};

const createCriterias = async (criterias, category) => {
  let responses = [];
  const criteriasToCreate = criterias?.filter((o) => o.value === o.label);

  if (criteriasToCreate && criteriasToCreate.length > 0) {
    const endpoint = Settings.getOpenLessonsCriteriasAPI();
    responses = await Promise.all(
      criteriasToCreate.map((criteria) => {
        const data = {
          Description: criteria.label,
          Type: category
        };
        return axios.post(endpoint, data);
      })
    );
  }

  const newCustomCriterias = responses.map((response) => response.data.Id);
  const customCriterias = criterias?.filter((o) => o.value !== o.label)?.map((o) => o.value);
  return [
    ...(Array.isArray(customCriterias) ? customCriterias : []),
    ...(Array.isArray(newCustomCriterias) ? newCustomCriterias : []),
  ];
};

const createOpenLesson = async (lesson) => {
  const endpoint = Settings.getOpenLessonsAPI();
  const criterias = await createOpenLessonCriterias(lesson);
  const data = {
    TeacherPlanId: lesson.planningId,
    Title: lesson.title,
    TeacherPlanCustomLessonEducationalCriterias: criterias.map((criteria) => ({
      EducationalCriteriaId: criteria,
    })),
    TeacherPlanCustomLessonDates: lesson.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateOpenLesson = async (lesson) => {
  const endpoint = Settings.getOpenLessonsAPI();
  const criterias = await createOpenLessonCriterias(lesson);
  const data = {
    Id: lesson.id,
    TeacherPlanId: lesson.planningId,
    Title: lesson.title,
    TeacherPlanCustomLessonEducationalCriterias: criterias.map((criteria) => ({
      EducationalCriteriaId: criteria,
    })),
    TeacherPlanCustomLessonDates: lesson.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const getCriterias = async () => {
  try {
    const endpoint = Settings.getOpenLessonsCriteriasAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getOpenLessons,
  deleteOpenLesson,
  createOpenLesson,
  updateOpenLesson,
  getCriterias
};
