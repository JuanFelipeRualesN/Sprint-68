import React from "react";
import { ResponsiveContainer, PieChart, Pie, Cell, Tooltip } from "recharts";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";
import { PulseLoader } from "react-spinners";

const InOffSchoolChart = ({ sessionTotals, loading = false }) => {
  const chartColors = { inSchool: "#10B981", offSchool: "#8B5CF6" };

  const chartData = [
    { name: "In-school", value: sessionTotals?.AvgOnSchoolPercent  ?? 0, color: chartColors.inSchool },
    { name: "Off-school", value: sessionTotals?.AvgOffSchoolPercent ?? 0, color: chartColors.offSchool },
  ];

  if (loading) return (
    <div style={{ marginBottom: '50px' }}>
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 230 }}>
        <PulseLoader size={8} color="#266fc2ff" />
      </div>
    </div>
  );

  return (
    <div style={{marginBottom: '50px'}}>
      <div className="charts-title">
        <Translate id="dashboards.usage-and-engagement.off-in-school-title" />
      </div>
      <div style={{ width: "100%", height: 230 }}>
        <ResponsiveContainer width="100%" height="100%">
          <PieChart > 
            <Pie
              data={chartData}
              dataKey="value"
              nameKey="name"
              startAngle={90}
              endAngle={-270}
              isAnimationActive
            >
              {chartData.map((entry, i) => (
                <Cell key={i} fill={entry.color} />
              ))}
            </Pie>
            <Tooltip
              formatter={(value) => [`${value}%`]}
              contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
            />
          </PieChart>
          <div className="charts-legends-container">
            <span className="legend-container">
              <span className="legend-color-box" style={{ background: chartColors.inSchool }} />
              <Translate id="dashboards.usage-and-engagement.in-school-hours" />
            </span>
            <span className="legend-container">
              <span className="legend-color-box" style={{ background: chartColors.offSchool }} />
              <Translate id="dashboards.usage-and-engagement.off-school-hours" />
            </span>
          </div>
        </ResponsiveContainer>
      </div>
    </div>
  );
}

export default withLocalize(InOffSchoolChart)