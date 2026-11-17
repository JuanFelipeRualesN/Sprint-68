import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getStrategies = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanStrategiesAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteStrategy = async (strategyId) => {
  const endpoint = Settings.getStrategiesDeleteAPI(strategyId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createStrategyTypes = async (categoryId, types) => {
  const typesToCreate = types?.filter((o) => o.value === o.label);

  if (!typesToCreate || typesToCreate.length <= 0) return [];

  const endpoint = Settings.getStrategiesTypesAPI();
  const responses = await Promise.all(
    typesToCreate.map((type) => {
      const data = {
        Name: type.label,
        Description: type.label,
        Type: 1,
        StrategyCategoryId: categoryId,
      };
      return axios.post(endpoint, data);
    })
  );

  return responses.map((response) => response.data.Id);
};

const createStrategy = async (strategy) => {
  const endpoint = Settings.getStrategiesAPI(strategy.planningId);

  const customTypes = strategy.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createStrategyTypes(
    strategy.categoryId,
    strategy.customs
  );

  const data = {
    TeacherPlanId: strategy.planningId,
    TeacherPlanStrategyCategories: [
      { StrategyCategoryId: strategy.categoryId },
    ],
    TeacherPlanStrategyTypes: [
      ...(Array.isArray(strategy.defaults) ? strategy.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      StrategyTypeId: type,
    })),
    TeacherPlanStrategyDates: strategy.implementationDates.map((date) => ({
      ImplementationDate: date,
    })),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateStrategy = async (strategy) => {
  const endpoint = Settings.getStrategiesAPI();

  const customTypes = strategy.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createStrategyTypes(
    strategy.categoryId,
    strategy.customs
  );

  const data = {
    Id: strategy.id,
    TeacherPlanId: strategy.planningId,
    TeacherPlanStrategyCategories: [
      { StrategyCategoryId: strategy.categoryId },
    ],
    TeacherPlanStrategyTypes: [
      ...(Array.isArray(strategy.defaults) ? strategy.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      StrategyTypeId: type,
    })),
    TeacherPlanStrategyDates: strategy.implementationDates.map((date) => ({
      ImplementationDate: date,
    })),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const getTypes = async () => {
  try {
    const endpoint = Settings.getStrategiesTypesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const getCategories = async () => {
  try {
    const endpoint = Settings.getStrategiesCategoriesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getStrategies,
  deleteStrategy,
  createStrategy,
  updateStrategy,
  getTypes,
  getCategories,
};
