// If they are: they proceed to the page
// If not: they are redirected to the login page.
import React from 'react'
import { Redirect, Route } from 'react-router-dom'
import SessionManager from '../../custom-classes/session-manager';
import Settings from "../../custom-classes/settings";
/**
 * Manages the Private routes. DO NOT delete this class
 * @param Component
 * @param rest
 * @returns {XML}
 * @constructor
 */
const TeacherRoute = ({ component: Component, ...rest }) => {

    // Add your own authentication on the below line.
    let isLoggedIn = SessionManager.evaluateSession();
    // Check if comes from Microsoft Teams.
    if (window.location.href.indexOf("teams=true") > -1) {
        SessionManager.setMicrosoftTeams();
    }

    const sessionDetails = SessionManager.getSessionDetails();
    if (sessionDetails === null) {
        isLoggedIn = false;
    }
    else {
        if (sessionDetails.Roles.indexOf(Settings.ROLE_TEACHER_STR) <= -1) {
            isLoggedIn = false;
        }
    }

    // If version is different from msj, block the route
    const isLearningGapRoute = rest.path === "/learning-gap" || (rest.path && rest.path.includes("/learning-gap"));
    const isLearningGapBlocked = isLearningGapRoute && Settings.PROJECT_VERSION !== "msj";

    return (
        <Route
            {...rest}
            render={props => {
                if (isLearningGapBlocked) {
                    return <Redirect to={{ pathname: '/', state: { from: props.location } }} />;
                }

                return isLoggedIn ? (
                    <Component {...props} />
                ) : (
                    <Redirect to={{ pathname: '/', state: { from: props.location } }} />
                );
            }}
        />
    )
};

export default TeacherRoute;