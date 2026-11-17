import React, {useEffect} from 'react';
import { Col, Row} from 'reactstrap';
import {Link} from "react-router-dom";
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server';
import {Translate} from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {
    faIdBadge,
    faMoneyCheck, faShoppingBasket,
    faTag
} from '@fortawesome/free-solid-svg-icons';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * To handle subscription details.
 */
const CommercialModule = () => {
    // Initialize translations based on the current version
    useVersionedTranslations();

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom mb-3 block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal mt-3">
                                <Translate id="commercial.commercial-area"/>
                            </div>
                        </div>
                    </div>

                    <Row>
                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                        <FontAwesomeIcon icon={faIdBadge} size="2x"/>{' '}
                                                     <span className="font-now-bold" style={{marginLeft: 10}}>
                                                         <Translate id="commercial.commercial-subscriptions"/>
                                                     </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="commercial.commercial-subscriptions-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link
                                        to={{
                                            pathname: "/commercial-subscriptions-search",
                                            search: "?subscriptionType=H"
                                        }}
                                        className="btn btn-info"
                                    >
                                        <Translate id="subscription-module.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>
                        </Col>
                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                            <FontAwesomeIcon icon={faMoneyCheck} size="2x"/>{' '}
                                            <span className="font-now-bold" style={{marginLeft: 10}}>
                                                <Translate id="commercial.commercial-plans"/>
                                            </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="subscription-module.plans-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/commercial-plans-search" className="btn btn-info">
                                        <Translate id="subscription-module.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>
                        </Col>

                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                            <FontAwesomeIcon icon={faTag} size="2x"/>{' '}
                                            <span className="font-now-bold" style={{marginLeft: 10}}>
                                                <Translate id="subscription-module.coupons"/>
                                            </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="subscription-module.coupons-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/coupons" className="btn btn-info">
                                        <Translate id="subscription-module.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>
                        </Col>

                        {/** COMMERCIAL PRODUCT **/}
                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                        <FontAwesomeIcon icon={faTag} size="2x"/>{' '}
                                        <span className="font-now-bold" style={{marginLeft: 10}}>
                                                <Translate id="subscription-module.product"/>
                                            </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="subscription-module.products-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/commercial-products" className="btn btn-info">
                                        <Translate id="subscription-module.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>
                        </Col>

                    </Row>
                </div>
            </div>

        </div>
    )
}

export default withLocalize(CommercialModule);