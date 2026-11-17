import React, { useState } from 'react';
import { Button } from 'reactstrap';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { useTranslation } from 'react-i18next';
import EventsDataTable from "../datatables/events-data-table";
import "../styles/style.css";

const EventsSearch = () => {
    const { t } = useTranslation('events');
    const [showCreateModal, setShowCreateModal] = useState(false);

    const openCreateModal = () => setShowCreateModal(true);
    const closeCreateModal = () => setShowCreateModal(false);

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">{t('events-search.page-title')}</div>
                        </div>
                        <div className="col txt-right">
                            <Button color="warning" onClick={openCreateModal}>
                                <FontAwesomeIcon icon={faPlus} />{' '}{t('events-search.create-button')}
                            </Button>
                        </div>
                    </div>

                    <EventsDataTable showCreateModal={showCreateModal} onCloseCreateModal={closeCreateModal} />

                    <hr />
                </div>
            </div>
        </div>
    );
};

export default EventsSearch;
