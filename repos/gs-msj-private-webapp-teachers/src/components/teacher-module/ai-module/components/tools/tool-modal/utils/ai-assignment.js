import { generateStreamedResponse } from '../../../../../../../api/ai-generate';
import { setHomework } from '../../../../../../../api/teachers';
import { buildQuestionsFromAi, resolveQuizTypeId } from '../../../../../../quiz-creator/services/ai-quiz-mapper';
import { latexToPlainText } from './latex-to-text';

/**
 * Turns the questions an AI tool generated into a draft assignment the teacher can open, review and
 * publish in the editor.
 *
 * The generated document is clean, so a second AI call converts it
 * into the JSON the assignment editor's AI panel uses and the result is posted to teachersapi/homework/create.
 */

// Total points of the created assignment
const ASSIGNMENT_SCORE = '100';

const TEMPERATURE = 0;

export const ASSIGNMENT_ERRORS = {
    NO_QUESTIONS: 'noQuestions',
    SAVE: 'save',
};

const assignmentError = (code) => Object.assign(new Error(code), { code });

const escapeHtml = (text) => String(text)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')

const stripImages = (markdown) => String(markdown || '').replace(/!\[[^\]]*\]\([^)]*\)/g, '');

// Plain text to the HTML the assignment editor stores
const toParagraphs = (text) => String(text || '')
    .split(/\n+/)
    .map(line => line.trim())
    .filter(Boolean)
    .map(line => `<p>${escapeHtml(line).replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')}</p>`)
    .join('');

export const createAssignment = async ({ output, sourceText = '', sourceIsReading = false, conversionKey = 'conversionSystem', t }) => {
    const text = (key, vars = {}) => t(`tool-modal:assignment.${key}`, { ...vars, interpolation: { escapeValue: false } });

    // The assignment shows text as is, so formulas go as plain text
    const request = text('conversionRequest', {
        content: latexToPlainText(stripImages(output)),
        sourceBlock: String(sourceText || '').trim() ? text('sourceBlock', { text: latexToPlainText(sourceText) }) : '',
    });

    const raw = await generateStreamedResponse(text(conversionKey), request, undefined, TEMPERATURE);

    let data = null;

    try {
        const json = String(raw || '').match(/\{[\s\S]+\}/);
        data = json ? JSON.parse(json[0]) : null;
    } catch {
        data = null;
    }
    if (!data || !Array.isArray(data.questions) || data.questions.length === 0) {
        throw assignmentError(ASSIGNMENT_ERRORS.NO_QUESTIONS);
    }

    // A question without text has nothing to fix in the editor, so it is dropped.
    // Anything else (a duplicated option, a missing correct answer) is left for the editor's own validation
    const { questions: mapped, instructions } = buildQuestionsFromAi(data.questions);
    const questions = mapped.filter(q => String(q.QuizQuestionModel.Desc || '').trim());
    if (questions.length === 0) throw assignmentError(ASSIGNMENT_ERRORS.NO_QUESTIONS);

    const title = String(data.title || '').trim();
    const ownReading = sourceIsReading ? latexToPlainText(String(sourceText || '')).trim() : '';
    const readingText = ownReading || String(data.readingText || '').trim();

    let response;
    try {
        response = await setHomework({
            QuizModel: {
                Title: title,
                Instruction: toParagraphs(data.instructions) + (readingText ? `<p><strong>${escapeHtml(text('readingHeading'))}</strong></p>${toParagraphs(readingText)}` : ''),
                CustomValue: ASSIGNMENT_SCORE,
                QuizTypeId: resolveQuizTypeId(questions),
            },
            QuizRubricModel: { Desc: '' },
            QuizAttachmentModelList: [],
            QuizMatchInstructionsModel: instructions,
            QuestionRequestModelList: questions.map(q => ({
                pid: q.pid,
                QuizQuestionModel: q.QuizQuestionModel,
                QuizQuestionResponseModelList: q.QuizQuestionResponseModelList,
            })),
        });
    } catch {
        throw assignmentError(ASSIGNMENT_ERRORS.SAVE);
    }

    const quizId = response?.data?.QuizRequestModel?.QuizModel?.Id;
    if (!quizId) throw assignmentError(ASSIGNMENT_ERRORS.SAVE);

    return { quizId, title, questionCount: questions.length };
};
