import axios from "./interceptors";
import Settings from "../custom-classes/settings";

/**
 * Returns the countries full details
 * @param onlyAvailables If call the only availables
 * @param filterByLanguage If filter by langage
 * @returns {*|Promise.<T>}
 */
export const getCountryFullDetails = (onlyAvailables, filterByLanguage)=>
{
    return axios.get(Settings.getCountriesFullDetailsAPI(filterByLanguage, onlyAvailables));
};

/**
 * Return the states or provinces if the country has one
 * @param countryISO
 * @returns {Promise.<T>|*}
 */
export const getStateProvinces = (countryISO) =>
{
    return axios.get(Settings.getStateProvincesAPI(countryISO));
};

