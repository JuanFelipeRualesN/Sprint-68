/**
 * Created by portiz 2020/05/12
 */
import React,  { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {archiveTeacherPlanAssignment, archiveTeacherPlanLesson} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Modal to archive an item of the planning
 */
const ModalPlanningItemArchive = ({ planningId, planningItemId, lessonOrQuiz, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setErrorMessage('');
        setLoading(false);
        setShowErrorMessage(false);
        setIsVisible(visible);
    }, [planningItemId, planningId, lessonOrQuiz, visible]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            loading,
            planningId,
            planningItemId,
            lessonOrQuiz,
            showErrorMessage,
            visible: newVisible,
        });
    };

    // Toogles when yes
    const toggleYes = () => {
        setLoading(true);
        
        if(lessonOrQuiz === "LESSON")
        {
            // Archive lesson
            let apiCall = archiveTeacherPlanLesson(planningId, planningItemId);
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
                        errorMessage,
                        loading: false,
                        planningId,
                        planningItemId,
                        lessonOrQuiz,
                        showErrorMessage,
                        visible: false,
                    });
                }
            });
        }
        else if(lessonOrQuiz === "QUIZ")
        {
            // Archive assignment
            let apiCall = archiveTeacherPlanAssignment(planningId, planningItemId);
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
                        errorMessage,
                        loading: false,
                        planningId,
                        planningItemId,
                        lessonOrQuiz,
                        showErrorMessage,
                        visible: false,
                    });
                }
            });
        }
    };

    // Toggles when no
    const toggleNo = () => {
        setIsVisible(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            loading,
            planningId,
            planningItemId,
            lessonOrQuiz,
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
                            <Translate id="planning.archive-item-note" />
                            <hr/>
                            <p className="text-center">
                                <Button color="danger" onClick={toggleYes} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#fff"}

                                        />: null}{' '}
                                    <Translate id="planning.archive-yes" />
                                </Button>
                            </p>
                            <p className="text-center">
                                <Button color="info" onClick={toggleNo} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#fff"}

                                        />: null}{' '}
                                    <Translate id="planning.archive-no" />
                                </Button>
                            </p>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
}

export default ModalPlanningItemArchive;