/**
 * Created by portiz on 4/27/20.
 */
import React, { useState, useEffect } from 'react';
import {Container, FormGroup, Col, Label, Alert, Row, Button, Input} from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize, Translate } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
/** End of Multiple Language imports **/
import Settings from '../../../custom-classes/settings';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import FormGroupPassword from '../../shared-module-components/form/form-group-new-password';
import FromGroupEmail from '../../shared-module-components/form/form-group-email';
import FormGroupAddress from '../../shared-module-components/form/form-group-address';
import FormGroupName from '../../shared-module-components/form/form-group-name';
import FormGroupPhone from '../../shared-module-components/form/form-group-phone';
import ModalChildrenPolicy from '../../shared-module-components/modal/modal-child-policy';
import ModalTerms from '../../shared-module-components/modal/modal-terms';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import ModalSuccessRegistrationFree from '../modal/modal-success-registration-free';
import SessionManager from '../../../custom-classes/session-manager';
import LiveChat from 'react-livechat';
import 'react-s-alert/dist/s-alert-default.css';
import 'react-s-alert/dist/s-alert-css-effects/bouncyflip.css';
import ModalReturnPolicy from '../../shared-module-components/modal/modal-return-policy';
import {faUser, faSchool} from '@fortawesome/free-solid-svg-icons';
import FormGroupPublicPrivate from "../../school-module/form/form-public-private";
import SelectorSchool from "../../school-module/selector/selector-school";
import queryString from 'query-string';
import {registerFreeUser} from "../../../api/users";
import './style.css';
import teachersImage from '../../../assets/images/common-icons/Register-icon-Teachers.png';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to register an user for free.
 */
