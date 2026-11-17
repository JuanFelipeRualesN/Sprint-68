/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import { Pie } from 'react-chartjs';
/**
 * Generic component for pie charts
 */
const PieChart = ({ data, sizeChart }) => {
    const [chartSize, setChartSize] = useState(sizeChart ?? "250");

    useEffect(() => {
        setChartSize(typeof sizeChart !== 'undefined' ? sizeChart : "250");
    }, [sizeChart]);

    // Specify the chart you want to try
    let PieChartComp = Pie;

    // The options
    let options = {
        segmentShowStroke : true,

        //String - The colour of each segment stroke
        segmentStrokeColor : "#fff",

        //Number - The width of each segment stroke
        segmentStrokeWidth : 2,

        //Number - The percentage of the chart that we cut out of the middle
        percentageInnerCutout : 25, // This is 0 for Pie charts

        //Number - Amount of animation steps
        animationSteps : 100,

        //String - Animation easing effect
        animationEasing : "easeOutBounce",

        //Boolean - Whether we animate the rotation of the Doughnut
        animateRotate : true,

        //Boolean - Whether responsive or not
        responsive: true
    };

    // Renders the component
    return (
        <PieChartComp redraw data={data.chartData} options={options} width="600" height={chartSize}/>
    );
};

export default PieChart;