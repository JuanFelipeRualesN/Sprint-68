import React, { useState, useEffect, useContext } from "react";
import { Nav,
  NavItem,
  NavLink,
  TabContent,
  TabPane, Row, Col, Alert } from "reactstrap";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import {
  getUsersAdvanced,
  updateUserInfoByAdmin,
  registerUserByAdmin,
  getStatusAccount,
} from "../../../api/users";
import FormGroupAddress from "../../shared-module-components/form/form-group-address";
import FormGroupNewPassword from "../../shared-module-components/form/form-group-new-password";
import FormGroupNewName from "../../shared-module-components/form/form-group-name";
import FormGroupPhone from "../../shared-module-components/form/form-group-phone";
import FormGroupUsername from "../../shared-module-components/form/form-group-username";
import FormGroupEmail from "../../shared-module-components/form/form-group-email";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import SelectorRoles from "../../shared-module-components/selector/selector-roles";
import Select from "react-select";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import UserSchoolsDataTable from "../../school-module/datatables/user-school-data-table";
import AdminProfileConnectionDataTable from "../datatables/admin-profile-connections-data-table";
import { getRegions } from "../../../api/schools";
import { AppContext } from "../../../App.Context";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import {CardElement, useElements, useStripe} from "@stripe/react-stripe-js";
import {updateCustomerPaymentMethod} from "../../../api/subscriptions";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faInfoCircle} from "@fortawesome/free-solid-svg-icons";

/**
 * Form to handle user
 */
