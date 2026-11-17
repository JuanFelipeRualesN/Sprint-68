import React from 'react';
import {Col, Row} from 'reactstrap';
import {Link} from "react-router-dom";
import {withLocalize} from "react-localize-redux";
import {Translate} from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {
    faProjectDiagram, faPencilRuler
} from '@fortawesome/free-solid-svg-icons';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * To handle curriculum details.
 */
const CurriculumModule = () => {

    // Initialize translations based on the current version
    useVersionedTranslations();

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom mb-3 block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal mt-3">
                                    <Translate id="subscriptions.curricular-design-and-interface"/>
                            </div>
                        </div>
                    </div>

                    <Row>
                        <Col lg={{ size: 4 }} md={{ size: 6 }} sm={{ size: 12 }} xs={{ size: 12 }}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                        <FontAwesomeIcon icon={faPencilRuler} size="2x"/>{' '}
                                        <span className="font-now-bold" style={{marginLeft: 10}}>
                                                 <Translate id="subscription-module.product"/>
                                            </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="subscription-module.product-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/curriculum-products" className="btn btn-info">
                                        <Translate id="subscription-module.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>

                            </div>
                        </Col>


                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                        <FontAwesomeIcon icon={faProjectDiagram} size="2x"/>{' '}
                                        <span className="font-now-bold" style={{marginLeft: 10}}>
                                                 <Translate id="subscription-module.item-product-interface"/>
                                            </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="subscription-module.item-product-interface-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/curriculum-items-product" className="btn btn-info">
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

export default withLocalize(CurriculumModule);