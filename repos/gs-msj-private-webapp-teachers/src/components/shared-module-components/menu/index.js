import React, { useContext, useState, useEffect } from "react";
import "./style.css";
import "./style-refresh.css";
import "../../../styles/med.css";
import { Link } from "react-router-dom";
import { useLocation } from 'react-router-dom'
import { Translate } from "react-localize-redux";
import Settings, {DASHBOARD_AI_MODULE_AVAILABLE} from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { Redirect } from "react-router-dom";
import { AppContext } from "../../../App.Context";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faExclamationCircle, faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { getAsset } from "../../../utils/getAssets";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { fetchUnreadAlertsTotal } from "../profiles-prompt-alerts";

// Teacher Menu Icons
const IconHome = getAsset("NavMenu-icon-Home");
const IconStudents = getAsset("NavMenu-icon-Students");
const IconGroups = getAsset("NavMenu-icon-Groups");
const IconPlanning = getAsset("NavMenu-icon-Planning");
const IconProgress = getAsset("NavMenu-icon-Progress");
const IconLecturia = getAsset("NavMenu-icon-Lecturia");
const IconPending = getAsset("NavMenu-icon-Pending");
const IconInvitations = getAsset("NavMenu-icon-Invitations");
const IconMessages = getAsset("NavMenu-icon-Messages");
const IconChat = getAsset("NavMenu-icon-Chat");
const IconTeams = getAsset("NavMenu-icon-Teams");
const IconCatalog = getAsset("NavMenu-icon-Catalog");
const IconAvanza = getAsset("NavMenu-icon-Avanza");
const IconDashboards = getAsset("NavMenu-icon-Dashboards");
const IconAiTools = getAsset("NavMenu-icon-AiTools");
const IconMyDocuments = getAsset("NavMenu-icon-MyDocuments");
const HeaderLogo = getAsset("Header-icon-Logo");
const HeaderLogoMini = getAsset("Header-icon-Logo-Mini");

// Director Menu Icons
const IconAcademicContent = getAsset("NavMenuDirector-icon-AcademicContent");
const IconTeachers = getAsset("NavMenuDirector-icon-Teachers");
const IconCalendar = getAsset("NavMenuDirector-icon-Calendar");

// Admin Menu Icons
const IconSchools = getAsset("NavMenuAdmin-icon-Schools");
const IconSubscriptions = getAsset("NavMenuAdmin-icon-Subscriptions");
const IconUsers = getAsset("NavMenuAdmin-icon-Users");
const IconAccess = getAsset("NavMenuAdmin-icon-Access");
const IconReports = getAsset("NavMenuAdmin-icon-Reports");

const SIDEBAR_COLLAPSED_KEY = "sidebar-collapsed";

const MOBILE_QUERY = "(max-width: 992px)";

function useIsMobile() {
    const [isMobile, setIsMobile] = useState(
        () => typeof window !== "undefined" && window.matchMedia(MOBILE_QUERY).matches
    );
    useEffect(() => {
        const mql = window.matchMedia(MOBILE_QUERY);
        const onChange = (e) => setIsMobile(e.matches);
        // addListener is the deprecated form, still required by Safari < 14.
        if (mql.addEventListener) mql.addEventListener("change", onChange);
        else mql.addListener(onChange);
        setIsMobile(mql.matches);
        return () => {
            if (mql.removeEventListener) mql.removeEventListener("change", onChange);
            else mql.removeListener(onChange);
        };
    }, []);
    return isMobile;
}

