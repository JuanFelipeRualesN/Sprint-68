/**
 * DownloadMenu.jsx
 * Collapsed ⬇ dropdown for PDF / Word / PPTX / Worksheet export actions.
 * Avoids cluttering the toolbar with three separate competing buttons.
 */
import React from 'react';
import { useTranslation } from 'react-i18next';
import {Spinner} from "./spinner";


export default function DownloadMenu({ exporting, onPDF, onPPTX, onWorksheet, onWord }) {
    const { t } = useTranslation('tool-modal');
    const [open, setOpen] = React.useState(false);
    const ref = React.useRef(null);

    // Close the dropdown when the user clicks outside it
    React.useEffect(() => {
        if (!open) return undefined;
        const handleOutsideClick = (e) => {
            if (ref.current && !ref.current.contains(e.target)) setOpen(false);
        };
        document.addEventListener('mousedown', handleOutsideClick);
        return () => document.removeEventListener('mousedown', handleOutsideClick);
    }, [open]);

    const runAction = (fn) => { setOpen(false); fn(); };

    return (
        <div className="tm-download-menu" ref={ref}>
            <button
                type="button"
                className="tm-action-btn tm-download-trigger"
                onClick={() => setOpen(prev => !prev)}
                disabled={!!exporting}
                title={t('download.title')}
                style={{ position: 'relative' }}
            >
                {/* Kept in the layout (just hidden) so the button keeps its size */}
                <span style={{ visibility: exporting ? 'hidden' : 'visible' }}>
                    ⬇ {t('download.label')} <span className="tm-caret">▾</span>
                </span>
                {exporting && (
                    <span
                        style={{
                            position: 'absolute',
                            inset: 0,
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                        }}
                    >
                        <Spinner />
                    </span>
                )}
            </button>
            {open && (
                <div className="tm-download-dropdown" role="menu">
                    <button type="button" role="menuitem" onClick={() => runAction(onPDF)}>
                        📄 {t('download.pdf')}
                    </button>
                    <button type="button" role="menuitem" onClick={() => runAction(onWord)}>
                        📝 {t('download.word')}
                    </button>
                    <button type="button" role="menuitem" onClick={() => runAction(onPPTX)}>
                        📊 {t('download.pptx')}
                    </button>
                    <button type="button" role="menuitem" onClick={() => runAction(onWorksheet)}>
                        📝 {t('download.worksheet')}
                    </button>
                </div>
            )}
        </div>
    );
}