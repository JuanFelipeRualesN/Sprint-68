import React, { useState, useEffect } from "react";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { Link } from "react-router-dom";
import {Container, Row, Col, Card} from "react-bootstrap";
import Select from 'react-select';
import { getDataWarehouseOre, getDataWarehouseSchools, getDataWarehouseStatus, getDataWarehouseConnected } from "../../../api/data-warehouse";
import { FaSearch, FaArrowCircleLeft, FaFilter, FaDownload} from "react-icons/all";
import Pagination from "@material-ui/lab/Pagination";
import LocalizedDatePicker from "../../../utils/datepicker-locale";
import "react-datepicker/dist/react-datepicker.css";
import {Portal} from "react-overlays";
import Settings from "../../../custom-classes/settings";
import { Translate } from "react-localize-redux";
import { isRoleAllowed } from "../../../utils/utils";
import SessionManager from "../../../custom-classes/session-manager";
import NavbarClients from "../../shared-module-components/navbar/navbar-clients";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import DocumentViewer from "../documentViewer";

const ReportConnections = () => {
    const [ores, setOres] = useState([]);
    const [oreSelected, setOreSelected] = useState(null);
    const [schools, setSchools ] = useState([]);
    const [schoolSelected, setSchoolSelected] = useState(null);
    const [status, setStatus] = useState([]);
    const [statusSelected, setStatusSelected] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalResults, setTotalResults] = useState(null);
    const [results, setResults] = useState([]);
    const [pagination, setPagination] = useState({});
    const [documentUrl, setDocumentUrl] = useState('');
    const [loading, setLoading] = useState(false);
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
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        let status = statusSelected != null ? statusSelected.label : null;
        const res = await getDataWarehouseConnected(startDate, endDate, schoolId, oreId, status, false, pageSize, value - 1);
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

    useEffect(()=>{
        (async ()=>{
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

    useEffect(()=>{
        (async ()=>{
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
        (async () => {
            if (isAllowed()) {
                const statusCall = await getDataWarehouseStatus();
                const status = statusCall.data;
                const statusMap = status.map((item) => ({
                    value: item.StatusId,
                    label: item.Status
                }));
                setStatus(statusMap);
            }
            })();
        }, []);

    const handleOreChange = (selected) =>{
        setOreSelected(selected);
    }

    const handleSchoolChange = (selected)=>{
        setSchoolSelected(selected);
    }

    const handleStatusChange = (selected)=>{
        setStatusSelected(selected);
    }

    const onFormSearch = async ()=>{
        setLoading(true);
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        let status = statusSelected != null ? statusSelected.label : null;
        const res = await getDataWarehouseConnected(startDate, endDate, schoolId, oreId, status, false, pageSize, 0);
        const resFile = await getDataWarehouseConnected(startDate, endDate, schoolId, oreId, status, true, pageSize, 0);
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
        let status = statusSelected != null ? statusSelected.label : null;
        const res = await getDataWarehouseConnected(startDate, endDate, schoolId, oreId, status, true, pageSize, 0);
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
                            <FaArrowCircleLeft style={{ marginRight: '5px' }}/>
                            <Translate id="data-analysis.back" />
                        </Link>
                        <br/> */}
                        <h1 className="title">
                            <Translate id="data-analysis-connections.report-title" />
                        </h1>

                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Card className="box-shadow animated animate__animated animate__fadeInLeft reports-clear-card module-filter mt-negative">
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
                                    <div className="wraper-input mt-small  wrapper-input-datapicker-responsive">
                                        <div className="wrapper-input__col">
                                            <sup className="label-reports-date">
                                                <Translate id="data-analysis.from" />
                                            </sup> 
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
                                            <sup className="label-reports-date">
                                                <Translate id="data-analysis.to" />
                                            </sup>{' '}
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
                                </Row>
                                <Row>
                                    {isAllowed() ?
                                        <div className="wraper-input wraper-input-full-width wraper-input__responsive">
                                            <div className="wrapper-input__col wrapper-input__col-three-size">
                                                <Select
                                                    className="selectTrello"
                                                    menuPortalTarget={document.body}
                                                    placeholder={<Translate id="data-analysis.ore" />}
                                                    onChange={handleOreChange}
                                                    options={ores}
                                                    isMulti={false}
                                                />
                                            </div>
                                            <div className="wrapper-input__col wrapper-input__col-three-size">
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
                                            <div className="wrapper-input__col wrapper-input__col-three-size">
                                                <Select
                                                    className="selectTrello"
                                                    menuPortalTarget={document.body}
                                                    placeholder={<Translate id="data-analysis.status" />}
                                                    onChange={handleStatusChange}
                                                    options={status}
                                                    isMulti={false}
                                                />
                                            </div>
                                        </div>
                                        : null
                                    }
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
                                        <FaDownload/>
                                        <Translate id="data-analysis-connections.download-excel" />
                                    </button>
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

export default ReportConnections;
