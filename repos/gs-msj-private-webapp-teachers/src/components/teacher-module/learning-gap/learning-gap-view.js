import React, {useState, useEffect, useCallback} from 'react';
import { withLocalize, Translate } from 'react-localize-redux';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faAddressBook,
    faArrowLeft, faBookmark,
    faBookOpen, faList, faPlus,
    faSearch,
    faStickyNote,
    faUserGraduate,
    faBell,
    faExclamationCircle
} from '@fortawesome/free-solid-svg-icons';
import ProfilesDataTable from "../../shared-module-components/datatables/profiles-data-table";
import {
    Row,
    Col,
    Nav,
    NavItem,
    NavLink,
    TabContent,
    TabPane,
    Button,
    Modal,
    ModalHeader, ModalBody, ModalFooter
} from 'reactstrap';
import Settings from "../../../custom-classes/settings";
import LanguageManager from '../../../custom-classes/language-manager';
import SessionManager from '../../../custom-classes/session-manager';
import {renderToStaticMarkup} from "react-dom/server";
import ImgAvanza from "../../../assets/images/common-icons/Avanza-icon-Logo.png";
import IconStudents from "../../../assets/images/common-icons/Avanza-icon-Avatar.png";
import LearningGapProgress from "./learning-gap-progress/learning-gap-progress";
import LearningGapLessons from "./learning-gap-lessons/learning-gap-lessons";
import {useLocation, useHistory } from "react-router-dom";
import LearningGapAnnotations from "./learning-gap-annotations/learning-gap-annotations";
import LearningGapRecommendations from "./learning-gap-recommendations/learning-gap-recommendations";
import LearningGapNotifications from './learning-gap-notifications/learning-gap-notifications';
import {getLearningGapReadStudentYearScore, getLearningGapStudentHasNotifications} from "../../../api/teachers";
import {learningGapMinScoreValidator} from "./utils/learning-gap-min-score-validator";
import {PulseLoader} from "react-spinners";
import VideoPlayer from "../../../utils/video-player";
import videoEnglish from '../../../assets/videos/avanza-en.mp4'
import videoSpanish from '../../../assets/videos/avanza-es.mp4'
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

import { SendAccessLog } from '../../../utils/access_logger';

