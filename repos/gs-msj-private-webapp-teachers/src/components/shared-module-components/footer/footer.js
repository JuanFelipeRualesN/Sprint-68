import React, { useState, useEffect } from 'react';
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import LanguageSelector from '../selector/language-selector';
import { Col, Container, Row } from 'reactstrap';

import { faFacebook, faTwitter, faInstagram } from '@fortawesome/free-brands-svg-icons';
import ModalChildrenPolicy from '../modal/modal-child-policy';
import ModalTerms from '../modal/modal-terms';
import ModalReturnPolicy from '../modal/modal-return-policy';
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import { faChild } from "@fortawesome/free-solid-svg-icons";
import {Link} from "react-router-dom";

const Footer = () => {
    const [companyName, setCompanyName] = useState("Estudia y Avanza");
    const [isMicrosoftTeams, setIsMicrosoftTeams] = useState(false);
    const [link1] = useState("https://estudiayavanza.sanjuan.pr/");
    const [link2] = useState("http://www.edulabpr.com");
    const [link3] = useState(Settings.getDownloadAndroidAppURL());
    const [link4] = useState(Settings.getDownloadIpadsAppUrl());
    const [linkFacebook] = useState("https://www.facebook.com/genialskills");
    const [linkInstagram] = useState("https://www.instagram.com/genialskills");
    const [linkTwitter] = useState("https://www.twitter.com/genialskills");
    const [roles] = useState(SessionManager.getSessionDetails() === null ? [] : SessionManager.getSessionDetails().Roles);
    const [showModalChildrensPolicy, setShowModalChildrensPolicy] = useState(false);
    const [showModalReturnPolicy, setShowModalReturnPolicy] = useState(false);
    const [showModalTerms, setShowModalTerms] = useState(false);

    useEffect(() => {
        setIsMicrosoftTeams(SessionManager.getIfMicrosoftTeams());
    }, []);

    // Toggle the terms modal
    const toggleModalTerms = () => {
        setShowModalTerms(!showModalTerms);
    };

    // Toggles the modal of return policy
    const toggleModalReturnPolicy = () => {
        setShowModalReturnPolicy(!showModalReturnPolicy);
    };

    // Toggle de modal children policy
    const toggleModalChildrenPolicy = () => {
        setShowModalChildrensPolicy(!showModalChildrensPolicy);
    };

    return (
        <div className="no-print">
            <div ></div>
            <div className="page-footer font-small pt-4 card-dark-blue">
                <Container fluid className="text-center text-md-left">
                    <Row>
                        <Col md="12">
                            {/*<ScrollToTop showUnder={0}>
                            <span className="btn card-orange"><Translate id="footer.back-to-top"/></span>
                        </ScrollToTop>*/}
                        </Col>
                        <hr className="clearfix w-100 d-md-none" />
                        {!isMicrosoftTeams ?
                            <Col md="12" className="text-center">

                                <h5 className="text-uppercase mb-4 mt-3 font-weight-bold">
                                    <Translate id="footer.more-about-us" />
                                </h5>

                                {
                                    roles.indexOf(Settings.ROLE_STUDENT_STR) <= -1 ?
                                        <ul className="list-unstyled">
                                            <li><a href={link1} className="white-text"><Translate id="footer.store" /></a>
                                            </li>
                                            <li><a href={link2} className="white-text"><Translate
                                                id="footer.edulab" /></a></li>
                                            <li><a href={link3} className="white-text"><Translate
                                                id="footer.google-play" /></a></li>
                                            <li><a href={link4} className="white-text"><Translate
                                                id="footer.app-store" /></a>
                                            </li>
                                        </ul> : null
                                }
                            </Col> : null
                        }
                    </Row>
                </Container>
                <hr />
                <div className="text-center py-3">
                    <LanguageSelector />
                    {roles.indexOf(Settings.ROLE_STUDENT_STR) <= -1 ?
                        <div>
                            <br /><Translate id="user-help.customer-service" options={{ renderInnerHtml: true }} />{' '}
                            {" "}
                            <a href={`tel:1-${Settings.PHONE_NUMBER}`}  contenteditable="false">{Settings.PHONE_NUMBER}</a>{" "}
                        </div> : null
                    }
                </div>
                <div className="footer-bottom-background-color">
                    <hr className="hr-dark" />
                    {!isMicrosoftTeams && roles.indexOf(Settings.ROLE_STUDENT_STR) <= -1 ?
                        <div className="text-center">
                            <ul className="list-unstyled list-inline">
                                <li className="list-inline-item">
                                    <a href={linkFacebook} className="white-text">
                                        <FontAwesomeIcon icon={faFacebook} size="2x" />
                                    </a>
                                </li>
                                <li className="list-inline-item">
                                    <a href={linkTwitter} className="white-text">
                                        <FontAwesomeIcon icon={faTwitter} size="2x" />
                                    </a>
                                </li>
                                <li className="list-inline-item">
                                    <a href={linkInstagram} className="white-text">
                                        <FontAwesomeIcon icon={faInstagram} size="2x" />
                                    </a>
                                </li>
                            </ul>
                        </div> : null
                    }
                    <div className="footer-copyright text-center py-3">
                        {!isMicrosoftTeams ?
                            <Container fluid>
                                <ModalTerms isOpen={showModalTerms} toggle={toggleModalTerms} />
                                <ModalReturnPolicy isOpen={showModalTerms} toggle={toggleModalTerms} />
                                <ModalChildrenPolicy isOpen={showModalChildrensPolicy}
                                    toggle={toggleModalChildrenPolicy} />
                                <br />
                                &copy; {(new Date().getFullYear())} {' '}
                                <Translate id="footer.copyright" options={{ renderInnerHtml: false }} />
                                {' '} {companyName}
                            </Container> :
                            <Container fluid>
                                <Link to="/terms">

                                    <Translate>{({ translate }) => <span>{translate("terms.title", null, { renderInnerHtml: true })}</span>}</Translate>
                                </Link>
                                <br />
                                <Link to="/privacy">
                                    <span className="red-text" onClick={this.toggle} style={{ textDecoration: "underline" }}>
                                        <FontAwesomeIcon icon={faChild} />{' '}
                                        <Translate>{({ translate }) => <span>{translate("children-policy.title", null, { renderInnerHtml: true })}</span>}</Translate>
                                    </span>
                                </Link>
                            </Container>
                        }
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Footer;