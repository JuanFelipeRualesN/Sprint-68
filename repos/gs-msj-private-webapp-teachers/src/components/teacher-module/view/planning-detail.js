/**
 * Created by portiz on 05/07/2020.
 */
import React, { useState, useEffect } from 'react';
import {CardBody, Col, Container, Row, Button, Alert} from 'reactstrap';
import { Link, Redirect } from "react-router-dom";

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import LanguageManager from '../../../custom-classes/language-manager';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faCalendarAlt,
    faArrowAltCircleLeft,
    faPlus,
    faBookOpen,
    faTimes,
    faEdit,
    faSave,
    faTasks, faComments, faUserGraduate, faExclamationCircle, faArrowLeft, faClock, faFlag
} from '@fortawesome/free-solid-svg-icons';
import PlanningDetailAttachedItemsDataTable from "../datatable/planning-detail-attached-items-data-table";
import SessionManager from "../../../custom-classes/session-manager";
import { getTeacherPlanningDetails, modifyTeacherQuiz, modifyTeacherLesson, updatePlanName } from "../../../api/teachers";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import ModalPlanningAttachItem from "../modal/modal-planning-attach-item";
import ModalPlanningClose from "../modal/modal-planning-close";
import "./style.css";
import ForumsDataTable from "../datatable/forums-data-table";
import StudentsPlanningDataTable from "../datatable/students-planning-data-table";
import ModalCalendar from "../modal/modal-calendar";
import { convertMaxLengthSting } from '../../../utils/utils';
import { MAX_LENGTH_INPUT_NAME_INSTRUCTIONAL_PLAN } from "../../../constants/types";
import moment from "moment";
import "moment-timezone";
import 'moment/locale/es'
import { getTimeZone } from "../../../utils/utils";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the planning detail
 */
