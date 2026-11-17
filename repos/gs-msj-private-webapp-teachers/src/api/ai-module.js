import axios from "./interceptors";
import Settings from "../custom-classes/settings";

const STORAGE_KEY = 'genial.skills.documents.v1';

/**
 * Safely parses a JSON string, returning a fallback value on failure.
 * @param {string|null} raw
 * @param {*} fallback
 * @returns {*}
 */
const safeParse = (raw, fallback) => {
    try {
        return raw ? JSON.parse(raw) : fallback;
    } catch {
        return fallback;
    }
};

/**
 * Reads all documents from localStorage.
 * @returns {Array}
 */
const readAll = () => {
    if (typeof window === 'undefined') return [];
    const docs = safeParse(window.localStorage.getItem(STORAGE_KEY), []);
    return Array.isArray(docs) ? docs : [];
};

/**
 * Sorts documents by most-recently updated or created first.
 * @param {Array} docs
 * @returns {Array}
 */
const sortDocs = (docs) =>
    [...docs].sort(
        (a, b) => new Date(b.updatedAt || b.createdAt) - new Date(a.updatedAt || a.createdAt)
    );

/**
 * Persists documents to localStorage and dispatches a change event.
 * @param {Array} docs
 */
const writeAll = (docs) => {
    if (typeof window === 'undefined') return;
    window.localStorage.setItem(STORAGE_KEY, JSON.stringify(sortDocs(docs)));
    window.dispatchEvent(new CustomEvent('genial-documents-changed', { detail: docs }));
};

/**
 * Returns true when document `a` is newer than document `b`.
 * @param {Object} a
 * @param {Object} b
 * @returns {boolean}
 */
const isNewer = (a, b) =>
    new Date(a?.updatedAt || a?.createdAt || 0) >
    new Date(b?.updatedAt || b?.createdAt || 0);

/**
 * Merges local and cloud document arrays, keeping the most-recent version of each.
 * @param {Array} localDocs
 * @param {Array} cloudDocs
 * @returns {Array}
 */
const mergeDocuments = (localDocs, cloudDocs) => {
    const byId = new Map();
    for (const doc of cloudDocs || []) byId.set(doc.id, doc);
    for (const doc of localDocs || []) {
        const existing = byId.get(doc.id);
        if (!existing || isNewer(doc, existing)) byId.set(doc.id, doc);
    }
    return sortDocs([...byId.values()]);
};

/**
 * Returns all locally stored documents sorted by most-recently updated.
 * @returns {Array}
 */
export const listDocuments = () => sortDocs(readAll());

/**
 * Fetches all documents from the cloud API.
 * @returns {Promise<AxiosResponse>}
 */
export const fetchDocuments = () => {
    return axios.get(Settings.getDocumentsAPI());
};

/**
 * To get the subjects.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const fetchSubjects = () => {
    return axios.get(Settings.getAISubjectsAPI());
};

/**
 * To get the levels.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const fetchLevels = () => {
    return axios.get(Settings.getAILevelsAPI());
};



/**
 * Fetches learning standards filtered by subject and grade.
 * @param {string} subject
 * @param {string} grade
 * @param {number} typeId Athenas code type, standards by default.
 * @returns {Promise<AxiosResponse>}
 */
export const fetchAthenasCode = (subject, grade, typeId) => {
    return axios.get(Settings.getStandardsForAI(subject, grade, typeId));
};

/**
 * To fetch the lessons.
 * @param subject The subject to call.
 * @param grade The grade to call.
 * @param queryText The query text for title, objectives or standards.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const fetchLessons = (subject, grade, queryText) => {
    return axios.get(Settings.getLessonsForAI(subject, grade, queryText));
};

/**
 * Syncs local documents with the cloud, merging conflicts by recency.
 * Falls back to local-only data when the cloud is unavailable.
 * @returns {Promise<{ documents: Array, source: string, warning: string }>}
 */
