/**
 * tools-config.js
 * Tool definitions: fields, system prompts, and prompt builders.
 *
 * Rules enforced here:
 *  - All object keys / field name slugs are English identifiers.
 *  - All human-visible strings (labels, placeholders, option labels) come
 *    from t() at call time — never hardcoded in this file.
 *  - All LLM-bound strings (system prompts, buildPrompt output) come from
 *    t() so the AI responds in the active UI language — Rule 1.
 *  - `subtitle` fields removed: display text lives in aiTools translation
 *    files keyed by tool key, per the existing architecture comment.
 *  - `title` kept as an English fallback only (see comment below).
 *
 * Field schema:
 *   name        — English slug identifier (matched by tool-modal.jsx)
 *   labelKey    — i18next key resolved at render time for the visible label
 *   type        — 'text' | 'textarea' | 'select' | 'number' | 'standards' | 'checkbox'
 *                 | 'multiselect' (array of option values, shown as removable chips)
 *   excludeField — multiselect only: hides the option equal to that field's value (e.g. the main subject)
 *   allowCustom / customPlaceholderKey — multiselect only: the teacher can type entries of their own,
 *                 stored as their text next to the option slugs
 *   showIf      — { field, equals } or { field, notEmpty: true } (shown while that list field has items)
 *   headingKey  — checkbox only: heading shown above it, for a group of checkboxes
 *
 * Every tool also gets a `language` select (see addContentLanguage at the end of this file):
 *   defaultLanguage — tool-level 'lang-es' (default) or 'lang-en' for English-related tools.
 *   Choosing an English subject switches it to English (tool-modal.jsx, getDefaultLanguage).
 *   optionKeys  — for type:'select', array of English slugs; labels via
 *                 t('tools-config:options.<slug>') at render time
 *   placeholderKey — i18next key for the placeholder string
 *   defaultKey  — i18next key for the default *display* value of a select
 *   default     — raw non-string default (numbers, booleans)
 *   required    — boolean
 *   rows        — number (textarea only)
 *   subjectField / gradeField / subject / grade — for standards pickers
 *   codeType    — standards picker kind: 'standards' (default), 'expectations' (type 7) or 'units' (type 2)
 *   standardsField — for an expectations picker, the standards field whose selection groups its list
 *
 * buildPrompt(f, t) — second param is the i18next t function.
 *   f keys are the English field name slugs above.
 */

import { getSystemTeacher, getImageInstructions, getNoImageInstructions } from './tools-config-base.js';

// ---------------------------------------------------------------------------
// Shared option key arrays (English slugs → labels in translation files)
// ---------------------------------------------------------------------------

export const DEFAULT_AI_MODEL = "gpt-4o-mini"

// Field types whose form value is an array
const ARRAY_FIELD_TYPES = ['standards', 'multiselect'];

// Initial form value of a field, to avoid undefined in the form state
export const getFieldInitialValue = (field) => {
    if (field.default != null) return field.default;
    const slug = field.optionKeys && field.defaultKey?.startsWith('options.') ? field.defaultKey.slice('options.'.length) : null;
    if (slug && field.optionKeys.includes(slug)) return slug;
    return ARRAY_FIELD_TYPES.includes(field.type) ? [] : '';
}

const PROFILE_OPTIONS = [
    'ell-beginner', 'ell-intermediate',
    'sped-dyslexia', 'sped-adhd', 'sped-autism',
    'academic-lag-1grade', 'academic-lag-2grades',
    'academic-lag-prerequisites', 'advanced',
];

const GROUPING_OPTIONS = [
    'homogeneous', 'heterogeneous', 'by-interest', 'random-strategic',
];

const TONE_OPTIONS = ['warm-motivating', 'direct-professional', 'coaching'];

const LEVELS_OPTIONS   = ['3-levels', '4-levels', '5-levels'];
const GENRE_OPTIONS    = ['informative', 'narrative', 'descriptive', 'persuasive'];
const LENGTH_OPTIONS   = ['short-150', 'medium-300', 'long-500'];
const LEXILE_OPTIONS   = ['BR-200L', '200L-500L', '500L-800L', '800L-1000L', '1000L-1200L', '1200L+'];

const SCHOOL_MOMENT_OPTIONS = [
    'start-of-year', 'before-new-unit', 'mid-year-review', 'gap-closing',
];

const LAG_SEVERITY_OPTIONS = [
    '1-grade-behind', '2-grades-behind', '3plus-grades-behind', 'specific-gaps',
];

const INTERVENTION_TIME_OPTIONS = [
    '10-15min-daily', '20-30min-3x-week', '45-60min-pullout-2x', '1hr-daily-block',
];

const ASSESSMENT_TYPE_OPTIONS = [
    'formative', 'summative', 'performance-based', 'exit-ticket', 'short-quiz',
];

const ASSESSMENT_FORMAT_OPTIONS = [
    'multiple-choice', 'short-answer', 'open-essay', 'performance-task', 'mixed',
];

const ELL_LEVEL_OPTIONS = [
    'entering', 'emerging', 'developing', 'expanding', 'bridging',
];

const DOK_OPTIONS = [
    'dok-1', 'dok-2', 'dok-3', 'dok-4', 'dok-all',
];

// plan-my-lesson innovative initiative or project types
const INNOVATION_PROJECT_OPTIONS = [
    'project-technology', 'project-pbl', 'project-stem', 'project-steam',
    'project-innovation', 'project-entrepreneurship',
];

// plan-my-lesson reasonable accommodations. The prefix is the category: special education (sped),
// LSP, Section 504 and gifted. Each label includes its category, e.g. "Educación Especial — Tiempo adicional"
const ACCOMMODATION_OPTIONS = [
    'acc-sped-extra-time', 'acc-sped-seating', 'acc-sped-return-test', 'acc-sped-teacher-support',
    'acc-sped-chunk-work', 'acc-sped-large-print', 'acc-sped-oral-tests', 'acc-sped-short-assignments',
    'acc-sped-supplementary-service', 'acc-sped-frequent-review', 'acc-sped-positive-reinforcement',
    'acc-lsp-dictionary', 'acc-lsp-english-spanish', 'acc-lsp-tutor', 'acc-lsp-wida-code', 'acc-lsp-dle5-history',
    'acc-504-seating', 'acc-504-answer-on-test', 'acc-504-extra-time',
    'acc-gifted-acceleration',
];

// plan-my-lesson improvement plan: active student response types
const ACTIVE_RESPONSE_OPTIONS = [
    'response-teacher-led', 'response-teacher-led-group', 'response-student-led-groups',
    'response-guided-practice', 'response-individual-practice',
];

// Language the content is generated in, chosen per tool
const LANGUAGE_OPTIONS = ['lang-es', 'lang-en'];

// A subject is English when its code or label says so, e.g. "English / ELA", "Inglés / Artes del Lenguaje", "ENG"
const ENGLISH_SUBJECT = /\b(english|ingl[eé]s|ela|eng|ing)\b/i;

export const isEnglishSubject = (...texts) => texts.some(text => ENGLISH_SUBJECT.test(String(text || '')));

// Content language for a tool and subject: English for English subjects, otherwise the tool's default
export const getDefaultLanguage = (tool, subjectCode = '', subjectLabel = '') => {
    if (subjectCode && isEnglishSubject(subjectCode, subjectLabel)) return 'lang-en';
    return tool?.defaultLanguage || 'lang-es';
};

// Bloom's revised taxonomy levels: Remember, Understand, Apply, Analyze, Evaluate, Create.
// Used instead of Webb's DOK by plan-my-lesson, stem-unit-planner and differentiate-content.
const DOK_OPTIONS_BLOOM = [
    'dok-bloom-1', 'dok-bloom-2', 'dok-bloom-3', 'dok-bloom-4', 'dok-bloom-5', 'dok-bloom-6', 'dok-bloom-all'
]

// True for any DOK or Bloom level selection
const isThinkingLevel = (level) => DOK_OPTIONS.includes(level) || DOK_OPTIONS_BLOOM.includes(level);

const MASTERY_OPTIONS = [
    'mastery-needs-support', 'mastery-developing', 'mastery-at-level',
    'mastery-advanced', 'mastery-mixed',
];

const SUPPORT_NEEDS_OPTIONS = [
    'support-sped', 'support-ell-spanish', 'support-literacy',
    'support-attention', 'support-advanced', 'support-other',
];

const ASSESSMENT_PURPOSE_OPTIONS = [
    'assessment-diagnostic', 'assessment-formative', 'assessment-summative',
];

