import React, { useEffect, useState } from "react";
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import {getQuizzesTries} from "../../../api/gsweb";
import {getStudentLessonAverage, getStudentPlanLessonIdQuizResult} from '../../../api/teachers';
import LanguageManager from '../../../custom-classes/language-manager';
import SelectorExamV2 from '../selector/selector-exam-v2';
import TableTotalTries from '../table/table-total-tries';
import RandomRGBA from '../../../custom-classes/rgba-random';
import LoaderFullWindow from "../loader-windows/loader-full-window";
import "./style.css";
import Settings from "../../../custom-classes/settings";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import {formatUtcToLocal} from "../../../utils/utils";
import {Button} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEye} from "@fortawesome/free-solid-svg-icons";
/**
 * Class/component of the student average
 */
const ReportStudentPlanningLessonAverage = ({ planningId, lessonId, lessonTitle, studentId, studentName, planLessonId = null }) => {
    const [examId, setExamId] = useState("0");
    const [languages] = useState(Settings.LANGUAGES);
    const [loading, setLoading] = useState(false);
    const [passedTotal, setPassedTotal] = useState("0");
    const [notPassedTotal, setNotPassedTotal] = useState("0");
    const [triesData, setTriesData] = useState(null);
    const [triesChartData, setTriesChartData] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        callResults();
    }, []);

    const callResults = () => {
        setLoading(true);
        getStudentLessonAverage(planningId, studentId, lessonId)
            .then((response) => {
                const result = !isNaN(response.data) ? parseFloat(response.data) : response.data;
                setPassedTotal(result);
                setNotPassedTotal(!isNaN(response.data) ? (100 - parseFloat(response.data)).toFixed(2) : response.data);
                setLoading(false);
            });
    };

    const callTries = () => (newValue) => {
        if (typeof newValue === 'undefined') {
            setExamId(null);
            setTriesData(null);
            setTriesChartData(null);
            return;
        }

        setLoading(true);
        setExamId(null);
        setTriesData(null);
        setTriesChartData(null);

        setExamId(newValue);

        if(planLessonId !== null){
            // Call from teachers API
            let results =
                getStudentPlanLessonIdQuizResult(planLessonId, studentId, newValue).then((response)=> {

                    if(response.data.UserQuizResultId !== null){
                        let tryData = [
                            {
                                "Id": response.data.UserQuizResultId,
                                "TryNo": "1",
                                "TotalPercent": response.data.UserQuizFullDetails.TotalPercent,
                                "CreatedAt": response.data.UserQuizFullDetails.CreatedAt
                            }
                        ];
                        setTriesData(tryData);
                    }


                    const labelTry = LanguageManager.getTranslation("report-student-average.try", languages);
                    const labelTries = LanguageManager.getTranslation("report-student-average.tries", languages);

                    const labels = [];
                    const backgroundColors = [];
                    const datasets = [{label: '# ' + labelTries, borderWidth: 1}];
                    const dataArray = [];

                    for (let i = 0; i < response.data.length; i++) {
                        labels.push(labelTry + ' ' + (i + 1));
                        dataArray.push(response.data[i].TotalPercent);
                        backgroundColors.push(RandomRGBA.randomRgba());
                    }

                    datasets[0].data = dataArray;
                    datasets[0].fillColor = backgroundColors;
                    datasets[0].borderColor = backgroundColors;

                    setTriesChartData({ labels, datasets });
                    setLoading(false);
                });
        }
        else{
            getQuizzesTries(newValue, studentId)
                .then((response) => {
                    setTriesData(response.data);

                    const labelTry = LanguageManager.getTranslation("report-student-average.try", languages);
                    const labelTries = LanguageManager.getTranslation("report-student-average.tries", languages);

                    const labels = [];
                    const backgroundColors = [];
                    const datasets = [{label: '# ' + labelTries, borderWidth: 1}];
                    const dataArray = [];

                    for (let i = 0; i < response.data.length; i++) {
                        labels.push(labelTry + ' ' + (i + 1));
                        dataArray.push(response.data[i].TotalPercent);
                        backgroundColors.push(RandomRGBA.randomRgba());
                    }

                    datasets[0].data = dataArray;
                    datasets[0].fillColor = backgroundColors;
                    datasets[0].borderColor = backgroundColors;

                    setTriesChartData({ labels, datasets });
                    setLoading(false);
                });
        }
    };

    return (
        <div>
            {loading && <LoaderFullWindow />}
            <div className="wrapper-col wrapper-col__align-top wrapper-col__wrap-responsive mb-5 mt--5">
                <div className="col-auto-size">
                    <h1 className="progress-title">
                        <Translate id="report-student-average.student-lesson-average" options={{ renderInnerHtml: false }} />
                    </h1>
                    <div className="progress-student">
                        <Translate id="report-student-average.student" options={{ renderInnerHtml: true }} />: <b>{studentName}</b>
                    </div>
                    <div className="progress-lesson">
                        <Translate id="report-student-average.lesson" options={{ renderInnerHtml: true }} />: <b>{lessonTitle}</b>
                    </div>
                </div>
                <div className="col-fixed-size-second text-center progress-border-left">
                    <p className="progress-charts">
                        <Translate id="report-student-average.lesson-score" options={{ renderInnerHtml: false }} />
                    </p>
                    {studentId !== null && lessonId !== null && passedTotal !== "0" && notPassedTotal !== "0" && <h1>{passedTotal}</h1>}
                </div>
            </div>
            <div className="module-progress-exam">
                <h2 className="progress-sub-title">
                    <Translate id="report-student-average.average-lesson" options={{ renderInnerHtml: false }} />
                </h2>
                <div className="module-progress-exam__dropdown">
                    <p className="progress-charts">
                        <Translate id="report-student-average.select-exam" options={{ renderInnerHtml: true }} />
                    </p>
                    {lessonId !== null && studentId !== null && (
                        <div className="gray-dark-text">
                            <SelectorExamV2 lessonId={lessonId} updateParent={callTries()} />
                        </div>
                    )}
                </div>
                <div className="module-progress-exam__table mt-3">
                    {examId !== "0" && lessonId !== null && studentId !== null && triesData !== null && triesData.length > 0 && (
                        <TableTotalTries
                            showTries={planLessonId === null && planningId === null}
                            data={triesData}
                            profileId={typeof studentId.value === 'undefined' ? studentId : studentId.value}
                            refresh={triesData}
                        />
                    )}
                </div>
            </div>
        </div>
    );
};

export default withLocalize(ReportStudentPlanningLessonAverage);