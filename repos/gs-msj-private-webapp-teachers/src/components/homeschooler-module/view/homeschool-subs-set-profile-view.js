import React, { useState, useEffect, useCallback } from 'react';
import { Redirect, useLocation, useHistory } from 'react-router-dom';
import { Container, FormGroup, Col, Label, Row, Button} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';
import FormGroupUsername from '../../shared-module-components/form/form-group-username';
import FormGroupName from '../../shared-module-components/form/form-group-name';
import {registerProfile} from '../../../api/users';
import {insertSubscriptionToUser} from '../../../api/subscriptions';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import ModalAttachProfile from '../modal/modal-add-existing-profile-caution';
import ProfilesDataTable from '../../shared-module-components/datatables/profiles-data-table';
import FormGroupNewPassword from '../../shared-module-components/form/form-group-new-password';
import ModalSuccessSetupProfile from '../../shared-module-components/modal/modal-success-setup-profile-first-time';
import SelectorSex from '../../shared-module-components/selector/selector-sex';
import moment from 'moment';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUserPlus} from "@fortawesome/free-solid-svg-icons";
import "./style.css";

import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import SessionManager from '../../../custom-classes/session-manager';
import { SendAccessLog } from '../../../utils/access_logger';

/**
 * The component (window) to set the profile to subscription for an existing client
 */
