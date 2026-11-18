/**
 * Export utilities — convert AI tool output into downloadable files.
 *   - exportPDF(htmlNode, filename)              — visual fidelity via html2canvas + jsPDF
 *   - exportPPTX(markdown, filename, title, t)   — converts markdown into slide deck
 *   - exportWorksheet(markdown, filename, title, t) — student-facing worksheet (PDF)
 *
 * NOTE: Functions that render user-visible text require a `t` parameter (i18next TFunction).
 * Pass it from the calling React component via useTranslation().
 */

import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import {hasLatex, escapeHtml, markdownWithMathToHtml, addWrappedText, stripEditorMetadata,
    stripSlideNumberLabels, markdownToPlainLines, sanitizePdfText, latexToReadable,
    stripLayoutLatex, documentHeaderTitle} from './general-functions-exporters';

const NAVY  = '#27466C';
const TEAL  = '#3DA8A8';
const AMBER = '#E89B1E';
const MATHJAX_SRC = 'https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-svg.js';

let _mathJaxReady = null;
/** Loads MathJax v3 (tex-svg) once and resolves when it's ready to typeset. */
function ensureMathJax() {
    if (_mathJaxReady) return _mathJaxReady;
    _mathJaxReady = new Promise((resolve, reject) => {
        if (window.MathJax?.typesetPromise) return resolve(window.MathJax);
        window.MathJax = {
            tex: {
                inlineMath: [['$', '$'], ['\\(', '\\)']],
                displayMath: [['$$', '$$'], ['\\[', '\\]']],
            },
            // 'none' inlines every glyph path into its own container. A shared/global
            // cache uses <use> refs pointing OUTSIDE the node html2canvas captures, so
            // the math would rasterize blank. This is the key fix for the raster path.
            svg: { fontCache: 'none' },
            startup: { typeset: false },
        };
        const el = document.createElement('script');
        el.src = MATHJAX_SRC;
        el.async = true;
        el.onload = () =>
            window.MathJax.startup.promise.then(() => resolve(window.MathJax)).catch(reject);
        el.onerror = () => reject(new Error('MathJax failed to load'));
        document.head.appendChild(el);
    });
    return _mathJaxReady;
}

/**
 * @param {string} markdown
 * @param {string} filename
 * @param {string} title
 * @param {import('i18next').TFunction} t - i18next TFunction
 */
// ---------------------------------------------------------------------------
// Shared configuration
// ---------------------------------------------------------------------------

const LETTER_DOC_OPTS = { orientation: 'p', unit: 'mm', format: 'letter' };

// Brand palette as RGB tuples for jsPDF. These mirror the NAVY / TEAL hex
// strings used in the HTML/CSS path below (kept in one place instead of being
// re-typed as magic numbers throughout the drawing code).
const COLORS = {
    navy:   [39, 70, 108],   // #27466C — title, headings, table header fill
    teal:   [61, 168, 168],  // #3DA8A8 — accent rule under the title
    text:   [26, 39, 64],    // #1A2740 — body text
    white:  [255, 255, 255],
    zebra:  [243, 246, 249], // even-row table shading
    border: [190, 200, 210], // table cell borders
};

/** Create a US-Letter portrait jsPDF doc and return it with its page size. */
function createLetterDoc() {
    const doc = new jsPDF(LETTER_DOC_OPTS);
    return {
        doc,
        pageW: doc.internal.pageSize.getWidth(),
        pageH: doc.internal.pageSize.getHeight(),
    };
}

/** Slice a full-width, arbitrarily-tall image across as many pages as needed. */
function addTallImagePaged(pdf, imgData, imgW, imgH, pageH) {
    let offsetY = 0;
    let remaining = imgH;
    while (remaining > 0) {
        pdf.addImage(imgData, 'PNG', 0, -offsetY, imgW, imgH);
        remaining -= pageH;
        if (remaining > 0) {
            pdf.addPage();
            offsetY += pageH;
        }
    }
}

