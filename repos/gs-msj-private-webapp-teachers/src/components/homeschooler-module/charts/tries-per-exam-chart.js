import React from 'react';
import {
    ResponsiveContainer,
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    CartesianGrid,
    Legend
} from 'recharts';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";

const TriesPerExamChart = ({ attempts, translate }) => {

    const textAttempt = translate('homeschool.attempt');
    const formattedData = attempts.map((item) => ({
        Try: `${textAttempt + ' ' + item.TryNo}`,
        Percent: parseFloat(item.TotalPercent),
    }));

    return (

                <ResponsiveContainer width="100%" height={300}>
                    <LineChart data={formattedData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="Try" />
                        <YAxis domain={[0, 100]} tickFormatter={(val) => `${val}%`} />
                        <Tooltip formatter={(value) => `${value}%`} />
                        <Legend />
                        <Line
                            type="monotone"
                            dataKey="Percent"
                            name={<Translate id="homeschool.total-percent" options={{ renderInnerHtml: true }} />}
                            stroke="#8884d8"
                            activeDot={{ r: 8 }}
                        />
                    </LineChart>
                </ResponsiveContainer>

    );
};

export default withLocalize(TriesPerExamChart);
