import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getObservations = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanObservationsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteObservation = async (observationId) => {
  const endpoint = Settings.deleteTeacherPlanObservationsAPI(observationId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createObservation = async (observation) => {
  const endpoint = Settings.postCreateTeacherPlanObservationsAPI();
  const data = {
    TeacherPlanId: observation.planningId,
    IsCompleted: observation.achieved ? 1 : 0,
    Detail: observation.detail,
    TeacherPlanObservationDates: observation.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateObservation = async (observation) => {
  const endpoint = Settings.putUpdateTeacherPlanObservationsAPI();
  const data = {
    Id: observation.id,
    TeacherPlanId: observation.planningId,
    IsCompleted: observation.achieved ? 1 : 0,
    Detail: observation.detail,
    TeacherPlanObservationDates: observation.implementationDates.map(
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
  getObservations,
  deleteObservation,
  createObservation,
  updateObservation,
};
