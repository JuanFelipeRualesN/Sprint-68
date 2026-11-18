/**
 * tool-modal.jsx
 * Generic AI tool modal: form + streaming output + copy/regenerate.
 * All human-language strings go through t(). Zero hardcoded Spanish.
 *
 * Field name keys (subject, grade, unit, topic, gaps, standards, question,
 * severity) must match the English slugs defined in tools-config.js.
 *
 * Select fields come in two flavors (see tools-config.js field schema):
 *   - Static:  `optionKeys` — English slugs, labels via
 *              t('tools-config:options.<slug>') at render time.
 *   - Dynamic: `optionsSource: 'subjects' | 'levels'` — options fetched
 *              from the API (getSubjectOptions / getLevelOptions). Labels
 *              arrive already localized by the backend, so NO t() lookup.
 *
 * State (`values`) always stores the option VALUE (API code or slug),
 * never the display label. Labels are substituted only:
 *   - when rendering <option> text,
 *   - when building the LLM prompt (buildPrompt receives labels).
 *
 * IMAGES: the image-generator returns raw base64 (no `data:` prefix). Every
 * markdown string that reaches the renderer/exporters is passed through
 * normalizeBase64Images() so `![alt](iVBORw0KGgo...)` becomes
 * `![alt](data:image/png;base64,iVBORw0KGgo...)`. Without this the browser
 * treats the payload as a relative URL and the image renders broken.
 *
 * PERSISTENCE: there are TWO independently persisted bodies of content —
 *   - `output`               → saved to the document's `content`
 *   - `presentationMarkdown` → saved to the document's `presentationContent`
 * Both are debounced and de-duplicated against their own `lastSaved*` ref so
 * multi-MB base64 payloads are not re-POSTed on every keystroke.
 */
import React, { useEffect, useState, useRef } from 'react';
import { createPortal } from 'react-dom';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import Ic from '../../../../../../assets/svg/icons';
import AthenasCodePicker from './athenas-code-picker';
import AthenasLessonPicker from './athenas-lesson-picker';
import { exportMarkdownPDF, exportWorksheet } from './utils/exporters-pdf';
import { exportPPTX } from './utils/exporters-pptx';
import { exportWord } from './utils/exporters-word';
import { createAssignment } from './utils/ai-assignment.js';
import { saveGeneratedDocument, updateDocument } from '../../../../../../api/ai-module';

import FileUploadButton from './file-upload-button.jsx';
import AthenasLessonCard from './athenas-lesson-card.jsx';
import DownloadMenu from './download-menu.jsx';
import { DocumentWorkspace, WorksheetWorkspace } from './document-workspace.jsx';
import { SlideWorkspace } from './slide-workspace.jsx';

import { getSubjectOptions, getLevelOptions } from '../tool-config/tools-config-options.js';
import { DEFAULT_AI_MODEL, getFieldInitialValue, getDefaultLanguage } from '../tool-config/tools-config';
import {
    mdToHtml,
    resolveImageTags,
    cleanGeneratedOutput,
    normalizeBase64Images,      // NEW: bare base64 → data: URL
    formatLessonAsReference,
    buildAthenasAssistantPrompt,
    hasAthenasLesson,
} from './tool-modal-utils.jsx';
import Settings from "../../../../../../custom-classes/settings";
import { getAuthHeaders } from "../../../../../../api/interceptors.js";
import SearchableSelect from "./utils/searchable-select/searchable-select";
import MultiSelectChips from './multi-select-chips.jsx';

// ---------------------------------------------------------------------------
// Athenas assistant action keys — English identifiers only.
// Display labels are resolved via t('tool-modal:chat.actions.<key>') at render time.
// ---------------------------------------------------------------------------
const ATHENAS_ASSISTANT_ACTION_KEYS = ['deepen', 'practice', 'dok34', 'worksheet', 'ppt'];

// ---------------------------------------------------------------------------
// ToolModal
// ---------------------------------------------------------------------------

