/**
 * Created by portiz on 12/23/2020.
 */
import React, { useState, useEffect } from 'react';
import {CardBody, CardTitle, Col, Container, Row, Button, Card} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import TeachersDataTable from "../datatables/teachers-data-table";
import SelectorTeacherPlanning from "../../teacher-module/selector/selector-teacher-planning";
import './style.css';
import Settings from "../../../custom-classes/settings";
import PlanningCalendar from "../../teacher-module/calendar/planning-calendar";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowAltCircleLeft, faArrowLeft} from "@fortawesome/free-solid-svg-icons";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import SessionManager from '../../../custom-classes/session-manager';
import {SendAccessLog} from '../../../utils/access_logger';

/**
 * The component (window) to see the Teacher Calendar
 */
const TeacherCalendar = ({initialize, addTranslation}) => {
    const sessionDetails = SessionManager.getSessionDetails();
    const [selectedUserDetails, setSelectedUserDetails] = useState(null);
    const [selectedPlanning, setSelectedPlanning] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.remove('landing-white');
        try{            
            SendAccessLog({
                Username: sessionDetails.Username,
                InvitationCode: sessionDetails.InvitationCode,
                Role: sessionDetails.Roles[0],
                AreaId: "20"
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    const resetSelectedUser = () => {
        setSelectedUserDetails(null);
        setSelectedPlanning(null);
    };

    const setSelectedPlanningHandler = (e) => {
        setSelectedPlanning(null);
        setSelectedPlanning(e.value);
    };

    const handleSelectedUser = (e) => {
        setSelectedUserDetails(e);
        window.scrollTo(0, 0);
    };

    // Renders the main dashboard
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    {selectedUserDetails !== null ?
                    <>
                        <div className="col col-no-padding-left">
                                <span className="btn-back" onClick={resetSelectedUser}>
                                    <FontAwesomeIcon icon={faArrowLeft} />{' '}
                                    <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                                </span>
                        </div> </> : null
                    }
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                {selectedUserDetails === null ? <Translate id="calendar.select-teacher" options={{renderInnerHtml: true}}/>
                                : null}
                                </div>
                        </div>
                    </div>
                    {selectedUserDetails == null ?
                        <>
                        {/* <Row>
                            <Col xs={12}>
                                <b>
                                    <Translate id="calendar.select-teacher" options={{renderInnerHtml: true}}/>
                                </b>
                            </Col>
                        </Row> */}
                        <Row>
                            <Col xs={12}>
                                <div body className=" animated delay-05s fadeIn">
                                    <CardTitle>
                                    </CardTitle>
                                    <CardBody>
                                        <TeachersDataTable roleId={Settings.ROLE_TEACHER_ID} hideRoleSelected={true} redoSearch={undefined} updateParent={handleSelectedUser}/>
                                    </CardBody>
                                </div>
                            </Col>
                        </Row>
                        </> :
                        <div className="clear-style-card td-responsive module-school-step admin-school-table clear-pagination row" style={{marginTop: 10}}>
                        <Row>
                            <Col xs={12}>
                                <Translate id="calendar.selected-teacher" options={{renderInnerHtml: true}}/>
                            </Col>
                            <Col xs={12}>
                                    <b>{selectedUserDetails.GivenName + " " + selectedUserDetails.Surname}</b>
                                {' '}
                            </Col>
                        </Row>
                            <Row>
                                <Col xs={12}>
                                    <Translate id="calendar.select-teacher-plan" options={{renderInnerHtml: true}}/>
                                </Col>
                                <Col xs={12}>
                                    <SelectorTeacherPlanning teacherUserId={selectedUserDetails.UserId } updateParent={setSelectedPlanningHandler}/>
                                </Col>
                            </Row>
                        </div>
                    }
                    {
                        selectedPlanning !== null ?
                        <>
                            <hr/>
                            <PlanningCalendar teacherPlanId={selectedPlanning}/>
                        </> : null
                    }
                </div>
            </div>

        </div>
    )
};

export default withLocalize(TeacherCalendar);