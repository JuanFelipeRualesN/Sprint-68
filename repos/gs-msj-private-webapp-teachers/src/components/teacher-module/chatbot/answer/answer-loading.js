import React from 'react';
import { Fade, Box } from '@material-ui/core';
import "./style.css";

export const AnswerLoading = () => {
  return (
    <Fade in={true} timeout={500}>
      <Box 
        display="flex" 
        flexDirection="column" 
        justifyContent="space-between" 
        className="answer-container"
      >
        <Box flexGrow={1}>
          <p className="answer-text">
            <span className="loading-dots"/>
          </p>
        </Box>
      </Box>
    </Fade>
  );
};