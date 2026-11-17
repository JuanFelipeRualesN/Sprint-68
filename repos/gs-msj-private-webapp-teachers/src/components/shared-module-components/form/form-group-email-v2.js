import React, { useState, useEffect } from 'react';
import {Input, Row, Col, Alert} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle, faExclamationTriangle } from '@fortawesome/free-solid-svg-icons';
import {Translate} from "react-localize-redux";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
/**
 * Handles the email, including validation with the server, and evaluates if it seems real or not.
 */
const FormGroupEmail = ({ disabled, data, isForPasswordReset, updateParent }) => {
    const emailTranslation = LanguageManager.getTranslation("register.email", Settings.LANGUAGES);
    const [email, setEmail] = useState(data ? data.email : '');
    const [emailAvailable, setEmailAvailable] = useState(true);
    const [validEmail, setValidEmail] = useState(true);
    const [isDisabled, setIsDisabled] = useState(disabled);
    const [isPasswordReset, setIsPasswordReset] = useState(typeof isForPasswordReset !== 'undefined' ? isForPasswordReset : false);

    // If the props are not null, set the values
    useEffect(() => {
        if (typeof data !== 'undefined') {
            setEmail(prev => prev || data.email); // Only set it if empty (initial mount)
            setIsDisabled(disabled);
        }
    }, []);

    // Action when the email is changed
    const handleChange = (event) => {
        const newEmail = event.currentTarget.value;
        const isValid = Validator.isEmailValid(newEmail);

        setEmail(newEmail);
        setValidEmail(isValid);

        if (isValid) {
            if (!isPasswordReset) {
                Validator.isEmailAvailable(newEmail, (returned) => {
                    setEmailAvailable(returned);
                    const state = {
                        email: newEmail,
                        emailAvailable: returned,
                        validEmail: isValid,
                        disabled: isDisabled,
                        isForPasswordReset: isPasswordReset
                    };
                    updateParent(state);
                });
            } else {
                const state = {
                    email: newEmail,
                    emailAvailable: true,
                    validEmail: isValid,
                    disabled: isDisabled,
                    isForPasswordReset: isPasswordReset
                };
                updateParent(state);
            }
        } else {
            const state = {
                email: newEmail,
                emailAvailable: emailAvailable,
                validEmail: isValid,
                disabled: isDisabled,
                isForPasswordReset: isPasswordReset
            };
            updateParent(state);
        }
    };

    // Render the form group
    return (
        <div>
            <Row>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <Input disabled={isDisabled} value={email} type="email" name="email"
                           id="email"
                           className="mantine-like__control"
                           placeholder={emailTranslation}
                           onChange={handleChange}/>
                    {
                        !validEmail || !emailAvailable ?
                            <div>
                                <Alert color="info" isOpen={!validEmail}>
                                    <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.email-error"/>
                                </Alert>
                                <Alert color="danger" isOpen={!emailAvailable}>
                                    <FontAwesomeIcon icon={faExclamationTriangle} /> {' '}<Translate id="register.email-availability-error"/>
                                </Alert>
                            </div> : null
                    }
                </Col>
            </Row>
        </div>
    );
};

export default FormGroupEmail;