const DEMONSTRATION_FORM_OPTIONS = [
    'demo-written', 'demo-oral', 'demo-visual', 'demo-practical', 'demo-digital', 'demo-project',
];

const QUESTION_TYPE_OPTIONS = [
    'questions-open', 'questions-multiple-choice', 'questions-short-answer', 'questions-true-false', 'questions-mixed',
];

const COMPREHENSION_SKILL_OPTIONS = [
    'skill-main-idea', 'skill-sequence', 'skill-cause-effect', 'skill-compare-contrast',
    'skill-inference', 'skill-author-purpose', 'skill-point-of-view', 'skill-argument-evidence',
    'skill-summarize', 'skill-vocabulary', 'skill-other',
];

const SCIENCE_AREA_OPTIONS = [
    'science-biology', 'science-physics', 'science-chemistry', 'science-environmental', 'science-other',
];

const SOCIAL_DISCIPLINE_OPTIONS = [
    'social-history', 'social-geography', 'social-economics', 'social-other',
];

const READING_LEVEL_OPTIONS = [
    'reading-prebasic', 'reading-basic', 'reading-proficient', 'reading-advanced',
];

const COMPLEXITY_OPTIONS = [
    'complexity-basic', 'complexity-intermediate', 'complexity-advanced', 'complexity-combined',
];

const TEXT_LENGTH_OPTIONS = [
    'text-length-short', 'text-length-medium', 'text-length-long', 'text-length-custom',
];

const COMPREHENSION_BLOCKS = ['literal', 'inferential', 'critical'];

const TEXT_TYPE_OPTIONS = [
    'text-type-narrative', 'text-type-informative', 'text-type-explanatory',
    'text-type-argumentative', 'text-type-descriptive',
];

// Words requested for each text
const TEXT_LENGTH_WORDS = {
    'text-length-short': 150,
    'text-length-medium': 300,
    'text-length-long': 500,
};

// Deviation allowed around the requested words, it sets the min and max sent to the model
const TEXT_LENGTH_TOLERANCE = 0.1;

// lexile-assessment difficulty line.
// The model follows a concrete range of words per sentence better than the Lexile range name alone
const LEXILE_SENTENCE_WORDS = {
    'BR-200L': { min: 5, max: 8, cap: 12 },
    '200L-500L': { min: 8, max: 12, cap: 16 },
    '500L-800L': { min: 12, max: 16, cap: 22 },
    '800L-1000L': { min: 15, max: 19, cap: 26 },
    '1000L-1200L': { min: 18, max: 22, cap: 30 },
    '1200L+': { min: 20, max: 26, cap: 34 },
};

// Each tool has its own DOK prompt, since some tools generate items, lessons, or units, and some tools generate questions.
// The DOK instructions are different for each tool, so we need to keep them separate.
const DOK_FAMILY_NS = {
    lesson: 'dokInstructions',
    unit: 'dokInstructionsUnit',
    items: 'dokInstructionsItems',
    levels: 'dokInstructionsLevels',
    questions: 'dokQuestionsDistribution',
};

// Bloom instructions per tool family. Only the families whose tools offer Bloom levels
const BLOOM_FAMILY_NS = {
    lesson: 'bloomInstructions',
    unit: 'bloomInstructionsUnit',
    levels: 'bloomInstructionsLevels',
};

// DOK (or Bloom) block for a tool family. Empty without a level
const getDokInstructions = (dok, t, family = 'lesson', vars = {}) => {
    if (DOK_OPTIONS_BLOOM.includes(dok)) {
        const bloomNs = BLOOM_FAMILY_NS[family];
        if (!bloomNs) return '';
        return t(`tools-config:${bloomNs}.${dok}`, { ...vars, interpolation: { escapeValue: false } });
    }

    if (!DOK_OPTIONS.includes(dok)) {
        if (family !== 'questions') return '';
        // Default to all for DOK questions
        dok = 'dok-all';
    }

    const ns = DOK_FAMILY_NS[family] ?? DOK_FAMILY_NS.lesson;
    return t(`tools-config:${ns}.${dok}`, { ...vars, interpolation: { escapeValue: false } });
}

// DOK level of each marked plan-my-lesson section when "All" is selected.
const LESSON_DOK_ALL = { m5: 1, m6: 2, m7: 3, m8: 3, m10: 4 };

// Bloom levels of each marked plan-my-lesson section when "All" is selected.
// Development covers Understand and Apply so the six levels fit the five marked sections.
const LESSON_BLOOM_ALL = { m5: [1], m6: [2, 3], m7: [4], m8: [5], m10: [6] };

// [Bloom: level] mark with the translated level names, e.g. " **[Bloom: Comprender, Aplicar]**"
const getBloomMark = (levels, t) => t('tools-config:bloomMark', {
    levels: levels.map(n => t(`tools-config:options.dok-bloom-${n}`)).join(', '),
    interpolation: { escapeValue: false },
});

// Mark for one plan-my-lesson section slot, for a DOK or Bloom selection
const getLessonMark = (level, slot, t) => {
    if (level === 'dok-all') return ` **[DOK ${LESSON_DOK_ALL[slot]}]**`;
    if (DOK_OPTIONS.includes(level)) return ` **[DOK ${level.replace('dok-', '')}]**`;
    if (level === 'dok-bloom-all') return getBloomMark(LESSON_BLOOM_ALL[slot], t);
    if (DOK_OPTIONS_BLOOM.includes(level)) return getBloomMark([level.replace('dok-bloom-', '')], t);
    return '';
}

// System prompt marking rules for the selected level: the DOK key for DOK levels, the Bloom key for Bloom levels
const getLevelMarking = (level, t, dokKey, bloomKey) => {
    const key = DOK_OPTIONS.includes(level) ? dokKey : DOK_OPTIONS_BLOOM.includes(level) ? bloomKey : null;
    return key ? t(`tools-config:system.${key}`, { interpolation: { escapeValue: false } }) : '';
}

// plan-my-lesson section list: adds a [DOK n] or [Bloom: level] mark to sections 5–8 and 10,
// the class activities and homework.
const getSectionsList = (f, t) => {
    const marks = {};
    for (const slot of Object.keys(LESSON_DOK_ALL)) {
        marks[slot] = getLessonMark(f.dok, slot, t);
    }

    // If the user has selected any of the aligned standards, expectations, or units, add a line to the sections list
    const alignedKeys = [
        ((Array.isArray(f.standards) && f.standards.length) || String(f.standard || '').trim()) && 'alignedStandards',
        Array.isArray(f.expectations) && f.expectations.length && 'alignedExpectations',
        Array.isArray(f.units) && f.units.length && 'alignedUnits',
    ].filter(Boolean);
    const alignedSection = alignedKeys.map(key => `- **${t(`tools-config:defaults.${key}`)}**\n`).join('');
    const extras = getLessonExtras(f, t);
    const text = (key, vars = {}) => t(`tools-config:lessonExtras.${key}`, { ...vars, interpolation: { escapeValue: false } });

    // New sections go after Homework / extension, so the numbered sections the DOK instructions name keep their numbers
    const improvementItems = [
        extras.includeActiveResponsePlan && text('improvementItems.activeResponses'),
        extras.commonExperience && text('improvementItems.commonExperience'),
    ].filter(Boolean);
    const extraSections = [
        extras.integratedSubjects.length && text('sections.integratedSubjects'),
        extras.innovationProjects.length && text('sections.innovationProjects'),
        extras.accommodations.length && text('sections.accommodations'),
        improvementItems.length && text('sections.improvementPlan', { items: improvementItems.join(text('improvementItems.join')) }),
        extras.includeLearningCheck && text('sections.learningCheck'),
    ].filter(Boolean).map(line => `\n${line}`).join('');

    return t('tools-config:sectionsList', {
        ...marks,
        alignedSection,
        commonExperienceNote: extras.commonExperience ? text('commonExperienceNote') : '',
        extraSections,
        interpolation: { escapeValue: false },
    });
}

// Text values of a list field: trimmed, without empty entries or repeats
const cleanList = (values) => [...new Set(
    (Array.isArray(values) ? values : []).map(value => String(value ?? '').trim()).filter(Boolean)
)];

// Labels of a multiselect value: translated label for each option slug, the text itself for entries the teacher typed
const getOptionLabels = (values, optionKeys, t) => cleanList(
    cleanList(values).map(item => (optionKeys.includes(item) ? t(`tools-config:options.${item}`) : item))
);

