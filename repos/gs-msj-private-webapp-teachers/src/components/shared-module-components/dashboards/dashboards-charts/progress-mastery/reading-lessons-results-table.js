import React, { useCallback, useEffect, useRef, useState } from "react";
import { Button, Card, CardBody, CardHeader, Input } from "reactstrap";
import { Translate, withLocalize } from "react-localize-redux";
import { BeatLoader } from "react-spinners";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFilePdf, faSearch, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import DataTable from "../../../datatables/datatable";
import Paginationn from "../../../datatables/pagination";
import { getReadingLessonsResults } from "../../../../../api/reading-api";
import { getUserQuizDetails } from "../../../../../api/gsweb";
import LanguageManager from "../../../../../custom-classes/language-manager";
import { formatUtcToLocal } from "../../../../../utils/utils";
import { getExamTypeLabelKey, getSubjectLabelKey, getGradeLabel } from "./reading-results-labels";

// Only teachers and homeschooling accounts can read this report
export const canSeeReadingLessonsResults = (userRole) => {
    return userRole === "teacher" || userRole === "client";
};

// Results of the Athenas reading evaluations (R and LLI standards, K to 5th grade, sp, en, adl subjects)
const ReadingLessonsResultsTable = ({ translate, userRole, period, dateFrom, dateTo, userIds, schoolId, regionId }) => {
    const [items, setItems] = useState([]);
    const [totalPages, setTotalPages] = useState(0);
    const [page, setPage] = useState(0);
    const [loading, setLoading] = useState(false);
    const [downloadingId, setDownloadingId] = useState(null);
    const language = LanguageManager.getLanguage();
    const limiter = 10;

    const [filterDraft, setFilterDraft] = useState({ givenName: "", surname: "", username: "" });
    const [appliedFilters, setAppliedFilters] = useState({ givenName: "", surname: "", username: "" });

    const updateDraft = (field, value) => {
        setFilterDraft((previous) => ({ ...previous, [field]: value }));
    };

    const applyFilters = () => {
        setAppliedFilters(filterDraft);
    };

    const clearFilters = () => {
        const empty = { givenName: "", surname: "", username: "" };
        setFilterDraft(empty);
        setAppliedFilters(empty);
    };

    const hasFilters = Boolean(appliedFilters.givenName || appliedFilters.surname || appliedFilters.username);

    const allowed = canSeeReadingLessonsResults(userRole);

    const headerOptions = [
        { key: "first-name", name: translate("dashboards.progress-and-mastery.reading-results.first-name") },
        { key: "last-name", name: translate("dashboards.progress-and-mastery.reading-results.last-name") },
        { key: "username", name: translate("dashboards.progress-and-mastery.reading-results.username") },
        { key: "subject", name: translate("dashboards.progress-and-mastery.reading-results.subject") },
        { key: "lesson", name: translate("dashboards.progress-and-mastery.reading-results.lesson") },
        { key: "grade", name: translate("dashboards.progress-and-mastery.reading-results.grade") },
        { key: "exam-type", name: translate("dashboards.progress-and-mastery.reading-results.exam-type") },
        { key: "date", name: translate("dashboards.progress-and-mastery.reading-results.date") },
        { key: "result", name: translate("dashboards.progress-and-mastery.reading-results.result") },
        { key: "action", name: translate("dashboards.progress-and-mastery.reading-results.action") },
    ];

    const handleDownloadPdf = useCallback(async (userQuizResultId, studentId) => {
        setDownloadingId(userQuizResultId);
        try {
            const response = await getUserQuizDetails(userQuizResultId, studentId);
            const pdfLocation = response.data?.PDFLocation;

            if (pdfLocation) {
                window.open(pdfLocation, "_blank");
            }
        } catch (e) {
            console.error("error downloading the PDF:", e);
        } finally {
            setDownloadingId(null);
        }
    }, []);

    const userIdsKey = JSON.stringify(userIds ?? []);

    const fetchPage = useCallback(async (pageToLoad) => {
        setLoading(true);
        try {
            const res = await getReadingLessonsResults(period, dateFrom, dateTo, JSON.parse(userIdsKey), schoolId, regionId, pageToLoad, limiter, appliedFilters);
            setItems(res.data?.Items ?? []);
            setTotalPages(res.data?.TotalPages ?? 0);
        } catch (e) {
            console.error("error fetching reading lessons results:", e);
            setItems([]);
            setTotalPages(0);
        } finally {
            setLoading(false);
        }
    }, [period, dateFrom, dateTo, userIdsKey, schoolId, regionId, appliedFilters]);

    const requestKey = JSON.stringify({ period, dateFrom, dateTo, userIdsKey, schoolId, regionId, appliedFilters });
    const lastRequestKeyRef = useRef(null);

    // Reload from the first page whenever the dashboard filters change
    useEffect(() => {
        if (!allowed) return;

        if (lastRequestKeyRef.current === requestKey) return;

        lastRequestKeyRef.current = requestKey;

        setPage(0);
        fetchPage(0);
    }, [allowed, requestKey, fetchPage]);

    const handlePageChange = (newPage) => {
        setPage(newPage);
        fetchPage(newPage);
    };

    if (!allowed) return null;

    const data = items.map((item) => ({
        GivenName: item.GivenName ?? "",
        Surname: item.Surname ?? "",
        Username: item.Username ?? "",
        Subject: translate(getSubjectLabelKey(item.Subject)),
        LessonTitle: item.LessonTitle ?? "",
        Grade: getGradeLabel(item.LevelCode),
        ExamType: translate(getExamTypeLabelKey(item.ExamType)),
        TakenAt: formatUtcToLocal(item.TakenAt, language),
        TotalPercent: `${item.TotalPercent ?? 0}%`,
        Action: (
            <button
                type="button"
                className="btn-default button-secondary-custom btn-variant-responsive"
                onClick={() => handleDownloadPdf(item.UserQuizResultId, item.UserId)}
                disabled={downloadingId === item.UserQuizResultId}
            >
                {downloadingId === item.UserQuizResultId ? (
                    <span className="spinner-button-small animate-spin h-4 w-4 border-2 border-current border-t-transparent rounded-full" />
                ) : (
                    <FontAwesomeIcon icon={faFilePdf} />
                )}
                {" "}
                <Translate id="dashboards.progress-and-mastery.reading-results.download-pdf" options={{ renderInnerHtml: true }} />
            </button>
        ),
        value: item.UserQuizResultId,
    }));

    return (
        <Card className="shadow-sm border-0 mt-5">
            <CardHeader className="bg-primary text-white font-weight-bold">
                <Translate id="dashboards.progress-and-mastery.reading-results.title" />
            </CardHeader>
            <CardBody style={{ padding: "0px" }}>
                <div className="wrapper-col mb-3 px-3 pt-3">
                    <div className="four-col">
                        <div className="form-group-fields mt-0">
                            <div className="label">
                                <Translate id="dashboards.progress-and-mastery.reading-results.first-name" />
                            </div>
                            <Input
                                type="text"
                                value={filterDraft.givenName}
                                onChange={(e) => updateDraft("givenName", e.target.value)}
                                onKeyDown={(e) => e.key === "Enter" && applyFilters()}
                            />
                        </div>
                    </div>
                    <div className="four-col">
                        <div className="form-group-fields mt-0">
                            <div className="label">
                                <Translate id="dashboards.progress-and-mastery.reading-results.last-name" />
                            </div>
                            <Input
                                type="text"
                                value={filterDraft.surname}
                                onChange={(e) => updateDraft("surname", e.target.value)}
                                onKeyDown={(e) => e.key === "Enter" && applyFilters()}
                            />
                        </div>
                    </div>
                    <div className="four-col">
                        <div className="form-group-fields mt-0">
                            <div className="label">
                                <Translate id="dashboards.progress-and-mastery.reading-results.username" />
                            </div>
                            <Input
                                type="text"
                                value={filterDraft.username}
                                onChange={(e) => updateDraft("username", e.target.value)}
                                onKeyDown={(e) => e.key === "Enter" && applyFilters()}
                            />
                        </div>
                    </div>
                    <div className="four-col mt-3 d-flex align-items-end" style={{ gap: 8 }}>
                        <button type="button" className="btn-w-icon" onClick={applyFilters}>
                            <FontAwesomeIcon icon={faSearch} /> <Translate id="users-datatable.filter" />
                        </button>
                        {hasFilters && (
                            <Button
                                color="danger"
                                onClick={clearFilters}
                                title={translate("dashboards.progress-and-mastery.reading-results.clear-filters")}
                                style={{ height: "38px", padding: "0 12px" }}
                            >
                                <FontAwesomeIcon icon={faTrashAlt} />
                            </Button>
                        )}
                    </div>
                </div>

                {loading ? (
                    <div className="d-flex justify-content-center py-4">
                        <BeatLoader size={8} color="#395a8f" />
                    </div>
                ) : data.length > 0 ? (
                    <DataTable headerOptions={headerOptions} data={data} textAlign="left" />
                ) : (
                    <div className="text-center text-muted py-4">
                        <Translate id="dashboards.progress-and-mastery.reading-results.no-results" />
                    </div>
                )}

                {totalPages > 1 && (
                    <div className="d-flex mb-5">
                        <Paginationn
                            dapper
                            currentPage={page}
                            totalPages={totalPages}
                            updateParent={handlePageChange}
                        />
                    </div>
                )}
            </CardBody>
        </Card>
    );
};

export default withLocalize(ReadingLessonsResultsTable);
