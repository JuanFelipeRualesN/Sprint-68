/**
 * PresentationMode.jsx
 * Fullscreen presentation overlay. Arrow-key / PageUp / PageDown navigation,
 * ESC to exit, auto-fits to viewport. Reads styling from each slide's own
 * style / layout objects.
 */
import React from 'react';
import { useTranslation } from 'react-i18next';
import { mdToHtml } from './tool-modal-utils.jsx';
import { normalizeSlideStyle, defaultSlideStyle } from './tool-modal-slide-utils.jsx';
import { createPortal } from 'react-dom';

export default function PresentationMode({ slides, startAt = 0, onExit }) {
    const { t } = useTranslation('tool-modal');
    const [idx, setIdx] = React.useState(
        Math.min(startAt, Math.max(slides.length - 1, 0))
    );
    const total   = slides.length;
    const current = slides[Math.min(idx, total - 1)] || slides[0];

    React.useEffect(() => {
        const onKey = (e) => {
            if (e.key === 'Escape') { onExit(); return; }
            if (['ArrowRight', ' ', 'PageDown', 'Enter'].includes(e.key)) {
                setIdx(i => Math.min(i + 1, total - 1));
            } else if (['ArrowLeft', 'PageUp', 'Backspace'].includes(e.key)) {
                setIdx(i => Math.max(i - 1, 0));
            } else if (e.key === 'Home') { setIdx(0); }
            else if (e.key === 'End')   { setIdx(total - 1); }
        };
        window.addEventListener('keydown', onKey);
        document.documentElement.requestFullscreen?.().catch(() => {});
        document.body.style.overflow = 'hidden';
        return () => {
            window.removeEventListener('keydown', onKey);
            document.body.style.overflow = '';
            if (document.fullscreenElement) document.exitFullscreen?.().catch(() => {});
        };
    }, [total, onExit]);

    const style      = normalizeSlideStyle(current?.style || defaultSlideStyle());
    const bodyHtml   = mdToHtml(current?.body || '');
    const fontFamily = `${style.fontFace}, system-ui, sans-serif`;
    const align      = current?.align || 'left';

    return createPortal(
        <div
            className="tm-present-overlay"
            role="dialog"
            aria-modal="true"
            aria-label={t('presentation.ariaLabel')}
        >
            <div className="tm-present-stage" key={idx}>
                <article className="tm-present-slide" style={{ textAlign: align, fontFamily }}>
                    {current?.title && (
                        <h1
                            className="tm-present-title"
                            style={{ fontSize: `clamp(40px, ${Math.max(style.titleSize * 2.6, 48)}px, 96px)` }}
                        >
                            {current.title}
                        </h1>
                    )}
                    {current?.imageUrl && (
                        <div className="tm-present-image-wrap">
                            <img src={current.imageUrl} alt={current.imageAlt || ''} />
                        </div>
                    )}
                    {bodyHtml && (
                        <div
                            className="tm-present-body tm-md"
                            style={{ fontSize: `clamp(20px, ${Math.max(style.bodySize * 1.9, 24)}px, 36px)` }}
                            dangerouslySetInnerHTML={{ __html: bodyHtml }}
                        />
                    )}
                    {current?.linkUrl && (
                        <a
                            className="tm-present-link"
                            href={current.linkUrl}
                            target="_blank"
                            rel="noopener noreferrer"
                        >
                            🔗 {current.linkText || current.linkUrl}
                        </a>
                    )}
                </article>
            </div>

            <button
                className="tm-present-nav prev"
                onClick={() => setIdx(i => Math.max(i - 1, 0))}
                disabled={idx === 0}
                aria-label={t('presentation.prev')}
            >‹</button>
            <button
                className="tm-present-nav next"
                onClick={() => setIdx(i => Math.min(i + 1, total - 1))}
                disabled={idx === total - 1}
                aria-label={t('presentation.next')}
            >›</button>

            <div className="tm-present-chrome">
                <span className="tm-present-counter">
                    <strong>{String(idx + 1).padStart(2, '0')}</strong>
                    <span>/ {String(total).padStart(2, '0')}</span>
                </span>
                <div className="tm-present-dots" aria-hidden="true">
                    {slides.map((_, i) => (
                        <button
                            key={i}
                            type="button"
                            className={`dot ${i === idx ? 'on' : ''}`}
                            onClick={() => setIdx(i)}
                            aria-label={t('presentation.goToSlide', { number: i + 1 })}
                        />
                    ))}
                </div>
            </div>

            <button
                className="tm-present-exit"
                onClick={onExit}
                title={t('presentation.exitTitle')}
                aria-label={t('presentation.exit')}
            >✕</button>
            <div className="tm-present-hint" aria-hidden="true">
                {t('presentation.hint')}
            </div>
        </div>,
        document.body
    );
}
