/**
 * Created by portiz 2020/05/21
 */
import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {deleteInvitationEmailRequest} from "../../../api/users";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Modal to delete the invitation
 */
const ModalInvitationDelete = ({ invitationId, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [invitationIdState, setInvitationIdState] = useState(invitationId);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [visibleState, setVisibleState] = useState(visible);

    useEffect(() => {
        setErrorMessage('');
        setLoading(false);
        setInvitationIdState(invitationId);
        setShowErrorMessage(false);
        setVisibleState(visible);
    }, [invitationId, visible]);

    // Hide or shows the window
    const toggle = () => {
        setVisibleState(!visibleState);
        
        const state = {
            errorMessage: errorMessage,
            loading: loading,
            invitationId: invitationIdState,
            showErrorMessage: showErrorMessage,
            visible: !visibleState,
        };
        
        updateParent(state);
    };

    // Toggles when yes
    const toggleYes = () => {
        setLoading(true);
        
        let apiCall = deleteInvitationEmailRequest(invitationIdState);
        apiCall.then((response) => {
            setLoading(false);
            
            if(typeof response.data.Messages !== 'undefined' && response.data.Messages != null && response.data.Messages.length > 0) {
                let message = '';
                for(let i = 0; i < response.data.Messages.length; i++) {
                    message += response.data.Messages[i] + '<br/>';
                }
                setShowErrorMessage(true);
                setErrorMessage(message);
            } else {
                setVisibleState(false);
                
                const state = {
                    errorMessage: errorMessage,
                    loading: false,
                    invitationId: invitationIdState,
                    showErrorMessage: showErrorMessage,
                    visible: false,
                };
                
                updateParent(state);
            }
        });
    };
    // Toggles when no
    const toggleNo = () => {
        setVisibleState(false);
        
        const state = {
            errorMessage: errorMessage,
            loading: loading,
            invitationId: invitationIdState,
            showErrorMessage: showErrorMessage,
            visible: false,
        };
        
        updateParent(state);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={visibleState} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="connections.delete-invitation-title" />
                </ModalHeader>
                <ModalBody>
                    {showErrorMessage ?
                        <div className="content" dangerouslySetInnerHTML={{__html: errorMessage}}/>
                        :
                        <div>
                            <Translate id="connections.invitation-archive-note" />
                            <hr/>
                            <p className="text-center">
                                <Button color="danger" onClick={toggleYes} disabled={loading ? 'disabled' : ''}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#fff"}
                                        />: null}{' '}<Translate id="connections.invitation-delete-yes" />
                                </Button>
                                {' '}
                                <Button color="info" onClick={toggleNo} disabled={loading ? 'disabled' : ''}>
                                    <Translate id="connections.invitation-delete-no" />
                                </Button>
                            </p>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalInvitationDelete;