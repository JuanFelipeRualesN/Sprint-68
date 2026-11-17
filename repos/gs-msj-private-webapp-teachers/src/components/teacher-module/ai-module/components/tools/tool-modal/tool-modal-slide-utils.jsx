/**
 * tool-modal-slide-utils.jsx
 * Slide data model: parsing markdown → slide objects, serializing back,
 * layout/style normalization helpers. No React. No Spanish identifiers.
 */

// ---------------------------------------------------------------------------
// Canvas constants
// ---------------------------------------------------------------------------

export const SLIDE_CANVAS = { w: 1280, h: 720 };

export const DEFAULT_SLIDE_LAYOUT = {
    title: { x: 86,  y: 66,  w: 1110, h: 92  },
    body:  { x: 100, y: 178, w: 1080, h: 438 },
    image: { x: 824, y: 278, w: 320,  h: 230 },
};

export const FONT_OPTIONS = ['Poppins', 'Sora Variable', 'Georgia', 'Verdana'];

export const DEFAULT_SLIDE_STYLE = { fontFace: 'Poppins', bodySize: 18, titleSize: 30 };

// Slide type option values — labels are resolved via i18next at render time.
export const SLIDE_TYPE_VALUES = [
    'hook', 'objective', 'concept', 'example',
    'comparison', 'process', 'activity', 'recap', 'exit',
];

// ---------------------------------------------------------------------------
// Numeric helpers
// ---------------------------------------------------------------------------

export function clampNumber(value, min, max, fallback) {
    const n = Number(value);
    if (!Number.isFinite(n)) return fallback;
    return Math.max(min, Math.min(max, n));
}

// ---------------------------------------------------------------------------
// Layout / style normalization
// ---------------------------------------------------------------------------

export function normalizeBox(box, fallback) {
    const merged = { ...fallback, ...(box || {}) };
    return {
        x: clampNumber(merged.x, 0, SLIDE_CANVAS.w - 80, fallback.x),
        y: clampNumber(merged.y, 0, SLIDE_CANVAS.h - 60, fallback.y),
        w: clampNumber(merged.w, 120, SLIDE_CANVAS.w,    fallback.w),
        h: clampNumber(merged.h, 42,  SLIDE_CANVAS.h,    fallback.h),
    };
}

export function normalizeSlideLayout(layout = {}) {
    return {
        title: normalizeBox(layout?.title, DEFAULT_SLIDE_LAYOUT.title),
        body:  normalizeBox(layout?.body,  DEFAULT_SLIDE_LAYOUT.body),
        image: normalizeBox(layout?.image, DEFAULT_SLIDE_LAYOUT.image),
    };
}

export function normalizeSlideStyle(style = {}) {
    const bodySize  = clampNumber(style?.bodySize,  13, 26, DEFAULT_SLIDE_STYLE.bodySize);
    const titleSize = clampNumber(
        style?.titleSize, 20, 42,
        Math.max(bodySize + 10, DEFAULT_SLIDE_STYLE.titleSize)
    );
    const fontFace  = FONT_OPTIONS.includes(style?.fontFace)
        ? style.fontFace
        : DEFAULT_SLIDE_STYLE.fontFace;
    return { fontFace, bodySize, titleSize };
}

export function normalizeSlideType(type, index = 0) {
    const raw = String(type || '').trim().toLowerCase();
    if (raw === 'closing' || raw === 'exit-ticket') return 'exit';
    if (SLIDE_TYPE_VALUES.includes(raw)) return raw;
    return index === 0 ? 'hook' : 'concept';
}

export function defaultSlideLayout() {
    return normalizeSlideLayout(DEFAULT_SLIDE_LAYOUT);
}

export function defaultSlideStyle() {
    return normalizeSlideStyle(DEFAULT_SLIDE_STYLE);
}

// ---------------------------------------------------------------------------
// Parsing helpers
// ---------------------------------------------------------------------------

/**
 * Strip leading slide-number labels from heading text.
 * Matches both English ("Slide 1:") and Spanish ("Diapositiva 1:")
 * because LLMs sometimes emit them even when instructed not to.
 */
function stripSlideLabels(text = '') {
    return String(text)
        .replace(/^\s*(?:diapositiva|dispositiva|slide)\s*\d+\s*[:.)-]?\s*/i, '')
        .replace(/\*\*/g, '')
        .trim();
}

function parseSlideLayout(raw) {
    if (!raw) return defaultSlideLayout();
    try { return normalizeSlideLayout(JSON.parse(raw)); }
    catch { return defaultSlideLayout(); }
}

function parseSlideStyle(raw) {
    if (!raw) return defaultSlideStyle();
    try { return normalizeSlideStyle(JSON.parse(raw)); }
    catch { return defaultSlideStyle(); }
}

// ---------------------------------------------------------------------------
// Parse markdown → slide objects
// ---------------------------------------------------------------------------

/**
 * Parse a markdown string produced by the AI (or the editor) into an array
 * of slide objects. Notes are stored in a language-neutral HTML comment so
 * the serialization format is stable across UI languages.
 */
