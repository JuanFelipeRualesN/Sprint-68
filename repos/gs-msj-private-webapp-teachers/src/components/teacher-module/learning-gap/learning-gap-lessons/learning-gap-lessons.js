import React, { useState, useEffect, useCallback } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import DataTable from '../../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    faEye, faPlusSquare, faChalkboardTeacher, faEdit, faTrash, faExclamationTriangle, faPlus,
} from '@fortawesome/free-solid-svg-icons';
import { withLocalize, LocalizeProvider } from 'react-localize-redux';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../../custom-classes/language-manager";
import LocalizedDatePicker from '../../../../utils/datepicker-locale';
import "react-datepicker/dist/react-datepicker.css";
import moment from "moment";
import "moment-timezone";
import {useHistory} from 'react-router-dom';
import {
    deleteLearningGapAssignLessonToStudent,
    getLearningGapReadStudentLessonStatus, putLearningGapAssignLessonToStudent
} from "../../../../api/teachers";
import ImgAvanza from "../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import LoaderFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import LearningGapLessonView from "./learning-gap-lesson-view/learning-gap-lesson-view";
import {PulseLoader} from "react-spinners";
import LearningGapAddLessonForm from "./learning-gap-add-lesson-form/learning-gap-add-lesson-form";
import { Provider } from 'react-redux';
import store from '../../../../redux/store';

/**
 * To display the lessons assigned to the student.
 * @param translate The translate function
 * @param studentId The student ID
 * @param studentName The student name.
 * @param redoSearch A boolean to indicate if the search should be redone
 * @returns {Element} The LearningGapLessons component
 * @constructor
 */
const LearningGapLessons = ({ translate, studentId, studentName, redoSearch }) => {
    const [data, setData] = useState([]);
    const [viewLessonModalShow, setViewLessonModalShow] = useState(false);
    const [deleteLessonModalShow, setDeleteLessonModalShow] = useState(false);
    const [lessonId, setLessonId] = useState(null);
    const [selectedLesson, setSelectedLesson] = useState(null);
    const [addLessonModalShow, setAddLessonModalShow] = useState(false);
    const [selectedDate, setSelectedDate] = useState(null);
    const [editLessonModalShow, setEditLessonModalShow] = useState(false);
    const [headerOptions, setHeaderOptions] = useState([
        { key: "0", name: translate("learning-gap.title")},
        { key: "1", name: translate("learning-gap.limit-date")},
        { key: "2", name: "" },
        { key: "3", name: "" },
        { key: "4", name: "" }
    ]);
    const [loading, setLoading] = useState(false);
    const history = useHistory(); // Initialize useHistory hook
    const [smallLoading, setSmallLoading] = useState(false);

    const convertDataToDataTable = useCallback((dataToConvert) => {
        const newArray = dataToConvert.map(item => ({
            LessonTitle: item.LessonTitle + '<br/><span class="learning-gap-subtext">' + item.SubjectName + ' ' + item.LevelName + '</span>',
            LimitDate: item.LimitDate ? (
                <>
                    {moment.utc(item.LimitDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")}
                    {' '} <span className="btn only-icon btn btn-secondary"
                                onClick={() => editLesson(item)}
                >
                    <FontAwesomeIcon icon={faEdit} /></span>
                </>
            ) : (
                ""
            ),
            Show: <Button className="learning-gap-button-present-lesson "
                          onClick={() => showLesson(item)}
                          value={item.Id}>
                <FontAwesomeIcon icon={faChalkboardTeacher}/>{' '}
                <Translate id="learning-gap.show" options={{renderInnerHtml: true}}/>
            </Button>,
            View:  <button type="button" className="btn-default button-secondary-custom"
                             onClick={() => viewLesson(item)}
                             value={item.Id}>
                <FontAwesomeIcon icon={faEye}/>{' '}
                <Translate id="learning-gap.view" options={{renderInnerHtml: true}}/>
            </button>,
            Delete: <Button className="btn btn-danger only-icon" onClick={() => deleteLesson(item)}>
                <FontAwesomeIcon className="learning-gap-color-red" icon={faTrash} />
            </Button>,
            value: item.Id
        }));
        setData(newArray);
    }, []);

    useEffect(() => {
        setHeaderOptions([
            { key: "0", name: translate("learning-gap.title") },
            { key: "1", name: translate("learning-gap.limit-date") },
            { key: "2", name: "" },
            { key: "3", name: "" },
            { key: "4", name: "" }]);
    }, [translate])

    const viewLesson = (lesson) => {
        setLessonId(lesson.LessonId);
        setViewLessonModalShow(true);
    }

    const showLesson = (lesson) => {
        setLessonId(lesson.LessonId);
        history.push(`/students/lesson/${lesson.LessonId}?fromLearningGap=true&studentId=${studentId}&studentName=${studentName}`);
    }

    const deleteLesson = (lesson) => {
        setLessonId(lesson.LessonId);
        setSelectedLesson(lesson);
        setDeleteLessonModalShow(true);
    }

    const editLesson = (lesson) => {
        setLessonId(lesson.LessonId);
        setSelectedLesson(lesson);
        setSelectedDate(new Date(lesson.LimitDate));
        setEditLessonModalShow(true);
    }

    const handleDateChange = (date) => {
        setSelectedDate(date);
    };

    const saveUpdatedLessonRequest = () => {
        setSmallLoading(true);
        const jsDate = new Date(selectedDate);
        const isoString = jsDate.toISOString();
        putLearningGapAssignLessonToStudent(selectedLesson.Id, isoString, selectedLesson.LessonId).then(() => {
            setSmallLoading(false);
           setEditLessonModalShow(false);
              callStudentLessonProgress();
        }).catch(() => {
            setSmallLoading(false);
        });
    }

    const deleteLessonRequest = () => {
        setSmallLoading(true);
        deleteLearningGapAssignLessonToStudent(selectedLesson.Id).then(() => {
            setSmallLoading(false);
            setDeleteLessonModalShow(false);
            callStudentLessonProgress();
        }).catch(() => {
            setSmallLoading(false);
        });
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

    // Minimum date is tomorrow
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    return (
        <div className="learning-gap-fade-in">
            {loading ?   <div style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                width: "100%",
                height: "10vh"
            }}> <PulseLoader size={10} color="#395a8f" /> </div> :
            <>
            <Row className="align-vertical-center">
                <Col xs={8} className="align-vertical-center">
                    <span className="learning-gap-subtitle">
                        <Translate id="learning-gap.assigned-lessons" options={{renderInnerHtml: true}}/></span>
                </Col>
                <Col style={{textAlign: "right"}}>
                    <Button className="btn primary-btn btn btn-secondary"
                    onClick={()=>{setAddLessonModalShow(true)}}>
                        <FontAwesomeIcon icon={faPlus} />{' '}
                        <Translate id="learning-gap.assign-lesson" options={{renderInnerHtml: true}}/>
                    </Button>
                </Col>
            </Row>
            {data.length === 0 ? (
                <div className="learning-gap-welcome-area">
                    <Row>
                        <Col xs={12} style={{textAlign: "center"}}>
                            <img src={ImgAvanza} alt="Avanza" className="img-fluid" style={{maxWidth: 50, width: "100%", opacity: "40%"}} />
                        </Col>
                        <Col xs={12} style={{fontSize: 12, textAlign: "center"}}>
                            {' '}
                            <Translate id="learning-gap.no-progress-lessons" options={{renderInnerHtml: true}}/>
                        </Col>
                    </Row>
                </div>

            ) : (
                <>
                    <DataTable headerOptions={headerOptions} data={data} />
                </>
            )}
            {loading && <LoaderFullWindow />}

            <Modal
                   isOpen={viewLessonModalShow && lessonId !== null} toggle={()=>{setViewLessonModalShow(false)}} className="modal-full-width">
                <ModalHeader toggle={()=>{setViewLessonModalShow(false)}} />
                <ModalBody>
                    <Provider store={store}>
                        <LocalizeProvider store={store}>
                            <LearningGapLessonView lessonId={lessonId} />
                        </LocalizeProvider>
                    </Provider>
                </ModalBody>
            </Modal>

            <Modal isOpen={addLessonModalShow} className="modal-results">
                <ModalHeader toggle={()=>{setAddLessonModalShow(false)}}>
                    <Translate id="learning-gap.assign-lesson" options={{renderInnerHtml: true}}/>
                    <br/>
                    <span style={{fontSize: 13}}><i>
                        <Translate id="learning-gap.assign-all-fields" options={{renderInnerHtml: true}}/></i>
                    </span>
                </ModalHeader>
                <ModalBody>
                   <LearningGapAddLessonForm studentId={studentId} updateParent={()=>{
                          callStudentLessonProgress();
                          setAddLessonModalShow(false);
                   }} />
                </ModalBody>
            </Modal>
                </> }

            <Modal
                isOpen={deleteLessonModalShow && lessonId !== null}
                toggle={()=>{setDeleteLessonModalShow(false)}}>
                <ModalHeader toggle={()=>{setDeleteLessonModalShow(false)}} style={{width: "100%", textAlign: "center"}}>
                </ModalHeader>
                <ModalBody style={{textAlign: "center"}}>
                    <div className="learning-gap-color-red" style={{width: "100%"}}>
                        <FontAwesomeIcon icon={faExclamationTriangle} size="3x" />{' '}
                    </div>
                    <br/>
                    <Translate id="learning-gap.are-you-sure-you-want-to-delete" options={{renderInnerHtml: true}}/>
                    <br/>
                    {selectedLesson != null ? <span><b>{selectedLesson.LessonTitle}</b></span> : ""}
                </ModalBody>
                <ModalFooter>
                    <Button color="primary"
                            onClick={()=>{deleteLessonRequest()}}
                            disabled={smallLoading}>
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <>
                                <Translate id="learning-gap.delete"/></>)}
                    </Button>

                    <Button className="learning-gap-case-none" color="secondary"
                            onClick={()=>{setDeleteLessonModalShow(false)}}
                            disabled={smallLoading}>
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <>
                                <Translate id="learning-gap.cancel"/></>)}
                    </Button>
                </ModalFooter>
            </Modal>

            <Modal
                isOpen={editLessonModalShow && lessonId !== null && selectedLesson != null}
                toggle={()=>{setEditLessonModalShow(false)}}>
                <ModalHeader toggle={()=>{setEditLessonModalShow(false)}} style={{width: "100%", textAlign: "center"}}>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col>
                            <Translate id="learning-gap.modify-the-limit-date-for-lesson" options={{renderInnerHtml: true}}/>:
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            {selectedLesson != null ? <span><b>{selectedLesson.LessonTitle}</b></span> : ""}
                        </Col>
                    </Row>
                    {selectedLesson != null ?
                        <Row>
                            <Col><Translate id="learning-gap.limit-date" options={{renderInnerHtml: true}}/></Col>
                            <Col><LocalizedDatePicker
                                className='form-control'
                                selected={selectedDate}
                                minDate={tomorrow}
                                onChange={handleDateChange}
                                dateFormat="yyyy/MM/dd" // You can change the date format
                            /></Col>
                        </Row>
                     : null}
                </ModalBody>
                <ModalFooter>
                    <Button color="primary"
                            onClick={()=>{saveUpdatedLessonRequest()}}
                            disabled={smallLoading}>
                             {smallLoading ? (
                                <PulseLoader size={10} color="#395a8f" />
                            ) : (
                            <>
                                <Translate id="learning-gap.submit"/></>)}
                    </Button>
                    <Button className="learning-gap-case-none"
                            color="secondary"
                            onClick={()=>{setEditLessonModalShow(false)}}
                            disabled={smallLoading}>
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <>
                                <Translate id="learning-gap.cancel"/></>)}
                    </Button>
                </ModalFooter>
            </Modal>

        </div>
    );
};

export default withLocalize(LearningGapLessons);
