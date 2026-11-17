import React, { useState, useEffect, useCallback } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col, Modal, ModalHeader, ModalBody, ModalFooter, Input, Table} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import { Translate } from "react-localize-redux";
import DataTable from '../../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    getLearningGapStudentRecommendationPrint,
    getLearningGapStudentRecommendations
} from "../../../../api/teachers";
import LanguageManager from "../../../../custom-classes/language-manager";
import ImgAvanza from "../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {
    faEye,
    faPrint
} from "@fortawesome/free-solid-svg-icons";
import {PulseLoader} from "react-spinners";
import PaginationCustom from "../../../shared-module-components/datatables/pagination";
import iconLearningGapBooks from "../../../../assets/images/common-icons/Avanza-icon-Books.png";
import { getAsset } from '../../../../utils/getAssets';

const IconAvanzaBanner = getAsset('Avanza-icon-Banner');

/**
 * To display the recommendations for the student.
 * @param translate The translations.
 * @param studentId The id of the student.
 * @param redoSearch The redo search flag.
 * @returns {Element} The recommendations.
 * @constructor
 */
const LearningGapRecommendations = ({ translate, studentId, redoSearch }) => {

    const [data, setData] = useState([]);
    const [totalPages, setTotalPages] = useState(0);
    const [limiter, setLimiter] = useState(10);
    const [currentPage, setCurrentPage] = useState(1);
    const [loading, setLoading] = useState(false);
    const [userLanguage, setUserLanguage] = useState(LanguageManager.getLanguage());
    const [isRecommendationModalOpened, setRecommendationModalOpened] = useState(false);
    const [itemSelected, setItemSelected] = useState({});
    const [smallLoading, setSmallLoading] = useState(false);

    const headerOptions = [
        { key: "0", name: translate("learning-gap.lesson") },
        { key: "1", name: translate("learning-gap.competency") },
        { key: "2", name: translate("learning-gap.status") },
    ];

    /**
     * Converts Recommendations to data table format.
     */
    const convertDataToDataTable = useCallback((dataToConvert) => {
        const newArray = dataToConvert.map(item => {

            return {
                Lesson: item.LessonTitle === null ? <Translate id="learning-gap.not-found" options={{renderInnerHtml: true}}/> : item.LessonTitle,
                KeyWord: item.KeyWord === null ? <Translate id="learning-gap.not-found" options={{renderInnerHtml: true}}/> : item.KeyWord,
                View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(item)}
                    disabled={item.KeyWord === null}>
                    <FontAwesomeIcon icon={faEye} />{' '}
                    <Translate id="learning-gap.view-learning-sheet" options={{renderInnerHtml: true}}/>
                </button>,
                value: item.Id
            };
        });

        setData(newArray);
    }, [userLanguage]);

    /**
     * To call recommendations.
     * @type {(function(): void)|*}
     */
    const callStudentRecommendations = useCallback(() => {
        setLoading(true);
        let fromPage = currentPage - 1;
        if(fromPage <= 0)
            fromPage = 0;

        getLearningGapStudentRecommendations(studentId, fromPage, limiter).then(response => {
            setLoading(false);
            convertDataToDataTable(response.data.Items);
            setTotalPages(response.data.TotalPages)
        });
    }, [studentId, currentPage, limiter, convertDataToDataTable]);

    useEffect(() => {
        callStudentRecommendations();
    }, [studentId, currentPage, limiter]);

    const printRecommendation = () => {
        setSmallLoading(true);
        getLearningGapStudentRecommendationPrint(itemSelected.Id).then(response => {
            if (response && isValidURL(response.data)) {
                window.open(response.data, '_blank');
            } else {
                console.log("Not available");
            }
        }).catch(error => {
            console.error("Error fetching recommendation:", error);
        }).finally(()=> setSmallLoading(false));
    };

    // Helper function to check if the response is a valid URL
    const isValidURL = (url) => {
        try {
            new URL(url);
            return true;
        } catch (_) {
            return false;
        }
    };

    /* Handles pagination click. */
    const handlePageClick = useCallback((page) => {
        if (page !== currentPage) setCurrentPage(page);
    }, [currentPage]);

    const handleRowClick = (item) => {
        setRecommendationModalOpened(true);
        setItemSelected(item);
    }

    return (
        <div className="learning-gap-fade-in">
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
            ) : data.length === 0 ? (
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
                        <Translate id="learning-gap.recommendations-pending" options={{ renderInnerHtml: true }} />
                    </Col>
                </Row>
                </div>
            ) : (
                <Row>
                    <Col>
                         <span className='learning-gap-subtitle'>
                            <Translate id="learning-gap.recommended-learning-sheets-by-lessons"/>
                        </span>
                        <DataTable headerOptions={headerOptions} data={data} />
                        <PaginationCustom
                            totalPages={totalPages}
                            currentPage={currentPage}
                            updateParent={handlePageClick}
                            dapper={false}
                        />
                    </Col>
                </Row>
            )}
            <Modal isOpen={isRecommendationModalOpened}
                   toggle={() => setRecommendationModalOpened(false)} size="xl">
                <ModalHeader toggle={() => setRecommendationModalOpened(false)}/>
                <ModalBody style={{width: "90% !important"}}>
                    <Row className="align-items-center">
                        <Col xs={9} className="d-flex align-items-center">
                            <img src={iconLearningGapBooks} alt="Books" style={{ width: 50 }} />
                            <span style={{ fontSize: "22px", marginLeft: "15px" }}>
                                <b><Translate id="learning-gap.recommended-learning-sheet-to-work-on-gap" /></b>
                            </span>
                        </Col>
                        <Col className="text-right">
                            {itemSelected.KeyWord !== "Not found" && itemSelected.KeyWord !== "No encontrado" ?
                            <Button color="primary"
                                    disabled={smallLoading}
                                    className="btn primary-btn btn-secondary"
                                    onClick={() => printRecommendation()}>
                                {smallLoading ? <PulseLoader size={10} color="#fff" /> :
                                    <>
                                <FontAwesomeIcon icon={faPrint} />{' '}
                                <Translate id="learning-gap.print" />
                                    </>}
                            </Button> : null }
                        </Col>
                    </Row>

                    <hr/>
                    <Row>
                        <Col>
                            <img src={IconAvanzaBanner} alt="Banner" style={{width: "100%"}}/>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Table className="learning-gap-table">
                                <tbody>
                                <tr className="learning-gap-header">
                                    <td><b><Translate id="learning-gap.competency"/>:{' '}
                                        {itemSelected.KeyWord}</b></td>
                                </tr>
                                <tr>
                                    <td>
                                        <Translate id="learning-gap.according-to-results"/>
                                        {' '}
                                        <b><span dangerouslySetInnerHTML={{ __html:itemSelected.KeyWord}}/></b>.
                                        {' '}
                                        <Translate id="learning-gap.we-recommend-to-improve"/>
                                    </td>
                                </tr>
                                </tbody>
                            </Table>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <span dangerouslySetInnerHTML={{__html: itemSelected.TextDescription}}/>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Table className="learning-gap-table">
                                <tbody>
                                    <tr className="learning-gap-header">
                                        <td><b><Translate id="learning-gap.why-is-it-important"/></b></td>
                                    </tr>
                                    <tr>
                                        <td><span dangerouslySetInnerHTML={{ __html:itemSelected.TextImportance}}/></td>
                                    </tr>
                                </tbody>
                            </Table>
                        </Col>
                    </Row>


                </ModalBody>
                <ModalFooter>
                    <Button color="primary" className="learning-gap-button-present-lesson  btn btn-secondary"
                            onClick={() => setRecommendationModalOpened(false)}>
                        <Translate id="learning-gap.close" />
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default withLocalize(LearningGapRecommendations);
