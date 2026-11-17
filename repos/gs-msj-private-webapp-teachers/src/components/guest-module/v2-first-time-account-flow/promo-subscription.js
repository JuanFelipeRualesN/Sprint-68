/**
 * Created by portiz on 2/28/19.
 */
import React, { useState, useEffect } from 'react';
import { FormGroup, Col, Alert, Row, Button, CardHeader, Card, CardBody} from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from '../../../custom-classes/settings';
import FromGroupEmail from '../../shared-module-components/form/form-group-email-v2';
import FormGroupAddress from '../form/form-group-address-v2';
import ModalChildrenPolicy from '../../shared-module-components/modal/modal-child-policy';
import ModalTerms from '../../shared-module-components/modal/modal-terms';
import NumericInput from 'react-numeric-input';
import {registerSubmitPaymentV2Form} from '../../../api/users';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import SessionManager from '../../../custom-classes/session-manager';
import LiveChat from 'react-livechat';
import SelectorPlan from "../selector/selector-plan";
import AlertS from 'react-s-alert';
import 'react-s-alert/dist/s-alert-default.css';
import 'react-s-alert/dist/s-alert-css-effects/bouncyflip.css';
import ModalReturnPolicy from '../../shared-module-components/modal/modal-return-policy';
import QueryString from 'query-string';
import ModalSuccessRegistrationV2 from "../modal/modal-success-registration-v2";
import FormGroupNameTutorV2 from "../form/form-group-name-tutor-v2";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to register and subscribe a client for the first time. It's a single page subscription
 */
