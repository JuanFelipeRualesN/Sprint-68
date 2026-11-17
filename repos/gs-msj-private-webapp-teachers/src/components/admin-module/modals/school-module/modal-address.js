/**
 * Created by portiz on 2/7/19.
 */
import React, { useState, useEffect } from 'react';
import {
  saveEntitySchoolAddress,
  deleteSchoolAddress,
} from "../../../../api/schools";
import { Button, Modal, ModalBody, ModalFooter, Row, Col } from "reactstrap";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import FormGroupAddressV2 from "../../../shared-module-components/form/form-group-address-v2";
/**
 * Modal to save school addresses only
 */
const ModalSchoolAddress = ({ addressId, addressLine1, addressLine2, city, country, countryIso, disabled = false,
  entityId, isAddressEdit, nameAddress, region, state, stateIso, visibleAddress,
  zip, updateParent, saveDetailAddress }) => {
    const [addressIdState, setAddressIdState] = useState(addressId ?? null);
    const [addressLine1State, setAddressLine1State] = useState(addressLine1 ?? "");
    const [addressLine2State, setAddressLine2State] = useState(addressLine2 ?? "");
    const [cityState, setCityState] = useState(city ?? "");
    const [countryState, setCountryState] = useState(country ?? "");
    const [countryIsoState, setCountryIsoState] = useState(countryIso ?? "");
    const [disabledState, setDisabledState] = useState(disabled);
    const [disabledSave, setDisabledSave] = useState(false);
    const [entityIdState, setEntityIdState] = useState(entityId ?? null);
    const [isEdit, setIsEdit] = useState(isAddressEdit);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState("");
    const [nameAddressState, setNameAddressState] = useState(nameAddress ?? "");
    const [regionState, setRegionState] = useState(region ?? "");
    const [stateState, setStateState] = useState(state ?? "");
    const [stateIsoState, setStateIsoState] = useState(stateIso ?? "");
    const [validAddress, setValidAddress] = useState(false);
    const [validCity, setValidCity] = useState(false);
    const [validCountry, setValidCountry] = useState(false);
    const [validState, setValidState] = useState(false);
    const [validZipCode, setValidZipCode] = useState(false);
    const [visibleAddressState, setVisibleAddressState] = useState(visibleAddress);
    const [zipState, setZipState] = useState(zip ?? "");
    const [stateRequired, setStateRequired] = useState(false);
    const [entityName, setEntityName] = useState("");

    useEffect(() => {
        if (isEdit) {
            setEntityIdState(entityId ?? null);
            setDisabledState(disabled);
            setAddressLine1State(addressLine1 ?? "");
            setAddressLine2State(addressLine2 ?? "");
            setNameAddressState(nameAddress ?? "");
            setCityState(city ?? "");
            setStateState(state ?? "");
            setStateIsoState(stateIso ?? "");
            setCountryState(country ?? "");
            setCountryIsoState(countryIso ?? "");
            setRegionState(region ?? "");
            setZipState(zip ?? "");
            setAddressIdState(addressId ?? null);
            setVisibleAddressState(visibleAddress);
            setIsEdit(isAddressEdit);
            setStateRequired(false);
        } else {
            setEntityIdState(entityId ?? null);
            setDisabledState(disabled);
            setAddressLine1State("");
            setAddressLine2State("");
            setNameAddressState(nameAddress ?? "");
            setCityState("");
            setStateState(null);
            setStateIsoState(null);
            setCountryState(null);
            setCountryIsoState(null);
            setRegionState(null);
            setZipState("");
            setAddressIdState(null);
            setVisibleAddressState(visibleAddress);
            setIsEdit(isAddressEdit);
            setStateRequired(false);
        }
    }, [entityId, disabled, addressLine1, addressLine2, nameAddress, city, state, stateIso, 
        country, countryIso, region, zip, addressId, visibleAddress, isAddressEdit]);

    useEffect(() => {
        setMessage("");

        if (!isEdit) {
            setCityState("");
            setStateState(null);
            setStateIsoState(null);
            setCountryState(null);
            setCountryIsoState(null);
            setRegionState(null);
            setZipState("");
            setAddressIdState(null);
            setAddressLine1State("");
            setAddressLine2State("");
        }
    }, []);

    const deleteInfo = () => {
        if (addressIdState !== null) {
            let apiCalls = deleteSchoolAddress(addressIdState);

            apiCalls.then((response) => {
                let messageStr = "";
                for (let i = 0; i < response.data.Messages.length; i++) {
                    messageStr += response.data.Messages[i] + " ";
                }
                if (response.status === 200 || response.status === 201) {
                    setDisabledSave(true);
                }
                setMessage(messageStr);
                setLoading(false);
            });
        }
    };

    // Hide or shows the window
    const toggle = () => {
        setVisibleAddressState(!visibleAddressState);
        setDisabledSave(false);
        
        if (entityIdState) {
            updateParent(true);
        }
    };

    useEffect(() => {
        setDisabledSave(!validAddress || !validCity || !validCountry || !validZipCode || !regionState || (countryState == "USA" && !stateState));
    }, [validAddress, validCity, validCountry, validZipCode, regionState, stateState]);

    const saveAddress = async () => {
        if (validAddress && validCity && validCountry && validZipCode) {    
            const detailAddress = {
                entityName: entityName,
                entityId: entityIdState,
                addressLine1: addressLine1State,
                addressLine2: addressLine2State,
                city: cityState,
                state: stateState,
                country: countryState,
                zip: zipState,
                addressId: addressIdState,
                region: regionState
            };
            saveDetailAddress(detailAddress);      
        }
    };

    const updateInfo = (e) => {
        setValidAddress(e.validAddress);
        setValidCity(e.validCity);
        setValidState(e.validState);
        setValidCountry(e.validCountry);
        setValidZipCode(e.validZipCode);
        setAddressLine1State(e.addressLine1);
        setAddressLine2State(e.addressLine2);
        setCityState(e.city);
        setStateState(e.state);
        setCountryState(e.country);
        setZipState(e.zipCode);
        setRegionState(e.region);
        setStateState(e.stateProp)
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={visibleAddressState} toggle={toggle}>
                <ModalBody>
                    <FormGroupAddressV2
                        addressLine1={addressLine1State}
                        addressLine2={addressLine2State}
                        city={cityState}
                        state={stateState === null ? "" : stateState}
                        country={countryState === null ? "" : countryState}
                        zipCode={zipState}
                        region={regionState}
                        disabled={disabledState}
                        stateRequired={stateRequired}
                        updateParent={updateInfo}
                        showRegionAddress={true}
                    />
                </ModalBody>
                <ModalFooter>
                    <Row>
                        <Col xs={12}>
                            {message !== ""}
                            {message}
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            {isEdit ? (
                                <Button
                                    className="text-left"
                                    color="danger"
                                    onClick={deleteInfo}
                                >
                                    <Translate id="school-module.delete" />
                                </Button>
                            ) : null}

                            <Button
                                className="btn-yesno"
                                color="success"
                                onClick={saveAddress}
                                disabled={disabledSave}
                            >
                                <Translate id="school-module.save" />
                            </Button>
                            <Button
                                className="btn-yesno"
                                color="secondary"
                                onClick={toggle}
                            >
                                <Translate id="school-module.close" />
                            </Button>
                        </Col>
                    </Row>
                </ModalFooter>
            </Modal>
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default ModalSchoolAddress;