// plan-my-lesson inputs that add sections and instructions to the prompt
const getLessonExtras = (f, t) => ({
    // integratedSubjects arrive as labels (the modal maps subject codes), the main subject is not an integration with itself
    integratedSubjects: cleanList(f.integratedSubjects).filter(subject => subject !== String(f.subject || '').trim()),
    innovationProjects: getOptionLabels(f.innovationProjects, INNOVATION_PROJECT_OPTIONS, t),
    accommodations: getOptionLabels(f.accommodations, ACCOMMODATION_OPTIONS, t),
    activeResponses: getOptionLabels(f.activeResponses, ACTIVE_RESPONSE_OPTIONS, t),
    // The checkboxes only count while there are active response types (the modal hides and clears them otherwise)
    includeActiveResponsePlan: !!f.includeActiveResponsePlan && cleanList(f.activeResponses).length > 0,
    includeLearningCheck: !!f.includeLearningCheck && cleanList(f.activeResponses).length > 0,
    commonExperience: String(f.commonExperience || '').trim(),
});

// plan-my-lesson block with the teacher's extra inputs and the rules to apply each one. Empty without inputs
const getLessonContextBlock = (f, t) => {
    const extras = getLessonExtras(f, t);
    const text = (key, vars = {}) => t(`tools-config:lessonExtras.${key}`, { ...vars, interpolation: { escapeValue: false } });
    const list = (items) => items.join(', ');

    const parts = [
        extras.integratedSubjects.length && text('integratedSubjects', {
            subjects: list(extras.integratedSubjects),
            subject: f.subject,
            grade: f.grade,
        }),
        extras.innovationProjects.length && text('innovationProjects', {
            projects: list(extras.innovationProjects),
            duration: f.duration || 60,
        }),
        extras.accommodations.length && [
            text('accommodationsHeader'),
            ...extras.accommodations.map(item => text('accommodationsItem', { item })),
            text('accommodationsRules'),
        ].join(''),
        extras.activeResponses.length && [
            text('activeResponses', { responses: list(extras.activeResponses) }),
            extras.includeActiveResponsePlan ? text('activeResponsesPlan') : '',
        ].join(''),
        extras.includeLearningCheck && text('learningCheck', { responses: list(extras.activeResponses) }),
        extras.commonExperience && text('commonExperience', { text: extras.commonExperience }),
    ].filter(Boolean);

    return parts.length ? text('intro') + parts.join('') : '';
}

// stem-unit-planner section list, one template where only the final project
// line changes with the DOK level
const getUnitSectionsList = (dok, t) => {
    const key = isThinkingLevel(dok) ? dok : 'none';
    return t('tools-config:unitSectionsList.template', {
        projectLine: t(`tools-config:unitSectionsList.project.${key}`),
        interpolation: { escapeValue: false },
    });
}

// differentiate-content tier list for the selected DOK.
const getLevelsSectionsList = (dok, t) => {
    const key = isThinkingLevel(dok) ? dok : 'none';
    return t(`tools-config:levelsSectionsList.${key}`, { interpolation: { escapeValue: false } });
}

// assessment-creator section list for the selected DOK. Fills {{count}} (max 5 for
// exit tickets) and {{cases}} (DOK 3 groups about 3 items per case).
const getAssessmentSectionsList = (f, t) => {
    const key = DOK_OPTIONS.includes(f.dok) ? f.dok : 'none';
    const count = Number(f.count) || 10;
    const itemCount = f.type === 'exit-ticket' ? Math.min(count, 5) : count;
    const cases = Math.ceil(itemCount / 3);
    return t(`tools-config:assessmentSectionsList.${key}`, { cases, count: itemCount, interpolation: { escapeValue: false } });
}

// assessment-creator DOK text.
const getAssessmentDokBlock = (dok, t) => {
    if (dok === 'dok-all') return getDokInstructions(dok, t, 'items');
    if (!DOK_OPTIONS.includes(dok)) return '';
    const level = dok.replace('dok-', 'DOK ');
    return `\n\n${t('tools-config:fields.dok')}: ${level} — ${t(`tools-config:options.${dok}`)}`;
}

// math-exam section list for the selected DOK.
// DOK 4 swaps problems, answer key for tasks and a rubric, every other level keeps the original list
const getMathExamSectionsList = (f, t) => {
    const key = f.dok === 'dok-4' ? 'dok-4' : 'none';
    return t(`tools-config:mathExamSectionsList.${key}`, {
        count: f.count || 10,
        interpolation: { escapeValue: false },
    });
}

// Chooses which dokQuestionsClosing text ends the prompt: 'dok-4' or 'dok-all' for
// those levels, 'none' for DOK 1–3.
const getDokQuestionsClosing = (dok, t) => {
    const level = DOK_OPTIONS.includes(dok) ? dok : 'dok-all';
    const key = level === 'dok-4' || level === 'dok-all' ? level : 'none';
    return t(`tools-config:dokQuestionsClosing.${key}`, { interpolation: { escapeValue: false } });
}

// differentiate-process and differentiate-product block for the group's mastery level
const getMasteryInstructions = (mastery, t) => {
    if (!MASTERY_OPTIONS.includes(mastery)) return '';
    return t(`tools-config:masteryInstructions.${mastery}`, { interpolation: { escapeValue: false } });
}

// differentiate-process and differentiate-product block for the selected need
const getSupportNeedsInstructions = (need, t, otherText = '') => {
    if (!SUPPORT_NEEDS_OPTIONS.includes(need)) return '';
    if (need === 'support-other') {
        const detail = String(otherText || '').trim();
        if (!detail) return '';
        return t('tools-config:supportNeedsInstructions.support-other', {
            detail,
            interpolation: { escapeValue: false },
        });
    }
    return t(`tools-config:supportNeedsInstructions.${need}`, { interpolation: { escapeValue: false } });
}

// differentiate-product modality line
const getDemonstrationFormLine = (form, t) => {
    if (!DEMONSTRATION_FORM_OPTIONS.includes(form)) {
        return t('tools-config:demonstrationFormLine.none', { interpolation: { escapeValue: false } });
    }
    return t('tools-config:demonstrationFormLine.selected', {
        form: t(`tools-config:options.${form}`),
        interpolation: { escapeValue: false },
    });
}

// lexile-assessment text length line
const getTextLengthLine = (length, t, customWords = '') => {
    if (!TEXT_LENGTH_OPTIONS.includes(length)) return '';
    const words = length === 'text-length-custom' ? Math.round(Number(customWords)) : TEXT_LENGTH_WORDS[length];
    if (!(words > 0)) return '';
    return t('tools-config:defaults.textLengthLine', {
        words,
        min: Math.round(words * (1 - TEXT_LENGTH_TOLERANCE)),
        max: Math.round(words * (1 + TEXT_LENGTH_TOLERANCE)),
        interpolation: { escapeValue: false },
    });
}

// lexile-assessment text type line
const getTextTypeLine = (type, t) => {
    if (!TEXT_TYPE_OPTIONS.includes(type)) return '';
    return t('tools-config:defaults.textTypeLine', {
        type: t(`tools-config:options.${type}`),
        note: t(`tools-config:textTypeInstructions.${type}`),
        interpolation: { escapeValue: false },
    });
}

// lexile-assessment difficulty line for the Lexile range
const getSentenceLengthLine = (range, t) => {
    const words = LEXILE_SENTENCE_WORDS[range];
    if (!words) return '';
    return t('tools-config:defaults.sentenceLengthLine', { ...words, interpolation: { escapeValue: false } });
}

// lexile-assessment question focus line for the text type
const getQuestionFocusLine = (type, t) => {
    if (!TEXT_TYPE_OPTIONS.includes(type)) return '';
    return t('tools-config:defaults.questionFocusLine', {
        focus: t(`tools-config:questionFocusInstructions.${type}`),
        interpolation: { escapeValue: false },
    });
}

// dok-questions and comprehension-questions question type line
const getQuestionTypeLine = (type, t) => {
    if (!QUESTION_TYPE_OPTIONS.includes(type)) return '';
    return t('tools-config:questionTypeLine', {
        type: t(`tools-config:options.${type}`),
        note: t(`tools-config:questionTypeInstructions.${type}`),
        interpolation: { escapeValue: false },
    });
}

