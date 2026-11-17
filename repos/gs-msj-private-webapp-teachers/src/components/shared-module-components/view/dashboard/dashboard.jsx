import React, { useState, useEffect, useContext } from "react";
import { Card, CardBody, Col, Row } from "reactstrap";
import { Link, Redirect } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { useTranslation, Trans } from 'react-i18next';
import TableSubscriptions from "../../../homeschooler-module/table/table-subscriptions";
import TableActivity from "../../../homeschooler-module/table/table-activity";
import SessionManager from "../../../../custom-classes/session-manager";
import Settings from "../../../../custom-classes/settings";
import QuickGrid from "./quick-grid";
import Ic from "../../../../assets/svg/icons";
import { AppContext } from "../../../../App.Context";

import {callActivityReportCreate, callStudentsCounting} from "../../../../api/users";
import { getTotalOfUnreadMessages } from "../../../../api/messages";
import { insertMiniGamesLockCreate, getMiniGamesLock } from "../../../../api/gsweb";
import { getTotalPendingAssignmentsForAllPlans } from "../../../../api/teachers";

import { getAsset } from "../../../../utils/getAssets";
const iconPending = getAsset("Home-icon-DashboardPendingAssignments");
const iconGroup = getAsset("Home-icon-DashboardGroups");
const iconAddPlan = getAsset("Home-icon-DashboardNewPlan");
const iconConfigurations = getAsset("Home-icon-DashboardSettings");
const iconSyllabus = getAsset("Home-icon-DashboardSyllabus");
const iconReports = getAsset("Home-icon-DashboardReports");
const iconPlanning = getAsset("Home-icon-DashboardPlans");
const iconSchools = getAsset("Home-icon-DashboardSchools");

import "../style.css";
import "../../../../styles/design-tokens.css";
import "../../../../styles/ai-module-and-dashboard.css";
import "./style-dashboard.css";
import Welcome from "./welcome-banner";
import {getTeacherGroupsTotal} from "../../../../api/teachers";
import { fetchUnreadAlertsTotal } from "../../profiles-prompt-alerts";
import AITools from "../../../teacher-module/ai-module/components/tools/ai-tools";
import CalendarEventsWeekly from "./calendar-events-weekly";
import LanguageSelector from "../../selector/language-selector";

const { IS_APP_MANAGED_BY_REQUIRED } = Settings;

const ADMIN_ROLES = [
  Settings.ROLE_EMPLOYEE_STR,
  Settings.ROLE_ADMIN_STR,
  Settings.ROLE_ADMIN_ORE_STR,
];

/* Soft pastel tints — pale background + darker icon color. Title stays in ink. */
const TINTS = {
  teal:       { bg: "#D6EFEE", fg: "#2F8E8E" },
  coral:      { bg: "#FCE5DA", fg: "#C75A38" },
  turquoise:  { bg: "#D6F0EC", fg: "#1F8478" },
  amber:      { bg: "#FCEACF", fg: "#B27516" },
  green:      { bg: "#DCEFE0", fg: "#3A8E5C" },
  terracotta: { bg: "#F6DCD9", fg: "#A0413D" },
  indigo:     { bg: "#E3E7FB", fg: "#4A5BC4" },
  pink:       { bg: "#F9DDEC", fg: "#B0447E" },
  red:        { bg: "#F6DAD8", fg: "#B03A34" },
  blue:       { bg: "#DBE8F9", fg: "#3468A8" },
  violet:     { bg: "#E5E1F7", fg: "#5F4DA8" },
  sand:       { bg: "#EFE7D3", fg: "#8A7430" },
  gray:       { bg: "#E9E9E9", fg: "#5B5B5B" },
};

/**
 * The component (window) to handle the main dashboard of the users
 */
