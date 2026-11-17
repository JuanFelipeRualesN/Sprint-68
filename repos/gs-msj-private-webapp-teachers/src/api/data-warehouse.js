import axios from "./interceptors";
import Settings from "../custom-classes/settings";

export const getDataWarehouseGrades = () => {
    return axios.get(Settings.getDataWarehouseGradesAPI());
}

export const getDataWarehouseSubjects = () => {
    return axios.get(Settings.getDataWarehouseSubjectsAPI());
}

export const getDataWarehouseRoles = () => {
    return axios.get(Settings.getDataWarehouseRolesAPI());
}

export const getDataWarehouseStatus = () => {
    return axios.get(Settings.getDataWarehouseStatusAPI());
}

export const getPowerBIReports = (reportId, workspaceId) => {
    return axios.get(Settings.getPowerBIReportsAPI(reportId, workspaceId));
}

export const getDataWarehouseCountry = (countryIso, country) => {
    return axios.get(Settings.getDataWarehouseCountryAPI(countryIso, country));
}

export const getDataWarehouseOre = (countryIso) => {
    return axios.get(Settings.getDataWarehouseOreAPI(countryIso));
}

export const getDataWarehouseStates = (countryIso) => {
    return axios.get(Settings.getDataWarehouseStatesAPI(countryIso));
}

export const getDataWarehouseCities = (countryIso, stateIso, oreId) => {
    return axios.get(Settings.getDataWarehouseCitiesAPI(countryIso, stateIso, oreId));
}

export const getDataWarehouseSchools = (schoolType, city, oreId, stateIsoCode, countryIso, schoolIdentifier, limit) => {
    return axios.get(Settings.getDataWarehouseSchoolsAPI(schoolType, city, oreId, stateIsoCode, countryIso, schoolIdentifier, limit));
}

export const getDataWarehouseUsers = (username, userId, email, firstName, lastName, roleId, status, role, schoolId, sie, oreId, callDocument, limit, pageNumber) => {
    return axios.get(Settings.getDataWarehouseUsersAPI(username, userId, email, firstName, lastName, roleId, status, role, schoolId, sie, oreId, callDocument, limit, pageNumber));
}

export const getDataWarehouseConnected = (fromDate, toDate, schoolId, oreId, status, callDocument, limit, pageNumber) => {
    return axios.get(Settings.getDataWarehouseConnectedAPI(fromDate, toDate, schoolId, oreId, status, callDocument, limit, pageNumber));
}

export const getDataWarehouseAcademics = (fromDate, toDate, subject, level, schoolId, oreId, callDocument, limit, pageNumber) => {
    return axios.get(Settings.getDataWarehouseAcademicsAPI(fromDate, toDate, subject, level, schoolId, oreId, callDocument, limit, pageNumber));
}

export const getDataWarehouseStudentActivitiesByTeacher = (fromDate, toDate, callDocument, limit, pageNumber) => {
    return axios.get(Settings.getDataWarehouseStudentActivitiesByTeacherAPI(fromDate, toDate, callDocument, limit, pageNumber));
}
