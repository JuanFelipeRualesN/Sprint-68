import React, { useState } from 'react';
import { Link, Redirect } from "react-router-dom";
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle, faExclamationTriangle} from '@fortawesome/free-solid-svg-icons';

/**
 * Modal success for client registration
 */
const ModalSuccess = ({isEdit, orderNo, subscriptionId, userFullName, visible}) => {
  const [isEditState, setIsEditState] = useState(isEdit);
  const [orderNoState, setOrderNoState] = useState(orderNo);
  const [redirect, setRedirect] = useState(false);
  const [subscriptionIdState, setSubscriptionIdState] = useState(subscriptionId ?? '');
  const [visibleState, setVisibleState] = useState(visible);

  // Hide or shows the window
  const toggle = () => {
    setRedirect(true);
  };

  if (redirect && isEditState)
    return (
      <Redirect
        to={{
          pathname: "/add-child-profile",
          subscriptionId: subscriptionIdState
        }}
      />
    );

  if (redirect && !isEditState)
    return (
      <Redirect
        to={{
          pathname: "/set-up-child-profile",
          subscriptionId: subscriptionIdState
        }}
      />
    );

  return (
    <div>
      <Modal isOpen={visibleState} toggle={toggle}>
        <ModalHeader toggle={toggle} />
        <ModalBody>
          <h4>
            <FontAwesomeIcon
              className="green-text"
              icon={
                subscriptionIdState === 'undefined' ||
                subscriptionIdState === null ||
                subscriptionIdState === 'null'
                  ? faExclamationTriangle
                  : faCheckCircle
              }
            />{' '}
            {subscriptionIdState === 'undefined' ||
            subscriptionIdState === null ||
            subscriptionIdState === 'null' ? (
              <Translate id="modal-welcome.welcome-alert" options={{ renderInnerHtml: true }} />
            ) : (
              <Translate id="commercial.transaction-completed" options={{ renderInnerHtml: true }} />
            )}
          </h4>
          <br />
          <h3>
            {subscriptionIdState === 'undefined' ||
            subscriptionIdState === null ||
            subscriptionIdState === 'null' ? (
              <Translate id="modal-welcome.bank-declined" options={{ renderInnerHtml: true }} />
            ) : (
              <Translate id="commercial.your-order-was-confirmed" />
            )}
          </h3>
          {subscriptionIdState === 'undefined' ||
          subscriptionIdState === null ||
          subscriptionIdState === 'null' ? (
            {}
          ) : (
            <div>
              <h2 className="green-text centered-text">{orderNoState}</h2>
              <br />
              <h4>
                <Translate id="commercial.next-attach-the-profiles-to-the-subscription" />
              </h4>
            </div>
          )}
        </ModalBody>
        {subscriptionIdState === 'undefined' ||
        subscriptionIdState === null ||
        subscriptionIdState === 'null' ? (
          <ModalFooter>
            <div>
              <Link to="/subscription-add-new" className="btn-default button-primary-custom" onClick={toggle}>
                <Translate id="modal-welcome.go-to-select-plan" />
              </Link>
            </div>
          </ModalFooter>
        ) : (
          <ModalFooter>
            {isEditState ? (
              <div>
                <Link
                  to={{
                    pathname: "/add-child-profile",
                    subscriptionId: subscriptionIdState
                  }}
                  className="btn-default button-secondary-custom"
                  onClick={toggle}
                >
                  <Translate id="modal-welcome.go-to-add-new-user" />
                </Link>
              </div>
            ) : (
              <Link
                to={{
                  pathname: "/set-up-child-profile",
                  subscriptionId: subscriptionIdState
                }}
                className="btn-default button-secondary-custom"
                onClick={toggle}
              >
                <Translate id="modal-welcome.okay" />
              </Link>
            )}
          </ModalFooter>
        )}
      </Modal>
    </div>
  );
};

export default ModalSuccess;