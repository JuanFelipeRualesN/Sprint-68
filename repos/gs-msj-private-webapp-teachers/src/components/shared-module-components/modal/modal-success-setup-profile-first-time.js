import React, { useState, useEffect, useCallback } from 'react';
import { Button, Modal, ModalBody} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle} from '@fortawesome/free-solid-svg-icons';
import {Link} from "react-router-dom";

/**
 * Modal success when setting up the profile for the first time (using Hooks)
 */
const ModalSuccessSetupProfile = ({ visible, spacesLeft, updateParent }) => {
    const [modal, setModal] = useState(visible);

    // Sync prop `visible` if it changes after mount
    useEffect(() => {
        setModal(visible);
    }, [visible]);

    const toggle = useCallback(() => {
        // Intentionally left blank since original class also had an empty toggle
    }, []);

    const toggleYes = () => {
        setModal(false);

        updateParent({
            modal: false,
            answer: true
        });
    };

    return (
        <Modal isOpen={modal} toggle={toggle}>
            <ModalBody>
                <h2 className="centered-text">
                    <FontAwesomeIcon className="green-text" icon={faCheckCircle} />{' '}
                    <Translate id="profile-register.profile-setup-success" options={{ renderInnerHtml: true }} />
                </h2>
                <br />
                {spacesLeft && <Translate id="profile-register.add-new-student-spaces-left" />}
                <hr />
                <div className="text-center">
                    {spacesLeft && (
                        <div>
                            <Button color="warning" onClick={toggleYes}>
                                <Translate id="profile-register.yes-add-new-student" />
                            </Button>
                            <h3><Translate id="profile-register.or" /></h3>
                        </div>
                    )}
                </div>
                <p className="text-center">
                    <Link className="btn btn-success" to="/download-first-time" color="success">
                        <Translate id="profile-register.download-the-app" />
                    </Link>
                </p>
            </ModalBody>
        </Modal>
    );
};

export default ModalSuccessSetupProfile;