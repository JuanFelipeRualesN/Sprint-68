/**
 * Created by portiz 2020/05/06
 */
import React, { useState, useEffect }  from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {archiveQuiz} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Modal to archive an homework
 */
const ModalHomeworkArchive = ({ quizId, visible, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setLoading(false);
        setErrorMessage('');
        setShowErrorMessage(false);
        setIsVisible(visible);
    }, [quizId, visible]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            loading,
            errorMessage,
            quizId,
            showErrorMessage,
            visible: newVisible,
        });
    };

    // Toogles when yes
    const toogleYes = () => {
        setLoading(true);
        // Archive
        let apiCall = archiveQuiz(quizId);

        apiCall.then((response) => {
            setLoading(false);
            
            if(typeof response.data.Messages !== 'undefined' && response.data.Messages != null && response.data.Messages.length > 0)
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
                    loading: false,
                    errorMessage,
                    quizId,
                    showErrorMessage,
                    visible: false,
                });
            }
        });
    };

    // Toggles when no
    const toogleNo = () => {
        setIsVisible(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            loading,
            errorMessage,
            quizId,
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
                            <Translate id="planning.archive-note-question" />
                            <hr/>
                            <p className="text-center">
                                <Button color="danger" onClick={toogleYes} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#fff"}

                                        />: null}{' '}
                                    <Translate id="planning.archive-yes" />
                                </Button>
                            </p>
                            <p className="text-center">
                                <Button color="info" onClick={toogleNo} disabled={loading ? "disabled": ""}>
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

export default ModalHomeworkArchive;