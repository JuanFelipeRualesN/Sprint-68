import React, { useEffect, useState, useCallback } from "react";
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Col } from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getTeacherPlanningStudentAverage } from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    faCheck,
    faEye,
    faChartPie, faGraduationCap, faInfoCircle,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ModalReportPlanningLessonAverage from '../modal/modal-report-planning-lesson-average';
import ModalPrePostResults from '../modal/modal-pre-post-results';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {formatUtcToLocal, getTimeZone} from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";
/**
 * Data table to handle the teacher planning lesson student results
 */
const PlanningLessonStudentResultsDataTable = ({ planningId, studentId, studentName, redoSearch }) => {
    const [average, setAverage] = useState("");
    const [currentPage, setCurrentPage] = useState(1);
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [lessonId, setLessonId] = useState(null);
    const [teacherPlanLessonId, setTeacherPlanLessonId] = useState(null);
    const [lessonTitle, setLessonTitle] = useState("");
    const [visibleModalPlanningLessonAverage, setVisibleModalPlanningLessonAverage] = useState(false);
    const [visibleModalPrePostTest, setVisibleModalPrePostTest] = useState(false);
    const [language, setLanguage] = useState(LanguageManager.getLanguage());

    const headerOptions = [
        { key: "0", name: "" },
        { key: "1", name: LanguageManager.getTranslation("progress.lesson-title", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("progress.average", Settings.LANGUAGES) },
        { key: "3", name: (
                <span className="header-with-tooltip">
                  {LanguageManager.getTranslation("progress.completed", Settings.LANGUAGES)}{" "}
                                <span
                                    className="info-icon"
                                    style={{marginLeft: 2}}
                                    data-tooltip={LanguageManager.getTranslation("progress.completed-tooltip", Settings.LANGUAGES)}
                                > {' '}
                    <FontAwesomeIcon icon={faInfoCircle} />
                  </span>
                </span>
            ) },
        { key: "4", name: LanguageManager.getTranslation("progress.completed-date", Settings.LANGUAGES) },
        { key: "5", name: "" },
        { key: "6", name: LanguageManager.getTranslation("progress.pre-post-results", Settings.LANGUAGES) },
    ];

    const convertDataToDataTable = useCallback((dataToConvert) => {
        const newArray = [];
        console.log("Data to convert", dataToConvert);
        if (Array.isArray(dataToConvert) && dataToConvert.length > 0) {
            dataToConvert.forEach((item) => {
                const completed = item.Status === "C" ? (
                    <FontAwesomeIcon icon={faCheck} className="green-text" />
                ) : (
                    ""
                );

                newArray.push({
                    ForGrading:
                        item.ForGrading === "1" ? (
                            <span className="dt-icons-colored" data-tooltip="Cuenta para nota">
                             <FontAwesomeIcon icon={faGraduationCap} className="dt-icons-size" />
                            </span>
                        ) : (
                            ""
                        ),
                    LessonTitle: item.Title,
                    Average: item.TotalPercent,
                    Completed: completed,
                    DateCompleted: formatUtcToLocal(item.SubmittedDate, language),
                    View: (
                        <button
                            type="button"
                            className="btn-default button-secondary-custom"
                            onClick={() => handleViewClick(item.LessonId, item.Title, item.TeacherPlanLessonId)}
                            value={item.LessonId}
                            name={item.Title}
                        >
                            <FontAwesomeIcon icon={faEye} /> <Translate id="planning.view-only" options={{ renderInnerHtml: true }} />
                        </button>
                    ),
                    Vie2: (
                        <button
                            type="button"
                            className="btn-default button-secondary-custom btn-variant-responsive"
                            onClick={() => handleClickOpenModalPrePost(item.LessonId, item.Title, item.TeacherPlanLessonId)}
                        >
                            <FontAwesomeIcon icon={faChartPie} /> <Translate id="planning.results-pre-post" options={{ renderInnerHtml: true }} />
                        </button>
                    ),
                    value: item.Id,
                });
            });
        }

        setData(newArray);
    }, []);

    const doSearch = useCallback(() => {
        setLoading(true);
        getTeacherPlanningStudentAverage(planningId, studentId).then((response) => {
            const details = response.data.PlanDetails?.[0]?.AssignmentDetails || [];
            setAverage(response.data.OverallAverage);
            setLoading(false);
            convertDataToDataTable(details);
        });
    }, [planningId, studentId, convertDataToDataTable]);

    useEffect(() => {
        doSearch();
    }, [doSearch]);

    useEffect(() => {
        doSearch();
    }, [redoSearch, doSearch]);

    const handleViewClick = (id, title, planLessonId) => {
        setLessonId(id);
        setLessonTitle(title);
        setTeacherPlanLessonId(planLessonId);
        setVisibleModalPlanningLessonAverage(true);
    };

    const closeModalPlanningLessonAverage = () => {
        setVisibleModalPlanningLessonAverage(false);
    };

    const toggleModalPrePost = (visible) => {
        setVisibleModalPrePostTest(visible);
    };

    const handleClickOpenModalPrePost = (lessonId, lessonTitle, teacherPlanLessonId) => {
        setLessonId(lessonId);
        setLessonTitle(lessonTitle);
        setTeacherPlanLessonId(teacherPlanLessonId);
        setVisibleModalPrePostTest(true);
    };

    return (
        <div>
            <div className="wrapper-average-progress">
                <div className="wrapper-average-progress__note">
                    <small>
                        <FontAwesomeIcon icon={faGraduationCap} className="dt-icons-size" /> <Translate id="progress.for-grading" options={{ renderInnerHtml: true }} />
                    </small>
                </div>
                <div className="wrapper-average-progress__title">
                    <Translate id="progress.lesson-average" options={{renderInnerHtml: true}}/>: <b>{average}</b>
                </div>
            </div>

            <DataTable headerOptions={headerOptions} data={data}/>
            {loading && <LoaderFullWindow/>}

            <Col xs={12}>
                <hr/>
            </Col>

            <Col xs={12}>
                {visibleModalPlanningLessonAverage && (
                    <ModalReportPlanningLessonAverage
                        visible={visibleModalPlanningLessonAverage}
                        studentName={studentName}
                        studentId={studentId}
                        lessonId={lessonId}
                        planningId={planningId}
                        lessonTitle={lessonTitle}
                        planLessonId={teacherPlanLessonId}
                        updateParent={closeModalPlanningLessonAverage}
                    />
                )}

                {visibleModalPrePostTest && (
                    <ModalPrePostResults
                        visible={visibleModalPrePostTest}
                        studentName={studentName}
                        studentId={studentId}
                        lessonId={lessonId}
                        planningId={planningId}
                        lessonTitle={lessonTitle}
                        teacherPlanLessonId={teacherPlanLessonId}
                        updateParent={toggleModalPrePost}
                    />
                )}
            </Col>
        </div>
    );
};

export default PlanningLessonStudentResultsDataTable;