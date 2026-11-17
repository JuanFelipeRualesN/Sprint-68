/**
 * multi-select-chips.jsx
 * The modal's SearchableSelect used as an "add" picker: choosing an option adds it as a chip
 * below the select, and each chip has an × to remove it. Chosen options leave the dropdown.
 * With `allowCustom`, a text input under the select adds the teacher's own entries as chips too
 * (Enter or the + button). Custom entries are stored as their text.
 *
 * Props:
 *   value             string[]            option values and custom texts
 *   onChange          (string[]) => void
 *   options           { value, label }[]
 *   exclude           string[]            values to hide (e.g. the lesson's main subject)
 *   placeholder       string
 *   loading           boolean
 *   ariaLabel         string
 *   allowCustom       boolean
 *   customPlaceholder string
 */

import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import SearchableSelect from './utils/searchable-select/searchable-select';
import './multi-select-chips.css';

const normalize = (text) => String(text).trim().toLowerCase();

function MultiSelectChips({
                              value = [],
                              onChange,
                              options = [],
                              exclude = [],
                              placeholder = '',
                              loading = false,
                              ariaLabel,
                              allowCustom = false,
                              customPlaceholder = '',
                          }) {
    const { t } = useTranslation(['tool-modal', 'tools-config']);
    const [customText, setCustomText] = useState('');
    const selected = Array.isArray(value) ? value : [];

    // Custom entries have no option, so their label is the text itself
    const labelOf = (item) => options.find(option => option.value === item)?.label ?? item;

    const available = options.filter(option => !selected.includes(option.value) && !exclude.includes(option.value));
    const chips = selected.filter(item => !exclude.includes(item));

    const add = (item) => {
        if (item && !selected.includes(item)) onChange([...selected, item]);
    };

    const remove = (item) => onChange(selected.filter(entry => entry !== item));

    // A typed text equal to an option label adds that option; one already chosen is not added twice
    const addCustom = () => {
        const text = customText.trim();
        if (!text) return;
        const option = options.find(o => normalize(o.label) === normalize(text));
        const alreadyChosen = selected.some(item => normalize(labelOf(item)) === normalize(text));
        if (!alreadyChosen) add(option ? option.value : text);
        setCustomText('');
    };

    let selectPlaceholder = placeholder;
    if (loading) selectPlaceholder = t('form.loading');
    else if (!available.length) selectPlaceholder = t('tools-config:ui.noOptions');

    return (
        <div className="pml-multiselect">
            {/* The select always shows the placeholder: each choice becomes a chip */}
            <SearchableSelect
                value=""
                options={available}
                disabled={loading || !available.length}
                onChange={add}
                placeholder={selectPlaceholder}
                ariaLabel={ariaLabel}
                noResultsText={t('common.noResults')}
                clearLabel={t('common.clear')}
            />

            {allowCustom && (
                <div className="pml-custom">
                    <input
                        type="text"
                        value={customText}
                        placeholder={customPlaceholder}
                        onChange={(e) => setCustomText(e.target.value)}
                        onKeyDown={(e) => {
                            // Enter adds the entry instead of submitting the tool form
                            if (e.key === 'Enter') { e.preventDefault(); addCustom(); }
                        }}
                        aria-label={customPlaceholder}
                    />
                    <button
                        type="button"
                        className="pml-custom-add"
                        onClick={addCustom}
                        disabled={!customText.trim()}
                        aria-label={t('tools-config:ui.add')}
                        title={t('tools-config:ui.add')}
                    >
                        +
                    </button>
                </div>
            )}

            {chips.length > 0 && (
                <ul className="pml-chips">
                    {chips.map(item => (
                        <li key={item} className="pml-chip">
                            <span className="pml-chip-label">{labelOf(item)}</span>
                            <button
                                type="button"
                                className="pml-chip-remove"
                                onClick={() => remove(item)}
                                aria-label={t('tools-config:ui.remove', { label: labelOf(item) })}
                            >
                                ×
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default MultiSelectChips;
