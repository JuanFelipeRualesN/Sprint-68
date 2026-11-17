/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import { Input, Label, Alert, FormGroup } from "reactstrap";
import Validator from "../../../../custom-classes/validator";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faInfoCircle } from "@fortawesome/free-solid-svg-icons";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Handles the form group of the person name
 */
const FormGroupNewSchoolName = ({ disabled, entityName, updateParent }) => {
    const [entityNameState, setEntityNameState] = useState(entityName);
    const [validName, setValidName] = useState(true);

    useEffect(() => {
        if (entityName !== entityNameState) {
            setEntityNameState(entityName);
        }
    }, [entityName]);

    // Action when the first name is typed
    const handleNameChange = (event) => {
        const newEntityName = event.currentTarget.value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]/g, '');
        setEntityNameState(newEntityName);
        
        // Check if the school name is valid 
        const isValid = Validator.isFirstNameValid(newEntityName);
        setValidName(isValid);
        
        updateParent({
            disabled,
            entityName: newEntityName,
            validName: isValid,
        });
    };

    // Render the form group
    return (
        <FormGroup>
            <Label for="name">
                * <Translate id="school-module.name" />
            </Label>
            <Input
                disabled={disabled}
                value={entityNameState}
                type="text"
                name="name"
                id="name"
                onChange={handleNameChange}
            />
            {/* <br/> */}
            {!validName ? (
                <Alert color="info" isOpen={!validName}>
                    <FontAwesomeIcon icon={faInfoCircle} />{" "}
                    <Translate id="school-module.message-name-required" />
                </Alert>
            ) : null}
        </FormGroup>
    );
}

export default FormGroupNewSchoolName;
