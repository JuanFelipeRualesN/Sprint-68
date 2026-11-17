import React, { useState } from "react";
import { IconButton, Tooltip  } from "@material-ui/core";
import { Box, TextField } from '@material-ui/core';
import { Send as SendIcon, FlashOn as FlashOnIcon, Assignment as AssignmentIcon } from "@material-ui/icons";
import { makeStyles } from "@material-ui/core/styles";
import { withLocalize } from "react-localize-redux";
import "./style.css";

const useStyles = makeStyles({
  tooltip: {
    fontSize: "12px",
    padding: "8px 12px",
  },
  planningButton: {
    padding: '6px !important',
    backgroundColor: 'transparent !important',
    color: 'rgb(102, 190, 78) !important',
    border: '1px solid rgb(102, 190, 78) !important',
    borderRadius: '6px !important',
    '&:hover': {
      backgroundColor: 'rgba(102, 190, 78, 0.1) !important',
    },
  },
  planningButtonActive: {
    padding: '6px !important',
    backgroundColor: 'rgb(102, 190, 78) !important',
    color: '#fff !important',
    border: '1px solid rgb(102, 190, 78) !important',
    borderRadius: '6px !important',
    '&:hover': {
      backgroundColor: 'rgb(92, 170, 68) !important',
    },
  },
});

const QuestionInput = ({ translate, onSend, disabled, placeholder, clearOnSend, togglePlanningMode, planningMode }) => {
  const classes = useStyles();
  const [question, setQuestion] = useState("");

  const sendQuestion = () => {
    if (disabled || !question.trim()) return;

    onSend(question);

    if (clearOnSend) {
      setQuestion("");
    }
  };

  const onEnterPress = (ev) => {
    if (ev.key === "Enter" && !ev.shiftKey) {
      ev.preventDefault();
      sendQuestion();
    }
  };

  const onQuestionChange = (ev) => {
    const newValue = ev.target.value;
    if (!newValue) {
      setQuestion("");
    } else if (newValue.length <= 1000) {
      setQuestion(newValue);
    }
  };

  const sendQuestionDisabled = disabled || !question.trim();

  return (
    <Box display="flex" className="question-input-container">
      <TextField
        className="question-input-text-area"
        placeholder={placeholder}
        multiline
        fullWidth
        rowsMin={3}
        rowsMax={15}
        value={question}
        onChange={onQuestionChange}
        onKeyDown={onEnterPress}
        InputProps={{
          disableUnderline: true,
          style: {
            borderRadius: "10px",
            padding: "10px 5px 50px 5px",
            fontSize: "14px",
          },
        }}
      />

      <Box className="planning-buttons-container">
        <Tooltip title={translate("chatbot.create-quick-planning")} placement="top" classes={{ tooltip: classes.tooltip }}>
          <IconButton
            size="small"
            onClick={() => togglePlanningMode('quick')}
            disabled={disabled}
            className={planningMode === 'quick' ? classes.planningButtonActive : classes.planningButton}
          >
            <FlashOnIcon style={{ fontSize: '20px' }} />
          </IconButton>
        </Tooltip>

        <Tooltip title={translate("chatbot.create-detailed-planning")} placement="top" classes={{ tooltip: classes.tooltip }}>
          <IconButton
            size="small"
            onClick={() => togglePlanningMode('detailed')}
            disabled={disabled}
            className={planningMode === 'detailed' ? classes.planningButtonActive : classes.planningButton}
          >
            <AssignmentIcon style={{ fontSize: '20px' }} />
          </IconButton>
        </Tooltip>
      </Box>

      <div className="question-input-buttons-container">
        <IconButton
          className="question-input-send-button"
          aria-label="Ask Questions"
          onClick={sendQuestion}
          disabled={sendQuestionDisabled}
        >
          <SendIcon style={{ color: "rgb(102, 190, 78)" }} />
        </IconButton>
      </div>
    </Box>
  );
};

export default withLocalize(QuestionInput);