import React, { useState, useEffect } from "react";
import {
    Col,
    CardBody,
    Row, Card,
    Modal, ModalHeader, ModalBody
} from "reactstrap";
import "./style.css";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import "./style.css";
import QuizResultsCharts from "../charts/score-per-quiz-chart";
import {getProfileLessonAverageDetails} from "../../../api/gsweb";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowLeft} from "@fortawesome/free-solid-svg-icons";
import {useLocation} from "react-router-dom";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
/**
 * Class/component of the student average
 */
const RecordHomeschoolLesson = () => {

    const { search } = useLocation();
    const query = new URLSearchParams(search);
    const [dataLessonDetailsResults, setDataLessonDetailsResults] = useState(null);

    const studentId = query.get('studentId');
    const lessonId = query.get('lessonId');
    const lessonTitle = query.get('lessonTitle');

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        setDataLessonDetailsResults(fetchLessonAverageDetails());
    },[]);

    const fetchLessonAverageDetails = async () => {
        try {
            const response = await getProfileLessonAverageDetails(studentId, lessonId);
            setDataLessonDetailsResults(response.data);
            return response.data;
        } catch (error) {
            console.error("Error fetching lesson average details:", error);
            return [];
        }
    }

    const goBackToRecordHomeschool = () => {
        window.history.back();
    }

    return (
        <div>
            <div className="container-padding animated fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col mb-3">
                        <div className="col col-no-padding-left">
                            <a className="btn-back" onClick={goBackToRecordHomeschool}>
                                <FontAwesomeIcon icon={faArrowLeft}/>{" "}
                                <Translate id="general.back-action"/>
                            </a>
                            <div className="title-principal mt-3">
                                  <span className="title-principal--inside">
                                    {lessonTitle}
                                  </span>
                            </div>
                        </div>
                    </div>
                    <Row>
                        <Col xs={12}>
                            <h2 className="text-2xl font-bold mb-6 text-gray-800 lesson-results-title">
                                <Translate id="homeschool.results-per-quiz" options={{renderInnerHtml: true}}/>
                            </h2>
                            <small>
                                <Translate id="homeschool.results-per-quiz-subtitle" options={{renderInnerHtml: true}}/>
                            </small>
                        </Col>
                    </Row>
                    {dataLessonDetailsResults ?
                     <QuizResultsCharts results={dataLessonDetailsResults}/> : null}
                </div>
            </div>
        </div>
    );
};

export default withLocalize(RecordHomeschoolLesson);
