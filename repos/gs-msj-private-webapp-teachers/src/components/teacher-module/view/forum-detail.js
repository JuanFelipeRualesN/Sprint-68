/**
 * Created by portiz on 12/17/2020.
 */
import React, { useState, useEffect } from 'react';
import {Col, Container, Row} from 'reactstrap';
import {Link, Redirect} from "react-router-dom";

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import "./style.css";
import SessionManager from "../../../custom-classes/session-manager";
import ForumCreationAndView from '../forum/forum-creation-view-handle';
import {
    faArrowAltCircleLeft, faArrowLeft
} from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to handle the forum detail
 */
const ForumDetail = () => {
    const teacherPlan = SessionManager.getSelectedPlanning();
    const [forumId] = useState(SessionManager.getSelectedForum());
    const [teacherPlanId] = useState(teacherPlan !== null ? teacherPlan.planningId : null);
    const [disabled] = useState(teacherPlan !== null ? teacherPlan.planningClosed : false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Renders the main dashboard
    if(typeof forumId === 'undefined' || forumId == null) {
        return (<Redirect to={{
            pathname: "/v2/plannings"
        }}/>);
    }

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <Row>
                        <Col xs={12}>
                            <Link to={"/planning-detail"} className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>

                        </Col>
                    </Row>

                    <Row className="mt-4">
                        <Col xs={12}>
                            <ForumCreationAndView 
                                forumId={forumId} 
                                teacherPlanId={teacherPlanId} 
                                isCreation={false} 
                                disabled={disabled}
                            />
                        </Col>
                    </Row>
                </div>
            </div>

        </div>
    );
};

export default withLocalize(ForumDetail);