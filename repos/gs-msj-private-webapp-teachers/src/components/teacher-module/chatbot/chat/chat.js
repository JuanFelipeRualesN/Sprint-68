import React, { useState, useRef, useEffect } from 'react'
import { Translate, withLocalize } from 'react-localize-redux'
import QuestionInput from '../question-input/question-input'
import { sendMessageToChatbot } from '../../../../api/chatbot'
import Answer from '../answer/answer'
import UserChatMessage from '../user-chat-message/user-chat-message'
import { AnswerLoading } from '../answer/answer-loading'
import { AnswerError } from '../answer/answer-error'
import { FaCheck } from 'react-icons/fa';
import { Bot, RotateCcw } from 'lucide-react';
import { Button } from '@material-ui/core'
import InfoBoxes from './info-boxes/info-boxes'
import CalendarSelector from '../calendar/calendar-selector'
import "./style.css"

const MODULE = "lessons";
const PROFILE = "teachers";

const Chat = ({ translate }) => {
  const [answers, setAnswers] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [conversationId , setConversationId] = useState("");
  const [planningMode, setPlanningMode] = useState(null);

  const [showIntro, setShowIntro] = useState(true);
  const [hidingIntro, setHidingIntro] = useState(false);

  const lastQuestionRef = useRef({ actual: "", display: "" });
  const chatMessageStreamEnd = useRef(null);

  useEffect(() => {
    if (chatMessageStreamEnd.current) {
      chatMessageStreamEnd.current.scrollIntoView({ behavior: "smooth" });
    }
  }, [isLoading, answers]);

  // Function to hide the intro section when a question is asked
  const hideIntro = () => {
    if (!showIntro) return;
    setHidingIntro(true);
    setTimeout(() => setShowIntro(false), 350);
  };

  // Format the date selection message
  const formatDateSelectionMessage = (jsonString) => {
    try {
      const parsed = JSON.parse(jsonString);
      const dates = parsed.selected_dates;
      const count = dates.length;
      
      return {
        count,
        dates
      };
    } catch {
      return null;
    }
  };

  const makeApiRequestGpt = async (question, displayMessage = null) => {
    hideIntro();
    lastQuestionRef.current = {
      actual: question,
      display: displayMessage || question
    };
    setError(null);
    setIsLoading(true);

    try {
      const history = answers.map(a => ({ user: a[0], bot: a[1].answer }));

      const module = planningMode ? "plannings" : MODULE;
      const detailedPlanning = planningMode === 'detailed';

      const request = {
        history: [...history, { user: question, bot: undefined }],
        conversationId,
        query: question,
        module: module,
        profile: PROFILE,
        detailedPlanning: detailedPlanning
      };
    
      const response = await sendMessageToChatbot(request);
      const result = response.data;

      setConversationId(result.conversation_id || "");

      const newAnswer = {
        answer: result.answer,
        thoughts: result.thoughts || null,
        data_points: result.data_points || [],
        conversation_id: result.conversation_id || "",
        show_calendar: result.show_calendar || false
      };

      setAnswers(prev => [...prev, [displayMessage || question, newAnswer]]);

    } catch (error) {
      console.error("Error calling the API:", error);
      setError(error);
    } finally {
      setIsLoading(false);
    }
  };

  // Clear the chat state
  const clearChat = () => {
    lastQuestionRef.current = { actual: "", display: "" };
    setError(null);
    setAnswers([]);
    setConversationId("");
  };

  // Reset the conversation and show the intro section again
  const handleResetConversation = () => {
    clearChat();
    setPlanningMode(null);
    setShowIntro(true);
    setHidingIntro(false);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const togglePlanningMode = (mode) => {
    setPlanningMode(prevMode => prevMode === mode ? null : mode);
  };

  const handleDatesSelected = async (datesJsonString) => {
    const dateInfo = formatDateSelectionMessage(datesJsonString);
    const displayMessage = `${dateInfo.count} fecha${dateInfo.count !== 1 ? 's' : ''} seleccionada${dateInfo.count !== 1 ? 's' : ''}`;
    
    await makeApiRequestGpt(datesJsonString, displayMessage);
  };
  

  return (
    <div className="chat-content-box">
      <div className="chat-actions">
        <span>
          <Button
            variant="outlined"
            size="small"
            onClick={handleResetConversation}
            disabled={isLoading}
            startIcon={<RotateCcw size={16} />}
            className="chat-restart-button"
          >
            <Translate id="chatbot.restart-conversation" />
          </Button>
        </span>
      </div>
      {showIntro && (
        <div className={`chat-intro ${hidingIntro ? "chat-intro-hide" : ""}`}>
          <Bot size={64} color='#66be4e' style={{minHeight: '50px'}}/>
          <div className="chatbot-title">
            <Translate id="chatbot.welcome" options={{ renderInnerHtml: true }}/>
          </div>
          <br />
          <div className="chat-description">
            <Translate id="chatbot.description" options={{ renderInnerHtml: true }}/>
          </div>
          <br />
          {window.innerWidth > 400 ? <InfoBoxes /> :
            <>
              <div style={{fontSize: '13px', marginBottom: '8px', display: 'flex', alignItems: 'center'}}>
                <FaCheck size={16} color="#65b95c" style={{marginRight: "8px"}}/>
                <Translate id="chatbot.search-lessons" options={{ renderInnerHtml: true }}/>
              </div>
              <div style={{fontSize: '13px', marginBottom: '8px', display: 'flex', alignItems: 'center'}}>
                <FaCheck size={16} color="#65b95c" style={{marginRight: "8px"}}/>
                <Translate id="chatbot.study-plan" options={{ renderInnerHtml: true }}/>
              </div>
              <div style={{fontSize: '13px', display: 'flex', alignItems: 'center'}}>
                <FaCheck size={16} color="#65b95c" style={{marginRight: "8px"}}/>
                <Translate id="chatbot.lessons-info" options={{ renderInnerHtml: true }}/>
              </div>
            </>
          }
        </div>
      )}
      <div className="chat-container">
        {!lastQuestionRef.current.actual ? (
          <div className="chat-empty-state"></div>
        ) : (
        <div className="chat-message-stream">
          {answers.map((answer, index) => (
            <div key={index}>
              <UserChatMessage message={answer[0]} />
              <div className="chat-message-gpt">
                <Answer
                  key={index}
                  answer={answer[1]}
                />
                {answer[1].show_calendar && (
                  <CalendarSelector onDatesSelected={handleDatesSelected} />
                )}
              </div>
            </div>
          ))}
          {isLoading && (
            <>
              <UserChatMessage message={lastQuestionRef.current.display} />
              <div className="chat-message-gpt-min-width ">
                  <AnswerLoading />
              </div>
            </>
          )}
          {error ? (
            <>
              <UserChatMessage message={lastQuestionRef.current.display} />
              <div className="chat-message-gpt-min-width">
                <AnswerError
                  error={error.toString()} 
                  onRetry={() => makeApiRequestGpt(lastQuestionRef.current.actual)} 
                />
              </div>
            </>
          ) : null}
            <div ref={chatMessageStreamEnd} />
        </div>
        )}
        <div className="chat-input">
          <QuestionInput 
            clearOnSend 
            placeholder={translate("chatbot.ask-question")} 
            disabled={isLoading} 
            onSend={question => makeApiRequestGpt(question)} 
            togglePlanningMode={togglePlanningMode}
            planningMode={planningMode}
          />
        </div>

      </div>
    </div>
  )
}

export default withLocalize(Chat)