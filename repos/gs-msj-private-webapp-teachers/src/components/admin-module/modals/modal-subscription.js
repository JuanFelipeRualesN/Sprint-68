import React, { useState, createContext } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Translate } from "react-localize-redux";
import Wizard from '../views/subscriptions-wizard/wizard';
/**
 * Modal for update the subscription
 */
export const WizardContext = createContext({});

const ModalSubscription = ({ 
    isEdit, 
    isReadOnly, 
    planId, 
    subscriptionId, 
    userId, 
    visible, 
    updateParent, subscriptionType
}) => {
    const [isVisible, setIsVisible] = useState(visible);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent(newVisible);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results clear-paddingTable-responsive">
                <ModalHeader toggle={toggle}>
                </ModalHeader>
                <ModalBody>
                    <WizardContext.Provider value={{ data: { subscriptionId, userId, planId }, toggle }}>
                        <Wizard subscriptionType={subscriptionType} />
                    </WizardContext.Provider>
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close" />
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalSubscription;