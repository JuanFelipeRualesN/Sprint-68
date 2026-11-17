import React, { useState, useEffect, useCallback } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { withLocalize } from 'react-localize-redux';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';

import {
    getLearningGapReadStudentYearScore,
} from "../../../../../api/teachers";
import {PulseLoader} from "react-spinners";
import LearningGapAnnualScoreArea from "./learning-gap-annual-score/learning-gap-annual-score-area";
import LearningGapDiagnosticScoreArea from "./learning-gap-diagnostic-score/learning-gap-diagnostic-score-area";
import LearningGapCurrentProgressArea from "./learning-gap-current-progress-area/learning-gap-current-progress-area";

/**
 * To handle the learning gap scores area.
 * @param translate The translation function.
 * @param studentId The student ID.
 * @returns {Element} The learning gap scores area.
 * @constructor
 */
const LearningGapScoresArea = ({ translate, studentId }) => {
    const [dataAnnualScore, setDataAnnualScore] = useState([]);
    const [dataDiagnosticScore, setDataDiagnosticScore] = useState([]);
    const [loading, setLoading] = useState(false);

    /**
     * To call the annotations.
     * @type {(function(): void)|*}
     */
    const callLearningGapStudentYearScore = useCallback(() => {

        setLoading(true);
        getLearningGapReadStudentYearScore(studentId).then(response => {
            setLoading(false);
            setDataAnnualScore(response.data);
            setDataDiagnosticScore(response.data);
        });
    }, [studentId]);

    useEffect(() => {
        callLearningGapStudentYearScore();
    }, []);

    const reloadData = () => {
        callLearningGapStudentYearScore();
    }

    return (
        <>
            {loading ? (
                <div
                    style={{
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        width: "100%",
                        height: "10vh",
                    }}
                >
                    <PulseLoader size={10} color="#395a8f" />
                </div>
            ) : (
                <>
                    {  <LearningGapAnnualScoreArea studentId={studentId} data={dataAnnualScore} reloadData={reloadData}/> }
                    {  <LearningGapCurrentProgressArea overallScore={dataAnnualScore} translate={translate} /> }
                    <hr/>
                    {  <LearningGapDiagnosticScoreArea studentId={studentId} data={dataDiagnosticScore} reloadData={reloadData}/> }
                </>
            )}
        </>
    );
};

export default withLocalize(LearningGapScoresArea);

