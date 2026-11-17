/**
 * Created by portiz on 5/13/19.
 */
import React, { useState, useEffect } from 'react';
import {Media, Card, CardBody, Col, Button, Form, FormGroup, Row} from 'reactstrap';
import {Link, Redirect} from "react-router-dom";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faEnvelope} from '@fortawesome/free-solid-svg-icons';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import LanguageToggle from "../../shared-module-components/selector/language-selector";
/** End of Multiple Language imports **/

import LanguageManager from '../../../custom-classes/language-manager';
import FromGroupNewPassword from '../../shared-module-components/form/form-group-new-password';
import {callPasswordResetNewPassword} from '../../../api/users';

import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';

import queryString from 'query-string';
import SessionManager from "../../../custom-classes/session-manager";
import Logo from '../../../assets/images/logo.png';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to reset the password
 */
const AccountAccessRecovery = ({ location }) => {
    const [loading, setLoading] = useState(false);
    const [password, setPassword] = useState('');
    const [propsModal, setPropsModal] = useState(null);
    const [sessionToken, setSessionToken] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [token, setToken] = useState(null);
    const [validPassword, setValidPassword] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        const values = queryString.parse(location.search);
        setToken(values.token);
    }, [location.search]);

    // Hide or shows alert modal.
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Renders the generic modal used for messages
    const renderGenericModal = () => {
        return (
            <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/>
        );
    };

    // Handle the reset password process.
    const handleSubmit = () => {
        setLoading(true);
        let apiCall = callPasswordResetNewPassword(token, password, password);
        apiCall.then((response) => {
            let modalProps = {
                bodyText: response.data.Messages[0],
                visible: true
            };
            if (typeof response.data.Token !== 'undefined' && response.data.Token !== null) {
                SessionManager.setSession(response.data);
                setSessionToken(response.data.Token);
            }

            setPropsModal(modalProps);
            setShowModal(true);
            setLoading(false);
        });
    };

    const handlePasswordInput = (e) => {
        setValidPassword(e.validPassword);
        setPassword(e.password);
    };

    const renderForm = () => {
        if(typeof sessionToken !== 'undefined' && sessionToken !== null) {
            return (<Redirect to={{
                pathname: "/dashboard"
            }}/>);
        }

        return <div>
            <Card>
                <CardBody>
                    <FontAwesomeIcon icon={faEnvelope}/>{' '}
                    <Translate id="password-reset.enter-email"/>
                </CardBody>
                <CardBody>
                    <Form>
                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <FromGroupNewPassword isForPasswordReset={true} updateParent={handlePasswordInput}/>
                            </Col>
                        </FormGroup>

                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <Button color="success" block onClick={handleSubmit}
                                        disabled={!validPassword}>
                                    <Translate id="general.submit"/>
                                </Button>
                            </Col>
                        </FormGroup>

                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <Link to="/" className="btn-default button-secondary-custom btn-block">
                                    <Translate id="password-reset.go-back-to-login"/>
                                </Link>
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <LanguageToggle/>
                            </Col>
                        </FormGroup>
                        <Row>
                            <Col xs={{size: 8, offeset: 12}} lg={{size: 8, offset: 4}} className="text-center">
                                <Media src={Logo} alt="Company logo" style={{maxWidth: 120}}/>
                            </Col>
                        </Row>
                    </Form>
                </CardBody>
            </Card>
        </div>;
    };

    // Renders the main form
    return (
        <div>
            <Col sm={{size: 8, offset: 2}} lg={{size: 6, offset: 3}} style={{marginTop: 100}}>
                {renderForm()}
            </Col>

            {loading ? <LoaderFullWindow/> : null}
            {showModal ? renderGenericModal() : null}
        </div>
    );
};

export default withLocalize(AccountAccessRecovery); // withLocaliza is added to support language. Else, don't add it.