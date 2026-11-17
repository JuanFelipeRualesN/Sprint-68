import Settings from "../../../../custom-classes/settings";

/**
 * To validate the minimum codes that are required to not show the scores required.
 * @param data
 * @returns {boolean}
 */
export const learningGapMinScoreValidator = (data) => {
    const requiredSubjectCodes = Settings.getLearningGapMinimumSubjectsRequired();
    // Check if all required subject codes are in the data and have valid scores
    // Loop through required subject codes
    for (let i = 0; i < requiredSubjectCodes.length; i++) {
        const code = requiredSubjectCodes[i];

        // Find the subject in data
        const subject = data.find((item) => item.SubjectCode === code);

        // If the subject is missing or scores are null, return false
        if (
            !subject || // Subject not found
            subject.InitialGradeScore === null
        ) {
            return false;
        }
    }

    // If all required subjects pass, return true
    return true;
};