export function parseEditableSlides(markdown) {
    const lines = String(markdown || '').split('\n');
    const slides = [];
    let current = null;

    function flush() {
        if (!current) return;
        const rawBody = current.body.join('\n').trim();

        const image       = rawBody.match(/!\[([^\]]*)\]\(([^)\s]+)\)/);
        const link        = rawBody.match(/\[([^\]]+)\]\((https?:\/\/[^)\s]+)\)/);
        const alignMatch  = rawBody.match(/<!-- align:\s*(left|center|right|justify)\s*-->/i);
        const layoutMatch = rawBody.match(/<!-- layout:\s*([\s\S]*?)\s*-->/i);
        const styleMatch  = rawBody.match(/<!-- style:\s*([\s\S]*?)\s*-->/i);
        const typeMatch   = rawBody.match(/<!-- type:\s*([a-z-]+)\s*-->/i);
        const claimMatch = rawBody.match(/<!-- claim: ([\s\S]*?) -->/i);
        const visualMatch = rawBody.match(/<!-- visual:\s*([\s\S]*?)\s*-->/i);
        // Language-neutral notes marker: <!-- notes: ... --> (written by slidesToMarkdown)
        // Also accept the legacy Spanish label for backward compatibility with existing documents.
        const notesMatch = rawBody.match(
            /(?:<!-- notes: ([\s\S]*?) -->|(?:^|\n)(?:Notes?|Notas? del maestro|Notas?):\s*([\s\S]+))/i
        );
        const body = rawBody
            .replace(/!\[[^\]]*\]\([^)]+\)/g, '')
            .replace(/\[([^\]]+)\]\((https?:\/\/[^)\s]+)\)/g, '$1')
            .replace(/<!-- align:\s*(left|center|right|justify)\s*-->/gi, '')
            .replace(/<!-- layout:\s*[\s\S]*?\s*-->/gi, '')
            .replace(/<!-- style:\s*[\s\S]*?\s*-->/gi, '')
            .replace(/<!-- type:\s*[a-z-]+\s*-->/gi, '')
            .replace(/<!-- claim:\s*[\s\S]*?\s*-->/gi, '')
            .replace(/<!-- visual:\s*[\s\S]*?\s*-->/gi, '')
            .replace(/<!-- notes:\s*[\s\S]*?\s*-->/gi, '')
            .replace(/(?:^|\n)(?:Notes?|Notas? del maestro|Notas?):\s*[\s\S]+$/i, '')
            .trim();

        slides.push({
            ...current,
            body,
            imageAlt:     image?.[1] || '',
            imageUrl:     image?.[2] || '',
            linkText:     link?.[1] || '',
            linkUrl:      link?.[2] || '',
            align:        alignMatch?.[1] || 'left',
            layout:       parseSlideLayout(layoutMatch?.[1]),
            style:        parseSlideStyle(styleMatch?.[1]),
            type:         normalizeSlideType(typeMatch?.[1], slides.length),
            claim:        claimMatch?.[1]?.trimStart() || '',
            visualIntent: visualMatch?.[1]?.trim() || '',
            // Prefer the comment-based marker; fall back to the legacy text label
            notes:        (notesMatch?.[1] || notesMatch?.[2] || '').trimStart(),
        });
    }

    for (const line of lines) {
        const heading = line.match(/^(#{1,3})\s+(.+)/);
        if (heading) {
            flush();
            current = { title: stripSlideLabels(heading[2]), body: [] };
            continue;
        }
        if (!current) current = { title: '', body: [] };
        current.body.push(line);
    }
    flush();

    return slides.length
        ? slides
        : [{ title: '', body: markdown || '', align: 'left', layout: defaultSlideLayout(), style: defaultSlideStyle() }];
}

// ---------------------------------------------------------------------------
// Serialize slide objects → markdown
// ---------------------------------------------------------------------------

/**
 * Serialize slide objects back to markdown. Notes are written as a
 * language-neutral HTML comment so round-tripping is stable regardless of
 * which UI language was active when the slide was created.
 */
export function slidesToMarkdown(slides) {
    return slides.map((slide, index) => {
        const heading = index === 0 ? '#' : '##';
        const parts = [slide.body || ''];
        if (slide.imageUrl)     parts.push(`![${slide.imageAlt || slide.title || 'Image'}](${slide.imageUrl})`);
        if (slide.linkUrl)      parts.push(`[${slide.linkText || slide.linkUrl}](${slide.linkUrl})`);
        if (slide.type)         parts.push(`<!-- type: ${normalizeSlideType(slide.type, index)} -->`);
        if (slide.claim)        parts.push(`<!-- claim: ${slide.claim} -->`);
        if (slide.visualIntent) parts.push(`<!-- visual: ${slide.visualIntent} -->`);
        if (slide.align && slide.align !== 'left') parts.push(`<!-- align: ${slide.align} -->`);
        if (slide.layout)       parts.push(`<!-- layout: ${JSON.stringify(normalizeSlideLayout(slide.layout))} -->`);
        if (slide.style)        parts.push(`<!-- style: ${JSON.stringify(normalizeSlideStyle(slide.style))} -->`);
        // Language-neutral notes comment — readable by parseEditableSlides regardless of UI locale
        if (slide.notes)        parts.push(`<!-- notes: ${slide.notes} -->`);

        const title = slide.title || 'Content';
        return `${heading} ${title}\n\n${parts.filter(Boolean).join('\n\n')}`.trim();
    }).join('\n\n');
}
