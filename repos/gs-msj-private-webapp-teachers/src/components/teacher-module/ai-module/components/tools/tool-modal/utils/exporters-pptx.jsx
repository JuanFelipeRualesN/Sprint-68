// exporters-pptx.js
// PPTX exporter with LaTeX support. Formulas are rasterized to PNG (MathJax -> SVG -> canvas)
// in a pre-pass, then placed as images. Browser-only (needs DOM + canvas + MathJax CDN).

import PptxGenJS from "pptxgenjs";
import {
    latexToReadable,
    sanitizePdfText,
    stripSlideNumberLabels,
} from './general-functions-exporters';

const SLIDE_TYPE_KEYS = {
    hook:       'export-utils:slideTypes.hook',
    objective:  'export-utils:slideTypes.objective',
    concept:    'export-utils:slideTypes.concept',
    example:    'export-utils:slideTypes.example',
    comparison: 'export-utils:slideTypes.comparison',
    process:    'export-utils:slideTypes.process',
    activity:   'export-utils:slideTypes.activity',
    recap:      'export-utils:slideTypes.recap',
    exit:       'export-utils:slideTypes.exit',
};

const TYPE_ACCENTS = {
    hook:       'E86F3A',
    objective:  '2A7F62',
    concept:    '2F5DA8',
    example:    '7A5AF8',
    comparison: 'C66A3D',
    process:    '0F8B8D',
    activity:   'D9436E',
    recap:      '4F5E70',
    exit:       '1F7A8C',
};

// Subject detection patterns — these are regex against content, not display labels
const SUBJECT_THEMES = [
    { test: /ciencia|biolog|quim|fisic|ecosistema|materia|energia/i,     bg: 'F4FAF8', ink: '143B3A', accent: '2A9D8F', accent2: 'E9C46A', soft: 'DDF3EE' },
    { test: /matem|geometr|algebra|fraccion|decimal|ecuaci/i,             bg: 'F7F6FF', ink: '1E2454', accent: '5B6EE1', accent2: 'F2B705', soft: 'E7E9FF' },
    { test: /social|historia|geograf|civica|comunidad|econom/i,           bg: 'FBF6EE', ink: '3D3024', accent: 'B85C38', accent2: '2F6F73', soft: 'F0E2CF' },
    { test: /espa|lectura|cuento|poema|vocabulario|escritura/i,           bg: 'FFF8F7', ink: '402632', accent: 'C44569', accent2: '3B7A57', soft: 'F8DDE5' },
];

const MATH_EX_RATIO = 0.5; // 1 math-ex ≈ half the font size, keeps math ~text height

// ═══════════════════════════════════════════════════════════════════
//  LaTeX → PNG pipeline
// ═══════════════════════════════════════════════════════════════════

// ---- MathJax bootstrap (singleton) ----
let _mjReady = null;
function ensureMathJax() {
    if (typeof window === 'undefined')
        return Promise.reject(new Error('LaTeX rendering needs a browser (MathJax).'));
    if (window.MathJax?.tex2svg) return Promise.resolve(window.MathJax);
    if (_mjReady) return _mjReady;

    _mjReady = new Promise((resolve, reject) => {
        window.MathJax = {
            loader: { load: ['input/tex-full', 'output/svg'] },
            tex: { packages: { '[+]': ['ams', 'color', 'boldsymbol', 'cases', 'mathtools'] } },
            svg: { fontCache: 'local' }, // self-contained per-SVG glyph defs (rasterizable)
            startup: {
                typeset: false,
                ready: () => { window.MathJax.startup.defaultReady(); resolve(window.MathJax); }
            }
        };
        const s = document.createElement('script');
        s.src = 'https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-svg.js';
        s.async = true;
        s.onerror = () => reject(new Error('Failed to load MathJax'));
        document.head.appendChild(s);
    });
    return _mjReady;
}

function loadImage(src) {
    return new Promise((res, rej) => {
        const im = new Image();
        im.onload = () => res(im);
        im.onerror = () => rej(new Error('image load failed'));
        im.src = src;
    });
}

// ---- Tokenizer: split a string into text / math runs ----
const MATH_RE = () =>
    /(\$\$[\s\S]+?\$\$|\\\[[\s\S]+?\\\]|\$[^$\n]+?\$|\\\([\s\S]+?\\\))/g;

export function hasMath(text) {
    return typeof text === 'string' && MATH_RE().test(text);
}

export function tokenizeMath(text) {
    const out = [];
    let last = 0;
    const re = MATH_RE();
    let m;
    while ((m = re.exec(text)) !== null) {
        if (m.index > last) out.push({ type: 'text', value: text.slice(last, m.index) });
        const raw = m[0];
        let display = false, body = raw;
        if (raw.startsWith('$$'))       { display = true;  body = raw.slice(2, -2); }
        else if (raw.startsWith('\\[')) { display = true;  body = raw.slice(2, -2); }
        else if (raw.startsWith('\\(')) { display = false; body = raw.slice(2, -2); }
        else                            { display = false; body = raw.slice(1, -1); }
        out.push({ type: 'math', value: body.trim(), display });
        last = m.index + raw.length;
    }
    if (last < text.length) out.push({ type: 'text', value: text.slice(last) });
    return out;
}

