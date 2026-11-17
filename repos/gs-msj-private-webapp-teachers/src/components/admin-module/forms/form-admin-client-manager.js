/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import {registerUserByAdmin, getUsersAdvanced} from "../../../api/users";
import { Row, Col, Button} from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import FormGroupUsername from "../../shared-module-components/form/form-group-username";
import FormGroupEmail from "../../shared-module-components/form/form-group-email";
import FormGroupNewPassword from "../../shared-module-components/form/form-group-new-password";
import FormGroupAddress from "../../shared-module-components/form/form-group-address";
import FormGroupNewName from "../../shared-module-components/form/form-group-name";
import FormGroupPhone from "../../shared-module-components/form/form-group-phone";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import ModalClients from '../modals/modal-clients';
import Settings from '../../../custom-classes/settings';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The form to register the client by the admin
 */
const FormRegistrationByAdmin = ({data, updateParent}) => {
    const [addressLine1, setAddressLine1] = useState('');
    const [addressLine2, setAddressLine2] = useState('');
    const [city, setCity] = useState('');
    const [country, setCountry] = useState('');
    const [didMount, setDidMount] = useState(false); // If the component did mount
    const [disabledAddress, setDisabledAddress] = useState(false); // To disable address field
    const [disabledEmail, setDisabledEmail] = useState(false); // To Disable email field
    const [disabledName, setDisabledName] = useState(false); // To disable name field
    const [disabledPassword, setDisabledPassword] = useState(false); // To disable the password field
    const [disabledPhoneNumber, setDisabledPhoneNumber] = useState(false); // To disable phone number field
    const [disabledUsername, setDisabledUsername] = useState(false); // To disable the username field
    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [loading, setLoading] = useState(false); // To show or hide the loading window
    const [mobile, setMobile] = useState(false);
    const [modalClientVisible, setModalClientVisible] = useState(false); // To hide or show the modal to choose clients
    const [password, setPassword] = useState('');
    const [passwordRetype, setPasswordRetype] = useState('');
    const [phone, setPhone] = useState('');
    const [proceedToInsert, setProceedToInsert] = useState(false); // To proceed to the insertation. It evaluates if the fields are correct.
    const [propsModal, setPropsModal] = useState(null); // The generic data for the generic modal of messages
    const [showAddExistingAccountButton, setShowAddExistingAccountButton] = useState(true); // To hide or show the existing button
    const [showInsertionButton, setShowInsertionButton] = useState(true); // To hide or show the insertion button of the form
    const [showModal, setShowModal] = useState(false); // If show the generic modal
    const [state, setState] = useState('');
    const [stateRequired, setStateRequired] = useState(false); // If the state (address) is required
    const [successfulTransaction, setSuccessfulTransaction] = useState(true); // If the transaction was successful or not. The default is true, so we don't show any error messages at the beggining
    const [userId, setUserId] = useState(null); // The ID of the user (created or selected)
    const [username, setUsername] = useState('');
    const [usernameAvailable, setUsernameAvailable] = useState(true);
    const [validAddress, setValidAddress] = useState(false);
    const [validCity, setValidCity] = useState(false);
    const [validCountry, setValidCountry] = useState(false);
    const [validEmail, setValidEmail] = useState(false);
    const [validFirstName, setValidFirstName] = useState(false);
    const [validLastName, setValidLastName] = useState(false);
    const [validPassword, setValidPassword] = useState(false);
    const [validPhone, setValidPhone] = useState(false);
    const [validState, setValidState] = useState(false);
    const [validUsername, setValidUsername] = useState(false);
    const [validZipCode, setValidZipCode] = useState(false);
    const [zipCode, setZipCode] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        if (typeof data.userData !== 'undefined' && data.userData !== null) {
            const user = data.userData[0];

            setAddressLine1(user.Address.AddressLine1);
            setAddressLine2(user.Address.AddressLine2);
            setCity(user.Address.City);
            setCountry(user.Address.CountryIsoCode2);
            setDisabledAddress(data.disabledAddress);
            setDisabledEmail(data.disabledEmail);
            setDisabledName(data.disabledName);
            setDisabledPassword(data.disabledPassword);
            setDisabledPhoneNumber(data.disabledPhoneNumber);
            setDisabledUsername(data.disabledUsername);
            setEmail(user.Email);
            setFirstName(user.FullName.GivenName);
            setLastName(user.FullName.Surname);
            setPhone(user.Phone);
            setShowAddExistingAccountButton(false);
            setShowInsertionButton(false); // Is for read
            setState(user.Address.StateIsoCode);
            setStateRequired(typeof user.Address.StateIsoCode !== 'undefined' && user.Address.StateIsoCode !== null);
            setUsername(user.Username);
            setZipCode(user.Address.Zip);
        }

        if(data.didMount)
        {
            setMobile(data.mobile);
        }
    }, [data]);

    // Searchs the client and put the data in the info fillers
    const searchClient = () => {
        let apiCallUser = getUsersAdvanced(userId, null, null, null, null, null, null, null,null, null, null, 1, 1);
        apiCallUser.then((response)=>{
            const currentState = {
                addressLine1: response.data.UserInfo[0].Address.AddressLine1,
                addressLine2: response.data.UserInfo[0].Address.AddressLine2,
                city: response.data.UserInfo[0].Address.City,
                country: response.data.UserInfo[0].Address.CountryIsoCode2,
                disabledAddress: true,
                disabledEmail: true,
                disabledName: true,
                disabledPassword: true,
                disabledPhoneNumber: true,
                disabledUsername: true,
                email: response.data.UserInfo[0].Email,
                firstName: response.data.UserInfo[0].FullName.GivenName,
                lastName: response.data.UserInfo[0].FullName.Surname,
                phone: response.data.UserInfo[0].Phone,
                showInsertionButton: false,
                state: response.data.UserInfo[0].Address.StateIsoCode,
                stateRequired: typeof response.data.UserInfo[0].Address.StateIsoCode !== 'undefined' && response.data.UserInfo[0].Address.StateIsoCode !== null,
                username: response.data.UserInfo[0].Username,
                zipCode: response.data.UserInfo[0].Address.Zip,
            };
            
            setAddressLine1(currentState.addressLine1);
            setAddressLine2(currentState.addressLine2);
            setCity(currentState.city);
            setCountry(currentState.country);
            setDisabledAddress(currentState.disabledAddress);
            setDisabledEmail(currentState.disabledEmail);
            setDisabledName(currentState.disabledName);
            setDisabledPassword(currentState.disabledPassword);
            setDisabledPhoneNumber(currentState.disabledPhoneNumber);
            setDisabledUsername(currentState.disabledUsername);
            setEmail(currentState.email);
            setFirstName(currentState.firstName);
            setLastName(currentState.lastName);
            setPhone(currentState.phone);
            setShowInsertionButton(currentState.showInsertionButton);
            setState(currentState.state);
            setStateRequired(currentState.stateRequired);
            setUsername(currentState.username);
            setZipCode(currentState.zipCode);
            
            updateParent(currentState);
        });
    };

    // Handles the selected client from the clients modal
    const handleSelectedUser = (e) => {
        setModalClientVisible(false);
        setUserId(e !== null && e.UserId !== null ? e.UserId : null);
        
        if((e !== null && e.UserId !== null ? e.UserId : null) !== null)
            searchClient();
    };

    // Renders the generic modal
    const renderGenericModal = () => {
        return (
            <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/>
        );
    };

    // Toggles alert modal (alert from registration)
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Submit register
    const submitRegister = () => {
        setLoading(true);

        // Submits the registration
        let apiCall = registerUserByAdmin(username, password, passwordRetype,
            email, firstName, lastName, phone, addressLine1,
            addressLine2,
            city, state, country, zipCode,
            country,
            Settings.ROLE_MAIN_CLIENT_ID, null);
        apiCall.then((response)=>{
            let propsModalObj = {
                bodyText: response.data.Messages[0],
                visible: true
            };

            if(response.status === 200 || response.status === 201) {
                if (response.data.UserId !== null) {
                    setDisabledAddress(true);
                    setDisabledEmail(true);
                    setDisabledName(true);
                    setDisabledPassword(true);
                    setDisabledPhoneNumber(true);
                    setDisabledUsername(true);
                    setLoading(false);
                    setProceedToInsert(false);
                    setPropsModal(propsModalObj);
                    setShowAddExistingAccountButton(false);
                    setShowModal(true);
                    setUserId(response.data.UserId);
                }
            }
            
            const currentState = {
                addressLine1, addressLine2, city, country, disabledAddress: true,
                disabledEmail: true, disabledName: true, disabledPassword: true,
                disabledPhoneNumber: true, disabledUsername: true, email, firstName,
                lastName, loading: false, phone, proceedToInsert: false,
                showAddExistingAccountButton: false, state, username, zipCode,
                userId: response.data.UserId
            };
            updateParent(currentState);
        });
    };

    // Sets if submit button is ready or not
    const setSubmitButtonState = () => {
        setProceedToInsert(
            validUsername
            && validFirstName
            && validLastName
            && validUsername
            && validPassword
            && validEmail
            && validAddress
            && validCity
            && validCountry
            && (
                (stateRequired && validState)
                || (!stateRequired)
            )
            && validZipCode
        );
    };

    // Saves the username state and verifies if the next button is ready
    const saveUsername = (newValue) => {
        setUsername(newValue.username);
        setValidUsername(newValue.validUsername);
        setUsernameAvailable(newValue.usernameAvailable);
        setSubmitButtonState();
    };

    // Save the email state and verifies  if the next button is ready
    const saveEmail = (newValue) => {
        setEmail(newValue.email);
        setValidEmail(newValue.validEmail);
        setSubmitButtonState();
    };

    // Save the password state and verifies if the next button is ready
    const savePassword = (newValue) => {
        setPassword(newValue.password);
        setPasswordRetype(newValue.passwordRetype);
        setValidPassword(newValue.validPassword);
        setSubmitButtonState();
    };

    // Save the name details and verifies if the next button is ready
    const saveName = (newValue) => {
        setFirstName(newValue.firstName);
        setLastName(newValue.lastName);
        setValidFirstName(newValue.validFirstName);
        setValidLastName(newValue.validLastName);
        setSubmitButtonState();
    };

    // Save the phone details and verifies if the next button is ready
    const savePhone = (newValue) => {
        setPhone(newValue.phone);
        setValidPhone(newValue.validPhone);
        setSubmitButtonState();
    };

    // Saves the address details and verifies if the next button is ready
    const saveAddress = (newValue) => {
        setAddressLine1(newValue.addressLine1);
        setAddressLine2(newValue.addressLine2);
        setCity(newValue.city);
        setCountry(newValue.country);
        setState(newValue.state);
        setValidAddress(newValue.validAddress);
        setValidCity(newValue.validCity);
        setValidCountry(newValue.validCountry);
        setValidState(newValue.validState);
        setValidZipCode(newValue.validZipCode);
        setZipCode(newValue.zipCode);
        setSubmitButtonState();
    };

    const handleClientButton = () => {
        setModalClientVisible(true);
    };

    const getCurrentState = () => ({
        addressLine1, addressLine2, city, country, didMount, disabledAddress,
        disabledEmail, disabledName, disabledPassword, disabledPhoneNumber,
        disabledUsername, email, firstName, lastName, loading, mobile,
        modalClientVisible, password, passwordRetype, phone, proceedToInsert,
        showAddExistingAccountButton, showInsertionButton, showModal, state,
        stateRequired, successfulTransaction, userId, username, usernameAvailable,
        validAddress, validCity, validCountry, validEmail, validFirstName,
        validLastName, validPassword, validPhone, validState, validUsername,
        validZipCode, zipCode
    });

    // Renders the landing page
    return (
        <div>
            <Row>
                <h2><b><Translate id="register.client-data"/></b></h2>
            </Row>
            <hr/>
            <Row>
                <Col sm={{size: 12}}>
                    <h3>1. <Translate id="register.general-information"/></h3>
                    <small> <Translate id="register.general-information-instructions"/></small>
                    {' '}
                    {
                        showAddExistingAccountButton ?
                            <button className="btn-default button-primary-custom" onClick={handleClientButton}>
                                <Translate id="register.add-existing-account"/>
                            </button> : null
                    }
                    <hr/>
                </Col>
                <Col sm={{size:12}}>
                    <FormGroupNewName disabled={disabledName} data={getCurrentState()} updateParent={saveName}/>
                    <FormGroupPhone disabled={disabledPhoneNumber} data={getCurrentState()} updateParent={savePhone}/>
                </Col>
            </Row>
            <Row>
                <Col sm={{size: 12}}>
                    <h3>2. <Translate id="register.account-details"/></h3>
                    <small> <Translate id="register.account-details-instructions"/></small>
                    <hr/>
                </Col>
                <Col sm={{size:12}}>
                    <FormGroupUsername disabled={disabledUsername} data={getCurrentState()} updateParent={saveUsername}/>
                    <FormGroupNewPassword disabled={disabledPassword} data={getCurrentState()} updateParent={savePassword}/>
                    <FormGroupEmail disabled={disabledEmail} data={getCurrentState()} updateParent={saveEmail}/>
                </Col>
            </Row>
            <Row>
                <Col sm={{size: 12}}>
                    <h3>3. <Translate id="register.billing-address"/></h3>
                     <small> <Translate id="register.billing-address-instructions"/></small>
                    <hr/>
                </Col>
                <Col sm={{size:12}}>
                    <FormGroupAddress showAddressLines={true} showAddressZipCode={true} disabled={disabledAddress} data={getCurrentState()} updateParent={saveAddress}/>
                </Col>
            </Row>
            <hr/>
            {showInsertionButton ?
                <Row>
                    <Col sm={{size: 8}}>
                        <h4><Translate id="register.client-data-submit-instructions"/></h4>
                    </Col>
                    <Col sm={{size:4}}>
                        <Button onClick={submitRegister} color="success" block disabled={!proceedToInsert}>
                            <Translate id="general.submit"/>
                        </Button>
                    </Col>
                </Row>: null
            }
            <div>
                {loading ? <LoadingFullWindow/> : null}
                {showModal ? renderGenericModal() : null}
                {modalClientVisible ? <ModalClients visible={modalClientVisible}
                                                               updateParent={handleSelectedUser} /> : null}
            </div>
        </div>
    )
};

export default withLocalize(FormRegistrationByAdmin);