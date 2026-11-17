import React from 'react'
import "./style.css"

const UserChatMessage = ({message}) => {
  return (
    <div className="user-chat-message-container">
        <div className="user-chat-message">{message}</div>
    </div>
  )
}

export default UserChatMessage