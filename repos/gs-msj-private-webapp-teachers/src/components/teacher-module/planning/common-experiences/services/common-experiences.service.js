import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getCommonExperiences = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanCommonExperiencesAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteCommonExperience = async (observationId) => {
  const endpoint = Settings.deleteTeacherPlanCommonExperiencesAPI(observationId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createCommonExperience = async (observation) => {
  const endpoint = Settings.postCreateTeacherPlanCommonExperiencesAPI();
  const data = {
    TeacherPlanId: observation.planningId,
    IsCompleted: observation.achieved ? 1 : 0,
    Detail: observation.detail,
    TeacherPlanCommonExperienceDates: observation.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateCommonExperience = async (observation) => {
  const endpoint = Settings.putUpdateTeacherPlanCommonExperiencesAPI();
  const data = {
    Id: observation.id,
    TeacherPlanId: observation.planningId,
    IsCompleted: observation.achieved ? 1 : 0,
    Detail: observation.detail,
    TeacherPlanCommonExperienceDates: observation.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

export {
  getCommonExperiences,
  deleteCommonExperience,
  createCommonExperience,
  updateCommonExperience,
};