const Menu = ({ collapsed, onToggleCollapse, drawerOpen = false, onCloseDrawer }) => {
    const { state } = useContext(AppContext);
    let sessionDetails = SessionManager.getSessionDetails();
    const location = useLocation();

    const isMobile = useIsMobile();

    const [internalCollapsed, setInternalCollapsed] = useState(() => {
        try {
            return localStorage.getItem(SIDEBAR_COLLAPSED_KEY) === "true";
        } catch (e) {
            return false;
        }
    });

    const isControlled = collapsed !== undefined;
    const isCollapsed = isMobile ? false : (isControlled ? collapsed : internalCollapsed);

    const toggleCollapse = () => {
        if (onToggleCollapse) onToggleCollapse();
        if (!isControlled) setInternalCollapsed((v) => !v);
    };

    useEffect(() => {
        document.body.classList.toggle("sidebar-collapsed", isCollapsed);
        if (isMobile) return;
        try {
            localStorage.setItem(SIDEBAR_COLLAPSED_KEY, String(isCollapsed));
        } catch (e) {
        }
    }, [isCollapsed, isMobile]);

    const [totalUnreadPromptAlerts, setTotalUnreadPromptAlerts] = useState(0);

    const showPromptAlertsBadge = (sessionDetails?.Roles || []).includes(Settings.ROLE_ADMIN_STR);

    useEffect(() => {
        if (!showPromptAlertsBadge) {
            return;
        }

        let cancelled = false;

        (async () => {
            try {
                const total = await fetchUnreadAlertsTotal();
                if (!cancelled) setTotalUnreadPromptAlerts(total);
            } catch (error) {
                console.error("Error fetching total unread prompt alerts:", error);
            }
        })();

        return () => { cancelled = true; };
    }, [showPromptAlertsBadge]);

    // Initialize translations based on the current version
    useVersionedTranslations();

    if (sessionDetails == null) {
        return (
            <Redirect to="/" />
        )
    }
    const isRoleAllowed = (allowRoles) => {
        let roles = sessionDetails.Roles;
        return allowRoles.some(role => roles.includes(role))
    };

    const isRoleDenied = (deniedRoles) => {
        let roles = sessionDetails.Roles;
        return !deniedRoles.some(role => roles.includes(role))
    };
    //allow or denied access to routes
    const allowStudents = [Settings.ROLE_DIRECTOR_STR, Settings.ROLE_TEACHER_STR, Settings.ROLE_PARENT_NO_CLIENT_STR, Settings.ROLE_CLIENT_STR],
        deniedInvitatios = Settings.IS_APP_MANAGED_BY_REQUIRED ? [Settings.ROLE_DIRECTOR_STR, Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR]
            : [Settings.ROLE_DIRECTOR_STR, Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR, Settings.ROLE_CLIENT_STR],
        deniedChat = [Settings.ROLE_DIRECTOR_STR, Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR],
        allowTeacherGroups = [Settings.ROLE_TEACHER_STR],
        allowAiTools = [Settings.ROLE_TEACHER_STR],
        allowPlanning = [Settings.ROLE_TEACHER_STR],
        allowPendings = [Settings.ROLE_TEACHER_STR],
        allowProgressHomeschool = [Settings.ROLE_CLIENT_STR],
        allowProgress = [Settings.ROLE_DIRECTOR_STR, Settings.ROLE_TEACHER_STR],
        allowDashboards = [Settings.ROLE_DIRECTOR_STR, Settings.ROLE_TEACHER_STR, Settings.ROLE_ADMIN_ORE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_CLIENT_STR],
        allowCalendar = [Settings.ROLE_DIRECTOR_STR],
        allowSchool = [Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR],
        allowSubscription = [Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR],
        allowUsersSearch = [Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR],
        allowAccess = [Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR],
        allowEvents = [Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR];

    const getColorMenu = (role) => {
        switch (role) {
            case Settings.ROLE_DIRECTOR_STR:
                return "wrapper-menu-director";
            case Settings.ROLE_TEACHER_STR:
                return "wrapper-menu-teacher";
            case Settings.ROLE_PARENT_NO_CLIENT_STR:
                return "wrapper-menu-parent";
            case Settings.ROLE_CLIENT_STR:
                return "wrapper-menu-homeschooler";
            case Settings.ROLE_EMPLOYEE_STR:
                return "wrapper-menu-admin";
            case Settings.ROLE_ADMIN_STR:
                return "wrapper-menu-admin";
            case Settings.ROLE_ADMIN_ORE_STR:
                if (Settings.PROJECT_VERSION.trim() === "med-rd") {
                    return "wrapper-menu-admin-ore";
                }
                return "wrapper-menu-admin";
            default:
                return "";
        }
    };

    const isActive = (...paths) => paths.some(p => location.pathname.indexOf(p) > -1);

    const handleNavClick = () => { if (onCloseDrawer) onCloseDrawer(); };

    // Clamp a raw count for the pill; null hides the badge entirely.
    const badgeValue = (count) => {
        if (!count || count <= 0) return null;
        return count > 99 ? "99+" : count;
    };

    const NavItem = ({ to, labelId, labelText, icon, iconAlt, active, iconNode, badge }) => (
        <Link
            to={to}
            className={`med-nav-item ${active ? "active" : ""}`}
            data-tip={labelText || ""}
            onClick={handleNavClick}
        >
            <span className="med-nav-tile">
                {iconNode ? iconNode : <img src={icon} alt={iconAlt || ""} />}
            </span>
            <span className="med-nav-label">
                {labelId
                    ? <Translate id={labelId} options={{ renderInnerHtml: true }} />
                    : labelText}
            </span>
            {badge != null && <em className="med-nav-badge">{badge}</em>}
        </Link>
    );

    const NavSection = ({ labelId, labelText }) => (
        <div className="med-nav-section">
            <span>
                {labelId
                    ? <Translate id={labelId} options={{ renderInnerHtml: true }} />
                    : labelText}
            </span>
        </div>
    );

    const hasAcademic =
        isRoleAllowed(allowStudents) ||
        isRoleAllowed([Settings.ROLE_DIRECTOR_STR]) ||
        isRoleAllowed(allowTeacherGroups) ||
        isRoleAllowed(allowPlanning) ||
        isRoleAllowed(allowPendings) ||
        isRoleAllowed(allowProgress) ||
        isRoleAllowed(allowProgressHomeschool) ||
        isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) ||
        isRoleAllowed(allowCalendar);

    const showMessages = !(
        (isRoleAllowed([Settings.ROLE_ADMIN_STR]) && !state.variables.MessageManagementAdmin) ||
        (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR]) && !state.variables.MessageManagementAdminOre) ||
        (isRoleAllowed([Settings.ROLE_DIRECTOR_STR]) && !state.variables.MessageManagementPrincipal) ||
        (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && !state.variables.MessageManagementParent) ||
        (isRoleAllowed([Settings.ROLE_TEACHER_STR]) && !state.variables.MessageManagementTeacher)
    );

    const showInvitations =
        (isRoleAllowed([Settings.ROLE_TEACHER_STR]) && state.variables.InvitationManagementTeacher) ||
        (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && state.variables.InvitationManagementParent);

    const showChat =
        (Settings.getChatStatus() && isRoleAllowed([Settings.ROLE_TEACHER_STR]) && state.variables.ChatManagementTeacher) ||
        (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && state.variables.ChatManagementParent) ||
        isRoleAllowed([Settings.ROLE_CLIENT_STR]);

    const showTeams =
        isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_CLIENT_STR]) &&
        state.variables.TeamMeetsManagementTeacher;

    const showAvanza = isRoleAllowed([Settings.ROLE_TEACHER_STR]) && Settings.PROJECT_HAS_AVANZA;
    const showCatalog = isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_CLIENT_STR]);
    const showCommercial =
        isRoleAllowed(allowSubscription) &&
        Settings.PROJECT_VERSION.trim() !== "msj" &&
        Settings.PROJECT_VERSION.trim() !== "med-rd";

    const hasComms = showInvitations || showChat || showMessages || showTeams;

    const hasAdmin =
        isRoleAllowed(allowSchool) ||
        isRoleAllowed(allowSubscription) ||
        isRoleAllowed(allowUsersSearch) ||
        isRoleAllowed(allowAccess) ||
        isRoleAllowed(allowEvents) ||
        showCommercial;

    const avanzaIcon = (
        <span style={{ position: 'relative', display: 'inline-block', verticalAlign: 'middle' }}>
            <img src={IconAvanza} alt="Avanza Icon" />
            {state.notifications.hasUnreadNotifications && (
                <FontAwesomeIcon
                    icon={faExclamationCircle}
                    style={{
                        position: 'absolute',
                        top: '0',
                        right: '0',
                        padding: '0',
                        color: '#d41f13',
                        backgroundColor: 'white',
                        borderRadius: '50%',
                        fontSize: '14px',
                        transform: 'translate(50%, -10%)',
                    }}
                />
            )}
        </span>
    );

    return (
        <aside
            className={`med-sidebar ${isCollapsed ? "collapsed" : ""} ${drawerOpen ? "drawer-open" : ""} ${getColorMenu(sessionDetails.Roles[0])}`}
        >
            {/* ── Head: brand + collapse toggle ── */}
            <div className="med-sidebar-head">
                {isCollapsed ? (
                    <img className="med-logo-mini" src={HeaderLogoMini} alt="Header Logo Mini" />
                ) : (
                    <div className="med-sidebar-brand">
                        <img src={HeaderLogo} alt="Header Logo" />
                    </div>
                )}
                {/* Collapse toggle is desktop-only; on mobile the burger controls the drawer. */}
                {!isMobile && (
                    <button
                        type="button"
                        className="med-collapse-btn"
                        onClick={toggleCollapse}
                        title={isCollapsed ? "Expandir menú" : "Recoger menú"}
                        aria-label={isCollapsed ? "Expandir menú" : "Recoger menú"}
                    >
                        <FontAwesomeIcon icon={isCollapsed ? faChevronRight : faChevronLeft} />
                    </button>
                )}
            </div>

            <nav className="med-sidebar-nav" aria-label="Navegación principal">

                <NavItem
                    to="/dashboard"
                    icon={IconHome}
                    iconAlt="Home icon"
                    labelId="navbar.dashboard"
                    active={isActive("/dashboard")}
                />

                {/* ── Academic ── */}
                {!isCollapsed && hasAcademic && <NavSection labelId="navbar.section-academic" />}

                {isRoleAllowed(allowStudents) && (
                    <NavItem
                        to="/profile-update"
                        icon={IconStudents}
                        iconAlt="Icon students"
                        labelId="navbar.students"
                        active={isActive("/profile-update")}
                    />
                )}

                {isRoleAllowed([Settings.ROLE_DIRECTOR_STR]) && (
                    <NavItem
                        to="/profile-teacher"
                        icon={IconTeachers}
                        iconAlt="Teachers icon"
                        labelId="navbar.teachers"
                        active={isActive("/profile-teacher")}
                    />
                )}

                {isRoleAllowed(allowTeacherGroups) && (
                    <NavItem
                        to="/teacher-groups"
                        icon={IconGroups}
                        iconAlt="Groups icon"
                        labelId="navbar.groups"
                        active={isActive("/teacher-groups")}
                    />
                )}

                {isRoleAllowed(allowPlanning) && (
                    <NavItem
                        to="/planning-select"
                        icon={IconPlanning}
                        iconAlt="Planning icon"
                        labelId="navbar.planning"
                        active={isActive("/planning-select", "/plannings", "/planning-detail", "/assignments", "/assignment-manager")}
                    />
                )}

                {isRoleAllowed(allowProgress) && (
                    <NavItem
                        to="/record"
                        icon={IconProgress}
                        iconAlt="Progress icon"
                        labelId="navbar.progress"
                        active={isActive("/record")}
                    />
                )}

                {isRoleAllowed(allowProgressHomeschool) && (
                    <NavItem
                        to="/record-homeschool"
                        icon={IconProgress}
                        iconAlt="Progress icon"
                        labelId="navbar.progress"
                        active={isActive("/record-homeschool")}
                    />
                )}

                {isRoleAllowed(allowPendings) && (
                    <NavItem
                        to="/pending-teacher-assignments"
                        icon={IconPending}
                        iconAlt="Pending assignments icon"
                        labelId="navbar.pendings"
                        active={isActive("/pending-teacher-assignments")}
                    />
                )}

                {isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && (
                    <NavItem
                        to="/progress-average"
                        icon={IconProgress}
                        iconAlt="Progress icon"
                        labelId="navbar.progress"
                        active={isActive("/progress-average")}
                    />
                )}

                {isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && (
                    <NavItem
                        to="/pending-assignments"
                        icon={IconPending}
                        iconAlt="Pending Assignments icon"
                        labelId="navbar.assignments"
                        active={isActive("/pending-assignments")}
                    />
                )}

                {isRoleAllowed([Settings.ROLE_DIRECTOR_STR]) && (
                    <NavItem
                        to="/academic-content-select"
                        icon={IconAcademicContent}
                        iconAlt="Academic content icon"
                        labelId="navbar.academic-content"
                        active={isActive("/academic-content")}
                    />
                )}

                {isRoleAllowed(allowCalendar) && (
                    <NavItem
                        to="/calendar"
                        icon={IconCalendar}
                        iconAlt="Calendar icon"
                        labelId="navbar.calendar"
                        active={isActive("/calendar")}
                    />
                )}

                {showCatalog && (
                    <NavItem
                        to="/lesson-catalog"
                        icon={IconCatalog}
                        iconAlt="Lesson catalog icon"
                        labelId="navbar.lesson-catalog"
                        active={isActive("/lesson-catalog")}
                    />
                )}

                {/* ── AI tools ── */}
                {!isCollapsed && isRoleAllowed(allowAiTools) && <NavSection labelId="navbar.section-ai" />}

                {isRoleAllowed(allowAiTools) && (
                    <NavItem
                        to="/ai-tools"
                        icon={IconAiTools}
                        iconAlt="AI tools icon"
                        labelId="navbar.ai-tools"
                        active={isActive("/ai-tools")}
                    />
                )}

                {isRoleAllowed(allowAiTools) && (
                    <NavItem
                        to="/my-documents"
                        icon={IconMyDocuments}
                        iconAlt="My documents icon"
                        labelId="navbar.my-documents"
                        active={isActive("/my-documents")}
                    />
                )}

                {isRoleAllowed(allowDashboards) && DASHBOARD_AI_MODULE_AVAILABLE && (
                    <NavItem
                        to="/metrics"
                        icon={IconLecturia}
                        iconAlt="LecturIA icon"
                        labelId="navbar.reading-module"
                        active={isActive("/metrics")}
                    />
                )}

                {showAvanza && (
                    <NavItem
                        to="/learning-gap"
                        iconNode={avanzaIcon}
                        labelId="navbar.learning-gap"
                        active={isActive("/learning-gap")}
                    />
                )}

                {/* ── Communication ── */}
                {!isCollapsed && hasComms && <NavSection labelId="navbar.section-communication" />}

                {showInvitations && (
                    <NavItem
                        to="/invitations"
                        icon={IconInvitations}
                        iconAlt="Invitations icon"
                        labelId="navbar.invitations"
                        active={isActive("/invitations")}
                    />
                )}

                {showChat && (
                    <NavItem
                        to="/chat"
                        icon={IconChat}
                        iconAlt="Chat icon"
                        labelId="navbar.chat"
                        active={isActive("/chat")}
                    />
                )}

                {showMessages && (
                    <NavItem
                        to="/messages"
                        icon={IconMessages}
                        iconAlt="Messages icon"
                        labelId="navbar.messages"
                        active={isActive("/messages")}
                    />
                )}

                {showTeams && (
                    <NavItem
                        to="/teams-signin"
                        icon={IconTeams}
                        iconAlt="Teams icon"
                        labelText="Teams"
                        active={isActive("/teams-signin")}
                    />
                )}

                {/* ── Administration ── */}
                {!isCollapsed && hasAdmin && <NavSection labelId="navbar.section-administration" />}

                {isRoleAllowed(allowSchool) && (
                    <NavItem
                        to="/school-entity-search"
                        icon={IconSchools}
                        iconAlt="Schools icon"
                        labelId="navbar.schools"
                        active={isActive("/school")}
                    />
                )}

                {isRoleAllowed(allowSubscription) && (
                    <NavItem
                        to="/curriculum-module"
                        icon={IconSubscriptions}
                        iconAlt="Curriculum icon"
                        labelId="navbar.curriculum"
                        active={isActive("/curriculum")}
                    />
                )}

                {showCommercial && (
                    <NavItem
                        to="/commercial-module"
                        icon={IconSubscriptions}
                        iconAlt="Commercial Icons"
                        labelId="navbar.commercial"
                        active={isActive("/commercial")}
                    />
                )}

                {isRoleAllowed(allowUsersSearch) && (
                    <NavItem
                        to="/users-search"
                        icon={IconUsers}
                        iconAlt="Users icon"
                        labelId="navbar.users"
                        active={isActive("user")}
                        badge={showPromptAlertsBadge ? badgeValue(totalUnreadPromptAlerts) : null}
                    />
                )}

                {isRoleAllowed(allowAccess) && (
                    <NavItem
                        to="/access"
                        icon={IconAccess}
                        iconAlt="Access icon"
                        labelId="navbar.access"
                        active={isActive("access")}
                    />
                )}

                {isRoleAllowed(allowAccess) && (
                    <NavItem
                        to="/events-search"
                        icon={IconCalendar}
                        iconAlt="Events icon"
                        labelId="navbar.events"
                        active={isActive("/events-search")}
                    />
                )}
            </nav>
        </aside>
    );
};

export default Menu;
