/**
 * Created by portiz on 10/28/20.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Input, Col, Alert, Row } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import { confirmUserSchool } from "../../../../api/users";
/** End of Multiple Language imports **/

/**
 * To confirm the user school.
 */
const ModalConfirmUserSchool = ({ bodyText, userId, institutionId, showModal, updateParent }) => {
    const [showErrorAlert, setShowErrorAlert] = useState(false);
    const [recordNo, setRecordNo] = useState('');
    const [loading, setLoading] = useState(false);

    const handleRecordNoChange = (e) => {
        setRecordNo(e.currentTarget.value);
    };

    // Hide or shows the window
    const toggle = () => {
        // Updates the parent (send it owns to state to update it)
        updateParent(true, 'C');
    };

    const toggleYes = async () => {
        if (recordNo.length === 0) {
            setShowErrorAlert(true);
            return;
        }

        setLoading(true);
        try {
            await confirmUserSchool(userId, institutionId, recordNo);
            setLoading(false);
            updateParent(true);
        } catch {
            setLoading(false);
        }
    };

    const toggleNo = () => {
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
                            {showErrorAlert &&
                                <Alert color="danger">
                                    <Translate id="users-schools.record-no-required" />
                                </Alert>
                            }
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <Button color="success" onClick={toggleYes}>
                                <Translate id="users-schools.confirm" />
                            </Button>{' '}
                            <Button color="secondary" onClick={toggleNo}>
                                <Translate id="users-schools.cancel" />
                            </Button>
                        </Col>
                    </Row>
                </ModalFooter>
            </Modal>
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default ModalConfirmUserSchool;