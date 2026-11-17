import React, { useEffect, useState, useCallback } from "react";
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getReceivedMessages } from "../../../api/messages";
import DataTable from '../../shared-module-components/datatables/datatable-row-action';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import {
    faEnvelope,
    faEnvelopeOpen
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {formatUtcToLocal, getTimeZone} from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";

/**
 * Data table to handle the received messages
 */
const MessagesReceivedDataTable = ({ redoSearch, updateParent }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [totalPages, setTotalPages] = useState(1);

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("messages.sender", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("messages.subject", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("messages.date", Settings.LANGUAGES) },
        { key: "3", name: "" }
    ];

    const handlePageClick = (page) => {
        setCurrentPage(page);
    };

    const handleRowClick = (id) => {
        updateParent(id);
    };

    const convertDataToDataTable = useCallback((dataToConvert) => {
        const lang = LanguageManager.getLanguage();
        moment.locale(lang);

        const newArray = [];

        if (Array.isArray(dataToConvert)) {
            for (const item of dataToConvert) {
                const row = {
                    Name: item.Read ? item.UserFullName : <b>{item.UserFullName}</b>,
                    Subject: item.Read ? item.Subject : <b>{item.Subject}</b>,
                    DateCreated: <b>{formatUtcToLocal(item.DateCreated, lang)}</b>,
                    View: (
                        <FontAwesomeIcon
                            icon={item.Read ? faEnvelopeOpen : faEnvelope}
                            value={item.Id}
                            onClick={() => handleRowClick(item.Id)}
                            size="lg"
                        />
                    ),
                    value: item.Id
                };

                newArray.push(row);
            }
        }

        setData(newArray);
    }, []);

    const doSearch = useCallback(async () => {
        setLoading(true);
        try {
            const response = await getReceivedMessages(currentPage, 10);
            setTotalPages(response.data.TotalPages || 1);
            convertDataToDataTable(response.data.EmailMessageModelList || []);
        } catch (error) {
            // optionally log error
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
                <Translate id="messages.no-received" options={{ renderInnerHtml: true }} />
            </div>
        );
    }

    return (
        <div className="td-received-w-bold syle-date-three delete-before-col-2">
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

export default MessagesReceivedDataTable;