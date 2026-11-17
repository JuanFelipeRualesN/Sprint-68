/**
 * Created by portiz 2020/05/07
 */
import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {archivePlanning} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
import {FaExclamationTriangle} from "react-icons/all";
import {getClosePlanning} from "../../../api/teachers";
/** End of Multiple Language imports **/

/**
 * Modal to close a planning
 */
const ModalPlanningClose = ({ planningId, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [messages, setMessages] = useState('');
    const [isVisible, setIsVisible] = useState(visible);
    const [isPlanClosed, setIsPlanClosed] = useState(false);

    useEffect(() => {
        setErrorMessage('');
        setLoading(false);
        setShowErrorMessage(false);
        setIsVisible(visible);
    }, [planningId, visible]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent({
            errorMessage,
            loading,
            planningId,
            showErrorMessage,
            messages,
            visible: newVisible,
            isPlanClosed,
        });
    };

    // Toggles when yes
    const toggleYes = async () => {
        setLoading(true);
        let messages = "";
        let response = await getClosePlanning(planningId);
        response.data.Messages.forEach((message) => {
            messages += message;
        });
        setLoading(false);
        setIsVisible(false);
        setIsPlanClosed(true);
        setMessages(messages);
        
        updateParent({
            errorMessage,
            loading: false,
            planningId,
            showErrorMessage,
            messages: messages,
            visible: false,
            isPlanClosed: true,
        });
    };

    const toggleNo = () => {
        setIsVisible(false);
        updateParent({
            errorMessage,
            loading,
            planningId,
            showErrorMessage,
            messages,
            visible: false,
            isPlanClosed,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="planning.planning-close" />
                </ModalHeader>
                <ModalBody>
                    {showErrorMessage ?
                        <div className="content" dangerouslySetInnerHTML={{__html: errorMessage}}/>
                        :
                        <div>

                            <div className="warning-text modal-close_icon-adjustment">
                                <FaExclamationTriangle/>{' '}
                                <Translate id="planning.planning-close-instructions" />
                            </div>                                
                            <p className="text-right">
                                <Button className="mr-3"  color="danger" onClick={toggleYes} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#f4ffef"}
                                        /> : null}
                                    <Translate id="planning.planning-close-yes" />
                                </Button>

                                <Button color="info" onClick={toggleNo} disabled={loading ? "disabled": ""}>
                                    <Translate id="planning.planning-close-no" />
                                </Button>
                            </p>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
}

export default ModalPlanningClose;