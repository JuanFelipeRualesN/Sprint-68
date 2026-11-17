import React, { useState, useEffect } from 'react';
import {
    Col,
    Row,
    CardBody,
} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import ProfilesDataTable from "../../../shared-module-components/datatables/profiles-data-table";
import Settings from "../../../../custom-classes/settings";
import { withRouter } from 'react-router-dom';
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';

const StudentProgress = ({ history }) => {
    const [isStaff, setIsStaff] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleStudentSelected = (selected) => {
        const studentId = selected.UserId;
        history.push(`/progress-average-student/${studentId}`);
    };

    return (
        <>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="progress.record-student" /></div>
                        </div>
                    </div>
                    <Row style={{ minHeight: 600 }}>
                        <Col
                            sm={{ size: 12, offset: 0 }}
                            lg={{ size: 12, offset: 0 }}
                            md={{ size: 12, offset: 0 }}
                        >
                            <div>
                                <CardBody className="clear-padding-card">
                                    <Row className="td-w-rollover td-responsive special-top-space syle-date-five">
                                        <Col
                                            sm={{ size: 12 }}
                                            md={{ size: 12 }}
                                            lg={{ size: 12 }}
                                        >
                                            <p>
                                                <b>
                                                    <Translate
                                                        id="progress.select-student"
                                                        options={{ renderInnerHtml: true }}
                                                    />
                                                </b>
                                            </p>
                                            <ProfilesDataTable
                                                isStaff={isStaff}
                                                roleId={Settings.ROLE_STUDENT_ID}
                                                updateParent={handleStudentSelected}
                                            />
                                        </Col>
                                    </Row>
                                </CardBody>
                            </div>
                        </Col>
                    </Row>
                </div>
            </div>
        </>
    );
};

export default withRouter(withLocalize(StudentProgress));