// ---- Rasterize one formula to a high-res PNG (+ intrinsic ex geometry) ----
async function renderMath(latex, { display, color = '222222', exPxBig = 44, supersample = 2 }) {
    const MJ = await ensureMathJax();
    const container = MJ.tex2svg(latex, { display: !!display });
    const svg = container.querySelector('svg');
    if (!svg) throw new Error('MathJax produced no SVG');

    const wEx = parseFloat(svg.getAttribute('width'))  || 1;
    const hEx = parseFloat(svg.getAttribute('height')) || 1;
    let depthEx = 0;
    const va = (container.getAttribute('style') || '').match(/vertical-align:\s*(-?[\d.]+)ex/);
    if (va) depthEx = Math.abs(parseFloat(va[1])); // baseline depth below the box bottom

    const wPx = Math.max(1, Math.round(wEx * exPxBig));
    const hPx = Math.max(1, Math.round(hEx * exPxBig));
    svg.setAttribute('width', wPx);
    svg.setAttribute('height', hPx);
    svg.setAttribute('xmlns', 'http://www.w3.org/2000/svg');
    svg.style.color = '#' + color; // resolves the paths' currentColor

    const xml = new XMLSerializer().serializeToString(svg);
    const url = URL.createObjectURL(new Blob([xml], { type: 'image/svg+xml;charset=utf-8' }));
    let svgImg;
    try { svgImg = await loadImage(url); } finally { URL.revokeObjectURL(url); }

    const cv = document.createElement('canvas');
    cv.width = wPx * supersample;
    cv.height = hPx * supersample;
    const ctx = cv.getContext('2d');
    ctx.setTransform(supersample, 0, 0, supersample, 0, 0);
    ctx.drawImage(svgImg, 0, 0, wPx, hPx);

    const dataUri = cv.toDataURL('image/png');
    const img = await loadImage(dataUri); // preload for the compositor
    return { img, dataUri, wEx, hEx, depthEx };
}

// Perceived luminance of a 6-hex color, used to pick the best prerendered color variant.
function _lum(hex) {
    const h = String(hex).replace('#', '');
    const r = parseInt(h.slice(0, 2), 16) || 0;
    const g = parseInt(h.slice(2, 4), 16) || 0;
    const b = parseInt(h.slice(4, 6), 16) || 0;
    return 0.2126 * r + 0.7152 * g + 0.0722 * b;
}

// ---- Pre-pass: rasterize every formula across all slides, in each palette color ----
// Returns a cache object with a color-aware .lookup(latex, display, color).
export async function prerenderMath(slides, { colors = ['222222'], fallback } = {}) {
    const map    = new Map();
    const wanted = new Map(); // tag -> tok

    const scan = (text) => {
        if (typeof text !== 'string') return;
        for (const tok of tokenizeMath(text)) {
            if (tok.type !== 'math') continue;
            const tag = `${tok.display ? 'D' : 'I'}|${tok.value}`;
            if (!wanted.has(tag)) wanted.set(tag, tok);
        }
    };

    for (const s of slides) {
        scan(s.title); scan(s.body); scan(s.claim);
        (s.bullets || []).forEach(scan);
        // table cells can't hold images in pptx -> they stay as fallback text
    }

    for (const [tag, tok] of wanted) {
        for (const color of colors) {
            try {
                map.set(`${color}|${tag}`, await renderMath(tok.value, { display: tok.display, color }));
            } catch { /* try next color; fallback covers total failure */ }
        }
        map.set(`fallback|${tag}`, { fallback: fallback ? fallback(tok.value) : tok.value });
    }

    return {
        colors: colors.slice(),
        lookup(latex, display, color) {
            const tag   = `${display ? 'D' : 'I'}|${latex}`;
            const exact = map.get(`${color}|${tag}`);
            if (exact && exact.img) return exact;
            // Nearest-luminance variant (e.g. muted-gray text on a dark slide → white math).
            const target = _lum(color);
            let best = null, bestD = Infinity;
            for (const c of this.colors) {
                const e = map.get(`${c}|${tag}`);
                if (e && e.img) {
                    const d = Math.abs(_lum(c) - target);
                    if (d < bestD) { bestD = d; best = e; }
                }
            }
            return best || map.get(`fallback|${tag}`) || null;
        },
    };
}

// ---- Geometry helpers shared by inline compositor & standalone display ----
const pxFromEx = (ex, fontPt) => ex * (fontPt * 96 / 72) * MATH_EX_RATIO;
const cssFont  = (f) => (f && /\s/.test(f) ? `"${f}"` : f) || 'Arial';

function wrapAtoms(atoms, maxW, spaceW) {
    const lines = [];
    let line = [], w = 0;
    const flushLine = () => { lines.push({ items: line, w }); line = []; w = 0; };
    for (const a of atoms) {
        if (a.t === 'break') { if (line.length) flushLine(); else lines.push({ items: [], w: 0 }); continue; }
        const add = (line.length ? spaceW : 0) + a.w;
        if (line.length && w + add > maxW) flushLine();
        if (line.length) w += spaceW;
        line.push(a); w += a.w;
    }
    if (line.length) flushLine();
    return lines;
}

// Composite a math-containing paragraph (text + inline/display formulas) to one PNG.
// Honors '\n' as a hard line break so multi-line bullets (e.g. answer options) survive.
export function composeParagraph(text, cache, {
    fontPt = 18, fontFamily = 'Arial', bold = false, color = '222222',
    maxWidthIn = 9, lineHeight = 1.32, prefix = ''
}) {
    const meas = document.createElement('canvas').getContext('2d');
    const fontPx = fontPt * 96 / 72;
    const fontSpec = `${bold ? '600 ' : ''}${fontPx}px ${cssFont(fontFamily)}`;
    meas.font = fontSpec;
    const spaceW = meas.measureText(' ').width;
    const maxW = maxWidthIn * 96;
    const ascent = fontPx * 0.8, descent = fontPx * 0.2;
    const lineH = fontPx * lineHeight;

    const atoms = [];
    if (prefix) atoms.push({ t: 'word', s: prefix, w: meas.measureText(prefix).width });

    const segments = String(text).split('\n');
    segments.forEach((segment, si) => {
        if (si > 0) atoms.push({ t: 'break' });
        for (const tok of tokenizeMath(segment)) {
            if (tok.type === 'text') {
                for (const wrd of tok.value.split(/\s+/).filter(Boolean))
                    atoms.push({ t: 'word', s: wrd, w: meas.measureText(wrd).width });
            } else {
                const e = cache && cache.lookup(tok.value, tok.display, color);
                if (e && e.img) {
                    const w = pxFromEx(e.wEx, fontPt);
                    atoms.push({ t: 'math', img: e.img, w, h: pxFromEx(e.hEx, fontPt), depth: pxFromEx(e.depthEx, fontPt) });
                } else {
                    const s = (e && e.fallback) || tok.value;
                    atoms.push({ t: 'word', s, w: meas.measureText(s).width });
                }
            }
        }
    });

    const lines = wrapAtoms(atoms, maxW, spaceW).map(ln => {
        let above = ascent, below = descent;
        for (const a of ln.items) if (a.t === 'math') {
            above = Math.max(above, a.h - a.depth);
            below = Math.max(below, a.depth);
        }
        return { ...ln, above, h: above + below };
    });

    const totalW = Math.max(1, ...lines.map(l => l.w));
    const totalH = lines.reduce((s, l) => s + Math.max(lineH, l.h), 0);

    const SS = 2;
    const cv = document.createElement('canvas');
    cv.width = Math.ceil(totalW * SS);
    cv.height = Math.ceil(totalH * SS);
    const ctx = cv.getContext('2d');
    ctx.setTransform(SS, 0, 0, SS, 0, 0);
    ctx.font = fontSpec;
    ctx.fillStyle = '#' + color;
    ctx.textBaseline = 'alphabetic';

    let y = 0;
    for (const ln of lines) {
        const rowH = Math.max(lineH, ln.h);
        const baseline = y + ln.above;
        let x = 0;
        ln.items.forEach((a, i) => {
            if (i) x += spaceW;
            if (a.t === 'word') ctx.fillText(a.s, x, baseline);
            else ctx.drawImage(a.img, x, baseline - (a.h - a.depth), a.w, a.h);
            x += a.w;
        });
        y += rowH;
    }

    return { dataUri: cv.toDataURL('image/png'), wIn: totalW / 96, hIn: totalH / 96 };
}

