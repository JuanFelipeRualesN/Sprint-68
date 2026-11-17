import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faApple} from "@fortawesome/free-brands-svg-icons";
import Settings from "../../../custom-classes/settings";
/** End of Multiple Language imports **/

/**
 * Modal for Mac download
 */
const ModalDownloadMac = ({ visible, updateParent }) => {
    const [linkMacDownload] = useState(Settings.getDownloadMacAppURL());
    const [visibleState, setVisibleState] = useState(visible);

    useEffect(() => {
        setVisibleState(visible);
    }, [visible]);

    // Hide or shows the window
    const toggle = () => {
        setVisibleState(!visibleState);
        
        const state = {
            linkMacDownload: linkMacDownload,
            visible: !visibleState,
        };
        
        updateParent(state);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal style={{zIndex: 99999}} isOpen={visibleState} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="general.attention"/>
                </ModalHeader>
                <ModalBody>
                    <Translate id="register-v2.mac-installation" options={{renderInnerHtml: true}}/>
                    <a href={linkMacDownload} className="btn  btn-success btn-block">
                        <FontAwesomeIcon icon={faApple}  />{'  '}
                        <small className="small-text">
                            <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                        </small>
                    </a>
                </ModalBody>
                <ModalFooter>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalDownloadMac;