import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getDifferentiationStrategies = async (planningId) => {
  try {
    const endpoint = Settings.getDifferentiationStrategiesByTeacherPlanIdAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteDifferentiationStrategy = async (differentiationStrategyId) => {
  const endpoint = Settings.deleteTeacherPlanDifferentiationStrategiesAPI(differentiationStrategyId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createDifferentiationStrategyTypes = async (types) => {
  const typesToCreate = types?.filter((o) => o.value === o.label);

  if (!typesToCreate || typesToCreate.length <= 0) return [];

  const endpoint = Settings.postCreateDifferentiationStrategyTypesAPI();
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

const createDifferentiationStrategy = async (differentiationStrategy) => {
  const endpoint = Settings.postCreateTeacherPlanDifferentiationStrategiesAPI();

  const customTypes = differentiationStrategy.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createDifferentiationStrategyTypes(differentiationStrategy.customs);

  const data = {
    TeacherPlanId: differentiationStrategy.planningId,
    TeacherPlanDifferentiationStrategyCategories: (Array.isArray(differentiationStrategy.categoryId) ? differentiationStrategy.categoryId : []).map(
      (category) => ({
        DifferentiationStrategyCategoryId: category,
      })
    ),
    TeacherPlanDifferentiationStrategyTypes: [
      ...(Array.isArray(differentiationStrategy.defaults) ? differentiationStrategy.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      DifferentiationStrategyTypesId: type,
    })),
    TeacherPlanDifferentiationStrategyDates: differentiationStrategy.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateDifferentiationStrategy = async (differentiationStrategy) => {
  const endpoint = Settings.putUpdateTeacherPlanDifferentiationStrategiesAPI();

  const customTypes = differentiationStrategy.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createDifferentiationStrategyTypes(differentiationStrategy.customs);

  const data = {
    Id: differentiationStrategy.id,
    TeacherPlanId: differentiationStrategy.planningId,
    TeacherPlanDifferentiationStrategyCategories: (Array.isArray(differentiationStrategy.categoryId) ? differentiationStrategy.categoryId : []).map(
      (category) => ({
        DifferentiationStrategyCategoryId: category,
      })
    ),
    TeacherPlanDifferentiationStrategyTypes: [
      ...(Array.isArray(differentiationStrategy.defaults) ? differentiationStrategy.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      DifferentiationStrategyTypesId: type,
    })),
    TeacherPlanDifferentiationStrategyDates: differentiationStrategy.implementationDates.map(
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
    const endpoint = Settings.getDifferentiationStrategiesTypeAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const getCategories = async () => {
  try {
    const endpoint = Settings.getDifferentiationStrategyCategoriesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getDifferentiationStrategies,
  deleteDifferentiationStrategy,
  createDifferentiationStrategy,
  updateDifferentiationStrategy,
  getTypes,
  getCategories,
};