// ═══════════════════════════════════════════════════════════════════
//  Math-aware text primitives
// ═══════════════════════════════════════════════════════════════════

// Draw a single string into a box: native addText when there's no math (crisp, selectable),
// or a contain-fitted composed PNG when the string contains LaTeX.
function addAutoText(s, text, box, deck, opts = {}) {
    const {
        fontSize = 14, color = deck.ink, fontFace = deck.fontFace,
        bold = false, align = 'left', valign, fit = 'shrink', bullet = false,
    } = opts;
    const cache = deck?.mathCache;

    if (cache && hasMath(text)) {
        const png = composeParagraph(text, cache, {
            fontPt: fontSize, fontFamily: fontFace, bold, color,
            maxWidthIn: box.w, prefix: bullet ? '•  ' : '',
        });
        let w = png.wIn, h = png.hIn;
        const maxH  = box.h || Infinity;
        const scale = Math.min(box.w / w, maxH / h, 1);
        w *= scale; h *= scale;

        let x = box.x;
        if (align === 'center') x += (box.w - w) / 2;
        else if (align === 'right') x += (box.w - w);

        let y = box.y;
        if (box.h) {
            if (valign === 'mid')    y += Math.max(0, (box.h - h) / 2);
            else if (valign === 'bottom') y += Math.max(0, box.h - h);
        }
        s.addImage({ data: png.dataUri, x, y, w, h });
        return;
    }

    const textOpts = {
        ...box, fontSize, color, fontFace, bold, align, fit,
        bullet: bullet ? { code: '2022' } : false,
    };
    if (valign) textOpts.valign = valign;
    s.addText(text, textOpts);
}

// Renders an array of paragraph strings top-to-bottom, math-aware.
// Text-only lines stay native; math lines become images. Returns the Y after the last line.
function renderStackedText(s, items, box, deck, opts = {}) {
    const { fontPt = 16, gap = 0.1, bullet = false, color = deck.ink, fontFamily = deck.fontFace } = opts;
    const cache  = deck.mathCache;
    const bottom = box.y + (box.h || Infinity);
    let y = box.y;

    for (const raw of items) {
        if (!raw) continue;

        if (cache && hasMath(raw)) {
            const png = composeParagraph(raw, cache, {
                fontPt, fontFamily, color, maxWidthIn: box.w, prefix: bullet ? '•  ' : '',
            });
            let w = png.wIn, h = png.hIn;
            if (w > box.w) { const sc = box.w / w; w *= sc; h *= sc; }
            s.addImage({ data: png.dataUri, x: box.x, y, w, h });
            y += h + gap;
        } else {
            const h = 0.3 * estimateBulletLines(raw);
            s.addText(raw, {
                x: box.x, y, w: box.w, h,
                fontSize: fontPt, color, fontFace: fontFamily,
                bullet: bullet ? { code: '2022' } : false, valign: 'top', fit: 'shrink',
            });
            y += h + gap;
        }
        if (y > bottom) break; // pagination handles the overflow
    }
    return y;
}

// ═══════════════════════════════════════════════════════════════════
//  Export entry point
// ═══════════════════════════════════════════════════════════════════

/**
 * @param {string}   markdown
 * @param {string}   filename
 * @param {string}   title
 * @param {import('i18next').TFunction} t - i18next TFunction
 */
export async function exportPPTX(markdown, filename = 'presentation.pptx', title = '', t) {
    const pptx = new PptxGenJS();
    pptx.layout = 'LAYOUT_WIDE';
    pptx.author = 'Genial Skills';

    const smartSlides = paginateSlides(
        parseMarkdownToSlides(markdown, t, { preserveMath: true })
    );
    const deck = buildDeckTheme(title, smartSlides);

    // Rasterize every formula up front so rendering can stay synchronous.
    // Render in both the dark ink (light slides) and white (dark slides) so math is
    // always legible. latexToReadable is the graceful fallback when a formula won't render.
    deck.mathCache = await prerenderMath(smartSlides, {
        colors: [deck.ink, 'FFFFFF'],
        fallback: latexToReadable,
    });

    renderCoverSlide(pptx, pptx.addSlide(), deck, t);
    smartSlides.forEach((slide, index) => {
        const s = pptx.addSlide();
        renderSmartSlide(pptx, s, slide, index, deck, t);
        if (slide.notes) s.addNotes(latexToReadable(slide.notes)); // notes stay plain text
    });
    return pptx.writeFile({ fileName: filename });
}

// ─────────── Deck theme ───────────

function buildDeckTheme(title, slides) {
    const text  = `${title}\n${slides.map(s => `${s.title} ${s.body}`).join('\n')}`;
    const match = SUBJECT_THEMES.find(theme => theme.test.test(text));
    const base  = match || { bg: 'F7F9FC', ink: '17233C', accent: '2F6FBB', accent2: 'F2A65A', soft: 'E5EEF8' };
    return {
        ...base,
        title:       sanitizePdfText(title || slides[0]?.title || ''),
        fontFace:    'Poppins',
        displayFace: 'Georgia',
    };
}

