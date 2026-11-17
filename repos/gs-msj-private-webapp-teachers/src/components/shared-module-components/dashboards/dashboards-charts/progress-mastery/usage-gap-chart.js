import React from "react";
import { Translate } from "react-localize-redux";
import { withLocalize } from "react-localize-redux";

const UsageGapsChart = ({}) => {

  const mockedData = [
    { school: "School A", usageGap: "80%" },
    { school: "School B", usageGap: "65%" },
    { school: "School C", usageGap: "70%" },
  ]

  return (
    <div style={{paddingRight: "25px", paddingLeft: "25px", marginTop: "40px"}} >
      <div className="charts-title" >
         <Translate id="dashboards.progress-and-mastery.usage-gap" />
      </div>
      <div>
        {mockedData.map((item, index) => (
          <div key={index} style={{ display: "flex", justifyContent: "space-between", padding: "8px 0", fontSize: "14px", color: "#7A7A7A"}}>
            <span>{item.school}</span>
            <span>{item.usageGap}</span>
          </div>
        ))}
        <div style={{ borderBottom: "1px solid #eee"}}>
          <div style={{ display: "flex", justifyContent: "space-between", padding: "8px 0", fontSize: "14px", color: "#266fc2ff", fontWeight: "600"}}>
            <span>
              <Translate id="dashboards.progress-and-mastery.average" />
            </span>
            <span>72%</span>
          </div>
        </div>
        <div style={{ marginTop: "15px", fontSize: "14px", color: "#7A7A7A"}}>
            <span style={{color: "red", fontWeight: "600"}}>5% {" "}</span> 
            <Translate id="dashboards.progress-and-mastery.group-usage" />
        </div>
      </div>
    </div>
  );
};

export default withLocalize(UsageGapsChart);
