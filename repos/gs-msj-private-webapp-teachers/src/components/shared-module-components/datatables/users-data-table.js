/**
 * Created by portiz on 2/18/19.
 */
import React, { useState, useEffect, useRef } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Input } from 'reactstrap';
import LoaderFullWindow from '../loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getUsersAdvanced } from "../../../api/users";
import PaginationCustom from './pagination';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import DatePicker from '../date-picker/date-picker-recent';
import SelectorRoles from '../selector/selector-roles';
import moment from 'moment';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import "../../admin-module/styles/style.css";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import { PromptAlertIndicator, AlertIcon, fetchUsersPromptSummary, fetchUnreadAlertsTotal } from "../profiles-prompt-alerts";
import "../profiles-prompt-alerts/components/prompt-unread-banner.css";

/**
 * Data table to handle the users
 */
const UsersDataTable = (props) => {
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
        { key: "0", name: LanguageManager.getTranslation("users-datatable.user-id", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("users-datatable.username", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("users-datatable.first-name", Settings.LANGUAGES) },
        { key: "3", name: LanguageManager.getTranslation("users-datatable.last-name", Settings.LANGUAGES) },
        { key: "4", name: LanguageManager.getTranslation("users-datatable.email", Settings.LANGUAGES) },
        { key: "5", name: LanguageManager.getTranslation("users-datatable.phone-number", Settings.LANGUAGES) },
        { key: "6", name: LanguageManager.getTranslation("users-datatable.role", Settings.LANGUAGES) },
        { key: "7", name: LanguageManager.getTranslation("users-datatable.invitation-code", Settings.LANGUAGES) },
        { key: "8", name: LanguageManager.getTranslation("users-datatable.city", Settings.LANGUAGES) },
        { key: "9", name: LanguageManager.getTranslation("users-datatable.state", Settings.LANGUAGES) },
        { key: "10", name: LanguageManager.getTranslation("users-datatable.country", Settings.LANGUAGES) },
        { key: "11", name: LanguageManager.getTranslation("users-datatable.last-session", Settings.LANGUAGES) },
        { key: "12", name: LanguageManager.getTranslation("users-datatable.status", Settings.LANGUAGES) },
        { key: "13", name: LanguageManager.getTranslation("users-datatable.created-at", Settings.LANGUAGES) }
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
    const [alertsByUser, setAlertsByUser] = useState({});
    const [unreadAlertsTotal, setUnreadAlertsTotal] = useState(0);
    const searchRequestId = useRef(0);

    const canViewPromptAlerts = isRoleAllowed(
        [Settings.ROLE_TEACHER_STR, Settings.ROLE_ADMIN_STR],
        SessionManager.getSessionDetails()?.Roles || []
    );

    useEffect(() => {
        doSearch();
    }, [props.redoSearch, currentPage]);

    useEffect(() => {
        if (!canViewPromptAlerts) {
            return;
        }

        let cancelled = false;

        (async () => {
            try {
                const total = await fetchUnreadAlertsTotal();
                if (!cancelled) setUnreadAlertsTotal(total);
            } catch (error) {
                console.error("Error fetching unread alerts total:", error);
            }
        })();

        return () => { cancelled = true; };
    }, [props.redoSearch]);

    /**
     * Fetches the prompt-alerts summary for the given page and fills the counts
     * once it arrives. Off the critical path, so a failure just leaves the
     * column empty. Ignores its result if a newer search has started in the
     * meantime (stale response).
     */
    const loadPromptAlertsForPage = async (userInfo, requestId) => {
        if (!canViewPromptAlerts || userInfo.length === 0) return;
        try {
            const summary = await fetchUsersPromptSummary(userInfo.map((user) => user.UserId));
            if (requestId !== searchRequestId.current) return;
            setAlertsByUser(summary.byUser);
        } catch (error) {
            console.error("Error fetching prompt alerts summary:", error);
        }
    };

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
        setCurrentPage(0);
        doSearch();
    };

    const handleRowClick = (e) => {
        props.updateParent(e);
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];

        if (typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
                newArray.push({
                    UserId: dataToConvert[i].UserId,
                    Username: typeof dataToConvert[i].Username !== 'undefined' && dataToConvert[i].Username !== null ? dataToConvert[i].Username : '',
                    GivenName: typeof dataToConvert[i].GivenName !== 'undefined' && dataToConvert[i].GivenName !== null ? dataToConvert[i].GivenName : '',
                    Surname: typeof dataToConvert[i].Surname !== 'undefined' && dataToConvert[i].Surname !== null ? dataToConvert[i].Surname : '',
                    Email: typeof dataToConvert[i].Email !== 'undefined' && dataToConvert[i].Email !== null ? dataToConvert[i].Email : '',
                    Phone: typeof dataToConvert[i].Phone !== 'undefined' && dataToConvert[i].Phone !== null ? dataToConvert[i].Phone : '',
                    Role: typeof dataToConvert[i].Role !== 'undefined' && dataToConvert[i].Role !== null ? dataToConvert[i].Role : '',
                    Token: typeof dataToConvert[i].Token !== 'undefined' && dataToConvert[i].Token !== null ? dataToConvert[i].Token : '',
                    City: typeof dataToConvert[i].City !== 'undefined' && dataToConvert[i].City !== null ? dataToConvert[i].City : '',
                    State: typeof dataToConvert[i].State !== 'undefined' && dataToConvert[i].State !== null ? dataToConvert[i].State : '',
                    Country: typeof dataToConvert[i].Country !== 'undefined' && dataToConvert[i].Country !== null ? dataToConvert[i].Country : '',
                    LastSession: typeof dataToConvert[i].LastSession !== 'undefined' && dataToConvert[i].LastSession !== null ? dataToConvert[i].LastSession : '',
                    Status: dataToConvert[i].Status,
                    CreatedAt: dataToConvert[i].CreatedAt,
                    View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(dataToConvert[i])}>
                        <Translate id="general.select" options={{ renderInnerHtml: true }} />
                    </button>,
                    value: dataToConvert[i].UserId
                });
            }
            setData(newArray);
        }
    };

    // Call the results to display
    const doSearch = () => {
        const requestId = ++searchRequestId.current;
        setLoading(true);

        let apiCall = getUsersAdvanced(userId, username, firstName, lastName, phone, email, roleId, dateFrom,
                                        dateTo, createdDateFrom, createdDateTo, currentPage, limiter, false, schoolId);

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            setData([]);
            
            if (response.data.UserInfo !== null) {
                convertDataToDataTable(response.data.UserInfo);
                loadPromptAlertsForPage(response.data.UserInfo, requestId);
            }
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
            createdAt, createdDateFrom, createdDateTo, currentPage, data, dateFrom, dateTo, email, firstName, headerOptions,
            hideRoleSelected, lastName, limiter, loading, phone, planId, roleId, schoolId, status, subscriptionId, totalPages, username,
            userId
        };
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            {canViewPromptAlerts && unreadAlertsTotal > 0 && (
                <div className="prompt-unread-banner" role="status">
                    <AlertIcon />
                    <span>
                        <Translate
                            id={unreadAlertsTotal === 1
                                ? "prompt-alerts.unread-alerts-banner-singular"
                                : "prompt-alerts.unread-alerts-banner-plural"}
                            data={{ count: unreadAlertsTotal }}
                        />
                    </span>
                </div>
            )}
            <div className="wrapper-col mb-4">
                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.first-name" /></div>
                        <Input type="text" onChange={handleFirstNameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.last-name" /></div>
                        <Input type="text" onChange={handleLastNameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.email" /></div>
                        <Input type="text" onChange={handleEmailChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.username" /></div>
                        <Input type="text" onChange={handleUsernameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.phone-number" /></div>
                        <Input type="text" onChange={handlePhoneChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="users-datatable.user-id" /></div>
                        <Input type="text" onChange={handleUserIdChange} />
                    </div>
                </div>

                <div className="four-col"></div>
                <div className="four-col"></div>

                <div className="col-full-size mt-3 mb-2"></div>

                {!hideRoleSelected ?
                    <div className="col-full-size mb-3">
                        <div className="form-group-fields form-group-fields__select">
                            {/* <div className="label"><Translate id="users-datatable.role"/></div> */}
                            <SelectorRoles roleId={roleId} updateParent={handleRoleChange} />
                        </div>
                    </div> : null}

                <div className="four-col">
                    <div className="form-group-fields form-group-fields__datapicker">
                        <div className="label"><Translate id="users-datatable.last-session-from" /></div>
                        <DatePicker data={getCurrentState()} updateParent={saveFromDate} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields form-group-fields__datapicker">
                        <div className="label"><Translate id="users-datatable.last-session-to" /></div>
                        <DatePicker data={getCurrentState()} updateParent={saveToDate} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields form-group-fields__datapicker">
                        <div className="label"><Translate id="users-datatable.created-from" /></div>
                        <DatePicker data={getCurrentState()} updateParent={saveCreatedFromDate} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields form-group-fields__datapicker">
                        <div className="label"><Translate id="users-datatable.created-to" /></div>
                        <DatePicker data={getCurrentState()} updateParent={saveCreatedToDate} />
                    </div>
                </div>

                <div className="four-col mt-3">
                    <Button className="btn-simple--only-text" onClick={filter}>
                        <FontAwesomeIcon icon={faSearch} />{' '}
                        <Translate id="users-datatable.filter" />
                    </Button>
                </div>

            </div>
            <div className="wrapper-table-admin mt-3">
                <table className="content-table-admin">
                    <thead>
                        <tr>
                            <th width="60">ID</th>
                            <th><Translate id="school-module.name" /></th>
                            <th><Translate id="school-module.address" /></th>
                            <th><Translate id="users-datatable.invitation-code" /></th>
                            <th><Translate id="users-datatable.status" /></th>
                            {canViewPromptAlerts && <th><Translate id="prompt-alerts.column-title" /></th>}
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((entity, index) => {
                            const unreadCount = (alertsByUser[entity.UserId] || {}).unreadCount || 0;
                            return (
                            <tr key={index}>
                                <td data-column="ID:">{entity.UserId}</td>
                                <td><span className="table-admin--title">{entity.GivenName}&ensp;{entity.Surname}</span><p>{entity.Email}</p><p className="txt-LighterBlue">Usuario: {entity.Username}  /  rol: {entity.Role}</p></td>

                                <td data-column="Dirección:">
                                    <p>{entity.City}. {entity.State}. {entity.Country}</p>
                                    <p>{entity.Phone}</p>
                                </td>

                                <td data-column="Código:">
                                    <p>{entity.Token}</p>
                                </td>

                                <td data-column="Estatus:" className="txt-LighterBlue">
                                    <p>{entity.Status}</p>
                                    <p><strong><Translate id="users-datatable.created-at" />:&ensp;</strong>{entity.CreatedAt}</p>
                                    <p><strong><Translate id="users-datatable.last-session" />:&ensp;</strong>{entity.LastSession}</p>
                                </td>

                                {canViewPromptAlerts && (
                                    <td data-column={`${LanguageManager.getTranslation("prompt-alerts.column-title", Settings.LANGUAGES)}:`}>
                                        <PromptAlertIndicator
                                            unreadCount={unreadCount}
                                            label={LanguageManager.getTranslation(
                                                unreadCount === 1
                                                    ? "prompt-alerts.new-alert-singular"
                                                    : "prompt-alerts.new-alert-plural",
                                                Settings.LANGUAGES
                                            )}
                                            title={LanguageManager.getTranslation("prompt-alerts.column-title", Settings.LANGUAGES)}
                                        />
                                    </td>
                                )}
                                <td>{entity.View}</td>
                            </tr>);
                        })}
                    </tbody>
                </table>
            </div>
            <PaginationCustom totalPages={totalPages}
                currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
            {loading ? <LoaderFullWindow /> : null}
        </div>
    );
};

export default UsersDataTable;