// ─────────── Slide type helpers ───────────

function normalizeSlideType(type, index = 0) {
    const raw = String(type || '').trim().toLowerCase();
    if (raw === 'closing' || raw === 'exit-ticket') return 'exit';
    if (SLIDE_TYPE_KEYS[raw]) return raw;
    return index === 0 ? 'hook' : 'concept';
}

// ─────────── Slide renderers ───────────

function renderCoverSlide(pptx, slide, deck, t) {
    slide.background = { color: deck.ink };
    slide.addShape(pptx.ShapeType.rect,    { x: 0,   y: 0,    w: 13.333, h: 7.5, fill: { color: deck.ink },             line: { transparency: 100 } });
    slide.addShape(pptx.ShapeType.ellipse, { x: 8.6, y: -1.0, w: 4.9,    h: 4.9, fill: { color: deck.accent, transparency: 85 }, line: { color: deck.accent2, transparency: 30, pt: 2 } });
    slide.addShape(pptx.ShapeType.rect,    { x: 0.7, y: 0.72, w: 1.15,   h: 0.08, fill: { color: deck.accent },          line: { transparency: 100 } });
    slide.addText(t('export-utils:cover.eyebrow'),         { x: 0.7,  y: 0.95, w: 3.8,  h: 0.24, fontSize: 8.5,  bold: true, color: deck.accent2, charSpace: 1.4, fontFace: deck.fontFace });
    slide.addText(deck.title,                             { x: 0.7,  y: 2.05, w: 8.8,  h: 1.85, fontSize: 40,   bold: true, color: 'FFFFFF',      fontFace: deck.displayFace, fit: 'shrink' });
    slide.addText(t('export-utils:cover.byline'),          { x: 0.74, y: 6.55, w: 4.2,  h: 0.32, fontSize: 11,             color: 'D7DEE8',      fontFace: deck.fontFace });
    slide.addShape(pptx.ShapeType.rect,    { x: 10.2, y: 5.74, w: 2.15,   h: 0.72, fill: { color: deck.accent },          line: { transparency: 100 } });
    slide.addText(t('export-utils:cover.readyBadge'),      { x: 10.38, y: 5.95, w: 1.8, h: 0.2, fontSize: 10,  bold: true, color: 'FFFFFF', align: 'center', fontFace: deck.fontFace });
}

function renderSmartSlide(pptx, s, slide, index, deck, t) {
    const type = normalizeSlideType(slide.type, index);
    const current = { ...slide, type };
    const rendererMap = {
        hook:       renderHookSlide,
        objective:  renderObjectiveSlide,
        concept:    renderConceptSlide,
        example:    renderExampleSlide,
        comparison: renderComparisonSlide,
        process:    renderProcessSlide,
        activity:   renderActivitySlide,
        recap:      renderRecapSlide,
        exit:       renderExitSlide,
    };
    const renderer = rendererMap[type] || renderConceptSlide;
    renderer(pptx, s, current, index, deck, t);
}

function baseSlide(pptx, s, slide, index, deck, t, opts = {}) {
    const accent = TYPE_ACCENTS[slide.type] || deck.accent;
    const dark   = !!opts.dark;
    s.background = { color: dark ? deck.ink : deck.bg };
    s.addShape(pptx.ShapeType.rect, { x: 0, y: 0, w: 13.333, h: 7.5, fill: { color: dark ? deck.ink : deck.bg }, line: { transparency: 100 } });
    s.addShape(pptx.ShapeType.rect, { x: 0, y: 0, w: 13.333, h: 0.11, fill: { color: accent }, line: { transparency: 100 } });
    s.addText(
        (t(SLIDE_TYPE_KEYS[slide.type] || 'export-utils:slideTypes.concept')).toUpperCase(),
        { x: 0.62, y: 0.4, w: 2.1, h: 0.24, fontSize: 8, bold: true, color: dark ? deck.accent2 : accent, charSpace: 1.2, fontFace: deck.fontFace }
    );
    s.addText(String(index + 1).padStart(2, '0'), { x: 12.05, y: 6.93, w: 0.62, h: 0.24, fontSize: 8.5, bold: true, color: dark ? 'D7DEE8' : '7D8797', align: 'right', fontFace: deck.fontFace });
    return { accent, ink: dark ? 'FFFFFF' : deck.ink, muted: dark ? 'D7DEE8' : '637083', dark };
}

function renderHookSlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t, { dark: true });
    addAutoText(s, slide.title, { x: 0.72, y: 1.22, w: 7.9, h: 1.22 }, deck, { fontSize: 34, bold: true, color: 'FFFFFF', fontFace: deck.displayFace });
    addAutoText(s, slide.claim || firstSentence(slide), { x: 0.78, y: 2.72, w: 6.2, h: 1.1 }, deck, { fontSize: 17, color: ui.muted, fontFace: deck.fontFace });
    renderBulletCards(pptx, s, bulletDefaults(slide, t, 'hook'), 0.76, 4.28, 2.55, 1.05, ui.accent, 'FFFFFF', deck.fontFace, deck);
  //  renderImageOrVisual(pptx, s, slide, { x: 8.3, y: 1.22, w: 3.8, h: 4.72 }, deck, ui.accent, t, true);
}

function renderObjectiveSlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t);
    addAutoText(s, slide.title, { x: 0.72, y: 0.92, w: 6.8, h: 0.72 }, deck, { fontSize: 27, bold: true, color: ui.ink, fontFace: deck.displayFace });
    addAutoText(s, slide.claim || t('export-utils:defaults.objectiveClaim'), { x: 7.85, y: 0.98, w: 3.8, h: 0.58 }, deck, { fontSize: 13, color: ui.muted, fontFace: deck.fontFace });
    renderNumberedLanes(pptx, s, bulletDefaults(slide, t, 'objective'), 0.84, 2.12, 11.55, 0.88, ui.accent, deck);
}

