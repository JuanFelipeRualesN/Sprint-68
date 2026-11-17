import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getProjects = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanProjectsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteProject = async (projectId) => {
  const endpoint = Settings.getProjectsDeleteAPI(projectId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createProjectTypes = async (types) => {
  const typesToCreate = types?.filter((o) => o.value === o.label);

  if (!typesToCreate || typesToCreate.length <= 0) return [];

  const endpoint = Settings.getProjectsTypesAPI();
  const responses = await Promise.all(
    typesToCreate.map((type) => {
      const data = {
        Description: type.label,
        Type: 1,
      };
      return axios.post(endpoint, data);
    })
  );

  return responses.map((response) => response.data.Id);
};

const createProject = async (project) => {
  const endpoint = Settings.getProjectsAPI();

  const customTypes = project.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createProjectTypes(project.customs);

  const data = {
    TeacherPlanId: project.planningId,
    TeacherPlanInitiativeProjectInnovationTypes: [
      ...(Array.isArray(project.defaults) ? project.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      InitiativeProjectInnovationTypeId: type,
    })),
    TeacherPlanInitiativeProjectInnovationDates: project.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };

  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateProject = async (project) => {
  const endpoint = Settings.getProjectsAPI();

  const customTypes = project.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createProjectTypes(project.customs);

  const data = {
    Id: project.id,
    TeacherPlanId: project.planningId,
    TeacherPlanInitiativeProjectInnovationTypes: [
      ...(Array.isArray(project.defaults) ? project.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      InitiativeProjectInnovationTypeId: type,
    })),
    TeacherPlanInitiativeProjectInnovationDates: project.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const getTypes = async () => {
  try {
    const endpoint = Settings.getProjectsTypesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export { getProjects, deleteProject, createProject, updateProject, getTypes };
