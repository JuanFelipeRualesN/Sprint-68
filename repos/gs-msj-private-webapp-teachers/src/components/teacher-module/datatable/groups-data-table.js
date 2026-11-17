import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button} from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import {Translate} from "react-localize-redux";
import {getTeacherGroups} from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    faCheck,
    faTrash,
    faEdit
} from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import ModalGroupArchive from "../modal/modal-group-archive";
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Data table to handle the teacher groups
 */
const GroupsDataTable = ({ redoSearch, updateParent }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const [showModalArchive, setShowModalArchive] = useState(false);
    const [groupId, setGroupId] = useState(null);

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("group-management.name", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("group-management.subject-and-grade", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("group-management.students-quantity", Settings.LANGUAGES) },
        { key: "3", name: "" },
        { key: "4", name: "" },
    ];

    useEffect(() => {
        doSearch();
    }, [currentPage]);

    useEffect(() => {
        if (redoSearch) doSearch();
    }, [redoSearch]);

    const doSearch = () => {
        setLoading(true);
        getTeacherGroups(limiter, currentPage).then((response) => {
            setLoading(false);
            setTotalPages(response.data.TotalPages);
            convertDataToDataTable(response.data.TeacherGroupModelList);
        });
    };

    const convertDataToDataTable = (dataToConvert) => {
        if (!dataToConvert || dataToConvert.length === 0) {
            setData([]);
            return;
        }

        const newArray = dataToConvert.filter(item => item.Available !== "0").map(item => {
            const published = item.ReadyToPublish === "1" ? <FontAwesomeIcon icon={faCheck} className="green-text" /> : '';
            return {
                Name: item.Name,
                SubjectAndGrade: item.SubjectCode ? `${item.SubjectCode} ${item.LevelCode}` : "",
                TotalStudents: item.TotalStudents,
                View: (
                    <button type="button" className="button-secondary-custom btn-default" onClick={() => handleViewClick(item.Id, item.Name)}>
                        <FontAwesomeIcon icon={faEdit} size="lg" />{' '}
                        <Translate id="group-management.update-group" options={{ renderInnerHtml: true }} />
                    </button>
                ),
                Available: (
                    <Button className="btn btn-danger" onClick={() => handleArchiveClick(item.Id)}>
                        <FontAwesomeIcon icon={faTrash} size="lg" />
                    </Button>
                ),
                value: item.Id
            };
        });
        setData(newArray);
    };

    const handlePageClick = (page) => {
        setCurrentPage(page);
    };

    const handleViewClick = (value, name) => {
        updateParent(value, name);
    };

    const handleArchiveClick = (id) => {
        setGroupId(id);
        setShowModalArchive(true);
    };

    const toggleModalArchive = () => {
        setShowModalArchive(false);
        doSearch();
    };

    if (data.length === 0) {
        return (
            <div>
                {loading && <LoaderFullWindow />}
                <Translate id="group-management.no-groups-found" options={{ renderInnerHtml: true }} />
            </div>
        );
    }

    return (
        <div>
            <DataTable headerOptions={headerOptions} data={data} />
            <PaginationCustom totalPages={totalPages} currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
            {loading && <LoaderFullWindow />}
            {showModalArchive && (
                <ModalGroupArchive
                    groupId={groupId}
                    visible={showModalArchive}
                    toggle={toggleModalArchive}
                    updateParent={toggleModalArchive}
                />
            )}
        </div>
    );
};

export default GroupsDataTable;