import React, { useState, useEffect, useCallback, useRef, useMemo } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Modal, Button } from "react-bootstrap";
import {Row, Col, Input} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import { Translate } from "react-localize-redux";
import DataTable from '../../../../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import "moment-timezone";
import {
    getLearningGapRequiredSubjects, postLearningGapCreateOrUpdateGradeScoreManually
} from "../../../../../../api/teachers";
import ImgAvanza from "../../../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import ImgGapScore from "../../../../../../assets/images/common-icons/Avanza-icon-Score.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit, faCheck, faTimes, faPlus} from "@fortawesome/free-solid-svg-icons";
import {PulseLoader} from "react-spinners";
import LearningGapRequiredSubjectsForm from "./learning-gap-required-subjects-form";
import Settings from "../../../../../../custom-classes/settings";

/**
 * To handle the annual score area.
 * @param translate The translation function.
 * @param studentId The student ID.
 * @param data The data related to the area.
 * @param reloadData The function to reload the data.
 * @returns {Element} The annual score area.
 * @constructor
 */
const LearningGapAnnualScoreArea = ({ studentId, translate, data, reloadData }) => {
    const [annualScoreData, setAnnualScoreData] = useState([]);
    const [editingRow, setEditingRow] = useState(null); // Track the currently editing row
    const [editedRow, setEditedRow] = useState({}); // Track the edited values
    const [isInitialGradeFocused, setInitialGradeFocused] = useState(false);
    const [isFinalGradeFocused, setFinalGradeFocused] = useState(false);
    const [smallLoading, setSmallLoading] = useState(false);
    const [smallLoadingSubmitInitialGrades, setSmallLoadingSubmitInitialGrades] = useState(false);
    const [showModalAddInitialGrade, setShowModalAddInitialGrade] = useState(false);
    const [requiredSubjects, setRequiredSubjects] = useState([]);
    const [allowSaveScores, setAllowSaveScores] = useState(false);
    const [subjectsGrades, setSubjectsGrades] = useState([]);
    const [showModal, setShowModal] = useState(false);

    const headerOptionsAnnualScore = [
        { key: "0", name: translate("learning-gap.subject") },
        { key: "1", name: translate("learning-gap.initial-grade") },
        { key: "2", name: translate("learning-gap.final-grade") },
        { key: "3", name: translate("learning-gap.earning") },
        { key: "4", name: "" },
    ];

    /**
     * Converts annotations to data table format.
     */
    const convertDataToDataTableAnnualScore = useCallback((dataToConvert) => {
        const newArray = dataToConvert.map((item) =>
            {
                const initialScore = item.InitialGradeScore !== null ? Number(item.InitialGradeScore) : null;
                const finalScore = item.FinalGradeScore !== null ? Number(item.FinalGradeScore) : null;
                let earning = null;

                if (initialScore !== null && finalScore !== null) {
                    earning = finalScore - initialScore;
                    earning = earning.toFixed(2);
                }
                return {
                    SubjectCode: item.SubjectName,
                    InitialGrade: item.InitialGradeScore,
                    FinalGradeScore: item.FinalGradeScore,
                    Earning: earning !== null ? (
                            <span style={{ color: earning > 0 ? 'green' : earning < 0 ? 'red' : 'black' }}>
                                        {earning > 0 ? `+${earning}` : earning}
                                    </span> ) : null,
                value: item.SubjectCode,
                };
            });
        setAnnualScoreData(newArray);
    }, []);

    useEffect(() => {
        if (data && Array.isArray(data)) {
            convertDataToDataTableAnnualScore(data);
        }
    }, [data, convertDataToDataTableAnnualScore]);

    /**
     * Handles the edit button click.
     */
    const handleEdit = (row) => {
        setEditingRow(row.value); // Set the ID of the row being edited
        setEditedRow({ ...row }); // Copy the row's data to editedRow
    };

    /**
     * Handles input changes for the edited row.
     */
    const handleInputChange = (field, value) => {
        if (field === "FinalGradeScore") {
            setFinalGradeFocused(true);
            setInitialGradeFocused(false);
        } else {
            setInitialGradeFocused(true);
            setFinalGradeFocused(false);
        }

        // Allow only numbers (including decimals) and empty input
        const numericRegex = Settings.getRegexScore();
        if (numericRegex.test(value)) {
            if (value === "" || parseFloat(value) <= 100) {
                // Allow empty input or numbers up to 100
                setEditedRow((prev) => ({ ...prev, [field]: value })); // Update only the specified field
            }
        }
    };

    const handleSave = () => {
        setShowModal(true); // Show the confirmation modal
    };

    /**
     * Handles saving the edited row.
     */
    const confirmSave = () => {
        setShowModal(false);
        const initialScore = editedRow.InitialGradeScore ? Number(editedRow.InitialGradeScore) : null;
        const finalScore = editedRow.FinalGradeScore ? Number(editedRow.FinalGradeScore) : null;

        let earning = null;
        if (initialScore !== null && finalScore !== null) {
            earning = finalScore - initialScore;
            earning = earning.toFixed(2);
        }


        setAnnualScoreData((prev) =>
            prev.map((row) =>
                row.value === editingRow
                    ? {
                        ...row,
                        ...editedRow,
                        Earning: earning !== null ? (
                            <span style={{ color: earning > 0 ? 'green' : earning < 0 ? 'red' : 'black' }}>
                            {earning > 0 ? `+${earning}` : earning}
                        </span>
                        ) : null
                    }
                    : row
            )
        );
        // Save the edited row to the database
        postLearningGapCreateOrUpdateGradeScoreManually(studentId, [{
            SubjectCode: editedRow.value,
            InitialGradeScore: editedRow.InitialGrade,
            FinalGradeScore: editedRow.FinalGradeScore
        }]).then((response) => {
            if (response.data) {
                reloadData();
                }
        }).finally(() => setSmallLoading(false));

        setEditingRow(null);
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
                setShowModalAddInitialGrade(false);
                reloadData();
            }
        }).finally(() => setSmallLoadingSubmitInitialGrades(false));
    }

    /**
     * Handles canceling the edit operation.
     */
    const handleCancel = () => {
        setEditingRow(null); // Exit edit mode without saving
        setEditedRow({});
    };

    const tableData = useMemo(
        () =>
            annualScoreData.map((row) => {
                if (row.value === editingRow) {
                    // Render editable row
                    return {
                        ...row,
                        InitialGrade: (
                            <Input
                                type="text"
                                value={editedRow.InitialGrade || ""}
                                onChange={(e) =>
                                    handleInputChange("InitialGrade", e.target.value)}
                                autoFocus={isInitialGradeFocused} // Focus only when a value is entered
                            />
                        ),
                        FinalGradeScore: (
                            <Input
                                type="text"
                                value={editedRow.FinalGradeScore || ""}
                                onChange={(e) => handleInputChange("FinalGradeScore", e.target.value)}
                                autoFocus={isFinalGradeFocused} // Focus only when a value is entered
                            />
                        ),
                        actions: (
                            <>
                                <button
                                    className="btn btn-secondary only-icon"
                                    onClick={handleSave}
                                    style={{ marginRight: "5px" }}
                                    disabled={!editedRow.InitialGrade || editedRow.InitialGrade.trim() === ""}
                                >
                                    <FontAwesomeIcon
                                        icon={faCheck}
                                        style={{ color: !editedRow.InitialGrade || editedRow.InitialGrade.trim() === "" ? "gray" : "green" }}
                                    />
                                </button>
                                <button
                                    className="btn btn-secondary only-icon"
                                    onClick={handleCancel}
                                >
                                    <FontAwesomeIcon
                                        icon={faTimes}
                                        style={{ color: "red" }}
                                    />
                                </button>
                            </>
                        ),
                    };
                }
                // Render normal row
                return {
                    ...row,
                    actions: (
                        <button
                            className="btn btn-secondary only-icon"
                            onClick={() => handleEdit(row)}
                        >
                            <FontAwesomeIcon icon={faEdit} />
                        </button>
                    ),
                };
            }),
        [annualScoreData, editingRow, editedRow]
    );


    /**
     * When add grades is clicked.
     */
    // Open required fields.
    const openAddGrades = () => {
        setShowModalAddInitialGrade(true);
        setSmallLoading(true);
        // Read from api.
        getLearningGapRequiredSubjects().then((response) => {
            if (response.data) {
                setRequiredSubjects(response.data);
                setSmallLoading(false);
            }
        }).finally(() => setSmallLoading(false));
    };

    /**
     * To check if grades can be sent and set the assign button as enabled or disabled.
     * @param grades The grades.
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

    return (
        <>
            <Modal show={showModalAddInitialGrade && requiredSubjects !== null
                && requiredSubjects.length > 0} onHide={() => setShowModalAddInitialGrade(false)}>
                <Modal.Header toggle={() => setShowModalAddInitialGrade(false)}>
                    <Translate id="learning-gap.assign-initial-score" options={{ renderInnerHtml: true }} />
                    {' '}
                    <img src={ImgGapScore} alt="Avanza" style={{ width: 30, marginLeft: 20 }} />
                </Modal.Header>
                <Modal.Body>
                    <LearningGapRequiredSubjectsForm requiredSubjects={requiredSubjects}
                                                     updateParent={checkIfGradesCanBeSent} />
                </Modal.Body>
                <Modal.Footer>
                    <Button color="primary"
                            onClick={()=>{submitInitialGrades()}}
                            disabled={smallLoadingSubmitInitialGrades || !allowSaveScores}>
                        {smallLoadingSubmitInitialGrades ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <>
                                <Translate id="learning-gap.assign"/></>)}
                    </Button>
                    <Button
                        color="secondary"
                        onClick={() => setShowModalAddInitialGrade(false)}
                        disabled={smallLoadingSubmitInitialGrades}
                    >
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <Translate id="learning-gap.cancel" />
                        )}
                    </Button>
                </Modal.Footer>
            </Modal>

            <Row>
                <Col md={tableData.length === 0 ? 9 : 12}>
                    <span className='learning-gap-subtitle'><Translate id="learning-gap.class-grade"/></span>
                    <br/>
                    <small><Translate id="learning-gap.add-initial-grade-explanation"/></small>
                </Col>
                {tableData.length === 0 ?
                    <Col md={3} style={{textAlign: "right"}}>
                        <button className="btn-default button-primary-custom"
                                disabled={smallLoading}
                                onClick={()=>{openAddGrades()}}>
                            {smallLoading ? (
                                <PulseLoader size={10} color="#395a8f" />
                            ) :
                                <>
                            <FontAwesomeIcon icon={faPlus} />{' '}
                            <Translate id="learning-gap.add-grades" options={{ renderInnerHtml: true }} />
                                </>
                            }
                        </button>
                    </Col> : null
                }
            </Row>
            <Row>
                <Col>
            {
        tableData.length === 0 ?

                    <div className="learning-gap-welcome-area">
                        <Row>
                            <Col xs={12} style={{ textAlign: "center" }}>
                                <img
                                    src={ImgAvanza}
                                    alt="Avanza"
                                    className="img-fluid"
                                    style={{ maxWidth: 50, width: "100%", opacity: "40%" }}
                                />
                            </Col>
                            <Col xs={12} style={{ fontSize: 12, textAlign: "center" }}>
                                <Translate id="learning-gap.add-initial-grade-explanation" options={{ renderInnerHtml: true }} />
                            </Col>
                        </Row>
                    </div>
                :
                <DataTable headerOptions={headerOptionsAnnualScore} data={tableData} />
            }
                </Col>
                </Row>
            <Modal show={showModal} onHide={()=>{ setEditingRow(null); setShowModal(false);}}>
                <Modal.Header closeButton>
                    <Modal.Title><Translate id="learning-gap.confirm-score-update"/></Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Translate id="learning-gap.warning-saving-diagnostic"/>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => { setEditingRow(null); setShowModal(false);}}>
                        <Translate id="learning-gap.cancel"/>
                    </Button>
                    <Button variant="primary" onClick={confirmSave}>
                        <Translate id="learning-gap.confirm"/>
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default withLocalize(LearningGapAnnualScoreArea);
