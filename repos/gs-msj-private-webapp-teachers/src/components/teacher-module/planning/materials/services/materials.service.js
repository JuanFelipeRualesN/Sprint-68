import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getMaterials = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanMaterialsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteMaterial = async (materialId) => {
  const endpoint = Settings.getMaterialsDeleteAPI(materialId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createMaterialTypes = async (types) => {
  const typesToCreate = types?.filter((o) => o.value === o.label);

  if (!typesToCreate || typesToCreate.length <= 0) return [];

  const endpoint = Settings.getMaterialsTypesAPI();
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

const createMaterial = async (material) => {
  const endpoint = Settings.getMaterialsAPI();

  const customTypes = material.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createMaterialTypes(material.customs);

  const data = {
    TeacherPlanId: material.planningId,
    TeacherPlanMaterialTypes: [
      ...(Array.isArray(material.defaults) ? material.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      MaterialTypeId: type,
    })),
    TeacherPlanMaterialDates: material.implementationDates.map((date) => ({
      ImplementationDate: date,
    })),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateMaterial = async (material) => {
  const endpoint = Settings.getMaterialsAPI();

  const customTypes = material.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createMaterialTypes(material.customs);

  const data = {
    Id: material.id,
    TeacherPlanId: material.planningId,
    TeacherPlanMaterialTypes: [
      ...(Array.isArray(material.defaults) ? material.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      MaterialTypeId: type,
    })),
    TeacherPlanMaterialDates: material.implementationDates.map((date) => ({
      ImplementationDate: date,
    })),
  };
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const getTypes = async () => {
  try {
    const endpoint = Settings.getMaterialsTypesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getMaterials,
  deleteMaterial,
  createMaterial,
  updateMaterial,
  getTypes,
};
