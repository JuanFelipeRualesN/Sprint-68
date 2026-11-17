/**
 * Created by portiz on 8/15/2018.
 */
import React from 'react';

/**
 * The generic barchart. Requires react-chartjs
 */
const BarChart = ({ data }) => {
    // Specify the chart you want to try
    let BarChartComp = require("react-chartjs").Bar;

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
        <BarChartComp redraw data={data.triesChartData} options={options} width={600} height={250}/>
    );
};

export default BarChart;