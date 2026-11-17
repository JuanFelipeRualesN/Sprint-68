import axios from "./interceptors";
import Settings from "../custom-classes/settings";
import LanguageManager from "../custom-classes/language-manager";
import SessionManager from "../custom-classes/session-manager";

/**
 * To call to enter a new password when the user forgets his password
 * @param token The token included in the email
 * @param password The password
 * @param passwordRetype The password retype
 * @returns {*}
 */
export const callPasswordResetNewPassword = (token, password, passwordRetype) =>{
    let data = {
        Token: token,
        Password: password,
        PasswordRetype: passwordRetype
    };

    return axios({
        method: "POST",
        url: Settings.getPasswordResetNewPasswordAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To get the users's contacts.
 * @param page The current page in pagination.
 * @param pageSize The page size.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getUsersContacts = (page, pageSize) =>{
    return axios.get(Settings.getContactsTeacher(page, pageSize));
};

/**
 * To register that the user connect to the app at some point.
 * @returns {*}
 */
export const callActivityReportCreate = () =>{
    return axios({
        method: "POST",
        url: Settings.getActivityReportAPI(),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Sets the contact details for client account
 * @param firstName
 * @param lastName
 * @param city
 * @param phone
 * @returns {Promise<any | string>}
 */
export const setContactRegisterV2 = (firstName, lastName, city, phone) =>{
    let data = {
        FirstName: firstName,
        LastName: lastName,
        City: city,
        Phone: phone,
    };

    return axios({
        method: "POST",
        url: Settings.getContactDetailsRegisterV2API(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Calls the users that can be handled.
 * @returns {Promise<any | string>}
 */
export const callUsersThatCanBeHandled = () =>{

    return axios({
        method: "POST",
        url: Settings.getHandledUsersAPI(),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Set the client account password.
 * @param password
 * @param passwordRetype
 * @returns {Promise<any | string>}
 */
export const setPasswordRegisterV2 = (password, passwordRetype) =>{
    let data = {
        Password: password,
        PasswordRetype: passwordRetype,
    };

    return axios({
        method: "POST",
        url: Settings.getPasswordUpdateRegisterV2API(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To reset the password by email petition
 * @param email The user email
 * @returns {*} returns the data
 */
export const getPasswordResetPetition = (email) =>{
    let data = {
        Email: email,
    };

    return axios({
        method: "POST",
        url: Settings.getPasswordResetPetitionAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To get the usages of report schools.
 * @param entityIds Id of entities
 * @param dateFrom DateFrom
 * @param dateTo DateTo
 * @returns {Promise<any | string>}
 */
export const getUsageReportSchools = (entityIds, dateFrom, dateTo) =>{
    let data = {
        EntityIds: entityIds,
        DateFrom: dateFrom,
        DateTo: dateTo
    };

    return axios({
        method: "POST",
        url: Settings.getSchoolModuleReportsUsageAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
}

/**
 * Returns the roles.
 * @returns {*}
 */
export const getRoles = () => {
    return axios.get(Settings.getRolesReadAPI());
};

export const getStatusAccount = () => {
    return axios.get(Settings.getStatusAccountAPI());
};

/**
 * To register the user for the version 2
 * @param state The state of the address
 * @param stateIso The iso code of the state
 * @param country The country
 * @param countryIso The country iso code
 * @param planId The id of the plan
 * @param stripeToken The stripe token
 * @param subscriptionQuantity The quantity of subscriptions
 * @param email The email
 * @param couponPlatformId The coupon platform
 * @param firstName First name
 * @param lastName Last Name
 * @returns {*}
 */
export const registerSubmitPaymentV2Form = (state, stateIso, country, countryIso, planId, stripeToken,
                                            subscriptionQuantity, email, couponPlatformId, firstName, lastName, isPromo) =>{
    let data = {
        FirstName: firstName !== null && String(firstName).trim().length !== 0 ? String(firstName) : null,
        LastName: lastName !== null && String(lastName).trim().length !== 0 ? String(lastName) : null,
        LangCode: LanguageManager.getLanguage(),
        Token: SessionManager.getSession(),
        State: state !== null && String(state).trim().length !== 0 ? String(state) : null,
        StateIso: stateIso !== null && String(stateIso).trim().length !== 0 ? String(stateIso) : null,
        Country: country !== null && String(country).trim().length !== 0 ? String(country) : null,
        CountryIso: countryIso !== null && String(countryIso).trim().length !== 0 ? String(countryIso) : null,
        PlanId: planId !== null && String(planId).trim().length !== 0 ? String(planId) : null,
        StripeToken: stripeToken !== null && String(stripeToken).trim().length !== 0 ? String(stripeToken) : null,
        SubscriptionsQuantity: subscriptionQuantity === 0 ? 1 : subscriptionQuantity,
        Email: email !== null && String(email).trim().length !== 0 ? String(email) : null,
        CouponPlatformId: couponPlatformId !== null && String(couponPlatformId).trim().length !== 0 ?
            String(couponPlatformId) : null
    };

    let apiCall = Settings.getRegisterPaymentV2API();
    if(isPromo)
        apiCall = Settings.getRegisterPaymentV2PromoAPI();

    return axios({
        method: "POST",
        url: apiCall,
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To register a teacher by themselves.
 * @param firstName First name
 * @param lastName Last Name of user
 * @param phone Phone
 * @param addressLine1 The Address Line 1
 * @param addressLine2 The address line 2
 * @param city The address city.
 * @param state The address state.
 * @param stateIso The address state iso.
 * @param country The country.
 * @param zipCode The zip code.
 * @param countryIso The country ISO.
 * @param email The user student.
 * @param password The user password.
 * @param passwordRetype The user password retype.
 * @param schoolId The school Id.
 * @param schoolUserIdentifier The user school identifier.
 * @param invitation An invitation token.
 * @param isTeacher If is a teacher or not.
 * @returns {Promise<any | string>}
 */
export const registerFreeUser = (firstName, lastName, phone, addressLine1, addressLine2, city, state, stateIso,
                                 country, zipCode, countryIso, email,
                                 password, passwordRetype, schoolId, schoolUserIdentifier, invitation, isTeacher) =>{
    let data = {
        LangCode: LanguageManager.getLanguage(),
        RoleId: isTeacher ? Settings.ROLE_TEACHER_ID : Settings.ROLE_PARENT_ID,
        Phone: typeof phone !== 'undefined' && phone !== null && phone.trim().length > 0 ?
            phone.trim().replace(/\D/g, "") : null,
        AddressLine1: addressLine1 !== null && String(addressLine1).trim().length !== 0 ? String(addressLine1) : null,
        AddressLine2: addressLine2 !== null && String(addressLine2).trim().length !== 0 ? String(addressLine2) : null,
        City: city !== null && String(city).trim().length !== 0 ? String(city) : null,
        State: state !== null && String(state).trim().length !== 0 ? String(state) : null,
        StateIso: stateIso !== null && String(stateIso).trim().length !== 0 ? String(stateIso) : null,
        Country: country !== null && String(country).trim().length !== 0 ? String(country) : null,
        ZipCode: zipCode !== null && String(zipCode).trim().length !== 0 ? String(zipCode) : null,
        CountryIso: countryIso !== null && String(countryIso).trim().length !== 0 ? String(countryIso) : null,
        Email: email !== null && String(email).trim().length !== 0 ? String(email) : null,
        FirstName: firstName !== null && String(firstName).trim().length !== 0 ? String(firstName) : null,
        LastName: lastName !== null && String(lastName).trim().length !== 0 ? String(lastName) : null,
        Password: password !== null && String(password).trim().length !== 0 ? String(password) : null,
        PasswordRetype: passwordRetype !== null && String(passwordRetype).trim().length !== 0 ?
            String(passwordRetype) : null,
        SchoolId: schoolId !== null && String(schoolId).trim().length !== 0 ? String(schoolId) : null,
        SchoolUserIdentifier: schoolUserIdentifier !== null && String(schoolUserIdentifier).trim().length !== 0 ? String(schoolUserIdentifier) : null,
        RegistrationConnectionRequestToken: invitation !== null && String(invitation).trim().length !== 0 ? String(invitation) : null,
    };

    return axios({
        method: "POST",
        url: Settings.getRegisterFreeAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To delete the invitation email request.
 * @param invitationId Invitation Id to delete
 * @returns {Promise<any | string>}
 */
export const deleteInvitationEmailRequest = (invitationId) =>{
    return axios({
        method: "POST",
        url: Settings.getDeleteInvitationByEmailAPI(invitationId),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * To send a connection request.
 */
export const sendConnectionRequest = (invitationCode) =>{
    return axios({
        method: "POST",
        url: Settings.getSendInvitationRequestAPI(invitationCode),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * To send a connection request to user student by email (used to send connections to the parents).
 */
export const sendConnectionRequestByEmail = (studentId, parentEmail) =>{
    let dataToSend = {
        UserIds: [studentId],
        Email: parentEmail,
    };
    return axios({
        method: "POST",
        data: JSON.stringify(dataToSend),
        url: Settings.getSendInvitationByEmailRequestAPI(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * To get the sent invitation requests by emails
 * @returns {Promise<any | string>}
 */
export const getSentInvitationRequestsByEmail = (limiter, page) => {
    return axios.get(Settings.getUserSentInvitationsAPI(limiter, page));
};

/**
 * To get the user details (the parameters are filters)
 * @param userId
 * @param username
 * @param firstName
 * @param lastName
 * @param phoneNumber
 * @param email
 * @param roleId
 * @param lastSessionFrom
 * @param lastSessionTo
 * @param createdDateFrom
 * @param createdDateTo
 * @param currentPage
 * @param limiter
 * @param requestByClient
 * @param schoolId
 * @returns {Promise<any | string>}
 */
export const getUsersAdvanced = (userId, username, firstName, lastName, phoneNumber, email, roleId, lastSessionFrom,
                                 lastSessionTo, createdDateFrom, createdDateTo, currentPage, limiter, requestByClient, schoolId) =>{
    let data = {
        UserIds: typeof userId !== 'undefined' && userId !== null && String(userId).trim().length !== 0 ? [userId] : null,
        Username: typeof username !== 'undefined' && username !== null && String(username).trim().length !== 0 ? username : null,
        FirstName: typeof firstName !== 'undefined' && firstName !== null && String(firstName).trim().length !== 0 ? firstName : null,
        LastName:  typeof lastName !== 'undefined' && lastName !== null && String(lastName).trim().length !== 0 ? lastName : null,
        PhoneNumber: typeof phoneNumber !== 'undefined' && phoneNumber !== null && phoneNumber.trim().length > 0 ?
            phoneNumber.trim().replace(/\D/g, "") : null,
        Email:  typeof email !== 'undefined' && email !== null && String(email).trim().length !== 0 ? email : null,
        RoleId:  typeof roleId !== 'undefined' && roleId !== null && String(roleId).trim().length !== 0 ? roleId : null,
        LastSessionFrom:  typeof lastSessionFrom !== 'undefined' &&  lastSessionFrom !== null && String(lastSessionFrom).trim().length !== 0 ? lastSessionFrom : null,
        LastSessionTo:  typeof lastSessionTo !== 'undefined' &&  lastSessionTo !== null && String(lastSessionTo).trim().length !== 0 ? lastSessionTo : null,
        CreatedFrom: typeof createdDateFrom !== 'undefined' &&  createdDateFrom !== null && String(createdDateFrom).trim().length !== 0 ? createdDateFrom : null,
        CreatedTo: createdDateTo !== 'undefined' &&  createdDateTo !== null && String(createdDateTo).trim().length !== 0 ? createdDateTo : null,
        PageFrom: currentPage,
        Limiter: limiter,
        LangCode: LanguageManager.getLanguage(),
        Token: SessionManager.getSession(),
        RequestByClient: requestByClient,
        SchoolIds: typeof schoolId !== 'undefined' && schoolId !== null && String(schoolId).trim().length !== 0 ? [schoolId] : null
    };
    return axios({
        method: "POST",
        data: JSON.stringify(data),
        url: Settings.getUsersAdvanceRead(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Updates the password profile
 * @param password The password
 * @param passwordRetype The password retype
 * @param profileId The profile Id
 * @returns {*|Promise.<T>}
 */
export const updateProfilePassword = (password, passwordRetype, profileId) =>{
    let data = {
        "Password": password,
        "PasswordRetype": passwordRetype,
        "ProfileId": profileId
    };
    return axios({
        method: "POST",
        data: JSON.stringify(data),
        url: Settings.getProfileUpdatePasswordAPI(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Updates the profile information.
 * @param firstName
 * @param lastName
 * @param sex
 * @param dob
 * @param profileId
 * @param email
 */
export const updateProfileInformation = (firstName, lastName, sex, dob, profileId, email) =>{
    let data = {
        "FirstName": firstName,
        "LastName": lastName,
        "Gender": sex,
        "DoB": dob,
        "Email": email,
        "ProfileId": profileId
    };
    return axios({
        method: "POST",
        data: JSON.stringify(data),
        url: Settings.getProfileUpdateInformationAPI(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Returns the profile information
 * @returns {Promise.<T>|*}
 */
export const getProfileInformation = (profileId) =>{
    return axios.get(Settings.getProfileInformationReadAPI(profileId));
};

/**
 * Update user main address
 * @param addressLine1
 * @param addressLine2
 * @param city
 * @param state
 * @param country
 * @param countryIsoCode
 * @param zipCode
 * @returns {*|Promise.<T>}
 */
export const updateUserMainAddress = (addressLine1, addressLine2, city, state, country, countryIsoCode, zipCode) =>{
    let data = {
        "AddressLine1": addressLine1,
        "AddressLine2": addressLine2,
        "City": city,
        "State": state,
        "StateIso": state,
        "Country": country,
        "CountryIsoCode": countryIsoCode,
        "ZipCode": zipCode
    };
    return axios({
        method: "POST",
        data: JSON.stringify(data),
        url: Settings.getUserMainAddressUpdateAPI(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Endpoint to send the account details to the client in step 4 of configuration
 * @returns {Promise<any | string>}
 */
export const sendEmailWithAccountDetails = () =>{
    return axios({
        method: "POST",
        url: Settings.getEmailAccountDetailsEmailAPI(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Inserts the new password for the main account
 * @param oldPassword The current password
 * @param newPassword The new password
 * @param newPasswordRetype The new password retype
 * @returns {Promise.<T>|*}
 */
export const insertNewPassword = (oldPassword, newPassword, newPasswordRetype) =>{
    let data = {
        "OldPassword": oldPassword,
        "Password": newPassword,
        "PasswordRetype": newPasswordRetype
    };

    return axios({
        method: "POST",
        url: Settings.getUserNewPasswordInsertAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Returns the profiles by applying filters.
 * @param firstName The first name to search.
 * @param lastName The last name to search.
 * @param username The username.
 * @param pageFrom Current page in pagination.
 * @param limiter How many items per page.
 * @param roleId The role's id.
 * @returns {AxiosPromise}
 */
export const getProfiles = (firstName, lastName, username, pageFrom, limiter, roleId) =>{
    let data = {
        "OnlyActive": false,
        "Limiter": limiter,
        "PageFrom": pageFrom,
        "Username": username,
        "FirstName": firstName,
        "LastName": lastName,
        "RoleId": roleId,
    };

    return axios({
        method: "POST",
        url: Settings.getProfilesAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Return the subscription profiles
 * @param subscriptionId
 * @param firstName
 * @param lastName
 * @param username
 * @param limiter how many per page
 * @param pageFrom Current page
 * @param roleId
 * @returns {Promise<any | string>}
 */
export const getSubscriptionProfiles = (subscriptionId, firstName, lastName, username, limiter, pageFrom, roleId) =>{
    let data = {
        "SubscriptionId": subscriptionId,
        "OnlyActive": false,
        "Limiter": limiter,
        "PageFrom": pageFrom,
        "Username": username,
        "FirstName": firstName,
        "LastName": lastName,
        "RoleId": roleId
    };

    return axios({
        method: "POST",
        url: Settings.getSubscriptionProfilesAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Get the main account profiles.
 * @param subscriptionId
 * @param firstName
 * @param lastName
 * @param username
 * @param limiter
 * @param pageFrom
 * @param roleId
 * @returns {Promise<any | string>}
 */
export const getMainAccountProfiles = (subscriptionId, firstName, lastName, username, limiter, pageFrom, roleId) =>{
    let data = {
        "SubscriptionId": subscriptionId,
        "OnlyActive": false,
        "Limiter": limiter,
        "PageFrom": pageFrom,
        "Username": username,
        "FirstName": firstName,
        "LastName": lastName,
        "RoleId": roleId
    };

    return axios({
        method: "POST",
        url: Settings.getMainAccountProfilesAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Get the report usages.
 * @param userId The id of the client
 * @param dateFrom The date from
 * @param dateTo The date to
 * @returns {*}
 */
export const getReportUsage = (userId, dateFrom, dateTo) =>{
    let data = {
        CreatedFrom: dateFrom,
        CreatedTo: dateTo,
        UserId: userId
    };

    return axios({
        method: "POST",
        url: Settings.getReportUsageAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Verifies if the username is available
 * @param username The username to check
 */
export const verifyIfUsernameIsAvailable = (username) =>{
    return axios.get(Settings.getUsernameAvailabilityAPI(username));
};

/**
 * Verifies if the email is available
 * @param email The email
 */
export const isEmailAvailable = (email) =>{
    return axios.get(Settings.getEmailAvailabilityAPI(email));
};

/**
 * Update user info by admin
 * @param username
 * @param password
 * @param passwordRetype
 * @param email
 * @param firstname
 * @param lastname
 * @param phone
 * @param addressLine1
 * @param addressLine2
 * @param city
 * @param state
 * @param countryIso
 * @param zipCode
 * @param country
 * @param roleId
 * @param usernameChange If the username was changed
 * @param passwordChange If the passowrd was changed
 * @param emailChange If the email was changed
 * @param nameChange If the name was changed
 * @param phoneChange If the phone was changed
 * @param addressChange If the address was changed
 * @param roleChange If the role was changed
 * @param userId The id of the user.
 * @param paymentChange If the payment was change or not
 * @param substripeToken The stripe token of the new payment method
 */
export const updateUserInfoByAdmin = (username, password, passwordRetype, email, firstname,
                                      lastname, phone,
                                      addressLine1, addressLine2, city, state, countryIso, zipCode, country,
                                      roleId,
                                      oreId,
                                      substripeToken,
                                      usernameChange, passwordChange, emailChange, nameChange, phoneChange,
                                      addressChange, roleChange, paymentChange,
                                      userId, statusId, oreChange) =>{
    let data = {
        "Username": username,
        "Password": password,
        "PasswordRetype": passwordRetype,
        "Email": email,
        "FirstName": firstname,
        "LastName": lastname,
        "Phone": phone,
        "DoB": null,
        "AddressLine1": addressLine1,
        "AddressLine2": addressLine2,
        "City": city,
        "State": state,
        "StateIso": state,
        "CountryIso": countryIso,
        "ZipCode": zipCode,
        "Country": country,
        "Gender": null,
        "RoleId": roleId,
        "UsernameChange": usernameChange,
        "PasswordChange": passwordChange,
        "EmailChange": emailChange,
        "NameChange": nameChange,
        "PhoneChange": phoneChange,
        "AddressChange": addressChange,
        "RoleChange": roleChange,
        "UserId": userId,
        "StripeToken": substripeToken,
        "PaymentMethodChange": paymentChange,
        "StatusId": statusId,
        "OreId": oreId,
        "OreChange": oreChange,
    };

    return axios({
        method: "POST",
        url: Settings.getUserInfoUpdateByAdminAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Register the client via an admin window
 * @param username The username
 * @param password The password
 * @param passwordRetype The password retyping
 * @param email The email of the client
 * @param firstname The first name of the client
 * @param lastname The last name of the client
 * @param phone The phone of the client
 * @param addressLine1 The address line 1
 * @param addressLine2
 * @param city
 * @param state
 * @param countryIso
 * @param zipCode
 * @param country
 * @param roleId
 * @returns {*|Promise.<T>}
 */
export const registerUserByAdmin = (username, password, passwordRetype, email, firstname,
                                    lastname, phone, addressLine1, addressLine2, city, state, countryIso, zipCode, country,
                                    roleId, oreId, schoolId, schoolUserIdentifier) =>{
    let data = {
        "Username": username,
        "Password": password,
        "PasswordRetype": passwordRetype,
        "Email": email,
        "FirstName": firstname,
        "LastName": lastname,
        "Phone": phone,
        "DoB": null,
        "AddressLine1": addressLine1,
        "AddressLine2": addressLine2,
        "City": city,
        "State": state,
        "StateIso": state,
        "CountryIso": countryIso,
        "ZipCode": zipCode,
        "Country": country,
        "Gender": null,
        "RoleId": roleId,
        "SchoolId": schoolId,
        "SchoolExternalIdentifier": schoolUserIdentifier,
        "OreId": oreId
    };

    return axios({
        method: "POST",
        url: Settings.getUserRegisterByAdminAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To register profile to the subscription.
 * @param subscriptionId
 * @param username
 * @param password
 * @param passwordRetype
 * @param firstname
 * @param lastname
 * @param sex
 * @param dob
 * @param isStaff
 * @param roleId
 * @param schoolUserIdentifier
 * @param email
 * @param isHomeschool If the profile is homeschooling or not
 * @returns {Promise<any | string>}
 */
export const registerProfile = (subscriptionId, username, password, passwordRetype, firstname,
                                lastname, sex, dob, isStaff, roleId, schoolUserIdentifier, email, isHomeschool = false) =>{
    let data = {
        "SubscriptionId": subscriptionId,
        "Username": username,
        "Password": password,
        "PasswordRetype": passwordRetype,
        "FirstName": firstname,
        "LastName": lastname,
        "DoB": dob,
        "Gender": sex,
        "RoleId": roleId,
        "SchoolUserIdentifier": schoolUserIdentifier,
        "Email": email,
    };

    return axios({
        method: "POST",
        url: Settings.getRegisterProfileAPI(isHomeschool),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Verifies the login
 * @param username The username to check
 * @param password The password
 */
export const getLoginClient = (username, password) =>{
    let data = {
        "Username": username,
        "Password": password
    };

    return axios({
        method: "POST",
        url: Settings.getLoginClientAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

export const getLoginSso = (idToken) =>{
    let data = {
        Token: idToken,
        Domain: "teachers"        
    };

    return axios({
        method: "POST",
        url: Settings.getLoginSsoAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Return the address of the main account.
 */
export const getUserMainAddress = () =>{
    return axios({
        method: "POST",
        url: Settings.getUserMainAddressAPI(),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Returns the profile connections by pagination.
 * @param fromPage From Page.
 * @param limiter Limiter.
 * @param userId Id of the user to read the connections. Optional. If not sent, it will read the connections of user in session. If sent, it has to be the administrator.
 * @returns {Promise<any | string>}
 */
export const getProfileConnectionsByPagination = (fromPage, limiter, userId) => {
    return axios.get(Settings.getUserProfileConnectionsByPaginationAPI(fromPage, limiter, userId));
};

/**
 * Creates the user school by subscription.
 * @param subscriptionId Subscription Id
 * @returns {Promise<any | string>}
 */
export const createUserSchoolBySubscription = (subscriptionId) => {
    return axios({
        method: "POST",
        url: Settings.getUserSchoolCreateRelationBySubscriptionAPI(subscriptionId),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * Returns the profile connections requests by pagination.
 * @param fromPage From Page.
 * @param limiter Limiter.
 * @returns {Promise<any | string>}
 */
export const getProfilesConnectionsRequests = (fromPage, limiter) => {
    return axios.get(Settings.getUserProfileConnectionsRequestsByPaginationAPI(fromPage, limiter));
};

/**
 * Adds the connection by admin.
 * @param userIdA User Id A to relate.
 * @param userIdB User Id B to relate.
 * @param isUserAAlpha If user A can control user b.
 * @returns {Promise<any | string>}
 */
export const addConnectionByAdmin = (userIdA, userIdB, isUserAAlpha) => {
    return axios({
        method: "POST",
        url: Settings.getProfileConnectionAddByAdminAPI(userIdA, userIdB, isUserAAlpha),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * To remove profile connection.
 * @param userIdA The id of the user.
 * @param userIdB (optional. If not sent, it will delete the relationship between the logged user and the user Id A. If sent,
 * which is available only for the staff, it will deleted the selected users (userIdA and userIdB) relationship.
 * @returns {Promise<any | string>}
 */
export const removeProfileConnection = (userIdA, userIdB) => {
    return axios({
        method: "POST",
        url: Settings.getProfileConnectionDeleteRequestAPI(userIdA, userIdB),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * To remove profile connection request
 * @param userId The id of the user.
 * @returns {Promise<any | string>}
 */
export const removeProfileConnectionRequest = (userId) => {
    return axios({
        method: "POST",
        url: Settings.getProfileRequestConnectionDeleteRequestAPI(userId),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * Accepts a profile connection request.
 * @param userId The Id of the user.
 * @returns {Promise<any | string>}
 */
export const acceptProfileConnectionRequest = (userId) => {
    return axios({
        method: "POST",
        url: Settings.getProfileAcceptConnectionRequestAPI(userId),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * To get the institutions of the user.
 * @param userId The id of the user.
 * @returns {Promise<any | string>}
 */
export const getUserSchools = (userId) => {
    return axios.get(Settings.getSchoolUsersWithInfoAPI(userId));
};

/**
 * Deletes the user school.
 * @param userId The id of the user.
 * @param entityId The entity of the user.
 * @returns {Promise<any | string>}
 */
export const deleteUserSchool = (userId, entityId) => {
    return axios({
        method: "POST",
        url: Settings.getDeleteUserSchoolsAPI(userId, entityId),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * Confirms the user school.
 * @param userId The id of the user.
 * @param entityId The entity id.
 * @param identifier The user's school record.
 * @returns {Promise<any | string>}
 */
export const confirmUserSchool = (userId, entityId, identifier) => {
    return axios({
        method: "POST",
        url: Settings.getConfirmUserSchoolsAPI(userId, entityId, identifier),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * Adds the user to school.
 * @param userId The id of the user.
 * @param entityId The entity id.
 * @param identifier THe identifier.
 */
export const addUserSchool = (userId, entityId, identifier) => {
    return axios({
        method: "POST",
        url: Settings.getAddUserSchoolsAPI(userId, entityId, identifier),
        headers: {"Content-Type": "application/json"},
    });
};

/**
 * To get the user details from same school(the parameters are filters)
 * @param userId
 * @param username
 * @param firstName
 * @param lastName
 * @param phoneNumber
 * @param email
 * @param roleId
 * @param lastSessionFrom
 * @param lastSessionTo
 * @param createdDateFrom
 * @param createdDateTo
 * @param currentPage
 * @param limiter
 * @param requestByClient
 * @param schoolId
 * @returns {Promise<any | string>}
 */
export const getProfilesAdvanced = (userId, username, firstName, lastName, phoneNumber, email, roleId, lastSessionFrom,
                                           lastSessionTo, createdDateFrom, createdDateTo, currentPage, limiter, requestByClient, schoolId) =>{
    let data = {
        UserId: typeof userId !== 'undefined' && userId !== null && String(userId).trim().length !== 0 ? userId : null,
        Username: typeof username !== 'undefined' && username !== null && String(username).trim().length !== 0 ? username : null,
        FirstName: typeof firstName !== 'undefined' && firstName !== null && String(firstName).trim().length !== 0 ? firstName : null,
        LastName:  typeof lastName !== 'undefined' && lastName !== null && String(lastName).trim().length !== 0 ? lastName : null,
        PhoneNumber: typeof phoneNumber !== 'undefined' && phoneNumber !== null && phoneNumber.trim().length > 0 ?
            phoneNumber.trim().replace(/\D/g, "") : null,
        Email:  typeof email !== 'undefined' && email !== null && String(email).trim().length !== 0 ? email : null,
        RoleId:  typeof roleId !== 'undefined' && roleId !== null && String(roleId).trim().length !== 0 ? roleId : null,
        LastSessionFrom:  typeof lastSessionFrom !== 'undefined' &&  lastSessionFrom !== null && String(lastSessionFrom).trim().length !== 0 ? lastSessionFrom : null,
        LastSessionTo:  typeof lastSessionTo !== 'undefined' &&  lastSessionTo !== null && String(lastSessionTo).trim().length !== 0 ? lastSessionTo : null,
        CreatedFrom: typeof createdDateFrom !== 'undefined' &&  createdDateFrom !== null && String(createdDateFrom).trim().length !== 0 ? createdDateFrom : null,
        CreatedTo: createdDateTo !== 'undefined' &&  createdDateTo !== null && String(createdDateTo).trim().length !== 0 ? createdDateTo : null,
        PageFrom: currentPage,
        Limiter: limiter,
        RequestByClient: requestByClient,
        SchoolId: typeof schoolId !== 'undefined' && schoolId !== null && String(schoolId).trim().length !== 0 ? schoolId : null
    };

    return axios({
        method: "POST",
        url: Settings.getProfilesAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Returns the permissions list.
 * @returns {Promise<any | string>}
 */
export const getPermissions = () => {
    return axios.get(Settings.getPermissionsAPI());
};

/**
 * Returns the permission by id.
 * @returns {Promise<any | string>}
 */
export const getPermissionById = (id) => {
    return axios.get(Settings.getPermissionByIdAPI(id));
};

/**
 * To create a new permission.
 * @param name The permission name.
 * @param desc The permission description.
 * @returns {Promise<any | string>}
 */
export const postPermissionCreate = (name, desc) =>{
    let data = {
        Name: name,
        Desc: desc,
    };

    return axios({
        method: "POST",
        url: Settings.getPermissionCreateAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Updates an existing permission
 * @param id The id.
 * @param name The permission name.
 * @param desc The permission description.
 * @returns {Promise<any | string>}
 */
export const postPermissionUpdate = (id, name, desc) =>{
    let data = {
        Id: id,
        Name: name,
        Desc: desc,
    };

    return axios({
        method: "POST",
        url: Settings.getPermissionModifyAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Returns the role by id.
 * @returns {Promise<any | string>}
 */
export const getRoleById = (id) => {
    return axios.get(Settings.getRolesByIdAPI(id));
};

/**
 * To create a new role.
 * @param name The permission name.
 * @param desc The permission description.
 * @param rolePermissionList The list for role permissions
 * @returns {Promise<any | string>}
 */
export const postRolesCreate = (name, desc, rolePermissionList) =>{
    let rolePermissionToAddList = [];
    for(let i=0; i < rolePermissionList.length; i++)
    {
        rolePermissionToAddList.push({
            PermissionId: rolePermissionList[i].value,
        });
    }

    let data = {
        Name: name,
        Desc: desc,
        RolePermissionModelList:rolePermissionToAddList
    };

    return axios({
        method: "POST",
        url: Settings.getRolesCreateAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Updates an existing role.
 * @param id The id of the role.
 * @param name The permission name.
 * @param desc The permission description.
 * @param rolePermissionList The list for role permissions
 * @returns {Promise<any | string>}
 */
export const postRolesUpdate = (id, name, desc, rolePermissionList) =>{
    let rolePermissionToAddList = [];
    for(let i=0; i < rolePermissionList.length; i++)
    {
        rolePermissionToAddList.push({
            PermissionId: rolePermissionList[i].value,
        });
    }

    let data = {
        Id: id,
        Name: name,
        Desc: desc,
        RolePermissionModelList:rolePermissionToAddList
    };

    return axios({
        method: "POST",
        url: Settings.getRolesModifyAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/** For new teacher dashboard 07/02/2026 **/

/**
 * Gets how many students the teacher has.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const callStudentsCounting = () => {
    return axios.get(Settings.getStudentsCountingAPI());
};

/**
 * Gets the school ids of the user.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const callUserSchoolIds = () =>{
    return axios.get(Settings.getUserSchoolIdsAPI());
}

/**
 * To get the user details (the parameters are filters)
 * @param userId
 * @param username
 * @param firstName
 * @param lastName
 * @param userSchoolRecord
 * @param email
 * @param roleId
 * @param lastSessionFrom
 * @param lastSessionTo
 * @param createdDateFrom
 * @param createdDateTo
 * @param currentPage
 * @param limiter
 * @param requestByClient
 * @param schoolId
 * @returns {Promise<any | string>}
 */
export const getUsersAdvancedAdmin = (userId, username, firstName, lastName, userSchoolRecord, email, roleId,
                                        createdDateFrom, createdDateTo, currentPage, limiter, requestByClient, schoolId) =>{
    let data = {
        UserIds: Array.isArray(userId)
            ? (userId.length > 0 ? userId : null)
            : (typeof userId !== 'undefined' && userId !== null && String(userId).trim().length !== 0 ? [userId] : null),
        Username: typeof username !== 'undefined' && username !== null && String(username).trim().length !== 0 ? username : null,
        FirstName: typeof firstName !== 'undefined' && firstName !== null && String(firstName).trim().length !== 0 ? firstName : null,
        LastName:  typeof lastName !== 'undefined' && lastName !== null && String(lastName).trim().length !== 0 ? lastName : null,
        UserSchoolRecord: typeof userSchoolRecord !== 'undefined' && userSchoolRecord !== null && userSchoolRecord.trim().length > 0 ?
            userSchoolRecord.trim().replace(/\D/g, "") : null,
        Email:  typeof email !== 'undefined' && email !== null && String(email).trim().length !== 0 ? email : null,
        RoleId:  typeof roleId !== 'undefined' && roleId !== null && String(roleId).trim().length !== 0 ? roleId : null,
        CreatedFrom: typeof createdDateFrom !== 'undefined' &&  createdDateFrom !== null && String(createdDateFrom).trim().length !== 0 ? createdDateFrom : null,
        CreatedTo: createdDateTo !== 'undefined' &&  createdDateTo !== null && String(createdDateTo).trim().length !== 0 ? createdDateTo : null,
        PageFrom: currentPage,
        Limiter: limiter,
        LangCode: LanguageManager.getLanguage(),
        Token: SessionManager.getSession(),
        RequestByClient: requestByClient,
        SchoolIds: Array.isArray(schoolId)
            ? (schoolId.length > 0 ? schoolId : null)
            : (typeof schoolId !== 'undefined' && schoolId !== null && String(schoolId).trim().length !== 0 ? [schoolId] : null)
    };
    return axios({
        method: "POST",
        data: JSON.stringify(data),
        url: Settings.getUsersAdvanceAdminRead(),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Gets id+email only (no name) for every user of the given schools. Unlike
 * getUsersAdvancedAdmin (for-admin), this has no per-call cap. Names for these ids must be
 * resolved separately.
 * @param schoolIds Array of school ids to filter by.
 * @param limiter How many rows to request.
 * @returns {Promise<any | string>}
 */
export const getUserEmailsBySchools = (schoolIds, limiter) => {
    let data = {
        SchoolIds: schoolIds,
        RoleId: null,
        UserIds: null,
        Email: null,
        UserSchoolRecord: null,
        PageFrom: 0,
        Limiter: limiter,
    };
    return axios({
        method: "POST",
        data: JSON.stringify(data),
        url: Settings.getUserEmailsAPI(),
        headers: { "Content-Type": "application/json" }
    });
};
