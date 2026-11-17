import React, { useState, useEffect } from 'react';
import { Row, Col, Button
} from 'reactstrap';
import {BeatLoader, DotLoader, PulseLoader, RingLoader} from "react-spinners";

/** Multiple Language imports **/
import { Translate, LocalizeProvider } from "react-localize-redux";
/** End of Multiple Language imports **/
import 'react-datepicker/dist/react-datepicker.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlusSquare} from "@fortawesome/free-solid-svg-icons";
import LocalizedDatePicker from '../../../../../utils/datepicker-locale';
import "react-datepicker/dist/react-datepicker.css";
import LearningGapSelectorSubject from "./learning-gap-selector-subject";
import moment from "moment";
import LanguageManager from "../../../../../custom-classes/language-manager";
import SelectorLesson from "./learning-gap-selector-lesson";
import SelectorStudentAvanza from '../../../selector/selector-student-avanza';
import LearningGapLessonView from "../learning-gap-lesson-view/learning-gap-lesson-view";
import {postLearningGapAssignLessonToStudent} from "../../../../../api/teachers";
import { Provider } from 'react-redux';
import store from '../../../../../redux/store';


/**
 * Form to add a lesson to a learning gap.
 * It's also used in the lesson catalog section to assign a lesson to a student.
 * @returns {Element} The LearningGapAddLessonForm component.
 * @constructor
 */
const LearningGapAddLessonForm = ({studentId, updateParent, isCatalog, selectedLessonProp, students}) => {
    const [selectedSubject, setSelectedSubject] = useState(null);
    const [selectedLevel, setSelectedLevel] = useState(null);
    const [selectedDate, setSelectedDate] = useState(null);
    const [selectedLesson, setSelectedLesson] = useState(null);
    const [language, setLanguage] = useState(LanguageManager.getLanguage()); // Use LanguageManager to get current language
    const [smallLoading, setSmallLoading] = useState(false);
    const [selectedStudentId, setSelectedStudentId] = useState(null);
    useEffect(() => {
        moment.locale(language); // Set moment locale globally
    }, [language]);

    const handleSubjectLevelChange = (selectedOption) => {
        setSelectedSubject(selectedOption.subject);
        setSelectedLevel(selectedOption.level);
    };

    const handleDateChange = (date) => {
        setSelectedDate(date);
    };

    const handleLessonChange = (selectedOption) => {
        setSelectedLesson(selectedOption.value);
    }

    const submitLesson = () => {
        setSmallLoading(true);
        const jsDate = new Date(selectedDate);
        const isoString = jsDate.toISOString(); // Produces: "2025-01-25T05:00:00.000Z"

        // If the component is being used in the lesson catalog section, use the selected student id from the student selector
        if (selectedStudentId) {
            postLearningGapAssignLessonToStudent(selectedStudentId, selectedLessonProp.LessonModel.Id, selectedLessonProp.LessonModel.SubjectCode, isoString).
            then(() => {
                setSmallLoading(false);
                updateParent();
            })
            .catch((error) => {
                setSmallLoading(false);
                console.error(error);
            });
            return;
        }

        postLearningGapAssignLessonToStudent(studentId, selectedLesson, selectedSubject, isoString)
            .then((response) => {
                setSmallLoading(false);
                updateParent();
            })
            .catch((error) => {
                setSmallLoading(false);
                console.error(error);
            });   
    }
    
    // Set the student id when the component is used in the lesson catalog section
    const handleStudentSelection = (studentId) => {
        setSelectedStudentId(studentId);
    }

    // Minimum date is tomorrow
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    return (
        <>
            <Row>
                <Col>
                    <b><Translate id="learning-gap.limit-date" /></b>
                </Col>
                <Col>
                    <span className='datapicker_input-step'>
                    <LocalizedDatePicker
                        className='form-control'
                        selected={selectedDate}
                        minDate={tomorrow}
                        onChange={handleDateChange}
                        dateFormat="yyyy/MM/dd" // You can change the date format
                    />
                    </span>
                </Col>
            </Row>
            <Row>
                <Col>
                    {isCatalog ? <b><Translate id="lesson-catalog.select-student"/> </b> : <b> <Translate id="learning-gap.subject-and-level"/> </b>}
                </Col>
                <Col>
                    {/* If the component is being used in the lesson catalog section, call the student selector to choose which student will be assigned the selected lesson */}
                    {isCatalog ? <SelectorStudentAvanza students={students} onStudentIdSelect={handleStudentSelection}/> : <LearningGapSelectorSubject updateParent={handleSubjectLevelChange} />}
                </Col>
            </Row>
            <Row>
                { selectedSubject && selectedLevel ?
                    <>
                <Col>
                    <b><Translate id="learning-gap.lesson"/></b>
                </Col>
                <Col>

                    <SelectorLesson subjectCode={selectedSubject} levelCode={selectedLevel}
                                    updateParent={handleLessonChange} />
                </Col>
                    </>: null }
            </Row>
            <Row>
                {selectedLesson || selectedLessonProp ?
                <Col style={{textAlign: "right"}}>
                    <Button className="btn primary-btn"
                            onClick={submitLesson}
                            disabled={selectedDate === null || smallLoading || (isCatalog && selectedStudentId === null) }>
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <>
                        <FontAwesomeIcon icon={faPlusSquare}/>  {' '}
                       <Translate id="learning-gap.assign-lesson"/></>)}
                    </Button>
                </Col> : null
                }
            </Row>
            <Row>
                <hr/>
            </Row>
            <Row>
                <Provider store={store}>
                    <LocalizeProvider store={store}>
                        {selectedLesson ?
                        <Col>
                            <LearningGapLessonView lessonId={selectedLesson} />
                        </Col> : null}
                    </LocalizeProvider>
                </Provider>
            </Row>

        </>
    );
};

export default LearningGapAddLessonForm;