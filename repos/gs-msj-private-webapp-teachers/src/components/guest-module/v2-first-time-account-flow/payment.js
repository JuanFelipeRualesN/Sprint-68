import React, { useState, useEffect } from 'react';
import {FormGroup, Col, Alert, Row, Button, CardHeader, Card, CardBody, Label} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import {CardElement, useElements, useStripe} from '@stripe/react-stripe-js';
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from '../../../custom-classes/settings';
import FromGroupEmail from '../../shared-module-components/form/form-group-email-v2';
import FormGroupAddress from '../form/form-group-address-v2';
import ModalChildrenPolicy from '../../shared-module-components/modal/modal-child-policy';
import ModalTerms from '../../shared-module-components/modal/modal-terms';
import NumericInput from 'react-numeric-input';
import {registerSubmitPaymentV2Form} from '../../../api/users';
import {getPlanTotalCosts} from '../../../api/subscriptions';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import SessionManager from '../../../custom-classes/session-manager';
import FormGroupCoupon from '../form/form-coupon-v2';
import CalculationBox from '../calculation-box/calculation-box-v2';
import LiveChat from 'react-livechat';
import SelectorPlan from "../selector/selector-plan";
import 'react-s-alert/dist/s-alert-default.css';
import 'react-s-alert/dist/s-alert-css-effects/bouncyflip.css';
import ModalReturnPolicy from '../../shared-module-components/modal/modal-return-policy';
import QueryString from 'query-string';
import ModalSuccessRegistrationV2 from "../modal/modal-success-registration-v2";
import FormGroupNameTutorV2 from "../form/form-group-name-tutor-v2";
import './style.css';
import {useHistory} from "react-router-dom";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import usePaymentCheckoutTracking from '../../../hooks/usePaymentCheckoutTracking';

/**
 * The component (window) to register and subscribe a client for the first time. It's a single page subscription
 */
