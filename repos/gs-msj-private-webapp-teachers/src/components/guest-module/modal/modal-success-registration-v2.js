import React, { useState } from 'react';
import { Link, Redirect } from "react-router-dom";
import {Modal, ModalHeader, ModalBody, ModalFooter, Col, Row} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle, faExclamationTriangle} from '@fortawesome/free-solid-svg-icons';

/**
 * Modal success for client registration version 2
 */
const ModalSuccessRegistrationV2 = ({ isEdit, orderNo, subscriptionId, userFullName, visible }) => {
    const [redirect, setRedirect] = useState(false);
    const [isEditState] = useState(isEdit);
    const [orderNoState] = useState(orderNo);
    const [userFullNameState] = useState(userFullName ?? '');
    const [visibleState] = useState(visible);

    const toggle = () => {
        setRedirect(true);
    };

    if (redirect && isEditState) {
        return (
            <Redirect to={{
                pathname: "/add-child-profile",
                search: `?subscriptionId=${subscriptionId}`
            }} />
        );
    }

    if (redirect && !isEditState) {
        return (
            <Redirect to={{
                pathname: "/register-v2",
                search: `?subscriptionId=${subscriptionId}`
            }} />
        );
    }

    const isSubscriptionInvalid = subscriptionId === 'undefined' ||
        subscriptionId === null ||
        subscriptionId === 'null';

    return (
        <div>
            <Modal isOpen={visibleState} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <h2 className="titles-general"><FontAwesomeIcon className="green-text" icon={
                        isSubscriptionInvalid ? faExclamationTriangle : faCheckCircle
                    }/> {' '}

                        {
                            isSubscriptionInvalid ?
                                <Translate id="modal-welcome.welcome-alert"
                                           options={{renderInnerHtml: true}}/> :
                                <Translate id="commercial.welcome" options={{renderInnerHtml: true}}/>
                        }
                    </h2>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col>
                            {
                                isSubscriptionInvalid ?
                                    <Translate id="modal-welcome.bank-declined" options={{renderInnerHtml: true}}/> :
                                    <Translate id="commercial.your-confirmation-number-is"/>
                            }
                        </Col>
                    </Row>
                    <Row>
                    <Col>
                            {
                                isSubscriptionInvalid ?
                                    null :
                                    <div>
                                        <span className="green-text centered-text">
                                            {orderNoState}
                                        </span>
                                        <br/>
                                        <span><Translate id="commercial.next-steps-explanation"/></span>
                                    </div>
                            }
                        </Col>
                    </Row>

                </ModalBody>
                {
                    isSubscriptionInvalid ?
                        <ModalFooter>
                            <div>
                                <Link to="/subscription-add-new" className="btn-default button-primary-custom" onClick={toggle}>
                                    <Translate id="modal-welcome.go-to-select-plan"/>
                                </Link>
                            </div>
                        </ModalFooter>
                        :

                        <ModalFooter>
                            {
                                isEditState ?
                                    <div>
                                        <Link to={{
                                            pathname: "/add-child-profile",
                                            search: `?subscriptionId=${subscriptionId}`
                                        }} className="btn-default button-secondary-custom" onClick={toggle}>

                                            <Translate id="modal-welcome.go-to-add-new-user"/>
                                        </Link>
                                    </div> :
                                    <Link to={{
                                        pathname: "/register-v2",
                                        search: `?subscriptionId=${subscriptionId}`
                                    }} className="btn-default button-secondary-custom" onClick={toggle}>
                                       
                                        <Translate id="modal-welcome.okay"/>
                                    </Link>
                            }
                        </ModalFooter>
                }
            </Modal>
        </div>
    );
};

export default ModalSuccessRegistrationV2;