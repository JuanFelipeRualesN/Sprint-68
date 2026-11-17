import React, { useEffect, useState, useCallback } from "react";
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button } from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { getTeacherQuizzes } from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    faCheck, faEdit, faEye, faTrash
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ModalHomeworkArchive from "../modal/modal-homework-archive";
import ModalExamPreview from "../modal/modal-exam-preview";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { getTimeZone } from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";


/**
 * Data table to handle the teacher quizzes
 */
const TeacherQuizzesDataTable = ({ redoSearch, updateParent }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [limiter] = useState(100);
    const [loading, setLoading] = useState(false);
    const [showModalArchive, setShowModalArchive] = useState(false);
    const [showModalExamPreview, setShowModalExamPreview] = useState(false);
    const [quizId, setQuizId] = useState(null);
    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("planning.title", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("planning.published", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("planning.created-date", Settings.LANGUAGES) },
        { key: "3", name: "" },
        { key: "4", name: "" },
        { key: "5", name: "" },
    ];

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    const handlePageClick = (e) => {
        setCurrentPage(e);
        doSearch();
    };

    const handleViewClick = (e) => {
        updateParent(e.currentTarget.value);
    };

    const handleArchiveClick = (e) => {
        setQuizId(e.currentTarget.value);
        setShowModalArchive(true);
    };

    const handlePreviewClick = (e) => {
        e.persist();
        setQuizId(e.currentTarget.value);
        setShowModalExamPreview(true);
    };

    const toggleModalArchive = useCallback(() => {
        doSearch();
        setShowModalArchive(false);
    }, []);

    const toggleModalPreview = () => {
        setShowModalExamPreview(false);
    };

    const convertDataToDataTable = (dataToConvert) => {
        const newArray = [];

        if (Array.isArray(dataToConvert) && dataToConvert.length > 0) {
            dataToConvert.forEach(item => {
                if (item.Available === "0") return;

                const published = item.ReadyToPublish === "1"
                    ? <FontAwesomeIcon icon={faCheck} className="green-text" />
                    : "";

                newArray.push({
                    Title: item.Title,
                    ReadyToPublish: published,
                    CreatedAt: item.CreatedAt
                        ? moment.utc(item.CreatedAt, "M/DD/YYYY H:mm:ss a")
                            .tz(getTimeZone())
                            .format("MM/DD/YYYY hh:mm a")
                        : "",
                    Preview: (
                        <button type="button" className="btn-default button-secondary-custom" onClick={handlePreviewClick} value={item.Id}>
                            <FontAwesomeIcon icon={faEye} />{" "}
                            <Translate id="planning.preview" options={{ renderInnerHtml: true }} />
                        </button>
                    ),
                    View: item.ReadyToPublish !== "1" ? (
                        <button type="button" className="btn-default button-primary-custom" onClick={handleViewClick} value={item.Id}>
                            <FontAwesomeIcon icon={faEdit} />{" "}
                            <Translate id="planning.edit" options={{ renderInnerHtml: true }} />
                        </button>
                    ) : null,
                    Available: (
                        <Button className="btn btn-danger" onClick={handleArchiveClick} value={item.Id}>
                            <FontAwesomeIcon icon={faTrash} />{" "}
                            <Translate id="planning.archive" options={{ renderInnerHtml: true }} />
                        </Button>
                    ),
                    value: item.Id,
                });
            });

            setData(newArray);
        }
    };

    const doSearch = async () => {
        setLoading(true);
        try {
            const response = await getTeacherQuizzes(false);
            convertDataToDataTable(response.data.QuizModelList);
        } catch (error) {
            console.error("Failed to load teacher quizzes", error);
        }
        setLoading(false);
    };

    return (
        <div>
            <DataTable headerOptions={headerOptions} data={data} />
            {loading && <LoaderFullWindow />}
            {showModalArchive && (
                <ModalHomeworkArchive
                    quizId={quizId}
                    visible={showModalArchive}
                    toggle={toggleModalArchive}
                    updateParent={toggleModalArchive}
                />
            )}
            {showModalExamPreview && (
                <ModalExamPreview
                    quizId={quizId}
                    visible={showModalExamPreview}
                    preview={true}
                    toggle={toggleModalPreview}
                    updateParent={toggleModalPreview}
                />
            )}
        </div>
    );
};

export default TeacherQuizzesDataTable;