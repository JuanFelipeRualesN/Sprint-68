import axios from "./interceptors";
import Settings from "../custom-classes/settings";


/**
 * Get reading diagnostic questions with pagination support.
 * * @param {string|null} subjectCode
 * @param {string|null} levelCode
 * @param {string|null} constructedCodeLike
 * @param {number} fromPage // New parameter: 1-based index
 * @param {number} limit    // New parameter: items per page
 * @returns {Promise<any>} Diagnostic questions list
 */
export const getReadingQuestionDiagnostics = (
    subjectCode = null,
    levelCode = null,
    constructedCodeLike = null,
    fromPage = 1,  // Default to the first page
    limit = 10     // Default to 10 items per page
) => {
    const params = {};

    if (subjectCode) params.subjectCode = subjectCode;
    if (levelCode) params.levelCode = levelCode;
    if (constructedCodeLike) params.constructedCodeLike = constructedCodeLike;

    // --- ADD PAGINATION PARAMETERS ---
    // These must be included to utilize the backend pagination
    params.fromPage = fromPage;
    params.limit = limit;
    // ---------------------------------

    return axios.get(Settings.getAthenasReadigDiagnosticQuestions(), { params });
};
/**
 * Get the quiz from Athenas
 * @param quizId Quiz ID
 * @returns {Promise<string|any>}
 */
export const getQuiz = (quizId) =>
{
    return axios.get(Settings.getQuizAPI(quizId));
};

/**
 * To get the exams of the lesson's id sent.
 * @param lessonId Lesson's id.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getLessonExams = (lessonId)=>{
    return axios.get(Settings.getLessonExamsAPI(lessonId));
};

/**
 * Returns the lesson details
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonDetails = (lessonId) =>{
    return axios({
        method: "POST",
        url: Settings.getLessonDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Returns the lesson details through the Teachers API
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonDetailsForTeachers = (lessonId) =>{
    return axios.get(Settings.getLessonDetailsTeachersAPI(lessonId));
};

/**
 * Get the lessons
 * @param subjectCode Subject code
 * @param levelCode level code
 * @returns {Promise<any>}
 */
export const getLessons = (subjectCode, levelCode) =>{
    let data = {
        "LessonModelList": [{
            "LevelCode": levelCode,
            "SubjectCode": subjectCode
        }]
    };

    return axios({
        method: "POST",
        url: Settings.getLessonsByLessonsModelAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};


export const getMaterialAdditionalByLesson = (lessonId, page, pageSize) => {
    page = page - 1;
    return axios.get(
        Settings.getMaterialAdditionalByLessonAPI(lessonId, page, pageSize)
    );
};

export const getMaterialAdditionalById = (materialId) => {
    return axios.get(
        Settings.getMaterialAdditionalById(materialId)
    );
};

export const getMaterialAdditionalAPI= () => {
    return axios.get(
        Settings.getMaterialAdditionalAPI()
    );
};

export const getLibraries = (data) => {
  return axios({
    method: "POST",
    url: Settings.getMaterialAdditionalAPI(),
    data: JSON.stringify(data),
    headers: { "Content-Type": "application/json" },
  });
};

/** To get the additional material **/
export const getMaterialOffline = (lessonId, quizId = 0, format = "pdf") => {
    /** Modified by: Pedro Ortiz on 01/20/2021
     * The endpoint is different from the original. After we clarified with Ovidio Hernández that
     * the offline lesson should be the same material that Athenas has, we change it to the
     * endpoint that is responsible of printing the lesson for students
     * **/
    return axios.get(
        Settings.getMaterialOfflineAPI(lessonId, quizId, format)
    );
};


/**
 * To get the quiz id for student.
 * @param quizId
 * @param response
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getQuizStudent = (quizId, response) =>
    {
        return axios.get(Settings.getQuizAPIStudent(quizId));
    };

/**
 * Normalize display grade strings like "5to", "1ro", "K", "k" to the Athenas
 * code format which is just the digit or lowercase "k".
 */
export function normalizeGrade(g) {
    if (g == null) return null;
    const s = String(g).trim().toLowerCase();
    if (s === 'k' || s === 'kinder' || s === 'kindergarten') return 'k';
    const m = s.match(/^(\d{1,2})/);
    return m ? m[1] : s;
}

/**
* Translate a display subject name (or already-code value) to the Athenas
* subjectCode used by the API. Returns an array because one display name can
* fan out to multiple codes (e.g. "Ciencias" → sci-sp + bi-sp + qu-sp + fi-sp).
*   "Matemáticas"        → ["mat-sp", "mat-en"]
*   "mat-sp"             → ["mat-sp"]  (passthrough if it's already a code)
*   "Español / Lenguaje" → ["sp"]
*/
const SUBJECT_NAME_TO_CODES = {
    'matemáticas':       ['mat-sp', 'mat-en'],
    'matematicas':       ['mat-sp', 'mat-en'],
    'matemática':        ['mat-sp', 'mat-en'],
    'matematica':        ['mat-sp', 'mat-en'],
    'math':              ['mat-en', 'mat-sp'],
    'ciencias':          ['sci-sp', 'sci-en', 'bi-sp', 'qu-sp', 'fi-sp'],
    'ciencias naturales':['sci-sp'],
    'science':           ['sci-en', 'sci-sp'],
    'español':           ['sp'],
    'espanol':           ['sp'],
    'español / lenguaje':['sp'],
    'lenguaje':          ['sp'],
    'inglés':            ['en'],
    'ingles':            ['en'],
    'inglés / ela':      ['en'],
    'english':           ['en'],
    'ela':               ['en'],
    'estudios sociales': ['sci-so'],
    'social studies':    ['sci-so'],
    'biología':          ['bi-sp'],
    'biologia':          ['bi-sp'],
    'biology':           ['bi-en', 'bi-sp'],
    'química':           ['qu-sp'],
    'quimica':           ['qu-sp'],
    'chemistry':         ['che-en', 'qu-sp'],
    'física':            ['fi-sp'],
    'fisica':            ['fi-sp'],
    'physics':           ['phy-en', 'fi-sp'],
    'álgebra':           ['a1-sp', 'a2'],
    'algebra':           ['a1-sp', 'a2'],
    'álgebra i':         ['a1-sp'],
    'álgebra ii':        ['a2'],
    'geometría':         ['geo-sp'],
    'geometria':         ['geo-sp'],
    'precalculus':       ['pc-en'],
    'precálculo':        ['pc-en'],
};

/** To normalize subjects **/
export function normalizeSubject(s) {
    if (!s) return [];
    const key = String(s).trim().toLowerCase();
    // Already a code? (contains a hyphen, or is short like "sp"/"en")
    if (/^[a-z0-9]{1,3}(-[a-z0-9]+)?$/.test(key)) return [key];
    return SUBJECT_NAME_TO_CODES[key] || [];
}
