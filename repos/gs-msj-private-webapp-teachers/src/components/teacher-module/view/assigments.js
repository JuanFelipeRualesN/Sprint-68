import React, { useEffect, useState } from "react";
import { CardBody, Col, Row, Button } from "reactstrap";
import { Link, Redirect } from "react-router-dom";

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPlus,
  faArrowLeft,
} from "@fortawesome/free-solid-svg-icons";
import TeacherQuizzesDataTable from "../datatable/teacher-quizzes-data-table";
import "./style.css";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import SessionManager from "../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../utils/access_logger";

/**
 * The component (window) to handle the client dashboard
 */
const Assignments = () => {
  const userDetails = SessionManager.getSessionDetails();
  const [goToAssignmentManager, setGoToAssignmentManager] = useState(false);
  const [selectedQuiz, setSelectedQuiz] = useState(null);

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    document.body.classList.remove("landing-white");

    try{        
      SendAccessLog({
        UserName: userDetails.Username,
        InvitationCode: userDetails.InvitationCode,
        Role: userDetails.Roles[0],
        AreaId: "4",
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  }, []);

  const handleGoToQuizNew = () => {
    setSelectedQuiz(null);
    setGoToAssignmentManager(true);
  };

  const handleSelectedQuiz = (e, type) => {
    setSelectedQuiz(e);
    setGoToAssignmentManager(true);
  };

  if (goToAssignmentManager) {
    return (
        <Redirect
            to={{
              pathname: "/assignment-manager",
              quizId: selectedQuiz,
            }}
        />
    );
  }

  return (
      <div>
        <div className="container-padding animated delay-05s fadeIn">
          <div className="wrapper-principal-container">
            <div className="wrapper-col wrapper-col--flex-end borderGray-bottom mb-3 block-display-responsive">
              <div className="col col-no-padding-left">
                <Link to="/planning-select" className="btn-back">
                  <FontAwesomeIcon icon={faArrowLeft} />{" "}
                  <Translate
                      id="general.back-action"
                      options={{ renderInnerHtml: true }}
                  />
                </Link>
                <div className="title-principal mt-3">
                  <Translate
                      id="planning.assignments"
                      options={{ renderInnerHtml: true }}
                  />
                </div>
              </div>
              <div className="col txt-right">
                <Button className="btn primary-btn" onClick={handleGoToQuizNew}>
                  <FontAwesomeIcon icon={faPlus} />{" "}
                  <Translate
                      id="planning.new-assignment"
                      options={{ renderInnerHtml: true }}
                  />
                </Button>
              </div>
            </div>
            <Row style={{ minHeight: 600 }}>
              <Col xs={12}>
                <div className="animated delay-05s fadeIn danger-btn-simple space-btn-icon secondary-btn-simple td-responsive-message syle-date-three">
                  <CardBody>
                    <TeacherQuizzesDataTable updateParent={handleSelectedQuiz} />
                  </CardBody>
                </div>
              </Col>
            </Row>
          </div>
        </div>
      </div>
  );
};

export default withLocalize(Assignments);
