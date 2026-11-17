import React, { useState, useEffect } from "react";
import {Input, Col, Row, Button, Form, InputGroup,  InputGroupText} from 'reactstrap';
import FormGroupNewPasswordTutorV2 from "../form-group-new-password-tutor-v2";
import {Translate} from "react-localize-redux";
import {FaEnvelope} from "react-icons/all";
import {setPasswordRegisterV2} from '../../../../api/users';
import LoaderFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";

/**
 * Handle the class to an update administrator account password
 */
const FormAdministratorAccount = ({ disabled, email, updateParent }) => {
    const [emailState, setEmailState] = useState(email);
    const [password, setPassword] = useState('');
    const [passwordRetype, setPasswordRetype] = useState('');
    const [proceedUpdatePassword, setProceedUpdatePassword] = useState(false);
    const [validPassword, setValidPassword] = useState(true);
    const [loading, setLoading] = useState(false);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [propsModal, setPropsModal] = useState(null);

    useEffect(() => {
        setEmailState(email);
    }, [email]);

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    const savePassword = (newValue) => {
        setPassword(newValue.password);
        setPasswordRetype(newValue.passwordRetype);
        setValidPassword(newValue.validPassword);
        setSubmitPasswordButtonState(newValue.validPassword);
    };

    const setSubmitPasswordButtonState = (isValid) => {
        setProceedUpdatePassword(isValid);
    };

    const savePasswordAPI = () => {
        setLoading(true);

        let apiCall = setPasswordRegisterV2(password, passwordRetype);

        apiCall.then((response) => {
            if(response.status !== 200 && response.status !== 201) {
                let propsModal = {
                    bodyText: response.data.Messages[0],
                    visible: true
                };
                setPropsModal(propsModal);
                setShowErrorModal(true);
                setLoading(false);
            }
            else
            {
                setLoading(false);
                updateParent(true);
            }
        });
    };

    const renderAdministratorAccount = () => {
        return (
            <Form>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:12}}>
                        <h2 className="centered-text">
                            <Translate id="register-v2.administrator-account"/>
                        </h2>
                    </Col>
                </Row>
                <Row>
                    <hr/>
                </Row>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 8, offset: 2}}>
                        <small>
                            <Translate id="register-v2.administrator-account-details"/>
                        </small>
                    </Col>
                </Row>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 8, offset: 2}}>
                        <InputGroup>
                            <Input disabled={true} value={emailState} type="email" name="email" id="email"/>
                           <div className="input-group-append">
                                <InputGroupText><FaEnvelope /></InputGroupText>
                            </div>
                        </InputGroup>
                    </Col>
                </Row>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 8, offset: 2}}>
                        <FormGroupNewPasswordTutorV2 updateParent={savePassword}/>
                    </Col>
                </Row>
                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}} className="text-right">
                        <Button color="success"
                                disabled={!proceedUpdatePassword}
                                onClick={savePasswordAPI}>
                            <Translate id="register-v2.submit"
                                       options={{renderInnerHtml: false}}/>
                        </Button>
                    </Col>
                </Row>
            </Form>)
    };

    // Render the form group
    return (
        <div>
            <Row>
                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    {renderAdministratorAccount()}
                </Col>
            </Row>
            {loading ? <LoaderFullWindow/> : null}
            {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
        </div>
    );
};

export default FormAdministratorAccount;