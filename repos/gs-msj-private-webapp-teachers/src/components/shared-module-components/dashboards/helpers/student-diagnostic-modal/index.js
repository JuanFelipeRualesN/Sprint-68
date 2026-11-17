import React from 'react';
import {Modal, ModalBody, ModalFooter, ModalHeader, Table} from "reactstrap";
import { Translate } from 'react-localize-redux';
import { diagnosticResultsMockedData } from '../data';

const StudentDiagnosticModal = ({modalOpen, toggle, selectedStudents}) => {
  const studentsToFilter = Array.isArray(selectedStudents) ? selectedStudents : [selectedStudents].filter(Boolean);

  let filteredDiagnostics = [];

  if (studentsToFilter.length > 0) {
    filteredDiagnostics = diagnosticResultsMockedData.filter(
        (result) => studentsToFilter.includes(result.studentName)
    );
  }

  return (
    <Modal isOpen={modalOpen} toggle={toggle} size="lg">
      <ModalHeader className="text-white">
        <Translate id ="dashboards.diagnostic-results" />:
        <span className="font-weight-bold ml-1"> 
          {selectedStudents.length === 1 ? `${selectedStudents[0]}` : `${selectedStudents.length} estudiantes seleccionados`}
        </span>
      </ModalHeader>
      <ModalBody>
        {filteredDiagnostics.length > 0 ? (
          <Table bordered striped size="sm" className="mb-0">
            <thead className="bg-light">
              <tr>
                {selectedStudents.length > 1 && 
                  <th>
                    <Translate id="dashboards.student" />
                  </th>
                } 
                <th>
                  <Translate id="dashboards.subject" />
                </th>
                <th>
                  <Translate id="dashboards.level" />
                </th>
                <th className="text-right">
                  <Translate id="dashboards.score" />
                </th>
              </tr>
            </thead>
            <tbody>
              {filteredDiagnostics.map((diag, index) => (
                <tr key={index}>
                  {selectedStudents.length > 1 && <td>{diag.studentName}</td>}
                  <td>{diag.subject === 'en' ? 'Inglés' : 'Español'}</td>
                  <td>{diag.level}</td>
                  <td className="text-right">
                    <span className={`font-weight-bold text-${diag.score >= 70 ? 'success' : 'danger'}`}>
                        {diag.score}%
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>) : null}
      </ModalBody>
      <ModalFooter>
        <button className="btn-default button-primary-custom" onClick={toggle}>
          <Translate id="dashboards.close" />
        </button>
      </ModalFooter>
    </Modal>
  );
}

export default StudentDiagnosticModal;