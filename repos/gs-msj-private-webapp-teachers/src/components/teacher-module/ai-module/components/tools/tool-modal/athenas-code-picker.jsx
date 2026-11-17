import React, { useState, useEffect, useMemo, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import {GRADES, SUBJECTS} from "../tool-config/tools-config-base";
import {fetchAthenasCode} from "../../../../../../api/ai-module";
import {fold} from "./utils/searchable-select/searchable-select";

/**
 * AthenasCodePicker — fetches the Athenas codes of one kind (standards, expectations or units) 
 * for the given subject + grade and lets the teacher multi-select. 
 * Returns the selected codes (full objects) via onChange.
 *
 * Props:
 *   subject:  string (e.g. "Mathematics")
 *   grade:    string (e.g. "5th", "K")
 *   codeType: 'standards' | 'expectations' | 'units'
 *   relatedStandards: for expectations, the selected standards, their expectations are listed first
 *   value:    array of selected standard objects
 *   onChange: (selectedArray) => void
 */

const CODE_TYPES = {
    units:        { typeId: 2,    keys: 'athenas-code-picker:units.' },
    standards:    { typeId: null, keys: 'athenas-code-picker:standards.' },
    expectations: { typeId: 7,    keys: 'athenas-code-picker:expectations.' },
};

// Identity of a code
const keyOf = (code) => (code.Id ?? code.Code);

export default function AthenasCodePicker({ subject, grade, codeType = 'standards', relatedStandards = [], value = [], onChange }) {
    const subjectValue = SUBJECTS.find(s => s.label === subject)?.value ?? subject;
    const gradeValue   = GRADES.find(g => g.label === grade)?.value   ?? grade;
    const { typeId, keys } = CODE_TYPES[codeType] || CODE_TYPES.standards;
    const isExpectations = codeType === 'expectations';

    const { t } = useTranslation(['athenas-code-picker', 'common']);

    const [items, setItems]  = useState([]);
    const [loading, setLoad] = useState(false);
    const [error, setError]  = useState('');
    const [q, setQ]          = useState('');
    const [expanded, setExp] = useState(false);

    // Fetch when derived subject + grade change
    useEffect(() => {
        if (!subjectValue || !gradeValue) {
            setItems([]);
            setLoad(false);
            return;
        }

        let cancelled = false;
        setLoad(true);
        setError('');

        fetchAthenasCode(subjectValue, gradeValue, typeId)
            .then((res) => {

                if (!cancelled) {
                    setItems(res?.data?.Items ?? []);
                    setLoad(false);
                }
            })
            .catch((err) => {
                if (!cancelled) {
                    setError(err.message || 'Failed to load standards');
                    setLoad(false);
                }
            });

        return () => { cancelled = true; };

    }, [subjectValue, gradeValue, typeId]);

    // Clear selections when subject/grade changes (selected may not exist in new set)
    useEffect(() => {
        onChange?.([]);
    }, [subjectValue, gradeValue]);

    const selectedCodes = useMemo(() => new Set(value.map(keyOf)), [value]);

    // Selected standards by Id
    const standardsById = new Map((isExpectations ? relatedStandards : []).map(s => [(s.Id), s]));

    const filtered = useMemo(() => {
        if (!q.trim()) return items;
        // Case and accent-insensitive
        const term = fold(q).trim();
        return items.filter(s =>
            fold(s.Code).includes(term) ||
            fold(s.Description).includes(term) ||
            fold(s.CodeTypeName).includes(term)
        );
    }, [items, q]);

    // Group filtered standards by codeTypeName for visual structure. With standards selected,
    // expectations are grouped under each of them first
    const grouped = useMemo(() => {
        const groups = {};
        if (standardsById.size) {
            const label = (std) => `${std.Code} · ${std.Description}`;
            for (const std of standardsById.values()) groups[label(std)] = [];

            const other = t(`${keys}otherGroup`);
            for (const expectation of filtered) {
                // Get the standard associated with this expectation
                const std = standardsById.get((expectation.RelationCodeId));
                (groups[std ? label(std) : other] ??= []).push(expectation);
            }
            // A selected standard without expectations or none matching the search shows no group
            for (const name of Object.keys(groups)) if (!groups[name].length) delete groups[name];
            return groups;
        }
        for (const std of filtered) {
            (groups[std.CodeTypeName || '—'] ??= []).push(std);
        }
        return groups;
    }, [filtered, standardsById, keys, t]);

    // An expectation code is only its number, so it takes its standard's code when
    // that standard is selected, or the type name otherwise
    const chipLabel = (code) => {
        if (!isExpectations) return code.Code;
        const std = standardsById.get((code.RelationCodeId));
        return std ? `${std.Code} ${code.Code}` : t(`${keys}chip`, { code: code.Code });
    }

    function toggle(std) {
        const key = keyOf(std);
        if (selectedCodes.has(key)) onChange?.(value.filter(s => keyOf(s) !== key));
        else onChange?.([...value, std]);
    }

    function clear() { onChange?.([]); }

    // When neither subject nor grade are set the picker cannot do anything useful.
    if (!subject && !grade) return null;

    // Only one of (subject, grade) is set — show a hint about what is still missing.
    if (!subject || !grade) {
        // The standards picker already shows it, so the units and expectations ones stay hidden
        if (codeType !== 'standards') return null;
        const missingKey = !subject ? 'subject' : 'grade';
        return (
            <div className="sp-hint">
                {t(`${keys}hint.chooseMissing`, { missing: t(`athenas-code-picker:fields.${missingKey}`) })}
            </div>
        );
    }

    return (
        <div className={`sp ${expanded ? 'expanded' : ''}`}>
            <div className="sp-head">
                <button
                    type="button"
                    className="sp-toggle"
                    onClick={() => setExp(e => !e)}
                    aria-expanded={expanded}
                >
                    <span className={`sp-chev ${expanded ? 'open' : ''}`}>▸</span>
                    <span>{t(`${keys}panel.title`)}</span>
                    {value.length > 0 && <span className="sp-count">{value.length}</span>}
                    {loading && <span className="sp-loading" />}
                </button>
                {value.length > 0 && (
                    <button type="button" className="sp-clear" onClick={clear}>
                        {t('common:general.clear')}
                    </button>
                )}
            </div>

            {expanded && (
                <div className="sp-body">
                    {error ? (
                        <div className="sp-error">{t('athenas-code-picker:status.error', { message: error })}</div>
                    ) : loading ? (
                        <div className="sp-empty">{t('athenas-code-picker:status.loading', 'Loading…')}</div>
                    ) : items.length === 0 ? (
                        <div className="sp-empty">
                            {t(`${keys}status.empty`, { subject, grade })}
                        </div>
                    ) : (
                        <>
                            <div className="sp-search">
                                <input
                                    type="text"
                                    value={q}
                                    onChange={(e) => setQ(e.target.value)}
                                    placeholder={t(`${keys}search.placeholder`, {
                                        count: items.length,
                                        subject,
                                        grade,
                                    })}
                                />
                            </div>

                            {value.length > 0 && (
                                <div className="sp-selected">
                                    {value.map(std => (
                                        <button
                                            type="button"
                                            key={keyOf(std)}
                                            className="sp-chip"
                                            onClick={() => toggle(std)}
                                            title={std.Description}
                                        >
                                            {chipLabel(std)} ×
                                        </button>
                                    ))}
                                </div>
                            )}

                            <div className="sp-list" role="listbox">
                                {Object.entries(grouped).map(([domain, list]) => (
                                    <div key={domain} className="sp-group">
                                        <div className="sp-group-head">
                                            {domain}
                                            <span className="sp-group-count">{list.length}</span>
                                        </div>
                                        {list.map(std => {
                                            const checked = selectedCodes.has(keyOf(std));
                                            return (
                                                <label key={keyOf(std)} className={`sp-item ${checked ? 'on' : ''}`}>
                                                    <input
                                                        type="checkbox"
                                                        checked={checked}
                                                        onChange={() => toggle(std)}
                                                    />
                                                    <span className="sp-code">{std.Code}</span>
                                                    <span className="sp-exp">{std.Description}</span>
                                                </label>
                                            );
                                        })}
                                    </div>
                                ))}
                                {filtered.length === 0 && (
                                    <div className="sp-empty">
                                        {t('athenas-code-picker:search.noResults', { query: q })}
                                    </div>
                                )}
                            </div>
                        </>
                    )}
                </div>
            )}
        </div>
    );
}