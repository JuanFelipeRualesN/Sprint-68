import React, { useState, useEffect, useRef } from 'react';
import {Input, Row, Col, Label, Alert} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle, faExclamationTriangle } from '@fortawesome/free-solid-svg-icons';
import {Translate} from "react-localize-redux";

/**
 * Handles the email, including validation with the server, and evaluates if it seems real or not.
 */
const FormGroupEmail = ({data, disabled, required = true, isForPasswordReset = false, updateParent}) => {
    const [email, setEmail] = useState('');
    const [validEmail, setValidEmail] = useState(true);
    const [emailAvailable, setEmailAvailable] = useState(true);
    const emailRef = useRef('');

    useEffect(() => {
        if (data) {
            setEmail(data.email);
            emailRef.current = data.email;
        }
    }, [data]);

    // Action when the email is changed
    const handleChange = (event) => {
        const value = event.currentTarget.value;
        const isValid = Validator.isEmailValid(value);
        emailRef.current = value;

        setEmail(value);
        setValidEmail(isValid);

        const state = {
            email: value,
            validEmail: isValid,
            emailAvailable,
            disabled,
            isForPasswordReset,
            required
        };

        if (!isValid) {
            updateParent(state);
            return;
        }

        updateParent(state);

        if (!isForPasswordReset) {
            Validator.isEmailAvailable(value, (returned) => {
                if (emailRef.current === value) {
                    setEmailAvailable(returned);
                    updateParent({
                        ...state,
                        emailAvailable: returned
                    });
                }
            });
        } else {
            updateParent(state);
        }
    };

    return (
        <div>
            <Row>
                <Col sm="12">
                    <Label for="email" className="label-forms-blue">
                        {required && '* '}<Translate id="register.email" />
                    </Label>
                    <Input
                        className="mantine-like__control"
                        disabled={disabled}
                        value={email}
                        type="email"
                        name="email"
                        id="email"
                        onChange={handleChange}
                    />
                    {!required && (
                        <small>
                            <Translate id="register.email-optional-note" />
                        </small>
                    )}
                    <br />
                    {(!validEmail || !emailAvailable) && (
                        <div>
                            {!validEmail && (
                                <Alert color="info">
                                    <FontAwesomeIcon icon={faInfoCircle} />{' '}
                                    <Translate id="register.email-error" />
                                </Alert>
                            )}
                            {!emailAvailable && (
                                <Alert color="danger">
                                    <FontAwesomeIcon icon={faExclamationTriangle} />{' '}
                                    <Translate id="register.email-availability-error" />
                                </Alert>
                            )}
                        </div>
                    )}
                </Col>
            </Row>
        </div>
    );
};

export default FormGroupEmail;