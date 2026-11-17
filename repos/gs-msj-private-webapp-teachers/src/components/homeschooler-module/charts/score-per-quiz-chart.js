import React, {useState, useCallback, useEffect} from "react";
import {
    Tooltip, ResponsiveContainer, Cell, Legend, PieChart, Pie, LineChart, Line, XAxis, CartesianGrid, YAxis,
} from "recharts";
import {
    Col,
    Row, ModalHeader, ModalBody, Modal, ModalFooter
} from "reactstrap";
import {faChartLine, faEye, faPrint} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { Translate } from "react-localize-redux";
import {getQuizzesTries, getUserQuizDetails} from "../../../api/gsweb";
import TriesPerExamChart from "./tries-per-exam-chart";
import ResultsToHTMLConverter from "../../../custom-classes/results-html-converter";
import Settings from "../../../custom-classes/settings";
const COLORS = ['#98cb9a', '#d9827d'];
const COLORS_PENDING = ['#c9c9c9', '#c9c9c9'];

/**
 * To handle the quiz results of a lesson.
 * @param results The results of each quiz.
 * @returns {Element}
 * @constructor
 */
const QuizResultsCharts = ({ results }) => {
    const [loadingPdf, setLoadingPdf] = useState(false);
    const [isModalTriesOpen, setIsModalTriesOpen] = useState(false);
    const [attempts, setAttempts] = useState(null);
    const [loadingTries, setLoadingTries] = useState(false);
    const [examInfo, setExamInfo] = useState(null);
    const [pdfLocation, setPdfLocation] = useState(null);
    const [languages] = useState(Settings.LANGUAGES);
    const [isModalViewResultsOpen, setIsModalViewResultsOpen] = useState(false);
    const [prePostLineChartData, setPrePostLineChartData] = useState(null);
    const [itemPrePdf, setItemPrePdf] = useState(null);
    const [itemPostPdf, setItemPostPdf] = useState(null);
    const [examResults, setExamResults] = useState(null);

    useEffect(() => {
        if (!Array.isArray(results) || results.length === 0) {
            return;
        }

        const chartData = results
            .filter(item => item.IsExam === "2" || item.IsExam === "3")
            .map(item => ({
                name: `${item.QuizTitle} - ${item.IsExam === "2" ? "Pre" : "Post"}`,
                value: parseFloat(item.TotalPercent),
                pdfLocation: item.PdfLocation || null,
            }));

        // Set the exam results
        setExamResults(results.filter(item => item.IsExam === "1"));

        setPrePostLineChartData(chartData);
        setItemPrePdf(results.filter(item => item.IsExam === "2"));
        setItemPostPdf(results.filter(item => item.IsExam === "3"));

    }, [results]);

    // Helper to translate status
    const getTranslatedStatus = useCallback((status) => {
        if (status === 'P') {
            return <Translate id="homeschool.pending" options={{renderInnerHtml: true}}/>;
        }
        if (status === 'C') {
            return <Translate id="homeschool.completed" options={{renderInnerHtml: true}}/>;
        }
        return status; // Fallback if status is neither P nor C
    }, []);

    const handleTries = ((quizId, studentId) => {
        setLoadingTries(true);
        getQuizzesTries(quizId, studentId)
            .then(response => {
                setAttempts(response.data);
                setIsModalTriesOpen(true);
            })
            .catch(error => {
                console.error("Error tries");
            }).finally(()=>{
            setLoadingTries(false);
        });
    });

    // Function to prepare data for the PieChart
    const getPieChartData = useCallback((totalPercent) => {
        const correctPercentage = parseFloat(totalPercent);
        const incorrectPercentage = 100 - correctPercentage;

        return [
            {name: <Translate id="homeschool.correct" options={{renderInnerHtml: true}}/>, value: correctPercentage},
            {
                name: <Translate id="homeschool.incorrect" options={{renderInnerHtml: true}}/>,
                value: incorrectPercentage
            },
        ];
    }, []);

    // Custom Tooltip for Pie Chart
    const CustomTooltip = ({active, payload, label}) => {
        if (active && payload && payload.length) {
            const data = payload[0].payload; // Access the original data item
            return (
                <div className="p-2 bg-white border border-gray-300 rounded-md shadow-lg text-sm">
                    <p className="font-bold">{data.name}: {data.value}%</p>
                </div>
            );
        }
        return null;
    };

    // Function to handle PDF download
    const handleDownloadPDF = useCallback((userQuizId, studentId) => {
        setLoadingPdf(true);
        getUserQuizDetails(userQuizId, studentId)
            .then(response => {
                const resultsHtml = ResultsToHTMLConverter.getHtmlResults(
                    response.data.UserQuizFullDetails,
                    response.data.StudentName,
                    languages,
                    true
                );
                setExamInfo(resultsHtml);
                setPdfLocation(response.data.PDFLocation);
                setLoadingPdf(false);
                setIsModalViewResultsOpen(true);
            })
            .catch(error => {
                console.error("Error downloading PDF:", error);
            }).finally(()=>{
                setLoadingPdf(false);

        })
    }, []);

    return (
        <Row>
            <Col>
                {prePostLineChartData != null && prePostLineChartData.length > 0 ? (
                    <Col xs={12} className="mb-4">
                        <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                            {/* Title section at the top */}
                            {/* Header section at the top */}
                            <div className="p-2 homeschooling-results-header">
                                {/* Left: Title */}
                                <span className="text-lg font-semibold text-gray-800 lesson-results-title">
                                   <Translate id={"homeschool.pre-post-comparison"}/>
                                </span>
                                <br/>
                                {/* Right: Actions (Download and See Tries buttons) */}
                                <div> {/* Use flex and gap for spacing between buttons */}
                                    <button
                                        onClick={
                                        itemPrePdf != null && itemPrePdf.length > 0 &&
                                        itemPrePdf[itemPrePdf.length - 1].Status === "C"
                                            ?
                                            () => handleDownloadPDF(itemPrePdf[0].Id, itemPrePdf[0].UserId) : null}
                                        className={itemPrePdf && !loadingPdf &&
                                        itemPrePdf[itemPrePdf.length - 1].Status === "C" ?
                                            "button-text-xs button-edit-transparent-background cursor-pointer" :
                                            "button-text-xs button-edit-transparent-background-disabled"}
                                        disabled={loadingPdf || itemPrePdf === null} // Disable while PDF is loading
                                    >
                                        {loadingPdf ? <span
                                                className="spinner-button-small animate-spin h-4 w-4 border-2 border-current border-t-transparent rounded-full"/> :
                                            <FontAwesomeIcon icon={faEye}/>}
                                        {' '}
                                        <Translate id="homeschool.view-result-pre" options={{renderInnerHtml: true}}/>
                                    </button>
                                    <button
                                        onClick={itemPostPdf && itemPostPdf.length > 0 && itemPostPdf[itemPostPdf.length - 1].Status === "C" ?
                                            () => handleDownloadPDF(itemPostPdf[0].Id, itemPostPdf[0].UserId) : null}
                                        className={itemPostPdf && !loadingPdf  && itemPostPdf[itemPostPdf.length - 1].Status === "C" ?
                                            "button-text-xs button-edit-transparent-background cursor-pointer" :
                                            "button-text-xs button-edit-transparent-background-disabled"}
                                        disabled={loadingPdf || itemPostPdf === null} // Disable while PDF is loading
                                    >
                                        {loadingPdf ? <span
                                                className="spinner-button-small animate-spin h-4 w-4 border-2 border-current border-t-transparent rounded-full"/> :
                                            <FontAwesomeIcon icon={faEye}/>}
                                        {' '}
                                        <Translate id="homeschool.view-result-post" options={{renderInnerHtml: true}}/>
                                    </button>
                                </div>
                            </div>
                            <div className="p-3 flex flex-col items-center">
                                <ResponsiveContainer width="100%" height={300}>
                                    <LineChart width={600} height={300} data={prePostLineChartData}>
                                        <XAxis dataKey="name"/>
                                        <YAxis domain={[0, 100]}/>
                                        <Tooltip/>
                                        <Line
                                            type="monotone"
                                            dataKey="value"
                                            stroke="#8884d8"
                                            dot={{r: 4}}
                                            connectNulls={true}
                                        />
                                    </LineChart>
                                </ResponsiveContainer>
                            </div>
                        </div>
                    </Col>
                    ) :
                    <Col xs={12} className="mb-4">
                        <div className="spinner-container">
                                <span className="spinner-button-alt"/>
                            </div>
                    </Col>}
            </Col>
            {examResults && examResults.length > 0 ? (
                examResults.map((item, index) => (
                    <Col key={item.Id || index} xs={12} sm={12} md={6} className="mb-4">
                        <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                            {/* Title section at the top */}
                            {/* Header section at the top */}
                            <div
                                className="p-2 homeschooling-results-header">
                                {/* Left: Title */}
                                <span className="text-lg font-semibold text-gray-800 lesson-results-title">
                                    {item.QuizTitle}
                                </span>
                                <br/>
                                {/* Right: Actions (Download and See Tries buttons) */}
                                <div> {/* Use flex and gap for spacing between buttons */}
                                    {/* Download Results Button */}
                                    <button
                                        onClick={item.Status === 'C' && item.QuizTitle ?
                                            () => handleDownloadPDF(item.Id, item.UserId) : null}
                                        className={item.Status === 'C' && item.QuizTitle && !loadingPdf ?
                                            "button-text-xs button-edit-transparent-background cursor-pointer" :
                                            "button-text-xs button-edit-transparent-background-disabled"}
                                        disabled={loadingPdf || item.Status !== 'C' || item.QuizTitle === null} // Disable while PDF is loading
                                    >
                                        {loadingPdf ? <span
                                                className="spinner-button-small animate-spin h-4 w-4 border-2 border-current border-t-transparent rounded-full"/> :
                                            <FontAwesomeIcon icon={faEye}/>}
                                        {' '}
                                        <Translate id="homeschool.view-result" options={{renderInnerHtml: true}}/>
                                    </button>

                                    {/* See Tries Button (assuming it should also trigger PDF download or similar action) */}
                                    <button
                                        onClick={item.Status === 'C' && item.QuizTitle ?
                                            () => handleTries(item.QuizId, item.UserId) : null}
                                        className={item.Status === 'C' && item.QuizTitle && !loadingTries ?
                                            "button-text-xs button-edit-transparent-background cursor-pointer" :
                                            "button-text-xs button-edit-transparent-background-disabled"}
                                        disabled={loadingTries || item.Status !== 'C' || item.QuizTitle === null} // Disable while PDF is loading
                                    >
                                        {loadingTries ?
                                            <span
                                                className="spinner-button-small animate-spin h-4 w-4 border-2 border-current border-t-transparent rounded-full"/> :
                                            <FontAwesomeIcon icon={faChartLine}/>}
                                        {' '}
                                            <Translate id="homeschool.see-tries" options={{ renderInnerHtml: true }} />
                                        </button>
                                    </div>
                            </div>

                            {/* Content section */}
                            <div className="p-3 flex flex-col items-center">
                                {item.Status === 'C' && (
                                    <small className="text-sm text-gray-600 mb-1">
                                    <span className="font-medium">
                                        <Translate id="homeschool.score" options={{renderInnerHtml: true}}/>:
                                    </span>{' '}
                                        {item.TotalCorrect}/{item.TotalQuestions} ({item.TotalPercent}%)
                                    </small>
                                )}

                                <p className="text-sm text-gray-600 mb-1">
                                    <small>
                                        <Translate id="homeschool.status" options={{renderInnerHtml: true}}/>:
                                    </small>{' '}
                                    <small>{getTranslatedStatus(item.Status)}</small>
                                </p>

                                <ResponsiveContainer width="100%" height={200}>
                                    <PieChart>
                                        <Pie
                                            data={getPieChartData(item.TotalPercent)}
                                            cx="50%"
                                            cy="50%"
                                            innerRadius={60}
                                            outerRadius={80}
                                            paddingAngle={5}
                                            dataKey="value"
                                            labelLine={false}
                                            label={({cx, cy, midAngle, innerRadius, outerRadius, value, index}) => {
                                                if (index === 0) {
                                                    const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
                                                    const x = cx + radius * Math.cos(-midAngle * Math.PI / 180);
                                                    const y = cy + radius * Math.sin(-midAngle * Math.PI / 180);
                                                    return (
                                                        <text
                                                            x={x}
                                                            y={y}
                                                            fill="black"
                                                            textAnchor={x > cx ? 'start' : 'end'}
                                                            dominantBaseline="central"
                                                            className="font-bold text-lg"
                                                        >
                                                            {`${value}%`}
                                                        </text>
                                                    );
                                                }
                                                return null;
                                            }}
                                        >
                                            {(item.Status === "P" ? COLORS_PENDING : COLORS).map((color, idx) => (
                                                <Cell key={`cell-${idx}`} fill={color}/>
                                            ))}
                                        </Pie>
                                        {item.Status === 'C' ?
                                        <Tooltip content={<CustomTooltip/>}/> : null
                                        }
                                        <Legend/>
                                    </PieChart>
                                </ResponsiveContainer>
                            </div>
                        </div>
                    </Col>
                ))
            ) : (
                <Col>
                    <div className="spinner-container">
                        <span className="spinner-button-alt"/>
                    </div>
                </Col>
            )}
            {isModalTriesOpen ?
            <Modal isOpen={isModalTriesOpen} toggle={()=>{setIsModalTriesOpen(false)}} size="lg">
                <ModalHeader toggle={()=>{setIsModalTriesOpen(false)}}>
                    <Translate id="homeschool.tries-per-exam"
                               options={{renderInnerHtml: true}}/>
                </ModalHeader>
                <ModalBody>
                    <TriesPerExamChart attempts={attempts} />
                </ModalBody>
            </Modal> : null
            }
            {isModalViewResultsOpen ?
                <Modal isOpen={isModalViewResultsOpen} toggle={()=>{setIsModalViewResultsOpen(false)}} className="modal-results modal-progress-results">
                    <ModalHeader className="card-white" toggle={()=>{setIsModalViewResultsOpen(false)}}>
                        <Translate id="report-student-average.results" options={{ renderInnerHtml: false }} />{' '}
                        {pdfLocation && (
                            <a
                                href={pdfLocation}
                                target="_blank"
                                download={pdfLocation}
                                className="btn btn-primary"
                            >
                                <FontAwesomeIcon icon={faPrint} />{' '}
                                <Translate id="report-student-average.print" options={{ renderInnerHtml: false }} />
                            </a>
                        )}
                    </ModalHeader>
                    <ModalBody className="card-white">
                        {examInfo && <div dangerouslySetInnerHTML={{ __html: examInfo }} />}
                    </ModalBody>
                    <ModalFooter className="card-white"></ModalFooter>
                </Modal>
                : null
            }
        </Row>
    )
}

export default QuizResultsCharts;