const RegisterFree = ({location, addTranslation, initialize}) => {
    const [chatLiveChatLicense] = useState(Settings.CHAT_LIVE_CHAT_LICENSE);
    const [city, setCity] = useState('');
    const [country, setCountry] = useState('');
    const [email, setEmail] = useState('');
    const [emailAvailable, setEmailAvailable] = useState(true);
    const [firstName, setFirstName] = useState('');
    const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());
    const [isParent, setIsParent] = useState(false);
    const [lastName, setLastName] = useState('');
    const [loading, setLoading] = useState(false);
    const [password, setPassword] = useState('');
    const [passwordRetype, setPasswordRetype] = useState('');
    const [phone, setPhone] = useState('');
    const [previousAddressCountry, setPreviousAddressCountry] = useState('');
    const [previousAddressState, setPreviousAddressState] = useState('');
    const [privateOrPublic, setPrivateOrPublic] = useState(1);
    const [redoSchoolSearch, setRedoSchoolSearch] = useState(false);
    const [schoolId, setSchoolId] = useState('');
    const [schoolRecordIdentifier, setSchoolRecordIdentifier] = useState('');
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [showModalReturnPolicy, setShowModalReturnPolicy] = useState(false);
    const [showModalTerms, setShowModalTerms] = useState(false);
    const [showModalChildrensPolicy, setShowModalChildrensPolicy] = useState(false);
    const [state, setState] = useState('');
    const [tokenInvitation, setTokenInvitation] = useState(null);
    const [validCity, setValidCity] = useState(true);
    const [validCountry, setValidCountry] = useState(true);
    const [validEmail, setValidEmail] = useState(true);
    const [validFirstName, setValidFirstName] = useState(true);
    const [validLastName, setValidLastName] = useState(true);
    const [validPassword, setValidPassword] = useState(true);
    const [validPhone, setValidPhone] = useState(true);
    const [validPrivateOrPublic, setValidPrivateOrPublic] = useState(false);
    const [validSchool, setValidSchool] = useState(false);
    const [validState, setValidState] = useState(true);
    const [propsModal, setPropsModal] = useState(null);
    const [zipCode, setZipCode] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.add('background-alt');
        const values = queryString.parse(location.search);
        setIsParent(values.isParent);
        setTokenInvitation(values.tokenInvitation);

        return () => {
            document.body.classList.remove('background-alt');
        };
    }, []);

    const handleEntityPrivateOrPublic = (e, valid) => {
        setPrivateOrPublic(e);
        setValidPrivateOrPublic(valid);
    };

    const handleIdentifierRecord = (e) => {
        setSchoolRecordIdentifier(e.currentTarget.value);
    };

    // Toggles the modal of return policy
    const toggleModalReturnPolicy = () => {
        setShowModalReturnPolicy(!showModalReturnPolicy);
    };

    // Toggles the success modal
    const toggleSuccessModal = () => {
        setShowModalSuccess(false);
    };

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    // Submits the registration
    const submitRegistration = () => {
        setLoading(true);

        // Saves the account
        let apiCall = registerFreeUser(firstName, lastName, phone, "", "", city, state, state, country, zipCode, country,
                                        email, password, passwordRetype, schoolId, schoolRecordIdentifier, tokenInvitation, !isParent);
        apiCall.then((response) => {
            if (response.status !== 201 && response.status !== 200) {
                let propsModal = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(propsModal);
                setShowErrorModal(true);
                setLoading(false);
            } else {
                // If Token is not null, set token (Token)
                // Set success messages
                SessionManager.setSession(response.data);
                setShowModalSuccess(true);
                setLoading(false);
            }
        });
    };

    // Save the password state and verifies if the next button is ready
    const savePassword = (newValue) => {
        setPassword(newValue.password);
        setPasswordRetype(newValue.passwordRetype);
        setValidPassword(newValue.validPassword);
    };

    // Save the email state and verifies  if the next button is ready
    const saveEmail =  (newValue) => {
        setEmail(newValue.email);
        setValidEmail(newValue.validEmail);
        setEmailAvailable(newValue.emailAvailable);
    };

    // Save the name details and verifies if the next button is ready
    const saveName = (newValue) => {
        setFirstName(newValue.firstName);
        setLastName(newValue.lastName);
        setValidFirstName(newValue.validFirstName);
        setValidLastName(newValue.validLastName);
    };

    // Save the phone details and verifies if the next button is ready
    const savePhone = (newValue) => {
        setPhone(newValue.phone);
        setValidPhone(newValue.validPhone);
    };

    const saveSchool = (newValue) => {
        setRedoSchoolSearch(false);
        setValidSchool(true);
        setSchoolId(newValue);
    };

    // Saves the address details and verifies if the next button is ready
    const saveAddress = (newValue) => {
        setRedoSchoolSearch(false);
        setCity(newValue.city);
        setCountry(newValue.country);
        setSchoolId('');
        setState(newValue.state);
        setValidCity(newValue.validCity);
        setValidCountry(newValue.validCountry);
        setValidPhone(newValue.validPhone);
        setValidSchool(false);
        setValidState(newValue.validState);

        if(previousAddressState !== newValue.state)
        {
            setPreviousAddressState(newValue.state);
        }
        if(previousAddressCountry !== newValue.country){
            setPreviousAddressCountry(newValue.country);
        }
        setRedoSchoolSearch(true);
    };

    // Toggle the terms modal
    const toggleModalTerms = () => {
        setShowModalTerms(!showModalTerms);
    };

    // Toggle modal children policy
    const toggleModalChildrenPolicy = () => {
        setShowModalChildrensPolicy(!showModalChildrensPolicy);
    };

    // Renders the landing page
    return (
            <div>
                <div className="container-padding animated delay-05s fadeIn">
                    <Container className="container-full">
                        <Row className="centered-text">
                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                <h1 >
                                    {isParent ?
                                        <Translate id="register-parent.genial-web-for-parents"
                                                   options={{renderInnerHtml: true}}/>
                                        :
                                        <Translate id="register-teacher.genial-web-for-teachers"
                                                   options={{renderInnerHtml: true}}/>
                                    }
                                </h1>
                                <hr/>
                                <small>
                                    {isParent ?
                                        <>
                                            <Translate id="register-parent.notes" options={{renderInnerHtml: true}}/>{': '}
                                            <Translate id="register-parent.note-explanation" options={{renderInnerHtml: true}}/>
                                        </>
                                        : <>
                                            <Translate id="register-teacher.notes" options={{renderInnerHtml: true}}/>{': '}
                                            <Translate id="register-teacher.note-explanation" options={{renderInnerHtml: true}}/>
                                        </>
                                    }
                                </small>
                            </Col>
                        </Row>
                        <Row>
                            <Col xs={{size:12}} sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>

                                <Row>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}} className="centered-text">
                                        <img src={teachersImage} alt="Teachers"
                                             className="animated delay-2s fadeIn"/>
                                    </Col>
                                </Row>
                            </Col>
                            <Col className="animated delay-05s fadeIn" style={{border: 'solid lightgray 1px', borderRadius: '10px', backgroundColor: 'whitesmoke', padding: '20px'}} xs={{size:12}} sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                <Row>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <h3>
                                            <FontAwesomeIcon className="fa-1x" icon={faUser}/>{' '}
                                            {isParent ?
                                                <Translate id="register-parent.create-account"
                                                           options={{renderInnerHtml: true}}/> :
                                                <Translate id="register-teacher.create-account"
                                                           options={{renderInnerHtml: true}}/>
                                            }
                                        </h3>
                                        <hr/>
                                    </Col>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <FormGroupName data={{firstName, lastName}} updateParent={saveName}/>
                                    </Col>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <FromGroupEmail data={{email, emailAvailable}} updateParent={saveEmail}/>
                                    </Col>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <FormGroupPassword data={{password, passwordRetype}} updateParent={savePassword}/>
                                    </Col>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <FormGroupPhone data={{phone}} updateParent={savePhone}/>
                                    </Col>
                                    {!isParent ?
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 10}} lg={{size: 10}}>
                                        <FormGroup row>
                                            <Col sm={{size: 12}}>
                                                <Label for="school-identifier-record">
                                                    <Translate id="register-teacher.record-no"/>
                                                </Label>
                                                <Input value={schoolRecordIdentifier}
                                                       type="text" name="school-identifier-record"
                                                       id="school-identifier-record" onChange={handleIdentifierRecord}/>
                                            </Col>
                                        </FormGroup>
                                    </Col> : null}
                                    {!isParent ?
                                        <>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <h4>
                                            <FontAwesomeIcon className="fa-1x" icon={faSchool}/>{' '}
                                            <Translate id="register-teacher.school-details" options={{renderInnerHtml: true}}/>
                                        </h4>
                                        <hr/>
                                        </Col>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            <FormGroupPublicPrivate isRegisterForm={true} privateOrPublic={privateOrPublic} updateParent={handleEntityPrivateOrPublic}/>
                                        </Col>
                                        </>
                                        : null}
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <FormGroupAddress showAddressLines={false} showAddressZipCode={false} data={{city, country, state, zipCode}} updateParent={saveAddress}/>
                                    </Col>

                                    {city !== null && city !== '' && country !== null && country !== ''
                                    && validPrivateOrPublic ?
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            <SelectorSchool
                                                redoSearch={redoSchoolSearch}
                                                city={city} country={country}
                                                            privateOrPublic={privateOrPublic}
                                                            state={state}
                                                            updateParent={saveSchool}/>
                                        </Col> : null
                                    }

                                </Row>
                                <hr/>
                                <Row>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                        <small>
                                            <Translate id="registration-full-form.by-submitting"/>
                                            <ModalChildrenPolicy isOpen={showModalChildrensPolicy} toggle={toggleModalChildrenPolicy}/>
                                            <ModalTerms isOpen={showModalTerms} toggle={toggleModalTerms} color="link"/>
                                            <ModalReturnPolicy isOpen={showModalTerms} toggle={toggleModalTerms}/>
                                        </small>
                                        <hr/>
                                        {

                                             !validPassword
                                            || typeof password === 'undefined'
                                            || password === ''
                                            || password === null
                                            || typeof passwordRetype === 'undefined'
                                            || passwordRetype === ''
                                            || passwordRetype === null

                                            || !validCity
                                            || typeof city === 'undefined'
                                            || city === ''
                                            || city === null

                                            || !validCountry
                                            || typeof country === 'undefined'
                                            || country === ''
                                            || country === null

                                            || !validState

                                            || !validEmail
                                            || typeof email === 'undefined'
                                            || email === ''
                                            || email === null


                                            || !validPhone
                                            || typeof phone === 'undefined'
                                            || phone === ''
                                            || phone === null


                                            || !validFirstName
                                            || typeof firstName === 'undefined'
                                            || firstName === ''
                                            || firstName === null

                                            || !validLastName
                                            || typeof lastName === 'undefined'
                                            || lastName === ''
                                            || lastName === null
                                                 &&
                                             (
                                                 !isParent
                                                && (!validSchool
                                             || !validPrivateOrPublic
                                             || schoolRecordIdentifier === ''
                                             ))
                                                 ?
                                                <Alert color="danger" >
                                                    <Translate id="register.check-fields"/>
                                                </Alert> : null
                                        }
                                        <br/>
                                        <Button block color="success" className="font-now" disabled={
                                            !validPassword
                                            || typeof password === 'undefined'
                                            || password === ''
                                            || password === null
                                            || typeof passwordRetype === 'undefined'
                                            || passwordRetype === ''
                                            || passwordRetype === null

                                            || !validCity
                                            || typeof city === 'undefined'
                                            || city === ''
                                            || city === null

                                            || !validCountry
                                            || typeof country === 'undefined'
                                            || country === ''
                                            || country === null

                                            || !validState

                                            || !validEmail
                                            || typeof email === 'undefined'
                                            || email === ''
                                            || email === null


                                            || !validPhone
                                            || typeof phone === 'undefined'
                                            || phone === ''
                                            || phone === null


                                            || !validFirstName
                                            || typeof firstName === 'undefined'
                                            || firstName === ''
                                            || firstName === null

                                            || !validLastName
                                            || typeof lastName === 'undefined'
                                            || lastName === ''
                                            || lastName === null
                                            &&
                                            (
                                                !isParent
                                                && (!validSchool
                                                    || !validPrivateOrPublic
                                                    || schoolRecordIdentifier === ''
                                                ))
                                        } onClick={submitRegistration}>
                                            <h2>
                                                <Translate id="register-teacher.register-yourself"/>
                                            </h2>
                                        </Button>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>

                        {!isMicrosoftTeams ?
                            <LiveChat license={chatLiveChatLicense} />
                            : null
                        }
                    </Container>

                </div>
                {loading ? <LoaderFullWindow/> : null}
                {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
                {showModalSuccess ?
                    <ModalSuccessRegistrationFree isParent={isParent} visible={showModalSuccess} updateParent={toggleSuccessModal}/> : null}

            </div>
        )
};

export default withLocalize(RegisterFree);