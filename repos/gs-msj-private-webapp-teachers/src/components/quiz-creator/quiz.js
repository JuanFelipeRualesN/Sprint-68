import React, {
  useState, useEffect, useRef, useCallback, useContext, forwardRef, useImperativeHandle
} from 'react';
import {
  Row, Col, Button, Modal, ModalHeader, ModalBody, ModalFooter,
} from 'reactstrap';
import { Translate, LocalizeContext } from 'react-localize-redux';
import LanguageManager from '../../custom-classes/language-manager';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Alert from 'react-s-alert';
import CircularProgress from '@material-ui/core/CircularProgress';
import { Redirect } from 'react-router-dom';

import QuizHeader from './subcomponents/quiz-header';
import Premise   from './subcomponents/premise';
import ToolBar   from './subcomponents/toolbar';
import {validateQuiz} from '../../utils/quiz-validation';
import {fetchQuiz, saveQuiz, publishQuiz}    from './services/quiz-service';
import { PremiseTypes, resolveQuizTypeId, buildQuestionsFromAi } from './services/ai-quiz-mapper';
import useVersionedTranslations from '../../hooks/useVersionedTranslations';

import './subcomponents/premise.css';

/**
 * Hook to normalize HTML content for comparison in quiz validation
 */
function useHtmlNormalizer() {
  return useCallback((html = '') => {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');

    const cleanNode = (node) => {
      if (node.nodeType === Node.ELEMENT_NODE) {
        const tag = node.tagName.toLowerCase();

        if (tag === 'temporary') {
          const parent = node.parentNode;
          while (node.firstChild) parent.insertBefore(node.firstChild, node);
          parent.removeChild(node);
          return;
        }

        node.classList.remove('ql-cell-focused', 'ql-table-temporary');
        if (!node.className) node.removeAttribute('class');

        [...node.attributes].forEach(attr => {
          const name = attr.name.toLowerCase();
          if (name === 'contenteditable') node.removeAttribute(attr.name);
          if (/^data-(row|cell)/.test(name)) node.removeAttribute(attr.name);
        });

        if (tag === 'audio' || tag === 'video') {
          [...node.childNodes].forEach(ch => {
            if (ch.nodeType === Node.TEXT_NODE) {
              ch.textContent = '';
            } else if (
              ch.nodeType === Node.ELEMENT_NODE &&
              ch.tagName.toLowerCase() === 'span' &&
              ch.getAttribute('contenteditable') === 'false' &&
              (ch.textContent || '').trim() === ''
            ) {
              ch.remove();
            }
          });
        }

        const attrs = [...node.attributes].map(a => [a.name, a.value]);
        attrs.sort((a, b) => a[0].localeCompare(b[0]));
        attrs.forEach(([name]) => node.removeAttribute(name));
        attrs.forEach(([name, value]) => node.setAttribute(name, value));

        [...node.childNodes].forEach(cleanNode);
      }

      if (node.nodeType === Node.TEXT_NODE) {
        const parent = node.parentNode?.nodeName?.toLowerCase();

        if (parent === 'audio' || parent === 'video') {
          node.textContent = '';
          return;
        }

        node.textContent = node.textContent
          .replace(/\uFEFF/g, '')
          .replace(/\s+/g, ' ')
          .trim();
      }
    };

    cleanNode(doc.body);

    const visibleText = (doc.body.textContent || '')
      .replace(/\u00A0/g, ' ')
      .replace(/\s+/g, ' ')
      .trim();

    const hasMedia = !!doc.body.querySelector('img,video,audio,iframe,embed,object,svg,math');

    if (!visibleText && !hasMedia) return '';

    return doc.body.innerHTML
      .replace(/'/g, '"')
      .replace(/&(?!(amp|lt|gt|quot|#\d+);)/g, '&amp;')
      .replace(/\s+/g, ' ')
      .trim();
  }, []);
}

/**  
 * Hook that stores the first data and detects if changes have occurred.
 * Returns hasChanges and commitSnapshot.
 */
function useUnsavedChanges(getCurrentState, normalizeHtml) {
  const initialSnapshot = useRef(null);

  useEffect(() => {
    if(initialSnapshot.current === null)
    {
      initialSnapshot.current = getCurrentState();
    }
  }, [getCurrentState]);

  const hasChanges = useCallback(() => {
    const before = initialSnapshot.current;
    const now    = getCurrentState();

    if (!before) return false;

    // compare fields from quiz data
    const simpleKeys = [
      'Title', 'Instruction', 'Rubric',
      'CustomValue', 'QuizTypeId', 'FilesCount',
    ];
    for (const key of simpleKeys) {
      const a = /<[^>]+>/.test(before[key])
        ? normalizeHtml(before[key])
        : before[key];

      const b = /<[^>]+>/.test(now[key])
        ? normalizeHtml(now[key])
        : now[key];
        
      if (a !== b) return true;
    }

    // compare QuestionRequestModelList
    const questionInitialData = Array.isArray(before.QuestionRequestModelList)
      ? before.QuestionRequestModelList
      : [];
    const questionChangeData = Array.isArray(now.QuestionRequestModelList)
      ? now.QuestionRequestModelList
      : [];

    if (questionInitialData.length !== questionChangeData.length) return true;



    for (let i = 0; i < questionInitialData.length; i++) {
      const itemsInitialData = questionInitialData[i]
      const itemsChangeData = questionChangeData[i];

      if (normalizeHtml(itemsInitialData.QuizQuestionModel.Desc) !== normalizeHtml(itemsChangeData.QuizQuestionModel.Desc)) return true;
      if (itemsInitialData.QuizQuestionResponseModelList.length !== itemsChangeData.QuizQuestionResponseModelList.length) return true;

      for (let j = 0; j < itemsInitialData.QuizQuestionResponseModelList.length; j++) {
        const responseInitialData = itemsInitialData.QuizQuestionResponseModelList[j];
        const responseChangeData = itemsChangeData.QuizQuestionResponseModelList[j];

        const normalizeResponseValue = (value) => {
          if (value == "1.00") return "1";
          if (value == "0.00") return "0";
          return value;
        }

        if (normalizeHtml(responseInitialData.Response) !== normalizeHtml(responseChangeData.Response)) return true;
        if (normalizeResponseValue(responseInitialData.Correct) !== normalizeResponseValue(responseChangeData.Correct)) return true;
        if (normalizeResponseValue(responseInitialData.Value) !== normalizeResponseValue(responseChangeData.Value)) return true;
      }
    }

    return false;
  }, [getCurrentState, normalizeHtml]);

  const commitSnapshot = useCallback(() => {
    initialSnapshot.current = getCurrentState();
  }, [getCurrentState]);

  return { hasChanges, commitSnapshot };
}


/**
 * Hook to manage quiz save and draft state for a quiz editor.
 * Returns draft, loading, addQuestion, updateQuestion, setDraft, saveDraft.
*/
function useQuizDraft(quizId) {
  const [draft,    setDraft]    = useState(null);
  const [loading,  setLoading]  = useState(true);

  /**
   * Appends a question of the given type.
   *
   * Match is the exception: it needs at least two premises to be meaningful,
   * so it adds a pair sharing one QuizMatchInstructionId — the same shape the
   * AI import and the older quizzes produce.
   */
  const addQuestion = (typeId = PremiseTypes.SELECTION) =>
    setDraft(d => {
      const base = () => ({
        pid: `${Date.now()}-${Math.random()}`,
        QuizQuestionModel: { Desc: '', QuestionTypeId: typeId },
        QuizQuestionResponseModelList: [],
      });

      const additions = [];
      const newInstructions = [];
      if (typeId === PremiseTypes.MATCH) {
        const matchGroupId = Math.random().toString(36).substring(7);
        for (let i = 0; i < 2; i++) {
          const q = base();
          q.QuizQuestionModel.QuizMatchInstructionId = matchGroupId;
          additions.push(q);
        }
        // The group needs its instruction row too
        newInstructions.push({ Instruction: '', Id: matchGroupId });
      } else {
        additions.push(base());
      }

      return {
        ...d,
        // A quiz holding more than one kind of question is stored as mixed;
        // otherwise it keeps the single type it actually contains.
        QuizTypeId: resolveQuizTypeId([
          ...(d?.QuestionRequestModelList || []),
          ...additions,
        ], d?.QuizTypeId),
        QuestionRequestModelList: [
          ...(d?.QuestionRequestModelList || []),
          ...additions,
        ],
        QuizMatchInstructionsModel: [
          ...(d?.QuizMatchInstructionsModel || []),
          ...newInstructions,
        ],
      };
    });

  const updateQuestion = (pid, updater) =>
    setDraft(d => ({
      ...d,
      QuestionRequestModelList: d.QuestionRequestModelList.map(q =>
        q.pid === pid ? updater(q) : q
      ),
    }));

  const saveDraft = () => saveQuiz(draft);

  const publishDraft = () => publishQuiz(draft);

  useEffect(() => {
    let active = true;
    (async () => {
      const data = quizId ? await fetchQuiz(quizId) : {
        Title: '', Instruction: '', Rubric: '', CustomValue: '',
        QuizTypeId: '1', FilesCount: 0, QuestionRequestModelList: [],
        QuizAttachmentModelList: [], QuizMatchInstructionsModel: []
      };
      if (active) {
        const withPid = (data.QuestionRequestModelList || []).map(q => ({
          ...q,
          pid: q.pid ?? q.Id ?? `${Date.now()}-${Math.random()}`,
        }));
        data.QuestionRequestModelList = withPid;
        setDraft(data);
        setLoading(false);
      }
    })();
    return () => { active = false; };
  }, [quizId]);

  return { draft, loading, addQuestion, updateQuestion, setDraft, saveDraft, publishDraft };
}

function ExitGuardModal({ visible, onConfirm, onCancel }) {
  return (
    <Modal isOpen={visible} toggle={onCancel}>
      <ModalHeader toggle={onCancel}>
        <Translate id="quiz.leave" />
      </ModalHeader>
      <ModalBody className="text-center">
        <h4><Translate id="quiz.goback-warning-title" /></h4>
        <p ><Translate id="quiz.goback-warning-description" /></p>
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={onConfirm}>
          <Translate id="quiz.yes-btn" />
        </Button>
        <Button color="secondary" onClick={onCancel}>
          <Translate id="quiz.cancel-btn" />
        </Button>
      </ModalFooter>
    </Modal>
  );
}

function QuizQuestions({ 
  questions,
  quizTypeId,
  onUpdate,
  onRemove,
  setSelectedPremise,
  published,
  setSelectedAnswer,
  languages
}) {
  if (!Array.isArray(questions) || questions.length === 0) return null;

  return questions.map((q, idx) => (
    <Col 
      key={`${q.pid}-${q.QuizQuestionModel?.QuestionTypeId ?? quizTypeId}`}
      xs={12}
      tabIndex={0}
      className="premise-container"
      onFocus={() => setSelectedPremise(q?.pid)}
    >
      <Premise
        premise={q}
        index={idx}
        quizTypeId={quizTypeId}
        published={published}
        onQuestionChange={(pid, desc) =>
          onUpdate(pid, (prev) => ({
            ...prev,
            QuizQuestionModel: { ...(prev.QuizQuestionModel || {}), Desc: desc },
          }))
        }
        onAnswersChange={(pid, newAnswers) =>
            onUpdate(pid, (prev) => ({
              ...prev,
              QuizQuestionResponseModelList: newAnswers,
            }))
          
        }
        onRemove={onRemove}
        onSelectAnswer={(pid, answerIdx) => setSelectedAnswer({ pid, idx: answerIdx })}
        languages={languages}
        />
    </Col>
  ));
}

/**
 * Rebuilds the current quiz into the same JSON shape the AI produces, so the
 * assistant can read what is on screen, edit it and hand it back without
 * dropping anything it did not author.
 */
function buildAiContextFromDraft(draft) {
  if (!draft) return { title: '', instructions: '', questions: [] };

  const matchInstructions = draft.QuizMatchInstructionsModel || [];
  const questions = [];
  const seenMatchGroups = new Set();

  for (const q of draft.QuestionRequestModelList || []) {
    const model = q.QuizQuestionModel || {};
    const responses = q.QuizQuestionResponseModelList || [];
    const typeId = String(model.QuestionTypeId || '1');

    if (typeId === '2') {
      // Match questions were exploded one-per-pair; regroup them so the AI sees
      // the same single question it originally sent.
      const groupId = model.QuizMatchInstructionId;
      if (seenMatchGroups.has(groupId)) continue;
      seenMatchGroups.add(groupId);

      const pairs = (draft.QuestionRequestModelList || [])
        .filter(other => other.QuizQuestionModel?.QuizMatchInstructionId === groupId)
        .map(other => ({
          l: other.QuizQuestionModel?.Desc || '',
          r: other.QuizQuestionResponseModelList?.[0]?.Response || '',
        }));

      questions.push({
        TypeId: 2,
        Text: matchInstructions.find(i => i.Id === groupId)?.Instruction || '',
        Pairs: pairs,
      });
      continue;
    }

    const correctIndex = responses.findIndex(r => r.Correct === '1');
    questions.push({
      TypeId: Number(typeId),
      Text: model.Desc || '',
      ...(typeId === '3' ? {} : {
        Options: responses.map(r => r.Response || ''),
        CorrectIndex: correctIndex >= 0 ? correctIndex : 0,
      }),
    });
  }

  return {
    title: draft.Title || '',
    instructions: draft.Instruction || '',
    questions,
  };
}

const Quiz = forwardRef(function Quiz({ quizId }, ref) {
  const normalizeHtml  = useHtmlNormalizer();
  const {
    draft,
    loading,
    addQuestion,
    updateQuestion,
    setDraft,
    saveDraft,
    publishDraft
  } = useQuizDraft(quizId);

  const [showExit, setShowExit]   = useState(false);
  const [goList,   setGoList]     = useState(false);
  const [selectedPremise, setSelectedPremise] = useState(null);
  const [selectedAnswer, setSelectedAnswer] = useState({ pid: null, idx: null });
  const [published, setPublished] = useState(false);

  /**
   * Surface for the AI assistant panel.
   *
   * Upstream this form runs on react-hook-form, so the equivalent methods call
   * setValue(). Here the quiz lives in useQuizDraft's local state, so the same
   * three operations are expressed against setDraft instead — the data shape
   * and the AI contract are identical either way.
   */
  useImperativeHandle(ref, () => ({
    fillFromAI(data) {
      setDraft(current => {
        const next = { ...(current || {}) };

        if (data.title) next.Title = data.title;
        if (data.instructions) next.Instruction = data.instructions;
        if (!Array.isArray(data.questions)) return next;

        const { questions, instructions } = buildQuestionsFromAi(data.questions);

        // Carry over existing ids and point values by index so the backend
        // updates the current rows instead of inserting duplicates.
        const existing = current?.QuestionRequestModelList || [];
        questions.forEach((q, i) => {
          const prev = existing[i];
          if (!prev) return;

          const prevId = prev.QuizQuestionModel?.Id;
          if (prevId) q.QuizQuestionModel.Id = prevId;

          const prevValue = prev.QuizQuestionModel?.CustomValue;
          if (prevValue != null) q.QuizQuestionModel.CustomValue = prevValue;

          const prevResponses = prev.QuizQuestionResponseModelList || [];
          (q.QuizQuestionResponseModelList || []).forEach((resp, j) => {
            const prevRespId = prevResponses[j]?.Id;
            if (prevRespId) resp.Id = prevRespId;
          });
        });

        next.QuestionRequestModelList = questions;
        next.QuizMatchInstructionsModel = instructions;
        // Without this the quiz would keep its previous type and every
        // generated question would render with that single editor.
        next.QuizTypeId = resolveQuizTypeId(questions, next.QuizTypeId);
        return next;
      });
    },

    postponeAutoSave() {},

    getFormContext() {
      return buildAiContextFromDraft(draft);
    },
  }), [draft, setDraft]);

  const { hasChanges, commitSnapshot } = useUnsavedChanges(
    () => draft,
    normalizeHtml,
  );

  const { languages } = useContext(LocalizeContext);

  useVersionedTranslations();

  // check if the quiz is avaible for publishing
  useEffect(() => {
    if (draft?.Id && draft?.QuestionRequestModelList?.length > 0) {
      setPublished(true);
    } else {
      setPublished(false);
    }
  }, [draft]);

  const showMessage = messages => {
    Alert.info(messages.join('\n'), {
      position: 'top-right',
      effect: 'slide',
      timeout: 5000
    });
  };

  const publishQuiz = async () => {
    const { valid, messages } = validateQuiz(draft);
    if (!valid) {
      Alert.error(
        <>
          <b>{messages.length} <Translate id="quiz.warnings" />:</b>
          <ul>{messages.map((m, i) => <li key={i}>{m}</li>)}</ul>
        </>,
        { position: 'top-right', effect: 'slide', timeout: 10000 },
      );
      return;
    }
    try {
      await publishDraft().then(quizInfo => {
        commitSnapshot();
        setGoList(true);
        showMessage(quizInfo.Messages);
      });
    } catch {
      Alert.error('Failed to save the quiz. Please try again.');
    }
  };

const handleQuizChange = useCallback((changes) => {
  setDraft(prev => {
    let updated = { ...prev, ...changes };
    if ('QuizTypeId' in changes) {
      updated.QuestionRequestModelList = (updated.QuestionRequestModelList || []).map(q => ({
        ...q,
        QuizQuestionResponseModelList: [],
      }));
    }
    return updated;
  });
});

  const removePremise = (pid) => {
    setDraft(prev => {
      const remaining = prev.QuestionRequestModelList.filter(q => q.pid !== pid);
      
      // Drop match instructions whose last question just went away, so the
      // payload never carries a group with nothing in it.
      const groupsStillUsed = new Set(
        remaining
          .map(q => q.QuizQuestionModel?.QuizMatchInstructionId)
          .filter(Boolean)
      );

      return {
        ...prev,
        QuestionRequestModelList: remaining,
        QuizMatchInstructionsModel: (prev.QuizMatchInstructionsModel || [])
          .filter(instruction => groupsStillUsed.has(instruction.Id)),
      };
    });
  };

  /**
   * Folds what the server returned on save back into the draft.
   *
   * The quiz id and the attachments are not enough: the server also assigns an
   * id to every question and response it just created. Without carrying those
   * back, the next save posts the same questions with no id, and the update
   * treats anything it cannot match by id as deleted — wiping the questions of
   * a quiz that was only saved twice.
   *
   * Questions are paired by position, which is the order they were posted in.
   * The client-side QuizMatchInstructionId is kept: it groups match premises in
   * the editor, and the value the server sends back is its own.
   */
  const applySavedQuiz = (id, attachments, serverQuestions) => {
    setDraft(prev => {
      const current = prev?.QuestionRequestModelList || [];
      const fromServer = serverQuestions || [];

      const questions = current.map((currentQuestion, index) => {
        const serverQuestion = fromServer[index];
        if (!serverQuestion) return currentQuestion;

        const currentModel = currentQuestion.QuizQuestionModel || {};
        const serverModel = serverQuestion.QuizQuestionModel || {};

        // Only fill in the id of a brand-new question; an existing one keeps
        // whatever the editor holds.
        const model = (serverModel.Id && !currentModel.Id)
          ? { ...serverModel, QuizMatchInstructionId: currentModel.QuizMatchInstructionId }
          : currentModel;

        // Responses are deleted and recreated on every update, so their ids
        // change even when the text did not.
        const responses = (currentQuestion.QuizQuestionResponseModelList || []).map((currentResponse, responseIndex) => {
          const serverResponse = serverQuestion.QuizQuestionResponseModelList?.[responseIndex];
          if (!serverResponse) return currentResponse;
          if (currentResponse?.Id && currentResponse.Id === serverResponse.Id) return currentResponse;
          return serverResponse;
        });

        return { ...currentQuestion, QuizQuestionModel: model, QuizQuestionResponseModelList: responses };
      });

      return {
        ...prev,
        Id: id,
        QuizAttachmentModelList: attachments,
        QuestionRequestModelList: questions,
      };
    });
  };

  const handleBack = () => (hasChanges() ? setShowExit(true) : setGoList(true));
  if (goList) return <Redirect to="/assignments" />;
  if (loading || !draft) {
    return (
      <div style={{ position: 'relative' }}>
        <CircularProgress
          style={{
            position: 'absolute',
            right: '5px',
            top: '5px',
            zIndex: 1,
            width: '10px',
            height: '10px'
          }} />
      </div>
  )};  


  return (
    <>
      <Row>
        <Col xs={12}>
          <Row>
            <Col sm={12}>
              <button className="float-left btn-back" onClick={handleBack}>
                <FontAwesomeIcon icon={faArrowLeft} /> {LanguageManager.getTranslation("quiz.back", languages)}
              </button>

              { published ? (
              <button className="float-right btn-default button-primary-custom"
                      onClick={publishQuiz}>
                <Translate id="quiz.publish" />
              </button>
              ) : null}
            </Col>

            <Col className="premise-container">
              <QuizHeader
                Title={draft.Title}
                Instruction={draft.Instruction}
                Rubric={draft.Rubric}
                CustomValue={draft.CustomValue}
                QuizTypeId={draft.QuizTypeId}
                FilesCount={draft.FilesCount}
                onQuizChange={handleQuizChange}
                addQuizFile={setDraft}
                languages={languages}
                published={published}

              />
            </Col>
          </Row>

          <Row>
            <QuizQuestions
              questions={draft.QuestionRequestModelList}
              quizTypeId={draft.QuizTypeId}
              onUpdate={updateQuestion}
              setSelectedPremise={setSelectedPremise}
              setSelectedAnswer={setSelectedAnswer}
              onRemove={removePremise}
              languages={languages}
              setPublished={setPublished}
            />
          </Row>
        </Col>

        <Col xs={12} md={1}>
          <ToolBar 
            QuizRequestModel={draft}
            addPremise={addQuestion} 
            saveQuiz={saveDraft} 
            setQuizChanges={applySavedQuiz}
            onSuccessfulSave={commitSnapshot}
            showMessage={showMessage}
          />
        </Col>
      </Row>

      <ExitGuardModal
        visible={showExit}
        onConfirm={() => setGoList(true)}
        onCancel={() => setShowExit(false)}
      />
    </>
  );
});

export default Quiz;