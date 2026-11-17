import axios from "./interceptors";
import Settings from "../custom-classes/settings";
import { callUserSchoolIds } from "./users";

/**
 * To save school entity
 * @param name The name of the entity (school, program, etc)
 * @param entityId The entity Id if anything. Can be null, which means is new.
 * @param entityTypeId The type (if school, program, etc).
 * @param entityCode The license code or the school unique id.
 * @param privateOrPublic Private or Public.
 * @returns {Promise<any | string>}
 */
export const saveEntitySchool = (name, entityId, entityTypeId, entityCode, privateOrPublic) => {
    let data = {
        EntityModifier: {
            Entities: [
                {
                    Id: entityId,
                    Name: name,
                    EntityTypeId: entityTypeId,
                    Token: entityCode,
                    Private: privateOrPublic,
                },
            ]
        }
    };
    return axios({
        method: "POST",
        url: Settings.getSchoolEntityModifyAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};


/**
 * Save phone school
 * @param entityId entityId
 * @param phone Phone
 * @param phoneId Phone Id
 * @returns {*}
 */
export const savePhoneSchool = (entityId, phone, phoneId) => {
    let request = {
        EntityModifier: {
            Phones: [{
                Id: phoneId,
                EntityId: entityId,
                Phone: phone
            }]
        }
    };
    return axios({
        method: "POST",
        url: Settings.getSchoolModulePhoneModifyAPI(),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To save school entity address
 * @param name
 * @param entityId
 * @param addressLine1
 * @param addressLine2
 * @param city
 * @param state
 * @param country
 * @param zipCode
 * @param addressId
 * @param region
 * @returns {Promise<any | string>}
 */
export const saveEntitySchoolAddress = (name, entityId, addressLine1, addressLine2, city, state, country, zipCode, addressId, region) => {
    let request = {
        EntityModifier: {
            Addresses: [
                {
                    EntityId: entityId,
                    NameAddress: name,
                    AddressLine1: addressLine1,
                    AddressLine2: addressLine2,
                    City: city,
                    State: state,
                    StateIsoCode: state,
                    Country: country,
                    CountryIsoCode2: country,
                    Zip: zipCode,
                    Id: addressId,
                    RegionId: region
                }
            ]
        }
    };
    return axios({
        method: "POST",
        url: Settings.getSchoolModuleAddressModifyAPI(),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To delete the school address
 * @param addressId The id of the address
 * @returns {*}
 */
export const deleteSchoolAddress = (addressId) => {
    let data = {
        EntityModifier: {
            Addresses: [
                {
                    Id: addressId
                }
            ]
        }
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolModuleAddressDeleteAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To delete school phone
 * @param phoneId The ID Phone
 * @returns {*}
 */
export const deleteSchoolPhone = (phoneId) => {
    let data = {
        EntityModifier: {
            Phones: [
                {
                    Id: phoneId
                }
            ]
        }
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolModulePhoneDeleteAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To add a relation between entity
 * @param entityId
 * @param entityTypeId
 * @param relationEntityId
 * @returns {*}
 */
export const addSchoolEntityRelation = (entityId, entityTypeId, relationEntityId) => {
    let data = {
        EntityModifier: {
            Relations: [{
                EntityId: entityId,
                RelationEntityId: relationEntityId,
                TypeId: entityTypeId
            }]
        }
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolModuleRelationsModifyAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To delete an entity relation
 * @param relationId
 * @param entityId
 * @returns {Promise<any | string>}
 */
export const deleteSchoolEntityRelation = (relationId, entityId) => {
    let data = {
        EntityModifier: {
            Relations: [{
                RelationEntityId: relationId,
                EntityId: entityId
            }]
        }
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolModuleRelationsDeleteAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To get the school entities
 * @param entityType entity type if school, program, etc
 * @param name to filter by name
 * @param city to filter by city
 * @param state to filter by state
 * @param telephone to filter by telephone
 * @param currentPage the current pagenumber
 * @param limiter the limiter per page
 * @returns {*}
 */
 export const getEntitiesSchool = (entityType, name, city, state, telephone, currentPage, limiter) => {
    return axios({
        method: "GET",
        url: Settings.getSchoolEntitiesReadAPI( currentPage, limiter, null, entityType, name, city, state, telephone),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To get the entity school details.
 * @param entityId
 * @returns {Promise<any | string>}
 */
 export const getEntitiesSchoolDetails = (entityId) => {
    return axios({
        method: "GET",
        url: Settings.getSchoolEntityDetailsReadAPI(entityId),
        headers: { "Content-Type": "application/json" },
    });
};


/**
 * To get the usages of report schools.
 * @param entityIds Id of entities
 * @param dateFrom DateFrom
 * @param dateTo DateTo
 * @returns {Promise<any | string>}
 */
export const getUsageReportSchools = (entityIds, dateFrom, dateTo) => {
    let request = {
        EntityIds: entityIds,
        DateFrom: dateFrom,
        DateTo: dateTo
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolModuleReportsUsageAPI(),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To get the regions for schools.
 * @param countryIso The iso code of the country
 * @returns {Promise<any | string>}
 */
export const getRegions = (countryIso) => {
    return axios.get(Settings.getSchoolRegionsAPI(countryIso));
};

/**
 * Get school entities by entityCode, entityName and private or public
 * @param entityCode
 * @param entityName
 * @param privateOrPublic
 * @param entityType
 * @returns {Promise<any>}
 */
export const getSchoolEntities = (entityCode, entityName, privateOrPublic, entityType) => {
    let data = {
        "EntityModifier": {
            "Entities": [{
                "Token": entityCode,
                "Name": entityName,
                "Private": privateOrPublic,
                "EntityTypeId": entityType,
            }]
        }
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolEntityModelReadAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

export const getSchoolEntityDetails = (id) => {

    return axios({
        method: "GET",
        url: Settings.getSchoolEntityDetailsReadAPI(id),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Update school subscriptions.
 * @param schoolId School Id.
 * @param programId Program Id,
 * @param subscriptionId Subscription Id.
 * @returns {Promise<any | string>}
 */
export const updateEntitySubscriptionRelation = (schoolId, programId, subscriptionId) => {
    let param = {
        SubscriptionId: subscriptionId,
    };
    if (schoolId != null) {
        let arraySchools = [schoolId];
        param["SchoolIds"] = arraySchools;
    }
    if (programId != null) {
        let arrayPrograms = [programId];
        param["ProgramIds"] = arrayPrograms;
    }

    return axios({
        method: "POST",
        url: Settings.getUpdateEntitySubscriptionAPI(),
        data: JSON.stringify(param),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * TO get the school-module for simple selection.
 * @param privateOrPublic
 * @param state
 * @param country
 * @param city
 * @returns {Promise<any | string>}
 */
 export const getSchoolsForSimpleSelect = (privateOrPublic, state, country, city) => {
    return axios.get(Settings.getSchoolEntitiesReadAPI(0, 300, null, 1, null, city, state, null, privateOrPublic));
};

/**
 * TO get the country regions  
* @param countryIsoCode
* @returns {Promise<any | string>}
*/
export const getEducationalRegions = (countryIsoCode) => {
    return axios.get(Settings.getCountryRegionsAPI(countryIsoCode));
}

export const getAcademicsDetailsByEntity = (entityId, page, pageSize) => {
    return axios.get(Settings.getAcademicsDetailsByEntityAPI(entityId, page, pageSize));
}

export const getDetailByAcademicDetails = (entityId, entityAcademicDetailId) => {
    return axios.get(Settings.getDetailByAcademicDetailsAPI(entityId, entityAcademicDetailId));
}

export const createAcademicsDetails = (request, entityId) => {
    return axios({
        method: "POST",
        url: Settings.getAcademicsDetailsCreateAPI(entityId),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

export const modifyAcademicsDetails = (request, entityAcademicDetailId) => {
    return axios({
        method: "POST",
        url: Settings.getAcademicsDetailsModifyAPI(entityAcademicDetailId),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

export const getAvailabilityByRecordNumber = (entityId, recordNo) => {
    return axios.get(Settings.getAvailabilityByRecordNumberAPI(entityId, recordNo));
}


/** For new teacher dashboard 07/02/2026 **/
/**
 * To get the current period details by school ids.
 * @param schoolIds The school ids of the user.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const callCurrentPeriodTermYears = (schoolIds) =>{
    return axios.get(Settings.getAcademicPeriodsCurrentAPI(schoolIds));
};


/**
 * To get the usages of report schools.
 * @param entityIds Id of entities
 * @param dateFrom DateFrom
 * @param dateTo DateTo
 * @returns {Promise<any | string>}
 */
/**
 * To get the school ids belonging to a region (ORE).
 * @param oreId The region (ORE) id.
 * @returns {Promise<any | string>}
 */
export const getOreSchoolsIds = (oreId) => {
    return axios.get(`${Settings.API_SCHOOLS_URL}/schoolsapi/ore/${oreId}/schools_ids`);
};

/**
 * To get school entities by a list of ids.
 * @param ids Array of entity ids.
 * @returns {Promise<any | string>}
 */
export const getSchoolEntitiesByIds = (ids) => {
    let data = {
        EntityModifier: {
            Entities: ids.map(id => ({ Id: id })),
        },
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolEntityModelReadAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

// Academic periods used as date-range presets in the prompt history filter.
// Returns MED's "Items" shape, built from the school ids on the token plus
// their current periods, so only the current term is offered, and each id is
// derived since periods carry none
export const getAcademicYears = async () => {
    const empty = { data: { Items: [], PageTotals: 0 } };

    try {
        const schoolsResponse = await callUserSchoolIds();
        const schoolIds = schoolsResponse?.data ?? [];

        if (!Array.isArray(schoolIds) || schoolIds.length === 0) {
            return empty;
        }

        const periodsResponse = await axios.get(
            Settings.getAcademicPeriodsCurrentAPI(schoolIds)
        );
        const periods = periodsResponse?.data ?? [];

        if (!Array.isArray(periods)) {
            return empty;
        }

        return {
            data: {
                Items: periods.map((period) => ({
                    Id: `${period.PeriodNumber}-${period.PeriodFrom}`,
                    PeriodNumber: period.PeriodNumber,
                    FullTermStartDate: period.PeriodFrom,
                    FullTermEndDate: period.PeriodTo,
                })),
                PageTotals: periods.length,
            },
        };
    } catch (error) {
        console.error("Error fetching academic periods:", error);
        return empty;
    }
};
