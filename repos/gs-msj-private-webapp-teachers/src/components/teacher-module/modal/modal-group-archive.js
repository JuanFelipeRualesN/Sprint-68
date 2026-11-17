/**
 * Created by portiz 2020/05/16
 */
import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {archiveGroup} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Modal to archive an group
 */
const ModalGroupArchive = ({ groupId, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setErrorMessage('');
        setLoading(false);
        setShowErrorMessage(false);
        setIsVisible(visible);
    }, [groupId, visible]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            groupId,
            loading,
            showErrorMessage,
            visible: newVisible,
        });
    };

    // Toggles when yes
    const toggleYes = () => {
        setLoading(true);
        // Archive planning
        let apiCall = archiveGroup(groupId);
        apiCall.then((response) => {
            setLoading(false);
            
            if(typeof response.data.Messages !== 'undefined' && response.data.Messages != null
                && response.data.Messages.length > 0)
            {
                let message = '';
                for(let i=0; i < response.data.Messages.length; i++)
                {
                    message += response.data.Messages[i] + '<br/>';
                }
                setShowErrorMessage(true);
                setErrorMessage(message);
            }
            else
            {
                setIsVisible(false);
                updateParent({
                    errorMessage,
                    groupId,
                    loading: false,
                    showErrorMessage,
                    visible: false,
                });
            }
        });
    };

    // Toggles when no
    const toggleNo = () => {
        setIsVisible(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            groupId,
            loading,
            showErrorMessage,
            visible: false,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="group-management.delete" />
                </ModalHeader>
                <ModalBody>
                    {showErrorMessage ?
                        <div className="content" dangerouslySetInnerHTML={{__html: errorMessage}}/>
                        :
                        <div>
                            <Translate id="group-management.group-archive-note" />
                            <hr/>
                            <p className="text-center">
                                <Button color="danger" onClick={toggleYes} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#fff"}

                                        />: null}{' '}
                                    <Translate id="group-management.archive-yes" />
                                </Button>
                                {' '}
                                <button className="btn-default button-secondary-custom" onClick={toggleNo} disabled={loading ? "disabled": ""}>
                                    <Translate id="group-management.no" />
                                </button>
                            </p>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
}

export default ModalGroupArchive;