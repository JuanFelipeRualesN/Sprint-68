import React, { useEffect, useState } from 'react';
import { Form, FormGroup, Input, Col, Row } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { withLocalize, LocalizeContext } from 'react-localize-redux';
import LanguageManager from '../../../custom-classes/language-manager';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

// component for true or false question
const TrueOrFalse = ({
    premise,
    published = false,
    onAnswersChange,
}) => {
    useVersionedTranslations();

    const pid = premise.pid;

  const { languages } = React.useContext(LocalizeContext);

  const trueLabel  = LanguageManager.getTranslation('quiz.answers-true-options',  languages);
  const falseLabel = LanguageManager.getTranslation('quiz.answers-false-options', languages);

    const localizeResponse = response => {
        if (['True', 'True/Cierto', trueLabel].includes(response)) return trueLabel;
        if (['False', 'False/Falso', falseLabel].includes(response)) return falseLabel;
        return response;
    };

    
    const defaultOptions = [
        { Response: trueLabel, Correct: '1', Value: '1' },
        { Response: falseLabel, Correct: '0', Value: '0' },
    ];

    const [options, setOptions] = useState(defaultOptions);
    const [selectedAnswer, setSelectedAnswer] = useState(trueLabel);

    useEffect(() => {
        const incoming = premise.QuizQuestionResponseModelList;
        if (Array.isArray(incoming) && incoming.length > 0) {
            const mapped = incoming.map(item => ({
                ...item,
                Response: localizeResponse(item.Response),
            }));
            setOptions(mapped);
            const correct = mapped.find(o => o.Correct === '1');
            setSelectedAnswer(correct?.Response || trueLabel);
        } else {
            setOptions(defaultOptions);
            setSelectedAnswer(trueLabel);
            onAnswersChange?.(pid, defaultOptions);
        }
    }, [premise.pid]);

    const handleAnswerChange = e => {
        const resp = e.currentTarget.value;
        const updated = options.map(opt => ({
            ...opt,
            Correct: opt.Response === resp ? '1' : '0',
            Value: opt.Response === resp ? '1' : '0',
        }));
        setSelectedAnswer(resp);
        setOptions(updated);
        onAnswersChange?.(pid, updated);
    };


    return (
        <Form>
            <Row>
                <Col xs="6">
                    <FormGroup>
                        <h5>
                            {LanguageManager.getTranslation("quiz.correct-answer", languages)}
                        </h5>
                        <Row>
                            <Col xs="12">
                                <Input
                                    type="select"
                                    name='select'
                                    id='exampleSelect'
                                    disabled={published}
                                    value={selectedAnswer}
                                    onChange={handleAnswerChange}
                                >
                                    <option value={trueLabel}>{trueLabel}</option>
                                    <option value={falseLabel}>{falseLabel}</option>
                                </Input>
                            </Col>
                        </Row>
                    </FormGroup>
                </Col>
            </Row>
        </Form>
    );
};



export default withLocalize(TrueOrFalse);