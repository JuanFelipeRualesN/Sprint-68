import React from "react";
import { Box, Button } from "@material-ui/core";
import { CircleAlert } from "lucide-react";
import "./style.css"

export const AnswerError = ({ error, onRetry }) => {
  return (
    <Box className="answer-container"  flexDirection="column">
      <CircleAlert size={28} color="red" aria-hidden="true" />
      <p className="answer-text">{error}</p>
      <Button
        variant="contained"
        color="primary"
        className="retry-button"
        onClick={onRetry}
      >
        Retry
      </Button>
    </Box>
  );
};
