/**
 * Created by portiz on 2/7/19.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalBody, ModalFooter} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import FormCancellationSubscription from '../forms/form-cancel-subscription';

/**
 * Modal to cancel the subscription
 */
const ModalCancellationSubscription = ({ 
    cancelAtPeriodEnd, 
    disabled, 
    isEmployee, 
    subscriptionId, 
    visible, 
    updateParent 
}) => {
    const [isVisible, setIsVisible] = useState(visible);
    const [cancelAtPeriodEndState] = useState(cancelAtPeriodEnd ? cancelAtPeriodEnd : false);
    const [isEmployeeState] = useState(isEmployee ? isEmployee : false);
    const [subscriptionIdState] = useState(subscriptionId ? subscriptionId : null);
    
    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent(newVisible);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalBody>
                    <FormCancellationSubscription
                        disabled={disabled}
                        cancelAtPeriodEnd={cancelAtPeriodEndState}
                        subscriptionId={subscriptionIdState}
                        isEmployee={isEmployeeState}
                    />
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="subscription-cancellation.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalCancellationSubscription;