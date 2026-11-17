import React from "react";

const PATHS = {
    home:        '<path d="M4 11l8-6 8 6"/><path d="M6 10v9h5v-5h2v5h5v-9"/>',
    students:    '<circle cx="12" cy="8" r="3.4"/><path d="M5.5 20a6.5 6.5 0 0 1 13 0"/>',
    groups:      '<circle cx="9" cy="9" r="3"/><path d="M3.5 19a5.5 5.5 0 0 1 11 0"/><path d="M16 6.2a3 3 0 0 1 0 5.6"/><path d="M17.5 13a5 5 0 0 1 3.5 6"/>',
    planning:    '<rect x="4" y="5" width="16" height="15" rx="2"/><path d="M4 9.5h16"/><path d="M8 3v4M16 3v4"/><path d="M8.5 14l2 2 3.5-3.5"/>',
    progress:    '<path d="M5 5v14h14"/><path d="M8.5 15l3-3.5 2.5 2 3.5-5"/>',
    pending:     '<rect x="6" y="4.5" width="12" height="15.5" rx="2"/><path d="M9.5 4.5h5v2.5h-5z"/><path d="M9 12.5h6M9 16h4"/>',
    invitations: '<circle cx="9.5" cy="8.5" r="3.2"/><path d="M4 20a5.5 5.5 0 0 1 11 0"/><path d="M18.5 8.5v5M16 11h5"/>',
    chat:        '<path d="M20 12a7 7 0 0 1-9.5 6.5L4 20l1.5-5.5A7 7 0 1 1 20 12z"/>',
    messages:    '<rect x="4" y="6" width="16" height="12" rx="2"/><path d="M4.5 7.5l7.5 5 7.5-5"/>',
    teams:       '<rect x="3" y="6.5" width="12" height="11" rx="2"/><path d="M15 10l6-3v9l-6-3z"/>',
    catalog:     '<path d="M5.5 4.5h11a1 1 0 0 1 1 1v13H7a1.5 1.5 0 0 0-1.5 1.5z"/><path d="M5.5 19.5a1.5 1.5 0 0 1 1.5-1.5h10.5"/>',
    aitools:     '<path d="M12 3l1.8 4.6L18 9.4l-4.2 1.8L12 16l-1.8-4.8L6 9.4l4.2-1.8z"/><path d="M18.5 15l.9 2.3 2.1.9-2.1.9-.9 2.3-.9-2.3-2.1-.9 2.1-.9z"/>',
    documents:   '<path d="M7 3.5h7l4 4v13H7z"/><path d="M14 3.5v4h4"/><path d="M9.5 12h6M9.5 15.5h4.5"/>',
    depr:        '<path d="M4 19V6.5A2.5 2.5 0 0 1 6.5 4H20v13H6.5A2.5 2.5 0 0 0 4 19z"/><path d="M4 19a2.5 2.5 0 0 0 2.5 2.5H20"/><path d="M9 8.5h7M9 12h5"/>',
    help:        '<circle cx="12" cy="12" r="8.5"/><path d="M9.7 9.4a2.4 2.4 0 0 1 4.3.9c0 1.6-2 1.9-2 3.4"/><circle cx="12" cy="17" r=".7" fill="currentColor" stroke="none"/>',
    deliver:     '<path d="M12 3l8 4.3v8.4L12 20l-8-4.3V7.3z"/><path d="M4.2 7.3L12 11.6l7.8-4.3M12 11.6V20"/>',
    bell:        '<path d="M6 16.5V11a6 6 0 0 1 12 0v5.5l1.6 1.8H4.4z"/><path d="M10 19.5a2 2 0 0 0 4 0"/>',
    chevron:     '<path d="M6 9l6 6 6-6"/>',
    chevronL:    '<path d="M15 6l-6 6 6 6"/>',
    chevronR:    '<path d="M9 6l6 6-6 6"/>',
    burger:      '<path d="M4 7h16M4 12h16M4 17h16"/>',
    profile:     '<circle cx="12" cy="8" r="3.6"/><path d="M5 20a7 7 0 0 1 14 0"/>',
    settings:    '<circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z"/>',
    logout:      '<path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><path d="M16 17l5-5-5-5"/><path d="M21 12H9"/>',
    bell2:       '<path d="M18 8a6 6 0 0 0-12 0c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.7 21a2 2 0 0 1-3.4 0"/>',
    shield:      '<path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>',
    calendar: 'M8 2v4M16 2v4M3 10h18M5 4h14a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2z',
    check: 'M9 11l3 3 8-8M21 12v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11',
    users: 'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75',
    group: 'M17 21v-2a4 4 0 0 0-3-3.87M13 3.13a4 4 0 0 1 0 7.75M21 21v-2a4 4 0 0 0-3-3.87M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM1 21v-2a4 4 0 0 1 4-4h4a4 4 0 0 1 4 4v2',
    mail: 'M4 4h16a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2zM22 7l-10 6L2 7',

};

export function LineIcon({ name, size = 20 }) {
    return (
        <span
            style={{ display: "inline-flex", width: size, height: size, flexShrink: 0 }}
            aria-hidden="true"
            dangerouslySetInnerHTML={{
                __html:
                    `<svg viewBox="0 0 24 24" width="100%" height="100%" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">${PATHS[name] || ""}</svg>`,
            }}
        />
    );
}

const ICON_PATHS = {
    calendar: 'M8 2v4M16 2v4M3 10h18M5 4h14a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2z',
    check: 'M9 11l3 3 8-8M21 12v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11',
    users: 'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75',
    group: 'M17 21v-2a4 4 0 0 0-3-3.87M13 3.13a4 4 0 0 1 0 7.75M21 21v-2a4 4 0 0 0-3-3.87M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM1 21v-2a4 4 0 0 1 4-4h4a4 4 0 0 1 4 4v2',
    mail: 'M4 4h16a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2zM22 7l-10 6L2 7',
};

export function LineIconAlt({ name, size = 19 }) {
    return (
        <svg
            width={size}
            height={size}
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="1.8"
            strokeLinecap="round"
            strokeLinejoin="round"
            aria-hidden="true"
            focusable="false"
        >
            <path d={ICON_PATHS[name] || ICON_PATHS.check} />
        </svg>
    );
}