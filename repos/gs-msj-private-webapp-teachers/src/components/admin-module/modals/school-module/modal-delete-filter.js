/**
 * Created by portiz on 2/3/20.
 */
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Generic modal. Set whatever you want to put in this message box.
 */
const ModalDeleteFilter = ({ bodyText, entityId, visibleDeleteFilter, updateParent }) => {
    // Hide or shows the window
    const toggle = () => {
        // Updates the parent (send it owns to state to update it)
        updateParent(true,'C');
    };

    const toggleYes = () => {
        // Updates the parent (send it owns to state to update it)
        updateParent(true,'Y', entityId);
    };

    const toggleNo = () => {
        // Updates the parent (send it owns to state to update it)
        updateParent(true,'N');
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={visibleDeleteFilter} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="general.attention"/>
                </ModalHeader>
                <ModalBody>
                    <Translate id="school-module.remove-filter"/>
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={toggleYes}>
                        <Translate id="general.yes"/>
                    </Button>
                    <Button color="primary" onClick={toggleNo}>
                        <Translate id="general.no"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalDeleteFilter;