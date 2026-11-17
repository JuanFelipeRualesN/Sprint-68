import React, { useState, useEffect } from 'react';
import {
    Input,
    Col,
    Button,
    Card,
    CardBody,
    CardHeader,
    Row,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter, Alert
} from 'reactstrap';
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import {updateUserMainAddress, getUserMainAddress, insertNewPassword} from "../../../api/users";
import FormGroupAddress from '../form/form-group-address';
import FormGroupNewPassword from '../form/form-group-new-password';
import Settings from '../../../custom-classes/settings';
import SessionManager from "../../../custom-classes/session-manager";
import {BeatLoader} from "react-spinners";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import Menu from "../menu/index";
import "./style.css";
import {CardElement, useElements, useStripe} from "@stripe/react-stripe-js";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faInfoCircle} from "@fortawesome/free-solid-svg-icons";
import {updateCustomerPaymentMethod} from "../../../api/subscriptions";
import ModalGeneric from "../modal/modal-generic";

/**
 * The component (window), where the user can update her information
 */
const UpdateInformation = ({className}) => {
    const [addressLine1, setAddressLine1] = useState('');
    const [addressLine2, setAddressLine2] = useState('');
    const [city, setCity] = useState('');
    const [country, setCountry] = useState('');
    const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());
    const [loadingAddress, setLoadingAddress] = useState(false);
    const [loadingPassword, setLoadingPassword] = useState(false);
    const [loadingPayment, setLoadingPayment] = useState(false);
    const [modal, setModal] = useState(false);
    const [modalMessage, setModalMessage] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [password, setPassword] = useState('');
    const [passwordRetype, setPasswordRetype] = useState('');
    const [proceedSubmitAddress, setProceedSubmitAddress] = useState(true);
    const [proceedSubmitPassword, setProceedSubmitPassword] = useState(false);
    const [roles] = useState(SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles);
    const [state, setState] = useState('');
    const [validAddress, setValidAddress] = useState(true);
    const [validCity, setValidCity] = useState(true);
    const [validCountry, setValidCountry] = useState(true);
    const [validState, setValidState] = useState(true);
    const [validZipCode, setValidZipCode] = useState(true);
    const [zipCode, setZipCode] = useState('');
    const [cardComplete, setCardComplete] = useState(false);
    const [stripeErrorMessage, setStripeErrorMessage] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const stripe = useStripe();
    const elements = useElements();

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        if(roles.indexOf(Settings.ROLE_CLIENT_STR) > - 1) {
            let apiCall = getUserMainAddress();
            apiCall.then((response) => {
                setAddressLine1(response.data.AddressLine1);
                setAddressLine2('');
                setCity(response.data.City);
                setState(response.data.State);
                setCountry(response.data.CountryIsoCode2); // Default
                setZipCode(response.data.Zip);
            });
        }
    }, [roles]);

    // Hide or shows the message window
    const toggle = () => {
        setModal(!modal);
    };

    // Saves the old password in the state
    const saveOldPasswordState = (event) => {
        setOldPassword(event.currentTarget.value);
        setSubmitNewPasswordState();
    };

    const renderGenericModal = () => {
        return (
            <ModalGeneric
                data={propsModal}
                updateParent={toggleAlertModal}
            />
        );
    };

    // Saves the password state
    const savePasswordState = (newValue) => {
        setPassword(newValue.password);
        setPasswordRetype(newValue.passwordRetype);
        setSubmitNewPasswordState();
    };

    // Save the address
    const saveAddress = (newValue) => {
        setAddressLine1(newValue.addressLine1);
        setAddressLine2(newValue.addressLine2);
        setCity(newValue.city);
        setCountry(newValue.country);
        setState(newValue.state);
        setZipCode(newValue.zipCode);
        setValidAddress(newValue.validAddress);
        setValidCity(newValue.validCity);
        setValidCountry(newValue.validCountry);
        setValidState(newValue.validState);
        setValidZipCode(newValue.validZipCode);
        setSubmitAddressButtonState();
    };

    // Set the state of the button to submit the address
    const setSubmitNewPasswordState = () => {
        setProceedSubmitPassword(
            oldPassword.length > 5
            && password.length > 5
            && passwordRetype.length > 5
        );
    };

    // Saves the new password in the api call
    const saveNewPassword = () => {
        setLoadingPassword(true);
        // Saves the new password
        let apiCall = insertNewPassword(oldPassword, password, passwordRetype);
        apiCall.then((response)=> {
            setModalMessage(response.data.Messages[0]);
            setLoadingPassword(false);
            toggle();
        });
        setLoadingPassword(false);
    };

    // Send to the service the new address
    const updateUserMainAddressHandler = () => {
        setLoadingAddress(true);
        // Saves the new password
        let apiCall = updateUserMainAddress(addressLine1, addressLine2, city, state, country, country, zipCode);
        apiCall.then((response)=> {
            setModalMessage(response.data.Messages[0]);
            setLoadingAddress(false);
            toggle();
        });
    };

    // Sets the submit address button state
    const setSubmitAddressButtonState = () => {
        setProceedSubmitAddress(
            validAddress
            && validCity
            && validCountry
            && validState
            && validZipCode
        );
    };

    const updatePaymentMethod = async (event) => {
        // Ensure event is passed if this is an event handler (e.g., from a button click)
        if (event) {
            event.preventDefault(); // Prevent default form submission if this is a form handler
        }

        setLoadingPayment(true); // Start loading state

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
            const response = await updateCustomerPaymentMethod(null, null, paymentMethod.id);

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
            setStripeErrorMessage(errorMessage);


        } finally {
            // This will always run, regardless of success or failure
            setLoadingPayment(false);
        }
    };

    const retrieveSubTokenStripe = (event) => {
        setCardComplete(event.complete);
        if (event.error) {
            setStripeErrorMessage(event.error.message);
        } else {
            setStripeErrorMessage(null);
        }
    };

    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Call the item list
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                    <Row style={{minHeight:600}}>
                        {roles.indexOf(Settings.ROLE_CLIENT_STR) > - 1 ?
                            <>
                                <Col sm={{size: 12, offset: 0}} md={{size: 6}} lg={{size: 6}}>
                                    <Card className="card-white">
                                        <CardHeader>
                                            <Row>
                                                <h4 className="titles-general">
                                                    <Translate id="form-updates.edit-your-info"
                                                               options={{renderInnerHtml: true}}/>
                                                </h4>
                                            </Row>
                                            <Row>
                                                <small>
                                                    <Translate id="form-updates.edit-info-subtitle"
                                                               options={{renderInnerHtml: true}}/>
                                                </small>
                                            </Row>
                                        </CardHeader>
                                        <CardBody>
                                            <Row>
                                                <Col xs={{size:12}}>
                                                    <div style={{marginLeft: 10}}>
                                                        <FormGroupAddress data={{addressLine1, addressLine2, city, country, state, zipCode, validAddress, validCity, validCountry, validState, validZipCode}} updateParent={saveAddress}/>
                                                    </div>
                                                </Col>
                                                <Col sm={{size: 10, offset: 1}} lg={{size: 10, offset: 1}}
                                                     md={{size: 10, offset: 1}}>

                                                    <Button block color="success"
                                                            disabled={!proceedSubmitAddress || loadingAddress ? "disabled": ""}
                                                            onClick={updateUserMainAddressHandler}>
                                                        {loadingAddress ?
                                                            <BeatLoader
                                                                size={10}
                                                                color={"#f6ffff"}
                                                            /> : null
                                                        }
                                                        <Translate id="general.submit" options={{renderInnerHtml: true}}/>
                                                    </Button>
                                                </Col>
                                            </Row>
                                        </CardBody>
                                    </Card>
                                </Col>
                                <Col sm={{size: 12, offset: 0}} md={{size: 6}} lg={{size: 6}}>
                                    <Card className="card-white">
                                        <CardHeader>
                                            <Row>
                                                <h4 className="titles-general">
                                                    <Translate id="form-updates.update-payment-info"
                                                               options={{renderInnerHtml: true}}/>
                                                </h4>
                                            </Row>
                                            <Row>
                                                <small>
                                                    <Translate id="form-updates.update-payment-info-subtitle"
                                                               options={{renderInnerHtml: true}}/>
                                                </small>
                                            </Row>
                                        </CardHeader>
                                        <CardBody>
                                            <Row>
                                                <Col xs={12} style={{padding: 10}}>
                                                    <Alert color="info">
                                                        <FontAwesomeIcon icon={faInfoCircle}/> {' '}
                                                        <Translate id="form-updates.update-payment-information-security"/>
                                                    </Alert>
                                                </Col>
                                            </Row>
                                            <Row>
                                                <Col>
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
                                                    <button
                                                            disabled={!cardComplete || loadingPayment ? "disabled" : ""}
                                                            className="btn-default button-primary-custom"
                                                            onClick={updatePaymentMethod}>
                                                        {loadingPayment ?
                                                            <BeatLoader
                                                                size={10}
                                                                color={"#f6ffff"}
                                                            /> : null}
                                                        <Translate id="general.submit"/>
                                                    </button>
                                                </Col>
                                            </Row>
                                        </CardBody>
                                    </Card>
                                </Col>
                            </>
                            : null}
                        <Col sm={{size: 12, offset: 0}} md={{size: 6}} lg={{size: 6}}>
                            <Card className="card-white">
                                <CardHeader>
                                    <Row>
                                        <h4 className="titles-general">
                                            <Translate id="form-updates.reset-your-password-title"
                                                       options={{renderInnerHtml: true}}/>
                                            </h4>
                                        </Row>
                                    <Row>
                                        <small>
                                            <Translate id="form-updates.reset-your-password-subtitle"
                                                       options={{renderInnerHtml: true}}/>
                                        </small>
                                    </Row>
                                </CardHeader>
                                <CardBody>
                                    <Row className="pl-4 pr-4">
                                        <Col sm={{size: 12, offset: 0}} lg={{size: 12, offset: 0}}
                                             md={{size: 12, offset: 0}}>
                                            <p className="label-forms-blue">
                                                <Translate id="form-updates.old-password-instructions"
                                                           options={{renderInnerHtml: true}}/>
                                            </p>
                                            <Input  className="mantine-like__control" type="password" onChange={saveOldPasswordState}/>
                                        </Col>
                                    </Row>
                                    <Row className="pl-4 pr-4">
                                        <Col sm={{size: 12, offset: 0}} lg={{size: 12, offset: 0}}
                                             md={{size: 12, offset: 0}}>
                                            <FormGroupNewPassword data={{password, passwordRetype}}
                                                                  updateParent={savePasswordState}/>
                                        </Col>
                                        <Col sm={{size: 10, offset: 1}} lg={{size: 10, offset: 1}}
                                             md={{size: 10, offset: 1}}>
                                            <button className="btn-default btn-submit w-100"
                                                    disabled={!proceedSubmitPassword || loadingPassword ? "disabled" : ""}
                                                    onClick={saveNewPassword}>
                                                {loadingPassword ?
                                                    <BeatLoader
                                                    size={10}
                                                    color={"#f6ffff"}
                                                /> : null }
                                                <Translate id="general.submit"/>
                                            </button>
                                        </Col>
                                    </Row>
                                </CardBody>
                            </Card>
                            <br/>
                        </Col>
                    </Row>
                    <Modal isOpen={modal} toggle={toggle} className={className}>
                        <ModalHeader className="card-white" toggle={toggle}>
                            <Translate id="form-updates.attention" options={{renderInnerHtml: false}}/>
                        </ModalHeader>
                        <ModalBody className="card-white">
                            <p>{modalMessage}</p>
                        </ModalBody>
                        <ModalFooter className="card-white">
                            <button onClick={toggle} className="btn-default button-secondary-custom">
                                <Translate id="subscriptions.close" options={{renderInnerHtml: false}}/>
                            </button>
                        </ModalFooter>
                    </Modal>
            </div>
            {showModal ? renderGenericModal() : null}
        </div>
    )
};

export default withLocalize(UpdateInformation);