// Turns the LaTeX the AI tools write into plain text

import { INLINE_MATH_RE } from '../tool-modal-utils';

const SYMBOLS = {
    times: '×', div: '÷', cdot: '·', ast: '*', pm: '±', mp: '∓',
    le: '≤', leq: '≤', ge: '≥', geq: '≥', ne: '≠', neq: '≠', lt: '<', gt: '>',
    approx: '≈', equiv: '≡', cong: '≅', sim: '~', propto: '∝',
    to: '→', rightarrow: '→', leftarrow: '←', Rightarrow: '⇒', Leftrightarrow: '⇔',
    infty: '∞', circ: '°', degree: '°', angle: '∠', measuredangle: '∠', triangle: '△',
    perp: '⊥', parallel: '∥', ldots: '…', dots: '…', cdots: '…',
    cup: '∪', cap: '∩', in: '∈', notin: '∉', subset: '⊂', subseteq: '⊆', emptyset: '∅',
    alpha: 'α', beta: 'β', gamma: 'γ', delta: 'δ', Delta: 'Δ', epsilon: 'ε', theta: 'θ',
    lambda: 'λ', mu: 'μ', pi: 'π', rho: 'ρ', sigma: 'σ', Sigma: 'Σ', phi: 'φ', omega: 'ω', Omega: 'Ω',
    quad: ' ', qquad: ' ',
};

const OPERATORS = new Set(['×', '÷', '·', '±', '∓', '≤', '≥', '≠', '≈', '≡', '≅', '→', '←', '⇒', '⇔']);

const DROPPED = new Set([
    'left', 'right', 'middle', 'displaystyle', 'textstyle', 'limits',
    'big', 'Big', 'bigg', 'Bigg', 'bigl', 'bigr', 'Bigl', 'Bigr',
]);

const WRAPPERS = new Set([
    'text', 'textrm', 'textbf', 'textit', 'textnormal', 'mbox', 'mathrm', 'mathbf', 'mathit', 'mathsf',
    'mathbb', 'mathcal', 'operatorname', 'boxed', 'overline', 'underline', 'bar', 'vec', 'hat',
    'widehat', 'overrightarrow', 'overleftrightarrow',
]);

const FRACTIONS = new Set(['frac', 'dfrac', 'tfrac', 'cfrac']);

const ESCAPED_SPACES = { ',': ' ', ';': ' ', ':': ' ', '!': '', ' ': ' ' };

const SUPERSCRIPTS = {
    0: '⁰', 1: '¹', 2: '²', 3: '³', 4: '⁴', 5: '⁵', 6: '⁶', 7: '⁷', 8: '⁸', 9: '⁹',
    '+': '⁺', '-': '⁻', '=': '⁼', '(': '⁽', ')': '⁾', n: 'ⁿ', i: 'ⁱ',
};

const SUBSCRIPTS = {
    0: '₀', 1: '₁', 2: '₂', 3: '₃', 4: '₄', 5: '₅', 6: '₆', 7: '₇', 8: '₈', 9: '₉',
    '+': '₊', '-': '₋', '=': '₌', '(': '₍', ')': '₎',
    a: 'ₐ', e: 'ₑ', h: 'ₕ', k: 'ₖ', l: 'ₗ', m: 'ₘ', n: 'ₙ', o: 'ₒ', p: 'ₚ', s: 'ₛ', t: 'ₜ', x: 'ₓ',
};

const SINGLE_TERM = /^-?[0-9A-Za-zÁÉÍÓÚÑáéíóúñ.,π⁰¹²³⁴⁵⁶⁷⁸⁹]+$/;

const has = (map, key) => Object.prototype.hasOwnProperty.call(map, key);
const term = (text) => (SINGLE_TERM.test(text) ? text : `(${text})`);

// converts a single LaTeX expression to plain text
const script = (text, map, mark) => {
    const chars = [...text];
    if (chars.length && chars.every(c => has(map, c))) return chars.map(c => map[c]).join('');
    return chars.length > 1 ? `${mark}(${text})` : `${mark}${text}`;
};

