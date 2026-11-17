import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faAndroid} from "@fortawesome/free-brands-svg-icons";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
/** End of Multiple Language imports **/

/**
 * Modal for android download
 */
const ModalDownloadAndroid = ({ visible, updateParent }) => {
    const [linkGooglePlay] = useState(Settings.getDownloadAndroidAppURL());
    const [isMicrosoftTeams] = useState(SessionManager.getIfMicrosoftTeams());
    const [visibleState, setVisibleState] = useState(visible);

    useEffect(() => {
        setVisibleState(visible);
    }, [visible]);

    // Hide or shows the window
    const toggle = () => {
        setVisibleState(!visibleState);
        
        const state = {
            linkGooglePlay: linkGooglePlay,
            isMicrosoftTeams: isMicrosoftTeams,
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
                    {!isMicrosoftTeams ?
                        <Translate id="register-v2.android-installation" options={{renderInnerHtml: true}}/> :
                        <Translate id="register-v2.android-installation-teams" options={{renderInnerHtml: true}}/>
                    }
                    {isMicrosoftTeams ? <small><Translate id="register-v2.outside-play-to-download" options={{renderInnerHtml: true}}/></small> :
                    <a href={linkGooglePlay} className="btn  btn-success btn-block">
                        <FontAwesomeIcon icon={faAndroid}  />{'  '}
                        <small className="small-text">
                            <Translate id="register-v2.download" options={{renderInnerHtml: true}}/>
                        </small>
                    </a>}
                </ModalBody>
                <ModalFooter/>
            </Modal>
        </div>
    );
};

export default ModalDownloadAndroid;