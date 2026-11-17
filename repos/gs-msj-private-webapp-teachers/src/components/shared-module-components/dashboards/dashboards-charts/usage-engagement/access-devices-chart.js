import React from "react";
import { ResponsiveContainer, PieChart, Pie, Cell, Tooltip } from "recharts";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";
import { PulseLoader } from "react-spinners";

const AccessDevicesChart = ({ sessionTotals, loading = false }) => {
  const colors = { mobile: "#0EA5E9", tablet: "#F59E0B", desktop: "#10B981" };
  
  const pieData = [
    { key: "mobile", name: <Translate id="dashboards.usage-and-engagement.device-mobile" />, value: sessionTotals?.AvgMobilePercent ?? 0, color: colors.mobile },
    { key: "tablet", name: <Translate id="dashboards.usage-and-engagement.device-tablet" />, value: sessionTotals?.AvgTabletPercent ?? 0, color: colors.tablet },
    { key: "desktop", name: <Translate id="dashboards.usage-and-engagement.device-desktop" />, value: sessionTotals?.AvgDesktopPercent ?? 0, color: colors.desktop },
  ];

  if (loading) return (
    <div style={{ marginBottom: "30px", marginTop: "70px" }}>
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: 230 }}>
        <PulseLoader size={8} color="#266fc2ff" />
      </div>
    </div>
  );

  return (
    <div style={{ marginBottom: "30px", marginTop: "70px" }}>
      <div className="charts-title">
        <Translate id="dashboards.usage-and-engagement.most-used-devices-title" />
      </div>

      <div style={{ width: "100%", height: 230 }}>
        <ResponsiveContainer width="100%" height="100%">
          <PieChart margin={{ top: 6, right: 8, left: 8, bottom: 6 }}>
            <Pie
              data={pieData}
              dataKey="value"
              nameKey="name"
              cx="50%"
              cy="50%"
              innerRadius={70}
              outerRadius={90}
              startAngle={90}
              endAngle={-270}
              labelLine={false}
              label={({ value }) => `${Math.round(value)}%`}
              isAnimationActive
            >
              {pieData.map((entry, index) => (
                <Cell key={`cell-${index}`} fill={entry.color} />
              ))}
            </Pie>

            <Tooltip
              formatter={(val, name) => [(`${val}%`), name]}
              contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
            />
          </PieChart>
        </ResponsiveContainer>

        <div className="charts-legends-container">
          <span className="legend-container">
            <span className="legend-color-box" style={{ background: colors.mobile }} />
            <Translate id="dashboards.usage-and-engagement.device-mobile" />
          </span>
          <span className="legend-container">
            <span className="legend-color-box" style={{ background: colors.tablet }} />
            <Translate id="dashboards.usage-and-engagement.device-tablet" />
          </span>
          <span className="legend-container">
            <span className="legend-color-box" style={{ background: colors.desktop }} />
            <Translate id="dashboards.usage-and-engagement.device-desktop" />
          </span>
        </div>
      </div>
    </div>
  );
};

export default withLocalize(AccessDevicesChart);
