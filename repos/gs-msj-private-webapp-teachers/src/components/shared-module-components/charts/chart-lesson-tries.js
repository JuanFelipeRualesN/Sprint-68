/** * Created by portiz on 8/15/2018. */
import React, { useState, useEffect } from 'react';
import BarChart from './chart-bar';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * To manage the charts of lesson tries
 */
const ChartLessonTries = ({ data }) => {
    const [languages, setLanguages] = useState(data.languages);
    const [triesChartData, setTriesChartData] = useState(data.triesChartData);

    // Sets the chart data
    const setData = () => {
        setTriesChartData(triesChartData);
    };

    // Called on the first time to set the data
    useEffect(() => {
        setData();
    }, []);

    // Call when updated
    useEffect(() => {
        setTriesChartData(data.triesChartData);
        setData();
    }, [data]);

    getCurrentState = () => ({
        languages,
        triesChartData
    });

    // Renders the bar chart
    const renderItem = () => {
        return <BarChart redraw data={getCurrentState()} refresh={getCurrentState()}/>;
    };

    // Renders the component
    return (
        <div>
            <h3><Translate id="report-student-average.student-tries"/></h3>
            <div className="tries-chart-row">
                {renderItem()}
            </div>
        </div>
    );
};

export default ChartLessonTries;