import React, { useEffect, useState, useCallback } from "react";
import {Modal, ModalHeader, ModalBody, Row, Col, CardBody} from 'reactstrap';
import Settings from "../../../custom-classes/settings";
import FormMultipleChoice from "../form/exams/form-multiple-choice";
import FormOpenQuestion from "../form/exams/form-open-question";
import FormMatch from "../form/exams/form-match";
import FormMixed from "../form/exams/form-mixed";
import {getCustomQuizByTeacher} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
import './modal-exam-preview.css';
import WrapperOverlappingMedia from '../wrapper-overlapping-media';
import { SendAccessLog } from "../../../utils/access_logger";
import SessionManager from "../../../custom-classes/session-manager";

/**
 * To preview the exam/homework created by teacher (for teacher use only).
 */
const ModalExamPreview = ({
        visible,
        quizId,
        preview = false,
        updateParent,
        authorizationCode,
        token
    }) => {
    const [modal, setModal] = useState(visible);
    const [loading, setLoading] = useState(false);
    const [quizModel, setQuizModel] = useState(null);
    const [questionRequestModelList, setQuestionRequestModelList] = useState([]);
    const [quizAttachmentModelList, setQuizAttachmentModelList] = useState([]);
    const [showExpiredLinkMessage, setShowExpiredLinkMessage] = useState(false);
    const [rubricModel, setRubricModel] = useState(null);
    const userDetails = SessionManager.getSessionDetails();

    const callExam = useCallback(() => {
        setLoading(true);
        getCustomQuizByTeacher(quizId)
            .then((response) => {
                setQuizModel(response.data.QuizModel);
                setQuestionRequestModelList(response.data.QuestionRequestModelList);
                setQuizAttachmentModelList(response.data.QuizAttachmentModelList);
                setRubricModel(response.data.QuizRubricModel);
                setLoading(false);
                try{            
                    SendAccessLog({
                        Username: userDetails.Username,
                        InvitationCode: userDetails.InvitationCode,
                        Role: userDetails.Roles[0],
                        AreaId: "4",
                        Details: "Previewed an assignment",
                        AssignmentId: response.data.QuizModel.Id,
                        AssignmentTitle: response.data.QuizModel.Title,
                    })
                }
                catch (error){
                    // empty catch block to avoid breaking the application
                }
            })
            .catch(() => {
                setShowExpiredLinkMessage(true);
                setLoading(false);
            });
    }, [quizId]);

    useEffect(() => {
        callExam();
    }, [callExam]);

    const toggle = () => {
        setModal(!modal);
        updateParent(false);
    };

    return (
        <div>
            <Modal isOpen={modal} toggle={toggle} className="modal-results clear-style-card">
                <ModalHeader toggle={toggle} className="no-print" />
                <ModalBody className="ql-editor ql-snow">
                    <Row>
                        <Col xs={12}>
                            <div className="animated delay-05s fadeIn mt--5">
                                {loading ? (
                                    <div className="text-center">
                                        <BeatLoader color="#00a0b5" />
                                    </div>
                                ) : (
                                    <WrapperOverlappingMedia>
                                        <CardBody>
                                            {quizModel && (quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE ||
                                                quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_SELECTION) && (
                                                <FormMultipleChoice
                                                    preview={preview}
                                                    authorizationCode={authorizationCode}
                                                    token={token}
                                                    quizModel={quizModel}
                                                    questionRequestModelList={questionRequestModelList}
                                                    quizAttachmentModelList={quizAttachmentModelList}
                                                    showResponseExtra={quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE}
                                                />
                                            )}

                                            {quizModel && quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_OPEN && (
                                                <FormOpenQuestion
                                                    preview={preview}
                                                    authorizationCode={authorizationCode}
                                                    token={token}
                                                    quizModel={quizModel}
                                                    questionRequestModelList={questionRequestModelList}
                                                    quizAttachmentModelList={quizAttachmentModelList}
                                                    rubricModel={rubricModel}
                                                />
                                            )}

                                            {quizModel && quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_MATCH && (
                                                <FormMatch
                                                    preview={preview}
                                                    authorizationCode={authorizationCode}
                                                    token={token}
                                                    quizModel={quizModel}
                                                    questionRequestModelList={questionRequestModelList}
                                                    quizAttachmentModelList={quizAttachmentModelList}
                                                />
                                            )}

                                            {quizModel && quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_MIX && (
                                                <FormMixed
                                                    preview={preview}
                                                    authorizationCode={authorizationCode}
                                                    token={token}
                                                    quizModel={quizModel}
                                                    questionRequestModelList={questionRequestModelList}
                                                    quizAttachmentModelList={quizAttachmentModelList}
                                                    rubricModel={rubricModel}
                                                />
                                            )}
                                        </CardBody>
                                    </WrapperOverlappingMedia>
                                )}
                            </div>
                        </Col>
                    </Row>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalExamPreview;