import React, { useEffect, useState, useContext, useCallback, useRef } from 'react';
import { Form, FormGroup, Col, Row } from 'reactstrap';
import { Translate, withLocalize, LocalizeContext } from 'react-localize-redux';
import Editor from '../../common/editor';
import LanguageManager from '../../../custom-classes/language-manager';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import { Subject } from '../../../custom-classes/subject';

const attachmentTopic = 'quiz.attachments';

const Match = ({
    premise,
    disabled = false,
    onAnswersChange,
    onSelectAnswer,
}) => {
    useVersionedTranslations();
    const { languages } = useContext(LocalizeContext);

    const pid = premise?.pid;
    const lastSentRef = useRef('');
    const editorRef = useRef(null);
    const subscriberIdRef = useRef(null);

    const sig = (arr)=> JSON.stringify(arr.map(a=>({
    Response: a?.Response ?? '', Correct: a?.Correct ?? '1', Value: a?.Value ?? '1'
    })));

    const [answers, setAnswers] = useState([
        { Response: '', Correct: '1', Value: '0' },
    ]);
    const [value, setValue] = useState('0');

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

    useEffect(() => {
        const list = premise?.QuizQuestionResponseModelList;
        if (Array.isArray(list) && list.length > 0) {
            setAnswers(list);
            setValue(list[0]?.Value ?? '0');
        } else {
            setAnswers([{ Response: '', Correct: '1', Value: '0' }]);
            setValue('0');
        }
    }, [pid]);

    const handleAnswerChange = (correctAnswer) => {
        const updated = answers.map(a => ({ ...a, Value: '1', Response: correctAnswer }));
        const next = sig(updated);
        if (next === lastSentRef.current) return;
        lastSentRef.current = next;

        setAnswers(updated);
        setValue('1');
        onAnswersChange?.(pid, updated);
    }

    const handleFocusEditor = useCallback(() => {
        Subject.setActiveSubscriber({
        topic: attachmentTopic,
        subscriberId: subscriberIdRef.current,
        });
    }, []);


    return (
        <Form>
            <Row>
                <Col xs="12">
                    <FormGroup>
                        <Row>
                            <Col xs="12" className="m-2">
                                <h5>
                                    {LanguageManager.getTranslation("quiz.mantch-option", languages)}
                                </h5>
                            </Col>

                            {answers.map((answer, index) => (
                                <Col key={answer?.pid ?? `${pid}-${index}`} xs="12" className="m-2">
                                    <Editor
                                        key={`${answer?.pid}-${LanguageManager.getLanguage()}`}                                        ref={editorRef}
                                        defaultValue={answer?.Response ?? ''}
                                        onChange={handleAnswerChange}
                                        disabled={disabled}
                                        placeholder={`${LanguageManager.getTranslation('quiz.enter-option', languages)} ${index + 1}`}
                                        onFocus={handleFocusEditor}
                                    />
                                </Col>
                            ))}
                        </Row>
                    </FormGroup>
                </Col>
            </Row>
        </Form>
    );
};

export default withLocalize(Match);
