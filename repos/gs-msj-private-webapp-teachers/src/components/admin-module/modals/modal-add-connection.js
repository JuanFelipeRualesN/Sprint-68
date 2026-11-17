import React, { useState, useRef, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col } from 'reactstrap';
import { Translate } from "react-localize-redux";
import UsersDataTable from "../../shared-module-components/datatables/users-data-table";
import { Checkbox } from 'pretty-checkbox-react';
import {addConnectionByAdmin} from "../../../api/users";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import Swal from 'sweetalert2';

/**
 * Model to add profile connections
 */
const ModalAddConnection = ({ visible, userId, name, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);
    const [isAlphaUser, setIsAlphaUser] = useState(false);
    const [backendSuccessful, setBackendSuccessful] = useState(false);
    const [messageBackend, setMessageBackend] = useState('');

    // Create a ref to store the latest state
    const isAlphaUserRef = useRef(isAlphaUser);

    // Use a useEffect to keep the ref's value in sync with the state
    useEffect(() => {
        isAlphaUserRef.current = isAlphaUser;
    }, [isAlphaUser]);

    useEffect(() => {
        if (messageBackend.length === 0) return;

        const text = Array.isArray(messageBackend) ? messageBackend.join(' ') : messageBackend;

        Swal.fire({ text }).then(() => setMessageBackend(''));
    }, [messageBackend]);

    const changeCheckBox = (e) => {
        const checked = e.currentTarget.checked;
        setIsAlphaUser(checked);

    };

    const toggle = () => {
        setIsVisible(!isVisible);
        updateParent(null);
    };

    const handleSelectedUser = (e) => {
        setLoading(true);
        setMessageBackend('');

        // Get the latest value from the ref instead of the stale state variable
        const currentIsAlphaUser = isAlphaUserRef.current;


        let apiCall = addConnectionByAdmin(userId, e.UserId, currentIsAlphaUser);

        apiCall.then((response) => {
            setLoading(false);
            setMessageBackend('');
            setBackendSuccessful(response.status === 201 || response.status === 200);

            if (typeof response.data.Messages !== 'undefined' && response.data.Messages != null && response.data.Messages.length > 0) {
                setMessageBackend(response.data.Messages);
            }
        }, () => {
            setLoading(false);
        });
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="connections.add-connection"/>
                    <br/>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col style={{color: "red"}}>
                            <Translate id="connections.check-this"/> {' '}
                            <Checkbox color="primary" onChange={changeCheckBox} hasFocus bigger/> {' '} <Translate
                            id="connections.check-this-if"/> <b>{name} </b> {' '} <Translate
                            id="connections.check-this-2"/>
                            <br/>
                            <small><Translate id="connections.add-connection-director-note"/></small>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <hr/>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <UsersDataTable updateParent={handleSelectedUser}/>
                        </Col>
                    </Row>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={toggle} color="info">
                        <Translate id="general.close"/>
                    </Button>
                </ModalFooter>
                {loading ? <LoadingFullWindow/> : null}
            </Modal>
        </div>
    );
}

export default ModalAddConnection;