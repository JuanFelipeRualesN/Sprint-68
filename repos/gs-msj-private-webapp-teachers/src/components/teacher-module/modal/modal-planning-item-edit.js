/**
 * Created by portiz 2020/05/12
 */
import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody, Row, Col, Input, Alert } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import { modifyTeacherQuiz, modifyTeacherLesson, getPlanLessonDetail, getAllTimeZones } from "../../../api/teachers";
import Select from 'react-select'
import { BeatLoader } from "react-spinners";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faCalendarAlt,
} from '@fortawesome/free-solid-svg-icons';
import LocalizedDatePicker from '../../../utils/datepicker-locale';
import 'react-datepicker/dist/react-datepicker.css';
import moment from 'moment';
import TimePicker from 'react-time-picker';
/** End of Multiple Language imports **/

/**
 * Modal to edit a planning item
 */
const ModalPlanningItemEdit = ({planningId, lessonOrQuizId, lessonOrQuiz, forGrading, dueDate, triesQuantity, id, visible, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [dueDateState, setDueDateState] = useState(dueDate ? new Date(moment(dueDate, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD HH:mm:ss")) : null);
    const [forGradingState, setForGradingState] = useState(forGrading);
    const [selectedTriesOption, setSelectedTriesOption] = useState({ "value": 1, "label": 1 });
    const [isVisible, setIsVisible] = useState(visible);
    const [exams, setExams] = useState([]);
    const [validDates, setValidDates] = useState(true);
    const [timezoneOptions, setTimezoneOptions] = useState([]);
    const [isRequiredPrePost, setIsRequiredPrePost] = useState(false);

    useEffect(() => {
        if (lessonOrQuiz === "LESSON") {
            getDetailsPlanLesson(id);
        }
    }, []);

    const getDetailsPlanLesson = async (planLessonId) => {
        setLoading(true);
        try {
            const response = await getPlanLessonDetail(planLessonId);
            const data = response.data;
            const timezones = await getTimezones();

            const examsData = data.TeacherPlanLessonQuizModels.map(item => {
                return {
                    Id: item.Id,
                    QuizId: item.QuizId,
                    Title: item.QuizTitle,
                    DueDate: item.DueDate ? new Date(moment(item.DueDate, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD HH:mm:ss")) : "",
                    hour: item.DueDate ? moment(item.DueDate).format("HH:mm") : "",
                    duration: item.Duration,
                    timezone: timezones.filter(timezone => timezone.value === item.DueDateTimezone)[0],
                };
            });

            setLoading(false);
            setDueDateState(data.TeacherPlanLessonModel.DueDate ? new Date(moment(data.TeacherPlanLessonModel.DueDate, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD HH:mm:ss")) : "");
            setExams(examsData);
            setIsRequiredPrePost(!!parseInt(data.TeacherPlanLessonModel.PrePostRequired));
        } catch {
            setLoading(false);
        }
    };

    const handleDueDateSelection = (e) => {
        setDueDateState(e);
    };

    const handleDinamicsDueDatesQuiz = (date, id) => {
        const arrExams = [...exams];
        const index = arrExams.findIndex(item => item.QuizId === id);
        arrExams[index].DueDate = date;

        const valid = validateDueDatesQuiz(exams);

        setExams(arrExams);
        setValidDates(valid);
    };

    const validateDueDatesQuiz = (arrDates) => {
        let valid = true;

        for (let item of arrDates) {
            if (!item.DueDate) {
                valid = false;
                break;
            }
        }
        return valid;
    };

    // Handle value for grading
    const handleForGrading = (event) => {
        setForGradingState(event.currentTarget.checked);
    };

    // Handle select for tries
    const handleOnChangeSelectTries = (selected) => {
        setSelectedTriesOption(selected);
    };

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(!isVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            loading,
            planningId,
            lessonOrQuizId,
            lessonOrQuiz,
            forGrading: forGradingState,
            dueDate: dueDateState,
            triesQuantity,
            id,
            selectedTriesOption,
            visible: !isVisible,
            exams,
            validDates,
            timezoneOptions,
            isRequiredPrePost,
        });
    };

    // Toogles when yes
    const toggleYes = () => {
        if (lessonOrQuiz === "LESSON") {
            if (!(validateDueDatesQuiz(exams))) {
                setValidDates(false);
                return;
            }
            const dueDatesQuiz = exams.map(item => {
                return {
                    Id: item.Id,
                    QuizId: item.QuizId,
                    DueDate: item.DueDate ? `${moment(item.DueDate).format("YYYY-MM-DD")} ${item.hour}:00` : null,
                    DueDateTimezone: item.timezone.value,
                    Duration: parseInt(item.duration),
                };
            });

            setLoading(true);
            modifyTeacherLesson(id, moment(dueDateState).format("YYYY-MM-DD"), forGradingState, triesQuantity, isRequiredPrePost, dueDatesQuiz, planningId).then((response) => {
                setIsVisible(false);
                // Updates the parent (send it owns to state to update it)
                updateParent({
                    loading: false,
                    planningId,
                    lessonOrQuizId,
                    lessonOrQuiz,
                    forGrading: forGradingState,
                    dueDate: dueDateState,
                    triesQuantity,
                    id,
                    selectedTriesOption,
                    visible: false,
                    exams,
                    validDates,
                    timezoneOptions,
                    isRequiredPrePost,
                });
            }).catch((error) => {

            }).then(() => {
                setLoading(false);
            });
        }
        else if (lessonOrQuiz === "QUIZ") {
            setLoading(true);
            modifyTeacherQuiz(id, planningId, lessonOrQuizId, forGradingState, moment(dueDateState).format("YYYY-MM-DD"), triesQuantity).then((response) => {
                setIsVisible(false);
                updateParent({
                    loading: false,
                    planningId,
                    lessonOrQuizId,
                    lessonOrQuiz,
                    forGrading: forGradingState,
                    dueDate: dueDateState,
                    triesQuantity,
                    id,
                    selectedTriesOption,
                    visible: false,
                    exams,
                    validDates,
                    timezoneOptions,
                    isRequiredPrePost,
                });
            }).catch((error) => {

            }).then(() => {
                setLoading(false);
            });
        }
    };

    const toggleNo = () => {
        setIsVisible(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            loading,
            planningId,
            lessonOrQuizId,
            lessonOrQuiz,
            forGrading: forGradingState,
            dueDate: dueDateState,
            triesQuantity,
            id,
            selectedTriesOption,
            visible: false,
            exams,
            validDates,
            timezoneOptions,
            isRequiredPrePost,
        });
    };

    const getTimezones = async () => {
        setLoading(true);
        try {
            const res = await getAllTimeZones();
            const timezones = res.data;
            const options = timezones.map(item => {
                return {
                    value: item.Code,
                    label: item.Desc
                };
            });
            setLoading(false);
            setTimezoneOptions(options);
            return options;
        } catch {
            setLoading(false);
            return;
        }
    };

    const handleHourDueDatesQuiz = (hour, id) => {
        const arrExams = [...exams];
        const index = arrExams.findIndex(item => item.QuizId === id);
        arrExams[index].hour = hour;

        setExams(arrExams);
    };

    const hadleTimezoneQuiz = (timezone, id) => {
        const arrExams = [...exams];
        const index = arrExams.findIndex(item => item.QuizId === id);
        arrExams[index].timezone = timezone;

        setExams(arrExams);
    };

    const hadleDurationQuiz = (duration, id) => {
        const arrExams = [...exams];
        const index = arrExams.findIndex(item => item.QuizId === id);
        arrExams[index].duration = duration;

        setExams(arrExams);
    };

    const onChangeRequiredPrePost = (e) => {
        setIsRequiredPrePost(e.currentTarget.checked);
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results modal-fix-header">
                <ModalHeader toggle={toggle}>
                    <Translate id="planning.edit" />
                </ModalHeader>
                <ModalBody>
                    <div>

                        <Row>
                            <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                <b><Translate id="planning.due-date" /></b>
                            </Col>
                            <Col className="wrapper-icon-datapicker">
                                <FontAwesomeIcon icon={faCalendarAlt} />{' '}
                                <LocalizedDatePicker
                                    className="form-control"
                                    selected={dueDateState}
                                    onChange={date => handleDueDateSelection(date)}
                                    dateFormat="MM/dd/yyyy"
                                    minDate={new Date(moment().add(1, 'days').format("YYYY-MM-DD HH:mm:ss"))}
                                />
                            </Col>
                        </Row>

                        <Row className="wrapper-row-labels-timezone pt-2">
                            <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                            </Col>
                            <Col className="wrapper-icon-datapicker teacher-manag-col-flex">
                                <Translate id="planning.label-field-date" />
                            </Col>

                            <Col className="module-time-picker col-space-adjus teacher-manag-col-flex">
                                <Translate id="planning.label-field-hour" />
                            </Col>

                            <Col className="col-space-adjus teacher-manag-col-flex">
                                <Translate id="planning.label-field-timezone" />
                            </Col>

                            <Col className="module-number-picker col-space-adjus teacher-manag-col-flex">                                    
                                <Translate id="planning.label-field-duration" />
                            </Col>
                        </Row>
                        <div className='stripe-rows b-bottom'>
                        {lessonOrQuiz === "LESSON" && exams.length > 0 ?
                            exams.map((item) => (
                                <Row key={item.QuizId} className="timeZone-labels">
                                    <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                        <b>{item.Title}</b>
                                    </Col>
                                    <Col className="wrapper-icon-datapicker teacher-manag-col-flex">
                                        <label><Translate id="planning.label-field-date" /></label>
                                        <FontAwesomeIcon icon={faCalendarAlt} />{' '}
                                        <LocalizedDatePicker
                                            className="form-control"
                                            selected={item.DueDate}
                                            onChange={date => handleDinamicsDueDatesQuiz(date, item.QuizId)}
                                            dateFormat="MM/dd/yyyy"
                                            minDate={new Date(moment().add(1, 'days').format("YYYY-MM-DD HH:mm:ss"))}
                                        />
                                    </Col>

                                    <Col className="module-time-picker col-space-adjus teacher-manag-col-flex">
                                        <label><Translate id="planning.label-field-hour" /></label>
                                        <TimePicker clearIcon={null} locale={"en-EN"} openClockOnFocus={false} onChange={hour => handleHourDueDatesQuiz(hour, item.QuizId)} value={item.hour} />
                                    </Col>

                                    <Col className="col-space-adjus teacher-manag-col-flex">
                                        <label><Translate id="planning.label-field-timezone" /></label>
                                        <Select
                                            classNamePrefix="mantine-like"
                                            className="mantine-like__control"
                                            placeholder={"Timezone"}
                                            options={timezoneOptions}
                                            onChange={selected => hadleTimezoneQuiz(selected, item.QuizId)}
                                            value={item.timezone}
                                        />
                                    </Col>

                                    <Col className="module-number-picker col-space-adjus teacher-manag-col-flex">
                                        <label><Translate id="planning.label-field-duration" /></label>
                                        <Input type="number" min={15} value={item.duration} onChange={e => hadleDurationQuiz(e.target.value, item.QuizId)} />
                                    </Col>
                                </Row>
                            )) : null}
                        </div>

                        <Row>
                            <Col className='clear-check mt-1'>
                                <div className="b-bottom pb-2 pt-2">
                                    <Input type="checkbox" checked={forGradingState} value={forGradingState}
                                        onChange={handleForGrading} />{' '}
                                    {lessonOrQuiz === "LESSON" ? <span className='ml-4 check-font'><Translate id="planning.planning-for-grading" /></span> : <span className='ml-4 check-font'><Translate id="planning.planning-for-grading-assignment" /></span>}
                                </div>
                            </Col>
                        </Row>

                        {lessonOrQuiz === "LESSON" && <Row>

                            <Col className='clear-check mt-1'>
                                <div className="b-bottom pb-2 mb-2">
                                    <Input type="checkbox" checked={isRequiredPrePost} value={isRequiredPrePost}
                                        onChange={onChangeRequiredPrePost} />{' '}
                                    <span className='ml-4 check-font'><Translate id="planning.title-required-pre-post" /></span>
                                </div>
                            </Col>
                        </Row>}


                        {/* <Row>
                            <Col>
                                <p><Translate id="planning.planning-tries-quantity" /></p>
                                <Select options={optionsSelectTries}
                                    value={selectedTriesOption}
                                    isDisabled={true} //whilemean the usecase is unclear
                                    placeholder={<Translate id="planning.select-tries-quantity" />}
                                    onChange={handleOnChangeSelectTries} />

                            </Col>

                        </Row> */}


                        <Row>
                            <Col xs={12}>
                                {!validDates ?
                                    <Alert color="danger"><Translate id="planning.validate-dates-exams" /></Alert> : null}
                            </Col>
                        </Row>

                        <p className="text-right mt-4 modal-cancel">
                            <Button className="mr-2 primary-btn" color="info" onClick={toggleYes} disabled={loading ? "disabled" : ""}>
                                {loading ?
                                    <BeatLoader
                                        size={10}
                                        color={"#fff"}

                                    /> : null}{' '}
                                <Translate id="planning.edit" />
                            </Button>

                            <Button onClick={toggleNo} disabled={loading ? "disabled" : ""}>
                                {loading ?
                                    <BeatLoader
                                        size={10}
                                        color={"#fff"}

                                    /> : null}{' '}
                                <Translate id="planning.cancel" />
                            </Button>
                        </p>
                    </div>
                </ModalBody>
            </Modal>
        </div>
    );
};


export default ModalPlanningItemEdit;
