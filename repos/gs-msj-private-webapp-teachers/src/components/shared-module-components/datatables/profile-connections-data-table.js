import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col} from 'reactstrap';
import LoaderFullWindow from '../loader-windows/loader-full-window';
import {Translate} from "react-localize-redux";
import {removeProfileConnection,getProfileConnectionsByPagination} from "../../../api/users";
import DataTable from './datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import ModalInvitationDelete from "../modal/modal-invitation-delete";
import ModalSendInvitationCode from "../modal/modal-send-invitation-code";
import PaginationCustom from "./pagination";
import LanguageManager from "../../../custom-classes/language-manager";
import {Alert} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUserTimes, faUserPlus} from "@fortawesome/free-solid-svg-icons";
import Settings from "../../../custom-classes/settings";

/**
 * Handles profile connections (this is almost an equivalent of friends on FB).
 */
const ProfileConnectionsDataTable = ({ redoSearch }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const [messageBackend, setMessageBackend] = useState(null);
    const [showModalArchive, setShowModalArchive] = useState(false);
    const [showModalSendCode, setShowModalSendCode] = useState(false);
    const [invitationId, setInvitationId] = useState(null);
    const [limiter] = useState(10);

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("connections.name", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("connections.user", Settings.LANGUAGES) },
        { key: "2 ", name: "" },
    ];

    useEffect(() => {
        doSearch();
    }, [currentPage, redoSearch]);

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const handleSendClick = () => {
        setShowModalSendCode(true);
    };

    const handleArchiveClick = (e) => {
        setLoading(true);
        setMessageBackend(null);

        removeProfileConnection(e.currentTarget.value).then((response) => {
        setLoading(false);
        setMessageBackend(null);
        if (response.data.Messages?.length > 0) {
            setMessageBackend(response.data.Messages);
        }
        doSearch();
        });
    };

    const toggleModalArchive = () => {
        doSearch();
        setShowModalArchive(false);
    };

    const toggleSendInvitationModal = () => {
        setShowModalSendCode(false);
        setCurrentPage(0);
        doSearch();
    };

    const convertDataToDataTable = (dataToConvert) => {
        const newArray = dataToConvert?.map((item) => ({
        Name: item.GivenName + " " + item.Surname,
        Username: item.Username,
        Delete: (
            <Button className="btn btn-danger" onClick={handleArchiveClick} value={item.UserId}>
            <FontAwesomeIcon icon={faUserTimes} />{' '}
            <Translate id="connections.delete-invitation" options={{ renderInnerHtml: true }} />
            </Button>
        ),
        value: item.UserId,
        }));
        setData(newArray);
    };

    const doSearch = () => {
        setLoading(true);
        getProfileConnectionsByPagination(currentPage, limiter).then((response) => {
        setLoading(false);
        setTotalPages(response.data.PageTotals);
        convertDataToDataTable(response.data.UserInfo);
        });
    };

    return (
            <div>
                {messageBackend != null ?
                    <Alert color="primary">
                    {messageBackend}
                </Alert> : null}
                <Row>
                    <Col className="text-right">
                        <button className='button-primary-custom' onClick={handleSendClick}>
                            <FontAwesomeIcon icon={faUserPlus}/>{' '}
                            <Translate id="connections.send-request" options={{renderInnerHtml: true}}/>
                        </button>
                    </Col>
                </Row>
                <Row>
                    <hr/>
                </Row>
                {data.length > 0 ?
                    <div>
                        <DataTable headerOptions={headerOptions} data={data} />
                        <PaginationCustom
                            totalPages={totalPages}
                            currentPage={currentPage}
                            updateParent={handlePageClick}
                            dapper={true}
                        />
                    </div>
                    : <div>
                        <Translate id="connections.no-connections" options={{renderInnerHtml: true}}/>
                    </div>
                }
                {loading ? <LoaderFullWindow/> : null}
                {showModalArchive ? <ModalInvitationDelete
                    invitationId={invitationId}
                    visible={showModalArchive}
                    toggle={toggleModalArchive}
                    updateParent={toggleModalArchive} /> : null }
                {showModalSendCode ?
                    <ModalSendInvitationCode
                        visible={showModalSendCode}
                        updateParent={toggleSendInvitationModal}/> : null
                }

            </div>
        );
};

export default ProfileConnectionsDataTable;