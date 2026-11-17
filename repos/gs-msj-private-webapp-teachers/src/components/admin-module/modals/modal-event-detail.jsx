import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button, Row, Col } from 'reactstrap';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPencilAlt, faFileDownload } from "@fortawesome/free-solid-svg-icons";
import { useTranslation } from 'react-i18next';
import dayjs from 'dayjs';
import { getEvent } from "../../../api/events";
import { getEventFileSignedUrl } from "../../../api/events-files";
import { getFileExtension } from "../../../utils/utils";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { EVENT_STATUS_BADGES, EVENT_TEAMS_SYNC_STATUS_BADGES, formatAudienceSummary } from "../constants/event-status";
import "../styles/style.css";

const formatEventDate = (value) => value ? dayjs(value).format("MM/DD/YYYY hh:mm a") : "—";

const SectionHeader = ({ title }) => (
    <div style={{ borderBottom: '2px solid #eceefa', paddingBottom: '0.4em', marginBottom: '1em', marginTop: '1.5em' }}>
        <h6 style={{ color: '#27466C', fontWeight: 'bold', margin: 0 }}>{title}</h6>
    </div>
);

const ModalEventDetail = ({ visible, event, onEdit, updateParent }) => {
    const { t } = useTranslation('events');
    const [isVisible, setIsVisible] = useState(visible);
    const [loading, setLoading] = useState(false);
    const [eventDetail, setEventDetail] = useState(event);
    const [eventAttachmentList, setEventAttachmentList] = useState([]);

    useEffect(() => {
        setIsVisible(visible);
    }, [visible]);

    useEffect(() => {
        if (event?.Id) {
            getEventDetail(event.Id);
        }
    }, [event]);

    const getEventDetail = async (id) => {
        setLoading(true);
        try {
            const res = await getEvent(id);
            setEventDetail(res.data.EventModel);
            setEventAttachmentList(res.data.EventAttachmentModelList || []);
        } catch {
        } finally {
            setLoading(false);
        }
    };

    const openAttachment = async (attachmentUrl) => {
        const extension = getFileExtension(attachmentUrl) || '';
        const fileName = extension ? attachmentUrl.slice(0, -(extension.length + 1)) : attachmentUrl;
        try {
            const res = await getEventFileSignedUrl(fileName, extension);
            window.open(res.data.FileUrl, '_blank');
        } catch { }
    };

    const toggle = () => {
        setIsVisible(false);
        updateParent();
    };

    if (!event) return null;

    const statusBadge = EVENT_STATUS_BADGES[eventDetail?.Status];
    const teamsBadge = EVENT_TEAMS_SYNC_STATUS_BADGES[eventDetail?.TeamsSyncStatus];

    return (
        <>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    {eventDetail?.Title || '—'}
                </ModalHeader>
                <ModalBody>
                    <SectionHeader title={t('events-form.section-basic-info')} />
                    <div className="form-group-fields mb-3">
                        <div className="label">{t('events-form.description-label')}</div>
                        <p style={{ margin: 0 }}>{eventDetail?.Description || '—'}</p>
                    </div>
                    <div className="form-group-fields mb-3">
                        <div className="label">{t('events-table.column-audience')}</div>
                        <p style={{ margin: 0 }}>{formatAudienceSummary(t, eventDetail?.AudienceSummary) || '—'}</p>
                    </div>

                    <SectionHeader title={t('events-form.section-date-place')} />
                    <Row>
                        <Col md={6}>
                            <div className="form-group-fields mb-3">
                                <div className="label">{t('events-table.column-start')}</div>
                                <p style={{ margin: 0 }}>{formatEventDate(eventDetail?.StartDate)}</p>
                            </div>
                        </Col>
                        <Col md={6}>
                            <div className="form-group-fields mb-3">
                                <div className="label">{t('events-form.end-label')}</div>
                                <p style={{ margin: 0 }}>{formatEventDate(eventDetail?.EndDate)}</p>
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col md={6}>
                            <div className="form-group-fields mb-3">
                                <div className="label">{t('events-form.location-label')}</div>
                                <p style={{ margin: 0 }}>{eventDetail?.Location || '—'}</p>
                            </div>
                        </Col>
                        <Col md={6}>
                            <div className="form-group-fields mb-3">
                                <div className="label">{t('events-table.column-timezone')}</div>
                                <p style={{ margin: 0 }}>{eventDetail?.TimeZoneCode || '—'}</p>
                            </div>
                        </Col>
                    </Row>

                    <SectionHeader title={t('events-table.column-status')} />
                    <div className="form-group-fields mb-3">
                        {statusBadge ? (
                            <span className={`badge ${statusBadge.cls}`}>{t(`events-common.status-${eventDetail.Status}`)}</span>
                        ) : (
                            <p style={{ margin: 0 }}>—</p>
                        )}
                    </div>

                    <SectionHeader title={t('events-table.column-teams')} />
                    <div className="form-group-fields mb-3">
                        {teamsBadge ? (
                            <span className={`badge ${teamsBadge.cls}`}>{t(`events-common.teams-status-${eventDetail.TeamsSyncStatus}`)}</span>
                        ) : (
                            <p style={{ margin: 0 }}>—</p>
                        )}
                    </div>

                    <SectionHeader title={t('events-form.section-attachments')} />
                    <div className="form-group-fields mb-3">
                        {eventAttachmentList.length > 0 ? (
                            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.5em' }}>
                                {eventAttachmentList.map((attachment, index) => (
                                    <span
                                        key={`${attachment.AttachmentUrl}-${index}`}
                                        onClick={() => openAttachment(attachment.AttachmentUrl)}
                                        style={{
                                            display: 'inline-flex',
                                            alignItems: 'center',
                                            gap: '0.5em',
                                            backgroundColor: '#EBF5FA',
                                            color: '#197BAA',
                                            borderRadius: '4px',
                                            padding: '0.3em 0.6em',
                                            fontSize: '0.85em',
                                            cursor: 'pointer',
                                        }}
                                    >
                                        <FontAwesomeIcon icon={faFileDownload} />
                                        {attachment.AttachmentUrl}
                                    </span>
                                ))}
                            </div>
                        ) : (
                            <p style={{ margin: 0 }}>—</p>
                        )}
                    </div>
                </ModalBody>
                <ModalFooter>
                    <Button color="warning" onClick={onEdit}>
                        <FontAwesomeIcon icon={faPencilAlt} />{' '}{t('events-table.edit')}
                    </Button>
                    <Button color="secondary" onClick={toggle}>{t('events-detail.close-button')}</Button>
                </ModalFooter>
            </Modal>
            {loading && <LoaderFullWindow />}
        </>
    );
};

export default ModalEventDetail;
