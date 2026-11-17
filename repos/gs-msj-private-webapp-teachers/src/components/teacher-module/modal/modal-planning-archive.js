/**
 * Created by portiz 2020/05/07
 */
import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {archivePlanning} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Modal to archive an planning
 */
const ModalPlanningArchive = ({ planningId, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setErrorMessage('');
        setLoading(false);
        setShowErrorMessage(false);
        setIsVisible(visible);
    }, [planningId, visible]);

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(!isVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            loading,
            planningId,
            showErrorMessage,
            visible: !isVisible,
        });
    };

    // Toggles when yes
    const toggleYes = () => {
        setLoading(true);
        // Archive planning
        let apiCall = archivePlanning(planningId);
        apiCall.then((response) => {
            if(typeof response.data.Messages !== 'undefined' && response.data.Messages != null && response.data.Messages.length > 0)
            {
                let message = '';
                for(let i=0; i < response.data.Messages.length; i++)
                {
                    message += response.data.Messages[i] + '<br/>';
                }
                setLoading(false);
                setShowErrorMessage(true);
                setErrorMessage(message);
            }
            else
            {
                setLoading(false);
                setIsVisible(false);
                updateParent({
                    errorMessage,
                    loading: false,
                    planningId,
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
            loading,
            planningId,
            showErrorMessage,
            visible: false,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="planning.archive" />
                </ModalHeader>
                <ModalBody>
                    {showErrorMessage ?
                        <div className="content" dangerouslySetInnerHTML={{__html: errorMessage}}/>
                        :
                        <div>
                            <Translate id="planning.archive-semester-note" />
                            <hr/>
                            <p className="text-center">
                                <Button color="danger" onClick={toggleYes} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#f4ffef"}
                                        /> : null}
                                    <Translate id="planning.archive-yes" />
                                </Button>
                            </p>
                            <p className="text-center">
                                <Button color="info" onClick={toggleNo} disabled={loading ? "disabled": ""}>
                                    <Translate id="planning.archive-no" />
                                </Button>
                            </p>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalPlanningArchive;