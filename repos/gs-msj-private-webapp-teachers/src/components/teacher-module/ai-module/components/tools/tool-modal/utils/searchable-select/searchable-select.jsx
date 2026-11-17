/**
 * searchable-select.jsx
 * Drop-in replacement for <select> with type-to-filter.
 *
 * - options: [{ value, label }] — same shape resolveOptions() returns.
 * - value / onChange(value): stores the option VALUE (code/slug), never the label.
 * - Filtering is case- AND accent-insensitive ("matematicas" finds "Matemáticas").
 *   Prefix matches are ranked above substring matches.
 * - Keyboard: ↑/↓ move, Enter selects, Esc closes (without closing the modal),
 *   Home/End jump, Tab closes.
 * - The dropdown list is portaled to <body> with fixed positioning so it is
 *   never clipped by the modal's overflow containers; it flips upward when
 *   there is not enough room below.
 * - All strings come in via props (translate them with t() in the caller).
 */
import React from 'react';
import { createPortal } from 'react-dom';
import './searchable-select.css';
const LIST_MAX_HEIGHT = 260;
let idCounter = 0;

/** Lowercase + strip diacritics. Kept 1:1-ish per char for highlighting. */
export const fold = (s) =>
    String(s ?? '').normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();

/** Wrap the first accent-insensitive match of `q` in <mark>. */
function highlight(label, q) {
    const text = String(label ?? '');
    if (!q) return text;
    const chars = Array.from(text);
    let norm = '';
    const map = []; // index in `norm` → index in `chars`
    chars.forEach((c, i) => {
        const f = fold(c);
        for (let k = 0; k < f.length; k++) map.push(i);
        norm += f;
    });
    const idx = norm.indexOf(q);
    if (idx === -1) return text;
    const start = map[idx];
    const end = map[idx + q.length - 1] + 1;
    return (
        <>
            {chars.slice(0, start).join('')}
            <mark className="tm-ss-mark">{chars.slice(start, end).join('')}</mark>
            {chars.slice(end).join('')}
        </>
    );
}

