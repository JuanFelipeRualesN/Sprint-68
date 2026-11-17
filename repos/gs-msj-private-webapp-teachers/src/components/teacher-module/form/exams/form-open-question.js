import React, { useState, useCallback } from "react";
import {Button, Card, Col, Row, CardBody, Alert} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {submitStudentCustomQuiz} from "../../../../api/teachers";
import {BeatLoader} from "react-spinners";

/**
 * Component to view the quiz details. (Open Question)
 */
const FormOpenQuestion = ({
                              authorizationCode,
                              preview = false,
                              quizModel,
                              questionRequestModelList,
                              quizAttachmentModelList,
                              token,
                              submitAvailable = true,
                                rubricModel = null,
                          }) => {
    const [questionQuizList, setQuestionQuizList] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [showMessageError, setShowMessageError] = useState(false);
    const [apiErrorMessage, setApiErrorMessage] = useState("");

    const handleChangeText = (userResponseId, questionId, event) => {
        const value = event.currentTarget.value;
        const updatedList = [...questionQuizList];
        const index = updatedList.findIndex(q => q.QuestionId === questionId);

        if (index >= 0) {
            updatedList[index].ResponseExtra = value;
        } else {
            updatedList.push({
                QuestionId: questionId,
                ResponseExtra: value
            });
        }

        setQuestionQuizList(updatedList);
    };

    const saveResults = useCallback(async () => {
        setLoading(true);

        // Attach responses as expected by the API
        const questionsWithResponses = questionQuizList.map(q => ({
            ...q,
            QuestionResponses: [{ ResponseExtra: q.ResponseExtra }]
        }));

        try {
            const response = await submitStudentCustomQuiz(token, authorizationCode, questionsWithResponses);

            if (response.status !== 200 && response.status !== 201) {
                setApiErrorMessage(response.data?.Messages?.[0] || "Unknown error");
                setShowMessageError(true);
            } else {
                setShowSuccessMessage(true);
            }
        } catch (e) {
            setApiErrorMessage("Submission failed.");
            setShowMessageError(true);
        }

        setLoading(false);
    }, [authorizationCode, questionQuizList, token]);

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
            </a>
            <br />
        </div>
    ));

    const questions = questionRequestModelList.map(item => (
        <div key={item.QuizQuestionModel.Id} className="card-little-bit-gray padding-20">
            <Row>
                <Col xs={12}>
                    <p>
                        <b>
                            <div dangerouslySetInnerHTML={{ __html: item.QuizQuestionModel.Desc }} />
                        </b>
                    </p>
                </Col>
            </Row>
            <Row>
                <Col xs={12}>
          <textarea
              className="form-control"
              rows="4"
              disabled={preview}
              onChange={!preview ? (e) => handleChangeText(item.QuizQuestionModel.Id, item.QuizQuestionModel.Id, e) : undefined}
              value={item.QuizQuestionModel.ResponseExtra || ""}
          />
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
                    <Row>
                        <Col xs={12}>
                            <p>
                                <div dangerouslySetInnerHTML={{ __html: quizModel.Instruction }} />
                            </p>
                        </Col>
                    </Row>
                    {rubricModel !== null && rubricModel.Desc !== "" &&
                        <Row>
                            <Col xs={12}>
                                <p>
                                    <div dangerouslySetInnerHTML={{ __html: rubricModel.Desc }} />
                                </p>
                            </Col>
                        </Row>
                    }
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
        </div>
    );
};

export default FormOpenQuestion;