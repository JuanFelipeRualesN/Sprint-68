import katex from 'katex';
import { marked } from 'marked';
import {
    AlignmentType, Document, ExternalHyperlink, HeadingLevel, ImageRun, Math as WordMath, MathAngledBrackets,
    MathCurlyBrackets, MathFraction, MathLimitLower, MathLimitUpper, MathRadical, MathRoundBrackets, MathRun,
    MathSquareBrackets, MathSubScript, MathSubSuperScript, MathSuperScript, Packer, Paragraph, ShadingType, Table,
    TableCell, TableRow, TextRun, WidthType,
} from 'docx';
import { stripEditorMetadata, latexToReadable } from './general-functions-exporters';
import { INLINE_MATH_RE } from '../tool-modal-utils';
import { WORD_STYLES, PAGE, CONTENT_WIDTH, MAX_IMAGE_WIDTH_PX, TABLE, listIndent, listLevels } from './exporters-word-styles';

// Builds a Word document from markdown, returning a 'docx' document object
export const buildWordDocument = async (markdown, title = '') => {
    const clean = stripEditorMetadata(String(markdown || ''));

    const equations = [];
    const stash = (tex, display) => `@@WMATH${equations.push({ tex: tex.trim(), display }) - 1}@@`;

    const withTokens = clean
        .replace(/\$\$([\s\S]+?)\$\$/g, (_, tex) => stash(tex, true))
        .replace(/\\\[([\s\S]+?)\\\]/g, (_, tex) => stash(tex, true))
        .replace(INLINE_MATH_RE, (_, tex, padded) => stash(tex ?? padded, false))
        .replace(/\\\(([^\n]+?)\\\)/g, (_, tex) => stash(tex, false));

    // Build a readable version of the markdown with the equations replaced by their LaTeX
    const readable = latexToReadable(withTokens.replace(/\$/g, '@@WDOLLAR@@')).replace(/@@WDOLLAR@@/g, '$');

    const tokens = marked.lexer(readable, { gfm: true });
    const ctx = {
        equations,
        images: await loadImages(tokens),
        numbering: [{ reference: 'bullets', levels: listLevels(false) }],
    };

    const children = [
        ...(title ? [new Paragraph({ style: 'DocTitle', text: title })] : []),
        ...tokens.flatMap((token) => blockToDocx(token, ctx)),
    ];

    const { width, height, margin } = PAGE;
    return new Document({
        title,
        styles: WORD_STYLES,
        numbering: { config: ctx.numbering },
        sections: [{
            properties: { page: { size: { width, height }, margin: { top: margin, right: margin, bottom: margin, left: margin } } },
            children: children.length ? children : [new Paragraph('')],
        }],
    });
};

// Paragraphs and tables for one block token
const blockToDocx = (token, ctx, first = {}, rest = first) => {
    switch (token.type) {
        case 'heading':
            return [new Paragraph({ heading: HeadingLevel[`HEADING_${token.depth}`], children: inline(inlineToRuns(token.tokens, ctx)) })];
        case 'paragraph':
        case 'text':
            return paragraphs(token.tokens ? inlineToRuns(token.tokens, ctx) : textRuns(token.text, ctx), first, rest);
        case 'list':
            return listToDocx(token, ctx);
        case 'table':
            return [tableToDocx(token, ctx)];
        case 'blockquote':
            return token.tokens.flatMap((child) => blockToDocx(child, ctx, { style: 'BlockQuote' }));
        case 'code':
            return [new Paragraph({
                ...first,
                style: 'CodeBlock',
                children: token.text.split('\n').map((line, i) => (i ? new TextRun({ text: line, break: 1 }) : new TextRun(line))),
            })];
        case 'hr':
            return [new Paragraph({ style: 'HorizontalRule' })];
        case 'html': {
            const text = decodeEntities(token.text.replace(/<[^>]+>/g, '')).trim();
            return text ? [new Paragraph({ ...first, children: [new TextRun(text)] })] : [];
        }
        default:
            return [];
    }
};

const listToDocx = (list, ctx, level = 0) => {
    let reference = 'bullets';
    if (list.ordered) {
        reference = `ordered-${ctx.numbering.length}`;
        ctx.numbering.push({ reference, levels: listLevels(true, level, list.start === '' ? 1 : list.start) });
    }
    const numbered = { style: 'ListParagraph', numbering: { reference, level } };
    const under = { style: 'ListParagraph', indent: { left: listIndent(level).left } };

    return list.items.flatMap((item) => itemBlocks(item).flatMap((token, i) => (token.type === 'list'
        ? listToDocx(token, ctx, level + 1)
        : blockToDocx(token, ctx, i === 0 ? numbered : under, under))));
};

const itemBlocks = (item) => {
    const [checkbox, text, ...others] = item.tokens;
    if (checkbox?.type !== 'checkbox') return item.tokens;
    return [{ type: 'paragraph', ...text, tokens: [checkbox, ...(text?.tokens ?? [])] }, ...others];
};

const ALIGNMENTS = { left: AlignmentType.LEFT, center: AlignmentType.CENTER, right: AlignmentType.RIGHT };

const tableToDocx = (token, ctx) => {
    const columnWidth = Math.floor(CONTENT_WIDTH / token.header.length);
    const cell = (content, column, header) => new TableCell({
        width: { size: columnWidth, type: WidthType.DXA },
        margins: TABLE.cellMargins,
        shading: header ? { type: ShadingType.CLEAR, fill: TABLE.headerFill, color: 'auto' } : undefined,
        children: [new Paragraph({
            style: header ? 'TableHeader' : 'TableText',
            alignment: ALIGNMENTS[token.align[column]],
            children: inline(inlineToRuns(content.tokens, ctx)),
        })],
    });
    const { border } = TABLE;

    return new Table({
        width: { size: CONTENT_WIDTH, type: WidthType.DXA },
        columnWidths: token.header.map(() => columnWidth),
        borders: { top: border, bottom: border, left: border, right: border, insideHorizontal: border, insideVertical: border },
        rows: [
            new TableRow({ tableHeader: true, children: token.header.map((content, i) => cell(content, i, true)) }),
            ...token.rows.map((row) => new TableRow({ children: row.map((content, i) => cell(content, i, false)) })),
        ],
    });
};

const inlineToRuns = (tokens = [], ctx, format = {}) => tokens.flatMap((token) => {
    switch (token.type) {
        case 'strong':
            return inlineToRuns(token.tokens, ctx, { ...format, bold: true });
        case 'em':
            return inlineToRuns(token.tokens, ctx, { ...format, italics: true });
        case 'del':
            return inlineToRuns(token.tokens, ctx, { ...format, strike: true });
        case 'codespan':
            return [new TextRun({ ...format, text: decodeEntities(token.text), style: 'InlineCode' })];
        case 'escape':
            return [new TextRun({ ...format, text: token.text })];
        case 'br':
            return [new TextRun({ break: 1 })];
        case 'checkbox':
            return [new TextRun({ ...format, text: token.checked ? '☑ ' : '☐ ' })];
        case 'link':
            if (!/^(https?:|mailto:)/i.test(token.href)) return inlineToRuns(token.tokens, ctx, format);
            return [new ExternalHyperlink({ link: token.href, children: inline(inlineToRuns(token.tokens, ctx, { ...format, style: 'Hyperlink' })) })];
        case 'image':
            return [imageRun(token, ctx, format)];
        case 'html':
            if (/^<br\s*\/?>$/i.test(token.text.trim())) return [new TextRun({ break: 1 })];
            return textRuns(token.text.replace(/<[^>]+>/g, ''), ctx, format);
        default:
            return token.tokens ? inlineToRuns(token.tokens, ctx, format) : textRuns(token.text ?? '', ctx, format);
    }
});

