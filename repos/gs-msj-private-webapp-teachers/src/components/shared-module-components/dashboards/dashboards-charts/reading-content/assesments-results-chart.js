import React from "react";
import {ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip} from "recharts";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";

const AssesmentsResultsChart = ({translate}) => {

  const mockedData = [
    { level: "Inicial", value: 40 },
    { level: "Intermedia", value: 78 },
    { level: "Final", value: 70 },
  ]
  
  return (
  <div style={{ marginTop: "50px" }}>
    <div className="charts-title" style={{ marginBottom: 8 }}>
      <Translate id="dashboards.reading-and-content.assessments-results" />
    </div>

    <div style={{ width: "100%", height: 320 }}>
      <ResponsiveContainer width="100%" height="100%">
        <BarChart
          data={mockedData}
          margin={{ top: 12, right: 16, bottom: 8, left: 8 }}
          barSize={42}
        >
          <CartesianGrid strokeDasharray="3 3" stroke="rgba(0,0,0,0.1)" />
          <XAxis 
            dataKey="level" 
            tick={{ fontSize: 14 }}
            axisLine={false}
            tickLine={false}
          />
          <YAxis
            type="number"
            domain={[0, 100]}
            ticks={[0, 20, 40, 60, 80, 100]}
            tick={{ fontSize: 14 }}
            axisLine={false}
            tickLine={false}
          />
          <Tooltip
            formatter={(v) => [`${v}`, translate("dashboards.reading-and-content.score")]}
            labelFormatter={(label) => translate("dashboards.reading-and-content.evaluation") + `: ${label}`}
            cursor={{ fill: "rgba(0,0,0,0.04)" }}
          />
          <Bar dataKey="value" name="Score" radius={[6, 6, 6, 6]}  fill="#4c7edaff"/>
        </BarChart>
      </ResponsiveContainer>
    </div>
  </div>
  );
};

export default withLocalize(AssesmentsResultsChart);
