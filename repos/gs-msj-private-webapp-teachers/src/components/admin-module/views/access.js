import React, { useEffect } from 'react';
import {Link} from "react-router-dom";
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faUserLock, faLockOpen, faIdBadge, faMoneyCheck, faTag} from '@fortawesome/free-solid-svg-icons';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import {Col, Row} from "reactstrap";
import './style-selector-school.css'
/**
 * To handle access details.
 */
const Access = () => {
    // Initialize translations based on the current version
    useVersionedTranslations();

    // Renders the main dashboard
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom mb-3 block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal mt-3">
                                <Translate id="access.access"/>
                            </div>
                        </div>
                    </div>

                    <Row>
                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                        <FontAwesomeIcon icon={faUserLock} size="2x"/>{' '}
                                        <span className="font-now-bold" style={{marginLeft: 10}}>
                                                        <Translate id="access.roles"/>
                                                     </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="access.roles-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/roles-manager" className="btn-simple--only-text">
                                        <Translate id="access.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>
                        </Col>

                        {/** Permissions **/}
                        <Col lg={{size: 4}} md={{size: 6}} sm={{size: 12}} xs={{size: 12}}>
                            <div className="border border-gray-200 rounded-lg shadow-sm flex flex-col h-100">
                                <div className="p-2 homeschooling-results-header">
                                    <Row>
                                        <FontAwesomeIcon icon={faLockOpen} size="2x"/>{' '}
                                        <span className="font-now-bold" style={{marginLeft: 10}}>
                                                <Translate id="access.permissions"/>
                                            </span>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col items-center">
                                    <Row>
                                        <Col>
                                            <small>
                                                <Translate id="access.permissions-subtitle"/>
                                            </small>
                                        </Col>
                                    </Row>
                                </div>
                                <div className="p-3 flex flex-col" style={{textAlign: "right"}}>
                                    <Link to="/permission-manager" className="btn-simple--only-text">
                                        <Translate id="access.go" options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>
                        </Col>

                    </Row>
                </div>
            </div>
        </div>

    );
};

export default withLocalize(Access);