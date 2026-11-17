/**
 * Created by portiz on 07/20/2020.
 */
import React, { useState, useEffect } from 'react';
import {Alert, Card, CardBody, Col, Container, Row} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import Settings from "../../../custom-classes/settings";
import FormMultipleChoice from '../form/exams/form-multiple-choice';
import {getQuiz} from "../../../api/athenas";
import FormMatch from "../form/exams/form-match";
import {Link, Redirect} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowAltCircleLeft, faFileContract} from "@fortawesome/free-solid-svg-icons";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import './style.css';

/**
 * The component (window) to submit quizzes from Genial by student.
 */
const QuizGenial = ({ location }) => {
    const [extraInfo, setExtraInfo] = useState(null);
    const [quizId, setQuizId] = useState(location.quizId ?? null);
    const [lessonId, setLessonId] = useState(location.lessonId ?? null);
    const [level, setLevel] = useState(location.level ?? null);
    const [onlyView, setOnlyView] = useState(location.onlyView ?? false);
    const [isTeacher, setIsTeacher] = useState(location.isTeacher ?? false);
    const [planningId, setPlanningId] = useState(location.planningId ?? null);
    const [planningName, setPlanningName] = useState(location.planningName ?? null);
    const [quizTitle, setQuizTitle] = useState(null);
    const [quizModel, setQuizModel] = useState(null);
    const [questionRequestModelList, setQuestionRequestModelList] = useState([]);
    const [quizAttachmentModelList, setQuizAttachmentModelList] = useState([]);
    const [isPractice, setIsPractice] = useState(false);
    const [subject, setSubject] = useState(location.subject ?? null);
    const [loading, setLoading] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        callExam();
    }, []);

    const callExam = () => {
        let apiCall = getQuiz(quizId);
        apiCall.then((response) => {
            setExtraInfo(typeof response.data.QuizRequestModel.QuizExtraInfoModel !== 'undefined' && response.data.QuizRequestModel.QuizExtraInfoModel !== null ? response.data.QuizRequestModel.QuizExtraInfoModel.ExtraInfo : null);
            setQuizTitle(response.data.QuizRequestModel.QuizModel.Title + ' (' + response.data.QuizRequestModel.QuizModel.LevelOfKnowledge + ') ');
            setQuizModel(response.data.QuizRequestModel.QuizModel);
            setQuestionRequestModelList(response.data.QuizRequestModel.QuestionRequestModelList);
        });
    };

    if (typeof quizId === 'undefined' || quizId === null)
    {
        if(typeof lessonId === 'undefined' || lessonId == null) {
            return (<Redirect to={{
                pathname: "/dashboard"
            }}/>);
        }
        else
        {
            return (<Redirect to={{
                pathname: '/lesson',
                lessonId: lessonId
            }}/>);
        }
    }

    // Renders the main dashboard
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <Container className="container-wrapper">
                    <Row>
                        <Col xs={{size:12}}>
                            <h2 className="font-roboto">
                                <FontAwesomeIcon icon={faFileContract}/>{' '}
                                {quizTitle != null ? quizTitle : null}
                            </h2>

                        </Col>
                        {isTeacher ?
                        <Col xs={{size:12}}>
                            <Link to={{
                                pathname: '/lesson-presentation',
                                lessonId: lessonId,
                                planningId: planningId,
                                planningName: planningName,
                            }} className="btn-default button-primary-custom">
                                <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>
                        </Col> :
                            <Col xs={{size:12}}>
                                <Link to={{
                                    pathname: '/lesson',
                                    lessonId: lessonId
                                }} className="btn-default button-primary-custom">
                                    <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                    <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                                </Link>
                            </Col>
                        }

                    </Row>
                    <hr/>

                    <Row>
                        <Col xs={{size:12}}>
                            <Alert>
                                {isTeacher ?
                                    <Translate id="planning.quiz-view-note" options={{renderInnerHtml: true}}/> : null
                                }
                            </Alert>
                        </Col>
                    </Row>
                    <Row>

                        <Col xs={12}>
                            <Card body className="card-white animated delay-05s fadeIn">
                                <CardBody>
                                    {quizModel != null &&
                                    (quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE ||
                                        quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_SELECTION)
                                        ?
                                        <FormMultipleChoice authorizationCode={''}
                                                            submitAvailable={!onlyView}
                                                            lessonId={lessonId}
                                                            isGenialQuiz={true}
                                                            token={''}
                                                            quizModel={quizModel}
                                                            questionRequestModelList={questionRequestModelList}
                                                            quizAttachmentModelList={quizAttachmentModelList}
                                                            showResponseExtra={quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE}
                                                            level={level}
                                                            subject={subject}
                                                            extraInfo={extraInfo}
                                        /> : null}
                                    {quizModel != null &&
                                    quizModel.QuizTypeId === Settings.CUSTOM_QUIZ_TYPE_MATCH
                                        ?
                                        <FormMatch authorizationCode={''}
                                                   isGenialQuiz={true}
                                                   token={''}
                                                   lessonId={lessonId}
                                                   quizModel={quizModel}
                                                   questionRequestModelList={questionRequestModelList}
                                                   quizAttachmentModelList={quizAttachmentModelList}
                                                   level={level}
                                                   subject={subject}
                                                   extraInfo={extraInfo}
                                                   submitAvailable={!onlyView}
                                        /> : null}

                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </Container>
            </div>
            {loading ? <LoaderFullWindow/> : null}

        </div>
    );
};

export default withLocalize(QuizGenial);