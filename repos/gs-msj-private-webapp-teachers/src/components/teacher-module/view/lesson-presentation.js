/**
 * Created by portiz on 05/06/2020.
 */
import React, { useState, useEffect } from 'react';
import {Card, CardBody, Col, Container, Row, Button} from 'reactstrap';
import {Link, Redirect} from "react-router-dom";

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import Iframe from 'react-iframe';
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import {
    getLessonDetailsStudent,
    getLessonExamsHomeschooling,
    getLessonConcept, getLessonDefinitions, getLessonExamples, getLessonPractices
} from "../../../api/gsweb";
import './style.css';
import { getAsset } from '../../../utils/getAssets';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const IconConcept = getAsset('LessonsMenu-icon-Concept');
const IconVocabulary = getAsset('LessonsMenu-icon-Vocabulary');
const IconPractice = getAsset('LessonsMenu-icon-Practice');
const IconChallenge = getAsset('LessonsMenu-icon-Challenge');
const IconExample = getAsset('LessonsMenu-icon-Examples');

/**
 * The component (window) to handle the lesson window for presentations.
 */
const LessonPresentation = ({initialize}) => {
    const lessonDetails = SessionManager.getSelectedLessonToShow();
    
    const [active, setActiveTab] = useState('concept'); // Active tab
    const [concept, setConcept] = useState(null); // HTML that comes from Athenas. It must be dangerously inserted.
    const [containerFluid, setContainerFluid] = useState(false); // Container Fluid to make bigger or smaller lessons main container
    const [definitions, setDefinitions] = useState(null); // List of lessons definitions. Format is HTML that comes from Athenas. It must be dangerously inserted.
    const [definitionUrl, setDefinitionUrl] = useState(null); // Definition URL to be inserted in a frame. This happens only if there is a templated assigned to the lesson.
    const [examples, setExamples] = useState(null); // List of lesson examples (HTML format). It must be dangerously inserted.
    const [exampleUrl, setExampleUrl] = useState(null); // Example URL to be inserted in a link. frame happens only if there is a templated assigned to the lesson.
    const [examList, setExamList] = useState([]); // List of lesson exams
    const [level, setLevel] = useState("1"); // Lesson level
    const [lessonTitle, setLessonTitle] = useState(null);
    const [lessonAverage, setLessonAverage] = useState(null); // The student average for lesson
    const [lessonId, setLessonId] = useState(lessonDetails !== null ? lessonDetails.lessonId : null);
    const [loading, setLoading] = useState(false); // To show or hide loading window or components of loading when loading
    const [planningId, setPlanningId] = useState(lessonDetails !== null ? lessonDetails.planningId : null);
    const [planningName, setPlanningName] = useState(lessonDetails !== null ? lessonDetails.planningName : null);
    const [practiceList, setPracticeList] = useState([]); // The practices
    const [roles, setRoles] = useState(SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles);
    const [subject, setSubject] = useState(null); // Lesson subject
    const [subjectTitle, setSubjectTitle] = useState("");

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        callLessonDescription(lessonId);
        resize();
    }, []);

    const resize = () => {
        setContainerFluid(window.innerWidth <= 760);
    };

    const setActive = (e) => {
        if(e.currentTarget.value === "definitions")
        {
            // Call definitions
            let apiLesson = getLessonDefinitions(lessonId);
            apiLesson.then(((response)=>{
                if(response.data.IsDefinitionALink)
                {
                    setDefinitionUrl(response.data.LessonDefinitionModelList[0].Desc);
                }
                else {
                    let htmlDefinitionString = "";
                    for (let i = 0; i < response.data.LessonDefinitionModelList.length; i++) {

                        if(response.data.LessonDefinitionModelList[i].Name !== null)
                            htmlDefinitionString += '<b class="vocabulary-title">' + response.data.LessonDefinitionModelList[i].Name;

                        htmlDefinitionString += '</b><br/>' + response.data.LessonDefinitionModelList[i].Desc;
                    }
                    setDefinitions(htmlDefinitionString);
                }
            }))
        }
        else if(e.currentTarget.value === "examples")
        {
            // Call examples
            let apiLesson = getLessonExamples(lessonId);
            apiLesson.then((response) => {
                if (response.data.IsExampleALink) {
                    setExampleUrl(response.data.LessonExampleModelList[0].Desc);
                } else {
                    let htmlExampleString = "";
                    for (let i = 0; i < response.data.LessonExampleModelList.length; i++) {
                        if (response.data.LessonExampleModelList[i].Name !== null)
                            htmlExampleString += '<b class="vocabulary-title">' + response.data.LessonExampleModelList[i].Name + '</b>';
                        htmlExampleString += '<br/>' + response.data.LessonExampleModelList[i].Desc;
                    }
                    setExamples(htmlExampleString);
                }
            });
        }
        else if(e.currentTarget.value === "practices")
        {
            // Call practices
            let apiLesson = getLessonPractices(lessonId);
            apiLesson.then((response)=>{
                let practices = [];
                for(let i = 0; i < response.data.length; i++) {
                    practices.push(
                        <Link to={{
                            pathname: "/quiz",
                            quizId: response.data[i].Id,
                            lessonId: response.data[i].LessonId,
                            isTeacher: true,
                            planningId: planningId,
                            planningName: planningName,
                            onlyView: true,
                            subject: subject,
                            level: level,
                        }}>
                            <Row>
                                <Col xs={{size: 6}}><u>{response.data[i].Title}</u></Col>
                            </Row>
                        </Link>);
                }

                setPracticeList(practices);
            })
        }
        else if(e.currentTarget.value === "exams") {
            // Call exams
            let apiLesson = getLessonExamsHomeschooling(lessonId);
            apiLesson.then((response) => {
                let exams = [];

                for (let i = 0; i < response.data.length; i++) {
                    exams.push(
                        <Link to={{
                            pathname: "/quiz",
                            quizId: response.data[i].Id,
                            isTeacher: true,
                            lessonId: lessonId,
                            planningId: planningId,
                            planningName: planningName,
                            onlyView: true,
                            subject: subject,
                            level: level,
                        }}>
                            <Row>
                                <Col xs={{size: 6}}> <u>{response.data[i].Title}</u></Col>
                            </Row>
                        </Link>);
                }

                setExamList(exams);
            });
        }

        setActiveTab(e.currentTarget.value);
    };
    // Call lesson details
    const callLessonDescription = (lessonId) => {
        setLoading(true);

        let apiLessonGenerics = getLessonDetailsStudent(lessonId);
        apiLessonGenerics.then((response =>{
            setLoading(false);
            setLessonTitle(response.data.LessonTitle);
            setLevel(response.data.LevelCode);
            setSubject(response.data.SubjectCode);
        }));

        let apiLesson = getLessonConcept(lessonId);
        apiLesson.then((response =>{
            setLoading(false);
            setConcept(response.data.Concept);
                // To support language library add the following lines
            initialize({
                languages: Settings.LANGUAGES,
                options: {renderToStaticMarkup, defaultLanguage: response.data.lessonLangCode}
            });
        }));
    };

    if (typeof lessonId === 'undefined' || lessonId == null)
    {
        return (<Redirect  to={{
            pathname: "/plannings",
            planningName: planningName,
            planningId: planningId,
        }}/>);
    }

    let layoutClasses = "animated delay-05s fadeIn";
        // Renders the main dashboard
        return (
            <div>
                <div className="container-padding animated delay-05s fadeIn">
                    <Container fluid>
                        <Row className="sidebar-background" style={{minHeight: '800px'}}>
                            <Col xs={2} md={2} lg={1} className="sidebar-background sidebar-padding-top">
                                <Row className="text-center white-text">
                                    <Col xs={12}>
                                        <Button 
                                            className={active === 'concept' ? 'btn-transparent-selected button-concept-background': 'btn-transparent button-concept-background'}
                                            style={{backgroundImage: `url(${IconConcept})`}}
                                            value="concept" 
                                            onClick={setActive}
                                        >
                                        </Button>
                                        <small><Translate id="student-courses.concept" options={{renderInnerHtml: true}}/></small>
                                    </Col>
                                    <Col xs={12} className="margin-top-mandatory-25">
                                        <Button 
                                            className={active === 'definitions' ? 'btn-transparent-selected button-definitions-background': 'btn-transparent button-definitions-background'} 
                                            style={{backgroundImage: `url(${IconVocabulary})`}}
                                            value="definitions" 
                                            onClick={setActive}
                                        >
                                            <br/>

                                        </Button>
                                        <small>
                                            <Translate id="student-courses.definitions" options={{renderInnerHtml: true}}/>
                                        </small>
                                    </Col>
                                    <Col xs={12} className="margin-top-mandatory-25">
                                        <Button
                                            className={active === 'examples' ? 'btn-transparent-selected button-examples-background' : 'btn-transparent button-examples-background'} 
                                            style={{backgroundImage: `url(${IconExample})`}}
                                            value="examples" 
                                            onClick={setActive}
                                        >
                                        </Button>
                                        <small>
                                            <Translate id="student-courses.examples" options={{renderInnerHtml: true}}/>
                                        </small>
                                    </Col>
                                    <Col xs={12} className="margin-top-mandatory-25">
                                        <Button 
                                            className={active === 'practices' ? 'btn-transparent-selected button-practice-background': 'btn-transparent button-practice-background'}
                                            style={{backgroundImage: `url(${IconPractice})`}}
                                            value="practices" 
                                            onClick={setActive}
                                        >
                                        </Button>
                                        <small>
                                            <Translate id="student-courses.practices" options={{renderInnerHtml: true}}/>
                                        </small>
                                    </Col>
                                    <Col xs={12} className="margin-top-mandatory-25">
                                        <Button 
                                            className={active === 'exams' ? 'btn-transparent-selected button-exam-background': 'btn-transparent button-exam-background'}
                                            style={{backgroundImage: `url(${IconChallenge})`}}
                                            value="exams" 
                                            onClick={setActive}
                                        >
                                        </Button>
                                        <small>
                                            <Translate id="student-courses.challenges" options={{renderInnerHtml: true}}/>
                                        </small>
                                    </Col>
                                    <Col xs={12} className="margin-top-mandatory-25">
                                        <Link to={{
                                            pathname: 'planning-select'
                                            }}  className="link-white">
                                            <Button className='btn-transparent button-finish-background'>
                                            </Button>
                                            <small>
                                                <Translate id="student-courses.finish" options={{renderInnerHtml: true}}/>
                                            </small>
                                        </Link>
                                    </Col>
                                </Row>
                            </Col>
                            <Col xs={10} md={10} lg={11} className="padding-top-mandatory-110 background-whitish wrapper-iframe-kids">
                                <Row>
                                    <Col xs={{size:12}}>
                                        <h4 className="font-roboto title-lesson">
                                            <b>{subjectTitle}
                                                {subjectTitle !== "" ? " - ": ""}
                                                {lessonTitle != null ? lessonTitle : null}
                                            </b>
                                        </h4>
                                    </Col>
                                </Row>
                                <Row>
                                    {concept !== null ?
                                        <Col xs={{size:12}}>
                                            <Card className="border-none">
                                                {
                                                    active === 'concept' ?
                                                        <CardBody className={layoutClasses}>
                                                            <div dangerouslySetInnerHTML={{__html: concept}}/>
                                                        </CardBody> : null
                                                }

                                                {
                                                    active === 'definitions' ?
                                                        <CardBody className={layoutClasses}>
                                                            {definitionUrl !== null ?
                                                                <Iframe url={definitionUrl}
                                                                        width="100%"
                                                                        height="500px"
                                                                        id="myId"
                                                                        className="myClassname"
                                                                        display="initial"
                                                                        position="relative"/>
                                                                :
                                                                <div
                                                                    dangerouslySetInnerHTML={{__html: definitions}}/>
                                                            }
                                                        </CardBody> : null
                                                }

                                                {
                                                    active === 'examples' ?
                                                        <CardBody className={layoutClasses}>
                                                            {exampleUrl !== null ?
                                                                <Iframe url={exampleUrl}
                                                                        width="100%"
                                                                        height="500px"
                                                                        id="myId"
                                                                        className="myClassname"
                                                                        display="initial"
                                                                        position="relative"/>
                                                                :
                                                                <div
                                                                    dangerouslySetInnerHTML={{__html: examples}}/>
                                                            }
                                                        </CardBody> : null
                                                }

                                                {
                                                    active === 'practices' ?
                                                        <CardBody>
                                                            {practiceList}
                                                        </CardBody> : null
                                                }

                                                {
                                                    active === 'exams' ?
                                                        <CardBody>
                                                            {examList}
                                                        </CardBody> : null
                                                }
                                            </Card>
                                        </Col> : null
                                    }
                                </Row>
                            </Col>
                        </Row>
                    </Container>
                </div>
                {loading ? <LoaderFullWindow/> : null}

            </div>
        )
};

export default withLocalize(LessonPresentation);