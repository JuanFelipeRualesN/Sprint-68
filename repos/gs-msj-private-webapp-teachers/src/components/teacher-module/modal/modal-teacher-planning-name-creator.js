/**
 * Created by portiz 2020/05/08
 */
import React, {useState, useEffect} from 'react';
import { Button, Modal, ModalHeader, ModalBody, Input, ModalFooter, Row, Col } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import { createPlanning } from "../../../api/teachers";
import { BeatLoader } from "react-spinners";
import { FaExclamationTriangle, FaCheck } from "react-icons/all";
import { getSubscritionsAssociatedTeacher } from "../../../api/subscriptions";
import LanguageManager from "../../../custom-classes/language-manager";
import Select from 'react-select';
import moment from 'moment';
import { convertMaxLengthSting } from "../../../utils/utils";
import Heading from '../planning/shared/components/heading';
import FormDatePicker from '../planning/shared/components/form/form-date-picker';
import FormField from '../planning/shared/components/form/form-field';
import { translate } from '../planning/shared/utils';
import { Alert } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faExclamationCircle } from "@fortawesome/free-solid-svg-icons";

/** End of Multiple Language imports **/

/**
 * Modal to create teacher planning
 */
const ModalTeacherPlanningNameCreator = ({visible, isCatalog, updateParent}) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [planningName, setPlanningName] = useState('');
    const [loading, setLoading] = useState(false);
    const [showMessage, setShowMessage] = useState(false);
    const [subscriptions, setSubscriptions] = useState([]);
    const [subscriptionsOptions, setSubscriptionsOptions] = useState([]);
    const [periodsOptions, setPeriodsOptions] = useState([]);
    const [subjectsOptions, setSubjectsOptions] = useState([]);
    const [subscriptionSelected, setSubscriptionSelected] = useState(null);
    const [periodSelected, setPeriodSelected] = useState(null);
    const [subjectsLevelsSelected, setSubjectsLevelsSelected] = useState(null);
    const [submited, setSubmited] = useState(false);
    const [language] = useState(LanguageManager.getLanguage());
    const [openDate, setOpenDate] = useState();
    const [closeDate, setCloseDate] = useState();
    const [weekNumber, setWeekNumber] = useState(1);
    const [showOpenDateError, setShowOpenDateError] = useState(false);
    const [showCloseDateError, setShowCloseDateError] = useState(false);

    useEffect(() => {
        fetchSubscriptionsAssociatedTeacher();
    }, []);

    const handleNameChange = (e) => {
        const value = e.currentTarget.value;
        const newValue = convertMaxLengthSting(value, 50);
        setPlanningName(newValue);
    }

    const handleWeekNumberChange = (e) => {
        const value = e.currentTarget.value;
        setWeekNumber(value);
    }

    const toggle = () => {
        updateParent({ visible: !visible });
    };

    const toggleYes = async () => {
        setLoading(true)
        try {
            const formattedOpenDate = openDate ? openDate.format("YYYY-MM-DD") : null;
            const formattedCloseDate = closeDate ? closeDate.format("YYYY-MM-DD") : null;

            const response = await createPlanning(planningName, periodSelected.value, subjectsLevelsSelected.subject, 
                subjectsLevelsSelected.level, periodSelected.periodTo, formattedOpenDate, formattedCloseDate, weekNumber );
            if (response.data.Messages != null && response.data.Messages.length > 0) {
                let message = '';
                for (let i = 0; i < response.data.Messages.length; i++) {
                    message += response.data.Messages[i] + '<br/>';
                }
                setShowMessage(true);
                setErrorMessage(message);
                setLoading(false);
                setSubmited(true);
            }            
            
        } catch (error) {
            console.error("An error has ocurred: ", error);
        } finally {
            setLoading(false);
        }
    }

    const toggleNo = () => {
        updateParent({ visible: false });
    }

    const fetchSubscriptionsAssociatedTeacher = async () => {
        setLoading(true);
        try {
            const response = await getSubscritionsAssociatedTeacher();
            const subscriptions = response.data;
            const monthNamesEn = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            const monthNamesEs = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];

            // Get only active subscriptions
            const activeSubscriptions = subscriptions.filter(item =>
                new Date(item.EntityAcademicDetailFullResponseModel?.EntityAcademicDetailModel.FullTermEndDate) >= new Date()
            );

            if (activeSubscriptions.length > 1) {
                const options = activeSubscriptions.map(item => {
                    const { FullTermStartDate, FullTermEndDate } = item.EntityAcademicDetailFullResponseModel.EntityAcademicDetailModel;
                    let dateFromFormatted = moment(FullTermStartDate);
                    let dateToFormatted = moment(FullTermEndDate);
                    let dateFromMonth = "";
                    let dateToMonth = "";
                    if (language === "es") {
                        dateFromMonth = monthNamesEs[dateFromFormatted.month()];
                        dateToMonth = monthNamesEs[dateToFormatted.month()];
                    }
                    else {
                        dateFromMonth = monthNamesEn[dateFromFormatted.month()];
                        dateToMonth = monthNamesEn[dateToFormatted.month()];
                    }
                    return {
                        value: item.SubscriptionModel.Id,
                        label: dateFromMonth + " " + dateFromFormatted.year().toString() + " - " + dateToMonth + " " + dateToFormatted.year().toString() + " | " + item.PlanModel.Name,
                    };
                });

                setSubscriptions(activeSubscriptions);
                setSubscriptionsOptions(options);
                setLoading(false);
            } else if (activeSubscriptions.length == 1) {
                const subscription = activeSubscriptions[0];
                const periods = subscription.EntityAcademicDetailFullResponseModel?.AcademicPeriodModelList?.filter((p) => new Date(p.PeriodTo) >= new Date()) || [];
                const options = periods.map(item => {
                    return {
                        value: item.Id,
                        label: item.Id + ": " + moment(item.PeriodFrom, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY") + " - " + moment(item.PeriodTo, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY"),
                        periodFrom: item.PeriodFrom,
                        periodTo: item.PeriodTo
                    };
                });
                const subjectsLevels = subscription.SubscriptionProductModelList;
                const subjectsOptions = subjectsLevels.map((item, i) => {
                    return {
                        subject: item.Subject,
                        label: item.SubjectName + ' ' + item.Level,
                        level: item.Level,
                        value: i
                    };
                });

                setLoading(false);
                setSubscriptions(activeSubscriptions);
                setPeriodsOptions(options);
                setSubjectsOptions(subjectsOptions);
                setSubscriptionSelected(subscription);
            } else{
                setLoading(false);
                setSubscriptions(activeSubscriptions);
            }
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    }

    const handleChangeSubscription = (selected) => {
        const subscription = subscriptions.find(item => item.SubscriptionModel.Id === selected.value);
        const periods = subscription.EntityAcademicDetailFullResponseModel?.AcademicPeriodModelList || [];

        // Get only active periods
        const activePeriods = periods.filter((p) => new Date(p.PeriodTo) >= new Date());

        const options = activePeriods.map(period => {
            return {
                value: period.Id,
                label: period.Id + ": " + moment(period.PeriodFrom, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY") + " - " + moment(period.PeriodTo, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY"),
                periodFrom: period.PeriodFrom,
                periodTo: period.PeriodTo
            };
        });
        const subjectsLevels = subscription.SubscriptionProductModelList;
        const subjectsOptions = subjectsLevels.map((item, i) => {
            return {
                subject: item.Subject,
                label: item.SubjectName + ' ' + item.Level,
                level: item.Level,
                value: i
            };
        });
        setPeriodsOptions(options);
        setSubjectsOptions(subjectsOptions);
        setSubscriptionSelected(subscription);
        setPeriodSelected(null);
        setSubjectsLevelsSelected(null);
    }

    const handleChangePeriods = (selected) => {
        setPeriodSelected(selected);

        const periodFrom = moment(selected.periodFrom);
        const today = moment().startOf('day');

        // Set close date to 6 days after open date or periodTo if it is earlier
        const open = today.isAfter(periodFrom) ? today : periodFrom;
        const close = open.clone().add(6, 'days') < moment(selected.periodTo) ? open.clone().add(6, 'days') : moment(selected.periodTo);

        if (isCatalog) {
            setOpenDate(open);
            setCloseDate(close);
        }

        setShowOpenDateError(false);
        setShowCloseDateError(false);
    };

    const handleChangeSubjectsLevels = (selected) => {
        setSubjectsLevelsSelected(selected);
    }

    // Validate if open date is after close date and vice versa
    const validateDates = (open, close) => {
        if (!open || !close) {
            setShowOpenDateError(false);
            setShowCloseDateError(false);
            return;
        }

        const isOpenAfterClose = open.isAfter(close);
        const isCloseBeforeOpen = close.isBefore(open);

        setShowOpenDateError(isOpenAfterClose);
        setShowCloseDateError(isCloseBeforeOpen);
    };

    // Handle date change for planning open and close dates
    const handleDateChange = (field, date) => {
        if (!date) return;

        const formattedDate = moment(date);

        if (field === "OpenDate") {
            setOpenDate(formattedDate);
            validateDates(formattedDate, closeDate);
        } else if (field === "CloseDate") {
            setCloseDate(formattedDate);
            validateDates(openDate, formattedDate);
        }
    };

    const today = moment().startOf('day');
    const periodFrom = moment(periodSelected?.periodFrom);
    const periodTo = moment(periodSelected?.periodTo);

    const fromDate = periodFrom.isBefore(today) ? today : periodFrom;

    return (
        <div>
            <Modal isOpen={visible} toggle={toggle} centered={isCatalog} size='lg'>
                <ModalHeader toggle={toggle}>
                    <Translate id="planning.planning-create" />
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col xs={12} lg={3}>
                            <b><Translate id="planning.name" options={{ renderInnerHtml: true }} /></b>
                        </Col>
                        <Col>
                            <Input value={planningName} type="text" name="planningName"
                                id="planningName" onChange={handleNameChange} />
                            <small><Translate id="planning.planning-create-name" options={{ renderInnerHtml: true }} /></small>
                        </Col>
                    </Row>
                    {subscriptions.length > 1 && (<div style={{ marginBottom: '1rem' }}>
                        <Row>
                            <Col xs={12} lg={3}>
                                <b><Translate id="planning.planning-subsctiption" /></b>
                            </Col>
                            <Col>
                                <Select
                                    classNamePrefix="mantine-like"
                                    className="mantine-like__control"
                                    placeholder={<Translate id="planning.select-subsctiption" />}
                                    options={subscriptionsOptions}
                                    onChange={handleChangeSubscription}
                                />
                            </Col>
                        </Row>
                    </div>)}
                    <div style={{ marginBottom: '1rem' }}>
                        <Row>
                            <Col xs={12} lg={3}>
                                <b><Translate id="planning.planning-perid" /></b>
                            </Col>
                            <Col>
                                {loading ?
                                    <BeatLoader
                                        size={10}
                                        color={"#6bc4ff"}
                                    /> :
                                    <Select
                                        classNamePrefix="mantine-like"
                                        className="mantine-like__control"
                                        value={periodSelected}
                                        placeholder={<Translate id="planning.select-period" />}
                                        options={periodsOptions}
                                        onChange={handleChangePeriods}
                                        noOptionsMessage={() => <Translate id="planning.no-active-periods-found" />}
                                    />}
                            </Col>
                        </Row>
                    </div>
                    <Row>
                        <Col xs={12} lg={3}>
                            <b><Translate id="planning.subject-grade" options={{ renderInnerHtml: true }} /></b>
                        </Col>
                        <Col>
                            {loading ?
                                <BeatLoader
                                    size={10}
                                    color={"#6bc4ff"}
                                /> :
                                <Select
                                    classNamePrefix="mantine-like"
                                    className="mantine-like__control"
                                    value={subjectsLevelsSelected}
                                    placeholder={<Translate id="planning.select-subject-grade" />}
                                    options={subjectsOptions}
                                    onChange={handleChangeSubjectsLevels}
                                />

                            }
                        </Col>
                    </Row>
                    {isCatalog &&
                    <>
                        <Heading as="h6" className="mt-4">
                            <Translate id="planning.period-message" />
                        </Heading>

                        <div className="wrapperFlex contentForms-inline">
                            <div className="col pl-0">
                                <FormDatePicker
                                    name="openDate"
                                    label={
                                    <span>
                                        <span className="required">* </span>
                                        <Translate id="planning.start-date" />
                                    </span>
                                    }
                                    onChange={(date) => {
                                        if (!date) return;
                                        handleDateChange('OpenDate', date);
                                    }}
                                    showIcon={false}
                                    minDate={fromDate.toDate()}
                                    maxDate={periodTo.toDate()}
                                    value={openDate}
                                    disabled={loading || !periodSelected}
                                    filterDate={() => periodSelected?.periodFrom || periodSelected?.periodTo}
                                />
                                {showOpenDateError && (
                                    <Alert color="warning">
                                        <FontAwesomeIcon icon={faExclamationCircle} />{' '}
                                        <Translate id="planning.open-date-error" />
                                    </Alert>
                                )}
                            </div>

                            <div className="col pl-0">
                                <FormDatePicker
                                name="closeDate"
                                label={
                                    <span>
                                        <span className="required">* </span>
                                        <Translate id="planning.end-date" />
                                    </span>
                                }
                                onChange={(date) => {
                                    if (!date) return;
                                    handleDateChange('CloseDate', date);
                                }}
                                showIcon={false}
                                minDate={fromDate.toDate()}
                                maxDate={periodTo.toDate()}
                                value={closeDate}
                                disabled={loading || !periodSelected}
                                filterDate={() => periodSelected?.periodFrom || periodSelected?.periodTo}
                                />
                                {showCloseDateError && (
                                    <Alert color="warning">
                                        <FontAwesomeIcon icon={faExclamationCircle} />{' '}
                                        <Translate id="planning.close-date-error" />
                                    </Alert>
                                )}
                            </div>

                            <div className="pr-0 pl-0">
                                <FormField
                                    name="weekNumber"
                                    label={
                                    <span>
                                        <span className="required">* </span>
                                        <Translate id="planning.week-number" />
                                    </span>
                                    }
                                    placeholder={translate("planning.week-number")}
                                    onChange={handleWeekNumberChange}
                                    value={weekNumber}
                                    disabled={loading}
                                />
                            </div>
                        </div>
                    </>
                    }   

                    {showMessage ? (
                        <div className="sucessfull-txt">
                            <FaCheck />{' '}
                            <div dangerouslySetInnerHTML={{ __html: errorMessage }} />
                        </div>
                    )
                        :
                        null
                    }
                </ModalBody>
                <ModalFooter>
                    <Row className="warning-text">
                        <Col>
                            <FaExclamationTriangle />{' '}
                            <Translate id="planning.planning-subject-grade-note" />
                        </Col>
                    </Row>
                    <Row>
                        <p className="text-right">
                            <Button disabled={!planningName || loading || !periodSelected || !subjectsLevelsSelected || !subscriptions.length 
                                || submited || (isCatalog && !openDate) || (isCatalog && !closeDate)  || (isCatalog && !weekNumber) 
                                || (isCatalog && showCloseDateError == true) || (isCatalog && showOpenDateError == true) }
                                color="success" onClick={toggleYes}>
                                {loading ?
                                    <BeatLoader
                                        size={10}
                                        color={"#fff"}

                                    /> : null}{' '}
                                <Translate id="planning.create-plan" options={{ renderInnerHtml: true }} />
                            </Button>{' '}
                            <Button color="secondary" onClick={toggleNo}>
                                <Translate id="planning.close" />
                            </Button>
                        </p>
                    </Row>
                </ModalFooter>
            </Modal>
        </div>
    );
}
export default ModalTeacherPlanningNameCreator;