const FormHandleUser = (props) => {
  const [addressChange, setAddressChange] = useState(false);
  const [addressLine1, setAddressLine1] = useState("");
  const [addressLine2, setAddressLine2] = useState("");
  const [city, setCity] = useState("");
  const [country, setCountry] = useState("");
  const [email, setEmail] = useState("");
  const [emailAvailable, setEmailAvailable] = useState(true);
  const [emailChange, setEmailChange] = useState(false);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [loading, setLoading] = useState(false);
  const [nameChange, setNameChange] = useState(false);
  const [password, setPassword] = useState("");
  const [passwordChange, setPasswordChange] = useState(false);
  const [passwordRetype, setPasswordRetype] = useState("");
  const [paymentMethodChange, setPaymentMethodChange] = useState(false);
  const [phone, setPhone] = useState("");
  const [phoneChange, setPhoneChange] = useState(false);
  const [proceedSubmitGeneral, setProceedSubmitGeneral] = useState(false);
  const [propsModal, setPropsModal] = useState(null);
  const [roleId, setRoleId] = useState(null);
  const [roleChange, setRoleChange] = useState(false);
  const [estatusChange, setEstatusChange] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [showPaymentMethod, setShowPaymentMethod] = useState(false);
  const [showProfileError, setShowProfileError] = useState(false); // This is when a profile role is chosen. The profile role cannot be created here. It must be created on the subscription area.
  const [stateValue, setStateValue] = useState("");
  const [stateRequired, setStateRequired] = useState(false); // If the state (address) is required
  const [stripeApiKey] = useState(Settings.getStripeToken());
  const [stripeUpdatePayment] = useState(true);
  const [subTokenStripe, setSubTokenStripe] = useState(null);
  const [userId, setUserId] = useState(null);
  const [username, setUsername] = useState("");
  const [usernameAvailable, setUsernameAvailable] = useState(true);
  const [usernameChange, setUsernameChange] = useState(false);
  const [validAddress, setValidAddress] = useState(true);
  const [validCity, setValidCity] = useState(true);
  const [validCountry, setValidCountry] = useState(true);
  const [validEmail, setValidEmail] = useState(true);
  const [validFirstName, setValidFirstName] = useState(true);
  const [validLastName, setValidLastName] = useState(true);
  const [validPassword, setValidPassword] = useState(true);
  const [validPhone, setValidPhone] = useState(true);
  const [validRole, setValidRole] = useState(true);
  const [validState, setValidState] = useState(true);
  const [validUsername, setValidUsername] = useState(true);
  const [validZipCode, setValidZipCode] = useState(true);
  const [visible, setVisible] = useState(props.visible);
  const [zipCode, setZipCode] = useState("");
  const [estatusOptions, setEstatusOptions] = useState([]);
  const [estatusSelected, setEstatusSelected] = useState(null);
  const [regionsOptions, setRegionsOptions] = useState([]);
  const [regionSelected, setRegionSelected] = useState(null);
  const [showRegions, setShowRegions] = useState(false);
  const [regionRequired, setRegionRequired] = useState(false);
  const [oreChange, setOreChange] = useState(false);
  // State for Stripe CardElement specific errors and completeness
  const [cardComplete, setCardComplete] = useState(false);
  const [stripeErrorMessage, setStripeErrorMessage] = useState(null);
  const [activeTab, setActiveTab] = useState("1");
  const isAdmin = SessionManager.getUserRole() === Settings.ROLE_EMPLOYEE_STR ||
      SessionManager.getUserRole() === Settings.ROLE_ADMIN_STR;
  const stripe = useStripe();
  const elements = useElements();


  const context = useContext(AppContext);
  const sessionDetails = SessionManager.getSessionDetails();

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    loadStatus();
    loadRegions();
    if (props.userId) {
      doSearch(props.userId);
    }
  }, []);

  useEffect(() => {
    evaluateUpdate();
  }, [props.isEdit, validUsername, validFirstName, validLastName, validEmail, validPhone,
      validAddress, validCity, validCountry, validZipCode, validState, validPassword, usernameAvailable, 
      roleId, showProfileError, stateRequired, regionRequired, usernameChange, nameChange, estatusChange,
      emailChange, phoneChange, addressChange, roleChange, oreChange, paymentMethodChange, subTokenStripe
  ]);

  const evaluateUpdate = () => {
    if (props.isEdit) {
      // Validate if the user is allowed to proceed
      const hasAnyChange = usernameChange || nameChange || emailChange || phoneChange || addressChange || 
        roleChange || estatusChange || oreChange || (passwordChange && validPassword);

      let canProceed = hasAnyChange;

      if (usernameChange && (!validUsername || !username?.trim().length || !usernameAvailable)) canProceed = false;
      if (nameChange && (!validFirstName || !validLastName || !firstName?.trim().length || !lastName?.trim().length)) canProceed = false;
      if (emailChange && (!validEmail || !email?.trim().length)) canProceed = false;
      if (phoneChange && (!validPhone || !phone?.trim().length)) canProceed = false;

      if (
        addressChange &&
        (
          !validAddress || !validCity || !validState || !validCountry || !validZipCode ||
          (!addressLine1?.trim().length && !addressLine2?.trim().length) ||
          !city?.trim().length || !country?.trim().length || !zipCode?.trim().length
        )
      ) {
        canProceed = false;
      }

      if (
        roleId === Settings.ROLE_MAIN_CLIENT_ID &&
        paymentMethodChange &&
        (!subTokenStripe || subTokenStripe === "")
      ) {
        canProceed = false;
      }

      if (roleChange && (!roleId || roleId === "")) canProceed = false;

      if (showProfileError) canProceed = false;

      setProceedSubmitGeneral(canProceed);
    } else {

      const canProceed =
        usernameAvailable &&
        validUsername &&
        username?.trim().length > 0 &&
        validFirstName &&
        firstName?.trim().length > 0 &&
        validLastName &&
        lastName?.trim().length > 0 &&
        validEmail &&
        email?.trim().length > 0 &&
        validPhone &&
        phone?.trim().length > 0 &&
        validAddress &&
        ((addressLine1?.trim().length > 0) || (addressLine2?.trim().length > 0)) &&
        validCity &&
        city?.trim().length > 0 &&
        validCountry &&
        country?.trim().length > 0 &&
        validZipCode &&
        zipCode?.trim().length > 0 &&
        ((validState && stateRequired) || !stateRequired) &&
        validPassword &&
        password?.length > 0 &&
        passwordRetype?.length > 0 &&
        roleId &&
        roleId.length > 0 &&
        !showProfileError &&
        !regionRequired;

      setProceedSubmitGeneral(canProceed);
    }
  };

  const savePhone = (e) => {
    setPhone(e.phone);
    setValidPhone(e.validPhone);
    setPhoneChange(true);
  };

  const saveName = (e) => {
    setFirstName(e.firstName);
    setLastName(e.lastName);
    setValidFirstName(e.validFirstName);
    setValidLastName(e.validLastName);
    setNameChange(true);
  };

  const saveUsername = (e) => {
    setUsername(e.username);
    setValidUsername(e.validUsername);
    setUsernameAvailable(e.usernameAvailable);
    setUsernameChange(true);
  };

  const savePassword = (e) => {
    setPassword(e.password);
    setPasswordRetype(e.passwordRetype);
    setValidPassword(e.validPassword);
    setPasswordChange(true);
  };

  const saveEmail = (e) => {
    setEmail(e.email);
    setEmailAvailable(e.emailAvailable);
    setValidEmail(e.validEmail);
    setEmailChange(true);
  };

  const saveRole = (e) => {
    // The child role cannot be added here. It must be added it the subscriptions area, and attach the profile to
    // the subscription
    if (e === Settings.ROLE_STUDENT_ID || e === Settings.ROLE_TEACHER_ID || e === Settings.ROLE_DIRECTOR_ID) {
      setShowProfileError(true);
    } else {
      setShowProfileError(false);
    }

    if (e === Settings.ROLE_MAIN_CLIENT_ID && props.isEdit) {
      setShowPaymentMethod(true);
    } else {
      setShowPaymentMethod(false);
    }

    if (e === Settings.ROLE_ADMIN_ORE_ID) {
      setShowRegions(true);
      setRegionRequired(true);
    } else {
      setShowRegions(false);
      setRegionRequired(false);
    }

    setRoleId(e);
    setRoleChange(true);
  };

  const saveAddress = (e) => {
    setAddressLine1(e.addressLine1);
    setAddressLine2(e.addressLine2);
    setCity(e.city);
    setCountry(e.country);
    setStateValue(e.state);
    setStateRequired(e.stateRequired);
    setValidAddress(e.validAddress);
    setValidCity(e.validCity);
    setValidCountry(e.validCountry);
    setValidPhone(e.validPhone);
    setValidState(e.validState);
    setValidZipCode(e.validZipCode);
    setZipCode(e.zipCode);
    setAddressChange(true);
  };

  // Hide or shows the window
  const toggle = (tab) => {
    if (activeTab !== tab) setActiveTab(tab);
  };

  // Call the results to display
  const doSearch = async (userId) => {
    setLoading(true);

    try {
      const response = await getUsersAdvanced(userId, null, null, null, null, null, null, null, null, null, null,0, 1);

      const user = response.data.UserInfo[0];

      let stateCountry = "";
      if (response.data.UserInfo[0].StateIsoCode !== null)
        stateCountry = response.data.UserInfo[0].StateIsoCode;
      else stateCountry = response.data.UserInfo[0].State;

      let ore;
      const oreId = user.OreId;
      if (oreId) {
        const regionsOpt = await loadRegions();
        ore = regionsOpt.find((item) => item.value === oreId);
      }

      setUserId(user.UserId);
      setFirstName(user.GivenName || "");
      setLastName(user.Surname || "");
      setPassword("");
      setPasswordRetype("");
      setUsername(user.Username || "");
      setEmail(user.Email || "");
      setPhone(user.Phone || "");
      setAddressLine1(user.AddressLine1 || "");
      setAddressLine2(user.AddressLine2 || "");
      setCity(user.City || "");
      setStateValue(stateCountry || "");
      setCountry(user.CountryIsoCode2 || "");
      setZipCode(user.Zip || "");
      setRoleId(user.RoleId);
      setShowPaymentMethod(user.RoleId === Settings.ROLE_MAIN_CLIENT_ID);
      setLoading(false);
      setEstatusSelected({
        value: user.StatusId,
        label: user.Status
      });
      setShowRegions(user.RoleId === Settings.ROLE_ADMIN_ORE_ID);
      setRegionSelected(ore ?? null);
    } catch {
      setLoading(false);
    }
  };

  // Save the details entered. Depends if the user is for update or it's a new user
  const saveDetails = async () => {
    setLoading(true);
    const cleanFirstName = trimAndNormalizeText(firstName);
    const cleanLastName = trimAndNormalizeText(lastName);
    
    try {
      if (userId) {
        const response = await updateUserInfoByAdmin(
          username,
          password,
          passwordRetype,
          email,
          cleanFirstName,
          cleanLastName,
          phone,
          addressLine1,
          addressLine2,
          city,
          stateValue,
          country,
          zipCode,
          country,
          roleId,
          regionSelected ? regionSelected.value : null,
          subTokenStripe,
          usernameChange,
          passwordChange,
          emailChange,
          nameChange,
          phoneChange,
          addressChange,
          roleChange,
          paymentMethodChange,
          userId,
          estatusSelected.value,
          oreChange
        );

        let message = "";
        for (let i = 0; i < response.data.Messages.length; i++) {
          message += response.data.Messages[i] + "<br/>";
        }

        let propsModal = {
          bodyText: message,
          visible: true,
        };

        if (message.length > 0) {
          setPropsModal(propsModal);
          setShowModal(true);
        }

        if (props.userId) {
          doSearch(props.userId);
        }

        setUsernameChange(false);
        setPasswordChange(false);
        setEmailChange(false);
        setNameChange(false);
        setPhoneChange(false);
        setAddressChange(false);
        setRoleChange(false);
        setPaymentMethodChange(false);
        setOreChange(false);
      } else {
        setLoading(true);
          try {
            const response = await registerUserByAdmin(
              username,
              password,
              passwordRetype,
              email,
              cleanFirstName,
              cleanLastName,
              phone,
              addressLine1,
              addressLine2,
              city,
              stateValue,
              country,
              zipCode,
              country,
              roleId,
              regionSelected ? regionSelected.value : null
            );

            let propsModal = {
              bodyText: response.data.Messages[0],
              visible: true,
            };
  
            if (response.data.UserId) {
              setPropsModal(propsModal);
              setShowModal(true);
  
              props.handleRedirect(response.data.UserId);
              doSearch(response.data.UserId);
            }

            setUsernameChange(false);
            setPasswordChange(false);
            setEmailChange(false);
            setNameChange(false);
            setPhoneChange(false);
            setAddressChange(false);
            setRoleChange(false);
            setPaymentMethodChange(false);
            setOreChange(false);
          } catch (error) {}
      }
    } catch (error) {} 
      finally {
      setLoading(false);
    }
  };

  const loadStatus = async () => {
    setLoading(true);
    try {
      const res = await getStatusAccount();
      const data = res.data;
      const options = data.map((item) => ({
        value: item.Id,
        label: item.Desc,
      }));
      setEstatusOptions(options);
      setLoading(false);
    } catch (error) {
      setLoading(false);
    }
  };

  const loadRegions = async () => {
    setLoading(true);
    let options = [];
    try {
      const res = await getRegions();
      const data = res.data;
      options = data.map((item) => ({ value: item.Id, label: item.Name }));
      setRegionsOptions(options);
      setLoading(false);
    } catch (error) {
      setLoading(false);
    }
    return options;
  };

  const handleChangeStatus = (selected) => {
    setEstatusSelected(selected);
    setEstatusChange(true);
  };

  const handleChangeRegion = (selected) => {
    setRegionSelected(selected);
    setRegionRequired(false);
    setOreChange(regionSelected && regionSelected.value !== selected.value);
  };

  const updatePaymentMethod = async (event) => {
    // Ensure event is passed if this is an event handler (e.g., from a button click)
    if (event) {
      event.preventDefault(); // Prevent default form submission if this is a form handler
    }

    setLoading(true); // Start loading state

    try {
      // Ensure Stripe.js and Elements are loaded
      if (!stripe || !elements) {
        throw new Error("Stripe.js has not loaded yet. Please try again.");
      }

      const cardElement = elements.getElement(CardElement);

      // Basic validation for CardElement completeness
      if (!cardElement || !cardElement._complete) { // Access _complete for direct check
        throw new Error("Please complete your card details.");
      }

      // Create a Payment Method from the CardElement
      const { paymentMethod, error: stripeError } = await stripe.createPaymentMethod({
        type: 'card',
        card: cardElement,
      });

      if (stripeError) {
        // Handle Stripe-specific errors (e.g., invalid card number)
        throw new Error(stripeError.message);
      }

      // Call your backend API with the new Payment Method ID
      const response = await updateCustomerPaymentMethod(email, userId, paymentMethod.id);

      if (response.status !== 200 && response.status !== 201) {
        // Handle backend-specific errors (e.g., validation errors from your server)
        let backendMessage = response.data?.Messages?.length > 0
            ? response.data.Messages[0]
            : "An unknown error occurred on the server.";
        throw new Error(backendMessage);
      }

      // Success case: Update UI for success
      let propsModal = {
        bodyText: response.data.Messages[0], // Or a translation key
        visible: true,
        // You might want a success type for your modal component
      };
      setPropsModal(propsModal);
      setShowModal(true);

      // Optionally clear the CardElement after successful update
      cardElement.clear();

    } catch (error) {
      // Catch any errors from Stripe API, backend, or custom validation
      console.error("Error updating payment method:", error);
      let errorMessage = error.message || "An unexpected error occurred.";

      let propsModal = {
        bodyText: errorMessage,
        visible: true,
        // You might want an error type for your modal component
      };
      setPropsModal(propsModal);
      setShowModal(true);

    } finally {
      // This will always run, regardless of success or failure
      setLoading(false);
    }
  };

  // Renders the generic modal
  const renderGenericModal = () => {
    return (
      <ModalGeneric
        data={propsModal}
        updateParent={toggleAlertModal}
      />
    );
  };

  // Toggles alert modal (alert from registration)
  const toggleAlertModal = () => {
    setShowModal(false);
  };
  const getCurrentState = () => ({
    addressChange, addressLine1, addressLine2, city, country, email, emailAvailable, emailChange,
    firstName, lastName, loading, nameChange, password, passwordChange, passwordRetype,
    paymentMethodChange, phone, phoneChange, proceedSubmitGeneral, propsModal, roleId, roleChange,
    showModal, showPaymentMethod, showProfileError, stateValue, stateRequired, stripeApiKey,
    stripeUpdatePayment, subTokenStripe, userId, username, usernameAvailable, usernameChange,
    validAddress, validCity, validCountry, validEmail, validFirstName, validLastName, validPassword,
    validPhone, validRole, validState, validUsername, validZipCode, visible, zipCode,
    estatusOptions, estatusSelected, regionsOptions, regionSelected, showRegions, regionRequired, oreChange,
  })

  const retrieveSubTokenStripe = (event) => {
    setCardComplete(event.complete);
    if (event.error) {
      setStripeErrorMessage(event.error.message);
    } else {
      setStripeErrorMessage(null);
    }
  };

  const trimAndNormalizeText = (text) => {
      return text.trim().replace(/\s{2,}/g, ' '); 
  };

  return  (
      <>
        <div className="wrapper-col borderGray-bottom block-display-responsive">
          <div className="col col-no-padding-left">
            <div className="title-principal">
              <Translate id="user-management.general-details" />
            </div>
          </div>
        </div>

        {/* Tabs Navigation */}
        <Nav tabs>
          <NavItem>
            <NavLink
                className={activeTab === "1" ? "nav-link-selected " : ""}
                onClick={() => toggle("1")}
            >
              <Translate id="profile-update.general-information"
                         options={{ renderInnerHtml: false }} />
            </NavLink>
          </NavItem>

          {userId !== null &&
              roleId !== null &&
              roleId !== Settings.ROLE_ADMIN_ID &&
              roleId !== Settings.ROLE_ADMIN_ORE_ID &&
              roleId !== Settings.ROLE_EMPLOYEE_ID && (
                  <>
                    <NavItem>
                      <NavLink
                          className={activeTab === "2" ? "nav-link-selected " : ""}
                          onClick={() => toggle("2")}
                      >
                        <Translate id="profile-update.user-schools"
                                   options={{ renderInnerHtml: false }} />
                      </NavLink>
                    </NavItem>

                    <NavItem>
                      <NavLink
                          className={activeTab === "3" ? "nav-link-selected " : ""}
                          onClick={() => toggle("3")}
                      >
                        <Translate id="connections.relations" options={{renderInnerHtml: true}}/>
                      </NavLink>
                    </NavItem>

                    {roleId === Settings.ROLE_MAIN_CLIENT_ID && (
                    <NavItem>
                      <NavLink
                          className={activeTab === "4" ? "nav-link-selected " : ""}
                          onClick={() => toggle("4")}
                      >
                        <Translate id="user-management.payment-method" />
                      </NavLink>
                    </NavItem>)}
                  </>
              )}
        </Nav>

        {/* Tabs Content */}
        <TabContent activeTab={activeTab} className="mt-3">
          {/* Tab 1 - General Info */}
          <TabPane tabId="1">
            <div className="wrapper-col mb-4 two-col-regular input-fullwidth block-display-responsive pr-4">
              <div className="two-col">
                <div className="form-group-fields label-homeschooler">
                  <FormGroupNewName
                      disabled={
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdmin) ||
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_ORE_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdminOre)
                      }
                      data={getCurrentState()}
                      updateParent={saveName}
                  />
                </div>
              </div>

              <div className="two-col">
                <div className="form-group-fields label-homeschooler inline-group col-group-form">
                  <FormGroupUsername
                      disabled={
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdmin) ||
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_ORE_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdminOre)
                      }
                      data={getCurrentState()}
                      updateParent={saveUsername}
                  />

                  <FormGroupPhone
                      disabled={
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdmin) ||
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_ORE_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdminOre)
                      }
                      data={getCurrentState()}
                      updateParent={savePhone}
                  />
                </div>
              </div>

              <div className="two-col">
                <div className="form-group-fields label-homeschooler">
                  <FormGroupEmail
                      disabled={
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdmin) ||
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_ORE_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdminOre)
                      }
                      data={getCurrentState()}
                      updateParent={saveEmail}
                  />
                </div>
              </div>

              <div className="two-col">
                <div className="form-group-fields label-homeschooler">
                  <FormGroupNewPassword
                      disabled={
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdmin) ||
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_ORE_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdminOre)
                      }
                      data={getCurrentState()}
                      updateParent={savePassword}
                  />
                </div>
              </div>

              <div className="two-col">
                <div className="form-group-fields__select form-group-fields__select-height">
                  <div className="admin-subtitle">
                    <Translate id="user-management.role" />
                  </div>
                  {typeof roleId !== "undefined" && roleId !== null && props.isEdit ? (
                      <SelectorRoles
                          roleId={roleId}
                          updateParent={saveRole}
                          disabled={
                              (isRoleAllowed(
                                      [Settings.ROLE_ADMIN_STR],
                                      sessionDetails.Roles
                                  ) &&
                                  !context.state.variables.UserManagementAdmin) ||
                              (isRoleAllowed(
                                      [Settings.ROLE_ADMIN_ORE_STR],
                                      sessionDetails.Roles
                                  ) &&
                                  !context.state.variables.UserManagementAdminOre)
                          }
                      />
                  ) : null}
                  {!props.isEdit ? (
                      <SelectorRoles
                          roleId={roleId}
                          updateParent={saveRole}
                          disabled={
                              (isRoleAllowed(
                                      [Settings.ROLE_ADMIN_STR],
                                      sessionDetails.Roles
                                  ) &&
                                  !context.state.variables.UserManagementAdmin) ||
                              (isRoleAllowed(
                                      [Settings.ROLE_ADMIN_ORE_STR],
                                      sessionDetails.Roles
                                  ) &&
                                  !context.state.variables.UserManagementAdminOre)
                          }
                      />
                  ) : null}
                </div>
                <Alert color="danger" isOpen={showProfileError}>
                  <Translate id="user-management.profiles-error" />
                </Alert>
                {showRegions && (
                    <div className="form-group-fields__select form-group-fields__select-height">
                      <label className="label-homeschooler clear-p">
                        * <Translate id="user-management.sub-title-regions" />
                      </label>
                      <Select
                          classNamePrefix="mantine-like"
                          className="mantine-like__control"
                          value={regionSelected}
                          placeholder={
                            <Translate id="user-management.select-regions" />
                          }
                          options={regionsOptions}
                          onChange={handleChangeRegion}
                      />
                    </div>
                )}
              </div>

              {props.isEdit && (
                  <div className="two-col">
                    <div className="form-group-fields__select form-group-fields__select-height">
                      <div className="admin-subtitle">
                        <Translate id="user-management.status" />
                      </div>
                      <Select
                          classNamePrefix="mantine-like"
                          className="mantine-like__control"
                          value={estatusSelected}
                          placeholder="Seleccione estatus"
                          options={estatusOptions}
                          onChange={handleChangeStatus}
                          isDisabled={
                              (isRoleAllowed(
                                      [Settings.ROLE_ADMIN_STR],
                                      sessionDetails.Roles
                                  ) &&
                                  !context.state.variables.UserManagementAdmin) ||
                              (isRoleAllowed(
                                      [Settings.ROLE_ADMIN_ORE_STR],
                                      sessionDetails.Roles
                                  ) &&
                                  !context.state.variables.UserManagementAdminOre)
                          }
                      />
                    </div>
                  </div>
              )}

              <div className="col-full-size mt-4">
                <div className="admin-subtitle">
                  <Translate id="user-management.address" />
                </div>

                <div className="form-group-fields form-group-fields__select form-group-fields__select-height label-homeschooler">
                  <FormGroupAddress
                      data={getCurrentState()}
                      updateParent={saveAddress}
                      disabled={
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdmin) ||
                          (isRoleAllowed(
                                  [Settings.ROLE_ADMIN_ORE_STR],
                                  sessionDetails.Roles
                              ) &&
                              !context.state.variables.UserManagementAdminOre)
                      }
                  />
                </div>
              </div>
              {(isRoleAllowed(
                      [Settings.ROLE_ADMIN_STR],
                      sessionDetails.Roles
                  ) &&
                  context.state.variables.UserManagementAdmin) ||
              (isRoleAllowed(
                      [Settings.ROLE_ADMIN_ORE_STR],
                      sessionDetails.Roles
                  ) &&
                  context.state.variables.UserManagementAdminOre) ? (
                  <div className="col-full-size text-right">
                    <button
                        className="btn-default button-primary-custom"
                        disabled={!proceedSubmitGeneral}
                        onClick={saveDetails}
                    >
                      <Translate id="general.submit" />
                    </button>
                  </div>
              ) : null}
            </div>
          </TabPane>

          {/* Tab 2 - Schools */}
          <TabPane tabId="2">
            <Row>
              <Col>
                <UserSchoolsDataTable userId={props.userId} />
              </Col>
            </Row>
          </TabPane>

          {/* Tab 3 - Profile Connections */}
          <TabPane tabId="3">
            <Row>
              <Col>
                <AdminProfileConnectionDataTable
                    name={firstName + " " + lastName}
                    userId={props.userId}
                    roleId={roleId}
                    availableDeleteButton={isAdmin}
                    availableAddButton={isAdmin}
                />
              </Col>
            </Row>
          </TabPane>
          <TabPane tabId="4">
            <Row>
              <Col sm={8}  md="6" lg="4">
                <Alert color="info">
                  <FontAwesomeIcon icon={faInfoCircle}/> {' '}
                <Translate id="form-updates.update-payment-information-security"/>
                </Alert>
              </Col>
            </Row>
            <Row>
              <Col sm="8" md="6" lg="4">
                <CardElement
                    id="card-element"
                    onChange={retrieveSubTokenStripe}
                    options={{
                      style: {
                        base: {
                          fontSize: '16px',
                          color: '#424770',
                          '::placeholder': {
                            color: '#aab7c4',
                          },
                        },
                        invalid: {
                          color: '#9e2146',
                        },
                      },
                    }}
                />
                {stripeErrorMessage && (
                    <div style={{ color: 'red', marginTop: '10px' }}>
                      {stripeErrorMessage}
                    </div>
                )}
              </Col>
              <Col sm="4">
                <button disabled={!cardComplete} className="btn-default button-primary-custom" onClick={updatePaymentMethod}>
                  <Translate id="general.submit" />
                </button>
              </Col>
            </Row>
          </TabPane>
        </TabContent>

        {loading ? <LoadingFullWindow /> : null}
        {showModal ? renderGenericModal() : null}
      </>
  );
};
export default withLocalize(FormHandleUser);
