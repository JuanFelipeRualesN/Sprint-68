/**
 * Created by portiz on 10/09/20.
 */
import React, { useState, useEffect, useContext } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col, CardHeader} from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCheck, faTrash, faUserGraduate} from "@fortawesome/free-solid-svg-icons";
import {deleteUserSchool, getUserSchools} from "../../../api/users";
import ModalAddUserSchool from "../modal/modal-add-user-school";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import SessionManager from "../../../custom-classes/session-manager";
import { AppContext } from "../../../App.Context";

/**
 * Data table to handle the users schools.
 */
const UserSchoolsDataTable = ({ userId }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const [data, setData] = useState([]);
    const [headerOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("users-schools.institution", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("users-schools.identifier", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("users-schools.city", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("users-schools.state", Settings.LANGUAGES)},
        {key: "4", name: LanguageManager.getTranslation("users-schools.country", Settings.LANGUAGES)},
        {key: "5", name: ""},
        {key: "6", name: LanguageManager.getTranslation("users-schools.confirmed", Settings.LANGUAGES)},
    ]);
    const [limiter] = useState(100);
    const [loading, setLoading] = useState(false);
    const [showConfirmModal, setShowConfirmModal] = useState(false);
    const [currentUserId] = useState(userId);
    const [totalPages, setTotalPages] = useState(1);
    const [visibleModalPlanningLessonAverage] = useState(false);

    const context = useContext(AppContext);
    const sessionDetails = SessionManager.getSessionDetails();

    useEffect(() => {
        // Call data
        doSearch();
    }, []);

    const handleConfirmClick = () => {
        setShowConfirmModal(true);
    };

    const hideModal = (redoSearch) => {
        setShowConfirmModal(false);
        if(redoSearch)
        {
            doSearch();
        }
    };

    // To delete user school.
    const handleDeleteClick = (userId, institutionId) => () => {
        setLoading(true);
        let apiCall = deleteUserSchool(userId, institutionId);
        apiCall.then(() => {
            setLoading(false);
            doSearch();
        });
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];

        if (typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
            newArray.push({
                Institution: dataToConvert[i].InstitutionName,
                InstitutionIdentifier: dataToConvert[i].InstitutionIdentifier,
                City: dataToConvert[i].City,
                State: dataToConvert[i].State,
                Country: dataToConvert[i].Country,
                Delete: (isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdmin) || 
                        (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdminOre)
                ? (
                    <Button className="btn btn-danger" onClick={handleDeleteClick(dataToConvert[i].UserId, dataToConvert[i].InstitutionId)}>
                        <FontAwesomeIcon icon={faTrash} />{' '}
                        <Translate id="users-schools.delete" options={{ renderInnerHtml: true }} />
                    </Button>
                    )
                : null,
                Confirm: dataToConvert[i].Confirmed ? (
                <FontAwesomeIcon icon={faCheck} style={{ color: 'green' }} />
                ) : null,
                value: dataToConvert[i].Id
            });
            }

            setData(newArray);
        }
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);
        let apiCall = getUserSchools(currentUserId);
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(1);
            setData([]);
            convertDataToDataTable(response.data);
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Row>
            {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdminOre)) && <Col xs={12} className="text-right">
                    <button type="button" className="btn-default button-secondary-custom" onClick={handleConfirmClick}>
                        <FontAwesomeIcon icon={faUserGraduate}/>{' '}
                        <Translate id="users-schools.add-user-to-institution"/>
                    </button>
                </Col>}
            </Row>
            {data.length > 0 ?
                <DataTable headerOptions={headerOptions} data={data} />
                :<b><small><Translate id="users-schools.no-schools" options={{renderInnerHtml: true}}/></small></b>
            }
            {loading ? <LoaderFullWindow/> : null}
            {
                showConfirmModal ?
                    <ModalAddUserSchool userId={currentUserId}
                                        showModal={showConfirmModal} updateParent={hideModal}/> : null
            }
        </div>
    );
};

export default UserSchoolsDataTable;