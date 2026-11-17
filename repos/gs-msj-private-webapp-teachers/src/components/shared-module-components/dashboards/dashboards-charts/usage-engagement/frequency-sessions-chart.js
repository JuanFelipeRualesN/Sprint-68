import React from "react";
import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip } from "recharts";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";
import { PulseLoader } from "react-spinners";
import { buildSkeleton, tickTranslate, tooltipLabel } from "../chart-skeleton";

// Builds chart data with both minutes and sessions
const buildSessionSkeleton = (granularity, details, period) => {
  const minutesData = buildSkeleton(granularity, details, period, "TotalTimeSeconds", "minutes", "sum");
  const sessionsData = buildSkeleton(granularity, details, period, "TotalSessions", "sessions", "sum");
  return minutesData.map((data, i) => ({
    ...data,
    minutes: toMinutes(data.minutes),
    sessions: sessionsData[i].sessions,
  }));
};

const toMinutes = (seconds) => Math.round((seconds ?? 0) / 60);

// chart component for the usage and sessions frequency chart, showing total minutes and sessions for the selected period and granularity 
const ChartBlock = ({ title, chartData, dataKey, color, yLabel, translate }) => (
  <div style={{ width: "100%", height: 290, marginBottom: 30 }}>
    {title && <div className="charts-title">{title}</div>}
    <ResponsiveContainer width="100%" height="100%">
      <BarChart data={chartData} margin={{ top: 12, right: 16, left: 16, bottom: 12 }} barCategoryGap={12}>
        <CartesianGrid strokeDasharray="3 3" stroke="rgba(0,0,0,0.08)" />
        <XAxis
          dataKey="label"
          axisLine={false}
          tickLine={false}
          tick={{ fontSize: 13 }}
          tickFormatter={tickTranslate(translate)}
        />
        <YAxis
          axisLine={false}
          tickLine={false}
          tick={{ fontSize: 13 }}
          label={{
            value: yLabel,
            angle: -90,
            position: "insideLeft",
            offset: 10,
            style: { textAnchor: "middle", fontSize: "14px" },
          }}
        />
        <Tooltip
          formatter={(value) => [value, yLabel]}
          labelFormatter={tooltipLabel(translate)}
          cursor={{ fill: "rgba(0,0,0,0.04)" }}
          contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
        />
        <Bar 
          dataKey={dataKey}
          fill={color}
          radius={[6, 6, 0, 0]} 
          maxBarSize={36} 
        />
      </BarChart>
    </ResponsiveContainer>
  </div>
);

const FrequencySessionsChart = ({translate, sessionMetrics = [], granularity = "week", period, loading = false }) => {
  const chartData = buildSessionSkeleton(granularity, sessionMetrics, period);
  const isEmpty = chartData.every(d => d.sessions === 0 && d.minutes === 0);

  if (loading) return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 200 }}>
      <PulseLoader size={8} color="#3B82F6" />
    </div>
  );

  if (isEmpty) return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%', color: '#94a3b8', fontSize: 14 }}>
      <Translate id="dashboards.usage-and-engagement.no-results" />
    </div>
  );

  return (
    <div style={{ display: "grid", gap: 16 }}>
      <ChartBlock
        title={<Translate id="dashboards.usage-and-engagement.frequency-usage-title" />}
        chartData={chartData}
        dataKey="minutes"
        color="#3B82F6"
        yLabel={translate("dashboards.usage-and-engagement.usage-time")}
        translate={translate}
      />
      <ChartBlock
        chartData={chartData}
        dataKey="sessions"
        color="#10B981"
        yLabel={translate("dashboards.usage-and-engagement.sessions")}
        translate={translate}
      />
    </div>
  );
};

export default withLocalize(FrequencySessionsChart);
