/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import PieChart from './chart-pie';
/** Multiple Language imports **/
import LanguageManager from '../../../custom-classes/language-manager';
/** End of Multiple Language imports **/

/**
 * Manages the student average by Subject and grade
 */
const ChartStudentAverage = ({ data }) => {
    const [chartData, setChartData] = useState(null);
    const [languages, setLanguages] = useState(data.languages);
    const [notPassedTotal, setNotPassedTotal] = useState(data.notPassedTotal);
    const [passedTotal, setPassedTotal] = useState(data.passedTotal);
    const [productId, setProductId] = useState(data.productId);
    const [studentId, setStudentId] = useState(data.studentId);

    // Sets the chart data
    const setData = () => {
        let labelPassed = LanguageManager.getTranslation("report-student-average.passed", languages);
        let labelNotPassed = LanguageManager.getTranslation("report-student-average.not-passed", languages);

        let dataChart = [{
            value: notPassedTotal,
            color:"#F7464A",
            highlight: "#ce0606",
            label: labelNotPassed
        }, {
            value: passedTotal,
            color: "#42f456",
            highlight: "#a8ff7a",
            label: labelPassed
        }];

        setChartData(dataChart);
    };

    // Called on the first time
    useEffect(() => {
        setData();
    }, []);

    // Call when updated
    useEffect(() => {
        setLanguages(data.languages);
        setNotPassedTotal(data.notPassedTotal);
        setPassedTotal(data.passedTotal);
        setProductId(data.productId);
        setStudentId(data.studentId);
        setData();
    }, [data]);

    const getCurrentState = () => ({
        chartData,
        languages,
        notPassedTotal,
        passedTotal,
        productId,
        studentId
    });

    // Renders the pie chart
    const renderItem = () => {
        return <PieChart redraw data={getCurrentState()} refresh={getCurrentState()} />;
    };

    // Render the chart only if there is any data. If not, do not render anything
    return(
        <div>
            {chartData ?
                <div className="average-chart-row">
                    {renderItem()}
                </div> : null
            }
        </div>
    );
};

export default ChartStudentAverage;