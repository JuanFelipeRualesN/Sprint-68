import React, { useState } from 'react';
import { Col, Card, CardBody, CardHeader, Container, Row, Button } from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import SelectorLesson from "../../shared-module-components/selector/selector-lesson-by-subject-level";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPrint, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { Link } from "react-router-dom";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import "./style.css";
import SelectorSubject from "../../teacher-module/selector/selector-subject";
import { getLessonPDF } from "../../../api/teachers";
import { getLessonDetails } from "../../../api/athenas";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import { SendAccessLog } from '../../../utils/access_logger';
/**
 * The component (window) to update the syllabus
 */
const Syllabus = () => {
    const sessionDetails = SessionManager.getSessionDetails();
    const [loading, setLoading] = useState(false);
    const [subjectCode, setSubjectCode] = useState(null);
    const [levelCode, setLevelCode] = useState(null);
    const [sessionValid, setSessionValid] = useState(true);
    const [syllabusInfo, setSyllabusInfo] = useState(null);
    const [lessonId, setLessonId] = useState(null);
    // *** NEW STATE: Used to force SelectorLesson to reset
    const [lessonSelectorKey, setLessonSelectorKey] = useState(0);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Handle subject id selected
    const handleSubjectCodeSelection = (value) => {
        // 1. Reset states dependent on the subject/level change
        setSubjectCode(null);
        setLevelCode(null);
        setLessonId(null); // *** Clears the currently selected lesson ID
        setSyllabusInfo(null); // *** Clears the displayed syllabus content

        // *** Increments the key to force the SelectorLesson component to remount (reset)
        setLessonSelectorKey(prevKey => prevKey + 1);

        // 2. Set new subject/level
        setSubjectCode(value.subject);
        setLevelCode(value.level);
    };

    // Call the syllabus data
    const callSyllabus = (newValue) => {
        setSyllabusInfo('');
        setLoading(true);
        setLessonId(newValue.value);

        let apiSyllabus = getLessonDetails(newValue.value);
        let dataSyllabus;
        let lessonInfo;
        apiSyllabus.then((response) => {
            dataSyllabus = response.data.LessonModifierRequestModel.LessonDetailModel.Description;
            lessonInfo = response.data.LessonModifierRequestModel;
        }).then(() => {
            setSyllabusInfo(dataSyllabus);
            setLoading(false);
        }).finally(() => {
            try{
                SendAccessLog({
                    UserName: sessionDetails.Username,
                    InvitationCode: sessionDetails.InvitationCode,
                    Role: sessionDetails.Roles[0],
                    AreaId: "21",
                    Details: "Syllabus - Lesson details",
                    LessonId: lessonInfo.LessonModel.Id ?? null,
                    LessonTitle: lessonInfo.LessonModel.LessonTitle ?? null,
                    Subject: lessonInfo.LessonModel.SubjectCode ?? null,
                    Grade: lessonInfo.LessonModel.LevelCode ?? null,
                })
            } catch (e) {
                // empty catch block to avoid breaking the application
            }
        });
    };

    // Opens a new tab with the PDF location.
    const printLesson = () => {
        let apiCall = getLessonPDF(lessonId);

        setLoading(true);
        // After the api call, fill the data
        apiCall.then((response) => {
            setLoading(false);
            window.open(response.data);
        });
    };

    const roles = sessionDetails.Roles;

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <Container className="container-wrapper">
                    {isRoleAllowed([Settings.ROLE_DIRECTOR_STR], roles) && !Settings.IS_APP_MANAGED_BY_REQUIRED ? <div className="col col-no-padding-left">
                        <Link to="/academic-content-select" className="btn-back">
                            <FontAwesomeIcon icon={faArrowLeft} />{' '}
                            <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                        </Link>
                    </div> : null}
                    <Row>
                        <Col xs={{ size: 12 }}>
                            <div className="wrapper-col borderGray-bottom block-display-responsive">
                                <div className="col col-no-padding-left">
                                    <div className="title-principal">
                                        <Translate id="dashboard.syllabus" options={{ renderInnerHtml: true }} />
                                    </div>
                                </div>
                            </div>
                        </Col>
                    </Row>
                    <Col sm={{ size: 12, offset: 0 }} lg={{ size: 12, offset: 0 }}>
                        <Card className="wrapper-general">
                            <CardHeader>
                                <h4>
                                    <Translate id="syllabus.lesson-syllabus" options={{ renderInnerHtml: true }} />
                                </h4>
                                <small>
                                    <Translate id="syllabus.instructions" options={{ renderInnerHtml: true }} />
                                </small>
                            </CardHeader>
                            <CardBody>
                                <Row>
                                    <Col sm={{ size: 12 }} md={{ size: 4 }} lg={{ size: 3 }}>
                                        <p><b><Translate id="syllabus.select-class"
                                                         options={{ renderInnerHtml: true }} /></b></p>
                                        <SelectorSubject updateParent={handleSubjectCodeSelection} />
                                    </Col>
                                    <Col sm={{ size: 12 }} md={{ size: 8 }} lg={{ size: 9 }}>
                                        <p><b><Translate id="syllabus.select-lesson"
                                                         options={{ renderInnerHtml: true }} /></b></p>
                                        {subjectCode !== null && levelCode !== null ?
                                            <SelectorLesson
                                                key={lessonSelectorKey} // *** Key prop ensures component resets on subject/level change
                                                subjectCode={subjectCode}
                                                levelCode={levelCode}
                                                updateParent={callSyllabus} /> : null}
                                    </Col>
                                </Row>
                                <Row>
                                    <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                                        <hr />
                                        <div className="margin-top-mandatory-60 container-padding">
                                            {syllabusInfo != null ?
                                                <>
                                                    <Row>
                                                        <Col style={{ textAlign: "right" }}>
                                                            <button className='btn-default button-secondary-custom' onClick={printLesson}>
                                                                <FontAwesomeIcon icon={faPrint} />{' '}
                                                                <Translate id="syllabus.print-full-lesson-details" options={{ renderInnerHtml: true }} />
                                                            </button></Col>
                                                    </Row>
                                                    <Row>
                                                        <div
                                                            dangerouslySetInnerHTML={{ __html: (syllabusInfo) }} />
                                                    </Row>
                                                </> : null}
                                        </div>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Card>
                    </Col>
                </Container>
                {loading ?
                    <LoaderFullWindow /> : null
                }
            </div>
        </div>
    );
};

export default withLocalize(Syllabus);