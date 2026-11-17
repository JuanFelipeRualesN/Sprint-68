import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import ReportStudentPlanningLessonAverage from '../../shared-module-components/view/report-student-planning-lesson-average';
/**
 * Modal to see report planning details
 */
const ModalReportPlanningLessonAverage = ({studentId, studentName, lessonId, lessonTitle, visible, planningId, planLessonId,
                                              updateParent}) => {
    const [isVisible, setIsVisible] = useState(visible);

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(!isVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent(!isVisible);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                </ModalHeader>
                <ModalBody>
                    <ReportStudentPlanningLessonAverage 
                        studentId={studentId} 
                        studentName={studentName} 
                        lessonId={lessonId} 
                        planningId={planningId} 
                        lessonTitle={lessonTitle}
                        planLessonId={planLessonId}
                    />
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalReportPlanningLessonAverage;