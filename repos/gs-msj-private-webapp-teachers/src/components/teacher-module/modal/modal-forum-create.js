/**
 * Created by portiz 2020/05/20
 */
import React, { useState, useEffect }from 'react';
import {Modal, ModalHeader, ModalBody, Row, Col} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import ForumCreationAndView from '../forum/forum-creation-view-handle';
/** End of Multiple Language imports **/

/**
 * Modal to create a new forum
 */
const ModalForumCreate = ({ teacherPlanId, studentId, visible, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setIsVisible(visible);
    }, [visible]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            loading,
            teacherPlanId,
            studentId,
            visible: newVisible,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="forum.create-forum" />
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                            <ForumCreationAndView isCreation={true} teacherPlanId={teacherPlanId} updateParent={toggle}/>
                        </Col>
                    </Row>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalForumCreate;