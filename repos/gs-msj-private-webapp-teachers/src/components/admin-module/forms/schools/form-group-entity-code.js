/**
 * Created by portiz on 4/15/2020.
 */
import React, { useState, useEffect, useRef } from 'react';
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
const FormGroupEntityCode = ({ disabled, entityCode, initialCode, updateParent }) => {
    const [entityCodeState, setEntityCodeState] = useState(entityCode);
    const [initialCodeState, setInitialCodeState] = useState(initialCode);
    const [validCode, setValidCode] = useState(true);
    const inputRef = useRef(null);
    const timeoutRef = useRef(null);

    useEffect(() => {
        if (initialCode !== initialCodeState) {
            setInitialCodeState(initialCode);
        }
    }, [initialCode]);

    useEffect(() => {
        if (entityCode && inputRef.current) {
            inputRef.current.value = entityCode;
        }
    }, [entityCode]);

    // Action when the first name is typed
    const handleChange = (event) => {
        const newEntityCode = event.currentTarget.value;

        // Clear previous timeout
        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
        }
        
        // Use timeout to synchronize states
        timeoutRef.current = setTimeout(() => {
        setEntityCodeState(newEntityCode);

        if (newEntityCode.trim().length === 0) {
            setValidCode(false);
            updateParent({
                disabled,
                entityCode: newEntityCode,
                initialCode: initialCodeState,
                validCode: false,
            });
            return;
        }

        if (newEntityCode !== initialCodeState) {
            // Check if School code is available
            setValidCode(Validator.isSchoolCodeAvailable(
                newEntityCode,
                (returned) => {
                    setValidCode(returned);
                    updateParent({
                        disabled,
                        entityCode: newEntityCode,
                        initialCode: initialCodeState,
                        validCode: returned,
                    });
                }
            ));
        } else {
            updateParent({
                disabled,
                entityCode: newEntityCode,
                initialCode: initialCodeState,
                validCode,
            });
        }
        }, 100);    
    };

    // Render the form group
    return (
        <FormGroup>
            <Label for="name">
                * <Translate id="school-module.invitation-code" />
            </Label>
            <Input
                ref={inputRef}
                disabled={disabled}
                defaultValue={entityCode || ""}
                type="text"
                name="code"
                id="code"
                onChange={handleChange}
            />
            {/* <br/> */}
            {typeof validCode !== "undefined" &&
            validCode !== undefined &&
            !validCode ? (
                <Alert color="info" isOpen={!validCode}>
                    <FontAwesomeIcon icon={faInfoCircle} />{" "}
                    <Translate id="school-module.message-code-not-available" />
                </Alert>
            ) : null}
        </FormGroup>
    );
};

export default FormGroupEntityCode;
