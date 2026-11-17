import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Input } from 'reactstrap';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { Translate } from "react-localize-redux";
import { getCoupons } from "../../../api/subscriptions";
import Pagination from "../../shared-module-components/datatables/pagination";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faSearch,
} from '@fortawesome/free-solid-svg-icons';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import "../styles/style.css"

const CouponDataTable = ({ redoSearch, updateParent }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const [data, setData] = useState([]);
    const [headerOptions, setHeaderOptions] = useState([
        { key: "0", name: LanguageManager.getTranslation("coupons-manager.id", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("coupons-manager.name", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("coupons-manager.code", Settings.LANGUAGES) },
        { key: "3", name: LanguageManager.getTranslation("coupons-manager.available", Settings.LANGUAGES) },
        { key: "4", name: LanguageManager.getTranslation("coupons-manager.currency", Settings.LANGUAGES) },
        { key: "5", name: LanguageManager.getTranslation("coupons-manager.amount-off", Settings.LANGUAGES) },
        { key: "6", name: LanguageManager.getTranslation("coupons-manager.duration", Settings.LANGUAGES) },
        { key: "7", name: LanguageManager.getTranslation("coupons-manager.duration-in-months", Settings.LANGUAGES) },
        { key: "8", name: LanguageManager.getTranslation("coupons-manager.max-redemptions", Settings.LANGUAGES) },
        { key: "9", name: LanguageManager.getTranslation("coupons-manager.percent-off", Settings.LANGUAGES) },
        { key: "10", name: LanguageManager.getTranslation("coupons-manager.platform-id", Settings.LANGUAGES) },
        { key: "11", name: LanguageManager.getTranslation("coupons-manager.redeem-by", Settings.LANGUAGES) },
        { key: "12", name: LanguageManager.getTranslation("coupons-manager.interval", Settings.LANGUAGES) }
    ]);
    const [limiter, setLimiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState(null);
    const [code, setCode] = useState(null);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    useEffect(() => {
        if (currentPage !== 1) {
            doSearch();
        }
    }, [currentPage]);

    const handleCodeChange = (e) => {
        setCode(e.currentTarget.value);
    };

    const handleNameChange = (e) => {
        setName(e.currentTarget.value);
    };

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const filter = () => {
        doSearch();
    };

    const handleRowClick = (e) => {
        updateParent(e.PlatformId);
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];
        for (let i = 0; i < dataToConvert.length; i++) {
            newArray.push({
                Id: dataToConvert[i].Id,
                Name: dataToConvert[i].Name,
                Code: dataToConvert[i].Code,
                Available: dataToConvert[i].Available === "1" ? "Y" : "N",
                Currency: dataToConvert[i].Currency,
                AmountOff: dataToConvert[i].AmountOff,
                Duration: dataToConvert[i].Duration,
                DurationInMonths: dataToConvert[i].DurationInMonths,
                MaxRedemptions: dataToConvert[i].MaxRedemptions,
                PercentOff: dataToConvert[i].PercentOff,
                PlatformId: dataToConvert[i].PlatformId,
                RedeemBy: dataToConvert[i].RedeemBy,
                IntervalIntended: dataToConvert[i].IntervalIntended,
                View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(dataToConvert[i])}>
                    <Translate id="general.select" options={{ renderInnerHtml: true }} />
                </button>,
                value: dataToConvert[i].PlatformId,
            });
        }
        setData(newArray);
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getCoupons(
            name,
            code,
            null,
            null,
            null,
            currentPage,
            limiter);
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            convertDataToDataTable(response.data.Coupons);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <div className="wrapper-col mb-4">
                <div className="three-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="coupons-manager.code" /></div>
                        <Input type="text" onChange={handleCodeChange} />
                    </div>
                </div>

                <div className="three-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="coupons-manager.name" /></div>
                        <Input type="text" onChange={handleNameChange} />
                    </div>
                </div>

                <div className="three-col mt-3">
                    <Button className="btn-simple--only-text" onClick={filter}>
                        <FontAwesomeIcon icon={faSearch} />{' '}
                        <Translate id="general.search" />
                    </Button>
                </div>

            </div>
            <div className="wrapper-table-admin">
                <table className="content-table-admin">
                    <thead>
                        <tr>
                            <th width="40"><Translate id="coupons-manager.id" /></th>
                            <th><Translate id="coupons-manager.name" /></th>
                            <th><Translate id="coupons-manager.redeem-by" /> (UTC) </th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((entity, index) => (
                            <tr key={index}>
                                <td data-column="ID:">{entity.Id}</td>
                                <td><span className="table-admin--title">{entity.Name}</span><p className="txt-LighterBlue">
                                    <Translate id="coupons-manager.code" />: {entity.Code} </p> <p className="txt-LighterBlue">
                                    <Translate id="coupons-manager.platform-code" />: {entity.PlatformId}</p></td>
                                <td data-column="Redimir hasta:"><span className="txt-LighterBlue">{entity.RedeemBy}</span></td>
                                <td>
                                    <p><strong><Translate id="coupons-manager.available" />:&ensp;</strong> {entity.Available} /
                                        <strong><Translate id="coupons-manager.currency" />:&ensp;</strong> {entity.Currency} /
                                        <strong><Translate id="coupons-manager.amount-off" />:&ensp;</strong> {entity.AmountOff} </p>
                                    <p><strong><Translate id="coupons-manager.duration" />:&ensp;</strong> {entity.Duration} /
                                        <strong><Translate id="coupons-manager.repeating" />: </strong>{entity.DurationInMonths} /
                                        <strong>% <Translate id="coupons-manager.discount" />:&ensp;</strong> {entity.PercentOff} </p>
                                    <p><strong><Translate id="coupons-manager.max-redemptions" />:&ensp;</strong> {entity.MaxRedemptions} </p>
                                    <p><strong><Translate id="coupons-manager.interval" />:&ensp;</strong>{entity.IntervalIntended}</p>
                                </td>
                                <td>{entity.View}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
            <Pagination totalPages={totalPages}
                currentPage={currentPage} updateParent={handlePageClick} dapper={false} />
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default CouponDataTable;