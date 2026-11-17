import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Row,
    CardBody,
    Form,
} from 'reactstrap';
import {Link} from "react-router-dom";
import {withLocalize} from "react-localize-redux";
import {Translate} from "react-localize-redux";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import PlanDataTable from '../datatables/plan-data-table';
import ModalPlans from '../modals/modal-plans';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import {renderToStaticMarkup} from "react-dom/server";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowLeft} from "@fortawesome/free-solid-svg-icons";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

// This is the item list
const PlansSearch = () => {
    const [isEdit, setIsEdit] = useState(true);
    const [loading, setLoading] = useState(false);
    const [planId, setPlanId] = useState(null);
    const [propsModal, setPropsModal] = useState(null);
    const [redoSearch, setRedoSearch] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [showPlansModal, setShowPlansModal] = useState(false);
    
    // Initialize translations based on the current version
    useVersionedTranslations();

    const handlePlanSelection = (e) => {
        setPlanId(e.Id);
        setShowPlansModal(true);
    };

    // Toggle alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Toggle the update plan modal
    const toggleUpdateModal = (e) => {
        setShowPlansModal(e);
        setRedoSearch(true);
    };

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="col col-no-padding-left">
                        <Link to="/commercial-module" className="btn-back">
                            <FontAwesomeIcon icon={faArrowLeft}/>{" "}
                            <Translate
                                id="general.back-action"
                                options={{renderInnerHtml: true}}
                            />
                        </Link>
                    </div>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="commercial.commercial-plans"/>
                            </div>
                        </div>
                        <div className="col txt-right">
                            <Link className="btn-default button-primary-custom" to="/commercial-plans-manager">
                                <Translate id="plan-creator.add-new-plan"/>

                            </Link>
                        </div>
                    </div>

                    <Row className="clear-style-card td-responsive">
                        <Col xs={12}>
                            <Card>

                                {/* <CardHeader  className="text-right">
                                    <Link className="btn-default button-primary-custom" to="/plans-manager">
                                        <Translate id="plan-creator.add-new-plan"/>
                                    </Link>
                                </CardHeader> */}
                                <CardBody>
                                    <Form>
                                        <PlanDataTable
                                            isCommercial={true}
                                            redoSearch={redoSearch}
                                            updateParent={handlePlanSelection}
                                        />
                                    </Form>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                    <hr/>
                </div>
            </div>

            {loading && <LoadingFullWindow/>}

            {showModal && (
                <ModalGeneric
                    data={propsModal}
                    updateParent={toggleAlertModal}
                />
            )}

            {showPlansModal && (
                <ModalPlans
                    planId={planId}
                    isEdit={isEdit}
                    visible={showPlansModal} 
                    updateParent={toggleUpdateModal} 
                />
            )}
        </div>
    );
};

export default withLocalize(PlansSearch);