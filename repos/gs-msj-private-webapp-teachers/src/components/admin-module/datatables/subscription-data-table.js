import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Col, Row, Button, Input } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { getSubscriptionsAdvanced } from "../../../api/subscriptions";
import Pagination from "../../shared-module-components/datatables/pagination";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import "../styles/style.css"

const SubscriptionDataTable = ({ subscriptionsIds, redoSearch, updateParent, subscriptionType }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [email, setEmail] = useState(null);
    const [firstName, setFirstName] = useState(null);
    const [lastName, setLastName] = useState(null);
    const [limiter, setLimiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [planId, setPlanId] = useState(null);
    const [status, setStatus] = useState(null);
    const [subscriptionId, setSubscriptionId] = useState(null);
    const [totalPages, setTotalPages] = useState(1);
    const [username, setUsername] = useState(null);

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
        setStatus(e.currentTarget.value === '' ? null : e.currentTarget.value);
    };

    const handleEmailChange = (e) => {
        setEmail(e.currentTarget.value);
    };

    const handleSubscriptionIdChange = (e) => {
        setSubscriptionId(e.currentTarget.value);
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
        for (let i = 0; i < dataToConvert.length; i++) {
            newArray.push({
                Id: dataToConvert[i].Id,
                PlatformId: dataToConvert[i].PlatformId,
                FullName: dataToConvert[i].FullName,
                UserName: dataToConvert[i].Username,
                Quantity: dataToConvert[i].Quantity,
                Email: dataToConvert[i].Email,
                CurrentPeriodEnd: dataToConvert[i].CurrentPeriodEnd,
                Status: dataToConvert[i].Status,
                PlanName: typeof dataToConvert[i].Plan != 'undefined' && dataToConvert[i].Plan != null ? dataToConvert[i].Plan.Name : '',
                PlanId: typeof dataToConvert[i].Plan != 'undefined' && dataToConvert[i].Plan != null ? dataToConvert[i].Plan.Id : '',
                UserId: dataToConvert[i].UserId,
                View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(dataToConvert[i])}>
                    <Translate id="general.select" options={{ renderInnerHtml: true }} />
                </button>,
                value: dataToConvert[i].Id,
            });
        }
        setData(newArray);
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getSubscriptionsAdvanced(
            subscriptionId,
            email,
            username,
            firstName,
            lastName,
            status,
            planId,
            currentPage,
            limiter,
            subscriptionsIds,
            subscriptionType
        );

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            convertDataToDataTable(response.data.Subscriptions);
        });
    };

    return (
        <div>
            <div className="wrapper-col mb-4">

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="subscriptions.first-name" /></div>
                        <Input type="text" onChange={handleFirstNameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="subscriptions.last-name" /></div>
                        <Input type="text" onChange={handleLastNameChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="subscriptions.email" /></div>
                        <Input type="text" onChange={handleEmailChange} />
                    </div>
                </div>
                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="subscriptions.username" /></div>
                        <Input type="text" onChange={handleUsernameChange} />
                    </div>
                </div>
                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="subscriptions.id" /></div>
                        <Input type="text" onChange={handleSubscriptionIdChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="subscriptions.status" /></div>
                        <Input type="select" onChange={handleStatusChange}>
                            <option value="">---</option>
                            <option value="active">active</option>
                            <option value="canceled">canceled</option>
                            <option value="trialing">trialing</option>
                            <option value="past_due">past_due</option>
                        </Input>
                    </div>
                </div>

                <div className="four-col mt-3">
                    <Button className="btn-simple--only-text" onClick={filter}>
                        <FontAwesomeIcon icon={faSearch} />{' '}
                        <Translate id="general.search" />
                    </Button>
                </div>
                <div className="four-col"></div>
            </div>

            <div className="wrapper-table-admin mt-3">
                <table className="content-table-admin">
                    <thead>
                        <tr>
                            <th width="60">ID</th>
                            <th><Translate id="school-module.name" /></th>
                            <th><Translate id="subscriptions.current-period-end" /></th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((entity, index) => (
                            <tr key={index}>
                                <td data-column="ID:">{entity.Id}</td>
                                <td><span className="table-admin--title">{entity.FullName}</span><p>{entity.Email}</p><p className="txt-LighterBlue">ID (plataforma): {entity.PlatformId}</p>  <p className="txt-LighterBlue">Usuario: {entity.UserName}</p></td>
                                <td data-column="Período actual:"><span className="txt-LighterBlue">{entity.CurrentPeriodEnd}</span></td>
                                <td>
                                    <p><strong><Translate id="subscriptions.status" />:&ensp;</strong>{entity.Status}</p>
                                    <p><strong><Translate id="subscriptions.plan" />:&ensp;</strong>{entity.PlanName}</p>
                                    <p><strong><Translate id="subscriptions.plan-id" />:&ensp;</strong> {entity.PlanId} / <strong><Translate id="subscriptions.user-id" />:&ensp;</strong> {entity.UserId}</p>
                                    <p><strong><Translate id="subscriptions.quantity" />:&ensp;</strong>{entity.Quantity}</p>
                                </td>
                                <td>{entity.View}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <Pagination totalPages={totalPages}
                currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default SubscriptionDataTable;