export const syncDocumentsWithCloud = async () => {
    const localDocs = readAll();
    try {
        const { data } = await axios.get(Settings.getDocumentsAPI());

        if (data.source && data.source !== 'supabase') {
            return { documents: sortDocs(localDocs), source: data.source, warning: data.warning || '' };
        }

        const cloudDocs = Array.isArray(data.documents) ? data.documents : [];
        const merged = mergeDocuments(localDocs, cloudDocs);
        writeAll(merged);

        const cloudById = new Map(cloudDocs.map((doc) => [doc.id, doc]));
        const toUpload = merged.filter((doc) => {
            const cloud = cloudById.get(doc.id);
            return !cloud || isNewer(doc, cloud);
        });
        await Promise.allSettled(toUpload.map((doc) => upsertCloudDocument(doc)));

        return { documents: listDocuments(), source: 'supabase', warning: '' };
    } catch (error) {
        return { documents: sortDocs(localDocs), source: 'local', warning: String(error?.message || error) };
    }
};

/**
 * Creates and persists a new AI-generated document locally and syncs it to the cloud.
 * Call sites must pass translated fallback strings via `defaultTitle` and `defaultToolTitle`
 * so that stored metadata matches the active UI language.
 * Example: saveGeneratedDocument({ ..., defaultTitle: t('documents:defaults.title'), defaultToolTitle: t('documents:defaults.toolTitle') })
 * @param {Object} params
 * @param {string} params.title - Document title.
 * @param {string} params.toolTitle - Name of the tool that generated the document.
 * @param {string} params.category - Document category slug.
 * @param {string} params.model - AI model identifier.
 * @param {string} params.content - Document content.
 * @param {string} params.prompt - Prompt used to generate the document.
 * @param {Object} params.values - Form values used as input.
 * @param {string} [params.kind='markdown'] - Content kind/format.
 * @param {string} [params.defaultTitle] - Translated fallback for title.
 * @param {string} [params.defaultToolTitle] - Translated fallback for toolTitle.
 * @returns {Object|null} The saved document, or null if content is empty.
 */
export const saveGeneratedDocument = ({
                                          title,
                                          toolTitle,
                                          category,
                                          model,
                                          content,
                                          prompt,
                                          values,
                                          kind = 'markdown',
                                          defaultTitle = 'ai-document',
                                          defaultToolTitle = 'ai-tool',
                                      }) => {
    const cleanContent = String(content || '').trim();
    if (!cleanContent) return null;

    const now = new Date().toISOString();
    const id = `doc_${Date.now()}_${Math.random().toString(36).slice(2, 8)}`;
    const doc = {
        id,
        title: title || toolTitle || defaultTitle,
        toolTitle: toolTitle || title || defaultToolTitle,
        category: category || 'ai',
        kind,
        model: model || '',
        content: cleanContent,
        prompt: prompt || '',
        values: values || {},
        createdAt: now,
        updatedAt: now,
    };

    writeAll([doc, ...readAll()]);
    upsertCloudDocument(doc).catch(() => {});
    return doc;
};

/**
 * Upserts a single document to the cloud API.
 * @param {Object} doc
 * @returns {Promise<AxiosResponse>}
 */
export const upsertCloudDocument = (doc) => {
    return axios.post(Settings.getDocumentsAPI(), doc);
};

/**
 * Deletes a document by ID from local storage and the cloud.
 * @param {string} id - The document ID.
 */
export const deleteDocument = (id) => {
    writeAll(readAll().filter((doc) => doc.id !== id));
    axios.delete(Settings.getDocumentByIdAPI(id)).catch(() => {});
};

/**
 * Applies a partial update to an existing document locally and syncs it to the cloud.
 * @param {string} id - The document ID.
 * @param {Object} patch - Fields to update.
 * @returns {Object|null} The updated document, or null if not found.
 */
