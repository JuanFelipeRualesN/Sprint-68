import React, { useState, useEffect, useRef } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSave } from "@fortawesome/free-solid-svg-icons";
import AddEvent from '../views/add-event';
import "../styles/style.css";

const ModalEventForm = ({ visible, event, updateParent }) => {
    const { t } = useTranslation('events');
    const [isVisible, setIsVisible] = useState(visible);
    const addEventRef = useRef(null);
    const isEdit = Boolean(event?.Id);

    useEffect(() => {
        setIsVisible(visible);
    }, [visible]);

    const toggle = () => {
        setIsVisible(!isVisible);
        updateParent(!isVisible);
    };

    const handleSave = () => {
        addEventRef.current?.submit();
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} size="lg" className="modal-results">
                <ModalHeader toggle={toggle}>
                    {isEdit ? t('events-modal.edit-title') : t('events-modal.create-title')}
                </ModalHeader>
                <ModalBody>
                    <div className="wrapper-content-academic-details input-responsive">
                        <AddEvent ref={addEventRef} event={event} toggle={toggle} />
                    </div>
                </ModalBody>
                <ModalFooter style={{ justifyContent: 'flex-end', gap: '0.5em' }}>
                    <Button color="secondary" onClick={toggle}>{t('events-modal.cancel-button')}</Button>
                    <Button color="warning" onClick={handleSave}>
                        <FontAwesomeIcon icon={faSave} />{' '}
                        {isEdit ? t('events-form.save-button') : t('events-form.create-button')}
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalEventForm;