// comprehension-questions skill line. With DOK 4 the skill is a task type, so the instructions are different
const getComprehensionSkillLine = (skill, t, otherText = '', dok = '') => {
    if (!COMPREHENSION_SKILL_OPTIONS.includes(skill)) return '';
    const detail = String(otherText || '').trim();
    if (skill === 'skill-other' && !detail) return '';
    const line = dok === 'dok-4' ? 'comprehensionSkillTaskLine' : 'comprehensionSkillLine';
    return t(`tools-config:defaults.${line}`, {
        skill: t(`tools-config:options.${skill}`),
        note: t(`tools-config:comprehensionSkillInstructions.${skill}`, { detail, interpolation: { escapeValue: false } }),
        interpolation: { escapeValue: false },
    });
}

// leveled-reading student level line
const getReadingLevelLine = (level, t) => {
    if (!READING_LEVEL_OPTIONS.includes(level)) return '';
    return t('tools-config:defaults.readingLevelLine', {
        level: t(`tools-config:options.${level}`),
        note: t(`tools-config:readingLevelInstructions.${level}`),
        interpolation: { escapeValue: false },
    });
}

// science-assistant area line for the system prompt, the area's field of study plus the rule
// to keep general questions inside it
const getScienceAreaLine = (area, t, otherText = '') => {
    if (!SCIENCE_AREA_OPTIONS.includes(area)) return '';
    const detail = String(otherText || '').trim();
    if (area === 'science-other' && !detail) return '';
    return t('tools-config:system.scienceArea', {
        area: area === 'science-other' ? detail : t(`tools-config:options.${area}`),
        note: t(`tools-config:scienceAreaInstructions.${area}`),
        interpolation: { escapeValue: false },
    });
}

// social-studies-assistant discipline line for the system prompt, the discipline's field of study
// plus the rule to keep general questions inside it
const getSocialDisciplineLine = (discipline, t, otherText = '') => {
    if (!SOCIAL_DISCIPLINE_OPTIONS.includes(discipline)) return '';
    const detail = String(otherText || '').trim();
    if (discipline === 'social-other' && !detail) return '';
    return t('tools-config:system.socialDiscipline', {
        discipline: discipline === 'social-other' ? detail : t(`tools-config:options.${discipline}`),
        note: t(`tools-config:socialDisciplineInstructions.${discipline}`),
        interpolation: { escapeValue: false },
    });
}

// choice-board item for the selected thinking level
const getChoiceBoardDokItem = (dok, t) => {
    if (!DOK_OPTIONS.includes(dok)) return '';
    return '\n' + t(`tools-config:choiceBoardDok.${dok}`, { interpolation: { escapeValue: false } });
}

// comprehension-questions thinking level
const getComprehensionDokLine = (dok, t) => {
    if (!DOK_OPTIONS.includes(dok)) return '';
    return t(`tools-config:comprehensionDok.${dok}`, { interpolation: { escapeValue: false } });
}

// comprehension-questions block list. Each line comes from the level (with DOK 4, every item is an extended task)
const getComprehensionBlocks = (f, t) => {
    const level = DOK_OPTIONS.includes(f.dok) ? f.dok : 'none';
    const amounts = { literal: f.literalCount || 4, inferential: f.inferentialCount || 3, critical: f.criticalCount || 2 };

    return COMPREHENSION_BLOCKS
        .map(block => t(`tools-config:comprehensionBlocks.${level}.${block}`, { amount: amounts[block], interpolation: { escapeValue: false } }))
        .join('\n');
}

// math-assistant thinking level for the system prompt
const getMathAssistantDokLine = (dok, t) => {
    if (!DOK_OPTIONS.includes(dok)) return '';
    return t(`tools-config:mathAssistantDok.${dok}`, { interpolation: { escapeValue: false } });
}

// differentiate-content student needs line
const getStudentNeedsLine = (needs, t) => {
    const text = String(needs || '').trim();
    if (!text) return '';
    return t('tools-config:defaults.studentNeedsLine', { needs: text, interpolation: { escapeValue: false } });
}

// math-exam and assessment-creator learning objective line
const getObjectiveLine = (objective, t) => {
    const text = String(objective || '').trim();
    if (!text) return '';
    return '\n' + t('tools-config:defaults.objectiveLine', { objective: text, interpolation: { escapeValue: false } });
}

const getMathExamQuestionType = (f) => (QUESTION_TYPE_OPTIONS.includes(f.type) ? f.type : 'questions-mixed');

// math-exam note on what the format means
const getMathExamFormatNote = (f, t) => {
    // DOK 4 change the exam structure, so the format is not relevant
    if (f.dok === 'dok-4') return '';
    return ' ' + t(`tools-config:questionTypeInstructions.${getMathExamQuestionType(f)}`, { interpolation: { escapeValue: false } });
}

// ---------------------------------------------------------------------------
// TOOLS
// ---------------------------------------------------------------------------

