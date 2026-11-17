/**
 * FileUploadButton.jsx
 * Small upload widget that POSTs a file to /api/extract-text and passes the
 * extracted plain text back to the parent via onText().
 */
import React from 'react';
import { useTranslation } from 'react-i18next';
import {extractTextFromFile} from "../../../../../../api/ai-module";

export default function FileUploadButton({ onText }) {
    const { t } = useTranslation('tool-modal');
    const [busy, setBusy]   = React.useState(false);
    const [error, setError] = React.useState('');
    const inputRef = React.useRef(null);

    async function handleFile(e) {
        const file = e.target.files?.[0];
        if (!file) return;
        setBusy(true);
        setError('');
        try {
            const text = await extractTextFromFile(file);
            onText(text);
        } catch (err) {
            setError(err.message || String(err));
        } finally {
            setBusy(false);
            if (inputRef.current) inputRef.current.value = '';
        }
    }

    return (
        <span className="tm-upload">
            <button
                type="button"
                className="tm-upload-btn"
                onClick={() => inputRef.current?.click()}
                disabled={busy}
                title={t('upload.tooltip')}
            >
                {busy ? t('upload.busy') : t('upload.label')}
            </button>
            <input
                ref={inputRef}
                type="file"
                accept=".pdf,.docx,.txt,application/pdf,application/vnd.openxmlformats-officedocument.wordprocessingml.document,text/plain"
                style={{ display: 'none' }}
                onChange={handleFile}
            />
            {error && <span className="tm-upload-err">{error}</span>}
        </span>
    );
}
