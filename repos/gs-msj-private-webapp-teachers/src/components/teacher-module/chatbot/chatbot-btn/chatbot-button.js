import React from 'react';
import "./style.css";
import { Bot } from 'lucide-react';
import { useHistory, useLocation } from 'react-router-dom';
import {AI_CHAT_BOT_AVAILABLE} from "../../../../custom-classes/settings";
const ChatbotButton = () => {
  const history = useHistory();
  const location = useLocation();

  const handleButtonClick = () => {
    history.push('/chatbot');
  };

  if (location.pathname === '/chatbot') {
    return null;
  }

  if(!AI_CHAT_BOT_AVAILABLE)
      return null;

  return (
    <div className="chatbot-button-container">
      <button className="chatbot-button" onClick={handleButtonClick}>
        <Bot size={24} />
      </button>
      <span style={{fontWeight: 'bold'}}>Chatbot</span>
    </div>
  );
};

export default ChatbotButton;