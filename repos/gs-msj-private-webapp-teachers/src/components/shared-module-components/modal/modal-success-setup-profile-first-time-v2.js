import React, { useState } from 'react';
import { Button, Modal, ModalBody} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle} from '@fortawesome/free-solid-svg-icons';
/**
 * Modal success when setting up the profile for the first time
 */
const ModalSuccessSetupProfileV2 = ({visible, spacesLeft, updateParent}) => {
    const [modal, setModal] = useState(visible);
    const [spacesLeftState, setSpacesLeftState] = useState(spacesLeft);
    const [answer, setAnswer] = useState(false);

    // Hide or shows the window
       const toggle = () => {
           setModal(!modal);
           setAnswer(false);


           const state = {
               modal: !modal,
               spacesLeft: spacesLeftState,
               answer: false
           };

           // Updates the parent (send it owns to state to update it)
           updateParent(state);
      };

    // Toggles when yes
    const toggleYes = () => {
        setModal(!modal);
        setAnswer(true);
        
        const state = {
            modal: !modal,
            spacesLeft: spacesLeftState,
            answer: true
        };
        
        // Updates the parent (send it owns to state to update it)
        updateParent(state);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
        <Modal isOpen={modal} toggle={toggle}>
            <ModalBody>
            <h2 className="centered-text">
                <FontAwesomeIcon className="green-text" icon={faCheckCircle} />{' '}
                <Translate id="profile-register.profile-setup-success" options={{ renderInnerHtml: true }} />
            </h2>
            <br />
            {spacesLeftState ? <Translate id="profile-register.add-new-student-spaces-left" /> : null}
            <hr />
            <div className="text-center">
                {spacesLeftState ? (
                <div>
                    <Button color="warning" onClick={toggleYes}>
                    <Translate id="profile-register.yes-add-new-student" />
                    </Button>
                </div>
                ) : null}
            </div>
            </ModalBody>
        </Modal>
        </div>
    );
};

export default ModalSuccessSetupProfileV2;