const SinglePageSubscription = ({ initialize, addTranslation, location }) => {
    const parsedString = QueryString.parse(location.search);
    const studentQuantityTranslation = LanguageManager.getTranslation("subscription-steps.students-quantity", Settings.LANGUAGES);

    const [country, setCountry] = useState('');
    const [couponPlatformId, setCouponPlatformId] = useState(null);
    const [didMount, setDidMount] = useState(false);
    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [loading, setLoading] = useState(false);
    const [planAvailableForIso, setPlanAvailableForIso] = useState(true);
    const [planId, setPlanId] = useState(parsedString?.planId ?? null);
    const [previousAddressCountry, setPreviousAddressCountry] = useState('');
    const [previousAddressState, setPreviousAddressState] = useState('');
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [state, setState] = useState('');
    const [stripeApiKey] = useState(Settings.getStripeToken());
    const [studentsQuantity, setStudentsQuantity] = useState(null);
    const [subscriptionId, setSubscriptionId] = useState('');
    const [subTokenStripe, setSubTokenStripe] = useState(null);
    const [validCheckout, setValidCheckout] = useState(false);
    const [validCountry, setValidCountry] = useState(true);
    const [validFirstName, setValidFirstName] = useState(true);
    const [validLastName, setValidLastName] = useState(true);
    const [validEmail, setValidEmail] = useState(true);
    const [validState, setValidState] = useState(true);
    const [showModalReturnPolicy, setShowModalReturnPolicy] = useState(false);
    const [showModalTerms, setShowModalTerms] = useState(false);
    const [showModalChildrensPolicy, setShowModalChildrensPolicy] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const [emailAvailable, setEmailAvailable] = useState(true);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.add('landing-white'); // Custom background for students
        setDidMount(true);
        checkValidCheckout();
    }, []);

    const changeSelectedPlan = (e) => {
        setPlanId(e.value);
        checkValidCheckout();
    };

    // Handles when the how to add new subjects click is clicked
    // const howToAddNewSubjectsClick = (e) => {
    //     AlertS.info(<Translate id="registration-full-form.how-to-add-new-subject-explained"/>, {
    //         position: 'bottom-left',
    //         effect: 'bouncyflip',
    //         timeout: 5000
    //     });
    // };

    // Toggles the modal of return policy
    const toggleModalReturnPolicy = () => {
        setShowModalReturnPolicy(!showModalReturnPolicy);
    };

    // Toggles the success modal
    const toggleSuccessModal = () => {
        setShowModalSuccess(false);
    };

    // Toogles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    // Submits the registration
    const submitRegistration = () => {
        setLoading(true);

        if (validCheckout) {
            let apiCall = registerSubmitPaymentV2Form(
                state,
                state,
                country,
                country,
                planId,
                subTokenStripe,
                studentsQuantity,
                email,
                couponPlatformId,
                firstName,
                lastName,
                true);

            apiCall.then((response) => {
                if (response.status !== 200 && response.status !== 201) {
                    let modalProps = {
                        bodyText: response.data.Messages[0],
                        visible: true
                    };
                    setPropsModal(modalProps);
                    setShowErrorModal(true);
                    setLoading(false);
                } else {
                    SessionManager.setSession(response.data);
                    setSubscriptionId(response.data.Subscriptions[0].SubscriptionId);
                    setShowModalSuccess(true);
                    setLoading(false);
                }
            });
        } else {
            setLoading(false);
        }
    };

    // Save the email state and verifies  if the next button is ready
    const saveEmail = (newValue) => {
        setEmail(newValue.email);
        setValidEmail(newValue.validEmail);
        setEmailAvailable(newValue.emailAvailable);
        checkValidCheckout();
    };

    // Saves the address details and verifies if the next button is ready
    const saveAddress = (newValue) => {
        setCountry(newValue.country);
        setState(newValue.state);
        setValidCountry(newValue.validCountry);
        setValidState(newValue.validState);

        if (previousAddressState !== newValue.state) {
            setPreviousAddressState(newValue.state);
        } else {
            checkValidCheckout();
        }

        if (previousAddressCountry !== newValue.country) {
            setPreviousAddressCountry(newValue.country);
        } else {
            checkValidCheckout();
        }
    };

    // Action when the selection of quantity of student changes
    const handleQuantityProfilesChange = (value) => {
        setStudentsQuantity(value);
        checkValidCheckout()
    };

    // Toggle the terms modal
    const toggleModalTerms = () => {
        setShowModalTerms(!showModalTerms);
    };
    
    // Toggle de modal children policy
    const toggleModalChildrenPolicy = () => {
        setShowModalChildrensPolicy(!showModalChildrensPolicy);
    };

    const setCouponCode = (e) => {
        setCouponPlatformId(e);
        checkValidCheckout();
    };

    const saveName = (newValue) => {
        setValidFirstName(newValue.validFirstName);
        setValidLastName(newValue.validLastName);
        setFirstName(newValue.firstName);
        setLastName(newValue.lastName);
        checkValidCheckout();
    };

    const checkValidCheckout = () => {
        if (typeof studentsQuantity === 'undefined'
            || studentsQuantity === null
            || studentsQuantity <= 0
            || !validCountry
            || typeof country === 'undefined'
            || country === ''
            || country === null
            || !validState
            || !validEmail
            || typeof email === 'undefined'
            || email === ''
            || email === null
            || typeof planId === 'undefined'
            || planId === null
            || firstName === ''
            || lastName === ''
            || !validFirstName
            || !validLastName) {
            setValidCheckout(false);
        } else {
            setValidCheckout(true);
        }
    };

    return (
        <div>
            <Row>
                <Col xs={{size:12}} sm={{size: 12}} md={{size: 8, offset: 2}} lg={{size: 8, offset: 2}}>
                    <div className="margin-20-percent">
                        <Card className="card-white">
                            <CardHeader><h2><Translate id="promo-subscription.subscription" options={{renderInnerHtml: true}}/></h2></CardHeader>
                            <CardBody>

                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 10,  offset: 1}} lg={{size: 8, offset: 2}}>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}> <h4>
                                            <Translate id="promo-subscription.subscription-details" options={{renderInnerHtml: true}}/></h4>
                                            <hr/>
                                        </Col>
                                    </Row>

                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}>
                                            <SelectorPlan callPromo={true} updateParent={changeSelectedPlan}/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}>
                                            <NumericInput
                                                onChange={handleQuantityProfilesChange}
                                                placeholder={studentQuantityTranslation}
                                                className="form-control" strict={true}
                                                max={5} mobile={'auto'} />
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}
                                             style={{marginTop:20}}> <h4>
                                            <Translate id="promo-subscription.tutor-information" options={{renderInnerHtml: true}}/>
                                        </h4>
                                            <hr/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:12}}>
                                            <FormGroupNameTutorV2 updateParent={saveName}/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}>
                                            <FromGroupEmail data={{email, validEmail, emailAvailable}} updateParent={saveEmail}/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            <FormGroupAddress data={{country, state, validCountry, validState, previousAddressCountry, previousAddressState}} updateParent={saveAddress}/>
                                        </Col>
                                    </Row>
                                    <hr/>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            {!planAvailableForIso ?
                                                <FormGroup row>
                                                    <Col sm={{ size: 10, offset: 1 }}>
                                                        <Alert color="danger" isOpen={!planAvailableForIso}>
                                                            <Translate id="plan-calculator-form.plan-not-available-for-your-country"/>
                                                        </Alert>
                                                    </Col>
                                                </FormGroup> : null
                                            }
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:7}} sm={{size: 7}} md={{size: 5}} lg={{size: 5}}>
                                            <Button disabled={!validCheckout} block color="success" className="font-now" onClick={submitRegistration}>
                                                <Translate id="promo-subscription.submit"/>
                                            </Button>
                                        </Col>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 7}} lg={{size: 7}}>
                                            {
                                                !validCheckout ?
                                                    <Alert color="info" isOpen={planAvailableForIso}>
                                                        <Translate id="register.check-fields"/>
                                                    </Alert> : null
                                            }
                                        </Col>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            <small>
                                                <Translate id="registration-full-form.by-submitting"/>
                                                <ModalChildrenPolicy isOpen={showModalChildrensPolicy} toggle={toggleModalChildrenPolicy}/>
                                                <ModalTerms isOpen={showModalTerms} toggle={toggleModalTerms} color="link"/>
                                                <ModalReturnPolicy isOpen={showModalTerms} toggle={toggleModalTerms}/>
                                            </small>
                                            <hr/>

                                            <br/>
                                        </Col>
                                    </Row>
                                    <LiveChat license={Settings.CHAT_LIVE_CHAT_LICENSE} />
                                </Col>
                            </CardBody>
                        </Card>
                    </div>
                </Col>
            </Row>
            {loading ? <LoaderFullWindow/> : null}
            {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
            {showModalSuccess ?
                <ModalSuccessRegistrationV2
                    isEdit={false}
                    userFullName={firstName + ' ' + lastName}
                    visible={showModalSuccess}
                    subscriptionId={subscriptionId}
                    orderNo={"ORD" + subscriptionId}
                    updateParent={toggleSuccessModal}/> : null}

        </div>
    );
};

export default withLocalize(SinglePageSubscription);