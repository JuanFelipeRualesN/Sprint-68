import { useState, useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { generateStreamedResponse } from '../ai-assistant';
import { SYS_ASSIGNMENT, getSysPlanning, CHIPS_ASSIGNMENT, CHIPS_PLANNING, QUESTION_TYPE_LABELS, OPTION_LETTERS, buildAssignmentUserMessage } from '../ai-panel-config';
import '../style.css';

// ─────────────────────────────────────────────────────────────────────────────
//  AI PANEL
// ─────────────────────────────────────────────────────────────────────────────
// Chat-style panel that talks to the AI in two modes: "assignment" (quiz creator,
// generates title/instructions/questions) or "planning" (lesson-plan section text).
// mode: "assignment" | "planning"
// sectionLabel: current lesson-plan section, only used in planning mode
// onFill: called with the parsed AI JSON when the user clicks "use this"
// onActivity: called on user input, used by the parent to postpone autosave
// getFormContext: returns the quiz currently in the form ({title, instructions, questions})
//   or null when it's empty. Used to send "edition" vs "creation" context to the AI.
export default function AIPanel({ mode, sectionLabel, onFill, onActivity, getFormContext }) {
  const isAssignment = mode === "assignment";
  const { t } = useTranslation('quiz');
  const [messages, setMessages] = useState([]);
  const [input,      setInput]      = useState("");
  const [loading,    setLoading]    = useState(false);
  const [canFill,    setCanFill]    = useState(false); // true once the AI reply is valid JSON, ready to apply
  const [lastParsed, setLastParsed] = useState(null);  // last parsed AI JSON, passed to onFill
  const chatRef = useRef();

  // Auto-scroll the chat to the latest message
  useEffect(() => {
    if (chatRef.current) chatRef.current.scrollTop = chatRef.current.scrollHeight;
  }, [messages]);

  const SYS_PLANNING = getSysPlanning(sectionLabel);
  const CHIPS        = isAssignment ? CHIPS_ASSIGNMENT : CHIPS_PLANNING;

  // Turns the raw AI JSON string into a readable chat message (title, instructions,
  // numbered questions with options/pairs/answer). Falls back to the raw text if
  // it isn't valid quiz JSON (e.g. planning mode or a malformed reply).
  function formatAIResponseAsChat(rawText) {
    try {
      const jsonMatch = rawText.match(/\{[\s\S]+\}/);
      if (!jsonMatch) return rawText;
      const data = JSON.parse(jsonMatch[0]);
      if (!data.questions) return rawText;

      const lines = [];

      if (data.title) lines.push(`📝 ${data.title}\n`);
      if (data.instructions) lines.push(`${t('quiz.ai-panel-instructions-label')}${data.instructions}\n`);

      data.questions.forEach((question, index) => {
        const typeId = String(question.TypeId || '1');
        lines.push(`${index + 1}. [${t(QUESTION_TYPE_LABELS[typeId]) || typeId}] ${question.Text || ''}`);

        if (typeId === '1' && Array.isArray(question.Options)) {
          question.Options.forEach((option, i) => {
            const isCorrect = i === (question.CorrectIndex ?? -1);
            lines.push(`   ${OPTION_LETTERS[i] ?? i + 1}. ${option}${isCorrect ? ' ✓' : ''}`);
          });
        }

        if (typeId === '2' && Array.isArray(question.Pairs)) {
          question.Pairs.forEach(pair => lines.push(`   ${pair.l} → ${pair.r}`));
        }

        if (typeId === '4') {
          const correctLabel = (question.CorrectIndex ?? 0) === 0 ? 'Cierto ✓ / Falso' : 'Cierto / Falso ✓';
          lines.push(`   Respuesta: ${correctLabel}`);
        }
      });

      return lines.join('\n');
    } catch {
      return rawText;
    }
  }

  // Sends a message (typed or from a suggestion chip) to the AI and appends
  // the reply to the chat. Also parses the reply as JSON so "Fill" can use it.
  async function send(textFromChip) {
    const userMessage = textFromChip || input.trim();
    if (!userMessage || loading) return;
    setInput("");
    const messagesWithUserInput = [...messages, { role: "usr", text: userMessage }];
    setMessages([...messagesWithUserInput, { role: "typ", text: t('quiz.ai-panel-thinking') }]);
    setLoading(true);
    try {
      // Assignment mode splits the payload: all the context (current quiz + chat history)
      // goes into the system prompt, and `user` carries only what the teacher wrote. The
      // model gets the same total input, but the audit can store the prompt (user) apart
      // from the context (system) with no backend change. Planning mode is unchanged.
      const assignmentPayload = isAssignment
        ? buildAssignmentUserMessage(getFormContext?.(), messages, userMessage)
        : null;
      const systemPrompt = isAssignment ? `${SYS_ASSIGNMENT}\n\n${assignmentPayload.context}` : SYS_PLANNING;
      const userPayload  = isAssignment ? assignmentPayload.request : userMessage;
      const aiResponse   = await generateStreamedResponse(systemPrompt, userPayload);
      let parsedJsonResponse = null;
      try {
        const jsonMatch = aiResponse.match(/\{[\s\S]+\}/);
        if (jsonMatch) parsedJsonResponse = JSON.parse(jsonMatch[0]);
      } catch (_) {}
      setMessages([...messagesWithUserInput, { role: "ast", rawResponse: aiResponse }]);
      if (parsedJsonResponse) { setLastParsed(parsedJsonResponse); setCanFill(true); }
    } catch {
      setMessages([...messagesWithUserInput, { role: "ast", text: t('quiz.ai-panel-error') }]);
    }
    setLoading(false);
  }

  // Applies the last parsed AI JSON to the parent form via onFill
  function handleFill() {
    if (onFill && lastParsed) onFill(lastParsed);
    setCanFill(false);
    setMessages(previousMessages => [...previousMessages, { role: "ast", text: t('quiz.ai-panel-content-applied') }]);
  }

  return (
    <div className="ai-panel">
      <div className="ai-panel-header">
        <span className="ai-badge">✨ IA</span>
        <p className="ai-panel-title">{isAssignment ? t('quiz.ai-panel-title-assignment') : t('quiz.ai-panel-title-planning')}</p>
      </div>
      {/* Suggestion chips: clicking one sends its text as a message */}
      <div className="ai-chips">
        {CHIPS.map((chipKey, index) => <button key={index} className="ai-chip" onClick={() => send(t(chipKey))}>{t(chipKey)}</button>)}
      </div>
      {/* Chat history: greeting plus each user/AI message, auto-scrolled into view */}
      <div className="ai-chat" ref={chatRef}>
        <div className="ai-msg ast">
          {isAssignment ? t('quiz.ai-panel-greeting-assignment') : t('quiz.ai-panel-greeting-planning')}
        </div>
        {messages.map((message, index) => (
          <div key={index} className={`ai-msg ${message.role}`}>
            {message.rawResponse ? formatAIResponseAsChat(message.rawResponse) : message.text}
          </div>
        ))}
      </div>
      {/* Text input: onActivity notifies the parent to postpone its autosave while typing */}
      <div className="ai-input-row">
        <input className="ai-input" placeholder={t('quiz.ai-panel-placeholder')} value={input}
          onChange={event => { setInput(event.target.value); onActivity?.(); }} onKeyDown={event => event.key === "Enter" && send()} disabled={loading} />
        <button className="ai-send-btn" onClick={() => send()} disabled={loading || !input.trim()}>
          {loading ? "…" : t('quiz.ai-panel-send')}
        </button>
      </div>
      {/* "Fill" button only shows once the last AI reply parsed into valid quiz JSON */}
      {canFill && <button className="ai-fill-btn" onClick={handleFill}>{t('quiz.ai-panel-fill')}</button>}
    </div>
  );
}