import React, {useCallback, useEffect, useRef, useState} from "react";
import { NavItem } from "reactstrap";
import {Translate, withLocalize} from "react-localize-redux";
import { NavLink, useHistory } from "react-router-dom";
import SessionManager from "../../../custom-classes/session-manager";
import UserMenu from "../../../assets/images/common-icons/Header-icon-UserMenu.svg";
import Button from "@material-ui/core/Button";
import Menu from "@material-ui/core/Menu";
import MenuItem from "@material-ui/core/MenuItem";
import HelpDialog from "../modal/modal-clickup";
import "./style.css";
import "./style-navbar.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faHome,
  faIdCard,
  faCogs,
  faSitemap,
  faTasks, faEllipsisV,
} from "@fortawesome/free-solid-svg-icons";
import LogoutItem from "./logout";
import Settings from "../../../custom-classes/settings";
import ModalShowMeHowToUseItAlt from "../../teacher-module/modal/modal-show-me-how-to-use-it-alt";
import {renderToStaticMarkup} from "react-dom/server";
import { getAsset } from "../../../utils/getAssets";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";
import { Icon } from '@iconify/react';
const ImageBackground = getAsset("Header-icon-Dropdown");
const IconAvatar = getAsset("Common-icon-Avatar");

const NavbarClients = (props) => {
  const { showExtra, initialize, addTranslation } = props;

  // Use useRef for DOM references
  const ref = useRef(null);

  // Use useState for component state
  const [nameUser, setNameUser] = useState("");
  const [invitationCode, setInvitationCode] = useState("");
  const [imageBackground] = useState(ImageBackground); // ImageBackground is static, no need for setState
  const [isParentsTeamsAPPState] = useState(
      typeof isParentsTeamsAPP !== "undefined" ? isParentsTeamsAPP : false
  );
  const [isLoggedIn, setIsLoggedIn] = useState(
      SessionManager.getSession() != null && SessionManager.getSession().length > 0
  );
  const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams()); // Static once set
  const [isOpen, setIsOpen] = useState(false);
  const [isOpenClickupDialog, setIsOpenClickupDialog] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);
  const [roles, setRoles] = useState([]);
  const [modalShowHowToUseGSWeb, setModalShowHowToUseGSWeb] = useState(false);
  const [showExtraState] = useState(
      typeof showExtra !== "undefined" ? showExtra : true
  );
  const history = useHistory();

  // Initialize translations based on the current version
  useVersionedTranslations();

  // Helper functions - convert to regular functions or useCallback
  const isRoleAllowed = useCallback((allowRoles, sessionRoles) => {
    if(sessionRoles === null || sessionRoles.length === 0) {
        return false; // No roles in session, deny access
    }
    return allowRoles.some(role => sessionRoles.includes(role));
  }, []); // No dependencies, so can be cached

  const isRoleDenied = useCallback((deniedRoles, sessionRoles) => {
    if(typeof sessionRoles === undefined || sessionRoles === null || sessionRoles.length === 0) {
      return false; // No roles in session, deny access
    }

    return !deniedRoles.some(role => sessionRoles.includes(role));
  }, []); // No dependencies, so can be cached

  // Event Handlers - use useCallback for functions that depend on state/props
  const toggle = useCallback(() => {
    setIsOpen(prevIsOpen => {
      const newIsOpen = !prevIsOpen;
      if (newIsOpen) {
        document.addEventListener("keydown", handleHideDropdown, true);
        document.addEventListener("click", handleClickOutside, true);
      } else {
        document.removeEventListener("keydown", handleHideDropdown, true);
        document.removeEventListener("click", handleClickOutside, true);
      }
      return newIsOpen;
    });
  }, []); // Dependencies: handleHideDropdown, handleClickOutside

  const toggleModal = useCallback(() => {
    setModalShowHowToUseGSWeb(false);
  }, []);

  const handleClickStart = useCallback(() => {
    setModalShowHowToUseGSWeb(true);
  }, []);

  const handleHideDropdown = useCallback(() => {
    setIsOpen(false);
  }, []);

  const handleClickOutside = useCallback((event) => {
    if (ref.current && !ref.current.contains(event.target)) {
      setIsOpen(false);
    }
  }, []); // No dependencies

  const setOpenClickupDialog = useCallback((isOpenVal) => {
    if (isOpenVal) {
      window.open(Settings.SUPPORT_URL, '_blank');
    }
    setIsOpenClickupDialog(isOpenVal);
  }, []);

  const openClickupDialog = useCallback(() => {
    setOpenClickupDialog(true);
  }, [setOpenClickupDialog]);

  const closeClickupDialog = useCallback(() => {
    setOpenClickupDialog(false);
  }, [setOpenClickupDialog]);

  const handleMenuClick = useCallback((event) => {
    setAnchorEl(event.currentTarget);
  }, []);

  const handleMenuClose = useCallback(() => {
    setAnchorEl(null);
  }, []);



  const postCrossDomainMessage = useCallback(() => {
    const sessionDetails = SessionManager.getSessionDetails();
    let iframe = document.createElement('iframe');
    iframe.src = Settings.URL_DELIVERES;
    iframe.loading = 'lazy';
    iframe.style = 'visibility: hidden;';
    document.body.appendChild(iframe);
    iframe.onload = () => {
      let win = iframe.contentWindow;
      win.postMessage(sessionDetails, Settings.URL_DELIVERES);
      const url = `${Settings.URL_DELIVERES}/#/analysis/dashboard`;
      window.open(url, "_blank");
    }

    try{            
      SendAccessLog({
        Username: sessionDetails.Username,
        InvitationCode: sessionDetails.InvitationCode,
        Role: sessionDetails.Roles[0],
        AreaId: "13"
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  }, []);

  const goToSiteDeliveres = useCallback(() => {
    postCrossDomainMessage();
  }, [postCrossDomainMessage]);

  const goToAnalysisApp = () => {
    history.push("/analysis/dashboard");
  };


  // useEffect for componentDidMount logic
  useEffect(() => {
    const sessionDetails = SessionManager.getSessionDetails();
    if (sessionDetails !== null) {
      if (localStorage.getItem("modal-tour-seen") !== "1") {
        if (isRoleDenied([
          Settings.ROLE_EMPLOYEE_STR,
          Settings.ROLE_ADMIN_STR,
          Settings.ROLE_DIRECTOR_STR,
          Settings.ROLE_PARENT_NO_CLIENT_STR,
          Settings.ROLE_CLIENT_STR,
          Settings.ROLE_STUDENT_STR
        ], sessionDetails.Roles)) {
          setModalShowHowToUseGSWeb(true);
        }
      }

      setNameUser(sessionDetails.Name || "");
      setInvitationCode(sessionDetails.InvitationCode || "");
      setRoles(sessionDetails.Roles || []);
    }

    // Cleanup for event listeners when component unmounts or isOpen changes
    return () => {
      document.removeEventListener("keydown", handleHideDropdown, true);
      document.removeEventListener("click", handleClickOutside, true);
    };
  }, [isRoleDenied]); // isRoleDenied is a dependency because it's used inside the effect

  return (
      <>
        <div className="wrapper-header">
          <header className="main-header">
            {props.onHamburger && (
              <button
                type="button"
                className="med-hamburger"
                onClick={props.onHamburger}
                aria-label="Abrir menú"
              >
                <span />
                <span />
                <span />
              </button>
            )}
            <div className="main-header_GenialSkills-logo">
              {
                  Settings.ENVIRONMENT === "development" &&
                  <span className="noticesDev">Dev</span>
              }
              {
                  Settings.ENVIRONMENT === "uat" &&
                  <span className="noticesBeta">Uat</span>
              }
            </div>
            <div className="main-header_top-bar-menu">
              <nav className="top-bar-menu">
                <li>
                  {roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 ? (
                      <a
                          className="help"
                          style={{ cursor: "pointer" }}
                          onClick={handleClickStart}
                      >
                        <FontAwesomeIcon icon={faSitemap} />{" "}
                        <span className="help-title">
                      <Translate
                          id="dashboard-steps.start-tour"
                          options={{ renderInnerHtml: true }}
                      />
                    </span>
                      </a>
                  ) : null}
                </li>
                <li>
                  {isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_DIRECTOR_STR, Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR], roles) ? (
                      <a
                          className="help ml-3"
                          onClick={goToAnalysisApp}
                          style={{ cursor: "pointer" }}
                      >
                        <FontAwesomeIcon icon={faTasks} />{" "}
                        <span className="help-title">
                      <Translate
                          id="navbar.deliverables"
                          options={{ renderInnerHtml: true }}
                      />
                    </span>
                      </a>
                  ) : null}
                </li>
                {isLoggedIn ?
                    <li className="ml-3 mr-4">
                      <Button className="custom-menu-button" aria-controls="simple-menu" aria-haspopup="true" onClick={handleMenuClick}>
                        <i className='fas fa-hands-helping'></i>
                        <span className="help-title">
                      <Translate
                          id="navbar.help"
                          options={{ renderInnerHtml: true }}
                      />
                    </span>
                      </Button>

                      <Menu
                          id="simple-menu"
                          className="dropdown-top-help"
                          anchorEl={anchorEl}
                          keepMounted
                          open={Boolean(anchorEl)}
                          onClose={handleMenuClose}>
                        <MenuItem className="dropdown-top-help--item" onClick={handleMenuClose}>
                          <NavItem>
                            <NavLink to="/help">
                              <Translate
                                  id="clickup-modal.option-questions"
                                  options={{ renderInnerHtml: true }}
                              />
                            </NavLink>
                          </NavItem>
                        </MenuItem>
                        <MenuItem className="dropdown-top-help--item" onClick={openClickupDialog}>
                          <Translate id="clickup-modal.option-form"
                                     options={{ renderInnerHtml: true }} />
                        </MenuItem>
                      </Menu>
                    </li> : null}
                <li>
                  {showExtraState ? (
                      <div className="user-comp">
                        <ul className="user-comp_profile-wrapper">
                          <li>
                            <div
                                className="user-comp_profile"
                                onClick={toggle}
                            >
                          <span className="user-menu-icon">
                            <FontAwesomeIcon icon={faEllipsisV} />
                          </span>
                              <div className="double-border--principal">
                                <img src={IconAvatar} alt="User Avatar" />
                              </div>
                              {isLoggedIn ? (
                                  <span className="user-name">
                              {nameUser}
                            </span>
                              ) : null}
                            </div>
                          </li>
                        </ul>
                        {isOpen ? (
                            <ul
                                ref={ref}
                                className="user-comp_menu"
                                style={{
                                  backgroundImage: `url(${ImageBackground})`,
                                }}
                            >
                              <div className="user-comp_info-user">
                                <div className="double-border">
                                  <img src={IconAvatar} alt="User Avatar" />
                                </div>
                                <h1>{nameUser}</h1>
                                {isLoggedIn ? (
                                    <h4>
                                      <Translate
                                          id="navbar.invitation-code"
                                          options={{ renderInnerHtml: true }}
                                      />
                                      : {invitationCode}
                                    </h4>
                                ) : null}
                              </div>

                              <ul className="user-comp_submenu">
                                {isLoggedIn ? (
                                    <NavItem>
                                      <NavLink to="/dashboard">
                                        <FontAwesomeIcon icon={faHome} />{" "}
                                        <Translate
                                            id="navbar.dashboard"
                                            options={{ renderInnerHtml: true }}
                                        />
                                      </NavLink>
                                    </NavItem>
                                ) : null}

                                {(!isMicrosoftTeams && Settings.IS_APP_MANAGED_BY_REQUIRED &&
                                    !isLoggedIn) ||
                                (isLoggedIn && Settings.IS_APP_MANAGED_BY_REQUIRED &&
                                    roles.indexOf(
                                        Settings.ROLE_CLIENT_STR
                                    ) > -1) ? (
                                    <NavItem>
                                      <NavLink to="/payment">
                                        <FontAwesomeIcon icon={faIdCard} />{" "}
                                        {isLoggedIn ? (
                                            <Translate
                                                id="navbar.add-new-plan"
                                                options={{ renderInnerHtml: true }}
                                            />
                                        ) : roles.indexOf(
                                            Settings.ROLE_TEACHER_STR
                                        ) === -1 ? (
                                            <Translate
                                                id="navbar.subscribe"
                                                options={{ renderInnerHtml: true }}
                                            />
                                        ) : null}
                                      </NavLink>
                                    </NavItem>
                                ) : null}

                                {!isLoggedIn || (isRoleAllowed([Settings.ROLE_ADMIN_STR], roles)) && (
                                    <NavItem>
                                      <NavLink to="/configuration">
                                        <FontAwesomeIcon icon={faCogs} />{" "}
                                        <Translate
                                            id="navbar.settings"
                                            options={{ renderInnerHtml: true }}
                                        />
                                      </NavLink>
                                    </NavItem>
                                )}
                                <LogoutItem />
                              </ul>
                            </ul>
                        ) : null}
                      </div>
                  ) : null}
                </li>
              </nav>
            </div>
          </header>
          {modalShowHowToUseGSWeb ? (
              <ModalShowMeHowToUseItAlt
                  visible={modalShowHowToUseGSWeb}
                  updateParent={toggleModal}
              />
          ) : null}
          {isOpenClickupDialog && <HelpDialog open={isOpenClickupDialog} onClose={closeClickupDialog} />}
        </div>
      </>
  );
};


export default withLocalize(NavbarClients);
