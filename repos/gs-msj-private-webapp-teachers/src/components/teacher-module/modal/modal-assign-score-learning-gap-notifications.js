import React, {useState, useEffect} from 'react'
import { Modal, ModalBody, ModalHeader, ModalFooter, Button, Input } from 'reactstrap'
import { postLearningGapCreateOrUpdateDiagnosticScoreManually } from '../../../api/teachers';
import { Translate } from 'react-localize-redux'
import { faPen} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PulseLoader } from 'react-spinners';
import Settings from '../../../custom-classes/settings';
import LearningGapEditableScoreTable from '../learning-gap/learning-gap-progress/learning-gap-editable-score-table.js/learning-gap-editable-score-table'

// Modal to assign scores to students for diagnostic tests or lessons quizzes from notifications section
const AssignScoreModal = ({isOpen, handleClose, studentId, studentName, selectedItem, quizResults, diagnosticResults, fetchNotifications}) => {
  const [smallLoading, setSmallLoading] = useState(false)
  const [initialScore, setInitialScore] = useState('');
  const [finalScore, setFinalScore] = useState('');
  const [hasSubmitted, setHasSubmitted] = useState(false);
  const isInitialScoreEmpty = initialScore === '';
  const isDiagnosticTest = selectedItem?.NotificationType === "diagnostic";

  // Check the current selected diagnostic test
  const currentDiagnosticResult = diagnosticResults?.find(
    result => result?.Id == selectedItem?.DiagnosticTestId
  );
  
  // Assign the initial and final scores from the selected diagnostic test
  useEffect(() => {
    if (currentDiagnosticResult) {
      setInitialScore(currentDiagnosticResult.InitialDiagnosticScore || '');
      setFinalScore(currentDiagnosticResult.FinalDiagnosticScore || '');
    }
  }, [currentDiagnosticResult]);

  // Check if the initial and final scores have changed to enable/disable the submit button
  const hasScoreChanged = () => {
    const originalInitialScore = currentDiagnosticResult?.InitialDiagnosticScore || '';
    const originalFinalScore = currentDiagnosticResult?.FinalDiagnosticScore || '';
    
    return (initialScore !== originalInitialScore) || (finalScore !== originalFinalScore);
  };

  const isSubmitDisabled = smallLoading || isInitialScoreEmpty || !hasScoreChanged();

  // Get the submit status from lessons results modal to refresh the notifications
  const setHasSubmittedState = (state) => {
    setHasSubmitted(state);
  }
 
  // Function to change the score of a diagnostic test
  const handleUpdatedScore = async () => {
    setSmallLoading(true)
    const learningGapDiagnosticTestModel = {
      Id: selectedItem?.DiagnosticTestId,
      InitialDiagnosticScore: initialScore,
      FinalDiagnosticScore: finalScore,
    }
    try {
      await postLearningGapCreateOrUpdateDiagnosticScoreManually(studentId, learningGapDiagnosticTestModel)
    } catch (error) {
      console.error("Error updating lesson request:", error)
    } finally {
      setSmallLoading(false)
      handleClose()
      fetchNotifications()
    }
  }

  // Fetch notifications after closing lessons results modal
  const closeAndCallNotifications = () => {
    handleClose()
    if (hasSubmitted) {
      fetchNotifications()
    }
    setHasSubmitted(false)    
  }

  // Validate score input to allow only numbers, two decimals and a maximum of 100
  const validateScoreInput = (value) => {
    const numericRegex = Settings.getRegexScore();
    if (numericRegex.test(value)) {
      if (value === "" || parseFloat(value) <= 100) {
        return true;
      }
    }
    return false;
  };

  // Handle initial score change
  const handleInitialScoreChange = (e) => {
    const newValue = e.target.value;
    if (validateScoreInput(newValue)) {
      setInitialScore(newValue);
    }
  };
  
  // Handle final score change
  const handleFinalScoreChange = (e) => {
    const newValue = e.target.value;
    if (validateScoreInput(newValue)) {
      setFinalScore(newValue);
    }
  };

  return ( !isDiagnosticTest ? 
    <Modal 
      isOpen={isOpen}
      toggle={closeAndCallNotifications}
      centered
      className="learning-gap-student-quiz-grades-modal"
    >
      <ModalHeader toggle={closeAndCallNotifications} >
        <b><Translate id="learning-gap.score-lesson" options={{renderInnerHtml: true}}/></b>
      </ModalHeader>
      <ModalBody>
        <div className="learning-gap-quiz-results-info-area">
          <span className="learning-gap-quiz-results-info-area-title">
            <Translate id="learning-gap.student" options={{renderInnerHtml: true}}/>: {studentName}
          </span>
          <br/>
          <span className="learning-gap-quiz-results-info-area-subtitle">
            <Translate id="learning-gap.lesson" options={{renderInnerHtml: true}}/>: {selectedItem?.LessonTitle}
          </span>
        </div>
  
        <div className="learning-gap-quiz-results-score-area">
          <LearningGapEditableScoreTable
            learningGapAssignedLessonId={selectedItem?.AssignedLessonId}
            data={quizResults}
            studentId={studentId}
            setHasSubmittedState={setHasSubmittedState}  
          />
        </div>
      </ModalBody>
    </Modal> 
    : 
    <Modal
      isOpen={isOpen}
      size='lg'
      toggle={handleClose}     
      centered
    >
      <ModalHeader className="justify-content-center">
        <div style={{color: '#2a426d'}}>
          <FontAwesomeIcon icon={faPen} className="mr-2" />
          <b>
            <Translate id="progress.assign-score" options={{renderInnerHtml: true}}/>
          </b>
        </div>
      </ModalHeader>
      <ModalBody>
        <span>
          <Translate id="learning-gap.assign-score-for-diagnostic" options={{renderInnerHtml: true}}/>
        </span>
        {" "}
        <b>{selectedItem?.LessonTitle}</b>
        {" "}
        <span>
          <Translate id="learning-gap.for-the-student" options={{renderInnerHtml: true}}/>
        </span>
        {" "}
        <b>{studentName}</b>
        <div className='mt-4' style={{display: 'flex', alignItems: 'center', justifyContent: 'space-between'}}>
          <b>
            <Translate id="learning-gap.initial-diagnostic-score" options={{renderInnerHtml: true}}/>
          </b>
          <div className="form-group-fields" style={{margin: '0'}}>
            <Input 
              type="text"
              value={initialScore}
              onChange={handleInitialScoreChange}
            />
          </div>
        </div>

        <div className='mt-4' style={{display: 'flex', alignItems: 'center', justifyContent: 'space-between'}}>
          <b>
            <Translate id="learning-gap.final-diagnostic-score" options={{renderInnerHtml: true}}/>
          </b>
          <div className="form-group-fields" style={{margin: '0'}}>
            <Input 
              type="text"
              value={finalScore}
              onChange={handleFinalScoreChange}
            />
          </div>
        </div>
      </ModalBody>
      <ModalFooter className="justify-content-center mb-2">
        <Button color='primary'
          className='btn-reply-submit-notifications'
          onClick={handleUpdatedScore}
          disabled={isSubmitDisabled}
        >
          {smallLoading ? (
            <PulseLoader size={10} color="#395a8f" />) 
            : (
            <>
              <Translate id="learning-gap.submit"/>
            </>
            )
          }
        </Button>
        <Button className="learning-gap-case-none"
          color="secondary"
          onClick={handleClose}
          disabled={smallLoading}
        >
          {smallLoading ? (
            <PulseLoader size={10} color="#395a8f" />) 
            : (
            <>
              <Translate id="learning-gap.cancel"/>
            </>
            )
          }
        </Button>
      </ModalFooter>
    </Modal>
  )
}

export default AssignScoreModal