import { Card, CardBody, CardHeader } from "reactstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheckCircle, faTimesCircle } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import { BeatLoader } from "react-spinners";
import React, { useState, useEffect, useCallback } from "react";
import { getModuleUsage } from "../../../../../api/reading-api";
import Paginationn from "../../../datatables/pagination";

const usageBadgeClass = (usage) => (usage > 70 ? "badge-success" : "badge-warning");

// Map a module-usage response into the school rows shown for admin/adminOre.
const mapSchoolUsage = (moduleUsage) => {
    const regions = moduleUsage?.ByRegion ?? [];
    const drilledRegion = regions.find((region) => Array.isArray(region.Schools?.Items));
    const items = (drilledRegion?.Schools?.Items ?? []).map((school) => ({
        id: school.SchoolId,
        name: school.SchoolName,
        usage: school.UsagePercent,
        totalStudents: school.TotalStudents,
        usedStudents: school.UsedStudents,
    }));

    // Fall back to the region summary when no school is drilled into.
    const regionItems = regions.map((region) => ({
        id: region.RegionId,
        name: region.RegionName,
        usage: region.UsagePercent,
        totalStudents: region.TotalStudents,
        usedStudents: region.UsedStudents,
    }));
    return {
        items: items.length > 0 ? items : regionItems,
        totalPages: drilledRegion?.Schools?.TotalPages ?? 0,
    };
};

// Renders a list of rows with a usage percentage badge.
const UsageList = ({ items }) => (
    <ul className="list-group list-group-flush">
        {items.map((item) => (
            <li key={item.id} className="list-group-item">
                <div className="d-flex justify-content-between align-items-center">
                    <strong>{item.name}</strong>
                    <span className={`badge badge-pill ${usageBadgeClass(item.usage)}`}>
                        <Translate id="dashboards.usage-and-engagement.usage" />: {item.usage}%
                    </span>
                </div>
                {item.teacherName && (
                    <small className="d-block text-muted mt-1">{item.teacherName}</small>
                )}
                {item.totalStudents != null && (
                    <small className="d-block text-muted mt-1">
                        <Translate id="dashboards.usage-and-engagement.total-students" />: {item.totalStudents}
                        {" · "}
                        <Translate id="dashboards.usage-and-engagement.used-students" />: {item.usedStudents}
                    </small>
                )}
            </li>
        ))}
        {items.length === 0 && (
            <li className="list-group-item text-center text-muted">
                <Translate id="dashboards.usage-and-engagement.no-results" />
            </li>
        )}
    </ul>
);

// School usage list
const SchoolUsage = ({ moduleUsage, schoolId, regionId }) => {
    const initial = mapSchoolUsage(moduleUsage);
    const [items, setItems] = useState(initial.items);
    const [totalPages, setTotalPages] = useState(initial.totalPages);
    const [page, setPage] = useState(0);
    const [loading, setLoading] = useState(false);
    const limiter = 10

    useEffect(() => {
        const mapped = mapSchoolUsage(moduleUsage);
        setItems(mapped.items);
        setTotalPages(mapped.totalPages);
        setPage(0);
    }, [moduleUsage]);

    const fetchPage = useCallback(async (pageToLoad) => {
        setLoading(true);
        try {
            const res = await getModuleUsage(schoolId, regionId, null, pageToLoad, limiter);
            const mapped = mapSchoolUsage(res.data);
            setItems(mapped.items);
            setTotalPages(mapped.totalPages);
        } catch (e) {
            console.error("error fetching module usage page:", e);
        } finally {
            setLoading(false);
        }
    }, [schoolId, regionId]);

    const handlePageChange = (newPage) => {
        setPage(newPage);
        fetchPage(newPage);
    };

    return (
        <>
            {loading ? (
                <div className="d-flex justify-content-center py-4">
                    <BeatLoader size={8} color="#395a8f" />
                </div>
            ) : (
                <UsageList items={items} />
            )}
            {totalPages > 1 && (
                <div className="d-flex justify-content-center mt-3">
                    <Paginationn
                        dapper
                        currentPage={page}
                        totalPages={totalPages}
                        updateParent={handlePageChange}
                    />
                </div>
            )}
        </>
    );
};

export const UsageGapsChart = ({ userRole, data, moduleUsage, schoolId, regionId }) => {

    if (userRole === "admin" || userRole === "adminOre") {
        return (
            <Card className="shadow-sm border-0 mt-5">
                <CardHeader className="bg-primary text-white font-weight-bold">
                    <Translate id="dashboards.usage-and-engagement.usage-gaps-by-school" />
                </CardHeader>
                <CardBody>
                    <SchoolUsage
                        moduleUsage={moduleUsage}
                        schoolId={schoolId}
                        regionId={regionId}
                    />
                </CardBody>
            </Card>
        );
    }
    else if (userRole === "director") {
        return (
            <Card className="shadow-sm border-0 mt-5">
                <CardHeader className="bg-primary text-white font-weight-bold">
                    <Translate id="dashboards.usage-and-engagement.usage-gaps-by-group" />
                </CardHeader>
                <CardBody>
                    <UsageList items={data.director} />
                </CardBody>
            </Card>
        );
    }
    else if (userRole === "teacher") {
        const usingUsers = data.teacher.filter((u) => u.isUsing);
        const notUsingUsers = data.teacher.filter((u) => !u.isUsing);

        return (
            <div className="row mt-5">
                <div className="col-12 mb-3 px-3">
                    <h5 className="title-secondary font-weight-bold">
                        <Translate id="dashboards.usage-and-engagement.usage-gaps-by-student" />
                    </h5>
                </div>
                <div className="col-lg-6 mb-4">
                    <Card className="shadow-sm border-0 h-100">
                        <CardHeader className="bg-success text-white font-weight-bold">
                            <Translate id="dashboards.usage-and-engagement.using-the-module" /> ({usingUsers.length})
                        </CardHeader>
                        <CardBody className="p-0">
                            <ul className="list-group list-group-flush">
                                {usingUsers.map((user) => (
                                    <li key={user.id} className="list-group-item d-flex justify-content-between align-items-center">
                                        {user.name}
                                        <FontAwesomeIcon icon={faCheckCircle} className="text-success" />
                                    </li>
                                ))}
                                {usingUsers.length === 0 &&
                                    <li className="list-group-item text-center text-muted">
                                        <Translate id="dashboards.usage-and-engagement.no-active-students" />
                                    </li>}
                            </ul>
                        </CardBody>
                    </Card>
                </div>
                <div className="col-lg-6 mb-4">
                    <Card className="shadow-sm border-0 h-100">
                        <CardHeader className="bg-danger text-white font-weight-bold">
                            <Translate id="dashboards.usage-and-engagement.no-use" /> ({notUsingUsers.length})
                        </CardHeader>
                        <CardBody className="p-0">
                            <ul className="list-group list-group-flush">
                                {notUsingUsers.map((user) => (
                                    <li key={user.id} className="list-group-item d-flex justify-content-between align-items-center">
                                        {user.name}
                                        <FontAwesomeIcon icon={faTimesCircle} className="text-danger" />
                                    </li>
                                ))}
                                {notUsingUsers.length === 0 &&
                                    <li className="list-group-item text-center text-muted">
                                        <Translate id="dashboards.usage-and-engagement.all-students-active" />
                                    </li>
                                }
                            </ul>
                        </CardBody>
                    </Card>
                </div>
            </div>
        );
    }

    return null;
};