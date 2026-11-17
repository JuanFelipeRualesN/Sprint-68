import React from 'react';
import { withLocalize } from "react-localize-redux";
import FormHandleUser from "../forms/form-handle-user";
import { Link } from "react-router-dom";
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowAltCircleLeft, faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { withRouter } from "react-router-dom";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import {
    PromptSafetyAlert,
    TutorPromptHistory,
    usePromptMonitoring,
} from "../../shared-module-components/profiles-prompt-alerts";
/**
 * Modal for update a plan
 */
const UserManager = ({ history, location }) => {

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleRedirect = (userId) => {
        history.push(`/user-manager?userId=${userId}`);
    };

    // Render a generic modal. The body text should be sent in the text
    const query = new URLSearchParams(location.search);
    const userId = query.get("userId");

    // Prompt monitoring is visible only to teachers and central admins (NOT
    // admin-ore nor director). Which USERS show alerts stays data-driven (the
    // backend decides); this gate is about who is allowed to see the feature.
    const canViewPromptAlerts = isRoleAllowed(
        [Settings.ROLE_TEACHER_STR, Settings.ROLE_ADMIN_STR],
        SessionManager.getSessionDetails()?.Roles || []
    );

    // AI tutor prompt monitoring for the user being viewed.
    const promptMonitoring = usePromptMonitoring({
        enabled: !!userId && canViewPromptAlerts,
        userId,
    });

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="col col-no-padding-left">
                        <Link to="/users-search" className="btn-back">
                            <FontAwesomeIcon icon={faArrowLeft} />{" "}
                            <Translate
                                id="general.back-action"
                                options={{ renderInnerHtml: true }}
                            />
                        </Link>
                    </div>
                    <FormHandleUser
                        userId={userId}
                        isEdit={userId ? true : false}
                        handleRedirect={handleRedirect}
                    />
                    {userId && canViewPromptAlerts && (
                        <>
                            <PromptSafetyAlert count={promptMonitoring.flaggedCount} />
                            <TutorPromptHistory
                                prompts={promptMonitoring.prompts}
                                page={promptMonitoring.page}
                                totalPages={promptMonitoring.totalPages}
                                onPageChange={promptMonitoring.setPage}
                                loading={promptMonitoring.loading}
                                from={promptMonitoring.from}
                                to={promptMonitoring.to}
                                periods={promptMonitoring.periods}
                                onDateRangeChange={promptMonitoring.setDateRange}
                            />
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

export default withRouter(withLocalize(UserManager));
