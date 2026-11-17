import {Card, CardBody, CardHeader} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCalendarCheck, faClock, faUsers} from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import React from "react";

export const DynamicUsageMetricsCard = ({ sessionTotals, selectedEntity, userRole }) => {
    const defaultLabel = userRole === 'teacher' ? <Translate id="dashboards.all-students" /> : <Translate id="dashboards.all-schools" />;
    const entityLabel = selectedEntity || defaultLabel;

    const totalSessions  = sessionTotals?.TotalSessions;
    const totalActiveDays = sessionTotals?.TotalActiveDays;
    const avgMinutes = sessionTotals?.AvgTimeSeconds && Math.round(sessionTotals.AvgTimeSeconds / 60);

    return (
        <Card className="shadow-sm border-0 mb-4">
            <CardHeader className="bg-secondary text-white font-weight-bold">
                <Translate id="dashboards.usage-and-engagement.key-usage-metrics" />: {entityLabel}
            </CardHeader>
            <CardBody>
                <div className="row text-center">
                    <div className="col-md-4 border-right">
                        <FontAwesomeIcon icon={faUsers} size="2x" className="title-secondary mb-2 usage-metric-icon" />
                        <h4 className="font-weight-bold usage-metric-text title-primary">{totalSessions}</h4>
                        <p className="text-muted mb-0 usage-metric-text">
                            <Translate id="dashboards.usage-and-engagement.total-sessions" />
                        </p>
                    </div>
                    <div className="col-md-4 border-right">
                        <FontAwesomeIcon icon={faCalendarCheck} size="2x" className="title-secondary mb-2 usage-metric-icon" />
                        <h4 className="font-weight-bold usage-metric-text title-primary">{totalActiveDays}</h4>
                        <p className="text-muted mb-0 usage-metric-text">
                            <Translate id="dashboards.usage-and-engagement.active-days" />
                        </p>
                    </div>
                    <div className="col-md-4">
                        <FontAwesomeIcon icon={faClock} size="2x" className="title-secondary mb-2 usage-metric-icon" />
                        <h4 className="font-weight-bold usage-metric-text title-primary">{avgMinutes} min</h4>
                        <p className="text-muted mb-0 usage-metric-text">
                            <Translate id="dashboards.usage-and-engagement.average-usage-time" />
                        </p>
                    </div>
                </div>
            </CardBody>
        </Card>
    );
};