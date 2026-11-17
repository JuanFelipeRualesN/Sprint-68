import React, {useMemo} from "react";
import {CardBody, Table} from "reactstrap";
import { Translate } from "react-localize-redux";

export const AverageLessonResults = ({ data: fullData, isDirector = false, selectedLanguages, selectedFocusAreas }) => {
    const filteredData = useMemo(() => {
        let results = fullData;

        if (selectedLanguages && selectedLanguages.length > 0) {
            results = results.filter(lesson => selectedLanguages.includes(lesson.subject));
        }

        if (selectedFocusAreas && selectedFocusAreas.length > 0) {
            results = results.filter(lesson => selectedFocusAreas.includes(lesson.focusArea));
        }

        return results;

    }, [fullData, selectedLanguages, selectedFocusAreas]);

    return (
        <CardBody className="p-3 d-flex flex-column">
            <div className="mb-3 title-secondary font-weight-bold">
                <Translate id="dashboards.average-results-per-lesson" />
            </div>
            <div className="table-responsive flex-grow-1">
                <Table borderless size="sm" className="mb-0">
                    <thead className="bg-light">
                    <tr>

                        <th style={{ width: '60%' }}>
                            <Translate id="dashboards.lesson-title" />
                        </th>
                        <th style={{ width: '20%' }}>
                            <Translate id="dashboards.code" />
                        </th>
                        {isDirector ? <th style={{width: '20%'}}>
                            <Translate id="dashboards.group" />
                        </th> : null}
                        <th style={{width: '20%', textAlign: 'right' }}>
                            <Translate id="dashboards.average-score" />
                        </th>
                    </tr>
                    </thead>
                    <tbody>
                    {filteredData.map((lesson, index) => (
                        <tr key={index} className={index % 2 === 0 ? 'bg-white' : 'bg-light'}>
                            <td>{lesson.title}</td>
                            <td>{lesson.code}</td>
                            {isDirector ? <td>{lesson.group}</td> : null}
                            <td className="font-weight-bold" style={{textAlign: 'right'}}>
                                    <span
                                        className={lesson.average >= 90 ? 'text-success' : lesson.average >= 70 ? 'text-warning' : 'text-danger'}>
                                        {lesson.average}%
                                    </span>
                            </td>

                        </tr>
                    ))}
                    </tbody>
                </Table>
            </div>
            {filteredData.length === 0 && (
                <p className="text-center text-muted mt-3">
                    <Translate id="dashboards.usage-and-engagement.no-lesson-data-available" />
                </p>
            )}
        </CardBody>
    );
};