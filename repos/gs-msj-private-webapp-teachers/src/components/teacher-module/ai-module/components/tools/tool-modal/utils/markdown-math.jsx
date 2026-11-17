import React from 'react';
import katex from 'katex';
import 'katex/dist/katex.min.css';
import { INLINE_MATH_RE } from '../tool-modal-utils';

function escapeHtml(text = '') {
    return String(text)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

const isTableLine = (line) => /^\s*\|.*\|\s*$/.test(line);
const isSeparatorLine = (line) =>
    /^\s*\|?\s*:?-{3,}:?\s*(\|\s*:?-{3,}:?\s*)*\|?\s*$/.test(line);

function splitCells(line) {
    return line
        .trim()
        .replace(/^\|/, '')
        .replace(/\|$/, '')
        .split('|')
        .map((c) => c.trim());
}

// Reads the |---|:---:|---:| separator row for per-column alignment
function parseAlignments(separatorLine) {
    return splitCells(separatorLine).map((cell) => {
        const left = cell.startsWith(':');
        const right = cell.endsWith(':');
        if (left && right) return 'center';
        if (right) return 'right';
        return null; // default (left)
    });
}

function safeUrl(url = '') {
    const value = String(url).trim();
    if (/^(https?:|data:image\/)/i.test(value)) return value;
    if (value.startsWith('/')) return value;
    return '';
}

function renderMath(tex, displayMode) {
    try {
        return katex.renderToString(tex, {
            displayMode,
            throwOnError: false,
            strict: 'ignore',
        });
    } catch {
        return displayMode
            ? `<pre class="tm-math-err">${escapeHtml(tex)}</pre>`
            : `<code>${escapeHtml(tex)}</code>`;
    }
}

function renderTableCell(cell) {
    return escapeHtml(cell)
        .replace(/`([^`\n]+)`/g, '<code>$1</code>')
        .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
        .replace(/(^|[\s(])\*([^*\n]+)\*(?=[\s).,!?]|$)/g, '$1<em>$2</em>');
}

function tableToHtml({ header, alignments, rows }) {
    const alignAttr = (c) =>
        alignments[c] ? ` style="text-align:${alignments[c]}"` : '';

    const thead =
        '<thead><tr>' +
        header.map((cell, c) => `<th${alignAttr(c)}>${renderTableCell(cell)}</th>`).join('') +
        '</tr></thead>';

    const tbody =
        '<tbody>' +
        rows.map((row) => {
            const cells = row.slice(0, header.length);
            while (cells.length < header.length) cells.push('');
            return (
                '<tr>' +
                cells.map((cell, c) => `<td${alignAttr(c)}>${renderTableCell(cell)}</td>`).join('') +
                '</tr>'
            );
        }).join('') +
        '</tbody>';

    return `<table class="tm-md-table">${thead}${tbody}</table>`;
}

// ---------- List helpers ----------

// Matches "- item", "* item", "1. item", "1) item" at any indentation.
const LIST_ITEM_RE = /^(\s*)(?:([-*])|(\d+)[.)])\s+(.+)$/;

/**
 * Renders one contiguous block of list lines into nested <ol>/<ul>.
 * Indentation drives nesting; numbered items keep their own numbers
 * via <li value="N">, so numbering never resets to 1 mid-list.
 */
function renderListBlock(blockLines) {
    let html = '';
    const stack = []; // { indent, type }

    const closeLevel = () => {
        const level = stack.pop();
        html += `</li></${level.type}>`;
    };

    for (const line of blockLines) {
        const m = line.match(LIST_ITEM_RE);
        const indent = m[1].length;
        const type = m[3] !== undefined ? 'ol' : 'ul';
        const valueAttr = type === 'ol' ? ` value="${m[3]}"` : '';
        const content = m[4];

        if (!stack.length || indent > stack[stack.length - 1].indent) {
            // Deeper indent: open a nested list inside the current <li>
            html += `<${type}><li${valueAttr}>${content}`;
            stack.push({ indent, type });
            continue;
        }

        // Shallower indent: close nested levels until we're back at it
        while (stack.length > 1 && stack[stack.length - 1].indent > indent) {
            closeLevel();
        }

        // Same level but different list type (e.g. ol -> ul): swap lists
        if (stack[stack.length - 1].type !== type) {
            closeLevel();
            html += `<${type}><li${valueAttr}>${content}`;
            stack.push({ indent, type });
        } else {
            html += `</li><li${valueAttr}>${content}`;
        }
    }

    while (stack.length) closeLevel();
    return html;
}

/**
 * Walks the text line by line, groups list blocks (allowing blank lines
 * between items as long as the list resumes), and renders them with
 * nesting support. Blocks are padded with blank lines so the paragraph
 * splitter isolates them.
 */
function convertLists(text) {
    const lines = text.split('\n');
    const out = [];
    let i = 0;

    while (i < lines.length) {
        if (!LIST_ITEM_RE.test(lines[i])) {
            out.push(lines[i]);
            i++;
            continue;
        }

        const block = [];
        while (i < lines.length) {
            if (LIST_ITEM_RE.test(lines[i])) {
                block.push(lines[i]);
                i++;
                continue;
            }
            if (!lines[i].trim()) {
                // Blank line: only ends the list if it doesn't resume after
                let j = i;
                while (j < lines.length && !lines[j].trim()) j++;
                if (j < lines.length && LIST_ITEM_RE.test(lines[j])) {
                    i = j;
                    continue;
                }
            }
            break;
        }

        out.push('', renderListBlock(block), '');
    }

    return out.join('\n');
}


export function markdownMathToHtml(markdown = '') {
    const mathParts = [];
    const images = [];
    const tables = [];
    const stashMath = (latex, displayMode) => {
        const id = mathParts.length;
        mathParts.push({ latex, displayMode });
        return `@@MATH_${id}@@`;
    };
    const stashImage = (alt, url) => {
        const id = images.length;
        images.push({ alt, url });
        return `@@IMAGE_${id}@@`;
    };

    let work = String(markdown)
        .replace(/!\[([^\]]*)\]\(([^)\s]+)\)/g, (_, alt, url) => stashImage(alt, url))
        .replace(/\$\$([\s\S]+?)\$\$/g, (_, tex) => stashMath(tex.trim(), true))
        .replace(/\\\[([\s\S]+?)\\\]/g, (_, tex) => stashMath(tex.trim(), true))
        .replace(INLINE_MATH_RE, (_, tex, padded) => stashMath(tex ?? padded, false))
        .replace(/\\\(([^\n]+?)\\\)/g, (_, tex) => stashMath(tex, false));

    // Stash tables AFTER math/images (so cells can contain @@MATH_n@@ tokens)
    // and BEFORE escaping. The token is padded with blank lines so the
    // paragraph splitter isolates it into its own part.
    {
        const lines = work.split('\n');
        const out = [];
        for (let i = 0; i < lines.length; i++) {
            if (
                isTableLine(lines[i]) &&
                i + 1 < lines.length &&
                isSeparatorLine(lines[i + 1])
            ) {
                const header = splitCells(lines[i]);
                const alignments = parseAlignments(lines[i + 1]);
                const rows = [];
                let j = i + 2;
                while (j < lines.length && isTableLine(lines[j])) {
                    rows.push(splitCells(lines[j]));
                    j++;
                }
                const id = tables.length;
                tables.push({ header, alignments, rows });
                out.push('', `@@TABLE_${id}@@`, '');
                i = j - 1;
            } else {
                out.push(lines[i]);
            }
        }
        work = out.join('\n');
    }

    let html = escapeHtml(work)
        .replace(/^######\s+(.+)$/gm, '<h6>$1</h6>')
        .replace(/^#####\s+(.+)$/gm, '<h5>$1</h5>')
        .replace(/^####\s+(.+)$/gm, '<h4>$1</h4>')
        .replace(/^###\s+(.+)$/gm, '<h3>$1</h3>')
        .replace(/^##\s+(.+)$/gm, '<h2>$1</h2>')
        .replace(/^#\s+(.+)$/gm, '<h1>$1</h1>')
        .replace(/`([^`\n]+)`/g, '<code>$1</code>')
        .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
        .replace(/(^|[\s(])\*([^*\n]+)\*(?=[\s).,!?]|$)/g, '$1<em>$2</em>');

    // Nested-aware list conversion (replaces the old flat regex passes)
    html = convertLists(html);

    html = html.split(/\n{2,}/).map((part) => {
        if (/^\s*<(h\d|ul|ol|table|pre|img|div)/.test(part)) return part;
        return `<p>${part.replace(/\n/g, '<br/>')}</p>`;
    }).join('\n');

    // Restore tables BEFORE math/images so tokens inside cells get rendered
    // by the passes below. Also unwrap <p>@@TABLE_n@@</p> — a <table> inside
    // a <p> is invalid HTML and browsers eject it.
    html = html.replace(/(?:<p>\s*)?@@TABLE_(\d+)@@(?:\s*<\/p>)?/g, (_, n) => {
        const item = tables[Number(n)];
        return item ? tableToHtml(item) : '';
    });

    html = html.replace(/@@MATH_(\d+)@@/g, (_, n) => {
        const item = mathParts[Number(n)];
        if (!item) return '';
        const rendered = renderMath(item.latex, item.displayMode);
        return item.displayMode ? `<div class="tm-math-block">${rendered}</div>` : rendered;
    });

    html = html.replace(/@@IMAGE_(\d+)@@/g, (_, n) => {
        const item = images[Number(n)];
        if (!item) return '';
        const src = safeUrl(item.url);
        if (!src) return '';
        return `<img src="${escapeHtml(src)}" alt="${escapeHtml(item.alt)}" class="tm-md-img" loading="lazy" />`;
    });

    // Drop empty paragraphs produced by the padding blank lines
    html = html.replace(/<p>\s*<\/p>/g, '');

    return html;
}
export default function MarkdownMath({ children, className = '' }) {
    const html =  React.useMemo(() => markdownMathToHtml(children || ''), [children]);
    return (
        <div
            className={`markdown-math ${className}`.trim()}
            dangerouslySetInnerHTML={{ __html: html }}
        />
    );
}
