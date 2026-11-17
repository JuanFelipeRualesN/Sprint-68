/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import {Media, Card, CardBody, Col, Button, Form, FormGroup, Row, Container} from 'reactstrap';
import {Link} from "react-router-dom";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faEnvelope, faLock} from '@fortawesome/free-solid-svg-icons';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import LanguageToggle from "../../shared-module-components/selector/language-selector";
/** End of Multiple Language imports **/

import FormGroupEmail from '../../shared-module-components/form/form-group-email';
import {getPasswordResetPetition} from '../../../api/users';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import './style.css';
import { getAsset } from '../../../utils/getAssets';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const LoginBackground = getAsset("Login-icon-Background");

/**
 * The component (window) to reset the password
 */
const PasswordReset = () => {
    const [email, setEmail] = useState('');
    const [loading, setLoading] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [validEmail, setValidEmail] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

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

    // Handle the reset password submition.
    const handleSubmit = async () => {
        setLoading(true);
        try {
            const response = await getPasswordResetPetition(email);
            let modalProps = {
                bodyText: response.data.Messages[0],
                visible: true
            };
            setPropsModal(modalProps);
            setShowModal(true);
            setLoading(false);
        } catch (error) {
            setLoading(false);
        }
    };

    // Handles when the user inputs the email
    const handleUserEmailInput = (e) => {
        setValidEmail(e.validEmail);
        setEmail(e.email);
    };

    // Renders the main form when the user enters the email.
    const renderForm = () => {
        return <div>
            <Card className='container-login-teacher'>
                <CardBody>
                    <div className='login-header-PReset'>
                        <p className='iconPReset'><FontAwesomeIcon icon={faLock}/>{' '}</p>
                        <small><Translate id="password-reset.enter-email"/></small>
                    </div>
                    <Form>
                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <FormGroupEmail isForPasswordReset={true} updateParent={handleUserEmailInput}/>
                            </Col>
                        </FormGroup>

                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <button className="btn-default button-primary-custom btn-block"  block onClick={handleSubmit}
                                        disabled={!validEmail}>
                                    <Translate id="general.submit"/>
                                </button>
                            </Col>
                        </FormGroup>

                        <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <Link to="/" className="btn-default button-primary-custom btn-block">
                                    <Translate id="password-reset.go-back-to-login"/>
                                </Link>
                            </Col>
                        </FormGroup>
                        {/* <FormGroup row>
                            <Col sm={{size: 10, offset: 1}}>
                                <LanguageToggle/>
                            </Col>
                        </FormGroup>                         */}
                    </Form>
                </CardBody>
            </Card>
        </div>;
    };

    return (
        // <div>
        //     <Col sm={{size: 8, offset: 2}} lg={{size: 6, offset: 3}} style={{marginTop: 100}}>
        //         {this.renderForm()}
        //     </Col>
        //     {this.state.loading ? <LoaderFullWindow/> : null}
        //     {this.state.showModal ? this.renderGenericModal() : null}
        // </div>
        <div className='wrapper-container-login' style={{ backgroundImage: `url(${LoginBackground})`, backgroundPosition: 'center top', backgroundRepeat: 'no-repeat' }}>
            <div className="container-padding ">
                <Container className="container-wrapper">
                    <Row>
                        <Col sm={{ size: 8, offset: 2 }} lg={{ size: 6, offset: 3 }} style={{ marginTop: 100 }}>
                            {renderForm()}
                        </Col>
                    </Row>
                </Container>
            </div>
            {loading ? <LoaderFullWindow /> : null}
            {showModal ? renderGenericModal() : null}
        </div>
    );
};

export default withLocalize(PasswordReset); // withLocaliza is added to support language. Else, don't add it.