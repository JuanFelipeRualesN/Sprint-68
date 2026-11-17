import React, { useState, useEffect, useCallback, useRef, useMemo } from 'react';
import { Modal, Button } from "react-bootstrap";
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Row, Col, Input, ModalHeader, ModalBody} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import { Translate } from "react-localize-redux";
import DataTable from '../../../../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import "moment-timezone";
import {
    getLearningGapDiagnosticScore,
    postLearningGapCreateOrUpdateDiagnosticScoreManually
} from "../../../../../../api/teachers";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEdit, faCheck, faTimes, faPlus} from "@fortawesome/free-solid-svg-icons";
import {PulseLoader} from "react-spinners";
import ImgAvanza from "../../../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import Settings from "../../../../../../custom-classes/settings";
import ImgGapScore from "../../../../../../assets/images/common-icons/Avanza-icon-Score.png";
import LearningGapDiagnosticForm from "../../../learning-gap-diagnostics/learning-gap-diagnostic-form";

/**
 * To handle the diagnostic score area.
 * @param translate The translation function.
 * @param studentId The student ID.
 * @param reloadData The function to reload the data.
 * @returns {Element} The diagnostic score area.
 * @constructor
 */
const LearningGapDiagnosticScoreArea = ({ studentId, translate }) => {
    const [diagnosticScoreData, setDiagnosticScoreData] = useState([]);
    const [editingRow, setEditingRow] = useState(null); // Track the currently editing row
    const [editedRow, setEditedRow] = useState({}); // Track the edited values
    const [isInitialGradeFocused, setInitialGradeFocused] = useState(false);
    const [isFinalGradeFocused, setFinalGradeFocused] = useState(false);
    const [smallLoading, setSmallLoading] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [assignDiagnosticTestShow, setAssignDiagnosticTestShow] = useState(false);

    const headerOptionsDiagnosticScore = [
        { key: "0", name: translate("learning-gap.subject") },
        { key: "1", name: translate("learning-gap.initial-diagnostic-score") },
        { key: "2", name: translate("learning-gap.final-diagnostic-score") },
        { key: "3", name: translate("learning-gap.earning") },
        { key: "4", name: "" },
    ];

    const getDiagnosticScoreDate = async () => {
        try {
            const response = await getLearningGapDiagnosticScore(studentId);

            if (response.data) {
                const newArray = response.data.map((item) => {
                    const initialScore = item.InitialDiagnosticScore !== null ? parseFloat(item.InitialDiagnosticScore) : null;
                    const finalScore = item.FinalDiagnosticScore !== null ? parseFloat(item.FinalDiagnosticScore) : null;
                    let earning = null;

                    if (initialScore !== null && finalScore !== null) {
                        earning = finalScore - initialScore;
                        earning = earning.toFixed(2);
                    }

                    return {
                        SubjectCode: `${item.SubjectName} ${item.LevelCode}`,
                        InitialDiagnosticScore: initialScore,
                        FinalDiagnosticScore: finalScore,
                        Earning: earning !== null ? (
                            <span style={{ color: earning > 0 ? 'green' : earning < 0 ? 'red' : 'black' }}>
                            {earning > 0 ? `+${earning}` : earning}
                        </span>
                        ) : null,
                        value: item.Id,
                    };
                });

                setDiagnosticScoreData(newArray);
            }
        } catch (error) {
            console.error("Error fetching diagnostic scores:", error);
        }
    };

    useEffect(() => {
        if (studentId) {
            getDiagnosticScoreDate();
        }
    }, [studentId]);


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
        if (field === "FinalDiagnosticScore") {
            setFinalGradeFocused(true);
            setInitialGradeFocused(false);
        } else {
            setInitialGradeFocused(true);
            setFinalGradeFocused(false);
        }

        // Allow only valid numeric inputs (including decimals) and empty input
        const numericRegex = Settings.getRegexScore();
        if (numericRegex.test(value)) {
            if (value === "" || parseFloat(value) <= 100) {
                setEditedRow((prev) => ({ ...prev, [field]: value }));
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
        const initialScore = editedRow.InitialDiagnosticScore ? parseFloat(editedRow.InitialDiagnosticScore) : null;
        const finalScore = editedRow.FinalDiagnosticScore ? parseFloat(editedRow.FinalDiagnosticScore) : null;

        let earning = null;
        if (initialScore !== null && finalScore !== null) {
            earning = finalScore - initialScore;
            earning = earning.toFixed(2);
        }

        setDiagnosticScoreData((prev) =>
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

        setSmallLoading(true);

        postLearningGapCreateOrUpdateDiagnosticScoreManually(studentId, {
            Id: editedRow.value,
            InitialDiagnosticScore: editedRow.InitialDiagnosticScore,
            FinalDiagnosticScore: editedRow.FinalDiagnosticScore
        }).finally(() => setSmallLoading(false));

        setEditingRow(null);
    };
    /**
     * Handles canceling the edit operation.
     */
    const handleCancel = () => {
        setEditingRow(null); // Exit edit mode without saving
        setEditedRow({});
    };

    const handleCloseDiagnosticAdminModal = async () => {
        setAssignDiagnosticTestShow(false);
        // Call the function to reload the data if needed
         await getDiagnosticScoreDate();
    }

    const tableData = useMemo(
        () =>
            diagnosticScoreData.map((row) => {
                if (row.value === editingRow) {
                    // Render editable row
                    return {
                        ...row,
                        InitialDiagnosticScore: (
                            <Input
                                type="text"
                                value={editedRow.InitialDiagnosticScore || ""}
                                onChange={(e) =>
                                    handleInputChange("InitialDiagnosticScore", e.target.value)}
                                autoFocus={isInitialGradeFocused} // Focus only when a value is entered
                            />
                        ),
                        FinalDiagnosticScore: (
                            <Input
                                type="text"
                                value={editedRow.FinalDiagnosticScore || ""}
                                onChange={(e) =>
                                    handleInputChange("FinalDiagnosticScore", e.target.value)}
                                autoFocus={isFinalGradeFocused} // Focus only when a value is entered
                            />
                        ),
                        actions: (
                            <>
                                <button
                                    className="btn btn-secondary only-icon"
                                    onClick={handleSave}
                                    style={{ marginRight: "5px" }}
                                    disabled={!editedRow.InitialDiagnosticScore || editedRow.InitialDiagnosticScore === ""}
                                >
                                    <FontAwesomeIcon
                                        icon={faCheck}
                                        style={{ color: !editedRow.InitialDiagnosticScore || editedRow.InitialDiagnosticScore === "" ? "gray" : "green" }}
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
        [diagnosticScoreData, editingRow, editedRow]
    );

    return (
        <>
            <Row>
                <Col>
                    <span className='learning-gap-subtitle'><Translate id="learning-gap.add-diagnostic-score"/></span>
                    <br/>
                    <small>
                        <Translate id="learning-gap.add-diagnostic-score-subtitle"/>
                    </small>
                </Col>
                <Col md={3} style={{textAlign: "right"}}>
                    <button
                        className="btn-default button-primary-custom"
                        onClick={() => {
                            setAssignDiagnosticTestShow(true);
                        }}
                    >
                        <FontAwesomeIcon icon={faPlus} />
                        <Translate id="learning-gap.assign-test" options={{ renderInnerHtml: true }} />
                    </button>
                </Col>
            </Row>
            { tableData.length === 0 ?
            <Row>
                <Col>
                    <div className="learning-gap-welcome-area">
                        <Row>
                            <Col xs={12} style={{textAlign: "center"}}>
                                <img src={ImgAvanza} alt="Avanza" className="img-fluid"
                                     style={{maxWidth: 50, width: "100%", opacity: "40%"}}/>
                            </Col>
                            <Col xs={12} style={{fontSize: 12, textAlign: "center"}}>
                                {' '}
                                <Translate id="learning-gap.please-assign-diagnostic-test"/>
                            </Col>
                        </Row>
                    </div>
                </Col>
            </Row>
                :
                <>

                    {

                        smallLoading ?  <PulseLoader color="#007CBA" loading={smallLoading} size={8}/> :
                            <Row>
                                <Col><DataTable headerOptions={headerOptionsDiagnosticScore} data={tableData}/>
                                </Col>
                            </Row>


                    }
                </>
            }
            <Modal show={showModal} onHide={()=>{ setEditingRow(null); setShowModal(false);}}>
                <Modal.Header closeButton>
                    <Modal.Title><Translate id="learning-gap.confirm-score-update-diagnostic"/></Modal.Title>
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

            <Modal show={assignDiagnosticTestShow} onHide={handleCloseDiagnosticAdminModal} size="xl">
                <Modal.Header closeButton>
                    <b><Translate id="learning-gap.administrate-diagnostic-test" options={{renderInnerHtml: true}}/></b>
                    {' '}
                    <img src={ImgGapScore} alt="Avanza" style={{ width: 30, marginLeft: 20 }} />
                </Modal.Header>
                <Modal.Body>
                    <LearningGapDiagnosticForm studentId={studentId} updateParent={handleCloseDiagnosticAdminModal} />
                </Modal.Body>
            </Modal>
        </>
    );
};

export default withLocalize(LearningGapDiagnosticScoreArea);
