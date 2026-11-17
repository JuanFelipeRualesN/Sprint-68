import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalFooter, ModalBody } from 'reactstrap';
import { putLearningGapAssignLessonToStudent, postLearningGapDiagnosticTestDates } from '../../../api/teachers';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendarAlt, faExclamationTriangle } from '@fortawesome/free-solid-svg-icons';
import { Translate } from 'react-localize-redux';
import { PulseLoader } from 'react-spinners';
import LocalizedDatePicker from '../../../utils/datepicker-locale';
import 'react-dates/lib/css/_datepicker.css';

// Modal for changing the limit date of a lesson or diagnostic test in learning gap notifications section
const ChangeLimitDateModal = ({ isOpen, handleClose, selectedItem, studentName, diagnosticResults, fetchNotifications }) => {
  const [smallLoading, setSmallLoading] = useState(false);
  const [selectedLessonDate, setSelectedLessonDate] = useState(null);
  const [selectedInitialDate, setSelectedInitialDate] = useState(null);
  const [selectedFinalDate, setSelectedFinalDate] = useState(null);
  const [diagnosticInitialDateChanged, setDiagnosticInitialDateChanged] = useState(false);
  const [diagnosticFinalDateChanged, setDiagnosticFinalDateChanged] = useState(false);
  const [hasLessonDateChanged, setHasLessonDateChanged] = useState(false);
  const [hasDiagnosticDateChanged, setHasDiagnosticDateChanged] = useState(false);
  const [showDateError, setShowDateError] = useState(false);

  // Check if the selected item is a diagnostic test
  const isDiagnosticTest = selectedItem?.NotificationType === "diagnostic";
  
  // Get the current diagnostic result
  const currentDiagnosticResult = diagnosticResults?.find(
    result => result?.Id == selectedItem?.DiagnosticTestId
  );
 
  // Minimum date is tomorrow
  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);

  useEffect(() => {
    const { initialDate, finalDate, lessonDate } = getOriginalDates();

    if (isDiagnosticTest) {
      setSelectedInitialDate(initialDate);
      setSelectedFinalDate(finalDate);
    } else {
      setSelectedLessonDate(lessonDate);
    }
  }, [selectedItem, currentDiagnosticResult, isDiagnosticTest]);

  // Function to get the original dates for the lesson or diagnostic test from the response
  const getOriginalDates = () => {
    if (isDiagnosticTest && currentDiagnosticResult) {
      let initialDate;
      let finalDate;
      
      if (currentDiagnosticResult?.InitialDiagnosticDateLimit) {
        initialDate = new Date(currentDiagnosticResult.InitialDiagnosticDateLimit);
      }
      if (currentDiagnosticResult?.FinalDiagnosticDateLimit) {
        finalDate = new Date(currentDiagnosticResult.FinalDiagnosticDateLimit);
      }
      
      return { initialDate, finalDate };
    } else if (selectedItem) {
      let lessonDate = null;
      
      if (selectedItem?.LimitDate) {
        lessonDate = new Date(selectedItem.LimitDate);
      }
      
      return { lessonDate };
    }

    return { initialDate: null, finalDate: null, lessonDate: null };
  };

  // Handler to change the lesson date
  const handleLessonDateChange = (date) => {
    setSelectedLessonDate(date);
    if (selectedItem?.LimitDate) {
      const originalDate = new Date(selectedItem.LimitDate);
      const newDate = new Date(date);
      setHasLessonDateChanged(originalDate.getTime() !== newDate.getTime());
    }
  };

  // Handler to change the initial date for diagnostic tests
  const handleInitialDiagnosticDateChange = (date) => {
    const hasError = selectedFinalDate && date >= selectedFinalDate;
    setShowDateError(hasError);

    setSelectedInitialDate(date);
    const originalDate = new Date(currentDiagnosticResult?.InitialDiagnosticDateLimit);
    const newDate = new Date(date);
    const initialDateChanged = originalDate.getTime() !== newDate.getTime();
    setDiagnosticInitialDateChanged(initialDateChanged);
    setHasDiagnosticDateChanged(initialDateChanged || diagnosticFinalDateChanged);
  };
  
  // Handler to change the final date for diagnostic tests
  const handleFinalDiagnosticDateChange = (date) => {
    const hasError = selectedInitialDate && date <= selectedInitialDate;
    setShowDateError(hasError);
  
    setSelectedFinalDate(date);
    if (currentDiagnosticResult?.FinalDiagnosticDateLimit) {
      const originalDate = new Date(currentDiagnosticResult.FinalDiagnosticDateLimit);
      const newDate = new Date(date);
      const finalDateChanged = originalDate.getTime() !== newDate.getTime();
      setDiagnosticFinalDateChanged(finalDateChanged);
      setHasDiagnosticDateChanged(diagnosticInitialDateChanged || finalDateChanged);
    }
  };

  // Submit lesson date changes
  const handleDateChangeLesson = async () => {
    setSmallLoading(true);
    try {
      const jsDate = new Date(selectedLessonDate);
      const isoString = jsDate.toISOString();
      
      await putLearningGapAssignLessonToStudent(
        selectedItem?.AssignedLessonId,
        isoString,
        selectedItem?.LessonId
      );
    } catch (error) {
      console.error("Error updating lesson date", error);
    } finally {
      setSmallLoading(false);
      handleClose();
      fetchNotifications();
    }
  };

  // Submit diagnostic test dates changes
  const handleDateChangeDiagnostic = async () => {
    setSmallLoading(true);
    try {
      const studentsIdsToSubmit = [selectedItem?.StudentId];
      const groupsIdsToSubmit = [];
      const subjectCode = currentDiagnosticResult?.SubjectCode;
      const selectedLevel = currentDiagnosticResult?.LevelCode;
      
      const jsDateInitialDate = new Date(selectedInitialDate);
      const isoStringInitialDate = jsDateInitialDate.toISOString();
      
      const jsDateFinalDate = new Date(selectedFinalDate);
      const isoStringFinalDate = jsDateFinalDate.toISOString();

      const id = currentDiagnosticResult?.Id;
      
      await postLearningGapDiagnosticTestDates(
        id,
        studentsIdsToSubmit, 
        groupsIdsToSubmit, 
        subjectCode, 
        selectedLevel, 
        isoStringInitialDate, 
        isoStringFinalDate,
        diagnosticInitialDateChanged,
        diagnosticFinalDateChanged
      );
    } catch (error) {
      console.error("Error updating diagnostic test dates", error);
    } finally {
      setSmallLoading(false);
      handleClose();
      fetchNotifications();
    }
  };

  // Close the modal and reset states
  const handleModalClose = () => {
    setDiagnosticInitialDateChanged(false);
    setDiagnosticFinalDateChanged(false);
    setHasLessonDateChanged(false);
    setHasDiagnosticDateChanged(false);

    // Reset dates when closing the modal
    const { initialDate, finalDate, lessonDate } = getOriginalDates();

    if (isDiagnosticTest) {
      setSelectedInitialDate(initialDate);
      setSelectedFinalDate(finalDate);
    } else {
      setSelectedLessonDate(lessonDate);
    }

    handleClose();
  };

  return (
    <Modal
      size="lg"
      isOpen={isOpen}
      centered
      toggle={handleModalClose}
    >
      <ModalBody>
        <div className="text-center" style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#2a426d', fontWeight: 'bold', fontSize: '20px' }}>
          <FontAwesomeIcon size="lg" icon={faCalendarAlt} className="mr-4" />
          <span>
            <Translate id="learning-gap.change-limit-date" options={{renderInnerHtml: true}}/>
          </span>
        </div>

        <div className='mt-4'>
          <span>
            {isDiagnosticTest ? 
              <Translate id="learning-gap.change-limit-date-for-diagnostic" options={{renderInnerHtml: true}}/> 
              :
              <Translate id="learning-gap.change-limit-date-for-phase" options={{renderInnerHtml: true}}/>
            }  
          </span>
          {" "}
          {selectedItem?.LessonTitle && <span><b>{selectedItem.LessonTitle}</b></span>}
          {" "}
          <span>
            <Translate id="learning-gap.for-the-student" options={{renderInnerHtml: true}}/>
          </span>
          {" "}
          <b style={{fontWeight: 'bold'}}>{studentName}</b>
        </div>

        {isDiagnosticTest ? (
          <>
            <div className='mt-4' style={{display: 'flex', alignItems: 'center', justifyContent: 'space-between'}}>
              <span>
                <Translate id="learning-gap.initial-date-diagnostic-test" options={{renderInnerHtml: true}}/>
              </span>
              <div>
                <LocalizedDatePicker
                  className='form-control'
                  selected={selectedInitialDate}
                  minDate={tomorrow}
                  onChange={handleInitialDiagnosticDateChange}
                  dateFormat="yyyy/MM/dd"
                  onChangeRaw={(e) => {
                    e.preventDefault();
                  }}
                />
              </div>
            </div>
            <div className='mt-4' style={{display: 'flex', alignItems: 'center', justifyContent: 'space-between'}}>
              <span>
                <Translate id="learning-gap.final-date-diagnostic-test" options={{renderInnerHtml: true}}/>
              </span>
              <div>
                <LocalizedDatePicker
                  className='form-control'
                  selected={selectedFinalDate}
                  minDate={tomorrow}
                  onChange={handleFinalDiagnosticDateChange}
                  dateFormat="yyyy/MM/dd"
                  onChangeRaw={(e) => {
                    e.preventDefault();
                  }}
                />
              </div>
            </div>
          </>
        ) : (
          <div className='mt-4' style={{display: 'flex', alignItems: 'center', justifyContent: 'space-between'}}>
            <span>
              <Translate id="learning-gap.limit-date" options={{renderInnerHtml: true}}/>
            </span>
            <div>
              <LocalizedDatePicker
                className='form-control'
                selected={selectedLessonDate}
                minDate={tomorrow}
                onChange={handleLessonDateChange}
                dateFormat="yyyy/MM/dd"
                onChangeRaw={(e) => {
                  e.preventDefault();
                }}
              />
            </div>
          </div>
        )}
        {showDateError && (
          <div className="text-danger mt-2 text-center" style={{ color: 'red', fontSize: '14px', fontWeight: 'bold' }}>
            <FontAwesomeIcon icon={faExclamationTriangle} style={{ color: 'orange', marginRight: '5px' }} />
            <Translate id="learning-gap.date-error-message"/>
          </div>
        )}
      </ModalBody>   
      <ModalFooter className="justify-content-center mb-2 mt-0">
        <Button 
          color='primary'
          className='btn-reply-submit-notifications'
          onClick={isDiagnosticTest ? handleDateChangeDiagnostic : handleDateChangeLesson}
          disabled={smallLoading || (isDiagnosticTest ? (!hasDiagnosticDateChanged || !selectedInitialDate || !selectedFinalDate || showDateError) : !hasLessonDateChanged)}
          >
          {smallLoading ? (
            <PulseLoader size={10} color="#395a8f" />
          ) : (
            <Translate id="learning-gap.submit"/>
          )}
        </Button>
        <Button 
          className="learning-gap-case-none"
          color="secondary"
          onClick={handleModalClose}
          disabled={smallLoading}
        >
          {smallLoading ? (
            <PulseLoader size={10} color="#395a8f" />
          ) : (
            <Translate id="learning-gap.cancel"/>
          )}
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default ChangeLimitDateModal;