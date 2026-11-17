/**
 * Created by portiz on 4/12/19.
 */
import React from 'react';

/**
 * Generic component for pie charts
 */
const PieChart = ({ chartData }) => {
    // Specify the chart you want to try
    let PieChartComp = require("react-chartjs").Pie;

    // The options
    let options = {
        segmentShowStroke : true,

        //String - The colour of each segment stroke
        segmentStrokeColor : "#fff",

        //Number - The width of each segment stroke
        segmentStrokeWidth : 2,

        //Number - The percentage of the chart that we cut out of the middle
        percentageInnerCutout : 0, // This is 0 for Pie charts

        //Number - Amount of animation steps
        animationSteps : 100,

        //String - Animation easing effect
        animationEasing : "easeOutSine",

        //Boolean - Whether we animate the rotation of the Doughnut
        animateRotate : true,

        //Boolean - Whether responsive or not
        responsive: true
    };

    // Renders the component
    return (
        <PieChartComp redraw data={chartData} options={options} width="600" height="250"/>
    );
};

export default PieChart;