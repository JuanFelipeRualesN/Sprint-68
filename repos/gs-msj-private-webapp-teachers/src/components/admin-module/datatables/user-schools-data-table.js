/**
 * Created by portiz on 10/07/20.
 */
import React, { useState, useEffect, useContext } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button} from 'reactstrap';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import DataTable from "../../shared-module-components/datatables/datatable";
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCheck, faTrash} from "@fortawesome/free-solid-svg-icons";
import ModalConfirmUserSchool from "../modals/school-module/modal-confirm-user-school";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import {deleteUserSchool, getUserSchools} from "../../../api/users";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import SessionManager from "../../../custom-classes/session-manager";
import { AppContext } from "../../../App.Context";

/**
 * Data table to handle the users schools.
 */
const UserSchoolsDataTable = ({ userId }) => {
    const context = useContext(AppContext);
    
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [headerOptions, setHeaderOptions] = useState([
        {key: "0", name: LanguageManager.getTranslation("users-schools.institution", Settings.LANGUAGES)},
        {key: "1", name: LanguageManager.getTranslation("users-schools.identifier", Settings.LANGUAGES)},
        {key: "2", name: LanguageManager.getTranslation("users-schools.city", Settings.LANGUAGES)},
        {key: "3", name: LanguageManager.getTranslation("users-schools.state", Settings.LANGUAGES)},
        {key: "4", name: LanguageManager.getTranslation("users-schools.country", Settings.LANGUAGES)},
        {key: "5", name: LanguageManager.getTranslation("users-schools.delete", Settings.LANGUAGES)},
        {key: "6", name: LanguageManager.getTranslation("users-schools.confirm", Settings.LANGUAGES)}
    ]);
    const [limiter, setLimiter] = useState(100);
    const [loading, setLoading] = useState(false);
    const [showConfirmModal, setShowConfirmModal] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const [visibleModalPlanningLessonAverage, setVisibleModalPlanningLessonAverage] = useState(false);
    const [institutionId, setInstitutionId] = useState(null);

    useEffect(() => {
        doSearch();
    }, []);

    const handleConfirmClick = (userIdParam, institutionIdParam) => () => {
        setInstitutionId(institutionIdParam);
        setShowConfirmModal(true);
    };

    const hideModal = (redoSearch) => {
        setShowConfirmModal(false);
        if(redoSearch) {
            doSearch();
        }
    };

    // TO delete user school.
    const handleDeleteClick = (userIdParam, institutionIdParam) => () => {
        setLoading(true);
        let apiCall = deleteUserSchool(userIdParam, institutionIdParam);
        apiCall.then(() => {
            setLoading(false);
            doSearch();
        });
    };

    const convertDataToDataTable = (dataToConvert) => {
        const sessionDetails = SessionManager.getSessionDetails();
        setData([]);
        
        let newArray = [];

        if(typeof dataToConvert !== 'undefined' && dataToConvert !== null && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
                newArray.push({
                    Institution: dataToConvert[i].InstitutionName,
                    InstitutionIdentifier: dataToConvert[i].InstitutionIdentifier,
                    City: dataToConvert[i].City,
                    State:  dataToConvert[i].State,
                    Country: dataToConvert[i].Country,
                    Delete:  (isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdminOre) ? <Button className="btn btn-danger" onClick={handleDeleteClick(dataToConvert[i].UserId, dataToConvert[i].InstitutionId)}>
                        <FontAwesomeIcon icon={faTrash}/>{' '}
                        <Translate id="users-schools.delete" options={{renderInnerHtml: true}}/>
                    </Button> : null,
                    Confirm: !dataToConvert[i].Confirmed ?
                                <Button className="btn btn-success" onClick={handleConfirmClick(dataToConvert[i].UserId, dataToConvert[i].InstitutionId)}>
                                    <FontAwesomeIcon icon={faCheck}/>{' '}
                                    <Translate id="users-schools.confirm" options={{renderInnerHtml: true}}/>
                                </Button> : null,
                    value: dataToConvert[i].Id
                });
            }
            setData(newArray);
        }
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);
        let apiCall = getUserSchools(userId);
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
            {data.length > 0 ?
                <DataTable headerOptions={headerOptions} data={data} />
                : <Translate id="users-schools.no-schools" options={{renderInnerHtml: true}}/>
            }
            {loading && <LoadingFullWindow/>}
            {
                showConfirmModal &&
                <ModalConfirmUserSchool userId={userId} institutionId={institutionId}
                                        showModal={showConfirmModal} updateParent={hideModal}/>
            }
        </div>
    );
};

export default UserSchoolsDataTable;