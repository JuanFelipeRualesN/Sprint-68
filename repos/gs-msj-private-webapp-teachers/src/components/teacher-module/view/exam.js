/**
 * Created by portiz on 05/21/2020.
 */
import React, { useState, useEffect } from 'react';
import {Card, CardBody, CardTitle, Col, Row, FormGroup, Label} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import Settings from "../../../custom-classes/settings";
import FormMultipleChoice from '../form/exams/form-multiple-choice';
import QueryString from "query-string";
import {getCustomQuiz} from "../../../api/teachers";
import FormOpenQuestion from "../form/exams/form-open-question";
import FormMatch from "../form/exams/form-match";
import SessionManager from "../../../custom-classes/session-manager";
import {Link} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowAltCircleLeft} from "@fortawesome/free-solid-svg-icons";
import FileUploadQuiz from "../file-uploader/file-upload-quiz";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import './style.css';

/**
 * The component (window) to submit exams by student.
 */
const Exam = ({ location }) => {
    const parsedString = QueryString.parse(location.search);
    const [attachmentsURL, setAttachmentsURL] = useState([]);
    const [attachmentsHREF, setAttachmentsHREF] = useState([]);
    const [attachmentsName, setAttachmentsName] = useState([]);
    const [authorizationCode] = useState(
        typeof parsedString !== 'undefined' && typeof parsedString.authorization !== 'undefined' && parsedString.authorization !== null ? parsedString.authorization : null
    );
    const [fromWeb] = useState(
        typeof parsedString !== 'undefined' && typeof parsedString.fromWeb !== 'undefined' && parsedString.fromWeb !== null ? parsedString.fromWeb : null
    );
    const [quizModel, setQuizModel] = useState(null);
    const [questionRequestModelList, setQuestionRequestModelList] = useState([]);
    const [quizAttachmentModelList, setQuizAttachmentModelList] = useState([]);
    const [token, setToken] = useState(
        typeof parsedString !== 'undefined' && typeof parsedString.token !== 'undefined' && parsedString.token !== null ? parsedString.token : null
    );
    const [showExpiredLinkMessage, setShowExpiredLinkMessage] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    const callExam = () => {
        let apiCall = getCustomQuiz(authorizationCode, token);
        apiCall.then((response) => {
            if(response.status !== 200 && response.status !== 201) {
                setShowExpiredLinkMessage(true);
                return;
            }
            setQuizModel(response.data.QuizModel);
            setQuestionRequestModelList(response.data.QuestionRequestModelList);
            setQuizAttachmentModelList(response.data.QuizAttachmentModelList);
        });
    };

    useEffect(() => {
        // Overwrite token if connected
        if(SessionManager.getSession() !== null) {
            setToken(SessionManager.getSession());
        }
    }, []);

    useEffect(() => {
        if (token) {
            callExam();
        }
    }, [token, authorizationCode]);

    useEffect(() => {
        if(!fromWeb) {
            document.body.classList.add('exam-background');
        }
    }, [fromWeb]);

    const setUploadedFile = (fileUrl, fileName) => {
        const newAttachmentNames = [...attachmentsName, fileName];
        const newAttachments = [...attachmentsURL, fileUrl];
        const newAttachmentsHREFs = [...attachmentsHREF, <a href={fileUrl}>{fileName}</a>];

        setAttachmentsName(newAttachmentNames);
        setAttachmentsURL(newAttachments);
        setAttachmentsHREF(newAttachmentsHREFs);
    };

    // Renders the main dashboard
    const listAttachmentsItems = attachmentsHREF.map((fileDetail) =>
        <li>{fileDetail}</li>
    );

    if (typeof token === 'undefined' || token === null || typeof authorizationCode === 'undefined'  || authorizationCode === null || showExpiredLinkMessage) {
        return(
            <div>
                <Card body className="card-white animated delay-05s fadeIn">
                    <CardBody>
                        <h3><Translate id="exam.invalid"/></h3>
                    </CardBody>
                </Card>
            </div>
        );
    }

    return (
        <div>
            <Row>
                <Col xs={12}>
                    <Card body className="card-white animated delay-05s fadeIn">
                        <CardTitle>
                            {fromWeb ?
                                <Col xs={{size:12}}>
                                    <Link to="/dashboard" className="btn-default button-primary-custom">
                                        <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                        <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </Col> : null
                            }
                        </CardTitle>
                        <CardBody>
                            {fromWeb ?
                                <Row>
                                    <Col xs={12}>
                                        <FormGroup row>
                                            <Label for="exampleFile" sm={2}><Translate id="messages.attach-file"
                                                                                       options={{renderInnerHtml: true}}/></Label>
                                            <Col sm={10}>
                                                {listAttachmentsItems}
                                                <FileUploadQuiz authorizationCode={authorizationCode} updateParent={setUploadedFile} token={token}/>
                                            </Col>
                                        </FormGroup>
                                    </Col>
                                </Row> : null
                            }
                            {quizModel != null &&
                            (quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE ||
                                quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_SELECTION)
                                ?
                                <FormMultipleChoice authorizationCode={authorizationCode}
                                                    token={token}
                                                    quizModel={quizModel}
                                                    questionRequestModelList={questionRequestModelList}
                                                    quizAttachmentModelList={quizAttachmentModelList}
                                                    showResponseExtra={quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE}
                                /> : null}

                            {quizModel != null &&
                            quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_OPEN
                                ?
                                <FormOpenQuestion authorizationCode={authorizationCode}
                                                  token={token}
                                                  quizModel={quizModel}
                                                  questionRequestModelList={questionRequestModelList}
                                                  quizAttachmentModelList={quizAttachmentModelList}
                                /> : null}

                            {quizModel != null &&
                            quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_MATCH
                                ?
                                <FormMatch authorizationCode={authorizationCode}
                                           token={token}
                                           quizModel={quizModel}
                                           questionRequestModelList={questionRequestModelList}
                                           quizAttachmentModelList={quizAttachmentModelList}
                                /> : null}

                        </CardBody>
                    </Card>
                </Col>
            </Row>
        </div>
    );
};

export default withLocalize(Exam);