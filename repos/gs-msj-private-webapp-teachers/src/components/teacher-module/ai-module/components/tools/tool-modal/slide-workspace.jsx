/**
 * SlideWorkspace.jsx
 * Full slide editor: thumbnail rail, canvas with drag handles, inspector
 * panel, pedagogical intent fields, teacher notes, and image picker overlay.
 */
import React from 'react';
import { useTranslation } from 'react-i18next';
import PresentationMode from './presentation-mode.jsx';
import {
    SLIDE_CANVAS, FONT_OPTIONS,
    parseEditableSlides, slidesToMarkdown,
    normalizeSlideLayout, normalizeSlideStyle, normalizeSlideType,
    defaultSlideLayout, defaultSlideStyle, normalizeBox,
} from './tool-modal-slide-utils.jsx';
import {
    mdToHtml, markdownToHtmlForEditor, htmlToMarkdown,
} from './tool-modal-utils.jsx';
import Settings, {IMAGE_NEGATIVE_PROMPT} from "../../../../../../custom-classes/settings";
import { getAuthHeaders } from "../../../../../../api/interceptors.js";

// ---------------------------------------------------------------------------
// SlideBodyEditor — rich contenteditable body input
// ---------------------------------------------------------------------------

function SlideBodyEditor({ html, onChange, align, slideIndex, style }) {
    const { t } = useTranslation('tool-modal');
    const editorRef = React.useRef(null);

    React.useEffect(() => {
        if (editorRef.current) editorRef.current.innerHTML = html;
        // Re-sync only when the active slide index changes, not on every keystroke
        
    }, [slideIndex]);

    const handleInput = () => {
        if (editorRef.current) onChange(editorRef.current.innerHTML);
    };

    return (
        <div
            ref={editorRef}
            contentEditable
            className="tm-slide-body-input rich-editor"
            style={{
                textAlign: align,
                outline: 'none',
                fontFamily: `${style.fontFace}, system-ui, sans-serif`,
                fontSize: `${style.bodySize}px`,
            }}
            onInput={handleInput}
            placeholder={t('slide.bodyPlaceholder')}
        />
    );
}

// ---------------------------------------------------------------------------
// MovableSlideBox — draggable text box overlay on the canvas
// ---------------------------------------------------------------------------

function MovableSlideBox({ box, className = '', label, onMove, children }) {
    const safeBox = normalizeBox(box, SLIDE_CANVAS);

    function beginDrag(e) {
        e.preventDefault();
        e.stopPropagation();
        const canvas = e.currentTarget.closest('.tm-slide-canvas');
        if (!canvas) return;
        const rect  = canvas.getBoundingClientRect();
        const scale = rect.width / SLIDE_CANVAS.w;
        const start = { x: e.clientX, y: e.clientY, box: safeBox };

        const onPointerMove = (moveEvent) => {
            onMove(normalizeBox({
                ...start.box,
                x: start.box.x + (moveEvent.clientX - start.x) / scale,
                y: start.box.y + (moveEvent.clientY - start.y) / scale,
            }, start.box));
        };
        const onPointerUp = () => {
            window.removeEventListener('pointermove', onPointerMove);
            window.removeEventListener('pointerup', onPointerUp);
        };
        window.addEventListener('pointermove', onPointerMove);
        window.addEventListener('pointerup', onPointerUp, { once: true });
    }

    return (
        <div
            className={`tm-slide-textbox ${className}`}
            style={{
                left:   `${(safeBox.x / SLIDE_CANVAS.w) * 100}%`,
                top:    `${(safeBox.y / SLIDE_CANVAS.h) * 100}%`,
                width:  `${(safeBox.w / SLIDE_CANVAS.w) * 100}%`,
                height: `${(safeBox.h / SLIDE_CANVAS.h) * 100}%`,
            }}
        >
            <button type="button" className="tm-slide-drag-handle" onPointerDown={beginDrag} title={label}>
                {label}
            </button>
            {children}
        </div>
    );
}

