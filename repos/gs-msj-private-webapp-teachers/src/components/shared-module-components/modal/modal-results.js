import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {Link} from "react-router-dom";
/** End of Multiple Language imports **/

/**
 * Generic modal. Set whatever you want to put in this message box.
 */
const ModalResults = ({lessonId, visible, coinsTotal, diamondsTotal, totalCorrect, totalQuestions, totalPercent, tryNo}) => {
  const [lessonIdState, setLessonIdState] = useState(lessonId);
  const [visibleState, setVisibleState] = useState(visible);
  const [coinsTotalState, setCoinsTotalState] = useState(parseInt(coinsTotal));
  const [diamondsTotalState, setDiamondsTotalState] = useState(parseInt(diamondsTotal));
  const [totalCorrectState, setTotalCorrectState] = useState(totalCorrect);
  const [totalQuestionsState, setTotalQuestionsState] = useState(totalQuestions);
  const [totalPercentState, setTotalPercentState] = useState(totalPercent);
  const [tryNoState, setTryNoState] = useState(tryNo);

  // Hide or shows the window
  const toggle = () => {
    setVisibleState(!visibleState);
  };

  // Render a generic modal. The body text should be sent in the text
  return (
    <div>
      <Modal style={{ zIndex: 99999 }} isOpen={visibleState} toggle={toggle}>
        <ModalHeader toggle={toggle}>
          <Translate id="student-courses.you-have-submitted-quiz" />
        </ModalHeader>
        <ModalBody className="text-center">
          <h1>
            {totalCorrectState} {'/'} {totalQuestionsState}
          </h1>
          <h2>{totalPercentState}{'%'}</h2>
          <p>
            <Translate id="student-courses.try" />
            {tryNoState}
          </p>
          {coinsTotalState > 0 ? (
            <div>
              <Translate id="student-courses.coins-won" /> {coinsTotalState}
            </div>
          ) : null}
          <br />
          {diamondsTotalState > 0 ? (
            <div>
              <Translate id="student-courses.diamond-won" /> {diamondsTotalState}
            </div>
          ) : null}
        </ModalBody>
        <ModalFooter>
          <Button color="info" onClick={toggle}>
            <Translate id="student-courses.see-my-responses" />
          </Button>
          <Link
            to={{
              pathname: '/lesson',
              lessonId: lessonIdState,
            }}
            className="btn-default button-primary-custom"
            onClick={toggle}
          >
            <Translate id="student-courses.back-to-lesson" />
          </Link>
        </ModalFooter>
      </Modal>
    </div>
  );
};

export default ModalResults;