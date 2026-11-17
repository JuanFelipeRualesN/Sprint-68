/**
 * Created by portiz on 05/21/20.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col} from 'reactstrap';
import LoaderFullWindow from '../loader-windows/loader-full-window';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {getSentInvitationRequestsByEmail} from "../../../api/users";
import DataTable from './datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import ModalInvitationDelete from "../modal/modal-invitation-delete";
import ModalSendInvitationEmail from "../modal/modal-send-invitation-email";
import PaginationCustom from "./pagination";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faTrash, faUserPlus} from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Data table to handle the planning connection request by email data table.
 */
const ProfileConnectionRequestsByEmailDataTable = ({ redoSearch }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const [data, setData] = useState([]);
    const [headerOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("connections.invitation-code", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("connections.email", Settings.LANGUAGES)},
        {key: "2", name: ""},
    ]);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [showModalArchive, setShowModalArchive] = useState(false);
    const [showModalSendCode, setShowModalSendCode] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const [invitationId, setInvitationId] = useState(null);

    useEffect(() => {
        doSearch();
    }, [redoSearch, currentPage]);

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const handleSendClick = () => {
        setShowModalSendCode(true);
    };

    const handleArchiveClick = (newValue) => {
        setShowModalArchive(true);
        setInvitationId(newValue.currentTarget.value);
    };

    const toggleModalArchive = () => {
        doSearch();
        setShowModalArchive(false);
    };

    const toggleModalPlanningStudent = (e, reload) => {
        setShowModalSendCode(false);
        if(reload) {
            doSearch();
        }
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];

        if(typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
                newArray.push({
                    UserCodeToConnectWith: dataToConvert[i].UserCodeToConnectWith,
                    Email: dataToConvert[i].Email,
                    Delete: <Button className="btn btn-danger" onClick={handleArchiveClick} value={dataToConvert[i].Id}>
                        <FontAwesomeIcon icon={faTrash}/>{' '}
                        <Translate id="connections.delete-invitation" options={{renderInnerHtml: true}}/>
                    </Button>,
                    value: dataToConvert[i].Id
                });
            }
            setData(newArray);
        }
    };

    const doSearch = () => {
        setLoading(true);

        let apiCall = getSentInvitationRequestsByEmail(limiter, currentPage);
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.TotalPages);
            setData([]);
            convertDataToDataTable(response.data.ProfileConnectionInvitationByEmailModelList);
        });
    };

    return (
        <div>
            <Row>
                <Col className="text-right">
                    <Button color="success" onClick={handleSendClick}>
                        <FontAwesomeIcon icon={faUserPlus}/>{' '}
                        <Translate id="connections.send-invitation" options={{renderInnerHtml: true}}/>
                    </Button>
                </Col>
            </Row>
            <Row>
                <hr/>
            </Row>
            {data.length > 0 ?
                <div>
                    <DataTable headerOptions={headerOptions} data={data} />
                    <PaginationCustom totalPages={totalPages}
                                      currentPage={currentPage} updateParent={handlePageClick} dapper={false} />
                </div>
                : <div>
                    <Translate id="connections.there-is-no-invitations" options={{renderInnerHtml: true}}/>
                </div>
            }
            {loading ? <LoaderFullWindow/> : null}
            {showModalArchive ? <ModalInvitationDelete
                invitationId={invitationId}
                visible={showModalArchive}
                toggle={toggleModalArchive}
                updateParent={toggleModalArchive} /> : null }
            {showModalSendCode ?
                <ModalSendInvitationEmail
                    visible={showModalSendCode}
                    updateParent={toggleModalPlanningStudent}/> : null
            }
        </div>
    );
};

export default ProfileConnectionRequestsByEmailDataTable;