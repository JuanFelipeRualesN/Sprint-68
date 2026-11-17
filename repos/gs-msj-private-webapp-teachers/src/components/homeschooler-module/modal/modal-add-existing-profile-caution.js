import React, { useState, useCallback } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
import { Translate } from "react-localize-redux";

/**
 * Modal to ask if the user wants to attach the user to the subscription
 */
const ModalAttachProfile = ({ studentName, visibleModalAttach, updateParent }) => {
    // This function will be called when the modal is closed without a "Yes" or "No" action
    // (e.g., by clicking the 'X' button in the header or backdrop).
    // It signals to the parent that the modal should close and the answer is 'false'.
    const handleClose = useCallback(() => {
        // We pass an object back to updateParent to signal closure and the answer
        updateParent({ answer: false, visibleModalAttach: false });
    }, [updateParent]); // `updateParent` is a dependency

    // Toggles when 'Yes' button is clicked
    const toggleYes = useCallback(() => {
        // Signal to parent that modal should close and answer is true
        updateParent({ answer: true, visibleModalAttach: false });
    }, [updateParent]);

    // Toggles when 'No' button is clicked
    const toggleNo = useCallback(() => {
        // Signal to parent that modal should close and answer is false
        updateParent({ answer: false, visibleModalAttach: false });
    }, [updateParent]);

    return (
        <div>
            {/* Modal's isOpen is controlled by the 'visibleModalAttach' prop from parent */}
            <Modal isOpen={visibleModalAttach} toggle={handleClose}>
                <ModalHeader toggle={handleClose}>
                    <Translate id="general.attention" />
                </ModalHeader>
                <ModalBody>
                    <Translate id="subscription-set-profile-existing-account.are-you-sure-profile-to-subscription" />
                    {/* studentName is directly from props */}
                    <h3 className='mt-2 homeschooler-modal-title'>{studentName}</h3>
                    <hr/>
                    <p style={{textAlign: 'right'}}>
                        <button className="btn-default button-primary-custom btn-yesno" onClick={toggleYes}>
                            <Translate id="subscription-set-profile-existing-account.yes" />
                        </button>
                        <button className="btn-default button-secondary-custom btn-yesno" onClick={toggleNo}>
                            <Translate id="subscription-set-profile-existing-account.no" />
                        </button>
                    </p>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalAttachProfile;