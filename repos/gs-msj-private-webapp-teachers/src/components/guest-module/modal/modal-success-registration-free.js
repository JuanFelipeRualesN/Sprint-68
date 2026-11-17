/**
 * Created by portiz on 04/30/20.
 */
import React, { useState } from 'react';
import { Link, Redirect } from "react-router-dom";
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle} from '@fortawesome/free-solid-svg-icons';

/**
 * Modal success for free registration
 */
const ModalSuccessRegistrationFree = (props) => {
    const [isEdit] = useState(props.isEdit);
    const [isParent] = useState(props.isParent);
    const [orderNo] = useState(props.orderNo);
    const [redirect, setRedirect] = useState(false);
    const [subscriptionId] = useState(props.subscriptionId ?? '');
    const [userFullName] = useState(props.userFullName ?? '' );
    const [visible] = useState(props.visible);

    // Hide or shows the window
    const toggle = () => {
        setRedirect(true);
    };

    if (redirect)
        return (<Redirect  to={{
            pathname: "/dashboard",
            subscriptionId: subscriptionId
        }}/>);

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={visible} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                <h2><FontAwesomeIcon className="green-text" icon={ faCheckCircle }/> {' '}
                    <Translate id="modal-welcome.welcome" options={{renderInnerHtml: true}}/>
                </h2>
                </ModalHeader>
                <ModalBody>
                    <p>
                        {!isParent ?
                            <Translate id="modal-welcome.teacher-next-steps" options={{renderInnerHtml: true}}/>
                            :
                            <Translate id="modal-welcome.parent-next-steps" options={{renderInnerHtml: true}}/>
                        }
                    </p>
                </ModalBody>
                    <ModalFooter>
                        <Link to="/dashboard" className="btn-default button-primary-custom" onClick={toggle}>
                            <Translate id="modal-welcome.go-to-dashboard" options={{renderInnerHtml: true}}/>
                        </Link>
                    </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalSuccessRegistrationFree;