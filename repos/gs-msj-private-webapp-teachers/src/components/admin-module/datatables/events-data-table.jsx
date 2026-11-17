import React, { useState, useEffect, useRef } from 'react';
import { Button, Input, Row, Col, Card, CardBody } from 'reactstrap';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faSearch, faEye, faPen, faTrashAlt,
    faClock, faArrowUp, faArrowDown, faMapMarkerAlt
} from "@fortawesome/free-solid-svg-icons";
import dayjs from 'dayjs';
import { useTranslation } from 'react-i18next';
import Pagination from "../../shared-module-components/datatables/pagination";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import Swal from 'sweetalert2';
import { getEvents, deleteEvent } from "../../../api/events";
import ModalEventForm from "../modals/modal-event-form";
import ModalEventDetail from "../modals/modal-event-detail";
import { EVENT_STATUS_BADGES, EVENT_TEAMS_SYNC_STATUS_BADGES, formatAudienceSummary } from "../constants/event-status";
import Settings from "../../../custom-classes/settings";
import "../styles/style.css";
import "../../shared-module-components/datatables/style-datatables.css";

const PAGE_SIZE_OPTIONS = [5, 10, 25];
const TITLE_TRUNCATE_LENGTH = 40;

const formatEventDate = (value) => value ? dayjs(value).format("MM/DD/YYYY hh:mm a") : "";

const truncateTitle = (title) => title && title.length > TITLE_TRUNCATE_LENGTH
    ? `${title.slice(0, TITLE_TRUNCATE_LENGTH)}...`
    : title;

