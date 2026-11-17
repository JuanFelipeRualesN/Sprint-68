/**
 * Created by portiz on 2/7/19.
 */
import React, { useState, useEffect } from 'react';
import {Input, Col, Row, Alert, Button} from 'reactstrap';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {cancelSubscription, getCancellationLogDetails} from "../../../api/subscriptions";
import Checkbox from '@material-ui/core/Checkbox';
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import SelectorCancellationReasons from '../selectors/selector-cancellation-reasons';
import { confirmUserSchool } from '../../../api/users';

/**
 * Class to handle the address form group
 */
const FormCancellationSubscription = ({cancelAtPeriodEnd: cancelAtPeriodEndProp, cancellationReason: cancellationReasonProp, disabled: disabledProp, 
                                        isEmployee: isEmployeeProp, subscriptionId: subscriptionIdProp}) => {
    const [cancelAtPeriodEnd, setCancelAtPeriodEnd] = useState(cancelAtPeriodEndProp || false);
    const [cancellationReason, setCancellationReason] = useState(cancellationReasonProp || '');
    const [disabled, setDisabled] = useState(disabledProp || null);
    const [isEmployee] = useState(isEmployeeProp || false);
    const [showCancelledDetails, setShowCancelledDetails] = useState(false);
    const [cancelledReason, setCancelledReason] = useState('');
    const [cancelledRequestDate, setCancelledRequestDate] = useState('');
    const [cancelledImmediately, setCancelledImmediately] = useState(false);
    const [cancelledReasonId, setCancelledReasonId] = useState(-1);
    const [cancellationReasonId, setCancellationReasonId] = useState(-1);
    const [propsModal, setPropsModal] = useState({ bodyText: '', visible: false });
    const [showError, setShowError] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [subscriptionId, setSubscriptionId] = useState(subscriptionIdProp || null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        setCancelAtPeriodEnd(cancelAtPeriodEndProp || false);
        setCancellationReason(cancellationReasonProp || '');
        setDisabled(disabledProp || null);
        setSubscriptionId(subscriptionIdProp || null);
        setShowCancelledDetails(false);
    }, [cancelAtPeriodEndProp, cancellationReasonProp, disabledProp, subscriptionIdProp]);

    useEffect(() => {
        // Verify
        /**
         * Edited by ET 2021-09-30 cancellation reasons
         */
        const apiCall = getCancellationLogDetails(subscriptionId);
        apiCall.then((response) => {
            if (response.data.length > 0 &&response.data[0].CancellationLogs !== null &&response.data[0].CancellationLogs.length > 0) {
            setShowCancelledDetails(true);
            setCancelledReason(response.data[0].CancellationLogs[0].Reason);
            setCancelledReasonId(response.data[0].CancellationLogs[0].CancellationReasonId);
            setCancelledRequestDate(response.data[0].CancellationLogs[0].CreatedAt);
            setCancelledImmediately(response.data[0].CancellationLogs[0].CancellationNow === '1');
            }
        });
    }, [subscriptionId]);

    const handleCheckChange = () => {
        setCancelAtPeriodEnd(!cancelAtPeriodEnd);
    };

    const handleInput = (e) => {
        setCancellationReason(e.currentTarget.value);
    };

    const setReasonId = (reasonId) => {
        setCancellationReasonId(reasonId);
    };

    const toggleAlertModal = () => {
        setShowModal(false);
    }

    const submitCancellation = async () => {
        let reason = String(cancellationReason).trim();
        if (cancellationReasonId === -1) {
            setShowError(true);
        } else {
            setLoading(true);
            try {
                const response = await cancelSubscription(
                    subscriptionId,
                    !cancelAtPeriodEnd, // To cancel immediately
                    reason,
                    cancellationReasonId
                );

                let messageToShow = '';
                for (let i = 0; i < response.data.Messages.length; i++) {
                    messageToShow += response.data.Messages[i] + '<br/>';
                }

                const modalProps = {
                    bodyText: messageToShow,
                    visible: true
                };

                setPropsModal(modalProps);
                setShowModal(true);

                if (response.status === 200 || response.status === 201) {
                    setDisabled(true);
                }
            } catch (error) {
                console.error(error);
            } finally {
                setLoading(false);
            }
        }
    };


    const renderCancelled = () => (
        <div>
            <Row>
                <Col><h2><Translate id="subscription-cancellation.title" options={{ renderInnerHtml: true }} /></h2></Col>
            </Row>
            <hr />
            <Row>
                <Col><small><Translate id="subscription-cancellation.cancellation-requested" options={{ renderInnerHtml: true }} /></small></Col>
            </Row>
            <Row>
                <Col><b><Translate id="subscription-cancellation.cancellation-reason" options={{ renderInnerHtml: true }} /></b></Col>
                <Col><SelectorCancellationReasons isDisabled={true} reasonId={cancelledReasonId} /></Col>
                <Col>{cancelledReason}</Col>
            </Row>
            <Row>
                <Col><b><Translate id="subscription-cancellation.cancellation-request-date" options={{ renderInnerHtml: true }} /></b></Col>
                <Col>{cancelledRequestDate}</Col>
            </Row>
            {!cancelledImmediately && (
                <Row>
                <Col>
                    <Alert color="warning">
                    <Translate id="subscription-cancellation.cancellation-will-be-soon" options={{ renderInnerHtml: true }} />
                    </Alert>
                </Col>
                </Row>
            )}
            {loading && <LoadingFullWindow />}
            {showModal && <ModalGeneric data={propsModal} updateParent={toggleAlertModal} />}
        </div>
    );

    const renderFormRequest = () => (
        <div>
            <Row>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <h2><Translate id="subscription-cancellation.title" options={{ renderInnerHtml: true }} /></h2>
                </Col>
                <hr />
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <small><Translate id="subscription-cancellation.cancel-immediately-instructions" options={{ renderInnerHtml: true }} /></small>
                </Col>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                <Checkbox
                    checked={cancelAtPeriodEnd}
                    onChange={handleCheckChange}
                    disabled={disabled}
                />
                <small><Translate id="subscription-cancellation.cancel-immediately" options={{ renderInnerHtml: true }} /></small>
                </Col>
                <hr />
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                <b><Translate id="subscription-cancellation.reasons" options={{ renderInnerHtml: true }} /></b><br />
                <small><Translate id="subscription-cancellation.reasons-instructions" options={{ renderInnerHtml: true }} /></small>
                </Col>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                <SelectorCancellationReasons setReasonId={setReasonId} />
                </Col>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                <Input
                    type="textarea"
                    value={cancellationReason}
                    disabled={disabled}
                    onChange={handleInput}
                />
                </Col>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                <small className="red-text">
                    <Alert color="warning">
                    <Translate id="subscription-cancellation.submit-note" options={{ renderInnerHtml: true }} />
                    </Alert>
                </small>
                </Col>
                {showError && (
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <Alert color="danger">
                    <Translate id="subscription-cancellation.cancellation-required" options={{ renderInnerHtml: true }} />
                    </Alert>
                </Col>
                )}
                <Col sm={{size: 7, offset: 5}} md={{size: 7, offset: 5}} lg={{size: 7, offset: 5}}>
                <button disabled={disabled} className="btn-default button-primary-custom" block onClick={submitCancellation}>
                    <Translate id="subscription-cancellation.submit" options={{ renderInnerHtml: true }} />
                </button>
                </Col>
            </Row>
            {loading && <LoadingFullWindow />}
            {showModal && <ModalGeneric data={propsModal} updateParent={toggleAlertModal} />}
        </div>
    );

    return <div>{showCancelledDetails ? renderCancelled() : renderFormRequest()}</div>;
};

export default FormCancellationSubscription;