import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Input } from 'reactstrap';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import {Translate} from "react-localize-redux";
import {getPlansAdvanced} from "../../../api/subscriptions";
import Pagination from "../../shared-module-components/datatables/pagination";
import DataTable from "../../shared-module-components/datatables/datatable";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch} from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

const PlanDataTable = ({ redoSearch, updateParent, isCommercial }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const [data, setData] = useState([]);
    const [headerOptions, setHeaderOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("plan.plan-id", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("plan.plan-name", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("plan.interval", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("plan.currency", Settings.LANGUAGES)},
        {key: "4", name: LanguageManager.getTranslation("plan.amount", Settings.LANGUAGES)},
        {key: "5", name: LanguageManager.getTranslation("plan.profiles-quantity", Settings.LANGUAGES)},
        {key: "6", name: ""}
    ]);
    const [limiter, setLimiter] = useState(5);
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState(null);
    const [planId, setPlanId] = useState(null);
    const [platformId, setPlatformId] = useState(null);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    useEffect(() => {
        setHeaderOptions(
            [
                {key: "0", name: LanguageManager.getTranslation("plan.plan-id", Settings.LANGUAGES)},
                {key: "1", name: LanguageManager.getTranslation("plan.plan-name", Settings.LANGUAGES)},
                {key: "2", name: LanguageManager.getTranslation("plan.interval", Settings.LANGUAGES)},
                {key: "3", name: LanguageManager.getTranslation("plan.currency", Settings.LANGUAGES)},
                {key: "4", name: LanguageManager.getTranslation("plan.amount", Settings.LANGUAGES)},
                {key: "5", name: LanguageManager.getTranslation("plan.profiles-quantity", Settings.LANGUAGES)},
                {key: "6", name: ""}
            ]
        )
    }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        if (currentPage !== 0) {
            doSearch();
        }
    }, [currentPage]);

    const handlePlanIdChange = (e) => {
        setPlanId(e.currentTarget.value);
    };

    const handlePlanStripeChange = (e) => {
        setPlatformId(e.currentTarget.value);
    };

    const handlePlanName = (e) => {
        setName(e.currentTarget.value);
    };

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const filter = () => {
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
                Id: dataToConvert[i].Id,
                Name: dataToConvert[i].Name,
                Interval: Settings.IS_APP_MANAGED_BY_REQUIRED ? dataToConvert[i].Interval : "",
                Currency: Settings.IS_APP_MANAGED_BY_REQUIRED ? dataToConvert[i].Currency : "",
                Amount:  Settings.IS_APP_MANAGED_BY_REQUIRED ? dataToConvert[i].Amount : "",
                ProfilesQuantity: Settings.IS_APP_MANAGED_BY_REQUIRED ? dataToConvert[i].ProfilesQuantity: "",
                View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(dataToConvert[i])}>
                    <Translate id="general.select" options={{renderInnerHtml: true}}/>
                </button>,
                value: dataToConvert[i].Id,
            });
        }
        setData(newArray);
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getPlansAdvanced(planId,
            platformId, name, currentPage,
            limiter, true, isCommercial);
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            convertDataToDataTable(response.data.Plans);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <div className="wrapper-col mb-4">
                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="plan.plan-id"/></div>
                        <Input type="text" onChange={handlePlanIdChange} />
                    </div>
                </div>
                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="plan.plan-stripe-id"/></div>
                        <Input type="text" onChange={handlePlanStripeChange} />
                    </div>
                </div>

                <div className="four-col">
                    <div className="form-group-fields">
                        <div className="label"><Translate id="plan.plan-name"/></div>
                        <Input type="text"  onChange={handlePlanName} />
                    </div>
                </div>

                <div className="four-col mt-3">
                    <Button className="btn-simple--only-text" onClick={filter}>
                        <FontAwesomeIcon icon={faSearch}/>{' '}
                        <Translate id="general.search"/>
                    </Button>
                </div>

            </div>
            {/* <Row>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                        <Translate id="plan.plan-id"/>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Input type="text" onChange={handlePlanIdChange} />
                        </Col>
                    </Row>
                </Col>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Translate id="plan.plan-stripe-id"/>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Input type="text" onChange={handlePlanStripeChange} />
                        </Col>
                    </Row>
                </Col>
            </Row>

            <Row>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                    <Row>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Translate id="plan.plan-name"/>
                        </Col>
                        <Col lg={{size: 6}} md={{size: 6}} sm={{size: 6}} xs={{size: 6}}>
                            <Input type="text"  onChange={handlePlanName} />
                        </Col>
                    </Row>
                </Col>
                <Col lg={{size: 6}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                </Col>
            </Row>

            <Row>
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
                              currentPage={currentPage} updateParent={handlePageClick} dapper={false}/>
            {loading && <LoadingFullWindow/>}
        </div>
    );
};

export default PlanDataTable;