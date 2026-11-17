import React, { useState, useEffect } from "react";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { useHistory, Link } from "react-router-dom";
import {Container, Row, Col, Card} from "react-bootstrap";
import { getDataWarehouseStudentActivitiesByTeacher } from "../../../api/data-warehouse";
import { FaSearch, FaArrowCircleLeft, FaFilter} from "react-icons/all";
import Pagination from "@material-ui/lab/Pagination";
import LocalizedDatePicker from "../../../utils/datepicker-locale";
import "react-datepicker/dist/react-datepicker.css";
import {Portal} from "react-overlays";
import { Translate } from "react-localize-redux";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleDenied } from "../../../utils/utils";
import NavbarClients from "../../shared-module-components/navbar/navbar-clients";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import DocumentViewer from "../documentViewer";

const ReportStudentsActivityTeacher = () => {
    const history = useHistory();
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalResults, setTotalResults] = React.useState(null);
    const [results, setResults] = useState([]);
    const [pagination, setPagination] = useState({});
    const [documentUrl, setDocumentUrl] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [startDate, setStartDate] = useState(new Date(new Date().setDate(new Date().getDate() - 30)));
    const [endDate, setEndDate] = useState(new Date());
    const count = Math.ceil(totalResults / pageSize);
    const sessionDetails = SessionManager.getSessionDetails();

    // Initialize translations based on the current version
    useVersionedTranslations();

    const CalendarContainer = ({ children }) => {
        const el = document.getElementById("calendar-portal");
        return <Portal container={el}>{children}</Portal>;
    };

    const handlePageChange = async (event, value) => {
        setLoading(true);
        setPage(value);
        const res = await getDataWarehouseStudentActivitiesByTeacher(startDate, endDate, false, pageSize, value - 1);
        setTotalResults(res.data.Count);
        setResults(res.data.Results);
        setLoading(false);
    };

    useEffect(() => {
        setPagination({ page, pageSize });
    }, [page]);

    useEffect(() => {
        if(isRoleDenied([Settings.ROLE_TEACHER_STR], sessionDetails.Roles)) {
            history.push("/analysis/dashboard");
        }
    }, []);

    const onFormSearch = async ()=>{
        setLoading(true);
        setDocumentUrl(null);
        const res = await getDataWarehouseStudentActivitiesByTeacher(startDate, endDate, false, pageSize, 0);
        const resFile = await getDataWarehouseStudentActivitiesByTeacher(startDate, endDate, true, pageSize, 0);
        setPage(0)
        setTotalResults(res.data.Count);
        setResults(res.data.Results);
        setDocumentUrl(resFile.data.File);
        setLoading(false);
    }

    const onDocumentSearch = async ()=>{
        setLoading(true);
        const res = await getDataWarehouseStudentActivitiesByTeacher(startDate, endDate, true, pageSize, 0);
        if(typeof res.data !== undefined && res.data.File !== ""){
            const link = document.createElement('a');
            link.href = res.data.File;
            link.setAttribute('download', res.data.File);
            document.body.appendChild(link);
            link.click();
        }
        setLoading(false);
    }

    return (
        <>
            {/* <NavbarClients /> */}
            <Container  className="deliverables-wrapper-container container-data">
                <Row>
                    <Col>
                        {/* <Link className="btn-back" to="/analysis/dashboard">
                            <FaArrowCircleLeft style={{ marginRight: '5px' }}/>
                            <Translate id="data-analysis.back" />
                        </Link>
                        <br/> */}
                        <h1 className="title margin-top-30">
                            <Translate id="data-analysis-connections.report-title" />
                        </h1>

                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Card className="box-shadow animated animate__animated animate__fadeInLeft reports-clear-card mt-negative module-filter">
                            <Card.Body>
                                <Row>
                                    <Col>
                                        <h6 className="title-filter"> <FaFilter/> {' '} <Translate id="data-analysis.filters" /></h6>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <div className="wraper-input wraper-input__flexEnd filter-activity-teacher mt-3">
                                            <div className="wrapper-input__col">
                                                <sup><Translate id="data-analysis.from" /></sup>
                                                <LocalizedDatePicker 
                                                    selected={startDate} 
                                                    popperContainer={CalendarContainer} 
                                                    onChange={(date) => {
                                                        if (date) {
                                                            setStartDate(date);
                                                        } else {
                                                            setStartDate(startDate);
                                                        }
                                                    }}
                                                />
                                            </div>
                                            <div className="wrapper-input__col">
                                                <sup><Translate id="data-analysis.to" /></sup>
                                                <LocalizedDatePicker 
                                                    selected={endDate} 
                                                    popperContainer={CalendarContainer} 
                                                    onChange={(date) => {
                                                        if (date) {
                                                            setEndDate(date);
                                                        } else {
                                                            setEndDate(endDate);
                                                        }
                                                    }}
                                                />
                                            </div>
                                            <div className="wrapper-input__col">
                                                <button className="button-secondary" onClick={onFormSearch}>
                                                <FaSearch/> {' '}
                                                 <Translate id="data-analysis.search" />
                                                </button>
                                            </div>
                                        </div>
                                    </Col>
                                </Row>
                                {/* <Row>
                                    <Col style={{textAlign: 'right'}}>
                                        <button className="button-secondary" onClick={onFormSearch}>
                                            <FaSearch/> {' '}
                                            {t("search")}
                                        </button>
                                    </Col>
                                </Row> */}
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>
                <Row>
                    <hr style={{borderTop: '0px'}}/>
                </Row>
                <Row>
                    
                    <Col className="animated animate__animated animate__fadeInLeft text-right">
                        <div className="wrapper-reports-btn-results">
                        {
                            results.length > 0 ?
                                <>
                                    <button onClick={onDocumentSearch} className="button-download-document animated animate__animated animate__fadeIn">
                                        <Translate id="data-analysis.download-excel-activity" />
                                    </button>
                                    <b className="animated animate__animated animate__fadeIn"> <Translate id="data-analysis.total" />: {totalResults} </b>
                                    <DocumentViewer blobUrl={documentUrl}/>
                                </> : null
                        }
                        </div>
                    </Col>                    
                </Row>
                {loading ? <LoadingFullWindow /> : null}
            </Container>
        </>
    );
};

export default ReportStudentsActivityTeacher;
