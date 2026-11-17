// ---- Mobile bottom navigation (role-aware, self-contained) ----
import {useContext, useEffect, useState} from "react";
import {Link, useLocation} from "react-router-dom";
import LanguageManager from "../../../../custom-classes/language-manager";
import { LineIcon } from "../../line-icons/line-icon";
import { AppContext } from "../../../../App.Context";
import SessionManager from "../../../../custom-classes/session-manager";
import Settings from "../../../../custom-classes/settings";
import {getTotalPendingAssignmentsForAllPlans} from "../../../../api/teachers";
import {getTotalOfUnreadMessages} from "../../../../api/messages";

// Max tabs to render on the bar. Anything past this is dropped
// (or folded into a "More" tab — see note below).
const MAX_ITEMS = 5;

/* Candidate tabs in priority order. The first MAX_ITEMS whose `allow`
 * predicate passes for the current user get rendered. `allow` receives
 * { isRoleAllowed, vars } so it can check roles + feature flags exactly
 * like the sidebar does. `label` uses the same i18n keys as the sidebar;
 * only the first word is shown to keep the bar compact. `icon` names map
 * to your LineIcon set — adjust these to whatever glyphs actually exist.
 * `badgeSource` (optional) names a live count resolved at render time. */
const BOTTOM_NAV_CANDIDATES = [
    {
        key: "dashboard",
        href: "/dashboard",
        icon: "home",
        labelKey: "navbar.dashboard",
        allow: () => true, // always visible
    },
    {
        key: "teachers",
        href: "/profile-teacher",
        icon: "groups",
        labelKey: "navbar.teachers",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_DIRECTOR_STR]),
    },
    {
        key: "planning",
        href: "/planning-select",
        icon: "planning",
        labelKey: "navbar.planning",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_TEACHER_STR]),
    },
    {
        key: "pendings",
        href: "/pending-teacher-assignments",
        icon: "pending",
        labelKey: "navbar.pendings",
        badgeSource: "pendingAssignments",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_TEACHER_STR]),
    },
    {
        key: "progress",
        href: "/record",
        icon: "progress",
        labelKey: "navbar.progress",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([
                Settings.ROLE_DIRECTOR_STR,
                Settings.ROLE_TEACHER_STR,
                Settings.ROLE_CLIENT_STR,
            ]),
    },
    {
        key: "messages",
        href: "/messages",
        icon: "messages",
        labelKey: "navbar.messages",
        badgeSource: "unreadMessages",
        allow: ({ isRoleAllowed, vars }) =>
            !(
                (isRoleAllowed([Settings.ROLE_ADMIN_STR]) && !vars.MessageManagementAdmin) ||
                (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR]) && !vars.MessageManagementAdminOre) ||
                (isRoleAllowed([Settings.ROLE_DIRECTOR_STR]) && !vars.MessageManagementPrincipal) ||
                (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && !vars.MessageManagementParent) ||
                (isRoleAllowed([Settings.ROLE_TEACHER_STR]) && !vars.MessageManagementTeacher)
            ),
    },
    {
        key: "ai-tools",
        href: "/ai-module",
        icon: "aitools",
        labelKey: "navbar.ai-tools",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_TEACHER_STR]),
    },
    {
        key: "students",
        href: "/profile-update",
        icon: "students",
        labelKey: "navbar.students",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([
                Settings.ROLE_DIRECTOR_STR,
                Settings.ROLE_TEACHER_STR,
                Settings.ROLE_PARENT_NO_CLIENT_STR,
                Settings.ROLE_CLIENT_STR,
            ]),
    },
    {
        key: "groups",
        href: "/teacher-groups",
        icon: "groups",
        // label depends on a feature flag, like the sidebar
        labelKey: "navbar.groups",
        altLabelKey: "navbar.subgroups",
        useAltLabel: ({ vars }) => !vars.GroupInternalManagementTeacher,
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_TEACHER_STR]),
    },

    {
        key: "progress-parent",
        href: "/progress-average",
        icon: "chart",
        labelKey: "navbar.progress",
        allow: ({ isRoleAllowed, vars }) =>
            isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) &&
            vars.ProgressManagementParent,
    },
    {
        key: "assignments",
        href: "/pending-assignments",
        icon: "file-text",
        labelKey: "navbar.assignments",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]),
    },
    {
        key: "calendar",
        href: "/calendar",
        icon: "calendar",
        labelKey: "navbar.calendar",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_DIRECTOR_STR]),
    },
    {
        key: "chat",
        href: "/chat",
        icon: "message-square",
        labelKey: "navbar.chat",
        allow: ({ isRoleAllowed, vars }) =>
            (isRoleAllowed([Settings.ROLE_TEACHER_STR]) && vars.ChatManagementTeacher) ||
            (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR]) && vars.ChatManagementParent),
    },

    {
        key: "schools",
        href: "/school-module",
        icon: "building",
        labelKey: "navbar.schools",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([
                Settings.ROLE_EMPLOYEE_STR,
                Settings.ROLE_ADMIN_STR,
                Settings.ROLE_ADMIN_ORE_STR,
            ]),
    },
    {
        key: "subscriptions",
        href: "/subscription-module",
        icon: "credit-card",
        labelKey: "navbar.subscriptions",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR]),
    },
    {
        key: "users",
        href: "/users-search",
        icon: "search",
        labelKey: "navbar.users",
        allow: ({ isRoleAllowed }) =>
            isRoleAllowed([
                Settings.ROLE_EMPLOYEE_STR,
                Settings.ROLE_ADMIN_STR,
                Settings.ROLE_ADMIN_ORE_STR,
            ]),
    },
];

export function MobileBottomNav({ onNavigate }) {
    const { pathname } = useLocation();
    const { state } = useContext(AppContext);

    const sessionDetails = SessionManager.getSessionDetails();
    if (sessionDetails == null) return null;
    const sessionRoles = sessionDetails.Roles;
    const [totalPendingAssignments, setTotalPendingAssignments] = useState(0);
    const [totalUnreadMessages, setTotalUnreadMessages] = useState(0);

    const roles = sessionDetails.Roles || [];
    const vars = state?.variables || {};
    const isRoleAllowed = (allowRoles) => allowRoles.some((r) => roles.includes(r));

    const ctx = { isRoleAllowed, vars };
    const items = BOTTOM_NAV_CANDIDATES
        .filter((item) => item.allow(ctx))
        .slice(0, MAX_ITEMS);

    const isActive = (item) => {
        if (item.href === "/dashboard") {
            return pathname === "/" || pathname.startsWith("/dashboard");
        }
        return pathname.startsWith(item.href);
    };

    const resolveLabel = (item) => {
        const key = item.useAltLabel?.(ctx) ? item.altLabelKey : item.labelKey;
        return LanguageManager.getTranslation(key, Settings.LANGUAGES);
    };

    // Live counts keyed by the candidate's `badgeSource`.
    const badgeCounts = {
        pendingAssignments: totalPendingAssignments,
        unreadMessages: totalUnreadMessages,
    };

    const resolveBadge = (item) => {
        const count = item.badgeSource ? badgeCounts[item.badgeSource] : 0;
        return count > 0 ? count : null; // hide the badge when zero
    };

    useEffect(() => {
        if (sessionRoles.includes(Settings.ROLE_TEACHER_STR)) {
            callTotalPendingAssignments();
        }
        callUnreadMessages();
    }, []);

    const callTotalPendingAssignments = async () => {
        try {
            const response = await getTotalPendingAssignmentsForAllPlans();
            setTotalPendingAssignments(response.data.TotalPendingAssignments);
        } catch (error) {
            console.error("Error fetching total pending assignments:", error);
        }
    };

    const callUnreadMessages = async () => {
        try {
            const response = await getTotalOfUnreadMessages();
            setTotalUnreadMessages(response.data.TotalUnread);
        } catch (error) {
            console.error("Error fetching total unread messages:", error);
        }
    };

    return (
        <nav className="med-bottom-nav">
            {items.map((item) => {
                const active = isActive(item);
                const label = resolveLabel(item);
                const badge = resolveBadge(item);
                return (
                    <Link
                        key={item.key}
                        to={item.href}
                        className={`med-bottom-item ${active ? "active" : ""}`}
                        aria-current={active ? "page" : undefined}
                        onClick={() => onNavigate?.(item.href)}
                    >
                        <LineIcon name={item.icon} size={21} />
                        <span>{label.split(" ")[0]}</span>
                        {badge != null && (
                            <em className="med-bottom-badge">
                                {badge > 99 ? "99+" : badge}
                            </em>
                        )}
                    </Link>
                );
            })}
        </nav>
    );
}
