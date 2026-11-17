import React, { useState, useEffect, useCallback } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col, Modal, ModalHeader, ModalBody} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import { Translate } from "react-localize-redux";
import DataTable from '../../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import moment from "moment";
import "moment-timezone";
import {
    getLearningGapReadStudentLessonQuizResultList,
    getLearningGapReadStudentLessonStatus
} from "../../../../api/teachers";
import LanguageManager from "../../../../custom-classes/language-manager";
import ImgAvanza from "../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChartPie, faPlus} from "@fortawesome/free-solid-svg-icons";
import LearningGapEditableScoreTable from "./learning-gap-editable-score-table.js/learning-gap-editable-score-table";
import {PulseLoader} from "react-spinners";
import LearningGapDiagnosticForm from "../learning-gap-diagnostics/learning-gap-diagnostic-form";
import ImgGapScore from "../../../../assets/images/common-icons/Avanza-icon-Score.png";
import LeaningGapEarningProcessModal from "../learning-gap-earning-process/learning-gap-earning-process";

/**
 * To display the learning gap progress of the student.
 * @param translate The translate function.
 * @param studentId The student ID.
 * @param redoSearch The redo search flag.
 * @param studentName The student name.
 * @returns {Element}
 * @constructor
 */
const LearningGapProgress = ({ translate, studentId, studentName, redoSearch }) => {
    const [data, setData] = useState([]);
    const [quizResultsData, setQuizResultsData] = useState(null);
    const [lessonTitle, setLessonTitle] = useState("");
    const [learningGapAssignedLessonId, setLearningGapAssignedLessonId] = useState(null);
    const [assignDiagnosticTestShow, setAssignDiagnosticTestShow] = useState(false);
    const [lessonAssignedResultsModalShow, setLessonAssignedResultsModalShow] = useState(false);
    const [headerOptions, setHeaderOptions] = useState([
        { key: "0", name: translate("learning-gap.title") },
        { key: "1", name: translate("learning-gap.subject-and-grade") },
        { key: "2", name: translate("learning-gap.limit-date") },
        { key: "3", name: translate("learning-gap.status") },
        { key: "4", name: translate("learning-gap.results") },
    ]);
    const [loading, setLoading] = useState(false);
    const [smallLoading, setSmallLoading] = useState(false);
    const [learningGapModalShow, setLearningGapModalShow] = useState(true);

    useEffect(() => {
        setHeaderOptions([
            { key: "0", name: translate("learning-gap.title") },
            { key: "1", name: translate("learning-gap.subject-and-grade") },
            { key: "2", name: translate("learning-gap.limit-date") },
            { key: "3", name: translate("learning-gap.status") },
            { key: "4", name: translate("learning-gap.results") },
        ]);
    }, [translate])

    const convertDataToDataTable = useCallback((dataToConvert) => {

        const newArray = dataToConvert.map(item => ({
            LessonTitle: item.LessonTitle,
            SubjectAndGrade: item.SubjectName + ' ' + item.LevelName,
            LimitDate: item.LimitDate ?
                moment.utc(item.LimitDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY") : ""
            ,
            Status: item.Completed === "1" ?
                <Translate id="learning-gap.completed"/>  :
                <Translate id="learning-gap.pending"/>,
            Results: <Button className="learning-gap-button-present-lesson"
                             onClick={() => selectAssignLessonId(item)}
                              value={item.Id}  disabled={smallLoading}>
                {smallLoading ? (
                    <PulseLoader size={10} color="#395a8f" />
                ) :
                    <>
                <FontAwesomeIcon icon={faChartPie}/>{' '}
                <Translate id="learning-gap.results" options={{renderInnerHtml: true}}/>
                    </> }
            </Button>,
            value: item.Id
        }));
        setData(newArray);
    }, []);

    /**
     * To set the lesson title and call the API to get the student lesson progress.
     * @param e The event.
     * @returns {Promise<void>}
     */
    const selectAssignLessonId = async (e) => {
        setLessonTitle(e.LessonTitle);
        try {
            setLearningGapAssignedLessonId(e.Id);
            await callStudentLessonProgressPerQuiz(e.Id)
        } catch (error) {
            console.error("Error fetching lesson progress:", error);
            // Optionally show an error message to the user
        }
    };

    /**
     * To call the API to get the student lesson progress per quiz.
     * @type {(function(*): void)|*}
     */
    const callStudentLessonProgressPerQuiz = useCallback((assignedLessonId) => {
         setSmallLoading(true);
        getLearningGapReadStudentLessonQuizResultList(assignedLessonId).then(response => {
            setSmallLoading(false);
            setQuizResultsData(response.data);
            setLessonAssignedResultsModalShow(true);
        });
    }, []);

    const closeResultsModal = () => {
        setLessonAssignedResultsModalShow(false);
        // Reread the data
        callStudentLessonProgress();
    }

    /**
     * To call the API to get the student lesson progress.
     * @type {(function(): void)|*}
     */
    const callStudentLessonProgress = useCallback(() => {
        setLoading(true);
        getLearningGapReadStudentLessonStatus(studentId).then(response => {
            setLoading(false);
            convertDataToDataTable(response.data);
        });
    }, [studentId, convertDataToDataTable]);

    useEffect(() => {
        callStudentLessonProgress();
    }, [callStudentLessonProgress]);

    // Trigger re-fetch when the tab is clicked
    useEffect(() => {
        if (redoSearch) {
           callStudentLessonProgress();
        }
    }, [redoSearch, callStudentLessonProgress]);

    return (
        <div>
            <Row className="learning-gray-areas align-vertical-center learning-gap-fade-in">
                <Col xs={7} sm={8} md={7} lg={9} className="align-vertical-center">
                    <b><Translate id="learning-gap.add-diagnostic-test" options={{renderInnerHtml: true}} /></b>
                </Col>
                <Col xs={5} sm={4} md={4} lg={3} className="learning-gray-areas align-vertical-center" style={{textAlign: "right"}}>
                <button className="btn-default button-primary-custom mr-2" style={{margin: "0 auto"}}
                            onClick={ ()=> setAssignDiagnosticTestShow(true)}>
                        <FontAwesomeIcon icon={faPlus} />
                        <Translate id="learning-gap.assign-test" options={{renderInnerHtml: true}} />
                    </button>
                </Col>
            </Row>
            <Row>
                <Col xs={12}>
                    <span className="learning-gap-subtitle"> <Translate id="learning-gap.tests-progress" options={{renderInnerHtml: true}}/></span>
                </Col>
            </Row>
            {loading ?   <div style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                width: "100%",
                height: "10vh"
            }}> <PulseLoader size={10} color="#395a8f" /> </div> : <>
            {data.length === 0 ? (
                    <div className="learning-gap-welcome-area learning-gap-fade-in">
                        <Row>
                            <Col xs={12} style={{textAlign: "center"}}>
                                <img src={ImgAvanza} alt="Avanza" className="img-fluid" style={{maxWidth: 50, width: "100%", opacity: "40%"}} />
                            </Col>
                            <Col xs={12} style={{fontSize: 12, textAlign: "center"}}>
                                {' '}
                               <Translate id="learning-gap.no-assigned-lessons" options={{renderInnerHtml: true}}/>
                            </Col>
                        </Row>
                    </div>

            ) : (
                <>
                    <DataTable headerOptions={headerOptions} data={data} />
                </>
            )}
            </> }
            <Modal isOpen={assignDiagnosticTestShow} toggle={()=> setAssignDiagnosticTestShow(false)} size="xl">
                <ModalHeader toggle={()=> setAssignDiagnosticTestShow(false)} >
                    <b><Translate id="learning-gap.administrate-diagnostic-test" options={{renderInnerHtml: true}}/></b>
                    {' '}
                    <img src={ImgGapScore} alt="Avanza" style={{ width: 30, marginLeft: 20 }} />
                </ModalHeader>
                <ModalBody>
                  <LearningGapDiagnosticForm studentId={studentId} updateParent={() => {setAssignDiagnosticTestShow(false)}} />
                </ModalBody>
            </Modal>

            <Modal isOpen={lessonAssignedResultsModalShow} toggle={()=>
                closeResultsModal()}
            className="learning-gap-student-quiz-grades-modal">
                <ModalHeader toggle={()=> closeResultsModal()} >
                    <b><Translate id="learning-gap.score-lesson" options={{renderInnerHtml: true}}/></b>
                </ModalHeader>
                <ModalBody>
                    <div className="learning-gap-quiz-results-info-area">
                        <span className="learning-gap-quiz-results-info-area-title">
                            <Translate id="learning-gap.student" options={{renderInnerHtml: true}}/>: {studentName}
                        </span>
                        <br/>
                        <span className="learning-gap-quiz-results-info-area-subtitle">
                            <Translate id="learning-gap.lesson" options={{renderInnerHtml: true}}/>: {lessonTitle}
                        </span>
                    </div>

                    <div className="learning-gap-quiz-results-score-area">
                    <LearningGapEditableScoreTable
                                                   learningGapAssignedLessonId={learningGapAssignedLessonId}
                                                   studentId={studentId} data={quizResultsData} />
                    </div>
                </ModalBody>
            </Modal>
            <LeaningGapEarningProcessModal isOpen={learningGapModalShow} studentId={studentId} toggle={()=>{
                setLearningGapModalShow(false)
            }} updateParent={()=>{
                setLearningGapModalShow(false)}} />
        </div>
    );
};

export default withLocalize(LearningGapProgress);
