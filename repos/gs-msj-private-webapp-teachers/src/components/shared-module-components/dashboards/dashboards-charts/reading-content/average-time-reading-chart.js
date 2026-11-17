import React from "react";
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip } from "recharts";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";
import { PulseLoader } from "react-spinners";
import { buildAvgTimeSkeleton } from "./content-skeleton";
import { tickTranslate, tooltipLabel } from "../chart-skeleton";

const AverageTimeReadingChart = ({ translate, contentMetrics, granularity = "week", selectedPeriod, loading = false }) => {
  const avgTimeByPeriod = contentMetrics?.AvgTimeByPeriod ?? [];
  const chartData = buildAvgTimeSkeleton(granularity, avgTimeByPeriod, selectedPeriod);

  // Show a message when there is no data or every value is 0.
  const hasData = avgTimeByPeriod.length > 0 && avgTimeByPeriod.some(item => item.AvgTimeSeconds > 0);

  if (loading) return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
      <PulseLoader size={8} color="#266fc2ff" />
    </div>
  );

  return (
    <>
      <div className="charts-title" style={{ marginBottom: 8 }}>
         <Translate id="dashboards.progress-and-mastery.average-time-per-reading" />
      </div>

      {hasData ? (
        <div style={{ width: "100%", height: 280 }} >
          <ResponsiveContainer width="100%" height="100%">
            <LineChart
              data={chartData}
              margin={{ top: 12, right: 16, left: 16, bottom: 12 }}
            >
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
                  value: translate("dashboards.progress-and-mastery.average-time"),
                  angle: -90,
                  position: "insideLeft",
                  offset: 10,
                  style: { textAnchor: "middle", fontSize: "15px" },
                }}
              />
              <Tooltip
                labelFormatter={tooltipLabel(translate)}
                formatter={(value) => [value, translate("dashboards.progress-and-mastery.average-time")]}
                cursor={{ stroke: "rgba(0,0,0,0.15)" }}
                contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
              />
              <Line
                dataKey="avgTime"
                stroke="#266fc2ff"
                strokeWidth={3}
                dot={{ r: 3 }}
                activeDot={{ r: 5 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
      ) : (
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280, color: '#94a3b8', fontSize: 14 }}>
          <Translate id="dashboards.usage-and-engagement.no-results" />
        </div>
      )}
    </>
  );
};

export default withLocalize(AverageTimeReadingChart);
