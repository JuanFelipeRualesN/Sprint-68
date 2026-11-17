/**
 * Created by portiz on 05/07/2020.
 */
import React, { useState, useEffect } from 'react';
import { CardBody, Col, Container, Row, Button } from 'reactstrap';
import { Link, Redirect } from "react-router-dom";

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faCalendarAlt,
    faArrowAltCircleLeft,
    faPlus, faGraduationCap, faExclamationCircle, faArrowLeft,
} from '@fortawesome/free-solid-svg-icons';
import PlanningDataTable from "../datatable/planning-data-table";
import SessionManager from "../../../custom-classes/session-manager";
import ModalTeacherPlanningNameCreator from "../modal/modal-teacher-planning-name-creator";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the teacher plannings
 */
const Plannings = () => {
    const [planningId, setPlanningId] = useState(null);
    const [planningName, setPlanningName] = useState('');
    const [planningClosed, setPlanningClosed] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [roles, setRoles] = useState(SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles);
    const [selectedQuiz, setSelectedQuiz] = useState(null);
    const [sessionValid, setSessionValid] = useState(true);
    const [showNewPlanningModalWindow, setShowNewPlanningModalWindow] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    /** Evaluate the resize, to show the proper navbar menu style*/
    useEffect(() => {
        document.body.classList.remove('landing-white');
    }, []);

    const handleSelectedPlanning = (e, e2, e3) => {
        setPlanningId(e);
        setPlanningName(e2);
        setPlanningClosed(e3);
    };

    const closeNewPlanningModal = () => {
        setShowNewPlanningModalWindow(false);
        setRedoSearch(true);
    };

    const openNewPlanningModal = () => {
        setShowNewPlanningModalWindow(true);
        setRedoSearch(false);
    };

    if (planningId !== null) {
        SessionManager.setSelectedPlanning(planningId, planningName);

        return (<Redirect push to={{
            pathname: "/planning-detail",
        }} />);
    }

    // Renders the main dashboard
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col wrapper-col--flex-end borderGray-bottom mb-3 block-display-responsive">
                        <div className="col col-no-padding-left">
                            <Link to="/planning-select" className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft} />{' '}
                                <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                            </Link>
                            <div className="title-principal mt-3">
                                <Translate id="planning.planning-instructional" options={{ renderInnerHtml: true }} />
                            </div>
                        </div>
                        <div className="col txt-right">
                            <button onClick={openNewPlanningModal} className="btn-default button-primary-custom">
                                <FontAwesomeIcon icon={faPlus} />{' '}
                                <Translate id="planning.new" options={{ renderInnerHtml: true }} />
                            </button>
                        </div>
                    </div>
                    <div className="section-description mb-3">
                        <div className="wrapper-average-progress">
                            <div className="wrapper-average-progress__note">
                                <small> <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />{" "}
                                    <Translate id="planning.planning-closing-notes" options={{ renderInnerHtml: true }} />
                                </small>
                            </div>
                        </div>
                        <small><Translate id="planning.planning-required-subtitle" options={{ renderInnerHtml: true }} /></small>
                    </div>

                    {/* <Row>
                        <Col xs={{size:12}}>
                            <h2>
                                <FontAwesomeIcon icon={faCalendarAlt}/>{' '}
                                <span>
                            <Translate id="planning.planning" options={{renderInnerHtml: true}}/>
                            </span>
                            </h2>
                            <small>
                                <Translate id="planning.my-content-subtitle" options={{renderInnerHtml: true}}/>
                            </small>
                        </Col>
                        <Col xs={{size:12}}>
                            <Link to="/planning-select" className="btn btn-warning">
                                <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>
                        </Col>
                    </Row> */}
                    {/* <hr/> */}
                    {/* <Row>
                        <Col lg={{size: 2, offset: 10}} md={{size: 2, offset: 10}} sm={{size: 12}} xs={{size: 12}} className="text-right">
                            <Button onClick={openNewPlanningModal} className="btn btn-success">
                                <FontAwesomeIcon icon={faPlus}/>{' '}
                                <Translate id="planning.new" options={{renderInnerHtml: true}}/>
                            </Button>
                        </Col>
                    </Row> */}
                    <Row style={{ minHeight: 600 }}>
                        <Col xs={12}>
                            <div body className="animated delay-05s fadeIn danger-btn-simple space-btn-icon secondary-btn-simple td-responsive td-inline-3btn inline-table-4">
                                <CardBody>
                                    <PlanningDataTable redoSearch={redoSearch} updateParent={handleSelectedPlanning} />
                                </CardBody>
                            </div>
                        </Col>
                    </Row>
                </div>
            </div>

            {showNewPlanningModalWindow && <ModalTeacherPlanningNameCreator visible={showNewPlanningModalWindow} updateParent={closeNewPlanningModal} />}
        </div>
    );
};

export default withLocalize(Plannings);