function ToolModal({ tool, onClose, embedded = false, initialValues = null, onSwitchTool = null }) {
    const { t } = useTranslation(['tool-modal', 'ai-tools', 'tools-config', 'common', 'export-utils']);
    const buildChatPrompt = React.useCallback(
        (raw, action) => buildAthenasAssistantPrompt(raw, action, t),
        [t]
    );

    // Single default model for all tools — picker removed from the UI by request.
    // To swap models later, change DEFAULT_AI_MODEL or restore the dropdown.
    const [model] = React.useState(DEFAULT_AI_MODEL);

    console.log("Tool", tool);
    // -----------------------------------------------------------------------
    // Dynamic options (subjects / levels from the API).
    // Labels are already localized by the backend (language header/cookie),
    // values are the API `Code` fields.
    // -----------------------------------------------------------------------
    const [dynOptions,     setDynOptions]     = React.useState({ subjects: [], levels: [] });
    const [optionsLoading, setOptionsLoading] = React.useState(true);

    React.useEffect(() => {
        let cancelled = false;
        Promise.all([getSubjectOptions(), getLevelOptions()])
            .then(([subjects, levels]) => {
                if (!cancelled) setDynOptions({ subjects, levels });
            })
            .catch(() => { /* loaders already fall back internally */ })
            .finally(() => { if (!cancelled) setOptionsLoading(false); });
        return () => { cancelled = true; };
    }, []);

    /** Options for a select field: dynamic (API) or static (translated slugs). */
    const resolveOptions = React.useCallback((field) => {
        if (field.optionsSource === 'subjects') return dynOptions.subjects;
        if (field.optionsSource === 'levels')   return dynOptions.levels;
        return (field.optionKeys ?? []).map(slug => ({
            value: slug,
            label: t(`tools-config:options.${slug}`),
        }));
    }, [dynOptions, t]);

    /** Display label for a stored value (code/slug). Falls back to the raw value. */
    const getOptionLabel = React.useCallback((field, val) => {
        const match = resolveOptions(field).find(o => o.value === val);
        return match?.label ?? val;
    }, [resolveOptions]);

    // -----------------------------------------------------------------------
    // Initial form values
    // -----------------------------------------------------------------------

    const initial = React.useMemo(() => {
        const base = Object.fromEntries(tool.fields.map(f => [f.name, getFieldInitialValue(f)]));
        // Apply any initial values passed by a parent (e.g. follow-up workflows
        // such as diagnostic tests → intervention plan).
        if (initialValues) {
            for (const key of Object.keys(initialValues)) {
                if (initialValues[key] !== undefined && initialValues[key] !== null) {
                    base[key] = initialValues[key];
                }
            }
            // A subject passed by a parent decides the content language, unless the parent passed one too
            if ('language' in base && !initialValues.language && base.subject) {
                base.language = getDefaultLanguage(tool, base.subject);
            }
        }
        return base;
    }, [tool, initialValues]);

    const [values,               setValues]               = React.useState(initial);
    const [output,               setOutput]               = React.useState('');
    const [status,               setStatus]               = React.useState('idle'); // idle | streaming | done | error
    const [error,                setError]                = React.useState('');
    const [history,              setHistory]              = React.useState([]);
    const [savedDoc,             setSavedDoc]             = React.useState(null);
    const [imageStatus,          setImageStatus]          = React.useState('');
    const [showForm,             setShowForm]             = React.useState(true);
    // viewMode: 'document' (default) | 'presentation' | 'worksheet'
    const [viewMode,             setViewMode]             = React.useState('document');
    const [presentationMarkdown, setPresentationMarkdown] = React.useState('');
    const [converting,           setConverting]           = React.useState(''); // '' | 'presentation'
    const [athenasAction,        setAthenasAction]        = React.useState('deepen');
    // Map of fieldName → structured Athenas lesson object (for the lesson card UI)
    const [pickedLessons,        setPickedLessons]        = React.useState({});
    const [exporting,            setExporting]            = React.useState('');
    const [interactiveError,     setInteractiveError]     = React.useState('');
    const [assignment, setAssignment] = useState(null);

    const routerHistory = useHistory();
    const assignmentRun = useRef(0);
    const abortRef  = React.useRef(null);
    const outRef    = React.useRef(null);
    // Last content actually persisted — prevents re-POSTing multi-MB base64
    // payloads on every keystroke in the document workspace.
    const lastSaved = React.useRef('');
    // Same idea, but for the slide deck. Tracked separately because the deck
    // lives in its own state (`presentationMarkdown`) and its own DB column.
    const lastSavedPresentation = React.useRef('');

    const isChat = tool.isChat;

    // -----------------------------------------------------------------------
    // Once dynamic options arrive:
    //  1. Apply `defaultCode` for dynamic selects (never overwriting a value
    //     the user or initialValues already set).
    //  2. Rehydrate legacy values: previously the modal stored display LABELS
    //     ('Mathematics', '5th') instead of codes — map them back to codes so
    //     saved documents / initialValues from the old scheme still preselect.
    // -----------------------------------------------------------------------
    React.useEffect(() => {
        if (optionsLoading) return;
        setValues(prev => {
            let changed = false;
            const next = { ...prev };
            for (const field of tool.fields) {
                if (!field.optionsSource) continue;
                const opts = resolveOptions(field);
                const current = next[field.name];

                if (current) {
                    // Value present but not a known code → try legacy label→code mapping
                    const byValue = opts.find(o => o.value === current);
                    if (!byValue) {
                        const byLabel = opts.find(
                            o => String(o.label).toLowerCase() === String(current).toLowerCase()
                        );
                        if (byLabel) { next[field.name] = byLabel.value; changed = true; }
                    }
                } else if (field.defaultCode) {
                    const match = opts.find(o => o.value === field.defaultCode);
                    if (match) { next[field.name] = match.value; changed = true; }
                }
            }
            return changed ? next : prev;
        });
    }, [optionsLoading, tool, resolveOptions]);

    // -----------------------------------------------------------------------
    // Side effects
    // -----------------------------------------------------------------------

    React.useEffect(() => {
        if (embedded) return undefined;
        const onKey = (e) => { if (e.key === 'Escape') onClose(); };
        window.addEventListener('keydown', onKey);
        return () => window.removeEventListener('keydown', onKey);
    }, [embedded, onClose]);

    React.useEffect(() => {
        if (outRef.current) outRef.current.scrollTop = outRef.current.scrollHeight;
    }, [output]);

    // Auto-save whenever the user edits the generated document.
    // Debounced and de-duplicated: generated documents can carry inline base64
    // images (~1–2 MB per PNG), so a naive save-on-every-change is expensive.
    React.useEffect(() => {
        if (!savedDoc?.id || status !== 'done') return undefined;
        if (output === lastSaved.current) return undefined;
        const timer = setTimeout(() => {
            lastSaved.current = output;
            updateDocument(savedDoc.id, { content: output });
        }, 1200);
        return () => clearTimeout(timer);
    }, [output, savedDoc, status]);

    // Auto-save whenever the user edits the SLIDE DECK.
    // SlideWorkspace writes to `presentationMarkdown`, which the document
    // effect above does not observe — without this effect, edits made right
    // after the first "Convert to presentation" were never persisted.
    React.useEffect(() => {
        if (!savedDoc?.id || status !== 'done') return undefined;
        if (!presentationMarkdown) return undefined;
        if (presentationMarkdown === lastSavedPresentation.current) return undefined;
        const timer = setTimeout(() => {
            lastSavedPresentation.current = presentationMarkdown;
            updateDocument(savedDoc.id, { presentationContent: presentationMarkdown });
        }, 1200);
        return () => clearTimeout(timer);
    }, [presentationMarkdown, savedDoc, status]);

    // -----------------------------------------------------------------------
    // Field helpers
    // -----------------------------------------------------------------------

    const setField = (name, val) => setValues(prev => {
        const next = { ...prev, [name]: val };
        // Choosing a subject resets the content language: English subjects → English,
        // the rest → the tool's default. The teacher can still change it afterwards.
        if (name === 'subject' && 'language' in prev) {
            const subjectField = tool.fields.find(f => f.name === 'subject');
            next.language = getDefaultLanguage(tool, val, subjectField ? getOptionLabel(subjectField, val) : '');
        }
        return next;
    });

    // showIf condition: { field, equals } for one value, { field, notEmpty: true } for a list with items
    const meetsShowIf = (showIf, vals) => {
        if (showIf.all) return showIf.all.every(condition => meetsShowIf(condition, vals));
        const target = vals[showIf.field];
        if (showIf.notEmpty) return Array.isArray(target) ? target.length > 0 : !!target;
        if (showIf.in) return showIf.in.includes(target);
        if (showIf.notIn) return !showIf.notIn.includes(target);
        return target === showIf.equals;
    };

    // Show a conditional field only if its showIf condition is met
    const isFieldVisible = (field) => {
        if (field.hideWithAthenasLesson && hasAthenasLesson(values.question)) return false;
        if (!field.showIf) return true;
        return meetsShowIf(field.showIf, values);
    };

    // Clear a field when it gets hidden, so its unseen value never reaches the prompt
    useEffect(() => {
        const lesson = hasAthenasLesson(values.question);
        const stale = tool.fields.filter(f => values[f.name] && (
            (f.showIf && !meetsShowIf(f.showIf, values)) || (f.hideWithAthenasLesson && lesson)
        ));
        if (!stale.length) return;
        setValues(prev => {
            const next = { ...prev };
            for (const f of stale) next[f.name] = '';
            return next;
        });
    }, [tool, values]);

    const canSubmit = tool.fields.every(f => {
        if (f.type === 'standards' || !f.required) return true;
        if (!isFieldVisible(f)) return true;
        const val = values[f.name];
        if (Array.isArray(val)) return val.length > 0;
        return val && String(val).trim();
    });

    // -----------------------------------------------------------------------
    // Build system prompt (with optional standards injection)
    // -----------------------------------------------------------------------

    function buildSystemWithAthenasCodes() {
        let sys = tool.system || '';
        for (const codeType of ['standards', 'expectations', 'units']) {
            const selected = tool.fields
                .filter(f => f.type === 'standards' && (f.codeType || 'standards') === codeType)
                .flatMap(f => (Array.isArray(values[f.name]) ? values[f.name] : []));
            if (!selected.length) continue;
            const block = selected.map(s => `- **${s.Code}** — ${s.Description}`).join('\n');
            sys += t(`${codeType}.systemBlock`, { block, interpolation: { escapeValue: false } });
        }
        return sys;
    }

    // -----------------------------------------------------------------------
    // Optional few-shot example (matched by subject + grade)
    // -----------------------------------------------------------------------

    async function fetchFewshotExample() {

        if (!tool.useFewshot) return null;
        // Field names are English slugs — 'subject' and 'grade' match tools-config.js.
        // NOTE: values now hold API CODES (e.g. 'MATH', '5'). If the fewshot
        // endpoint filters by display name instead of code, switch to:
        //   getOptionLabel({ optionsSource: 'subjects' }, values.subject)
        const subj = values.subject;
        const gr   = values.grade;
        if (!subj || !gr) return null;
        try {
            const params = new URLSearchParams();
            params.set('subject', subj);
            const res  = await fetch(`/api/fewshot-plans?${params}`);
            const json = await res.json();
            if (!json.examples?.length) return null;
            const gradeDigit = String(gr).match(/\d+|K/i)?.[0] || '';
            return json.examples.find(ex => (ex.scope || '').includes(gradeDigit)) || json.examples[0];
        } catch { return null; }
    }

    // -----------------------------------------------------------------------
    // Core generation (streaming SSE)
    // -----------------------------------------------------------------------

    async function generate(e) {
        e?.preventDefault?.();
        if (!canSubmit || status === 'streaming') return;

        setError('');
        setOutput('');
        setSavedDoc(null);
        assignmentRun.current += 1;
        setAssignment(null);
        setShowForm(true);
        setViewMode('document');
        setPresentationMarkdown('');
        setStatus('streaming');
        lastSaved.current = '';
        lastSavedPresentation.current = '';

        // For API-backed selects, state holds codes ('MATH', '5') but the LLM
        // prompt needs human-readable localized labels ('Matemáticas', '5th').
        const promptValues = { ...values };
        for (const field of tool.fields) {
            if (field.optionsSource && values[field.name]) {
                const value = values[field.name];
                // Multi-selects (e.g. integratedSubjects) hold a list of codes
                promptValues[field.name] = Array.isArray(value)
                    ? value.map(v => getOptionLabel(field, v))
                    : getOptionLabel(field, value);
            }
        }

        const rawUserPrompt = tool.buildPrompt(promptValues, t);
        console.log("RawUserPrompt", rawUserPrompt);

        const userPrompt = isChat ? buildChatPrompt(rawUserPrompt, athenasAction) : rawUserPrompt;
        let systemPrompt = [buildSystemWithAthenasCodes(), tool.buildSystem(t, values)].filter(Boolean).join('\n\n');

        console.log("SystemPrompt", systemPrompt);
        const example = await fetchFewshotExample();
        if (example) {
            systemPrompt += t('fewshot.injection', {
                subject:    example.subject,
                scope:      example.scope,
                unit:       example.unit,
                week:       example.week || '',
                weekSuffix: example.week ? t('fewshot.weekSuffix', { week: example.week }) : '',
                text:       example.text,
                interpolation: { escapeValue: false },
            });
        }

        const ctrl = new AbortController();
        abortRef.current = ctrl;

        console.log("REQUEST", JSON.stringify({ model, system: systemPrompt, user: userPrompt }));
        try {
            const res = await fetch(Settings.API_AI_URL + '/aiservicesapi/generate?isFromTeacherApp=true', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
                body:    JSON.stringify({ model, system: systemPrompt, user: systemPrompt + ' ' + userPrompt }),
                signal:  ctrl.signal,
            });
            if (!res.ok || !res.body) {
                const txt = await res.text().catch(() => '');
                throw new Error(`HTTP ${res.status} ${txt}`);
            }

            const reader  = res.body.getReader();
            const decoder = new TextDecoder();
            let acc = '';
            let buf = '';

            while (true) {
                const { value, done } = await reader.read();
                if (done) break;
                buf += decoder.decode(value, { stream: true });
                const lines = buf.split('\n');
                buf = lines.pop() ?? '';
                for (const line of lines) {
                    if (!line.startsWith('data:')) continue;
                    const payload = line.slice(5).trim();
                    if (payload === '[DONE]') continue;
                    try {
                        const json = JSON.parse(payload);
                        if (json.error) { setStatus('error'); setError(json.error); ctrl.abort(); return; }
                        if (json.delta) { acc += json.delta; setOutput(acc); }
                    } catch { /* ignore non-JSON keep-alives */ }
                }
            }

            setStatus('done');
            // resolveImageTags swaps [IMAGE: ...] tags for generated images.
            // The generator returns raw base64, so normalize before rendering.
            acc = await resolveImageTags(acc, setOutput, setImageStatus, t);
            acc = normalizeBase64Images(acc);
            console.log("Normalized", acc);
            acc = cleanGeneratedOutput(acc);
            console.log("Cleaned", acc);
            setOutput(acc);
            setShowForm(false);
            setViewMode('document');

            lastSaved.current = acc;
            // AWAITED: saveGeneratedDocument is async, so assigning its promise
            // straight into state left `savedDoc.id` undefined and every
            // subsequent updateDocument() call silently no-op'd.
            const doc = await saveGeneratedDocument({
                title:     tool.title,
                toolTitle: tool.title,
                category:  tool.category,
                model,
                content:   acc,
                prompt:    userPrompt,
                values,   // codes — language-independent, safe to rehydrate later
                kind:      tool.outputKind || 'markdown',
            });
            setSavedDoc(doc);
            // The document is ready, the assignment reports its own progress
            if (values.createAssignment) {
                handleCreateAssignment(acc);
                if (isChat) setField('createAssignment', false);
            }

            if (isChat) setHistory(prev => [...prev, { q: userPrompt, a: acc, model }]);
        } catch (err) {
            if (err.name === 'AbortError') { setStatus('idle'); return; }
            setStatus('error');
            setError(err.message || String(err));
        }
    }

    function cancel() {
        abortRef.current?.abort();
        setStatus('idle');
    }
    const handleCreateAssignment = async (content) => {
        const run = assignmentRun.current;
        setAssignment({ status: 'creating' });
        try {
            // Text the teacher typed in the tool, the conversion copies it into the assignment so the student can read what the questions ask about.
            // Left out when it is an Athenas lesson, because that text is the teacher's lesson plan (activities, objectives, descriptions etc), not something for the student to read.
            const fieldText = tool.assignmentSourceField ? values[tool.assignmentSourceField] : '';
            const sourceText = hasAthenasLesson(fieldText) ? '' : fieldText;
            // When the text is the reading itself, it goes to the assignment as is instead of the AI's copy
            const created = await createAssignment({
                output: content,
                sourceText,
                sourceIsReading: !!tool.assignmentSourceIsReading,
                conversionKey: tool.assignmentConversionKey,
                t,
            });
            if (run === assignmentRun.current) setAssignment({ status: 'done', ...created });
        } catch (e) {
            if (run === assignmentRun.current) setAssignment({ status: 'error', code: e.code });
        }
    }

    // The assignment editor reads the quiz id from the location
    const openAssignment = () => {
        if (assignment?.quizId) routerHistory?.push({ pathname: '/assignment-manager', quizId: assignment.quizId });
    };

    // -----------------------------------------------------------------------
    // SSE stream → single string (used by convertToPresentation)
    // -----------------------------------------------------------------------

    async function streamToString({ system, user }) {
        const res = await fetch(Settings.API_AI_URL + '/aiservicesapi/generate?isFromTeacherApp=true', {
            method:  'POST',
            headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
            body:    JSON.stringify({ model, system, user }),
        });
        if (!res.ok || !res.body) throw new Error(`HTTP ${res.status}`);

        const reader  = res.body.getReader();
        const decoder = new TextDecoder();
        let acc = '';
        let buf = '';

        while (true) {
            const { value, done } = await reader.read();
            if (done) break;
            buf += decoder.decode(value, { stream: true });
            const lines = buf.split('\n');
            buf = lines.pop() ?? '';
            for (const line of lines) {
                if (!line.startsWith('data:')) continue;
                const payload = line.slice(5).trim();
                if (payload === '[DONE]') continue;
                try {
                    const json = JSON.parse(payload);
                    if (json.error) throw new Error(json.error);
                    if (json.delta) acc += json.delta;
                } catch { /* ignore */ }
            }
        }
        return acc;
    }

    // -----------------------------------------------------------------------
    // "Convert to Presentation" — dedicated AI pass, then opens slide editor
    // -----------------------------------------------------------------------

    async function convertToPresentation() {
        if (presentationMarkdown) { setViewMode('presentation'); return; }
        setConverting('presentation');
        setError('');
        try {
            const sys = t('convertToPresentation.systemPrompt', { interpolation: { escapeValue: false } });
            // Strip already-rendered images so the model doesn't echo alt text
            // into bullets — and so multi-MB data URLs never enter the prompt.
            const sourceClean = String(output)
                .replace(/!\[[^\]]*\]\([^)]+\)/g, '')
                .replace(/\n{3,}/g, '\n\n')
                .trim();
            const user = t('convertToPresentation.userPrompt', { sourceClean, interpolation: { escapeValue: false } });

            const res = await fetch(Settings.API_AI_URL + '/aiservicesapi/generate?isFromTeacherApp=true', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
                body:    JSON.stringify({ model: DEFAULT_AI_MODEL, system: sys, user }),
            });
            if (!res.ok || !res.body) throw new Error(`HTTP ${res.status}`);

            const reader  = res.body.getReader();
            const decoder = new TextDecoder();
            let md  = '';
            let buf = '';

            while (true) {
                const { value, done } = await reader.read();
                if (done) break;
                buf += decoder.decode(value, { stream: true });
                const lines = buf.split('\n');
                buf = lines.pop() ?? '';
                for (const line of lines) {
                    if (!line.startsWith('data:')) continue;
                    const payload = line.slice(5).trim();
                    if (payload === '[DONE]') continue;
                    try {
                        const json = JSON.parse(payload);
                        if (json.error) throw new Error(json.error);
                        if (json.delta) md += json.delta;
                    } catch { /* ignore */ }
                }
            }

            // Deduplicate identical [IMAGE: ...] tags within each slide section
            md = md.split(/(?=^## )/m).map(section => {
                const seen = new Set();
                return section.replace(/\[IMAGE:\s*([^\]]+)\]/gi, (full, prompt) => {
                    const key = prompt.trim().toLowerCase();
                    if (seen.has(key)) return '';
                    seen.add(key);
                    return full;
                });
            }).join('');

            md = await resolveImageTags(md, () => {}, setImageStatus, t);
            md = normalizeBase64Images(md);
            md = cleanGeneratedOutput(md);
            setPresentationMarkdown(md);
            setViewMode('presentation');

            // Persist the freshly converted deck right away, so the conversion
            // survives a reload even if the user never edits a single slide.
            if (savedDoc?.id) {
                lastSavedPresentation.current = md;
                try {
                    await updateDocument(savedDoc.id, { presentationContent: md });
                } catch (saveErr) {
                    // Non-fatal: the debounced effect will retry on the next edit.
                    lastSavedPresentation.current = '';
                    console.error('Failed to persist presentation', saveErr);
                }
            }
        } catch (err) {
            setError(t('convert.error', { message: err.message || err }));
        } finally {
            setConverting('');
        }
    }

    // -----------------------------------------------------------------------
    // Copy to clipboard
    // -----------------------------------------------------------------------

    function copyOut() {
        navigator.clipboard.writeText(output).then(() => {
            const el = document.querySelector('.tm-copy');
            if (el) { el.classList.add('ok'); setTimeout(() => el.classList.remove('ok'), 1200); }
        });
    }

    // -----------------------------------------------------------------------
    // Export handlers
    // -----------------------------------------------------------------------

    // Name in the webapp language
    const toolName  = t(`ai-tools:tools.${tool.key}.name`, { defaultValue: tool.title });
    const safeTitle = toolName.replace(/\s+/g, '_');

    async function handleExportPDF() {
        setExporting('pdf');
        try { await exportMarkdownPDF(normalizeBase64Images(output), `${safeTitle}.pdf`, toolName, t); }
        finally { setExporting(''); }
    }

    const handleExportWord = async () => {
        setExporting('word');
        try {
            await exportWord(normalizeBase64Images(output), `${safeTitle}.docx`, toolName, t);
        } finally {
            setExporting('');
        }
    }

    async function handleExportPPTX() {
        setExporting('pptx');
        const source = normalizeBase64Images(presentationMarkdown || output);
        try { await exportPPTX(source, `${safeTitle}.pptx`, toolName, t); }
        finally { setExporting(''); }
    }

    async function handleExportWorksheet() {
        setExporting('worksheet');
        try { await exportWorksheet(normalizeBase64Images(output), `${safeTitle}_worksheet.pdf`, toolName, t); }
        finally { setExporting(''); }
    }

    // -----------------------------------------------------------------------
    // Diagnostic tests → intervention plan bridge
    // -----------------------------------------------------------------------

    function handleSwitchToIntervention() {
        if (!onSwitchTool) return;
        // 'unit' and 'topic' are English field slugs defined in tools-config.js
        const topic    = values.unit || values.topic || t('interventionBridge.defaultTopic');
        // Strip images first: a data URL would eat the whole 1800-char budget.
        const plain     = String(output || '').replace(/!\[[^\]]*\]\([^)]+\)/g, '');
        const truncated = plain.slice(0, 1800);
        const gapsSeed  = t('interventionBridge.gapsSeed', {
            topic,
            truncated,
            interpolation: { escapeValue: false },
        });
        onSwitchTool('intervention-plan', {
            // subject/grade are API codes — they match the intervention-plan
            // selects (same optionsSource) and preselect correctly.
            subject:   values.subject,
            grade:     values.grade,
            // Pass the English slug, NOT a translated label: the severity
            // select's option values are slugs and translate at render time.
            severity:  '1-grade-behind',
            gaps:      gapsSeed,
            standards: Array.isArray(values.standards) ? values.standards : [],
            expectations: Array.isArray(values.expectations) ? values.expectations : [],
        });
    }

    // -----------------------------------------------------------------------
    // Interactive assessment
    // -----------------------------------------------------------------------

    async function handleMakeInteractive() {
        setExporting('interactive');
        setInteractiveError('');
        try {
            const res = await fetch('/aiservicesapi/interactive-assessments', {
                method:  'POST',
                headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
                body: JSON.stringify({
                    title:        tool.title,
                    // Send both code and localized label while the endpoint migrates.
                    subject:      values.subject || '',
                    subjectLabel: values.subject
                        ? getOptionLabel({ optionsSource: 'subjects' }, values.subject)
                        : '',
                    grade:        values.grade || '',
                    gradeLabel:   values.grade
                        ? getOptionLabel({ optionsSource: 'levels' }, values.grade)
                        : '',
                    sourceTool:   tool.title,
                    markdown:     normalizeBase64Images(output),
                    values,
                }),
            });
            const json = await res.json();
            if (!res.ok) throw new Error(json.error || `HTTP ${res.status}`);
            window.location.assign(json.teacherUrl);
        } catch (err) {
            setInteractiveError(err.message || String(err));
        } finally {
            setExporting('');
        }
    }

    // -----------------------------------------------------------------------
    // Derived booleans
    // -----------------------------------------------------------------------

    const showWorkspaceHeader = status !== 'streaming' && output !== '';
    const canMakeInteractive  = !!tool.canMakeInteractive;
    // Detect whether the Athenas lesson has been injected into the question field
    const chatHasAthenasLesson = isChat && hasAthenasLesson(values.question);

    // Streaming preview: normalize on the fly so partially-streamed images
    // that already have their payload appear immediately.
    const streamingHtml = React.useMemo(
        () => mdToHtml(normalizeBase64Images(output)),
        [output]
    );

    // -----------------------------------------------------------------------
    // Render
    // -----------------------------------------------------------------------

    const content = (
        <div
            className={embedded ? 'tm-workspace-shell' : 'tm-backdrop'}
            onMouseDown={(e) => { if (!embedded && e.target === e.currentTarget) onClose(); }}
        >
            <div
                className={`tm-modal ${embedded ? 'embedded' : ''}`}
                role={embedded ? 'region' : 'dialog'}
                aria-label={t(`ai-tools:tools.${tool.key}.name`)}
            >
                {showWorkspaceHeader ? (
                    <header className="tm-suite-head">
                        <div className="tm-suite-header-left">
                            <button
                                type="button"
                                className="tm-suite-back-btn"
                                onClick={onClose}
                                aria-label={embedded ? t('common.backToTools') : t('common.close')}
                            >
                                ← {t('common.back')}
                            </button>
                            <div className="tm-suite-title-area">
                                <h3>{t(`ai-tools:tools.${tool.key}.name`)}</h3>
                            </div>
                        </div>

                        <div className="tm-suite-actions" role="toolbar" aria-label={t('output.actionsAriaLabel')}>
                            {viewMode !== 'document' && (
                                <button type="button" className="tm-action-btn ghost" onClick={() => setViewMode('document')} title={t('output.backToDoc')}>
                                    ← {t('output.backToDoc')}
                                </button>
                            )}
                            {viewMode === 'document' && (
                                <>
                                    <button type="button" className="tm-action-btn convert" onClick={convertToPresentation} disabled={!!converting} title={t('output.convertTitle')}>
                                        {converting === 'presentation' ? t('output.converting') : `📊 ${t('output.convertBtn')}`}
                                    </button>
                                    <button type="button" className="tm-action-btn worksheet" onClick={() => setViewMode('worksheet')} title={t('output.worksheetTitle')}>
                                        📝 {t('output.worksheetBtn')}
                                    </button>
                                </>
                            )}
                            <DownloadMenu
                                exporting={exporting}
                                onPDF={handleExportPDF}
                                onPPTX={handleExportPPTX}
                                onWorksheet={handleExportWorksheet}
                                onWord={handleExportWord}
                            />
                            <button type="button" className="tm-copy tm-action-btn" onClick={copyOut} title={t('output.copy')}>
                                📋 {t('output.copy')}
                            </button>
                            {/**canMakeInteractive && (
                             <button type="button" className="tm-action-btn" onClick={handleMakeInteractive} disabled={!!exporting} title={t('output.interactiveTitle')}>
                             {exporting === 'interactive' ? '⏳' : '▦'} {t('output.interactive')}
                             </button>
                             )**/}
                            {tool.suggestsInterventionPlan && onSwitchTool && (
                                <button
                                    type="button"
                                    className="tm-action-btn"
                                    onClick={handleSwitchToIntervention}
                                    title={t('output.interventionTitle')}
                                    style={{ background: '#FFE9D6', color: '#A8521A', fontWeight: 700 }}
                                >
                                    🫶 {t('output.intervention')}
                                </button>
                            )}
                            <button
                                type="button"
                                className={`tm-header-toggle-form ${showForm ? 'active' : ''}`}
                                onClick={() => setShowForm(prev => !prev)}
                                title={t('output.paramsTitle')}
                            >
                                ⚙️ {t('output.params')}
                            </button>
                        </div>
                    </header>
                ) : (
                    <header className="tm-head">
                        <div>
                            <div className="tm-eyebrow"><Ic.sparkle /> {t('modal.eyebrow')}</div>
                            <h2>{t(`ai-tools:tools.${tool.key}.name`)}</h2>
                            <p>{t(`ai-tools:tools.${tool.key}.subtitle`)}</p>
                            {!tool.generatesImages && (
                                <p className="tm-head-notice">{t('modal.noImagesNotice')}</p>
                            )}
                        </div>
                        <button
                            className={embedded ? 'tm-back' : 'tm-close'}
                            onClick={onClose}
                            aria-label={embedded ? t('common.backToTools') : t('common.close')}
                        >
                            {embedded ? t('common.back') : '×'}
                        </button>
                    </header>
                )}

                <div className={`tm-body ${showForm ? '' : 'form-collapsed'}`}>
                    <form className="tm-form" onSubmit={generate}>
                        {/* Athenas assistant action selector */}
                        {isChat && (
                            <label className="tm-field tm-athenas-action">
                                <span>{t('chat.athenasActionLabel')}</span>
                                <SearchableSelect
                                    value={athenasAction}
                                    onChange={setAthenasAction}
                                    options={ATHENAS_ASSISTANT_ACTION_KEYS.map(key => ({
                                        value: key,
                                        label: t(`chat.actions.${key}`),
                                    }))}
                                    ariaLabel={t('chat.actions.label')}
                                    placeholder={t('chat.actions.placeholder')}
                                    noResultsText={t('common.noResults')}
                                    clearLabel={t('common.clear')}
                                />
                                <small>{t('chat.athenasActionHint')}</small>
                            </label>
                        )}

                        {/* Dynamic tool fields */}
                        {tool.fields.filter(isFieldVisible)
                            .sort((a, b) => (a.name === 'createAssignment') - (b.name === 'createAssignment'))
                            .map(field => {

                                if (field.type === 'standards') {
                                    // values[...] now holds the API CODE for subject/grade.
                                    // AthenasCodePicker (and its backend) must match by code.
                                    // Fixed-subject standards fields define `subjectCode`
                                    // in tools-config.js (e.g. 'MATH') instead of a name.
                                    const subj = field.subjectField ? values[field.subjectField] : field.subject;
                                    const gr   = field.gradeField   ? values[field.gradeField]   : field.grade;
                                    return (
                                        <div key={field.name} className="tm-field">
                                            <AthenasCodePicker
                                                subject={subj}
                                                grade={gr}
                                                codeType={field.codeType}
                                                relatedStandards={field.standardsField && Array.isArray(values[field.standardsField]) ? values[field.standardsField] : undefined}
                                                value={Array.isArray(values[field.name]) ? values[field.name] : []}
                                                onChange={(arr) => setField(field.name, arr)}
                                            />
                                        </div>
                                    );
                                }

                                if (field.type === 'checkbox') {
                                    return (
                                        <div key={field.name} className="tm-field tm-field-checkbox">
                                            {/* Optional heading above a group of checkboxes, e.g. "La lección incluye:" */}
                                            {field.headingKey && <span>{t('tools-config:' + field.headingKey)}</span>}
                                            <label>
                                                <input
                                                    type="checkbox"
                                                    checked={!!values[field.name]}
                                                    onChange={(e) => setField(field.name, e.target.checked)}
                                                />
                                                <span>{t('tools-config:' + field.labelKey)}</span>
                                            </label>
                                            {field.name === 'createAssignment' && <small>{t('assignment.hint')}</small>}
                                        </div>
                                    );
                                }

                                // Several values shown as removable chips below the select
                                if (field.type === 'multiselect') {
                                    return (
                                        <div key={field.name} className="tm-field">
                                        <span className="tm-field-row">
                                            <span>{t('tools-config:' + field.labelKey)}{field.required ? ' *' : ''}</span>
                                        </span>
                                            <MultiSelectChips
                                                value={values[field.name]}
                                                onChange={(arr) => setField(field.name, arr)}
                                                options={resolveOptions(field)}
                                                exclude={field.excludeField && values[field.excludeField] ? [values[field.excludeField]] : []}
                                                placeholder={field.placeholderKey ? t(`tools-config:${field.placeholderKey}`) : t('form.choose')}
                                                loading={!!field.optionsSource && optionsLoading}
                                                ariaLabel={t('tools-config:' + field.labelKey)}
                                                allowCustom={!!field.allowCustom}
                                                customPlaceholder={field.customPlaceholderKey ? t(`tools-config:${field.customPlaceholderKey}`) : ''}
                                            />
                                        </div>
                                    );
                                }

                                return (
                                    <div key={field.name} className="tm-field">
                                    <span className="tm-field-row">
                                        <span>{t('tools-config:' + field.labelKey)}{field.required ? ' *' : ''}</span>
                                        {field.type === 'textarea' && (
                                            <span style={{ display: 'inline-flex', gap: 6, flexWrap: 'wrap' }}>
                                                <FileUploadButton onText={(text) => setField(field.name, text)} />
                                                <AthenasLessonPicker
                                                    subject={values.subject}
                                                    grade={values.grade}
                                                    onPick={(lesson) => {
                                                        setField(field.name, formatLessonAsReference(lesson, t));
                                                        setPickedLessons(prev => ({ ...prev, [field.name]: lesson }));
                                                    }}
                                                />
                                            </span>
                                        )}
                                    </span>

                                        {field.type === 'textarea' ? (
                                            pickedLessons[field.name] ? (
                                                <AthenasLessonCard
                                                    lesson={pickedLessons[field.name]}
                                                    onRemove={() => {
                                                        setPickedLessons(prev => {
                                                            const next = { ...prev };
                                                            delete next[field.name];
                                                            return next;
                                                        });
                                                        setField(field.name, '');
                                                    }}
                                                />
                                            ) : (
                                                <textarea
                                                    rows={field.rows || 4}
                                                    placeholder={field.placeholderKey ? t(`ai-tools:${field.placeholderKey}`) : undefined}
                                                    value={values[field.name]}
                                                    onChange={(e) => setField(field.name, e.target.value)}
                                                />
                                            )
                                        ) : field.type === 'longtext' ? (
                                            <textarea
                                                rows={field.rows || 3}
                                                // Placeholders live in ai-tools for older tools and in tools-config for newer ones
                                                placeholder={field.placeholderKey
                                                    ? t(`ai-tools:${field.placeholderKey}`, { defaultValue: t(`tools-config:${field.placeholderKey}`) })
                                                    : undefined}
                                                value={values[field.name]}
                                                onChange={(e) => setField(field.name, e.target.value)}
                                            />
                                        ) : field.type === 'select' ? (
                                            (() => {
                                                const isDynamic = !!field.optionsSource;
                                                const opts = resolveOptions(field);
                                                const busy = isDynamic && optionsLoading;
                                                const off = field.disabledIf && meetsShowIf(field.disabledIf, values);
                                                const warn = !off && field.warnIf && meetsShowIf(field.warnIf, values);
                                                return (
                                                    <>
                                                        <SearchableSelect
                                                            value={values[field.name]}
                                                            options={opts}
                                                            disabled={busy || off}
                                                            onChange={(v) => setField(field.name, v)}
                                                            placeholder={busy ? t('form.loading') : t('form.choose')}
                                                            ariaLabel={field.label}
                                                            noResultsText={t('common.noResults')}
                                                            clearLabel={t('common.clear')}
                                                        />
                                                        {off && field.disabledIf.noteKey && (
                                                            <small className="tm-field-note">{t('tools-config:' + field.disabledIf.noteKey)}</small>
                                                        )}
                                                        {warn && (
                                                            <small className="tm-field-note">{t('tools-config:' + field.warnIf.noteKey)}</small>
                                                        )}
                                                    </>
                                                );
                                            })()
                                        ) : (() => {
                                            const off = field.disabledIf && meetsShowIf(field.disabledIf, values);
                                            return (
                                                <>
                                                    <input
                                                        type={field.type || 'text'}
                                                        placeholder={field.placeholderKey ? t(`tools-config:${field.placeholderKey}`) : undefined}
                                                        value={values[field.name]}
                                                        disabled={off}
                                                        onChange={(e) => setField(field.name, e.target.value)}
                                                    />
                                                    {off && field.disabledIf.noteKey && (
                                                        <small className="tm-field-note">{t('tools-config:' + field.disabledIf.noteKey)}</small>
                                                    )}
                                                </>
                                            );
                                        })()}
                                    </div>
                                );
                            })}

                        {status === 'streaming' ? (
                            <button type="button" className="tm-btn danger" onClick={cancel}>
                                {t('form.cancel')}
                            </button>
                        ) : (
                            <button type="submit" className="tm-btn primary" disabled={!canSubmit}>
                                <Ic.sparkle />
                                {isChat
                                    ? (chatHasAthenasLesson ? t('form.createWithAthenas') : t('form.ask'))
                                    : t('form.generate')
                                }
                                <Ic.arrow />
                            </button>
                        )}
                    </form>

                    {/* Output panel */}
                    <div className="tm-output">
                        {!showWorkspaceHeader && (
                            <div className="tm-out-head">
                                <div className="tm-out-title">
                                    {status === 'streaming' && <span className="tm-spinner" />}

                                    {status === 'streaming' && <span>{t('output.generating')}</span>}
                                    {status === 'done'      && <span>{savedDoc ? t('output.saved') : t('output.result')}</span>}
                                    {status === 'error'     && <span style={{ color: '#b3261e' }}>{t('output.error')}</span>}
                                    {imageStatus            && <span className="tm-img-status">🎨 {imageStatus}</span>}
                                </div>
                                {output && status !== 'streaming' && (
                                    <div className="tm-out-actions">
                                        <button type="button" className="tm-copy" onClick={copyOut} title={t('output.copy')}>
                                            📋 {t('output.copy')}
                                        </button>
                                        <button type="button" onClick={handleExportPDF} disabled={!!exporting} title={t('download.pdf')}>
                                            {exporting === 'pdf' ? '⏳' : '📄'} {t('download.pdf')}
                                        </button>
                                        <button type="button" onClick={handleExportWord} disabled={!!exporting} title={t('download.word')}>
                                            {exporting === 'word' ? '⏳' : '📝'} {t('download.word')}
                                        </button>
                                        <button type="button" onClick={handleExportPPTX} disabled={!!exporting} title={t('download.pptx')}>
                                            {exporting === 'pptx' ? '⏳' : '📊'} {t('download.pptxShort')}
                                        </button>
                                        <button type="button" onClick={handleExportWorksheet} disabled={!!exporting} title={t('download.worksheet')}>
                                            {exporting === 'worksheet' ? '⏳' : '📝'} {t('download.worksheetShort')}
                                        </button>
                                        {/**canMakeInteractive && (
                                         <button type="button" onClick={handleMakeInteractive} disabled={!!exporting} title={t('output.interactiveTitle')}>
                                         {exporting === 'interactive' ? '⏳' : '▦'} {t('output.interactive')}
                                         </button>
                                         )**/}
                                        {tool.suggestsInterventionPlan && onSwitchTool && (
                                            <button
                                                type="button"
                                                onClick={handleSwitchToIntervention}
                                                title={t('output.interventionTitle')}
                                                style={{ background: '#FFE9D6', color: '#A8521A', fontWeight: 700 }}
                                            >
                                                🫶 {t('output.intervention')}
                                            </button>
                                        )}
                                        <button type="button" onClick={generate} title={t('form.regenerate')}>
                                            ↻ {t('form.regenerate')}
                                        </button>
                                    </div>
                                )}
                            </div>
                        )}

                        {interactiveError && <div className="tm-inline-error">{interactiveError}</div>}

                        {assignment && (
                            <div className={`tm-assignment tm-assignment--${assignment.status}`} role="status">
                                {assignment.status === 'creating' && (
                                    <span className="tm-assignment-msg"><span className="tm-spinner" /> {t('assignment.creating')}</span>
                                )}
                                {assignment.status === 'done' && (
                                    <>
                                        <span className="tm-assignment-msg">
                                          {t('assignment.created', { title: assignment.title, questionCount: assignment.questionCount })}
                                        </span>
                                        <button type="button" onClick={openAssignment}>{t('assignment.open')}</button>
                                    </>
                                )}
                                {assignment.status === 'error' && (
                                    <>
                                        <span className="tm-assignment-msg">
                                            {t(`assignment.errors.${assignment.code || 'save'}`, { defaultValue: t('assignment.errors.save') })}
                                        </span>
                                        <button type="button" onClick={() => handleCreateAssignment(output)}>{t('assignment.retry')}</button>
                                    </>
                                )}
                            </div>
                        )}

                        {error ? (
                            <pre className="tm-error">{error}</pre>
                        ) : status === 'streaming' ? (
                            <div ref={outRef} className="tm-md" dangerouslySetInnerHTML={{ __html: streamingHtml }} />
                        ) : output ? (
                            <>
                                {viewMode === 'document'     && <DocumentWorkspace  value={output}               onChange={setOutput} />}
                                {viewMode === 'presentation' && <SlideWorkspace      value={presentationMarkdown} onChange={setPresentationMarkdown} />}
                                {viewMode === 'worksheet'    && <WorksheetWorkspace value={output} onChange={setOutput} title={t(`ai-tools:tools.${tool.key}.name`)} />}
                            </>
                        ) : (
                            <div className="tm-out-placeholder">
                                <span className="tm-dim">{t('output.placeholder')}</span>
                            </div>
                        )}

                        {isChat && history.length > 0 && status === 'done' && (
                            <div className="tm-chat-history">
                                <details>
                                    <summary>{t('chat.history', { count: history.length })}</summary>
                                    {history.slice(0, -1).reverse().map((entry, i) => (
                                        <div key={i} className="tm-chat-item">
                                            <div className="tm-chat-q">{entry.q}</div>
                                            <div
                                                className="tm-md"
                                                dangerouslySetInnerHTML={{ __html: mdToHtml(normalizeBase64Images(entry.a)) }}
                                            />
                                        </div>
                                    ))}
                                </details>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );

    // Modal mode renders into document.body so the backdrop escapes any
    // transformed / overflow-clipped ancestor; embedded mode stays inline.
    // Guard against SSR where `document` is undefined.
    if (embedded) return content;
    if (typeof document === 'undefined') return null;
    return createPortal(content, document.body);
}

export default ToolModal;
