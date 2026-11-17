// Self-contained SVG spinner (SMIL animation — no external CSS needed)
export function Spinner() {
    return (
        <svg width="16" height="16" viewBox="0 0 50 50" style={{ display: 'block' }} aria-hidden="true">
            <circle
                cx="25" cy="25" r="20"
                fill="none" stroke="currentColor" strokeWidth="5"
                strokeLinecap="round" strokeDasharray="90 150"
            >
                <animateTransform
                    attributeName="transform" type="rotate"
                    from="0 25 25" to="360 25 25"
                    dur="0.8s" repeatCount="indefinite"
                />
            </circle>
        </svg>
    );
}