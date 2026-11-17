/**
 * tool-modal-config-adapter.js
 *
 * Drop-in helpers that bridge the new tools-config.js API into tool-modal.jsx
 * without touching every call site. Import these two functions and replace the
 * two lines inside `generate()` that build the system prompt and user prompt.
 *
 * BEFORE (tool-modal.jsx lines that must change):
 *   let systemPrompt = buildSystemWithStandards();          // used tool.system
 *   const rawUserPrompt = tool.buildPrompt(values);         // old single-arg form
 *
 * AFTER:
 *   import { resolveSystem, resolveUserPrompt } from './tool-modal-config-adapter.js';
 *   let systemPrompt = resolveSystem(tool, t, values);
 *   const rawUserPrompt = resolveUserPrompt(tool, values, t);
 *
 * Also update the field-rendering loop in tool-modal.jsx:
 *   - Replace `field.label` with `t(field.labelKey)` (or keep both for fallback).
 *   - Replace `field.placeholder` with `t(field.placeholderKey)`.
 *   - For select fields replace `field.options` with
 *     `field.optionKeys.map(key => ({ value: key, label: t(\`tools-config:options.\${key}\`) }))`.
 *   - For the default value of a select field use
 *     `field.defaultKey ? t(field.defaultKey) : field.default` only in the initial
 *     values computation; the stored VALUE is always the English slug.
 */

/**
 * Build the full system prompt for a tool, including optional standards block.
 *
 * @param {object}   tool   - Tool config object from TOOLS
 * @param {Function} t      - i18next t function
 * @param {object}   values - Current field values (for standards injection)
 * @returns {string}
 */
export function resolveSystem(tool, t, values = {}) {
    // New API: tool.buildSystem(t) — replaces the static tool.system string
    let sys = typeof tool.buildSystem === 'function'
        ? tool.buildSystem(t)
        : (tool.system || '');  // backwards-compat with any legacy tool.system string

    // Inject selected standards (same logic as buildSystemWithStandards in tool-modal.jsx)
    const standardsFields = (tool.fields || []).filter(f => f.type === 'standards');
    const allStandards = [];
    for (const field of standardsFields) {
        const selected = values[field.name];
        if (Array.isArray(selected) && selected.length) allStandards.push(...selected);
    }
    if (allStandards.length) {
        const block = allStandards.map(s => `- **${s.Code}** — ${s.Expectation}`).join('\n');
        sys += t('tool-modal:standards.systemBlock', { block, interpolation: { escapeValue: false } });
    }
    return sys;
}

/**
 * Build the raw user prompt for a tool.
 *
 * @param {object}   tool   - Tool config object from TOOLS
 * @param {object}   values - Current field values
 * @param {Function} t      - i18next t function
 * @returns {string}
 */
export function resolveUserPrompt(tool, values, t) {
    // New API: tool.buildPrompt(f, t) — second arg is t
    return tool.buildPrompt(values, t);
}
