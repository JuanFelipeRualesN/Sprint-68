import axios from "./interceptors";
import Settings from "../custom-classes/settings";

/**
 * Event attachments reuse the generic TeacherAPI file endpoints — Settings.js keeps the
 * historical (quiz-related) names for some of them.
 */
const fileRulesEndpoint = Settings.getQuizFileRulesAPI();
const fileUploadEndpoint = Settings.getQuizFileUploadAPI();
const fileDeleteEndpoint = Settings.getDeleteFileAPI();
const fileSignatureEndpoint = Settings.getSignatureFileApi();

/**
 * Get the supported file rules (allowed extensions, max size) for attachments.
 * @returns {Promise}
 */
export const getEventFileRules = () => {
    return axios.get(fileRulesEndpoint);
};

/**
 * Upload a single attachment file.
 * @param {File} file The file to upload.
 * @returns {Promise}
 */
export const uploadEventFile = (file) => {
    const formData = new FormData();
    formData.append("file", file);
    return axios({
        method: "POST",
        url: fileUploadEndpoint,
        data: formData,
    });
};

/**
 * Delete a previously uploaded file.
 * @param {string} fileName The generated file name (as returned by uploadEventFile).
 * @returns {Promise}
 */
export const deleteEventFile = (fileName) => {
    return axios({
        method: "POST",
        url: fileDeleteEndpoint,
        data: JSON.stringify({ fileName }),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Get a freshly signed URL to open/download a persisted file.
 * @param {string} fileName The generated file name, without extension.
 * @param {string} extension The file extension, without the leading dot.
 * @returns {Promise}
 */
export const getEventFileSignedUrl = (fileName, extension) => {
    return axios.get(`${fileSignatureEndpoint}${fileName}/${extension}`);
};
