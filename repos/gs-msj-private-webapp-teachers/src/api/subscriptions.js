import axios from "./interceptors";
import * as axios_ni from 'axios';
import Settings from "../custom-classes/settings";
import moment from 'moment';
import CurrencyManager from "../custom-classes/currency-manager";
import LanguageManager from "../custom-classes/language-manager";
import SessionManager from "../custom-classes/session-manager";

/**
 * To get the subject and level interface of a item product.
 * @param itemProductId The id of the item product.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getItemProductCourseInterface = (itemProductId) => {
    return axios.get(Settings.getItemProductInterfaceAPI(itemProductId));
};

/**
 * Get the subscription cancellation logs
 * @param subscriptionId the Id of the subscription
 * @returns {Promise<string|any>}
 */
export const getCancellationLogDetails = (subscriptionId) => {
    return axios.get(Settings.getSubscriptionCancellationLogsAPI(subscriptionId));
};

/**
 * To get the client general subscriptions data.
 * @returns {Promise<AxiosResponse<never>>}
 */
export const getClientSubscriptionsGeneralData = () =>{
    return axios.get(Settings.getClientSubscriptionsGeneralDataAPI())
}
/**
 * 
 * @returns {Promise<string|any>}
 */
export const getCancellationReasons = () => {
    return axios({
        method: "GET",
        url: Settings.getCancellationReasonsAPI(),
        headers: { 'Accept-Language': LanguageManager.getLanguage() }
    });
}