// ---------------------------------------------------------------------------
// SlideWorkspace — main export
// ---------------------------------------------------------------------------

export function SlideWorkspace({ value, onChange }) {
    const { t } = useTranslation('tool-modal');
    const slides  = React.useMemo(() => parseEditableSlides(value), [value]);
    const [active, setActive] = React.useState(0);
    const current = slides[Math.min(active, Math.max(slides.length - 1, 0))] || slides[0];

    const [showImagePicker,  setShowImagePicker]  = React.useState(false);
    const [aiImagePrompt,    setAiImagePrompt]    = React.useState('');
    const [generatingImage,  setGeneratingImage]  = React.useState(false);
    const [customImageUrl,   setCustomImageUrl]   = React.useState('');
    const [imageError,       setImageError]       = React.useState('');
    const [showInspector,    setShowInspector]    = React.useState(false);
    const [showNotes,        setShowNotes]        = React.useState(true);
    const [showIntent,       setShowIntent]       = React.useState(true);
    const [presenting,       setPresenting]       = React.useState(false);

    React.useEffect(() => {
        if (active > slides.length - 1) setActive(Math.max(slides.length - 1, 0));
    }, [active, slides.length]);

    // -----------------------------------------------------------------------
    // Slide mutation helpers
    // -----------------------------------------------------------------------

    function updateSlide(index, patch) {
        onChange(slidesToMarkdown(slides.map((slide, i) => (i === index ? { ...slide, ...patch } : slide))));
    }

    function updateLayoutPart(part, box) {
        const layout = normalizeSlideLayout(current?.layout || defaultSlideLayout());
        updateSlide(active, { layout: { ...layout, [part]: normalizeBox(box, layout[part]) } });
    }

    function updateSlideStyle(patch) {
        const style = normalizeSlideStyle(current?.style || defaultSlideStyle());
        updateSlide(active, { style: normalizeSlideStyle({ ...style, ...patch }) });
    }

    function addSlide() {
        onChange(slidesToMarkdown([...slides, {
            title:        t('slide.newTitle'),
            body:         t('slide.newBody'),
            type:         'concept',
            claim:        t('slide.newClaim'),
            /* visualIntent: t('slide.newVisual') ,*/
            align:        'left',
            layout:       defaultSlideLayout(),
            style:        defaultSlideStyle(),
        }]));
        setActive(slides.length);
    }

    function removeSlide(index) {
        const next = slides.filter((_, i) => i !== index);
        onChange(slidesToMarkdown(next.length ? next : [{
            title:  t('slide.defaultTitle'),
            body:   '',
            type:   'concept',
            align:  'left',
            layout: defaultSlideLayout(),
            style:  defaultSlideStyle(),
        }]));
        setActive(Math.max(0, index - 1));
    }

    function duplicateSlide() {
        if (!current) return;
        const copy = { ...current, title: `${current.title || t('slide.defaultTitle')} ${t('slide.copySuffix')}` };
        const next = [...slides.slice(0, active + 1), copy, ...slides.slice(active + 1)];
        onChange(slidesToMarkdown(next));
        setActive(active + 1);
    }

    // -----------------------------------------------------------------------
    // Image drag
    // -----------------------------------------------------------------------

    function beginImageDrag(e) {
        e.preventDefault();
        e.stopPropagation();
        const canvas = e.currentTarget.closest('.tm-slide-canvas');
        if (!canvas) return;
        const rect  = canvas.getBoundingClientRect();
        const scale = rect.width / SLIDE_CANVAS.w;
        const start = { x: e.clientX, y: e.clientY, box: currentLayout.image };

        const onMove = (event) => {
            updateLayoutPart('image', {
                ...start.box,
                x: start.box.x + (event.clientX - start.x) / scale,
                y: start.box.y + (event.clientY - start.y) / scale,
            });
        };
        const onUp = () => {
            window.removeEventListener('pointermove', onMove);
            window.removeEventListener('pointerup', onUp);
        };
        window.addEventListener('pointermove', onMove);
        window.addEventListener('pointerup', onUp, { once: true });
    }

    // -----------------------------------------------------------------------
    // Image picker handlers
    // -----------------------------------------------------------------------

    function handleLocalImageUpload(e) {
        const file = e.target.files?.[0];
        if (!file) return;
        setImageError('');
        const reader = new FileReader();
        reader.onload = (event) => {
            if (event.target?.result) {
                updateSlide(active, { imageUrl: event.target.result, imageAlt: file.name });
                setShowImagePicker(false);
            }
        };
        reader.onerror = () => setImageError(t('imagePicker.readError'));
        reader.readAsDataURL(file);
    }

    async function handleGenerateAIImage() {
        if (!aiImagePrompt.trim() || !current) return;
        setGeneratingImage(true);
        setImageError('');
        try {
            const enhancedPrompt = `${aiImagePrompt}`;
            const requestBody = {
                prompt: enhancedPrompt,
                negative_prompt: IMAGE_NEGATIVE_PROMPT,
                steps: 20,
                cfg_scale: 7
            };
            const res  = await fetch(Settings.API_AI_URL + '/aiservicesapi/ImageGenerate', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
                body: JSON.stringify(requestBody),
            });
            const data = await res.json();

            const imageUrls = data.ImageUrls || data.imageUrls;

            if (!res.ok || !imageUrls || imageUrls.length === 0) {
                throw new Error(data.error || t('imagePicker.generateError'));
            }
            updateSlide(active, { imageUrl: imageUrls[0], imageAlt: aiImagePrompt.trim() });
            setShowImagePicker(false);
            setAiImagePrompt('');
        } catch (err) {
            setImageError(err.message || t('imagePicker.communicationError'));
        } finally {
            setGeneratingImage(false);
        }
    }

    // -----------------------------------------------------------------------
    // Derived values
    // -----------------------------------------------------------------------

    const hasMedia        = current && (current.imageUrl || current.linkUrl);
    const currentBodyHtml = React.useMemo(
        () => markdownToHtmlForEditor(current?.body || ''),
        
        [current?.body, active]
    );
    const currentLayout = normalizeSlideLayout(current?.layout || defaultSlideLayout());
    const currentStyle  = normalizeSlideStyle(current?.style   || defaultSlideStyle());

    // -----------------------------------------------------------------------
    // Render
    // -----------------------------------------------------------------------

    return (
        <>
            <div className="tm-studio">
                {/* ── Top bar ── */}
                <header className="tm-studio-topbar">
                    <div className="tm-studio-brand">
                        <span className="tm-studio-mark" aria-hidden="true">◆</span>
                        <div className="tm-studio-brand-text">
                            <span className="tm-studio-eyebrow">Studio</span>
                            <span className="tm-studio-counter">
                                <strong>{String(active + 1).padStart(2, '0')}</strong>
                                <span className="slash" aria-hidden="true">/</span>
                                <span className="total">{String(slides.length).padStart(2, '0')}</span>
                                <span className="label">{t('slide.slides')}</span>
                            </span>
                        </div>
                    </div>
                    <div className="tm-studio-top-actions">
                        <button type="button" className="tm-studio-btn" onClick={duplicateSlide} title={t('slide.duplicateTitle')}>
                            <span aria-hidden="true">⎘</span> {t('slide.duplicate')}
                        </button>
                        <button type="button" className="tm-studio-btn danger" onClick={() => removeSlide(active)} title={t('slide.deleteTitle')}>
                            <span aria-hidden="true">✕</span> {t('slide.delete')}
                        </button>
                        <button type="button" className="tm-studio-btn primary" onClick={() => setPresenting(true)} title={t('slide.presentTitle')}>
                            <span className="play" aria-hidden="true">▶</span> {t('slide.present')}
                        </button>
                    </div>
                </header>

                {/* ── Body ── */}
                <div
                    className="tm-studio-body"
                    style={{ gridTemplateColumns: showInspector && hasMedia ? '240px 1fr 280px' : '240px 1fr' }}
                >
                    {/* Slide rail */}
                    <aside className="tm-studio-rail" aria-label={t('slide.railAriaLabel')}>
                        <button type="button" className="tm-studio-rail-add" onClick={addSlide}>
                            <span aria-hidden="true">＋</span> {t('slide.addSlide')}
                        </button>
                        <ol className="tm-studio-rail-list">
                            {slides.map((slide, i) => (
                                <li key={`${i}-${slide.title}`}>
                                    <button
                                        type="button"
                                        className={`tm-studio-rail-item ${i === active ? 'active' : ''}`}
                                        onClick={() => setActive(i)}
                                    >
                                        <span className="num">{String(i + 1).padStart(2, '0')}</span>
                                        <div className="meta">
                                            <strong className="title">{slide.title || t('slide.untitled')}</strong>
                                            <span className="preview">
                                                {(slide.body || '').replace(/[#*`>\-]/g, '').trim().slice(0, 48) || t('slide.noContent')}
                                            </span>
                                        </div>
                                        {slide.imageUrl && (
                                            <span className="badge" title={t('slide.hasImage')} aria-hidden="true">◐</span>
                                        )}
                                    </button>
                                </li>
                            ))}
                        </ol>
                    </aside>

                    {/* Canvas + toolbar */}
                    {current && (
                        <main className="tm-studio-stage">
                            <div className="tm-studio-toolbar" role="toolbar" aria-label={t('slide.toolbarAriaLabel')}>
                                <div className="tg">
                                    <select
                                        className="ti"
                                        value={currentStyle.fontFace}
                                        onChange={(e) => updateSlideStyle({ fontFace: e.target.value })}
                                        title={t('slide.typography')}
                                    >
                                        {FONT_OPTIONS.map(font => <option key={font} value={font}>{font}</option>)}
                                    </select>
                                    <select
                                        className="ti narrow"
                                        value={currentStyle.bodySize}
                                        onChange={(e) => {
                                            const bodySize = Number(e.target.value);
                                            updateSlideStyle({ bodySize, titleSize: Math.min(42, bodySize + 12) });
                                        }}
                                        title={t('slide.bodySize')}
                                    >
                                        {[14, 16, 18, 20, 22, 24, 26].map(size => (
                                            <option key={size} value={size}>{size}</option>
                                        ))}
                                    </select>
                                </div>

                                <div className="tg">
                                    <button type="button" title={t('slide.bold')}   onMouseDown={(e) => e.preventDefault()} onClick={() => document.execCommand('bold',                false)}><strong>B</strong></button>
                                    <button type="button" title={t('slide.italic')} onMouseDown={(e) => e.preventDefault()} onClick={() => document.execCommand('italic',              false)}><em>I</em></button>
                                    <button type="button" title={t('slide.list')}   onMouseDown={(e) => e.preventDefault()} onClick={() => document.execCommand('insertUnorderedList', false)}>• {t('slide.list')}</button>
                                </div>

                                <div className="tg align">
                                    {['left', 'center', 'right'].map(dir => (
                                        <button
                                            key={dir}
                                            type="button"
                                            title={t(`slide.align${dir.charAt(0).toUpperCase() + dir.slice(1)}`)}
                                            className={current.align === dir ? 'active' : ''}
                                            onClick={() => updateSlide(active, { align: dir })}
                                            aria-label={t(`slide.align${dir.charAt(0).toUpperCase() + dir.slice(1)}`)}
                                        >
                                            {dir === 'left' ? '⟵' : dir === 'center' ? '↔' : '⟶'}
                                        </button>
                                    ))}
                                </div>

                                <div className="tg insert">
                                    <button type="button" onClick={() => setShowImagePicker(true)} title={t('slide.addImageTitle')}>
                                        <span aria-hidden="true">＋</span> {t('slide.addImage')}
                                    </button>
                                    <button
                                        type="button"
                                        onClick={() => {
                                            updateSlide(active, {
                                                linkUrl:  current.linkUrl  || 'https://',
                                                linkText: current.linkText || t('slide.defaultLinkText'),
                                            });
                                            setShowInspector(true);
                                        }}
                                        title={t('slide.addLinkTitle')}
                                    >
                                        <span aria-hidden="true">＋</span> {t('slide.addLink')}
                                    </button>
                                    {hasMedia && (
                                        <button
                                            type="button"
                                            className={showInspector ? 'active' : ''}
                                            onClick={() => setShowInspector(!showInspector)}
                                            title={t('slide.propertiesTitle')}
                                        >
                                            ⚙ {t('slide.properties')}
                                        </button>
                                    )}
                                </div>

                                <div className="tg right">
                                    <button type="button" title={t('slide.undo')} onMouseDown={(e) => e.preventDefault()} onClick={() => document.execCommand('undo', false)}>↶</button>
                                    <button type="button" title={t('slide.redo')} onMouseDown={(e) => e.preventDefault()} onClick={() => document.execCommand('redo', false)}>↷</button>
                                </div>
                            </div>

                            <section
                                className="tm-studio-canvas-wrap"
                                aria-label={t('slide.canvasAriaLabel', { number: active + 1 })}
                            >
                                <div className="tm-studio-canvas-frame" key={active}>
                                    <div
                                        className={`tm-slide-canvas ${current.imageUrl ? 'has-image' : ''}`}
                                        style={{ textAlign: current.align || 'left' }}
                                    >
                                        <div className="tm-slide-canvas-content" aria-hidden="true" />

                                        <MovableSlideBox
                                            box={currentLayout.title}
                                            label={t('slide.titleBoxLabel')}
                                            className="title-box"
                                            onMove={(box) => updateLayoutPart('title', box)}
                                        >
                                            <input
                                                className="tm-slide-title-input"
                                                value={current.title}
                                                onChange={(e) => updateSlide(active, { title: e.target.value })}
                                                placeholder={t('slide.titlePlaceholder')}
                                                style={{
                                                    textAlign:  current.align || 'left',
                                                    fontFamily: `${currentStyle.fontFace}, system-ui, sans-serif`,
                                                    fontSize:   `${currentStyle.titleSize}px`,
                                                }}
                                            />
                                        </MovableSlideBox>

                                        <MovableSlideBox
                                            box={currentLayout.body}
                                            label={t('slide.bodyBoxLabel')}
                                            className="body-box"
                                            onMove={(box) => updateLayoutPart('body', box)}
                                        >
                                            <SlideBodyEditor
                                                html={currentBodyHtml}
                                                align={current.align || 'left'}
                                                slideIndex={active}
                                                style={currentStyle}
                                                onChange={(newHtml) => {
                                                    updateSlide(active, { body: htmlToMarkdown(newHtml) });
                                                }}
                                            />
                                        </MovableSlideBox>

                                        {current.imageUrl ? (
                                            <div
                                                className="tm-slide-canvas-image"
                                                style={{
                                                    left:   `${(currentLayout.image.x / SLIDE_CANVAS.w) * 100}%`,
                                                    top:    `${(currentLayout.image.y / SLIDE_CANVAS.h) * 100}%`,
                                                    width:  `${(currentLayout.image.w / SLIDE_CANVAS.w) * 100}%`,
                                                    height: `${(currentLayout.image.h / SLIDE_CANVAS.h) * 100}%`,
                                                }}
                                            >
                                                <button
                                                    type="button"
                                                    className="tm-slide-drag-handle image-handle"
                                                    onPointerDown={beginImageDrag}
                                                    title={t('slide.moveImage')}
                                                >
                                                    {t('slide.imageBoxLabel')}
                                                </button>
                                                <img src={current.imageUrl} alt={current.imageAlt || ''} />
                                                <button
                                                    type="button"
                                                    className="tm-slide-image-remove"
                                                    onClick={() => updateSlide(active, { imageUrl: '', imageAlt: '' })}
                                                    title={t('slide.removeImage')}
                                                >×</button>
                                            </div>
                                        ) : (
                                            <button
                                                type="button"
                                                className="tm-canvas-add-image-placeholder"
                                                onClick={() => setShowImagePicker(true)}
                                                title={t('slide.addImageTitle')}
                                            >
                                                <span>＋ {t('slide.addImageOrAI')}</span>
                                            </button>
                                        )}

                                        {current.linkUrl && (
                                            <a
                                                className="tm-slide-link-chip"
                                                href={current.linkUrl}
                                                target="_blank"
                                                rel="noopener noreferrer"
                                            >
                                                🔗 {current.linkText || current.linkUrl}
                                            </a>
                                        )}
                                    </div>
                                </div>
                            </section>

                            {/* Pedagogical intent + teacher notes */}
                            <div className="tm-studio-sidekick">
                                <details
                                    className="tm-studio-collapsible"
                                    open={showIntent}
                                    onToggle={(e) => setShowIntent(e.currentTarget.open)}
                                >
                                    <summary>
                                        <span className="dot intent" aria-hidden="true" />
                                        {t('slide.pedagogicalIntent')}
                                    </summary>
                                    <div className="tm-studio-collapsible-body grid2">
                                        <label>
                                            <span>{t('slide.centralIdea')}</span>
                                            <input
                                                value={current.claim || ''}
                                                onChange={(e) => updateSlide(active, { claim: e.target.value })}
                                                placeholder={t('slide.centralIdeaPlaceholder')}
                                            />
                                        </label>
                                        {/*} <label>
                                            <span>{t('slide.visual')}</span>
                                            <input
                                                value={current.visualIntent || ''}
                                                onChange={(e) => updateSlide(active, { visualIntent: e.target.value })}
                                                placeholder={t('slide.visualPlaceholder')}
                                            />
                                        </label> */}
                                    </div>
                                </details>

                                <details
                                    className="tm-studio-collapsible"
                                    open={showNotes}
                                    onToggle={(e) => setShowNotes(e.currentTarget.open)}
                                >
                                    <summary>
                                        <span className="dot notes" aria-hidden="true" />
                                        {t('slide.teacherNotes')}
                                        <span className="muted">{t('slide.teacherNotesSubtitle')}</span>
                                    </summary>
                                    <div className="tm-studio-collapsible-body">
                                        <textarea
                                            className="tm-studio-notes-textarea"
                                            value={current.notes || ''}
                                            onChange={(e) => updateSlide(active, { notes: e.target.value })}
                                            placeholder={t('slide.notesPlaceholder')}
                                        />
                                    </div>
                                </details>
                            </div>
                        </main>
                    )}

                    {/* Inspector panel */}
                    {showInspector && hasMedia && (
                        <aside className="tm-studio-inspector" aria-label={t('slide.inspectorAriaLabel')}>
                            <header className="tm-studio-inspector-head">
                                <strong>{t('slide.properties')}</strong>
                                <button
                                    type="button"
                                    className="tm-studio-inspector-close"
                                    onClick={() => setShowInspector(false)}
                                    aria-label={t('common.close')}
                                >✕</button>
                            </header>
                            {current.imageUrl && (
                                <div className="tm-studio-inspector-box">
                                    <span className="title">🖼 {t('slide.imageSection')}</span>
                                    <label>
                                        <small>{t('common.url')}</small>
                                        <input
                                            value={current.imageUrl || ''}
                                            onChange={(e) => updateSlide(active, { imageUrl: e.target.value })}
                                            placeholder="https://..."
                                        />
                                    </label>
                                    <label>
                                        <small>{t('slide.imageAlt')}</small>
                                        <input
                                            value={current.imageAlt || ''}
                                            onChange={(e) => updateSlide(active, { imageAlt: e.target.value })}
                                            placeholder={t('slide.imageAltPlaceholder')}
                                        />
                                    </label>
                                </div>
                            )}
                            {current.linkUrl && (
                                <div className="tm-studio-inspector-box">
                                    <span className="title">🔗 {t('slide.linkSection')}</span>
                                    <label>
                                        <small>{t('common.text')}</small>
                                        <input
                                            value={current.linkText || ''}
                                            onChange={(e) => updateSlide(active, { linkText: e.target.value })}
                                            placeholder={t('slide.linkTextPlaceholder')}
                                        />
                                    </label>
                                    <label>
                                        <small>{t('common.url')}</small>
                                        <input
                                            value={current.linkUrl || ''}
                                            onChange={(e) => updateSlide(active, { linkUrl: e.target.value })}
                                            placeholder="https://..."
                                        />
                                    </label>
                                </div>
                            )}
                        </aside>
                    )}
                </div>

                {/* Image picker overlay */}
                {showImagePicker && (
                    <div
                        className="tm-image-picker-overlay"
                        onClick={(e) => { if (e.target === e.currentTarget) setShowImagePicker(false); }}
                    >
                        <div className="tm-image-picker-card">
                            <header className="tm-image-picker-header">
                                <h3>{t('imagePicker.title')}</h3>
                                <button type="button" className="tm-image-picker-close" onClick={() => setShowImagePicker(false)}>×</button>
                            </header>
                            <div className="tm-image-picker-body">
                                {/* AI generation */}
                                <div className="tm-image-picker-section">
                                    <h4>🎨 {t('imagePicker.aiTitle')}</h4>
                                    <p>{t('imagePicker.aiDescription')}</p>
                                    <div className="tm-ai-prompt-box">
                                        <input
                                            type="text"
                                            placeholder={t('imagePicker.aiPlaceholder')}
                                            value={aiImagePrompt}
                                            onChange={(e) => setAiImagePrompt(e.target.value)}
                                            disabled={generatingImage}
                                        />
                                        <button
                                            type="button"
                                            className="tm-btn primary"
                                            onClick={handleGenerateAIImage}
                                            disabled={generatingImage || !aiImagePrompt.trim()}
                                        >
                                            {generatingImage ? t('imagePicker.generating') : t('imagePicker.generateBtn')}
                                        </button>
                                    </div>
                                    {generatingImage && (
                                        <div className="tm-ai-image-loader">
                                            <span className="tm-spinner" /> {t('imagePicker.generatingHint')}
                                        </div>
                                    )}
                                </div>

                                {/* Local upload
                                <div className="tm-image-picker-section">
                                    <h4>📁 {t('imagePicker.uploadTitle')}</h4>
                                    <p>{t('imagePicker.uploadDescription')}</p>
                                    <input
                                        type="file"
                                        accept="image/*"
                                        onChange={handleLocalImageUpload}
                                        id="canvas-local-img-upload"
                                        style={{ display: 'none' }}
                                    />
                                    <button
                                        type="button"
                                        className="tm-btn secondary"
                                        onClick={() => document.getElementById('canvas-local-img-upload').click()}
                                    >
                                        {t('imagePicker.selectFile')}
                                    </button>
                                </div> */}

                                {/* URL input */}
                                <div className="tm-image-picker-section">
                                    <h4>🔗 {t('imagePicker.urlTitle')}</h4>
                                    <p>{t('imagePicker.urlDescription')}</p>
                                    <div className="tm-url-input-box">
                                        <input
                                            type="text"
                                            placeholder="https://example.com/my-image.jpg"
                                            value={customImageUrl}
                                            onChange={(e) => setCustomImageUrl(e.target.value)}
                                        />
                                        <button
                                            type="button"
                                            className="tm-btn secondary"
                                            onClick={() => {
                                                if (customImageUrl.trim()) {
                                                    updateSlide(active, { imageUrl: customImageUrl.trim(), imageAlt: current.title });
                                                    setShowImagePicker(false);
                                                    setCustomImageUrl('');
                                                }
                                            }}
                                            disabled={!customImageUrl.trim()}
                                        >
                                            {t('imagePicker.addUrl')}
                                        </button>
                                    </div>
                                </div>

                                {imageError && <div className="tm-image-picker-error">⚠️ {imageError}</div>}
                            </div>
                        </div>
                    </div>
                )}
            </div>

            {presenting && (
                <PresentationMode slides={slides} startAt={active} onExit={() => setPresenting(false)} />
            )}
        </>
    );
}
