/**
 * Created by portiz 2020/05/20
 */
import React, { useState }  from 'react';
import {Button, Modal, ModalHeader, ModalBody, Row, Col, FormGroup, Label} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {sendTeacherPlanToStudentLesson} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCheckCircle} from "@fortawesome/free-solid-svg-icons";
import SelectorMultiGroups from "../selector/selector-multi-groups";
/** End of Multiple Language imports **/

/**
 * Modal to send the planning to the students.
 */
const ModalPlanningSendStudent = ({ planningId, planningName, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [messageReturnedAPI, setMessageReturnedAPI] = useState('');
    const [recipients, setRecipients] = useState([]);
    const [recipientsGroup, setRecipientsGroup] = useState([]);
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);
    const [loading, setLoading] = useState(false);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            planningId,
            planningName,
            messageReturnedAPI,
            recipients,
            recipientsGroup,
            showSuccessMessage,
            showErrorMessage,
            visible: newVisible,
        });
    };

    // Toggles when yes
    const toggleYes = () => {
        setLoading(true);

        let apiCall = sendTeacherPlanToStudentLesson(recipientsGroup.concat(recipients), planningId);
        apiCall.then((response) => {
            setLoading(false);
            
            if(response.status === 200 || response.status === 201)
            {
                let message = '';
                for(let i=0; i < response.data.Messages.length; i++)
                {
                    message += response.data.Messages[i] + '<br/>';
                }
                setShowSuccessMessage(true);
                setMessageReturnedAPI(message);
            }
            else
            {
                setIsVisible(false);
            }
        });
    };

    // Toggles when no
    const toggleNo = () => {
        setIsVisible(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            planningId,
            planningName,
            messageReturnedAPI,
            recipients,
            recipientsGroup,
            showSuccessMessage,
            showErrorMessage,
            visible: false,
        });
    };

    const setSelectedGroups = (e) => {
        let toAdd = [];
        for(let i=0; i < e.length;i++)
        {
            toAdd.push(e[i].value);
        }
        setRecipientsGroup(toAdd);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="planning.send-planning-to-students" />
                </ModalHeader>
                <ModalBody>
                    {showSuccessMessage ?
                        <div className="animated fadeIn text-left">
                            <h3>
                                <FontAwesomeIcon icon={faCheckCircle} style={{color: 'green'}} className="float-left margin-right-10"/>{' '}
                                <div className="content" dangerouslySetInnerHTML={{__html: messageReturnedAPI}}/>
                            </h3>
                        </div>
                        :
                        <div>
                            <Translate id="planning.select-students-planning" />
                            <hr/>
                            <Row>
                                <Col xs={12}>
                                    <FormGroup row>
                                        <Label for="to" sm={4}><Translate id="planning.send-by-group" options={{renderInnerHtml: true}}/></Label>
                                        <Col sm={8}>
                                            <SelectorMultiGroups updateParent={setSelectedGroups}/>
                                        </Col>
                                    </FormGroup>
                                    {/*<FormGroup row>
                                        <Label for="to" sm={4}><Translate id="planning.send-by-student" options={{renderInnerHtml: true}}/></Label>
                                        <Col sm={8}>
                                            <SelectorMultiStudent updateParent={setSelectedStudents}/>
                                        </Col>
                                    </FormGroup>*/}
                                </Col>
                            </Row>
                            
                            <div className="modal-footer primary-button p-0">
                                <Button 
                                    disabled={(recipientsGroup.length === 0 && recipients.length === 0) || loading ? "disabled" : ""} 
                                    color="none" 
                                    onClick={toggleYes}
                                    variant="none"
                                    className="button-primary-custom"
                                >
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#fff"}

                                        />: null}{' '}
                                    <Translate id="planning.send" options={{renderInnerHtml: true}}/>
                                </Button>{' '}
                                    <Button color="secondary" onClick={toggleNo}>
                                        <Translate id="planning.cancel" />
                                    </Button>
                            </div>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalPlanningSendStudent;