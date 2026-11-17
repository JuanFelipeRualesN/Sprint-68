import React, { useState, useContext } from "react";
import { Table, Input, Button } from "reactstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {faEdit, faCheck, faTimes, faPrint} from "@fortawesome/free-solid-svg-icons";
import {Translate} from "react-localize-redux";
import {
    postLearningGapAssignManualQuizToStudent
} from "../../../../../api/teachers";
import {getUserQuizDetails} from "../../../../../api/gsweb";
import Settings from "../../../../../custom-classes/settings";
import {PulseLoader} from "react-spinners";
import { AppContext } from "../../../../../App.Context";
import LoadingFullWindow from "../../../../shared-module-components/loader-windows/loader-full-window";

/**
 * To handle the score per quiz.
 * @param data The data to show.
 * @param studentId The id of the student.
 * @param learningGapAssignedLessonId The assigned lesson id.
 * @returns {Element} Returns the element.
 * @constructor
 */
const LearningGapEditableScoreTable = ({ data, studentId, learningGapAssignedLessonId, setHasSubmittedState }) => {
    const [editingRow, setEditingRow] = useState(null); // Track which row is being edited
    const [scores, setScores] = useState(data); // Track scores for the table
    const [originalScores, setOriginalScores] = useState({}); // Store original scores
    const [smallLoading, setSmallLoading] = useState(false); // Loading state for print result
    const [isLoading, setIsLoading] = useState(false); // Loading state for quiz results
    const {refreshNotifications} = useContext(AppContext);

    // Handle editing
    const handleEdit = (quizId) => {
        const originalScore = scores.find(row => row.QuizId === quizId)?.Score;
        setOriginalScores(prev => ({ ...prev, [quizId]: originalScore })); // Store original score
        setEditingRow(quizId);
    };

    // Handle score input change
    const handleInputChange = (quizId, value) => {
        setScores((prevScores) =>
            prevScores.map((row) =>
                row.QuizId === quizId ? { ...row, Score: value } : row
            )
        );
    };

    // Handle save action
    const handleSave = (quizId) => {
        const rowToSave = scores.find((row) => row.QuizId === quizId);
        saveStudentScore(quizId, studentId); // Call the save function
        setEditingRow(null); // Exit edit mode
    };

    // Handle cancel action
    const handleCancel = () => {
        if (editingRow !== null) {
            setScores((prevScores) =>
                prevScores.map((row) =>
                    row.QuizId === editingRow ? { ...row, Score: originalScores[editingRow] } : row
                )
            );
        }
        setEditingRow(null); // Exit edit mode
    };

    // Function to save the student score (implement as needed)
    const saveStudentScore = async (quizId, studentId) => {
        try {
            setIsLoading(true);
            // Add your API call or logic to save the score
            await postLearningGapAssignManualQuizToStudent(learningGapAssignedLessonId, studentId, quizId, scores.find((row) => row.QuizId === quizId).Score);           
        } catch (error) {
            console.error("Error changing quiz score:", error);  
        } finally {
            // Reset the loading state and refresh notifications after changing the score
            setHasSubmittedState?.(true)
            refreshNotifications();
            setIsLoading(false);
        }
    };

    // To print result.
    const printResult = (gsWebResultId) => {
        setSmallLoading(true);
        getUserQuizDetails(gsWebResultId, studentId).then(response => {
            if (response) {
                window.open(response.data.PDFLocation, '_blank');
            } else {
                console.log("Not available");
            }
        }).catch(error => {
            console.error("Error fetching recommendation:", error);
        }).finally(() => setSmallLoading(false));
    };

    return (
    <>
        <Table striped>
            <thead>
            <tr>
                <th><Translate id="learning-gap.quiz-title" options={{renderInnerHtml: true}}/></th>
                <th><Translate id="learning-gap.score" options={{renderInnerHtml: true}}/></th>
                <th><Translate id="learning-gap.edit" options={{renderInnerHtml: true}}/></th>
                <th></th>
            </tr>
            </thead>
            <tbody>
            {scores.map((row) => (
                <tr key={row.QuizId}>
                    <td>{row.QuizTitle}</td>
                    <td>
                        {editingRow === row.QuizId ? (
                            <Input
                                type="text"
                                value={row.Score}
                                style={{ maxWidth: 80 }}
                                onChange={(e) => {
                                    const value = e.target.value;
                                    if (Settings.getRegexScore().test(value) && value <= 100) {
                                        handleInputChange(row.QuizId, value);
                                    }
                                }}
                            />
                        ) : (
                            row.Score
                        )}
                    </td>
                    <td>
                        {editingRow === row.QuizId ? (
                            <>
                                <Button
                                    className="btn btn-secondary only-icon"
                                    size="sm"
                                    onClick={() => handleSave(row.QuizId)}
                                    disabled={!row.Score || row.Score === ""}
                                >
                                    <FontAwesomeIcon icon={faCheck} color={!row.Score || row.Score === "" ?
                                    "gray": "green"} />
                                </Button>{" "}
                                <Button
                                    style={{ marginLeft: 5 }}
                                    className="btn only-icon btn-danger"
                                    size="sm"
                                    onClick={handleCancel}
                                >
                                    <FontAwesomeIcon icon={faTimes} color="red" />
                                </Button>
                            </>
                        ) : (
                            <Button
                                color="primary"
                                size="sm"
                                onClick={() => handleEdit(row.QuizId)}
                            >
                                <FontAwesomeIcon icon={faEdit} />
                            </Button>
                        )}
                    </td>
                    <td>
                        {row.GswebUsquiResultsId !== null  ?
                        <>
                        <Button
                            className="btn btn-secondary only-icon"
                            size="sm"
                            disabled={smallLoading}
                            onClick={() => {printResult(row.GswebUsquiResultsId)}}
                        >
                            {smallLoading ?  <PulseLoader size={5} color="#395a8f" /> :
                                <>
                            <FontAwesomeIcon icon={faPrint} color={!row.Score || row.Score === "" ?
                            "gray": "green"} />
                                </>}
                        </Button>{" "} </> : null
                        }
                    </td>
                </tr>
            ))}

            </tbody>
        </Table>
        {isLoading && <LoadingFullWindow />}
    </>
    );
};

export default LearningGapEditableScoreTable;
