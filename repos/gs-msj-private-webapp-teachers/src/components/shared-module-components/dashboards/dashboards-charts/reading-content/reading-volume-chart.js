import React from "react";
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from "recharts";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";
import { buildContentSkeleton } from "./content-skeleton";
import { tickTranslate, tooltipLabel } from "../chart-skeleton";

const ReadingVolumeChart = ({ translate, contentMetrics, granularity = "week", selectedPeriod, loading = false }) => {
  const pagesByPeriod = contentMetrics?.PagesByPeriod ?? [];
  const chartData = buildContentSkeleton(granularity, pagesByPeriod, selectedPeriod);

  // Show a message when there is no data or every value is 0.
  const hasData = pagesByPeriod.length > 0 && pagesByPeriod.some(item => item.TotalPages > 0);

  if (loading) return (
    <div style={{ height: 300 }}/>
  );

  return (
    <div style={{ marginTop: "50px" }}>
      <div className="charts-title" >
         <Translate id="dashboards.reading-and-content.reading-volume" />
      </div>

      {hasData ? (
        <div style={{ width: "100%", height: 300 }}>
          <ResponsiveContainer>
            <AreaChart
              data={chartData}
              margin={{ top: 12, right: 16, left: 16, bottom: 12 }}
            >
              <CartesianGrid strokeDasharray="3 3" stroke="#e0e0e0" />
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
                  value: translate("dashboards.reading-and-content.pages"),
                  angle: -90,
                  position: "insideLeft",
                  offset: 10,
                  style: { textAnchor: "middle", fontSize: "15px" },
                }}
              />
              <Tooltip
                formatter={(v) => [v, translate("dashboards.reading-and-content.pages-read")]}
                labelFormatter={tooltipLabel(translate)}
                cursor={{ stroke: "rgba(0,0,0,0.15)" }}
                contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
              />
              <Area
                type="monotone"
                dataKey="pages"
                stroke="#6bc4ff"
                fill="#6bc4ff"
                fillOpacity={0.3}
                dot={{ r: 3 }}
                activeDot={{ r: 5 }}
              />
            </AreaChart>
          </ResponsiveContainer>
        </div>
      ) : (
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 300, color: '#94a3b8', fontSize: 14 }}>
          <Translate id="dashboards.usage-and-engagement.no-results" />
        </div>
      )}
    </div>
  );
};

export default withLocalize(ReadingVolumeChart);
