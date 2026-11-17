/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState } from 'react';
import { Input, Col, Row, Label, Alert } from "reactstrap";
import Validator from "../../../custom-classes/validator";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faInfoCircle } from "@fortawesome/free-solid-svg-icons";
/** End of Multiple Language imports **/
import SelectorCountry from "../selector/selector-country";
import SelectorState from "../selector/selector-state";
import { getCountries } from "../../../api/gsweb";
import SelectorRegion from "../../admin-module/selectors/selector-region";

/**
 * Class to handle the address form group
 */
const FormGroupAddressV2 = ({addressLine1, addressLine2, city, country, disabled, showAddressLines, showAddressZipCode, showRegionAddress,
                          region, state: stateProp, stateRequired, validAddress, validCity, validCountry, validPhone, validState: validStateProp,
                          validZipCode, zipCode, updateParent}) => {
  const [addressLine1State, setAddressLine1State] = useState(addressLine1 ?? "");
  const [addressLine2State, setAddressLine2State] = useState(addressLine2 ?? "");
  const [cityState, setCityState] = useState(city ?? "");
  const [countryState, setCountryState] = useState(country ?? "PR"); // Default PR
  const [showAddressLinesState, setShowAddressLinesState] = useState(showAddressLines ?? true);
  const [showAddressZipCodeState, setShowAddressZipCodeState] = useState(showAddressZipCode ?? true);
  const [showRegionAddressState, setShowRegionAddressState] = useState(showRegionAddress ?? false);
  const [regionState, setRegionState] = useState(region ?? "");
  const [state, setState] = useState(stateProp ?? "");
  const [stateRequiredState, setStateRequiredState] = useState(stateRequired ?? false);
  const [validAddressState, setValidAddressState] = useState(validAddress ?? true);
  const [validCityState, setValidCityState] = useState(validCity ?? true);
  const [validCountryState, setValidCountryState] = useState(validCountry ?? true);
  const [validPhoneState, setValidPhoneState] = useState(validPhone ?? true);
  const [validState, setValidState] = useState(validStateProp ?? true);
  const [validZipCodeState, setValidZipCodeState] = useState(validZipCode ?? true);
  const [zipCodeState, setZipCodeState] = useState(zipCode ?? "");
  const [redoSearchRegionState, setRedoSearchRegionState] = useState(false);

  const handleAddressLine1Change = (event) => {
    const newAddressLine1 = event.currentTarget.value;
    const newValidAddress = Validator.isAddressValid(newAddressLine1);
    const newValidCity = Validator.isCityValid(cityState);
    const newValidCountry = Validator.isCountryValid(countryState);
    const newValidZipCode = Validator.isZipCodeValid(zipCodeState);
    
    setAddressLine1State(newAddressLine1);
    setValidAddressState(newValidAddress);
    setValidCityState(newValidCity);
    setValidCountryState(newValidCountry);
    setValidZipCodeState(newValidZipCode);
    setRedoSearchRegionState(false);
    
    updateParent({
      addressLine1: newAddressLine1,
      addressLine2: addressLine2State,
      city: cityState,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: state,
      stateRequired: stateRequiredState,
      validAddress: newValidAddress,
      validCity: newValidCity,
      validCountry: newValidCountry,
      validPhone: validPhoneState,
      validStateProp: validState,
      validZipCode: newValidZipCode,
      zipCode: zipCodeState,
      redoSearchRegion: false
    });
  };

  const handleAddressLine2Change = (event) => {
    const newAddressLine2 = event.currentTarget.value;
    
    setAddressLine2State(newAddressLine2);
    setRedoSearchRegionState(false);

    updateParent({
      addressLine1: addressLine1State,
      addressLine2: newAddressLine2,
      city: cityState,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: state,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: validCityState,
      validCountry: validCountryState,
      validPhone: validPhoneState,
      validStateProp: validState,
      validZipCode: validZipCodeState,
      zipCode: zipCodeState,
      redoSearchRegion: false
    });
  };

  const handleCityChange = (event) => {
    const newCity = event.currentTarget.value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]/g, '');
    const newValidCity = Validator.isCityValid(newCity);
    
    setCityState(newCity);
    setValidCityState(newValidCity);
    setRedoSearchRegionState(false);
    
    updateParent({
      addressLine1: addressLine1State,
      addressLine2: addressLine2State,
      city: newCity,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: state,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: newValidCity,
      validCountry: validCountryState,
      validPhone: validPhoneState,
      validStateProp: validState,
      validZipCode: validZipCodeState,
      zipCode: zipCodeState,
      redoSearchRegion: false
    });
  };

  const handleStateChange = (event) => {
    const newState = event.currentTarget.value;
    const newValidState = Validator.isStateValid(newState);
    
    setState(newState);
    setValidState(newValidState);
    setRedoSearchRegionState(false);
    
    updateParent({
      addressLine1: addressLine1State,
      addressLine2: addressLine2State,
      city: cityState,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: newState,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: validCityState,
      validCountry: validCountryState,
      validPhone: validPhoneState,
      validStateProp: newValidState,
      validZipCode: validZipCodeState,
      zipCode: zipCodeState,
      redoSearchRegion: false
    });
  };

  const handleCountryChange = (event) => {
    const newCountry = event.currentTarget.value;
    const newValidCountry = Validator.isCountryValid(newCountry);
    
    setCountryState(newCountry);
    setValidCountryState(newValidCountry);
    setRedoSearchRegionState(true);
    
    updateParent({
      addressLine1: addressLine1State,
      addressLine2: addressLine2State,
      city: cityState,
      country: newCountry,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: state,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: validCityState,
      validCountry: newValidCountry,
      validPhone: validPhoneState,
      validStateProp: validState,
      validZipCode: validZipCodeState,
      zipCode: zipCodeState,
      redoSearchRegion: true
    });
  };

  const handleZipCodeChange = (event) => {
    const newZipCode = event.currentTarget.value.replace(/[^0-9]/g, '');
    const newValidZipCode = Validator.isZipCodeValid(newZipCode);
    
    setZipCodeState(newZipCode);
    setValidZipCodeState(newValidZipCode);
    setRedoSearchRegionState(false);
    
    updateParent({
      addressLine1: addressLine1State,
      addressLine2: addressLine2State,
      city: cityState,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: state,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: validCityState,
      validCountry: validCountryState,
      validPhone: validPhoneState,
      validStateProp: validState,
      validZipCode: newValidZipCode,
      zipCode: newZipCode,
      redoSearchRegion: false
    });
  };

  const saveCountry = () => (newValue) => {
    if (newValue === null || newValue === "0") {
      setValidState(true);
      setStateRequiredState(false);
      setState("");
      setRedoSearchRegionState(true);
      setRegionState("");
      setValidCountryState(false);
      setCountryState("");
      
      updateParent({
        addressLine1: addressLine1State,
        addressLine2: addressLine2State,
        city: cityState,
        country: "",
        disabled,
        showAddressLines: showAddressLinesState,
        showAddressZipCode: showAddressZipCodeState,
        showRegionAddress: showRegionAddressState,
        region: "",
        stateProp: "",
        stateRequired: false,
        validAddress: validAddressState,
        validCity: validCityState,
        validCountry: false,
        validPhone: validPhoneState,
        validStateProp: true,
        validZipCode: validZipCodeState,
        zipCode: zipCodeState,
        redoSearchRegion: true
      });
      return;
    }

    if (newValue !== null && newValue !== "0") {
      let apiCall = getCountries(newValue);
      apiCall.then((response) => {
        if (!response.data[0].HasStates) {
          setValidState(true);
          setStateRequiredState(false);
          setState("");
          setRedoSearchRegionState(true);
          setValidCountryState(Validator.isCountryValid(newValue));
          setCountryState(newValue);
          
          updateParent({
            addressLine1: addressLine1State,
            addressLine2: addressLine2State,
            city: cityState,
            country: newValue,
            disabled,
            showAddressLines: showAddressLinesState,
            showAddressZipCode: showAddressZipCodeState,
            showRegionAddress: showRegionAddressState,
            region: regionState,
            stateProp: "",
            stateRequired: false,
            validAddress: validAddressState,
            validCity: validCityState,
            validCountry: Validator.isCountryValid(newValue),
            validPhone: validPhoneState,
            validStateProp: true,
            validZipCode: validZipCodeState,
            zipCode: zipCodeState,
            redoSearchRegion: true
          });
        } else {
          setStateRequiredState(response.data[0].HasStates);
          setValidCountryState(Validator.isCountryValid(newValue));
          setState("");
          setRedoSearchRegionState(true);
          setValidState(false);
          setCountryState(newValue);
          
          updateParent({
            addressLine1: addressLine1State,
            addressLine2: addressLine2State,
            city: cityState,
            country: newValue,
            disabled,
            showAddressLines: showAddressLinesState,
            showAddressZipCode: showAddressZipCodeState,
            showRegionAddress: showRegionAddressState,
            region: regionState,
            stateProp: "",
            stateRequired: response.data[0].HasStates,
            validAddress: validAddressState,
            validCity: validCityState,
            validCountry: Validator.isCountryValid(newValue),
            validPhone: validPhoneState,
            validStateProp: false,
            validZipCode: validZipCodeState,
            zipCode: zipCodeState,
            redoSearchRegion: true
          });
        }
      });
    }
  };

  const saveStateProvince = () => (newValue) => {
    const newValidState = Validator.isStateValid(newValue);
    
    setState(newValue);
    setValidState(newValidState);
    
    updateParent({
      addressLine1: addressLine1State,
      addressLine2: addressLine2State,
      city: cityState,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: regionState,
      stateProp: newValue,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: validCityState,
      validCountry: validCountryState,
      validPhone: validPhoneState,
      validStateProp: newValidState,
      validZipCode: validZipCodeState,
      zipCode: zipCodeState,
      redoSearchRegion: redoSearchRegionState
    });
  };

  const saveRegion = (e) => {
    setRegionState(e);
    
    updateParent({
      addressLine1: addressLine1State,
      addressLine2: addressLine2State,
      city: cityState,
      country: countryState,
      disabled,
      showAddressLines: showAddressLinesState,
      showAddressZipCode: showAddressZipCodeState,
      showRegionAddress: showRegionAddressState,
      region: e,
      stateProp: state,
      stateRequired: stateRequiredState,
      validAddress: validAddressState,
      validCity: validCityState,
      validCountry: validCountryState,
      validPhone: validPhoneState,
      validStateProp: validState,
      validZipCode: validZipCodeState,
      zipCode: zipCodeState,
      redoSearchRegion: redoSearchRegionState
    });
  };

  return (
    <div>
      {showAddressLinesState ? (
        <Row>
          <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
            <Label for="address">
              *{" "}
              <Translate
                id="register.address"
                options={{ renderInnerHtml: true }}
              />
            </Label>
            <Input
              disabled={disabled}
              value={addressLine1State}
              type="text"
              name="address"
              id="address"
              onChange={handleAddressLine1Change}
            />
          </Col>
          <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
            <Input
              disabled={disabled}
              value={addressLine2State}
              type="text"
              name="address-line-2"
              id="address-line-2"
              onChange={handleAddressLine2Change}
            />
          </Col>
          {!validAddressState ? (
            <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
              <Alert color="info" isOpen={!validAddressState}>
                <FontAwesomeIcon icon={faInfoCircle} />{" "}
                <Translate id="register.address-line-1-error" />
              </Alert>
            </Col>
          ) : null}
        </Row>
      ) : null}
      <Row>
        <Col sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
          <Label for="city">
            * <Translate id="register.city" />
          </Label>
          <Input
            disabled={disabled}
            value={cityState}
            type="text"
            name="city"
            id="city"
            onChange={handleCityChange}
          />
          <br />
          {!validCityState ? (
            <Alert color="info" isOpen={!validCityState}>
              <FontAwesomeIcon icon={faInfoCircle} />{" "}
              <Translate id="register.city-error" />
            </Alert>
          ) : null}
        </Col>
        {
          <Col sm={{ size: 12 }} md={{ size: 6 }} lg={{ size: 6 }}>
            {stateRequiredState ? (
              <Label for="state">
                * <Translate id="register.state" />
              </Label>
            ) : null}
            <SelectorState
              disabled={disabled}
              data={{
                addressLine1: addressLine1State,
                addressLine2: addressLine2State,
                city: cityState,
                redoSearchRegion: redoSearchRegionState,
                country: countryState,
                disabled,
                showAddressLines: showAddressLinesState,
                showAddressZipCode: showAddressZipCodeState,
                showRegionAddress: showRegionAddressState,
                state: state,
                stateRequired: stateRequiredState,
                validAddress: validAddressState,
                validCity: validCityState,
                validCountry: validCountryState,
                validPhone: validPhoneState,
                validState: validState,
                validZipCode: validZipCodeState,
                zipCode: zipCodeState,
              }}              value={state}
              updateParent={saveStateProvince("state")}
            />
            <br />
            {!validState ? (
              <Alert color="danger" isOpen={!validState}>
                <Translate id="register.state-error" />
              </Alert>
            ) : null}
          </Col>
        }
      </Row>

      <Row>
        <Col sm={{ size: 12 }} md={{ size: 8 }} lg={{ size: 7 }}>
          <Label for="country">
            * <Translate id="register.country" />
          </Label>
          <SelectorCountry
            disabled={disabled}
            data={countryState}
            value={countryState}
            updateParent={saveCountry("country")}
          />
          <br />
          {!validCountryState ? (
            <Alert color="info" isOpen={!validCountryState}>
              <FontAwesomeIcon icon={faInfoCircle} />{" "}
              <Translate id="register.country-error" />
            </Alert>
          ) : null}
        </Col>

        {showRegionAddressState && countryState !== "" ? (
          <Col>
            <Label for="region">
              <Translate id="register.region" />
            </Label>
            <SelectorRegion
              countryIso={countryState}
              disabled={disabled}
              redoSearchRegion={redoSearchRegionState}
              value={regionState}
              updateParent={saveRegion}
            />
          </Col>
        ) : null}

        {showAddressZipCodeState ? (
          <Col sm={{ size: 12 }} md={{ size: 4 }} lg={{ size: 5 }}>
            <Label for="zip-code">
              * <Translate id="register.zip-code" />
            </Label>
            <Input
              disabled={disabled}
              value={zipCodeState}
              type="text"
              name="zip-code"
              id="zip-code"
              onChange={handleZipCodeChange}
            />
            <br />
            {!validZipCodeState ? (
              <Alert color="info" isOpen={!validZipCodeState}>
                <FontAwesomeIcon icon={faInfoCircle} />{" "}
                <Translate id="register.zip-code-error" />
              </Alert>
            ) : null}
          </Col>
        ) : null}
      </Row>
    </div>
  );
};

export default FormGroupAddressV2;
