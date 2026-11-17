/**
 * Created by portiz on 03/16/20.
 */
import React, { useState, useEffect } from 'react';
import { Col, Row, Button} from 'reactstrap';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindows, faApple, faAndroid } from '@fortawesome/free-brands-svg-icons';
import Settings from '../../../custom-classes/settings';

import ModalDownloadAndroid from "../download-os/modal-download-android";
import ModalDownloadIpad from "../download-os/modal-download-ipad";
import ModalDownloadMac from "../download-os/modal-download-mac";
import ModalDownloadWindows from "../download-os/modal-download-windows";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * Component to download the page
 */
const PlatformDownloadList = () => {
    const [linkAppStore] = useState(Settings.getDownloadIpadsAppUrl());
    const [linkGooglePlay] = useState(Settings.getDownloadAndroidAppURL());
    const [linkMacDownload] = useState(Settings.getDownloadMacAppURL());
    const [linkWindowsDownload] = useState(Settings.getDownloadWindowsAppURL());
    const [modalAndroidDownloadVisible, setModalAndroidDownloadVisible] = useState(false);
    const [modalIpadDownloadVisible, setModalIpadDownloadVisible] = useState(false);
    const [modalMacDownloadVisible, setModalMacDownloadVisible] = useState(false);
    const [modalWindowsDownloadVisible, setModalWindowsDownloadVisible] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    const buttonAndroidDownloadClick = () => {
        setModalAndroidDownloadVisible(!modalAndroidDownloadVisible);
    };

    const buttoniPadDownloadClick = () => {
        setModalIpadDownloadVisible(!modalIpadDownloadVisible);
    };

    const buttonMacDownloadClick = () => {
        setModalMacDownloadVisible(!modalMacDownloadVisible);
    };

    const buttonWindowsDownloadClick = () => {
        setModalWindowsDownloadVisible(!modalWindowsDownloadVisible);
    };

    return(
        <div className="container-white container-padding  mod-ppal">
            <div>
                <Row className="text-center">
                    <Col lg={{size: 12}} md={{size:12}} sm={{size:12}} xs={{size:12}}>
                        <h2>
                            <Translate id="register-v2.select-os" options={{renderInnerHtml: true}}/>
                        </h2>
                        <small> <Translate id="register-v2.os-instructions" options={{renderInnerHtml: true}}/></small>
                    </Col>
                    <Col lg={{size: 12}} md={{size:12}} sm={{size:12}} xs={{size:12}}>
                        <hr/>
                    </Col>
                </Row>
                <Row>
                    <Col lg={{size: 12}} md={{size:12}} sm={{size:12}} xs={{size:12}} className="mod-central">
                        <h3><FontAwesomeIcon icon={faWindows} size="2x" />{'  '} Windows</h3>
                        <ul className="text-left">
                            <li><Translate id="register-v2.operating-system" options={{renderInnerHtml: true}}/>: Windows 7, Windows 8, Windows 8.1, Windows 10, Windows 11</li>
                            <li><Translate id="register-v2.monitor" options={{renderInnerHtml: true}}/>: 1280 x 800</li>
                            <li><Translate id="register-v2.processor" options={{renderInnerHtml: true}}/>: Intel Core, AMD</li>
                            <li><Translate id="register-v2.memory-ram" options={{renderInnerHtml: true}}/>: 4GB RAM</li>
                            <li><Translate id="register-v2.hard-drive" options={{renderInnerHtml: true}}/>: 4 GB </li>
                            <li><Translate id="register-v2.wireless-connection" options={{renderInnerHtml: true}}/>:
                                {' '}<Translate id="general.yes" options={{renderInnerHtml: true}}/>
                            </li>
                        </ul>
                        <Button className="btn  btn-success btn-block" onClick={buttonWindowsDownloadClick}>
                            <FontAwesomeIcon icon={faWindows}  />{'  '}
                            <small className="small-text white-text">
                                <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                            </small>
                        </Button>
                    </Col>
                        {/* <Col lg={{size: 6}} md={{size:6}} sm={{size:12}} xs={{size:12}} className="mod-central">
                            <h3><FontAwesomeIcon icon={faAndroid} size="2x" />{'  '} Android Tablet & Google Chromebooks </h3>
                            <ul className="text-left">
                                <li><Translate id="register-v2.operating-system" options={{renderInnerHtml: true}}/>: 8.0 Oreo+ (<Translate id="register-v2.recommended" options={{renderInnerHtml: true}}/>) / ChromeOS</li>
                                <li><Translate id="register-v2.monitor" options={{renderInnerHtml: true}}/>: 1280 x 800</li>
                                <li><Translate id="register-v2.processor" options={{renderInnerHtml: true}}/>: 2GHz + </li>
                                <li><Translate id="register-v2.memory-ram" options={{renderInnerHtml: true}}/>: 2GB RAM</li>
                                <li><Translate id="register-v2.hard-drive" options={{renderInnerHtml: true}}/>: 3 GB </li>
                                <li><Translate id="register-v2.wireless-connection" options={{renderInnerHtml: true}}/>:
                                    {' '}<Translate id="general.yes" options={{renderInnerHtml: true}}/>
                                </li>
                            </ul>
                            <Button className="btn  btn-success btn-block" onClick={buttonAndroidDownloadClick}>
                                <FontAwesomeIcon icon={faAndroid}  />{'  '}
                                <small className="small-text white-text">
                                    <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                                </small>
                            </Button>
                        </Col> */}
                </Row>
                    {/* <Row>
                        <Col lg={{size: 6}} md={{size:6}} sm={{size:12}} xs={{size:12}} className="mod-central">
                            <h3><FontAwesomeIcon icon={faApple} size="2x" />{'  '} iPads</h3>
                            <ul className="text-left">
                                <li><Translate id="register-v2.operating-system" options={{renderInnerHtml: true}}/>: 10.0 +</li>
                                <li><Translate id="register-v2.monitor" options={{renderInnerHtml: true}}/>: 2048x1536</li>
                                <li><Translate id="register-v2.processor" options={{renderInnerHtml: true}}/>: 1GHz+</li>
                                <li><Translate id="register-v2.memory-ram" options={{renderInnerHtml: true}}/>: 2GB RAM</li>
                                <li><Translate id="register-v2.hard-drive" options={{renderInnerHtml: true}}/>: 2GB </li>
                                <li><Translate id="register-v2.wireless-connection" options={{renderInnerHtml: true}}/>:
                                    {' '}<Translate id="general.yes" options={{renderInnerHtml: true}}/>
                                </li>
                            </ul>
                            <Button className="btn  btn-success btn-block" onClick={buttoniPadDownloadClick}>
                                <FontAwesomeIcon icon={faApple}  />{'  '}
                                <small className="small-text white-text">
                                    <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                                </small>
                            </Button>
                        </Col>

                        <Col lg={{size: 6}} md={{size:6}} sm={{size:12}} xs={{size:12}} className="mod-central">
                            <h3><FontAwesomeIcon icon={faApple} size="2x" />{'  '} Mac </h3>
                            <ul className="text-left">
                                <li><Translate id="register-v2.operating-system" options={{renderInnerHtml: true}}/>: OS X 10.12 (Sierra) +</li>
                                <li><Translate id="register-v2.monitor" options={{renderInnerHtml: true}}/>: 1280 x 800</li>
                                <li><Translate id="register-v2.processor" options={{renderInnerHtml: true}}/>: Intel Dual Core + </li>
                                <li><Translate id="register-v2.memory-ram" options={{renderInnerHtml: true}}/>: 4GB RAM</li>
                                <li><Translate id="register-v2.hard-drive" options={{renderInnerHtml: true}}/>: 4 GB </li>
                                <li><Translate id="register-v2.wireless-connection" options={{renderInnerHtml: true}}/>:
                                    {' '}<Translate id="general.yes" options={{renderInnerHtml: true}}/>
                                </li>
                            </ul>
                            <Button className="btn  btn-success btn-block" onClick={buttonMacDownloadClick}>
                                <FontAwesomeIcon icon={faApple}  />{'  '}
                                <small className="small-text white-text">
                                    <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                                </small>
                            </Button>
                        </Col>
                    </Row> */}
            </div>
            {/* <ModalDownloadAndroid visible={state.modalAndroidDownloadVisible} updateParent={buttonAndroidDownloadClick}/>
            <ModalDownloadIpad visible={state.modalIpadDownloadVisible} updateParent={buttoniPadDownloadClick}/>
            <ModalDownloadMac visible={state.modalMacDownloadVisible} updateParent={buttonMacDownloadClick}/> */}
            <ModalDownloadWindows visible={modalWindowsDownloadVisible} updateParent={buttonWindowsDownloadClick}/>
        </div>
    );
};

export default withLocalize(PlatformDownloadList);