import React, { useState } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter} from 'reactstrap';
import FormHandleUser from "../forms/form-handle-user";
/**
 * Modal for update a plan
 */
const ModalUpdateUserDetails = ({ isEdit = false, userId = null, visible, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent(newVisible);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal className="modal-results" isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle}/>
                <ModalBody className="padding-20">
                    <FormHandleUser isEdit={isEdit} userId={userId} />
                </ModalBody>
                <ModalFooter/>
            </Modal>
        </div>
    );
};

export default ModalUpdateUserDetails;