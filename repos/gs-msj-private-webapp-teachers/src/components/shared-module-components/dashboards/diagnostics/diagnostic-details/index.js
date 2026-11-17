import React, { useState, useEffect, useMemo } from 'react';
import Select from 'react-select';
import { useDiagnostics } from '../helpers';
import QuestionSearchModal from '../question-search-modal';
import {
    Button,
    Form,
    FormGroup,
    Label,
    Alert,
    ListGroup,
    ListGroupItem,
    Row,
    Col,
    Container,
    Input,
    Card,
    CardBody
} from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faSave,
    faArrowLeft,
    faPlusSquare,
    faTrashAlt,
    faClipboardList,
    faExclamationTriangle // Added missing icon
} from '@fortawesome/free-solid-svg-icons';
import {Translate, withLocalize} from 'react-localize-redux';
import {formatQuestionSummary} from '../helpers';
import useVersionedTranslations from "../../../../../hooks/useVersionedTranslations";
import LanguageManager from "../../../../../custom-classes/language-manager";
import {createCustomReadingDiagnostic, updateCustomReadingDiagnostic} from "../../../../../api/reading-api";

const GRADE_OPTIONS = ['k', '1', '2', '3', '4', '5'].map(g => ({ value: g, label: g }));
const SUBJECT_OPTIONS = ['English', 'Español'].map(s => ({ value: s, label: s }));

