/**
 * tools-config-base.js
 * Stable constants: grade list, subject list, model registry.
 * System-prompt factories accept `t` so the LLM receives instructions in the
 * active UI language and responds accordingly — Rule 1.
 */

// ---------------------------------------------------------------------------
// Models — OpenRouter slugs. Swap freely from the model selector.
// ---------------------------------------------------------------------------

export const MODELS = [
    { id: 'anthropic/claude-sonnet-4.5',  label: 'Claude Sonnet 4.5 — balanced',      tier: 'premium' },
    { id: 'anthropic/claude-opus-4.1',    label: 'Claude Opus 4.1 — highest quality', tier: 'premium' },
    { id: 'anthropic/claude-haiku-4.5',   label: 'Claude Haiku 4.5 — fast',           tier: 'fast'    },
    { id: 'openai/gpt-4o',                label: 'GPT-4o — precise',                  tier: 'premium' },
    { id: 'openai/gpt-4o-mini',           label: 'GPT-4o mini — economical',          tier: 'fast'    },
    { id: 'google/gemini-2.5-flash',      label: 'Gemini 2.5 Flash — fast',           tier: 'fast'    },
    { id: 'google/gemini-2.5-pro',        label: 'Gemini 2.5 Pro — reasoning',        tier: 'premium' },
];

// ---------------------------------------------------------------------------
// Grade and subject lists — English option values.
// Display labels for any Spanish-specific subject names live in translation files.
// ---------------------------------------------------------------------------

export const GRADES = [
    { label: 'K',    value: 'k' },
    { label: '1st',  value: '1' },
    { label: '2nd',  value: '2' },
    { label: '3rd',  value: '3' },
    { label: '4th',  value: '4' },
    { label: '5th',  value: '5' },
    { label: '6th',  value: '6' },
    { label: '7th',  value: '7' },
    { label: '8th',  value: '8' },
    { label: '9th',  value: '9' },
    { label: '10th', value: '10' },
    { label: '11th', value: '11' },
    { label: '12th', value: '12' },
];

export const SUBJECTS = [
    { label: 'Mathematics',          value: 'mat-sp' },
    { label: 'Science',              value: 'sci-sp' },
    { label: 'Spanish / Language Arts', value: 'sp' },
    { label: 'English / ELA',        value: 'en' },
    { label: 'Social Studies',       value: 'sci-so' },
    { label: 'Art',                  value: 'av-sp' },
    { label: 'Music',                value: 'mu-sp' },
    { label: 'Physical Education',   value: 'edf-sp' }
];

// ---------------------------------------------------------------------------
// System prompt factories — must accept `t` so prompts are rendered in the
// active UI language. The LLM then responds in that same language.
// ---------------------------------------------------------------------------

/**
 * Core teacher assistant system prompt.
 * @param {Function} t - i18next t function from the calling component
 */
export function getSystemTeacher(t) {
    return t('tools-config:system.teacher');
}

/**
 * Append-only image generation instructions for tools that support [IMAGE: ...] tags.
 * @param {Function} t - i18next t function from the calling component
 */
export function getImageInstructions(t) {
    return t('tools-config:system.imageInstructions');
}

// Instructions for tools that do not support image generation.
export function getNoImageInstructions(t) {
    return t('tools-config:system.noImages');
}
