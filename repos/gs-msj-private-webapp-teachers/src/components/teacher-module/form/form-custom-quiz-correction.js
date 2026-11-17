import React, {useState, useEffect} from 'react';
import { Button, Card, Col, Row, Input, CardBody, Alert } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { getUserCustomQuizDetails, correctCustomQuizManual } from "../../../api/teachers";
import Editor from '../../common/editor';
import {formatUtcToLocal} from "../../../utils/utils";
import LanguageManager from "../../../custom-classes/language-manager";
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Component to view the quiz details.
 */
const FormCustomQuizCorrection = ({ planQuizId, studentId, studentQuizId, updateParent }) => {
    const [autoCorrect, setAutoCorrect] = useState(false);
    const [quizTitle, setQuizTitle] = useState('');
    const [instructions, setInstructions] = useState('');
    const [totalPercent, setTotalPercent] = useState('');
    const [totalCorrect, setTotalCorrect] = useState(null);
    const [feedback, setFeedback] = useState('');
    const [questions, setQuestions] = useState([]);
    const [status, setStatus] = useState('');
    const [dateSubmitted, setDateSubmitted] = useState('');
    const [customValue, setCustomValue] = useState('');
    const [rubric, setRubric] = useState('');
    const [rubricLoaded, setRubricLoaded] = useState(false);
    const [showMessageError, setShowMessageError] = useState(false);
    const [apiErrorMessage, setApiErrorMessage] = useState('');
    const [attachments, setAttachments] = useState([]);
    const [studentQuizFiles, setStudentQuizFiles] = useState([]);
    const [feedbackEditor, setFeedbackEditor] = useState('');
    const language = LanguageManager.getLanguage();
    const isTeacher = SessionManager.getUserRole() === Settings.ROLE_TEACHER_STR;

    useEffect(() => {
        getUserCustomQuizDetails(studentId, studentQuizId).then((response) => {
            setAutoCorrect(response.data.AutoCorrect);
            setQuizTitle(response.data.QuizTitle);
            setInstructions(response.data.Instructions);
            setTotalPercent(response.data.TotalPercent);
            setTotalCorrect(response.data.TotalCorrect);
            setFeedback(response.data.Feedback || '');
            setQuestions(response.data.QuestionQuizList);
            setStatus(response.data.Status);
            setDateSubmitted(formatUtcToLocal(response.data.DateSubmitted, language));
            setAttachments(response.data.AttachmentURLs || []);
            setCustomValue(response.data.CustomValue);
            setStudentQuizFiles(response.data.UserQuizFileModelList || []);
            setRubric(response.data.Rubric ? '<br/>' + response.data.Rubric : '');
            setRubricLoaded(true);
            // If status is 'C', set the feedback editor with the feedback
            // Only set the rubric if it exists and the quiz status is 'P'
            setFeedbackEditor((response.data.Feedback || '') + (response.data.Rubric && response.data.Status == "P" ? '<br/>' + response.data.Rubric : ''));
        });
    }, [studentId, studentQuizId]);

    const handleChange = (Id, e) => {
        const value = e.currentTarget.value.replace(/\D/g, '').replace(/([0-9])([0-9]{2})$/, '$1.$2').replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ',');
        const updatedQuestions = questions.map(q => ({
            ...q,
            QuestionResponses: q.QuestionResponses.map(r => r.Id === Id ? { ...r, AssignedValue: value } : r)
        }));
        setQuestions(updatedQuestions);
    };

    const validateAssignedValue = () => {
        if (status === 'C') return true;
        return questions.every(q => q.QuestionResponses.every(r => r.AssignedValue));
    };

    const saveCorrection = () => {
        correctCustomQuizManual(questions, studentQuizId, feedbackEditor, planQuizId, autoCorrect).then((response) => {
            setShowMessageError(true);
            setApiErrorMessage(response.data.Messages[0]);
            if (response.status === 200 || response.status === 201) {
                updateParent(true);
            }
        });
    };

    const attachmentsList = (attachments || []).map(item => (
        <a href={item.AttachmentUrl} download key={item.Id}><Translate id="progress.file" /> {item.Id}</a>
    ));

    const studentAttachmentsList = (studentQuizFiles || []).map(item => (
        <a href={item.FileUrl} download key={item.Id}><Translate id="progress.file" /> {item.Id}</a>
    ));

    const questionList = questions.map((items, idx) => (
        <div className="card-little-bit-gray padding-20" key={idx}>
            <Row><Col xs={12}><p><b><span className="ql-editor" dangerouslySetInnerHTML={{ __html: items.Question }} /></b></p></Col></Row>
            <Row>
                {items.QuestionResponses.map((resp, j) => (
                    <Col xs={12} key={j}>
                        {resp.CorrectAnswer ? (
                            <span className="green-text ql-editor"><span dangerouslySetInnerHTML={{ __html: resp.Response }} /></span>
                        ) : (
                            <span className="ql-editor" dangerouslySetInnerHTML={{ __html: resp.Response }} />
                        )}
                        {!autoCorrect && status !== 'C' && (
                            <Row>
                                <Col xs={6}>
                                    <p><Translate id="progress.assign-score" /></p>
                                    <small>
                                        <Translate id="progress.assign-score-note" /><b> {customValue} </b>
                                        <Translate id="progress.assign-score-note-2" />
                                    </small>
                                    {(resp.SelectedByUser || items.QuestionResponses.length === 1) && (
                                        <Input
                                            type="text"
                                            disabled={!resp.SelectedByUser && items.QuestionResponses.length > 1}
                                            onChange={(e) => handleChange(resp.Id, e)}
                                            value={resp.AssignedValue}
                                        />
                                    )}
                                </Col>
                            </Row>
                        )}
                    </Col>
                ))}
            </Row>
            <Row>
                <Col xs={12}>
                    <Translate id="progress.student-response" />: <b><span dangerouslySetInnerHTML={{ __html: items.QuestionResponses[0].UserSelectedResponseText }} /></b>
                    <br />{items.QuestionResponses[0].ResponseExtra}
                </Col>
            </Row>
            <hr />
        </div>
    ));

    return (
        <div>
            <Card className="card-white">
                <CardBody>
                    <Row>
                        <Col xs={12}><h3>{quizTitle}</h3></Col>
                    </Row>
                    <Row>
                        <Col xs={12} md={6}>{totalPercent}%</Col>
                        <Col xs={12} md={6}><small>{dateSubmitted}</small></Col>
                    </Row>
                    <Row><Col xs={12}><hr /></Col></Row>
                    <Row><Col xs={12}><span className="ql-editor" dangerouslySetInnerHTML={{ __html: instructions }} /></Col></Row>
                    <Row>{attachmentsList}</Row>
                    <Row><Col xs={12}>{questionList}</Col></Row>
                    <Row>
                        <Col xs={12}>{studentAttachmentsList.length > 0 && <Translate id="progress.student-attachments" />}</Col>
                        <Col xs={12}>{studentAttachmentsList}</Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <Translate id="progress.feedback" /><br />
                                    <small><Translate id="progress.feedback-instructions" /></small>
                            {rubricLoaded && isTeacher && (status === 'P' || status === 'C') && (
                                <Editor
                                    defaultValue={feedbackEditor}
                                    onChange={setFeedbackEditor}
                                />
                            )}
                            {rubricLoaded && !isTeacher && status === 'C' && (
                                // Native inputs can't render HTML, and Editor's readOnly mode renders blank content,
                                // so non-teachers get a disabled-looking box with the HTML rendered inside instead.
                                <div
                                    className="form-control ql-editor"
                                    style={{ height: 'auto', backgroundColor: '#e9ecef', opacity: 1, pointerEvents: 'none' }}
                                    dangerouslySetInnerHTML={{ __html: feedbackEditor }}
                                />
                            )}
                        </Col>
                    </Row>
                    <Row>
                        {showMessageError && (
                            <Col xs={12}><Alert>{apiErrorMessage}</Alert></Col>
                        )}
                        <Col xs={12}>
                            {isTeacher && (status === 'P' || status === 'C') && (
                                <Button color="success" onClick={saveCorrection} disabled={!validateAssignedValue()}>
                                    <Translate id="progress.save" />
                                </Button>
                            )}
                        </Col>
                    </Row>
                </CardBody>
            </Card>
        </div>
    );
};

export default FormCustomQuizCorrection;