const textRuns = (text, ctx, format = {}) => {
    const parts = decodeEntities(text).split(/@@WMATH(\d+)@@/);
    const isDisplay = (i) => i > 0 && i < parts.length && ctx.equations[Number(parts[i])].display;

    return parts.flatMap((part, i) => {
        if (i % 2) return [equationRun(ctx.equations[Number(part)], format)];
        let s = part.replace(/\n/g, ' ');
        if (isDisplay(i - 1)) s = s.trimStart();
        if (isDisplay(i + 1)) s = s.trimEnd();
        return s ? [new TextRun({ ...format, text: s })] : [];
    });
};

// Splits runs into paragraphs, giving each display equation a centered paragraph of its own
const paragraphs = (runs, first = {}, rest = first) => {
    const out = [];
    let line = [];
    const push = (options) => out.push(new Paragraph({ ...(out.length ? rest : first), ...options }));
    const flush = () => {
        if (line.length) push({ children: line });
        line = [];
    };

    for (const run of runs) {
        if (!run.display) {
            line.push(run);
            continue;
        }
        flush();
        push({ alignment: AlignmentType.CENTER, children: [run.display] });
    }
    flush();
    return out;
};

// Where a paragraph can't be split,  display equations stay inline.
const inline = (runs) => runs.map((run) => run.display ?? run);

// An equation as native Word math, or as readable text when KaTeX can't parse the LaTeX
const equationRun = ({ tex, display }, format) => {
    let run;
    try {
        const mathMl = katex.renderToString(tex, { output: 'mathml', displayMode: display, throwOnError: true });
        run = new WordMath({ children: mathComponents(parseMathMl(mathMl).children) });
    } catch {
        run = new TextRun({ ...format, text: latexToReadable(tex) });
    }
    return display ? { display: run } : run;
};

