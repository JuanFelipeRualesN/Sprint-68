import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Translate } from "react-localize-redux";
import AddDetails from '../views/add-details';

const ModalAddDetails = ({ visible, entityId, entityAcademicDetail, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);

    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent(newVisible);
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                </ModalHeader>
                <ModalBody>
                    <div className="wrapper-content-academic-details input-responsive">
                        <AddDetails entityId={entityId} entityAcademicDetail={entityAcademicDetail} toggle={toggle} />
                    </div>
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close" />
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalAddDetails;