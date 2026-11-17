import React, { useState, useContext, useEffect } from 'react';
import {
    Alert,
    Card,
    CardBody,
    Col,
    Button,
    Form,
    FormGroup,
    Label,
    Input,
    Row,
    Container,
    InputGroupText, InputGroup
} from 'reactstrap';
import { Link, Redirect } from "react-router-dom";
import { faEye } from '@fortawesome/free-solid-svg-icons';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { getLoginClient } from '../../../api/users';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import NavbarClients from '../../shared-module-components/navbar/navbar-clients';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import SessionManager from '../../../custom-classes/session-manager';

import Settings from "../../../custom-classes/settings";
import './style.css';
import LiveChat from "react-livechat";
import { isRoleAllowed } from "../../../utils/utils";
import Sso from './loginSso';
import { AppContext } from "../../../App.Context";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { getAsset } from '../../../utils/getAssets';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const LoginBackground = getAsset("Login-icon-Background");

/**
 * The component (window) login
 */

const Login = () => {
    const context = useContext(AppContext);
    const [showPassword, setShowPassword] = useState(false);
    const [state, setState] = useState({
        isLoggedIn: false,
        isMicrosoftTeams: SessionManager.getIfMicrosoftTeams(),
        loading: false,
        loginError: false,
        loginErrorMessage: '',
        roles: SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles,
        password: "",
        propsModal: null,
        showErrorModal: false,
        username: "",
        usernameOrPasswordValid: true,
    });

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleChangeUsername = (event) => {
        const username = event.currentTarget.value;
        setState(prev => ({
            ...prev,
            username,
            usernameOrPasswordValid: username.length > 0 && prev.password.length > 0
        }));
    };

    const handleChangePassword = (event) => {
        const password = event.currentTarget.value;
        setState(prev => ({
            ...prev,
            password,
            usernameOrPasswordValid: prev.username.length > 0 && password.length > 0
        }));
    };

    const toggleAlertModal = () => {
        setState(prev => ({ ...prev, showErrorModal: false }));
    };

    const postCrossRedirectStudentLogged = (session) => {
        let iframe = document.createElement('iframe');
        iframe.src = Settings.URL_SITE_STUDENTS;
        iframe.loading = 'lazy';
        iframe.style = 'visibility: hidden';
        document.body.appendChild(iframe);
        iframe.onload = () => {
            let win = iframe.contentWindow;
            win.postMessage({ type: 'login', body: session }, Settings.URL_SITE_STUDENTS);
            const url = `${Settings.URL_SITE_STUDENTS}/students/home`;
            window.location.href = url;
        }
    };

    const login = async () => {
        if (state.username.length === 0 || state.password.length === 0) {
            setState(prev => ({ ...prev, usernameOrPasswordValid: false }));
        } else {
            setState(prev => ({ ...prev, loading: true }));
            try {
                const response = await getLoginClient(state.username, state.password);
                const session = response.data;
                const projectEnv = Settings.PROJECT_ENVIRONMENT;
                session.ProjectEnvironment = projectEnv;
                if (isRoleAllowed([Settings.ROLE_STUDENT_STR], session.Roles)) {
                    postCrossRedirectStudentLogged(session);
                } else {
                    SessionManager.setSession(session);
                    await context.refreshNotifications();
                    setState(prev => ({ ...prev, isLoggedIn: true, loading: false, roles: session.Roles }));
                }
            } catch (error) {
                setState(prev => ({ ...prev, isLoggedIn: false, loading: false }));
            }
        }
    };

    const renderGenericModal = () => (
        <ModalGeneric data={state.propsModal} updateParent={toggleAlertModal} />
    );

    const handleKeyDown = (event) => {
        if (event.key === 'Enter' && state.username && state.password) {
            login();
        }
    };

    const renderForm = () => (
        <div>
            <Col sm={{ size: 10, offset: 1 }} md={{ size: 10, offset: 1 }} lg={{ size: 8, offset: 2 }}
                 className="margin-top-mandatory-25 animated delay-1s fadeIn container-login-teacher">
                <Card>
                    <CardBody>
                        <p className='title-login-teacher'><Translate id="login.welcome-back" options={{ renderInnerHtml: true }} /></p>
                        <Form onKeyDown={handleKeyDown}>
                            {context.state.variables.InternalLoginManagement && (<>
                                <FormGroup row>
                                    <Col sm={{ size: 10, offset: 1 }}>
                                        <Label for="username">
                                            <Translate id="login.login-by-username-or-password" />
                                        </Label>
                                        <Input type="text" name="username" id="username" onChange={handleChangeUsername} />
                                    </Col>
                                </FormGroup>
                                <FormGroup row>
                                    <Col sm={{ size: 10, offset: 1 }}>
                                        <Label for="password">
                                            <Translate id="login.password" />
                                        </Label>
                                        <InputGroup>
                                            <Input
                                                type={showPassword ? "text" : "password"}
                                                name="password"
                                                id="password"
                                                onChange={handleChangePassword}
                                            />
                                            <div className="input-group-append">
                                                <InputGroupText
                                                    onClick={() => setShowPassword(prev => !prev)}
                                                    style={{cursor: 'pointer'}}
                                                >
                                                    <FontAwesomeIcon icon={faEye}/>
                                                </InputGroupText>
                                            </div>
                                        </InputGroup>
                                    </Col>
                                </FormGroup>
                                <FormGroup row>
                                    <Col sm={{ size: 10, offset: 1 }}>
                                        <Alert color="danger" isOpen={!state.usernameOrPasswordValid}>
                                            <Translate id="login.username-password-required" />
                                        </Alert>
                                        <Alert color="danger" isOpen={state.loginError}>
                                            <span>{state.loginErrorMessage}</span>
                                        </Alert>
                                    </Col>
                                </FormGroup>
                            </>)}
                            {context.state.variables.InternalPasswordReset && <FormGroup row>
                                <Col sm={{ size: 10, offset: 1 }}>
                                    <Link color="link" to="/password-reset" className='login-teacher-link'>
                                        <Translate id="login.forgot-password" />
                                    </Link>
                                </Col>
                            </FormGroup>}
                            {context.state.variables.InternalLoginManagement && <FormGroup row>
                                <Col sm={{ size: 10, offset: 1 }}>
                                    <Button color="none" className='login-btn' block onClick={login} disabled={!state.usernameOrPasswordValid}>
                                        <Translate id="login.log-in" />
                                    </Button>
                                </Col>
                            </FormGroup>}
                            {context.state.variables.MicrosoftSSOManagement && <Sso />}
                            {Settings.IS_APP_MANAGED_BY_REQUIRED && Settings.getCheckoutStatus() && <FormGroup row>
                                <Col sm={{ size: 10, offset: 1 }}>
                                    {!state.isMicrosoftTeams ?
                                        <Link className="btn-default button-secondary-custom btn-block" color="link" to="/payment">
                                            <Translate id="login.register" />
                                        </Link> :
                                        <Link className="btn-default button-secondary-custom btn-block" color="link" to="/register-free">
                                            <Translate id="register.register-as-a-teacher" />
                                        </Link>
                                    }
                                </Col>
                            </FormGroup>}
                            <FormGroup>
                                <Col style={{textAlign: "center"}}>
                                    <small style={{color:"gray"}}>V.{Settings.getAppVersion()}</small>
                                </Col>
                            </FormGroup>
                        </Form>
                    </CardBody>
                </Card>
            </Col>
        </div>
    );

    if (SessionManager.isAuth()) {
        return <Redirect to="/dashboard" />;
    }

    return (
        <div className='wrapper-container-login' style={{ backgroundImage: `url(${LoginBackground})`, backgroundPosition: 'center top', backgroundRepeat: 'no-repeat' }}>
            <NavbarClients />
            <div className="container-padding ">
                <Container className="container-wrapper">
                    <Row>
                        <Col sm={{ size: 10, offset: 1 }} lg={{ size: 8, offset: 2 }} md={{ size: 10, offset: 1 }}>
                            {renderForm()}
                        </Col>
                    </Row>
                </Container>
            </div>
            {!state.isMicrosoftTeams ? <LiveChat license={Settings.CHAT_LIVE_CHAT_LICENSE} /> : null}
            {state.loading && <LoaderFullWindow />}
            {state.showErrorModal && renderGenericModal()}
        </div>
    );
};

export default withLocalize(Login);
