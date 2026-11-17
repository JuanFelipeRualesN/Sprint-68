/**
 * Created by portiz on 2/7/19.
 */
import React, { useState, useEffect } from 'react';
import {Button, Modal, ModalBody, ModalFooter, Row, Col, ModalHeader} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import EntitySchoolDataTable from '../../datatables/school-entity-data-table';
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
/**
 * Modal to save school entities relation only
 */
const ModalSchoolEntityFilter = ({ 
    entityId, isDelete = false, entityRelationId, entityTypeId, visibleRelationEntity, updateParent 
}) => {
    const [disabledSave, setDisabledSave] = useState(false);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [relationEntityId, setRelationEntityId] = useState(entityRelationId ?? null);
    const [relationEntityName, setRelationEntityName] = useState('');
    const [transactionValid, setTransactionValid] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);

    useEffect(() => {
        setMessage('');
    }, []);

    // Hide or shows the window
    const toggle = () => {
        updateParent(null, false);
    };

    const handleEntitySelection = (e) => {
        setRelationEntityId(e.EntityId);
        setRelationEntityName(e.EntityName);
        updateParent(e, false);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal className="modal-results" isOpen={visibleRelationEntity} toggle={toggle}>
                <ModalHeader toggle={toggle}/>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                           <EntitySchoolDataTable
                                    entityType={entityTypeId}
                                    redoSearch={redoSearch}
                                    updateParent={handleEntitySelection}/>
                        </Col>
                    </Row>
                </ModalBody>
                <ModalFooter>
                    <Row>
                        <Col xs={12}>
                            <Button color="secondary" onClick={toggle}>
                                <Translate id="school-module.close"/>
                            </Button>
                        </Col>
                    </Row>
                </ModalFooter>
            </Modal>
            {loading && <LoadingFullWindow/>}
        </div>
    );
};

export default ModalSchoolEntityFilter;