export const updateDocument = (id, patch) => {
    let updated = null;
    const docs = readAll().map((doc) => {
        if (doc.id !== id) return doc;
        updated = { ...doc, ...patch, updatedAt: new Date().toISOString() };
        return updated;
    });
    writeAll(docs);
    if (updated) {
        axios.patch(Settings.getDocumentByIdAPI(id), updated).catch(() => {});
    }
    return updated;
};

/**
 * Exports a document as a Markdown-formatted string.
 * Caller must pass translated label strings so the export matches the active UI language.
 * Example: exportDocumentMarkdown(doc, { tool: t('documents:export.tool'), model: t('documents:export.model'), date: t('documents:export.date'), defaultTitle: t('documents:defaults.title') })
 * @param {Object} doc - The document object.
 * @param {Object} labels - Translated field labels.
 * @param {string} labels.tool - Translated label for the tool field.
 * @param {string} labels.model - Translated label for the model field.
 * @param {string} labels.date - Translated label for the date field.
 * @param {string} labels.defaultTitle - Translated fallback document title.
 * @returns {string}
 */
export const exportDocumentMarkdown = (doc, labels = {}) => {
    const stamp = new Date(doc.createdAt || Date.now()).toLocaleString();
    return [
        `# ${doc.title || labels.defaultTitle || 'ai-document'}`,
        '',
        `${labels.tool || 'Tool'}: ${doc.toolTitle || 'AI'}`,
        doc.model ? `${labels.model || 'Model'}: ${doc.model}` : null,
        `${labels.date || 'Date'}: ${stamp}`,
        '',
        doc.content || '',
    ]
        .filter(Boolean)
        .join('\n');
};

/** ============== EXTRACT =================== **/
/**
 * Uploads a file to the extract-text API and returns the extracted text.
 * @param {File} file - The file to extract text from.
 * @returns {Promise<string>} The extracted text.
 */
export const extractTextFromFile = async (file) => {
    const fd = new FormData();
    fd.append('file', file);
    const { data } = await axios.post(Settings.getExtractTextAPI(), fd);
    return data.text || '';
};

/** ============== QUOTAS CONFIGURTION =================== **/

// Reads the AI quota configuration
export const getQuotasSettings = async () => {
    return axios.get(Settings.getQuotasSettingsAPI());
}


// Saves the AI quota configuration and returns the result.
export const updateQuotasSettings = async (quotasEnabled, aiEnabled, resources) => {
    return axios.put(Settings.getQuotasSettingsAPI(), {
        quotasEnabled,
        aiEnabled,
        defaultScope: 'Global',
        resources
    });
}

// Lists the quota limits that override the global default for a specific role, user or school
export const getQuotaLimits = async (scope, scopeId, currentPage = 0, limiter = 10) => {
    const params = new URLSearchParams();
    if (scope) params.append('scope', scope);
    if (scopeId) params.append('scopeId', scopeId);
    params.append('currentPage', currentPage);
    params.append('limiter', limiter);

    return axios.get(`${Settings.getQuotaLimitsAPI()}?${params.toString()}`);
}

// Lists the users who consumed a resource in the current period, with the limit governing each one.
export const getQuotaUserUsage = async (resourceTypeCode, scope, scopeId, userId, currentPage = 0, limiter = 10) => {
    const params = new URLSearchParams();
    if (resourceTypeCode) params.append('resourceTypeCode', resourceTypeCode);

    if (scope) params.append('scope', scope);
    if (scopeId) params.append('scopeId', scopeId);
    if (userId) params.append('userId', userId);

    params.append('currentPage', currentPage);
    params.append('limiter', limiter);

    return axios.get(`${Settings.getQuotaLimitsAPI()}/usage?${params.toString()}`);
}

// Creates or updates the override for one resource and target.
export const saveQuotaLimit = async (limit) => {
    return axios.post(Settings.getQuotaLimitsAPI(), limit);
}

// Removes an override so its target inherits the limit from the level above it again.
export const deleteQuotaLimit = async (id) => {
    return axios.delete(`${Settings.getQuotaLimitsAPI()}/${id}`);
}
