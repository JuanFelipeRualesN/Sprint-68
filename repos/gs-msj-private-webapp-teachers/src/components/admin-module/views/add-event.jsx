import React, { useState, useEffect } from 'react';
import { Button, Input, Row, Col } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import Dropzone from 'react-dropzone';
import Swal from 'sweetalert2';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPaperclip, faTimes } from "@fortawesome/free-solid-svg-icons";
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import moment from 'moment';
import { getEvent, createEvent, modifyEvent } from "../../../api/events";
import { getEventFileRules, uploadEventFile, deleteEventFile } from "../../../api/events-files";
import { getAllTimeZones } from "../../../api/teachers";
import { getFileExtension } from "../../../utils/utils";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import Select from "../../shared-module-components/selector/common-selector";
import DatePicker from "../../shared-module-components/date-picker/date-picker-recent";
import hourOptionsList from "../../shared-module-components/date-picker/time-options";
import AudienceSegmentation from "../selectors/audience-segmentation";
import "../styles/style.css";

dayjs.extend(customParseFormat);

const hourOptions = hourOptionsList.map(hour => ({ value: hour, label: hour }));

const splitIsoDateTime = (isoValue) => {
    if (!isoValue) return { date: '', hour: '' };
    const parsed = dayjs(isoValue);
    if (!parsed.isValid()) return { date: '', hour: '' };
    return { date: parsed.format('YYYY-MM-DD'), hour: parsed.format('hh:mm a') };
};

const buildIsoDateTime = (dateValue, hourLabel) => {
    if (!dateValue) return '';
    const dateOnly = dayjs(dateValue).format('YYYY-MM-DD');
    const combined = dayjs(`${dateOnly} ${hourLabel}`, 'YYYY-MM-DD hh:mm a');
    return combined.isValid() ? combined.format('YYYY-MM-DDTHH:mm:ss') : '';
};

// Converts a stored date into the Moment object the date picker expects.
const toPickerDate = (value) => {
    if (!value) return null;
    const parsed = moment(value);
    return parsed.isValid() ? parsed : null;
};

const bytesToSize = (bytes) => {
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (!bytes) return '0 Bytes';
    const i = Math.floor(Math.log(bytes) / Math.log(1024));
    return `${Math.round(bytes / Math.pow(1024, i))} ${sizes[i]}`;
};

const SectionHeader = ({ title }) => (
    <div style={{
        borderBottom: '2px solid #eceefa',
        paddingBottom: '0.4em',
        marginBottom: '1em',
        marginTop: '1.5em',
    }}>
        <h6 style={{ color: '#27466C', fontWeight: 'bold', margin: 0 }}>{title}</h6>
    </div>
);

