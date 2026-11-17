import React, { useState, useEffect, useContext } from "react";
import { useForm, Controller } from "react-hook-form";
import { getCountries } from "../../../../api/gsweb";
import { getStateProvinces } from "../../../../api/address";
import {
  verifyIfUsernameIsAvailable,
  isEmailAvailable,
  getUsersAdvanced
} from "../../../../api/users";
import { getAvailabilityByRecordNumber } from "../../../../api/schools";
import styled from "styled-components";
import Select from "react-select";
import InputMask from "react-input-mask";
import Validator from "../../../../custom-classes/validator";
import { registerUserByAdmin } from "../../../../api/users";
import Settings from "../../../../custom-classes/settings";
import SessionManager from "../../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../../utils/utils";
import ModalClients from "../../modals/modal-clients";
import { useHistory, useLocation } from "react-router-dom";
import { WizardContext } from "../../modals/modal-subscription";
import { AppContext } from "../../../../App.Context";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import { Translate } from "react-localize-redux";

const TextDangerStyled = styled.p`
  color: #fa2626;
  font-size: 11px;
  margin: auto;
  padding: 0 0 0.25em 0;
  text-align: right;
`;

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const Step1 = ({ stepCompleted, setUserId, isSchool }) => {
  const { data } = useContext(WizardContext);
  const { state } = useContext(AppContext);
  const history = useHistory();
  let location = useLocation();
  let query = useQuery();
  let clientId = query.get("userId") || (data ? data.userId : undefined);
  let entityIdQuery = query.get("entityId");
  const sessionDetails = SessionManager.getSessionDetails();
  const {
    watch,
    handleSubmit,
    control,
    setValue,
  } = useForm({
    defaultValues: {
      FirstName: '',
      LastName: '',
      Phone: '',
      Email: '',
      Username: '',
      Password: '',
      AddressLine1: '',
      AddressLine2: '',
      City: '',
      Country: null,
      State: null,
      ZipCode: '',
      UserSchoolRecord: ''
    }
  });

  const [loading, setLoading] = useState(false);
  const [disabledFields, setDisabledFields] = useState(false);
  const [countries, setCountries] = useState([]);
  const [states, setStates] = useState([]);
  const [emailAvailable, setEmailAvailable] = useState(true);
  const [usernameAvailable, setUsernameAvailable] = useState(true);
  const [recordAvailable, setRecordAvailable] = useState(true);
  const [modalClientVisible, setModalClientVisible] = useState(false);

  // Form validations
  const [validName, setValidName] = useState(false);
  const [validLastName, setValidLastName] = useState(false);
  const [validPhone, setValidPhone] = useState(false);
  const [validPassword, setValidPassword] = useState(false);
  const [validAddress, setValidAddress] = useState(false);
  const [validCity, setValidCity] = useState(false);
  const [validZipCode, setValidZipCode] = useState(false);
  const [validRecordNumber, setValidRecordNumber] = useState(false);
  const [validEmail, setValidEmail] = useState(false);
  const [validUsername, setValidUsername] = useState(false);
  const [validCountry, setValidCountry] = useState(false);
  const [validState, setValidState] = useState(false);

  useEffect(() => {
    if (clientId) {
      searchClient();
      setUserId(clientId)
      setDisabledFields(true);
    } else {
      loadCountries();
    }
  }, []);

  const searchClient = async () => {
    setLoading(true);
    try {
      const countries = await loadCountries();
      const res = await getUsersAdvanced(clientId, null, null, null, null, null, null, null,
        null, null, null, 0, 1, null, null);
      const client = res.data.UserInfo[0];
      if (client.CountryIsoCode2 === "USA") {
        const states = await loadStates();
        setValuesClient(client, countries, states);
      } else {
        setValuesClient(client, countries);
      }
      stepCompleted(true);
    } catch {
    } finally {
      setLoading(false);
    }
  }

  const loadCountries = async () => {
    setLoading(true);
    try {
      const res = await getCountries();
      const countries = res.data.map((elem) => ({
        value: elem.IsoCode2,
        label: elem.CountryName,
      }));
      setCountries(countries);
      return countries;
    } catch {
    } finally {
      setLoading(false);
    }
  };

  const onSelectCountry = async (selected) => {
    if (selected.value === "USA") {
      await loadStates();
    }
  };

  const loadStates = async () => {
    setLoading(true);
    try {
      const res = await getStateProvinces("USA");
      const states = res.data.map((elem) => ({
        value: elem.IsoCode,
        label: elem.Name,
      }));
      setStates(states);
      setLoading(false);
      return states;
    } catch {
      setLoading(false);
    }
  }

  const onSubmit = async (data) => {
    const {
      Username,
      Password,
      Email,
      FirstName,
      LastName,
      Phone,
      AddressLine1,
      AddressLine2,
      City,
      State,
      Country,
      ZipCode,
      UserSchoolRecord
    } = data;
    const cleanedFirstName = trimAndNormalizeText(FirstName);
    const cleanedLastName = trimAndNormalizeText(LastName);
    const cleanedCity = trimAndNormalizeText(City);
    const cleanedAddressLine1 = trimAndNormalizeText(AddressLine1);
    const cleanedAddressLine2 = trimAndNormalizeText(AddressLine2);
    setLoading(true);
    try {
      const response = await registerUserByAdmin(Username, Password, Password, Email, cleanedFirstName,
        cleanedLastName, Phone, cleanedAddressLine1, cleanedAddressLine2, cleanedCity, State ? State.value : "", Country.value, ZipCode, Country.value,
        Settings.ROLE_MAIN_CLIENT_ID, null, entityIdQuery ? entityIdQuery : null, UserSchoolRecord);

      let messages = response.data.Messages[0],
        userId = response.data.UserId;


      setUserId(userId);
      setDisabledFields(true);
      setLoading(false);
      stepCompleted(true);

      if (entityIdQuery) {
        history.push(`${location.pathname}?userId=${userId}&entityId=${entityIdQuery}&subscriptionType=${!isSchool ? "H" : "S"}`);
      } else {
        history.push(`${location.pathname}?userId=${userId}&subscriptionType=${!isSchool ? "H" : "S"}`);
      }
    } catch {
      setLoading(false);
    }

  };

  const setValuesClient = async (client, countriesList, statesList) => {
    setValue("FirstName", client.GivenName);
    setValue("LastName", client.Surname);
    setValue("Email", client.Email);
    setValue("Username", client.Username);
    setValue("Phone", client.Phone);
    setValue("AddressLine1", client.AddressLine1);
    setValue("AddressLine2", client.AddressLine2);
    setValue("City", client.City);
    setValue("ZipCode", client.Zip);
    setValue("UserSchoolRecord", client.UserSchoolRecord);
    const countriesArray = countriesList || countries;
    const country = countriesArray.find(item => item.value === client.CountryIsoCode2);
    setValue("Country", country);
    const statesArray = statesList || states;
    const state = statesArray.find(item => item.value === client.StateIsoCode);
    setValue("State", state);

    setValidName(true);
    setValidLastName(true);
    setValidEmail(true);
    setValidPhone(true);
    setValidPassword(true);
    setValidAddress(true);
    setValidCity(true);
    setValidZipCode(true);
    setValidRecordNumber(true);
    setValidEmail(true);
    setValidUsername(true);
    setValidCountry(true)
    setValidState(true)
  };

  const validateEmailAsync = async (email) => {
    if (Validator.isEmailValid(email)) {
      try {
        const res = await isEmailAvailable(email);
        setEmailAvailable(res.data);
      } catch { }
    }
  };

  const validateUsernameAsync = async (username) => {
    if (Validator.isUsernameValid(username)) {
      try {
        const res = await verifyIfUsernameIsAvailable(username);
        setUsernameAvailable(res.data);
      } catch { }
    }
  };

  const validateAvailabilityNumberRecord = async (entityId, record) => {
    if (String(record).trim().length >= 2) {
      try {
        const res = await getAvailabilityByRecordNumber(entityId, record);
        setRecordAvailable(res.data);
      } catch { }
    }
  };

  const handleSelectedUser = async (client) => {
    if (client) {
      setDisabledFields(true);
      setUserId(client.UserId);
      if (entityIdQuery) {
        history.push(`${location.pathname}?userId=${client.UserId}&entityId=${entityIdQuery}&subscriptionType=${!isSchool ? "H" : "S"}`);
      } else {
        history.push(`${location.pathname}?userId=${client.UserId}&subscriptionType=${!isSchool ? "H" : "S"}`);
      }

      if (client.CountryIsoCode2 === "USA") {
        const states = await loadStates();
        setValuesClient(client, undefined, states);
      } else {
        setValuesClient(client);
      }
      //enable next step
      stepCompleted(true);
    }
    setModalClientVisible(false);
  }

  const openModalClients = () => {
    setModalClientVisible(true);
  }

  const handleNameChange = (e) => {
    const filteredFirstName = e.currentTarget.value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]/g, '');
    setValue("FirstName", filteredFirstName);
    setValidName(Validator.isFirstNameValid(filteredFirstName));
  };

  const handleLastNameChange = (e) => {
    const filteredLastName = e.currentTarget.value.replace(/[^a-zA-ZÀ-ÿ\u00f1\u00d1\s'-]/g, '');
    setValue("LastName", filteredLastName);
    setValidLastName(Validator.isLastNameValid(filteredLastName));
  };

  const handlePhoneChange = (e) => {
    const phone = e.currentTarget.value
    setValue("Phone", phone);
    setValidPhone(Validator.isPhoneValid(phone));
  };

  const handlePasswordChange = (e) => {
    const password = e.currentTarget.value;
    setValue("Password", password);
    setValidPassword(Validator.isPasswordLengthValid(password));
  };

  const handleAddressChange = (e) => {
    const address = e.currentTarget.value;
    setValue("AddressLine1", address);
    setValidAddress(Validator.isAddressValid(address));
  };

  const handleCityChange = (e) => {
    const city = e.currentTarget.value;
    setValue("City", city);
    setValidCity(Validator.isCityValid(city));
  };

  const handleZipCodeChange = (e) => {
    const zipCode = e.currentTarget.value;
    setValue("ZipCode", zipCode);
    setValidZipCode(Validator.isZipCodeValid(zipCode));
  };

  const handleRecordNumberChange = (e) => {
    const filteredUserSchoolRecord = e.currentTarget.value.replace(/[^A-Za-z0-9]/g, '');
    setValue("UserSchoolRecord", filteredUserSchoolRecord);
    setValidRecordNumber(Validator.isRecordNumberValid(filteredUserSchoolRecord));
    validateAvailabilityNumberRecord(entityIdQuery, filteredUserSchoolRecord);
  };

  const handleEmailChange = (e) => {
    const email = e.currentTarget.value;
    setValue("Email", email);
    setValidEmail(Validator.isEmailValid(email));
    validateEmailAsync(email);
  };

  const handleUsernameChange = (e) => {
    const filteredUsername = e.currentTarget.value.replace(/[^a-zA-Z0-9.]/g, '');
    setValue("Username", filteredUsername);
    setValidUsername(Validator.isUsernameValid(filteredUsername));
    validateUsernameAsync(filteredUsername);
  };

  const handleCountryChange = async (selected) => {
    setValue("Country", selected);
    setValidCountry(selected !== null && selected !== undefined);
    
    setValue("State", null);
    
    if (selected && selected.value === "USA") {
      setValidState(false);
      await loadStates();
    } else {
      setValidState(true);
    }
  };

  const handleStateChange = (selected) => {
    setValue("State", selected);
    setValidState(selected !== null && selected !== undefined);
  };

  const trimAndNormalizeText = (text) => {
    return text.trim().replace(/\s{2,}/g, ' '); 
  };

  const canSubmit = validName && validLastName && validPhone && validPassword && validAddress && validCity &&
  validZipCode && validEmail && validCountry && validUsername &&

  (!entityIdQuery || (validRecordNumber && recordAvailable)) && emailAvailable && usernameAvailable

  return (
    <>
      <div className="content-wizard">
        <h1 className="content-wizard--title">
          <Translate
            id={((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !state.variables.EnrollmentManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !state.variables.EnrollmentManagementAdmin)) ? "wizard.step-1.client-create-select-read" : "wizard.step-1.client-create-select"}
            options={{ renderInnerHtml: true }} />
        </h1>
        {data && data.userId ? null : (<button className="content-wizard--w-icon" type="button" onClick={openModalClients}>
          {" "}
          <i className="fas fa-plus-circle" />   <Translate
            id="wizard.step-1.client-select-existing"
            options={{ renderInnerHtml: true }} />{" "}
        </button>)}
        {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && state.variables.EnrollmentManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && state.variables.EnrollmentManagementAdmin)) && <p>
          <small>
            <Translate
              id="wizard.step-1.client-create-instructions"
              options={{ renderInnerHtml: true }} />
          </small>
        </p>}

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="wrapper-wizard-form mt-3 input-responsive">
            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>*  <Translate
                  id="wizard.step-1.client-first-name"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="FirstName"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      type="text"
                      value={field.value || ''}
                      disabled={disabledFields}
                      onChange={handleNameChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
                {!validName && (
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-name" />
                  </TextDangerStyled>
                )}
              </div>
            </div>

            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>* <Translate
                  id="wizard.step-1.client-last-name"
                  options={{ renderInnerHtml: true }} /></label>
                  <Controller
                    name="LastName"
                    control={control}
                    render={({ field }) => (
                      <input
                        {...field}
                        type="text"
                        value={field.value || ''}
                        disabled={disabledFields}
                        onChange={handleLastNameChange}
                        className="form-wizard-fields__input"
                      />
                    )}
                  />
                {!validLastName && (
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-lastname" />
                  </TextDangerStyled>
                )}
              </div>
            </div>

            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>* <Translate
                  id="wizard.general.phone-number"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="Phone"
                  control={control}
                  render={({ field }) => (
                    <InputMask
                      {...field}
                      mask="999999999999"
                      maskChar=""
                      placeholder=""
                      disabled={disabledFields}
                      className="form-wizard-fields__input"
                      onChange={handlePhoneChange}
                    />
                  )}
                />
                {!validPhone && (
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-phone" />
                  </TextDangerStyled>
                )}
              </div>
            </div>

            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>
                  * <Translate
                    id="wizard.general.email"
                    options={{ renderInnerHtml: true }}
                  />
                </label>

                <Controller
                  name="Email"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      value={field.value}
                      disabled={disabledFields}
                      onChange={handleEmailChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />

                {!validEmail && emailAvailable && (
                  <TextDangerStyled>
                    <Translate
                      id={"wizard.general.email-invalid"}
                      options={{ renderInnerHtml: true }}
                    />
                  </TextDangerStyled>
                )}
                {!emailAvailable && (
                  <TextDangerStyled>
                    <Translate
                      id="wizard.general.email-not-available"
                      options={{ renderInnerHtml: true }}
                    />
                  </TextDangerStyled>
                )}
              </div>
            </div>

            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>*<Translate
                  id="wizard.general.username"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="Username"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      value={field.value}
                      disabled={disabledFields}
                      onChange={handleUsernameChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
                {!validUsername && usernameAvailable &&(
                  <TextDangerStyled>
                    <Translate 
                      id={"wizard.general.username-5-characters"}
                      options={{ renderInnerHtml: true }} 
                    />
                  </TextDangerStyled>
                )}
                {!usernameAvailable && !disabledFields && (
                  <TextDangerStyled> <Translate
                    id="wizard.general.username-not-available"
                    options={{ renderInnerHtml: true }} /></TextDangerStyled>
                )}
              </div>
            </div>

            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>*<Translate
                  id="wizard.general.password"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="Password"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      type="password"
                      value={field.value}
                      disabled={disabledFields}
                      onChange={handlePasswordChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
                {!validPassword && (
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-password" />
                  </TextDangerStyled>
                )}
              </div>
            </div>
            {entityIdQuery &&
            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>* <Translate id="wizard.general.number-record" /></label>
                <Controller
                  name="UserSchoolRecord"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      value={field.value}
                      disabled={disabledFields}
                      onChange={handleRecordNumberChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
                {!validRecordNumber && recordAvailable && (
                  <TextDangerStyled>
                    <Translate id="wizard.general.valid-record-required" />
                  </TextDangerStyled>
                )}
                {!recordAvailable && (
                  <TextDangerStyled><Translate id="wizard.general.record-not-available" /></TextDangerStyled>
                )}
              </div>
            </div>
            }

            <hr />

            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>* <Translate
                  id="wizard.general.address"
                  options={{ renderInnerHtml: true }} /></label>

                {!validAddress && (
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-address" />
                  </TextDangerStyled>
                )}
                <Controller
                  name="AddressLine1"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      type="text"
                      value={field.value || ''} 
                      disabled={disabledFields}
                      onChange={handleAddressChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
                <Controller
                  name="AddressLine2"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      type="text"
                      value={field.value}
                      disabled={disabledFields}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
              </div>
            </div>
          </div>

          <div className="wrapper-wizard-form">
            <div className="wrapper-wizard-form--col__four">
              <div className="form-wizard-fields">
                <label>* <Translate
                  id="wizard.general.city"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="City"
                  control={control}
                  render={({ field }) => (
                    <input
                      {...field}
                      type="text"
                      value={field.value || ''}
                      disabled={disabledFields}
                      onChange={handleCityChange}
                      className="form-wizard-fields__input"
                    />
                  )}
                />
                {!validCity && (
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-city" />
                  </TextDangerStyled>
                )}
              </div>
            </div>

            <div className="wrapper-wizard-form--col__four">
              <div className="form-wizard-fields wizard-fields__select">
                <label>* <Translate
                  id="wizard.general.country"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="Country"
                  control={control}
                  render={({ field }) => (
                    <Select
                      {...field}
                      isDisabled={disabledFields}
                      onChange={handleCountryChange}
                      classNamePrefix="mantine-like"
                      className="mantine-like__control"
                      options={countries}
                    />
                  )}
                />
                {!validCountry && (
                  <TextDangerStyled>
                    <Translate
                      id={"wizard.general.country-select"}
                      options={{ renderInnerHtml: true }}
                    />
                  </TextDangerStyled>
                )}
              </div>
            </div>

            {watch("Country") && watch("Country").value !== "PRI" && (
              <div className="wrapper-wizard-form--col__four">
                <div className="form-wizard-fields">
                  <label>* <Translate
                    id="wizard.general.state"
                    options={{ renderInnerHtml: true }} /></label>
                  <Controller
                    name="State"
                    control={control}
                    render={({ field }) => (
                      <Select
                        {...field}
                        disabled={disabledFields}
                        onChange={handleStateChange}
                        classNamePrefix="mantine-like"
                        className="mantine-like__control"
                        options={states}
                      />
                    )}
                  />
                  {!validState && watch("Country") && watch("Country").value === "USA" && (
                    <TextDangerStyled>
                      <Translate
                        id={"wizard.general.state-required"}
                        options={{ renderInnerHtml: true }}
                      />
                    </TextDangerStyled>
                  )}
                </div>
              </div>
            )}

            <div className="wrapper-wizard-form--col__four">
              <div className="form-wizard-fields">
                <label>*<Translate
                  id="wizard.general.zip-code"
                  options={{ renderInnerHtml: true }} /></label>
                <Controller
                  name="ZipCode"
                  control={control}
                  render={({ field }) => (
                    <InputMask
                      {...field}
                      mask="99999"
                      maskChar=""
                      placeholder=""
                      disabled={disabledFields}
                      className="form-wizard-fields__input"
                      onChange={handleZipCodeChange}
                    />
                  )}
                />
                {!validZipCode &&(
                  <TextDangerStyled>
                    <Translate id="wizard.step-1.invalid-zip-code" />
                  </TextDangerStyled>
                )}
              </div>
            </div>
          </div>
          <div className="content-wizard__footer">
            {disabledFields ? null : (
              <button className="content-wizard--primary button-primary-custom" type="submit" disabled={!canSubmit}>
                {" "}
                <Translate
                  id="wizard.general.create-cliente"
                  options={{ renderInnerHtml: true }} 
                />{" "}
              </button>
            )}
          </div>
        </form>
      </div>
      {modalClientVisible && <ModalClients entityId={entityIdQuery} visible={modalClientVisible}
        updateParent={handleSelectedUser} />}
      {loading ? <LoadingFullWindow /> : null}
    </>
  );
};

export default Step1;
