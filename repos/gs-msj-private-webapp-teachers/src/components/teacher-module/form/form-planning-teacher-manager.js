/**
 * Created by portiz on 5/08/20.
 */
import React, { useState, useEffect} from 'react';
import {
    Button,
    Col,
    Row,
    FormGroup,
    Label,
    Input, Alert
} from 'reactstrap';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faCalendarAlt,
} from '@fortawesome/free-solid-svg-icons';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import LocalizedDatePicker from '../../../utils/datepicker-locale';
import 'react-datepicker/dist/react-datepicker.css';
import SelectorSubject from "../selector/selector-subject";
import SelectorLesson from "../../shared-module-components/selector/selector-lesson-by-subject-level";
import SelectorTeacherAssignments from "../selector/selector-teacher-assignments";
import { addTeacherPlanAssignment, addTeacherPlanLesson, getLessonExamsTeachers, getAllTimeZones } from "../../../api/teachers";
import { BeatLoader } from "react-spinners";
import Select from 'react-select';
import moment from 'moment';
import TimePicker from 'react-time-picker';
import SelectorType from "../selector/selector-type";
import LearningGapLessonView
    from "../learning-gap/learning-gap-lessons/learning-gap-lesson-view/learning-gap-lesson-view";
import SelectorPlanning from '../selector/selector-planning';
import { Provider } from 'react-redux';
import { LocalizeProvider } from 'react-localize-redux';
import store from '../../../redux/store';
import FormMultiDatePicker from '../planning/shared/components/form/form-multi-date-picker';
import { faExclamationCircle } from "@fortawesome/free-solid-svg-icons";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const FormPlanningTeacherManager = (props) => {
    const [apiMessage, setApiMessage] = useState('');
    const [defaultTypeAssignmentOrLessons] = useState(props.defaultTypeAssignmentOrLessons ?? null);
    const [assignmentOrLesson, setAssignmentOrLesson] = useState(props.assignmentOrLesson ?? '0');
    const [dueDate, setDueDate] = useState(props.dueDate ?? null);
    const [levelCode, setLevelCode] = useState(null);
    const [forGrading, setForGrading] = useState(false);
    const [isGapClosing, setIsGapClosing] = useState(null);
    const [lessonId, setLessonId] = useState(props.lessonId ?? null);
    const [planningId, setPlanningId] = useState(props.planningId ?? null);
    const [quizId, setQuizId] = useState(props.quizId ?? null);
    const [showApiMessage, setShowApiMessage] = useState(false);
    const [exams, setExams] = useState([]);
    const [validDates, setValidDates] = useState(true);
    const [selectedTriesOption, setSelectedTriesOption] = useState({ "value": 1, "label": 1 });
    const [timezoneOptions, setTimezoneOptions] = useState([]);
    const [isRequiredPrePost, setIsRequiredPrePost] = useState(false);
    const [isCatalog] = useState(props.isCatalog ?? false);
    const [plannings] = useState(props.plannings);
    const [selectedPlanning, setSelectedPlanning] = useState(null);
    const [minDate, setMinDate] = useState(null);
    const [maxDate, setMaxDate] = useState(null);
    const [lessonDates, setLessonDates] = useState([]);
    const [maxPeriodDate, setMaxPeriodDate] = useState(null);
    const [subjectCode, setSubjectCode] = useState(null);
    const [loading, setLoading] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        if (defaultTypeAssignmentOrLessons !== null) {
            setAssignmentOrLesson(defaultTypeAssignmentOrLessons);
        }
        getTimezones();
    }, []);

    useEffect(() => {
        if (isCatalog && lessonId && timezoneOptions.length > 0) {
            handleLessonIdSelection();
        }
    }, [isCatalog, lessonId, timezoneOptions]);

    // Trigger success modal when the lesson is successfully assigned
    const triggerSuccessModal = () => {
        props.handleSuccessModal();
    };

    /* Handles assignment or lesson selection */
    const handleAssignmentOrLessonSelection = (e) => {
        setAssignmentOrLesson(e.currentTarget.value);
        setQuizId(null);
        setLessonId(null);
        setLevelCode(null);
        setSubjectCode(null);
    };

    const handleAssignmentSelection = (e) => {
        setQuizId(e.value);
        setLessonId(null);
    };

    /* Handles the due date of the assignment */
    const handleDueDateSelection = (e) => {
        setDueDate(e);
    }

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
        } catch {
            setLoading(false);
        }
    }

    /* Handle lesson ids */
    const handleLessonIdSelection = async (e) => {
        let selectedLessonId;
        // If the component is being used in the lesson catalog section, use the lessonId from the props
        if (isCatalog) {
            selectedLessonId = lessonId;
        } else {
            selectedLessonId = e.value;
        }
        try {
            const response = await getLessonExamsTeachers(selectedLessonId);
            const data = response.data;

            const exams = data.map(item => {
                return {
                    QuizId: item.Id,
                    Title: item.Title,
                    DueDate: "",
                    hour: '10:00',
                    duration: 60,
                    timezone: timezoneOptions[0],
                };
            });

            setLessonId(selectedLessonId);
            setQuizId(null);
            setLoading(false);
            setExams(exams);
        } catch {
            setLessonId(selectedLessonId);
            setQuizId(null);
            setLoading(false);
        }
    };

    const handleDinamicsDueDatesQuiz = (date, id) => {
        const arrExams = [...exams];
        const index = arrExams.findIndex(item => item.QuizId === id);
        arrExams[index].DueDate = date;

        const valid = validateDueDatesQuiz(exams);

        setExams(arrExams);
        setValidDates(valid);
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

    // const validateDurationTest = (duration) => {
    //     let validation = { valid: true, message: "" };
    //     if (!duration) {
    //         validation.valid = false;
    //         validation.message = <Translate id="planning.validation-field-duration-required" />;
    //     }
    //     if (parseInt(duration) < 15) {
    //         validation.valid = false;
    //         validation.message = <Translate id="planning.validation-field-duration-min" />;
    //     }
    //     return validation;
    // };

    /* Handle subject id selected */
    const handleSubjectCodeSelection = (value) => {
        setSubjectCode(value.subject);
        setLevelCode(value.level);
        setIsGapClosing(null);
        setLessonId(null);
    };

    /* Handle type of lesson */
    const handleTypeLessonSelection = (value) => {
        setIsGapClosing(value.value);
        setLessonId(null);
        // isGapClosing: value.value,
    };

    /* Handle value for grading */
    const handleForGrading = (event) => {
        setForGrading(event.currentTarget.checked);
    };

    /* Handle select for tries */
    const handleOnChangeSelectTries = (selected) => {
        setSelectedTriesOption(selected);
    };

    const savePlanningDetail = () => {
        if (quizId !== null) {
            setShowApiMessage(false);
            setApiMessage('');
            setLoading(true);

            addTeacherPlanAssignment(planningId, quizId, moment(dueDate).format("YYYY-MM-DD"),forGrading, selectedTriesOption.value).then((response) => {
                    props.updateParent(true);
                }).catch((error) => {
                    let messages = "";
                    error.response.data.Messages.forEach((message) => {
                        message += message + '<br/>';
                    })
                    setApiMessage(messages);
                    setShowApiMessage(true);
                }).then(() => {
                    setLoading(false);
                });
        } else if (lessonId !== null) {
            if (!(validateDueDatesQuiz(exams))) {
                setValidDates(false);
                return;
            }
            const dueDatesQuiz = exams.map(item => {
                return {
                    QuizId: item.QuizId,
                    DueDate: item.DueDate ? `${moment(item.DueDate).format("YYYY-MM-DD")} ${item.hour}:00` : null,
                    DueDateTimezone: item.timezone.value,
                    Duration: parseInt(item.duration),
                };
            });

            setShowApiMessage(false);
            setApiMessage('');
            setLoading(true);

            addTeacherPlanLesson(planningId,
                lessonId,
                moment(dueDate).format("YYYY-MM-DD"),
                forGrading,
                selectedTriesOption.value, 
                isRequiredPrePost, 
                dueDatesQuiz,
                lessonDates).then((response) => {

                    props.updateParent(true);

                    // Trigger succes modal when is being used from lesson catalog
                    if (isCatalog) {
                        triggerSuccessModal();
                    }

                }).catch((error) => {
                    let messages = "";
                    error.response.data.Messages.forEach((message) => {
                        message += message + '<br/>';
                    })
                    setApiMessage(messages);
                    setShowApiMessage(true);
                    setLoading(false);
                }).then(() => {
                    setLoading(false);
                });
        }
    };

    const onChangeRequiredPrePost = (e) => {
        setIsRequiredPrePost(e.currentTarget.checked);
    };

    const handlePlanningSelection = (planningId) => {
        const selectedPlanning = plannings.find(planning => planning.PlanId === planningId);

        let minDate;
        let maxDate;

        if (selectedPlanning && selectedPlanning.OpenDate) {
            minDate = new Date(selectedPlanning.OpenDate);
        }
        if (selectedPlanning && selectedPlanning.CloseDate) {
            maxDate = new Date(selectedPlanning.CloseDate);
        }

        setPlanningId(planningId);
        setSelectedPlanning(selectedPlanning);
        setMinDate(minDate);
        setMaxDate(maxDate);
        setMaxPeriodDate(selectedPlanning.AcademicPeriodFullDetail.PeriodTo);
        setDueDate(null);
        setIsRequiredPrePost(false);
        setForGrading(false);
        setExams(exams.map(exam => ({
            ...exam,
            DueDate: null,
            duration: 60,
        })));
    };

    // Handle the lesson dates selection
    const handleOnChangeLessonDates = (dates) => {
        if (!dates || dates.length === 0) {
            setLessonDates([]);
            return;
        }
        const formattedLessonDates = dates.map(date => {
            return {
                Date: moment(date).format("YYYY-MM-DD"),
                ImplementationDate: moment(date).format("YYYY-MM-DD")
            };
        });

        setLessonDates(formattedLessonDates);
    };

    const getPlanningIsDisabled = () => {
        if ((isCatalog && selectedPlanning !== null) && (selectedPlanning?.OpenDate == null || selectedPlanning?.CloseDate == null)) {
            return { disabled: true, text: 'old-planning' };
        } else if (isCatalog && selectedPlanning !== null) {
            const periodToDate = new Date(selectedPlanning?.AcademicPeriodFullDetail?.PeriodTo)
            const today = new Date()
            if (today > periodToDate) {
                return { disabled: true, text: 'period-ended' };
            }
        }

        return { disabled: false, text: null };
    };

    const planningStatus = getPlanningIsDisabled()
    const parsedMaxDate = new Date(maxPeriodDate?.format("YYYY-MM-DD HH:mm:ss"))

    return (
            <div className='wrapper-lesson-modal'>
                {defaultTypeAssignmentOrLessons === null && isCatalog === false  ?
                    <Row>
                        <Col lg={{ size: 4 }} md={{ size: 4 }} sm={{ size: 12 }} xs={{ size: 12 }}>
                            <b><Translate id="planning.type" /></b>
                        </Col>

                        <Col lg={{ size: 4 }} md={{ size: 4 }} sm={{ size: 12 }} xs={{ size: 12 }}>
                            <FormGroup check>
                                <Label check>
                                    <Input checked={assignmentOrLesson === '0' ? "checked" : ''} type="radio" name="lessonsOrAssignmentsRadio" value="0" onClick={handleAssignmentOrLessonSelection} />{' '}
                                    <Translate id="planning.assignments" />
                                </Label>
                            </FormGroup>
                        </Col>
                        <Col lg={{ size: 4 }} md={{ size: 4 }} sm={{ size: 12 }} xs={{ size: 12 }}>
                            <FormGroup check>
                                <Label check>
                                    <Input checked={assignmentOrLesson === '1' ? "checked" : ''} type="radio" name="lessonsOrAssignmentsRadio" value="1" onClick={handleAssignmentOrLessonSelection} />{' '}
                                    <Translate id="planning.lessons" />
                                </Label>
                            </FormGroup>
                        </Col>
                    </Row> : null}

                <div className="b-bottom pb-2 mb-2">
                    {assignmentOrLesson === "0" && isCatalog === false ? <>
                        <Row>
                            <Col xs={{ size: 12 }}>
                                <span className='tiny-font'><Translate id="planning.title-attach-assignment" /></span>
                            </Col>
                        </Row>
                        <Row>
                            <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                <b><Translate id="planning.assignments" /></b>
                            </Col>
                            <Col lg={{ size: 5 }} md={{ size: 5 }} xs={{ size: 12 }}>
                                <SelectorTeacherAssignments updateParent={handleAssignmentSelection} />
                            </Col>
                        </Row>

                        <Row>
                            <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                <b><Translate id="planning.due-date" /></b>
                            </Col>
                            <Col className="wrapper-icon-datapicker">
                                <FontAwesomeIcon icon={faCalendarAlt} />{' '}
                                <LocalizedDatePicker
                                    className="form-control"
                                    selected={dueDate}
                                    onChange={date => handleDueDateSelection(date)}
                                    dateFormat="MM/dd/yyyy"
                                    minDate={new Date(moment().add(1, 'days').format("YYYY-MM-DD HH:mm:ss"))}
                                />
                            </Col>
                        </Row>

                        <Row>
                            <Col className='clear-check mt-1'>
                                <Input type="checkbox" checked={forGrading} value={forGrading}
                                    onChange={handleForGrading} />{' '}
                                <span className='ml-4 check-font'><Translate id="planning.planning-for-grading-assignment" /></span>

                            </Col>
                        </Row> </>
                        : null
                    }

                    {assignmentOrLesson === "1" ?  <>
                        <Row>
                            <Col xs={{ size: 12 }}>
                                <span className='tiny-font'><Translate id="planning.title-attach-lesson" /></span>
                            </Col>
                        </Row>
                        <Row>
                            <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                <b><Translate id="planning.subject-grade" /></b>
                            </Col>
                            <Col lg={{ size: 5 }} md={{ size: 5 }} xs={{ size: 12 }}>
                                <SelectorSubject updateParent={handleSubjectCodeSelection} />
                            </Col>
                        </Row>
                        
                        {/*IsGapClosing*/}
                        
                        {subjectCode !== null && levelCode !== null ?
                        <Row>
                            <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                <b><Translate id="planning.type-lesson" /></b>
                            </Col>
                            <Col lg={{ size: 5 }} md={{ size: 5 }} xs={{ size: 12 }}>
                                <SelectorType updateParent={handleTypeLessonSelection} />
                            </Col>
                        </Row> : null}
                        

                        {subjectCode !== null && levelCode !== null && isGapClosing !== null ?
                            <Row>
                                <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                                    <b><Translate id="planning.select-lesson" /></b>
                                </Col>
                                <Col lg={{ size: 5 }} md={{ size: 5 }} xs={{ size: 12 }}>
                                    {subjectCode !== null ?
                                        <SelectorLesson subjectCode={subjectCode}
                                            levelCode={levelCode}
                                            isGapClosing={isGapClosing}
                                            updateParent={handleLessonIdSelection} />
                                        : null
                                    }
                                </Col>
                            </Row> : null}
                    </> : null
                    }
                </div>
                
                {/* Call SelectorPlanning to when the component is being used from lesson catalog section */}
                {isCatalog &&  <SelectorPlanning plannings={plannings} onPlanningSelect={handlePlanningSelection} />}
                
                {lessonId && <div className="b-bottom pb-2">
                    <Row>
                        <Col className='clear-check mt-1'>
                            <Input disabled={(planningStatus.disabled == true || !selectedPlanning) && isCatalog} type="checkbox" checked={forGrading} value={forGrading}
                                onChange={handleForGrading} />{' '}
                            <span className='ml-4 check-font'><Translate id="planning.planning-for-grading" /></span>

                        </Col>
                    </Row>
                    {isCatalog && 
                        <div className="Modal--confirmation-body text-left px-0 pb-0">
                            <Alert
                                color="danger"
                                isOpen={planningStatus.disabled}
                                className="alert-info"
                            >
                                <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />
                                {' '}
                                <span>
                                {planningStatus.text === 'old-planning' && (
                                    <Translate id="lesson-catalog.old-planning-warning" />
                                )}
                                {planningStatus.text === 'period-ended' && (
                                    <Translate id="planning.no-active-periods-no-edit" />
                                )}
                                </span>
                            </Alert>
                        </div>
                    }
                </div>}     
                {lessonId && <div className="b-bottom pb-2 mb-2 stripe-rows">
                    {isCatalog && 
                    <Row>
                        <div style={{padding: '10px 0px 35px 15px'}} className="Region-w-borderBottom">
                            <FormMultiDatePicker
                                label={<Translate id="messages.select-available-dates" />}
                                minDate={new Date()}
                                maxDate={maxDate}
                                onChange={(dates) => {
                                    if (!dates) return;
                                    handleOnChangeLessonDates(dates);
                                }}
                                filterDate={() => minDate || maxDate}
                                isCatalog={isCatalog}
                            />
                        </div>
                    </Row>
                    }
                    <Row>
                        <Col xs={{ size: 12 }}>
                            <span className='tiny-font'><Translate id="planning.title-duedate-timezone" /></span>
                        </Col>
                    </Row>

                    <Row>
                        <Col lg={{ size: 3 }} md={{ size: 3 }} xs={{ size: 12 }}>
                            <b><Translate id="planning.due-date" /></b>
                        </Col>
                        <Col className="wrapper-icon-datapicker">
                            <FontAwesomeIcon icon={faCalendarAlt} />{' '}
                            <LocalizedDatePicker
                                className="form-control"
                                selected={dueDate}
                                onChange={date => handleDueDateSelection(date)}
                                dateFormat="MM/dd/yyyy"
                                minDate={new Date(moment().add(1, 'days').format("YYYY-MM-DD HH:mm:ss"))}
                                readOnly={(planningStatus.disabled || !selectedPlanning) && isCatalog}
                                maxDate={maxDate}
                            />
                        </Col>
                    </Row>

                    <Row className="wrapper-row-labels-timezone">
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

                    {lessonId && exams.length > 0 ?
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
                                        readOnly={(planningStatus.disabled || !selectedPlanning) && isCatalog}
                                        minDate={new Date(moment().add(1, 'days').format("YYYY-MM-DD HH:mm:ss"))}
                                        maxDate={parsedMaxDate}
                                    />
                                </Col>

                                <Col className="module-time-picker col-space-adjus teacher-manag-col-flex">
                                    <label><Translate id="planning.label-field-hour" /></label>
                                    <TimePicker clearIcon={null} disabled={(planningStatus.disabled || !selectedPlanning) && isCatalog} locale={"en-EN"} openClockOnFocus={false} onChange={hour => handleHourDueDatesQuiz(hour, item.QuizId)} value={item.hour} />
                                </Col>

                                <Col className="col-space-adjus teacher-manag-col-flex">
                                    <label><Translate id="planning.label-field-timezone" /></label>
                                    <Select
                                        placeholder={"Select timezone"}
                                        classNamePrefix="mantine-like"
                                        className="mantine-like__control"
                                        options={timezoneOptions}
                                        onChange={selected => hadleTimezoneQuiz(selected, item.QuizId)}
                                        value={item.timezone}
                                    />
                                </Col>

                                <Col className="module-number-picker col-space-adjus teacher-manag-col-flex">
                                    <label><Translate id="planning.label-field-duration" /></label>
                                    <Input disabled={(planningStatus.disabled || !selectedPlanning) && isCatalog} type="number" min={15} value={item.duration} onChange={e => hadleDurationQuiz(e.target.value, item.QuizId)} />
                                </Col>
                            </Row>
                        )) : null}
                </div>}

                {(assignmentOrLesson === "1" || isCatalog) && lessonId ? <div className="pb-2">
                    <Row>
                        <Col className='clear-check mt-1'>
                            <Input disabled={(planningStatus.disabled || !selectedPlanning) && isCatalog} type="checkbox" checked={isRequiredPrePost} value={isRequiredPrePost}
                                onChange={onChangeRequiredPrePost} />{' '}
                            <span className='ml-4 check-font'><Translate id="planning.title-required-pre-post" /></span>

                        </Col>
                    </Row>
                </div> : null}

                <Row>
                    <Col xs={12}>
                        {!validDates ?
                            <Alert color="danger"><Translate id="planning.validate-dates-exams" /></Alert> : null}
                    </Col>
                    <Col className="text-right">
                        <Button className="mr-2 button-primary-custom" color="none" variant="none"
                            onClick={savePlanningDetail}
                            disabled={(lessonId === null && quizId === null) || dueDate === null || loading || (isCatalog && planningId === null) || (isCatalog && lessonDates.length === 0)}>
                            {loading ?
                                <BeatLoader
                                    size={10}
                                    color={"#e0fff6"} /> : null
                            }
                            <Translate id="planning.attach-assignment" />
                        </Button>
                    </Col>
                </Row>
                <Provider store={store}>
                    <LocalizeProvider store={store}>
                        {lessonId !== null  ?
                            <LearningGapLessonView lessonId={lessonId} />
                            : null
                        }
                    </LocalizeProvider>
                </Provider> 
            </div>
    )
}

export default withLocalize(FormPlanningTeacherManager);