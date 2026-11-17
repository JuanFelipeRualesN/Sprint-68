import React from "react";
import { Paper, Typography } from "@material-ui/core";
import { Translate } from "react-localize-redux";
import { FaSearch, FaBookOpen, FaClipboardList, FaLightbulb } from "react-icons/fa";
import "./style.css";

const InfoBoxes = () => {
  const items = [
    { id: "chatbot.search-lessons-by-standards", color: "linear-gradient(135deg, #FFD65A, #FFC247)", icon: <FaSearch /> },
    { id: "chatbot.search-lessons-by-expectations", color: "linear-gradient(135deg, #6CC1FF, #4E9EFF)", icon: <FaBookOpen /> },
    { id: "chatbot.study-plan", color: "linear-gradient(135deg, #6DD58C, #4CB46E)", icon: <FaClipboardList  /> },
    { id: "chatbot.lessons-info", color: "linear-gradient(135deg, #FF8AB8, #FF6A95)", icon: <FaLightbulb /> },
  ];

  return (
    <div className="info-box-container">
      <div className="info-box-grid">
        {items.map((item, index) => (
          <Paper key={index} className="info-box" elevation={0.1}>
            <div className="info-box-icon-wrapper" style={{ background: item.color }}>
              {item.icon}
            </div>
            <Typography className="info-box-text">
              <Translate id={item.id} options={{ renderInnerHtml: true }} />
            </Typography>
          </Paper>
        ))}
      </div>
    </div>
  );
}

export default InfoBoxes;
