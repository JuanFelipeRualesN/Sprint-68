/**
 * Created by portiz on 12/23/20.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Col, Row, Button, Input} from 'reactstrap';
import LoaderFullWindow from '../loader-windows/loader-full-window';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {getProfilesAdvanced, getUsersAdvancedSameSchool} from "../../../api/users";
import PaginationCustom from './pagination';
import DataTable from './datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import DatePicker from '../date-picker/date-picker-recent';
import SelectorRoles from '../selector/selector-roles';
import moment from 'moment';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEye, faSearch} from "@fortawesome/free-solid-svg-icons";

/**
 * Data table to handle the teachers
 */
const TeachersDataTable = (props) => {
    const [createdAt] = useState(moment());
    const [createdDateFrom, setCreatedDateFrom] = useState(null);
    const [createdDateTo, setCreatedDateTo] = useState(null);
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [dateFrom, setDateFrom] = useState(null);
    const [dateTo, setDateTo] = useState(null);
    const [email, setEmail] = useState(null);
    const [firstName, setFirstName] = useState(null);
    const [headerOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("users-datatable.username", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("users-datatable.first-name", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("users-datatable.last-name", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("users-datatable.email", Settings.LANGUAGES)},
        {key: "4", name: LanguageManager.getTranslation("users-datatable.role", Settings.LANGUAGES)},
        {key: "5", name: LanguageManager.getTranslation("users-datatable.invitation-code", Settings.LANGUAGES)},
        {key: "6", name: LanguageManager.getTranslation("users-datatable.last-session", Settings.LANGUAGES)},
        {key: "7", name: ""},
    ]);
    const [hideRoleSelected] = useState(!!props.roleId);
    const [lastName, setLastName] = useState(null);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [phone, setPhone] = useState(null);
    const [planId, setPlanId] = useState(null);
    const [roleId, setRoleId] = useState(props.roleId ? props.roleId : null);
    const [schoolId] = useState(props.schoolId ?? null);
    const [status, setStatus] = useState(null);
    const [subscriptionId, setSubscriptionId] = useState(null);
    const [totalPages, setTotalPages] = useState(1);
    const [username, setUsername] = useState(null);
    const [userId, setUserId] = useState(null);

    useEffect(() => {
        doSearch();
    }, [props.redoSearch, currentPage]);

    const handleStatusChange = (e) => {
        setStatus(e === '' ? null : e);
    };

    const handleRoleChange = (e) => {
        setRoleId(e === '' ? null : e);
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
        doSearch();
    };

    const handleRowClick = (e) => {
        props.updateParent(e);
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];

        if(typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            
            for (let i = 0; i < dataToConvert.length; i++) {
                newArray.push({
                    Username: typeof dataToConvert[i].Username !== 'undefined' && dataToConvert[i].Username !== null ? dataToConvert[i].Username : '',
                    GivenName:  typeof dataToConvert[i].GivenName !== 'undefined' && dataToConvert[i].GivenName !== null ? dataToConvert[i].GivenName : '',
                    Surname: typeof dataToConvert[i].Surname !== 'undefined' && dataToConvert[i].Surname !== null ? dataToConvert[i].Surname : '',
                    Email: typeof dataToConvert[i].Email !== 'undefined' && dataToConvert[i].Email !== null ? dataToConvert[i].Email : '',
                    Role: typeof dataToConvert[i].Role !== 'undefined' &&  dataToConvert[i].Role !== null ?  dataToConvert[i].Role: '',
                    Token: typeof dataToConvert[i].Token !== 'undefined' && dataToConvert[i].Token !== null ? dataToConvert[i].Token : '',
                    LastSession: typeof dataToConvert[i].LastSession !== 'undefined' && dataToConvert[i].LastSession !== null ? dataToConvert[i].LastSession : '',
                    View: <button type="button" className="button-secondary-custom btn-default" onClick={e => handleRowClick(dataToConvert[i])}>
                        <FontAwesomeIcon icon={faEye}  onClick={e => handleRowClick(dataToConvert[i])} size="lg"/>{' '}
                        <Translate id="calendar.view" options={{renderInnerHtml: true}}/>
                    </button>,
                    value: dataToConvert[i].UserId
                });
            }
            setData(newArray);
        }
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getProfilesAdvanced(userId, username, firstName, lastName, phone, email, Settings.ROLE_TEACHER_ID,
                                         dateFrom, dateTo, createdDateFrom, createdDateTo, currentPage, limiter, false);

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            setData([]);
            
            if(response.data.UserInfo !== null)
                convertDataToDataTable(response.data.UserInfo);
        });
    };

    const saveFromDate = (newValue) => {
        setDateFrom(newValue.date);
    };

    const saveToDate = (newValue) => {
        setDateTo(newValue.date);
    };

    const saveCreatedFromDate = (newValue) => {
        setCreatedDateFrom(newValue.date);
    };

    const saveCreatedToDate = (newValue) => {
        setCreatedDateTo(newValue.date);
    };

    const getCurrentState = () => {
        return { 
            createdAt, createdDateFrom, createdDateTo, currentPage, data, dateFrom, dateTo, email, firstName, headerOptions, hideRoleSelected,
            lastName, limiter, loading, phone, planId, roleId, schoolId, status, subscriptionId, totalPages, username, userId
        };
    };

    return (
            <div>
                <div className="wrapper-col mb-4">

                    <div className="four-col">
                        <div className="form-group-fields">
                            <div className="label"><Translate id="users-datatable.first-name"/></div>
                            <Input type="text"  onChange={handleFirstNameChange} />
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields">
                            <div className="label"><Translate id="users-datatable.last-name"/></div>
                            <Input type="text"  onChange={handleLastNameChange} />
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields">
                            <div className="label"><Translate id="users-datatable.email"/></div>
                            <Input type="text"  onChange={handleEmailChange} />
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields">
                            <div className="label"><Translate id="users-datatable.username"/></div>
                            <Input type="text" onChange={handleUsernameChange} />
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields">
                            <div className="label"><Translate id="users-datatable.phone-number"/></div>
                            <Input type="text"  onChange={handlePhoneChange} />
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields">
                            <div className="label"><Translate id="users-datatable.user-id"/></div>
                            <Input type="text" onChange={handleUserIdChange} />
                        </div>
                    </div>

                    <div className="four-col"> </div>
                    <div className="four-col"> </div>

                    <div className="col-full-size mt-3 mb-2"></div>

                    {
                        !hideRoleSelected ?
                            <div className="col-full-size mb-3">
                                <div className="form-group-fields form-group-fields__select">
                                    <div className="label"><Translate id="users-datatable.role"/></div>
                                    <SelectorRoles roleId={roleId} updateParent={handleRoleChange}/>
                                </div>
                            </div>: null
                    }

                    <div className="four-col">
                        <div className="form-group-fields form-group-fields__datapicker">
                            <div className="label"><Translate id="users-datatable.last-session-from"/></div>
                            <DatePicker data={getCurrentState()} updateParent={saveFromDate}/>
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields form-group-fields__datapicker">
                            <div className="label"><Translate id="users-datatable.last-session-to"/></div>
                            <DatePicker data={getCurrentState()} updateParent={saveToDate}/>
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields form-group-fields__datapicker">
                            <div className="label"><Translate id="users-datatable.created-from"/></div>
                            <DatePicker data={getCurrentState()} updateParent={saveCreatedFromDate}/>
                        </div>
                    </div>

                    <div className="four-col">
                        <div className="form-group-fields form-group-fields__datapicker">
                            <div className="label"><Translate id="users-datatable.created-to"/></div>
                            <DatePicker data={getCurrentState()} updateParent={saveCreatedToDate}/>
                        </div>
                    </div>

                    <div className="four-col mt-3">
                        <Button className="btn-simple--only-text" onClick={filter}>
                            <FontAwesomeIcon icon={faSearch} />{' '}
                            <Translate id="users-datatable.filter" />
                        </Button>
                    </div>

                </div>
                <Row>
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.user-id"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Input type="text" onChange={handleUserIdChange} />
                            </Col>
                        </Row>
                    </Col> */}
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.username"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Input type="text" onChange={handleUsernameChange} />
                            </Col>
                        </Row>
                    </Col> */}
                </Row>

                <Row>
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.first-name"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Input type="text"  onChange={handleFirstNameChange} />
                            </Col>
                        </Row>
                    </Col> */}
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.last-name"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Input type="text"  onChange={handleLastNameChange} />
                            </Col>
                        </Row>
                    </Col> */}
                </Row>


                <Row>
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.email"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Input type="text"  onChange={handleEmailChange} />
                            </Col>
                        </Row>
                    </Col> */}
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.phone-number"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Input type="text"  onChange={handlePhoneChange} />
                            </Col>
                        </Row>
                    </Col> */}
                </Row>

                {/* <Row>
                    {
                        !hideRoleSelected ?
                            <div className="col-full-size mb-3">
                                <div className="form-group-fields form-group-fields__select">
                                    <div className="label"><Translate id="users-datatable.role"/></div>
                                    <SelectorRoles roleId={roleId} updateParent={handleRoleChange}/>
                                </div>
                            </div>: null
                            // <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            //     <Row>
                            //         <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            //             <Translate id="users-datatable.role"/>
                            //         </Col>
                            //         <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            //             <SelectorRoles roleId={roleId} updateParent={handleRoleChange}/>
                            //         </Col>
                            //     </Row>
                            // </Col> 
                    }

                </Row> */}

                <Row>
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.last-session-from"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <DatePicker data={getCurrentState()} updateParent={saveFromDate}/>
                            </Col>
                        </Row>
                    </Col> */}
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.last-session-to"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <DatePicker data={getCurrentState()} updateParent={saveToDate}/>
                            </Col>
                        </Row>
                    </Col> */}
                </Row>

                <Row>
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.created-from"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <DatePicker data={getCurrentState()} updateParent={saveCreatedFromDate}/>
                            </Col>
                        </Row>
                    </Col> */}
                    {/* <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Translate id="users-datatable.created-to"/>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <DatePicker data={getCurrentState()} updateParent={saveCreatedToDate}/>
                            </Col>
                        </Row>
                    </Col> */}
                </Row>

                {/* <Row>
                    <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                        <Row>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            </Col>
                            <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                                <Button color="success" onClick={filter}>
                                    <Translate id="users-datatable.filter"/>
                                </Button>
                            </Col>
                        </Row>
                    </Col>
                    <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    </Col>
                </Row> */}

                <DataTable headerOptions={headerOptions} data={data} updateParent={handleRowClick}/>
                <PaginationCustom totalPages={totalPages} currentPage={currentPage} updateParent={handlePageClick} dapper={true}/>
                {loading ? <LoaderFullWindow/> : null}
            </div>
        );
};


export default TeachersDataTable;