/**
 * Created by portiz on 1/31/20.
 */

import React, { useState, useEffect } from 'react';
import {deleteSchoolEntityRelation, addSchoolEntityRelation} from "../../../../api/schools";
import { Button, Modal, ModalBody, ModalFooter, Row, Col} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import EntitySchoolDataTable from '../../datatables/school-entity-data-table';
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
/**
 * Modal to save school entities relation only
 */
const ModalSchoolEntity = ({ 
    entityId, isDelete = false, entityRelationId, entityTypeId, visibleRelationEntity, updateParent 
}) => {
    const [disabledSave, setDisabledSave] = useState(false);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [relationEntityId, setRelationEntityId] = useState(entityRelationId ?? null);
    const [relationEntityName, setRelationEntityName] = useState('');
    const [transactionValid, setTransactionValid] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [visibleRelationEntityState, setVisibleRelationEntityState] = useState(visibleRelationEntity);


    useEffect(() => {
        setMessage('');
    }, []);

    // Hide or shows the window
    const toggle = () => {
        setVisibleRelationEntityState(!visibleRelationEntityState);
        setDisabledSave(false);
        updateParent(true);
    };

    const handleEntitySelection = (e) => {
        setRelationEntityId(e.EntityId);
        setRelationEntityName(e.EntityName);
    };

    const handleEntityRelatedNoSelected = () => {
        setRelationEntityId(null);
        setRelationEntityName(null);
    };

    const saveRelation = () => {
        setLoading(true);
        setMessage('');

        let apiCalls = addSchoolEntityRelation(
            entityId, entityTypeId, relationEntityId);

        apiCalls.then((response) => {
            let messageStr = '';
            for (let i = 0; i < response.data.Messages.length; i++) {
                messageStr += response.data.Messages[i] + ' ';
            }
            if (response.status === 200 || response.status === 201) {
                setDisabledSave(true);
            }
            setMessage(messageStr);
            setLoading(false);
        });
    };

    const deleteRelation = () => {
        setLoading(true);
        setMessage('');

        let apiCalls = deleteSchoolEntityRelation(
            relationEntityId, entityId);

        apiCalls.then((response) => {
            let messageStr = '';
            for (let i = 0; i < response.data.Messages.length; i++) {
                messageStr += response.data.Messages[i] + ' ';
            }
            if (response.status === 200 || response.status === 201) {
                setDisabledSave(true);
            }
            setMessage(messageStr);
            setLoading(false);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal className="modal-results" isOpen={visibleRelationEntityState} toggle={toggle}>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                            {
                                isDelete ?
                                    <div>
                                        <Translate id="school-module.delete-relation"/>
                                        <p>
                                            {message}
                                        </p>
                                        <p>
                                            {disabledSave === false ?
                                                <div>
                                            <Button className="btn-yesno" color="success" onClick={deleteRelation}>
                                                <Translate id="school-module.yes"/>
                                            </Button>
                                            {' '}
                                            <Button className="btn-yesno" color="primary" onClick={toggle}>
                                                <Translate id="school-module.no" />
                                            </Button> </div>: null
                                            }
                                        </p>
                                    </div> : null
                            }
                            {relationEntityId === null &&
                            (typeof isDelete === 'undefined' || isDelete === false) ?
                            <EntitySchoolDataTable
                                entityType={entityTypeId}
                                redoSearch={redoSearch}
                                updateParent={handleEntitySelection}/> : null
                            }
                            {
                                relationEntityId !== null &&
                                (typeof isDelete === 'undefined' || isDelete === false)
                                    ?
                                    <div>
                                        <Translate id="school-module.associate-entity-with"/> {': '}
                                        <b>{relationEntityName}</b>
                                        <br/>
                                        <p>
                                            {message}
                                        </p>
                                        {disabledSave === false ?
                                            <div>
                                        <button className="btn-default button-primary-custom btn-yesno" onClick={saveRelation}>
                                            <Translate id="school-module.yes"/>
                                        </button>
                                        {' '}
                                        <Button className="btn-yesno" color="primary" onClick={handleEntityRelatedNoSelected}>
                                            <Translate id="school-module.no" />
                                        </Button>
                                                </div>
                                                : null
                                        }
                                    </div>
                             : null
                            }

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

export default ModalSchoolEntity;