import React from "react";
import { NavItem } from "reactstrap";
import { Translate } from "react-localize-redux";
import { NavLink } from "react-router-dom";
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useIsAuthenticated, useMsal } from "@azure/msal-react";
import {
    faSignOutAlt,
    faSignInAlt,
} from "@fortawesome/free-solid-svg-icons";

const LogoutItem = () => {
    const isAuthenticated = useIsAuthenticated();
    const isLoggedIn = SessionManager.getSession();
    const sessionDetails = SessionManager.getSessionDetails();
    const { instance } = useMsal();

    const onLogout = async () => {
        if (isAuthenticated) {
            await instance.logoutPopup({
                postLogoutRedirectUri: "/",
                mainWindowRedirectUri: "/",
            });
            SessionManager.deleteSession();
        } else {
            SessionManager.deleteSession();
        }
        if (isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles)) {
            postMessageSignOutStudents();
        }
    };

    const postMessageSignOutStudents = () => {
        let iframe = document.createElement('iframe');
        iframe.src = Settings.URL_SITE_STUDENTS;
        iframe.loading = 'lazy';
        iframe.style = 'visibility: hidden;';
        document.body.appendChild(iframe);
        iframe.onload = () => {
            let win = iframe.contentWindow;
            win.postMessage({ type: 'logout' }, Settings.URL_SITE_STUDENTS);
        }
    };

    if (isLoggedIn) {
        return (
            <NavItem>
                <a onClick={onLogout}>
                    <FontAwesomeIcon icon={faSignOutAlt} />{" "}
                    <Translate
                        id="navbar.logout"
                        options={{ renderInnerHtml: true }}
                    />
                </a>
            </NavItem>
        );
    }
    return (
        <NavItem>
            <NavLink to="/">
                <FontAwesomeIcon icon={faSignInAlt} />{" "}
                <Translate
                    id="navbar.login"
                    options={{ renderInnerHtml: true }}
                />
            </NavLink>
        </NavItem>
    );
};

export default LogoutItem;