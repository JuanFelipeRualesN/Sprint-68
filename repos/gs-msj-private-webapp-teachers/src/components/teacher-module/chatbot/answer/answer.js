import React, { useMemo } from "react";
import { Box } from '@material-ui/core';
import DOMPurify from "dompurify";
import { marked } from "marked";
import "./style.css";

marked.setOptions({
    gfm: true,
    breaks: true,
});

const Answer = ({answer}) => {
    // Sanitize and convert markdown to HTML
    const safeHtml = useMemo(() => {
        const raw = typeof answer === "string" ? answer : (answer?.answer ?? "");
        const html = marked.parse(raw);
        return DOMPurify.sanitize(html);
    }, [answer]);

    return (
        <Box className="answer-container" verticalAlign="space-between">
            <div className="answer-text" dangerouslySetInnerHTML={{ __html: safeHtml }}/>
        </Box>
    );
};

export default Answer;