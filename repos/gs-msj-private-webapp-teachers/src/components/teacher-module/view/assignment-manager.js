/**
 * Created by portiz on 05/06/2020.
 */
import React, { useState, useEffect, useRef } from 'react';
import { Col, Container, Row} from 'reactstrap';
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import SessionManager from "../../../custom-classes/session-manager";
import Quiz from "../../quiz-creator/quiz";

import { LocalizeProvider } from "react-localize-redux";
import { Provider } from 'react-redux';
import store from '../../../redux/store';
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import AlertModal from "../../quiz-creator/common/alert-modal";
import AIPanel from "../ai-assistant-chat/components/ai-panel";

/**
 * The component (window) to handle the planning selection
 */
const AssignmentManager = ({ location }) => {
    const [roles] = useState(
        SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles
    );
    const quizRef = useRef(null);
    const [quizId] = useState(
        typeof location !== 'undefined' && location !== null && typeof location.quizId !== 'undefined' && location.quizId !== null ? location.quizId : null
    );

    function handleAIFill(data) {
        quizRef.current?.fillFromAI(data);
    }

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        document.body.classList.remove('landing-white');
    }, []);
    // Renders the main dashboard
    return (
        <div>
            <AlertModal />
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col wrapper-col--flex-end borderGray-bottom mb-3 block-display-responsive">
                        <div className="col col-no-padding-left">
                            {/* <Link to="/assigments" className="btn-back">
                                <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link> */}
                            <div className="title-principal mt-3"><Translate id="quiz.quiz-creator" options={{renderInnerHtml: true}}/></div>
                        </div>
                    </div>
                    {/* <Row>
                        <Col xs={{size:12}}>
                            <h2>
                                <FontAwesomeIcon icon={faCalendarAlt}/>{' '}
                                <span>
                            <Translate id="quiz.quiz-creator" options={{renderInnerHtml: true}}/>
                            </span>
                            </h2>
                        </Col>
                    </Row> */}
                    <br/>
                    <Row className="quiz-creator--newstyle">
                        <Col xs={12}>
                            <Provider store={store}>
                                <LocalizeProvider store={store}>
                                    {quizId ?
                                        <Quiz ref={quizRef} quizId={quizId} /> : <Quiz ref={quizRef}/>}
                                </LocalizeProvider>
                            </Provider>
                        </Col>
                    </Row>
                    <AIPanel
                        mode="assignment"
                        onFill={handleAIFill}
                        onActivity={() => quizRef.current?.postponeAutoSave()}
                        getFormContext={() => quizRef.current?.getFormContext()}
                    />
                </div>
            </div>

        </div>
    );
};

export default withLocalize(AssignmentManager);