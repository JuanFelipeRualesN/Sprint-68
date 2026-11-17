/**
 * Created by portiz on 06/18/2020.
 */
import React, { useEffect } from 'react';
import { Col, Container, Row} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to show the privacy details. This is required to be on a link instead of integrated
 * by other apps as Microsoft Teams
 */
const Privacy = () => {
    // Initialize translations based on the current version
    useVersionedTranslations();

    return (
        <div className="card-white">
            <div className="container-padding animated delay-05s fadeIn">
                <Container className="container-full">
                    <Row>
                        <Col xs={12}>
                            <Translate>{({translate}) => <span>{translate("children-policy.conditions",  null, { renderInnerHtml: true })}</span>}</Translate>
                            <Translate>{({translate}) => <span>{translate("terms.conditions", null, { renderInnerHtml: true })}</span>}</Translate>
                        </Col>
                    </Row>
                </Container>
            </div>
        </div>
    );
};

export default withLocalize(Privacy);