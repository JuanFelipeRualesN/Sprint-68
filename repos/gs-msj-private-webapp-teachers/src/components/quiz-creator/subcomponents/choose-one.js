import React, { useState, useEffect, useRef, useCallback } from 'react';
import {
  Row, Col, Button, FormGroup, Label, Input
} from 'reactstrap';
import { Translate } from 'react-localize-redux';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import LanguageManager from '../../../custom-classes/language-manager';
import Editor from '../../common/editor';
import './premise.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import { Subject } from '../../../custom-classes/subject';
import { v4 as uuid } from 'uuid';


const attachmentTopic = 'quiz.attachments';

const ChooseOne =({
  premise,
  published = false,
  onAnswersChange,
  languages,
}) => {
  const subscriberIdRef = useRef(null);
  const editorRefs = useRef(new Map());
  const activeEditorIndexRef = useRef(null);
  const pid = premise.pid;
  const [answers, setAnswers] = useState([
    {
      _key: uuid(),
      Response: '',
      Correct: '1',
      Value: '1'
    },
    {
      _key: uuid(),
      Response: '',
      Correct: '0',
      Value: '0'
    }
  ]);
  const [selectedValue, setSelectedValue] = useState('0');

  useVersionedTranslations();

  // Store editor instance per index
  // Register or unregister editor node for this index
  const attachEditorRef = useCallback((key, node) => {
    if (node) {
      editorRefs.current.set(key, node);
    } else {
      editorRefs.current.delete(key);
    }
  }, []);

  // Focus handler that sets the active editor
  const handleFocusEditor = useCallback((key) => {
    activeEditorIndexRef.current = key;

    Subject.setActiveSubscriber({
      topic: attachmentTopic,
      subscriberId: subscriberIdRef.current,
    });
  }, []);

  useEffect(() => {
    // Subscribe once per component
    subscriberIdRef.current = Subject.subscribe({
      topic: attachmentTopic,
      callback: ({ type, url }) => {
        const key = activeEditorIndexRef.current;
        if (key == null) return;

        const editor = editorRefs.current.get(key);
        editor?.insertMedia(type, url);
      },
    });

    return () => {
      if (subscriberIdRef.current) {
        Subject.unsubscribe({
          topic: attachmentTopic,
          subscriberId: subscriberIdRef.current,
        });
      }
      // Clear local refs
      editorRefs.current.clear();
      activeEditorIndexRef.current = null;
    };
  }, []);

  useEffect(() => {
    if (
      premise?.QuizQuestionResponseModelList?.length > 0
    ) {
      const loaded = premise.QuizQuestionResponseModelList.map((ans) => ({
        _key: uuid(),
        ...ans,
      }));
      const correct = loaded.find(
        (a) => a.Correct === '1'
      );
      setAnswers(loaded);
      setSelectedValue(correct?.Value ?? '');
    } else {
      onAnswersChange( pid, answers);
    }
  }, [pid]);

  const handleAnswerChange = (value, key) => {
    const updated = answers.map((ans) =>
      ans._key === key ? { ...ans, Response: value } : ans
    );
    setAnswers(updated);
    onAnswersChange?.(pid, updated);
  };

  const handleCorrectChange = key => {
    const updated = answers.map((ans) =>
      ans._key === key
        ? { ...ans, Correct: '1', Value: '1' }
        : { ...ans, Correct: '0', Value: '0' }
    );
    setAnswers(updated);
    const selected = updated.find(a => a._key === key);
    setSelectedValue(selected.Value);
    onAnswersChange?.(pid, updated);
  };

  const handleRemoveOption = key => {
    const updated = answers.filter((ans) => ans._key !== key);
    setAnswers(updated);
    onAnswersChange?.(pid, updated);

    // If the removed option was focused, reset local active editor index
    if (activeEditorIndexRef.current === key) {
      activeEditorIndexRef.current = null;
    }
    editorRefs.current.delete(key)
   };

  const handleAddOption = () => {
    const updated = [
      ...answers,
      { _key: uuid(), Response: '', Correct: '0', Value: '0' },
    ];

    setAnswers(updated);
    onAnswersChange?.(pid, updated);
  };

  const placeholderOptions = LanguageManager.getTranslation("quiz.enter-option", languages);

  return (
    <div>
      <Row className="m-2">
        <Col xs="12">
          <h5>
            {LanguageManager.getTranslation("quiz.options", languages)}
          </h5>
        </Col>

        {answers.map((ans, index) => (
          <Col key={ans._key} xs="12" className="mb-3">
            <Row>
              <Col xs={12}>
                <Editor
                  key={`${ans._key}-${LanguageManager.getLanguage()}`}
                  ref={(node) => attachEditorRef(ans._key, node)}
                  defaultValue={ans.Response}
                  onChange={value => handleAnswerChange(value, ans._key)}
                  disabled={published}
                  placeholder={placeholderOptions}
                  onFocus={() => handleFocusEditor(ans._key)}
                />
              </Col>

              {!published && index > 1 && !ans.Id && (
                <Col xs="2" className="d-flex align-items-center">
                  <Button
                    color="danger"
                    onClick={() => handleRemoveOption(ans._key)}
                  >
                    <FontAwesomeIcon icon={faTrash} />
                  </Button>
                </Col>
              )}
            </Row>

            <Row className="mt-2">
              <Col>
                <FormGroup check>
                  <Label check htmlFor={`checkbox2-${ans._key}`}>
                    <Input
                      type="checkbox"
                      id={`checkbox2-${ans._key}`}
                      disabled={published}
                      checked={ans.Correct === '1'}
                      value={ans.Correct}
                      onChange={() => handleCorrectChange(ans._key)}
                    />{' '}
                    {LanguageManager.getTranslation("quiz.correct-answer", languages)}
                  </Label>
                </FormGroup>
              </Col>
            </Row>
          </Col>
        ))}

        {!published && (
          <Col xs="12">
            <Button color="primary" onClick={handleAddOption}>
              {LanguageManager.getTranslation("quiz.button-add-option", languages)}
            </Button>
          </Col>
        )}
      </Row>
    </div>
  );
}

export default ChooseOne;