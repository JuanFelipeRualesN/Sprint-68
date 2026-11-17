import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Row,
    CardBody,
    Form
} from 'reactstrap';
import {withLocalize} from "react-localize-redux";
import {Translate} from "react-localize-redux";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import SubscriptionDataTable from '../datatables/subscription-data-table';
import ModalSubscription from '../modals/modal-subscription';
import {renderToStaticMarkup} from "react-dom/server";
import LanguageManager from "../../../custom-classes/language-manager";
import {Link, useLocation} from "react-router-dom";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowLeft} from "@fortawesome/free-solid-svg-icons";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

// This is the item list

const SubscriptionsSearch = () => {
    const [isEdit, setIsEdit] = useState(true);
    const [isReadOnly, setIsReadOnly] = useState(true);
    const [loading, setLoading] = useState(false);
    const [planId, setPlanId] = useState(null);
    const [propsModal, setPropsModal] = useState(null);
    const [redoSearch, setRedoSearch] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [subscriptionId, setSubscriptionId] = useState(null);
    const [userId, setUserId] = useState(null);
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);

 // Initialize translations based on the current version
    useVersionedTranslations();

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Toggle the update plan modal
    const toggleUpdateModal = (e) => {
        setShowEditModal(e);
        setRedoSearch(true);
    };

    const handleSubscriptionSelection = (e) => {

        setSubscriptionId(e.Id);
        setPlanId(e.Plan.Id);
        setUserId(e.UserId);
        setShowEditModal(true);
    };

    // Renders the main page
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
                                {queryParams.get('subscriptionType') === "H" ?
                                    <Translate id="commercial.commercial-subscriptions"/> :
                                    <Translate id="subscriptions.subscriptions"/>
                                }
                            </div>
                        </div>
                        <div className="col txt-right">
                            <Link
                                className="btn-default button-primary-custom"
                                to={{
                                    pathname: "/subscription-manager",
                                    search: queryParams.get('subscriptionType') ? `?subscriptionType=${queryParams.get('subscriptionType')}` : ""
                                }}
                            >
                                {queryParams.get('subscriptionType') === "H" ? (
                                    <Translate id="commercial.add-commercial-subscription"/>
                                ) : (

                                    <Translate id="subscriptions.add-subscription"/>
                                )}
                            </Link>
                        </div>
                    </div>

                    <Row className="clear-style-card td-responsive">
                        <Col xs={12}>
                            <Card>
                                <CardBody>
                                    <Form>
                                        <SubscriptionDataTable redoSearch={redoSearch}
                                                               subscriptionType={queryParams.get('subscriptionType')}
                                                               updateParent={handleSubscriptionSelection}/>
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
                <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/>
            )}
            {showEditModal && (
                <ModalSubscription
                    subscriptionId={subscriptionId}
                    planId={planId}
                    userId={userId}
                    isEdit={isEdit}
                    subscriptionType={queryParams.get('subscriptionType')}
                    isReadOnly={isReadOnly}
                    visible={showEditModal} updateParent={toggleUpdateModal}
                />
            )}
        </div>
    );
};

export default withLocalize(SubscriptionsSearch);