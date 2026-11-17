import React, { useState, useEffect } from "react";
import {Input, Alert, InputGroup, InputGroupText, Row} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import {Translate} from "react-localize-redux";
import LanguageManager from "../../../custom-classes/language-manager";
import {FaPhone} from "react-icons/all";
import Settings from "../../../custom-classes/settings";

/**
 * The form group to manage the phone.
 * Includes an alert to validates the phone and the main function to update the parent phone
 */
const FormGroupPhoneV2 = (props) => {
    const [disabled, setDisabled] = useState(props.disabled);
    const [phone, setPhone] = useState(props.data ? props.data.phone : "");
    const [validPhone, setValidPhone] = useState(true);
    const phoneTranslation = LanguageManager.getTranslation("register-v2.phone", Settings.LANGUAGES);

    // If the props are not null, set the values
    useEffect(() => {
        setDisabled(props.disabled);
        if (typeof props.data !== 'undefined') {
            setPhone(props.data.phone);
        }
    }, [props.disabled, props.data]);

     // Handles the phone change and send the phone to the parent
    const handlePhoneChange = (event) => {
        const rawValue = event.currentTarget.value;

        // Remove non-digit characters
        const digitsOnly = rawValue.replace(/\D/g, '');

        setPhone(digitsOnly);

        // Check if phone is valid
        const isValid = Validator.isPhoneValid(digitsOnly);
        setValidPhone(isValid);

        props.updateParent({ disabled, phone: digitsOnly, validPhone: isValid });
    };
    
    //Renders the phone group
    return (
        <>
        <Row>
            <InputGroup>
                <Input disabled={disabled} value={phone} type="tel" name="phone"
                       id="phone"
                       maxLength={15}
                       placeholder={phoneTranslation}
                       onChange={handlePhoneChange}/>
                <div className="input-group-append">
                    <InputGroupText><FaPhone /></InputGroupText>
                </div>
            </InputGroup>
        </Row>
            <Row>
                {
                    !validPhone ?
                        <Alert color="danger" isOpen={!validPhone}>
                            <Translate id="register.phone-error" options={{renderInnerHtml: true}}/>
                        </Alert> : null
                }
            </Row>
        </>
    )
};

export default FormGroupPhoneV2;