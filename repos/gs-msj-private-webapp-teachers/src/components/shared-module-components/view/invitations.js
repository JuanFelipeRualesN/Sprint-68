import React, { useEffect, useState } from 'react';
import {CardBody, Col, Nav, NavItem, Row, TabContent, TabPane} from 'reactstrap';
import {withLocalize} from "react-localize-redux";
import {Translate} from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {
    faUsers,
    faUserCheck
} from '@fortawesome/free-solid-svg-icons';
import SessionManager from "../../../custom-classes/session-manager";
import ProfileConnectionRequestsByEmailDataTable from "../datatables/profile-connection-requests-by-email-data-table";
import ProfileConnectionsDataTable from "../datatables/profile-connections-data-table";
import ProfileConnectionRequestsDataTable from "../datatables/profile-connection-requests-data-table";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import "./style.css";
import { SendAccessLog } from '../../../utils/access_logger';
/**
 * The component (window) to handle the invitations
 */
const Invitations = (props) => {
    const [activeTab, setActiveTab] = useState("1");
    const [roles] = useState(SessionManager.getSessionDetails()?.Roles || null);
    const userDetails = SessionManager.getSessionDetails();

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        // Evaluate the resize, to show the proper navbar menu style
        document.body.classList.remove('landing-white');
        try{            
            SendAccessLog({
                Username: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "7"
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    const toggle = (tabNo) => {
        setActiveTab(tabNo);
    };

    /**
     * Renders the main dashboard
     * @returns {XML}
     */
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="connections.invitations" /></div>
                        </div>
                        <div className="col txt-right"></div>
                    </div>

                    <Row>
                        <Col xs={12}>
                            <div>
                                <CardBody>
                                    <Row>
                                        <Col xs={{ size: 12 }}>
                                            <div className="mt--5 tabs-strech danger-btn-simple table-no-margin-top clear-min-height invitation-btn clear-hr td-responsive special-top-space">
                                                <Nav tabs>
                                                    <NavItem className="txt-ellipsis">
                                                        <nav
                                                            className={activeTab === '1' ? "tab underline-div nav-link" : "tab nav-link"}
                                                            onClick={() => toggle('1')}
                                                        >
                                                            <FontAwesomeIcon icon={faUsers} />{' '}
                                                            <Translate id="connections.my-connections" />
                                                        </nav>
                                                    </NavItem>
                                                    <NavItem className="txt-ellipsis">
                                                        <nav
                                                            className={activeTab === '2' ? "tab underline-div nav-link" : "tab nav-link"}
                                                            onClick={() => toggle('2')}
                                                        >
                                                            <FontAwesomeIcon icon={faUserCheck} />{' '}
                                                            <Translate id="connections.my-invitations" />
                                                        </nav>
                                                    </NavItem>
                                                    {/*
                            this.state.roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 ?
                              <NavItem className="txt-ellipsis">
                                <NavLink
                                  className={activeTab === '3' ? "tab underline-div" : "tab"}
                                  onClick={() => toggle('3')}
                                >
                                  <FontAwesomeIcon icon={faUserFriends} />{' '}
                                  <Translate id="connections.parents-invitations" />
                                </NavLink>
                              </NavItem> : null
                          */}
                                                </Nav>
                                                <TabContent activeTab={activeTab}>
                                                    <TabPane tabId="1">
                                                        <div>
                                                            <CardBody>
                                                                {activeTab === '1' ? <ProfileConnectionsDataTable /> : null}
                                                            </CardBody>
                                                        </div>
                                                    </TabPane>
                                                    <TabPane tabId="2">
                                                        <div>
                                                            <CardBody>
                                                                {activeTab === '2' ? <ProfileConnectionRequestsDataTable /> : null}
                                                            </CardBody>
                                                        </div>
                                                    </TabPane>
                                                    {/*
                            this.state.roles.indexOf(Settings.ROLE_TEACHER_STR) > -1 ?
                              <TabPane tabId="3">
                                <div>
                                  <CardBody>
                                    {activeTab === '3' ? <ProfileConnectionRequestsByEmailDataTable /> : null}
                                  </CardBody>
                                </div>
                              </TabPane> : null
                          */}
                                                </TabContent>
                                            </div>
                                        </Col>
                                    </Row>
                                </CardBody>
                            </div>
                        </Col>
                    </Row>
                </div>
            </div>
        </div>
    );
};

export default withLocalize(Invitations);