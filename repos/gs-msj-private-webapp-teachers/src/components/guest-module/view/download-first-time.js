/**
 * Created by portiz on 3/4/19.
 */
import React, { useEffect } from 'react';
import { Container, Col,  Row, } from 'reactstrap';
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import PlatformDownloadList from '../item-list/platform-download-list';
import { Link } from "react-router-dom";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to download the app for the first time
 */
const DownloadFirstTime = ({ location }) => {

    // Initialize translations based on the current version
    useVersionedTranslations();

    return (
        <div>
            <div className="container-padding card-white">
                <Container className="container-full">
                    <Row className="centered-text">
                        <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <h1 >
                                <Translate id="downloads.download-app-for-your-child"/>
                            </h1>
                        </Col>
                    </Row>
                    <Row>
                        <PlatformDownloadList/>
                    </Row>
                    <Row>
                        <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <h2>
                                <Translate id="downloads.are-ready-downloaded-the-app"/>
                            </h2>
                            <br/>
                            <h3>
                                <Translate id="downloads.to-see-progress"/> {' '}
                                <Link className="btn-default button-primary-custom" to="/dashboard">
                                    <Translate id="downloads.here"/>
                                </Link>
                            </h3>
                        </Col>
                    </Row>
                </Container>
            </div>
        </div>
    );
};

export default withLocalize(DownloadFirstTime);