/**
 * Created by portiz on 3/29/19.
 */
import React, { useState, useEffect } from 'react';
import { Redirect} from "react-router-dom";
import { Container, FormGroup, Col, Label, Row, Button} from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
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

/**
 * The component (window) to set the profile to subscription for an existing client
 */
const SubscriptionSetProfileExistingAccount = ({ location, updateParent }) => {
    const [calendarFocused, setCalendarFocused] = useState(false);
    const [createdAt, setCreatedAt] = useState(moment());
    const [didMount, setDidMount] = useState(false);
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
    const [propsModal, setPropsModal] = useState(null);
    const [propsModalAddNewProfile, setPropsModalAddNewProfile] = useState(null);
    const [redoSearch, setRedoSearch] = useState(false);
    const [roleId] = useState(Settings.ROLE_STUDENT_ID);
    const [sex, setSex] = useState('');
    const [showAddNewProfileModal, setShowAddNewProfileModal] = useState(false); // Shows the modal to add a new user right now
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalAttachCaution, setShowModalAttachCaution] = useState(false);
    const [spacesLeft, setSpacesLeft] = useState(false);
    const [subscriptionId] = useState(1);
    const [transactionSuccess, setTransactionSuccess] = useState(false);
    const [username, setUsername] = useState('');
    const [usernameAvailable, setUsernameAvailable] = useState(true);
    const [validFirstName, setValidFirstName] = useState(false);
    const [validLastName, setValidLastName] = useState(false);
    const [validPassword, setValidPassword] = useState(false);
    const [validUsername, setValidUsername] = useState(false);
    const [visibleModalAttach, setVisibleModalAttach] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [cameFromRegister, setCameFromRegister] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        setDidMount(true);
    }, []);

    const handleExistingAccountSelected = (e) => {
        setProfileId(e.value);
        setProfileFullName(e.GivenName + ' ' + e.Surname + ' (' + e.Username + ')');
        setShowModalAttachCaution(true);
    };

    const toggleAttachProfile = (e) => {
        setShowModalAttachCaution(!showModalAttachCaution);
        // If the user answered a yes to insert the existing account, then add it.
        if(e.answer) {
            subtmitSelectedStudent();
        }
    };

    // Toggles the alert modal, and according to the response, it sends it to the whatever it needs to be sent
    const toogleAlertAddNewProfile = (data) => {
        setShowAddNewProfileModal(false);

        if(transactionSuccess && !data.answer) {
            setGoToDownload(true);
            // Updates the parent (send it owns to state to update it)
            updateParent({goToDownload: true});
        }

        if(cameFromRegister && !data.answer && transactionSuccess) {
            setGoToDownload(true);
            // Updates the parent (send it owns to state to update it)
            updateParent({goToDownload: true});
        }
        else if (transactionSuccess && !cameFromRegister && !data.answer) {
            setGoToDashboard(true);
            updateParent({goToDashboard: true});
        }
    };

    const setSubmitButtonState = () => {
        setProceedSubmit(
            validFirstName &&
            validLastName &&
            validPassword &&
            validUsername &&
            usernameAvailable &&
            subscriptionId != null && subscriptionId.length > 0
        );
    };

    useEffect(() => {
        setSubmitButtonState();
    }, [validFirstName, validLastName, validPassword, validUsername, usernameAvailable, subscriptionId]);

    // Saves the username state and verifies if the next button is ready
    const saveUsername = (newValue) => {
        setUsername(newValue.username);
        setValidUsername(newValue.validUsername);
        setUsernameAvailable(newValue.usernameAvailable);
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

    // Submit register
    const submitRegister = () => {
        setLoading(true);
        // Submits the registration
        let apiCall = registerProfile(subscriptionId,
            username,
            password, passwordRetype,
            firstName, lastName,
            sex, dob, false, roleId);

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
                
                let modalProps = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(modalProps);
                setShowErrorModal(true);
                setRedoSearch(false);
            }
            else {
                let modalProps = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(modalProps);
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

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    // Submits the selected student to the profile
    const subtmitSelectedStudent = async () => {
        setLoading(true);
        try {
            const response = await insertSubscriptionToUser(profileId, subscriptionId, false, roleId);
            let modalProps = {
                bodyText: response.data.Messages[0],
                visible: true
            };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
        } catch (error) {
            let msg = "";
            if (error.response.data.Messages && error.response.data.Messages.length) {
                error.response.data.Messages.forEach((message) => {
                  msg += message + "<br/>";
                });
            }
            let modalProps = {
                bodyText: msg,
                visible: true
            };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
        }
    };

    const state = {
        username,
        validUsername,
        usernameAvailable,
        firstName,
        lastName,
        password,
        passwordRetype,
    }

    if (subscriptionId === null
        || typeof subscriptionId === 'undefined'
        || subscriptionId === '') {
        return (<Redirect to={{
            pathname: "/dashboard"
        }}/>);
    }

    return (
        <div>

            <div className="container-padding animated fadeIn delay-05s">
            <Container className="container-wrapper">
                <div>
                    <h2><Translate id="subscription-set-profile-existing-account.title"/></h2>
                    <div className="section-description mb-3">
                    <small><Translate id="subscription-set-profile-existing-account.existing-profile-instructions"/></small>
                    </div>
                </div>
                        {/* <Row>
                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                <b className="centered-text" style={{marginTop:40}}>
                                    <Translate id="subscription-set-profile-existing-account.title"/>
                                </b>
                            </Col>
                        </Row> */}

                <Row>
                            {/* <small><Translate id="subscription-set-profile-existing-account.existing-profile-instructions"/></small> */}
                            {/* <Col style={{border: 'solid lightgray', backgroundColor: 'whitesmoke'}} */}
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
                    <h2>Crear nuevo perfil</h2>
                        <small><Translate id="subscription-set-profile-existing-account.or-add-new-profile-instructions"/></small>
                    </Col>

                </Row>
                <Row>
                    <Col>
                        <Row className="margin-top-10">
                            <Col className="inputs-homeschooler" xs={{size:12}} md={{size: 6}}>
                                <FormGroupUsername data={state} updateParent={saveUsername}/>
                            </Col>
                            <Col className="inputs-homeschooler" xs={{size:12}} md={{size: 6}}>
                                <FormGroupNewPassword data={state} updateParent={savePassword}/>
                            </Col>
                            <Col className="inputs-homeschooler" xs={{size:12}} md={{size: 6}}>
                                <FormGroupName data={state} updateParent={saveName}/>
                            </Col>
                            <Col className="inputs-homeschooler" xs={{size:12}} md={{size: 6}}>
                                <FormGroup row>
                                    <Col sm={{size: 12}}>
                                        <Label for="sex">
                                            <Translate id="profile-register.sex"/>
                                        </Label>
                                        <SelectorSex data={{sex}} updateParent={saveSex}/>
                                    </Col>
                                </FormGroup>
                            </Col>
                                    {/*<Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <FormGroup row>
                                            <Col sm={{size: 12}}>
                                                <Label for="dob">
                                                    <Translate id="profile-register.dob"/>
                                                </Label>
                                                <DatePicker data={this.state} updateParent={this.saveDob()}/>
                                            </Col>
                                        </FormGroup>
                                    </Col>*/}
                        </Row>
                        <hr/>
                        <Row>
                            <Col xs={{size:12}} sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                <button type="submit" className="btn-default button-primary-custom" onClick={submitRegister}>
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
            <ModalSuccessSetupProfile visible={showAddNewProfileModal} spacesLeft={spacesLeft} updateParent={toogleAlertAddNewProfile} />  
        : null}
        {showModalAttachCaution ?
            <ModalAttachProfile visibleModalAttach={showModalAttachCaution} studentName={profileFullName} updateParent={toggleAttachProfile}/> 
        : null}

        </div>
    );
};

export default withLocalize(SubscriptionSetProfileExistingAccount);