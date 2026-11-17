import React, { useEffect, useRef, useState, useCallback } from 'react';
import { Card, Button, CardBody, Col, Row } from 'reactstrap';
import { Translate } from 'react-localize-redux';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import TrueOrFalse from './true-or-false';
import ChooseOne from './choose-one';
import Match from './match';

import LanguageManager from '../../../custom-classes/language-manager';
import Editor from '../../common/editor';
import { Subject } from '../../../custom-classes/subject';
import WrapperOverlappingMedia from '../../teacher-module/wrapper-overlapping-media';

import './premise.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const attachmentTopic = 'quiz.attachments';

export default function Premise({
  premise,
  index,
  quizTypeId,
  published = false,
  languages,
  onQuestionChange,
  onAnswersChange,
  onRemove, 
  onSelectAnswer,
}) {
  const editorRef = useRef(null);
  const subscriberIdRef = useRef(null);
  const [focused, setFocused] = useState(false);

  useVersionedTranslations();

  useEffect(() => {
    subscriberIdRef.current = Subject.subscribe({
      topic: attachmentTopic,
      callback: ({ type, url }) => editorRef.current?.insertMedia(type, url),
    });

    return () => {
      Subject.unsubscribe({
        topic: attachmentTopic,
        subscriberId: subscriberIdRef.current,
      });
    };
  }, []);

  const handleFocusEditor = useCallback(() => {
    Subject.setActiveSubscriber({
      topic: attachmentTopic,
      subscriberId: subscriberIdRef.current,
    });
  }, []);

  const renderAnswers = () => {
    const pid = premise?.pid;

    // The question's own type decides the editor, which is what lets one quiz
    // mix selection, match, open and true/false.
    //
    // Falls back to the quiz-level type for every question saved before this
    // change: quiz_questions.question_type_id was never populated, so existing
    // assignments carry their type only on the quiz row.
    const effectiveTypeId = premise?.QuizQuestionModel?.QuestionTypeId ?? quizTypeId;

    switch (parseInt(effectiveTypeId, 10)) {
      case 4: 
        return (
          <TrueOrFalse
            premise={premise}
            onAnswersChange={(pidChild, answers) => onAnswersChange?.(pidChild, answers)}
          />
        );

      case 2: 
        return (
          <Match
            premise={premise}
            onAnswersChange={(pidChild, answers) => onAnswersChange?.(pidChild, answers)}
            onSelectAnswer={({ pid, idx }) => onSelectAnswer?.(pid, idx)}
          />
        );

      case 1: 
      default:
        return (
          <ChooseOne
            premise={premise}
            onAnswersChange={(pidChild, answers) =>
              onAnswersChange?.(pidChild, answers)
            }
            languages={languages}
          />
        );
      case 3:
        break;
    }
  };

  const pid = premise?.pid;

  return (
    <Card
      className={`m-2 ${focused ? 'selected-premise' : ''}`}
      onFocus={() => setFocused(true)}
      onBlur={() => setFocused(false)}
    >
      <CardBody>
        {(!published || (published && !premise.QuizQuestionModel.Id)) && (
          <Button
            className="position-absolute"
            style={{ right: '28px', zIndex: 99 }}
            color="danger"
            onClick={() => onRemove?.(pid)}
          >
            <FontAwesomeIcon icon={faTrash} />
          </Button>
        )}

        <Row>
          <Col className="m-2">
            <h5>
              {LanguageManager.getTranslation("quiz.premise", languages)}
              {' '}
              {index + 1}
            </h5>

            <WrapperOverlappingMedia>
              <Editor
                key={`${premise?.pid}-${LanguageManager.getLanguage()}`}
                ref={editorRef}
                defaultValue={premise?.QuizQuestionModel?.Desc || ''}
                onChange={(value) => onQuestionChange(pid, value)}
                placeholder={LanguageManager.getTranslation('quiz.question', languages)}
                onFocus={handleFocusEditor}
              />
            </WrapperOverlappingMedia>
          </Col>
        </Row>

        <Row>
          <Col className="m-2">{renderAnswers()}</Col>
        </Row>
      </CardBody>
    </Card>
  );
}
