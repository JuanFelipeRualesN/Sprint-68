import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getIntegrations = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanIntegrationsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteIntegration = async (integrationId) => {
  const endpoint = Settings.getIntegrationsDeleteAPI(integrationId);
  const response = await axios.delete(endpoint);
  return Promise.resolve(response.status === 200);
};

const createIntegration = async (integration) => {
  const endpoint = Settings.getIntegrationsAPI();
  const data = {
    TeacherPlanId: integration.planningId,
    TeacherPlanOtherSubjectInegrationSubjects: integration.subjects.map(
      (subject) => ({
        SubjectId: subject,
      })
    ),
    TeacherPlanOtherSubjectIntegrationDates: integration.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateIntegration = async (integration) => {
  const endpoint = Settings.getIntegrationsAPI();
  const data = {
    Id: integration.id,
    TeacherPlanId: integration.planningId,
    TeacherPlanOtherSubjectInegrationSubjects: integration.subjects.map(
      (subject) => ({
        SubjectId: subject,
      })
    ),
    TeacherPlanOtherSubjectIntegrationDates: integration.implementationDates.map(
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
  getIntegrations,
  deleteIntegration,
  createIntegration,
  updateIntegration,
};
