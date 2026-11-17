/**
 * tool-modal-utils.jsx
 * Pure helpers: HTML entity decoding, HTML stripping, LaTeX rendering,
 * Markdown→HTML conversion, output cleaning, and image-tag resolution.
 * No React. No i18n. No Spanish.
 */
import katex from 'katex';
import Settings, {IMAGE_NEGATIVE_PROMPT} from "../../../../../../custom-classes/settings";
import {getAuthHeaders} from "../../../../../../api/interceptors";

// ---------------------------------------------------------------------------
// HTML entity / tag utilities
// ---------------------------------------------------------------------------

const NAMED_HTML_ENTITIES = {
    amp: '&', lt: '<', gt: '>', quot: '"', apos: "'", nbsp: ' ',
    bull: '\u2022', iquest: '\u00bf', iexcl: '\u00a1', ndash: '-', mdash: '-',
    aacute: '\u00e1', eacute: '\u00e9', iacute: '\u00ed', oacute: '\u00f3', uacute: '\u00fa',
    Aacute: '\u00c1', Eacute: '\u00c9', Iacute: '\u00cd', Oacute: '\u00d3', Uacute: '\u00da',
    ntilde: '\u00f1', Ntilde: '\u00d1', uuml: '\u00fc', Uuml: '\u00dc',
    ldquo: '"', rdquo: '"', lsquo: "'", rsquo: "'",
};

export function decodeHtmlEntities(value = '') {
    return String(value).replace(/&(#x?[0-9a-f]+|[a-z]+);/gi, (match, entity) => {
        if (entity[0] === '#') {
            const isHex = entity[1]?.toLowerCase() === 'x';
            const code = parseInt(entity.slice(isHex ? 2 : 1), isHex ? 16 : 10);
            return Number.isFinite(code) ? String.fromCodePoint(code) : match;
        }
        return Object.prototype.hasOwnProperty.call(NAMED_HTML_ENTITIES, entity)
            ? NAMED_HTML_ENTITIES[entity]
            : match;
    });
}

/**
 * Strip HTML tags from Athenas API content (which returns definition bodies
 * as HTML including embedded <audio> elements). Produces plain readable text
 * suitable for LLM prompts.
 */
export function stripDefinitionHtml(html) {
    if (!html) return '';
    return decodeHtmlEntities(
        String(html)
            .replace(/<audio[\s\S]*?<\/audio>/gi, '')
            .replace(/<[^>]+>/g, ' ')
            .replace(/&nbsp;/g, ' ')
            .replace(/&amp;/g, '&')
    ).replace(/\s+/g, ' ').trim();
}

// ---------------------------------------------------------------------------
// LaTeX / KaTeX
// ---------------------------------------------------------------------------

/**
 * Render a single LaTeX snippet to HTML via KaTeX.
 * Falls back to escaped raw text on parse errors — which is common during
 * streaming when the math expression is not yet fully received.
 */
export function renderMath(tex, displayMode) {
    try {
        return katex.renderToString(tex, { displayMode, throwOnError: false, strict: 'ignore' });
    } catch {
        const escaped = tex
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
        return displayMode
            ? `<pre class="tm-math-err">${escaped}</pre>`
            : `<code>${escaped}</code>`;
    }
}

// Regular expression to match a single LaTeX snippet, either inline ($...$) or block ($$...$$)
export const INLINE_MATH_RE = /\$(?!\s)([^\n$]+?)(?<!\s)\$(?!\d)|\$[ \t]*([^\n$]*?\\[a-zA-Z][^\n$]*?)[ \t]*\$(?!\d)/g;

// ---------------------------------------------------------------------------
// Markdown → HTML
// ---------------------------------------------------------------------------

// Editor-only metadata comments written by slidesToMarkdown(). Because mdToHtml
// escapes all angle brackets before rendering, any surviving HTML comment shows
// up as visible text — so every key slidesToMarkdown emits must be stripped here.
const EDITOR_METADATA_COMMENT =
    /<!--\s*(?:type|claim|visual|align|layout|style|notes)\s*:[\s\S]*?-->[ \t]*\r?\n?/gi;

export function stripEditorMetadata(text = '') {
    return String(text)
        .replace(EDITOR_METADATA_COMMENT, '')
        .replace(/\n{3,}/g, '\n\n')
        .trim();
}