const PlanningDetail = () => {
    const planningDetails = SessionManager.getSelectedPlanning();
    const userDetails = SessionManager.getSessionDetails();

    const [dataQuizzes, setDataQuizzes] = useState([]);
    const [dataLessons, setDataLessons] = useState([]);
    const [loading, setLoading] = useState(false);
    const [roles, setRoles] = useState(SessionManager.getSessionDetails()?.Roles ?? null);
    const [planningId, setPlanningId] = useState(planningDetails?.planningId ?? null);
    const [planningName, setPlanningName] = useState(planningDetails?.planningName ?? null);
    const [newPlanningName, setNewPlanningName] = useState('');
    const [sessionValid, setSessionValid] = useState(true);
    const [showLessons, setShowLessons] = useState(true);
    const [showModalPlanningAttachItem, setShowModalPlanningAttachItem] = useState(false);
    const [showPlanningClose, setShowPlanningClose] = useState(false);
    const [isPlanClosed, setIsPlanClosed] = useState(false);
    const [genericModal, setGenericModal] = useState({
        bodyText: "",
        visible: false
    });
    const [showCalendar, setShowCalendar] = useState(false);
    const [showQuizzes, setShowQuizzes] = useState(false);
    const [showForums, setShowForums] = useState(false);
    const [showStudents, setShowStudents] = useState(false);
    const [defaultTypeAssignmentOrLessons, setDefaultTypeAssignmentOrLessons] = useState("0"); // "0" for Assignments, "1" for Lessons
    const [academicPeriodFullDetail, setAcademicPeriodFullDetail] = useState({});
    const [editingPlanningName, setEditingPlanningName] = useState(false);
    const [totalPages, setTotalPages] = useState(0);
    const [data, setData] = useState([]);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        moment.locale(LanguageManager.getLanguage());
        try{            
            SendAccessLog(
            {
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "3",
                PlanId: planningDetails?.planningId ?? null,
                PlanName: planningDetails?.planningName ?? null,
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        document.body.classList.remove('landing-white');
        doSearch();
    }, []);

    // Shows the lesson or homework attach modal.
    const handleAddNewItemToPlan = (e) => {
        setShowModalPlanningAttachItem(true);
        setDefaultTypeAssignmentOrLessons(e.currentTarget.value);
    };

    // Handles the logic when closing the planning attach window.
    const handleCloseModalPlanningAttachItem = (refreshData) => {
        setShowModalPlanningAttachItem(false);

        if (refreshData) {
            doSearch();
        }
    };

    const handleUpdatePlanningDetail = () => {
        doSearch();
    };

    const handleOnClosePlan = () => {
        /*getClosePlanning(planningId).then((response) => {

        }).catch((error) => {

        })*/
    };


    // Handle lesson esson update
    const handleUpdateLesson = (lessonId, forGrading) => {
        // setLoading(true);
        // modifyTeacherLesson(planningId,lessonId,forGrading).then((response) => {
        //     doSearch();
        // }).catch((error) => {

        // }).then(() => {
        //     setLoading(false);
        // });
    };

    // Handle Quiz update
    const handleUpdateQuizz = (quizId, forGrading) => {
        setLoading(true);
        modifyTeacherQuiz(planningId, quizId, forGrading).then((response) => {
            doSearch();
        }).catch((error) => {

        }).then(() => {
            setLoading(false);
        });
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);
        setDataQuizzes([]);
        setDataLessons([]);

        let apiCall = getTeacherPlanningDetails(planningId);
        apiCall.then((response) => {
            //update selected planning with current status
            SessionManager.setSelectedPlanning(planningId, planningName, (response.data.TeacherPlanModel.Opened === "0"));
            setLoading(false);
            setTotalPages(1);
            setIsPlanClosed(response.data.TeacherPlanModel.Opened === "0");
            setData([]);
            setAcademicPeriodFullDetail(response.data.AcademicPeriodFullDetail);

            if (typeof response.data.QuizzesAttachedModelList !== 'undefined' && response.data.QuizzesAttachedModelList != null &&
                response.data.QuizzesAttachedModelList.length > 0) {
                setDataQuizzes(response.data.QuizzesAttachedModelList);
            }

            if (typeof response.data.LessonAttachedModelList !== 'undefined' && response.data.LessonAttachedModelList != null &&
                response.data.LessonAttachedModelList.length > 0) {
                const cleanedLessonList = response.data.LessonAttachedModelList.map(
                    ({ TeacherPlanLessonDateModels, TeacherPlanLessonQuizModels, ...rest }) => rest
                );

                setDataLessons(cleanedLessonList);
            }
        });
    };

    const handleShowLessons = () => {
        setShowLessons(true);
        setShowForums(false);
        setShowQuizzes(false);
        setShowStudents(false);
    };

    const handleShowForums = () => {
        setShowLessons(false);
        setShowForums(true);
        setShowStudents(false);
        setShowQuizzes(false);
    };

    const handleShowStudents = () => {
        setShowLessons(false);
        setShowForums(false);
        setShowStudents(true);
        setShowQuizzes(false);
    };

    const handleShowQuizzes = () => {
        setShowLessons(false);
        setShowForums(false);
        setShowStudents(false);
        setShowQuizzes(true);
    };

    const hideCalendarModal = () => {
        setShowCalendar(false);
    };

    const hidePlanningCloseModal = (data) => {
        setShowPlanningClose(false);
        setIsPlanClosed(data.isPlanClosed);
        setGenericModal({
            visible: data.messages !== null && data.messages !== "",
            bodyText: data.messages
        });
    };

    const toggleAlertModal = () => {
        setGenericModal({
            visible: false,
            bodyText: ""
        });
    };

    const handleShowPlanningClose = () => {
        setShowPlanningClose(true);
    };

    const handleShowCalendarModal = () => {
        setShowCalendar(true);
    };

    const handleEditPlanName = () => {
        if (!editingPlanningName) {
            // Enter edit mode
            setEditingPlanningName(true);
            setNewPlanningName(planningName);
        } else {
            if (newPlanningName.trim() !== '') {
                // Save changes and exit edit mode
                updatePlanName(planningId, newPlanningName);
                SessionManager.setSelectedPlanning(planningId, newPlanningName, isPlanClosed);
                setEditingPlanningName(false);
                setPlanningName(newPlanningName);
            }
        }
    };

    const handlePlanNameChange = (e) => {
        setNewPlanningName(convertMaxLengthSting(e.target.value, MAX_LENGTH_INPUT_NAME_INSTRUCTIONAL_PLAN));
    };

    if (!academicPeriodFullDetail || !academicPeriodFullDetail.AcademicDetail || !academicPeriodFullDetail.AcademicPeriods)
    {
        // Render loading state or return null
        return null; // Or any other loading indicator
    }

    let academicDetails = null;
    let academicPeriod = null;
    if (academicPeriodFullDetail && academicPeriodFullDetail.AcademicDetail && academicPeriodFullDetail.AcademicPeriods)
    {
        academicDetails = academicPeriodFullDetail.AcademicDetail;
        academicPeriod = academicPeriodFullDetail.AcademicPeriods[0];
    }

    if (typeof planningId === 'undefined' || planningId == null) {
        return (<Redirect to={{
            pathname: "/plannings"
        }} />);
    }
    // Renders the main dashboard
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col wrapper-col--flex-end borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <Link to="/plannings" className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft} />{' '}
                                <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                            </Link>
                            <div className="title-principal mt-3">
                                <span className="title-principal--inside">
                                    {editingPlanningName ? <Translate id="planning.editing-name" options={{ renderInnerHtml: true }}/> : ''}
                                </span>

                                {editingPlanningName ? (
                                    <input
                                        className={"form-control-medium"}
                                        type="text"
                                        value={newPlanningName}
                                        onChange={handlePlanNameChange}
                                        onBlur={handleEditPlanName}
                                        autoFocus
                                        maxLength="80"
                                    />
                                ) : (
                                    <span>{planningName}</span>
                                )}
                                {' '}
                                <button onClick={handleEditPlanName} className="button-edit-transparent-background">
                                    {editingPlanningName ?  <FontAwesomeIcon icon={faSave} onClick={handleEditPlanName} /> : <FontAwesomeIcon icon={faEdit} />}
                                </button>
                            </div>
                        </div>
                        <div className="col txt-right">
                            <Button className="btn-simple--only-text" onClick={handleShowCalendarModal}><FontAwesomeIcon icon={faCalendarAlt} />{' '} <Translate id="planning.view-calendar" options={{ renderInnerHtml: true }} /></Button>

                            {!isPlanClosed ?
                                <Button className="ml-3 btn btn-danger" value={"1"} onClick={handleShowPlanningClose}>
                                    <FontAwesomeIcon icon={faTimes} />{' '}
                                    <Translate id="planning.planning-close" options={{ renderInnerHtml: true }} />
                                </Button>
                                :
                                <Button className="ml-3 btn btn-grey" value={"1"} onClick={handleShowPlanningClose} disabled={true}>
                                    <FontAwesomeIcon icon={faTimes} />{' '}
                                    <Translate id="planning.planning-is-closed" options={{ renderInnerHtml: true }} />
                                </Button>
                            }
                        </div>
                    </div>
                    <div className='mt-3'>
                    {academicDetails ?
                        <>
                            <span><Translate id="planning.academic-period" options={{ renderInnerHtml: true }} /> {academicPeriod.PeriodNumber}: {" "} <b>{moment(academicPeriod.PeriodFrom, "MM/DD/YYYY HH:mm:ss")
                                .format("MM/DD/YYYY")} - {moment(academicPeriod.PeriodTo, "MM/DD/YYYY HH:mm:ss").format("MM/DD/YYYY")}</b></span>
                            <p className='wrapper-academics-details-title'><Translate id="planning.academic-year" options={{ renderInnerHtml: true }} />: <span className='date-capitalize'>{moment(academicDetails.FullTermStartDate, "MM/DD/YYYY HH:mm:ss").format("MMM YYYY")}</span> {"-"} <span className='date-capitalize'>{moment(academicDetails.FullTermEndDate, "MM/DD/YYYY HH:mm:ss").format("MMM YYYY")}</span></p>
                        </>: null}
                    </div>
                    <div className="section-description wrapper-section-academics">
                        <div className="wrapper-info-success-academics">
                            {academicDetails ?
                                <div className="wrapper-info-academics-details">
                                    <small className={moment().isAfter(moment(academicPeriod.PeriodTo, "MM/DD/YYYY HH:mm:ss")) ? 'wrapper-text-danger' : ''}> <FontAwesomeIcon icon={faClock} className="dt-icons-size" />{" "}
                                        <Translate id="planning.academic-period-close" options={{ renderInnerHtml: true }} />: <b>{moment(academicPeriod.PeriodTo, "MM/DD/YYYY HH:mm:ss").add(academicDetails.DaysToClosePeriod, 'days').format("MM/DD/YYYY")}</b>
                                    </small>
                                    {moment().isAfter(moment(academicPeriod.PeriodTo, "MM/DD/YYYY HH:mm:ss"))
                                        && <p className='wrapper-text-danger'>
                                            <small>
                                                <Translate id="planning.academic-period-close-warning" options={{ renderInnerHtml: true }} />
                                            </small>
                                        </p>
                                    }
                                </div>: null
                            }
                        </div>
                    </div>
                    <div className="section-description mb-3">
                        <div className="wrapper-average-progress">
                            <div className="wrapper-average-progress__note">
                                <small> <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />{" "}
                                    <Translate id="planning.planning-close-manually-note" options={{ renderInnerHtml: true }} />
                                </small>
                            </div>
                        </div>
                    </div>
                    <div className="mb-5"></div>
                    <Row className="tabs-style-message">
                        <Col xs={6} md={3}>
                            <div className={showLessons ? "tab underline-div" : "tab"} onClick={handleShowLessons}>
                                <span><FontAwesomeIcon icon={faBookOpen} />{' '}
                                    <Translate id="planning.lessons" options={{ renderInnerHtml: true }} />
                                </span>
                            </div>
                        </Col>
                        <Col xs={6} md={3}>
                            <div className={showQuizzes ? "tab underline-div" : "tab"} onClick={handleShowQuizzes}>
                                <span><FontAwesomeIcon icon={faTasks} />{' '}
                                    <Translate id="planning.assignments" options={{ renderInnerHtml: true }} />
                                </span>
                            </div>
                        </Col>
                        <Col xs={6} md={3}>
                            <div className={showForums ? "tab underline-div" : "tab"} onClick={handleShowForums}>
                                <span><FontAwesomeIcon icon={faComments} />{' '}
                                    <Translate id="planning.forums" options={{ renderInnerHtml: true }} />
                                </span>
                            </div>
                        </Col>
                        <Col xs={6} md={3}>
                            <div className={showStudents ? "tab underline-div" : "tab"} onClick={handleShowStudents}>
                                <span><FontAwesomeIcon icon={faUserGraduate} />{' '}
                                    <Translate id="planning.groups" options={{ renderInnerHtml: true }} />
                                </span>
                            </div>
                        </Col>
                    </Row>
                    {/************* LESSONS ****************/}
                    {dataLessons !== 'undefined' && dataLessons !== null && dataLessons.length > 0 && showLessons ?
                        <Row style={{ minHeight: 600 }}>
                            <Col xs={12}>
                                {dataLessons.some( dL => dL.isGapClosing === "1") ?
                                <Alert color="success" style={{color: "#4ca7a2"}}>
                                   <FontAwesomeIcon icon={faFlag} className={"mr-3"}/> <Translate id="planning.planning-indicator" />
                                </Alert>
                                  : null}
                                <Row className="animated delay-05s fadeIn danger-btn-simple space-btn-icon secondary-btn-simple table-no-margin-top td-responsive syle-date-two clear-check lesson-traies-col td-inline-tries delete-col-5 td-delete-2btn">
                                    <Col xs={12} className="text-right mt-3 mb-3">
                                        <Button className="btn primary-btn" value={"1"} onClick={handleAddNewItemToPlan} disabled={isPlanClosed}>
                                            <FontAwesomeIcon icon={faPlus} />{' '}
                                            <Translate id="planning.planning-attach-lessons" options={{ renderInnerHtml: true }} />
                                        </Button>
                                    </Col>
                                    <Col xs={12}>
                                        <PlanningDetailAttachedItemsDataTable disabled={isPlanClosed} lessonOrQuiz={"LESSON"} planningName={planningName} planningId={planningId} data={dataLessons} updateParent={handleUpdatePlanningDetail} updateLesson={handleUpdateLesson} />
                                    </Col>
                                </Row>
                            </Col>
                        </Row> :
                        null
                    }
                    {
                        (typeof dataLessons === 'undefined' || dataLessons === null || dataLessons.length === 0)
                            && showLessons ?
                            <Row style={{ minHeight: 600 }}>
                                <Col xs={12}>
                                    <Row className="animated delay-05s fadeIn">
                                        <Col xs={12} className="text-right mt-3">
                                            <Button className="btn primary-btn" value={"1"} onClick={handleAddNewItemToPlan} disabled={isPlanClosed}>
                                                <FontAwesomeIcon icon={faPlus} />{' '}
                                                <Translate id="planning.planning-attach-lessons" options={{ renderInnerHtml: true }} />
                                            </Button>
                                        </Col>
                                        <Col xs={12}>
                                            <div body className="animated delay-05s fadeIn">
                                                <div className="wrapper-not-found-planning-details">
                                                    <p style={{ textAlign: 'center' }}>
                                                        <FontAwesomeIcon icon={faBookOpen} size='3x' />{' '}
                                                    </p>
                                                    <Translate id="planning.planning-no-lessons"
                                                        options={{ renderInnerHtml: true }} />
                                                </div>
                                            </div>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row> : null
                    }

                    {/************* QUIZZES ****************/}
                    {typeof dataQuizzes !== 'undefined' && dataQuizzes !== null && dataQuizzes.length > 0 && showQuizzes ?
                        <Row style={{ minHeight: 600 }}>
                            <Col xs={12}>
                                <Row className="animated delay-05s fadeIn danger-btn-simple space-btn-icon secondary-btn-simple table-no-margin-top td-responsive syle-date-two clear-check lesson-traies-col td-inline-tries delete-col-3 delete-before-4 delete-col-5">
                                    <Col xs={12} className="text-right mt-3 mb-3">
                                        <Button className="btn primary-btn" value={"0"} onClick={handleAddNewItemToPlan} disabled={isPlanClosed}>
                                            <FontAwesomeIcon icon={faPlus} />{' '}
                                            <Translate id="planning.planning-attach-homework" options={{ renderInnerHtml: true }} />
                                        </Button>
                                    </Col>
                                    <Col xs={12}>
                                        <PlanningDetailAttachedItemsDataTable lessonOrQuiz={"QUIZ"} planningId={planningId} data={dataQuizzes} updateParent={handleUpdatePlanningDetail} updateQuiz={handleUpdateQuizz} disabled={isPlanClosed} />
                                    </Col>
                                </Row>
                            </Col>
                        </Row> :
                        null
                    }
                    {
                        (typeof dataQuizzes === 'undefined' || dataQuizzes === null || dataQuizzes.length === 0)
                            && showQuizzes ?
                            <Row style={{ minHeight: 600 }}>
                                <Col xs={12}>
                                    <Row className="animated delay-05s fadeIn">
                                        <Col xs={12} className="text-right">
                                            <Button className="btn primary-btn" value={"0"} onClick={handleAddNewItemToPlan} disabled={isPlanClosed}>
                                                <FontAwesomeIcon icon={faPlus} />{' '}
                                                <Translate id="planning.planning-attach-homework" options={{ renderInnerHtml: true }} />
                                            </Button>
                                        </Col>
                                        <Col xs={12}>
                                            <div body className="animated delay-05s fadeIn">
                                                <div className="wrapper-not-found-planning-details">
                                                    <p style={{ textAlign: 'center' }}>
                                                        <FontAwesomeIcon icon={faTasks} size='3x' />{' '}
                                                    </p>
                                                    <Translate id="planning.planning-no-quizzes" options={{ renderInnerHtml: true }} />
                                                </div>
                                            </div>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row> : null
                    }
                    {/************* FORUMS ****************/}
                    {showForums ?
                        <ForumsDataTable teacherPlanId={planningId} disabled={isPlanClosed} /> : null
                    }
                    {showStudents ?
                        <StudentsPlanningDataTable teacherPlanId={planningId} disabled={isPlanClosed} /> : null
                    }
                </div>
            </div>
            {loading ? <LoaderFullWindow /> : null}
            {showCalendar ? <ModalCalendar visible={showCalendar} teacherPlanId={planningId} updateParent={hideCalendarModal} /> : null}
            {showPlanningClose ? <ModalPlanningClose visible={showPlanningClose} updateParent={hidePlanningCloseModal} planningId={planningId} /> : null}
            {genericModal.visible ? <ModalGeneric data={genericModal} updateParent={toggleAlertModal} /> : null}
            {showModalPlanningAttachItem ? <ModalPlanningAttachItem visible={showModalPlanningAttachItem} defaultTypeAssignmentOrLessons={defaultTypeAssignmentOrLessons} planningId={planningId} updateParent={handleCloseModalPlanningAttachItem} isCatalog={false} /> : null}
        </div>
    );
};

export default withLocalize(PlanningDetail);
