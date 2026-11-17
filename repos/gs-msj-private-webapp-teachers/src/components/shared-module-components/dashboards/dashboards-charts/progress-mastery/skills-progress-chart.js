import React from "react";
import { ResponsiveContainer, RadarChart, PolarGrid, PolarAngleAxis, PolarRadiusAxis, Radar, Tooltip } from "recharts";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";

const SkillsProgressChart = ({translate, data}) => {

  const skills = data.map(item => ({
    skill: item.skill,
    value: item.value
  }));

  return <div style={{ marginTop: '60px' }}>
    <div className="charts-title" style={{ marginBottom: "16px" }}>
      <Translate id="dashboards.progress-and-mastery.progress-in-skills" />
    </div>

    <div style={{ width: "100%", height: 320 }}>
      <ResponsiveContainer width="100%" height="100%">
        <RadarChart data={skills} cx="50%" cy="50%"  outerRadius={140}>
          <PolarGrid />
          <PolarAngleAxis
            dataKey="skill"
            radius={16}
            tick={({ x, y, payload }) => (
              <g transform={`translate(${x},${y})`}>
                <text dy={6} textAnchor="middle" style={{ fontSize: 14, fill: "#374151" }}>
                  <Translate id={payload?.value} />
                </text>
              </g>
            )}
          />
          <PolarRadiusAxis 
            domain={[0, 100]} 
            tickCount={6} 
            tick={{ fontSize: 13 }} 
          />          
          <Tooltip
            labelFormatter={(value) => translate(value)}
            formatter={(value) => [`${value}`, translate("dashboards.progress-and-mastery.score")]}
            contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
          />
          <Radar dataKey="value" name="Score" stroke="#8B5CF6" fill="#8B5CF6" fillOpacity={0.35} />
        </RadarChart>
      </ResponsiveContainer>
    </div>

  </div>
};

export default withLocalize(SkillsProgressChart);
