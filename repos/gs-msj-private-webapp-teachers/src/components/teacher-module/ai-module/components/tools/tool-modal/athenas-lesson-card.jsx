/**
 * athenas-lesson-card.jsx
 * Visible confirmation card shown when a teacher has loaded an Athenas lesson
 * into a form field. Replaces the raw textarea so the teacher can see exactly
 * what the AI will analyze, and can remove it if needed.
 */
import React from 'react';
import { useTranslation } from 'react-i18next';
import { stripDefinitionHtml } from './tool-modal-utils.jsx';

export default function AthenasLessonCard({ lesson, onRemove }) {
    const { t } = useTranslation('tool-modal');
    const [expanded, setExpanded] = React.useState(false);

    if (!lesson) return null;

    const desc       = stripDefinitionHtml(lesson.LessonDetailModel.Description || '');
    const concept    = stripDefinitionHtml(lesson.LessonDetailModel.Concept || '');
    const stdCount   = (lesson.standards   || []).length;
    const defsCount  = (lesson.LessonDefinitionModelList || []).length;
    const objsCount  = (lesson.LessonObjectiveModelList  || []).length;
    const exCount    = (lesson.LessonExampleModelList    || []).length;

    return (
        <div className="tm-athenas-card">
            <div className="tm-athenas-card-head">
                <div className="tm-athenas-card-title">
                    <span className="tm-athenas-card-icon">📘</span>
                    <div>
                        <div className="tm-athenas-card-name">
                            {lesson.LessonModel.LessonTitle || t('athenas.defaultTitle')}
                        </div>
                        <div className="tm-athenas-card-meta">
                            {lesson.LessonModel.SubjectCode && <span>{lesson.LessonModel.SubjectCode}</span>}
                            {lesson.LessonModel.LevelCode && (
                                <><span>·</span><span>{t('athenas.grade')} {lesson.LessonModel.LevelCode}</span></>
                            )}
                            {lesson.LessonModel.LessonNo && (
                                <><span>·</span><span>{t('athenas.lesson')} {lesson.LessonModel.LessonNo}</span></>
                            )}

                            {lesson.blueprint    && <span className="tm-athenas-pill blueprint">📐 Blueprint</span>}
                            {lesson.isGapClosing && <span className="tm-athenas-pill gap">🌉 Gap closing</span>}
                        </div>
                    </div>
                </div>
                <button
                    type="button"
                    className="tm-athenas-card-remove"
                    onClick={onRemove}
                    title={t('athenas.remove')}
                >
                    ✕
                </button>
            </div>

            <div className="tm-athenas-card-stats">
                <span><strong>{stdCount}</strong>  {t('athenas.standards')}</span>
                <span><strong>{objsCount}</strong> {t('athenas.objectives')}</span>
                <span><strong>{defsCount}</strong> {t('athenas.definitions')}</span>
                <span><strong>{exCount}</strong>   {t('athenas.examples')}</span>
            </div>

            {stdCount > 0 && (
                <div className="tm-athenas-card-standards">
                    {lesson.standards.slice(0, 6).map((std, i) => (
                        <span key={i} className="tm-athenas-std-chip" title={std.Description || ''}>
                            {std.Code}
                        </span>
                    ))}
                    {lesson.standards.length > 6 && (
                        <span className="tm-athenas-std-more">+{lesson.standards.length - 6}</span>
                    )}
                </div>
            )}

            <button
                type="button"
                className="tm-athenas-card-toggle"
                onClick={() => setExpanded(prev => !prev)}
            >
                {expanded ? t('athenas.hideContent') : t('athenas.showContent')}
            </button>

            {expanded && (
                <div className="tm-athenas-card-body">
                    {concept && (
                        <div className="tm-athenas-section">
                            <div className="tm-athenas-section-title">{t('athenas.centralConcept')}</div>
                            <div className="tm-athenas-section-text">{concept}</div>
                        </div>
                    )}
                    {desc && (
                        <div className="tm-athenas-section">
                            <div className="tm-athenas-section-title">{t('athenas.description')}</div>
                            <div className="tm-athenas-section-text">{desc}</div>
                        </div>
                    )}
                    {objsCount > 0 && (
                        <div className="tm-athenas-section">
                            <div className="tm-athenas-section-title">{t('athenas.learningObjectives')}</div>
                            <ul className="tm-athenas-section-list">
                                {lesson.LessonObjectiveModelList.map((obj, i) => {
                                    const text = stripDefinitionHtml(obj.Desc || obj.Description || '');
                                    return text ? <li key={i}>{text}</li> : null;
                                })}
                            </ul>
                        </div>
                    )}
                    {defsCount > 0 && (
                        <div className="tm-athenas-section">
                            <div className="tm-athenas-section-title">{t('athenas.conceptsDefinitions')}</div>
                            <ul className="tm-athenas-section-list">
                                {lesson.LessonDefinitionModelList.map((def, i) => {
                                    const term = def.Name || '';
                                    const body = stripDefinitionHtml(def.Desc || def.Description || '');
                                    return (term || body)
                                        ? <li key={i}><strong>{term}</strong>{term && body ? ': ' : ''}{body}</li>
                                        : null;
                                })}
                            </ul>
                        </div>
                    )}
                    {exCount > 0 && (
                        <div className="tm-athenas-section">
                            <div className="tm-athenas-section-title">{t('athenas.pedagogicalExamples')}</div>
                            <ul className="tm-athenas-section-list">
                                {lesson.LessonExampleModelList.map((ex, i) => {
                                    const name = ex.Name || '';
                                    const body = stripDefinitionHtml(ex.Desc || ex.Description || '');
                                    return (name || body)
                                        ? <li key={i}>{name && <strong>{name}: </strong>}{body}</li>
                                        : null;
                                })}
                            </ul>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}
