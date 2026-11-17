/**
 * Created by portiz on 09/28/20.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row} from 'reactstrap';
import LoaderFullWindow from '../loader-windows/loader-full-window';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {acceptProfileConnectionRequest, removeProfileConnectionRequest, getProfilesConnectionsRequests} from "../../../api/users";
import DataTable from './datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import PaginationCustom from "./pagination";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faBan, faCheck} from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Handles profile connections requests (this is almost an equivalent of friends on FB).
 */
const ProfileConnectionRequestsDataTable = ({redoSearch}) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [headerOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("connections.name", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("connections.user", Settings.LANGUAGES)},
        {key: "2", name: ""},
        {key: "3", name: LanguageManager.getTranslation("connections.accept", Settings.LANGUAGES)},
    ]);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        doSearch();
    }, [currentPage, redoSearch]);

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    // To delete an user relation.
    const handleDenyClick = (newValue) => {
        setLoading(true);

        let apiCall = removeProfileConnectionRequest(newValue.currentTarget.value);
        apiCall.then(() => {
            setLoading(false);
            doSearch();
        });
    };

    const handleAcceptClick = (newValue) => {
        setLoading(true);

        let apiCall = acceptProfileConnectionRequest(newValue.currentTarget.value);
        apiCall.then(() => {
            setLoading(false);
            doSearch();
        });
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];

        if(typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {

                newArray.push({
                    Name: dataToConvert[i].GivenName + ' ' + dataToConvert[i].Surname,
                    Username: dataToConvert[i].Username,
                    Deny: <Button className="btn btn-danger" onClick={handleDenyClick} value={dataToConvert[i].UserId}>
                        <FontAwesomeIcon icon={faBan}/>{' '} <Translate id="connections.deny" options={{renderInnerHtml: true}}/>
                    </Button>,
                    Accept: <Button className="btn btn-success" onClick={handleAcceptClick} value={dataToConvert[i].UserId}>
                        <FontAwesomeIcon icon={faCheck}/>{' '}<Translate id="connections.accept" options={{renderInnerHtml: true}}/>
                    </Button>,
                    value: dataToConvert[i].UserId
                });
            }
            setData(newArray);
        }
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getProfilesConnectionsRequests(currentPage, limiter);
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            setData([]);
            convertDataToDataTable(response.data.UserInfo);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Row>
                <hr/>
            </Row>
            {data.length > 0 ?
                <div>
                    <DataTable headerOptions={headerOptions} data={data} />
                    <PaginationCustom totalPages={totalPages}
                                      currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
                </div>
                : <div>
                    <Translate id="connections.no-requests" options={{renderInnerHtml: true}}/>
                </div>
            }
            {loading ? <LoaderFullWindow/> : null}
        </div>
    );
};

export default ProfileConnectionRequestsDataTable;