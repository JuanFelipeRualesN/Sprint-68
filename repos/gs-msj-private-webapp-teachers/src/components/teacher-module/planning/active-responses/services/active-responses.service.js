import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getActiveResponses = async (planningId) => {
  try {
    const endpoint = Settings.getActiveResponsesByTeacherPlanIdAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteActiveResponse = async (activeResponseId) => {
  const endpoint = Settings.deleteTeacherPlanActiveResponsesAPI(activeResponseId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createActiveResponseTypes = async (types) => {
  const typesToCreate = types?.filter((o) => o.value === o.label);

  if (!typesToCreate || typesToCreate.length <= 0) return [];

  const endpoint = Settings.postCreateActiveResponseTypesAPI();
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

const createActiveResponse = async (activeResponse) => {
  const endpoint = Settings.postCreateTeacherPlanActiveResponsesAPI();

  // const customTypes = activeResponse.customs
  //   ?.filter((o) => o.value !== o.label)
  //   ?.map((o) => o.value);
  // const newCustomTypes = await createActiveResponseTypes(activeResponse.customs);

  const data = {
    TeacherPlanId: activeResponse.planningId,
    TeacherPlanActiveResponseIncludes: [{
      IncludesStudentActiveResponse : activeResponse.includesStudentActiveResponse,
      IncludesLearningCheckActivity :  activeResponse.includesLearningCheckActivity
    }],
    TeacherPlanActiveResponseTypes: activeResponse.activeResponseTypeIds.map((type) => ({
      ActiveResponseTypesId: type,
    })),
    TeacherPlanActiveResponseDates: activeResponse.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  console.log('data', data)
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateActiveResponse = async (activeResponse) => {
  const endpoint = Settings.putUpdateTeacherPlanActiveResponsesAPI();

  const customTypes = activeResponse.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createActiveResponseTypes(activeResponse.customs);

  const data = {
    Id: activeResponse.id,
    TeacherPlanId: activeResponse.planningId,
    TeacherPlanActiveResponseIncludes: [{
      IncludesStudentActiveResponse : activeResponse.includesStudentActiveResponse,
      IncludesLearningCheckActivity :  activeResponse.includesLearningCheckActivity
    }],
    TeacherPlanActiveResponseTypes: activeResponse.activeResponseTypeIds.map((type) => ({
      ActiveResponseTypesId: type,
    })),
    TeacherPlanActiveResponseDates: activeResponse.implementationDates.map(
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
    const endpoint = Settings.getActiveResponsesTypeAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const getIncludes = async () => {
  try {
    const endpoint = Settings.getTeacherPlanActiveResponseIncludesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getActiveResponses,
  deleteActiveResponse,
  createActiveResponse,
  updateActiveResponse,
  getTypes,
  getIncludes,
};
