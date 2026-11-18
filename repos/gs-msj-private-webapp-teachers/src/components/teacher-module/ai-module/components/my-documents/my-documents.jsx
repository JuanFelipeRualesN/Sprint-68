import React from 'react';
import { useTranslation } from 'react-i18next';
import Ic from '../../../../../assets/svg/icons';
import MarkdownMath from '../tools/tool-modal/utils/markdown-math.jsx';
import { SlideWorkspace } from '../tools/tool-modal/slide-workspace';
import { deleteDocument, listDocuments, fetchDocuments, updateDocument } from '../../../../../api/ai-module';
import { exportMarkdownPDF, exportWorksheet } from '../tools/tool-modal/utils/exporters-pdf.jsx';
import { exportPPTX } from '../tools/tool-modal/utils/exporters-pptx.jsx';
import { exportWord } from '../tools/tool-modal/utils/exporters-word.jsx';
import {ExportButton} from "../tools/tool-modal/export-button";
import {toDisplayMarkdown} from "../tools/tool-modal/tool-modal-utils";
import { TOOLS } from "../tools/tool-config/tools-config";

function formatDate(value) {
    if (!value) return '';
    return new Intl.DateTimeFormat('es-PR', {
        month: 'short',
        day: '2-digit',
        year: 'numeric',
        hour: 'numeric',
        minute: '2-digit',
    }).format(new Date(value));
}

// Classification helper for smart badges
// Returns an internal English slug — display label resolved via t() at render time
function classifyDoc(doc) {
    const tool = String(doc.toolTitle || '').toLowerCase();
    const cat = String(doc.category || '').toLowerCase();

    if (
        tool.includes('lección') ||
        tool.includes('leccion') ||
        tool.includes('lesson') ||
        tool.includes('plan') ||
        tool.includes('clase') ||
        tool.includes('class') ||
        cat === 'planificacion' ||
        cat === 'planificación'
    ) {
        return 'lesson';
    }
    if (
        tool.includes('rúbrica') ||
        tool.includes('rubric') ||
        tool.includes('rubrica') ||
        tool.includes('examen') ||
        tool.includes('exam') ||
        tool.includes('test') ||
        tool.includes('prueba') ||
        tool.includes('pregunta') ||
        tool.includes('practice') ||
        tool.includes('question') ||
        tool.includes('dok') ||
        cat === 'evaluacion' ||
        cat === 'evaluación'
    ) {
        return 'assessment';
    }
    return 'other';
}

// Category filter tabs configuration — labels resolved via t() at render time
const CATEGORY_TABS = [
    { key: 'all',        icon: '🗂️' },
    { key: 'lesson',     icon: '📝' },
    { key: 'assessment', icon: '📊' },
    { key: 'other',      icon: '📁' },
];

function toolDisplayName(toolTitle, t) {
    const tool = Object.values(TOOLS).find((item) => item.title === toolTitle);
    return tool ? t(`ai-tools:tools.${tool.key}.name`, { defaultValue: toolTitle }) : toolTitle;
}

