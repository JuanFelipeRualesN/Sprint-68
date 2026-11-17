import React, { useState, useEffect } from 'react';
import {
    Row, Col, Button,
} from 'reactstrap';
import {PulseLoader} from "react-spinners";
import { ClipLoader } from "react-spinners";

/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import 'react-datepicker/dist/react-datepicker.css';
import LocalizedDatePicker from '../../../../utils/datepicker-locale';
import ImgCommunication from "../../../../assets/images/common-icons/Avanza-icon-Communication.png";
import LearningGapSelectorSubject from "../learning-gap-lessons/learning-gap-add-lesson-form/learning-gap-selector-subject";
import moment from "moment";
import LanguageManager from "../../../../custom-classes/language-manager";
import {
    getLearningGapDiagnosticPreview,
    getLearningGapDiagnosticScore,
    postLearningGapDiagnosticTestDates
} from "../../../../api/teachers";
import SelectorMultiUser from "../../../message-module/selector/selector-multi-user";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {
    faBook,
    faCalendarAlt,
    faCheckCircle, faChevronDown, faChevronUp,
    faUserGraduate,
    faUsers
} from "@fortawesome/free-solid-svg-icons";
import SelectorMultiGroups from "../../selector/selector-multi-groups";


const LearningGapDiagnosticForm = ({studentId, updateParent, updateProcess}) => {
    const [selectedSubject, setSelectedSubject] = useState(null);
    const [selectedLevel, setSelectedLevel] = useState(null);
    const [selectedInitialDate, setSelectedInitialDate] = useState(null);
    const [selectedFinalDate, setSelectedFinalDate] = useState(null);
    const [assignedDiagnosticToStudent, setAssignedDiagnosticToStudent] = useState(false);
    const [language, setLanguage] = useState(LanguageManager.getLanguage()); // Use LanguageManager to get current language
    const [smallLoading, setSmallLoading] = useState(false);
    const [studentsToSubmit, setStudentsToSubmit] = useState([{value: studentId}]);
    const [groupsToSubmit, setGroupsToSubmit] = useState([]);
    const [showMultipleSendMessage, setShowMultipleSendMessage] = useState(false);
    const [mainProcessLoading, setMainProcessLoading] = useState(false);
    const [diagnosticInitialDateChanged, setDiagnosticInitialDateChanged] = useState(false);
    const [diagnosticFinalDateChanged, setDiagnosticFinalDateChanged] = useState(false);
    const [originalDiagnosticInitialDate, setOriginalDiagnosticInitialDate] = useState(null);
    const [originalDiagnosticFinalDate, setOriginalDiagnosticFinalDate] = useState(null);
    const [diagnosticDetails, setDiagnosticDetails] = useState(null);
    const [previewOpened, setPreviewOpened] = useState(false);
    const [previewLoading, setPreviewLoading] = useState(false);
    const [diagnosticTestId, setDiagnosticTestId] = useState(null);

    useEffect(() => {
        moment.locale(language); // Set moment locale globally
    }, [language]);

    const handleSubjectLevelChange = (selectedOption) => {
        setSelectedSubject(selectedOption.subject);
        setSelectedLevel(selectedOption.level);
        setPreviewOpened(false);

        // Reset the initial and final date
        setSelectedInitialDate(null);
        setSelectedFinalDate(null);
        setAssignedDiagnosticToStudent(false);

        // Call the results to check if the student has already taken the diagnostic test.
        getDiagnosticScoreDate(selectedOption.subject, selectedOption.level);
    };

    const toggleAccordionPreview = () => {
        setPreviewOpened(!previewOpened);
        if (!previewOpened) {
            getLearningGapDiagnosticPreviewCall();
        }
    };

    const getDiagnosticScoreDate = async (subjectCode, levelCode) => {
        setSmallLoading(true);
        try {
            const response = await getLearningGapDiagnosticScore(studentId, subjectCode, levelCode);
            if (response.data !== null && response.data.length > 0) {
                const initialDateStr = response.data[0].InitialDiagnosticDateLimit;
                const finalDateStr = response.data[0].FinalDiagnosticDateLimit;
                setDiagnosticTestId(response.data[0]?.Id);
                // Convert to Date object
                const initialDate = initialDateStr ? new Date(initialDateStr) : null;
                const finalDate = finalDateStr ? new Date(finalDateStr) : null;
                setSelectedInitialDate(initialDate);
                setSelectedFinalDate(finalDate);
                setOriginalDiagnosticInitialDate(initialDate);
                setOriginalDiagnosticFinalDate(finalDate);
                setAssignedDiagnosticToStudent(true);
            }
            else
            {
                setAssignedDiagnosticToStudent(false);
            }
            setSmallLoading(false);
        } catch (error) {
            setSmallLoading(false);
        } finally {
            setSmallLoading(false);
        }
    };

    const getLearningGapDiagnosticPreviewCall = async () => {
        setPreviewLoading(true);
        setDiagnosticDetails(null);
        const diagnosticDetails = await getLearningGapDiagnosticPreview(selectedSubject, selectedLevel);
        if (diagnosticDetails != null && diagnosticDetails.data !== null && diagnosticDetails.data.DiagnosticTests != null &&
            diagnosticDetails.data.DiagnosticTests.length > 0) {
            setDiagnosticDetails(diagnosticDetails.data);
        }
        setPreviewLoading(false);
    }

    const handleInitialDateChange = (date) => {

        // Compare new date with original date
        if(originalDiagnosticInitialDate !== null && date !== null){
            const newDate = new Date(date);
            const originalDate = new Date(originalDiagnosticInitialDate);
            if(newDate !== originalDate) {
                setDiagnosticInitialDateChanged(true);
            }
            else{
                setDiagnosticInitialDateChanged(false);
            }
        }
        else{
            setDiagnosticInitialDateChanged(false);
        }

        setSelectedInitialDate(date);
    }

    const handleFinalDateChange = (date) => {

        // Compare new date with original date
        if(originalDiagnosticFinalDate !== null && date !== null){
            const newDate = new Date(date);
            const originalDate = new Date(originalDiagnosticFinalDate);
            if(newDate !== originalDate) {
                setDiagnosticFinalDateChanged(true);
            }
            else{
                setDiagnosticFinalDateChanged(false);
            }
        }
        else{
            setDiagnosticFinalDateChanged(false);
        }

        setSelectedFinalDate(date);
    }

    const setSelectedStudents = (students) => {
        setStudentsToSubmit(students);
    }

    const setSelectedGroups = (groups) => {
        setGroupsToSubmit(groups);
    }

    const submitDiagnosticTests = async () => {
        setSmallLoading(true);
        setMainProcessLoading(false);
        const jsDateInitialDate = new Date(selectedInitialDate);
        const isoStringInitialDate = jsDateInitialDate.toISOString(); // Produces: "2025-01-25T05:00:00.000Z"

        const jsDateFinalDate = new Date(selectedFinalDate);
        const isoStringFinalDate = jsDateFinalDate.toISOString(); // Produces: "2025-01-25T05:00:00.000Z"

        // Set students to submit as array with only the studentId
        let studentsIdsToSubmit = [];
        if(studentsToSubmit != null && studentsToSubmit.length > 0){
            studentsIdsToSubmit = studentsToSubmit.map(student => student.value);
        }

        // Set groups to submit as array with only the groupId
        let groupsIdsToSubmit = [];
        if(groupsToSubmit != null && groupsToSubmit.length > 0){
            groupsIdsToSubmit = groupsToSubmit.map(group => group.value);
        }

        // If multiple students are selected, show the multiple send message
        if(studentsIdsToSubmit.length > 1 || groupsIdsToSubmit.length > 0){
            setShowMultipleSendMessage(true);
            setMainProcessLoading(true);
        }

        await postLearningGapDiagnosticTestDates(diagnosticTestId, studentsIdsToSubmit, groupsIdsToSubmit, selectedSubject, selectedLevel, isoStringInitialDate,
            isoStringFinalDate, diagnosticInitialDateChanged, diagnosticFinalDateChanged)
            .then((response) => {
                setSmallLoading(false);
                setMainProcessLoading(false);
                // Reload
                getDiagnosticScoreDate(selectedSubject, selectedLevel);

            })
            .catch((error) => {
                setSmallLoading(false);
                setMainProcessLoading(false);

            }).finally(() => {
                setSmallLoading(false);
                setMainProcessLoading(false);
                if(updateProcess !== undefined && updateProcess !== null) {
                    updateProcess();
                }
            });
    }

    // Minimum date is tomorrow
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    return (
        showMultipleSendMessage ?
            <div className="fade-in">
                <Row>
                    <Col style={{textAlign: "center"}}>
                        <img src={ImgCommunication} alt="Communication" style={{maxWidth: 200}}/>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Translate id="learning-gap.message-learning-group"/>
                    </Col>
                </Row>
                <Row>
                    <Col style={{textAlign: "center"}}>
                        <br/>
                        <b> <span> <Translate id="learning-gap.status"/></span>: </b>
                        {mainProcessLoading ? <>
                                <ClipLoader size={15} color={"#123abc"} loading={true}/>
                                {' '} <span> <Translate id="learning-gap.processing"/></span>
                            </>
                            : <>
                                <span style={{color: "green", fontSize: "18px", fontWeight: "bold"}}>✅</span>
                                {' '} <span> <Translate id="learning-gap.completed"/></span>
                            </>}
                    </Col>
                </Row>

            </div> :
            <>
            <Row>
                    <Col>
                        <FontAwesomeIcon icon={faBook} color={"gray"} />{' '}
                    <b><Translate id="learning-gap.subject-and-level"/> </b>
                </Col>
                <Col>
                    <LearningGapSelectorSubject updateParent={handleSubjectLevelChange} />
                </Col>
            </Row>
            <Row>
                <Col>
                    <FontAwesomeIcon icon={faCalendarAlt} color={"gray"} />{' '}
                    <Translate id="learning-gap.initial-date-diagnostic-test" options={{renderInnerHtml: true}}/>
                </Col>

                <Col>
                    <span className='datapicker_input-step'>
                        {smallLoading ? <PulseLoader color={"#007bff"} loading={smallLoading} size={10} /> :
                        <LocalizedDatePicker
                            disabled={selectedSubject === null}
                            className='form-control'
                            selected={selectedInitialDate ?? null}
                            minDate={tomorrow}
                            onChange={handleInitialDateChange}
                            dateFormat="yyyy/MM/dd" // You can change the date format
                            onChangeRaw={(e) => {
                                e.preventDefault();
                            }}
                        />
                        }
                    </span>
                </Col>
            </Row>
            <Row>
                <Col>
                    <FontAwesomeIcon icon={faCalendarAlt} color={"gray"} />{' '}
                    <Translate id="learning-gap.final-date-diagnostic-test" options={{renderInnerHtml: true}}/>
                </Col>

                <Col>
                    <span className='datapicker_input-step'>
                         {smallLoading ?
                             <PulseLoader color={"#007bff"} loading={smallLoading} size={10} /> :
                                <LocalizedDatePicker
                                    disabled={selectedSubject === null}
                                    className='form-control'
                                    selected={selectedFinalDate ?? null}
                                    minDate={tomorrow}
                                    onChange={handleFinalDateChange}
                                    dateFormat="yyyy/MM/dd" // You can change the date format
                                    onChangeRaw={(e) => {
                                        e.preventDefault();
                                    }}
                                />
                         }
                    </span>
                </Col>
            </Row>
            {selectedSubject !== null ?
            <Row>
                <Col>
                    <b><Translate id="learning-gap.assigned-to-student"/></b>
                </Col>
                <Col>
                    {smallLoading ? <PulseLoader color={"#007bff"} loading={smallLoading} size={10} /> :
                        assignedDiagnosticToStudent ? <Translate id="learning-gap.yes"/> : <Translate id="learning-gap.no"/>
                    }
                </Col>
            </Row> : null
            }
            <Row>
                <Col>
                    <FontAwesomeIcon icon={faUsers}  color={"gray"} />{' '}
                    <b><Translate id="learning-gap.assign-to-group"/></b></Col>
                <Col> <SelectorMultiGroups updateParent={setSelectedGroups} /></Col>
            </Row>
            <Row>
                <Col>
                    <FontAwesomeIcon icon={faUserGraduate}  color={"gray"} />{' '}
                    <b><Translate id="learning-gap.assign-to-student"/></b>
                </Col>
                <Col><SelectorMultiUser updateParent={setSelectedStudents} selectedDefault={[studentId]} /></Col>
            </Row>
            <Row>
                <Col style={{textAlign: "right"}}>
                    <button
                        className="btn-default button-secondary-custom txt-uppercase learning-gap-case-none"
                        disabled={previewLoading || selectedSubject === null}
                        onClick={toggleAccordionPreview}
                        style={{ marginBottom: 10, marginRight: 10}}
                    >
                        {previewLoading ? (
                            <span>
                        <ClipLoader color={"#ececec"} loading={previewLoading} size={10} />{" "}
                                <Translate id="learning-gap.preview" options={{ renderInnerHtml: true }} />
                    </span>
                        ) : (
                            <span>
                        <Translate id="learning-gap.preview" options={{ renderInnerHtml: true }} />
                                {" "}
                                <FontAwesomeIcon icon={previewOpened ?
                                    faChevronUp : faChevronDown} />
                    </span>
                        )}
                    </button>
                    <button type="submit" className="btn-default button-primary-custom mr-2 learning-gap-case-none"
                            disabled={
                        selectedSubject === null ||
                                selectedInitialDate === null ||
                                selectedFinalDate === null || smallLoading ||
                                (!studentsToSubmit?.length && !groupsToSubmit?.length)}
                            onClick={submitDiagnosticTests} style={{marginBottom: 10}}>
                        <Translate id="learning-gap.assign" options={{renderInnerHtml: true}} />
                    </button>
                    <button className="learning-gap-case-none btn-default button-secondary-custom"
                            onClick={ ()=> updateParent(false)} style={{marginBottom: 10}}>
                        <Translate id="learning-gap.cancel" options={{renderInnerHtml: true}} />
                    </button>
                </Col>
            </Row>
                {previewOpened ? (previewLoading ?
                    <Row style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100%" }}>
                        <ClipLoader size={55} color={"#416872"} loading={true} />
                    </Row> : <Row>
                    <Col>
                        <hr/>
                        {typeof diagnosticDetails !== 'undefined' && diagnosticDetails !== null ?
                        <div className="quiz-details" style={{padding: "10px", backgroundColor: "#e7e7e7", borderRadius: 10, overflow: "auto",  height: 380}}>
                            <strong><Translate
                                id="learning-gap.subject"/>:</strong> {diagnosticDetails?.SubjectName} - {diagnosticDetails?.LevelName}
                            <br/>
                            <strong><Translate
                                id="learning-gap.instruction"/>:</strong> {diagnosticDetails?.DiagnosticTests[0]?.Instructions}
                            <br/>
                            {diagnosticDetails?.DiagnosticTests?.[0]?.ExtraInfo ? (
                                <>
                                    <strong><Translate id="learning-gap.extra-info"/>:</strong>
                                    <div
                                        dangerouslySetInnerHTML={{
                                            __html: diagnosticDetails.DiagnosticTests[0].ExtraInfo,
                                        }}
                                    />
                                    <br/>
                                </>
                            ) : null}

                            <strong><Translate
                                id="learning-gap.questions"/>:</strong>
                            {diagnosticDetails?.DiagnosticTests[0].Questions.map((question, questionIndex) => (
                                <div key={questionIndex} style={{marginBottom: "10px"}}>
                                    <div
                                        className="question-details"
                                        style={{
                                            padding: "10px",
                                            backgroundColor: "#f9f9f9",
                                            border: "1px solid #ccc",
                                        }}
                                    >
                                        <div
                                            dangerouslySetInnerHTML={{
                                                __html: question.Description,
                                            }}
                                        />
                                        <h6 style={{marginTop: 4}}>{question.Responses != null &&
                                        question.Responses.length > 0 ?
                                            <Translate id="learning-gap.responses"/> : null}:</h6>
                                        <ul>
                                            {question.Responses.map(
                                                (response, responseIndex) => (
                                                    <li key={responseIndex}
                                                        style={{display: "flex", alignItems: "center"}}>

                                                        <div
                                                            dangerouslySetInnerHTML={{
                                                                __html: response.Description,
                                                            }}
                                                            style={{marginRight: "10px"}}
                                                        />

                                                        {response.IsCorrect === 1 && (
                                                            <FontAwesomeIcon
                                                                icon={faCheckCircle}
                                                                style={{color: "green"}}
                                                            />
                                                        )}
                                                    </li>
                                                )
                                            )}
                                        </ul>
                                    </div>
                                </div>
                            ))}
                        </div>
                        :  <Translate id="learning-gap.no-preview-available" options={{renderInnerHtml: true}} />}
                    </Col>
                </Row>) : null}
            </>
    );
};

export default LearningGapDiagnosticForm;