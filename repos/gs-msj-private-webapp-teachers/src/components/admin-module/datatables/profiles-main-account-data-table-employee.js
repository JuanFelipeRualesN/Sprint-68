/**
 * Created by portiz on 3/29/19.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Col, Row, Button, Input} from 'reactstrap';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {getMainAccountProfiles} from "../../../api/users";
import Pagination from "../../shared-module-components/datatables/pagination";
import DataTable from "../../shared-module-components/datatables/datatable";
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import moment from 'moment';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch} from "@fortawesome/free-solid-svg-icons";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

const ProfilesMainAccountDataTableEmployee = ({ roleId, subscriptionId, redoSearch, updateParent }) => {
    const [createdAt, setCreatedAt] = useState(moment());
    const [createdDateFrom, setCreatedDateFrom] = useState(null);
    const [createdDateTo, setCreatedDateTo] = useState(null);
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [dateFrom, setDateFrom] = useState(null);
    const [dateTo, setDateTo] = useState(null);
    const [email, setEmail] = useState(null);
    const [firstName, setFirstName] = useState(null);
    const [headerOptions, setHeaderOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("users-datatable.username", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("users-datatable.first-name", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("users-datatable.last-name", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("users-datatable.invitation-code", Settings.LANGUAGES)},
        {key: "4", name: LanguageManager.getTranslation("users-datatable.last-session", Settings.LANGUAGES)},
        {key: "5", name: ""}
    ]);
    const [lastName, setLastName] = useState(null);
    const [limiter, setLimiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [phone, setPhone] = useState(null);
    const [planId, setPlanId] = useState(null);
    const [status, setStatus] = useState(null);
    const [totalPages, setTotalPages] = useState(1);
    const [username, setUsername] = useState(null);
    const [userId, setUserId] = useState(null);

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    useEffect(() => {
        doSearch();
    }, [currentPage]);

    const handleStatusChange = (e) => {
        setStatus(e === '' ? null : e);
    };

    const handleEmailChange = (e) => {
        setEmail(e.currentTarget.value);
    };

    const handlePhoneChange = (e) => {
        setPhone(e.currentTarget.value);
    };

    const handleUserIdChange = (e) => {
        setUserId(e.currentTarget.value);
    };

    const handleUsernameChange = (e) => {
        setUsername(e.currentTarget.value);
    };

    const handleFirstNameChange = (e) => {
        setFirstName(e.currentTarget.value);
    };

    const handleLastNameChange = (e) => {
        setLastName(e.currentTarget.value);
    };

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const filter = () => {
        setCurrentPage(0);
        doSearch();
    };

    const handleRowClick = (e) => {
        updateParent(e);
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];

        for(let i=0; i < dataToConvert.length; i++) {
            newArray.push({
                Username: dataToConvert[i].Username,
                GivenName: dataToConvert[i].GivenName,
                Surname: dataToConvert[i].Surname,
                Token: dataToConvert[i].Token,
                LastSession: dataToConvert[i].LastSession,
                View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(dataToConvert[i])}>
                    <Translate id="general.select" options={{renderInnerHtml: true}}/>
                </button>,
                value: dataToConvert[i].UserId,
            });
        }
        setData(newArray);
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getMainAccountProfiles(
            subscriptionId,
            firstName,
            lastName,
            username,
            limiter,
            currentPage,
            roleId);

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            setData([]);
            
            if(typeof response.data.UserInfo !== 'undefined' && response.data.UserInfo !== null)
                convertDataToDataTable(response.data.UserInfo);
        });
    };

    const saveFromDate = () => (newValue) => {
        setDateFrom(newValue.date);
    };

    const saveToDate = () => (newValue) => {
        setDateTo(newValue.date);
    };

    const saveCreatedFromDate = () => (newValue) => {
        setCreatedDateFrom(newValue.date);
    };

    const saveCreatedToDate = (field) => (newValue) => {
        setCreatedDateTo(newValue.date);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <div className="wrapper-col mb-4">
                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.username"/></div>
                        <Input type="text" value={username} onChange={handleUsernameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.first-name"/></div>
                        <Input type="text" value={firstName} onChange={handleFirstNameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.last-name"/></div>
                        <Input type="text" value={lastName}  onChange={handleLastNameChange} />
                    </div>
                </div>

                <div className="four-col mt-3">
                    <button className="btn-default button-secondary-custom" onClick={filter}>
                        <FontAwesomeIcon icon={faSearch}/>{' '}
                        <Translate id="general.search"/>
                    </button>
                </div>
            </div>
            {/* <Row>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Translate id="users-datatable.username"/>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Input type="text" value={username} onChange={handleUsernameChange} />
                        </Col>
                    </Row>
                </Col>
            </Row> */}

            {/* <Row>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Translate id="users-datatable.first-name"/>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Input type="text" value={firstName} onChange={handleFirstNameChange} />
                        </Col>
                    </Row>
                </Col>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Translate id="users-datatable.last-name"/>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Input type="text" value={lastName}  onChange={handleLastNameChange} />
                        </Col>
                    </Row>
                </Col>
            </Row> */}
            {/* <Row>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Button color="success" onClick={filter}>
                                <FontAwesomeIcon icon={faSearch}/>{' '}
                                <Translate id="general.search"/>
                            </Button>
                        </Col>
                    </Row>
                </Col>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                </Col>
            </Row> */}

            <DataTable headerOptions={headerOptions} data={data}/>
            <Pagination totalPages={totalPages}
                              currentPage={currentPage} updateParent={handlePageClick} dapper={true}/>
            {loading && <LoadingFullWindow/>}
        </div>
    );
};

export default ProfilesMainAccountDataTableEmployee;