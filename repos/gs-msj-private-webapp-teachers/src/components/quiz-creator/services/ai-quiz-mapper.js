// Functions to map the AI's quiz output into the shape this form expects
// Shared between the AI tools and the AI quiz creator

/**
 * Question types, matching the `quiz_types` rows.
 */
export const PremiseTypes = {
  SELECTION: '1',
  MATCH: '2',
  OPEN: '3',
  TRUE_FALSE: '4',
};

// Quiz-level type meaning "holds more than one question type".
export const MIXED_QUIZ_TYPE = '5';

/**
 * Works out the quiz-level type from the questions it holds.
 *
 * The quiz-level value is what older assignments rely on to pick an editor,
 * and what the FK on quizs.quiz_type_id accepts, so it is kept consistent:
 * one type when every question agrees, mixed when they do not.
 */
export const resolveQuizTypeId = (questions, fallback) => {
  const types = [...new Set(
    (questions || [])
      .map(q => q?.QuizQuestionModel?.QuestionTypeId)
      .filter(Boolean)
      .map(String)
  )];

  if (types.length === 0) return fallback ?? PremiseTypes.SELECTION;
  if (types.length === 1) return types[0];
  return MIXED_QUIZ_TYPE;
}

// Reads match-question pairs from any of the AI's possible field name variants
export const  normalizePairs = (question) => {
  const candidates = question.Pairs || question.pairs || question.pareados || question.pareos || question.matches || question.matching || null;
  if (!Array.isArray(candidates) || candidates.length === 0) return [{ l: '', r: '' }];
  return candidates.map(pair => ({
    l: pair?.l ?? pair?.L ?? pair?.left ?? pair?.premise ?? pair?.premisa ?? pair?.lhs ?? pair?.term ?? '',
    r: pair?.r ?? pair?.R ?? pair?.right ?? pair?.response ?? pair?.respuesta ?? pair?.rhs ?? pair?.match ?? pair?.definition ?? '',
  }));
}

/**
 * Turns the AI's question list into this form's shape.
 *
 * The four TypeIds map onto the `quiz_types` rows: 1 selection, 2 match,
 * 3 open, 4 true/false. Match is the odd one — it becomes one question per
 * pair, tied together by a shared QuizMatchInstructionId, with the prompt kept
 * aside as the group's instruction.
 */
export const buildQuestionsFromAi = (aiQuestions) => {
  const newQuestions = [];
  const newInstructions = [];

  for (const question of aiQuestions) {
    const typeId = String(question.TypeId || '1');
    const questionText = question.Text || question.text || question.Prompt || question.prompt || '';
    const pid = () => `${Date.now()}-${Math.random()}`;

    if (typeId === '1') {
      const options = Array.isArray(question.Options) ? question.Options : ['', '', '', ''];
      const correctIndex = question.CorrectIndex ?? question.correctIndex ?? 0;
      newQuestions.push({
        pid: pid(),
        QuizQuestionModel: { Desc: questionText, QuestionTypeId: '1' },
        QuizQuestionResponseModelList: options.map((option, i) => ({
          Response: option,
          Correct: i === correctIndex ? '1' : '0',
          Value: i === correctIndex ? '1' : '0',
        })),
      });
    } else if (typeId === '2') {
      const normalizedPairs = normalizePairs(question);
      const matchGroupId = Math.random().toString(36).substring(7);
      for (const pair of normalizedPairs) {
        newQuestions.push({
          pid: pid(),
          QuizQuestionModel: { Desc: pair.l, QuestionTypeId: '2', QuizMatchInstructionId: matchGroupId },
          QuizQuestionResponseModelList: [{ Response: pair.r, Correct: '1', Value: '1' }],
        });
      }
      newInstructions.push({ Instruction: questionText, Id: matchGroupId });
    } else if (typeId === '3') {
      newQuestions.push({
        pid: pid(),
        QuizQuestionModel: { Desc: questionText, QuestionTypeId: '3' },
        QuizQuestionResponseModelList: [],
      });
    } else if (typeId === '4') {
      const correctIndex = question.CorrectIndex ?? question.correctIndex ?? 0;
      newQuestions.push({
        pid: pid(),
        QuizQuestionModel: { Desc: questionText, QuestionTypeId: '4' },
        QuizQuestionResponseModelList: [
          { Response: 'Cierto', Correct: correctIndex === 0 ? '1' : '0', Value: correctIndex === 0 ? '1' : '0' },
          { Response: 'Falso', Correct: correctIndex === 1 ? '1' : '0', Value: correctIndex === 1 ? '1' : '0' },
        ],
      });
    }
  }

  return { questions: newQuestions, instructions: newInstructions };
}
