/**
 * Created by portiz on 4/15/2020.
 */
import React, { useState, useEffect } from 'react';
import {Input, Label, FormGroup, Alert} from 'reactstrap';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Handles the form group of the person name
 */
const FormGroupPublicPrivate = ({ disabled, isRegisterForm = false, privateOrPublic, updateParent }) => {
    const [privateOrPublicState, setPrivateOrPublicState] = useState( privateOrPublic ?? 0);
    const [showRegisterHomeSchoolError, setShowRegisterHomeSchoolError] = useState(false);
    const [validSelection, setValidSelection] = useState(true);

    useEffect(() => {
        if(typeof privateOrPublic !== 'undefined' && privateOrPublic !== null) {
            setPrivateOrPublicState(privateOrPublic);
        }
    }, [privateOrPublic]);

    // Action when the first name is typed
    const handleOnChange = (event) => {
        const value = event.currentTarget.value;
        
        if(value === "2") {
            setPrivateOrPublicState(value);
            setShowRegisterHomeSchoolError(true);
            setValidSelection(false);
            updateParent(value, false);
        } else {
            setPrivateOrPublicState(value);
            setShowRegisterHomeSchoolError(false);
            setValidSelection(true);
            updateParent(value, true);
        }
    };

    // Render the form group
    return (
        <FormGroup check>
            <Label check>
                <Input type="radio" name="private-public" value={"1"}
                       checked={privateOrPublicState == "1"}
                       onChange={handleOnChange}/>{' '}
                <Translate id="school-module.private"/>
            </Label>
            {/* <br/> */}
            <Label check>
                <Input type="radio" checked={privateOrPublicState == "0"} name="private-public" value={"0"} onChange={handleOnChange}/>{' '}
                <Translate id="school-module.public"/>
            </Label>
            {/* <br/> */}
            {isRegisterForm ?
                <Label check>
                    <Input type="radio" name="private-public" value={"2"} onChange={handleOnChange}/>{' '}
                    <Translate id="school-module.homeschooling"/>
                </Label>
                : null }
            {
                showRegisterHomeSchoolError ?
                    <Alert color="primary">
                        <Translate id="school-module.homeschooling-note" options={{renderInnerHtml: true}}/>
                    </Alert> : null
            }
        </FormGroup>
    );
};

export default FormGroupPublicPrivate;