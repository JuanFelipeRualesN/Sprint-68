import React from 'react';
import {Col, Row, Form} from 'reactstrap';
import PlatformDownloadList from "../../item-list/platform-download-list";
import {Translate} from "react-localize-redux";
import {Link} from "react-router-dom";
import Settings from "../../../../custom-classes/settings";

/**
 * Handle the class to a new password reset
 */
const FormAppDownload = () => {
  return (
      <>
        <Row>
            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}} style={{textAlign: "center"}}>
                <h2>
                    <b><Translate id="commercial.please-read" options={{renderInnerHtml: true}}/></b>
                </h2>
                <small>
                    <Translate id="commercial.please-read-subtitle" options={{renderInnerHtml: true}}/>
                </small>
            </Col>
        </Row>
        <Row>
            <Col>
                <hr/>
            </Col>
        </Row>
        <Row>
            <Col>
                <h4 className="titles-general">
                    <Translate id="commercial.your-site" options={{renderInnerHtml: true}}/>:
                </h4>
            </Col>
        </Row>
          <Row>
              <Col>
                  <ul>
                      <li>
                          <Translate id="commercial.your-site-subtitle" options={{renderInnerHtml: true}}/>
                      </li>
                  </ul>
              </Col>
          </Row>
          <Row>
              <Col>
                  <h4 className="titles-general">
                      <Translate id="commercial.student-site" options={{renderInnerHtml: true}}/>:
                  </h4>
              </Col>
          </Row>
          <Row>
              <Col>
                  <ul>
                      <li>
                          <Translate id="commercial.student-must-connect-through" options={{renderInnerHtml: true}}/>
                          <br/>
                          <span className="label-forms-blue">
                              {Settings.URL_SITE_STUDENTS}
                          </span>
                      </li>
                      <li>
                          <Translate id="commercial.student-credentials-explanation" options={{renderInnerHtml: true}}/>

                      </li>
                      <li>
                          <Translate id="commercial.student-site-explanation" options={{renderInnerHtml: true}}/>
                          <ol>
                              <li>
                                  <Translate id="commercial.student-site-explanation-1"
                                             options={{renderInnerHtml: true}}/>
                              </li>
                              <li>
                                  <Translate id="commercial.student-site-explanation-2"
                                             options={{renderInnerHtml: true}}/>
                              </li>
                          </ol>
                      </li>

                  </ul>
              </Col>

          </Row>
          <Row>
              <Col xs={{size: 12}} sm={{size: 12}} md={{size: 6, offset: 3}} lg={{size: 6, offset: 3}}>
                  <Link className="btn-default button-primary-custom" to="/dashboard">
                <Translate id="register-v2.go-to-dashboard" options={{renderInnerHtml: true}}/>

        </Link>
      </Col>
    </Row>
      </>
  );
};

export default FormAppDownload;