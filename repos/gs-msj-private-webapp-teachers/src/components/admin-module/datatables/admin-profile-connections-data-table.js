import React, { useState, useEffect, useContext } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col} from 'reactstrap';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import {Translate} from "react-localize-redux";
import {removeProfileConnection, getProfileConnectionsByPagination} from "../../../api/users";
import DataTable from "../../shared-module-components/datatables/datatable";
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import Pagination from "../../shared-module-components/datatables/pagination";
import LanguageManager from "../../../custom-classes/language-manager";
import {Alert} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUserTimes, faUserPlus, faCheck} from "@fortawesome/free-solid-svg-icons";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import ModalAddConnection from "../modals/modal-add-connection";
import { AppContext } from "../../../App.Context";
/**
 * Handles profile connections (this is almost an equivalent of friends on FB).
 */
const AdminProfileConnectionDataTable = ({ roleId, userId, name, availableDeleteButton, availableAddButton }) => {
    const context = useContext(AppContext);
    
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [headerOptions, setHeaderOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("connections.name", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("connections.user", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("connections.user-is-controlled", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("connections.remove", Settings.LANGUAGES)},
    ]);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [messageBackend, setMessageBackend] = useState(null);
    const [showModalArchive, setShowModalArchive] = useState(false);
    const [showModalAddConnection, setShowModalAddConnection] = useState(false);
    const [showModalSendCode, setShowModalSendCode] = useState(false);
    const [totalPages, setTotalPages] = useState(1);

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    useEffect(() => {
        doSearch();
    }, [currentPage]);

    const handleSendClick = () => () => {
        setShowModalAddConnection(true);
    };

    const closeModal = () => {
        setShowModalAddConnection(false);
        doSearch();
    };

    // To delete a user relation.
    const handleArchiveClick = () => (newValue) => {
        setLoading(true);
        setMessageBackend(null);

        let apiCall = removeProfileConnection(newValue.currentTarget.value, userId);
        apiCall.then((response) => {
            setLoading(false);
            setMessageBackend(null);
            
            if(typeof response.data.Messages !== 'undefined' && response.data.Messages != null && response.data.Messages.length > 0) {
                setMessageBackend(response.data.Messages);
            }
            doSearch();
        });
    };

    const toggleModalArchive = () => {
        doSearch();
        setShowModalArchive(false);
    };

    const toggleSendInvitationModal = (e, reload) => {
        setShowModalSendCode(false);
        if(reload) {
            doSearch();
        }
    };

    const convertDataToDataTable = (dataToConvert) => {
        const sessionDetails = SessionManager.getSessionDetails();
        setData([]);
        
        let newArray = [];

        if(typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
                newArray.push({
                    Name: dataToConvert[i].GivenName + ' ' + dataToConvert[i].Surname,
                    Username: dataToConvert[i].Username,
                    UserHasControl: dataToConvert[i].UserHasControl ?
                        <FontAwesomeIcon icon={faCheck} style={{color:'green'}}/> : null,
                    Delete:
                        <Button className="btn btn-danger" disabled={availableDeleteButton ? "" : "disabled"} onClick={handleArchiveClick()} value={dataToConvert[i].UserId}>
                        <FontAwesomeIcon icon={faUserTimes}/>{' '}
                        <Translate id="connections.delete-invitation" options={{renderInnerHtml: true}}/>
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

        let apiCall = getProfileConnectionsByPagination(currentPage, limiter, userId);
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.PageTotals);
            setData([]);
            convertDataToDataTable(response.data.UserInfo);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    if (roleId === Settings.ROLE_STUDENT_ID) {
        return(<>
            <div className="alert alert-info fade show">
                <Translate id="connections.relations-note-students" options={{renderInnerHtml: true}}/>
            </div>
        </>);
    }

    return (
        <div>
            {messageBackend != null ?
                <Alert color="primary">
                    {messageBackend}
                </Alert> : null}
            <Row>
            { availableAddButton ?
                <Col className="text-right">
                    <button type="button" className="btn-default button-secondary-custom" onClick={handleSendClick()}>
                        <FontAwesomeIcon icon={faUserPlus}/>{' '}
                        <Translate id="connections.add-connection" options={{renderInnerHtml: true}}/>
                    </button>
                </Col> : null}

            </Row>
            {data.length > 0 ?
                <div>
                    <DataTable headerOptions={headerOptions} data={data}  />
                    <Pagination totalPages={totalPages}
                                      currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
                </div>
                : <div>
                    <small><Translate id="connections.no-connections-admin" options={{renderInnerHtml: true}}/></small>
                </div>
            }
            {loading && <LoadingFullWindow/>}
            {showModalAddConnection && <ModalAddConnection userId={userId} name={name} visible={showModalAddConnection} updateParent={closeModal}/>}
        </div>
    );
};

export default AdminProfileConnectionDataTable;