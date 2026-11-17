import React, { useState, useEffect } from "react";
import {
    Card,
    CardBody,
    CardTitle,
    Col,
    Container,
    Row,
    Button,
} from "reactstrap";
import { Link, Redirect } from "react-router-dom";
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";

import { callActivityReportCreate } from "../../../api/users";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChalkboardTeacher } from "@fortawesome/free-solid-svg-icons";
import { getAsset } from "../../../utils/getAssets";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from '../../../utils/access_logger';

const IconPlan = getAsset("Teacher-icon-Plans");
const StudentIcon = getAsset("Teacher-icon-Student");

/**
 * The component (window) to handle the main dashboard of the users
 */
const AcademicContentSelect = ({ history }) => {
    const [roles, setRoles] = useState([]);
    const [sessionValid, setSessionValid] = useState(true);
    const [redirectLogin, setRedirectLogin] = useState(false);
    const [userName, setUserName] = useState('');
    const [invitationCode, setInvitationCode] = useState('');
    const [subjectTitle, setSubjectTitle] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        let sessionDetails = SessionManager.getSessionDetails();

        if (sessionDetails == null) {
            setRedirectLogin(true);
        }
        else {
            setRoles(sessionDetails.Roles);
            
            if (sessionDetails.Roles.indexOf(Settings.ROLE_CLIENT_STR) > -1) {
                // checkIfMiniGamesAreEnable();
            }
            if (sessionDetails.Roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 || sessionDetails.Roles.indexOf(Settings.ROLE_PARENT_NO_CLIENT_STR) > -1) {
                // callUnreadMessages();
            }
            if (sessionDetails.Roles.indexOf(Settings.ROLE_PARENT_NO_CLIENT_STR) > -1) {
                // callPendingAssignments();
            }

            setUserName(sessionDetails.Name);
            setInvitationCode(sessionDetails.InvitationCode);

            let courseSelected = SessionManager.getSelectedCourse();
            if (courseSelected != null) {
                setSubjectTitle(courseSelected.Name);
            }

            let token = SessionManager.getSession();
            if (token != null) {
                callActivityReportCreate(SessionManager.getSession());
            }
        }
    }, []);

    // Renders the main dashboard
    const isRoleAllowed = (allowRoles) => {
        return allowRoles.some(role => roles.includes(role));
    };

    const isRoleDenied = (deniedRoles) => {
        return !deniedRoles.some(role => roles.includes(role));
    };

    const postCrossDomainMessage = () => {
        const sessionDetails = SessionManager.getSessionDetails();
        let iframe = document.createElement('iframe');
        iframe.src = Settings.URL_SITE_STUDENTS;
        iframe.loading = 'lazy';
        iframe.style = 'visibility: hidden;';
        document.body.appendChild(iframe);
        iframe.onload = () => {
            let win = iframe.contentWindow;
            win.postMessage({ type: 'login', body: sessionDetails }, Settings.URL_SITE_STUDENTS);
            const url = `${Settings.URL_SITE_STUDENTS}/students/lesson`;
            window.open(url, "_blank");
        }
    };

    const goToSiteStudentsLesson = () => {
        try{
            let userDetails = SessionManager.getSessionDetails();
            SendAccessLog(
                {
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "21",
                Details: "Accessing students platform",
                })
        } catch (e) {
        // empty catch block to avoid breaking the application
        }
        history.push("/students/lesson");
        
    };

    const allowStudents = [Settings.ROLE_DIRECTOR_STR],
          allowSyllabus = [Settings.ROLE_DIRECTOR_STR];

    return (
        <div>
            <div className="container-padding  animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    {/* <div className="wrapper-col block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate
                                id="academic-content.academic-content-title"
                                options={{ renderInnerHtml: true }}
                            />
                            </div>
                        </div>
                        <div className="col txt-right">
                        </div>
                    </div> */}
                    <div className="wrapper-planning mt-5">
                        {isRoleAllowed(allowSyllabus) ? (
                            <div className="planning-two-col">
                                <div className="mod-planning">
                                    <div className="mod-planning-title">
                                        <div className="mod-planning-title__fixed-size">
                                            <img src={IconPlan} alt="Plan icon" />
                                            {/* <img
                                        src={require("../../../assets/images/buttons-dashboard-13.png")}
                                        alt="dashboard-syllabus"
                                        /> */}
                                        </div>
                                        <div className="mod-planning-title__auto-size">
                                            <h1 className="mod-planning-title__title academic-content-title">
                                                <Translate id="dashboard.syllabus"
                                                    options={{ renderInnerHtml: true }}
                                                />
                                            </h1>
                                        </div>
                                    </div>
                                    <p>
                                        <Translate id="dashboard.syllabus-subtitle"
                                            options={{ renderInnerHtml: true }}
                                        />
                                    </p>
                                    <div className="center-button btn-bg-DarkBlue">
                                        <Link to="/syllabus" className="centered-text btn btn-default button-primary-custom">
                                            <Translate id="dashboard.enter"
                                                options={{ renderInnerHtml: true }}
                                            />
                                        </Link>
                                    </div>
                                </div>
                            </div>
                        ) : null}
                        {isRoleAllowed(allowStudents) ? (
                            <div className="planning-two-col">
                                <div className="mod-planning">
                                    <div className="mod-planning-title">
                                        <div className="mod-planning-title__fixed-size">
                                            <img src={StudentIcon} alt="Student icon" />
                                            {/* <img
                                        src={require("../../../assets/images/buttons-dashboard-05.png")}
                                        alt="dashboard-profiles"
                                        /> */}
                                        </div>

                                        <div className="mod-planning-title__auto-size">
                                            <h1 className="mod-planning-title__title academic-content-title">
                                                <Translate id="academic-content.academic-content-students-platform"
                                                    options={{ renderInnerHtml: true }}
                                                />
                                            </h1>
                                        </div>
                                    </div>

                                    <p>
                                        <Translate id="academic-content.academic-content-students-platform-explanation"
                                            options={{ renderInnerHtml: true }}
                                        />
                                    </p>

                                    <div className="center-button btn-bg-LightGreen btn-space-svg">
                                        <a onClick={goToSiteStudentsLesson} className="centered-text btn btn-default button-primary-custom">
                                            <FontAwesomeIcon icon={faChalkboardTeacher} />{' '}
                                            <Translate id="profile-update.link-to-students" options={{ renderInnerHtml: true }} />
                                        </a>
                                    </div>

                                </div>
                            </div>
                        ) : null}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default withLocalize(AcademicContentSelect);
