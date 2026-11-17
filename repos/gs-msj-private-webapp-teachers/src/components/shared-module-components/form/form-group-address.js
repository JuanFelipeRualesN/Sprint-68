import React, { useState, useEffect, useRef } from 'react';
import {Input, Col, Row, Label, Alert} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import {Translate} from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import SelectorCountry from '../selector/selector-country';
import SelectorState from '../selector/selector-state';
import {getCountries} from "../../../api/gsweb";
import SelectorRegion from "../../admin-module/selectors/selector-region";
import {DEFAULT_COUNTRY_ISO_ADDRESS} from "../../../custom-classes/settings";

/**
 * Class to handle the address form group
 */
const FormGroupAddress = (props) => {
    const [addressLine1, setAddressLine1] = useState(props.data?.addressLine1 || '');
    const [addressLine2, setAddressLine2] = useState(props.data?.addressLine2 || '');
    const [city, setCity] = useState(props.data?.city || '');
    const [country, setCountry] = useState(props.data?.country || DEFAULT_COUNTRY_ISO_ADDRESS);
    const [state, setState] = useState(props.data?.state ?? props.data?.stateRegion ?? '');
    const [zipCode, setZipCode] = useState(props.data?.zipCode || '');
    const [regionId, setRegionId] = useState(props.data?.regionId || '');
    const [redoSearchRegion, setRedoSearchRegion] = useState(props.redoSearchRegion ?? false);
    const [disabled] = useState(props.disabled);
    const [showAddressLines] = useState(props.showAddressLines ?? true);
    const [showAddressZipCode] = useState(props.showAddressZipCode ?? true);
    const [showRegionAddress] = useState(props.showRegionAddress ?? false);
    const [stateRequired, setStateRequired] = useState(props.data?.stateRequired || false);
    const [validAddress, setValidAddress] = useState(true);
    const [validCity, setValidCity] = useState(true);
    const [validCountry, setValidCountry] = useState(true);
    const [validPhone, setValidPhone] = useState(true);
    const [validState, setValidState] = useState(true);
    const [validZipCode, setValidZipCode] = useState(true);

    const addressLine1Ref = useRef(null);
    const addressLine2Ref = useRef(null);
    const cityRef = useRef(null);
    const zipCodeRef = useRef(null);

    // FIX: load from props.data only once. Before, this ran on every keystroke
    // (the parent sends a new data object each time) and reset the state field.
    // To load a different address later, give this component a new `key` in the parent.
    const initialized = useRef(false);

    useEffect(() => {
        if (!props.data || initialized.current) return;
        initialized.current = true;

        setAddressLine1(props.data.addressLine1 || '');
        setAddressLine2(props.data.addressLine2 || '');
        setCity(props.data.city || '');
        setState(props.data.state ?? props.data.stateRegion ?? '');
        setCountry(props.data.country || DEFAULT_COUNTRY_ISO_ADDRESS);
        setRegionId(props.data.regionId || null);
        setZipCode(props.data.zipCode || '');
        setStateRequired(props.data.stateRequired || false);

        if (addressLine1Ref.current) addressLine1Ref.current.value = props.data.addressLine1 || '';
        if (addressLine2Ref.current) addressLine2Ref.current.value = props.data.addressLine2 || '';
        if (cityRef.current) cityRef.current.value = props.data.city || '';
        if (zipCodeRef.current) zipCodeRef.current.value = props.data.zipCode || '';
    }, [props.data]);

    // redoSearchRegion still follows the prop on its own
    useEffect(() => {
        setRedoSearchRegion(props.redoSearchRegion || false);
    }, [props.redoSearchRegion]);

    useEffect(() => {
        getCountry();
    }, []);

    const getCountry = () => {
        // Get countries
        let apiCall = getCountries();
        apiCall.then((response) => {});
    };

    const getCurrentState = () => {
        return {
            addressLine1,
            addressLine2,
            city,
            country,
            state,
            zipCode,
            regionId,
            redoSearchRegion,
            stateRequired,
            validAddress,
            validCity,
            validCountry,
            validPhone,
            validState,
            validZipCode,
        };
    };

    const handleAddressLine1Change = (event) => {
        const value = event.currentTarget.value;
        setAddressLine1(value);
        setRedoSearchRegion(false);

        // Check if address is valid
        const isValidAddress = Validator.isAddressValid(value);
        const isValidCity = Validator.isCityValid(city);
        const isValidCountry = Validator.isCountryValid(country);
        const isValidZipCode = Validator.isZipCodeValid(zipCode);

        setValidAddress(isValidAddress);
        setValidCity(isValidCity);
        setValidCountry(isValidCountry);
        setValidZipCode(isValidZipCode);

        const newState = {
            ...getCurrentState(),
            addressLine1: value,
            redoSearchRegion: false,
            validAddress: isValidAddress,
            validCity: isValidCity,
            validCountry: isValidCountry,
            validZipCode: isValidZipCode,
        };

        props.updateParent(newState);
    };

    const handleAddressLine2Change = (event) => {
        const value = event.currentTarget.value;
        setAddressLine2(value);
        setRedoSearchRegion(false);

        const newState = {
            ...getCurrentState(),
            addressLine2: value,
            redoSearchRegion: false,
        };

        props.updateParent(newState);
    };

    // Action when the city (address) change
    const handleCityChange = (event) => {
        const value = event.currentTarget.value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]/g, '');
        setCity(value);
        setRedoSearchRegion(false);

        // Check if address is valid
        const isValidCity = Validator.isCityValid(value);
        setValidCity(isValidCity);

        const newState = {
            ...getCurrentState(),
            city: value,
            redoSearchRegion: false,
            validCity: isValidCity,
        };

        props.updateParent(newState);
    };

    // Action when the state (address) change
    const handleStateChange = (event) => {
        const value = event.currentTarget.value;
        setState(value);
        setRedoSearchRegion(false);

        // Check if address is valid
        const isValidState = Validator.isStateValid(value);
        setValidState(isValidState);

        const newState = {
            ...getCurrentState(),
            state: value,
            redoSearchRegion: false,
            validState: isValidState,
        };

        props.updateParent(newState);
    };

    // Handles when the country is changed
    const handleCountryChange = (event) => {
        const value = event.currentTarget.value;
        setCountry(value);
        setRedoSearchRegion(true);

        // Check if address is valid
        const isValidCountry = Validator.isCountryValid(value);
        setValidCountry(isValidCountry);

        const newState = {
            ...getCurrentState(),
            country: value,
            redoSearchRegion: true,
            validCountry: isValidCountry,
        };

        props.updateParent(newState);
    };

    // Handles when the zip code is changed
    const handleZipCodeChange = (event) => {
        const value = event.currentTarget.value.replace(/\D/g, '');
        setZipCode(value);
        setRedoSearchRegion(false);

        // Check if address is valid
        const isValidZipCode = Validator.isZipCodeValid(value);
        setValidZipCode(isValidZipCode);

        const newState = {
            ...getCurrentState(),
            zipCode: value,
            redoSearchRegion: false,
            validZipCode: isValidZipCode,
        };

        props.updateParent(newState);
    };

    // Set the country selected
    const saveCountry = () => (newValue) => {
        if (newValue === null || newValue === "0") {
            setValidState(true);
            setStateRequired(false);
            setState('');
            setValidCountry(false);
            setCountry('');

            const newState = {
                ...getCurrentState(),
                validState: true,
                stateRequired: false,
                state: '',
                validCountry: false,
                country: '',
            };

            props.updateParent(newState);
            return;
        }

        if (newValue !== null && newValue !== "0") {
            // Check if country has states
            let apiCall = getCountries(newValue);
            apiCall.then((response) => {
                // If it doesn't have states
                if (!response.data[0].HasStates) {
                    const isValidCountry = Validator.isCountryValid(newValue);
                    setValidState(true);
                    setStateRequired(false);
                    setState('');
                    setValidCountry(isValidCountry);
                    setCountry(newValue);

                    const newState = {
                        ...getCurrentState(),
                        validState: true,
                        stateRequired: false,
                        state: '',
                        validCountry: isValidCountry,
                        country: newValue,
                    };

                    props.updateParent(newState);
                } else {
                    const isValidCountry = Validator.isCountryValid(newValue);
                    setStateRequired(response.data[0].HasStates);
                    setValidCountry(isValidCountry);
                    setState('');
                    setValidState(false);
                    setCountry(newValue);

                    const newState = {
                        ...getCurrentState(),
                        stateRequired: response.data[0].HasStates,
                        validCountry: isValidCountry,
                        state: '',
                        validState: false,
                        country: newValue,
                    };

                    props.updateParent(newState);
                }
            });
        }
    };

    // Saves the state province when change
    const saveStateProvince = () => (newValue) => {
        setState(newValue);
        const isValidState = Validator.isStateValid(newValue);
        setValidState(isValidState);

        const newState = {
            ...getCurrentState(),
            state: newValue,
            validState: isValidState,
        };

        props.updateParent(newState);
    };

    const saveRegion = (e) => {
        setRegionId(e);
    };

    // Render the form group
    return (
        <div>
            {showAddressLines ?
                <Row>
                    <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="address" className="label-forms-blue">
                            * <Translate id="register.address" options={{renderInnerHtml: true}}/>
                        </Label>
                        <Input
                            className="mantine-like__control"
                            ref={addressLine1Ref}
                            disabled={disabled}
                            defaultValue={addressLine1}
                            type="text"
                            name="address"
                            id="address"
                            onChange={handleAddressLine1Change}
                        />
                    </Col>
                    <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Input
                            className="mantine-like__control"
                            ref={addressLine2Ref}
                            disabled={disabled}
                            defaultValue={addressLine2}
                            type="text"
                            name="address-line-2"
                            id="address-line-2"
                            onChange={handleAddressLine2Change}
                        />
                    </Col>
                    {!validAddress ?
                        <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <Alert color="info" isOpen={!validAddress}>
                                <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.address-line-1-error"/>
                            </Alert>
                        </Col> : null
                    }
                </Row> : null}
            <Row>
                <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                    <Label for="city" className="label-forms-blue">
                        * <Translate id="register.city"/>
                    </Label>
                    <Input
                        className="mantine-like__control"
                        ref={cityRef}
                        disabled={disabled}
                        value={city}
                        type="text"
                        name="city"
                        id="city"
                        onChange={handleCityChange}
                    />
                    <br/>
                    {!validCity ?
                        <Alert color="info" isOpen={!validCity}>
                            <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.city-error"/>
                        </Alert> : null
                    }
                </Col>
                <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                    { stateRequired ?
                        <Label for="state" className="label-forms-blue">
                            * <Translate id="register.state"/>
                        </Label> : null
                    }
                    <SelectorState disabled={disabled} data={getCurrentState()} value={state} updateParent={saveStateProvince("state")}/>
                    <br/>
                    {!validState ?
                        <Alert color="danger" isOpen={!validState}>
                            <Translate id="register.state-error"/>
                        </Alert> : null
                    }
                </Col>
            </Row>

            <Row>
                <Col sm={{size: 12}} md={{size: 8}} lg={{size: 7}}>
                    <Label for="country" className="label-forms-blue">
                        * <Translate id="register.country"/>
                    </Label>
                    <SelectorCountry disabled={disabled} data={country} value={country}
                                     updateParent={saveCountry("country")}/>
                    <br/>
                    {!validCountry ?
                        <Alert color="info" isOpen={!validCountry}>
                            <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.country-error"/>
                        </Alert> : null
                    }
                </Col>
                {showRegionAddress && country !== '' ?
                    <SelectorRegion countryIso={country} disabled={disabled} redoSearchRegion={redoSearchRegion} value={regionId} updateParent={saveRegion} /> : null
                }

                {showAddressZipCode ?
                    <Col sm={{size: 12}} md={{size: 4}} lg={{size: 5}}>
                        <Label for="zip-code" className="label-forms-blue">
                            * <Translate id="register.zip-code"/>
                        </Label>
                        <Input
                            className="mantine-like__control"
                            ref={zipCodeRef}
                            disabled={disabled}
                            value={zipCode}
                            type="text"
                            name="zip-code"
                            id="zip-code"
                            onChange={handleZipCodeChange}
                        />
                        <br/>
                        {!validZipCode ?
                            <Alert color="info" isOpen={!validZipCode}>
                                <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="register.zip-code-error"/>
                            </Alert> : null }
                    </Col> : null
                }
            </Row>
        </div>
    );
};

export default FormGroupAddress;
