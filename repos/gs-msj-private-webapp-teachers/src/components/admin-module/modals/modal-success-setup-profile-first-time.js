/**
 * Created by portiz on 3/4/19.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle} from '@fortawesome/free-solid-svg-icons';
import {Link} from "react-router-dom";
/**
 * Modal success when setting up the profile for the first time
 */
const ModalSuccessSetupProfile = ({ visible, spacesLeft, updateParent }) => {
    const [modal, setModal] = useState(visible);
    const [answer, setAnswer] = useState(false);

    // Hide or shows the window
    const toggle = () => {};

    // Toogles when yes
    const toogleYes = () => {
        const newModal = !modal;
        setModal(newModal);
        setAnswer(true);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            modal: newModal,
            spacesLeft,
            answer: true,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={modal} toggle={toggle}>
                <ModalBody>
                    <h2 className="centered-text">
                        <FontAwesomeIcon className="green-text" icon={faCheckCircle}/> {' '}
                        <Translate id="profile-register.profile-setup-success" options={{renderInnerHtml: true}}/></h2>
                    <br/>
                    {
                        spacesLeft ? <Translate id="profile-register.add-new-student-spaces-left" /> : null
                    }
                    <hr/>
                    <div className="text-center">
                        {
                            spacesLeft ?
                                <div>
                                <button className="btn-default button-primary-custom" onClick={toogleYes}>
                                    <Translate id="profile-register.yes-add-new-student" />
                                </button>
                                    <h3> <Translate id="profile-register.or" /> </h3>
                                </div>
                                : null
                        }
                    </div>
                    <p className="text-center">
                        <Link className="btn btn-success" to="/download-first-time" color="success">
                            <Translate id="profile-register.download-the-app" />
                        </Link>
                    </p>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalSuccessSetupProfile;