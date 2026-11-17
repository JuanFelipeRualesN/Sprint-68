import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faWindows} from "@fortawesome/free-brands-svg-icons";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
/** End of Multiple Language imports **/

/**
 * Modal for Windows download
 */
const ModalDownloadWindows = ({ visible, updateParent }) => {
    const [linkWindowsDownload] = useState(Settings.getDownloadWindowsAppURL());
    const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());
    const [visibleState, setVisibleState] = useState(visible);

    useEffect(() => {
        setVisibleState(visible);
    }, [visible]);

    // Hide or shows the window
    const toggle = () => {
        setVisibleState(!visibleState);
        
        const state = {
            linkWindowsDownload: linkWindowsDownload,
            isMicrosoftTeams: isMicrosoftTeams,
            visible: !visibleState,
        };
        
        updateParent(state);
    };

    const handleDownload = () => {
        window.open(linkWindowsDownload, "_blank");
        toggle();
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal style={{zIndex: 99999}} isOpen={visibleState} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="general.attention"/>
                </ModalHeader>
                <ModalBody>
                    {!isMicrosoftTeams ?
                        <Translate id="register-v2.windows-installation" options={{renderInnerHtml: true}}/> :
                        <Translate id="register-v2.windows-installation-teams" options={{renderInnerHtml: true}}/>
                    }
                    <a className="btn  btn-success btn-block" onClick={handleDownload}>
                        <FontAwesomeIcon icon={faWindows}  />{'  '}
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

export default ModalDownloadWindows;