const parseMathMl = (html) => {
    const root = { tag: 'root', attrs: {}, children: [] };
    const stack = [root];
    const xml = html.match(/<math[\s\S]*<\/math>/)?.[0] ?? '';

    for (const [, close, tag, attrs, selfClose, text] of xml.matchAll(/<(\/?)([\w:-]+)([^>]*?)(\/?)>|([^<]+)/g)) {
        const parent = stack[stack.length - 1];
        if (text !== undefined) parent.children.push(text);
        else if (close) stack.pop();
        else {
            const node = { tag, attrs: Object.fromEntries([...attrs.matchAll(/([\w:-]+)="([^"]*)"/g)].map(([, k, v]) => [k, v])), children: [] };
            parent.children.push(node);
            if (!selfClose) stack.push(node);
        }
    }
    return root;
};

const textOf = (node) => decodeEntities(node.children.filter((child) => typeof child === 'string').join(''));

// Converts MathML nodes to Word math components
const mathComponents = (nodes) => {
    const out = [];
    for (const item of nodes.flatMap(mathItems)) {
        const last = out[out.length - 1];
        const joinable = item.mathText !== undefined && last?.mathText !== undefined;
        if (joinable && last.normal === item.normal && last.style === item.style) last.mathText += item.mathText;
        else out.push(item.mathText !== undefined ? { ...item } : item);
    }
    return out.map((item) => {
        if (item.mathText === undefined) return item;
        if (item.normal) return new MathRun({ text: item.mathText, normalText: true });
        return item.style ? new MathRun({ text: item.mathText, style: item.style }) : new MathRun(item.mathText);
    });
};

// One MathML node as docx components
const mathItems = (node) => {
    if (typeof node === 'string') return [];
    const kids = node.children.filter((child) => typeof child !== 'string');
    const [a, b, c] = kids;
    const part = (child) => mathComponents(child ? [child] : []);

    switch (node.tag) {
        case 'mi': {
            const text = textOf(node);
            return [{ mathText: text, style: node.attrs.mathvariant === 'normal' || [...text].length > 1 ? 'plain' : undefined }];
        }
        case 'mn':
        case 'mo':
            return [{ mathText: textOf(node) }];
        case 'mtext':
        case 'ms':
            return [{ mathText: textOf(node), normal: true }];
        case 'mspace': {
            const em = parseFloat(node.attrs.width) || 0;
            if (em >= 0.9) return [{ mathText: ' ', normal: true }];
            return em >= 0.2 ? [{ mathText: ' ', normal: true }] : [];
        }
        case 'mfrac':
            return [new MathFraction({ numerator: part(a), denominator: part(b) })];
        case 'msqrt':
            return [new MathRadical({ children: mathComponents(kids) })];
        case 'mroot':
            return [new MathRadical({ children: part(a), degree: part(b) })];
        case 'msup':
            return [new MathSuperScript({ children: part(a), superScript: part(b) })];
        case 'msub':
            return [new MathSubScript({ children: part(a), subScript: part(b) })];
        case 'msubsup':
            return [new MathSubSuperScript({ children: part(a), subScript: part(b), superScript: part(c) })];
        case 'mover':
            return accented(node, a, b) ?? [new MathLimitUpper({ children: part(a), limit: part(b) })];
        case 'munder':
            return [new MathLimitLower({ children: part(a), limit: part(b) })];
        case 'munderover':
            return [new MathLimitUpper({ children: [new MathLimitLower({ children: part(a), limit: part(b) })], limit: part(c) })];
        case 'mtable':
            return kids.flatMap((row, i) => [...(i ? [{ mathText: ' ', normal: true }] : []), ...mathItems(row)]);
        case 'mrow':
            return brackets(kids) ?? kids.flatMap(mathItems);
        case 'annotation':
        case 'annotation-xml':
        case 'mphantom':
            return [];
        default:
            return kids.flatMap(mathItems);
    }
};

const ACCENT_MARKS = { 'ˊ': '́', 'ˋ': '̀', '˜': '̃', '~': '̃', '¨': '̈', 'ˆ': '̂', '^': '̂' };

// Rejoins a letter and the accent KaTeX splits off
const accented = (node, base, mark) => {
    if (node.attrs.accent !== 'true' || !base || !mark || !/^(mi|mn|mtext)$/.test(base.tag)) return null;
    const combining = ACCENT_MARKS[textOf(mark)];
    const joined = combining && (textOf(base) + combining).normalize('NFC');
    if (!joined || [...joined].length !== 1) return null;
    return mathItems({ ...base, children: [joined] });
};

const FENCES = { '(': MathRoundBrackets, '[': MathSquareBrackets, '{': MathCurlyBrackets, '⟨': MathAngledBrackets };

// Rejoins a fenced expression into a single component, if the opening and closing fence are recognized
const brackets = (kids) => {
    const [open, close] = [kids[0], kids[kids.length - 1]];
    const fenced = kids.length > 2 && [open, close].every((mo) => mo.tag === 'mo' && mo.attrs.fence === 'true');
    const Fence = fenced && FENCES[textOf(open)];
    return Fence ? [new Fence({ children: mathComponents(kids.slice(1, -1)) })] : null;
};

const ENTITIES = { amp: '&', lt: '<', gt: '>', quot: '"', apos: "'", nbsp: ' ' };

const decodeEntities = (s = '') => String(s).replace(/&(#x[\da-f]+|#\d+|\w+);/gi, (entity, name) => {
    if (name[0] !== '#') return ENTITIES[name] ?? entity;
    return String.fromCodePoint(name[1].toLowerCase() === 'x' ? parseInt(name.slice(2), 16) : parseInt(name.slice(1), 10));
});

const imageRun = (token, ctx, format) => {
    const image = ctx.images.get(token.href);
    if (!image) return new TextRun({ ...format, text: token.text || token.href, italics: true });
    return new ImageRun({
        type: image.type,
        data: image.data,
        transformation: { width: image.width, height: image.height },
        altText: { name: token.text || 'image', description: token.text },
    });
};

// Images are loaded up front
const loadImages = async (tokens) => {
    const sources = new Set();
    marked.walkTokens(tokens, (token) => {
        if (token.type === 'image') sources.add(token.href);
    });
    const images = new Map();
    await Promise.all([...sources].map(async (src) => images.set(src, await loadImage(src).catch(() => null))));
    return images;
};

const loadImage = async (src) => {
    let data = src.startsWith('data:') ? dataUrlBytes(src) : new Uint8Array(await (await fetch(src)).arrayBuffer());
    let type = imageType(data);
    if (!type) {
        data = await redrawAsPng(data);
        type = 'png';
    }
    const { width, height } = imageSize(data, type);
    const scale = Math.min(1, MAX_IMAGE_WIDTH_PX / width);
    return { type, data, width: Math.round(width * scale), height: Math.round(height * scale) };
};

const dataUrlBytes = (src) => {
    const [meta, payload = ''] = src.split(',');
    if (!/;base64/i.test(meta)) return new TextEncoder().encode(decodeURIComponent(payload));
    return Uint8Array.from(atob(payload), (ch) => ch.charCodeAt(0));
};

const imageType = (b) => {
    if (b[0] === 0x89 && b[1] === 0x50 && b[2] === 0x4E && b[3] === 0x47) return 'png';
    if (b[0] === 0xFF && b[1] === 0xD8) return 'jpg';
    if (b[0] === 0x47 && b[1] === 0x49 && b[2] === 0x46) return 'gif';
    if (b[0] === 0x42 && b[1] === 0x4D) return 'bmp';
    return null;
};

const redrawAsPng = (bytes) => new Promise((resolve, reject) => {
    const isSvg = /<svg[\s>]/i.test(new TextDecoder().decode(bytes.slice(0, 1024)));
    const url = URL.createObjectURL(new Blob([bytes], isSvg ? { type: 'image/svg+xml' } : {}));
    const img = new Image();
    img.onload = () => {
        const canvas = document.createElement('canvas');
        canvas.width = img.naturalWidth || MAX_IMAGE_WIDTH_PX;
        canvas.height = img.naturalHeight || Math.round(MAX_IMAGE_WIDTH_PX * 0.75);
        canvas.getContext('2d').drawImage(img, 0, 0, canvas.width, canvas.height);
        URL.revokeObjectURL(url);
        canvas.toBlob((blob) => (blob ? blob.arrayBuffer().then((buffer) => resolve(new Uint8Array(buffer))) : reject(new Error('png'))), 'image/png');
    };
    img.onerror = () => {
        URL.revokeObjectURL(url);
        reject(new Error('image'));
    };
    img.src = url;
});

// Pixel size read from the file header
const imageSize = (b, type) => {
    const view = new DataView(b.buffer, b.byteOffset, b.byteLength);
    if (type === 'png') return { width: view.getUint32(16), height: view.getUint32(20) };
    if (type === 'gif') return { width: view.getUint16(6, true), height: view.getUint16(8, true) };
    if (type === 'bmp') return { width: view.getInt32(18, true), height: Math.abs(view.getInt32(22, true)) };
    for (let i = 2; i + 9 < b.length;) {
        if (b[i] !== 0xFF || b[i + 1] === 0xFF) {
            i++;
            continue;
        }
        const marker = b[i + 1];
        if (marker >= 0xC0 && marker <= 0xCF && ![0xC4, 0xC8, 0xCC].includes(marker)) {
            return { width: view.getUint16(i + 7), height: view.getUint16(i + 5) };
        }
        i += 2 + view.getUint16(i + 2);
    }
    return { width: MAX_IMAGE_WIDTH_PX, height: Math.round(MAX_IMAGE_WIDTH_PX * 0.75) };
};

// Downloads the markdown as a .docx
export async function exportWord(markdown, filename = 'document.docx', title = '', t) {
    const blob = await Packer.toBlob(await buildWordDocument(markdown, title));
    const url  = URL.createObjectURL(blob);
    const a    = document.createElement('a');
    a.href     = url;
    a.download = filename.replace(/\.docx?$/i, '') + '.docx';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    setTimeout(() => URL.revokeObjectURL(url), 1000);
}

export default exportWord;
