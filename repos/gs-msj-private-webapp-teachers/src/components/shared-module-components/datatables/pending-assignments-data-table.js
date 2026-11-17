/**
 * Created by gperez on 12/11/2023.
 */
import React, { useState, useEffect } from 'react';
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";
import { Button } from "reactstrap";
import LoaderFullWindow from "../loader-windows/loader-full-window";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { getTeacherPendingAssignmentsForAllPlans } from "../../../api/teachers";
import PaginationCustom from "./pagination";
import DataTable from "./datatable";
import "react-dates/initialize";
import "react-dates/lib/css/_datepicker.css";
import {faCheckCircle, faUserTimes} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import "./style-datatables.css";
import { getTimeZone } from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";
import ModalCustomQuizCorrection from "../../teacher-module/modal/modal-custom-quiz-correction";
import SessionManager from '../../../custom-classes/session-manager';
import { SendAccessLog } from '../../../utils/access_logger';

const PendingAssignmentsDataTable = ({ roleId }) => {
    const userDetails = SessionManager.getSessionDetails();

    const [roleIdState, setRoleIdState] = useState(roleId);
    const [currentPage, setCurrentPage] = useState(0);
    const [totalPages, setTotalPages] = useState(1);
    const [limiter] = useState(10);
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState([]);
    const [showModalQuizCorrection, setShowModalQuizCorrection] = useState(false);
    const [studentQuizId, setStudentQuizId] = useState(null);
    const [planQuizIdToCorrect, setPlanQuizIdToCorrect] = useState(null);
    const [studentId, setStudentId] = useState(null);
    const [headerOptions] = useState([
      {
          key: "0",
          name: LanguageManager.getTranslation(
              "pending-teacher-assignments.username",
              Settings.LANGUAGES
          ),
      },
      {
          key: "1",
          name: LanguageManager.getTranslation(
              "pending-teacher-assignments.first-name",
              Settings.LANGUAGES
          ),
      },
      {
          key: "2",
          name: LanguageManager.getTranslation(
              "pending-teacher-assignments.last-name",
              Settings.LANGUAGES
          ),
      },
      {
          key: "3",
          name: LanguageManager.getTranslation(
              "pending-teacher-assignments.assignment",
              Settings.LANGUAGES
          ),
      },
      {
          key: "4",
          name: LanguageManager.getTranslation(
              "pending-teacher-assignments.date-submitted",
              Settings.LANGUAGES
          ),
      },
      { key: "5", name: "" },
    ]);

    useEffect(() => {
        doSearch();
    }, [currentPage]);

    const handlePageClick = (e) => {
        setCurrentPage(e);
    };

    const handleRowClick = (e) => {};
    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getTeacherPendingAssignmentsForAllPlans(
            currentPage,
            limiter
        );

        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(response.data.TotalPages);
            setData([]);
            
            if (response.data.PendingAssignments.length > 0) {
                convertDataToDataTable(response.data.PendingAssignments);
            }
        });
    };

    const convertDataToDataTable = (dataToConvert) => {
        setData([]);
        
        let newArray = [];
        for (let i = 0; i < dataToConvert.length; i++) {
            newArray.push({
                Username: dataToConvert[i].Student.Username,
                GivenName: dataToConvert[i].Student.GivenName,
                Surname: dataToConvert[i].Student.Surname,
                AssignmentName: dataToConvert[i].AssignmentName,
                DateSubmitted: dataToConvert[i].DateSubmitted
                    ? moment
                        .utc(dataToConvert[i].DateSubmitted, "MM/DD/YYYY HH:mm:ss")
                        .tz(getTimeZone())
                        .format("MM/DD/YYYY hh:mm a")
                    : "",
                Select: (
                    <button
                        type="button"
                        value={dataToConvert[i].Student.UserId}
                        className="btn-default button-secondary-custom"
                        onClick={() =>
                            handleViewClick(
                                dataToConvert[i].PlanId,
                                dataToConvert[i].PlanName,
                                dataToConvert[i].AssignmentId,
                                dataToConvert[i].AssignmentName,
                                dataToConvert[i].Student.UserId
                            )
                        }
                    >
                        {" "}
                        <Translate id="pending-teacher-assignments.correct" />{" "}
                    </button>
                ),
            });
        }
        setData(newArray);
    };

    const handleCloseModal = () => {
        setShowModalQuizCorrection(false);
        doSearch();
    };

    const handleViewClick = (planId, planName, quizId, quizName, studentId) => {
        setPlanQuizIdToCorrect(planId);
        setStudentQuizId(quizId);
        setStudentId(studentId);
        setShowModalQuizCorrection(true);
        
        try{            
            SendAccessLog({
                Username: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "6",
                PlanId: planId,
                PlanName: planName,
                AssignmentId: quizId,
                AssignmentTitle: quizName,
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    };

    return (
        <div>
            {data.length > 0 ? (
                <DataTable
                    headerOptions={headerOptions}
                    data={data}
                    updateParent={handleRowClick}
                />
            ) : (
                <div className="module-notfound">
                    <FontAwesomeIcon icon={faCheckCircle} />{" "}
                    <Translate id="notification-content.no-pending-assignments-to-be-corrected" />
                </div>
            )}
            <PaginationCustom
                totalPages={totalPages}
                currentPage={currentPage}
                updateParent={handlePageClick}
                dapper={true}
            />
            {loading ? <LoaderFullWindow /> : null}
            {showModalQuizCorrection ? (
                <ModalCustomQuizCorrection
                    visible={showModalQuizCorrection}
                    planQuizId={planQuizIdToCorrect}
                    studentId={studentId}
                    studentQuizId={studentQuizId}
                    updateParent={handleCloseModal}
                />
            ) : null}
        </div>
    );
};

export default PendingAssignmentsDataTable;
