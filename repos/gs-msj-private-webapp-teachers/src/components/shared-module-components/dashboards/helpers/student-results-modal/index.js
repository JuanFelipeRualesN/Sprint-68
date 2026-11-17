import React from "react";
import {Button, ListGroup, ListGroupItem, Modal, ModalBody, ModalFooter, ModalHeader} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPrint} from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import jsPDF from 'jspdf';
import 'jspdf-autotable';

export const StudentResultsModal = ({ modal, toggle, studentName, exam }) => {
    const handleGeneratePdf = () => {
        if (!exam) return;

        const doc = new jsPDF();
        const safeExamName = exam.examName.replace(/[^a-z0-9]/gi, '_');
        const fileName = `Resultado_Examen_${studentName.replace(/\s/g, '_')}_${safeExamName}.pdf`;

        doc.setFontSize(14);
        doc.text(`Reporte de Resultados: ${exam.examName}`, 14, 20);

        doc.setFontSize(12);
        doc.text(`Estudiante: ${studentName}`, 14, 30);
        doc.text(`Puntuación Obtenida: ${exam.score}%`, 14, 38);
        doc.text(`Tema: ${exam.topic}`, 14, 46);
        doc.text(`Fecha de Finalización: ${exam.completedDate}`, 14, 54);
        
        const keyAreasData = [
            { 
                area: 'Comprensión de Vocabulario', 
                score: `${Math.min(95, exam.score + 7)}%` 
            },
            { 
                area: 'Identificación Silábica', 
                score: `${Math.max(40, exam.score - 10)}%` 
            },
            { 
                area: 'Fluidez de Lectura', 
                score: `${Math.min(99, exam.score + 5)}%` 
            },
        ];
        const bodyData = keyAreasData.map(item => [
            item.area, 
            item.score
        ]);

        doc.autoTable({
            startY: 65,
            head: [['Área de Desempeño', 'Puntuación']],
            body: bodyData,
            styles: { fontSize: 10, cellPadding: 3 },
            headStyles: { fillColor: [41, 128, 185], textColor: 255 },
            columnStyles: {
                0: { cellWidth: 70 },
                1: { cellWidth: 30, halign: 'center' }
            },
            didDrawPage: function (data) {
                doc.setFontSize(10);
                doc.text("Reporte generado por la plataforma", data.settings.margin.left, doc.internal.pageSize.getHeight() - 10);
            }
        });

        doc.save(fileName);
    };

    if (!exam) return null;

    return (
        <Modal isOpen={modal} toggle={toggle} size="lg">
            <ModalHeader toggle={toggle} className="text-white">
                <Translate id="dashboards.exams-results" />: {studentName}
            </ModalHeader>
            <ModalBody>
                <h5 className="mb-3 title-secondary">{exam.examName}</h5>
                <p><strong><Translate id="dashboards.score-obtained" />:</strong> <span className={`font-weight-bold text-${exam.score >= 70 ? 'success' : 'danger'}`}>{exam.score}%</span></p>
                <p><strong><Translate id="dashboards.topic-detail" />:</strong> {exam.topic}</p>

                <h6 className="mt-4"><Translate id="dashboards.key-areas-performance" />:</h6>
                <ListGroup flush>
                    <ListGroupItem className="d-flex justify-content-between">
                        <Translate id="dashboards.vocabulary-comprehension" /> <span>{Math.min(95, exam.score + 7)}%</span>
                    </ListGroupItem>
                    <ListGroupItem className="d-flex justify-content-between">
                        <Translate id="dashboards.syllable-identification" /> <span>{Math.max(40, exam.score - 10)}%</span>
                    </ListGroupItem>
                    <ListGroupItem className="d-flex justify-content-between">
                        <Translate id="dashboards.reading-fluency" /> <span>{Math.min(99, exam.score + 5)}%</span>
                    </ListGroupItem>
                </ListGroup>
            </ModalBody>
            <ModalFooter>
                <button className="btn-default button-secondary-custom" onClick={handleGeneratePdf}>
                    <FontAwesomeIcon icon={faPrint} className="mr-1" />
                    <Translate id="dashboards.print-pdf" />
                </button>
                <button className="btn-default" onClick={toggle}><Translate id="dashboards.close" /></button>
            </ModalFooter>
        </Modal>
    );
};