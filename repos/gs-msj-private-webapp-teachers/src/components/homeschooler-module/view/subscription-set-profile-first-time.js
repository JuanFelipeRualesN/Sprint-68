/**
 * Created by portiz on 3/4/19.
 */
import React, { useState, useEffect } from 'react';
import { Redirect } from "react-router-dom";
import { Container, FormGroup, Col, Label, Row, Button} from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import FormGroupUsername from '../../shared-module-components/form/form-group-username';
import FormGroupName from '../../shared-module-components/form/form-group-name';
import {registerProfile} from '../../../api/users';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import FormGroupNewPassword from '../../shared-module-components/form/form-group-new-password';
import ModalSuccessSetupProfile from '../../shared-module-components/modal/modal-success-setup-profile-first-time';
import SelectorSex from '../../shared-module-components/selector/selector-sex';
import moment from 'moment';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import "./style.css";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to set the profile to subscription for the first time (new client)
 */
const SubscriptionAddNew = ({ location, initialize, addTranslation, updateParent }) => {
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
    const [propsModal, setPropsModal] = useState(null);
    const [propsModalAddNewProfile, setPropsModalAddNewProfile] = useState(null);
    const [sex, setSex] = useState('');
    const [showAddNewProfileModal, setShowAddNewProfileModal] = useState(false);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [spacesLeft, setSpacesLeft] = useState(false);
    const [subscriptionId] = useState(location.subscriptionId);
    const [transactionSuccess, setTransactionSuccess] = useState(false);
    const [username, setUsername] = useState('');
    const [usernameAvailable, setUsernameAvailable] = useState(true);
    const [validFirstName, setValidFirstName] = useState(false);
    const [validLastName, setValidLastName] = useState(false);
    const [validPassword, setValidPassword] = useState(false);
    const [validUsername, setValidUsername] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [cameFromRegister, setCameFromRegister] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        setDidMount(true);
    }, []);

    // Toggles the alert modal, and according to the response, it sends it to the whatever it needs to be sent
    const toogleAlertAddNewProfile = (data) => {
        setShowAddNewProfileModal(false);

        if(transactionSuccess && !data.answer) {
            setGoToDownload(true);
            // Updates the parent (send it owns to state to update it)
            updateParent({goToDownload: true});
        }

        // Updates the parent (send it owns to state to update it)
        if(cameFromRegister && !data.answer && transactionSuccess) {
            setGoToDownload(true);
            updateParent({goToDownload: true});
        }
        else if (transactionSuccess && !cameFromRegister && !data.answer) {
            setGoToDashboard(true);
            updateParent({goToDashboard: true});
        }
    };

    // Sets if submit button is ready or not
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
        // Submits the registration
        let apiCall = registerProfile(subscriptionId,
            username,
            password, passwordRetype,
            firstName, lastName,
            sex, dob, false);

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

                // If there is any space left in the subscription, show to add new modal
                if(response.data.SpacesInSubscriptionLeft) {
                    setShowAddNewProfileModal(true);
                    setSpacesLeft(true);
                    setLoading(false);
                } else {
                    setShowAddNewProfileModal(true);
                    setSpacesLeft(false);
                    setLoading(false);
                }
            } else {
                let modalProps = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(modalProps);
                setShowErrorModal(true);
            }
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

    const state = {
        username,
        validUsername,
        usernameAvailable,
        firstName,
        lastName,
        password,
        passwordRetype,
    }

    if(subscriptionId === null || typeof subscriptionId === 'undefined' || subscriptionId === '') {
        return (<Redirect to={{
            pathname: "/dashboard"
        }}/>);
    }

    return (
        <div>
            <div className="container-padding card-white">
                <Container className="container-wrapper">
                    <Row className="centered-text">
                        <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <h1 >
                                <Translate id="profile-register.setup-child-profile"/>
                            </h1>
                        </Col>
                    </Row>
                    <Row>
                        <Col style={{border: 'solid lightgray', backgroundColor: 'whitesmoke'}}
                             xs={{size:12}} sm={{size: 12}} md={{offset: 2, size: 8}} lg={{offset: 3, size: 6}}>
                            <Row className="margin-top-10">

                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                    <FormGroupUsername data={state} updateParent={saveUsername}/>
                                </Col>
                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                    <FormGroupNewPassword data={state} updateParent={savePassword}/>
                                </Col>
                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                    <FormGroupName data={state} updateParent={saveName}/>
                                </Col>
                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
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
                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                    <Button block color="success" className="font-now" onClick={submitRegister}>
                                        <h2><Translate id="profile-register.submit-profile"/></h2>
                                    </Button>
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

        </div>
    );
};

export default withLocalize(SubscriptionAddNew);