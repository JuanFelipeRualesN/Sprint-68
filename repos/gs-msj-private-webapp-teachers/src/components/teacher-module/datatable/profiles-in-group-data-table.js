import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Col, Row, Input, Alert } from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { getProfiles } from "../../../api/users";
import { getGroupDetails, deleteStudentToGroup, addStudentToGroup } from "../../../api/teachers";
import PaginationCustom from '../../shared-module-components/datatables/pagination';
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import { faUserPlus, faSearch, faUserTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import { convertMaxLengthSting } from "../../../utils/utils";

const ProfilesInGroupDataTable = ({ groupId, groupName, roleId, redoSearch }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [dataAllStudents, setDataAllStudents] = useState([]);
    const [dataInGroups, setDataInGroups] = useState([]);
    const [firstName, setFirstName] = useState(null);
    const [lastName, setLastName] = useState(null);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(0);
    const [roles] = useState(SessionManager.getSessionDetails()?.Roles || null);
    const [triggerSearch, setTriggerSearch] = useState(false);
    const [groupUserIds, setGroupUserIds] = useState(new Set());

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("users-datatable..full-name", Settings.LANGUAGES) },
        { key: "1", name: "" },
        { key: "2", name: "" },
    ];

    const headerOptionsAllStudents = [...headerOptions];

    useEffect(() => {
        doSearch();
    }, [currentPage, redoSearch]);

    useEffect(() => {
        doSearch();
    }, [currentPage, triggerSearch]);

    const handlePageClick = (page) => {
        setCurrentPage(page);
    };

    const handleFirstNameChange = (e) => {
        setFirstName(convertMaxLengthSting(e.currentTarget.value, 100));
    };

    const handleLastNameChange = (e) => {
        setLastName(convertMaxLengthSting(e.currentTarget.value, 100));
    };

    const handleDeleteFromGroup = (e) => {
        setLoading(true);
        deleteStudentToGroup(groupId, e.currentTarget.value).then(() => {
            doSearch();
        }).finally(() => {
            setLoading(false);
        });
    };

    const handleAddToGroupClick = (e) => {
        setLoading(true);
        addStudentToGroup(groupId, e.currentTarget.value).then(() => {
            doSearch();
        }).catch(() => {
            // do nothing
        }).finally(() => {
            setLoading(false);
        });
    };

    const convertDataToDataTable = (students) => {

        // Order the students by name.
        students.sort((a, b) => {
            const nameA = `${a.FirstName} ${a.LastName}`.toUpperCase();
            const nameB = `${b.FirstName} ${b.LastName}`.toUpperCase();
            if (nameA < nameB) return -1;
            if (nameA > nameB) return 1;
            return 0;
        });

        const newArray = students.map((item) => ({
            FullName: `${item.FirstName} ${item.LastName}`,
            Record: item.UserSchoolRecord,
            DeleteFromGroup: (
                <button className="btn btn-danger" onClick={handleDeleteFromGroup} value={item.UserId}>
                    <FontAwesomeIcon icon={faUserTimes} /> <Translate id="group-management.remove-from-group" options={{ renderInnerHtml: true }} />
                </button>
            ),
            value: item.UserId
        }));
        setDataInGroups(newArray);
    };


    const convertDataToDataTableAllStudents = (students, groupUserIds) => {
        const newArray = students.map((item) => ({
            GivenName: `${item.GivenName} ${item.Surname}`,
            UserSchoolRecord: item.UserSchoolRecord,
            AddToGroup: (
                <button
                    type="button"
                    className="btn-default button-secondary-custom"
                    onClick={handleAddToGroupClick}
                    value={item.UserId}
                    disabled={groupUserIds.has(item.UserId)}
                >
                    <FontAwesomeIcon icon={faUserPlus} />{' '}
                    <Translate id="group-management.add-student-to-group" options={{ renderInnerHtml: true }} />
                </button>
            ),
            value: item.UserId
        }));

        setDataAllStudents(newArray);
    };

    const doSearch = () => {
        setLoading(true);

        let localGroupUserIds = new Set(); // <-- scoped variable

        getGroupDetails(groupId)
            .then(response => {
                const studentList = response.data.StudentDetailModelList || [];
                convertDataToDataTable(studentList);
                localGroupUserIds = new Set(studentList.map(student => student.UserId));
            })
            .then(() => {
                return getProfiles(firstName, lastName, null, currentPage, limiter, roleId);
            })
            .then(response => {
                setTotalPages(response.data.PageTotals);
                convertDataToDataTableAllStudents(response.data.UserInfo || [], localGroupUserIds); // pass directly
            })
            .finally(() => {
                setLoading(false);
            });
    };

    const filter = () => {
        setCurrentPage(0);
        setTriggerSearch(prev => !prev); // toggle to force effect run
    };

    return (
        <div>
            {loading && <LoaderFullWindow />}
            <div className="section-description mb-3 mt--5">
                <Translate id="group-management.your-students" />
            </div>
            <Row className="mt-4">
                <Col xs={12} md={5} lg={5} className="wrapper-mod simpleModule p-4">
                    <h4><span className="title-principal--inside"><Translate id="group-management.students-in-group" />:</span> <strong>{groupName}</strong></h4>
                    <hr />
                    {dataInGroups.length > 0 ? (
                        <DataTable headerOptions={headerOptions} data={dataInGroups} />
                    ) : (
                        <Alert color="danger">
                            <Translate id="group-management.no-students-in-group" />
                        </Alert>
                    )}
                </Col>
                <Col xs={12} md={7} lg={7} className="pl-4">
                    <div className="wrapper-col mb-4">
                        <div className="three-col">
                            <div className="form-group-fields">
                                <div className="label"><Translate id="users-datatable.first-name" /></div>
                                <Input type="text" value={firstName || ''} onChange={handleFirstNameChange} />
                            </div>
                        </div>
                        <div className="three-col">
                            <div className="form-group-fields">
                                <div className="label"><Translate id="users-datatable.last-name" /></div>
                                <Input type="text" value={lastName || ''} onChange={handleLastNameChange} />
                            </div>
                        </div>
                        <div className="three-col mt-3">
                            <button className="btn-w-icon" onClick={filter}>
                                <FontAwesomeIcon icon={faSearch} /> <Translate id="users-datatable.filter" />
                            </button>
                        </div>
                    </div>
                    {dataAllStudents.length > 0 ? (
                        <div>
                            <DataTable headerOptions={headerOptionsAllStudents} data={dataAllStudents} />
                            <PaginationCustom totalPages={totalPages} currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
                        </div>
                    ) : (
                        <div>
                            {roles.includes(Settings.ROLE_TEACHER_STR) ? (
                                <Translate id="users-datatable.no-found-teachers" />
                            ) : (
                                <Translate id="users-datatable.no-found" />
                            )}
                        </div>
                    )}
                </Col>
            </Row>
        </div>
    );
};

export default ProfilesInGroupDataTable;