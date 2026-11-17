import React, {useMemo, useState} from "react";
import {Button, CardBody, ListGroup, ListGroupItem} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChevronRight} from "@fortawesome/free-solid-svg-icons";
import {StudentResultsModal} from "../student-results-modal";
import { Translate } from "react-localize-redux";

export const StudentExamResultsList = ({ examsData, selectedStudent, selectedFocusAreas, startDate, endDate }) => {
    const [modalOpen, setModalOpen] = useState(false);
    const [currentExam, setCurrentExam] = useState(null);
    const [currentStudentName, setCurrentStudentName] = useState('');

    const toggleModal = () => setModalOpen(!modalOpen);

    const handleViewResults = (studentName, exam) => {
        setCurrentStudentName(studentName);
        setCurrentExam(exam);
        toggleModal();
    };

    const filteredExams = useMemo(() => {
        let results = [];
        const isStudentSelected = selectedStudent && (Array.isArray(selectedStudent) ? selectedStudent.length > 0 : true);

        if (!isStudentSelected) {
            results = examsData.flatMap(student =>
                student.exams.map(exam => ({
                    studentName: student.name,
                    ...exam
                }))
            );
            results = results.filter(item => item.score > 0);
        } else {
            const studentsToFilter = Array.isArray(selectedStudent) 
                ? selectedStudent 
                : [selectedStudent];

            const selectedStudentsData = examsData.filter(student => studentsToFilter.includes(student.name));

            results = selectedStudentsData.flatMap(student => 
                student.exams.map(exam => ({
                    studentName: student.name,
                    ...exam
                }))
            );
        }

        if (selectedFocusAreas && selectedFocusAreas.length > 0) {
            results = results.filter(item => 
                selectedFocusAreas.includes(item.focusArea) 
            );
        }

        const start = startDate ? new Date(startDate) : null;
        const end = endDate ? new Date(endDate) : null;

        if (start || end) {
            results = results.filter(item => {
                const examCompletedDate = item.completedDate; 

                if (!examCompletedDate || item.completedDate === 'N/A') return false; 

                const examDate = new Date(examCompletedDate);

                let passesStart = true;
                let passesEnd = true;

                if (start) {
                    passesStart = examDate.getTime() >= start.getTime(); 
                }
                if (end) {
                    const endOfDay = new Date(end);
                    endOfDay.setDate(endOfDay.getDate() + 1); 
                    
                    passesEnd = examDate.getTime() < endOfDay.getTime();
                }

                return passesStart && passesEnd;
            });
        }

        return results;
    }, [examsData, selectedStudent, selectedFocusAreas]);

    return (
        <CardBody className="p-0">
            <div className="p-3 mb-0 text-center title-secondary font-weight-bold border-bottom">
                <Translate id="dashboards.recent-exams-results" />
            </div>
            <div style={{ maxHeight: '250px', overflowY: 'auto' }}>
                <ListGroup flush>
                    {filteredExams.length > 0 ? (
                        filteredExams.map((item, index) => (
                            <ListGroupItem
                                key={`${item.studentName}-${item.examName}-${index}`}
                                className={`d-flex justify-content-between align-items-center py-2 px-3 list-group-item-action ${index % 2 === 0 ? 'bg-white' : 'bg-light'}`}
                            >
                                <div style={{ fontSize: '0.9rem', textAlign: 'left', flexGrow: 1 }}>
                                    <strong className="d-block text-dark">{item.studentName}</strong>
                                    <small className="text-muted d-block">{item.examName}</small>
                                    <div className="mt-1">
                                        <small className="d-block text-secondary">
                                            <strong>
                                                <Translate id="dashboards.topic" />:</strong> {item.topic}
                                        </small>
                                        <small className="d-block text-secondary">
                                            <strong>
                                                <Translate id="dashboards.standard" />:</strong> {item.standardCode} | <strong><Translate id="dashboards.language" />:</strong> {item.language.toUpperCase()} | <strong><Translate id="dashboards.category" />:</strong> {item.category}
                                        </small>
                                        <small className="d-block text-secondary">
                                            <strong>
                                                <Translate id="dashboards.completed" />:</strong> {item.completedDate}
                                        </small>
                                    </div>
                                </div>
                                <div className="text-right d-flex align-items-center">
                                    <span className={`font-weight-bold mr-3 text-${item.score >= 70 ? 'success' : 'danger'}`}>
                                        {item.score}%
                                    </span>
                                    <Button
                                        size="sm"
                                        color="primary"
                                        outline
                                        onClick={() => handleViewResults(item.studentName, item)}
                                    >
                                        <Translate id="dashboards.view" />
                                        <FontAwesomeIcon icon={faChevronRight} className="ml-1" size="xs" />
                                    </Button>
                                </div>
                            </ListGroupItem>
                        ))
                    ) : (
                        <p className="p-3 text-center text-muted m-0">
                            {selectedStudent && selectedStudent.length > 0
                                ? `No hay resultados de exámenes para el/los estudiante(s) seleccionado(s).`
                                : 'No hay resultados de exámenes disponibles.'}
                        </p>
                    )}
                </ListGroup>
            </div>

            {currentExam && (
                <StudentResultsModal
                    modal={modalOpen}
                    toggle={toggleModal}
                    studentName={currentStudentName}
                    exam={currentExam}
                />
            )}
        </CardBody>
    );
};