// Export button that swaps to a centered spinner while loading,
// keeping the exact same width/height (label stays in DOM, just hidden).
import {Spinner} from "./spinner";

export function ExportButton({ className, onClick, disabled, title, loading, icon, label }) {
    return (
        <button
            type="button"
            className={className}
            onClick={onClick}
            disabled={disabled}
            title={title}
            style={{ position: 'relative' }}
        >
            <span
                style={{
                    display: 'inline-flex',
                    alignItems: 'center',
                    gap: 4,
                    visibility: loading ? 'hidden' : 'visible',
                }}
            >
                {icon} {label}
            </span>
            {loading && (
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
    );
}