const EventsDataTable = ({ showCreateModal, onCloseCreateModal }) => {
    const { t } = useTranslation('events');
    const [events,        setEvents]        = useState([]);
    const [loading,       setLoading]       = useState(false);
    const [page,          setPage]          = useState(0);
    const [totalPages,    setTotalPages]    = useState(0);
    const [totalItems,    setTotalItems]    = useState(0);
    const [paginationKey, setPaginationKey] = useState(0);
    const [pageSize,      setPageSize]      = useState(PAGE_SIZE_OPTIONS[0]);
    const [searchTitle,   setSearchTitle]   = useState('');
    const [filterStatus,        setFilterStatus]        = useState('');
    const [filterAudienceRole,  setFilterAudienceRole]  = useState('');
    const [sortDescending, setSortDescending] = useState(false);
    const [showModal,       setShowModal]       = useState(false);
    const [showDetailModal, setShowDetailModal] = useState(false);
    const [selectedEvent,   setSelectedEvent]   = useState(null);

    useEffect(() => {
        if (showCreateModal) {
            setSelectedEvent(null);
            setShowModal(true);
        }
    }, [showCreateModal]);

    // Initial load
    useEffect(() => {
        getEventsData(0, '', '', '', false, pageSize);
    }, []);

    // Live title search: debounced, so it doesn't fire on every keystroke. Status/audience
    // filters stay button-triggered — only the title text box is "real time" per spec. Skips
    // the very first render so it never duplicates the initial load above.
    const isFirstSearchRender = useRef(true);
    useEffect(() => {
        if (isFirstSearchRender.current) {
            isFirstSearchRender.current = false;
            return;
        }

        const handle = setTimeout(() => {
            setPage(0);
            setPaginationKey(k => k + 1);
            getEventsData(0, searchTitle, filterStatus, filterAudienceRole, sortDescending, pageSize);
        }, 400);

        return () => clearTimeout(handle);
    }, [searchTitle]);

    const getEventsData = async (fromPage, title, status, audienceRole, sortDesc, size) => {
        setLoading(true);
        try {
            const res = await getEvents(fromPage, size, title || null, null, status || null, audienceRole || null, sortDesc);
            const data = res.data;
            setEvents(data.EventModelList);
            setTotalPages(data.TotalPages);
            setTotalItems(data.TotalItems);
        } catch (error) {
            console.error('Error fetching events:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = () => {
        setPage(0);
        setPaginationKey(k => k + 1);
        getEventsData(0, searchTitle, filterStatus, filterAudienceRole, sortDescending, pageSize);
    };

    const handlePageClick = (newPage) => {
        setPage(newPage);
        getEventsData(newPage, searchTitle, filterStatus, filterAudienceRole, sortDescending, pageSize);
    };

    const handleSortToggle = () => {
        const newSortDescending = !sortDescending;
        setSortDescending(newSortDescending);
        setPage(0);
        setPaginationKey(k => k + 1);
        getEventsData(0, searchTitle, filterStatus, filterAudienceRole, newSortDescending, pageSize);
    };

    const handlePageSizeChange = (e) => {
        const newSize = Number(e.target.value);
        setPageSize(newSize);
        setPage(0);
        setPaginationKey(k => k + 1);
        getEventsData(0, searchTitle, filterStatus, filterAudienceRole, sortDescending, newSize);
    };

    const handleView = (event) => {
        setSelectedEvent(event);
        setShowDetailModal(true);
    };

    const handleCloseDetail = () => {
        setShowDetailModal(false);
        setSelectedEvent(null);
    };

    const handleEditFromDetail = () => {
        setShowDetailModal(false);
        setShowModal(true);
    };

    const handleEdit = (event) => {
        setSelectedEvent(event);
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setSelectedEvent(null);
        handleSearch();
        onCloseCreateModal();
    };

    const handleDelete = (event) => {
        Swal.fire({
            title: t('events-table.delete-confirm-title'),
            text: t('events-table.delete-confirm-text', { title: event.Title }),
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: t('events-table.delete-confirm-yes'),
            cancelButtonText: t('events-table.delete-confirm-cancel'),
        }).then((result) => {
            if (result.isConfirmed) {
                performDelete(event);
            }
        });
    };

    const performDelete = async (event) => {
        setLoading(true);
        try {
            await deleteEvent(event.Id);
            Swal.fire({
                toast: true,
                position: 'bottom-end',
                icon: 'success',
                title: t('events-table.delete-success'),
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
            });
            handleSearch();
        } catch (error) {
        } finally {
            setLoading(false);
        }
    };

    return (
        <Row className="clear-style-card td-responsive mt-3">
            <Col xs={12}>
                <Card>
                    <CardBody>

                        {/* Filters */}
                        <div className="wrapper-col mb-3">
                            <div className="four-col">
                                <div className="form-group-fields">
                                    <div className="label">{t('events-table.title-label')}</div>
                                    <Input
                                        type="text"
                                        placeholder={t('events-table.title-placeholder')}
                                        value={searchTitle}
                                        onChange={e => setSearchTitle(e.target.value)}
                                    />
                                </div>
                            </div>
                            <div className="four-col">
                                <div className="form-group-fields">
                                    <div className="label">{t('events-table.status-label')}</div>
                                    <Input
                                        type="select"
                                        value={filterStatus}
                                        onChange={e => setFilterStatus(e.target.value)}
                                    >
                                        <option value="">{t('events-table.status-all')}</option>
                                        {Object.keys(EVENT_STATUS_BADGES).map((value) => (
                                            <option key={value} value={value}>{t(`events-common.status-${value}`)}</option>
                                        ))}
                                    </Input>
                                </div>
                            </div>
                            <div className="four-col">
                                <div className="form-group-fields">
                                    <div className="label">{t('events-table.audience-label')}</div>
                                    <Input
                                        type="select"
                                        value={filterAudienceRole}
                                        onChange={e => setFilterAudienceRole(e.target.value)}
                                    >
                                        <option value="">{t('events-table.audience-all')}</option>
                                        <option value={Settings.ROLE_DIRECTOR_ID}>{t('events-table.audience-director')}</option>
                                        <option value={Settings.ROLE_TEACHER_ID}>{t('events-table.audience-teacher')}</option>
                                        <option value={Settings.ROLE_STUDENT_ID}>{t('events-table.audience-student')}</option>
                                    </Input>
                                </div>
                            </div>
                            <div className="four-col mt-3">
                                <Button className="btn-simple--only-text" onClick={handleSearch}>
                                    <FontAwesomeIcon icon={faSearch} />{' '}{t('events-table.search-button')}
                                </Button>
                            </div>
                        </div>

                        <div className="d-flex justify-content-end align-items-center mb-2" style={{ gap: '0.5em' }}>
                            <span style={{ color: '#9098AC', fontSize: '0.9em' }}>{t('events-table.page-size-label')}</span>
                            <Input
                                type="select"
                                bsSize="sm"
                                style={{ width: 'auto' }}
                                value={pageSize}
                                onChange={handlePageSizeChange}
                            >
                                {PAGE_SIZE_OPTIONS.map(size => (
                                    <option key={size} value={size}>{size}</option>
                                ))}
                            </Input>
                        </div>

                        {/* Table */}
                        <div className="wrapper-table-admin mt-3">
                            <table className="content-table-admin">
                                <thead>
                                    <tr>
                                        <th>{t('events-table.column-title')}</th>
                                        <th>
                                            {t('events-table.column-start')}{' '}
                                            <FontAwesomeIcon
                                                icon={sortDescending ? faArrowDown : faArrowUp}
                                                style={{ cursor: 'pointer' }}
                                                onClick={handleSortToggle}
                                                title={t('events-table.sort-tooltip')}
                                            />
                                        </th>
                                        <th>{t('events-form.location-label')}</th>
                                        <th>{t('events-table.column-timezone')}</th>
                                        <th>{t('events-table.column-status')}</th>
                                        <th>{t('events-table.column-audience')}</th>
                                        <th>{t('events-table.column-teams')}</th>
                                        <th>{t('events-table.column-actions')}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {events.map((event) => {
                                        const badge = EVENT_STATUS_BADGES[event.Status];
                                        const badgeLabel = badge ? t(`events-common.status-${event.Status}`) : (event.Status ?? '—');
                                        const teamsBadge = EVENT_TEAMS_SYNC_STATUS_BADGES[event.TeamsSyncStatus];
                                        return (
                                            <tr key={event.Id}>
                                                <td>
                                                    <span className="table-admin--title" style={{ fontWeight: '600', fontSize: '1.1em' }} title={event.Title}>{truncateTitle(event.Title)}</span>
                                                </td>
                                                <td data-column={`${t('events-table.column-start')}:`}>
                                                    <p><strong>{formatEventDate(event.StartDate)}</strong></p>
                                                    {event.EndDate && (
                                                        <p className="txt-LighterBlue">
                                                            <FontAwesomeIcon icon={faClock} size="xs" />{' '}
                                                            {t('events-table.until')} {formatEventDate(event.EndDate)}
                                                        </p>
                                                    )}
                                                </td>
                                                <td data-column={`${t('events-form.location-label')}:`}>
                                                    {event.Location && (
                                                        <>
                                                            <FontAwesomeIcon icon={faMapMarkerAlt} size="xs" className="txt-LighterBlue" />{' '}
                                                            {event.Location}
                                                        </>
                                                    )}
                                                </td>
                                                <td data-column={`${t('events-table.column-timezone')}:`}>
                                                    {event.TimeZoneCode}
                                                </td>
                                                <td data-column={`${t('events-table.column-status')}:`}>
                                                    <span className={`badge ${(badge || { cls: 'bg-secondary' }).cls}`}>{badgeLabel}</span>
                                                </td>
                                                <td data-column={`${t('events-table.column-audience')}:`}>
                                                    {formatAudienceSummary(t, event.AudienceSummary)}
                                                </td>
                                                <td data-column={`${t('events-table.column-teams')}:`}>
                                                    {teamsBadge && (
                                                        <span className={`badge ${teamsBadge.cls}`}>{t(`events-common.teams-status-${event.TeamsSyncStatus}`)}</span>
                                                    )}
                                                </td>
                                                <td>
                                                    <div style={{ display: 'flex', gap: '4px' }}>
                                                        <Button size="sm" color="info"    title={t('events-table.view-detail')} style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }} onClick={() => handleView(event)}>
                                                            <FontAwesomeIcon icon={faEye} />
                                                        </Button>
                                                        <Button size="sm" color="warning" title={t('events-table.edit')} style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }} onClick={() => handleEdit(event)}>
                                                            <FontAwesomeIcon icon={faPen} />
                                                        </Button>
                                                        <Button
                                                            size="sm"
                                                            color="danger"
                                                            title={t('events-table.delete')}
                                                            style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}
                                                            onClick={() => handleDelete(event)}
                                                        >
                                                            <FontAwesomeIcon icon={faTrashAlt} />
                                                        </Button>
                                                    </div>
                                                </td>
                                            </tr>
                                        );
                                    })}
                                    {!loading && events.length === 0 && (
                                        <tr>
                                            <td
                                                colSpan={8}
                                                style={{ textAlign: 'center', padding: '2.5em', color: '#9098AC' }}
                                            >
                                                {t('events-table.empty-state')}
                                            </td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>

                        {totalPages > 1 && (
                            <Pagination
                                key={paginationKey}
                                totalPages={totalPages}
                                currentPage={page}
                                updateParent={handlePageClick}
                                dapper={true}
                            />
                        )}

                    </CardBody>
                </Card>
            </Col>

            {loading && <LoaderFullWindow />}

            {showModal && (
                <ModalEventForm
                    visible={showModal}
                    event={selectedEvent}
                    updateParent={handleCloseModal}
                />
            )}
            {showDetailModal && (
                <ModalEventDetail
                    visible={showDetailModal}
                    event={selectedEvent}
                    onEdit={handleEditFromDetail}
                    updateParent={handleCloseDetail}
                />
            )}
        </Row>
    );
};

export default EventsDataTable;
