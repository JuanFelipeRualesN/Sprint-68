// Helpers to get the translation keys for the reading results labels, based on the exam type and subject

const EXAM_TYPE_KEYS = {
    "1": "dashboards.progress-and-mastery.reading-results.exam-type-quiz",
    "2": "dashboards.progress-and-mastery.reading-results.exam-type-pre",
    "3": "dashboards.progress-and-mastery.reading-results.exam-type-post",
};

const SUBJECT_KEYS = {
    adl: "dashboards.progress-and-mastery.reading-results.subject-adl",
    sp: "dashboards.progress-and-mastery.reading-results.subject-spanish",
    en: "dashboards.progress-and-mastery.reading-results.subject-english",
};

const UNKNOWN_KEY = "dashboards.progress-and-mastery.reading-results.unknown";

export const getExamTypeLabelKey = (examType) => {
    return EXAM_TYPE_KEYS[String(examType)] ?? UNKNOWN_KEY;
};

export const getSubjectLabelKey = (subject) => {
    return SUBJECT_KEYS[String(subject ?? "").toLowerCase()] ?? UNKNOWN_KEY;
};

export const getGradeLabel = (levelCode) => {
    const code = String(levelCode ?? "").toLowerCase();

    if (!code) return "";

    return code === "k" ? "K" : code;
};
