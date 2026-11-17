import React, { useEffect, useState, useCallback } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getSentMessages } from "../../../api/messages";
import DataTable from '../../shared-module-components/datatables/datatable-row-action';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import { faEnvelopeOpen } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {formatUtcToLocal, getTimeZone} from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";

/**
 * Data table to handle the sent messages
 */
const MessagesSentDataTable = ({ redoSearch, updateParent }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const limiter = 10;
    const language = LanguageManager.getLanguage();

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("messages.subject", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("messages.date", Settings.LANGUAGES) },
        { key: "2", name: "" },
    ];

    const handlePageClick = (page) => {
        setCurrentPage(page);
    };

    const handleRowClick = (id) => {
        updateParent(id);
    };

    const convertDataToDataTable = useCallback((dataToConvert) => {
        const newArray = dataToConvert.map((item) => {
            return {
                Subject: item.Subject,
                DateCreated: formatUtcToLocal(item.DateCreated, language),
                View: (
                    <FontAwesomeIcon
                        className="text-color-orange"
                        icon={faEnvelopeOpen}
                        value={item.Id}
                        onClick={() => handleRowClick(item.Id)}
                        size="lg"
                    />
                ),
                value: item.Id,
            };
        });

        setData(newArray);
    }, []);

    const doSearch = useCallback(async () => {
        setLoading(true);
        try {
            const response = await getSentMessages(currentPage, limiter);
            setTotalPages(response.data.TotalPages);
            convertDataToDataTable(response.data.EmailMessageModelList);
        } catch (error) {
            // Optionally handle error
        } finally {
            setLoading(false);
        }
    }, [currentPage, convertDataToDataTable]);

    useEffect(() => {
        doSearch();
    }, [doSearch]);

    useEffect(() => {
        if (redoSearch) {
            doSearch();
        }
    }, [redoSearch, doSearch]);

    if (data.length === 0 && !loading) {
        return (
            <div>
                <Translate id="messages.no-sent" options={{ renderInnerHtml: true }} />
            </div>
        );
    }

    return (
        <div className="td-sent-w-bold syle-date-two delete-before-col-1">
            {loading && <LoaderFullWindow />}
            <DataTable headerOptions={headerOptions} data={data} updateParent={handleRowClick} />
            <PaginationCustom
                totalPages={totalPages}
                currentPage={currentPage}
                updateParent={handlePageClick}
                dapper={true}
            />
        </div>
    );
};

export default MessagesSentDataTable;