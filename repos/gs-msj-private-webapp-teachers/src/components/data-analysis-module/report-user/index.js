import React, { useState, useEffect } from "react";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { Link } from "react-router-dom";
import { Translate } from "react-localize-redux";
import {Container, Row, Col, Card, Button} from "react-bootstrap";
import Select from 'react-select';
import { getDataWarehouseOre, getDataWarehouseSchools, getDataWarehouseRoles, getDataWarehouseStatus, getDataWarehouseUsers } from "../../../api/data-warehouse";
import { FaSearch, FaArrowCircleLeft, FaFilter, FaDownload} from "react-icons/all";
import Pagination from "@material-ui/lab/Pagination";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import NavbarClients from "../../shared-module-components/navbar/navbar-clients";
import DocumentViewer from "../documentViewer";

const ReportUser = () => {
    const [ores, setOres] = useState([]);
    const [oreSelected, setOreSelected] = useState(null);
    const [schools, setSchools ] = useState([]);
    const [schoolSelected, setSchoolSelected] = useState(null);
    const [roles, setRoles] = useState([]);
    const [roleSelected, setRoleSelected] = useState(null);
    const [status, setStatus] = useState([]);
    const [statusSelected, setStatusSelected] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalResults, setTotalResults] = useState(null);
    const [users, setUsers] = useState([]);
    const [pagination, setPagination] = useState({});
    const [sie, setSie] = useState(null);
    const [documentUrl, setDocumentUrl] = useState('');
    const [loading, setLoading] = useState(false);
    const count = Math.ceil(totalResults / pageSize);
    const sessionDetails = SessionManager.getSessionDetails();

    // Initialize translations based on the current version
    useVersionedTranslations()

    const directorRole = Settings.ROLE_DIRECTOR_STR;

    const handlePageChange = async (event, value) => {
        setLoading(true);
        setPage(value);
        let roleId = roleSelected != null ? roleSelected.value : null;
        let status = statusSelected != null ? statusSelected.label : null;
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        const res = await getDataWarehouseUsers(null, null, null, null, null, roleId, status, null,
            schoolId, sie, oreId, false, pageSize, value - 1);
        setTotalResults(res.data.Count);
        setUsers(res.data.Results);
        setLoading(false);
    };

    useEffect(() => {
        setPagination({ page, pageSize });
    }, [page]);

    useEffect(()=>{
        if(isRoleAllowed([directorRole], sessionDetails.Roles))
            return

        (async ()=>{
            const roleCall = await getDataWarehouseRoles();
            const roles = roleCall.data;
            const roleMap = roles.map((item) =>({
                value: item.RoleId,
                label: item.Role
            }));
            setRoles(roleMap);
        })();
    }, []);

    useEffect(()=>{
        if(isRoleAllowed([directorRole], sessionDetails.Roles))
            return

        (async ()=>{
            const statusCall = await getDataWarehouseStatus();
            const status = statusCall.data;
            const statusMap = status.map((item) =>({
                value: item.StatusId,
                label: item.Status
            }));
            setStatus(statusMap);
        })();
    }, []);

    useEffect(()=>{
        if(isRoleAllowed([directorRole], sessionDetails.Roles))
            return
        (async ()=>{
            const oreCall = await getDataWarehouseOre();
            const ores = oreCall.data;
            const oreMap = ores.map((item) =>({
                value: item.OreId,
                label: item.Ore,
                countryIso: item.CountryIso
            }));
            setOres(oreMap);
        })();
    }, []);

    useEffect(()=>{
        if(isRoleAllowed([directorRole], sessionDetails.Roles))
            return
        (async ()=>{
            const schoolCall = await getDataWarehouseSchools(null, null,
                oreSelected != null ? oreSelected.value : null,
                null, null, null, 900);
            const schools = schoolCall.data;
            const schoolMap = schools.map((item) =>({
                value: item.SchoolId,
                label: item.SchoolName,
                city: item.City
            }));
            setSchools(schoolMap);
        })();
    }, [oreSelected]);

    const handleOreChange = (selected) =>{
        setOreSelected(selected);
    }

    const handleRoleChange = (selected)=>{
        setRoleSelected(selected);
    }

    const handleStatusChange = (selected)=>{
        setStatusSelected(selected);
    }

    const handleSchoolChange = (selected)=>{
        setSchoolSelected(selected);
    }

    const onFormSearch = async ()=>{
        setLoading(true);
        let roleId = roleSelected != null ? roleSelected.value : null;
        let status = statusSelected != null ? statusSelected.label : null;
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        const res = await getDataWarehouseUsers(null, null, null, null, null, roleId, status,
            null, schoolId, sie, oreId, false, pageSize, 0);
        const resFile = await getDataWarehouseUsers(null, null, null, null, null, roleId, status,
            null, schoolId, sie, oreId, true, pageSize, 0);
        setPage(0)
        setTotalResults(res.data.Count);
        setUsers(res.data.Results);
        setDocumentUrl(resFile.data.File);
        setLoading(false);
    }

    const onDocumentSearch = async ()=>{
        setLoading(true);
        let roleId = roleSelected != null ? roleSelected.value : null;
        let status = statusSelected != null ? statusSelected.label : null;
        let schoolId = schoolSelected != null ? schoolSelected.value : null;
        let oreId = oreSelected != null ? oreSelected.value : null;
        const res = await getDataWarehouseUsers(null, null, null, null, null, roleId, status,
            null, schoolId, sie, oreId, true, pageSize, 0);
        if(typeof res.data !== undefined && res.data.file !== ""){
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
            <Container className="deliverables-wrapper-container container-data">
                
                <Row>
                    <Col>
                        {/* <Link className="btn-back" to="/analysis/dashboard">
                            <FaArrowCircleLeft style={{ marginRight: '5px' }} />
                            <Translate id="data-analysis.back" />
                        </Link>
                        <br/> */}
                        <h1 className="title">
                            <Translate id="data-analysis-users.report-title" />
                        </h1>

                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Card className="box-shadow-data animated animate__animated animate__fadeInLeft reports-clear-card module-filter mt-negative">
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
                                    <div className="wraper-input mt-small wraper-input-full-width wraper-input__responsive">
                                        <div className="wrapper-input__col wrapper-input__col-one-size">
                                            <input
                                                className="input-data"
                                                placeholder={"SIE"}
                                                name="sie"
                                                type="text"
                                                onChange={(e) => setSie(e.target.value)}
                                            />
                                        </div>
                                    </div>
                                </Row>
                                <Row>
                                    <div className="wraper-input mt-small wraper-input-full-width wraper-input__responsive">
                                        {!isRoleAllowed([directorRole], sessionDetails.Roles) ?
                                        <div className="wrapper-input__col wrapper-input__col-four-size">
                                            <Select
                                                className="selectTrello"
                                                menuPortalTarget={document.body}
                                                placeholder={<Translate id="data-analysis.role" />}
                                                onChange={handleRoleChange}
                                                options={roles}
                                                isMulti={false}
                                            />
                                            </div> : null}

                                        {!isRoleAllowed([directorRole], sessionDetails.Roles) ?
                                        <div className="wrapper-input__col wrapper-input__col-four-size">
                                            <Select
                                                className="selectTrello"
                                                menuPortalTarget={document.body}
                                                placeholder={<Translate id="data-analysis.ore" />}
                                                onChange={handleOreChange}
                                                options={ores}
                                                isMulti={false}
                                                />
                                        </div> : null}
                                        {!isRoleAllowed([directorRole], sessionDetails.Roles) ?
                                        <div className="wrapper-input__col wrapper-input__col-four-size">
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
                                            </div> : null}
                                        
                                        {!isRoleAllowed([directorRole], sessionDetails.Roles) ?
                                        <div className="wrapper-input__col wrapper-input__col-four-size">
                                            <Select
                                                className="selectTrello"
                                                menuPortalTarget={document.body}
                                                placeholder={<Translate id="data-analysis.status" />}
                                                onChange={handleStatusChange}
                                                options={status}
                                                isMulti={false}
                                            />
                                        </div> : null}


                                    </div>
                                </Row>
                                <Row>
                                    <Col>
                                        <div className="button-report">
                                            <button className="button-secondary-data" onClick={onFormSearch}>
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
                            users.length > 0 ?
                                <>
                                    <button onClick={onDocumentSearch} className="button-download-document animated animate__animated animate__fadeIn">
                                        <FaDownload/> 
                                        <Translate id="data-analysis.download-excel" />
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

export default ReportUser;
