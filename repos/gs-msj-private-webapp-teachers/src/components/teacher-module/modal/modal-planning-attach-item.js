/**
 * Created by portiz 2020/05/12
 */
import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter} from 'reactstrap';
/** Multiple Language imports **/
import FormPlanningTeacherManager from "../form/form-planning-teacher-manager";
/** End of Multiple Language imports **/
import { Translate } from 'react-localize-redux';

/**
 * Modal to attach an item to the planning
 */
const ModalPlanningAttachItem = ({ 
    planningId, 
    plannings, 
    visible, 
    defaultTypeAssignmentOrLessons, 
    isCatalog, 
    lessonId, 
    handleSuccessModal, 
    showCalendarDates = false, // Optional prop to show calendar dates
    updateParent 
}) => {
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setIsVisible(visible);
    }, [visible]);

    const refreshData = (value) => {
        if(value) {
            updateParent(true);
        }
    };

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            planningId,
            plannings,
            visible: newVisible,
            defaultTypeAssignmentOrLessons,
            isCatalog,
            lessonId,
            handleSuccessModal,
            showCalendarDates,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results" centered={isCatalog}>
                <ModalHeader toggle={toggle} className='pb-0'>
                    {isCatalog && 
                    <span style={{color: '#2a426d', fontWeight: 'bold'}}>
                        <Translate id="lesson-catalog.assign-lesson-to-plan" options={{ renderInnerHtml: false }} />
                    </span>}
                </ModalHeader>
                {/* If the component is used in the lesson catalog section, attach the planningId and lessonId directly from the props */}
                <ModalBody className='pt-2'>
                    <FormPlanningTeacherManager 
                        defaultTypeAssignmentOrLessons={defaultTypeAssignmentOrLessons} 
                        planningId={planningId} 
                        updateParent={refreshData} 
                        isCatalog={isCatalog}
                        lessonId={lessonId}
                        plannings={plannings}
                        handleSuccessModal={handleSuccessModal}
                    />
                </ModalBody>
                <ModalFooter/>
            </Modal>
        </div>
    );
}

export default ModalPlanningAttachItem;