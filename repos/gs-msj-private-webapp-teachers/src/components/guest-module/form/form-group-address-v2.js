import React, { useState, useEffect } from "react";
import { Col, Row, Alert} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import {Translate} from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import SelectorCountry from '../../shared-module-components/selector/selector-country';
import SelectorState from '../../shared-module-components/selector/selector-state';
import {getCountries} from "../../../api/gsweb";

/**
 * Class to handle the address form group
 */
const FormGroupAddress = (props) => {
    const [addressLine1, setAddressLine1] = useState(props.data ? props.data.addressLine1 : "");
    const [addressLine2, setAddressLine2] = useState(props.data ? props.data.addressLine2 : "");
    const [city, setCity] = useState(props.data ? props.data.city : "");
    const [country, setCountry] = useState(props.data ? props.data.country : "US");
    const [disabled, setDisabled] = useState(props.disabled);
    const [stateValue, setStateValue] = useState(props.data ? (props.data.state === null ? "" : props.data.state) : "");
    const [stateRequired, setStateRequired] = useState(props.data ? props.data.stateRequired : false);
    const [validAddress, setValidAddress] = useState(true);
    const [validCity, setValidCity] = useState(true);
    const [validCountry, setValidCountry] = useState(true);
    const [validPhone, setValidPhone] = useState(true);
    const [validState, setValidState] = useState(true);
    const [validZipCode, setValidZipCode] = useState(true);
    const [zipCode, setZipCode] = useState(props.data ? props.data.zipCode : "");

    useEffect(() => {
        if (typeof props.data !== "undefined") {
        setAddressLine1(props.data.addressLine1);
        setAddressLine2(props.data.addressLine2);
        setCity(props.data.city);
        setCountry(props.data.country);
        setDisabled(props.disabled);
        setStateValue(props.data.state === null ? "" : props.data.state);
        setStateRequired(props.data.stateRequired);
        setZipCode(props.data.zipCode);
        }
    }, [props.data]);

    // Set the country selected
    const saveCountry = () => (newValue) => {
        if (newValue === null || newValue === "0") {
            setValidState(true);
            setStateRequired(false);
            setStateValue("");
            setValidCountry(false);
            setCountry("");
            props.updateParent({
                addressLine1,
                addressLine2,
                city,
                country: "",
                state: "",
                stateRequired: false,
                zipCode,
                validAddress,
                validCity,
                validCountry: false,
                validState: true,
                validZipCode,
                validPhone,
            });
        } else {
            let apiCall = getCountries(newValue);
            // Check if country has states
            apiCall.then((response) => {
                // If it doesn't have states
                if (!response.data[0].HasStates) {
                    setValidState(true);
                    setStateRequired(false);
                    setStateValue("");
                    setValidCountry(Validator.isCountryValid(newValue));
                    setCountry(newValue);
                    props.updateParent({
                        addressLine1,
                        addressLine2,
                        city,
                        country: newValue,
                        state: "",
                        stateRequired: false,
                        zipCode,
                        validAddress,
                        validCity,
                        validCountry: Validator.isCountryValid(newValue),
                        validState: true,
                        validZipCode,
                        validPhone,
                    });
                } else {
                    setStateRequired(response.data[0].HasStates);
                    setValidCountry(Validator.isCountryValid(newValue));
                    setStateValue("");
                    setValidState(false);
                    setCountry(newValue);
                    props.updateParent({
                        addressLine1,
                        addressLine2,
                        city,
                        country: newValue,
                        state: "",
                        stateRequired: response.data[0].HasStates,
                        zipCode,
                        validAddress,
                        validCity,
                        validCountry: Validator.isCountryValid(newValue),
                        validState: false,
                        validZipCode,
                        validPhone,
                    });
                }
            });
        }
    };

    // Saves the state province when change
    const saveStateProvince = () => (newValue) => {
        setStateValue(newValue);
        const isValid = Validator.isStateValid(newValue);
        setValidState(isValid);
        props.updateParent({
            addressLine1,
            addressLine2,
            city,
            country,
            state: newValue,
            stateRequired,
            zipCode,
            validAddress,
            validCity,
            validCountry,
            validState: isValid,
            validZipCode,
            validPhone,
        });
    };

    // Render the form group
    return (
        <div>
            <Row>
                <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                    <SelectorCountry disabled={disabled} data={country} value={country}
                                        updateParent={saveCountry("country")}/>
                </Col>
                <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                    <SelectorState 
                        disabled={disabled} 
                        data={{
                            addressLine1,
                            addressLine2,
                            city,
                            country,
                            state: stateValue,
                            stateRequired,
                            zipCode,
                            validAddress,
                            validCity,
                            validCountry,
                            validState,
                            validZipCode,
                            validPhone,
                        }}
                        value={stateValue} 
                        updateParent={saveStateProvince("state")}/>
                </Col>
            </Row>
            <Row>
                <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                    {!validCountry ?
                        <Alert color="info" isOpen={!validCountry}>
                            <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.country-error"/>
                        </Alert> : null
                    }
                </Col>
                <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                    {!validState ?
                        <Alert color="danger" isOpen={!validState}>
                            <Translate id="register.state-error"/>
                        </Alert> : null
                    }
                </Col>
            </Row>
        </div>
    )
};


export default FormGroupAddress;