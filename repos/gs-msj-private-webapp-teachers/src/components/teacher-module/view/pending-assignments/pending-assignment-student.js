import React, { useState, useEffect } from "react";
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
import { useHistory, useParams } from "react-router-dom";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server';
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import PendingHomeworkDataTable from "../../datatable/pending-homework-data-table";
import PendingLessonsDataTable from "../../datatable/pending-lessons-data-table";
import "../style.css";
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";


const PendingAssignmentStudent = () => {
    const [activeTab, setActiveTab] = useState("1");
    const history = useHistory();
    const { studentId, planningId } = useParams();

    // Initialize translations based on the current version
    useVersionedTranslations();

    const goBack = () => {
        history.goBack();
    };

    const toggle = (tabNo) => {
        setActiveTab(tabNo);
    };

    return (
        <div>
            <div className="container-padding animated fadeIn">
                <Container className="container-wrapper">
                    <a className="btn-back" onClick={goBack}>
                        <FontAwesomeIcon icon={faArrowLeft} />{" "}
                        <Translate id="general.back-action" options={{ renderInnerHtml: false }} />
                    </a>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="pending-assignments.assignment" />
                            </div>
                        </div>
                    </div>
                    <Row>
                        <Col style={{ marginTop: 20 }}>
                            <Card>
                                <CardHeader>
                                    <small>
                                        <Translate id="assignments.subtitle" options={{ renderInnerHtml: false }} />
                                    </small>
                                </CardHeader>
                                <CardBody>
                                    <Row>
                                        <Col>
                                            {studentId && planningId && (
                                                <div className="margin-top-mandatory-25">
                                                    <Nav tabs>
                                                        <NavItem>
                                                            <NavLink
                                                                className={activeTab === "1" ? "tab underline-div" : "tab"}
                                                                onClick={() => toggle("1")}
                                                            >
                                                                <Translate id="pending-assignments.exams" />
                                                            </NavLink>
                                                        </NavItem>
                                                        <NavItem>
                                                            <NavLink
                                                                className={activeTab === "2" ? "tab underline-div" : "tab"}
                                                                onClick={() => toggle("2")}
                                                            >
                                                                <Translate id="assignments.homework" />
                                                            </NavLink>
                                                        </NavItem>
                                                    </Nav>
                                                    <TabContent activeTab={activeTab}>
                                                        <TabPane tabId="1">
                                                            <Card>
                                                                <CardBody>
                                                                    <PendingLessonsDataTable studentId={studentId} planningId={planningId} />
                                                                </CardBody>
                                                            </Card>
                                                        </TabPane>
                                                        <TabPane tabId="2">
                                                            <Card>
                                                                <CardBody>
                                                                    <PendingHomeworkDataTable studentId={studentId} planningId={planningId} />
                                                                </CardBody>
                                                            </Card>
                                                        </TabPane>
                                                    </TabContent>
                                                </div>
                                            )}
                                        </Col>
                                    </Row>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </Container>
            </div>
        </div>
    );
};

export default withLocalize(PendingAssignmentStudent);