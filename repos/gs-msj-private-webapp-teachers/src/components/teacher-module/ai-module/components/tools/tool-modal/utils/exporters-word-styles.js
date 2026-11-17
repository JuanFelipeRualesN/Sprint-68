// Word styles for the document and its elements

const NAVY = '27466C';
const TEAL = '3DA8A8';
const CODE_FILL = 'F2F4F7';

export const PAGE = { width: 12240, height: 15840, margin: 1440 };
export const CONTENT_WIDTH = PAGE.width - 2 * PAGE.margin;
export const MAX_IMAGE_WIDTH_PX = 620;

// Table borders are eighths of a point
export const TABLE = {
    border: { style: 'single', size: 4, color: 'B8C4D0' },
    headerFill: 'EEF3F8',
    cellMargins: { top: 80, bottom: 80, left: 120, right: 120 },
};

const heading = (size, spacing, extra = {}) => ({
    run: { font: 'Calibri', size, bold: true, color: NAVY },
    paragraph: { spacing, keepNext: true, ...extra },
});

const paragraphStyle = (id, name, options) => ({ id, name, basedOn: 'Normal', next: 'Normal', ...options });

export const WORD_STYLES = {
    default: {
        document: {
            run: { font: 'Calibri', size: 22, color: '1A1A1A' },
            paragraph: { spacing: { after: 160, line: 312 } },
        },
        heading1: heading(40, { before: 120, after: 240 }, {
            border: { bottom: { style: 'single', size: 12, color: TEAL, space: 4 } },
        }),
        heading2: heading(30, { before: 360, after: 120 }),
        heading3: heading(26, { before: 280, after: 80 }),
        heading4: heading(22, { before: 240, after: 80 }),
        heading5: heading(22, { before: 240, after: 80 }),
        heading6: heading(22, { before: 240, after: 80 }),
        hyperlink: { run: { color: '1F5FA8', underline: { type: 'single' } } },
    },
    paragraphStyles: [
        paragraphStyle('DocTitle', 'Document Title', {
            run: { size: 40, bold: true, color: NAVY },
            paragraph: { spacing: { after: 80 } },
        }),
        paragraphStyle('ListParagraph', 'List Paragraph', { paragraph: { spacing: { after: 80 } } }),
        paragraphStyle('TableHeader', 'Table Header', {
            run: { bold: true, color: NAVY },
            paragraph: { spacing: { after: 0 } },
        }),
        paragraphStyle('TableText', 'Table Text', { paragraph: { spacing: { after: 0 } } }),
        paragraphStyle('BlockQuote', 'Block Quote', {
            run: { color: '444444' },
            paragraph: { indent: { left: 240 }, border: { left: { style: 'single', size: 18, color: TEAL, space: 10 } } },
        }),
        paragraphStyle('CodeBlock', 'Code Block', {
            run: { font: 'Consolas', size: 20 },
            paragraph: { shading: { type: 'clear', fill: CODE_FILL, color: 'auto' }, spacing: { line: 240 } },
        }),
        paragraphStyle('HorizontalRule', 'Horizontal Rule', {
            paragraph: {
                spacing: { before: 120, after: 240 },
                border: { bottom: { style: 'single', size: 6, color: 'C9D3DE', space: 1 } },
            },
        }),
    ],
    characterStyles: [
        {
            id: 'InlineCode',
            name: 'Inline Code',
            basedOn: 'DefaultParagraphFont',
            run: { font: 'Consolas', size: 20, shading: { type: 'clear', fill: CODE_FILL, color: 'auto' } },
        },
    ],
};

export const listIndent = (level) => ({ left: 720 * (level + 1), hanging: 360 });

const BULLETS = ['•', '◦', '▪'];

export const listLevels = (ordered, startLevel = 0, start = 1) =>
    Array.from({ length: 9 }, (_, level) => ({
        level,
        format: ordered ? 'decimal' : 'bullet',
        text: ordered ? `%${level + 1}.` : BULLETS[level % BULLETS.length],
        alignment: 'left',
        start: ordered && level === startLevel ? start : 1,
        style: { paragraph: { indent: listIndent(level) } },
    }));
