/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from "react";
import {Input, Col, Alert, Row, InputGroup,  InputGroupText} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import {FaLock} from "react-icons/all";
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
import LanguageManager from "../../../custom-classes/language-manager";
/** End of Multiple Language imports **/
import Settings from "../../../custom-classes/settings";
/**
 * Handle the class to a new password reset
 */
const FormGroupNewPasswordStudentV2 = (props) => {
    const [disabled, setDisabled] = useState(props.disabled);
    const [password, setPassword] = useState(props.data ? props.data.password : "");
    const [passwordRetype, setPasswordRetype] = useState(props.data ? props.data.passwordRetype : "");
    const [validPassword, setValidPassword] = useState(true);

    const passwordStudentTranslation = LanguageManager.getTranslation("register-v2.password-student", Settings.LANGUAGES);

    //If the props are not null, set the values
    useEffect(() => {
        setDisabled(props.disabled);
        if (typeof props.password !== 'undefined') {
            setPassword(props.password);
            setPasswordRetype(props.passwordRetype);
        }
    }, []);


    // Action when the passwords are typed
    const handleChangePassword = (event) => {
        setPassword(event.currentTarget.value);
        // Check if password are equals and show error if not
        setValidPassword(Validator.isPasswordValid(event.currentTarget.value, event.currentTarget.value));
        props.updateParent({
            disabled,
            password: event.currentTarget.value,
            passwordRetype,
            validPassword: Validator.isPasswordValid(event.currentTarget.value, event.currentTarget.value),
        });
    }; // This has a callback to evaluate the warning. If we don't use a callback, is not going to read the
    // last character while we are checking if they are equal or not

    // Render the form group
    return (
        <div>
            <Row>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <InputGroup>
                        <Input disabled={disabled} value={password} type="password"
                                name="password"
                                id="password"
                                placeholder={passwordStudentTranslation}
                                onChange={handleChangePassword}/>
                        <div className="input-group-append">
                            <InputGroupText><FaLock /></InputGroupText>
                        </div>
                    </InputGroup>
                </Col>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>

                    {!validPassword ?
                        <Alert color="info" isOpen={!validPassword}>
                            <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.password-error"/>
                        </Alert> : null
                    }
                </Col>
            </Row>
        </div>
    )
};

export default FormGroupNewPasswordStudentV2;