/** Update customer payment method **/
export const updateCustomerPaymentMethod = (email, userId, stripeSourceToken) => {

    let data = {
        Email: email,
        UserId: userId,
        StripeToken: stripeSourceToken
    };

    return axios({
        method: "POST",
        url: Settings.getUpdatePaymentMethodAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Return subscriptions details as the total quantity by plans, etc.
 * @param planId The id of the plan if any
 * @param dateFrom The date from
 * @param dateTo The date to
 */
export const getReportSubscription = (planId, dateFrom, dateTo) => {
    let data = {
        DateFrom: dateFrom,
        DateTo: dateTo,
    };
    if (planId !== null) {
        data.Subscription = {
            Plan: {
                Id: planId
            }
        }
    }

    return axios({
        method: "POST",
        url: Settings.getReportSubscriptionsAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Get the subscription details of client by subscription Id
 * @param subscriptionId
 * @returns {Promise<any | string>}
 */
export const getSubscriptionDetailsClient = (subscriptionId) => {
    let data = {
        "Subscription": {
            "SubscriptionId": subscriptionId,
        },
    };
    return axios({
        method: "POST",
        url: Settings.getSubscriptionDetailsClientsAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};
/**
 * Get the terms for academic plans
 * @returns {Promise<any | string>}
 */
export const getTerms = () => {
    return axios.get(Settings.getTermsAcademicsAPI());
};

/**
 * Return the main account profiles
 * @param {*} profileId : The profile id of the student
 * @public
 */
export const getProfileProducts = (profileId) => {
    return axios.get(Settings.getProfileProductsAPI(profileId));
};

/**
 * To returns coupons.
 * @param name The name of the coupon
 * @param code The code of the coupon
 * @param available The available ones
 * @param currency The currency
 * @param platformId The platform Id
 * @param currentPage The current page of the search
 * @param limiter How many results we want.
 * @returns {*}
 */
export const getCoupons = (name, code, available, currency, platformId, currentPage, limiter) => {
    let data = {
        PageFrom: currentPage,
        Limiter: limiter,
        Coupon: {
            Name: name,
            Code: code,
            Available: available,
            PlatformId: platformId,
            Currency: currency
        }
    };
    return axios({
        method: "POST",
        url: Settings.getCouponsReadAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Creates a coupon
 * @param name The name of the coupon
 * @param code The code of the coupon
 * @param currency The currency of the coupon
 * @param percentOff The percentage off
 * @param amountOff The amount off
 * @param duration The duration The duration
 * @param durationInMonths How many months
 * @param maxRedemptions Max redemption
 * @param reedemBy date to be redeemed
 * @param available If available or not
 * @param interval (monthly, yearly, etc)
 */
export const createCoupon = (name, code, currency, percentOff, amountOff, duration, durationInMonths, maxRedemptions, reedemBy, available, interval) => {
    if (typeof reedemBy !== 'undefined' && reedemBy !== null) {
        let m = moment(reedemBy).utcOffset(0);
        m.set({ hour: 23, minute: 59, second: 59, millisecond: 59 });
        m.toISOString();
        reedemBy = m.format("YYYY-MM-DD HH:mm:ss");
    }

    let data = {
        Coupon: {
            Name: name,
            Code: code,
            Currency: currency !== null && String(currency).trim().length !== 0 ? String(currency) : null,
            PercentOff: percentOff !== null && String(percentOff).trim().length !== 0 ? String(percentOff) : null,
            AmountOff: amountOff !== null && String(amountOff).trim().length !== 0 ? String(amountOff) : null,
            Duration: duration !== null && String(duration).trim().length !== 0 ? String(duration) : null,
            DurationInMonths: durationInMonths !== null && String(durationInMonths).trim().length !== 0 ? String(durationInMonths) : null,
            MaxRedemptions: maxRedemptions,
            IntervalIntended: interval,
            RedeemBy: reedemBy,
            Available: available
        }
    };
    return axios({
        method: "POST",
        url: Settings.getCouponCreateAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Validates a coupon and return the couponPlatformId
 * @param couponCode The coupon code.
 * @param interval The interval.
 * @returns {*}
 */
export const getCouponValidate = (couponCode, interval) => {
    return axios_ni({
        method: "GET",
        url: Settings.getCouponValidateAPI(couponCode, interval),
        headers: { "Content-Type": "application/json",
            "Token": SessionManager.getSession(),
            'Accept-Language': LanguageManager.getLanguage() },
    });
};

/**
 * Returns the subscription details
 * @param subscriptionId The id of the subscription
 * @param email The email of the customer
 * @param username The username of the customer
 * @param firstName The First Name of the customer
 * @param lastName The Last Name of the customer
 * @param status The status of the subscription
 * @param planId The plan id of the subscription
 * @param currentPage The current page
 * @param limiter How many items per page.
 * @param subscriptionsIds The subscriptions' ids.
 * @param subscriptionType The type of the subscription (if any)
 * @returns {*}
 */
export const getSubscriptionsAdvanced = (subscriptionId, email, username,
    firstName, lastName,
    status, planId,
    currentPage, limiter, subscriptionsIds, subscriptionType) => {

    let subscriptionRequest = {
        Username: username !== null && String(username).trim().length !== 0 ? String(username) : null,
        FirstName: firstName !== null && String(firstName).trim().length !== 0 ? String(firstName) : null,
        LastName: lastName !== null && String(lastName).trim().length !== 0 ? String(lastName) : null,
        Email: email !== null && String(email).trim().length !== 0 ? String(email) : null,
        PageFrom: currentPage,
        Limiter: limiter,
        SubscriptionIds: typeof subscriptionsIds !== 'undefined' && subscriptionsIds !== null && subscriptionsIds.length > 0 ?
            subscriptionsIds : null,
        Subscription: {
            SubscriptionId: subscriptionId !== null
                && String(subscriptionId).trim().length !== 0 ? String(subscriptionId) : null,
            Status: status !== null
                && String(status).trim().length !== 0 ? String(status) : null,
            Plan: {
                PlanId: planId !== null
                    && String(planId).trim().length !== 0 ? String(planId) : null,
            }
        },
        Type: typeof subscriptionType !== 'undefined' && subscriptionType !== null && String(subscriptionType).trim().length > 0 ?
            String(subscriptionType) : null
    };

    return axios({
        method: "POST",
        url: Settings.getSubscriptionReadAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(subscriptionRequest)
    });
};


/**
 * Returns the plans by an advanced search.
 * @param planId The plan's id.
 * @param planSubstripeId The plan stripe's id.
 * @param planName The plan's name.
 * @param currentPage The current page in pagination.
 * @param limiter How many items per page.
 * @param detailsNeeded If include details.
 * @param isCommercial If the plan is commercial or not.
 * @param avoidIsForSchoolParam To call the plan independently of is for school or not.
 * @returns {AxiosPromise}
 */
export const getPlansAdvanced = (planId, planSubstripeId, planName, currentPage, limiter, detailsNeeded,
                                 isCommercial = null,  avoidIsForSchoolParam = false) => {
    let planRequest = {
        Id: planId !== null && String(planId).trim().length !== 0 ? planId : null,
        PlatformId: planSubstripeId !== null && String(planSubstripeId).trim().length !== 0 ? planSubstripeId : null,
        Name: planName !== null && String(planName).trim().length !== 0 ? planName : null,
        PageFrom: currentPage,
        Limiter: limiter,
        IsForSchool: isCommercial === true ? "0" : "1",
    };

    if(avoidIsForSchoolParam){
        planRequest.IsForSchool = null; // Avoid sending the param
    }


    let data = {
        "PlanRequest": planRequest,
        "PageFrom": currentPage,
        "Limiter": limiter,
        "DetailsNeeded": detailsNeeded
    };

    return axios({
        method: "POST",
        url: Settings.getPlansAdvancedReadAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To cancel a subscription.
 * @param subscriptionId The subscription's id.
 * @param cancelAtPeriodEnd If cancel at the period's end.
 * @param cancellationReason The cancellation reason.
 * @returns {AxiosPromise}
 */
export const cancelSubscription = (subscriptionId, cancelAtPeriodEnd, cancellationReason, cancellationReasonId) => {

    let data = {
        "SubscriptionId": subscriptionId,
        "CancelAtPeriodEnd": cancelAtPeriodEnd,
        "CancellationReason": cancellationReason,
        "CancellationReasonId": cancellationReasonId
    };
    return axios({
        method: "POST",
        url: Settings.getCancellationAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};


/**
 * To update plan details.
 * @param planDetails
 * @param isCommercial If the plan is commercial or not.
 * @returns {Promise<any | string>}
 */
export const updatePlan = (planDetails, isCommercial = false) => {

    return axios({
        method: "POST",
        url: Settings.getPlanUpdateAPI(isCommercial),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(planDetails)
    });
};

/**
 * Inserts a new Plan
 * @param planDetails The plan details
 * @param isCommercial If the plan is commercial.
 * @returns {*}
 */
export const insertPlan = (planDetails, isCommercial = false) => {

    return axios({
        method: "POST",
        url: Settings.getPlanCreateAPI(isCommercial),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(planDetails)
    });
};

/**
 * Get the taxes
 * @returns {Promise.<T>|*}
 */
export const getTaxes = () => {
    return axios.get(Settings.getTaxesAPI());
};

/**
 * Returns the billing intervals
 * @returns {*|Promise.<T>}
 */
export const getBillingInterval = () => {
    return axios.get(Settings.getBillingIntervalAPI());
};

/**
 * Returns the available currencies
 * @returns {*|Promise.<T>}
 */
export const getCurrencies = () => {
    return axios.get(Settings.getCurrenciesAPI());
};

/**
 * The employee create a subscription for the employee
 * @param planId The id of the plan
 * @param userId The id of the user
 * @param tokenStripe The token created by stripe
 * @param schoolId The id of the school if any,
 * @param programId The id of the program if any
 * @param billingMethodCode The billing method code (STR for Stripe, PRI for private)
 * @param expirationDate The expiration date of a private subscription
 * @returns {*|Promise.<T>}
 */
export const insertSubscriptionByEmployee = (planId, userId, tokenStripe, schoolId, programId, billingMethodCode, expirationDate) => {

    let data = {
        "SubTokenStripe": tokenStripe,
        "PlanId": planId,
        "Quantity": 1,
        "UserId": userId,
        "ProgramId": programId,
        "SchoolId": schoolId,
        "PlatformCode": billingMethodCode,
        "CurrentPeriodEnd": expirationDate
    };

    return axios_ni({
        method: "POST",
        url: Settings.getSubscribeUserToPlanByEmployeeAPI(),
        headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), "Accept-Language": LanguageManager.getLanguage() },
        data: JSON.stringify(data)
    });
};
/**
 * The admin create a subscription for the client
 * @param model  subscriptions properties
 */
export const insertSubscriptionbyEmployeeV2 = (model) => {

    return axios_ni({
        method: "POST",
        url: Settings.getSubscribeUserToPlanByEmployeeAPI(),
        headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), "Accept-Language": LanguageManager.getLanguage() },
        data: JSON.stringify(model)
    });
}

export const updateDetailsAcademics = (model, entityAcademicDetailId) => {

    return axios_ni({
        method: "POST",
        url: Settings.getDetailsAcademicsModifyAPI(entityAcademicDetailId),
        headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), "Accept-Language": LanguageManager.getLanguage() },
        data: JSON.stringify(model)
    });
}

export const getSubscritionsAssociatedTeacher = () => {
    return axios.get(Settings.getSubscritionsAssociatedTeacherAPI());
};

/**
 * To get the products.
 * @param onlyAvailable If only available products
 * @param requireItemProducts If call the item products (grades)
 * @param isCommercial If the products are commercial or not
 * @returns {Promise<any | string>}
 */
export const getProducts = (onlyAvailable, requireItemProducts, isCommercial = false) => {
    return axios.get(Settings.getProductsAPI(onlyAvailable, requireItemProducts, isCommercial));
};

/**
 * To get the schools entities related by subscription id
 * @param subscriptionId
 * @returns {*}
 */
export const getSubscriptionSchoolEntitiesRelatedAdvanced = (subscriptionId) => {
    return axios.get(Settings.getSubscriptionSchoolEntitiesRelatedAPI(subscriptionId));
};

export const getAcademicsDetailsSubscription = (subscriptionId) => {
    return axios.get(Settings.getAcademicsDetailsAPI(subscriptionId));
};

/**
 * To insert a profile to a subscription.
 * @param profileId  The profile's id.
 * @param subscriptionId The subscription's id.
 * @param isStaff If staff or not
 * @param roleId The role's id of the user to insert.
 * @returns {AxiosPromise}
 */
export const insertSubscriptionToUser = (profileId, subscriptionId, isStaff, roleId) => {

    let data = {
        "ProfileId": profileId,
        "SubscriptionId": subscriptionId,
        "RoleId": roleId,
    };

    return axios_ni({
        method: "POST",
        url: Settings.getAssignSeatAPI(),
        headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), "Accept-Language": LanguageManager.getLanguage() },
        data: JSON.stringify(data)
    });
};

/**
 * To remove profiles from subscriptions.
 * @param profileId The profile id.
 * @param subscriptionId The subscription id.
 * @returns {*}
 */
export const removeProfileFromSubscription = (profileId, subscriptionId) => {

    let data = {
        "ProfileIds": [profileId],
        "SubscriptionId": subscriptionId,
    };

    return axios_ni({
        method: "POST",
        url: Settings.getRemoveSeatAPI(),
        headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), "Accept-Language": LanguageManager.getLanguage() },
        data: JSON.stringify(data)
    });
};

/**
 * Calculate the plan costs
 * @param planId
 * @param quantity
 * @param couponPlatformId
 * @param stateIso
 * @param countryIso
 * @returns {Promise<any | string>}
 */
export const getPlanTotalCosts = (planId, quantity, couponPlatformId = null, stateIso = null,
                                  countryIso = null) => {
    const url = Settings.getPlanCostAPI(planId, quantity, couponPlatformId, stateIso,
        countryIso);
    return axios.get(url);
};

/**
 * Calculate the plan costs for admin
 * @param userId The id of the user.
 * @param planId The plan's id.
 * @param quantity How many students are going to be subscribed.
 * @param couponPlatformId The coupon platform (optional)
 * @returns {Promise<AxiosResponse<never>>}
 */
export const getPlanTotalAdmin = (userId, planId, quantity, couponPlatformId = null) => {
    const url = Settings.getPlanCostAdminAPI(userId, planId, quantity, couponPlatformId);
    return axios.get(url);
};

/**
* Call the commercial plans
* @param callPromo ture if call the promo plans (special plans), else false
* @returns {Promise<any | string>}
*/
export const getCommercialPlans = (callPromo) => {
    const currency = CurrencyManager.getCurrency("USD");

    const callApi = Settings.getPlansAvailableGuestsAPI({
        isSchool: false,
        onlyAvailables: true,
        isPrivate: false,
        currencyCode: currency,
        filterByLanguage: true,
        planId: null,
        specialPromo: !!callPromo, // convert to boolean just in case
    });

    return axios({
        method: "GET",
        url: callApi,
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Creates the subscriptions according to plan id
 * @param planId The id of the plan
 * @param quantity The quantity of selected students
 * @param tokenStripe Stripe
 * @returns {*|Promise.<T>}
 */
export const createSubscriptions = (planId, quantity, couponPlatformId, tokenStripe) => {

    let data = {
        "SubTokenStripe": tokenStripe,
        "PlanId": planId,
        "Quantity": quantity,
        "CouponPlatformId": couponPlatformId
    };

    return axios_ni({
        method: "POST",
        url: Settings.getSubscribeUserToPlanAPI(),
        headers: { "Content-Type": "application/json", "Token": SessionManager.getSession(), 'Accept-Language': LanguageManager.getLanguage() },
        data: JSON.stringify(data)
    });
};

/**
 * Returns the users' subscription.
 * @returns {AxiosPromise}
 */
export const getUserSubscriptions = (planId, quantity, couponPlatformId, stateISO, countryISO) => {
    return axios.get(Settings.getUserSubscriptionsAPI());
};
/**
 * Returns the users' subscriptions with SubjectCode and level
 * @returns {AxiosPromise}
 */
export const getUserActiveSubscriptions = () => {
    return axios.get(Settings.getUserActiveSubscriptionsAPI());
}
/**
 * Return the user subscriptions courses. If the user is not subscribed, then it return all the subjects and levels.
 * This logic is to make available all the courses for the teachers that have a free registration and also
 * to not confuse the teachers that are attached to a subscription and can see and assign everything.
 */
export const getSubscriptionCourses = () => {
    return axios.get(Settings.getUserSubscriptionProductsAPI());
};

/**
 * To get the item-products related to a product.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getItemProductsAPI = () => {
    return axios.get(Settings.getItemProductsAPI());
};

/**
 * To read a specific product details.
 * @param productId The id of the product to read.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getReadSpecificProductAPI = (productId) => {
    return axios.get(Settings.getSpecificProductAPI(productId));
};

/**
 * To read an specific item-product's details.
 * @param id The item-product's id.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getReadSpecificItemProductAPI = (id) => {
    return axios.get(Settings.getSpecificItemProductAPI(id));
};

/**
 * Updates an existing item-product.
 * @param id Item-product's id.
 * @param subjectCode The subject code that represents that item-product.
 * @param levelCode The level code that represents that item-product.
 * @param name The item-product's name.
 * @param available If currently available
 * @returns {AxiosPromise}
 */
export const postUpdateItemsProductsAPI = (id, subjectCode, levelCode, name, available) => {

    let data = {
        "SubjectCode": subjectCode,
        "LevelCode": levelCode,
        "Name": name,
        "Available": available ? "1" : "0",
        "Id": id,
    };

    return axios({
        method: "POST",
        url: Settings.getUpdateItemProductAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To create a new product.
 * @param name The name of the product.
 * @param platformId The id of the platform.
 * @param available If available or not.
 * @param itemProducts The item products list.
 * @param isCommercial If the product is commercial or not.
 * @returns {Promise<any | string>}
 */
export const postAddProductsAPI = (name, platformId, available, itemProducts, isCommercial) => {

    let list = [];
    for (let i = 0; i < itemProducts.length; i++) {
        list.push({
            ItemId: itemProducts[i].value,
            Available: "1"
        });
    }

    let productRequestModel = {
        ProductModel: {
            Name: name,
            PlatformId: platformId.length > 0 ? platformId : null,
            Available: available ? "1" : "0",
            IsCommercial: isCommercial ? "1": "0"
        },
        ProductItemProductModelList: list
    };

    return axios({
        method: "POST",
        url: Settings.getCreateProductAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(productRequestModel)
    });
};


/**
 * To update the product.
 * @param id The product's id.
 * @param name The name of the product.
 * @param platformId The platform id of the product.
 * @param available If available or not.
 * @param itemProducts The item products list.
 * @param isCommercial If the product is commercial or not.
 * @returns {Promise<any | string>}
 */
export const postUpdateProductsAPI = (id, name, platformId, available, itemProducts, isCommercial) => {

    let list = [];
    for (let i = 0; i < itemProducts.length; i++) {
        list.push({
            ProductId: id,
            ItemId: itemProducts[i].value,
            Available: "1",
        });
    }

    let productRequestModel = {
        ProductModel: {
            Id: id,
            Name: name,
            PlatformId: platformId,
            Available: available ? "1" : "0",
            IsCommercial: isCommercial ? "1" : "0"
        },
        ProductItemProductModelList: list
    };

    return axios({
        method: "POST",
        url: Settings.getModifyProductAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(productRequestModel)
    });
};


/**
 * To create a new item-product.
 * @param name The item-product's name.
 * @param available If available or not.
 * @param subjectCode The subject's code.
 * @param levelCode The level's code.
 * @returns {AxiosPromise}
 */
export const postItemsProductsAPI = (name, available, subjectCode, levelCode) => {

    let body = {
        "SubjectCode": subjectCode,
        "LevelCode": levelCode,
        "Name": name,
        "Available": available ? "1" : "0"
    };

    return axios({
        method: "POST",
        url: Settings.getItemProductCreateAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(body)
    });
};

export const getCourses = () => {
    return axios.get(Settings.getCoursesAPI());
};

export const getSubscriptionsActivePeriods = (studentId) => {
    return axios.get(Settings.getSubscriptionsActivePeriodsAPI(studentId));
};

export const getReportAverageByPeriod = (periodId, studentId, subjectCode, levelCode) => {
    if (subjectCode && levelCode) {
        return axios.get(
            `${Settings.API_TEACHER_URL}/teachersapi/reports/average/period/${periodId}?includeDetails=true&userId=${studentId}&subjectCode=${subjectCode}&levelCode=${levelCode}`
        );
    }
    return axios.get(
        `${Settings.API_TEACHER_URL}/teachersapi/reports/average/period/${periodId}?includeDetails=true&userId=${studentId}`
    );
};

export const getAcademicYear = (studentId) => {
    return axios.get(
        `${Settings.API_SUBSCRIPTION_URL}/subscriptionsapi/academics/user/years?userId=${studentId}`
    );
}

export const getAcademicYearGrades = (academicYearId, studentId) => {
    return axios.get(
        `${Settings.API_DATAWAREHOUSE_URL}/grades-academic?academicYearId=${academicYearId}&user_id=${studentId}`
    );
}

export const getReportAcademicYearGrades = (academicYearId, studentId) => {
    return axios.get(
        `${Settings.API_DATAWAREHOUSE_URL}/grades-academic-pdf?academicYearId=${academicYearId}&user_id=${studentId}`
    );
}