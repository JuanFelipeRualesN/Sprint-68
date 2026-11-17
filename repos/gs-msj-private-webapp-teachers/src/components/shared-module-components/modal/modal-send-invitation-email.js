/**
 * Created by portiz 2020/05/21
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, Col, ModalFooter, Row } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import { sendConnectionRequestByEmail } from "../../../api/users";
import { BeatLoader } from "react-spinners";
import ProfilesDataTable from "../datatables/profiles-data-table";
import FormGroupEmail from "../form/form-group-email-v2";
/** End of Multiple Language imports **/

/**
 * Modal to send connection request
 */
const ModalSendInvitationEmail = ({visible, updateParent}) => {
  const [messageBackend, setMessageBackend] = useState(null);
  const [invitationCode, setInvitationCode] = useState('');
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);
  const [reload, setReload] = useState(false);
  const [showMessage, setShowMessage] = useState(false);
  const [studentId, setStudentId] = useState(null);
  const [studentName, setStudentName] = useState('');
  const [visibleState, setVisibleState] = useState(visible);
  const [submited, setSubmited] = useState(false);
  const [validEmail, setValidEmail] = useState(false);

//   const handleNameChange = (e) => {
//     setInvitationCode(e.currentTarget.value);
//   };

  const handleEmailGroupChange = (e) => {
    setEmail(e.email);
    setValidEmail(e.validEmail);
  };

  const setStudentDetails = (newValue) => {
    setStudentId(newValue.value);
    setStudentName(newValue.GivenName + ' ' + newValue.Surname);
  };

  const showStudentBox = () => {
    setStudentId(null);
  };

  //Hide or shows the window
  const toggle = () => {
    setVisibleState(!visibleState);
    // Updates the parent (send it owns to state to update it)
    const state = {
      messageBackend,
      invitationCode,
      email,
      loading,
      reload,
      showMessage,
      studentId,
      studentName,
      visible: !visibleState,
      submited,
      validEmail
    };
    updateParent(state, reload);
  };

  // Toggles when yes
  const toogleYes = () => {
    setLoading(true);
    setReload(false);
    setSubmited(true);
    // Archive
    let apiCall = sendConnectionRequestByEmail(studentId, email);

    apiCall.then((response) => {
      setLoading(true);
      setShowMessage(false);
      setMessageBackend(null);
      
      if (response.data.Messages != null && response.data.Messages.length > 0) {
        let message = '';
        for (let i = 0; i < response.data.Messages.length; i++) {
          message += response.data.Messages[i] + '<br/>';
        }
        setShowMessage(true);
        setMessageBackend(message);
        setReload(true);
        setLoading(false);
      }
    });
  };

  // Toggles when no
  const toogleNo = () => {
    setVisibleState(false);
    // Updates the parent (send it owns to state to update it)
    const state = {
      messageBackend,
      invitationCode,
      email,
      loading,
      reload,
      showMessage,
      studentId,
      studentName,
      visible: false,
      submited,
      validEmail
    };
    updateParent(state, reload);
  };

  // Render a generic modal. The body text should be sent in the text
  return (
    <div>
      <Modal isOpen={visibleState} toggle={toggle} className="modal-results">
        <ModalHeader toggle={toggle}>
          <Translate id="connections.send-invitation" options={{ renderInnerHtml: true }} />
        </ModalHeader>
        <ModalBody>
          <Row>
            <Col xs={{ size: 12 }} sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
              <h4>
                <Translate id="connections.select-student-step-1" options={{ renderInnerHtml: true }} />
              </h4>
            </Col>
            <hr />
            {studentId === null || studentId === '' ? (
              <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }}>
                <ProfilesDataTable
                  value={studentId}
                  updateParent={setStudentDetails}
                />
              </Col>
            ) : null}
          </Row>
          <Row>
            {typeof studentId !== 'undefined' && studentId !== null && studentId !== '' ? (
              <Col xs={12}>
                <Translate id="connections.selected-student" options={{ renderInnerHtml: false }} />
                : <span className="text-color-custom">
                  <b> {studentName} </b>
                </span>{' '}
                <button className="btn-default button-primary-custom" onClick={showStudentBox}>
                  <Translate id="connections.select-student" options={{ renderInnerHtml: false }} />
                </button>
              </Col>
            ) : null}
          </Row>
          {typeof studentId !== 'undefined' && studentId !== null && studentId !== '' ? (
            <Row>
              <Col xs={{ size: 12 }}>
                <h4>
                  <Translate id="connections.select-student-step-2" options={{ renderInnerHtml: true }} />
                </h4>
              </Col>
              <hr />
              <Col lg={{ size: 5 }} md={{ size: 5 }} sm={{ size: 12 }} xs={{ size: 12 }}>
                <FormGroupEmail isForPasswordReset={true} updateParent={handleEmailGroupChange} />
              </Col>
            </Row>
          ) : null}
        </ModalBody>
        <ModalFooter>
          <p>
            {showMessage ? (
              <span className="alert alert-info" dangerouslySetInnerHTML={{ __html: messageBackend }} />
            ) : null}
          </p>
          <p className="text-right">
            <button
              disabled={!validEmail || !studentId || submited}
              className="btn-default button-primary-custom"
              onClick={toogleYes}
            >
              {loading ? <BeatLoader size={10} color={"#fff"} /> : null}{' '}
              <Translate id="modal-invitation.send-invitation" options={{ renderInnerHtml: true }} />
            </button>{' '}
            <Button color="secondary" onClick={toogleNo}>
              <Translate id="modal-invitation.close" />
            </Button>
          </p>
        </ModalFooter>
      </Modal>
    </div>
  );
};

export default ModalSendInvitationEmail;