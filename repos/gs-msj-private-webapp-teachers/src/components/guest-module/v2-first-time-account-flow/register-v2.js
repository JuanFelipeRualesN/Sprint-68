import React, { useState, useEffect } from 'react';
import {Col, Row, CardBody, Container} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';
/** End of Multiple Language imports **/

import Settings from '../../../custom-classes/settings';
import FormAdministratorAccount from "../form/v2-registration/form-administrator-account";
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import 'react-s-alert/dist/s-alert-default.css';
import 'react-s-alert/dist/s-alert-css-effects/bouncyflip.css';
import FormAdministratorContact from "../form/v2-registration/form-administrator-contact";
import FormStudentAccount from "../form/v2-registration/form-student-account";
import FormAppDownload from "../form/v2-registration/form-app-download";
import ModalSuccessRegistrationV2 from "../modal/modal-success-registration-v2";
import {sendEmailWithAccountDetails} from '../../../api/users';
import {getSubscriptionDetailsClient} from "../../../api/subscriptions";
import './style.css';

import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to register and subscribe a client for the first time. It's a single page subscription
 */
const RegisterV2 = ({location}) => {
    const imgStep1NonActive = 'step-1-a';
    const imgStep1Active = 'step-1-active-a';
    const imgStep1Completed = 'step-1-complete';

    const imgStep2NonActive = 'step-2-a';
    const imgStep2Active = 'step-2-active-a';
    const imgStep2Completed = 'step-2-complete';

    const imgStep3NonActive = 'step-3-a';
    const imgStep3Active = 'step-3-active-a';
    const imgStep3Completed = 'step-3-complete';

    const imgStep4NonActive = 'step-4-a';
    const imgStep4Active = 'step-4-active-a';
    const imgStep4Completed = 'step-4-complete';

    const [chatLiveChatLicense] = useState(Settings.CHAT_LIVE_CHAT_LICENSE);
    const [currentStep, setCurrentStep] = useState(1);
    const [email, setEmail] = useState('');
    const [imgStep1, setImgStep1] = useState(imgStep1Active);
    const [imgStep2, setImgStep2] = useState(imgStep2NonActive);
    const [imgStep3, setImgStep3] = useState(imgStep3NonActive);
    const [imgStep4, setImgStep4] = useState(imgStep4NonActive);
    const [loading, setLoading] = useState(false);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    const [studentsQuantity, setStudentsQuantity] = useState(0);
    const queryParams = new URLSearchParams(location.search);
    const [subscriptionId] = useState(queryParams.get('subscriptionId'));
    const [planId, setPlanId] = useState(null);
    const [propsModal, setPropsModal] = useState(null);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [studentName, setStudentName] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.remove('landing-white'); // Custom background for students
        getSubscriptionDetails();
    }, []);

    const changeSelectedPlan = (e) => {
        setPlanId(e);
    };

    // Toggles the success modal
    const toggleSuccessModal = () => {
        setShowModalSuccess(false);
    };

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    const getSubscriptionDetails = () => {
        setLoading(true);
        let apiCall = getSubscriptionDetailsClient(subscriptionId);
        apiCall.then((response)=> {
            setLoading(false);
            setEmail(response.data.Email);
            setStudentsQuantity(parseInt(response.data.Subscriptions[0].ProfilesTotalQuantity));
        });
    };

    const sendEmailSubscriptionDetails = () => {
        if(step3Completed) {
            sendEmailWithAccountDetails();
        }
    };

    const setStep1Complete = (valid) => {
        if(valid)
        {
            setImgStep1(imgStep1Completed);
            setImgStep2(imgStep2Active);
            setCurrentStep(2);
        }
    };

    const setStep2Complete = (valid) => {
        if(valid)
        {
            setImgStep2(imgStep2Completed);
            setImgStep3(imgStep3Active);
            setCurrentStep(3);
        }
    };

    const setStep3Complete = (valid) => {
        if(valid)
        {
            setImgStep3(imgStep3Completed);
            setImgStep4(imgStep4Active);
            setCurrentStep(4);
            sendEmailSubscriptionDetails();
        }
    };


    // Renders the landing page
    return (
        <div>
            <Container className="container-full">
            <Row>
                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 10, offset: 1}}>
                    <div className="margin-20-percent">
                        <div>
                            <CardBody>
                                <Row>
                                    <Col xs={{size:6}} sm={{size: 3}} md={{size: 3}} lg={{size: 3}} className="centered-text">
                                        <img className="register-v2-icons" src={`img/${imgStep1}.png`} alt="first step" />
                                        <br/>
                                        <small> <Translate id="register-v2.title-administrator"/></small>
                                    </Col>
                                    <Col xs={{size:6}} sm={{size: 3}} md={{size: 3}} lg={{size: 3}} className="centered-text">
                                        <img className="register-v2-icons" src={`img/${imgStep2}.png`} alt="second step"/>
                                        <br/>
                                        <small> <Translate id="register-v2.title-contact"/></small>
                                    </Col>
                                    <Col xs={{size:6}} sm={{size: 3}} md={{size: 3}} lg={{size: 3}} className="centered-text">
                                        <img className="register-v2-icons" src={`img/${imgStep3}.png`} alt="third step"/>
                                        <br/>
                                        <small> <Translate id="register-v2.title-student"/></small>
                                    </Col>
                                    <Col xs={{size:6}} sm={{size: 3}} md={{size: 3}} lg={{size: 3}} className="centered-text">
                                        <img className="register-v2-icons" src={`img/${imgStep4}.png`} alt="fourth step"/>
                                        <br/>
                                        <small> <Translate id="register-v2.title-system"/></small>
                                    </Col>
                                </Row>
                                {
                                    currentStep === 1 ?
                                        <Row className="animated delay-05s fadeIn">
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <hr/>
                                            </Col>
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <FormAdministratorAccount email={email}
                                                                            updateParent={setStep1Complete}/>
                                            </Col>
                                        </Row> : null
                                }
                                {
                                    currentStep === 2 ?
                                        <Row className="animated delay-05s fadeIn">
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <hr/>
                                            </Col>
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <FormAdministratorContact  updateParent={setStep2Complete}/>
                                            </Col>
                                        </Row> : null
                                }
                                {
                                    currentStep === 3 ?
                                        <Row className="animated delay-05s fadeIn">
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <hr/>
                                            </Col>
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                {studentName}
                                            </Col>
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <FormStudentAccount
                                                    studentsQuantity={studentsQuantity}
                                                    updateParent={setStep3Complete}
                                                    subscriptionId={subscriptionId} />
                                            </Col>
                                        </Row> : null
                                }
                                {
                                    currentStep === 4 ?
                                        <Row className="animated delay-05s fadeIn">
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <hr/>
                                            </Col>
                                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <FormAppDownload/>
                                            </Col>
                                        </Row> : null
                                }
                            </CardBody>
                        </div>
                    </div>
                </Col>
            </Row>
            </Container>
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
    )
};

export default withLocalize(RegisterV2);