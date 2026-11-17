/**
 * Created by portiz 2020/05/08
 */
import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalBody, Input, ModalFooter, ModalHeader } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import { sendConnectionRequest } from "../../../api/users";
import { BeatLoader } from "react-spinners";
import { convertMaxLengthSting } from "../../../utils/utils";
/** End of Multiple Language imports **/

/**
 * Modal to send connection request
 */
const ModalSendInvitationCode = ({ visible, updateParent}) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [invitationCode, setInvitationCode] = useState('');
    const [loading, setLoading] = useState(false);
    const [showMessage, setShowMessage] = useState(false);
    const [visibleState, setVisibleState] = useState(visible);

    useEffect(() => {
        setVisibleState(visible);
    }, [visible]);


    const handleNameChange = (e) => {
        const value = e.currentTarget.value;
        const newValue = convertMaxLengthSting(value, 20);
        setInvitationCode(newValue);
    };

    // Hide or shows the window
    const toggle = () => {
;
        setVisibleState(!visibleState);
        // Updates the parent (send it owns to state to update it)
        updateParent();
    };

    // Toggles when yes
    const toogleYes = () => {
        setLoading(true);
        // Archive
        let apiCall = sendConnectionRequest(invitationCode);

        apiCall.then((response) => {
        setLoading(true);
        
        if (response.data.Messages != null && response.data.Messages.length > 0) {
            let message = '';
            for (let i = 0; i < response.data.Messages.length; i++) {
            message += response.data.Messages[i] + '<br/>';
            }
            setShowMessage(true);
            setErrorMessage(message);
            setLoading(false);
        }
        });
    };

    return (
        <div>
        <Modal isOpen={visibleState} toggle={toggle}>
            <ModalHeader toggle={toggle} />
            <ModalBody>
            <p>
                <Translate id="modal-invitation.enter-invitation-code" options={{ renderInnerHtml: true }} />
            </p>
            <Input
                value={invitationCode}
                type="text"
                name="invitationCode"
                id="invitationCode"
                onChange={handleNameChange}
            />

            {showMessage ? (
                <div className="content message-invitation-wrapper miw-alert miw-sucess" dangerouslySetInnerHTML={{ __html: errorMessage }}
                />
            ) : null}
            </ModalBody>
            <ModalFooter>
            <p className="text-right">
                <button
                    disabled={invitationCode.length === 0 || loading ? "disabled" : ""}
                    className="btn-default button-primary-custom"
                    onClick={toogleYes}
                >
                {loading ? (
                    <BeatLoader size={10} color={"#fff"} />
                ) : null}{' '}
                <Translate id="modal-invitation.send-invitation" options={{ renderInnerHtml: true }} />
                </button>{' '}
                <Button color="secondary" onClick={toggle}>
                <Translate id="modal-invitation.close" />
                </Button>
            </p>
            </ModalFooter>
        </Modal>
        </div>
    );
};

export default ModalSendInvitationCode;