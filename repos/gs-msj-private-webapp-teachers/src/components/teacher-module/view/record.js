import React, { useState, useEffect } from "react";
import {
  Col,
  Card,
  CardBody,
  Row,
  Nav,
  NavItem,
  NavLink,
  TabContent,
  TabPane,
} from "reactstrap";
import "./style.css";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import Settings from "../../../custom-classes/settings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faBookOpen,
  faTasks,
  faStickyNote,
  faArrowLeft,
} from "@fortawesome/free-solid-svg-icons";
import ProfilesDataTable from "../../shared-module-components/datatables/profiles-data-table";
import AnnotationsDataTable from "../datatable/annotations-data-table";
import SelectorStudentsPlanning from "../selector/selector-students-planning";
import PlanningLessonStudentResultsDataTable from "../datatable/planning-lesson-student-results-data-table";
import PlanningHomeworkStudentDetailsDataTable from "../datatable/planning-homework-student-details-data-table";
import SessionManager from "../../../custom-classes/session-manager";
import "./style.css";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";
/**
 * Class/component of the student average
 */
const Record = ({ history }) => {
  const [activeTab, setActiveTab] = useState("1");
  const [studentId, setStudentId] = useState(null);
  const [studentName, setStudentName] = useState(null);
  const [planningId, setPlanningId] = useState(null);

  const userDetails = SessionManager.getSessionDetails();
  const isTeacher = userDetails.Roles.includes(Settings.ROLE_TEACHER_STR);
  const isStaff = false;
  const roleId = Settings.ROLE_STUDENT_ID;

  // Initialize translations based on the current version
  useVersionedTranslations();
  useEffect(() => {
    try {
      SendAccessLog({
          UserName: userDetails.Username,
          InvitationCode: userDetails.InvitationCode,
          Role: userDetails.Roles[0],
          AreaId: "5",
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  }, []);

  const showStudentBox = () => {
    setStudentId(null);
    setPlanningId(null);
  }

  const handleSelectedPlanning = (e) => {
    setPlanningId(null);
    setTimeout(() => setPlanningId(e.value), 0);

    try{      
      SendAccessLog({
        UserName: userDetails.Username,
        InvitationCode: userDetails.InvitationCode,
        Role: userDetails.Roles[0],
        AreaId: "5",
        Details: "Progress - Planning Selector by student",
        PlanId: e.value,
        PlanName: e.label,
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  };

  const handleSetStudentDetails = (newValue) => {
    setStudentId(null);
    setTimeout(() => {
      setStudentId(newValue.UserId);
      setStudentName(`${newValue.GivenName} ${newValue.Surname}`);
      window.scrollTo(0, 0);
    }, 0);
  };

  const toggle = (tabNo) => setActiveTab(tabNo);

  return (
      <div>
        <div className="container-padding animated fadeIn">
          <div className="wrapper-principal-container">
            <div className="wrapper-col mb-3">
              <div className="col col-no-padding-left">
                {studentId ? (
                    <>
                      <a className="btn-back" onClick={showStudentBox}>
                        <FontAwesomeIcon icon={faArrowLeft} />{" "}
                        <Translate id="general.back-action" />
                      </a>
                      <div className="col col-no-padding-left">
                    <span className="title-principal">
                      <Translate id="progress.record-student" />:
                    </span> {studentName}
                      </div>
                    </>
                ) : (
                    <div className="col col-no-padding-left">
                  <span className="title-principal">
                    <Translate id="progress.record-student" />
                  </span>
                    </div>
                )}
              </div>
            </div>

            <Row style={{ minHeight: 600 }}>
              <Col sm="12" md="12" lg="12">
                <CardBody className="clear-padding-card">
                  <Row className="td-w-rollover td-responsive special-top-space syle-date-five">
                    {!studentId && (
                        <Col sm="12">
                          <p>
                            <b>
                              <Translate id="progress.select-student" options={{ renderInnerHtml: true }} />
                            </b>
                          </p>
                          <ProfilesDataTable
                              value={studentId}
                              isStaff={isStaff}
                              roleId={roleId}
                              updateParent={handleSetStudentDetails}
                          />
                        </Col>
                    )}
                  </Row>

                  <Row>
                    {studentId && (
                        <Col xs="12" md="6">
                          <SelectorStudentsPlanning
                              studentId={studentId}
                              filterByTeacher={isTeacher}
                              updateParent={handleSelectedPlanning}
                          />
                        </Col>
                    )}
                  </Row>

                  <Row>
                    <Col xs="12">
                      {studentId && planningId && (
                          <div className="margin-top-mandatory-25 tabs-strech clear-style-card danger-btn-simple space-btn-icon secondary-btn-simple table-no-margin-top clear-min-height td-responsive special-top-space">
                            <Nav tabs>
                              <NavItem>
                                <nav
                                    className={activeTab === "1" ? "tab underline-div nav-link" : "tab nav-link"}
                                    onClick={() => toggle("1")}
                                >
                                  <FontAwesomeIcon icon={faBookOpen} />{" "}
                                  <Translate id="progress.lessons" />
                                </nav>
                              </NavItem>
                              {isTeacher && (
                                  <NavItem>
                                    <nav
                                        className={activeTab === "2" ? "tab underline-div nav-link" : "tab nav-link"}
                                        onClick={() => toggle("2")}
                                    >
                                      <FontAwesomeIcon icon={faStickyNote} />{" "}
                                      <Translate id="progress.annotations" />
                                    </nav>
                                  </NavItem>
                              )}
                              <NavItem>
                                <nav
                                    className={activeTab === "3" ? "tab underline-div nav-link" : "tab nav-link"}
                                    onClick={() => toggle("3")}
                                >
                                  <FontAwesomeIcon icon={faTasks} />{" "}
                                  <Translate id="progress.assignments" />
                                </nav>
                              </NavItem>
                            </Nav>

                            <TabContent activeTab={activeTab}>
                              <TabPane tabId="1">
                                <Card className="special-average syle-date-four">
                                  <CardBody>
                                    <PlanningLessonStudentResultsDataTable
                                        studentName={studentName}
                                        studentId={studentId}
                                        planningId={planningId}
                                    />
                                  </CardBody>
                                </Card>
                              </TabPane>
                              <TabPane tabId="2">
                                <Card className="special-average">
                                  <CardBody className="only--table-last-td-left syle-date-one">
                                    <AnnotationsDataTable
                                        studentId={studentId}
                                        planningId={planningId}
                                    />
                                  </CardBody>
                                </Card>
                              </TabPane>
                              <TabPane tabId="3">
                                <Card className="special-average syle-date-four syle-date-five">
                                  <CardBody>
                                    <PlanningHomeworkStudentDetailsDataTable
                                        isViewByTeacher={isTeacher}
                                        studentId={studentId}
                                        planningId={planningId}
                                    />
                                  </CardBody>
                                </Card>
                              </TabPane>
                            </TabContent>
                          </div>
                      )}
                    </Col>
                  </Row>
                </CardBody>
              </Col>
            </Row>
          </div>
        </div>
      </div>
  );
};

export default withLocalize(Record);
