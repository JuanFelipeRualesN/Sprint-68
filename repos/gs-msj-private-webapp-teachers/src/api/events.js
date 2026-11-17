import axios from "./interceptors";
import Settings from "../custom-classes/settings";

/**
 * Get a paginated, filtered list of events.
 * @param {number} fromPage Current page (0-based).
 * @param {number} limit Items per page.
 * @param {string|null} title Title filter.
 * @param {string|null} available Availability filter ("1" or "0").
 * @param {string|null} status Computed status filter ("upcoming", "ongoing", "finished", "cancelled").
 * @param {string|null} role Audience RoleId filter (Settings.ROLE_DIRECTOR_ID/ROLE_TEACHER_ID/ROLE_STUDENT_ID).
 * @param {boolean} sortDescending Sort by StartDate descending (farthest first) instead of the default ascending.
 * @returns {Promise}
 */
export const getEvents = (fromPage = 0, limit = 10, title = null, available = null, status = null, role = null, sortDescending = false) => {
    return axios.get(Settings.getEventsAPI(fromPage, limit, title, available, status, role, sortDescending));
};

/**
 * Get a single event by ID.
 * @param {number} eventId The event ID.
 * @returns {Promise}
 */
export const getEvent = (eventId) => {
    return axios.get(Settings.getEventAPI(eventId));
};

/**
 * Create a new event.
 * @param {object} request EventModifierRequestModel payload.
 * @returns {Promise}
 */
export const createEvent = (request) => {
    return axios({
        method: "POST",
        url: Settings.createEventAPI(),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Update an existing event.
 * @param {number} eventId The event ID.
 * @param {object} request EventModifierRequestModel payload.
 * @returns {Promise}
 */
export const modifyEvent = (eventId, request) => {
    return axios({
        method: "PUT",
        url: Settings.modifyEventAPI(eventId),
        data: JSON.stringify(request),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Delete an event.
 * @param {number} eventId The event ID.
 * @returns {Promise}
 */
export const deleteEvent = (eventId) => {
    return axios({
        method: "DELETE",
        url: Settings.deleteEventAPI(eventId),
        headers: { "Content-Type": "application/json" },
    });
};
