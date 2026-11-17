import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Translate } from "react-localize-redux";
import LoaderFullWindow from "../loader-windows/loader-full-window";
import {
    faPrint,
    faEye,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { getUserQuizDetails } from "../../../api/gsweb";
import ResultsToHTMLConverter from "../../../custom-classes/results-html-converter";
import Settings from "../../../custom-classes/settings";

const ModalQuizResults = ({studentId, usquiUserResultId, visible, updateParent}) => {
  const [studentIdState, setStudentIdState] = useState(studentId);
  const [usquiUserResultIdState, setUsquiUserResultIdState] = useState(usquiUserResultId);
  const [visibleState, setVisibleState] = useState(visible);
  const [loading, setLoading] = useState(false);
  const [examInfo, setExamInfo] = useState(null);
  const [languages, setLanguages] = useState(Settings.LANGUAGES);
  const [pdfLocation, setPdfLocation] = useState(null);

  useEffect(() => {
    loadQuiz();
  }, []);

  const loadQuiz = async () => {
    setLoading(true);
    try {
      const response = await getUserQuizDetails(usquiUserResultIdState, studentIdState);
      const resultsHtml = ResultsToHTMLConverter.getHtmlResults(response.data.UserQuizFullDetails, response.data.StudentName, languages, true);
      setExamInfo(resultsHtml);
      setPdfLocation(response.data.PDFLocation);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };


  // Hide or shows the window
  const toggle = () => {
    setVisibleState(!visibleState);
    // Updates the parent (send it owns to state to update it)
    updateParent(!visibleState);
  };

  // Render a generic modal. The body text should be sent in the text
  return (
    <>
      {examInfo !== null ? (
        <Modal isOpen={visibleState} toggle={toggle} className="modal-results modal-progress-results">
          <ModalHeader className="card-white" toggle={toggle}>
            <Translate id="report-student-average.results" options={{ renderInnerHtml: false }} />{' '}
            {examInfo !== null && examInfo.PDFLocation !== null ? (
              <a
                href={pdfLocation}
                target="_blank"
                download={pdfLocation}
                className="btn btn-primary"
              >
                <FontAwesomeIcon icon={faPrint} />{' '}
                <Translate id="report-student-average.print" options={{ renderInnerHtml: false }} />
              </a>
            ) : null}
          </ModalHeader>
          <ModalBody className="card-white">
            {examInfo !== null ? (
              <div dangerouslySetInnerHTML={{ __html: examInfo }} />
            ) : null}
          </ModalBody>
          <ModalFooter className="card-white"></ModalFooter>
        </Modal>
      ) : null}
      {loading ? <LoaderFullWindow /> : null}
    </>
  );
};


export default ModalQuizResults;