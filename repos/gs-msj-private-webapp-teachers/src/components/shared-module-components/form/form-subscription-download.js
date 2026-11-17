/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';import {
    Container,
    Row,
    Col,
} from 'reactstrap';
import {Link} from "react-router-dom";

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faCheckCircle} from '@fortawesome/free-solid-svg-icons';
import PlatformDownloadList from '../../guest-module/item-list/platform-download-list';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

// Component for subscriptions
const FormSubscriptionDownload = ({data, updateParent}) => {
  const [didMountState, setDidMountState] = useState(false);
  const [mobileState, setMobileState] = useState(data?.mobile);
  const [planIdState, setPlanIdState] = useState(null);
  const [propsModalState, setPropsModalState] = useState(null);
  const [showErrorModalState, setShowErrorModalState] = useState(false);
  const [userSubscribedState, setUserSubscribedState] = useState(false);
  const [validPlanState, setValidPlanState] = useState(true); // default
  const [subscriptionIdState, setSubscriptionIdState] = useState(null);

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    if (typeof data !== "undefined" && data.didMount) {
      setMobileState(data?.mobile);
    }
  }, [data]);

  const saveSubscriptionState = (subscriptionState) => {
    setUserSubscribedState(subscriptionState.userSubscribed);
    setSubscriptionIdState(subscriptionState.subscriptionId);
    
    const state = {
      didMount: didMountState,
      mobile: mobileState,
      planId: planIdState,
      propsModal: propsModalState,
      showErrorModal: showErrorModalState,
      userSubscribed: subscriptionState.userSubscribed,
      validPlan: validPlanState,
      subscriptionId: subscriptionState.subscriptionId
    };
    
    updateParent(state);
  };

  return (
    <div>
      <div className="container-padding animated delay-05s fadeIn">
        <Container className="container-wrapper">
          <Row>
            <Col xs={{ size: 4, offset: 4 }} lg={{ size: 2, offset: 5 }} />
          </Row>
          <Row className="text-center white-text">
            <Col className="text-center">
              <h2>
                <FontAwesomeIcon className="fa-1x" icon={faCheckCircle} />{' '}
                <Translate id="subscription-steps.step-4" options={{ renderInnerHtml: true }} />
              </h2>
              <small>
                <Translate
                  id="subscription-steps.step-4-subtitle"
                  options={{ renderInnerHtml: true }}
                />
              </small>
              <hr />
            </Col>
          </Row>
          <Row>
            <Col sm={{ size: 12, offset: 0 }} md={{ size: 12, offset: 0 }} lg={{ size: 12, offset: 0 }}>
              <PlatformDownloadList />
            </Col>
          </Row>
          <Row className="text-center white-text">
            <Col sm={{ size: 12, offset: 0 }} md={{ size: 12, offset: 0 }} lg={{ size: 12, offset: 0 }}>
              <Link className="btn btn-imp btn-danger btn-block " to="/dashboard">
                <Translate id="subscription-steps.or-go-to-dashboard" options={{ renderInnerHtml: true }} />
              </Link>
            </Col>
            <Col sm={{ size: 12, offset: 0 }} md={{ size: 12, offset: 0 }} lg={{ size: 12, offset: 0 }}>
              <Translate id="subscription-steps.go-to-dashboard-title" options={{ renderInnerHtml: true }} />
            </Col>
          </Row>
        </Container>
      </div>
    </div>
  );
};

export default withLocalize(FormSubscriptionDownload);