function renderConceptSlide(pptx, s, slide, index, deck, t) {
    const ui      = baseSlide(pptx, s, slide, index, deck, t);
    const bullets = bulletDefaults(slide, t, 'concept');
    const heavy   = !slide.imageUrl && bullets.reduce((n, b) => n + estimateBulletLines(b), 0) > 6;

    addAutoText(s, slide.title, { x: 0.72, y: 0.92, w: heavy ? 11.6 : 7.2, h: 0.72 }, deck, { fontSize: heavy ? 25 : 29, bold: true, color: ui.ink, fontFace: deck.displayFace });

    if (heavy) {
        // Text-heavy slide (e.g. exam questions): use the full width, skip the visual
        renderBulletList(s, bullets, { x: 0.88, y: 1.95, w: 11.6, h: 4.95 }, ui.ink, deck.fontFace, 14, deck);
        return;
    }

    addAutoText(s, slide.claim || firstSentence(slide), { x: 0.76, y: 1.78, w: 12, h: 0.76 }, deck, { fontSize: 14.5, color: ui.muted, fontFace: deck.fontFace });
    renderBulletList(s, bullets, { x: 0.88, y: 3.0, w: 5.35, h: 2.55 }, ui.ink, deck.fontFace, 16, deck);
   // renderImageOrVisual(pptx, s, slide, { x: 7.05, y: 1.34, w: 4.75, h: 4.55 }, deck, ui.accent, t);
}

function renderExampleSlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t);
    addAutoText(s, slide.title, { x: 0.74, y: 0.86, w: 12, h: 0.62 }, deck, { fontSize: 27, bold: true, color: ui.ink, fontFace: deck.displayFace });
    renderStepStrip(pptx, s, bulletDefaults(slide, t, 'example', true), 0.84, 2.0, 11.6, 3.25, ui.accent, deck);
    addAutoText(s, slide.claim || t('export-utils:defaults.exampleClaim'), { x: 0.9, y: 5.95, w: 10.6, h: 0.42 }, deck, { fontSize: 12.5, color: ui.muted, fontFace: deck.fontFace });
}

function renderComparisonSlide(pptx, s, slide, index, deck, t) {
    const ui    = baseSlide(pptx, s, slide, index, deck, t);
    addAutoText(s, slide.title, { x: 0.74, y: 0.88, w: 10.8, h: 0.66 }, deck, { fontSize: 27, bold: true, color: ui.ink, fontFace: deck.displayFace });
    const items = slide.bullets.length ? slide.bullets : splitBodyLines(slide.body);
    const left  = items.filter((_, i) => i % 2 === 0).slice(0, 4);
    const right = items.filter((_, i) => i % 2 === 1).slice(0, 4);
    renderComparePanel(s, t('export-utils:defaults.ideaA'), left,                                  0.82, 2.0, 5.45, 3.82, ui.accent,    deck);
    renderComparePanel(s, t('export-utils:defaults.ideaB'), right.length ? right : items.slice(2, 6), 7.05, 2.0, 5.45, 3.82, deck.accent2, deck);
}

function renderProcessSlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t);
    addAutoText(s, slide.title, { x: 0.74, y: 0.9, w: 9.6, h: 0.64 }, deck, { fontSize: 28, bold: true, color: ui.ink, fontFace: deck.displayFace });
    renderStepStrip(pptx, s, bulletDefaults(slide, t, 'process'), 0.78, 2.08, 11.85, 3.55, ui.accent, deck);
}

function renderActivitySlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t);
    s.addShape(pptx.ShapeType.rect, { x: 0.76, y: 1.05, w: 11.75, h: 5.55, fill: { color: 'FFFFFF' }, line: { color: deck.soft, pt: 1 } });
    addAutoText(s, slide.title, { x: 1.08, y: 1.34, w: 7.6, h: 0.66 }, deck, { fontSize: 28, bold: true, color: ui.ink, fontFace: deck.displayFace });
    addAutoText(s, slide.claim || t('export-utils:defaults.activityClaim'), { x: 1.12, y: 2.18, w: 4.8, h: 0.58 }, deck, { fontSize: 13, color: ui.muted, fontFace: deck.fontFace });
    renderBulletList(s, bulletDefaults(slide, t, 'activity'), { x: 6.45, y: 1.55, w: 5.35, h: 4.25 }, deck.ink, deck.fontFace, 15, deck);
    s.addShape(pptx.ShapeType.rect, { x: 1.1, y: 4.78, w: 3.2, h: 0.58, fill: { color: ui.accent }, line: { transparency: 100 } });
    s.addText(t('export-utils:defaults.activeWork'), { x: 1.28, y: 4.96, w: 2.7, h: 0.2, fontSize: 10, bold: true, color: 'FFFFFF', align: 'center', fontFace: deck.fontFace });
}

function renderRecapSlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t);
    addAutoText(s, slide.title, { x: 0.74, y: 0.96, w: 9.6, h: 0.64 }, deck, { fontSize: 29, bold: true, color: ui.ink, fontFace: deck.displayFace });
    renderBulletCards(pptx, s, bulletDefaults(slide, t, 'recap'), 0.82, 2.18, 2.78, 2.45, ui.accent, deck.ink, deck.fontFace, deck);
}

function renderExitSlide(pptx, s, slide, index, deck, t) {
    const ui = baseSlide(pptx, s, slide, index, deck, t, { dark: true });
    addAutoText(s, slide.title, { x: 0.78, y: 1.22, w: 8.4, h: 1.02 }, deck, { fontSize: 33, bold: true, color: 'FFFFFF', fontFace: deck.displayFace });
    addAutoText(s, slide.claim || t('export-utils:defaults.exitClaim'), { x: 0.82, y: 2.45, w: 5.7, h: 0.68 }, deck, { fontSize: 15, color: ui.muted, fontFace: deck.fontFace });
    // Keep mathCache when overriding theme colors for the dark exit lanes.
    renderNumberedLanes(pptx, s, bulletDefaults(slide, t, 'exit'), 0.9, 3.6, 10.8, 0.86, ui.accent, { ...deck, ink: 'FFFFFF', bg: deck.ink, soft: '344660' });
}

// ─────────── Visual placeholder ───────────

