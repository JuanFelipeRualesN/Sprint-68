/**
 * Created by portiz on 6/12/20.
 */
import React, {useState, useEffect} from 'react';
import {Modal, ModalHeader, ModalBody, Row, Col, CardBody} from 'reactstrap';
/** Multiple Language imports **/

import Settings from "../../../custom-classes/settings";
import FormMultipleChoice from "../form/exams/form-multiple-choice";
import FormOpenQuestion from "../form/exams/form-open-question";
import FormMatch from "../form/exams/form-match";
import {BeatLoader} from "react-spinners";
import PlanningCalendar from "../calendar/planning-calendar";
/** End of Multiple Language imports **/

/**
 * To preview the exam/homework created by teacher (for teacher use only).
 */
const ModalCalendar = ({ teacherPlanId, visible, updateParent }) => {
    const [modalVisible, setModalVisible] = useState(visible);

    // Hide or shows the window
    const toggle = () => {
        setModalVisible(!modalVisible);
        updateParent(false);
    };

    return (
        <div>
            <Modal isOpen={modalVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle} className="no-print" />
                <ModalBody>
                    <PlanningCalendar teacherPlanId={teacherPlanId} />
                </ModalBody>
            </Modal>
        </div>
    );
}
export default  ModalCalendar;