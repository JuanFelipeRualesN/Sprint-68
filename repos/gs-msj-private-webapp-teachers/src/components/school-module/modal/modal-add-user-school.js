/**
 * Created by portiz on 10/28/20.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Input, Col, Alert, Row } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSchool } from "@fortawesome/free-solid-svg-icons";
import FormGroupPublicPrivate from "../form/form-public-private";
import FormGroupAddress from "../../shared-module-components/form/form-group-address";
import SelectorSchool from "../selector/selector-school";
import { addUserSchool } from "../../../api/users";
import { getAvailabilityByRecordNumber } from "../../../api/schools";
/** End of Multiple Language imports **/

/**
 * To confirm the user school.
 */
const ModalAddUserSchool = ({userId: userIdProp, showModal: showModalProp, updateParent}) => {
    const [stateValue, setStateValue] = useState('');
    const [city, setCity] = useState('');
    const [country, setCountry] = useState('');
    const [previousAddressCountry, setPreviousAddressCountry] = useState('');
    const [previousAddressState, setPreviousAddressState] = useState('');
    const [privateOrPublic, setPrivateOrPublic] = useState(1);
    const [redoSchoolSearch, setRedoSchoolSearch] = useState(false);
    const [schoolId, setSchoolId] = useState('');
    const [userId] = useState(userIdProp);
    const [showModal, setShowModal] = useState(showModalProp);
    const [showErrorAlert, setShowErrorAlert] = useState(false);
    const [backendMessageError, setBackendMessageError] = useState('');
    const [recordNo, setRecordNo] = useState('');
    const [validAddress, setValidAddress] = useState(true);
    const [validCity, setValidCity] = useState(true);
    const [validCountry, setValidCountry] = useState(true);
    const [validPhone, setValidPhone] = useState(true);
    const [validPrivateOrPublic, setValidPrivateOrPublic] = useState(false);
    const [validSchool, setValidSchool] = useState(false);
    const [validState, setValidState] = useState(true);
    const [validRecordAvailable, setValidRecordAvailable] = useState(true);
    const [loading, setLoading] = useState(false);

    const handleRecordNoChange = (e) => {
        setRecordNo(e.currentTarget.value);
    };

    const handleEntityPrivateOrPublic = (e, valid) => {
        setPrivateOrPublic(e);
        setValidPrivateOrPublic(valid);
    };

    const saveSchool = () => (newValue) => {
        setRedoSchoolSearch(false);
        setValidSchool(true);
        setSchoolId(newValue);
    };

    // Saves the address details and verifies if the next button is ready
    const saveAddress = () => (newValue) => {
        setRedoSchoolSearch(false);
        setCity(newValue.city);
        setCountry(newValue.country);
        setSchoolId('');
        setStateValue(newValue.state);
        setValidCity(newValue.validCity);
        setValidCountry(newValue.validCountry);
        setValidPhone(newValue.validPhone);
        setValidSchool(false);
        setValidState(newValue.validState);

        if (previousAddressState !== newValue.state) {
        setPreviousAddressState(newValue.state);
        }
        if (previousAddressCountry !== newValue.country) {
        setPreviousAddressCountry(newValue.country);
        }
        setRedoSchoolSearch(true);
    };

    // Hide or shows the window
    const toggle = () => {
        setShowModal(!showModal);
        // Updates the parent (send it owns to state to update it)
        updateParent(true);
        updateParent(true, 'C');
    };

    const validateAvailabilityNumberRecord = async (entityId, record) => {
        const res = await getAvailabilityByRecordNumber(entityId, record);
        return res.data;
    };

    const toggleYes = async () => {
        if (recordNo.length === 0) {
            setShowErrorAlert(true);
            return;
        }

        const availabilityRecord = await validateAvailabilityNumberRecord(schoolId, recordNo);
        if (!availabilityRecord) {
            setValidRecordAvailable(false);
            return;
        }

        setLoading(true);
        try {
            const response = await addUserSchool(userId, schoolId, recordNo);
            if (response.status === 200 || response.status === 201) {
                setShowModal(!showModal);
                updateParent(true);
            }
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const toggleNo = () => {
        setShowModal(!showModal);
        // Updates the parent (send it owns to state to update it)
        updateParent(false);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={showModal} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="users-schools.add-user-to-institution" />
                </ModalHeader>
                <ModalBody>
                    <Translate id="users-schools.enter-record" />
                    <Input type="text" onChange={handleRecordNoChange} />
                </ModalBody>
                <ModalFooter>
                    <Row>
                        <Col xs={12}>
                            {showErrorAlert ?
                                <Alert color="danger">
                                    <Translate id="users-schools.record-no-required" />
                                </Alert> : null
                            }
                            {!validRecordAvailable &&
                                <Alert color="danger">
                                    <Translate id="wizard.general.record-not-available" />
                                </Alert>
                            }
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                            <h4>
                                <FontAwesomeIcon className="fa-1x" icon={faSchool} />{' '}
                                <Translate id="register-teacher.school-details" options={{ renderInnerHtml: true }} />
                            </h4>
                            <hr />
                        </Col>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                            <FormGroupPublicPrivate isRegisterForm={true} privateOrPublic={privateOrPublic} updateParent={handleEntityPrivateOrPublic} />
                        </Col>
                        <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                            <FormGroupAddress showAddressLines={false} showAddressZipCode={false} data={{
                                stateValue,
                                city,
                                country,
                                validAddress,
                                validCity,
                                validCountry,
                                validState
                            }} updateParent={saveAddress()} />
                        </Col>

                        {city !== null && city !== '' && country !== null && country !== ''
                            && validPrivateOrPublic ?
                            <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                                <SelectorSchool
                                    redoSearch={redoSchoolSearch}
                                    city={city} country={country}
                                    privateOrPublic={privateOrPublic}
                                    state={stateValue}
                                    updateParent={saveSchool()} />
                            </Col> : null}
                    </Row>
                    <Row>
                        {backendMessageError !== null && backendMessageError.length > 0 ?
                            <Col xs={12}>
                                <Alert color="danger">
                                    {backendMessageError}
                                </Alert>
                            </Col> : null
                        }
                        <Col xs={12}>

                            <Button color="success" onClick={toggleYes} disabled={!(schoolId != null && schoolId !== '' && recordNo !== null && recordNo !== '')}>
                                <Translate id="users-schools.confirm" />
                            </Button>
                            {' '}
                            <Button color="secondary" onClick={toggleNo}>
                                <Translate id="users-schools.cancel" />
                            </Button>
                        </Col>
                    </Row>
                </ModalFooter>
            </Modal>
            {loading ? <LoadingFullWindow /> : null}
        </div>
    );
};

export default ModalAddUserSchool;