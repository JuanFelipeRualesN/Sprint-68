import React, { useEffect, useState } from 'react';
import { Input, Col, Label, Alert, Row } from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import { Translate } from "react-localize-redux";
import { convertMaxLengthSting } from "../../../utils/utils";

/**
 * Handle the class to a new password reset
 */
const FormGroupNewPassword = ({ data, disabled, updateParent }) => {
    const [password, setPassword] = useState(data?.password || '');
    const [passwordRetype, setPasswordRetype] = useState(data?.passwordRetype || '');
    const [validPassword, setValidPassword] = useState(true);

    useEffect(() => {
        if (data) {
        setPassword(data.password || '');
        setPasswordRetype(data.passwordRetype || '');
        }
    }, [data]);

    // Validate the password and update the parent component
    const validateAndUpdate = (password, retypePassword) => {
        const isValid = Validator.isPasswordValid(retypePassword, password);
        setValidPassword(isValid);
        updateParent({
        password: password,
        passwordRetype: retypePassword,
        validPassword: isValid,
        disabled,
        });
    };

    // Action when the passwords are typed
    const handleChangePassword = (event) => {
        const value = convertMaxLengthSting(event.currentTarget.value, 20);
        setPassword(value);
        validateAndUpdate(value, passwordRetype);
    };

    // Action when the password is retyped
    const handleChangePasswordRetype = (event) => {
        const value = convertMaxLengthSting(event.currentTarget.value, 20);
        setPasswordRetype(value);
        validateAndUpdate(password, value);
    };

    return (
        <div>
            <Row>
                <Col sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
                    <Label for="password" className="label-forms-blue">
                        * <Translate id="register.password" />
                    </Label>
                    <Input disabled={disabled} value={password} type="password"
                           className="mantine-like__control"
                        name="password"
                        id="password"
                        onChange={handleChangePassword} />
                </Col>
                <Col sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
                    <Label for="retype_password" className="label-forms-blue">
                        * <Translate id="register.retype-password" />
                    </Label>
                    <Input disabled={disabled} value={passwordRetype} type="password"
                           className="mantine-like__control"
                        name="retype_password"
                        id="retype_password"
                        onChange={handleChangePasswordRetype} />
                </Col>
                <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>

                    {!validPassword ?
                        <Alert color="info" isOpen={!validPassword}>
                            <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.password-error" />
                        </Alert> : null
                    }
                </Col>
            </Row>
        </div>
    )
};

export default FormGroupNewPassword;