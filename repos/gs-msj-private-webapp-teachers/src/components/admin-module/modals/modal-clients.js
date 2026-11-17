/**
 * Created by portiz on 4/1/19.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import UsersDataTable from "../../shared-module-components/datatables/users-data-table";
import Settings from '../../../custom-classes/settings';

/**
 * Model that handles the clients
 */
const ModalClients = ({ visible, entityId, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);
    
    const entityIdValue = typeof entityId !== 'undefined' ? entityId : null;

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(!isVisible);
        updateParent(null);
    };

    const handleSelectedUser = (e) => {
        updateParent(e);
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="users-search.clients"/>
                    <br/>
                    <small>
                        <Translate id="users-search.clients-instructions"/>
                    </small>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                            <UsersDataTable schoolId={entityIdValue} roleId={Settings.ROLE_MAIN_CLIENT_ID} updateParent={handleSelectedUser}/>
                        </Col>
                    </Row>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={toggle} color="info">
                        <Translate id="general.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalClients;