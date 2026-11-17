import React, { useState, useEffect } from "react";
import { Col, Row, CardBody } from "reactstrap";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import PendingAssignmentsDataTable from "../../../shared-module-components/datatables/pending-assignments-data-table";
import Settings from "../../../../custom-classes/settings";
import { withRouter } from "react-router-dom";
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";
import SessionManager from "../../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../../utils/access_logger";

const PendingTeacherAssignments = () => {
    const userDetails = SessionManager.getSessionDetails();

    // Initialize translations based on the current version
    useVersionedTranslations();
    useEffect(() => {
        try{            
            SendAccessLog({
                Username: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "6"
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    return (
        <>
          <div className="container-padding animated delay-05s fadeIn">
              <div className="wrapper-principal-container">
                  <div className="wrapper-col borderGray-bottom block-display-responsive">
                      <div className="col col-no-padding-left">
                          <div className="title-principal">
                              <Translate id="pending-teacher-assignments.assignment-title" />
                          </div>
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
                                          <PendingAssignmentsDataTable roleId={Settings.ROLE_TEACHER_ID}/>
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

export default withRouter(withLocalize(PendingTeacherAssignments));