function renderImageOrVisual(pptx, s, slide, box, deck, accent, t, dark = false) {
    if (slide.imageUrl) {
        try {
            s.addImage({ path: slide.imageUrl, ...box, sizing: { type: 'contain', w: box.w, h: box.h } });
            return;
        } catch { /* fall through to placeholder */ }
    }
    s.addShape(pptx.ShapeType.rect,    { ...box, fill: { color: dark ? '26364F' : 'FFFFFF' }, line: { color: dark ? '4C5D78' : deck.soft, pt: 1.2 } });
    s.addShape(pptx.ShapeType.ellipse, { x: box.x + box.w * 0.18, y: box.y + box.h * 0.12, w: box.w * 0.64, h: box.w * 0.64, fill: { color: accent, transparency: 88 }, line: { color: accent, pt: 2, transparency: 10 } });
    s.addShape(pptx.ShapeType.rect,    { x: box.x + box.w * 0.22, y: box.y + box.h * 0.63, w: box.w * 0.55, h: 0.12, fill: { color: accent }, line: { transparency: 100 } });
    s.addText(
        slide.visualIntent || t('export-utils:defaults.visualPlaceholder'),
        { x: box.x + 0.38, y: box.y + box.h - 0.78, w: box.w - 0.76, h: 0.44, fontSize: 10.5, color: dark ? 'D7DEE8' : '657284', align: 'center', fontFace: deck.fontFace, fit: 'shrink' }
    );
}

// ─────────── Bullet helpers ───────────

/**
 * Returns slide bullets when present, otherwise falls back to translated defaults.
 */
function bulletDefaults(slide, t, type, useBody = false) {
    if (slide.bullets?.length) return slide.bullets;
    if (useBody) {
        const fromBody = splitBodyLines(slide.body);
        if (fromBody.length) return fromBody;
    }
    const defaults = t(`export-utils:bulletDefaults.${type}`, { returnObjects: true });
    return Array.isArray(defaults) ? defaults : [];
}

function renderBulletList(s, bullets, box, color, fontFace, fontSize = 15, deck) {
    const cache = deck?.mathCache;
    // If any bullet has math, stack line by line (mix native text + formula images).
    if (cache && bullets.some(hasMath)) {
        renderStackedText(s, bullets, box, deck, { fontPt: fontSize, color, fontFamily: fontFace, bullet: true });
        return;
    }
    // Pure text: keep the crisp native bulleted list.
    s.addText(bullets.map(item => ({ text: item, options: { bullet: { indent: 14 } } })), {
        ...box, fontSize, color, fontFace, fit: 'shrink', paraSpaceAfterPt: 8, valign: 'top',
    });
}

function renderBulletCards(pptx, s, bullets, x, y, w, h, accent, ink, fontFace, deck) {
    bullets.slice(0, 4).forEach((item, i) => {
        const cx = x + i * (w + 0.28);
        s.addShape(pptx.ShapeType.rect, { x: cx, y, w, h, fill: { color: 'FFFFFF', transparency: ink === 'FFFFFF' ? 92 : 0 }, line: { color: accent, transparency: ink === 'FFFFFF' ? 15 : 55, pt: 1.1 } });
        s.addShape(pptx.ShapeType.rect, { x: cx, y, w, h: 0.09, fill: { color: accent }, line: { transparency: 100 } });
        addAutoText(s, item, { x: cx + 0.18, y: y + 0.28, w: w - 0.36, h: h - 0.38 }, deck, { fontSize: 12.5, bold: true, color: ink, fontFace, valign: 'mid' });
    });
}

function renderNumberedLanes(pptx, s, bullets, x, y, w, h, accent, deck) {
    bullets.slice(0, 5).forEach((item, i) => {
        const top = y + i * (h + 0.24);
        s.addShape(pptx.ShapeType.rect,    { x, y: top, w, h, fill: { color: i % 2 ? deck.soft : 'FFFFFF', transparency: deck.bg === deck.ink ? 88 : 0 }, line: { color: deck.soft, pt: 1 } });
        s.addShape(pptx.ShapeType.ellipse, { x: x + 0.22, y: top + 0.18, w: 0.38, h: 0.38, fill: { color: accent }, line: { transparency: 100 } });
        s.addText(String(i + 1),  { x: x + 0.22, y: top + 0.28, w: 0.38, h: 0.1, fontSize: 7.5, bold: true, color: 'FFFFFF', align: 'center', fontFace: deck.fontFace });
        addAutoText(s, item, { x: x + 0.82, y: top + 0.2, w: w - 1.05, h: h - 0.22 }, deck, { fontSize: 14, color: deck.ink, fontFace: deck.fontFace, valign: 'mid' });
    });
}

function renderStepStrip(pptx, s, bullets, x, y, w, h, accent, deck) {
    const items  = bullets.slice(0, 5);
    const gap    = 0.18;
    const cardW  = (w - gap * (items.length - 1)) / items.length;
    items.forEach((item, i) => {
        const cx = x + i * (cardW + gap);
        s.addShape(pptx.ShapeType.rect, { x: cx, y, w: cardW, h, fill: { color: i === 0 ? accent : 'FFFFFF' }, line: { color: i === 0 ? accent : deck.soft, pt: 1 } });
        s.addText(String(i + 1).padStart(2, '0'), { x: cx + 0.22, y: y + 0.22, w: 0.68, h: 0.22, fontSize: 9, bold: true, color: i === 0 ? 'FFFFFF' : accent, fontFace: deck.fontFace });
        addAutoText(s, item, { x: cx + 0.24, y: y + 0.82, w: cardW - 0.48, h: h - 1.05 }, deck, { fontSize: 13, bold: i === 0, color: i === 0 ? 'FFFFFF' : deck.ink, fontFace: deck.fontFace, valign: 'mid' });
    });
}

function renderComparePanel(s, label, items, x, y, w, h, accent, deck) {
    s.addShape('rect', { x, y, w, h, fill: { color: 'FFFFFF' }, line: { color: deck.soft, pt: 1 } });
    s.addShape('rect', { x, y, w, h: 0.5, fill: { color: accent }, line: { transparency: 100 } });
    s.addText(label, { x: x + 0.24, y: y + 0.15, w: w - 0.48, h: 0.16, fontSize: 10, bold: true, color: 'FFFFFF', fontFace: deck.fontFace });
    renderBulletList(s, items, { x: x + 0.34, y: y + 0.9, w: w - 0.68, h: h - 1.2 }, deck.ink, deck.fontFace, 13, deck);
}

