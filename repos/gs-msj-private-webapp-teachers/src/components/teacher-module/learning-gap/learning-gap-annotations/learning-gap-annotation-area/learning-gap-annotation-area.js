import React, { useState, useEffect, useCallback } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col, Modal, ModalHeader, ModalBody, ModalFooter, Input} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import { Translate } from "react-localize-redux";
import DataTable from '../../../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import "moment-timezone";
import {
    getLearningGapStudentAnnotations,
    postLearningGapStudentAnnotations
} from "../../../../../api/teachers";
import LanguageManager from "../../../../../custom-classes/language-manager";
import ImgAvanza from "../../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus} from "@fortawesome/free-solid-svg-icons";
import {PulseLoader} from "react-spinners";
import PaginationCustom from "../../../../shared-module-components/datatables/pagination";

/**
 * To handle the learning gap annotation area where the teacher submit the annotations for the student.
 * @param translate The translate function.
 * @param studentId The student ID.
 * @returns {Element} The learning gap annotation area.
 * @constructor
 */
const LearningGapAnnotationArea = ({ translate, studentId }) => {

    const [data, setData] = useState([]);
    const [totalPages, setTotalPages] = useState(0);
    const [limiter, setLimiter] = useState(5);
    const [currentPage, setCurrentPage] = useState(1);
    const [annotationText, setAnnotationText] = useState("");
    const [addAnnotationModalShow, setAddAnnotationModalShow] = useState(false);
    const [loading, setLoading] = useState(false);
    const [smallLoading, setSmallLoading] = useState(false);
    const [userLanguage, setUserLanguage] = useState(LanguageManager.getLanguage());
    const maxLength = 2000;

    const headerOptions = [
        { key: "0", name: translate("learning-gap.created-date") },
        { key: "1", name: translate("learning-gap.annotation") },
    ];

    /**
     * Converts annotations to data table format.
     */
    const convertDataToDataTable = useCallback((dataToConvert) => {
        const newArray = dataToConvert.map(item => {
            // Get the user's timezone
            const userTimezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
            let createdAtServer = new Date(`${item.CreatedAt} UTC`);
            // Parse the UTC date string into a Date object
            const createdAtDate = new Date(createdAtServer); // Assuming the CreatedAt is in UTC (e.g., '2025-01-26T01:19:54Z')

            // Convert the date to the user's local timezone using Intl.DateTimeFormat
            const createdAtInUserTimeZone = createdAtDate.toLocaleString(userLanguage, {
                timeZone: userTimezone,  // Convert to the user's timezone
                year: 'numeric',
                month: 'long',  // Full month name (e.g., January, Enero)
                day: 'numeric',
                hour: 'numeric',
                minute: 'numeric',
                second: 'numeric',
                hour12: true // Use 12-hour format with AM/PM
            });

            return {
                CreatedAt: createdAtInUserTimeZone, // Store the formatted date
                Text: item.Text,
                value: item.Id
            };
        });

        setData(newArray);
    }, [userLanguage]);

    /**
     * To call the annotations.
     * @type {(function(): void)|*}
     */
    const callStudentAnnotations = useCallback(() => {
        setLoading(true);
        let fromPage = currentPage - 1;
        if(fromPage <= 0)
            fromPage = 0;

        getLearningGapStudentAnnotations(studentId, fromPage, limiter).then(response => {
            setLoading(false);
            convertDataToDataTable(response.data.Items);
            setTotalPages(response.data.TotalPages)
        });
    }, [studentId, currentPage, limiter, convertDataToDataTable]);

    useEffect(() => {
        callStudentAnnotations();
    }, [studentId, currentPage, limiter]);


    /**
     * Handles pagination click.
     */
    const handlePageClick = useCallback((page) => {
        if (page !== currentPage) setCurrentPage(page);
    }, [currentPage]);

    // Use useEffect to trigger callStudentAnnotations when currentPage changes
    useEffect(() => {

        if (currentPage !== null) {
            callStudentAnnotations();
        }
    }, [studentId, currentPage, limiter]);

    /**
     * Submits a new annotation.
     */
    const submitAnnotationRequest = () => {
        setSmallLoading(true);
        postLearningGapStudentAnnotations(studentId, annotationText)
            .then(() => {
                setAddAnnotationModalShow(false);
                callStudentAnnotations();
                setSmallLoading(false);
            })
            .catch(() => setSmallLoading(false));
    };

    return (
        <>
        {loading ? (
            <div
                style={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    width: "100%",
                    height: "10vh",
                }}
            >
                <PulseLoader size={10} color="#395a8f" />
            </div>
        ) : (
            <>
            <Row>
                <Col>
                     <span className='learning-gap-subtitle'>
                                <Translate id="learning-gap.annotations"/></span>
                    <br/>
                    <small>
                    <Translate id="learning-gap.add-annotations-of-student-progress"/>
                    </small>
                </Col>
                <Col md={3} style={{textAlign: "right"}}>
                    <button
                        className="btn-default button-primary-custom"
                        onClick={() => {
                            setAnnotationText("");
                            setAddAnnotationModalShow(true);
                        }}
                    >
                        <FontAwesomeIcon icon={faPlus} />
                        <Translate id="learning-gap.add-annotation" options={{ renderInnerHtml: true }} />
                    </button>
                </Col>
            </Row>
                    {data.length === 0 ? (
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
                                    <Translate id="learning-gap.no-annotations" options={{ renderInnerHtml: true }} />
                                </Col>
                            </Row>
                        </div>
                    ) : (
                        <Row>
                            <Col>
                            <DataTable textAlign={"left"} headerOptions={headerOptions} data={data} />
                            <PaginationCustom
                                totalPages={totalPages}
                                currentPage={currentPage}
                                updateParent={handlePageClick}
                                dapper={false}
                            />
                            </Col>
                        </Row>
                    )}
                </>
            )}
            <Modal isOpen={addAnnotationModalShow} toggle={() => setAddAnnotationModalShow(false)}>
                <ModalHeader toggle={() => setAddAnnotationModalShow(false)}>
                    <Translate id="learning-gap.annotations" options={{ renderInnerHtml: true }} />
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col>
                            <Translate id="learning-gap.add-the-annotation" options={{ renderInnerHtml: true }} />
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Input
                                maxLength={maxLength}
                                type="textarea"
                                value={annotationText}
                                rows="5"
                                onChange={(e) => setAnnotationText(e.target.value)}
                            />
                            <div style={{textAlign: "right", marginTop: "5px", fontSize: "0.9em", color: "#666"}}>
                                {annotationText.length}/{maxLength}
                            </div>
                        </Col>
                    </Row>
                </ModalBody>
                <ModalFooter>
                    <Button
                        color="primary"
                        onClick={submitAnnotationRequest}
                        disabled={smallLoading || !annotationText.trim()}
                    >
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <Translate id="learning-gap.submit" />
                        )}
                    </Button>
                    <Button
                        color="secondary"
                        onClick={() => setAddAnnotationModalShow(false)}
                        disabled={smallLoading}
                    >
                        {smallLoading ? (
                            <PulseLoader size={10} color="#395a8f" />
                        ) : (
                            <Translate id="learning-gap.cancel" />
                        )}
                    </Button>
                </ModalFooter>
            </Modal>
        </>
    );
};

export default withLocalize(LearningGapAnnotationArea);