export default function SearchableSelect({
    value,
    options = [],
    onChange,
    disabled = false,
    placeholder = '',
    noResultsText = '',
    clearLabel = '',
    ariaLabel,
    className = '',
}) {
    const [open,        setOpen]        = React.useState(false);
    const [query,       setQuery]       = React.useState('');
    const [activeIndex, setActiveIndex] = React.useState(0);
    const [pos,         setPos]         = React.useState(null);

    const wrapRef  = React.useRef(null);
    const inputRef = React.useRef(null);
    const listRef  = React.useRef(null);
    const listId   = React.useRef(`tm-ss-list-${++idCounter}`).current;

    // Compare as strings: native <select> did the same, and API codes may be numbers.
    const selected = options.find(o => String(o.value) === String(value ?? ''));
    const q = fold(query).trim();

    const filtered = React.useMemo(() => {
        if (!q) return options;
        const prefix = [];
        const contains = [];
        for (const o of options) {
            const l = fold(o.label);
            if (l.startsWith(q)) prefix.push(o);
            else if (l.includes(q)) contains.push(o);
        }
        return [...prefix, ...contains];
    }, [options, q]);

    // ---------------------------------------------------------------------
    // Open / close / select
    // ---------------------------------------------------------------------

    const openList = () => {
        if (disabled || open) return;
        setQuery('');
        const idx = options.findIndex(o => String(o.value) === String(value ?? ''));
        setActiveIndex(idx >= 0 ? idx : 0);
        setOpen(true);
    };

    const closeList = () => {
        setOpen(false);
        setQuery('');
    };

    const choose = (opt) => {
        if (!opt) return;
        onChange?.(opt.value);
        closeList();
    };

    const clear = (e) => {
        e.preventDefault();
        e.stopPropagation();
        onChange?.('');
        closeList();
        inputRef.current?.focus();
    };

    // Close if the field becomes disabled while open (e.g. options reloading).
    React.useEffect(() => { if (disabled) closeList(); }, [disabled]);

    // ---------------------------------------------------------------------
    // Positioning (fixed, portaled) — recomputed on scroll/resize while open
    // ---------------------------------------------------------------------

    React.useLayoutEffect(() => {
        if (!open) return undefined;
        const update = () => {
            const r = wrapRef.current?.getBoundingClientRect();
            if (!r) return;
            const below = window.innerHeight - r.bottom;
            const above = r.top;
            const flip = below < Math.min(LIST_MAX_HEIGHT, 200) && above > below;
            setPos({
                left: r.left,
                width: r.width,
                top: flip ? undefined : r.bottom + 4,
                bottom: flip ? window.innerHeight - r.top + 4 : undefined,
                maxHeight: Math.max(120, Math.min(LIST_MAX_HEIGHT, (flip ? above : below) - 12)),
            });
        };
        update();
        window.addEventListener('resize', update);
        window.addEventListener('scroll', update, true); // capture: catches inner scroll containers
        return () => {
            window.removeEventListener('resize', update);
            window.removeEventListener('scroll', update, true);
        };
    }, [open]);

    // Keep the active option visible while arrowing through the list.
    React.useEffect(() => {
        if (!open) return;
        const el = listRef.current?.querySelector(`[data-index="${activeIndex}"]`);
        el?.scrollIntoView({ block: 'nearest' });
    }, [activeIndex, open]);

    // ---------------------------------------------------------------------
    // Keyboard
    // ---------------------------------------------------------------------

    const onKeyDown = (e) => {
        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                if (!open) openList();
                else setActiveIndex(i => Math.min(i + 1, filtered.length - 1));
                break;
            case 'ArrowUp':
                e.preventDefault();
                if (!open) openList();
                else setActiveIndex(i => Math.max(i - 1, 0));
                break;
            case 'Home':
                if (open) { e.preventDefault(); setActiveIndex(0); }
                break;
            case 'End':
                if (open) { e.preventDefault(); setActiveIndex(filtered.length - 1); }
                break;
            case 'Enter':
                // Always block: Enter inside the form would otherwise trigger Generate.
                e.preventDefault();
                if (open) choose(filtered[activeIndex]);
                else openList();
                break;
            case 'Escape':
                if (open) {
                    // Stop the modal's window-level Escape handler from closing the modal.
                    e.preventDefault();
                    e.stopPropagation();
                    closeList();
                }
                break;
            case 'Tab':
                if (open) closeList();
                break;
            default:
                break;
        }
    };

    // ---------------------------------------------------------------------
    // Render
    // ---------------------------------------------------------------------

    const activeId = open && filtered[activeIndex] ? `${listId}-opt-${activeIndex}` : undefined;

    const list = open && pos && typeof document !== 'undefined' && createPortal(
        <ul
            ref={listRef}
            id={listId}
            role="listbox"
            className="tm-ss-list"
            style={{
                position: 'fixed',
                left: pos.left,
                width: pos.width,
                top: pos.top,
                bottom: pos.bottom,
                maxHeight: pos.maxHeight,
            }}
            // Keep focus in the input so blur doesn't close the list on click / scrollbar drag.
            onMouseDown={(e) => e.preventDefault()}
        >
            {filtered.length === 0 ? (
                <li className="tm-ss-empty" role="presentation">{noResultsText}</li>
            ) : filtered.map((opt, i) => {
                const isSelected = String(opt.value) === String(value ?? '');
                return (
                    <li
                        key={opt.value}
                        id={`${listId}-opt-${i}`}
                        data-index={i}
                        role="option"
                        aria-selected={isSelected}
                        className={
                            'tm-ss-option'
                            + (i === activeIndex ? ' active' : '')
                            + (isSelected ? ' selected' : '')
                        }
                        title={opt.label}
                        onMouseEnter={() => setActiveIndex(i)}
                        onClick={() => choose(opt)}
                    >
                        {highlight(opt.label, q)}
                    </li>
                );
            })}
        </ul>,
        document.body
    );

    return (
        <div
            ref={wrapRef}
            className={`tm-ss ${open ? 'open' : ''} ${disabled ? 'disabled' : ''} ${className}`}
        >
            <input
                ref={inputRef}
                type="text"
                role="combobox"
                aria-label={ariaLabel}
                aria-expanded={open}
                aria-controls={listId}
                aria-autocomplete="list"
                aria-activedescendant={activeId}
                autoComplete="off"
                spellCheck={false}
                disabled={disabled}
                // Closed: show the chosen label. Open: show what the user types,
                // with the current label as a hint in the placeholder.
                value={open ? query : (selected?.label ?? '')}
                placeholder={open && selected ? selected.label : placeholder}
                title={open ? '' : selected?.label}
                onFocus={openList}
                onClick={openList}
                onBlur={closeList}
                onChange={(e) => {
                    setQuery(e.target.value);
                    setActiveIndex(0);
                    if (!open) setOpen(true);
                }}
                onKeyDown={onKeyDown}
            />

            {selected && !disabled && (
                <button
                    type="button"
                    className="tm-ss-clear"
                    tabIndex={-1}
                    aria-label={clearLabel}
                    title={clearLabel}
                    onMouseDown={clear}
                >
                    ×
                </button>
            )}
            <span className="tm-ss-caret" aria-hidden="true">▾</span>

            {list}
        </div>
    );
}
