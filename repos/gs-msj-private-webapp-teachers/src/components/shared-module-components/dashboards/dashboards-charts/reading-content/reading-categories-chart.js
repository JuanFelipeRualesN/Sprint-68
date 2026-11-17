import React from "react";
import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip } from "recharts";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import { PulseLoader } from "react-spinners";

const ReadingCategoriesChart = ({ translate, categories = [], loading = false }) => {
  const chartData = categories.map(category => {
    const key = `dashboards.reading-and-content.categories.${category.CategoryName}`;
    const translated = translate(key);
    const categoryLabel = translated.startsWith("Missing") ? category.CategoryName : translated;
    return { topic: categoryLabel, completed: category.TotalReadings };
  });

  // Show a message when there is no data or every value is 0.
  const hasData = categories.length > 0 && categories.some(category => category.TotalReadings > 0);

  if (loading) return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
      <PulseLoader size={8} color="#266fc2ff" />
    </div>
  );

  return (
    <>
      <div className="charts-title" style={{ marginBottom: 8 }}>
        <Translate id="dashboards.reading-and-content.reading-categories" />
      </div>

      {hasData ? (
        <div style={{ width: "100%", height: 280 }}>
          <ResponsiveContainer width="100%" height="100%">
            <BarChart
              data={chartData}
              layout="vertical"
              margin={{ top: 12, right: 16, left: 16, bottom: 12 }}
              barCategoryGap={4}
            >
              <CartesianGrid strokeDasharray="3 3" stroke="rgba(0,0,0,0.1)" />
              <XAxis
                type="number"
                axisLine={false}
                tickLine={false}
                allowDecimals={false}
                tick={{ fontSize: 14 }}
              />
              <YAxis
                type="category"
                dataKey="topic"
                axisLine={false}
                tickLine={false}
                width={90}
                tick={{ fontSize: 14 }}
              />
              <Tooltip
                formatter={(val) => [`${val}`, ""]}
                labelFormatter={(label) => label}
                contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
                cursor={{ fill: "rgba(0,0,0,0.04)" }}
              />
              <Bar
                dataKey="completed"
                name={"Topics"}
                fill="#c342cfff"
                radius={[6, 6, 6, 6]}
                maxBarSize={28}
                minPointSize={8}
              />
            </BarChart>
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

export default withLocalize(ReadingCategoriesChart);
