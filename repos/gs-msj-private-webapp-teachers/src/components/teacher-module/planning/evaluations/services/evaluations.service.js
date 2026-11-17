import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getEvaluations = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanEvaluationsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteEvaluation = async (evaluationId) => {
  const endpoint = Settings.getEvaluationsDeleteAPI(evaluationId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createEvaluationTypes = async (evaluation) => {
  const diagnosticTypes = evaluation.diagnostic
      ? await createTypes(evaluation.diagnostic, 1)
      : [];

  const formativeTypes = evaluation.formative
      ? await createTypes(evaluation.formative, 2)
      : [];

  const summativeTypes = evaluation.summative
      ? await createTypes(evaluation.summative, 3)
      : [];

  return [...formativeTypes, ...summativeTypes, ...diagnosticTypes];
};

const createTypes = async (types, category) => {
  let responses = [];
  const typesToCreate = types.customs?.filter((o) => o.value === o.label);

  if (typesToCreate && typesToCreate.length > 0) {
    const endpoint = Settings.getEvaluationsTypesAPI();
    responses = await Promise.all(
      typesToCreate.map((type) => {
        const data = {
          Description: type.label,
          Type: category,
          Custom: 1
        };
        return axios.post(endpoint, data);
      })
    );
  }

  const newCustomTypes = responses.map((response) => response.data.Id);
  const customTypes = types.customs?.filter((o) => o.value !== o.label)?.map((o) => o.value);
  return [
    ...(Array.isArray(types.defaults) ? types.defaults : []),
    ...(Array.isArray(customTypes) ? customTypes : []),
    ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
  ];
};

const createEvaluation = async (evaluation) => {
  const endpoint = Settings.getEvaluationsAPI(evaluation.planningId);
  const types = await createEvaluationTypes(evaluation);
  const data = {
    TeacherPlanId: evaluation.planningId,
    TeacherPlanEvaluationTypes: types.map((type) => ({
      EvaluationTypeId: type,
    })),
    TeacherPlanEvaluationsDates: evaluation.implementationDates.map((date) => ({
      ImplementationDate: date,
    })),
  };

  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateEvaluation = async (evaluation) => {
  const endpoint = Settings.getEvaluationsAPI();
  const types = await createEvaluationTypes(evaluation);
  const data = {
    Id: evaluation.id,
    TeacherPlanId: evaluation.planningId,
    TeacherPlanEvaluationTypes: types.map((type) => ({
      EvaluationTypeId: type,
    })),
    TeacherPlanEvaluationsDates: evaluation.implementationDates.map((date) => ({
      ImplementationDate: date,
    })),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const getCategories = async () => {
  try {
    const endpoint = Settings.getEvaluationsTypesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getEvaluations,
  deleteEvaluation,
  createEvaluation,
  updateEvaluation,
  getCategories,
};