// ─────────── Slide content helpers ───────────

function firstSentence(slide) {
    const text = [slide.body, ...(slide.bullets || [])].join(' ').replace(/\s+/g, ' ').trim();
    return text.split(/(?<=[.!?])\s+/)[0] || '';
}

function splitBodyLines(body = '') {
    return String(body).split(/\n|[.;]\s+/).map(item => item.trim()).filter(Boolean);
}

/**
 * @param {string}   md
 * @param {import('i18next').TFunction} t - i18next TFunction
 * @param preserveMath When true, keep raw LaTeX so the exporter can rasterize it.
 */
function parseMarkdownToSlides(md, t, { preserveMath = false } = {}) {
    const lines  = md.split('\n');
    const slides = [];
    let cur      = null;

    // Regex matches notes labels in the source markdown (language-agnostic slug list)
    const notesPattern = /(?:^|\s)(?:Notes?|Notas?|Notas del maestro):\s*([\s\S]+)/i;

    // ---------- Table helpers ----------
    const isTableLine = (line) => /^\s*\|.*\|\s*$/.test(line);
    const isSeparatorLine = (line) => /^\s*\|?\s*:?-{3,}:?\s*(\|\s*:?-{3,}:?\s*)*\|?\s*$/.test(line);

    function parseCells(line) {
        return line
            .trim()
            .replace(/^\|/, '')
            .replace(/\|$/, '')
            .split('|')
            .map((c) => c.replace(/\*\*/g, '').trim());
    }

    function newSlide(title) {
        return {
            title, bullets: [], body: '', tables: [],
            imageAlt: '', imageUrl: '', linkText: '', linkUrl: '',
            align: 'left', notes: ''
        };
    }

    function flush() {
        if (!cur) return;
        const image       = cur.body.match(/!\[([^\]]*)\]\(([^)\s]+)\)/);
        const link        = cur.body.match(/\[([^\]]+)\]\((https?:\/\/[^)\s]+)\)/);
        const alignMatch  = cur.body.match(/<!-- align:\s*(left|center|right|justify)\s*-->/i);
        const layoutMatch = cur.body.match(/<!-- layout:\s*([\s\S]*?)\s*-->/i);
        const styleMatch  = cur.body.match(/<!-- style:\s*([\s\S]*?)\s*-->/i);
        const typeMatch   = cur.body.match(/<!-- type:\s*([a-z-]+)\s*-->/i);
        const claimMatch  = cur.body.match(/<!-- claim:\s*([\s\S]*?)\s*-->/i);
        const visualMatch = cur.body.match(/<!-- visual:\s*([\s\S]*?)\s*-->/i);
        const notes       = cur.body.match(notesPattern);

        cur.imageAlt    = image?.[1]  || '';
        cur.imageUrl    = image?.[2]  || '';
        cur.linkText    = link?.[1]   || '';
        cur.linkUrl     = link?.[2]   || '';
        cur.align       = alignMatch?.[1] || 'left';
        cur.layout      = parseSlideLayout(layoutMatch?.[1]);
        cur.style       = parseSlideStyle(styleMatch?.[1]);
        cur.type        = normalizeSlideType(typeMatch?.[1], slides.length);
        cur.claim       = claimMatch?.[1]?.trim()  || '';
        /*cur.visualIntent = visualMatch?.[1]?.trim() || ''; */
        cur.notes       = notes?.[1]?.trim()       || '';

        cur.body = cur.body
            .replace(/!\[[^\]]*\]\([^)]+\)/g, ' ')
            .replace(/\[([^\]]+)\]\((https?:\/\/[^)\s]+)\)/g, '$1')
            .replace(/<!-- align:\s*(left|center|right|justify)\s*-->/gi, ' ')
            .replace(/<!-- layout:\s*[\s\S]*?\s*-->/gi, ' ')
            .replace(/<!-- style:\s*[\s\S]*?\s*-->/gi, ' ')
            .replace(/<!-- type:\s*[a-z-]+\s*-->/gi, ' ')
            .replace(/<!-- claim:\s*[\s\S]*?\s*-->/gi, ' ')
            .replace(/<!-- visual:\s*[\s\S]*?\s*-->/gi, ' ')
            .replace(notesPattern, ' ')
            .trim();

        // Convert LaTeX to readable text ONLY when we're not preserving it for rasterization.
        if (!preserveMath) {
            cur.title   = latexToReadable(cur.title);
            cur.body    = latexToReadable(cur.body);
            cur.claim   = latexToReadable(cur.claim);
            cur.bullets = cur.bullets.map(latexToReadable).filter(Boolean);
        } else {
            cur.bullets = cur.bullets.filter(Boolean);
        }

        slides.push(cur);
        cur = null;
    }

    const defaultTitle = t('export-utils:defaults.slideTitle');

    for (let i = 0; i < lines.length; i++) {
        const raw  = lines[i];
        const line = raw.replace(/\*\*/g, '').replace(/^[#]+\s*/, m => m);

        // Any heading level (h1–h6) starts a new slide
        const heading = line.match(/^#{1,6}\s+(.+)/);
        if (heading) {
            flush();
            const titleText = stripSlideNumberLabels(heading[1])
                .replace(/\*\*/g, '')
                .replace(/^[\s-]+|[\s-]+$/g, '') // trim decorative dashes: "--- TITLE ---"
                .trim();
            cur = newSlide(titleText);
            continue;
        }

        // A horizontal rule (---) also ends the current slide
        if (/^\s*-{3,}\s*$/.test(line)) {
            flush();
            continue;
        }

        if (!cur) {
            cur = newSlide(defaultTitle);
        }

        // Table block: pipe line followed by a |---|---| separator
        if (isTableLine(raw) && i + 1 < lines.length && isSeparatorLine(lines[i + 1])) {
            const header = parseCells(raw);
            const rows = [];
            let j = i + 2;
            while (j < lines.length && isTableLine(lines[j])) {
                // Normalize row length to the header's column count
                const cells = parseCells(lines[j]).slice(0, header.length);
                while (cells.length < header.length) cells.push('');
                rows.push(cells);
                j++;
            }
            cur.tables.push({ header, rows });
            i = j - 1; // skip the consumed lines
            continue;
        }

        const bullet = line.match(/^\s*[-*]\s+(.+)/);
        const num    = line.match(/^\s*(\d+)[.)]\s+(.+)/);
        const option = line.match(/^\s*([A-Da-d])[.)]\s+(.+)/);

        if (bullet) {
            cur.bullets.push(bullet[1].replace(/\*\*/g, '').trim());
        } else if (num) {
            // Keep the original number so exam questions stay correctly numbered
            cur.bullets.push(`${num[1]}. ${num[2].replace(/\*\*/g, '').trim()}`);
        } else if (option && cur.bullets.length) {
            // Answer options (A. / B. / …) stay attached to their question as sub-lines
            cur.bullets[cur.bullets.length - 1] +=
                `\n${option[1].toUpperCase()}. ${option[2].replace(/\*\*/g, '').trim()}`;
        } else if (/_{4,}/.test(line)) {
            // Write-in blanks ("Espacio para trabajo: ____") don't belong on slides
            continue;
        } else if (line.trim()) {
            cur.body += line.trim() + ' ';
        }
    }
    flush();
    return slides.filter(item => item.title || item.bullets.length || item.body || item.tables.length);
}

