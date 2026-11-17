/**
 * Created by portiz on 12/17/20.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Col, Row} from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {deleteGroupFromPlan, getTeacherPlanStudents} from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import {
    faCheck,
    faTrash,
    faEdit, faPlus, faShare, faComments
} from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import ModalForumArchive from "../modal/modal-forum-archive";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import SessionManager from "../../../custom-classes/session-manager";
import {Redirect} from "react-router-dom";
import ModalForumCreate from "../modal/modal-forum-create";
import ModalPlanningSendStudent from "../modal/modal-planning-send-student";
import {faUserGraduate} from "@fortawesome/free-solid-svg-icons/faUserGraduate";

/**
 * Data table to handle the forums
 */
const StudentsPlanningDataTable = ({ disabled = false, teacherPlanId: initialTeacherPlanId = null,redoSearch}) => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showModalPlanningSendToStudents, setShowModalPlanningSendToStudents] = useState(false);
    const [teacherPlanId] = useState(initialTeacherPlanId);

    const headerOptions = [
        {key: "0", name: LanguageManager.getTranslation("planning.name", Settings.LANGUAGES)},
        {key: "1", name: ""},
        {key: "2", name: ""},
        {key: "3", name: ""}
    ];

    useEffect(() => {
        doSearch();
    }, [showModalPlanningSendToStudents]);

    useEffect(() => {
        if (redoSearch) {
            doSearch();
        }
    }, [redoSearch]);

    // Call the results to display
    const doSearch = async () => {
        setLoading(true);
        try {
            const res = await getTeacherPlanStudents(teacherPlanId);
            setData([]);
            convertDataToDataTable(res.data);
        } catch (error) {
            console.error('Error fetching teacher plan students:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleArchiveClick = () => async (newValue) => {
        try {
            await deleteGroupFromPlan(newValue.currentTarget.value, teacherPlanId);
            setData([]);
            doSearch();
        } catch (error) {
            console.error('Error deleting group from plan:', error);
        }
    };

    const handleSendClick = () => {
        setShowModalPlanningSendToStudents(true);
    };

    const toggleModalPlanningStudent = () => {
        setShowModalPlanningSendToStudents(false);
    };

    // Convert the results to a data table
    const convertDataToDataTable = (dataToConvert) => {
        let newArray = [];
        if (dataToConvert?.length > 0) {
            for (let i = 0; i < dataToConvert.length; i++) {
                newArray.push({
                    Name: `${dataToConvert[i].GroupName}`,
                    SubjectCode: dataToConvert[i].SubjectCode,
                    LevelCode: dataToConvert[i].LevelCode,
                    Remove: (
                        <Button 
                            className="btn btn-danger" 
                            onClick={handleArchiveClick()} 
                            value={dataToConvert[i].GroupId} 
                            disabled={disabled}
                        >
                            <FontAwesomeIcon icon={faTrash} /> {' '}
                            <Translate id="planning.remove" options={{renderInnerHtml: true}}/>
                        </Button>
                    ),
                    value: dataToConvert[i].GroupId
                });
            }
        }
        setData(newArray);
    };

    const renderEmptyState = () => (
        <div body className="animated delay-05s fadeIn">
            <div className="wrapper-not-found-planning-details">
                <p style={{textAlign: 'center'}}>
                    <FontAwesomeIcon icon={faUserGraduate} size='3x'/>{' '}
                </p>
                <Translate id="planning.planning-no-groups" options={{renderInnerHtml: true}}/>
            </div>
        </div>
    );

    // Render the datatable and a generic modal. The body text should be sent in the text
    return (
        <div className="animated delay-05s fadeIn danger-btn-simple space-btn-icon secondary-btn-simple table-no-margin-top td-responsive special-top-space delete-before-4 delete-before-2 td-block4col td-inline-2btn">
            <Row>
                <Col className="text-right mt-3 mb-3">
                    <Button 
                        className="btn primary-btn" 
                        onClick={handleSendClick} 
                        value={teacherPlanId} 
                        disabled={disabled}
                    >
                        <FontAwesomeIcon icon={faPlus} /> {' '}
                        <Translate id="planning.send-to-students" options={{renderInnerHtml: true}}/>
                    </Button>
                </Col>
            </Row>
            <Row>
                <Col xs={12}>
                    {!data?.length ? renderEmptyState() : (
                        <DataTable headerOptions={headerOptions} data={data}/>
                    )}
                </Col>
            </Row>
            {loading && <LoaderFullWindow/>}
            {showModalPlanningSendToStudents && (
                <ModalPlanningSendStudent
                    planningId={teacherPlanId}
                    visible={showModalPlanningSendToStudents}
                    updateParent={toggleModalPlanningStudent}
                />
            )}
        </div>
    );
};

export default StudentsPlanningDataTable;