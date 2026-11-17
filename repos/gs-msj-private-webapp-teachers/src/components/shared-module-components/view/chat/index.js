import React, { useState, useEffect, useRef } from 'react';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import LanguageManager from "../../../../custom-classes/language-manager";
import SessionManager from "../../../../custom-classes/session-manager";
import "./style.css";
import { getUsersContacts } from "../../../../api/users";
import Message from "./message";
import { convertMaxLengthSting } from "../../../../utils/utils";
import { SendAccessLog } from '../../../../utils/access_logger';
import { getAsset } from '../../../../utils/getAssets';
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';

const IconAvatar = getAsset("Common-icon-Avatar");
const ChatBackground = getAsset("Chat-icon-Background");

const Chat = ({ chatSelected, usernames, chats, setChatSelected, handleSendMessage, handleCreateChat}) => {
  const sessionDetails = SessionManager.getSessionDetails();
  const [message, setMessage] = useState("");
  const [tabActive, setTabActive] = useState("contacts");
  const [contacts, setContacts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(120);

  const messagesEndRef = useRef();
  
  // Initialize translations based on the current version
  useVersionedTranslations();
  useEffect(() => {
    try{            
      SendAccessLog({
        Username: sessionDetails.Username,
        InvitationCode: sessionDetails.InvitationCode,
        Role: sessionDetails.Roles[0],
        AreaId: "9"
      })
    }
    catch (error){
      // empty catch block to avoid breaking the application
    }
  }, []);

  useEffect(() => {
    getContactsChat();
  }, []);

  useEffect(() => {
    if (chatSelected) {
      scrollToBottom();
    }
  }, [chatSelected]);

  const getContactsChat = async () => {
    setLoading(true);
    try {
      const res = await getUsersContacts(page, pageSize);

      setContacts(res.data.UserInfo);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const scrollToBottom = () => {
    messagesEndRef.current.scrollIntoView({ behavior: "smooth" });
  };

  const sendMessage = (e) => {
    e.preventDefault();
    if (message) {
      handleSendMessage(message);
      setMessage("");
    }
  };

  const startConversation = (contact) => {
    handleCreateChat(contact.Username);
    setTabActive("messages");
  };

  const createChatName = (nickname) => {
    const contact = contacts.find(
      (elem) => elem.Username === nickname
    );
    return contact ? contact.GivenName + " " + contact.Surname : "";
  };

  const onChangeMessage = (e) => {
    const value = e.currentTarget.value;
    const newValue = convertMaxLengthSting(value, 300);
    setMessage(newValue);
  };

  const username = sessionDetails.Username;
  const name = sessionDetails.Name;
  
  return (
    <div>
      <div className="container-padding animated delay-05s fadeIn">
        <div className="wrapper-principal-container">
          <div className="wrapper-chat">
            {/* <h2>Chat. Bienvenido/a, {name} </h2> */}

            <div className="wrapper-col-chat">
              <div className="col-chat__contacts">
                <div className="chat__headerTitle">
                  {tabActive === "contacts" && (
                    <div className="chat__headerTitle--title">
                      <Translate id="chat.contacts" options={{ renderInnerHtml: false }} />
                    </div>
                  )}
                  {tabActive === "messages" && (
                    <div className="chat__headerTitle--title"> <Translate id="chat.messages" options={{ renderInnerHtml: false }} /> </div>
                  )}
                </div>

                <div className="chat__center-content">
                  {tabActive === "contacts" && typeof contacts !== 'undefined' && contacts.length > 0 && (
                    <div className="chat__center-content__wrapper-mod-contacts">
                      {contacts.map((item, i) => (
                        <div
                          key={i}
                          className="chat__center-content__article"
                          onClick={() => startConversation(item)}
                        >
                          <div className="content__article__avatar">
                            <img
                              className="content__article__avatar--img"
                              src={IconAvatar}
                            />
                          </div>
                          <div className="content__article__user">
                            <div className="content__article__user__name">
                              {item.GivenName} {item.Surname}
                            </div>
                            {usernames.includes(item.Username) ? (
                              <p className="content__article__user__status">
                                <span className="content__article__user__status--ON">
                                  <i className="fas fa-circle" />
                                </span>
                                <Translate id="chat.online" options={{ renderInnerHtml: false }} />
                              </p>
                            ) : (
                              <p className="content__article__user__status">
                                <span className="content__article__user__status--OFF">
                                  <i className="fas fa-circle" />
                                </span>
                                <Translate id="chat.disconnected" options={{ renderInnerHtml: false }} />
                              </p>
                            )}
                          </div>
                        </div>
                      ))}
                    </div>
                  )}
                  {tabActive === "messages" && (
                    <div className="chat__center-content__wrapper-mod-contacts">
                      {chats.map((item, i) => (
                        <div
                          key={i}
                          className="chat__center-content__article"
                          onClick={() => setChatSelected(item)}
                        >
                          <div className="content__article__avatar">
                            <img
                              className="content__article__avatar--img"
                              src={IconAvatar}
                            />
                          </div>
                          <div className="content__article__user">
                            <div
                              className={
                                chatSelected && item._id === chatSelected._id
                                  ? "content__article__user__name active"
                                  : "content__article__user__name"
                              }
                            >
                              {createChatName(item.name)}
                            </div>
                            {item.online && (
                              <p className="content__article__user__status">
                                <span className="content__article__user__status--ON">
                                  <i className="fas fa-circle" />
                                </span>
                                <Translate id="chat.online" options={{ renderInnerHtml: false }} />
                              </p>
                            )}
                            {!item.online && (
                              <p className="content__article__user__status">
                                <span className="content__article__user__status--OFF">
                                  <i className="fas fa-circle" />
                                </span>
                                <Translate id="chat.disconnected" options={{ renderInnerHtml: false }} />
                              </p>
                            )}
                          </div>

                          {!item.read[username] && (
                            <div className="content__article__user__pointer">
                              <i className="fas fa-circle" />
                            </div>
                          )}
                        </div>
                      ))}
                    </div>
                  )}
                </div>

                <ul className="col-chat__contacts__menu">
                  <li
                    className={
                      tabActive === "contacts"
                        ? "contacts__menu--item active"
                        : "contacts__menu--item"
                    }
                  >
                    <a
                      onClick={() => {
                        setChatSelected(null);
                        setTabActive("contacts");
                      }}
                    >
                      <p className="contacts__menu--item__icon">
                        <i className="fas fa-user" />
                      </p>
                      <p className="contacts__menu--item__txt"><Translate id="chat.contacts" options={{ renderInnerHtml: false }} /></p>
                    </a>
                  </li>

                  <li
                    className={
                      tabActive === "messages"
                        ? "contacts__menu--item active"
                        : "contacts__menu--item"
                    }
                  >
                    <a
                      onClick={() =>
                        setTabActive("messages")
                      }
                    >
                      <p className="contacts__menu--item__icon">
                        <i className="fas fa-comments" />
                      </p>
                      <p className="contacts__menu--item__txt"><Translate id="chat.messages" options={{ renderInnerHtml: false }} /></p>
                    </a>
                  </li>
                </ul>
              </div>

              <div className="col-chat__messages" style={{ backgroundImage: `url(${ChatBackground})` }}>
                {chatSelected && (
                  <div className="chat__headerTitle">
                    <div className="chat__center-content__article">
                      <div className="content__article__avatar">
                        <img
                          className="content__article__avatar--img"
                          src={IconAvatar}
                        />
                      </div>
                      <div className="content__article__user">
                        <div className="content__article__user__name">
                          {createChatName(chatSelected.name)}
                        </div>
                        {chatSelected.online && (
                          <p className="content__article__user__status">
                            <span className="content__article__user__status--ON">
                              <i className="fas fa-circle" />
                            </span>
                            <Translate id="chat.online" options={{ renderInnerHtml: false }} />
                          </p>
                        )}
                        {!chatSelected.online && (
                          <p className="content__article__user__status">
                            <span className="content__article__user__status--OFF">
                              <i className="fas fa-circle" />
                            </span>
                            <Translate id="chat.disconnected" options={{ renderInnerHtml: false }} />
                          </p>
                        )}
                      </div>
                    </div>
                  </div>
                )}

                {chatSelected && (
                  <div className="chat__center-content">
                    <div className="chat__center-content__wrapper-mod-messages">
                      {chatSelected.messages.map((message, i) => (
                        <div key={i}>
                          <Message message={message} username={username} />
                        </div>
                      ))}
                      <div ref={messagesEndRef} />
                    </div>

                    <form onSubmit={sendMessage} className="col-chat__messages__form">
                      <input
                        type="text"
                        name="user"
                        placeholder={LanguageManager.getLanguage() === "en" ? "Write messages" : "Escribir mensajes"}
                        value={message}
                        onChange={onChangeMessage}
                        onKeyPress={(e) =>
                          e.key === "Enter" ? sendMessage(e) : null
                        }
                      />
                      <button className="form__button--send">
                        <i className="fas fa-paper-plane" />
                      </button>
                    </form>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default withLocalize(Chat);
