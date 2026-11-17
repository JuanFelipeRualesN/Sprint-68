import React, { useState, useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { getLessonDetailsForTeachers, normalizeGrade, normalizeSubject } from '../../../../../../api/athenas';
import { fetchLessons } from "../../../../../../api/ai-module";
import { getSubjectOptions, getLevelOptions } from '../tool-config/tools-config-options.js';
import { readApiField as read } from '../../../../../../utils/utils';

// ---------------------------------------------------------------------------
// Fallbacks — used only if the subjects/levels API is unavailable.
// Subject codes only; display labels live in the translation files and are
// resolved via t('athenas-lesson-picker:subjects.<code>') at render time.
// ---------------------------------------------------------------------------
const FALLBACK_SUBJECT_CODES = [
    'sp', 'mat-sp', 'sci-sp', 'sci-so', 'en', 'mat-en', 'sci-en',
    'bi-sp', 'qu-sp', 'fi-sp', 'a1-sp', 'a2', 'geo-sp',
];

const FALLBACK_GRADE_CODES = ['k', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'];

/**
 * athenas-lesson-picker — searches the Athenas catalog (live API when there is a
 * JWT in localStorage.auth.Token, otherwise falls back to the local cache).
 *
 * Subject and grade dropdowns are populated from the AI subjects/levels API
 * (already localized by the backend). If the API fails or returns nothing,
 * the hardcoded fallback codes above are used with translation-file labels.
 *
 * Props (all optional; whatever is not provided becomes a dropdown in the modal):
 *   subject:  string  — API subject code (preferred) or legacy display name
 *   grade:    string  — API level code (preferred) or legacy "5th" / "5" / "K"
 *   onPick:   (lesson) => void
 */
export default function AthenasLessonPicker({ subject: subjectProp, grade: gradeProp, onPick }) {
    const { t } = useTranslation(['athenas-lesson-picker', 'common']);

    const [open,     setOpen]     = useState(false);
    const [items,    setItems]    = useState([]);
    const [total,    setTotal]    = useState(0);
    const [loading,  setLoading]  = useState(false);
    const [error,    setError]    = useState('');
    const [q,        setQ]        = useState('');
    const [picking,  setPicking]  = useState('');  // lesson Id currently loading detail

    // Internal filters — initialized from props, but editable by the user
    const [subject, setSubject] = useState('');
    const [grade,   setGrade]   = useState('');

    // API-driven dropdown options: [{ value, label }]
    const [subjectOptions, setSubjectOptions] = useState([]);
    const [gradeOptions,   setGradeOptions]   = useState([]);
    const [optionsLoading, setOptionsLoading] = useState(false);
    const optionsFetchedRef = useRef(false);

    const abortRef    = useRef(null);
    const debounceRef = useRef(null);

    // -----------------------------------------------------------------------
    // Load subjects + levels the first time the picker opens (cached loaders,
    // so repeated opens are free). Fallback lists use translated labels.
    // -----------------------------------------------------------------------
    useEffect(() => {
        if (!open || optionsFetchedRef.current) return;
        optionsFetchedRef.current = true;

        let cancelled = false;
        setOptionsLoading(true);
        Promise.all([getSubjectOptions(), getLevelOptions()])
            .then(([subjects, levels]) => {
                if (cancelled) return;
                setSubjectOptions(
                    subjects?.length
                        ? subjects
                        : FALLBACK_SUBJECT_CODES.map(code => ({
                            value: code,
                            label: t(`athenas-lesson-picker:subjects.${code}`),
                        }))
                );
                setGradeOptions(
                    levels?.length
                        ? levels
                        : FALLBACK_GRADE_CODES.map(code => ({
                            value: code,
                            label: code.toUpperCase() === 'K' ? 'K' : code,
                        }))
                );
            })
            .catch(() => {
                if (cancelled) return;
                setSubjectOptions(FALLBACK_SUBJECT_CODES.map(code => ({
                    value: code,
                    label: t(`athenas-lesson-picker:subjects.${code}`),
                })));
                setGradeOptions(FALLBACK_GRADE_CODES.map(code => ({
                    value: code,
                    label: code.toUpperCase() === 'K' ? 'K' : code,
                })));
            })
            .finally(() => { if (!cancelled) setOptionsLoading(false); });

        return () => { cancelled = true; };
    }, [open, t]);

    // -----------------------------------------------------------------------
    // Sync internal state when the modal opens or when props change.
    // Props may now be API codes (new scheme) or display names / "5th"
    // (legacy scheme) — accept both:
    //   1. exact match against loaded option values → use as-is
    //   2. otherwise run through the legacy normalizers
    // -----------------------------------------------------------------------
    useEffect(() => {
        if (!open) return;

        const rawSubject = subjectProp || '';
        const rawGrade   = gradeProp   || '';

        const subjectIsCode = subjectOptions.some(o => o.value === rawSubject);
        const gradeIsCode   = gradeOptions.some(o => o.value === rawGrade);

        setSubject(subjectIsCode ? rawSubject : (normalizeSubject(rawSubject)[0] || ''));
        setGrade(gradeIsCode ? rawGrade : (normalizeGrade(rawGrade) || ''));
    }, [open, subjectProp, gradeProp, subjectOptions, gradeOptions]);

    // Run search whenever filters change
    useEffect(() => {
        if (!open) return;

        clearTimeout(debounceRef.current);
        const delay = q ? 400 : 0;
        debounceRef.current = setTimeout(async () => {
            setLoading(true);
            setError('');
            try {
                const result = await fetchLessons(subject, grade, q);
                const details = read(result.data, 'LessonsDetails', {});

                setItems(read(details, 'Lessons', []).map(lesson => {
                    const model = read(lesson, 'LessonModel', {});

                    return {
                        ...lesson,
                        lessonModel: {
                            ...model,
                            id:          read(model, 'Id'),
                            lessonNo:    read(model, 'LessonNo'),
                            lessonTitle: read(model, 'LessonTitle'),
                            levelCode:   read(model, 'LevelCode'),
                            subjectCode: read(model, 'SubjectCode'),
                        },
                    };
                }));
                setTotal(read(details, 'TotalResults', 0));

            } catch (err) {
                if (err.name !== 'AbortError') setError(err.message || String(err));
            } finally {
                setLoading(false);
            }
        }, delay);

        return () => clearTimeout(debounceRef.current);
    }, [open, subject, grade, q]);

    async function handlePick(lesson) {
        setPicking(String(lesson.lessonModel.id));
        setError('');
        try {
            const detail = await getLessonDetailsForTeachers(lesson.lessonModel.id);
            const modifier = detail.data?.LessonModifierRequestModel;
            const hasRealContent = !!modifier && (
                modifier.LessonDetailModel?.Description ||
                modifier.LessonDetailModel?.Concept ||
                modifier.LessonDefinitionModelList?.length ||
                modifier.LessonExampleModelList?.length ||
                modifier.LessonObjectiveModelList?.length
            );

            if (!hasRealContent) {
                setError(t('athenas-lesson-picker:errors.loadFailed'));
                return;
            }
            onPick?.(modifier);

            setOpen(false);
        } catch (err) {

            setError(t('athenas-lesson-picker:errors.loadFailed'));
        } finally {
            setPicking('');
        }
    }

    return (
        <>
            <button type="button" className="tm-upload-btn" onClick={() => setOpen(true)}>
                📚 {t('athenas-lesson-picker:trigger.label')}
            </button>

            {open && (
                <div
                    className="alp-backdrop"
                    onMouseDown={(e) => { if (e.target === e.currentTarget) setOpen(false); }}
                >
                    <div className="alp-modal" role="dialog">
                        <header className="alp-head">
                            <div>
                                <h3>
                                    {t('athenas-lesson-picker:header.title')}

                                </h3>
                                <p>
                                    {loading
                                        ? t('athenas-lesson-picker:header.loading')
                                        : t('athenas-lesson-picker:header.count', { count: total.toLocaleString() })}
                                </p>
                            </div>
                            <button type="button" className="alp-close" onClick={() => setOpen(false)}>×</button>
                        </header>

                        {/* Filters — subject + grade dropdowns always visible so user can refine */}
                        <div className="alp-filters">
                            <label className="alp-filter">
                                <span>{t('athenas-lesson-picker:filters.subject')}</span>
                                <select
                                    value={subject}
                                    disabled={optionsLoading}
                                    onChange={(e) => setSubject(e.target.value)}
                                >
                                    <option value="">
                                        {optionsLoading
                                            ? t('common:general.loading')
                                            : t('athenas-lesson-picker:filters.allSubjects')}
                                    </option>
                                    {subjectOptions.map(opt => (
                                        <option key={opt.value} value={opt.value}>
                                            {opt.label} ({opt.value})
                                        </option>
                                    ))}
                                </select>
                            </label>

                            <label className="alp-filter">
                                <span>{t('athenas-lesson-picker:filters.grade')}</span>
                                <select
                                    value={grade}
                                    disabled={optionsLoading}
                                    onChange={(e) => setGrade(e.target.value)}
                                >
                                    <option value="">
                                        {optionsLoading
                                            ? t('common:general.loading')
                                            : t('athenas-lesson-picker:filters.allGrades')}
                                    </option>
                                    {gradeOptions.map(opt => (
                                        <option key={opt.value} value={opt.value}>
                                            {opt.label}
                                        </option>
                                    ))}
                                </select>
                            </label>
                        </div>

                        <div className="alp-search">
                            <input
                                type="text"
                                value={q}
                                onChange={(e) => setQ(e.target.value)}
                                placeholder={t('athenas-lesson-picker:search.placeholder')}
                                autoFocus
                            />
                        </div>

                        <div className="alp-list">
                            {error && (
                                <div className="alp-error">
                                    {t('common:general.error')}: {error}
                                </div>
                            )}
                            {!loading && items.length === 0 && !error && (
                                <div className="alp-empty">
                                    {t('athenas-lesson-picker:list.empty')}
                                    <br />
                                    <small
                                        dangerouslySetInnerHTML={{
                                            __html: t('athenas-lesson-picker:list.emptyHint'),
                                        }}
                                    />
                                </div>
                            )}
                            {items.map(lesson => (
                                <button
                                    key={lesson.lessonModel.id}
                                    type="button"
                                    className="alp-item"
                                    disabled={!!picking}
                                    onClick={() => handlePick(lesson)}
                                >
                                    <div className="alp-item-title">
                                        {lesson.lessonModel.lessonTitle}
                                        {picking === String(lesson.lessonModel.id) && (
                                            <span className="alp-loading"> · {t('athenas-lesson-picker:list.itemLoading')}</span>
                                        )}
                                    </div>
                                    <div className="alp-item-meta">
                                        {lesson.lessonModel.lessonNo && (
                                            <span>{t('athenas-lesson-picker:list.lessonNo', { no: lesson.lessonModel.lessonNo })}</span>
                                        )}
                                        <span>{lesson.lessonModel.subjectCode}</span>
                                        <span>·</span>
                                        <span>{t('athenas-lesson-picker:list.grade', { grade: lesson.lessonModel.levelCode })}</span>
                                        {/*{lesson.Blueprint   === '1' && <span className="alp-bp">📐 {t('athenas-lesson-picker:list.blueprint')}</span>}
                                        {lesson.IsGapClosing === '1' && <span className="alp-gc">🌉 {t('athenas-lesson-picker:list.gapClosing')}</span>} */}
                                    </div>
                                </button>
                            ))}
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}
