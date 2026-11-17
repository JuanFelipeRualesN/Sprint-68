import React, { useState, useCallback } from "react";
import {Button, Card, Col, Row, Input, CardBody, Alert,Label} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {submitStudentCustomQuiz} from "../../../../api/teachers";
import {submitGenialQuiz} from "../../../../api/gsweb";
import {BeatLoader} from "react-spinners";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";
import ModalResults from "../../../shared-module-components/modal/modal-results";

/**
 * Component to view the quiz details. (Multiple Choice)
 */
const FormMultipleChoice = ({
                                preview = false,
                                authorizationCode,
                                extraInfo = '',
                                isGenialQuiz = false,
                                level = null,
                                lessonId = null,
                                subject = null,
                                quizModel,
                                questionRequestModelList,
                                quizAttachmentModelList,
                                token,
                                showResponseExtra,
                                submitAvailable = true
                            }) => {
    const [questionQuizList, setQuestionQuizList] = useState([]);
    const [showCorrectResponse, setShowCorrectResponse] = useState(false);
    const [loading, setLoading] = useState(false);
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [showMessageError, setShowMessageError] = useState(false);
    const [apiErrorMessage, setApiErrorMessage] = useState("");
    const [modalResultsVisible, setModalResultsVisible] = useState(false);
    const [results, setResults] = useState(null);

    const handleChange = (userResponseId, questionId) => {
        const updated = [...questionQuizList];
        const index = updated.findIndex(q => q.QuestionId === questionId);
        const response = { ResponseUserId: userResponseId };

        if (index >= 0) {
            updated[index].QuestionResponses = [response];
        } else {
            updated.push({ QuestionId: questionId, QuestionResponses: [response] });
        }

        setQuestionQuizList(updated);
    };

    const handleChangeText = (userResponseId, questionId, event) => {
        const updated = [...questionQuizList];
        const response = {
            ResponseUserId: userResponseId,
            ResponseExtra: event.currentTarget.value
        };
        const index = updated.findIndex(q => q.QuestionId === questionId);

        if (index >= 0) {
            updated[index].QuestionResponses = [response];
        } else {
            updated.push({ QuestionId: questionId, QuestionResponses: [response] });
        }

        setQuestionQuizList(updated);
    };

    const saveResults = useCallback(async () => {
        setLoading(true);

        if (!isGenialQuiz) {
            try {
                const response = await submitStudentCustomQuiz(token, authorizationCode, questionQuizList);
                if (![200, 201].includes(response.status)) {
                    setApiErrorMessage(response.data?.Messages?.[0] || "Unknown error");
                    setShowMessageError(true);
                } else {
                    setShowSuccessMessage(true);
                }
            } catch {
                setApiErrorMessage("Submission failed.");
                setShowMessageError(true);
            }
            setLoading(false);
        } else {
            const UserResponseModelList = questionQuizList.map(q => ({
                QuizId: quizModel.Id,
                QuestionId: q.QuestionId,
                ResponseId: q.QuestionResponses[0].ResponseUserId,
                SubjectCode: subject,
                LevelCode: level
            }));

            const requestModel = {
                UserResponseModelList,
                Token: SessionManager.getSession(),
                LangCode: LanguageManager.getLanguage()
            };

            try {
                const response = await submitGenialQuiz(requestModel);
                const data = response.data;

                setResults({
                    coinsTotal: data.UserRewardModel.CoinsTotal,
                    diamondsTotal: data.UserRewardModel.DiamondsTotal,
                    totalCorrect: data.UserQuizResultModel.TotalCorrect,
                    totalQuestions: data.UserQuizResultModel.TotalQuestions,
                    totalPercent: data.UserQuizResultModel.TotalPercent,
                    tryNo: data.UserQuizResultModel.TryNo
                });

                setShowCorrectResponse(parseInt(quizModel.IsExam) === 0);
                setModalResultsVisible(true);
            } catch {
                setApiErrorMessage("Genial quiz submission failed.");
                setShowMessageError(true);
            }

            setLoading(false);
        }
    }, [authorizationCode, isGenialQuiz, level, quizModel, questionQuizList, subject, token]);

    if (showSuccessMessage) {
        return (
            <div>
                <h2><Translate id="exam.submit-success" options={{ renderInnerHtml: true }} /></h2>
            </div>
        );
    }

    const attachments = (quizAttachmentModelList || []).map(item => (
        <div key={item.Id}>
            <a href={item.AttachmentUrl} download>
                <Translate id="exam.attachment" />{": "}{item.Id}
            </a><br />
        </div>
    ));

    const questions = questionRequestModelList.map(q => (
        <div key={q.QuizQuestionModel.Id} className="card-little-bit-gray padding-20">
            <Row>
                <Col xs={12}>
                    <p><b><div dangerouslySetInnerHTML={{ __html: q.QuizQuestionModel.Desc }} /></b></p>
                </Col>
            </Row>
            <Row>
                <Col xs={12} className="d-flex flex-column">
                    {q.QuizQuestionResponseModelList.map(res => (
                        <div key={res.Id}>
                            <Label check>
                                <Input
                                    type="radio"
                                    name={q.QuizQuestionModel.Id}
                                    value={res.Id}
                                    disabled={preview}
                                    onChange={!preview ? () => handleChange(res.Id, q.QuizQuestionModel.Id) : undefined}
                                />{" "}
                                {showCorrectResponse && res.Correct === "1" ? (
                                    <div className="green-text" dangerouslySetInnerHTML={{ __html: res.Response }} />
                                ) : (
                                    <div dangerouslySetInnerHTML={{ __html: res.Response }} />
                                )}
                            </Label>
                            <br />
                            {showResponseExtra && (
                                <textarea
                                    rows="4"
                                    disabled={preview}
                                    onChange={!preview ? (e) => handleChangeText(res.Id, q.QuizQuestionModel.Id, e) : undefined}
                                    value={res.ResponseExtra || ""}
                                />
                            )}
                        </div>
                    ))}
                </Col>
            </Row>
            <hr />
        </div>
    ));

    const incomplete = questionRequestModelList.length !== questionQuizList.length;

    return (
        <div>
            <Card className="card-white">
                <CardBody>
                    <Row><Col xs={12}><h3>{quizModel.Title}</h3></Col></Row>
                    <Row><Col xs={12}><div dangerouslySetInnerHTML={{ __html: quizModel.Instruction }} /></Col></Row>
                    <Row><Col xs={12}><div dangerouslySetInnerHTML={{ __html: extraInfo }} /></Col></Row>
                    <Row><Col xs={12}><hr /></Col></Row>
                    <Row><Col xs={12}>{attachments}</Col></Row>
                    <Row><Col xs={12}>{questions}</Col></Row>
                    <Row>
                        {showMessageError &&
                            <Col xs={12}><Alert>{apiErrorMessage}</Alert></Col>
                        }
                        <Col xs={12}>
                            {incomplete && !preview &&
                                <Alert color="info">
                                    <Translate id="exam.responses-pending" options={{ renderInnerHtml: true }} />
                                </Alert>
                            }
                            {submitAvailable && !preview &&
                                <Button
                                    color="success"
                                    disabled={incomplete || loading}
                                    onClick={saveResults}
                                >
                                    {loading && <BeatLoader size={10} color="#fff" />}{" "}
                                    <Translate id="exam.submit" />
                                </Button>
                            }
                        </Col>
                    </Row>
                </CardBody>
            </Card>
            {modalResultsVisible && results &&
                <ModalResults
                    visible={modalResultsVisible}
                    lessonId={lessonId}
                    {...results}
                />
            }
        </div>
    );
};

export default FormMultipleChoice;