import React, { useState, useEffect } from "react";
import {Input, Col, Row, InputGroup,  InputGroupText} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import {FaUser} from "react-icons/all";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the form group of the person name
 */
const FormGroupNameTutorV2 = (props) => {
    const [disabled, setDisabled] = useState(props.disabled);
    const [firstName, setFirstName] = useState(props.data ? props.firstName : "");
    const [lastName, setLastName] = useState(props.data ? props.lastName : "");
    const [validFirstName, setValidFirstName] = useState(true);
    const [validLastName, setValidLastName] = useState(true);

    const nameAdministratorTranslation = LanguageManager.getTranslation("register-v2.name-administrator",Settings.LANGUAGES);
    const lastNameAdministratorTranslation = LanguageManager.getTranslation("register-v2.last-name-administrator",Settings.LANGUAGES);

    
    // If the props are not null, set the values
    useEffect(() => {
        setDisabled(props.disabled);
        if (typeof props.data !== "undefined") {
        setFirstName(props.firstName);
        setLastName(props.lastName);
        }
    }, [props.disabled, props.data, props.firstName, props.lastName]);

    // Action when the first name is typed
    const handleFirstNameChange = (event) => {
        setFirstName(event.currentTarget.value);
        // Check if password are equals and show error if not
        setValidFirstName(Validator.isFirstNameValid(event.currentTarget.value));
        setValidLastName(Validator.isLastNameValid(lastName));
        props.updateParent({
            disabled,
            firstName: event.currentTarget.value,
            lastName,
            validFirstName: Validator.isFirstNameValid(event.currentTarget.value),
            validLastName: Validator.isLastNameValid(lastName),
        });
    }; // This has a callback to evaluate the warning. If we don't use a callback, is not going to read the
    // last character while we are checking if they are equal or not

    // Handles when the password retype is changed
    const handleLastNameChange = (event) => {
        setLastName(event.currentTarget.value);
        // Check if password are equals and show error if not
        setValidFirstName(Validator.isLastNameValid(firstName));
        setValidLastName(Validator.isFirstNameValid(event.currentTarget.value));
        props.updateParent({
            disabled,
            firstName,
            lastName: event.currentTarget.value,
            validFirstName: Validator.isLastNameValid(firstName),
            validLastName: Validator.isFirstNameValid(event.currentTarget.value),
        });
    }; // This has a callback to evaluate the warning. If we don't use a callback, is not going to read the
    // last character while we are checking if they are equal or not

    // Render the form group
    return (
        <div>
            <Row>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 6}}>
                    <InputGroup>
                        <Input disabled={disabled} value={firstName} type="text" name="firstname"
                                id="firstname"
                               className="mantine-like__control"
                                placeholder={nameAdministratorTranslation}
                                onChange={handleFirstNameChange}/>
                        <div className="input-group-append">
                            <InputGroupText><FaUser /></InputGroupText>
                        </div>
                    </InputGroup>
                </Col>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 6}}>
                    <InputGroup>
                        <Input
                            disabled={disabled}
                            value={lastName}
                            className="mantine-like__control"
                            type="text"
                            name="lastname"
                            placeholder={lastNameAdministratorTranslation}
                            id="lastname" onChange={handleLastNameChange}/>
                        <div className="input-group-append">
                            <InputGroupText><FaUser /></InputGroupText>
                        </div>
                    </InputGroup>
                </Col>
            </Row>
        </div>
    )
};

export default FormGroupNameTutorV2;