export const TOOLS = {

    /* ─── Planning ─── */

    'plan-my-lesson': {
        category: 'planning',
        key: 'plan-my-lesson',
        title: 'Plan My Lesson',
        useFewshot: true,
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.grade',     type: 'select',   optionsSource: 'levels', default: '5', required: true },
            { name: 'dok',       labelKey: 'fields.bloom',     type: 'select',   optionKeys: DOK_OPTIONS_BLOOM },
            { name: 'topic',     labelKey: 'fields.topic',     type: 'text',     placeholderKey: 'placeholders.plan-my-lesson.topic', required: true },
            { name: 'duration',  labelKey: 'fields.duration',  type: 'number',   default: 60 },
            { name: 'standard',  labelKey: 'fields.standard',  type: 'text',     placeholderKey: 'placeholders.plan-my-lesson.standard' },
            { name: 'units', type: 'standards', codeType: 'units', subjectField: 'subject', gradeField: 'grade' },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'integratedSubjects', labelKey: 'fields.integratedSubjects', type: 'multiselect', optionsSource: 'subjects', excludeField: 'subject', placeholderKey: 'placeholders.plan-my-lesson.integratedSubjects' },
            { name: 'innovationProjects', labelKey: 'fields.innovationProjects', type: 'multiselect', optionKeys: INNOVATION_PROJECT_OPTIONS, allowCustom: true, placeholderKey: 'placeholders.plan-my-lesson.innovationProjects', customPlaceholderKey: 'placeholders.plan-my-lesson.innovationProjectsCustom' },
            { name: 'accommodations',     labelKey: 'fields.accommodations',     type: 'multiselect', optionKeys: ACCOMMODATION_OPTIONS, allowCustom: true, placeholderKey: 'placeholders.plan-my-lesson.accommodations', customPlaceholderKey: 'placeholders.plan-my-lesson.accommodationsCustom' },
            { name: 'activeResponses',    labelKey: 'fields.activeResponses',    type: 'multiselect', optionKeys: ACTIVE_RESPONSE_OPTIONS, placeholderKey: 'placeholders.plan-my-lesson.activeResponses' },
            // Shown once at least one active response type is chosen, under the "La lección incluye:" heading
            { name: 'includeActiveResponsePlan', labelKey: 'fields.includeActiveResponsePlan', headingKey: 'fields.lessonIncludes', type: 'checkbox', default: false, showIf: { field: 'activeResponses', notEmpty: true } },
            { name: 'includeLearningCheck',      labelKey: 'fields.includeLearningCheck',      type: 'checkbox', default: false, showIf: { field: 'activeResponses', notEmpty: true } },
            { name: 'commonExperience',   labelKey: 'fields.commonExperience',   type: 'longtext', rows: 3, placeholderKey: 'placeholders.plan-my-lesson.commonExperience' },
        ],
        generatesImages: true,
        buildSystem: (t, f = {}) => getSystemTeacher(t) + getImageInstructions(t) + getLevelMarking(f.dok, t, 'dokMarking', 'bloomMarking'),
        buildPrompt: (f, t) => t('tools-config:prompts.plan-my-lesson', {
            subject:  f.subject,
            grade:    f.grade,
            topic:    f.topic,
            duration: f.duration || 60,
            standardLine: f.standard
                ? '\n' + t('tools-config:defaults.standardLine', {
                standard: f.standard,
                interpolation: { escapeValue: false },
            })
                : '',
            sectionsList: getSectionsList(f, t),
            lessonContextBlock: getLessonContextBlock(f, t),
            dokBlock: getDokInstructions(f.dok, t),
            interpolation: { escapeValue: false },
        }),
    },

    'stem-unit-planner': {
        category: 'planning',
        key: 'stem-unit-planner',
        title: 'STEM Unit Planner',
        useFewshot: true,
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.grade',     type: 'select', optionsSource: 'levels', default: '5', required: true },
            { name: 'dok',       labelKey: 'fields.bloom',     type: 'select',   optionKeys: DOK_OPTIONS_BLOOM },
            { name: 'topic',     labelKey: 'fields.topic',     type: 'text',   placeholderKey: 'placeholders.stem-unit-planner.topic', required: true },
            { name: 'weeks',     labelKey: 'fields.weeks',     type: 'number', default: 3 },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        generatesImages: true,
        buildSystem: (t, f = {}) => getSystemTeacher(t) + getImageInstructions(t) + getLevelMarking(f.dok, t, 'dokMarkingUnit', 'bloomMarkingUnit'),
        buildPrompt: (f, t) => t('tools-config:prompts.stem-unit-planner', {
            weeks: f.weeks || 3,
            grade: f.grade,
            topic: f.topic,
            sectionsList: getUnitSectionsList(f.dok, t),
            dokBlock: getDokInstructions(f.dok, t, 'unit'),
            interpolation: { escapeValue: false },
        }),
    },

    'adapt-lesson': {
        category: 'differentiation',
        key: 'adapt-lesson',
        title: 'Adapt Lesson',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'lesson',    labelKey: 'fields.lesson',    type: 'textarea', placeholderKey: 'placeholders.adapt-lesson.lesson', required: true, rows: 8 },
            { name: 'profile',   labelKey: 'fields.profile',   type: 'select',   optionKeys: PROFILE_OPTIONS, required: true },
            { name: 'grade',     labelKey: 'fields.grade',     type: 'select',   optionsSource: 'levels', default: '5' },
        ],
        generatesImages: true,
        buildSystem: (t) => getSystemTeacher(t) + getImageInstructions(t) + t('tools-config:system.adaptLesson', { interpolation: { escapeValue: false } }),
        buildPrompt: (f, t) => t('tools-config:prompts.adapt-lesson', {
            profile: t(`tools-config:options.${f.profile}`),
            grade:   f.grade,
            lesson:  f.lesson,
            interpolation: { escapeValue: false },
        }),
    },

    'language-objectives': {
        category: 'planning',
        key: 'language-objectives',
        title: 'Language Objectives',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'content',   labelKey: 'fields.content',   type: 'textarea', placeholderKey: 'placeholders.language-objectives.content', required: true, rows: 4 },
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.grade',     type: 'select',   optionsSource: 'levels', default: null },
            { name: 'ellLevel',  labelKey: 'fields.ellLevel',  type: 'select',   optionKeys: ELL_LEVEL_OPTIONS, defaultKey: 'options.developing' },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.widaExpert'),
        buildPrompt: (f, t) => t('tools-config:prompts.language-objectives', {
            grade:    f.grade,
            ellLevel: t(`tools-config:options.${f.ellLevel}`),
            content:  f.content,
            interpolation: { escapeValue: false },
        }),
    },

    /* ─── Differentiation ─── */

    'group-strategy': {
        category: 'differentiation',
        key: 'group-strategy',
        title: 'Group Strategy',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'total',     labelKey: 'fields.total',     type: 'number',  default: 24, required: true },
            { name: 'objective', labelKey: 'fields.objective', type: 'text',    placeholderKey: 'placeholders.group-strategy.objective', required: true },
            { name: 'levels',    labelKey: 'fields.levels',    type: 'text',    placeholderKey: 'placeholders.group-strategy.levels', required: true },
            { name: 'type',      labelKey: 'fields.type',      type: 'select',  optionKeys: GROUPING_OPTIONS, defaultKey: 'options.heterogeneous' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.group-strategy', {
            total:     f.total,
            objective: f.objective,
            levels:    f.levels,
            type:      t(`tools-config:options.${f.type}`),
            interpolation: { escapeValue: false },
        }),
    },

    'differentiate-content': {
        category: 'differentiation',
        key: 'differentiate-content',
        title: 'Differentiate Content',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'lesson',    labelKey: 'fields.lesson',    type: 'textarea', placeholderKey: 'placeholders.adapt-lesson.lesson', required: false, rows: 8 },
            { name: 'subject',   labelKey: 'fields.subject', type: 'select', optionsSource: 'subjects' },
            { name: 'grade',     labelKey: 'fields.grade',  type: 'select', optionsSource: 'levels', default: null },
            { name: 'dok',       labelKey: 'fields.bloom',     type: 'select',   optionKeys: DOK_OPTIONS_BLOOM },
            { name: 'studentNeeds', labelKey: 'fields.studentNeeds', type: 'longtext', placeholderKey: 'placeholders.differentiate-content.studentNeeds', rows: 3 },
        ],
        generatesImages: true,
        buildSystem: (t, f = {}) => getSystemTeacher(t) + getImageInstructions(t) + t('tools-config:system.differentiateContent', { interpolation: { escapeValue: false } })
            + getLevelMarking(f.dok, t, 'dokMarkingLevels', 'bloomMarkingLevels'),
        buildPrompt: (f, t) => t('tools-config:prompts.differentiate-content', {
            topic: f.lesson,
            subject: f.subject,
            grade: f.grade,
            sectionsList: getLevelsSectionsList(f.dok, t),
            studentNeedsLine: getStudentNeedsLine(f.studentNeeds, t),
            dokBlock: getDokInstructions(f.dok, t, 'levels'),
            interpolation: { escapeValue: false },
        }),
    },

    'differentiate-process': {
        category: 'differentiation',
        key: 'differentiate-process',
        title: 'Differentiate Process',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'lesson',    labelKey: 'fields.lesson',  type: 'textarea', placeholderKey: 'placeholders.differentiate-process.lesson', required: true, rows: 4 },
            { name: 'grade',     labelKey: 'fields.grade',   type: 'select',   optionsSource: 'levels', default: '5th' },
            { name: 'mastery',   labelKey: 'fields.masteryLevel', type: 'select', optionKeys: MASTERY_OPTIONS, defaultKey: 'options.mastery-mixed' },
            { name: 'needs',     labelKey: 'fields.supportNeeds', type: 'select', optionKeys: SUPPORT_NEEDS_OPTIONS },
            { name: 'needsOther', labelKey: 'fields.supportNeedsOther', type: 'text', placeholderKey: 'placeholders.differentiate-process.needsOther', showIf: { field: 'needs', equals: 'support-other' } },
            { name: 'styles',    labelKey: 'fields.styles',  type: 'text',     placeholderKey: 'placeholders.differentiate-process.styles', defaultKey: 'defaults.learningStyles' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => {
            const masteryBlock = getMasteryInstructions(f.mastery, t);
            const needsBlock = getSupportNeedsInstructions(f.needs, t, f.needsOther);

            return t('tools-config:prompts.differentiate-process', {
                grade:  f.grade,
                lesson: f.lesson,
                styles: f.styles || t('tools-config:defaults.learningStyles'),
                // Show the group adjustments line only if either block is present
                groupAdjustmentsLine: masteryBlock || needsBlock ? t('tools-config:groupAdjustmentsLine', { interpolation: { escapeValue: false } }) : '',
                masteryBlock,
                needsBlock,
                interpolation: { escapeValue: false },
            });
        },
    },

    'differentiate-product': {
        category: 'differentiation',
        key: 'differentiate-product',
        title: 'Differentiate Product',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'objective', labelKey: 'fields.objective', type: 'textarea', placeholderKey: 'placeholders.differentiate-product.objective', required: true, rows: 4 },
            { name: 'grade',     labelKey: 'fields.grade',     type: 'select',   optionsSource: 'levels', default: null },
            { name: 'assessmentType', labelKey: 'fields.assessmentType', type: 'select', optionKeys: ASSESSMENT_PURPOSE_OPTIONS, required: true },
            { name: 'count',     labelKey: 'fields.count',     type: 'number',   default: 6 },
            { name: 'mastery',   labelKey: 'fields.masteryLevel', type: 'select', optionKeys: MASTERY_OPTIONS },
            { name: 'needs',     labelKey: 'fields.supportNeeds', type: 'select', optionKeys: SUPPORT_NEEDS_OPTIONS },
            { name: 'needsOther', labelKey: 'fields.supportNeedsOther', type: 'text', placeholderKey: 'placeholders.differentiate-process.needsOther', showIf: { field: 'needs', equals: 'support-other' } },
            { name: 'demonstrationForm', labelKey: 'fields.demonstrationForm', type: 'select', optionKeys: DEMONSTRATION_FORM_OPTIONS },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => {
            const masteryBlock = getMasteryInstructions(f.mastery, t);
            const needsBlock   = getSupportNeedsInstructions(f.needs, t, f.needsOther);

            return t('tools-config:prompts.differentiate-product', {
                count:     f.count || 6,
                grade:     f.grade,
                objective: f.objective,
                assessmentType: t(`tools-config:options.${f.assessmentType}`),
                assessmentTypeNote: t(`tools-config:assessmentTypeInstructions.${f.assessmentType}`, { interpolation: { escapeValue: false } }),
                demonstrationFormLine: getDemonstrationFormLine(f.demonstrationForm, t),
                groupAdjustmentsSection: masteryBlock || needsBlock ? t('tools-config:groupAdjustmentsSection', { interpolation: { escapeValue: false } }) : '',
                masteryBlock,
                needsBlock,
                interpolation: { escapeValue: false },
            });
        },
    },

    'intervention-plan': {
        category: 'differentiation',
        key: 'intervention-plan',
        title: 'Intervention Plan',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.gradeStudents', type: 'select', optionsSource: 'levels', default: null, required: true },
            { name: 'severity',  labelKey: 'fields.severity',  type: 'select',   optionKeys: LAG_SEVERITY_OPTIONS, defaultKey: 'options.1-grade-behind', required: true },
            { name: 'gaps',      labelKey: 'fields.gaps',      type: 'textarea', required: true, rows: 4, placeholderKey: 'placeholders.intervention-plan.gaps' },
            { name: 'count',     labelKey: 'fields.studentCount', type: 'number', default: 6 },
            { name: 'time',      labelKey: 'fields.time',      type: 'select',   optionKeys: INTERVENTION_TIME_OPTIONS, defaultKey: 'options.20-30min-3x-week', required: true },
            { name: 'weeks',     labelKey: 'fields.weeks',     type: 'number',   default: 6 },
            { name: 'standards', labelKey: 'fields.standardsGrade', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        generatesImages: true,
        buildSystem: (t) => getSystemTeacher(t) + getImageInstructions(t) + t('tools-config:system.interventionPlan', { interpolation: { escapeValue: false } }),
        buildPrompt: (f, t) => t('tools-config:prompts.intervention-plan', {
            weeks:    f.weeks || 6,
            count:    f.count || 6,
            grade:    f.grade,
            subject:  f.subject,
            severity: t(`tools-config:options.${f.severity}`),
            time:     t(`tools-config:options.${f.time}`),
            gaps:     f.gaps,
            interpolation: { escapeValue: false },
        }),
    },

    /* ─── Assessment ─── */

    'create-rubric': {
        category: 'assessment',
        key: 'create-rubric',
        title: 'Create Rubric',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'task',      labelKey: 'fields.task',     type: 'textarea', placeholderKey: 'placeholders.create-rubric.task', required: true, rows: 4 },
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.grade',    type: 'select',   optionsSource: 'levels', default: null },
            { name: 'levels',    labelKey: 'fields.levels',   type: 'select',   optionKeys: LEVELS_OPTIONS, defaultKey: 'options.4-levels' },
            { name: 'criteria',  labelKey: 'fields.criteria', type: 'text',     placeholderKey: 'placeholders.create-rubric.criteria' },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.create-rubric', {
            grade:    f.grade,
            task:     f.task,
            levels:   t(`tools-config:options.${f.levels}`),
            criteria: f.criteria || '',
            interpolation: { escapeValue: false },
        }),
    },

    'dok-questions': {
        category: 'assessment',
        key: 'dok-questions',
        title: 'DOK Questions',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'topic',     labelKey: 'fields.topicOrReading', type: 'textarea', placeholderKey: 'placeholders.dok-questions.topic', required: true, rows: 5 },
            { name: 'learningObjective', labelKey: 'fields.learningObjective', type: 'text', placeholderKey: 'placeholders.assessment-creator.learningObjective' },
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: false },
            { name: 'grade',     labelKey: 'fields.grade',          type: 'select',   optionsSource: 'levels', default: null },
            { name: 'dok',       labelKey: 'fields.dok',       type: 'select',   optionKeys: DOK_OPTIONS, required: true },
            { name: 'questionCount', labelKey: 'fields.questionCount', type: 'number', default: 10 },
            { name: 'questionType',  labelKey: 'fields.questionType',  type: 'select', optionKeys: QUESTION_TYPE_OPTIONS },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' }
        ],
        assignmentSourceField: 'topic',
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.dokQuestionsFormat', { interpolation: { escapeValue: false } }),
        buildPrompt: (f, t) => {
            const typed = Math.round(Number(f.questionCount));
            const count = typed > 0 ? typed : 10;

            return t('tools-config:prompts.dok-questions', {
                grade: f.grade,
                topic: f.topic,
                objectiveLine: getObjectiveLine(f.learningObjective, t),
                dokBlock: getDokInstructions(f.dok, t, 'questions', { count }),
                questionTypeLine: f.dok !== 'dok-4' ? getQuestionTypeLine(f.questionType, t) : '',
                closingLine: getDokQuestionsClosing(f.dok, t),
                interpolation: { escapeValue: false },
            });
        },
    },

    'math-exam': {
        category: 'assessment',
        key: 'math-exam',
        title: 'Math Exam',
        canMakeInteractive: true,
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'grade',     labelKey: 'fields.grade',  type: 'select', optionsSource: 'levels', default: null },
            { name: 'topic',     labelKey: 'fields.topic',  type: 'text',   placeholderKey: 'placeholders.math-exam.topic', required: true },
            { name: 'learningObjective', labelKey: 'fields.learningObjective', type: 'text', placeholderKey: 'placeholders.math-exam.learningObjective' },
            { name: 'count',     labelKey: 'fields.count',  type: 'number', default: 10 },
            { name: 'type',      labelKey: 'fields.questionType', type: 'select', optionKeys: QUESTION_TYPE_OPTIONS, default: 'questions-mixed' },
            { name: 'complexity', labelKey: 'fields.complexity', type: 'select', optionKeys: COMPLEXITY_OPTIONS },
            { name: 'dok',       labelKey: 'fields.dok',       type: 'select',   optionKeys: DOK_OPTIONS },
            { name: 'standards', type: 'standards', subject: 'mat-sp', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subject: 'mat-sp', gradeField: 'grade' },
        ],
        generatesImages: true,
        buildSystem: (t) => getSystemTeacher(t) + getImageInstructions(t) + t('tools-config:system.mathExam', { interpolation: { escapeValue: false } }),
        buildPrompt: (f, t) => t('tools-config:prompts.math-exam', {
            grade: f.grade,
            topic: f.topic,
            count: f.count || 10,
            type:  t(`tools-config:options.${getMathExamQuestionType(f)}`),
            formatNote: getMathExamFormatNote(f, t),
            objectiveLine: getObjectiveLine(f.learningObjective, t),
            complexityLine: COMPLEXITY_OPTIONS.includes(f.complexity)
                ? '\n' + t('tools-config:defaults.complexityLine', {
                level: t(`tools-config:options.${f.complexity}`),
                note: t(`tools-config:complexityInstructions.${f.complexity}`),
                interpolation: { escapeValue: false },
            })
                : '',
            sectionsList: getMathExamSectionsList(f, t),
            dokBlock: getDokInstructions(f.dok, t, 'items'),
            interpolation: { escapeValue: false },
        }),
    },

    'assessment-creator': {
        category: 'assessment',
        key: 'assessment-creator',
        title: 'Assessment Creator',
        canMakeInteractive: true,
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'subject',   labelKey: 'fields.subject',  type: 'select', optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.grade',    type: 'select', optionsSource: 'levels', default: null, required: true },
            { name: 'topic',     labelKey: 'fields.topicUnit',type: 'text',   placeholderKey: 'placeholders.assessment-creator.topic', required: true },
            { name: 'learningObjective', labelKey: 'fields.learningObjective', type: 'text', placeholderKey: 'placeholders.assessment-creator.learningObjective' },
            { name: 'type',      labelKey: 'fields.type',     type: 'select', optionKeys: ASSESSMENT_TYPE_OPTIONS, defaultKey: 'options.formative', required: true },
            { name: 'count',     labelKey: 'fields.count',    type: 'number', default: 10 },
            { name: 'format',    labelKey: 'fields.format',   type: 'select', optionKeys: ASSESSMENT_FORMAT_OPTIONS, defaultKey: 'options.mixed' },
            { name: 'dok',       labelKey: 'fields.dok',       type: 'select',   optionKeys: DOK_OPTIONS },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        // Base system prompt, the DOK rules are added only when a level is selected.
        buildSystem: (t, f = {}) => {
            const text = (key) => t(`tools-config:${key}`, { interpolation: { escapeValue: false } });
            const base = getSystemTeacher(t) + getNoImageInstructions(t) + text('system.assessmentCreator');

            if (!DOK_OPTIONS.includes(f.dok)) return base;

            return base + text('system.dokMarkingItems');
        },
        buildPrompt: (f, t) => t('tools-config:prompts.assessment-creator', {
            subject: f.subject,
            grade:   f.grade,
            topic:   f.topic,
            type:    t(`tools-config:options.${f.type}`),
            count:   f.count || 10,
            format:  t(`tools-config:options.${f.format}`),
            objectiveLine: getObjectiveLine(f.learningObjective, t),
            sectionsList: getAssessmentSectionsList(f, t),
            dokBlock: getAssessmentDokBlock(f.dok, t),
            interpolation: { escapeValue: false },
        }),
    },

    'diagnostic-tests': {
        category: 'assessment',
        key: 'diagnostic-tests',
        title: 'Diagnostic Tests',
        canMakeInteractive: true,
        suggestsInterventionPlan: true,
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'subject',   labelKey: 'fields.subject',  type: 'select', optionsSource: 'subjects', required: true },
            { name: 'grade',     labelKey: 'fields.grade',    type: 'select', optionsSource: 'levels', default: null, required: true },
            { name: 'unit',      labelKey: 'fields.upcomingUnit', type: 'text', placeholderKey: 'placeholders.diagnostic-tests.unit', required: true },
            { name: 'moment',    labelKey: 'fields.moment',   type: 'select', optionKeys: SCHOOL_MOMENT_OPTIONS, defaultKey: 'options.before-new-unit', required: true },
            { name: 'count',     labelKey: 'fields.count',    type: 'number', default: 12 },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.diagnosticTests', { interpolation: { escapeValue: false } }),
        buildPrompt: (f, t) => t('tools-config:prompts.diagnostic-tests', {
            subject: f.subject,
            grade:   f.grade,
            unit:    f.unit,
            moment:  t(`tools-config:options.${f.moment}`),
            count:   f.count || 12,
            interpolation: { escapeValue: false },
        }),
    },

    'personalized-feedback': {
        category: 'assessment',
        key: 'personalized-feedback',
        title: 'Personalized Feedback',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'student', labelKey: 'fields.student', type: 'text',     placeholderKey: 'placeholders.personalized-feedback.student', required: true },
            { name: 'work',    labelKey: 'fields.work',    type: 'textarea', placeholderKey: 'placeholders.personalized-feedback.work', required: true, rows: 5 },
            { name: 'tone',    labelKey: 'fields.tone',    type: 'select',   optionKeys: TONE_OPTIONS, defaultKey: 'options.warm-motivating' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.personalized-feedback', {
            student: f.student,
            work:    f.work,
            tone:    t(`tools-config:options.${f.tone}`),
            interpolation: { escapeValue: false },
        }),
    },

    'choice-board': {
        category: 'assessment',
        key: 'choice-board',
        title: 'Choice Board',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'topic',        labelKey: 'fields.topicUnit',       type: 'text',   placeholderKey: 'placeholders.choice-board.topic', required: true },
            { name: 'grade',        labelKey: 'fields.grade',           type: 'select', optionsSource: 'levels', default: null },
            { name: 'instructions', labelKey: 'fields.rules',           type: 'text',   placeholderKey: 'placeholders.choice-board.instructions', defaultKey: 'defaults.choiceBoardRules' },
            { name: 'dok',          labelKey: 'fields.dok',             type: 'select', optionKeys: DOK_OPTIONS },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.choice-board', {
            grade:        f.grade,
            topic:        f.topic,
            instructions: f.instructions || t('tools-config:defaults.choiceBoardRules'),
            dokItem:      getChoiceBoardDokItem(f.dok, t),
            interpolation: { escapeValue: false },
        }),
    },

    /* ─── Reading ─── */

    'leveled-reading': {
        category: 'reading',
        key: 'leveled-reading',
        title: 'Leveled Reading',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'topic',     labelKey: 'fields.topic',   type: 'text',   placeholderKey: 'placeholders.leveled-reading.topic', required: true },
            { name: 'grade',     labelKey: 'fields.grade',   type: 'select', optionsSource: 'levels', default: null },
            { name: 'readingLevel', labelKey: 'fields.readingLevel', type: 'select', optionKeys: READING_LEVEL_OPTIONS },
            { name: 'length',    labelKey: 'fields.length',  type: 'select', optionKeys: LENGTH_OPTIONS, defaultKey: 'options.medium-300' },
            { name: 'genre',     labelKey: 'fields.genre',   type: 'select', optionKeys: GENRE_OPTIONS, defaultKey: 'options.informative' },
            { name: 'standards', type: 'standards', subject: 'sp', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subject: 'sp', gradeField: 'grade' },
        ],
        generatesImages: true,
        buildSystem: (t) => getSystemTeacher(t) + getImageInstructions(t) + t('tools-config:system.leveledReading', { interpolation: { escapeValue: false } }),
        buildPrompt: (f, t) => t('tools-config:prompts.leveled-reading', {
            genre:  t(`tools-config:options.${f.genre}`),
            grade:  f.grade,
            topic:  f.topic,
            length: t(`tools-config:options.${f.length}`),
            readingLevelLine: getReadingLevelLine(f.readingLevel, t),
            interpolation: { escapeValue: false },
        }),
    },

    'academic-vocabulary': {
        category: 'reading',
        key: 'academic-vocabulary',
        title: 'Academic Vocabulary',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'topic',     labelKey: 'fields.topic',  type: 'text',   placeholderKey: 'placeholders.academic-vocabulary.topic', required: true },
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: false },
            { name: 'grade',     labelKey: 'fields.grade',  type: 'select', optionsSource: 'levels', default: null },
            { name: 'count',     labelKey: 'fields.count',  type: 'number', default: 10 },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        generatesImages: true,
        buildSystem: (t) => getSystemTeacher(t) + getImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.academic-vocabulary', {
            count: f.count || 10,
            topic: f.topic,
            grade: f.grade,
            interpolation: { escapeValue: false },
        }),
    },

    'comprehension-questions': {
        category: 'reading',
        key: 'comprehension-questions',
        title: 'Comprehension Questions',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'text',             labelKey: 'fields.text',             type: 'textarea', placeholderKey: 'placeholders.comprehension-questions.text', required: true, rows: 8 },
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: false },
            { name: 'grade',            labelKey: 'fields.grade',            type: 'select',   optionsSource: 'levels', default: null },
            { name: 'literalCount',     labelKey: 'fields.literalCount',     type: 'number',   default: 4 },
            { name: 'inferentialCount', labelKey: 'fields.inferentialCount', type: 'number',   default: 3 },
            { name: 'criticalCount',    labelKey: 'fields.criticalCount',    type: 'number',   default: 2 },
            { name: 'skill',      labelKey: 'fields.comprehensionSkill',      type: 'select', optionKeys: COMPREHENSION_SKILL_OPTIONS },
            { name: 'skillOther', labelKey: 'fields.comprehensionSkillOther', type: 'text',   placeholderKey: 'placeholders.comprehension-questions.skillOther', showIf: { field: 'skill', equals: 'skill-other' } },
            { name: 'questionType', labelKey: 'fields.questionType', type: 'select', optionKeys: QUESTION_TYPE_OPTIONS },
            { name: 'dok',        labelKey: 'fields.dok',        type: 'select', optionKeys: DOK_OPTIONS },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        assignmentSourceField: 'text',
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.comprehension-questions', {
            grade:            f.grade,
            text:             f.text,
            blocksList:       getComprehensionBlocks(f, t),
            comprehensionSkillLine: getComprehensionSkillLine(f.skill, t, f.skillOther, f.dok),
            questionTypeLine: f.dok !== 'dok-4' ? getQuestionTypeLine(f.questionType, t) : '',
            comprehensionDokLine: getComprehensionDokLine(f.dok, t),
            keyLine: t(`tools-config:defaults.${f.dok === 'dok-4' ? 'comprehensionKeyLineTasks' : 'comprehensionKeyLine'}`, { interpolation: { escapeValue: false } }),
            interpolation: { escapeValue: false },
        }),
    },

    'lexile-assessment': {
        category: 'reading',
        key: 'lexile-assessment',
        title: 'Lexile Assessment',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'range',     labelKey: 'fields.lexileRange',  type: 'select', optionKeys: LEXILE_OPTIONS, defaultKey: 'options.500L-800L', required: true },
            { name: 'topic',     labelKey: 'fields.textTopic',    type: 'text',   placeholderKey: 'placeholders.lexile-assessment.topic', required: true },
            { name: 'subject',   labelKey: 'fields.subject',   type: 'select',   optionsSource: 'subjects', required: false },
            { name: 'grade',     labelKey: 'fields.gradeStudent', type: 'select', optionsSource: 'levels', default: null },
            { name: 'textLength', labelKey: 'fields.textLength', type: 'select', optionKeys: TEXT_LENGTH_OPTIONS },
            { name: 'textLengthCustom', labelKey: 'fields.textLengthCustom', type: 'number', placeholderKey: 'placeholders.lexile-assessment.textLengthCustom', showIf: { field: 'textLength', equals: 'text-length-custom' } },
            { name: 'textType', labelKey: 'fields.textType', type: 'select', optionKeys: TEXT_TYPE_OPTIONS },
            { name: 'standards', type: 'standards', subjectField: 'subject', gradeField: 'grade' },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
            { name: 'expectations', type: 'standards', codeType: 'expectations', standardsField: 'standards', subjectField: 'subject', gradeField: 'grade' },
        ],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t),
        buildPrompt: (f, t) => t('tools-config:prompts.lexile-assessment', {
            range: f.range,
            grade: f.grade,
            topic: f.topic,
            textLengthLine: getTextLengthLine(f.textLength, t, f.textLengthCustom),
            textTypeLine: getTextTypeLine(f.textType, t),
            sentenceLengthLine: getSentenceLengthLine(f.range, t),
            questionFocusLine: getQuestionFocusLine(f.textType, t),
            interpolation: { escapeValue: false },
        }),
    },

    /* ─── Assistants (chat mode) ─── */

    'math-assistant': {
        category: 'assistants',
        isChat: true,
        key: 'math-assistant',
        title: 'Math Assistant',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'dok',      labelKey: 'fields.dok',      type: 'select',   optionKeys: DOK_OPTIONS },
            { name: 'question', labelKey: 'fields.question', type: 'textarea', placeholderKey: 'placeholders.math-assistant.question', required: true, rows: 5 },
            { name: 'createAssignment', labelKey: 'fields.createAssignment', type: 'checkbox', default: false },
        ],
        buildSystem: (t, f = {}) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.mathAssistant') + getMathAssistantDokLine(f.dok, t),
        buildPrompt: (f) => f.question,
    },

    'science-assistant': {
        category: 'assistants',
        isChat: true,
        key: 'science-assistant',
        title: 'Science Assistant',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'scienceArea', labelKey: 'fields.scienceArea', type: 'select', optionKeys: SCIENCE_AREA_OPTIONS, hideWithAthenasLesson: true },
            { name: 'scienceAreaOther', labelKey: 'fields.scienceAreaOther', type: 'text', placeholderKey: 'placeholders.science-assistant.scienceAreaOther', showIf: { field: 'scienceArea', equals: 'science-other' }, hideWithAthenasLesson: true },
            { name: 'question', labelKey: 'fields.question', type: 'textarea', placeholderKey: 'placeholders.science-assistant.question', required: true, rows: 5 },
        ],
        buildSystem: (t, f = {}) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.scienceAssistant') + getScienceAreaLine(f.scienceArea, t, f.scienceAreaOther),
        buildPrompt: (f) => f.question,
    },

    'language-assistant': {
        category: 'assistants',
        isChat: true,
        key: 'language-assistant',
        title: 'Language Assistant',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [{ name: 'question', labelKey: 'fields.question', type: 'textarea', placeholderKey: 'placeholders.language-assistant.question', required: true, rows: 5 }],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.languageAssistant'),
        buildPrompt: (f) => f.question,
    },

    'ela-assistant': {
        category: 'assistants',
        isChat: true,
        key: 'ela-assistant',
        title: 'ELA Assistant',
        defaultLanguage: 'lang-en',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [{ name: 'question', labelKey: 'fields.questionBilingual', type: 'textarea', placeholderKey: 'placeholders.ela-assistant.question', required: true, rows: 5 }],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.elaAssistant'),
        buildPrompt: (f) => f.question,
    },

    'social-studies-assistant': {
        category: 'assistants',
        isChat: true,
        key: 'social-studies-assistant',
        title: 'Social Studies Assistant',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [
            { name: 'socialDiscipline', labelKey: 'fields.socialDiscipline', type: 'select', optionKeys: SOCIAL_DISCIPLINE_OPTIONS, hideWithAthenasLesson: true },
            { name: 'socialDisciplineOther', labelKey: 'fields.socialDisciplineOther', type: 'text', placeholderKey: 'placeholders.social-studies-assistant.socialDisciplineOther', showIf: { field: 'socialDiscipline', equals: 'social-other' }, hideWithAthenasLesson: true },
            { name: 'question', labelKey: 'fields.question', type: 'textarea', placeholderKey: 'placeholders.social-studies-assistant.question', required: true, rows: 5 },
        ],
        buildSystem: (t, f = {}) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.socialStudiesAssistant') + getSocialDisciplineLine(f.socialDiscipline, t, f.socialDisciplineOther),
        buildPrompt: (f) => f.question,
    },

    'special-ed-coach': {
        category: 'assistants',
        isChat: true,
        key: 'special-ed-coach',
        title: 'Special Ed. Coach',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [{ name: 'question', labelKey: 'fields.question', type: 'textarea', placeholderKey: 'placeholders.special-ed-coach.question', required: true, rows: 5 }],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.specialEdCoach'),
        buildPrompt: (f) => f.question,
    },

    'ell-coach': {
        category: 'assistants',
        isChat: true,
        key: 'ell-coach',
        title: 'ELL Coach',
        defaultLanguage: 'lang-en',
        defaultModel: DEFAULT_AI_MODEL,
        fields: [{ name: 'question', labelKey: 'fields.question', type: 'textarea', placeholderKey: 'placeholders.ell-coach.question', required: true, rows: 5 }],
        buildSystem: (t) => getSystemTeacher(t) + getNoImageInstructions(t) + t('tools-config:system.ellCoach'),
        buildPrompt: (f) => f.question,
    },
};

