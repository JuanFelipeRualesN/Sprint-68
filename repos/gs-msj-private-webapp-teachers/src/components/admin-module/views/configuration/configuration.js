import React, { useState, useEffect, useContext } from 'react';
import {
    Col,
    Card,
    CardBody,
    CardHeader,
    Container,
    Row,
    Nav,
    NavItem,
    NavLink,
    TabContent, TabPane
} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { withRouter } from 'react-router-dom';
import { renderToStaticMarkup } from 'react-dom/server';
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowAltCircleLeft, faTasks, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { Link } from "react-router-dom";
import UpdateInformation from "../../../shared-module-components/view/update-information";
import AccordionDetail from './accordion';
import AccordionAiQuotasSettings from './ai-module/accordion-ai-quotas-settings';
import AccordionAiQuotaLimits from './ai-module/accordion-ai-quota-limits';
import AccordionAiQuotaUsage from './ai-module/accordion-ai-quota-usage';
import Settings from "../../../../custom-classes/settings";
import SessionManager from "../../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../../utils/utils";
import { AppContext } from "../../../../App.Context";
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';

const ConfigurationPage = ({ history }) => {
    const context = useContext(AppContext);
    
    const [activeTab, setActiveTab] = useState("1");
    const [languages] = useState(Settings.LANGUAGES);
    const [quotasEnabled, setQuotasEnabled] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        const sessionDetails = SessionManager.getSessionDetails();
        if ((isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.ConfigurationManagerAdminOre && context.state.variables.PasswordManagementAdminOre) || (isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles) && context.state.variables.ConfigurationManagerPrincipal && context.state.variables.PasswordManagementPrincipal) || (isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && context.state.variables.ConfigurationManagerTeacher && context.state.variables.PasswordManagementTeacher)) {
            toggle('2');
        }
    }, [context.state.variables]);

    const toggle = (tabNo) => {
        setActiveTab(tabNo);
    };

    const goBack = () => {
        history.goBack();
    };

    const sessionDetails = SessionManager.getSessionDetails();

    return (
        <div>
            <div className="container-padding animated fadeIn tabs-configure">
                <div className='wrapper-principal-container'>
                    <a className="btn-back" onClick={goBack}>
                        <FontAwesomeIcon icon={faArrowLeft} />{" "}
                        <Translate
                            id="general.back-action"
                            options={{ renderInnerHtml: false }}
                        />
                    </a>
                    <Row className="mt-2">
                        <Col sm={{ size: 12, offset: 0 }} lg={{ size: 12, offset: 0 }} md={{ size: 12, offset: 0 }}>
                            <CardBody>
                                <Row>
                                    <Col xs={{ size: 12 }}>
                                            <div>
                                                <Nav tabs>
                                                    <NavItem>
                                                        {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.PasswordManagementAdmin)
                                                                || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.PasswordManagementAdminOre)
                                                                || (isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles) && context.state.variables.PasswordManagementPrincipal)
                                                                || (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR], sessionDetails.Roles) && context.state.variables.PasswordManagementParent)
                                                                || (isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && context.state.variables.PasswordManagementTeacher)
                                                                || (isRoleAllowed([Settings.ROLE_CLIENT_STR], sessionDetails.Roles))
                                                            )
                                                            && <NavLink
                                                                className={activeTab === '1' ? "tab underline-div" : "tab"}
                                                                onClick={() => { toggle('1'); }}
                                                            >
                                                                <Translate id="module-settings.tab-information" />
                                                            </NavLink>}
                                                    </NavItem>
                                                    <NavItem>
                                                        {(isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)
                                                            && context.state.variables.ConfigurationManagerAdmin) && <NavLink
                                                            className={activeTab === '2' ? "tab underline-div" : "tab"}
                                                            onClick={() => { toggle('2'); }}
                                                        >
                                                            <Translate id="module-settings.tab-config" />
                                                        </NavLink>}
                                                    </NavItem>
                                                    <NavItem>
                                                        {(isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)
                                                            && context.state.variables.ConfigurationManagerAdmin) && <NavLink
                                                            className={activeTab === '3' ? "tab underline-div" : "tab"}
                                                            onClick={() => { toggle('3'); }}
                                                        >
                                                            <Translate id="module-settings.tab-ai-configuration" />
                                                        </NavLink>}
                                                    </NavItem>

                                                </Nav>
                                                <TabContent activeTab={activeTab}>
                                                    <TabPane tabId="1">
                                                        <CardBody>
                                                            {activeTab === '1' && <UpdateInformation />}
                                                        </CardBody>
                                                    </TabPane>
                                                    {(isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)
                                                            && context.state.variables.ConfigurationManagerAdmin)
                                                        && <TabPane tabId="2">
                                                            <CardBody>
                                                                {activeTab === '2' && <AccordionDetail />}
                                                            </CardBody>
                                                        </TabPane>}
                                                    {(isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)
                                                            && context.state.variables.ConfigurationManagerAdmin)
                                                        && <TabPane tabId="3">
                                                            <CardBody>
                                                                {activeTab === '3' && <>
                                                                    <AccordionAiQuotasSettings onQuotasEnabledChange={setQuotasEnabled} />
                                                                    <AccordionAiQuotaLimits quotasEnabledOverride={quotasEnabled} />
                                                                    <AccordionAiQuotaUsage />
                                                                </>}
                                                            </CardBody>
                                                        </TabPane>}
                                                </TabContent>
                                            </div>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Col>
                    </Row>
                </div>

            </div>
        </div>
    );
};

export default withRouter(withLocalize(ConfigurationPage));