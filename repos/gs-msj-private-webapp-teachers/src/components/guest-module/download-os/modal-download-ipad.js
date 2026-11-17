import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faApple} from "@fortawesome/free-brands-svg-icons";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
/** End of Multiple Language imports **/

/**
 * Modal for iPad download
 */
const ModalDownloadIpad = ({ visible, updateParent }) => {
    const [linkAppStore] = useState(Settings.getDownloadIpadsAppUrl());
    const [visibleState, setVisibleState] = useState(visible);
    const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());

    useEffect(() => {
        setVisibleState(visible);
    }, [visible]);

    // Hide or shows the window
    const toggle = () => {
        setVisibleState(!visibleState);
        
        const state = {
            linkAppStore: linkAppStore,
            visible: !visibleState,
            isMicrosoftTeams: isMicrosoftTeams,
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
                    {!isMicrosoftTeams ?
                        <Translate id="register-v2.ipad-installation" options={{renderInnerHtml: true}}/> :
                        <Translate id="register-v2.ipad-installation-teams" options={{renderInnerHtml: true}}/>
                    }
                    {isMicrosoftTeams ? <small><Translate id="register-v2.outside-play-to-download" options={{renderInnerHtml: true}}/></small> :

                    <a href={linkAppStore} className="btn  btn-success btn-block">
                        <FontAwesomeIcon icon={faApple}  />{'  '}
                        <small className="small-text">
                            <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                        </small>
                    </a>}
                </ModalBody>
                <ModalFooter>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalDownloadIpad;