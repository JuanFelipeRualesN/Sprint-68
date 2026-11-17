/**
 * Created by portiz on 3/4/19.
 */
import React, { useState, useEffect } from 'react';
import { Redirect } from "react-router-dom";
import { Container, Col, Row} from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import SessionManager from '../../../custom-classes/session-manager';
import PlanList from '../card-list/plan-list';
import Settings from '../../../custom-classes/settings';
import LiveChat from 'react-livechat';
import * as qs from 'query-string';
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to select the plan that the client want to subscribe
 */
const PlanSelect = ({ location }) => {
    const [planSelected, setPlanSelected] = useState(null);
    const [planTypeOutsideSrc] = useState(qs.parse(location.search).planType); // to allow outside source to put the plan type
    const [termSelected, setTermSelected] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.add('landing-white'); // Custom background for students
    }, []);

    const selectPlan = (e, e2) => {
        // If the person is registered already, sent it to the old form
        // If the person is not registered yet, send him to the new form
        setPlanSelected(e);
        setTermSelected(e2);
    };

    if (!Settings.IS_APP_MANAGED_BY_REQUIRED) {
        return (<Redirect push to={{
            pathname: "/",
        }}/>);
    }

    if (planSelected !== null && SessionManager.getSession() !== null && typeof SessionManager.getSession() !== 'undefined') {
        return (<Redirect push to={{
            pathname: "/subscription-add-new",
            planId: planSelected,
            termSelected: termSelected
        }}/>);
    } else if (planSelected !== null && (SessionManager.getSession() === null || typeof SessionManager.getSession() === 'undefined')) {
        return (<Redirect push to={{
            pathname: "/payment",
            search:
                "?planId="
                + planSelected
                + "&"
                + "termSelected="
                + termSelected
        }}/>);
    }

    return (
        <div>
            <div className="container-padding card-white">
                <Container className="container-full">
                    <Row className="centered-text">
                        <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <h1 >
                                <Translate id="plan.choose-plan"/>
                            </h1>
                        </Col>
                    </Row>

                    <PlanList term={planTypeOutsideSrc} updateParent={selectPlan}/>

                    <LiveChat license={Settings.CHAT_LIVE_CHAT_LICENSE} />
                </Container>
            </div>
        </div>
    );
};

export default withLocalize(PlanSelect);