const SinglePageSubscription = ({ location }) => {
    const parsedString = QueryString.parse(location.search);

    const studentQuantityTranslation = LanguageManager.getTranslation("subscription-steps.students-quantity", Settings.LANGUAGES);
    const [country, setCountry] = useState('PRI');
    const [couponPlatformId, setCouponPlatformId] = useState(parsedString?.coupon ?? null);
    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [loading, setLoading] = useState(false);
    const [planAvailableForIso, setPlanAvailableForIso] = useState(true);
    const [planCostDetails, setPlanCostDetails] = useState(false);
    const [planId, setPlanId] = useState(parsedString?.planId ?? null);
    const [planName, setPlanName] = useState('');
    const planIdFromUrl = !!parsedString?.planId;
    const [previousAddressCountry, setPreviousAddressCountry] = useState('');
    const [previousAddressState, setPreviousAddressState] = useState('');
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [state, setState] = useState('');
    const [studentQuantityTranslationState] = useState(studentQuantityTranslation);
    const [studentsQuantity, setStudentsQuantity] = useState(1);
    const [subscriptionId, setSubscriptionId] = useState('');
    const [validCalculation, setValidCalculation] = useState(false);
    const [validCheckout, setValidCheckout] = useState(false);
    const [validCountry, setValidCountry] = useState(true);
    const [validFirstName, setValidFirstName] = useState(false);
    const [validLastName, setValidLastName] = useState(false);
    const [validEmail, setValidEmail] = useState(false);
    const [validState, setValidState] = useState(true);
    const [emailAvailable, setEmailAvailable] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const [showModalReturnPolicy, setShowModalReturnPolicy] = useState(false);
    const [showModalTerms, setShowModalTerms] = useState(false);
    const [showModalChildrensPolicy, setShowModalChildrensPolicy] = useState(false);
    const [cardComplete, setCardComplete] = useState(false);
    const [stripeErrorMessage, setStripeErrorMessage] = useState(null);
    const [planInterval, setPlanInterval] = useState(parsedString?.term ?? null);
    const sessionDetails = SessionManager.getSessionDetails();
    const stripe = useStripe();
    const elements = useElements();
    const navigate = useHistory();

    // Initialize translations based on the current version
    useVersionedTranslations();

    if(!Settings.getCheckoutStatus()){
        // Redirect to login
        navigate.push('/dashboard');
    }

    usePaymentCheckoutTracking({
        planId,
        planName,
        studentsQuantity,
        planInterval,
        validFirstName,
        validLastName,
        validEmail,
        emailAvailable,
        validCountry,
        validState,
        validCalculation,
        firstName,
        lastName,
        email,
        country,
        state
    });

    useEffect(() => {
        if (typeof sessionDetails !== 'undefined' && sessionDetails !== null && sessionDetails.Roles !== null) {
            // If role is not a client, redirect to the home page
            if (sessionDetails.Roles[0].toLowerCase() !== Settings.ROLE_CLIENT_STR.toLowerCase()) {
                navigate.push('/dashboard');
            } else {
                // Build query string
                const params = new URLSearchParams();
                if (planId) params.append('planId', planId);
                if (couponPlatformId) params.append('coupon', couponPlatformId);
                if (planInterval) params.append('interval', planInterval);

                navigate.push({
                    pathname: '/subscription-add-new',
                    search: `?${params.toString()}`
                });
            }
        }

        document.body.classList.add('landing-white'); // Custom background for students
        calculateCosts(planId, studentsQuantity, country, state, couponPlatformId);
    }, []);


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

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    // Submits the registration
    const submitRegistration = async () => {
        setLoading(true);
        setStripeErrorMessage(null);
        setShowErrorModal(false);
        setPropsModal(null);

        if (planId === null || planId === "0" || !validCalculation || studentsQuantity === 0) {
            let modalProps = {
                bodyText: LanguageManager.getTranslation("guest.form-incomplete-error"), // Example generic error message
                visible: true
            };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
            return; // Stop if form data is incomplete
        }

        if (!stripe || !elements) {
            // Stripe.js has not yet loaded.
            const errorMessage = LanguageManager.getTranslation("commercial.stripe-not-loaded");
            setStripeErrorMessage(errorMessage);
            let modalProps = { bodyText: errorMessage, visible: true };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
            return;
        }

        const cardElement = elements.getElement(CardElement);

        if (!cardComplete || !cardElement) {
            const errorMessage = LanguageManager.getTranslation("commercial.complete-card-details");
            setStripeErrorMessage(errorMessage);
            let modalProps = { bodyText: errorMessage, visible: true };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
            return;
        }

        try {

            const { paymentMethod, error: stripeError } = await stripe.createPaymentMethod({
                type: 'card',
                card: cardElement,
                 billing_details: { // Optionally add billing details if collected
                     name: `${firstName} ${lastName}`,
                     email: email
                 }
                //     address: {
                //         country: country,
                //         state: state,
                //         // Add other address fields if available in your form
                //     },
                // },
            });

            if (stripeError) {
                console.error('[Stripe createPaymentMethod error]', stripeError);
                setStripeErrorMessage(stripeError.message);
                let modalProps = { bodyText: stripeError.message, visible: true };
                setPropsModal(modalProps);
                setShowErrorModal(true);
                setLoading(false);
                return;
            }

            let response = await registerSubmitPaymentV2Form( // Use await here
                state,
                state, // Assuming 'state' is used for both state and province as per your original code
                country,
                country, // Assuming 'country' is used for both country and region
                planId,
                paymentMethod.id,
                studentsQuantity,
                email,
                couponPlatformId,
                firstName,
                lastName,
                false
            );
            if (response.status === 201) {
                // Handle successful API response
                SessionManager.setSession(response.data);
                setSubscriptionId(response.data.RecentSubscriptionId);
                setShowModalSuccess(true);
                setValidCheckout(true);
            }
            else{
                // Handle error response
                // Get the Messages from the response
                let errorMessages = response.data.Messages || [];
                // Set the error messages a string to display in the modal
                let bodyText = errorMessages.length > 0 ? errorMessages.join(', ')
                    : LanguageManager.getTranslation("commercial.denied-payment");

                let modalProps = {
                    bodyText: bodyText,
                    visible: true
                };
                setPropsModal(modalProps);
                setShowErrorModal(true);
            }
        } catch (error) {
            // Catch any unexpected errors during the API call (e.g., network issues, unhandled rejections)
            console.error("API call failed:", error);
        } finally {
            // This block will always execute, whether the try block finishes successfully or a catch block handles an error
            setLoading(false);
        }
    };

    // Save the email state and verifies  if the next button is ready
    const saveEmail = (newValue) => {
        setEmail(newValue.email);
        setValidEmail(newValue.validEmail);
        setEmailAvailable(newValue.emailAvailable);
    };

    // Saves the address details and verifies if the next button is ready
    const saveAddress = (newValue) => {
        setCountry(newValue.country);
        setState(newValue.state);
        setValidCountry(newValue.validCountry);
        setValidState(newValue.validState);
        
        if(previousAddressState !== newValue.state) {
            setPreviousAddressState(newValue.state);
            calculateCosts(planId, studentsQuantity, newValue.country, newValue.state, couponPlatformId);
        }
        
        if(previousAddressCountry !== newValue.country) {
            setPreviousAddressCountry(newValue.country);
            calculateCosts(planId, studentsQuantity, newValue.country, newValue.state, couponPlatformId);
        }
    };

    // Render the total details
    const renderTotals = (costData) => {
        return(
            <CalculationBox costData={{costData}}/>
        );
    };

    const calculateCosts = (selectedPlanId, selectedStudentQuantity, selectedCountryIso, selectedStateIso,
                            couponEntered) => {

        if(selectedStudentQuantity > 0 && selectedPlanId !== null && selectedCountryIso !== null && selectedCountryIso !== '') {
            let apiCall = getPlanTotalCosts(
                selectedPlanId,
                selectedStudentQuantity,
                couponEntered,
                selectedStateIso,
                selectedCountryIso);
            apiCall.then((response) => {
                if(response.data !== null) {
                    setValidCalculation(true);
                } else {
                    setValidCalculation(false);
                }
                setPlanCostDetails(renderTotals(response.data));
            });
        } else {
            setPlanCostDetails(null);
            setValidCalculation(false);
        }
    };

    // Action when the selection of quantity of student changes
    const handleQuantityProfilesChange = (value) => {
        setStudentsQuantity(value);
        calculateCosts(planId, value, country, state, couponPlatformId);
    };

    // Toggle the terms modal
    const toggleModalTerms = () => {
        setShowModalTerms(!showModalTerms);
    };

    // Toggle de modal children policy
    const toggleModalChildrenPolicy = () => {
        setShowModalChildrensPolicy(!showModalChildrensPolicy);
    };

    const setCouponCodeHandler = (e) => {
        setCouponPlatformId(e);
        calculateCosts(planId, studentsQuantity, country, state, e);
    };

    const saveName = (newValue) => {
        setValidFirstName(newValue.validFirstName);
        setValidLastName(newValue.validLastName);
        setFirstName(newValue.firstName);
        setLastName(newValue.lastName);
    };

    // Set the stripe payment token (CardElement onChange handler)
    const retrieveSubTokenStripe = (event) => {
        setCardComplete(event.complete);
        if (event.error) {
            setStripeErrorMessage(event.error.message);
        } else {
            setStripeErrorMessage(null);
        }
    };

    return (
        <div>
            <Row>
                <Col xs={{size:12}} sm={{size: 12}} md={{size: 8, offset: 2}} lg={{size: 8, offset: 2}}>
                    <div style={{marginTop: '120px'}}>
                        <Card className="card-checkout">
                            <CardHeader><h5 className="label-forms-blue"><Translate id="register.payment" options={{renderInnerHtml: true}}/></h5></CardHeader>
                            <CardBody>
                                <Col xs={{size:12}} sm={{size: 12}} md={{size: 10,  offset: 1}} lg={{size: 8, offset: 2}}>
                                    <Row>
                                        <Col xs={12} sm={6}>
                                            <FormGroup>
                                            <Label for="plan" className="label-forms-blue"> <Translate id="guest.plan" options={{renderInnerHtml: true}}/></Label>
                                            <SelectorPlan
                                                disabled={planIdFromUrl}
                                                isCommercial={true} selectedPlan={planId} callPromo={false} updateParent={(selectedPlan) => {
                                                setPlanId(selectedPlan.value);
                                                setPlanName(selectedPlan.label);
                                                setPlanInterval(selectedPlan.interval);
                                                calculateCosts(selectedPlan.value, studentsQuantity, country, state, couponPlatformId);
                                            }} />
                                            </FormGroup>
                                        </Col>
                                        <Col xs={12} sm={6}>
                                            <FormGroup>
                                                <Label for="quantity" className="label-forms-blue">
                                                    <Translate id="guest.students-quantity" options={{renderInnerHtml: true}}/>
                                                </Label>
                                                <br/>
                                                <NumericInput
                                                    value={studentsQuantity}
                                                    onChange={handleQuantityProfilesChange}
                                                    placeholder={studentQuantityTranslationState}
                                                    className="mantine-like__control" strict={true}
                                                    max={5} min={1} mobile={'auto'} />
                                            </FormGroup>
                                        </Col>

                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}
                                             style={{marginTop:20}}>
                                            <span className="label-forms-blue">
                                                <Translate id="payment.basic-info" options={{renderInnerHtml: true}}/>
                                            </span>
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
                                            <FromGroupEmail data={{email, emailAvailable}} updateParent={saveEmail}/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            <FormGroupAddress 
                                                data={{
                                                    country,
                                                    state,
                                                    validCountry,
                                                    validState
                                                }}
                                                updateParent={saveAddress}/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}
                                             style={{marginTop:20}}>
                                            <span  className="label-forms-blue">
                                            <Translate id="payment.payment" options={{renderInnerHtml: true}}/>
                                           </span>
                                            <hr/>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}>
                                            {planInterval !== null ?
                                            <FormGroupCoupon interval={planInterval} couponPlatformId={couponPlatformId} updateParent={setCouponCodeHandler}/>
                                                : null
                                            }
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={{size:12}} sm={{size:12}} md={{size:12}} lg={{size:12}}>
                                            {/* Elements provider is here, CardElement inside */}
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
                                            {stripeErrorMessage && <div style={{ color: 'red', marginTop: '10px' }}>{stripeErrorMessage}</div>}

                                        </Col>
                                    </Row>
                                    <hr/>
                                    <Row>
                                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            {planCostDetails !== null ? planCostDetails : null}
                                        </Col>
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
                                            <Button block color="success" className="font-now"
                                                    onClick={submitRegistration} disabled={planId == null ||
                                            studentsQuantity == null || studentsQuantity <= 0 ||
                                            !validFirstName || !validLastName || !validCountry || !validState
                                            || !validEmail || !emailAvailable || !cardComplete}>
                                                <Translate id="payment.submit-payment"/>
                                            </Button>
                                        </Col>
                                        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            <small>
                                                <Translate id="registration-full-form.by-submitting"/>
                                                <ModalChildrenPolicy isOpen={showModalChildrensPolicy} toggle={toggleModalChildrenPolicy}/>
                                                <ModalTerms isOpen={showModalTerms} toggle={toggleModalTerms} color="link"/>
                                                <ModalReturnPolicy isOpen={showModalReturnPolicy} toggle={toggleModalReturnPolicy}/>
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
            {loading ? <LoaderFullWindow message={<Translate id="commercial.processing-transaction"/>} /> : null}
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