function DiagnosticDetails({ diagnosticId, navigateToList }) {
    // We will no longer rely on availableQuestions for display, only saveDiagnostic
    const { getDiagnosticById, saveDiagnostic, setCurrentDiagnostic } = useDiagnostics();

    const initialData = diagnosticId === 'new' ?
        // ⭐️ FIX 1: Initialize questions as an array of objects, not just IDs.
        { title: '', grade: 'k', subject: 'English', published: false, questions: [] } :
        getDiagnosticById(diagnosticId) || null;

    const [diagnostic, setDiagnostic] = useState(initialData);
    const [isModalOpen, setIsModalOpen] = useState(false);
    // State to track if required fields are selected
    const [canAddQuestion, setCanAddQuestion] = useState(false);
    const [language, setLanguage] = useState(LanguageManager.getLanguage());

    useEffect(() => {
        if (initialData) {
            setDiagnostic(initialData);
        }
        return () => setCurrentDiagnostic(null);
    }, [diagnosticId]);

    useEffect(() => {
        setLanguage(LanguageManager.getLanguage());
    },[LanguageManager.getLanguage()]);

    useVersionedTranslations();

    // --- Validation Effect: Check if Subject and Grade are selected ---
    useEffect(() => {
        const requiredFieldsSelected = diagnostic &&
            diagnostic.grade && diagnostic.grade.trim() !== '' &&
            diagnostic.subject && diagnostic.subject.trim() !== '';

        // console.log("Diagnostic", diagnostic);
        setCanAddQuestion(!!requiredFieldsSelected);
    }, [diagnostic]);


    // --- Conditional Rendering for Initial State & Published Status ---
    if (!diagnostic && diagnosticId !== 'new') return <Alert color="danger"><Translate id="dashboards.diagnostic_not_found">Diagnostic data not found.</Translate></Alert>;
    if (!diagnostic) return null;

    if (diagnostic.published) {
        return (
            <Alert color="warning" className="d-flex align-items-center">
                <FontAwesomeIcon icon={faExclamationTriangle} size="2x" className="me-3" />
                <div>
                    <Translate id="dashboards.diagnostic_published">This diagnostic is Published and cannot be modified.</Translate>
                    <Button color="secondary" size="sm" className="ms-3" onClick={navigateToList}>
                        <FontAwesomeIcon icon={faArrowLeft} className="me-1" />{' '}
                        <Translate id="dashboards.back_to_list">Back to List</Translate>
                    </Button>
                </div>
            </Alert>
        );
    }


    // --- Handlers ---
    const handleSave = async () => {
        const messages = {
            missingFields: language === "en"
                ? "Please complete Title, Grade and Subject before saving."
                : "Por favor complete Título, Grado y Materia antes de guardar.",

            insufficientQuestions: language === "en"
                ? "You must select at least 3 questions before saving."
                : "Debe seleccionar al menos 3 preguntas antes de guardar.",

            saveSuccess: language === "en"
                ? "Diagnostic saved successfully!"
                : "¡Diagnóstico guardado exitosamente!",

            saveError: language === "en"
                ? "There was an error saving the diagnostic. Please try again."
                : "Ocurrió un error al guardar el diagnóstico. Por favor, inténtelo de nuevo."
        };
        // ----------------------------------

        // --- Validations ---
        if (!diagnostic.title || !diagnostic.grade || !diagnostic.subject) {
            alert(messages.missingFields);
            return;
        }

        if (!diagnostic.questions || diagnostic.questions.length < 3) {
            alert(messages.insufficientQuestions);
            return;
        }

        // --- Backend payload must be PascalCase ---
        const payload = {
            Title: diagnostic.title,
            SubjectCode: diagnostic.subject === "English" ? "en" : "sp",
            LevelCode: diagnostic.grade,
            Published: diagnostic.published ?? false,
            QuestionIds: diagnostic.questions.map(q => q.id)
        };

        try {
            if (diagnosticId === "new") {
                await createCustomReadingDiagnostic(payload);
            } else {
                await updateCustomReadingDiagnostic(Number(diagnosticId), payload);
            }

            alert(messages.saveSuccess);
            navigateToList();

        } catch (error) {
            console.error("Error saving diagnostic:", error);
            alert(messages.saveError);
        }
    };

    const handleQuestionAdd = (question) => {
        // ⭐️ FIX 2: Store the full question object (q) if its ID is not already present.
        if (!diagnostic.questions.some(q => q.id === question.id)) {
            setDiagnostic(prev => ({
                ...prev,
                questions: [...prev.questions, question] // Store the full object
            }));
        }
        setIsModalOpen(false);
    };

    const handleQuestionDelete = (questionId) => {
        // ⭐️ FIX 3: Filter the array of objects by the question's ID.
        setDiagnostic(prev => ({
            ...prev,
            questions: prev.questions.filter(q => q.id !== questionId)
        }));
    };

    // ⭐️ FIX 4: The selected questions array is now just the diagnostic.questions array itself.
    const selectedQuestions = diagnostic.questions;
    // console.log("Selected questions" , selectedQuestions);

    // Get an array of existing IDs to pass to the modal for checking duplicates
    const existingQuestionIds = useMemo(() =>
            diagnostic.questions.map(q => q.id)
        , [diagnostic.questions]);

    const getSelectValue = (options, value) => options.find(option => option.value === value) || null;

    return (
        <Container fluid="md">
            {/* ... Navigation Header (Back Button) ... */}
            <Row className="mb-4 pb-3 border-bottom">
                <Col className="d-flex align-items-center">
                    <Button color="secondary" size="sm" onClick={navigateToList}>
                        <FontAwesomeIcon icon={faArrowLeft} className="me-2" />{' '}
                        <Translate id="dashboards.back_to_list">Back to List</Translate>
                    </Button>
                </Col>
            </Row>

            {/* --- Diagnostic Details Form Card --- */}
            <Card className="mb-4 shadow-sm">
                <CardBody>
                    <Form>
                        <Row>
                            <Col md={6}>
                                <FormGroup>
                                    <Label for="title"><Translate id="dashboards.diagnostic_title">Diagnostic Title</Translate></Label>
                                    <Input
                                        type="text"
                                        id="title"
                                        value={diagnostic.title}
                                        onChange={(e) => setDiagnostic({...diagnostic, title: e.target.value})}
                                        required
                                    />
                                </FormGroup>
                            </Col>
                        </Row>
                        <Row>
                            {/* Grade Selection (react-select) */}
                            <Col md={6}>
                                <FormGroup>
                                    <Label for="grade"><Translate id="dashboards.grade_label">Grade (K-5)</Translate></Label>
                                    <Select
                                        id="grade"
                                        options={GRADE_OPTIONS}
                                        value={getSelectValue(GRADE_OPTIONS, diagnostic.grade)}
                                        onChange={(selectedOption) => setDiagnostic({...diagnostic, grade: selectedOption.value})}
                                        classNamePrefix="react-select"
                                    />
                                </FormGroup>
                            </Col>

                            {/* Subject Selection (react-select) */}
                            <Col md={6}>
                                <FormGroup>
                                    <Label for="subject"><Translate id="dashboards.subject_label">Subject</Translate></Label>
                                    <Select
                                        id="subject"
                                        options={SUBJECT_OPTIONS}
                                        value={getSelectValue(SUBJECT_OPTIONS, diagnostic.subject)}
                                        onChange={(selectedOption) => setDiagnostic({...diagnostic, subject: selectedOption.value})}
                                        classNamePrefix="react-select"
                                    />
                                </FormGroup>
                            </Col>
                        </Row>
                    </Form>
                </CardBody>
            </Card>

            {/* --- Action Button (Save) --- */}
            <Row className="mb-5">
                <Col>
                    <Button color="success" onClick={handleSave} className="px-4">
                        <FontAwesomeIcon icon={faSave} className="me-2" />
                        {' '}
                        <Translate id="dashboards.save_diagnostic">Save Diagnostic</Translate>
                    </Button>
                </Col>
            </Row>

            {/* --- Selected Questions Section --- */}
            <Card className="shadow-sm">
                <CardBody>
                    <Row className="align-items-center mb-4 border-bottom pb-3">
                        <Col xs={7}>
                            <h3 className="mb-0">
                                <FontAwesomeIcon icon={faClipboardList} className="me-2 text-primary" />
                                {' '}
                                <Translate id="dashboards.selected_questions">Selected Questions</Translate> ({selectedQuestions.length})
                            </h3>
                            <small className="text-muted"><Translate id="dashboards.must_select_3">Must select at least 3 questions.</Translate></small>
                        </Col>
                        <Col xs={5} className="text-end">
                            <Button
                                color="info"
                                onClick={() => setIsModalOpen(true)}
                                // Disable button if canAddQuestion is false
                                disabled={!canAddQuestion}
                            >
                                <FontAwesomeIcon icon={faPlusSquare} className="me-2" />
                                {' '}
                                <Translate id="dashboards.add_question">Add Question</Translate>
                            </Button>
                            {/* Tooltip/message if the button is disabled */}
                            {!canAddQuestion && (
                                <small className="d-block text-danger mt-1">
                                    <Translate id="dashboards.select_grade_subject">Select Grade and Subject first.</Translate>
                                </small>
                            )}
                        </Col>
                    </Row>

                    {selectedQuestions.length === 0 ? (
                        <Alert color="warning">
                            <Translate id="dashboards.no_questions_selected">No questions selected yet.</Translate>
                        </Alert>
                    ) : (
                        <ListGroup flush>
                            {/* Iterate over the full question objects in selectedQuestions */}
                            {selectedQuestions.map(q => (
                                <ListGroupItem key={q.id} className="d-flex justify-content-between align-items-center py-2">
                                    <span>
                                        <b>{q.standard}</b>: {' '} {formatQuestionSummary(q.title)}
                                    </span>
                                    <Button color="danger" size="sm" onClick={() => handleQuestionDelete(q.id)}>
                                        <FontAwesomeIcon icon={faTrashAlt} /> <Translate id="dashboards.remove">Remove</Translate>
                                    </Button>
                                </ListGroupItem>
                            ))}
                        </ListGroup>
                    )}
                </CardBody>
            </Card>

            <QuestionSearchModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSelectQuestion={handleQuestionAdd}
                // Pass only the IDs to the modal for comparison
                existingQuestionIds={existingQuestionIds}
                subjectFilter={diagnostic.subject}
                gradeFilter={diagnostic.grade}
            />
        </Container>
    );
}
export default withLocalize(DiagnosticDetails);