import React, {useState, useEffect} from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button } from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import { getTeacherPlannings } from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import Alert from 'react-bootstrap/Alert';
import {
    faTrash,
    faEdit, faUserPlus, faBook, faInfoCircle
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ModalPlanningArchive from "../modal/modal-planning-archive";
import ModalPlanningSendStudent from "../modal/modal-planning-send-student";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import moment from "moment";
import "moment-timezone";
import 'moment/locale/es'

/**
 * Data table to handle the teacher planning
 */
const PlanningDataTable = ({ redoSearch, updateParent }) => {
    const [state, setState] = useState({
        currentPage: 1,
        data: [],
        headerOptions: [
            { key: "0", name: <Translate id="planning.title" options={{ renderInnerHtml: true }} /> },
            { key: "1", name: <Translate id="planning.close-plan" options={{ renderInnerHtml: true }} /> },
            { key: "2", name: <Translate id="planning.subject-grade" options={{ renderInnerHtml: true }} /> },
            { key: "3", name: <Translate id="planning.view" options={{ renderInnerHtml: true }} /> },
            { key: "4", name: <Translate id="planning.send-planning-to-students" options={{ renderInnerHtml: true }} /> },
            { key: "5", name: "" },
        ],
        limiter: 100,
        loading: false,
        showModalArchive: false,
        showModalPlanningSendToStudents: false,
        totalPages: 1,
        planningId: null,
    });

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    const handlePageClick = (e) => {
        setState((prev) => ({ ...prev, currentPage: e }));
        doSearch();
    };

    const handleViewClick = (newValue) => {
        updateParent(newValue.currentTarget.value, newValue.currentTarget.name);
    };

    const handleSendClick = (newValue) => {
        setState((prev) => ({
            ...prev,
            showModalPlanningSendToStudents: true,
            planningId: newValue.currentTarget.value
        }));
    };

    const handleArchiveClick = (newValue) => {
        setState((prev) => ({
            ...prev,
            showModalArchive: true,
            planningId: newValue.currentTarget.value
        }));
    };

    const toggleModalArchive = () => {
        doSearch();
        setState((prev) => ({ ...prev, showModalArchive: false }));
    };

    const toggleModalPlanningStudent = () => {
        setState((prev) => ({ ...prev, showModalPlanningSendToStudents: false }));
    };

    const convertDataToDataTable = (dataToConvert) => {
        let newArray = [];
        if (dataToConvert && dataToConvert.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
                if (dataToConvert[i].PlanAvailable === "0") continue;

                const { DaysToClosePeriod, PeriodTo } = dataToConvert[i].AcademicPeriodFullDetail;
                const openDate = dataToConvert[i].OpenDate;
                const isDisabled = openDate !== null && openDate !== "";

                newArray.push({
                    Name: (
                        <>
                            <span>{dataToConvert[i].PlanName}</span>
                            {isDisabled && (
                                <Alert variant="primary" className="alert-small-padding">
                                    <small>
                                        <FontAwesomeIcon icon={faInfoCircle} />{' '}
                                        <Translate id="planning.planning-not-compatible-with-instructional-plan" />
                                    </small>
                                </Alert>
                            )}
                        </>
                    ),
                    DateClosePlan: (
                        <span className="date-capitalize">
              <b>{moment(PeriodTo, "MM/DD/YYYY HH:mm:ss").add(DaysToClosePeriod, "days").format("MMM DD [,] YYYY")}</b>
            </span>
                    ),
                    SubjectName: `${dataToConvert[i].SubjectCode} ${dataToConvert[i].LevelCode}`,
                    View: (
                        <Button
                            className="button-secondary-custom"
                            color="none"
                            variant="none"
                            onClick={handleViewClick}
                            value={dataToConvert[i].PlanId}
                            name={dataToConvert[i].PlanName}
                            disabled={isDisabled}
                        >
                            <FontAwesomeIcon icon={faEdit} />{" "}
                            <Translate id="planning.view" options={{ renderInnerHtml: true }} />
                        </Button>
                    ),
                    Send: (
                        <Button
                            className="btn btn-success"
                            onClick={handleSendClick}
                            value={dataToConvert[i].PlanId}
                            disabled={isDisabled}
                        >
                            <FontAwesomeIcon icon={faUserPlus} />{" "}
                            <Translate id="planning.attach" options={{ renderInnerHtml: true }} />
                        </Button>
                    ),
                    Available: (
                        <Button
                            className="btn btn-danger"
                            onClick={handleArchiveClick}
                            value={dataToConvert[i].PlanId}
                            disabled={isDisabled}
                        >
                            <FontAwesomeIcon icon={faTrash} />{" "}
                            <Translate id="planning.archive" options={{ renderInnerHtml: true }} />
                        </Button>
                    ),
                    value: dataToConvert[i].PlanId,
                });
            }
        }
        setState((prev) => ({ ...prev, data: newArray }));
    };

    const doSearch = () => {
        setState((prev) => ({ ...prev, loading: true }));
        getTeacherPlannings().then((response) => {
            setState((prev) => ({ ...prev, loading: false, totalPages: 1 }));
            convertDataToDataTable(response.data);
        });
    };

    return (
        <div>
            {state.data.length === 0 ? (
                <div body className="animated delay-05s fadeIn">
                    <div className="wrapper-not-found-planning-details">
                        <p style={{ textAlign: 'center' }}>
                            <FontAwesomeIcon icon={faBook} size='3x' />{' '}
                        </p>
                        <Translate id="planning.no-plannings-found" options={{ renderInnerHtml: true }} />
                    </div>
                </div>
            ) : (
                <>
                    <DataTable headerOptions={state.headerOptions} data={state.data} />
                    {state.loading && <LoaderFullWindow />}
                    {state.showModalArchive && (
                        <ModalPlanningArchive
                            planningId={state.planningId}
                            visible={state.showModalArchive}
                            toggle={toggleModalArchive}
                            updateParent={toggleModalArchive}
                        />
                    )}
                    {state.showModalPlanningSendToStudents && (
                        <ModalPlanningSendStudent
                            planningId={state.planningId}
                            visible={state.showModalPlanningSendToStudents}
                            updateParent={toggleModalPlanningStudent}
                        />
                    )}
                </>
            )}
        </div>
    );
};

export default PlanningDataTable;