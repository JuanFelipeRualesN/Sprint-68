import React, {useEffect, useState} from 'react';
import {Modal, ModalHeader, ModalBody} from 'reactstrap';
import FormCustomQuizCorrection from "../form/form-custom-quiz-correction";

/**
 * Modal to see custom quizzes details
 */
const ModalCustomQuizCorrection = ({ visible, studentId, studentQuizId, planQuizId, updateParent }) => {
    const [modalVisible, setModalVisible] = useState(visible);
    const [currentStudentId, setCurrentStudentId] = useState(studentId);
    const [currentStudentQuizId, setCurrentStudentQuizId] = useState(studentQuizId);
    const [currentPlanQuizId, setCurrentPlanQuizId] = useState(planQuizId);

    useEffect(() => {
        setModalVisible(visible);
        setCurrentStudentId(studentId);
        setCurrentStudentQuizId(studentQuizId);
        setCurrentPlanQuizId(planQuizId);
    }, [visible, studentId, studentQuizId, planQuizId]);

    const toggle = () => {
        const newVisibility = !modalVisible;
        setModalVisible(newVisibility);
        updateParent({
            visible: newVisibility,
            studentId: currentStudentId,
            studentQuizId: currentStudentQuizId,
            planQuizId: currentPlanQuizId
        });
    };

    return (
        <Modal isOpen={modalVisible} toggle={toggle} className="modal-results clear-style-card modal-quiz-results">
            <ModalHeader toggle={toggle} />
            <ModalBody>
                <FormCustomQuizCorrection
                    planQuizId={currentPlanQuizId}
                    studentId={currentStudentId}
                    studentQuizId={currentStudentQuizId}
                    updateParent={toggle}
                />
            </ModalBody>
        </Modal>
    );
};

export default ModalCustomQuizCorrection;