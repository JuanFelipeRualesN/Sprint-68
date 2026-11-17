import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody, Row, Col } from 'reactstrap';
import { Translate } from "react-localize-redux";
import PlansDataTable from '../datatables/plan-data-table';

/**
 * Model that handles the clients
 */
const ModalPlanSelection = ({ visible, updateParent, isCommercial }) => {
    const [isVisible, setIsVisible] = useState(visible);

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(!isVisible);
        updateParent(null);
    };

    const handleSelected = (e) => {
        updateParent(e);
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results clear-paddingTable-responsive">
                <ModalHeader toggle={toggle}>
                    <Translate id="plan.plan-search"/>
                    <br/>
                    <small style={{fontSize: 14}}>
                        <Translate id="plan.plan-search-selection-instructions"/>
                    </small>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                            <PlansDataTable updateParent={handleSelected} isCommercial={isCommercial}/>
                        </Col>
                    </Row>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default ModalPlanSelection;