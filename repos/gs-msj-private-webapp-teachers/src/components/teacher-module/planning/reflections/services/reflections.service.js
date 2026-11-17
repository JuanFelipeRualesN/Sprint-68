import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getReflections = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanReflectionsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteReflection = async (reflectionId) => {
  const endpoint = Settings.getReflectionsDeleteAPI(reflectionId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createReflection = async (reflection) => {
  const endpoint = Settings.getReflectionsAPI();
  const data = {
    TeacherPlanId: reflection.planningId,
    IsCompleted: reflection.achieved ? 1 : 0,
    Detail: reflection.detail,
    TeacherPlanPraxisReflectionCompletedReasons: reflection.achieved ? [] : reflection.reasons?.map(
      (reason) => ({
        PraxisReflectionCompletedReasonId: reason,
      })
    ),
    TeacherPlanPraxisReflectionDates: reflection.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateReflection = async (reflection) => {
  const endpoint = Settings.getReflectionsAPI();
  const data = {
    Id: reflection.id,
    TeacherPlanId: reflection.planningId,
    IsCompleted: reflection.achieved ? 1 : 0,
    Detail: reflection.detail,
    TeacherPlanPraxisReflectionCompletedReasons: reflection.achieved ? [] : reflection.reasons.map(
      (reason) => ({
        PraxisReflectionCompletedReasonId: reason,
      })
    ),
    TeacherPlanPraxisReflectionDates: reflection.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const getNotAchievingReasons = async () => {
  try {
    const endpoint = Settings.getReflectionsNotAchievingReasonsAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getReflections,
  deleteReflection,
  createReflection,
  updateReflection,
  getNotAchievingReasons,
};
