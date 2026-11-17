/**
 * Created by portiz on 8/21/2018.
 */
import {isEmailAvailable, verifyIfUsernameIsAvailable} from "../api/users";
import {getSchoolEntities} from "../api/schools";

/**
 * Validates the email, username, etc
 */
class Validator {

    /**
     * Verifies if the email seems real
     * @param email
     * @returns {boolean}
     */
    static isEmailValid(email)
    {
        let re = /^(([^<>()\]\\.,;:\s@"]+(\.[^<>()\]\\.,;:\s@"]+)*)|(".+"))@(([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        //let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    };

    /**
     * Verifies if the email is available
     * @param email The email to check
     * @param callback The callback we need
     */
    static isEmailAvailable(email, callback)
    {
        // This is promise
        let apiCall = isEmailAvailable(email);
        apiCall.then((response)=>{
            callback(response.data);
        });
    }

    /**
     * Verifies if the password are equals
     * @param password1 The password
     * @param password2 The password retype
     * @returns {boolean} false if they are not equal
     */
    static arePasswordEquals(password1, password2)
    {
        return password1 === password2;
    }

    /**
     * Verifies if the length it's valid
     * @param password The password
     * @returns {boolean} false if not valid
     */
    static isPasswordLengthValid(password)
    {
        return (password.length >= 5);
    }

    /**
     * Verifies if the length and the password are equals
     * @param password1 The password
     * @param password2 The password retype
     * @returns {boolean} false if not valid
     */
    static isPasswordValid(password1, password2)
    {
        return this.arePasswordEquals(password1, password2) && this.isPasswordLengthValid(password1);
    }

    /**
     * Verifies if the username is available
     * @param username The username to check
     * @param callback The callback
     */
    static isUsernameAvailable(username, callback)
    {
        let apiCall = verifyIfUsernameIsAvailable(username);
        apiCall.then((response)=>{
            callback(response.data);
        });
    }

    /**
     * Verifies if school code is available.
     * @param schoolCode School code.
     * @param callback function to callback.
     */
    static isSchoolCodeAvailable(schoolCode, callback)
    {
        let apiCall = getSchoolEntities(schoolCode, null, null, null);
        apiCall.then((response)=>
        {
            if(response.data.length > 0){
                callback(false);
            }
            else
            {
                callback(true);
            }
        });
    }

    /**
     * Check if the username is valid. Set your own rules here
     * @param username The username
     * @returns {boolean} Returns true or false of valid or not
     */
    static isUsernameValid(username)
    {
        // Only letters and numbers, from a to z, A to Z and 0 to 9.
        // Doesn't include special characters or accented characters or tildes
        let re = /^([a-zA-Z0-9.]+)$/;
        return re.test(String(username)) && String(username).length >= 5;
    }

    /**
     * Verifies if the address is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isAddressValid(str)
    {
        return String(str).trim().length >= 3 && String(str).trim().length <= 120;
    }

    /**
     * Verifies if the city is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isCityValid(str)
    {
        return String(str).trim().length >= 2 && String(str).trim().length <= 35;
    }

    /**
     * Verifies if the state is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isStateValid(str)
    {
        return String(str) !== "0" && String(str).trim().length > 0;
    }

    /**
     * Verifies if the country is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isCountryValid(str)
    {
        return String(str) !== "0" && String(str).trim().length > 0;
    }

    /**
     * Verifies if the zip code is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isZipCodeValid(str)
    {
        return String(str).trim().length > 0 && String(str).trim().length < 12 ;
    }

    /**
     * Verifies if the record number is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isRecordNumberValid(str)
    {
        return String(str).trim().length >= 2;
    }

    /**
     * Verifies if name is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isFirstNameValid(str) {
        if (!str) return false;

        const trimmed = String(str).trim();

        // Optional: ensure only letters and spaces (supporting accented characters)
        const nameRegex = /^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]{2,50}$/;

        return nameRegex.test(trimmed);
    }

    /**
     * Verifies if the last name is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isLastNameValid(str) {
        if (!str) return false;

        const trimmed = String(str).trim();

        // Allow letters, spaces, apostrophes, and hyphens (with accented characters), length 2–50
        const nameRegex = /^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]{2,50}$/;

        return nameRegex.test(trimmed);
    }

    /**
     * Verifies if the phone is valid
     * @param str The string to check
     * @returns {boolean} True or false according if valid or not
     */
    static isPhoneValid(str) {
        if (!str) return false;
        const phone = String(str).trim();
        // Basic validation: must be digits only and at least 7 characters (can be adjusted)
        return /^\d{7,15}$/.test(phone);
    }
}

export default Validator;