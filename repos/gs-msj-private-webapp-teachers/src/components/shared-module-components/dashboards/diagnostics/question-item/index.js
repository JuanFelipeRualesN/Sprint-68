// QuestionItem.js

import React, { useState } from 'react';
import {
    Button,
    Accordion,
    AccordionItem,
    AccordionHeader,
    AccordionBody,
    Col,
    Row
} from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faCheck,
    faChevronDown, // For closed state
    faChevronUp,   // For open state
    faPlus
} from '@fortawesome/free-solid-svg-icons';
import { Translate } from 'react-localize-redux';
import {formatQuestionSummary} from '../helpers';

// Icon for the correct answer
const CheckIcon = () => (
    <FontAwesomeIcon icon={faCheck} style={{ color: 'green', marginLeft: '5px' }} />
);

export default function QuestionItem({ question, onSelect, isSelected }) {
    if (!question || !question.id) {
        console.error("QuestionItem received invalid data and was prevented from crashing:", question);
        return null;
    }

    const [openItemId, setOpenItemId] = useState(null);

    const toggleAccordion = (id) => {
        setOpenItemId(openItemId === id ? null : id);
    };

    const isOpen = openItemId === question.id;

    // Custom toggle icon component
    const ToggleIcon = () => (
        <FontAwesomeIcon
            icon={isOpen ? faChevronUp : faChevronDown}
            style={{ transition: 'transform 0.3s' }}
        />
    );

    // Generate the summary for the header
    const questionSummary = formatQuestionSummary(question.text);

    return (
        <Accordion open={openItemId} toggle={toggleAccordion} stayOpen>
            <AccordionItem>
                <AccordionHeader
                    targetId={question.id}
                >
                    <Row className="w-100 m-0 align-items-center"> {/* Added w-100 m-0 to ensure full width */}
                        <Col xs={11}>
                            {/* Content Wrapper */}
                            <div className="d-flex align-items-center">
                                <strong className="me-2 text-primary">{question.standard || 'N/A'}</strong>
                                {' '}
                                <span className="flex-grow-1 text-truncate me-2">
                                    {questionSummary || 'Untitled Question'}
                                </span>
                            </div>
                        </Col>
                        <Col xs={1} className="d-flex justify-content-end">
                            {/* Icon Container: Aligned to the right */}
                            <ToggleIcon />
                        </Col>
                    </Row>
                </AccordionHeader>

                <AccordionBody accordionId={question.id}>
                    {/* Render Question Text */}
                    <p>
                        <strong>
                            <Translate id="dashboards.question_text">Question Text:</Translate>
                        </strong>
                        <span
                            dangerouslySetInnerHTML={{ __html: question.text || '' }}
                            style={{ marginLeft: '5px', display: 'block' }}
                        />
                    </p>

                    <h5 className="mt-3">
                        <Translate id="dashboards.answers">Answers:</Translate>
                    </h5>
                    <ul style={{ listStyleType: 'none', paddingLeft: 0 }}>
                        {/* Ensure question.answers is an array before mapping */}
                        {(question.answers || []).map((answer, index) => (
                            <li key={index} className="mb-1 d-flex align-items-center">
                                {answer && answer.isCorrect ? (
                                    <CheckIcon />
                                ) : (
                                    <span style={{ visibility: 'hidden', width: '20px' }}></span>
                                )}

                                <span
                                    dangerouslySetInnerHTML={{ __html: (answer && answer.text) || '' }}
                                    style={{ marginLeft: '5px' }}
                                />
                            </li>
                        ))}
                    </ul>

                    <Button
                        color={isSelected ? "secondary" : "primary"}
                        onClick={onSelect}
                        disabled={isSelected}
                        className="mt-3"
                    >
                        <FontAwesomeIcon icon={faPlus} className="me-2" />{' '}
                        {isSelected ? (
                            <Translate id="dashboards.selected">Selected</Translate>
                        ) : (
                            <Translate id="dashboards.select_question">Select Question</Translate>
                        )}
                    </Button>
                </AccordionBody>
            </AccordionItem>
        </Accordion>
    );
}