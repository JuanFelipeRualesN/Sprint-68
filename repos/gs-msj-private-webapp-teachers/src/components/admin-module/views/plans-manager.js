import React, { useState, useEffect } from 'react';
import {
    Col,
    Row,
} from 'reactstrap';
import {Translate, withLocalize} from "react-localize-redux";
import FormGroupPlan from '../forms/form-group-plan';
import Settings from '../../../custom-classes/settings';
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import {Link} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowLeft} from "@fortawesome/free-solid-svg-icons";

import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * To handle commercial plans.
 * @param location
 * @returns {Element}
 * @constructor
 */
const CommercialPlanManager = ({ location }) => {

    const [didMount, setDidMount] = useState(false);
    const [isEdit] = useState(
        typeof location !== 'undefined' && location.isEdit ? location.isEdit : false
    );
    const [loading, setLoading] = useState(false);
    const [planId, setPlanId] = useState(
        typeof location !== 'undefined' && location.planId ? location.planId : null
    );
    const [propsModal, setPropsModal] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [stripeApiKey] = useState(Settings.getStripeToken());
    const [subscriptionsData, setSubscriptionsData] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        setDidMount(true);
    }, []);

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    const handlePlanCreated = (data) => {
        setPlanId(data.planId);
    };

    const state = {
        didMount,
        isEdit,
        loading,
        planId,
        propsModal,
        showModal,
        stripeApiKey,
        subscriptionsData,
    };

    // Renders the main page
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="col col-no-padding-left">
                        <Link to="/commercial-plans-search" className="btn-back">
                            <FontAwesomeIcon icon={faArrowLeft}/>{" "}
                            <Translate
                                id="general.back-action"
                                options={{renderInnerHtml: true}}
                            />
                        </Link>
                    </div>
                    <div className="col col-no-padding-left">
                        <div className="title-principal">
                            <Translate id="commercial.commercial-plans-create" options={{renderInnerHtml: true}}/>
                        </div>
                    </div>
                    <Row style={{ minHeight: 600, marginTop: 40}} className="mt-15px">
                        <Col xs={12} style={{padding: 20}}>
                            <FormGroupPlan data={state} isCommercial={true}
                                           updateParent={handlePlanCreated}/>
                        </Col>
                    </Row>
                </div>
            </div>

            {loading ? <LoadingFullWindow/> : null}
            {showModal ?
                <ModalGeneric isEdit={true} data={propsModal} updateParent={toggleAlertModal}/> : null}
        </div>
    );
};

export default withLocalize(CommercialPlanManager);