// Maps EventModel.Status (backend, TeacherAPI EventStatuses) to a Bootstrap badge class. The
// display label lives in events.json (events-common.status-<status>), resolved with useTranslation
// where the badge is rendered — this file only knows the status codes and their styling.
export const EVENT_STATUS_BADGES = {
    upcoming:  { cls: 'bg-info' },
    ongoing:   { cls: 'bg-success' },
    finished:  { cls: 'bg-secondary' },
    cancelled: { cls: 'bg-danger' },
};

// Maps EventModel.TeamsSyncStatus (backend, TeacherAPI EventTeamsSyncStatuses) to a Bootstrap
// badge class. Same pattern as EVENT_STATUS_BADGES above — reused for the Teams column. Labels
// live in events.json (events-common.teams-status-<status>).
export const EVENT_TEAMS_SYNC_STATUS_BADGES = {
    synced:  { cls: 'bg-success' },
    pending: { cls: 'bg-warning' },
    error:   { cls: 'bg-danger' },
};

// EventModel.AudienceSummary is a comma-separated list of language-neutral codes from
// EventAudienceSummaryCalculator (backend), not display text — translate each code and join
// them. Shared by events-data-table and modal-event-detail, so the mapping only lives once.
const AUDIENCE_SUMMARY_CODE_KEYS = {
    director: 'events-table.audience-director',
    teacher: 'events-table.audience-teacher',
    student: 'events-table.audience-student',
    other: 'events-table.audience-other-roles',
    schools: 'events-table.audience-schools',
    regions: 'events-table.audience-regions',
    'specific-users': 'events-table.audience-specific-users',
    none: 'events-table.audience-none',
};

export const formatAudienceSummary = (t, audienceSummary) => (audienceSummary || '')
    .split(',')
    .filter(Boolean)
    .map(code => AUDIENCE_SUMMARY_CODE_KEYS[code] ? t(AUDIENCE_SUMMARY_CODE_KEYS[code]) : code)
    .join(' · ');
