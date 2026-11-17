import {marked} from "marked";

export function stripLayoutLatex(s) {
    return s
        // Whole fill-in-blank line: \text{Label:} \quad \underline{\hspace{5cm}} → gone entirely.
        // A \text{...} chased by spacers into an \underline/\hspace blank is a blank, not content.
        .replace(
            /\\text\s*\{[^}]*\}\s*(?:\\(?:quad|qquad|,|;|:|!|enspace|thinspace)\s*|\\[hv]space\*?\s*\{[^}]*\}\s*)*\\(?:underline|hspace\*?|rule)\s*(?:\[[^\]]*\])?\s*\{[^}]*\}(?:\s*\{[^}]*\})?/g,
            ' '
        )
        // --- piecemeal cleanup for any blanks WITHOUT a leading label ---
        .replace(/\\rule\s*(\[[^\]]*\])?\s*\{[^}]*\}\s*\{[^}]*\}/g, ' ')
        .replace(/\\(?:h|v)space\*?\s*\{[^}]*\}/g, ' ')
        .replace(/\\[hv]?phantom\s*\{[^}]*\}/g, ' ')
        .replace(/\\underline\s*\{([^}]*)\}/g, '$1')
        .replace(/\\(?:quad|qquad|,|;|:|!|enspace|thinspace)\b/g, ' ')
        .replace(/(?<![\w.])\d+(?:\.\d+)?\s*(?:cm|mm|pt|em|ex|in|px)\b/g, ' ')
        .replace(/[ \t]{2,}/g, ' ');
}
export function latexToReadable(text = '') {
    let s = String(text)
        // Math delimiters: $$…$$, \[…\] (display) and $…$, \(…\) (inline)
        .replace(/\$\$([\s\S]+?)\$\$/g, '\n$1\n')
        .replace(/\\\[([\s\S]+?)\\\]/g, '\n$1\n')
        .replace(/\$([^$\n]+?)\$/g, '$1')
        .replace(/\\\(([\s\S]+?)\\\)/g, '$1');

    // Resolve \frac repeatedly so nested fractions also flatten
    let prev;
    do {
        prev = s;
        s = s.replace(/\\frac\s*\{([^{}]+)\}\s*\{([^{}]+)\}/g, '($1)/($2)');
    } while (s !== prev);

    return s
        .replace(/\\sqrt\s*\{([^{}]+)\}/g, 'sqrt($1)')
        .replace(/\\left|\\right/g, '')
        .replace(/\\times/g, 'x')
        .replace(/\\div/g, '/')
        .replace(/\\cdot/g, '*')
        .replace(/\\leq/g, '<=')
        .replace(/\\geq/g, '>=')
        .replace(/\\neq/g, '!=')
        .replace(/\\pi/g, 'pi')
        .replace(/\\theta/g, 'theta')
        .replace(/\\alpha/g, 'alpha')
        .replace(/\\beta/g, 'beta')
        .replace(/\\degree/g, ' degrees')
        .replace(/\^\\circ/g, ' degrees')
        .replace(/\^{([^{}]+)}/g, '^$1')
        .replace(/_{([^{}]+)}/g, '_$1')
        .replace(/\\[a-zA-Z]+/g, '')
        // Strip any stray \( \) \[ \] that slipped through (unmatched delimiters)
        .replace(/\\[()[\]]/g, '')
        .replace(/[{}]/g, '');
}

export function sanitizePdfText(text = '') {
    return String(text)
        .normalize('NFKC')
        .replace(/✅|☑️|✔️|✔/g, '[OK]')
        .replace(/❌|✖️|✖/g, '[X]')
        .replace(/⭐|🌟|★/g, '*')
        .replace(/➡️|➜|→/g, '->')
        .replace(/⬅️|←/g, '<-')
        .replace(/📌|🔹|🔸|▪️|▫️|•/g, '-')
        .replace(/[\u200D\uFE0E\uFE0F]/g, '')
        .replace(/[\u{1F000}-\u{1FAFF}]/gu, '')
        .replace(/[\u{2600}-\u{27BF}]/gu, '')
        .replace(/\u00A0/g, ' ')
        .replace(/[ \t]{2,}/g, ' ')
        .trimEnd();
}

export function markdownToPlainLines(markdown = '') {
    return sanitizePdfText(latexToReadable(stripEditorMetadata(stripSlideNumberLabels(markdown))))
        .replace(/```[\s\S]*?```/g, '')
        .replace(/!\[[^\]]*\]\([^)]+\)/g, '[image]')
        .replace(/\[([^\]]+)\]\([^)]+\)/g, '$1')
        .replace(/\*\*([^*]+)\*\*/g, '$1')
        .replace(/`([^`]+)`/g, '$1')
        .replace(/^>\s+/gm, '')
        .replace(/\r/g, '')
        .split('\n');
}

export function stripSlideNumberLabels(text = '') {
    // Regex matches "slide N" prefixes regardless of language spelling
    return String(text).replace(
        /^(\s*(?:#{1,6}\s*|[-*]\s+|\d+[.)]\s+)?)\s*(?:diapositiva|dispositiva|slide)\s*\d+\s*[:.)-]?\s*/gim,
        (_, prefix = '') => prefix
    );
}

export function stripEditorMetadata(text = '') {
    return String(text)
        .replace(/<!--\s*(?:layout|style):\s*[\s\S]*?-->/gi, '')
        .replace(/\n{3,}/g, '\n\n')
        .trim();
}

export function addWrappedText(doc, text, x, y, maxWidth, lineHeight) {
    const safeText = sanitizePdfText(text);
    if (!safeText.trim()) return y;
    const lines = doc.splitTextToSize(safeText, maxWidth);
    doc.text(lines, x, y);
    return y + lines.length * lineHeight;
}

export function markdownWithMathToHtml(markdown) {
    const store = [];
    const stash = (m) => `@@MATH${store.push(m) - 1}@@`;
    const src = String(markdown || '')
        .replace(/\$\$[\s\S]+?\$\$/g, stash)   // $$ display $$
        .replace(/\\\[[\s\S]+?\\\]/g, stash)   // \[ display \]
        .replace(/\$[^\n$]+?\$/g, stash)       // $ inline $
        .replace(/\\\([\s\S]+?\\\)/g, stash);  // \( inline \)
    const html = marked.parse(src, { gfm: true, breaks: false });
    return html.replace(/@@MATH(\d+)@@/g, (_, i) => store[Number(i)]);
}

export function escapeHtml(s = '') {
    return String(s).replace(/[&<>"']/g, (c) =>
        ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
}

/** True if the text contains any span the math pipeline would typeset. */
export function hasLatex(text = '') {
    const s = String(text);
    return /\$\$[\s\S]+?\$\$/.test(s)   // $$ ... $$
        || /\\\[[\s\S]+?\\\]/.test(s)   // \[ ... \]
        || /\$[^\n$]+?\$/.test(s)       // $ ... $
        || /\\\([\s\S]+?\\\)/.test(s);  // \( ... \)
}