const LearningGapView = () => {
    const sessionDetails = SessionManager.getSessionDetails();
    const history = useHistory();
    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const [activeTab, setActiveTab] = useState(query.get("tab") || "1");
    const [studentId, setStudentId] =  useState(query.get("studentId") || null); // useState(null);
    const [studentName, setStudentName] = useState(query.get("studentName") || null);
    const [isTeacher, setIsTeacher] = useState(false);
    const [isStaff, setIsStaff] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [isLearnMoreModalOpen, setIsLearnMoreModalOpen] = useState(false);
    const [showInitialGradeNeededMessage, setShowInitialGradeNeededMessage] = useState(false);
    const [smallLoading, setSmallLoading] = useState(false);
    const [videoSrc, setVideoSrc] = useState(videoEnglish);
    const [studentNotificationsInfo, setStudentsNotificationsInfo] = useState([]);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        // Clear query parameters
        history.replace(location.pathname);

        const roles = sessionDetails.Roles;
        setIsTeacher(roles.includes(Settings.ROLE_TEACHER_STR));

        if(LanguageManager.getLanguage() === 'es'){
            setVideoSrc(videoSpanish);
        }
        try{            
            SendAccessLog({
                Username: sessionDetails.Username,
                InvitationCode: sessionDetails.InvitationCode,
                Role: sessionDetails.Roles[0],
                AreaId: "11"
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    useEffect(() => {
        const checkIfStudentHasNotifications = async () => {
            try {
                const response = await getLearningGapStudentHasNotifications();
                setStudentsNotificationsInfo(response.data);
            } catch (error) {
                console.error("Error fetching notifications:", error);
            }
        };
    
        checkIfStudentHasNotifications();
    }, []);

    const handleStudentSelection = (newValue) => {
        setStudentId(newValue.UserId);
        setStudentName(`${newValue.GivenName} ${newValue.Surname}`);
        setActiveTab("1");
        window.scrollTo(0, 0);
    };

    /**
     * To check if initial grade was inserted. This makes a call to the API to get the student year scores.
     * @type {(function(): void)|*}
     */
    const checkIfInitialGradeWasInserted = useCallback(() => {
        setSmallLoading(true);
        getLearningGapReadStudentYearScore(studentId)
            .then((response) => {
                setSmallLoading(false);
                // Handle empty or invalid responses
                if (!response?.data?.length) {
                    setShowInitialGradeNeededMessage(true);
                    return;
                }

                // Validate if minimum scores are set
                const hasMinimum = learningGapMinScoreValidator(response.data);
                setShowInitialGradeNeededMessage(!hasMinimum);
            })
            .catch((error) => {
                console.error("Error fetching student year scores:", error);
                // Optionally handle error by setting a state or displaying a message
            });
    }, [studentId]);

    useEffect(()=>{
        checkIfInitialGradeWasInserted();
    }, [checkIfInitialGradeWasInserted])

    const showStudentBox = () => setStudentId(null);

    const toggleTab = (tabNo) => { setActiveTab(tabNo);
        if(tabNo === "1" || tabNo === "2"){
            setRedoSearch(true);
        }
        else {
            setRedoSearch(false);
            }
    }

    // Get the state from the student notifications when user is updating the notifications
    const refreshIfStudentHasNotifications = (state) => {
        setStudentsNotificationsInfo(state);
    }

    return (
        <div>
            <div className="container-padding animated learning-gap-fade-in">
                <div className="wrapper-principal-container">
                    {!studentId ? (<Row>
                        <Col xs="12">
                            <div className="learning-gap-welcome-area">
                                <Row>
                                    <Col xs={12} sm={12} md={3} lg={2} xl={2} className="align-vertical-center">
                                        <div style={{
                                            display: "flex",
                                            flexDirection: "column",
                                            alignItems: "center",
                                            width: "100%"
                                        }}>
                                            <img src={ImgAvanza} alt="Avanza" className="img-fluid"
                                                 style={{maxWidth: 160, width: "100%"}}/>
                                        </div>
                                    </Col>

                                    <Col xs={12} sm={12} md={6} lg={8} xl={8} className="align-vertical-center">
                                        <div style={{
                                            display: "flex",
                                            flexDirection: "column",
                                            alignItems: "center",
                                            width: "100%"
                                        }}>
                                            {/* Left-aligned text */}
                                            <div style={{width: "100%", textAlign: "left"}}>
                                            <span style={{fontWeight: "600", display: "block"}}>
                                                <Translate id="learning-gap.welcome" options={{renderInnerHtml: true}}/>
                                            </span>
                                            </div>

                                            {/* Centered button */}
                                            <div style={{width: "100%", textAlign: "center", marginTop: 10}}>
                                                <button
                                                    className="button-primary-custom text-uppercase mr-2"
                                                    onClick={() => setIsLearnMoreModalOpen(true)}
                                                >
                                                    <Translate id="learning-gap.learn-more"
                                                               options={{renderInnerHtml: true}}/>
                                                </button>
                                            </div>
                                        </div>
                                </Col>
                                    <Col xs={12} sm={12} md={3} lg={2} xl={2}>
                                        <div style={{
                                            display: "flex",
                                            flexDirection: "column",
                                            alignItems: "center",
                                            width: "100%"
                                        }}>
                                            <VideoPlayer src={videoSrc}/>
                                        </div>
                                    </Col>

                                </Row>
                            </div>
                        </Col>
                    </Row>) : null}
                    <div className="wrapper-col mb-3">
                        <div className="col col-no-padding-left">
                            {studentId ? (
                                <>
                                    <a className="btn-back" onClick={showStudentBox}>
                                        <FontAwesomeIcon icon={faArrowLeft}/> <Translate id="general.back-action"/>
                                    </a>
                                    <div className="title-principal mt-3">
                                        <span className="title-principal--inside">
                                          <Translate id="learning-gap.selected-student" />:
                                        </span> <span className="avanza-text">{studentName}</span>
                                    </div>
                                </>
                            ) : (
                                <div className="title-principal mt-3">
                                  <span className="title-principal--inside">
                                    <Translate id="learning-gap.students" />{' '}
                                      <img src={IconStudents} alt="students" className="img-fluid" style={{maxWidth: 50}} />
                                  </span>
                                </div>
                            )}
                        </div>
                    </div>
                    <Row style={{ minHeight: 600 }}>
                        <Col sm="12">
                            {!studentId && (
                                <><Translate id="learning-gap.students-subtitle"  options={{renderInnerHtml: true}}/>
                                    {<ProfilesDataTable
                                    value={studentId}
                                    isStaff={isStaff}
                                    roleId={Settings.ROLE_STUDENT_ID}
                                    updateParent={handleStudentSelection}
                                    isForLearningGapProfiles={true}
                                    studentNotificationsInfo={studentNotificationsInfo}
                                /> }
                                </>
                            )}

                            {studentId && (
                                <>
                                <div className="mt--5 tabs-strech danger-btn-simple table-no-margin-top clear-min-height invitation-btn clear-hr td-responsive special-top-space">
                                        <Nav tabs>
                                            <NavItem>
                                                <nav className={activeTab === "1" ? "tab underline-div learning-gap-tab-selected nav-link" : "tab nav-link"}
                                                         onClick={() => toggleTab("1")}>
                                                    <Translate id="learning-gap.progress"/>{" "}
                                                    <span className="learning-gap-tabs-icon-space">
                                                    <FontAwesomeIcon icon={faUserGraduate}/>
                                                    </span>
                                                </nav>
                                            </NavItem>
                                            <NavItem>
                                                <nav className={activeTab === "2" ? "tab underline-div learning-gap-tab-selected nav-link" : "tab nav-link"}
                                                         onClick={() => toggleTab("2")}>
                                                    <Translate id="learning-gap.lessons"/> {""}
                                                    <span className="learning-gap-tabs-icon-space">
                                                    <FontAwesomeIcon icon={faBookOpen}/>
                                                    </span>
                                                </nav>
                                            </NavItem>
                                            {isTeacher && (
                                                <NavItem>
                                                    <nav className={activeTab === "3" ? "tab underline-div learning-gap-tab-selected nav-link" : "tab nav-link"}
                                                             onClick={() => toggleTab("3")}>
                                                        <Translate id="progress.annotations"/> {" "}
                                                        <span className="learning-gap-tabs-icon-space">
                                                        <FontAwesomeIcon icon={faStickyNote}/>
                                                        </span>
                                                    </nav>
                                                </NavItem>
                                            )}
                                            <NavItem>
                                                <nav className={activeTab === "4" ? "tab underline-div learning-gap-tab-selected nav-link" : "tab nav-link"}
                                                         onClick={() => toggleTab("4")}>
                                                    <Translate id="learning-gap.recommendations" />
                                                    <span className="learning-gap-tabs-icon-space">
                                                    <FontAwesomeIcon icon={faList}/>
                                                        </span>

                                                </nav>
                                            </NavItem>
                                            {isTeacher && (
                                                <NavItem>
                                                    <nav 
                                                        className={activeTab === "5" ? "tab underline-div learning-gap-tab-selected nav-link" : "tab nav-link"}
                                                        onClick={() => toggleTab("5")}
                                                    >
                                                        {studentId && typeof studentNotificationsInfo === 'object'  && studentNotificationsInfo.map((item, index) => {
                                                            if (item.StudentId === studentId && item.HasNotifications) {
                                                                return (
                                                                    <FontAwesomeIcon
                                                                        icon={faExclamationCircle}
                                                                        style={{color: '#d41f13'}}
                                                                        key={index}
                                                                    />
                                                                );            
                                                            } 
                                                        })}
                                                        <span>
                                                            <Translate id="learning-gap.notifications"/>
                                                        </span>
                                                        <span className="learning-gap-tabs-icon-space">
                                                            <FontAwesomeIcon icon={faBell}/>
                                                        </span>
                                                    </nav>
                                                </NavItem>
                                            )}
                                        </Nav>
                                </div>
                                    <TabContent activeTab={activeTab}>
                                            <TabPane
                                                tabId="1"
                                                onClick={() => {
                                                    setRedoSearch(true);
                                                }}
                                            >
                                                {/* Substituted by the big modal
                                                 smallLoading ? (
                                                    <div style={{ textAlign: "center", marginTop: 20, width: "100%" }}>
                                                    <PulseLoader size={10} color="#395a8f" />
                                                    </div>
                                                ) : showInitialGradeNeededMessage ? (
                                                    <Row className="learning-gray-areas align-vertical-center learning-gap-fade-in">
                                                        <Col xs={7} sm={8} md={7} lg={9} className="learning-gray-areas align-vertical-center">
                                                            <b>
                                                                <Translate
                                                                    id="learning-gap.add-initial-grade"
                                                                    options={{ renderInnerHtml: true }}
                                                                />
                                                            </b>
                                                        </Col>
                                                        <Col
                                                            xs={5}
                                                            sm={4}
                                                            md={4}
                                                            lg={3}
                                                            className="learning-gray-areas align-vertical-center"
                                                            style={{ textAlign: "right" }}
                                                        >
                                                            <Button
                                                                className="btn primary-btn mr-2 btn-secondary btn-success "
                                                                onClick={() => setActiveTab("3")}
                                                            >
                                                                <FontAwesomeIcon icon={faPlus} />
                                                                <Translate
                                                                    id="learning-gap.add-grades"
                                                                    options={{ renderInnerHtml: true }}
                                                                />
                                                            </Button>
                                                        </Col>
                                                    </Row>
                                                ) : null */}
                                            <LearningGapProgress studentId={studentId} studentName={studentName}
                                                                 redoSearch={redoSearch} />
                                        </TabPane>
                                        <TabPane tabId="2" onClick={() => {
                                            setRedoSearch(true);
                                        }}>
                                            <LearningGapLessons studentId={studentId} studentName={studentName}
                                                                redoSearch={redoSearch} />
                                        </TabPane>
                                        <TabPane tabId="3">
                                            {activeTab === "3" && (
                                            <LearningGapAnnotations studentId={studentId} />
                                            )}
                                        </TabPane>
                                        <TabPane tabId="4">
                                            <LearningGapRecommendations studentId={studentId} />
                                        </TabPane>
                                        <TabPane tabId="5">
                                            {activeTab === "5" && ( 
                                            <LearningGapNotifications 
                                                studentId={studentId} 
                                                studentName={studentName}
                                                refreshIfStudentHasNotifications={refreshIfStudentHasNotifications}
                                            />
                                            )}
                                        </TabPane>
                                    </TabContent>
                                </>
                            )}
                        </Col>
                    </Row>
                </div>
            </div>
            <Modal isOpen={isLearnMoreModalOpen} toggle={()=> setIsLearnMoreModalOpen(false)}
                   className="modal-learning-gap">
                <ModalHeader toggle={()=> setIsLearnMoreModalOpen(false)} />
                <ModalBody>
                 <div style={{width: "100%"}}>
                    <Translate id="learning-gap.learn-more-explanation-paragraph-1" options={{renderInnerHtml: true}}/>
                     <br/>
                     <br/>
                     <Translate id="learning-gap.learn-more-explanation-paragraph-2" options={{renderInnerHtml: true}}/>
                     <br/>
                        <Row style={{marginTop: 10}}>
                            <Col xs={1}>
                                <div style={{width: "100%", textAlign: "right"}}>
                                    <FontAwesomeIcon icon={faSearch}/>
                                </div>
                            </Col>
                            <Col xs={11}>
                                <Translate id="learning-gap.learn-more-explanation-paragraph-3" options={{renderInnerHtml: true}}/>
                            </Col>
                        </Row>
                     <Row style={{marginTop: 10}}>
                         <Col xs={1}>
                             <div style={{width: "100%", textAlign: "right"}}>
                                 <FontAwesomeIcon icon={faAddressBook}/>
                             </div>
                         </Col>
                         <Col xs={11}>
                             <Translate id="learning-gap.learn-more-explanation-paragraph-4" options={{renderInnerHtml: true}}/>
                         </Col>
                     </Row>
                     <Row style={{marginTop: 10}}>
                         <Col xs={1}>
                             <div style={{width: "100%", textAlign: "right"}}>
                                 <FontAwesomeIcon icon={faBookmark}/>
                             </div>
                         </Col>
                         <Col xs={11}>
                             <Translate id="learning-gap.learn-more-explanation-paragraph-5" options={{renderInnerHtml: true}}/>
                         </Col>
                     </Row>
                     <br/>
                        <Translate id="learning-gap.learn-more-explanation-paragraph-6" options={{renderInnerHtml: true}}/>
                    </div>
                </ModalBody>
                <ModalFooter>
                    <div style={{width: "100%", textAlign: "center"}}>
                        <button className="btn-default button-primary-custom" onClick={()=> setIsLearnMoreModalOpen(false)}>
                            <Translate id="learning-gap.start" options={{renderInnerHtml: true}} />
                        </button>
                    </div>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default withLocalize(LearningGapView);