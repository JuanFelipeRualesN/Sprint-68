/**
 * Created by portiz on 4/1/19.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Modal to ask if the user wants to attach the user to the subscription
 */
const ModalAttachProfile = ({ studentName, visibleModalAttach, updateParent }) => {
    const [answer, setAnswer] = useState(false);
    const [modal, setModal] = useState(false);

    // Hide or shows the window
    const toggle = () => {
        const newModal = !modal;
        setModal(newModal);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            answer,
            studentName,
            visibleModalAttach,
            modal: newModal,
        });
    };

    // Toogles when yes
    const toogleYes = () => {
        const newModal = !modal;
        setModal(newModal);
        setAnswer(true);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            answer: true,
            studentName,
            visibleModalAttach,
            modal: newModal,
        });
    };

    // Toogles when no
    const toogleNo = () => {
        const newModal = !modal;
        setModal(newModal);
        setAnswer(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            answer: false,
            studentName,
            visibleModalAttach,
            modal: newModal,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={visibleModalAttach} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="general.attention" />
                </ModalHeader>
                <ModalBody>
                    <Translate id="subscription-set-profile-existing-account.are-you-sure-profile-to-subscription" />
                    <h3>{studentName}</h3>
                    <hr/>
                    <p style={{textAlign: 'right'}}>
                        <button className="btn-default button-primary-custom" onClick={toogleYes}>
                            <Translate id="subscription-set-profile-existing-account.yes" />
                        </button>
                        <Button style={{marginLeft: '10px'}} color="info" onClick={toogleNo}>
                            <Translate id="subscription-set-profile-existing-account.no" />
                        </Button>
                    </p>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalAttachProfile;