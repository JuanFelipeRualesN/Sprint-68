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
const SharedRoute = ({ component: Component, ...rest }) => {

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

    return (
        <Route
            {...rest}
            render={props =>
                isLoggedIn ? (
                    <Component {...props} />
                ) : (
                    <Redirect to={{ pathname: '/', state: { from: props.location } }} />
                )
            }
        />
    )
};

export default SharedRoute;