// ---------------------------------------------------------------------------
// Content language, added to every tool
// ---------------------------------------------------------------------------

const getLanguageName = (language, tool, t) => {
    const key = LANGUAGE_OPTIONS.includes(language) ? language : getDefaultLanguage(tool);
    return t(`tools-config:languageNames.${key}`);
};

// Full rule at the end of the system prompt
const getLanguageInstruction = (language, tool, t) => t('tools-config:languageInstruction', {
    language: getLanguageName(language, tool, t),
    interpolation: { escapeValue: false },
});

// Short reminder as the last line of the user prompt, so the language is the last thing the model reads
const getLanguageReminder = (language, tool, t) => t('tools-config:languageReminder', {
    language: getLanguageName(language, tool, t),
    interpolation: { escapeValue: false },
});

// Adds the "Idioma del contenido" select to each tool (after the subject, else after the grade, else first)
// and appends the language rule to its prompts. Chat tools get the rule in the system prompt only,
// since their user prompt is the teacher's question and is shown back in the chat history.
const addContentLanguage = (tool) => {
    if (!tool.fields.some(field => field.name === 'language')) {
        const field = {
            name: 'language',
            labelKey: 'fields.language',
            type: 'select',
            optionKeys: LANGUAGE_OPTIONS,
            default: getDefaultLanguage(tool),
            required: true,
        };
        const subjectIndex = tool.fields.findIndex(f => f.name === 'subject');
        const anchor = subjectIndex >= 0 ? subjectIndex : tool.fields.findIndex(f => f.name === 'grade');
        tool.fields.splice(anchor + 1, 0, field);
    }

    const buildSystem = tool.buildSystem;
    tool.buildSystem = (t, f = {}) => buildSystem(t, f) + getLanguageInstruction(f.language, tool, t);

    if (!tool.isChat) {
        const buildPrompt = tool.buildPrompt;
        tool.buildPrompt = (f, t) => buildPrompt(f, t) + getLanguageReminder(f.language, tool, t);
    }
};

Object.values(TOOLS).forEach(addContentLanguage);