// ─────────── Slide pagination ───────────

// Rough visual-line estimate for a bullet (multi-line bullets count each line,
// and long lines count wrapped lines at ~60 chars per line).
function estimateBulletLines(bullet = '') {
    return String(bullet)
        .split('\n')
        .reduce((n, l) => n + Math.max(1, Math.ceil(l.length / 60)), 0);
}

const MAX_LINES_PER_SLIDE = 11;

/**
 * Splits overflowing slides into continuation slides so content never
 * piles up on a single slide. Bullets are kept whole (a question and its
 * options never split across slides). Continuations get "(i/n)" titles.
 */
function paginateSlides(slides) {
    const out = [];
    for (const slide of slides) {
        const totalLines = slide.bullets.reduce((n, b) => n + estimateBulletLines(b), 0);
        if (totalLines <= MAX_LINES_PER_SLIDE || slide.bullets.length <= 1) {
            out.push(slide);
            continue;
        }

        const chunks = [];
        let chunk = [];
        let lines = 0;
        for (const b of slide.bullets) {
            const bLines = estimateBulletLines(b);
            if (chunk.length && lines + bLines > MAX_LINES_PER_SLIDE) {
                chunks.push(chunk);
                chunk = [];
                lines = 0;
            }
            chunk.push(b);
            lines += bLines;
        }
        if (chunk.length) chunks.push(chunk);

        chunks.forEach((bullets, i) => {
            out.push({
                ...slide,
                bullets,
                body:   i === 0 ? slide.body   : '',
                tables: i === 0 ? slide.tables : [],
                notes:  i === 0 ? slide.notes  : '',
                title:  chunks.length > 1 ? `${slide.title} (${i + 1}/${chunks.length})` : slide.title,
            });
        });
    }
    return out;
}

// ─────────── Layout / style parsing ───────────

const SLIDE_CANVAS   = { w: 1280, h: 720 };
const PPTX_CANVAS    = { w: 13.333, h: 7.5 };
const DEFAULT_SLIDE_LAYOUT = {
    title: { x: 86,  y: 66,  w: 1110, h: 92  },
    body:  { x: 100, y: 178, w: 1080, h: 438 },
    image: { x: 824, y: 278, w: 320,  h: 230 },
};
const FONT_OPTIONS        = ['Poppins', 'Sora', 'Overlock', 'Georgia', 'Verdana'];
const DEFAULT_SLIDE_STYLE = { fontFace: 'Poppins', bodySize: 18, titleSize: 30 };

function parseSlideLayout(raw) {
    if (!raw) return null;
    try { return JSON.parse(raw); } catch { return null; }
}

function parseSlideStyle(raw) {
    if (!raw) return null;
    try { return JSON.parse(raw); } catch { return null; }
}

function normalizeSlideLayout(layout = {}) {
    return {
        title: normalizeBox(layout?.title, DEFAULT_SLIDE_LAYOUT.title),
        body:  normalizeBox(layout?.body,  DEFAULT_SLIDE_LAYOUT.body),
        image: normalizeBox(layout?.image, DEFAULT_SLIDE_LAYOUT.image),
    };
}

function normalizeSlideStyle(style = {}) {
    const bodySize  = clampNumber(style?.bodySize,  13, 26, DEFAULT_SLIDE_STYLE.bodySize);
    const titleSize = clampNumber(style?.titleSize, 20, 42, Math.max(bodySize + 10, DEFAULT_SLIDE_STYLE.titleSize));
    const fontFace  = FONT_OPTIONS.includes(style?.fontFace) ? style.fontFace : DEFAULT_SLIDE_STYLE.fontFace;
    return { fontFace, bodySize, titleSize };
}

function normalizeBox(box, fallback) {
    const value = { ...fallback, ...(box || {}) };
    return {
        x: clampNumber(value.x, 0, SLIDE_CANVAS.w - 80, fallback.x),
        y: clampNumber(value.y, 0, SLIDE_CANVAS.h - 60, fallback.y),
        w: clampNumber(value.w, 120, SLIDE_CANVAS.w,    fallback.w),
        h: clampNumber(value.h, 42,  SLIDE_CANVAS.h,    fallback.h),
    };
}

function clampNumber(value, min, max, fallback) {
    const n = Number(value);
    if (!Number.isFinite(n)) return fallback;
    return Math.max(min, Math.min(max, n));
}

function pxBoxToInches(box) {
    return {
        x: (box.x / SLIDE_CANVAS.w) * PPTX_CANVAS.w,
        y: (box.y / SLIDE_CANVAS.h) * PPTX_CANVAS.h,
        w: (box.w / SLIDE_CANVAS.w) * PPTX_CANVAS.w,
        h: (box.h / SLIDE_CANVAS.h) * PPTX_CANVAS.h,
    };
}