/**
 * Produce a display-safe copy of the shared slide `value` for any surface that
 * is NOT the slide editor (markdown preview, PDF export, etc.).
 *
 * The editor metadata comments (type/layout/style/notes/…) must remain in the
 * stored `value` so parseEditableSlides can round-trip them, but they should be
 * invisible everywhere else. This returns a filtered copy WITHOUT mutating the
 * source, so the slide editor keeps parsing the untouched original.
 *
 * Usage on non-slide surfaces:
 *   generatePdf(toDisplayMarkdown(value));
 *   <RawPreview markdown={toDisplayMarkdown(value)} />
 *
 * mdToHtml() already applies stripEditorMetadata internally, so any surface that
 * renders through mdToHtml is covered without calling this explicitly.
 */
export function toDisplayMarkdown(value) {
    return stripEditorMetadata(value || '');
}

export function mdToHtml(md) {
    if (!md) return '';
    md = stripEditorMetadata(md);

    // Extract math blocks before any HTML-escaping or markdown passes can mangle them.
    // Block ($$...$$) first, then inline ($...$). Inline avoids matching currency,
    // see INLINE_MATH_RE
    const mathParts = [];
    const stashMath = (latex, displayMode) => {
        const idx = mathParts.length;
        mathParts.push({ latex, displayMode });
        return `__GSM_MATH_${idx}__`;
    };

    let work = md
        .replace(/\$\$([\s\S]+?)\$\$/g, (_, tex) => stashMath(tex.trim(), true))
        .replace(/\\\[([\s\S]+?)\\\]/g, (_, tex) => stashMath(tex.trim(), true))
        .replace(INLINE_MATH_RE, (_, tex, padded) => stashMath(tex ?? padded, false))
        .replace(/\\\(([^\n]+?)\\\)/g, (_, tex) => stashMath(tex, false));

    let html = work
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');

    // Tables: contiguous lines containing pipes
    html = html.replace(/((?:^\|.*\|\s*\n?)+)/gm, (block) => {
        const rows = block.trim().split('\n');
        if (rows.length < 2) return block;
        const cells = (r) => r.replace(/^\||\|$/g, '').split('|').map(c => c.trim());
        const head = cells(rows[0]);
        const isSep = /^\s*[:|\-\s]+\s*$/.test(rows[1].replace(/\|/g, ''));
        const bodyRows = (isSep ? rows.slice(2) : rows.slice(1)).map(cells);
        let out = '<table><thead><tr>' + head.map(h => `<th>${h}</th>`).join('') + '</tr></thead><tbody>';
        bodyRows.forEach(r => { out += '<tr>' + r.map(c => `<td>${c}</td>`).join('') + '</tr>'; });
        return out + '</tbody></table>';
    });

    html = html
        .replace(/^######\s+(.+)$/gm, '<h6>$1</h6>')
        .replace(/^#####\s+(.+)$/gm,  '<h5>$1</h5>')
        .replace(/^####\s+(.+)$/gm,   '<h4>$1</h4>')
        .replace(/^###\s+(.+)$/gm,    '<h3>$1</h3>')
        .replace(/^##\s+(.+)$/gm,     '<h2>$1</h2>')
        .replace(/^#\s+(.+)$/gm,      '<h1>$1</h1>')
        .replace(/`([^`\n]+)`/g,      '<code>$1</code>')
        .replace(/\*\*([^*]+)\*\*/g,  '<strong>$1</strong>')
        .replace(/(^|[\s(])\*([^*\n]+)\*(?=[\s).,!?]|$)/g, '$1<em>$2</em>')
        // Images wrapped in a link so clicking opens the full-size version
        .replace(
            /!\[([^\]]*)\]\(([^)\s]+)\)/g,
            '<a href="$2" target="_blank" rel="noopener noreferrer" class="tm-md-img-link" title="Click to view full size"><img src="$2" alt="$1" class="tm-md-img" loading="lazy" /></a>'
        )
        // Placeholder shown while an AI image is being generated
        .replace(
            /\[IMAGE_LOADING:\s*([^\]]+)\]/g,
            '<div class="tm-md-img-loading">🎨 Generating image: <em>$1</em></div>'
        );

    // Unordered lists
    html = html.replace(/(?:^|\n)((?:- .+\n?)+)/g, (_, list) => {
        const items = list.trim().split('\n')
            .map(l => `<li>${l.replace(/^- /, '')}</li>`).join('');
        return `\n<ul>${items}</ul>`;
    });

    // Ordered lists
    // An item runs to the next "number" marker, so multi-line questions don't each become their own <ol> restarting at 1
    html = html.replace(/(?:^|\n)((?:\d+\.[ \t].*(?:\n(?:[ \t]+.*)?)*)+)/g, (match, block) => {
        const parts = block.split(/\n(?=\d+\.[ \t])/).filter(x => x.trim());
        if (!parts.length) return match;
        const start = parseInt(parts[0], 10);
        const items = parts
            .map(item => `<li>${item.trim().replace(/^\d+\.[ \t]*/, "").replace(/\n[ \t]*/g, "<br/>")}</li>`)
            .join("");
        const startAttr = Number.isFinite(start) && start !== 1 ? ` start="${start}"` : "";
        return `\n<ol${startAttr}>${items}</ol>`;
    });

    // Paragraphs — lines not already inside a block element
    html = html.split(/\n{2,}/).map(p => {
        if (/^\s*<(h\d|ul|ol|table|pre)/.test(p)) return p;
        return `<p>${p.replace(/\n/g, '<br/>')}</p>`;
    }).join('\n');

    // Restore KaTeX-rendered math at placeholder positions
    html = html.replace(/__GSM_MATH_(\d+)__/g, (_, n) => {
        const item = mathParts[Number(n)];
        if (!item) return '';
        const rendered = renderMath(item.latex, item.displayMode);
        return item.displayMode
            ? `<div class="tm-math-block">${rendered}</div>`
            : rendered;
    });

    return html;
}

// ---------------------------------------------------------------------------
// Inline formatting for the rich slide editor
// ---------------------------------------------------------------------------

export function parseInlineFormatting(text) {
    if (!text) return '';

    const mathParts = [];
    const stashMath = (latex, displayMode) => {
        const idx = mathParts.length;
        mathParts.push({ latex, displayMode });
        return ` MATH${idx} `;
    };

    const work = text
        .replace(/\$\$([\s\S]+?)\$\$/g, (_, tex) => stashMath(tex.trim(), true))
        .replace(/\\\[([\s\S]+?)\\\]/g, (_, tex) => stashMath(tex.trim(), true))
        .replace(INLINE_MATH_RE, (_, tex, padded) => stashMath(tex ?? padded, false))
        .replace(/\\\(([^\n]+?)\\\)/g, (_, tex) => stashMath(tex, false));

    let out = work
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
        .replace(/\*([^*]+)\*/g, '<em>$1</em>');

    out = out.replace(/ MATH(\d+) /g, (_, n) => {
        const item = mathParts[Number(n)];
        if (!item) return '';
        const rendered = renderMath(item.latex, item.displayMode);
        const safeTex = item.latex
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
        const delim = item.displayMode ? '$$' : '$';
        return `<span class="tm-math-inline${item.displayMode ? ' display' : ''}" contenteditable="false" data-tex="${safeTex}" data-delim="${delim}">${rendered}</span>`;
    });

    return out;
}

export function markdownToHtmlForEditor(md) {
    if (!md) return '<div><br></div>';
    const lines = md.split('\n');
    let inList = false;
    const htmlParts = [];

    for (const line of lines) {
        const trimmed = line.trim();
        if (trimmed.startsWith('- ') || trimmed.startsWith('* ')) {
            if (!inList) { htmlParts.push('<ul>'); inList = true; }
            htmlParts.push(`<li>${parseInlineFormatting(trimmed.substring(2))}</li>`);
        } else {
            if (inList) { htmlParts.push('</ul>'); inList = false; }
            if (trimmed === '') {
                htmlParts.push('<div><br></div>');
            } else {
                htmlParts.push(`<div>${parseInlineFormatting(line)}</div>`);
            }
        }
    }
    if (inList) htmlParts.push('</ul>');
    return htmlParts.join('');
}

export function htmlToMarkdown(html) {
    if (!html) return '';
    const temp = document.createElement('div');
    temp.innerHTML = html;

    function clean(node) {
        if (node.nodeType === Node.TEXT_NODE) return node.nodeValue;
        if (node.nodeType !== Node.ELEMENT_NODE) return '';

        const tag = node.tagName.toLowerCase();

        if (tag === 'span' && node.classList?.contains('tm-math-inline')) {
            const tex = node.getAttribute('data-tex') || '';
            const delim = node.getAttribute('data-delim') || '$';
            return `${delim}${tex}${delim}`;
        }
        if (tag === 'li') {
            let childText = '';
            for (const child of node.childNodes) childText += clean(child);
            return `- ${childText.trim()}\n`;
        }
        if (tag === 'strong' || tag === 'b') {
            let childText = '';
            for (const child of node.childNodes) childText += clean(child);
            return `**${childText}**`;
        }
        if (tag === 'em' || tag === 'i') {
            let childText = '';
            for (const child of node.childNodes) childText += clean(child);
            return `*${childText}*`;
        }
        if (tag === 'br') return '\n';
        if (tag === 'div' || tag === 'p') {
            let childText = '';
            for (const child of node.childNodes) childText += clean(child);
            return childText.endsWith('\n') ? childText : childText + '\n';
        }
        let childText = '';
        for (const child of node.childNodes) childText += clean(child);
        return childText;
    }

    return clean(temp).replace(/\n{3,}/g, '\n\n').trim();
}

// ---------------------------------------------------------------------------
// Output cleaning
// ---------------------------------------------------------------------------

// Strip any image markdown whose src is a placeholder URL
const PLACEHOLDER_IMAGE_HOST = /^(?:https?:)?\/\/(?:[\w-]+\.)*(?:example\.(?:com|org|net)|placeholder\.com|via\.placeholder\.com|placehold\.(?:it|co)|dummyimage\.com|lorempixel\.com|placekitten\.com|yourdomain\.com|tu-?dominio\.com|url-?de-?la-?imagen)/i;

// Drop images with an invented placeholder URL, keeping the alt text as a caption.
export function stripPlaceholderImages(text = '') {
    return text.replace(
        /!\[([^\]]*)\]\(\s*([^)\s]+)(?:\s+"[^"]*")?\s*\)/g,
        (full, alt, src) => {
            if (!PLACEHOLDER_IMAGE_HOST.test(src)) return full;
            return alt.trim() ? `*${alt.trim()}*` : '';
        }
    );
}

/**
 * Remove slide-number prefixes and stray grade-level tokens that LLMs
 * sometimes prepend to slide content. Language-neutral — matches both
 * English ("slide 1") and Spanish ("diapositiva 1 / dispositiva 1").
 */
export function cleanGeneratedOutput(text = '') {
    return stripEditorMetadata(
        stripPlaceholderImages(
            String(text)
                .replace(
                    /^(\s*(?:#{1,6}\s*|[-*]\s+|\d+[.)]\s+)?)\s*(?:diapositiva|dispositiva|slide)\s*\d+\s*[:.)-]?\s*/gim,
                    (_, prefix = '') => prefix
                )
                .replace(/^\s*:\s*(?:K|Pre-K|Kinder|[1-9](?:ro|do|to|mo)?|1[0-2](?:mo)?)\s*$/gim, '')
                .replace(/\s*<br\s*\/?>\s*/gi, ' ')
        )
    );
}

// ---------------------------------------------------------------------------
// Image tag resolution
// ---------------------------------------------------------------------------

// Define a strong negative prompt to block text, watermarks, and random elements
function enhanceImagePrompt(prompt) {
    let subject = prompt.trim();

    // Remove common introductory instructions
    subject = subject.replace(
        /^(please\s+)?(create|generate|draw|illustrate|show|display|depict|make|produce|provide)\s+(me\s+)?(a|an|the)?\s*/i,
        ''
    );

    // Remove common educational and descriptive instructions
    subject = subject.replace(
        /\b(detailed|educational|interactive|comprehensive|visual|informative|labeled|labelled|diagram|infographic|illustration|image|picture|chart|showing|explaining|highlighting|demonstrating|including|with labels|with arrows)\b/gi,
        ''
    );

    // Remove instructions following common connectors
    subject = subject.split(/\b(including|showing|explaining|highlighting|demonstrating|illustrating|with labels|with arrows|that explains|which shows)\b/i)[0];

    // Clean up remaining punctuation and whitespace
    subject = subject
        .replace(/[,:;.!?]+/g, ' ')
        .replace(/\s+/g, ' ')
        .trim();

    // If the prompt is an abstract concept or short phrase (like "add and subtract"),
    // force Stable Diffusion to render it as a clean, structured visual object/icon.
    return `${subject}`;
}

/**
 * Find every [IMAGE: prompt] tag in the markdown, fire parallel requests to
 * /api/ImageGenerate, and replace each tag with the resulting ![alt](url)
 * markdown. Updates output incrementally so the user sees images appear as
 * they finish. Requires t() for status strings.
 *
 * The backend returns signed blob URLs in `imageUrls`, which are used
 * directly as the markdown image src (no base64 conversion needed).
 */
export async function resolveImageTags(text, setOutput, setImageStatus, t) {
    const re = /\[IMAGE:\s*([^\]]+)\]/gi;
    const matches = [...text.matchAll(re)];
    if (!matches.length) return text;

    let working = text.replace(re, (_, prompt) => `[IMAGE_LOADING: ${prompt.trim()}]`);
    setOutput(working);
    setImageStatus(t('imageStatus.generating', { done: 0, total: matches.length }));

    let done = 0;
    await Promise.all(matches.map(async (m) => {
        const prompt = m[1].trim();
        const placeholder = `[IMAGE_LOADING: ${prompt}]`;
        const requestBody = {
            prompt: enhanceImagePrompt(prompt),
            negative_prompt: IMAGE_NEGATIVE_PROMPT,
            steps: 20,
            cfg_scale: 7
        };
        try {
            const res = await fetch(Settings.API_AI_URL + '/aiservicesapi/ImageGenerate', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
                body: JSON.stringify(requestBody),
            });
            const data = await res.json();
            const imageUrls = data.ImageUrls || data.imageUrls;
            if (!res.ok || !imageUrls || imageUrls.length === 0) {
                throw new Error(data.error || t('imagePicker.generateError'));
            }
            const altText = prompt.slice(0, 80).replace(/"/g, "'");
            const src = imageUrls[0];   // already a usable signed URL
            working = working.replace(placeholder, `![${altText}](${src})`);
        } catch (err) {
            working = working.replace(
                placeholder,
                `*[⚠ ${t('imageStatus.unavailable')}: ${err.message || err}]*`
            );
        }
        done++;
        setOutput(working);
        setImageStatus(t('imageStatus.generating', { done, total: matches.length }));
    }));

    setImageStatus('');
    return working;
}

// ---------------------------------------------------------------------------
// Athenas lesson formatter (for LLM context injection)
// ---------------------------------------------------------------------------

/**
 * Format an Athenas lesson object into a rich plain-text reference block
 * suitable for injection into an LLM system or user prompt. Accepts `t`
 * so every section label is rendered in the active UI language.
 */
export function formatLessonAsReference(lesson, t) {

    const lines = [];
    lines.push(`📘 ${t('tool-modal:lessonRef.header')} — ${lesson.LessonModel.LessonTitle}`);
    const meta = [
        lesson.LessonModel.SubjectCode && `${t('tool-modal:lessonRef.subject')}: ${lesson.LessonModel.SubjectCode}`,
        lesson.LessonModel.LevelCode   && `${t('tool-modal:lessonRef.grade')}: ${lesson.LessonModel.LevelCode}`,
        lesson.LessonModel.LessonNo    && `${t('tool-modal:lessonRef.lessonNo')}: ${lesson.LessonModel.LessonNo}`,
        // A JSX-style {/* ... */} comment here evaluated to an empty object
        // and leaked into the meta line as "[object Object]".
        // lesson.blueprint    && '📐 Blueprint',
        // lesson.isGapClosing && '🌉 Gap closing',
    ].filter(Boolean).join(' · ');
    if (meta) lines.push(meta);

    // Use a Set to avoid duplicate standards
    const seenStandardCodes = new Set();

    lesson.standards = (lesson.ConstructedCodeHandleModelList)
        .flatMap(item => item?.ConstructedCodeCodeModels)
        .filter(code => code?.CodeTypeId === 1)
        .filter(code => {
            const key = code.Code ?? code.CodeDescription;
            if (seenStandardCodes.has(key)) return false;
            seenStandardCodes.add(key);
            return true;
        });


    if (lesson.standards?.length) {
        lines.push('');
        lines.push(t('tool-modal:lessonRef.standardsCovered'));
        lesson.standards.forEach(s => {
            lines.push(`  • ${s.Code}${s.Description ? ` — ${s.Description}` : ''}`);
        });
    }

    /*
    if (lesson.fullText) {
        const fullText = stripDefinitionHtml(lesson.fullText);
        if (fullText) {
            lines.push('');
            lines.push(t('tool-modal:lessonRef.fullContent'));
            lines.push(fullText);
        }
    } */

    if (lesson.LessonDetailModel.Description) {
        const desc = stripDefinitionHtml(lesson.LessonDetailModel.Description);
        if (desc) {
            lines.push('');
            lines.push(t('tool-modal:lessonRef.description'));
            lines.push(desc);
        }
    }

    if (lesson.LessonDetailModel.Concept) {
        const concept = stripDefinitionHtml(lesson.LessonDetailModel.Concept);
        if (concept) {
            lines.push('');
            lines.push(t('tool-modal:lessonRef.centralConcept'));
            lines.push(concept);
        }
    }

    if (lesson.LessonObjectiveModelList?.length) {
        lines.push('');
        lines.push(t('tool-modal:lessonRef.objectives', { count: lesson.LessonObjectiveModelList.length }));
        lesson.LessonObjectiveModelList.forEach(obj => {
            const text = stripDefinitionHtml(obj.Desc || obj.Description || '');
            if (text) lines.push(`  • ${text}`);
        });
    }

    if (lesson.LessonDefinitionModelList?.length) {
        lines.push('');
        lines.push(t('tool-modal:lessonRef.definitions', { count: lesson.LessonDefinitionModelList.length }));
        lesson.LessonDefinitionModelList.forEach(def => {
            const term = def.Name || def.DefinitionTitle || def.Term || def.Title || '';
            const body = stripDefinitionHtml(
                def.Desc || def.DefinitionText || def.Definition || def.Description || def.Body || ''
            );
            if (term || body) lines.push(`  • ${term}${term && body ? ': ' : ''}${body}`.trim());
        });
    }

    if (lesson.LessonExampleModelList?.length) {
        lines.push('');
        lines.push(t('tool-modal:lessonRef.examples', { count: lesson.LessonExampleModelList.length }));
        lesson.LessonExampleModelList.forEach(ex => {
            const name = ex.Name || ex.Title || '';
            const body = stripDefinitionHtml(ex.Desc || ex.Description || '');
            if (name || body) lines.push(`  • ${name}${name && body ? ': ' : ''}${body}`.trim());
        });
    }

    if (lesson.LessonPerformanceTaskModelList?.length) {
        lines.push('');
        lines.push(t('tool-modal:lessonRef.performanceTasks', { count: lesson.LessonPerformanceTaskModelList.length }));
        lesson.LessonPerformanceTaskModelList.forEach(task => {
            const text = stripDefinitionHtml(task.Desc || task.Description || task.Name || '');
            if (text) lines.push(`  • ${text}`);
        });
    }

    if (lesson.LessonStrategyModelList?.length) {
        lines.push('');
        lines.push(t('tool-modal:lessonRef.strategies', { count: lesson.LessonStrategyModelList.length }));
        lesson.LessonStrategyModelList.forEach(strat => {
            const text = stripDefinitionHtml(strat.Desc || strat.Description || strat.Name || '');
            if (text) lines.push(`  • ${text}`);
        });
    }

    if (lesson.LessonTransversalThemeModelList?.length) {
        const themeText = lesson.LessonTransversalThemeModelList
            .map(theme => stripDefinitionHtml(theme.Desc || theme.Name || ''))
            .filter(Boolean)
            .join(', ');
        if (themeText) {
            lines.push('');
            lines.push(`${t('tool-modal:lessonRef.themes')}: ${themeText}`);
        }
    }

    return lines.join('\n');
}

// ---------------------------------------------------------------------------
// Athenas assistant prompt builder
// ---------------------------------------------------------------------------

export function hasAthenasLesson(text) {
    return /LESSON|ATHENAS|Standards covered|Concepts \/ Definitions/i.test(String(text || ''));
}

/**
 * Wrap a raw user prompt with action-specific instructions when an Athenas
 * lesson is detected. Accepts `t` so the AI receives instructions in the
 * active UI language and responds accordingly.
 */
export function buildAthenasAssistantPrompt(rawPrompt, action, t) {

    const prompt = String(rawPrompt || '').trim();
    if (!hasAthenasLesson(prompt)) return prompt;

    const instructions = {
        deepen:    t('tool-modal:athenasPrompt.instructions.deepen'),
        practice:  t('tool-modal:athenasPrompt.instructions.practice'),
        dok34:     t('tool-modal:athenasPrompt.instructions.dok34'),
        worksheet: t('tool-modal:athenasPrompt.instructions.worksheet'),
        ppt:       t('tool-modal:athenasPrompt.instructions.ppt'),
    };

    const actionLabel = t(`tool-modal:chat.actions.${action}`, { defaultValue: action });

    return t('tool-modal:athenasPrompt.wrapper', {
        actionLabel,
        instruction: instructions[action] || instructions.deepen,
        prompt,
        interpolation: { escapeValue: false },
    });
}

// ---------------------------------------------------------------------------
// Image source normalization helpers
// ---------------------------------------------------------------------------

/**
 * NOTE: The backend now returns signed blob URLs (in `imageUrls`), so the
 * base64 conversion these helpers perform is no longer needed on the current
 * image path. They are kept only because other modules may still import them.
 * A signed URL passes straight through `toImageSrc` unchanged. Once a
 * project-wide search confirms no remaining imports, these can be removed.
 *
 * `toImageSrc` accepts: raw base64, a JSON envelope, a data: URL, or an
 * http(s)/blob URL, and always returns a usable <img src> ('' if unusable).
 */
export function toImageSrc(raw, mime = 'image/png') {
    if (!raw) return '';
    let s = String(raw).trim().replace(/^["']|["']$/g, '');

    // Already usable
    if (/^(data:|https?:\/\/|blob:)/i.test(s)) return s;

    // Some backends wrap it: {"b64_json": "..."} / {"image": "..."} / {"data": "..."}
    if (s.startsWith('{')) {
        try {
            const o = JSON.parse(s);
            s = o.b64_json || o.base64 || o.image || o.data || o.url || s;
            if (/^(data:|https?:\/\/|blob:)/i.test(s)) return s;
        } catch { /* keep the raw string */ }
    }

    // SSE chunking can inject newlines/spaces into the payload
    s = s.replace(/\s+/g, '');

    // Bare base64 → data URL. The length guard keeps ordinary relative paths
    // (e.g. 'images/foo') from being misread as base64.
    if (/^[A-Za-z0-9+/]+={0,2}$/.test(s) && s.length > 100) {
        return `data:${detectMime(s) || mime};base64,${s}`;
    }
    return s;
}

/** Sniffs the image type from the first bytes of a base64 payload. */
function detectMime(b64) {
    if (b64.startsWith('iVBORw0KGgo')) return 'image/png';
    if (b64.startsWith('/9j/'))        return 'image/jpeg';
    if (b64.startsWith('R0lGOD'))      return 'image/gif';
    if (b64.startsWith('UklGR'))       return 'image/webp';
    if (b64.startsWith('PHN2Zy')       // '<svg'
        || b64.startsWith('PD94bWw'))  // '<?xml'
        return 'image/svg+xml';
    return null;
}

/**
 * Rewrites every markdown image in a document whose src is bare base64
 * into a proper data: URL. Safe to call more than once — already-valid
 * sources (including signed URLs) pass through untouched.
 */
export function normalizeBase64Images(markdown) {
    if (!markdown) return markdown ?? '';
    return String(markdown).replace(
        /!\[([^\]]*)\]\(\s*([^)\s]+)(\s+"[^"]*")?\s*\)/g,
        (_m, alt, src, title = '') => `![${alt}](${toImageSrc(src)}${title})`
    );
}
