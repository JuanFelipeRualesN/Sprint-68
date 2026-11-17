import React from "react";
import "./style.css";

const Message = ({ message, username }) => {
  return message.sender === username ? (
    <div className="messageContainer justifyStart">
      <div className="messageBox backgroundLight">
        <p className="messageText colorDark">
          {message.message}
        </p>
      </div>
      <p className="sentText pl-10">{username}</p>
    </div>
  ) : (
    <div className="messageContainer justifyEnd">
      <p className="sentText pr-10">{message.sender}</p>
      <div className="messageBox backgroundBlue">
        <p className="messageText colorWhite">
          {message.message}
        </p>
      </div>
    </div>
  );
};

export default Message;