// Reads the argument of a fraction, root, text command, exponent or subscript
const readArg = (latex, start) => {
    let i = start;
    while (latex[i] === ' ') i++;
    if (latex[i] === '{') {
        let depth = 0;
        for (let j = i; j < latex.length; j++) {
            if (latex[j] === '\\') j++;
            else if (latex[j] === '{') depth++;
            else if (latex[j] === '}' && --depth === 0) return [latex.slice(i + 1, j), j + 1];
        }
        return [latex.slice(i + 1), latex.length];
    }
    if (latex[i] === '\\') {
        const command = /^\\+(?:[a-zA-Z]+|.)/.exec(latex.slice(i));
        if (command) return [command[0], i + command[0].length];
    }
    return [latex[i] || '', i + 1];
};


// Converts the LaTeX of one formula to plain text, calling itself for nested parts
const convert = (latex) => {
    let out = '';
    let i = 0;
    while (i < latex.length) {
        const char = latex[i];

        if (char === '\\') {
            // A run of backslashes counts as one, the math tool's own prompt shows "\\frac"
            let j = i;
            while (latex[j] === '\\') j++;
            const name = (/^[a-zA-Z]+/.exec(latex.slice(j)) || [''])[0];
            if (!name) {
                const next = latex[j] || '';
                out += has(ESCAPED_SPACES, next) ? ESCAPED_SPACES[next] : next;
                i = j + 1;
                continue;
            }
            i = j + name.length;

            if (FRACTIONS.has(name)) {
                const [numerator, afterNumerator] = readArg(latex, i);
                const [denominator, afterDenominator] = readArg(latex, afterNumerator);
                i = afterDenominator;
                if (/\d$/.test(out)) out += ' ';
                out += `${term(convert(numerator).trim())}/${term(convert(denominator).trim())}`;
            } else if (name === 'sqrt') {
                let index = '';
                const close = latex[i] === '[' ? latex.indexOf(']', i) : -1;
                if (close > i) {
                    index = convert(latex.slice(i + 1, close)).trim();
                    i = close + 1;
                }
                const [radicand, next] = readArg(latex, i);
                i = next;
                out += `${index ? script(index, SUPERSCRIPTS, '') : ''}√${term(convert(radicand).trim())}`;
            } else if (WRAPPERS.has(name)) {
                const [argument, next] = readArg(latex, i);
                i = next;
                out += convert(argument);
            } else if (has(SYMBOLS, name)) {
                out += OPERATORS.has(SYMBOLS[name]) ? ` ${SYMBOLS[name]} ` : SYMBOLS[name];
            } else if (!DROPPED.has(name)) {
                out += name;
            }
            continue;
        }

        if (char === '^' || char === '_') {
            const [argument, next] = readArg(latex, i + 1);
            i = next;
            const text = convert(argument).trim();
            if (char === '^' && text === '°') out += '°';
            else out += char === '^' ? script(text, SUPERSCRIPTS, '^') : script(text, SUBSCRIPTS, '_');
            continue;
        }

        // Grouping braces print nothing
        if (char !== '{' && char !== '}') out += char;
        i++;
    }
    return out;
};

const toText = (latex) => convert(latex).replace(/[ \t]{2,}/g, ' ').trim();

// Replaces LaTeX in a markdown string with its plain text form, leaving the rest of the markdown intact
export const latexToPlainText = (markdown) => String(markdown || '')
    .replace(/\$\$([\s\S]+?)\$\$/g, (_, tex) => toText(tex))
    .replace(/\\\[([\s\S]+?)\\\]/g, (_, tex) => toText(tex))
    .replace(INLINE_MATH_RE, (_, tex, padded) => toText(tex ?? padded))
    .replace(/\\\(([^\n]+?)\\\)/g, (_, tex) => toText(tex))
    .replace(/\\+[dtc]?frac\s*\{[^{}]*\}\s*\{[^{}]*\}/g, (tex) => toText(tex));
