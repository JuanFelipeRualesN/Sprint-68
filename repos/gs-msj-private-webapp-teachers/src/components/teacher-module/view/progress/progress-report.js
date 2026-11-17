import React, { useState, useEffect } from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { CardBody, Col, Nav, NavItem, NavLink, Row, TabContent, TabPane } from 'reactstrap';
import {
    faArrowAltCircleLeft, faArrowLeft,
} from '@fortawesome/free-solid-svg-icons';
import { Translate } from "react-localize-redux";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import { renderToStaticMarkup } from "react-dom/server";
import { withRouter } from 'react-router-dom';
import { withLocalize } from "react-localize-redux";
import Progress from './progress';
import Scores from './scores';
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';

const ProgressReport = ({ match, history }) => {
    const [loading, setLoading] = useState(false);
    const [activeTab, setActiveTab] = useState("1");

    // Initialize translations based on the current version
    useVersionedTranslations();

    const goBack = () => {
        history.goBack();
    };

    const toggle = (tabNo) => {
        setActiveTab(tabNo);
    };

    const setLoadingState = (loading) => {
        setLoading(loading);
    };

    const studentId = match.params.studentId;

    return (
        <>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <a className="btn-back" onClick={goBack}>
                        <FontAwesomeIcon icon={faArrowLeft} />{" "}
                        <Translate
                            id="general.back-action"
                            options={{ renderInnerHtml: false }}
                        />
                    </a>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="progress-average.progress-title" /></div>
                        </div>
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
                                                        <NavLink
                                                            className={activeTab === '1' ? "tab underline-div" : "tab"}
                                                            onClick={() => { toggle('1'); }}
                                                        >
                                                            <Translate id="progress-average.tab-progress" />
                                                        </NavLink>

                                                    </NavItem>
                                                    {/*(<NavItem className="txt-ellipsis">
                                                        <NavLink
                                                            className={activeTab === '2' ? "tab underline-div" : "tab"}
                                                            onClick={() => { toggle('2'); }}
                                                        >
                                                            <Translate id="progress-average.tab-score" />
                                                        </NavLink>
                                                    </NavItem> */}
                                                </Nav>
                                                <TabContent activeTab={activeTab}>
                                                    <TabPane tabId="1">
                                                        <div>
                                                            <CardBody>
                                                                {activeTab === '1' && <Progress studentId={studentId} setLoading={setLoadingState} />}
                                                            </CardBody>
                                                        </div>
                                                    </TabPane>
                                                    <TabPane tabId="2">
                                                        <div>
                                                            <CardBody>
                                                                {activeTab === '2' && <Scores studentId={studentId} setLoading={setLoadingState} />}
                                                            </CardBody>
                                                        </div>
                                                    </TabPane>
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
            {loading ? <LoadingFullWindow /> : null}
        </>
    );
};

export default withRouter(withLocalize(ProgressReport));