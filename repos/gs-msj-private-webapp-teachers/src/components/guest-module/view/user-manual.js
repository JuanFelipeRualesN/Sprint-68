import React, { useMemo } from 'react';
import { withLocalize, Translate } from 'react-localize-redux';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDownload, faHandsHelping, faPhoneAlt } from '@fortawesome/free-solid-svg-icons';
import Settings from '../../../custom-classes/settings';
import SessionManager from '../../../custom-classes/session-manager';
import FaqParents from '../../parent-module/faq/faq-parents';
import FaqTeachers from '../../teacher-module/faq/faq-teachers';
import FaqAdmin from '../../admin-module/faq/faq-admin';
import FaqAdminOre from '../../admin-module/faq/faq-admin-ore';
import FaqPrincipal from '../../principal-module/faq/faq-principal';
import FaqHomeschool from '../../homeschooler-module/faq/faq-homeschool';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';


/** Which FAQ and which manual each role gets */
const getRoleConfig = (role) => {
    const map = {
        [Settings.ROLE_CLIENT_STR]: { Faq: FaqHomeschool, manualUrl: null },
        [Settings.ROLE_PARENT_NO_CLIENT_STR]: { Faq: FaqParents, manualUrl: Settings.HTTP_USER_MANUAL_PARENT },
        [Settings.ROLE_DIRECTOR_STR]: { Faq: FaqPrincipal, manualUrl: Settings.HTTP_USER_MANUAL_DIRECTOR },
        [Settings.ROLE_TEACHER_STR]: { Faq: FaqTeachers, manualUrl: Settings.HTTP_USER_MANUAL_TEACHERS },
        [Settings.ROLE_ADMIN_ORE_STR]: { Faq: FaqAdminOre, manualUrl: Settings.HTTP_USER_MANUAL_ADMIN_ORE },
        [Settings.ROLE_ADMIN_STR]: { Faq: FaqAdmin, manualUrl: Settings.HTTP_USER_MANUAL_ADMIN },
    };
    return role != null ? map[role] || null : null;
};

/**
 * Help page: customer service contact, user manual download and role-specific FAQ
 */
const UserManual = () => {
    useVersionedTranslations();

    const userRole = SessionManager.getUserRole();
    const roleConfig = useMemo(() => getRoleConfig(userRole), [userRole]);

    // Hide the user manual download link for the MED projects (all roles)
    const isMedProject = Settings.PROJECT_VERSION.trim() === 'med-rd' || Settings.IS_MED_QUINDIO;
    const showManual = !isMedProject && roleConfig?.manualUrl;

    const FaqComponent = roleConfig?.Faq;

    return (
        <div className="container-padding animated delay-05s fadeIn">
            <div className="wrapper-principal-container user-help">

                {/* Header */}
                <header className="user-help__header">
                    <div className="user-help__heading">
                        <span className="title-principal">
                            <FontAwesomeIcon icon={faHandsHelping} />
                        </span>
                        <h2 className="title-principal">
                            <Translate id="help.help" options={{ renderInnerHtml: true }} />
                        </h2>
                    </div>

                    {showManual && (
                        <a
                            href={roleConfig.manualUrl}
                            download
                            target="_blank"
                            rel="noopener noreferrer"
                            className="user-help__download"
                        >
                            <FontAwesomeIcon icon={faDownload} />
                            <span>
                                <Translate id="faq.to-download-the-user-manual-press-here" options={{ renderInnerHtml: true }} />
                            </span>
                        </a>
                    )}
                </header>

                {/* Support contact */}
                <div className="user-help__support">
                    <span className="user-help__support-icon">
                        <FontAwesomeIcon icon={faPhoneAlt} />
                    </span>
                    <div className="user-help__support-text">
                        <Translate id="user-help.customer-service" options={{ renderInnerHtml: true }} />
                        <a href={`tel:1-${Settings.PHONE_NUMBER}`} className="user-help__phone">
                            {Settings.PHONE_NUMBER}
                        </a>
                    </div>
                </div>

                {/* FAQ */}
                {FaqComponent && (
                    <section className="user-help__faq">
                        <FaqComponent />
                    </section>
                )}
            </div>
        </div>
    );
};

export default withLocalize(UserManual);
