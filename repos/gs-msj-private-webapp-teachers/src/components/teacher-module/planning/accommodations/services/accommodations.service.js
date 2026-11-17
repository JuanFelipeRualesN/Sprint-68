import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const getAccommodations = async (planningId) => {
  try {
    const endpoint = Settings.getTeacherPlanAccommodationsAPI(planningId);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteAccommodation = async (accommodationId) => {
  const endpoint = Settings.getAccommodationsDeleteAPI(accommodationId);
  const response = await axios.delete(endpoint);
  const result = response.status === 200;

  return Promise.resolve(result);
};

const createAccommodationTypes = async (types) => {
  const typesToCreate = types?.filter((o) => o.value === o.label);

  if (!typesToCreate || typesToCreate.length <= 0) return [];

  const endpoint = Settings.getAccommodationsTypesAPI();
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

const createAccommodation = async (accommodation) => {
  const endpoint = Settings.getAccommodationsAPI();

  const customTypes = accommodation.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createAccommodationTypes(accommodation.customs);

  const data = {
    TeacherPlanId: accommodation.planningId,
    TeacherPlanAccommodationTypes: [
      ...(Array.isArray(accommodation.defaults) ? accommodation.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      AccommodationTypeId: type,
    })),
    TeacherPlanAccommodationDates: accommodation.implementationDates.map(
      (date) => ({
        ImplementationDate: date,
      })
    ),
  };
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateAccommodation = async (accommodation) => {
  const endpoint = Settings.getAccommodationsAPI();

  const customTypes = accommodation.customs
    ?.filter((o) => o.value !== o.label)
    ?.map((o) => o.value);
  const newCustomTypes = await createAccommodationTypes(accommodation.customs);

  const data = {
    Id: accommodation.id,
    TeacherPlanId: accommodation.planningId,
    TeacherPlanAccommodationTypes: [
      ...(Array.isArray(accommodation.defaults) ? accommodation.defaults : []),
      ...(Array.isArray(customTypes) ? customTypes : []),
      ...(Array.isArray(newCustomTypes) ? newCustomTypes : []),
    ].map((type) => ({
      AccommodationTypeId: type,
    })),
    TeacherPlanAccommodationDates: accommodation.implementationDates.map(
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
    const endpoint = Settings.getAccommodationsTypesAPI();
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

export {
  getAccommodations,
  deleteAccommodation,
  createAccommodation,
  updateAccommodation,
  getTypes,
};
