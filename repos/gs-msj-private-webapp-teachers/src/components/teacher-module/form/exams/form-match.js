import React, { useEffect, useState, useCallback } from "react";
import {Button, Card, Col, Row, Input, CardBody, Alert} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {submitStudentCustomQuiz} from "../../../../api/teachers";
import {submitGenialQuiz} from "../../../../api/gsweb";
import {BeatLoader} from "react-spinners";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";
import ModalResults from "../../../shared-module-components/modal/modal-results";

/**
 * Component to render a match quiz.
 */
const FormMatch = ({
                       authorizationCode,
                       extraInfo = '',
                       isGenialQuiz = false,
                       level = null,
                       lessonId = null,
                       subject = null,
                       preview = false,
                       quizModel,
                       questionRequestModelList,
                       quizAttachmentModelList,
                       token,
                       submitAvailable = true,
                   }) => {
    const [questionQuizList, setQuestionQuizList] = useState([]);
    const [responses, setResponses] = useState([]);
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [loading, setLoading] = useState(false);
    const [modalResultsVisible, setModalResultsVisible] = useState(false);
    const [results, setResults] = useState({});
    const [showMessageError, setShowMessageError] = useState(false);
    const [apiErrorMessage, setApiErrorMessage] = useState("");

    const shuffleArray = useCallback((array) => {
        for (let i = array.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [array[i], array[j]] = [array[j], array[i]];
        }
    }, []);

    const prepareResponsesColumns = useCallback(() => {
        let shuffled = questionRequestModelList.map(q => ({
            QuestionId: q.QuizQuestionModel.Id,
            ResponseId: q.QuizQuestionResponseModelList[0].Id,
            Response: q.QuizQuestionResponseModelList[0].Response,
        }));

        shuffleArray(shuffled);

        const letters = 'abcdefghijklmnopqrstuvwxyz'.split('');
        shuffled.forEach((r, i) => r.Letter = letters[i]);

        setResponses(shuffled);
    }, [questionRequestModelList, shuffleArray]);

    useEffect(() => {
        prepareResponsesColumns();
    }, [prepareResponsesColumns]);

    const handleChangeText = (questionId, element) => {
        const letter = element.currentTarget.value;
        const matched = responses.find(r => r.Letter === letter);
        if (!matched) return;

        const updatedList = [...questionQuizList];
        const index = updatedList.findIndex(q => q.QuestionId === questionId);

        const newResponse = {
            QuestionId: questionId,
            QuestionResponses: [{ ResponseUserId: matched.ResponseId }]
        };

        if (index >= 0) {
            updatedList[index] = newResponse;
        } else {
            updatedList.push(newResponse);
        }

        setQuestionQuizList(updatedList);
    };

    const saveResults = async () => {
        setLoading(true);

        if (!isGenialQuiz) {
            try {
                const response = await submitStudentCustomQuiz(token, authorizationCode, questionQuizList);
                if (![200, 201].includes(response.status)) {
                    setApiErrorMessage(response.data.Messages?.[0] || "Unknown error");
                    setShowMessageError(true);
                } else {
                    setShowSuccessMessage(true);
                }
            } catch (e) {
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
                LevelCode: level,
            }));

            const UserResponseRequestModel = {
                UserResponseModelList,
                Token: SessionManager.getSession(),
                LangCode: LanguageManager.getLanguage()
            };

            try {
                const response = await submitGenialQuiz(UserResponseRequestModel);
                const resData = response.data;
                setResults({
                    coinsTotal: resData.UserRewardModel.CoinsTotal,
                    diamondsTotal: resData.UserRewardModel.DiamondsTotal,
                    totalCorrect: resData.UserQuizResultModel.TotalCorrect,
                    totalQuestions: resData.UserQuizResultModel.TotalQuestions,
                    totalPercent: resData.UserQuizResultModel.TotalPercent,
                    tryNo: resData.UserQuizResultModel.TryNo
                });
                setModalResultsVisible(true);
            } catch (e) {
                setApiErrorMessage("Genial quiz submission failed.");
                setShowMessageError(true);
            }

            setLoading(false);
        }
    };

    if (showSuccessMessage) {
        return <h2><Translate id="exam.submit-success" options={{ renderInnerHtml: true }} /></h2>;
    }

    const attachments = (quizAttachmentModelList || []).map(item => (
        <div key={item.Id}>
            <a href={item.AttachmentUrl} download>
                <Translate id="exam.attachment" />{": "}{item.Id}
            </a>
            <br />
        </div>
    ));

    const responseList = responses.map(item => (
        <div className="card-little-bit-gray padding-20" key={item.ResponseId}>
            <Row>
                <Col xs={3}>{item.Letter}.</Col>
                <Col xs={9}><div dangerouslySetInnerHTML={{ __html: item.Response }} /></Col>
            </Row>
            <hr />
        </div>
    ));

    const questionList = questionRequestModelList.map(item => (
        <div className="card-little-bit-gray padding-20" key={item.QuizQuestionModel.Id}>
            <Row>
                <Col xs={3}>
                    <Input
                        type="text"
                        disabled={preview}
                        onChange={!preview ? (e) => handleChangeText(item.QuizQuestionModel.Id, e) : undefined}
                        value={item.QuizQuestionModel.SelectedLetter || ""}
                    />
                </Col>
                <Col xs={9}>
                    <div dangerouslySetInnerHTML={{ __html: item.QuizQuestionModel.Desc }} />
                </Col>
            </Row>
            <hr />
        </div>
    ));

    return (
        <div>
            <Card className="card-white">
                <CardBody>
                    <Row><Col xs={12}><h3>{quizModel.Title}</h3></Col></Row>
                    <Row><Col xs={12}><div dangerouslySetInnerHTML={{ __html: quizModel.Instruction }} /></Col></Row>
                    <Row><Col xs={12}><div dangerouslySetInnerHTML={{ __html: extraInfo }} /></Col></Row>
                    <Row><Col xs={12}><hr /></Col></Row>
                    <Row><Col xs={12}>{attachments}</Col></Row>
                    <Row>
                        <Col xs={12} md={6}>{questionList}</Col>
                        <Col xs={12} md={6}>{responseList}</Col>
                    </Row>
                    <Row>
                        {showMessageError &&
                            <Col xs={12}><Alert>{apiErrorMessage}</Alert></Col>
                        }

                        <Col xs={12}>
                            {questionRequestModelList.length !== questionQuizList.length && !preview &&
                                <Alert color="info">
                                    <Translate id="exam.responses-pending" options={{ renderInnerHtml: true }} />
                                </Alert>
                            }

                            {submitAvailable && !preview &&
                                <Button
                                    color="success"
                                    disabled={
                                        questionRequestModelList.length !== questionQuizList.length || loading
                                    }
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

            {modalResultsVisible && (
                <ModalResults
                    visible={modalResultsVisible}
                    lessonId={lessonId}
                    {...results}
                />
            )}
        </div>
    );
};

export default FormMatch;