// Loads an image as JPEG with its size, so jsPDF accepts any format
const loadPdfImage = async (src) => {
    const blob = await (await fetch(src)).blob();
    const url = URL.createObjectURL(blob);
    try {
        const img = await new Promise((resolve, reject) => {
            const el = new Image();
            el.onload = () => resolve(el);
            el.onerror = reject;
            el.src = url;
        });
        const canvas = document.createElement('canvas');
        canvas.width = img.naturalWidth;
        canvas.height = img.naturalHeight;
        const ctx = canvas.getContext('2d');
        ctx.fillStyle = '#fff';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0);
        return { data: canvas.toDataURL('image/jpeg', 0.9), width: canvas.width, height: canvas.height };
    } finally {
        URL.revokeObjectURL(url);
    }
}

/** Stamp a centered "page X of Y" footer on every page. */
function stampPageNumbers(doc, pageW, pageH, t) {
    const total = doc.internal.getNumberOfPages();
    for (let i = 1; i <= total; i++) {
        doc.setPage(i);
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(8.5);
        doc.setTextColor(130);
        doc.text(
            t('export-utils:pdf.pageOf', { page: i, total }),
            pageW / 2,
            pageH - 8,
            { align: 'center' }
        );
    }
}

// ---------------------------------------------------------------------------
// Main entry point
// ---------------------------------------------------------------------------

