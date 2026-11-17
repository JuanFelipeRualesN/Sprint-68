import React, { useState } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Row, Col} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import { Translate } from "react-localize-redux";
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import "moment-timezone";
import ImgAvanza from "../../../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Cell } from "recharts";

const CustomTooltip = ({ active, payload }) => {
    if (active && payload && payload.length) {
        return (
            <div style={{
                backgroundColor: "#fff",
                padding: "8px",
                border: "1px solid #ccc",
                borderRadius: "5px",
                fontSize: "14px",
                textAlign: "center"
            }}>
                <p style={{ margin: 0, fontWeight: "bold" }}>{payload[0].payload.SubjectName}</p>
                <p style={{ margin: 0 }}><Translate id="learning-gap.current-average"/>: {payload[0].value}</p>
            </div>
        );
    }
    return null;
};

/**
 * To handle the diagnostic score area.
 * @param translate The translation function.
 * @returns {Element} The diagnostic score area.
 * @constructor
 */
const LearningGapCurrentProgressArea = ({ overallScore }) => {
    const [smallLoading, setSmallLoading] = useState(false);

    // Define colors per subject
    const colorMapping = {
        "Matemáticas": "#4EC494",
        "Español": "#4662E5",
        "History": "#cf2b2b",
        "English": "#A34787",
        "Art": "#9133ca",
        "Geography": "#ff5733"
    };

    const defaultColor = "#4A90E2"; // Default color for subjects not in the list

    // Add color property to each subject dynamically
    const chartData = overallScore.map(score => ({
        ...score,
        fill: colorMapping[score.SubjectName] || defaultColor
    }));

    const allScoresNull = overallScore.every(score => score.ActualScore === null);

    return (
        <Row>
            {allScoresNull ? (
                <></>
            ) : (
                <Col xs={12} style={{ marginTop: "20px", textAlign: "center" }}>
                    <span className="learning-gap-subtitle">
                        <Translate id="learning-gap.current-avanza-progress-per-subject" />
                    </span>
                    <ResponsiveContainer width="100%" height={300}>
                        <BarChart data={chartData} margin={{ top: 20, right: 30, left: 20, bottom: 5 }} barSize={80}>
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="SubjectName" />
                            <YAxis />
                            <Tooltip content={<CustomTooltip />} />
                            <Bar dataKey="ActualScore" radius={[5, 5, 0, 0]}>
                                {chartData.map((entry, index) => (
                                    <Cell key={`cell-${index}`} fill={entry.fill} />
                                ))}
                            </Bar>
                        </BarChart>
                    </ResponsiveContainer>
                </Col>
            )}
        </Row>
    );
};


export default withLocalize(LearningGapCurrentProgressArea);
