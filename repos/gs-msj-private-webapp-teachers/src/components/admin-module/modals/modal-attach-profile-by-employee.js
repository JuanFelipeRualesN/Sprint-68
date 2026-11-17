/**
 * Created by admin on 02/19/20.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import FormAddProfileByEmployee from '../forms/form-add-profile-by-employee';

/**
 * Class of modal to attach profiles.
 */
const   ModalAttachProfileByEmployee = ({ roleId, schoolId, subscriptionId, visible, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);
    
    const schoolIdValue = typeof schoolId !== 'undefined' && schoolId != null && schoolId !== '' ? schoolId : null;

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent({
            roleId,
            schoolId: schoolIdValue,
            subscriptionId,
            visible: newVisible,
        });
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="subscriptions.attach-or-add-profile"/>
                </ModalHeader>
                <ModalBody>
                    <FormAddProfileByEmployee schoolId={schoolIdValue} roleId={roleId} subscriptionId={subscriptionId}/>
                </ModalBody>
                <ModalFooter>
                    <button onClick={toggle} className="btn-default button-secondary-custom">
                        <Translate id="subscriptions.close"/>
                    </button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default  ModalAttachProfileByEmployee;