const Dashboard = () => {
  const { t } = useTranslation(["navigation", "subscription", "common"]);
  const context = useContext(AppContext);
  const variables = context.state.variables;
  const sessionDetails = SessionManager.getSessionDetails();

  const [checked, setChecked] = useState(false); // Whether mini games are unlocked
  const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());
  const [modalAlertNews, setModalAlertNews] = useState(true);
  const [propsModal] = useState({ bodyText: t("dashboard.alert-news"), visible: true });
  const [pendingAssignments] = useState(false);
  const [roles, setRoles] = useState([]);
  const [totalPendingAssignments, setTotalPendingAssignments] = useState(0);
  const [totalTeacherGroups, setTotalTeacherGroups] = useState(0);
  const [totalUnreadMessages, setTotalUnreadMessages] = useState(0);
  const [totalUnreadPromptAlerts, setTotalUnreadPromptAlerts] = useState(0);
  const [totalStudents, setTotalStudents] = useState(0);
  const [userName, setUserName] = useState("");
  const [redirectLogin, setRedirectLogin] = useState(false);

  const hasRole = (role) => roles.includes(role);
  const hasAnyRole = (allowedRoles) => allowedRoles.some(hasRole);

  /**
   * True when the current user holds a role whose management flag is off.
   * `flagsByRole` maps a role string to its context feature flag.
   */
  const isDisabledForRole = (flagsByRole) =>
    Object.entries(flagsByRole).some(([role, flag]) => hasRole(role) && !flag);

  useEffect(() => {
    if (sessionDetails == null) {
      setRedirectLogin(true);
      return;
    }

    const sessionRoles = sessionDetails.Roles;
    setRoles(sessionRoles);
    setUserName(sessionDetails.Name);

    if (sessionRoles.includes(Settings.ROLE_CLIENT_STR)) {
      checkIfMiniGamesAreEnabled();
    }
    if (sessionRoles.includes(Settings.ROLE_TEACHER_STR)) {
      callTotalPendingAssignments();
      callTotalGroups();
      callTotalStudents();
    }
    // Prompt monitoring is visible only to teachers and central admins (NOT
    // admin-ore nor director), so the badge is fetched only for those.
    if (
      sessionRoles.includes(Settings.ROLE_TEACHER_STR) ||
      sessionRoles.includes(Settings.ROLE_ADMIN_STR)
    ) {
      callUnreadPromptAlerts();
    }
    callUnreadMessages();

    const token = SessionManager.getSession();
    if (token != null) {
      callActivityReportCreate(token);
    }
  }, []);

  const callUnreadMessages = async () => {
    try {
      const response = await getTotalOfUnreadMessages();
      setTotalUnreadMessages(response.data.TotalUnread);
    } catch (error) {
      console.error("Error fetching total unread messages:", error);
    }
  };

  const callUnreadPromptAlerts = async () => {
    try {
      const total = await fetchUnreadAlertsTotal();
      setTotalUnreadPromptAlerts(total);
    } catch (error) {
      console.error("Error fetching total unread prompt alerts:", error);
    }
  };

  const callTotalStudents = async() => {
    try {
      const response = await callStudentsCounting();
       setTotalStudents(response.data)
    } catch (error) {
      console.error("Error fetching total pending assignments:", error);
    }
  }

  const callTotalPendingAssignments = async () => {
    try {
      const response = await getTotalPendingAssignmentsForAllPlans();
      setTotalPendingAssignments(response.data.TotalPendingAssignments);
    } catch (error) {
      console.error("Error fetching total pending assignments:", error);
    }
  };

  const callTotalGroups = async () => {
    try {
      const response = await getTeacherGroupsTotal();
      setTotalTeacherGroups(response.data);
    } catch (error) {
      console.error("Error fetching total pending assignments:", error);
    }
  };

  const checkIfMiniGamesAreEnabled = async () => {
    try {
      const response = await getMiniGamesLock(true);
      setChecked(!response.data);
    } catch (error) {
      console.error("Error checking mini games status:", error);
    }
  };

  // Turn the locked video games area on or off
  const handleMiniGamesToggle = async () => {
    const newChecked = !checked;
    setChecked(newChecked);
    try {
      const response = await insertMiniGamesLockCreate(!newChecked);
      setChecked(!response.data);
    } catch (error) {
      console.error("Error updating mini games status:", error);
    }
  };

  const toggleAlertModal = () => setModalAlertNews((prev) => !prev);

  if (redirectLogin) {
    return <Redirect to="/" />;
  }

  const isTeacher = hasRole(Settings.ROLE_TEACHER_STR);
  const isDirector = hasRole(Settings.ROLE_DIRECTOR_STR);
  const isParent = hasRole(Settings.ROLE_PARENT_NO_CLIENT_STR);
  const isClient = hasRole(Settings.ROLE_CLIENT_STR);
  const isSchoolsReadOnly = isDisabledForRole({
    [Settings.ROLE_ADMIN_STR]: variables.SchoolManagementAdmin,
    [Settings.ROLE_ADMIN_ORE_STR]: variables.SchoolManagementAdminOre,
  });
  const isUsersReadOnly = isDisabledForRole({
    [Settings.ROLE_ADMIN_STR]: variables.UserManagementAdmin,
    [Settings.ROLE_ADMIN_ORE_STR]: variables.UserManagementAdminOre,
  });

  /**
   * Every quick-access card, in render order.
   * `visible` is the role/feature-flag gate: a card the user cannot
   * enter is never rendered.
   */
  const cards = [
    {
      id: "students",
      visible: isTeacher || isDirector || isParent || isClient,
      Icon: Ic.student,
      ...TINTS.terracotta,
      title: t("dashboard.manage-profiles"),
      desc: t(isTeacher ? "dashboard.manage-profiles-subtitle-teacher" : "dashboard.manage-profiles-subtitle"),
      route: "/profile-update",
      badge: totalUnreadPromptAlerts > 0 ? totalUnreadPromptAlerts : null,
    },
    {
      id: "add-new-plan",
      visible: IS_APP_MANAGED_BY_REQUIRED && isClient && !isMicrosoftTeams,
      iconSrc: iconAddPlan,
      alt: t("dashboard.alt.add-plan"),
      ...TINTS.indigo,
      title: t("dashboard.add-new-plan"),
      desc: t("dashboard.add-new-plan-subtitle"),
      route: "/select-plan",
    },
    {
      id: "groups",
      visible: isTeacher,
      Icon: Ic.groups,
      ...TINTS.teal,
      title: t(variables.GroupInternalManagementTeacher ? "dashboard.manage-group" : "dashboard.manage-subgroup"),
      desc: t(variables.GroupInternalManagementTeacher ? "dashboard.manage-group-subtitle" : "dashboard.manage-subgroup-subtitle"),
      route: "/teacher-groups",
    },
    {
      id: "pending-teacher",
      visible: isTeacher,
      Icon: Ic.pending,
      ...TINTS.coral,
      title: t("dashboard.pending"),
      desc: t("dashboard.pending-assignment-subtitle"),
      route: "/pending-teacher-assignments",
      badge: totalPendingAssignments > 0 ? totalPendingAssignments : null,
    },
    {
      id: "planning",
      visible: isTeacher,
      Icon: Ic.planning,
      ...TINTS.amber,
      title: t("dashboard.planning"),
      desc: t("dashboard.planning-subtitle"),
      route: "/planning-select",
    },
    {
      id: "progress",
      visible: isTeacher || isDirector || isParent || isClient,
      Icon: Ic.progress,
      ...TINTS.green,
      title: t("dashboard.progress"),
      desc: t("dashboard.progress-subtitle"),
      route: "/progress-select",
    },
    {
      id: "pending-family",
      visible: isParent,
      iconSrc: iconPending,
      alt: t("dashboard.alt.pending-assignments"),
      ...TINTS.coral,
      title: t("dashboard.pending-assignments"),
      desc: t("dashboard.pending-assignments-subtitle"),
      route: "/pending-assignments",
      badge: pendingAssignments ? "!" : null,
    },
    {
      id: "messages",
      visible: !isDisabledForRole({
        [Settings.ROLE_ADMIN_STR]: variables.MessageManagementAdmin,
        [Settings.ROLE_ADMIN_ORE_STR]: variables.MessageManagementAdminOre,
        [Settings.ROLE_DIRECTOR_STR]: variables.MessageManagementPrincipal,
        [Settings.ROLE_PARENT_NO_CLIENT_STR]: variables.MessageManagementParent,
        [Settings.ROLE_TEACHER_STR]: variables.MessageManagementTeacher,
      }),
      Icon: Ic.messages,
      ...TINTS.turquoise,
      title: t("dashboard.mail"),
      desc: t("dashboard.mail-subtitle"),
      route: "/messages",
      badge: totalUnreadMessages > 0 ? totalUnreadMessages : null,
    },
    {
      id: "syllabus",
      visible: isDirector,
      iconSrc: iconSyllabus,
      alt: t("dashboard.alt.syllabus"),
      ...TINTS.red,
      title: t(IS_APP_MANAGED_BY_REQUIRED ? "dashboard.syllabus" : "dashboard.academic-content"),
      desc: t("dashboard.syllabus-subtitle"),
      route: IS_APP_MANAGED_BY_REQUIRED ? "/syllabus" : "/academic-content-select",
    },

    /** ADMIN ONLY **/
    {
      id: "schools",
      visible: hasAnyRole(ADMIN_ROLES),
      iconSrc: iconSchools,
      alt: t("dashboard.alt.schools"),
      ...TINTS.blue,
      title: t("dashboard.schools"),
      desc: t(isSchoolsReadOnly ? "dashboard.school-subtitle-read" : "dashboard.school-subtitle"),
      route: "/school-entity-search",
    },
    // {
    //   id: "curricular-years",
    //   visible: hasRole(Settings.ROLE_ADMIN_STR),
    //   iconSrc: iconPlanning,
    //   alt: t("dashboard.alt.academic-years"),
    //   ...TINTS.violet,
    //   title: t("dashboard.academic-years"),
    //   desc: t("dashboard.academic-years-subtitle"),
    //   route: "/academic-years-search",
    // },
    {
      id: "users",
      visible: hasAnyRole(ADMIN_ROLES),
      iconSrc: iconGroup,
      alt: t("dashboard.alt.users"),
      ...TINTS.blue,
      title: t("dashboard.users"),
      desc: t(isUsersReadOnly ? "dashboard.users-subtitle-read" : "dashboard.users-subtitle"),
      route: "/users-search",
      badge: totalUnreadPromptAlerts > 0 ? totalUnreadPromptAlerts : null,
    },
    {
      id: "reports",
      visible: hasAnyRole(ADMIN_ROLES) && IS_APP_MANAGED_BY_REQUIRED,
      iconSrc: iconReports,
      alt: t("dashboard.alt.reports"),
      ...TINTS.sand,
      title: t("dashboard.reports"),
      desc: t("dashboard.reports-subtitle"),
      route: "/analysis/dashboard",
    },
    /** END OF ADMIN ONLY **/

    {
      id: "settings",
      visible: !isDisabledForRole({
        [Settings.ROLE_ADMIN_STR]: variables.ConfigurationManagerAdmin,
        [Settings.ROLE_ADMIN_ORE_STR]: variables.ConfigurationManagerAdminOre,
        [Settings.ROLE_TEACHER_STR]: variables.ConfigurationManagerTeacher,
        [Settings.ROLE_DIRECTOR_STR]: variables.ConfigurationManagerPrincipal,
        [Settings.ROLE_PARENT_NO_CLIENT_STR]: variables.ConfigurationManagerParent,
      }),
      iconSrc: iconConfigurations,
      alt: t("dashboard.alt.settings"),
      ...TINTS.gray,
      title: t("dashboard.settings"),
      desc: t(hasRole(Settings.ROLE_ADMIN_STR) ? "dashboard.settings-subtitle-admin" : "dashboard.settings-subtitle"),
      route: "/configuration",
    },
  ];

  const visibleCards = cards.filter((card) => card.visible);

  const showTeacherNewsModal =
    modalAlertNews &&
    isTeacher &&
    localStorage.getItem("modal-power-de-notification" + SessionManager.getSession()) !== "1";

  return (
    <div>
      <div className="container-padding animated delay-05s fadeIn adjust-height-cards">
        <div className="wrapper-principal-container">
          <Row>
            <Col lg={12}>

              <Row>
                <Col>
                  <Welcome firstName={userName}
                           totalMessagesUnread={totalUnreadMessages}
                           totalStudents={totalStudents}
                           totalGroups={totalTeacherGroups}
                           totalPendingHomework={totalPendingAssignments}
                           roles={roles}/>
                </Col>
              </Row>

              { roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 ? null :
              <Row>
                <Col>
                  <QuickGrid
                      title={t("common:general.quick-access")}
                      enterLabel={t("dashboard.enter")}
                      cards={visibleCards}
                  />
                </Col>
              </Row>}

              {IS_APP_MANAGED_BY_REQUIRED && isClient ? (
                  <Row>
                    <Col xs={12} className="clear-style-card">
                      <Card>
                        <CardBody>
                          <TableActivity/>
                        </CardBody>
                      </Card>
                    </Col>
                  </Row>
              ) : null}

              {isClient ? (
                  <div className="clear-style-card bg-light-gray p-2">
                    <div className="title-principal mb-2">
                      {t("dashboard.entertainment-games")}
                    </div>
                    <Card id="entertainment-games" className="clear-min-height bg-light-gray">
                      <CardBody>
                        <Row className="dashboard-entertainment">
                          <Col lg="10" md="10" sm="12" xs="12">
                            <Trans
                                i18nKey="dashboard.entertainment-games-instructions"
                                ns="navigation"
                                components={[<h4/>, <i/>, <br/>]}
                            />
                          </Col>
                          <Col lg="2" md="2" sm="12" xs="12" className="text-end">
                            <div className="form-check form-switch mb-3">
                              <input
                                  type="checkbox"
                                  className="form-check-input me-2"
                                  role="switch"
                                  style={{fontSize: "1.4rem", marginLeft: "0.5rem"}}
                                  id="normal-switch"
                                  onChange={handleMiniGamesToggle}
                                  checked={checked}
                              />
                            </div>
                          </Col>
                        </Row>
                      </CardBody>
                    </Card>
                  </div>
              ) : null}
              <br/>
              <Row>
                <Col>
                  <div className="med-section-head">
                    <h2>
                      { roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 ?
                      t("navigation:dashboard.plan-events") :
                          t("navigation:dashboard.just-events")
                      }
                      </h2>
                    <span className="rule"></span>
                    { roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 ?
                    <Link to={"/plannings"}>
                      {t("navigation:dashboard.open-plans")} →
                    </Link> : null}
                    </div>
                  <CalendarEventsWeekly/>
                </Col>
              </Row>
              {isTeacher ?
                  <Row>
                    <Col>
                      <br/>
                      <div className="med-section-head"><h2>
                        {t("navigation:dashboard.ai-tools")}
                      </h2><span className="rule"></span><Link to="/ai-tools">
                        {t("navigation:dashboard.see-all-ai")} →</Link></div>

                      <AITools/>
                    </Col>
                  </Row>

                  : null
              }

              {IS_APP_MANAGED_BY_REQUIRED && isClient ? (
                  <Row className="danger-btn-simple content-table-data td-responsive adjustment-font-table">
                    <Col lg="12" sm="12" md="12" xs="12">
                      <div className="wrapper-col wrapper-col__align-top block-display-responsive mt-4">
                        <div className="col col-no-padding-left">
                          <div className="title-principal mb-4">
                            {t("subscription:subscriptions.my-subscriptions")}
                          </div>
                        </div>
                        <div className="col txt-right">
                          <Link to="/subscription-add-new" className="btn button-generic">
                            <FontAwesomeIcon icon={faPlus}/>
                            {t("dashboard.add-new-subscriptions")}
                          </Link>
                        </div>
                      </div>
                      <TableSubscriptions/>
                    </Col>
                  </Row>
              ) : null}
            </Col>

          </Row>

          <Row>
            <Col>
              <footer className="med-dash-footer">
                <a target="_blank" href="/privacy">
                  {t("dashboard.children-policy")}
                </a>
                <LanguageSelector styleButton={true}/>
              </footer>
            </Col>
          </Row>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