export async function exportMarkdownPDF(markdown, filename = 'document.pdf', title = '', t) {

    markdown = stripEditorMetadata(markdown);   // add this line

    if (hasLatex(markdown)) {
        return exportMarkdownScientificPDF(markdown, filename, title, t);
    }

    const { doc, pageW, pageH } = createLetterDoc();
    const marginX = 18;
    const marginTop = 20;
    const marginBottom = 16;
    const maxWidth = pageW - marginX * 2;
    let y = marginTop;

    function ensureSpace(needed = 10) {
        if (y + needed <= pageH - marginBottom) return;
        doc.addPage();
        y = marginTop;
    }

    // ---------- Table helpers ----------

    const isTableLine = (line) => /^\s*\|.*\|\s*$/.test(line);
    const isSeparatorLine = (line) => /^\s*\|?\s*:?-{3,}:?\s*(\|\s*:?-{3,}:?\s*)*\|?\s*$/.test(line);

    function parseCells(line) {
        return line
            .trim()
            .replace(/^\|/, '')
            .replace(/\|$/, '')
            .split('|')
            .map((c) => latexToReadable(c.trim()));
    }

    function drawTable(header, body) {
        const fontSize = 9.5;
        const lineH = 4.3;
        const cellPad = 1.8;
        const cols = header.length;

        // Normalize row lengths to the header's column count
        const rows = body.map((r) => {
            const copy = r.slice(0, cols);
            while (copy.length < cols) copy.push('');
            return copy;
        });

        // Measure natural column widths
        doc.setFontSize(fontSize);
        const natural = header.map((h, c) => {
            doc.setFont('helvetica', 'bold');
            let w = doc.getTextWidth(h);
            doc.setFont('helvetica', 'normal');
            for (const r of rows) w = Math.max(w, doc.getTextWidth(r[c]));
            return w + cellPad * 2 + 1;
        });

        // Scale widths to fit the page
        const totalNatural = natural.reduce((a, b) => a + b, 0);
        let widths;
        if (totalNatural <= maxWidth) {
            const extra = maxWidth - totalNatural;
            widths = natural.map((w) => w + extra * (w / totalNatural));
        } else {
            widths = natural.map((w) => Math.max(14, (w / totalNatural) * maxWidth));
            const sum = widths.reduce((a, b) => a + b, 0);
            widths = widths.map((w) => (w / sum) * maxWidth);
        }

        function measureRow(cells, bold) {
            doc.setFont('helvetica', bold ? 'bold' : 'normal');
            doc.setFontSize(fontSize);
            const wrapped = cells.map((cell, c) =>
                doc.splitTextToSize(cell, widths[c] - cellPad * 2)
            );
            const lines = Math.max(1, ...wrapped.map((l) => l.length));
            return { wrapped, height: lines * lineH + cellPad * 2 };
        }

        function drawRow({ wrapped, height }, opts) {
            const { bold = false, fill = null, textColor = COLORS.text } = opts;
            let x = marginX;
            doc.setFont('helvetica', bold ? 'bold' : 'normal');
            doc.setFontSize(fontSize);
            doc.setDrawColor(...COLORS.border);
            for (let c = 0; c < wrapped.length; c++) {
                if (fill) {
                    doc.setFillColor(...fill);
                    doc.rect(x, y, widths[c], height, 'FD');
                } else {
                    doc.rect(x, y, widths[c], height);
                }
                doc.setTextColor(...textColor);
                doc.text(wrapped[c], x + cellPad, y + cellPad + lineH - 1.1);
                x += widths[c];
            }
            y += height;
        }

        // Header layout is position-independent, so measure it once and reuse
        // across every page instead of re-measuring on each draw.
        const headerRow = measureRow(header, true);
        const drawHeader = () =>
            drawRow(headerRow, { bold: true, fill: COLORS.navy, textColor: COLORS.white });

        ensureSpace(headerRow.height + lineH + cellPad * 2);
        drawHeader();

        rows.forEach((r, i) => {
            const m = measureRow(r, false);
            if (y + m.height > pageH - marginBottom) {
                doc.addPage();
                y = marginTop;
                drawHeader(); // repeat header on the new page
            }
            drawRow(m, { fill: i % 2 === 1 ? COLORS.zebra : null });
        });

        y += 4;
    }

    // ---------- Title ----------

    const headerTitle = documentHeaderTitle(markdown, title);
    if (headerTitle) {
        doc.setFont('helvetica', 'bold');
        doc.setFontSize(16);
        doc.setTextColor(...COLORS.navy);
        y = addWrappedText(doc, latexToReadable(headerTitle), marginX, y, maxWidth, 7) + 3;
        doc.setDrawColor(...COLORS.teal);
        doc.line(marginX, y, pageW - marginX, y);
        y += 8;
    }

    // ---------- Body ----------

    // Swap each image for a numbered placeholder line and load them all
    const sources = [];
    const withImages = markdown.replace(/!\[[^\]]*\]\(\s*([^)\s]+)(?:\s+"[^"]*")?\s*\)/g, (_, src) => `\n@@PDFIMG${sources.push(src) - 1}@@\n`);
    const images = await Promise.all(sources.map((src) => loadPdfImage(src).catch(() => null)));

    // Draws the image centered, scaled to fit the page width and half its height
    function drawImage(image) {
        if (!image) return;
        const maxHeight = (pageH - marginTop - marginBottom) * 0.5;
        let w = Math.min(maxWidth, image.width * 25.4 / 96);
        let h = (image.height * w) / image.width;
        if (h > maxHeight) {
            w *= maxHeight / h;
            h = maxHeight;
        }
        ensureSpace(h + 4);
        doc.addImage(image.data, 'JPEG', (pageW - w) / 2, y, w, h);
        y += h + 4;
    }

    const lines = markdownToPlainLines(withImages);

    for (let i = 0; i < lines.length; i++) {
        const line = lines[i].trimEnd();
        if (!line.trim()) { y += 3; continue; }

        const image = line.trim().match(/^@@PDFIMG(\d+)@@$/);
        if (image) {
            drawImage(images[Number(image[1])]);
            continue;
        }

        // Table block: current line + next line (separator) mark a table start
        if (isTableLine(line) && i + 1 < lines.length && isSeparatorLine(lines[i + 1])) {
            const header = parseCells(line);
            const body = [];
            let j = i + 2;
            while (j < lines.length && isTableLine(lines[j])) {
                body.push(parseCells(lines[j]));
                j++;
            }
            drawTable(header, body);
            i = j - 1; // skip the consumed lines
            continue;
        }

        const heading  = line.match(/^(#{1,6})\s+(.+)/);
        const bullet   = line.match(/^\s*[-*]\s+(.+)/);
        const numbered = line.match(/^\s*(\d+)[.)]\s+(.+)/);

        ensureSpace(12);
        if (heading) {
            const level = heading[1].length;
            doc.setFont('helvetica', 'bold');
            doc.setFontSize(level === 1 ? 15 : level === 2 ? 13 : 11.5);
            doc.setTextColor(...COLORS.navy);
            y += level === 1 ? 3 : 1;
            y = addWrappedText(doc, latexToReadable(heading[2]), marginX, y, maxWidth, 6.5) + 2;
            continue;
        }

        doc.setFont('helvetica', bullet || numbered ? 'bold' : 'normal');
        doc.setFontSize(10.5);
        doc.setTextColor(...COLORS.text);
        const text = bullet ? `- ${bullet[1]}` : numbered ? `${numbered[1]}. ${numbered[2]}` : line;
        y = addWrappedText(doc, latexToReadable(text), marginX, y, maxWidth, 5.5) + 1.5;
    }

    // ---------- Page numbers ----------

   // stampPageNumbers(doc, pageW, pageH, t);
    doc.save(filename);
}


/**
 * Exports markup (Markdown + optional LaTeX) to a PDF, fully client-side.
 * MathJax typesets the math, html2canvas rasterizes, jsPDF saves — silent download,
 * no dialog, no backend. Same signature as before.
 *
 * @param {string} markdown
 * @param {string} filename
 * @param {string} title
 * @param {Function} t   i18n translator (kept for signature parity)
 * @returns {Promise<void>}
 */
export async function exportMarkdownScientificPDF(markdown, filename = 'document.pdf', title = '', t) {
    const wrapper = document.createElement('div');
    wrapper.style.cssText = `
        position: fixed; left: -10000px; top: 0;
        width: 760px; padding: 32px 40px; box-sizing: border-box;
        background: #fff; color: #1A2740;
        font-family: 'Poppins', system-ui, sans-serif; font-size: 13px; line-height: 1.6;
    `;
    wrapper.innerHTML = `
        <style>
            .mdx h1,.mdx h2,.mdx h3,.mdx h4 { color: ${NAVY}; line-height: 1.25; margin: 1.3em 0 .5em; font-weight: 600; }
            .mdx h1 { font-size: 22px; border-bottom: 2px solid ${TEAL}; padding-bottom: 6px; }
            .mdx h2 { font-size: 18px; } .mdx h3 { font-size: 15px; }
            .mdx p { margin: 0 0 .7em; }
            .mdx ul,.mdx ol { margin: 0 0 .8em; padding-left: 1.4em; }
            .mdx li { margin: .2em 0; }
            .mdx code { background: #f3f4f6; padding: .1em .35em; border-radius: 4px; font-family: Consolas, monospace; font-size: .92em; }
            .mdx pre { background: #f6f8fa; padding: 12px 14px; border-radius: 8px; overflow: hidden; }
            .mdx pre code { background: none; padding: 0; }
            .mdx blockquote { margin: 0 0 1em; padding: .2em 1em; color: #555; border-left: 4px solid #d0d7de; }
            .mdx table { border-collapse: collapse; width: 100%; margin: 1em 0; }
            .mdx th,.mdx td { border: 1px solid #cbd3dd; padding: 7px 9px; text-align: left; font-size: 12px; }
            .mdx th { background: ${NAVY}; color: #fff; }
            .mdx tr:nth-child(even) td { background: #f3f6f9; }
        </style>
        <div class="mdx">
            ${documentHeaderTitle(markdown, title) ? `<h1>${escapeHtml(title)}</h1>` : ''}
            ${markdownWithMathToHtml(markdown)}
        </div>
    `;
    document.body.appendChild(wrapper);

    try {
        // Typeset LaTeX. If it fails (offline / no math), text still renders — don't block the export.
        try {
            const MJ = await ensureMathJax();
            await MJ.typesetPromise([wrapper]);
        } catch (e) {
            console.warn('exportMarkdownPDF: math typesetting skipped —', e?.message);
        }

        // Remove the invisible screen-reader MathML MathJax injects — html2canvas
        // can't honor its clip rule and would paint it over the visible formula,
        // producing the doubled/offset text.
        wrapper.querySelectorAll('mjx-assistive-mml').forEach((n) => n.remove());

        // Wait for web fonts so plain text doesn't get a font-swap ghost either.
        if (document.fonts?.ready) { try { await document.fonts.ready; } catch {} }

        // Let the browser lay out the freshly-inserted SVG before capturing.
        await new Promise((r) => requestAnimationFrame(() => requestAnimationFrame(r)));

        const canvas = await html2canvas(wrapper, { scale: 2, backgroundColor: '#ffffff', useCORS: true });
        const imgData = canvas.toDataURL('image/png');

        const { doc: pdf, pageW, pageH } = createLetterDoc();
        const imgW = pageW;
        const imgH = (canvas.height * imgW) / canvas.width;

        addTallImagePaged(pdf, imgData, imgW, imgH, pageH);

        pdf.save(filename); // silent download
    } finally {
        wrapper.remove();
    }
}

/** Builds the worksheet body HTML: list items become numbered questions with ruled answer lines. */
function worksheetBodyToHtml(markdown) {
    const isTableLine = (l) => /^\s*\|.*\|\s*$/.test(l);
    const isSeparatorLine = (l) => /^\s*\|?\s*:?-{3,}:?\s*(\|\s*:?-{3,}:?\s*)*\|?\s*$/.test(l);

    // markdownWithMathToHtml wraps a single-line fragment in <p>…</p>; unwrap it for inline use
    // while leaving math delimiters intact for MathJax to typeset later.
    const inlineMd = (s) =>
        markdownWithMathToHtml(String(s)).trim().replace(/^<p>/, '').replace(/<\/p>\s*$/, '');

    const answerLines =
        `<div class="ws-answers"><div class="ws-line"></div><div class="ws-line"></div><div class="ws-line"></div></div>`;

    const cleaned = stripEditorMetadata(stripSlideNumberLabels(markdown))
        .replace(/```[\s\S]*?```/g, '') // drop fenced code blocks, same as the jsPDF path
        .replace(/\r/g, '')
        .split('\n');

    const out = [];
    let questionNum = 0;

    for (let i = 0; i < cleaned.length; i++) {
        const line = cleaned[i].trim();
        if (!line) continue;

        // Table block: pipe line followed by a |---|---| separator
        if (isTableLine(line) && i + 1 < cleaned.length && isSeparatorLine(cleaned[i + 1])) {
            const tableLines = [line, cleaned[i + 1]];
            let j = i + 2;
            while (j < cleaned.length && isTableLine(cleaned[j])) tableLines.push(cleaned[j++]);
            out.push(`<div class="ws-table">${markdownWithMathToHtml(tableLines.join('\n'))}</div>`);
            i = j - 1;
            continue;
        }

        const heading = line.match(/^(#{1,6})\s+(.+)/);
        if (heading) {
            const level = Math.min(heading[1].length, 3);
            out.push(`<h${level} class="ws-h">${inlineMd(heading[2])}</h${level}>`);
            continue;
        }

        const numbered = line.match(/^\d+[.)]\s+(.+)/);
        const bullet = line.match(/^[-*]\s+(.+)/);
        if (numbered || bullet) {
            questionNum++;
            out.push(
                `<div class="ws-q"><span class="ws-q-num">${questionNum}.</span>` +
                `<span class="ws-q-text">${inlineMd(numbered ? numbered[1] : bullet[1])}</span></div>` +
                answerLines
            );
            continue;
        }

        out.push(`<p class="ws-p">${inlineMd(line)}</p>`);
    }

    return out.join('\n');
}

export async function exportWorksheetScientific(markdown, filename = 'worksheet.pdf', title = '', t) {
    const wrapper = document.createElement('div');
    wrapper.style.cssText = `
        position: fixed; left: -10000px; top: 0;
        width: 760px; box-sizing: border-box;
        background: #fff; color: #282828;
        font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.5;
    `;

    wrapper.innerHTML = `
        <style>
            .ws-band { background:#27466C; color:#fff; padding:16px 40px 14px; }
            .ws-title { font-size:22px; font-weight:700; line-height:1.2; }
            .ws-byline { font-size:12px; margin-top:4px; opacity:.92; }
            .ws-body { padding:20px 40px 40px; }
            .ws-inforow { display:flex; justify-content:space-between; font-size:14px;
                          border-bottom:1px solid #dcdcdc; padding-bottom:10px; margin-bottom:6px; }
            .ws-inforow .ws-fill { display:inline-block; border-bottom:1px solid #b4b4b4;
                                   width:180px; margin-left:6px; }
            .ws-inforow .ws-fill.sm { width:120px; }
            .ws-h { color:#27466C; font-weight:700; margin:18px 0 8px; line-height:1.25; }
            h1.ws-h{font-size:19px;} h2.ws-h{font-size:16px;} h3.ws-h{font-size:14px;}
            .ws-p { margin:6px 0; }
            .ws-q { display:flex; gap:8px; margin:16px 0 4px; }
            .ws-q-num { font-weight:700; color:#27466C; min-width:22px; }
            .ws-q-text { flex:1; }
            .ws-answers { margin:6px 0 14px; padding-left:30px; }
            .ws-line { border-bottom:1px solid #c8c8c8; height:26px; }
            .ws-table table { border-collapse:collapse; width:100%; margin:10px 0 16px; font-size:13px; }
            .ws-table th, .ws-table td { border:1px solid #bec8d2; padding:6px 8px; text-align:left; }
            .ws-table th { background:#27466C; color:#fff; font-weight:700; }
            .ws-table tr:nth-child(even) td { background:#f3f6f9; }
            mjx-container { color: inherit; }
        </style>
        <div class="ws">
            <div class="ws-band">
                <div class="ws-title">${escapeHtml(latexToReadable(title))}</div>
                <div class="ws-byline">${escapeHtml(t('export-utils:worksheet.headerByline'))}</div>
            </div>
            <div class="ws-body">
                <div class="ws-inforow">
                    <span>${escapeHtml(t('export-utils:worksheet.nameLabel'))}<span class="ws-fill"></span></span>
                    <span>${escapeHtml(t('export-utils:worksheet.dateLabel'))}<span class="ws-fill sm"></span></span>
                </div>
                ${worksheetBodyToHtml(markdown)}
            </div>
        </div>
    `;
    document.body.appendChild(wrapper);

    try {
        try {
            const MJ = await ensureMathJax();
            await MJ.typesetPromise([wrapper]);
        } catch (e) {
            console.warn('exportWorksheet: math typesetting skipped —', e?.message);
        }

        wrapper.querySelectorAll('mjx-assistive-mml').forEach((n) => n.remove());
        flattenResidualLatex(wrapper);
        if (document.fonts?.ready) { try { await document.fonts.ready; } catch {} }
        await new Promise((r) => requestAnimationFrame(() => requestAnimationFrame(r)));

        const canvas = await html2canvas(wrapper, { scale: 2, backgroundColor: '#ffffff', useCORS: true });
        const imgData = canvas.toDataURL('image/png');

        const { doc: pdf, pageW, pageH } = createLetterDoc();
        const imgW = pageW;
        const imgH = (canvas.height * imgW) / canvas.width;
        addTallImagePaged(pdf, imgData, imgW, imgH, pageH);

        pdf.save(filename);
    } finally {
        wrapper.remove();
    }
}

/**
 * Convert any bare LaTeX that MathJax didn't typeset (because it wasn't wrapped
 * in math delimiters) into readable text. Runs AFTER typesetting and skips the
 * <mjx-container> nodes MathJax produced, so real formulas are left untouched.
 */
function flattenResidualLatex(root) {
    const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT, {
        acceptNode(node) {
            if (node.parentElement?.closest('mjx-container')) return NodeFilter.FILTER_REJECT; // MathJax output
            return /\\[a-zA-Z]+|\$/.test(node.nodeValue)
                ? NodeFilter.FILTER_ACCEPT
                : NodeFilter.FILTER_SKIP;
        },
    });

    const targets = [];
    for (let n = walker.nextNode(); n; n = walker.nextNode()) targets.push(n);
    for (const node of targets) node.nodeValue = latexToReadable(stripLayoutLatex(node.nodeValue));
}

/**
 * @param {string}   markdown
 * @param {string}   filename
 * @param {string}   title
 * @param {import('i18next').TFunction} t - i18next TFunction
 */
export function exportWorksheet(markdown, filename = 'worksheet.pdf', title = '', t) {

    if (hasLatex(markdown)) {
        return exportWorksheetScientific(markdown, filename, title, t);
    }

    const doc    = new jsPDF({ orientation: 'p', unit: 'mm', format: 'letter' });
    const pageW  = doc.internal.pageSize.getWidth();
    const pageH  = doc.internal.pageSize.getHeight();
    const marginX   = 18;
    const marginTop = 38;
    const maxWidth  = pageW - 2 * marginX;
    let y = marginTop;

    // Header band
    doc.setFillColor(39, 70, 108);
    doc.rect(0, 0, pageW, 26, 'F');
    doc.setTextColor(255, 255, 255);
    doc.setFont('helvetica', 'bold');
    doc.setFontSize(16);
    doc.text(sanitizePdfText(latexToReadable(title)), marginX, 14);
    doc.setFont('helvetica', 'normal');
    doc.setFontSize(10);
    doc.text(t('export-utils:worksheet.headerByline'), marginX, 21);

    // Student info row
    doc.setTextColor(40, 40, 40);
    doc.setFontSize(11);
    doc.text(t('export-utils:worksheet.nameLabel'), marginX, 34);
    doc.text(t('export-utils:worksheet.dateLabel'), pageW - marginX - 50, 34);
    doc.setDrawColor(220);
    doc.line(marginX, 38, pageW - marginX, 38);

    // ---------- Table helpers ----------

    const isTableLine = (line) => /^\s*\|.*\|\s*$/.test(line);
    const isSeparatorLine = (line) => /^\s*\|?\s*:?-{3,}:?\s*(\|\s*:?-{3,}:?\s*)*\|?\s*$/.test(line);

    function parseCells(line) {
        return line
            .trim()
            .replace(/^\|/, '')
            .replace(/\|$/, '')
            .split('|')
            .map((c) => sanitizePdfText(c.trim()));
    }

    function drawTable(header, body) {
        const fontSize = 9.5;
        const lineH = 4.3;
        const cellPad = 1.8;
        const cols = header.length;

        // Normalize row lengths to the header's column count
        const rows = body.map((r) => {
            const copy = r.slice(0, cols);
            while (copy.length < cols) copy.push('');
            return copy;
        });

        // Measure natural column widths
        doc.setFontSize(fontSize);
        const natural = header.map((h, c) => {
            doc.setFont('helvetica', 'bold');
            let w = doc.getTextWidth(h);
            doc.setFont('helvetica', 'normal');
            for (const r of rows) w = Math.max(w, doc.getTextWidth(r[c]));
            return w + cellPad * 2 + 1;
        });

        // Scale widths to fit the page
        const totalNatural = natural.reduce((a, b) => a + b, 0);
        let widths;
        if (totalNatural <= maxWidth) {
            const extra = maxWidth - totalNatural;
            widths = natural.map((w) => w + extra * (w / totalNatural));
        } else {
            widths = natural.map((w) => Math.max(14, (w / totalNatural) * maxWidth));
            const sum = widths.reduce((a, b) => a + b, 0);
            widths = widths.map((w) => (w / sum) * maxWidth);
        }

        function measureRow(cells, bold) {
            doc.setFont('helvetica', bold ? 'bold' : 'normal');
            doc.setFontSize(fontSize);
            const wrapped = cells.map((cell, c) =>
                doc.splitTextToSize(cell, widths[c] - cellPad * 2)
            );
            const lines = Math.max(1, ...wrapped.map((l) => l.length));
            return { wrapped, height: lines * lineH + cellPad * 2 };
        }

        function drawRow({ wrapped, height }, opts) {
            const { bold = false, fill = null, textColor = [40, 40, 40] } = opts;
            let x = marginX;
            doc.setFont('helvetica', bold ? 'bold' : 'normal');
            doc.setFontSize(fontSize);
            doc.setDrawColor(190, 200, 210);
            for (let c = 0; c < wrapped.length; c++) {
                if (fill) {
                    doc.setFillColor(...fill);
                    doc.rect(x, y, widths[c], height, 'FD');
                } else {
                    doc.rect(x, y, widths[c], height);
                }
                doc.setTextColor(...textColor);
                doc.text(wrapped[c], x + cellPad, y + cellPad + lineH - 1.1);
                x += widths[c];
            }
            y += height;
        }

        function drawHeader() {
            const h = measureRow(header, true);
            drawRow(h, { bold: true, fill: [39, 70, 108], textColor: [255, 255, 255] });
        }

        if (y + measureRow(header, true).height + lineH + cellPad * 2 > pageH - 24) {
            doc.addPage();
            y = marginTop;
        }
        drawHeader();

        rows.forEach((r, i) => {
            const m = measureRow(r, false);
            if (y + m.height > pageH - 24) {
                doc.addPage();
                y = marginTop;
                drawHeader(); // repeat header on the new page
            }
            drawRow(m, { fill: i % 2 === 1 ? [243, 246, 249] : null });
        });

        // Restore body text styles
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(11);
        doc.setTextColor(40, 40, 40);
        y += 4;
    }

    // Content — convert LaTeX to readable text, strip markdown, then convert into questions.
    // latexToReadable handles all math delimiters ($…$, $$…$$, \(…\), \[…\]) and commands
    // (\frac, \times, …), so no separate math-stripping passes are needed here.
    const cleaned = sanitizePdfText(latexToReadable(stripEditorMetadata(stripSlideNumberLabels(markdown))))
        .replace(/```[\s\S]*?```/g, '')
        .replace(/!\[[^\]]*\]\([^)]+\)/g, '')
        .replace(/\[([^\]]+)\]\([^)]+\)/g, '$1')
        .replace(/\*\*([^*]+)\*\*/g, '$1')
        .replace(/`([^`]+)`/g, '$1')
        .replace(/^>\s+/gm, '')
        .replace(/\r/g, '')
        .split('\n');

    doc.setFont('helvetica', 'normal');
    doc.setFontSize(11);
    let questionNum = 0;

    for (let i = 0; i < cleaned.length; i++) {
        const line = cleaned[i].trim();
        if (!line) { y += 3; continue; }
        if (y > pageH - 24) { doc.addPage(); y = marginTop; }

        // Table block: pipe line followed by a |---|---| separator
        if (isTableLine(line) && i + 1 < cleaned.length && isSeparatorLine(cleaned[i + 1])) {
            const header = parseCells(line);
            const body = [];
            let j = i + 2;
            while (j < cleaned.length && isTableLine(cleaned[j])) {
                body.push(parseCells(cleaned[j]));
                j++;
            }
            drawTable(header, body);
            i = j - 1; // skip the consumed lines
            continue;
        }

        const heading = line.match(/^(#+)\s+(.+)/);
        if (heading) {
            const level = heading[1].length;
            doc.setFont('helvetica', 'bold');
            doc.setFontSize(level === 1 ? 14 : 12);
            const wrapped = doc.splitTextToSize(sanitizePdfText(heading[2]), maxWidth);
            y += 4;
            doc.text(wrapped, marginX, y);
            y += wrapped.length * 6 + 2;
            doc.setFont('helvetica', 'normal');
            doc.setFontSize(11);
            continue;
        }

        const num    = line.match(/^\d+\.\s+(.+)/);
        const bullet = line.match(/^[-*]\s+(.+)/);
        const text   = sanitizePdfText(num ? num[1] : bullet ? bullet[1] : line);
        if (num || bullet) questionNum++;
        const prefix  = (num || bullet) ? `${questionNum}.  ` : '';
        const wrapped = doc.splitTextToSize(sanitizePdfText(prefix + text), maxWidth);
        doc.text(wrapped, marginX, y);
        y += wrapped.length * 5.6;

        // After each question, leave three ruled lines for the student's answer
        if (num || bullet) {
            for (let k = 0; k < 3; k++) {
                y += 7;
                if (y > pageH - 18) { doc.addPage(); y = marginTop; }
                doc.setDrawColor(200);
                doc.line(marginX + 10, y, pageW - marginX, y);
            }
            y += 6;
        } else {
            y += 1;
        }
    }

    // Footer page numbers
    const total = doc.internal.getNumberOfPages();
    for (let i = 1; i <= total; i++) {
        doc.setPage(i);
        doc.setFontSize(9);
        doc.setTextColor(140);
        doc.text(
            t('export-utils:pdf.pageOf', { page: i, total }),
            pageW / 2,
            pageH - 8,
            { align: 'center' }
        );
    }

    doc.save(filename);
}