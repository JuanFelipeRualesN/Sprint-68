import React, {useEffect, useState} from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Col } from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getTeacherPlanningStudentHomeworkDetails } from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import {
    faEdit, faGraduationCap
} from '@fortawesome/free-solid-svg-icons';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import ModalCustomQuizCorrection from '../modal/modal-custom-quiz-correction';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import {formatUtcToLocal, getTimeZone} from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";

/**
 * Data table to handle the teacher planning homework student results
 */
const PlanningHomeworkStudentDetailsDataTable = ({
                                                     isViewByTeacher,
                                                     planningId,
                                                     studentId,
                                                     studentName,
                                                     redoSearch
                                                 }) => {
    const [average, setAverage] = useState('');
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const headerOptions = [
        { key: "0", name: "" },
        { key: "1", name: LanguageManager.getTranslation("progress.quiz-title", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("progress.average", Settings.LANGUAGES) },
        { key: "3", name: LanguageManager.getTranslation("progress.status", Settings.LANGUAGES) },
        { key: "4", name: LanguageManager.getTranslation("progress.due-date", Settings.LANGUAGES) },
        { key: "5", name: LanguageManager.getTranslation("progress.completed-date", Settings.LANGUAGES) },
        { key: "6", name: "" },
    ];
    const [showModalQuizCorrection, setShowModalQuizCorrection] = useState(false);
    const [planQuizIdToCorrect, setPlanQuizIdToCorrect] = useState(null);
    const [studentQuizId, setStudentQuizId] = useState(null);
    const language = LanguageManager.getLanguage();

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        if (redoSearch) doSearch();
    }, [redoSearch]);

    const handleCloseModal = () => {
        setShowModalQuizCorrection(false);
        doSearch();
    };

    const handleViewClick = (planQuizId) => (newValue) => {
        setStudentQuizId(newValue.currentTarget.value);
        setShowModalQuizCorrection(true);
        setPlanQuizIdToCorrect(planQuizId);
    };

    const convertDataToDataTable = (dataToConvert) => {
        if (!dataToConvert || dataToConvert.length === 0) {
            setData([]);
            return;
        }

        const formattedData = dataToConvert.map((item) => ({
            ForGrading: item.ForGrading === "1" ? (
                <span className="dt-icons-colored" data-tooltip="Cuenta para nota">
                    <FontAwesomeIcon icon={faGraduationCap} className="dt-icons-size" />
                </span>
            ) : (
                ""
            ),
            Title: item.Title,
            Average: item.TotalPercent,
            Status: item.Status,
            DueDate: formatUtcToLocal(item.DueDate, language),
            DateCompleted:  formatUtcToLocal(item.SubmittedDate, language),
            View:
                !isViewByTeacher && item.Status !== "C" ? null : (
                    <button
                        disabled={!item.UserQuizResultId}
                        className="btn-default button-secondary-custom"
                        onClick={handleViewClick(item.TeacherPlanQuizId)}
                        value={item.UserQuizResultId}
                    >
                        <FontAwesomeIcon icon={faEdit} />{' '}
                        {SessionManager.getSessionDetails().Roles.includes(Settings.ROLE_TEACHER_STR) ? (
                            <Translate id="planning.view-teacher" options={{ renderInnerHtml: true }} />
                        ) : (
                            <Translate id="planning.view-only" options={{ renderInnerHtml: true }} />
                        )}
                    </button>
                ),
            value: item.Id,
        }));

        setData(formattedData);
    };

    const doSearch = () => {
        setLoading(true);
        getTeacherPlanningStudentHomeworkDetails(planningId, studentId).then((response) => {
            setAverage(response.data.OverallAverage);
            const assignmentDetails =
                response.data.PlanDetails.length > 0 ? response.data.PlanDetails[0].AssignmentDetails : null;
            convertDataToDataTable(assignmentDetails);
            setLoading(false);
        });
    };

    return (
        <div>
            <div className="wrapper-average-progress">
                <div className="wrapper-average-progress__note">
                    <small>
                        <FontAwesomeIcon icon={faGraduationCap} className="dt-icons-size" />{" "}
                        <Translate id="progress.for-grading" options={{ renderInnerHtml: true }} />
                    </small>
                </div>
                <div className="wrapper-average-progress__title">
                    <Translate id="progress.homework-average" options={{ renderInnerHtml: true }} /> {': '}
                    <b>{average}</b>
                </div>
            </div>

            <DataTable headerOptions={headerOptions} data={data} />
            {loading && <LoaderFullWindow />}
            <Col xs={12}>
                <hr />
            </Col>

            {showModalQuizCorrection && (
                <ModalCustomQuizCorrection
                    visible={showModalQuizCorrection}
                    planQuizId={planQuizIdToCorrect}
                    studentId={studentId}
                    studentQuizId={studentQuizId}
                    updateParent={handleCloseModal}
                />
            )}
        </div>
    );
};

export default PlanningHomeworkStudentDetailsDataTable;