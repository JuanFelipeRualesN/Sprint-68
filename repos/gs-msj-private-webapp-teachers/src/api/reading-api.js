import axios from "./interceptors";
import Settings from "../custom-classes/settings";
import { dtoToForm, formToDto } from "../components/reading-ai-module/stories/helpers";

const BASE_URL = `${Settings.API_GS_WEB_URL}/readingapi/reading-diagnostics`;
const STORIES_URL = `${Settings.API_GS_WEB_URL}/readingapi/stories`;

/**
 * Create a new custom reading diagnostic with its associated questions.
 *
 * @param {Object} diagnostic
 * @param {string} diagnostic.title
 * @param {string} diagnostic.subjectCode
 * @param {string} diagnostic.levelCode
 * @param {boolean} diagnostic.published
 * @param {number[]} diagnostic.questionIds
 * @returns {Promise<import('axios').AxiosResponse<number>>} The created diagnostic ID.
 */
export const createCustomReadingDiagnostic = (diagnostic) => {
    return axios.post(`${BASE_URL}`, diagnostic);
};

/**
 * Get a custom reading diagnostic by ID.
 *
 * @param {number} id Diagnostic ID.
 * @param {boolean} includeQuestions Whether to include questions in the response.
 * @returns {Promise<import('axios').AxiosResponse<any>>}
 *          { diagnostic, questions }
 */
export const getCustomReadingDiagnosticById = (id, includeQuestions = true) => {
    const params = { includeQuestions };
    return axios.get(`${BASE_URL}/${id}`, { params });
};

/**
 * Get all custom reading diagnostics created by the current teacher.
 * The teacherId is still required in the route, but the backend uses the session user internally.
 *
 * @param {string} teacherId Teacher ID (for route compatibility).
 * @returns {Promise<import('axios').AxiosResponse<any[]>>}
 */
export const getCustomReadingDiagnosticsByTeacher = (teacherId) => {
    return axios.get(`${BASE_URL}/by-teacher`);
};

/**
 * Update an existing custom reading diagnostic and replace its question list.
 *
 * @param {number} id Diagnostic ID.
 * @param {Object} diagnostic
 * @param {string} diagnostic.title
 * @param {string} diagnostic.subjectCode
 * @param {string} diagnostic.levelCode
 * @param {boolean} diagnostic.published
 * @param {number[]} diagnostic.questionIds
 * @returns {Promise<import('axios').AxiosResponse<void>>}
 */
export const updateCustomReadingDiagnostic = (id, diagnostic) => {

    return axios.put(`${BASE_URL}/${id}`, diagnostic);
};

/**
 * Delete a custom reading diagnostic by ID.
 *
 * @param {number} id Diagnostic ID.
 * @returns {Promise<import('axios').AxiosResponse<void>>}
 */
export const deleteCustomReadingDiagnostic = (id) => {
    return axios.delete(`${BASE_URL}/${id}`);
};

// Get a list of stories, each including its ordered slides.
export const getAllStories = (fromPage = 0, limit = 12, filters = {}) => {
    const params = {};
    if (filters.levelId != null) params.levelId = filters.levelId;
    if (filters.categoryId != null) params.categoryId = filters.categoryId;
    if (filters.search) params.search = filters.search;

    return axios.get(`${STORIES_URL}/read/${fromPage}/${limit}`, { params });
};

// Get a single story by id, already mapped to the camelCase form shape.
export const getStoryById = async (id) => {
    const res = await axios.get(`${STORIES_URL}/${id}`);
    return dtoToForm(res.data);
};

// Create a new story along with its slides. Receives the camelCase form.
export const createStory = (form) => {
    return axios.post(`${STORIES_URL}/create`, formToDto(form));
};

// Update an existing story and sync its slides. Receives the camelCase form.
export const updateStory = (id, form) => {
    return axios.put(`${STORIES_URL}/${id}`, formToDto(form));
};

// Delete a story by id.
export const deleteStory = (id) => {
    return axios.delete(`${STORIES_URL}/${id}`);
};

// Get all levels available for stories.
export const getStoryLevels = () => {
    return axios.get(`${STORIES_URL}/levels`);
};

// Get all categories available for stories.
export const getStoryCategories = () => {
    return axios.get(`${STORIES_URL}/categories`);
};

// Get the session metrics intended for charts in the Usage & Engagement dashboard
export const getSessionMetrics = (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null) => {
    const body = { 
        UserIds: userIds,
        DateFrom: dateFrom,
        DateTo: dateTo,
        RegionId: regionId,
        SchoolId: schoolId,
    };

    const params = {};
    if (period) params.period = period;

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/sessions`, body, { params });
}

// Get the session metrics intended for indicators in the Usage & Engagement dashboard
export const getSessionTotals = (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null) => {
    const body = { 
        UserIds: userIds,
        DateFrom: dateFrom,
        DateTo: dateTo,
        RegionId: regionId,
        SchoolId: schoolId,
    };

    const params = {};
    if (period) params.period = period;

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/sessions/totals`, body, { params });
}

// Get the content metrics intended for charts in the Progress & Mastery dashboard
export const getContentMetrics = (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null) => {
    const body = { 
        UserIds: userIds,
        DateFrom: dateFrom,
        DateTo: dateTo,
        RegionId: regionId,
        SchoolId: schoolId,
    };

    const params = {};
    if (period) params.period = period;

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/content`, body, { params });
}

// Get the performance metrics intended for charts in the Reading & Content dashboard
export const getPerformanceMetrics = (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null) => {
    const body = { 
        UserIds: userIds,
        DateFrom: dateFrom,
        DateTo: dateTo,
        RegionId: regionId,
        SchoolId: schoolId,
    };

    const params = {};
    if (period) params.period = period;

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/performance`, body, { params });
}

// Get the unit completion metrics intended for Units Completed chart
export const getUnitMetrics = (userIds, schoolId = null, regionId = null) => {
    const body = {
        UserIds: userIds,
        RegionId: regionId,
        SchoolId: schoolId,
    };

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/performance/units`, body);
}

// Get the Athenas reading lessons evaluation results (R and LLI standards, K to 5th grade, sp, en, adl subjects)
export const getReadingLessonsResults = (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null, fromPage = 0, limit = 10, filters = {}) => {
    const body = {
        UserIds: userIds,
        DateFrom: dateFrom,
        DateTo: dateTo,
        RegionId: regionId,
        SchoolId: schoolId,
        GivenName: filters.givenName || null,
        Surname: filters.surname || null,
        Username: filters.username || null,
    };

    const params = { fromPage, limit };
    if (period) params.period = period;

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/performance/lessons/results`, body, { params });
}

// Get the reading lessons results already aggregated by date, intended for the comprehension chart
export const getReadingLessonsSeries = (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null) => {
    const body = {
        UserIds: userIds,
        DateFrom: dateFrom,
        DateTo: dateTo,
        RegionId: regionId,
        SchoolId: schoolId,
    };

    const params = {};
    if (period) params.period = period;

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/performance/lessons/results/series`, body, { params });
}

// Get the module usage metrics intended for Usage Gaps chart.
export const getModuleUsage = (schoolId = null, regionId = null, userIds = null, fromPage = 0, limit = 10) => {
    const body = {
        RegionId: regionId,
        SchoolId: schoolId,
    };

    if (userIds != null) {
        body.UserIds = userIds;
    }

    const params = {
        fromPage,
        limit
    };

    return axios.post(`${Settings.API_GS_WEB_URL}/readingapi/metrics/module/usage`, body, { params });
}
