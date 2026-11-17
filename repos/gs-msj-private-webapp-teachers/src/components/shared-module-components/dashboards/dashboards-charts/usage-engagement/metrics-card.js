import React from "react";
import { withLocalize } from "react-localize-redux";

const MetricsCard = ({title, value, icon, color, badgeColor, unit}) => {
  return (
    <div className="metric-card-container">
      <div style={{ display: "grid", gap: 4 }}>
        <span style={{ fontSize: 12, color: "#6b7280" }}>{title}</span>
        <div style={{ display: "flex", alignItems: "baseline", gap: 6 }}>
          <span style={{ fontSize: 28, fontWeight: 700, lineHeight: 1 }}>
            {value}
          </span>
          <span style={{ fontSize: 12, color: "#6b7280" }}>{unit}</span>
        </div>
      </div>

      <div className="metric-card-icon" style={{ backgroundColor: badgeColor }}
      >
        <div style={{ color }}>{icon}</div>
      </div>
    </div>
  );
};

export default withLocalize(MetricsCard);
