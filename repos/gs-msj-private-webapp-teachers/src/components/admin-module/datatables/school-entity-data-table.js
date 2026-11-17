/**
 * Created by portiz on 1/30/20.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Col, Row, Button, Input} from 'reactstrap';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {getEntitiesSchool} from "../../../api/schools";
import Pagination from "../../shared-module-components/datatables/pagination";
import DataTable from "../../shared-module-components/datatables/datatable";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch} from "@fortawesome/free-solid-svg-icons";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import "../styles/style.css"

const EntitySchoolDataTable = ({ entityType, redoSearch, updateParent }) => {
    const [city, setCity] = useState(null);
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [headerOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("school-module.id", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("school-module.name", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("school-module.invitation-code", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("school-module.address", Settings.LANGUAGES)},
        {key: "4", name: LanguageManager.getTranslation("school-module.city", Settings.LANGUAGES)},
        {key: "5", name: LanguageManager.getTranslation("school-module.state", Settings.LANGUAGES)},
        {key: "6", name: LanguageManager.getTranslation("school-module.country", Settings.LANGUAGES)},
        {key: "7", name: LanguageManager.getTranslation("school-module.zip-code", Settings.LANGUAGES)},
        {key: "8", name: LanguageManager.getTranslation("school-module.phone", Settings.LANGUAGES)},
    ]);
    const [limiter, setLimiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState(null);
    const [phone, setPhone] = useState(null);
    const [stateAddress, setStateAddress] = useState(null);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    useEffect(() => {
        doSearch();
    }, [currentPage]);

    const handleNameChange = (e) => {
        setName(e.currentTarget.value);
    };

    const handleStateAddressChange = (e) => {
        setStateAddress(e.currentTarget.value);
    };

    const handleCityAddressChange = (e) => {
        setCity(e.currentTarget.value);
    };

    const handlePhoneChange = (e) => {
        setPhone(e.currentTarget.value);
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
                Id: dataToConvert[i].EntityId,
                Name: dataToConvert[i].EntityName,
                Token: dataToConvert[i].EntityIdentifier,
                Address: dataToConvert[i].AddressLine1 ? dataToConvert[i].AddressLine1 : "" + " " + dataToConvert[i].AddressLine2 ? dataToConvert[i].AddressLine2 : "",
                City: dataToConvert[i].City,
                State: dataToConvert[i].State,
                Country: dataToConvert[i].Country,
                ZipCode: dataToConvert[i].ZipCode,
                Phone:  dataToConvert[i].Phone,
                View: <button type="button" className="btn-default button-secondary-custom" onClick={e => handleRowClick(dataToConvert[i])}>
                    <Translate id="general.select" options={{renderInnerHtml: true}}/>
                </button>,
                value: dataToConvert[i].EntityId,
            });
        }
        setData(newArray);
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getEntitiesSchool(
            entityType,
            name,
            city === "" ? null : city,
            stateAddress === "" ? null : stateAddress,
            phone === "" ? null : phone,
            currentPage,
            limiter
        );

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            convertDataToDataTable(response.data.SchoolEntities);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>

            <div className="wrapper-col mb-4">
                <div className="four-col">                
                    <div className="form-group-fields">
                        <div className="label"><Translate id="school-module.name"/></div>
                        <Input type="text" onChange={handleNameChange} />
                    </div>
                </div>

                <div className="four-col">                
                    <div className="form-group-fields">
                        <div className="label"><Translate id="school-module.phone"/></div>
                        <Input type="text" onChange={handlePhoneChange} />
                    </div>
                </div>

                <div className="four-col">                
                    <div className="form-group-fields">
                        <div className="label"><Translate id="school-module.state"/></div>
                        <Input type="text"  onChange={handleStateAddressChange} />
                    </div>
                </div>

                <div className="four-col">                
                    <div className="form-group-fields">
                        <div className="label"><Translate id="school-module.city"/></div>
                        <Input type="text"  onChange={handleCityAddressChange} />
                    </div>
                </div>

                <div className="four-col mt-3">
                    <Button className="btn-simple--only-text" onClick={filter}>
                        <FontAwesomeIcon icon={faSearch}/>{' '}
                        <Translate id="general.search"/>
                    </Button>
                </div>
            </div>
            <div className="wrapper-table-admin mt-3">
                <table className="content-table-admin">
                    <thead>
                        <tr>
                        <th width="40">ID</th>
                        <th><Translate id="school-module.name"/></th>
                        <th><Translate id="school-module.address"/></th>
                        <th></th>
                        </tr>
                    </thead>
                    <tbody>
                    {data.map((entity, index) => (
                    <tr key={index}>
                        <td data-column="ID:">{entity.Id}</td>
                        <td><span className="table-admin--title">{entity.Name}</span><p className="txt-LighterBlue">Código: {entity.Token}</p></td>
                        <td data-column="Dirección:">
                            <p>{entity.Address ? entity.Address + "." : ""} {entity.City ? entity.City + "." : ""} {entity.State ? entity.State + "." : ""} {entity.Country ? entity.Country + "." : ""} {entity.ZipCode ? entity.ZipCode : ""}</p>
                            <p>{entity.Phone}</p>
                        </td>
                        <td>{entity.View}</td>
                    </tr>
                    ))}
            </tbody>
        </table>
        </div>
        <Pagination totalPages={totalPages}
                                      currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
                    {loading && <LoadingFullWindow/>}
                </div>
            );
        };

export default EntitySchoolDataTable;