const HomeschoolSubsSetProfileView = (props) => {
    // React Router v5 hooks
    const location = useLocation();
    const history = useHistory(); // For programmatic navigation
    const userDetails = SessionManager.getSessionDetails();

    // --- State Hooks ---
    const [calendarFocused, setCalendarFocused] = useState(false);
    const [createdAt, setCreatedAt] = useState(moment());
    // `didMount` state is typically not needed in functional components with useEffect,
    // as useEffect with an empty dependency array acts as componentDidMount.
    const [dob, setDob] = useState(null);
    const [firstName, setFirstName] = useState('');
    const [goToDashboard, setGoToDashboard] = useState(false);
    const [goToDownload, setGoToDownload] = useState(false);
    const [lastName, setLastName] = useState('');
    const [loading, setLoading] = useState(false);
    const [password, setPassword] = useState('');
    const [passwordRetype, setPasswordRetype] = useState('');
    const [proceedSubmit, setProceedSubmit] = useState(false);
    const [profileFullName, setProfileFullName] = useState('');
    const [profileId, setProfileId] = useState('');
    const [profileUsername, setProfileUsername] = useState('');
    const [profileInvitationCode, setProfileInvitationCode] = useState('');
    const [profileRole, setProfileRole] = useState('');
    const [propsModal, setPropsModal] = useState(null);
    // `propsModalAddNewProfile` was in the original state but unused. Keeping for reference.
    // const [propsModalAddNewProfile, setPropsModalAddNewProfile] = useState(null);
    const [redoSearch, setRedoSearch] = useState(false);
    const [roleId] = useState(Settings.ROLE_STUDENT_ID); // Assuming ROLE_STUDENT_ID is static
    const [sex, setSex] = useState('');
    const [showAddNewProfileModal, setShowAddNewProfileModal] = useState(false);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalAttachCaution, setShowModalAttachCaution] = useState(false);
    // `showModalSuccess` was in the original state, but `toggleSuccessModal` didn't seem to be called.
    // Assuming `toggleSuccessModal` might have been intended for `showAddNewProfileModal`.
    const [spacesLeft, setSpacesLeft] = useState(false); // Value is never set in the original class.
    // Get subscriptionId from location.state or a direct prop (common in v5 routing)
    const [subscriptionId] = useState(location.subscriptionId || location.state?.subscriptionId);
    const [transactionSuccess, setTransactionSuccess] = useState(false);
    const [username, setUsername] = useState('');
    const [usernameAvailable, setUsernameAvailable] = useState(true);
    const [validFirstName, setValidFirstName] = useState(false);
    const [validLastName, setValidLastName] = useState(false);
    const [validPassword, setValidPassword] = useState(false);
    const [validUsername, setValidUsername] = useState(false);
    // `visibleModalAttach` was in the original state but unused in logic.
    // const [visibleModalAttach, setVisibleModalAttach] = useState(false);

    // `cameFromRegister` was used in `toggleAlertAddNewProfile` but not defined in state.
    const [cameFromRegister, setCameFromRegister] = useState(false); // Added as a placeholder

    // Initialize translations based on the current version
    useVersionedTranslations();

    // --- Helper Functions and Callbacks ---

    const handleExistingAccountSelected = useCallback((e) => {
        setProfileUsername(e.Username);
        setProfileInvitationCode(e.Token);
        setProfileRole(e.Role);
        setProfileId(e.UserId);
        setProfileFullName(`${e.GivenName} ${e.Surname} (${e.Username})`);
        setShowModalAttachCaution(true);
    }, []);

    const submitSelectedStudent = useCallback(async () => {
        setLoading(true);
        try {
            SendAccessLog({
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "24",
                Details: "Choose an existing profile to add to the enrollment",
                SubscriptionId: subscriptionId,
                ProfileInformation: {
                    UserId: profileId,
                    Role: profileRole,
                    InvitationCode: profileInvitationCode,
                    UserName: profileUsername
                }
            })
        }
        catch (error) {
            // empty catch block to avoid breaking the application
        }

        try {
            const response = await insertSubscriptionToUser(profileId, subscriptionId, false, roleId);
            const newPropsModal = {
                bodyText: response.data.Messages[0],
                visible: true
            };
            setPropsModal(newPropsModal);
            setShowErrorModal(true);
            setLoading(false);
        } catch (error) {
            setLoading(false);
            let msg = "An unexpected error occurred.";
            if (error.response && error.response.data && error.response.data.Messages && error.response.data.Messages.length) {
                msg = error.response.data.Messages.join("<br/>"); // Join messages from array
            }
            const newPropsModal = {
                bodyText: msg,
                visible: true
            };
            setPropsModal(newPropsModal);
            setShowErrorModal(true);
        }
    }, [profileId, subscriptionId, roleId]); // Dependencies

    const toggleAttachProfile = useCallback((e) => {
        setShowModalAttachCaution(prev => !prev);
        // If the user answered 'yes' to insert the existing account
        if (e.answer) {
            submitSelectedStudent();
        }
    }, [submitSelectedStudent]); // Dependency: submitSelectedStudent

    const toggleAlertAddNewProfile = useCallback((data) => {
        setShowAddNewProfileModal(false);

        // This logic remains the same, but using hook state and `history.push`
        // instead of setting state then calling updateParent
        if (transactionSuccess && !data.answer) {
            setGoToDownload(true);
            // Assuming updateParent might trigger navigation or update higher-level state
            props.updateParent({ goToDownload: true, transactionSuccess, cameFromRegister });
        } else if (cameFromRegister && !data.answer && transactionSuccess) {
            setGoToDownload(true);
            props.updateParent({ goToDownload: true, transactionSuccess, cameFromRegister });
        } else if (transactionSuccess && !cameFromRegister && !data.answer) {
            setGoToDashboard(true);
            props.updateParent({ goToDashboard: true, transactionSuccess, cameFromRegister });
        }
        // If goToDashboard or goToDownload flags are set, you might want to navigate here
        // For example:
        // if (goToDashboard) history.push('/dashboard');
        // if (goToDownload) history.push('/download'); // Or whatever the download path is
        // However, the original code passes state to updateParent, suggesting parent handles navigation.
    }, [transactionSuccess, cameFromRegister, props.updateParent]); // Dependencies

    const setSubmitButtonState = useCallback(() => {
        setProceedSubmit(
            validFirstName &&
            validLastName &&
            validPassword &&
            validUsername &&
            usernameAvailable &&
            subscriptionId !== null && subscriptionId?.length > 0
        );
    }, [validFirstName, validLastName, validPassword, validUsername, usernameAvailable, subscriptionId]);

    // Simplified save functions - they now directly receive newValue
    const saveUsername = useCallback((newValue) => {
        setUsername(newValue.username);
        setValidUsername(newValue.validUsername);
        setUsernameAvailable(newValue.usernameAvailable);
    }, []); // No dependencies as setters are stable

    const savePassword = useCallback((newValue) => {
        setPassword(newValue.password);
        setPasswordRetype(newValue.passwordRetype);
        setValidPassword(newValue.validPassword);
    }, []);

    const saveName = useCallback((newValue) => {
        setFirstName(newValue.firstName);
        setLastName(newValue.lastName);
        setValidFirstName(newValue.validFirstName);
        setValidLastName(newValue.validLastName);
    }, []);

    const submitRegister = useCallback(async () => {
        setLoading(true);
        try {
            const response = await registerProfile(
                subscriptionId,
                username,
                password,
                passwordRetype,
                firstName,
                lastName,
                sex,
                dob,
                false, // isParents
                roleId,
                null,
                null,
                true
            );

            setLoading(false);

            if (response.status === 200 || response.status === 201) {
                // Reset form fields
                try {
                    SendAccessLog({
                        UserName: userDetails.Username,
                        InvitationCode: userDetails.InvitationCode,
                        Role: userDetails.Roles[0],
                        AreaId: "24",
                        Details: "Create a new profile for student",
                        SubscriptionId: subscriptionId,
                        ProfileInformation: {
                            Role: "student",
                            UserName: username,
                            GiveName: firstName,
                            SurName: lastName
                        }
                    })
                }
                catch (error) {
                    // empty catch block to avoid breaking the application
                }
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

                const newPropsModal = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(newPropsModal);
                setShowErrorModal(true);
                // The original code sets redoSearch true then false in a callback.
                // This pattern is often used to re-trigger an effect.
                setRedoSearch(true);
                setTimeout(() => setRedoSearch(false), 0); // Reset for next potential trigger
            } else {
                const newPropsModal = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(newPropsModal);
                setShowErrorModal(true);
                setRedoSearch(false);
            }
        } catch (error) {
            setLoading(false);
            let msg = "An unexpected error occurred.";
            if (error.response && error.response.data && error.response.data.Messages && error.response.data.Messages.length) {
                // Assuming Messages is an array, join them
                msg = error.response.data.Messages.join("<br/>");
            }
            const newPropsModal = {
                bodyText: msg,
                visible: true
            };
            setPropsModal(newPropsModal);
            setShowErrorModal(true);
        }
    }, [subscriptionId, username, password, passwordRetype, firstName, lastName, sex, dob, roleId]); // Dependencies

    // --- Modal Toggles ---
    const toggleSuccessModal = useCallback(() => {
        // Based on the original class's `showModalSuccess` and its typical usage,
        // this might be intended to close the `ModalSuccessSetupProfile` if there's no separate state.
        setShowAddNewProfileModal(false);
    }, []);

    const toggleAlertModal = useCallback(() => {
        setShowErrorModal(false);
    }, []);

    // --- Effects ---
    // Effect to update submit button state whenever relevant form fields change
    useEffect(() => {
        setSubmitButtonState();
    }, [setSubmitButtonState, firstName, lastName, password, username, usernameAvailable]); // subscriptionId is stable, not needed here

    // --- Redirection Logic (React Router v5) ---
    // If subscriptionId is missing, redirect to dashboard.
    // In React Router v5, `Redirect` is a component you return.
    if (subscriptionId === null || typeof subscriptionId === 'undefined' || subscriptionId === '') {
        return <Redirect to="/dashboard"/>;
    }

    return (
        <div>
            <div className="container-padding animated fadeIn delay-05s">
                <Container className="container-wrapper">
                    <div>
                        <h2><Translate id="subscription-set-profile-existing-account.title"/></h2>
                        <div className="section-description mb-3">
                            <small>
                                <Translate id="subscription-set-profile-existing-account.existing-profile-instructions"/></small>
                        </div>
                    </div>

                    <Row>
                        <Col className="syle-date-five">
                            <ProfilesDataTable
                                roleId={roleId}
                                subscriptionId={subscriptionId}
                                redoSearch={redoSearch}
                                updateParent={handleExistingAccountSelected}/>
                        </Col>
                    </Row>
                    <hr/>
                    <Row>
                        <Col>
                            <h2><Translate id="subscription-set-profile-existing-account.create-new-profile"/></h2>
                            <small><Translate id="subscription-set-profile-existing-account.or-add-new-profile-instructions"/></small>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Row className="margin-top-10">
                                <Col className="inputs-homeschooler" xs={{size: 12}} md={{size: 6}}>
                                    {/* Pass only the relevant data to child components */}
                                    <FormGroupUsername data={{ username, validUsername, usernameAvailable }} updateParent={saveUsername}/>
                                </Col>
                                <Col className="inputs-homeschooler" xs={{size: 12}} md={{size: 6}}>
                                    <FormGroupNewPassword data={{ password, passwordRetype, validPassword }} updateParent={savePassword}/>
                                </Col>
                                <Col className="inputs-homeschooler" xs={{size: 12}} md={{size: 6}}>
                                    <FormGroupName data={{ firstName, lastName, validFirstName, validLastName }} updateParent={saveName}/>
                                </Col>
                            </Row>
                            <hr/>
                            <Row>
                                <Col xs={{size: 12}} sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                    <button className="btn-default button-primary-custom" onClick={submitRegister} disabled={!proceedSubmit}>
                                        <FontAwesomeIcon icon={faUserPlus}/>{' '}
                                        <Translate id="profile-register.submit-profile"/>
                                    </button>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </Container>
            </div>
            {loading ? <LoaderFullWindow/> : null}
            {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
            {showAddNewProfileModal ?
                <ModalSuccessSetupProfile visible={showAddNewProfileModal} spacesLeft={spacesLeft}
                                          updateParent={toggleAlertAddNewProfile} /> : null}
            {showModalAttachCaution ?
                <ModalAttachProfile visibleModalAttach={showModalAttachCaution}
                                    studentName={profileFullName}
                                    updateParent={toggleAttachProfile}
                /> : null
            }
        </div>
    );
};

// No `withLocalize` here as it's for class components.
// Ensure your localization setup is handled appropriately at a higher level or with hooks if available.
export default withLocalize(HomeschoolSubsSetProfileView);