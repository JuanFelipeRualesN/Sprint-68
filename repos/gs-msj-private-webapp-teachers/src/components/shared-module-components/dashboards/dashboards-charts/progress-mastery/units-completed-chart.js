import React from "react";
import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, LabelList } from "recharts";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import { PulseLoader } from "react-spinners";

const UnitsCompletedChart = ({ unitMetrics = [], translate, loading = false }) => {
  const chartData = unitMetrics.map(item => ({
    unit: `${translate("dashboards.progress-and-mastery.unit")} ${item.LevelCode}`,
    percentage: item.CompletionRate,
  }));

  // Show a message when there is no data or every completion rate is 0.
  const hasData = unitMetrics.length > 0 && unitMetrics.some(item => item.CompletionRate > 0);

  if (loading) return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
      <PulseLoader size={8} color="#266fc2ff" />
    </div>
  );

  return (
    <>
      <div className="charts-title" style={{ marginBottom: 8 }}>
        <Translate id="dashboards.progress-and-mastery.units-completed" />
      </div>

      {hasData ? (
        <div style={{ width: "100%", height: 280 }} >
          <ResponsiveContainer width="100%" height="100%">
            <BarChart
              data={chartData}
              layout="vertical"
              margin={{ top: 12, right: 48, left: 16, bottom: 12 }}
            >
              <CartesianGrid strokeDasharray="3 3" stroke="rgba(0,0,0,0.08)" />
              <XAxis
                type="number"
                axisLine={false}
                tickLine={false}
                allowDecimals={false}
                domain={[0, 100]}
                tick={{ fontSize: 14 }}
              />
              <YAxis
                type="category"
                dataKey="unit"
                axisLine={false}
                tickLine={false}
                width={90}
                tick={{ fontSize: 14 }}
              />
              <Tooltip
                formatter={(val) => [`${val}%`, ""]}
                labelFormatter={(label) => label}
                contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
                cursor={{ fill: "rgba(0,0,0,0.04)" }}
              />
              <Bar
                dataKey="percentage"
                name={"Units Completed"}
                fill="#17519cff"
                radius={[6, 6, 6, 6]}
                maxBarSize={28}
                minPointSize={4}
                background={{ fill: "#eef2f7", radius: 6 }}
              >
                <LabelList
                  dataKey="percentage"
                  position="right"
                  formatter={(val) => `${val}%`}
                  style={{ fontSize: 13, fill: "#475569", fontWeight: 600 }}
                />
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        </div>
      ) : (
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%', color: '#94a3b8', fontSize: 14 }}>
          <Translate id="dashboards.usage-and-engagement.no-results" />
        </div>
      )}

    </>
  );
};

export default withLocalize(UnitsCompletedChart);
