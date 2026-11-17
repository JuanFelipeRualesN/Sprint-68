import React, { useState, useEffect } from 'react';
import { FormGroup, Col, Label, Row, Button, Input, Alert } from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import FormGroupUsername from "../../shared-module-components/form/form-group-username";
import FormGroupNewName from "../../shared-module-components/form/form-group-name";
import { registerProfile } from "../../../api/users";
import { insertSubscriptionToUser } from "../../../api/subscriptions";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import ModalAttachProfile from '../modals/modal-add-existing-profile-caution';
import ProfilesMainAccountDataTableEmployee from '../datatables/profiles-main-account-data-table-employee';
import FormGroupNewPassword from "../../shared-module-components/form/form-group-new-password";
import ModalSuccessSetupProfile from '../modals/modal-success-setup-profile-first-time';
import SelectorSex from "../../shared-module-components/selector/selector-sex";
import moment from 'moment';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import FormGroupEmail from "../../shared-module-components/form/form-group-email";
import { getAvailabilityByRecordNumber } from "../../../api/schools";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to set the profile to subscription for an existing client
 */
const FormAddProfileByEmployee = ({ roleId, schoolId, location, subscriptionId, updateParent }) => {
    const [calendarFocused, setCalendarFocused] = useState(false);
    const [createdAt, setCreatedAt] = useState(moment());
    const [didMount, setDidMount] = useState(false);
    const [dob, setDob] = useState(null);
    const [email, setEmail] = useState('');
    const [emailAvailable, setEmailAvailable] = useState(true);
    const [firstName, setFirstName] = useState('');
    const [goToDashboard, setGoToDashboard] = useState(false);
    const [goToDownload, setGoToDownload] = useState(false);
    const [lastName, setLastName] = useState('');
    const [loading, setLoading] = useState(false);
    const [mobile, setMobile] = useState(false);
    const [password, setPassword] = useState('');
    const [passwordRetype, setPasswordRetype] = useState('');
    const [proceedSubmit, setProceedSubmit] = useState(false);
    const [profileFullName, setProfileFullName] = useState('');
    const [profileId, setProfileId] = useState('');
    const [propsModal, setPropsModal] = useState(null);
    const [propsModalAddNewProfile, setPropsModalAddNewProfile] = useState(null);
    const [redoSearch, setRedoSearch] = useState(false);
    const [schoolRecordIdentifier, setSchoolRecordIdentifier] = useState('');
    const [sex, setSex] = useState('');
    const [showAddNewProfileModal, setShowAddNewProfileModal] = useState(false);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalAttachCaution, setShowModalAttachCaution] = useState(false);
    const [spacesLeft, setSpacesLeft] = useState(false);
    const [currentSubscriptionId, setCurrentSubscriptionId] = useState(
        typeof location !== 'undefined' && location !== null && typeof location.subscriptionId !== 'undefined' && location.subscriptionId !== null 
            ? location.subscriptionId 
            : subscriptionId
    );
    const [transactionSuccess, setTransactionSuccess] = useState(false);
    const [username, setUsername] = useState('');
    const [usernameAvailable, setUsernameAvailable] = useState(true);
    const [validFirstName, setValidFirstName] = useState(false);
    const [validLastName, setValidLastName] = useState(false);
    const [validPassword, setValidPassword] = useState(false);
    const [validUsername, setValidUsername] = useState(false);
    const [visibleModalAttach, setVisibleModalAttach] = useState(false);
    const [validEmail, setValidEmail] = useState(false);
    const [validRecordAvailable, setValidRecordAvailable] = useState(true);
    const [cameFromRegister, setCameFromRegister] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleExistingAccountSelected = (e) => {
        setProfileId(e.UserId);
        setProfileFullName(e.GivenName + ' ' + e.Surname + ' (' + e.Username + ')');
        setShowModalAttachCaution(true);
    };

    const handleIdentifierRecord = async (e) => {
        let schoolRecordIdentifier = e.currentTarget.value;
        setSchoolRecordIdentifier(schoolRecordIdentifier);
        if (schoolId && schoolRecordIdentifier) {
            const availabilityRecord = await validateAvailabilityNumberRecord(schoolId, schoolRecordIdentifier);
            setValidRecordAvailable(availabilityRecord);            
        }
    };

    const toggleAttachProfile = (e) => {
        setShowModalAttachCaution(!showModalAttachCaution);
        // If the user answered a yes to insert the existing account, then add it.
        if (e.answer)
            subtmitSelectedStudent();
    };

    // Toggles the alert modal, and according to the response, it sends it to the whatever it needs to be sent
    const toogleAlertAddNewProfile = (data) => {
        setShowAddNewProfileModal(false);

        const state = {
            calendarFocused, createdAt, didMount, dob, email, emailAvailable, firstName, goToDashboard, goToDownload,
            lastName, loading, mobile, password, passwordRetype, proceedSubmit, profileFullName, profileId,
            propsModal, propsModalAddNewProfile, redoSearch, schoolRecordIdentifier, sex, showAddNewProfileModal,
            showErrorModal, showModalAttachCaution, spacesLeft, subscriptionId: currentSubscriptionId, transactionSuccess,
            username, usernameAvailable, validFirstName, validLastName, validPassword, validUsername,
            visibleModalAttach, validEmail, validRecordAvailable
        };

        if (transactionSuccess && !data.answer) {
            setGoToDownload(true);
            updateParent(state);
        }

        if (cameFromRegister && !data.answer && transactionSuccess) {
            setGoToDownload(true);
            updateParent(state);
        }
        else if (transactionSuccess && !cameFromRegister && !data.answer) {
            setGoToDashboard(true);
            updateParent(state);
        }
    };

    // Sets if submit button is ready or not
    useEffect(() => {
        const isFormValid = 
            validFirstName &&
            validLastName &&
            validPassword &&
            validUsername &&
            validEmail &&
            usernameAvailable &&
            emailAvailable &&
            firstName.length > 0 &&
            lastName.length > 0 &&
            password.length > 0 &&
            passwordRetype.length > 0 &&
            username.length > 0 &&
            currentSubscriptionId != null &&
            (schoolId != null && schoolId !== '' ? validRecordAvailable : true) &&
            currentSubscriptionId.length > 0

        setProceedSubmit(isFormValid);
    }, [validFirstName, validLastName, validPassword, validUsername, validEmail, sex, usernameAvailable, emailAvailable,
        firstName, lastName, password, passwordRetype, username, currentSubscriptionId]);

    // Saves the username state and verifies if the next button is ready
    const saveUsername = (newValue) => {
        setUsername(newValue.username);
        setValidUsername(newValue.validUsername);
        setUsernameAvailable(newValue.usernameAvailable);
    };

    const saveEmail = (newValue) => {
        setEmail(newValue.email);
        setValidEmail(newValue.validEmail);
        setEmailAvailable(newValue.emailAvailable);
    };

    // Save the password state and verifies if the next button is ready
    const savePassword = (newValue) => {
        setPassword(newValue.password);
        setPasswordRetype(newValue.passwordRetype);
        setValidPassword(newValue.validPassword);
    };

    // Save the name details and verifies if the next button is ready
    const saveName = (newValue) => {
        setFirstName(newValue.firstName);
        setLastName(newValue.lastName);
        setValidFirstName(newValue.validFirstName);
        setValidLastName(newValue.validLastName);
    };

    const validateAvailabilityNumberRecord = async (entityId, record) => {
        const res = await getAvailabilityByRecordNumber(entityId, record);
        return res.data;
    };

    // Submit register
    const submitRegister = async () => {
        setLoading(true);
        const cleanedFirstname = trimAndNormalizeText(firstName);
        const cleanedLastname = trimAndNormalizeText(lastName);

        let apiCall = registerProfile(currentSubscriptionId,
            username,
            password, passwordRetype,
            cleanedFirstname, cleanedLastname,
            sex, dob, true, roleId,
            schoolRecordIdentifier,
            email);

        apiCall.then((response) => {
            setLoading(false);
            
            // Reset fields
            if (response.status === 200 || response.status === 201) {
                setFirstName('');
                setLastName('');
                setPassword('');
                setPasswordRetype('');
                setUsername('');
                setDob(null);
                setSex('');
                setValidPassword(false);
                setUsernameAvailable(true);
                setValidUsername(false);
                setValidFirstName(false);
                setValidLastName(false);
                setCreatedAt(moment());
                setCalendarFocused(false);
                setProceedSubmit(false);
                setTransactionSuccess(true);
                
                let propsModal = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(propsModal);
                setShowErrorModal(true);
                setRedoSearch(false);
            }
            else {
                let propsModal = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(propsModal);
                setShowErrorModal(true);
                setRedoSearch(false);
            }
        }).catch((error) => {
            setLoading(false);
        });
    };

    // Saves the sex
    const saveSex = (newValue) => {
        setSex(newValue);
    };

    // Saves the DOB
    const saveDob = (newValue) => {
        setDob(newValue.date);
    };

    // Toggles the success modal
    const toggleSuccessModal = () => {
        setShowModalSuccess(false);
    };

    // Toogles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    // Submits the selected student to the profile
    const subtmitSelectedStudent = async () => {        
        setLoading(true);
        try {
            const response = await insertSubscriptionToUser(profileId, currentSubscriptionId, true, roleId);
            let propsModal = {
                bodyText: response.data.Messages[0],
                visible: true
            };
            setPropsModal(propsModal);
            setShowErrorModal(true);
            setLoading(false);
        } catch (error) {            
            let msg = "";
            if (error.response.data.Messages && error.response.data.Messages.length) {
                error.response.data.Messages.forEach((message) => {
                  msg += message + "<br/>";
                });
              }
              let propsModal = {
                bodyText: msg,
                visible: true
            };
            setPropsModal(propsModal);
            setShowErrorModal(true);
            setLoading(false);
        }
    };
    const getCurrentState = () => ({
        calendarFocused, createdAt, didMount, dob, email, emailAvailable, firstName, goToDashboard, goToDownload,
        lastName, loading, mobile, password, passwordRetype, proceedSubmit, profileFullName, profileId,
        propsModal, propsModalAddNewProfile, redoSearch, roleId, schoolId: schoolId || null, schoolRecordIdentifier,
        sex, showAddNewProfileModal, showErrorModal, showModalAttachCaution, spacesLeft, 
        subscriptionId: currentSubscriptionId, transactionSuccess, username, usernameAvailable, validFirstName,
        validLastName, validPassword, validUsername, visibleModalAttach, validEmail, validRecordAvailable
    })

    const trimAndNormalizeText = (text) => {
        return text.trim().replace(/\s{2,}/g, ' '); 
    };

    // Renders the landing page
    return (
        <div style={{marginTop: '-10px' }} className="padding-20">
            <Row>
                <small><Translate id="subscription-set-profile-existing-account.existing-profile-instructions" /></small>
                <Col 
                    xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                    <hr />
                    <ProfilesMainAccountDataTableEmployee roleId={roleId} subscriptionId={currentSubscriptionId} redoSearch={redoSearch} updateParent={handleExistingAccountSelected} />
                </Col>
            </Row>
            <hr />
            <Row>
                <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                    <h6><Translate id="subscription-set-profile-existing-account.or-add-new-profile-instructions" /></h6>
                </Col>
            </Row>
            <Row>
                <Col style={{backgroundColor: 'whitesmoke' }}
                    xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                    <Row className="mt-15px label-size-small">
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
                            <FormGroupUsername data={getCurrentState()} updateParent={saveUsername} />
                        </Col>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
                            <FormGroupEmail data={getCurrentState()} updateParent={saveEmail} required={false} />
                        </Col>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                            <FormGroupNewPassword data={getCurrentState()} updateParent={savePassword} />
                        </Col>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                            <FormGroupNewName data={getCurrentState()} updateParent={saveName} />
                        </Col>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
                            <FormGroup row> 
                                <Col sm={{ size: 12 }}>
                                    <Label for="sex" className="label-forms-blue">
                                        <Translate id="profile-register.sex" />
                                    </Label>
                                    <SelectorSex data={getCurrentState()} updateParent={saveSex} />
                                </Col>
                            </FormGroup>
                        </Col>
                        {
                            typeof schoolId !== 'undefined' && schoolId !== null && schoolId !== '' ?
                                <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
                                    <FormGroup row>
                                        <Col sm={{ size: 12 }}>
                                            <Label for="school-identifier-record" className="label-forms-blue">
                                                <Translate id="profile-register.school-id-record" />
                                            </Label>
                                            <Input value={schoolRecordIdentifier}
                                                   className="mantine-like__control"
                                                type="text" name="school-identifier-record"
                                                id="school-identifier-record" onChange={handleIdentifierRecord} />
                                        </Col>
                                        {!validRecordAvailable &&
                                            <Alert color="danger">
                                                <Translate id="wizard.general.record-not-available" />
                                            </Alert>
                                        }
                                    </FormGroup>
                                </Col> : null}
                    </Row>
                    <Row>
                        <Col xs={{ size: 12 }} >
                            <Button style={{width: '170px'}}  block color="success" className="" disabled={!proceedSubmit ? "disabled" : ""} onClick={submitRegister}>
                                <h4 style={{marginBottom: '0', color: 'white'}}><Translate id="profile-register.submit-profile" /></h4>
                            </Button>
                        </Col>
                    </Row>
                </Col>
            </Row>

            {loading && <LoadingFullWindow />}
            {showErrorModal && <ModalGeneric data={propsModal} updateParent={toggleAlertModal} />}
            {showAddNewProfileModal &&
                <ModalSuccessSetupProfile visible={showAddNewProfileModal} spacesLeft={spacesLeft}
                    updateParent={toogleAlertAddNewProfile} />}
            {showModalAttachCaution &&
                <ModalAttachProfile visibleModalAttach={showModalAttachCaution}
                    studentName={profileFullName}
                    updateParent={toggleAttachProfile}
                />}
        </div>
    );
};

export default withLocalize(FormAddProfileByEmployee);