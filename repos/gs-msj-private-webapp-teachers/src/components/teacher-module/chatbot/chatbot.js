import React from 'react'
import Chat from './chat/chat';
import "./style.css"
import {AI_CHAT_BOT_AVAILABLE} from "../../../custom-classes/settings";

const Chatbot = () => {
  return (
    <>
        {AI_CHAT_BOT_AVAILABLE?
      <div className="container-padding-chatbot animated delay-05s fadeIn">
        <div className="chatbot-container">
          <Chat />
        </div>
      </div> : null}
    </>
  )
}

export default Chatbot