const AddEvent = React.forwardRef(({ event, toggle }, ref) => {
    const { t } = useTranslation('events');
    const isEdit = Boolean(event?.Id);

    const [title, setTitle] = useState(event?.Title || '');
    const [description, setDescription] = useState(event?.Description || '');
    const [location, setLocation] = useState(event?.Location || '');
    const [startDate, setStartDate] = useState('');
    const [startHour, setStartHour] = useState('');
    const [endDate, setEndDate] = useState('');
    const [endHour, setEndHour] = useState('');
    const [timeZoneOptions, setTimeZoneOptions] = useState([]);
    const [selectedTimeZone, setSelectedTimeZone] = useState(null);
    const [eventTimeZoneCode, setEventTimeZoneCode] = useState('');
    const [eventAttachmentList, setEventAttachmentList] = useState([]);
    const [fileRules, setFileRules] = useState([]);
    const [uploadingFiles, setUploadingFiles] = useState(false);
    const [attachmentMessage, setAttachmentMessage] = useState({ visible: false, bodyText: "" });
    const [eventAudienceLists, setEventAudienceLists] = useState({
        EventRoleModelList: [],
        EventSchoolModelList: [],
        EventRegionModelList: [],
        EventUserModelList: [],
    });
    const [audienceLists, setAudienceLists] = useState({
        EventRoleModelList: [],
        EventSchoolModelList: [],
        EventRegionModelList: [],
        EventUserModelList: [],
    });
    const [validTitle, setValidTitle] = useState(true);
    const [validStartDate, setValidStartDate] = useState(true);
    const [validStartHour, setValidStartHour] = useState(true);
    const [validEndHour, setValidEndHour] = useState(true);
    const [validStartNotPast, setValidStartNotPast] = useState(true);
    const [validTimeZone, setValidTimeZone] = useState(true);
    const [loading, setLoading] = useState(false);
    const [messagesModal, setMessagesModal] = useState({
        visible: false,
        bodyText: ""
    });

    useEffect(() => {
        if (event?.Id) {
            getEventDetail(event.Id);
        }
    }, [event]);

    useEffect(() => {
        const loadCatalogs = async () => {
            try {
                const res = await getAllTimeZones();
                setTimeZoneOptions(res.data.map(item => ({ value: item.Code, label: item.Desc })));
            } catch { }
        };
        loadCatalogs();
    }, []);

    useEffect(() => {
        const loadFileRules = async () => {
            try {
                const res = await getEventFileRules();
                setFileRules(res.data.SupportedFileModelList || []);
            } catch { }
        };
        loadFileRules();
    }, []);

    useEffect(() => {
        if (timeZoneOptions.length === 0) {
            return;
        }

        if (eventTimeZoneCode) {
            const match = timeZoneOptions.find(option => option.value === eventTimeZoneCode);
            if (match) {
                setSelectedTimeZone(match);
            }
        }
    }, [timeZoneOptions, eventTimeZoneCode]);

    const toggleAlertModal = () => {
        setMessagesModal({
            visible: false,
            bodyText: "",
        });
        toggle();
    };

    const getEventDetail = async (id) => {
        setLoading(true);
        try {
            const res = await getEvent(id);
            const eventDetail = res.data.EventModel;
            setTitle(eventDetail.Title || '');
            setDescription(eventDetail.Description || '');
            setLocation(eventDetail.Location || '');
            const startSplit = splitIsoDateTime(eventDetail.StartDate);
            setStartDate(startSplit.date);
            setStartHour(startSplit.hour);
            const endSplit = splitIsoDateTime(eventDetail.EndDate);
            setEndDate(endSplit.date);
            setEndHour(endSplit.hour);
            setEventTimeZoneCode(eventDetail.TimeZoneCode || '');
            setEventAudienceLists({
                EventRoleModelList: res.data.EventRoleModelList || [],
                EventSchoolModelList: res.data.EventSchoolModelList || [],
                EventRegionModelList: res.data.EventRegionModelList || [],
                EventUserModelList: res.data.EventUserModelList || [],
            });
            setEventAttachmentList(res.data.EventAttachmentModelList || []);
        } catch {
        } finally {
            setLoading(false);
        }
    };

    const validateFileAgainstRules = (file) => {
        const extension = getFileExtension(file.name) || '';
        const rule = fileRules.find(item => item.Format.toLowerCase() === extension.toLowerCase());
        return Boolean(rule) && file.size <= Number(rule.MaxSize);
    };

    const handleFilesDrop = async (acceptedFiles) => {
        if (!acceptedFiles || acceptedFiles.length === 0) return;

        const allValid = acceptedFiles.every(validateFileAgainstRules);
        if (!allValid) {
            setAttachmentMessage({
                visible: true,
                bodyText: t('events-form.invalid-files-message'),
            });
            return;
        }

        setUploadingFiles(true);
        for (const file of acceptedFiles) {
            try {
                const res = await uploadEventFile(file);
                const newFileName = res.data.Messages[1];
                setEventAttachmentList(prev => [...prev, { AttachmentUrl: newFileName }]);
            } catch {
                setAttachmentMessage({
                    visible: true,
                    bodyText: t('events-form.upload-error-message'),
                });
                break;
            }
        }
        setUploadingFiles(false);
    };

    const handleRemoveAttachment = async (index) => {
        const attachment = eventAttachmentList[index];
        if (!attachment.Id) {
            try {
                await deleteEventFile(attachment.AttachmentUrl);
            } catch { }
        }
        setEventAttachmentList(prev => prev.filter((_, i) => i !== index));
    };

    const toggleAttachmentMessageModal = () => {
        setAttachmentMessage({ visible: false, bodyText: "" });
    };

    const handleSubmit = async () => {
        const titleOk = title.trim().length >= 3;
        const startDateOk = startDate.length > 0;
        const startHourOk = startHour.length > 0;
        const endHourOk = endDate.length === 0 || endHour.length > 0;
        const startDateTime = startDateOk && startHourOk ? buildIsoDateTime(startDate, startHour) : '';
        // New events only — editing an already-past event (e.g. to add attachments) must stay allowed.
        const startNotPastOk = isEdit || !startDateTime || !dayjs(startDateTime).isBefore(dayjs());
        const timeZoneOk = Boolean(selectedTimeZone?.value);
        setValidTitle(titleOk);
        setValidStartDate(startDateOk);
        setValidStartHour(startHourOk);
        setValidEndHour(endHourOk);
        setValidStartNotPast(startNotPastOk);
        setValidTimeZone(timeZoneOk);
        if (!titleOk || !startDateOk || !startHourOk || !endHourOk || !startNotPastOk || !timeZoneOk) return;

        const request = {
            EventModel: {
                ...(event?.Id ? { Id: event.Id, Available: "1" } : {}),
                Title: title,
                Description: description,
                Location: location,
                StartDate: buildIsoDateTime(startDate, startHour),
                EndDate: buildIsoDateTime(endDate, endHour),
                TimeZoneCode: selectedTimeZone?.value || '',
            },
            EventAttachmentModelList: eventAttachmentList,
            ...audienceLists,
        };

        setLoading(true);
        try {
            let response;
            if (event?.Id) {
                response = await modifyEvent(event.Id, request);
            } else {
                response = await createEvent(request);
            }

            if (response.data.Valid) {
                toggle();
                Swal.fire({
                    toast: true,
                    position: 'bottom-end',
                    icon: 'success',
                    title: isEdit ? t('events-form.toast-updated') : t('events-form.toast-created'),
                    showConfirmButton: false,
                    timer: 3000,
                    timerProgressBar: true,
                });
                return;
            }

            let msg = "";
            if (response.data.Messages?.length) {
                response.data.Messages.forEach(message => {
                    msg += message + "<br/>";
                });
            }
            setMessagesModal({
                visible: true,
                bodyText: msg,
            });
        } catch {
        } finally {
            setLoading(false);
        }
    };

    // Exposes the submit action to the parent modal, so the Save button can
    // live in the modal's own footer (alongside Cancel) instead of inside
    // this component's scrollable content.
    React.useImperativeHandle(ref, () => ({
        submit: handleSubmit,
    }));

    return (
        <>
            <SectionHeader title={t('events-form.section-basic-info')} />
            <Row>
                <Col md={8}>
                    <div className="form-group-fields mb-3 mt-0">
                        <div className="label">{t('events-form.title-label')}</div>
                        <Input
                            type="text"
                            value={title}
                            onChange={e => setTitle(e.target.value)}
                            placeholder={t('events-form.title-placeholder')}
                            maxLength={150}
                        />
                        {!validTitle && (
                            <p className="text-danger-styled-bold">{t('events-form.title-required')}</p>
                        )}
                    </div>
                </Col>
            </Row>
            <div className="form-group-fields mb-3 mt-0">
                <div className="label">{t('events-form.description-label')}</div>
                <Input
                    type="textarea"
                    value={description}
                    onChange={e => setDescription(e.target.value)}
                    rows={3}
                    placeholder={t('events-form.description-placeholder')}
                />
            </div>

            <SectionHeader title={t('events-form.section-date-place')} />
            <Row>
                <Col md={6}>
                    <div className="form-group-fields mb-3 mt-0">
                        <div className="label">{t('events-form.start-label')}</div>
                        <div style={{ display: 'flex', gap: '0.5em', flexWrap: 'wrap' }}>
                            <DatePicker
                                date={toPickerDate(startDate)}
                                updateParent={(value) => setStartDate(value.date)}
                            />
                            <Select
                                options={hourOptions}
                                value={startHour ? { value: startHour, label: startHour } : null}
                                onChange={(selected) => setStartHour(selected.value)}
                                placeholder={t('events-form.hour-placeholder')}
                                noOptionsMessage={() => t('audience-segmentation.no-options')}
                            />
                        </div>
                        {!validStartDate && (
                            <p className="text-danger-styled-bold">{t('events-form.start-date-required')}</p>
                        )}
                        {validStartDate && !validStartHour && (
                            <p className="text-danger-styled-bold">{t('events-form.start-hour-required')}</p>
                        )}
                        {validStartDate && validStartHour && !validStartNotPast && (
                            <p className="text-danger-styled-bold">{t('events-form.start-date-past')}</p>
                        )}
                    </div>
                </Col>
                <Col md={6}>
                    <div className="form-group-fields mb-3 mt-0">
                        <div className="label">{t('events-form.end-label')}</div>
                        <div style={{ display: 'flex', gap: '0.5em', flexWrap: 'wrap' }}>
                            <DatePicker
                                date={toPickerDate(endDate)}
                                updateParent={(value) => setEndDate(value.date)}
                            />
                            <Select
                                options={hourOptions}
                                value={endHour ? { value: endHour, label: endHour } : null}
                                onChange={(selected) => setEndHour(selected.value)}
                                placeholder={t('events-form.hour-placeholder')}
                                noOptionsMessage={() => t('audience-segmentation.no-options')}
                            />
                        </div>
                        {!validEndHour && (
                            <p className="text-danger-styled-bold">{t('events-form.end-hour-required')}</p>
                        )}
                    </div>
                </Col>
            </Row>
            <Row>
                <Col md={6}>
                    <div className="form-group-fields mb-3 mt-0">
                        <div className="label">{t('events-form.location-label')}</div>
                        <Input
                            type="text"
                            value={location}
                            onChange={e => setLocation(e.target.value)}
                            placeholder={t('events-form.location-placeholder')}
                        />
                    </div>
                </Col>
                <Col md={6}>
                    <div className="form-group-fields mb-3 mt-0">
                        <div className="label">{t('events-form.timezone-label')}</div>
                        <Select
                            options={timeZoneOptions}
                            value={selectedTimeZone}
                            onChange={setSelectedTimeZone}
                            placeholder={t('events-form.timezone-placeholder')}
                            noOptionsMessage={() => t('audience-segmentation.no-options')}
                        />
                        {!validTimeZone && (
                            <p className="text-danger-styled-bold">{t('events-form.timezone-required')}</p>
                        )}
                    </div>
                </Col>
            </Row>

            <SectionHeader title={t('events-form.section-audience-segmentation')} />
            <AudienceSegmentation
                eventRoleModelList={eventAudienceLists.EventRoleModelList}
                eventSchoolModelList={eventAudienceLists.EventSchoolModelList}
                eventRegionModelList={eventAudienceLists.EventRegionModelList}
                eventUserModelList={eventAudienceLists.EventUserModelList}
                updateParent={setAudienceLists}
            />

            <SectionHeader title={t('events-form.section-attachments')} />
            <Dropzone onDrop={handleFilesDrop} accept={fileRules.map(rule => `.${rule.Format}`).join(',')}>
                {({ getRootProps, getInputProps, isDragActive }) => (
                    <div
                        {...getRootProps()}
                        className={`events-dropzone${isDragActive ? ' events-dropzone--active' : ''}`}
                        style={{ marginBottom: '1em' }}
                    >
                        <input {...getInputProps()} />
                        <FontAwesomeIcon
                            icon={faPaperclip}
                            size="2x"
                            style={{ color: '#8DA5C2', marginBottom: '0.5em' }}
                        />
                        <p style={{ color: '#9098AC', margin: '0.4em 0 0', fontSize: '0.9em' }}>
                            {t('events-form.dropzone-text')}{' '}
                            <strong style={{ color: '#197BAA' }}>{t('events-form.dropzone-click')}</strong>
                        </p>
                        <small style={{ color: '#9098AC', fontSize: '0.8em' }}>
                            {fileRules.length > 0
                                ? fileRules.map(rule => `${rule.Format.toUpperCase()} (${t('events-form.file-rule-max')} ${bytesToSize(rule.MaxSize)})`).join(', ')
                                : t('events-form.loading-file-rules')}
                        </small>
                    </div>
                )}
            </Dropzone>
            {eventAttachmentList.length > 0 && (
                <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.5em', marginBottom: '1em' }}>
                    {eventAttachmentList.map((attachment, index) => (
                        <span
                            key={`${attachment.AttachmentUrl}-${index}`}
                            style={{
                                display: 'inline-flex',
                                alignItems: 'center',
                                gap: '0.5em',
                                backgroundColor: '#EBF5FA',
                                borderRadius: '4px',
                                padding: '0.3em 0.6em',
                                fontSize: '0.85em',
                            }}
                        >
                            <span style={{ color: '#0d6efd' }}>{attachment.AttachmentUrl}</span>
                            <FontAwesomeIcon
                                icon={faTimes}
                                style={{ color: '#dc3545', cursor: 'pointer' }}
                                onClick={() => handleRemoveAttachment(index)}
                            />
                        </span>
                    ))}
                </div>
            )}
            {uploadingFiles && <small style={{ color: '#9098AC' }}>{t('events-form.uploading-files')}</small>}

            {messagesModal.visible && <ModalGeneric data={messagesModal} updateParent={toggleAlertModal} />}
            {attachmentMessage.visible && <ModalGeneric data={attachmentMessage} updateParent={toggleAttachmentMessageModal} />}
            {loading && <LoaderFullWindow />}
        </>
    );
});

export default AddEvent;
