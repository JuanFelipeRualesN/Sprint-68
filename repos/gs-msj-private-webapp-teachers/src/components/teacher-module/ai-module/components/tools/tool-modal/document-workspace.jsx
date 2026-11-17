/**
 * DocumentWorkspace.jsx / WorksheetWorkspace.jsx
 * Simple view/edit workspaces for markdown output documents.
 */
import React from 'react';
import { useTranslation } from 'react-i18next';
import { mdToHtml, stripEditorMetadata } from './tool-modal-utils.jsx';

// ---------------------------------------------------------------------------
// DocumentWorkspace
// ---------------------------------------------------------------------------

export function DocumentWorkspace({ value, onChange }) {
    const { t } = useTranslation('tool-modal');
    const [docMode, setDocMode] = React.useState('preview'); // 'edit' | 'preview'
    const visibleValue = stripEditorMetadata(value);

    return (
        <div className="tm-doc-workspace">
            <div className="tm-doc-toolbar">
                <div className="tm-doc-mode-selector">
                    <button
                        type="button"
                        className={docMode === 'edit' ? 'active' : ''}
                        onClick={() => setDocMode('edit')}
                    >
                        ✏️ {t('doc.editMode')}
                    </button>
                    <button
                        type="button"
                        className={docMode === 'preview' ? 'active' : ''}
                        onClick={() => setDocMode('preview')}
                    >
                        👁️ {t('doc.previewMode')}
                    </button>
                </div>
            </div>
            <div className={`tm-doc-body mode-${docMode}`}>
                {docMode === 'edit' && (
                    <textarea
                        className="tm-doc-textarea"
                        value={visibleValue}
                        onChange={(e) => onChange(e.target.value)}
                        spellCheck
                        placeholder={t('doc.editPlaceholder')}
                    />
                )}
                {docMode === 'preview' && (
                    <div
                        className="tm-doc-preview tm-md"
                        dangerouslySetInnerHTML={{ __html: mdToHtml(visibleValue) }}
                    />
                )}
            </div>
        </div>
    );
}

// ---------------------------------------------------------------------------
// WorksheetWorkspace
// ---------------------------------------------------------------------------

export function WorksheetWorkspace({ value, onChange, title }) {
    const { t } = useTranslation('tool-modal');
    const visibleValue = stripEditorMetadata(value);

    return (
        <div className="tm-worksheet-workspace">
            <div className="tm-paper-preview">
                <div className="tm-paper">
                    <header>
                        <strong>{title}</strong>
                        <span>{t('worksheet.nameDate')}</span>
                    </header>
                    <div
                        className="tm-paper-body"
                        dangerouslySetInnerHTML={{ __html: mdToHtml(visibleValue) }}
                    />
                </div>
            </div>
            <label className="tm-work-editor">
                <span>{t('worksheet.editableContent')}</span>
                <textarea
                    value={visibleValue}
                    onChange={(e) => onChange(e.target.value)}
                />
            </label>
        </div>
    );
}
