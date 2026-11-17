import React, { useState, useEffect } from 'react';
import { Container, FormGroup, Col, Row, Button, CardHeader, Card, CardBody } from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from '../../../custom-classes/settings';
import NumericInput from 'react-numeric-input';
import { createSubscriptions, getPlanTotalCosts } from '../../../api/subscriptions';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import ModalSuccess from '../../shared-module-components/modal/modal-success-registration';
import CalculationBox from '../../guest-module/calculation-box/calculation-box-v2';
import LiveChat from 'react-livechat';
import "./style.css";
import SelectorPlan from "../../guest-module/selector/selector-plan";
import { CardElement, useStripe, useElements } from "@stripe/react-stripe-js";
import QueryString from "query-string";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import SessionManager from '../../../custom-classes/session-manager';
import { SendAccessLog } from '../../../utils/access_logger';

/**
 * The component (window) when the client acquires a new subscription
 */
const SubscriptionAddNew = ({ location }) => {
    // Hooks to access Stripe.js and Elements
    const stripe = useStripe();
    const elements = useElements();
    const userDetails = SessionManager.getSessionDetails();
    const parsedString = QueryString.parse(location.search);
    const [chatLiveChatLicense] = useState(Settings.CHAT_LIVE_CHAT_LICENSE);
    const [loading, setLoading] = useState(false);
    const [planCostDetails, setPlanCostDetails] = useState(null);
    const [planId, setPlanId] = useState(parsedString?.planId ?? null);
    const [planName, setPlanName] = useState(null);
    const [planInterval, setPlanInterval] = useState(parsedString?.interval ?? null);
    const planIdFromUrl = !!parsedString?.planId;
    const [couponPlatformId, setCouponPlatformId] = useState(parsedString?.coupon ?? null);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [studentsQuantity, setStudentsQuantity] = useState(1);
    const [subscriptionId, setSubscriptionId] = useState('');
    const [validCalculation, setValidCalculation] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [formCompleted, setFormCompleted] = useState(false); // This seems to be a flag for form submission status
    const [countryIso, setCountryIso] = useState(null);
    const [stateIso, setStateIso] = useState(null);

    // State for Stripe CardElement specific errors and completeness
    const [cardComplete, setCardComplete] = useState(false);
    const [stripeErrorMessage, setStripeErrorMessage] = useState(null);


    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        try {
        SendAccessLog({
            UserName: userDetails.Username,
            InvitationCode: userDetails.InvitationCode,
            Role: userDetails.Roles[0],
            AreaId: "24",
        })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);


    useEffect(() => {
        const params = new URLSearchParams(location.search);
        const urlPlanId = params.get("planId");

        if (urlPlanId) {
            setPlanId(urlPlanId);
        }

    }, [location.search]);

    useEffect(() => {
        calculateCosts();
    }, [planId, studentsQuantity, couponPlatformId, stateIso, countryIso]); // Added all dependencies for calculateCosts


    const setCouponCode = (e) => {
        setCouponPlatformId(e);
        // calculateCosts() is already called by the useEffect above when couponPlatformId changes
    };

    const toggleSuccessModal = () => {
        setShowModalSuccess(false);
    };

    const toggleAlertModal = () => {
        setShowErrorModal(false);
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

    // Submits the new subscription (now async and handles Stripe token creation)
    const submitSubscription = async () => { // Made async
        // Clear previous errors
        setStripeErrorMessage(null);
        setShowErrorModal(false);
        setPropsModal(null);

        if (planId === null || planId === "0" || !validCalculation || studentsQuantity === 0) {
            setFormCompleted(false);
            let modalProps = {
                bodyText: LanguageManager.getTranslation("guest.form-incomplete-error"), // Example generic error message
                visible: true
            };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            return; // Stop if form data is incomplete
        }
        
        try {
            SendAccessLog({
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "24",
                PlanSubscriptionId: planId,
                PlanSubscriptionName: planName,
                QuantityEnrollment: studentsQuantity
            })
        }
        catch (error) {
            // empty catch block to avoid breaking the application
        }
        
        setLoading(true); // Start loading
        setFormCompleted(true); // Indicate form submission started

        if (!stripe || !elements) {
            // Stripe.js has not yet loaded.
            const errorMessage = LanguageManager.getTranslation("general.stripe-not-loaded");
            setStripeErrorMessage(errorMessage);
            let modalProps = { bodyText: errorMessage, visible: true };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
            return;
        }

        const cardElement = elements.getElement(CardElement);

        if (!cardComplete || !cardElement) {
            const errorMessage = LanguageManager.getTranslation("general.complete-card-details");
            setStripeErrorMessage(errorMessage);
            let modalProps = { bodyText: errorMessage, visible: true };
            setPropsModal(modalProps);
            setShowErrorModal(true);
            setLoading(false);
            return;
        }

        try {
            // Create the token (Option 2)
            const { paymentMethod, error: stripeError } = await stripe.createPaymentMethod({
                type: 'card',
                card: cardElement,
                // billing_details: { // Optionally add billing details if collected
                //     name: `${firstName} ${lastName}`,
                //     email: email,
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

            // Call your backend API with the token ID
            let apiResponse = await createSubscriptions(planId, studentsQuantity, couponPlatformId, paymentMethod.id); // Pass token.id

            if (apiResponse.status !== 200 && apiResponse.status !== 201) {
                let message = apiResponse.data?.Messages?.length ?
                    apiResponse.data.Messages[0] : LanguageManager.getTranslation("general.unknown-api-error");
                let modalProps = {
                    bodyText: message,
                    visible: true
                };
                setPropsModal(modalProps);
                setShowErrorModal(true);
            } else {
                // Set success message
                setSubscriptionId(apiResponse.data.Subscriptions[0].SubscriptionId);
                setShowModalSuccess(true);
                // Reset form fields after successful submission
                setPlanId(null);
                setStudentsQuantity(1);
                setCouponPlatformId(null);
                setCardComplete(false);
                setStripeErrorMessage(null);
                // You might need to reset the CardElement itself. This usually requires remounting or using `cardElement.clear()`.
                // cardElement.clear(); // This method might not be directly available on all versions or elements
            }
        } catch (error) {
            console.error('Submission error:', error);

            const fallbackMessage = LanguageManager.getTranslation("general.unexpected-error");

            const errorMessage =
                error?.response?.data?.Messages?.length > 0
                    ? error.response.data.Messages[0]
                    : fallbackMessage;

            let modalProps = { bodyText: errorMessage, visible: true };
            setPropsModal(modalProps);
            setShowErrorModal(true);
        } finally {
            setLoading(false); // Always stop loading
        }
    };

    const renderTotals = (costData) => {
        if (planId === null || costData === null) {
            return null;
        }
        return (
            <CalculationBox costData={{ costData }} />
        );
    };

    const calculateCosts = async () => { // Made async to handle potential async API calls
        if (studentsQuantity > 0 && planId != null) {
            try {
                let response = await getPlanTotalCosts(planId, studentsQuantity, couponPlatformId, stateIso, countryIso);
                if (response.data !== null) {
                    setValidCalculation(true);
                    setPlanCostDetails(renderTotals(response.data));
                } else {
                    setValidCalculation(false);
                    setPlanCostDetails(null);
                }
            } catch (error) {
                console.error("Error calculating costs:", error);
                setValidCalculation(false);
                setPlanCostDetails(null);
                // Optionally show an error to the user
            }
        } else {
            setPlanCostDetails(null);
            setValidCalculation(false);
        }
    };

    const handleQuantityProfilesChange = (value) => {
        setStudentsQuantity(value);
        // calculateCosts() is already called by the useEffect above when studentsQuantity changes
    };

    return (
        <div>
            <div className="container-padding card-white">
                <Container className="container-wrapper">
                    <Row>
                        <Col>
                            <Card>
                                <CardHeader>
                                    <Translate id="guest.add-new-plan" />
                                </CardHeader>
                                <CardBody>
                                    <Row>
                                        <Col xs={12} sm={12} md={6}>
                                            <Translate id="guest.select-a-plan" options={{ renderInnerHtml: true }} />
                                        </Col>
                                        <Col>
                                            <SelectorPlan
                                                disabled={planIdFromUrl}
                                                isCommercial={true}
                                                selectedPlan={planId}
                                                callPromo={false}
                                                updateParent={(selectedPlan) => {
                                                    setPlanId(selectedPlan.value);
                                                    setPlanName(selectedPlan.label);
                                                    // calculateCosts() called by useEffect
                                                }}
                                            />
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={12} sm={12} md={6}>
                                            <Translate id="guest.select-the-quantity-of-students-to-enroll" options={{ renderInnerHtml: true }} />
                                        </Col>
                                        <Col>
                                            <NumericInput
                                                defaultValue={1}
                                                onChange={handleQuantityProfilesChange}
                                                className="mantine-like__control" strict={true} min={1}
                                                max={5} mobile={'auto'} />
                                            <br />
                                            <small>
                                                <Translate id="guest.select-the-quantity-of-students-to-enroll-subtitle" options={{ renderInnerHtml: true }} />
                                            </small>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={12} sm={12} md={6} />
                                        <Col>
                                            <FormGroup row>
                                                {planCostDetails !== null
                                                    ? planCostDetails : null}
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={12} sm={12} md={6}>
                                            <Translate id="commercial.payment-information" options={{ renderInnerHtml: true }} />
                                        </Col>
                                        <Col>
                                            <FormGroup>
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
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={6} />
                                        <Col xs={6}>
                                            <Button
                                                block
                                                color="success"
                                                onClick={submitSubscription}
                                                disabled={!validCalculation || loading || !stripe || !elements || !cardComplete} // Updated disabled logic
                                            >
                                                <Translate id="registration-full-form.submit-payment" />
                                            </Button>
                                        </Col>
                                    </Row>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </Container>
                <LiveChat license={chatLiveChatLicense} />
            </div>
            {loading ? <LoaderFullWindow /> : null}
            {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal} /> : null}
            {showModalSuccess ?
                <ModalSuccess
                    isEdit={true}
                    visible={showModalSuccess}
                    subscriptionId={subscriptionId}
                    orderNo={"ORD" + subscriptionId}
                    updateParent={toggleSuccessModal} /> : null}
        </div>
    );
};

export default withLocalize(SubscriptionAddNew);