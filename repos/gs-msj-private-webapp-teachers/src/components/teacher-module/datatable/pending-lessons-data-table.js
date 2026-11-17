import React, { useState, useEffect, useCallback } from "react";
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { getPendingLessonsAssignments } from "../../../api/teachers";
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import DataTable from "../../shared-module-components/datatables/datatable";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { getTimeZone } from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";

/**
 * Data table to show the pending lessons of the students.
 */
const PendingLessonsDataTable = ({ studentId, planningId }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const limiter = 10;

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("pending-assignments.lesson", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("assignments.due-date", Settings.LANGUAGES) }
    ];

    const getStyleWarning = useCallback((dueDate) => {
        const now = moment();
        const warn = moment().add(1, "days");
        const date = moment(dueDate, "MM/DD/YYYY HH:mm:ss");

        if (date < now) return "status-time-test--alert";
        if (date < warn) return "status-time-test--pending";
        return "status-time-test--process";
    }, []);

    const convertDataToDataTable = useCallback((dataToConvert) => {
        const lang = LanguageManager.getLanguage();
        moment.locale(lang);

        const newArray = [];

        if (Array.isArray(dataToConvert) && dataToConvert.length > 0) {
            for (let item of dataToConvert) {
                const dateMoment = moment(item.DueDate, "MM/DD/YYYY HH:mm:ss").utc().tz(getTimeZone());

                const formattedDate = lang === "es"
                    ? dateMoment.format("DD [de] MMMM [de] YYYY hh:mm A")
                    : dateMoment.format("MMMM DD, YYYY hh:mm A");

                newArray.push({
                    LessonTitle: item.LessonTitle,
                    DueDate: item.DueDate ? (
                        <div className={getStyleWarning(item.DueDate)}>
                            {formattedDate}
                        </div>
                    ) : "",
                    value: item.Id
                });
            }
        }

        setData(newArray);
    }, [getStyleWarning]);

    const doSearch = useCallback(async () => {
        setLoading(true);
        try {
            const response = await getPendingLessonsAssignments(studentId, planningId, currentPage, limiter);
            setTotalPages(response.data.TotalPages);
            convertDataToDataTable(response.data.LessonQuizModelList);
        } catch (e) {
            // Optionally log or handle the error
        } finally {
            setLoading(false);
        }
    }, [studentId, planningId, currentPage, convertDataToDataTable]);

    useEffect(() => {
        doSearch();
    }, [doSearch]);

    const handlePageClick = (page) => {
        setCurrentPage(page);
    };

    return (
        <div>
            <DataTable headerOptions={headerOptions} data={data} />
            {loading && <LoaderFullWindow />}
            <PaginationCustom
                totalPages={totalPages}
                currentPage={currentPage}
                updateParent={handlePageClick}
                dapper={true}
            />
        </div>
    );
};

export default PendingLessonsDataTable;
