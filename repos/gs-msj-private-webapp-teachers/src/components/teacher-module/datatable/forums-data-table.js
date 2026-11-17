import React, {useState, useEffect, useCallback} from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Col, Row } from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getTeacherForums } from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    faCheck,
    faTrash,
    faEdit, faPlus, faComments
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ModalForumArchive from "../modal/modal-forum-archive";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import SessionManager from "../../../custom-classes/session-manager";
import { Redirect } from "react-router-dom";
import ModalForumCreate from "../modal/modal-forum-create";
import { getTimeZone } from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";

/**
 * Data table to handle the forums
 */
const ForumsDataTable = ({ disabled: propDisabled, teacherPlanId: propTeacherPlanId, redoSearch }) => {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [showModalArchive, setShowModalArchive] = useState(false);
    const [showModalAddForum, setShowModalAddForum] = useState(false);
    const [redirectToForum, setRedirectToForum] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const [forumId, setForumId] = useState(null);

    const disabled = propDisabled ?? false;
    const teacherPlanId = propTeacherPlanId ?? null;

    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("forum.title", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("forum.created-date", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("forum.view", Settings.LANGUAGES) },
        { key: "3", name: "" },
    ];

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const handleViewClick = (e) => {
        setForumId(e.forumId);
        setRedirectToForum(true);
        SessionManager.setSelectedForum(e.currentTarget.value);
    };

    const handleArchiveClick = (newValue) => {
        setForumId(newValue.currentTarget.value);
        setShowModalArchive(true);
    };

    const toggleModalArchive = () => {
        doSearch();
        setShowModalArchive(false);
    };

    const handleCloseModalAddForum = () => {
        doSearch();
        setShowModalAddForum(false);
    };

    const handleShowModalAddForum = () => {
        setShowModalAddForum(true);
    };

    const convertDataToDataTable = useCallback((dataToConvert) => {
        const newArray = [];

        if (Array.isArray(dataToConvert)) {
            dataToConvert.forEach((item) => {
                if (item.Available === "0") return;

                const published = item.ReadyToPublish === "1" ? (
                    <FontAwesomeIcon icon={faCheck} className="green-text" />
                ) : '';

                newArray.push({
                    Name: item.Title,
                    CreatedAt: item.CreatedAt
                        ? moment.utc(item.CreatedAt, "M/DD/YYYY H:mm:ss a").tz(getTimeZone()).format("MM/DD/YYYY hh:mm a")
                        : "",
                    View: (
                        <button className="btn-default button-secondary-custom" onClick={(e) => handleViewClick(e)} value={item.Id}>
                            <FontAwesomeIcon icon={faEdit} /> <Translate id="forum.go" options={{ renderInnerHtml: true }} />
                        </button>
                    ),
                    Available: (
                        <Button className="btn btn-danger" onClick={(e) => handleArchiveClick(e)} value={item.Id} disabled={disabled}>
                            <FontAwesomeIcon icon={faTrash} /> <Translate id="forum.archive" options={{ renderInnerHtml: true }} />
                        </Button>
                    ),
                    value: item.Id
                });
            });
        }

        setData(newArray);
    }, [disabled]);

    const doSearch = useCallback(() => {
        setLoading(true);
        setData([]);

        const apiCall = getTeacherForums(teacherPlanId, currentPage, limiter);

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.TotalPages || 1);
            convertDataToDataTable(response.data.ForumModelList || []);
        });
    }, [teacherPlanId, currentPage, limiter, convertDataToDataTable]);

    useEffect(() => {
        doSearch();
    }, [doSearch]);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    if (redirectToForum) {
        return (
            <Redirect
                to={{
                    pathname: "/forum-details",
                    forumId,
                    disabled,
                    teacherPlanId
                }}
            />
        );
    }

    return (
        <div className="animated delay-05s fadeIn danger-btn-simple space-btn-icon secondary-btn-simple table-no-margin-top td-responsive syle-date-two inline-table-4">
            <Row>
                <Col className="text-right mt-3 mb-3">
                    <button className="btn-default button-primary-custom" onClick={handleShowModalAddForum} disabled={disabled}>
                        <FontAwesomeIcon icon={faPlus} /> <Translate id="planning.forums-create" options={{ renderInnerHtml: true }} />
                    </button>
                </Col>
            </Row>
            <Row>
                <Col xs={12}>
                    {data.length === 0 ? (
                        <div body className="animated delay-05s fadeIn">
                            <div className="wrapper-not-found-planning-details">
                                <p style={{ textAlign: 'center' }}>
                                    <FontAwesomeIcon icon={faComments} size='3x' />{' '}
                                </p>
                                <Translate id="planning.planning-no-forums" options={{ renderInnerHtml: true }} />
                            </div>
                        </div>
                    ) : (
                        <>
                            <DataTable headerOptions={headerOptions} data={data} />
                            <PaginationCustom totalPages={totalPages} currentPage={currentPage} updateParent={handlePageClick} dapper={true} />
                        </>
                    )}
                </Col>
            </Row>
            {loading && <LoaderFullWindow />}
            {showModalArchive && (
                <ModalForumArchive
                    forumId={forumId}
                    visible={showModalArchive}
                    toggle={toggleModalArchive}
                    updateParent={toggleModalArchive}
                />
            )}
            {showModalAddForum && (
                <ModalForumCreate
                    teacherPlanId={teacherPlanId}
                    visible={showModalAddForum}
                    toggle={handleCloseModalAddForum}
                    updateParent={handleCloseModalAddForum}
                />
            )}
        </div>
    );
};

export default ForumsDataTable;