export default function MyDocuments({ onNavigate }) {
    const { t } = useTranslation(['my-documents', 'common', 'ai-tools']);

    const [docs, setDocs] = React.useState(() => listDocuments());
    const [query, setQuery] = React.useState('');
    const [activeId, setActiveId] = React.useState(null);
    const [copied, setCopied] = React.useState(false);
    const [exporting, setExporting] = React.useState('');
    const [activeTab, setActiveTab] = React.useState('preview'); // 'preview' | 'edit' | 'presentation'
    const [selectedCat, setSelectedCat] = React.useState('all');

    React.useEffect(() => {
        fetchDocuments().then(res => setDocs(res.data.documents ?? []));
        const refresh = () => setDocs(listDocuments());
        window.addEventListener('genial-documents-changed', refresh);
        window.addEventListener('storage', refresh);
        return () => {
            window.removeEventListener('genial-documents-changed', refresh);
            window.removeEventListener('storage', refresh);
        };
    }, []);

    const filtered = React.useMemo(() => {
        let list = docs;

        // Category filter
        if (selectedCat !== 'all') {
            list = list.filter((doc) => classifyDoc(doc) === selectedCat);
        }

        // Search text filter
        const q = query.trim().toLowerCase();
        if (!q) return list;
        return list.filter((doc) =>
            [doc.title, doc.toolTitle, doc.category, doc.content]
                .filter(Boolean)
                .some((part) => String(part).toLowerCase().includes(q))
        );
    }, [docs, selectedCat, query]);

    const active = docs.find((doc) => doc.id === activeId) || filtered[0] || null;

    // Auto-switch tabs to preview if active document has no slides
    const hasSlides = active ? /^\s*##?\s+/m.test(active.content) : false;
    React.useEffect(() => {
        if (activeTab === 'presentation' && !hasSlides) {
            setActiveTab('preview');
        }
    }, [activeId, hasSlides, activeTab]);

    function renameActive(title) {
        if (!active) return;
        const updated = updateDocument(active.id, { title });
        if (updated) setDocs(listDocuments());
    }

    function renameActiveContent(content) {
        if (!active) return;
        const updated = updateDocument(active.id, { content });
        if (updated) setDocs(listDocuments());
    }

    function removeActive() {
        if (!active) return;
        if (window.confirm(t('my-documents:confirmDelete'))) {
            deleteDocument(active.id);
            setActiveId(null);
            setDocs(listDocuments());
        }
    }

    async function copyActive() {
        if (!active) return;
        await navigator.clipboard.writeText(active.content || '');
        setCopied(true);
        setTimeout(() => setCopied(false), 1200);
    }

    async function handleExportPDF() {
        if (!active) return;
        setExporting('pdf');
        try {
            const safeTitle = (active.title || t('my-documents:defaultDocTitle')).replace(/\s+/g, '_');
            await exportMarkdownPDF(active.content, `${safeTitle}.pdf`, toolDisplayName(active.toolTitle, t), t);
        } finally {
            setExporting('');
        }
    }

    const handleExportWord = async () => {
        if (!active) return;
        setExporting('word');
        try {
            const safeTitle = (active.title || t('my-documents:defaultDocTitle')).replace(/\s+/g, '_');
            await exportWord(active.content, `${safeTitle}.docx`, toolDisplayName(active.toolTitle, t), t);
        } finally {
            setExporting('');
        }
    }

    async function handleExportPPTX() {
        if (!active) return;
        setExporting('pptx');
        try {
            const safeTitle = (active.title || t('my-documents:defaultDocTitle')).replace(/\s+/g, '_');
            await exportPPTX(active.content, `${safeTitle}.pptx`, toolDisplayName(active.toolTitle, t), t);
        } finally {
            setExporting('');
        }
    }

    async function handleExportWorksheet() {
        if (!active) return;
        setExporting('worksheet');
        try {
            const safeTitle = (active.title || t('my-documents:defaultDocTitle')).replace(/\s+/g, '_');
            await exportWorksheet(active.content, `${safeTitle}_worksheet.pdf`, toolDisplayName(active.toolTitle, t), t);
        } finally {
            setExporting('');
        }
    }

    return (
        <div className="docs-page">
            <header className="docs-head">
                <div>
                    <div className="eyebrow">
                        <Ic.doc /> {t('my-documents:header.eyebrow')}
                    </div>
                    <h1 className="title-principal">{t('my-documents:header.title')}</h1>
                    <p>{t('my-documents:header.subtitle')}</p>
                </div>
            </header>

            <section className="docs-shell">
                <aside className="docs-list" aria-label={t('my-documents:sidebar.ariaLabel')}>
                    <div className="docs-list-head">
                        <strong>{filtered.length}</strong>
                        <span>
              {filtered.length === 1
                  ? t('my-documents:sidebar.documentSingular')
                  : t('my-documents:sidebar.documentPlural')}
            </span>
                    </div>

                    <div className="docs-categories">
                        {CATEGORY_TABS.map((tab) => (
                            <button
                                key={tab.key}
                                type="button"
                                className={`docs-cat-btn ${selectedCat === tab.key ? 'active' : ''}`}
                                onClick={() => setSelectedCat(tab.key)}
                            >
                                {tab.icon} {t(`my-documents:categories.${tab.key}`)}
                            </button>
                        ))}
                    </div>

                    <div className="docs-search-bar">
                        <div className="docs-search">
                            <Ic.search />
                            <input
                                value={query}
                                onChange={(e) => setQuery(e.target.value)}
                                placeholder={t('my-documents:sidebar.searchPlaceholder')}
                            />
                        </div>
                    </div>

                    <div className="docs-items-scroll">
                        {filtered.map((doc) => {
                            const classification = classifyDoc(doc);
                            return (
                                <button
                                    type="button"
                                    key={doc.id}
                                    className={`docs-item ${active?.id === doc.id ? 'active' : ''}`}
                                    onClick={() => {
                                        setActiveId(doc.id);
                                        setActiveTab('preview');
                                    }}
                                >
                                    <div className="docs-item-header">
                                        <span className="docs-item-title">{doc.title}</span>
                                        <span className={`docs-badge badge-${classification}`}>
                      {t(`my-documents:badges.${classification}`)}
                    </span>
                                    </div>
                                    <div className="docs-item-meta">{formatDate(doc.updatedAt)}</div>
                                    <span className="docs-item-preview">{(doc.content || '').slice(0, 120)}</span>
                                </button>
                            );
                        })}
                        {!filtered.length && (
                            <div className="docs-empty-mini">{t('my-documents:sidebar.emptyCategory')}</div>
                        )}
                    </div>
                </aside>

                <main className="docs-reader">
                    {active ? (
                        <>
                            <div className="docs-reader-header-area">
                                <div className="docs-reader-head">
                                    <div style={{ flex: 1 }}>
                                        <input
                                            className="docs-title-input"
                                            value={active.title || ''}
                                            onChange={(e) => renameActive(e.target.value)}
                                            placeholder={t('my-documents:reader.titlePlaceholder')}
                                            title={t('my-documents:reader.titleTooltip')}
                                        />
                                        <div className="docs-reader-meta">
                                            {t('my-documents:reader.toolLabel')}: <b>{toolDisplayName(active.toolTitle, t)}</b>{' '}
                                            · {t('my-documents:reader.createdLabel')}: {formatDate(active.createdAt)}
                                            {active.model
                                                ? ` · ${t('my-documents:reader.modelLabel')}: ${active.model}`
                                                : ''}
                                        </div>
                                    </div>
                                    <div className="docs-mode-tabs" role="tablist">
                                        <button
                                            type="button"
                                            className={`docs-mode-tab ${activeTab === 'preview' ? 'active' : ''}`}
                                            onClick={() => setActiveTab('preview')}
                                        >
                                            👁️ {t('my-documents:tabs.preview')}
                                        </button>
                                        <button
                                            type="button"
                                            className={`docs-mode-tab ${activeTab === 'edit' ? 'active' : ''}`}
                                            onClick={() => setActiveTab('edit')}
                                        >
                                            ✏️ {t('my-documents:tabs.edit')}
                                        </button>
                                        {hasSlides && (
                                            <button
                                                type="button"
                                                className={`docs-mode-tab ${activeTab === 'presentation' ? 'active' : ''}`}
                                                onClick={() => setActiveTab('presentation')}
                                            >
                                                📊 {t('my-documents:tabs.presentation')}
                                            </button>
                                        )}
                                    </div>
                                </div>

                                <div className="docs-action-bar">
                                    <div className="docs-download-buttons">
                                        <ExportButton
                                            className="docs-btn-action btn-pdf"
                                            onClick={handleExportPDF}
                                            disabled={!!exporting}
                                            loading={exporting === 'pdf'}
                                            title={t('my-documents:actions.downloadPdfTooltip')}
                                            icon="📄"
                                            label={t('my-documents:actions.downloadPdf')}
                                        />
                                        <ExportButton
                                            className="docs-btn-action btn-word"
                                            onClick={handleExportWord}
                                            disabled={!!exporting}
                                            loading={exporting === 'word'}
                                            title={t('my-documents:actions.downloadWordTooltip')}
                                            icon="📃"
                                            label={t('my-documents:actions.downloadWord')}
                                        />
                                        <ExportButton
                                            className="docs-btn-action btn-pptx"
                                            onClick={handleExportPPTX}
                                            disabled={!!exporting}
                                            loading={exporting === 'pptx'}
                                            title={t('my-documents:actions.downloadPptxTooltip')}
                                            icon="📊"
                                            label={t('my-documents:actions.downloadPptx')}
                                        />
                                        <ExportButton
                                            className="docs-btn-action btn-worksheet"
                                            onClick={handleExportWorksheet}
                                            disabled={!!exporting}
                                            loading={exporting === 'worksheet'}
                                            title={t('my-documents:actions.downloadWorksheetTooltip')}
                                            icon="📝"
                                            label={t('my-documents:actions.downloadWorksheet')}
                                        />
                                    </div>
                                    <div style={{ display: 'flex', gap: 6 }}>
                                        <button
                                            type="button"
                                            className="docs-btn-action"
                                            onClick={copyActive}
                                            title={t('my-documents:actions.copyTooltip')}
                                        >
                                            {copied
                                                ? `✅ ${t('my-documents:actions.copied')}`
                                                : `📋 ${t('my-documents:actions.copy')}`}
                                        </button>
                                        <button
                                            type="button"
                                            className="docs-btn-action danger"
                                            onClick={removeActive}
                                            title={t('my-documents:actions.deleteTooltip')}
                                        >
                                            🗑️ {t('my-documents:actions.delete')}
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <div
                                className={`docs-content-container ${
                                    activeTab === 'presentation' ? 'mode-presentation' : ''
                                }`}
                            >
                                {activeTab === 'preview' ? (
                                    <div className="docs-preview-sheet">
                                        <MarkdownMath>{toDisplayMarkdown(active.content)}</MarkdownMath>
                                    </div>
                                ) : activeTab === 'edit' ? (
                                    <textarea
                                        className="docs-editor-textarea"
                                        value={active.content}
                                        onChange={(e) => renameActiveContent(e.target.value)}
                                        placeholder={t('my-documents:reader.editorPlaceholder')}
                                    />
                                ) : (
                                    <div className="docs-slide-wrapper">
                                        <SlideWorkspace value={active.content} onChange={renameActiveContent} />
                                    </div>
                                )}
                            </div>
                        </>
                    ) : (
                        <div className="docs-empty-state">
                            <svg
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                strokeWidth="1.5"
                                strokeLinecap="round"
                                strokeLinejoin="round"
                            >
                                <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
                                <polyline points="14 2 14 8 20 8" />
                                <line x1="16" y1="13" x2="8" y2="13" />
                                <line x1="16" y1="17" x2="8" y2="17" />
                                <polyline points="10 9 9 9 8 9" />
                            </svg>
                            <h2>{t('my-documents:emptyState.title')}</h2>
                            <p>{t('my-documents:emptyState.subtitle')}</p>
                            {onNavigate && (
                                <button
                                    type="button"
                                    className="docs-cta-btn"
                                    onClick={() => onNavigate('/ai-tools')}
                                >
                                    {t('my-documents:emptyState.cta')}
                                </button>
                            )}
                        </div>
                    )}
                </main>
            </section>
        </div>
    );
}
