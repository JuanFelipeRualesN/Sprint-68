import React, { useState, useEffect } from "react";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { Link } from "react-router-dom";
import {Container, Row, Col, Card, Button} from "react-bootstrap";
import { Translate } from "react-localize-redux";
import Select from 'react-select';
import { getDataWarehouseOre, getDataWarehouseSchools, getDataWarehouseAcademics, 
        getDataWarehouseSubjects, getDataWarehouseGrades } from "../../../api/data-warehouse";
import {FaSearch, FaArrowCircleLeft, FaExclamationCircle, FaFilter, FaDownload} from "react-icons/all";
import Pagination from "@material-ui/lab/Pagination";
import LocalizedDatePicker from "../../../utils/datepicker-locale";
import "react-datepicker/dist/react-datepicker.css";
import { Portal } from "react-overlays";
import { isRoleAllowed } from "../../../utils/utils";
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import NavbarClients from "../../shared-module-components/navbar/navbar-clients";
import DocumentViewer from "../documentViewer";

const ReportAcademics = () => {
    const [ores, setOres] = useState([]);
    const [oreSelected, setOreSelected] = useState(null);
    const [schools, setSchools ] = useState([]);
    const [schoolSelected, setSchoolSelected] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalResults, setTotalResults] = useState(null);
    const [results, setResults] = useState([]);
    const [pagination, setPagination] = useState({});
    const [documentUrl, setDocumentUrl] = useState('');
    const [loading, setLoading] = useState(false);
    const [startDate, setStartDate] = useState(new Date(new Date().setDate(new Date().getDate() - 30)));
    const [endDate, setEndDate] = useState(new Date());
    const [subject, setSubject] = useState(null);
    const [level, setLevel] = useState(null);
    const [subjects, setSubjects] = useState([]);
    const [levels, setLevels] = useState([]);
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
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        let subjectCode = subject != null ? subject.value : null;
        let levelCode = level != null ? level.value : null;
        const res = await getDataWarehouseAcademics(startDate, endDate, subjectCode, levelCode, schoolId, oreId,false, pageSize, value - 1);
        setTotalResults(res.data.Count);
        setResults(res.data.Results);
        setLoading(false);
    };

    // Check if the user has the required role to access the component
    const isAllowed = () => {
        return isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) || isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) ||
            isRoleAllowed([Settings.ROLE_EMPLOYEE_STR], sessionDetails.Roles);
    }

    useEffect(() => {
        setPagination({ page, pageSize });
    }, [page]);

    useEffect(() => {
        (async () => {
            if(isAllowed()) {
                const oreCall = await getDataWarehouseOre(null);
                const ores = oreCall.data;
                const oreMap = ores.map((item) => ({
                    value: item.OreId,
                    label: item.Ore,
                    countryIso: item.CountryIso
                }));
                setOres(oreMap);
            }
        })();
    }, []);

    useEffect(() => {
        (async () => {
            if(isAllowed()) {
                const schoolCall = await getDataWarehouseSchools(null, null,
                    oreSelected != null ? oreSelected.value : null,
                    null, null, null, 100);
                const schools = schoolCall.data;
                const schoolMap = schools.map((item) => ({
                    value: item.SchoolId,
                    label: item.SchoolName,
                    city: item.City
                }));
                setSchools(schoolMap);
            }
        })();
    }, [oreSelected]);

    useEffect(()=>{
        (async ()=>{
            const subjectCall = await getDataWarehouseSubjects();
            const subjects = subjectCall.data;
            const subjectMap = subjects.map((item) =>({
                value: item.SubjectCode,
                label: item.SubjectName
            }));
            setSubjects(subjectMap);
        })();
    }, []);

    useEffect(()=>{
        (async ()=>{
            const levelCall = await getDataWarehouseGrades();
            const levels = levelCall.data;
            const levelMap = levels.map((item) =>({
                value: item.LevelCode,
                label: item.LevelName
            }));
            setLevels(levelMap);
        })();
    }, []);

    const handleOreChange = (selected) =>{
        setOreSelected(selected);
    }

    const handleSchoolChange = (selected)=>{
        setSchoolSelected(selected);
    }

    const onFormSearch = async ()=>{
        setLoading(true);
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        let subjectCode = subject != null ? subject.value : null;
        let levelCode = level != null ? level.value : null;
        const res = await getDataWarehouseAcademics(startDate, endDate, subjectCode, levelCode, schoolId, oreId, false, pageSize, 0);
        const resFile = await getDataWarehouseAcademics(startDate, endDate, subjectCode, levelCode, schoolId, oreId, true, pageSize, 0);
        setPage(0)
        setTotalResults(res.data.Count);
        setResults(res.data.Results);
        setDocumentUrl(resFile.data.File);
        setLoading(false);
    }

    const onDocumentSearch = async ()=>{

        setLoading(true);
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        let subjectCode = subject != null ? subject.value : null;
        let levelCode = level != null ? level.value : null;
        const res = await getDataWarehouseAcademics(startDate, endDate, subjectCode, levelCode, schoolId, oreId, true, pageSize, 0);
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
            <Container className="deliverables-wrapper-container col-responsive-m7 container-data">
                <Row>
                    <Col>
                        {/* <Link className="btn-back" to="/analysis/dashboard">
                            <FaArrowCircleLeft style={{ marginRight: '5px' }} />
                            <Translate id="data-analysis.back" />
                        </Link>
                        <br/> */}
                        <h1 className="title">
                            <Translate id="data-analysis-academic.report-title" />
                        </h1>
                        <div className="alert-message">
                            <small>
                            <FaExclamationCircle />{" "}: <Translate id="data-analysis-academic.note" />:
                                <ul>
                                    <li><Translate id="data-analysis-academic.note-1" /></li>
                                    <li><Translate id="data-analysis-academic.note-2" /></li>
                                    <li><Translate id="data-analysis-academic.note-3" /></li>
                                    <li><Translate id="data-analysis-academic.note-4" /></li>
                                    {isAllowed() ?
                                    <li>
                                        <Translate id="data-analysis-academic.note-5" />
                                    </li>  : null}
                                </ul>
                        </small>
                        </div>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Card className="box-shadow animated animate__animated animate__fadeInLeft reports-clear-card module-filter">
                            <Card.Body>
                                <Row>
                                    <Col>
                                        <h6 className="title-filter mb-big"> 
                                            <FaFilter/> {' '}
                                            <Translate id="data-analysis.filters" />
                                        </h6>
                                    </Col>
                                </Row>
                                <Row>                                    
                                    <div className="wraper-input mt-small margin-left-1 wrapper-input-datapicker-responsive">
                                        <div className="wrapper-input__col">
                                            <sup className="label-reports-date"><Translate id="data-analysis.from" /></sup>
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
                                            <sup className="label-reports-date"><Translate id="data-analysis.to" /></sup>{' '}
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
                                    </div>
                                    
                                    {
                                        isAllowed() ?
                                        <Col xs={12} md={7}>
                                        <div className="wraper-input wrapper-input-data-analysis">
                                            <div className="wrapper-input__col wrapper-input__col-two-size">
                                                <Select
                                                    className="selectTrello"
                                                    menuPortalTarget={document.body}
                                                    placeholder={<Translate id="data-analysis.ore" />}
                                                    onChange={handleOreChange}
                                                    options={ores}
                                                    isMulti={false}
                                                    />
                                            </div>
                                            <div className="wrapper-input__col wrapper-input__col-two-size">
                                                {setOreSelected != null ?
                                                    <Select
                                                        className="selectTrello"
                                                        menuPortalTarget={document.body}
                                                        value={schoolSelected}
                                                        placeholder={<Translate id="data-analysis.school" />}
                                                        onChange={handleSchoolChange}
                                                        options={schools}
                                                        isMulti={false}
                                                    /> : null}
                                            </div>
                                        </div>
                                        </Col> : null}
                                    
                                    <Col xs={12} md={7}>
                                    <div className="wraper-input">
                                        <div className="wrapper-input__col wrapper-input__col-two-size">
                                            <Select
                                                className="selectTrello"
                                                menuPortalTarget={document.body}
                                                placeholder={<Translate id="data-analysis.subject" />}
                                                onChange={subject => setSubject(subject)}
                                                options={subjects}
                                                isMulti={false}
                                            />
                                        </div>
                                        <div className="wrapper-input__col wrapper-input__col-two-size">
                                            <Select
                                                className="selectTrello"
                                                menuPortalTarget={document.body}
                                                placeholder={<Translate id="data-analysis.level" />}
                                                onChange={level => setLevel(level)}
                                                options={levels}
                                                isMulti={false}
                                            />
                                        </div>
                                    </div>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <div className="button-report">
                                            <button className="button-secondary" onClick={onFormSearch}>
                                                <FaSearch/> {' '}
                                                <Translate id="data-analysis.search" />
                                            </button>
                                        </div>
                                    </Col>
                                </Row>
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
                                        <FaDownload/> {' '} <Translate id="data-analysis-academic.download-excel" />
                                        </button>
                                        {' '}
                                        <b className="animated animate__animated animate__fadeIn"><Translate id="data-analysis.total" />: {totalResults} </b>
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

export default ReportAcademics;
