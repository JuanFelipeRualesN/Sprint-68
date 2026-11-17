import React from "react";
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Cell
} from "recharts";
import { Translate } from "react-localize-redux";

const AveragePerSubjectChart = ({ data, onSubjectClick }) => {
    const chartData = data.map(item => ({
        subject: item.SubjectName ?? item.subject,
        average: parseFloat(item.Average ?? item.average),
    }));

    const getColor = (average) => {
        if (average >= 90) return "#66d97f";
        if (average >= 80) return "#94d6a2";
        if (average >= 70) return "#f0eb90";
        if (average >= 60) return "#f0ce90";
        return "#f09390";
    };

    const handleBarClick = (data, index) => {
        if (onSubjectClick && data.subject) {
            onSubjectClick(data.subject);
        }
    };

    // --- New: Custom Tooltip formatter function ---
    const customTooltipFormatter = (value, name, props) => {
        // 'value' is the actual average number (e.g., 85.5)
        // 'name' is the dataKey, which is "average" in this case
        // 'props' contains other data, like payload, color, etc.

        // You can return an array if you want multiple lines in the tooltip for the value.
        // The second element in the array is typically the label.
        return [
            `${value}%`, // Display the value (e.g., 85.5%)
            <Translate id="homeschool.average" options={{ renderInnerHtml: true }} key="average-label" /> // Translated label
        ];
    };

    return (
        <div style={{ width: "100%", height: 300 }}>
            <ResponsiveContainer>
                <BarChart
                    data={chartData}
                    margin={{ top: 20, right: 30, left: 20, bottom: 5 }}
                >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="subject" />
                    <YAxis domain={[0, 100]} />
                    <Tooltip
                        formatter={customTooltipFormatter}
                        // Optional: labelFormatter={customTooltipLabelFormatter}
                    />
                    <Bar dataKey="average" onClick={handleBarClick} barSize={40}>
                        {chartData.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={getColor(entry.average)} />
                        ))}
                    </Bar>
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
};

export default AveragePerSubjectChart;
