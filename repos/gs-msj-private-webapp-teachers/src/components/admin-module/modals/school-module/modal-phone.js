/**
 * Created by portiz on 1/31/20.
 */

import React, { useState, useEffect } from 'react';
import {savePhoneSchool,deleteSchoolPhone} from "../../../../api/schools";
import { Button, Modal, ModalBody, ModalFooter, Row, Col} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import FormGroupPhone from "../../../shared-module-components/form/form-group-phone";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";

/**
 * Modal to save school addresses only
 */
const ModalSchoolPhone = ({ disabled = false, entityId, isEdit, phone, phoneId, visiblePhone, updateParent }) => {
    const [disabledSave, setDisabledSave] = useState(false);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [phoneState, setPhoneState] = useState(phone ?? '');
    const [phoneIdState, setPhoneIdState] = useState(phoneId ?? null);
    const [validPhone, setValidPhone] = useState(false);

    useEffect(() => {
        setDisabledSave(!validPhone);
    }, [validPhone]);

    useEffect(() => {
        if (isEdit) {
            setPhoneState(phone ?? '');
            setPhoneIdState(phoneId ?? null);
        } else {
            setPhoneState('');
            setPhoneIdState(null);
        }
    }, [phone, phoneId, isEdit]);

    useEffect(() => {
        setMessage('');

        if (!isEdit) {
            setPhoneState('');
            setPhoneIdState(null);
        }
    }, []);

    // Hide or shows the window
    const toggle = () => {
        setDisabledSave(false);
        updateParent(true);
    };

    const deleteInfo = () => {
        if (phoneIdState !== null) {
            let apiCalls = deleteSchoolPhone(phoneIdState);

            apiCalls.then((response) => {
                let messageStr = '';
                for (let i = 0; i < response.data.Messages.length; i++) {
                    messageStr += response.data.Messages[i] + ' ';
                }
                if (response.status === 200 || response.status === 201) {
                    setDisabledSave(true);
                }
                setMessage(messageStr);
                setLoading(false);
            });
        }
    };

    const savePhone = () => {
        if (validPhone) {
            setLoading(true);

            let apiCalls = savePhoneSchool(
                entityId, phoneState, phoneIdState);

            apiCalls.then((response) => {
                let messageStr = '';
                for (let i = 0; i < response.data.Messages.length; i++) {
                    messageStr += response.data.Messages[i] + ' ';
                }
                if (response.status === 200 || response.status === 201) {
                    setDisabledSave(true);
                }
                setMessage(messageStr);
                setLoading(false);
            });
        }
    };

    const updateInfo = (e) => {
        setValidPhone(e.validPhone);
        setPhoneState(e.phone);
    };

    // Render a generic modal. The body text should be sent in the text
    let dataPhone = {
        phone: phoneState,
        disabled: disabled
    };

    return (
        <div>
            <Modal isOpen={visiblePhone} toggle={toggle}>
                <ModalBody>
                    <FormGroupPhone
                        disabled={disabled}
                        data={dataPhone}
                        updateParent={updateInfo}
                    />
                </ModalBody>
                <ModalFooter>
                    <Row>
                        <Col xs={12}>
                            {message !== ''}
                            {message}
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            {isEdit ?
                                <button
                                    className="text-left btn-cancel btn-default" style={{marginRight: '10px'}} onClick={deleteInfo}>
                                    <Translate id="school-module.delete"/>
                                </button> : null}
                            <button className="btn-yes btn-default" onClick={savePhone} disabled={disabledSave}>
                                <Translate id="school-module.save"/>
                            </button>
                            <Button className="btn-yesno" onClick={toggle}>
                                <Translate id="school-module.close"/>
                            </Button>
                        </Col>
                    </Row>

                </ModalFooter>
            </Modal>
            {loading && <LoadingFullWindow/>}
        </div>
    );
};

export default ModalSchoolPhone;