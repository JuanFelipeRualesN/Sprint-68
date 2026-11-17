import React, {useEffect, useState} from "react";
import { Modal, Button } from "react-bootstrap";
import LearningGapRequiredSubjectsForm from "../learning-gap-annotations/learning-gap-scores-area/learning-gap-annual-score/learning-gap-required-subjects-form";
import LearningGapDiagnosticForm from "../learning-gap-diagnostics/learning-gap-diagnostic-form";
import ImageLearningGapEarningChart from "../../../../assets/svg/learning-gap-earning-chart.svg";
import ImageLearningGapGrades from "../../../../assets/svg/learning-gap-grades.svg";
import ImageLearningGapDiagnostic from "../../../../assets/svg/learning-gap-diagnostic.svg";
import ImageLearningGapThankYou from "../../../../assets/svg/learning-gap-thank-you.svg";
import LoaderFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import {
    getLearningGapDiagnosticScore, getLearningGapInitialDataSettings,
    getLearningGapReadStudentYearScore,
    getLearningGapRequiredSubjects, postLearningGapCreateOrUpdateGradeScoreManually, postLearningGapInitialDataSettings
} from "../../../../api/teachers";
import {Col, Row} from "reactstrap";
import {PulseLoader} from "react-spinners";
import {Translate} from "react-localize-redux";
const LeaningGapEarningProcessModal = ({ isOpen, toggle, studentId, updateParent }) => {
    const [step, setStep] = useState(1);
    const [grades, setGrades] = useState(null);
    const [assignDiagnosticTestShow, setAssignDiagnosticTestShow] = useState(true);
    const [requiredSubjectsToAdd, setRequiredSubjectsToAdd] = useState([]);
    const [diagnosticsCompleted, setDiagnosticsCompleted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [subjectsGrades, setSubjectsGrades] = useState([]);
    const [allowSaveScores, setAllowSaveScores] = useState(false);
    const [smallLoadingSubmitInitialGrades, setSmallLoadingSubmitInitialGrades] = useState(false);

    const handleNextStep = () => setStep(step + 1);
    const handleClose = () => {
        toggle();
        setStep(1);
    };

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {

                // Check the settings to see if the user has opted out of the process
                // If the user has opted out, skip the process
                const responseCheckInitialDataSettings =
                    await getLearningGapInitialDataSettings(studentId);

                // If not null, then the user has opted out
                if (typeof responseCheckInitialDataSettings.data !== 'undefined' &&
                    responseCheckInitialDataSettings.data !== null &&
                    responseCheckInitialDataSettings.data.StudentId !== null) {
                    toggle();
                    setLoading(false);
                    return;
                }

                let diagnosticDoneByStudent = true;
                const response = await getLearningGapRequiredSubjects();

                if (response.data) {
                    let requiredSubjectsResponse = response.data;

                    const responseYearScore = await getLearningGapReadStudentYearScore(studentId);
                    let studentYearScore = responseYearScore.data || [];

                    const responseDiagnostics = await getLearningGapDiagnosticScore(studentId);
                    let diagnosticsScore = responseDiagnostics.data || [];

                    let requiredSubjectToAddToArray = [];

                    for (let i = 0; i < requiredSubjectsResponse.length; i++) {
                        let requiredSubject = requiredSubjectsResponse[i];

                        // Check student year score
                        if (studentYearScore.length === 0) {
                            requiredSubjectToAddToArray.push(requiredSubject);
                        } else {
                            let studentScore = studentYearScore.find(s => s.SubjectCode === requiredSubject.SubjectCode);
                            if (!studentScore || studentScore.InitialGradeScore === null || studentScore.InitialGradeScore === "0") {
                                requiredSubjectToAddToArray.push(requiredSubject);
                            }
                        }

                        // Check if the required subject has a diagnostic assigned
                        // Find diagnostic.
                        let diagnosticCodeFound = diagnosticsScore.find(d => d.SubjectCode === requiredSubject.SubjectCode);
                        if (!diagnosticCodeFound) {
                            diagnosticDoneByStudent = false;
                        }
                    }

                    setRequiredSubjectsToAdd(requiredSubjectToAddToArray);

                    if (requiredSubjectToAddToArray.length === 0) {
                        if(diagnosticDoneByStudent) {
                            toggle();
                        }
                        else
                        {
                            setStep(3);
                        }
                    }
                    setDiagnosticsCompleted(diagnosticDoneByStudent);
                    setLoading(false);
                }
            } catch (error) {
                console.error("Error fetching data:", error);
                setLoading(false);
            }
        };

        fetchData();
    }, [studentId]);

    /**
     * To check if initial grades can be sent.
     * @param grades The grades to check.
     */
    const checkIfGradesCanBeSent = (grades) => {
        let allowSave = true;
        grades.forEach((grade) => {
            if (grade.Score === null || grade.Score === "") {
                allowSave = false;
            }
        });
        setSubjectsGrades(grades);
        setAllowSaveScores(allowSave);
    };

    /**
     * To submit initial grades.
     */
    const submitInitialGrades = () =>{

        let initialGradeScores = [];
        subjectsGrades.forEach((grade) => {
            initialGradeScores.push({
                SubjectCode: grade.SubjectCode,
                InitialGradeScore: grade.Score
            });
        });

        setSmallLoadingSubmitInitialGrades(true);
        // Insert the initial grades.
        postLearningGapCreateOrUpdateGradeScoreManually(studentId, initialGradeScores).then((response) => {
            if (response.data) {
                setSmallLoadingSubmitInitialGrades(false);
            }
        }).finally(() => {
            setSmallLoadingSubmitInitialGrades(false);

            if(diagnosticsCompleted){
                setStep(4);
            }
            else{
                setStep(3);
            }
        });
    }

    /**
     * To check if diagnostic tests were assigned by subject (not adding the level/grade).
     * @returns {Promise<void>}
     */
    const checkIfAllDiagnosticTestsAreAssigned = async () => {
        let diagnosticDoneByStudent = true;

        const response = await getLearningGapRequiredSubjects();
        if (response.data) {
            let requiredSubjectsResponse = response.data;
            const responseDiagnostics = await getLearningGapDiagnosticScore(studentId);
            let diagnosticsScore = responseDiagnostics.data || [];
            for (let i = 0; i < requiredSubjectsResponse.length; i++) {
                let requiredSubject = requiredSubjectsResponse[i];
                // Check if the required subject has a diagnostic assigned
                // Find diagnostic.
                let diagnosticCodeFound = diagnosticsScore.find(d => d.SubjectCode === requiredSubject.SubjectCode);
                if (!diagnosticCodeFound) {
                    diagnosticDoneByStudent = false;
                }
            }
            setDiagnosticsCompleted(diagnosticDoneByStudent);
        }

        // If completed, skip to the last step.
        if (diagnosticDoneByStudent) {
            setStep(4);
        }
    }

    /**
     * The logic about do not ask again.
     * @returns {Promise<void>}
     */
    const doNotAskAgainFunction = async () => {
        postLearningGapInitialDataSettings(studentId);
        setLoading(false);
        handleClose();
    }

    return (
         loading ?
        <LoaderFullWindow /> :

        <Modal show={isOpen} onHide={handleClose} backdrop="static" size="lg" centered>
            <Modal.Header closeButton>
            </Modal.Header>
            <Modal.Body className="d-flex flex-column justify-content-center align-items-center">
                {step === 1 && (
                    <div className="d-flex flex-column align-items-center animated learning-gap-fade-in">
                        <img src={ImageLearningGapEarningChart} alt="Learning Gap Earning Chart" className="img-fluid animated learning-gap-fade-in" style={{ maxHeight: 210 }} />
                        <h3 className="mt-3 learning-gap-quiz-results-info-area-title learning-gap-subtitle">
                            <Translate id="learning-gap.initial-data-measure-gain"/>
                        </h3>
                        <p><Translate id="learning-gap.initial-data-effective-use"/>{' '}
                            <Translate id="learning-gap.initial-data-gain-benefit"/></p>
                        <p><Translate id="learning-gap.initial-data-add-now"/></p>
                        <div className="d-flex justify-content-center gap-2">
                            <Button variant="primary" onClick={handleNextStep} style={{ marginRight: 10, width: 100 }}>
                                <Translate id="learning-gap.initial-data-yes"/>
                            </Button>
                            <Button variant="secondary" onClick={handleClose} style={{ marginRight: 10, width: 100 }}>
                                <Translate id="learning-gap.initial-data-no"/>
                            </Button>
                        </div>
                        <div style={{ width: "100%" }} className="text-right mt-3">
                            <Button variant="link" onClick={doNotAskAgainFunction}>
                                <span style={{ color: "#a5a5a5" }}>
                                      <Translate id="learning-gap.initial-data-dont-ask-again"/>
                                </span>
                            </Button>
                        </div>
                    </div>
                )}
                {step === 2 && (
                    <>
                        <img src={ImageLearningGapGrades} alt="Learning Gap Subjects"
                             className="img-fluid animated learning-gap-fade-in"
                             style={{maxHeight: 210}}/>
                        <h3 className="mt-3 learning-gap-quiz-results-info-area-title learning-gap-subtitle animated learning-gap-fade-in">
                            <Translate id="learning-gap.initial-data-what-grades"/>
                        </h3>
                        <p><Translate id="learning-gap.initial-data-grading-scale"/>{' '}
                            <Translate id="learning-gap.initial-data-grading-benefit"/>
                        </p>
                        <LearningGapRequiredSubjectsForm requiredSubjects={requiredSubjectsToAdd}
                                                         updateParent={checkIfGradesCanBeSent}/>
                        <div className="d-flex justify-content-center gap-2 animated learning-gap-fade-in">
                            <Button variant="primary" onClick={submitInitialGrades}
                                    disabled={!allowSaveScores || smallLoadingSubmitInitialGrades === true}
                                    style={{marginRight: 10, width: 100}}>
                                {smallLoadingSubmitInitialGrades ? (
                                    <PulseLoader size={10} color="#395a8f"/>
                                ) : ""}
                                <Translate id="learning-gap.initial-data-save"/></Button>
                            <Button variant="secondary" onClick={handleClose}
                                    style={{marginRight: 10, width: 100}}>
                                <Translate id="learning-gap.initial-data-cancel"/>
                            </Button>
                        </div>
                        <div style={{width: "100%"}} className="text-right mt-3">
                            <Button variant="link" onClick={doNotAskAgainFunction}>
                                <span style={{color: "#a5a5a5"}}>
                                      <Translate id="learning-gap.initial-data-dont-ask-again"/>
                                </span>
                            </Button>
                        </div>
                    </>

                )}
                {step === 3 && assignDiagnosticTestShow && (
                    <>
                        <img src={ImageLearningGapDiagnostic} alt="Learning Gap Diagnostic"
                             className="img-fluid animated learning-gap-fade-in"
                             style={{maxHeight: 210}}/>
                        <h3 className="mt-3 learning-gap-quiz-results-info-area-title learning-gap-subtitle animated learning-gap-fade-in">
                            <Translate id="learning-gap.initial-data-assign-diagnostic-test"/>
                        </h3>
                        <p>
                            <Translate id="learning-gap.initial-data-diagnostic-benefit"/>{' '}
                            <Translate id="learning-gap.initial-data-diagnostic-purpose"/>
                        </p>
                        <Row>
                            <Col>
                                <LearningGapDiagnosticForm studentId={studentId} updateProcess={() =>
                                {
                                    checkIfAllDiagnosticTestsAreAssigned();
                                }} updateParent={toggle}/>
                            </Col>
                        </Row>
                        <Row style={{width: "100%"}}>
                            <Col>
                            <div style={{width: "100%"}} className="text-right mt-3">
                                <Button variant="link" onClick={doNotAskAgainFunction}>
                                <span style={{color: "#a5a5a5"}}>
                                      <Translate id="learning-gap.initial-data-dont-ask-again"/>
                                </span>
                                </Button>
                            </div>
                            </Col>
                        </Row>
                    </>
                )}
                {step === 4 && (
                    <>
                        <img src={ImageLearningGapThankYou} alt="Learning Gap Thank you"
                             className="img-fluid animated learning-gap-fade-in"
                             style={{maxHeight: 210}}/>
                        <h3 className="mt-3 learning-gap-quiz-results-info-area-title learning-gap-subtitle animated learning-gap-fade-in">
                                <Translate id="learning-gap.initial-data-thank-you"/>
                            </h3>
                            <Translate id="learning-gap.initial-data-avanza-benefits"/>
                                <ul>
                                    <li> <Translate id="learning-gap.initial-data-record-tracking"/></li>
                                    <li><Translate id="learning-gap.initial-data-recommendations"/>
                                    </li>
                                    <li><Translate id="learning-gap.initial-data-comparative-gains"/></li>
                                </ul>
                            <div className="d-flex justify-content-center gap-2 animated learning-gap-fade-in">
                                <Button variant="secondary" onClick={handleClose}
                                        style={{marginRight: 10, width: 100}}>
                                    <Translate id="learning-gap.initial-data-close"/>
                                </Button>
                            </div>
                        </>
                    )}
            </Modal.Body>
        </Modal>
    );
};

export default LeaningGapEarningProcessModal;
