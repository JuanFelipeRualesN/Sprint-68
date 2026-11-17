/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import { Container} from 'reactstrap';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
/** End of Multiple Language imports **/
import PlatformDownloadList from '../item-list/platform-download-list';
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import './style.css';
import LiveChat from "react-livechat";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to download the app
 */
const DownloadPage = () => {
    const [isMicrosoftTeams, setIsMicrosoftTeams] = useState(false);
    const [roles] = useState(SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles);
    const [showSubscribeButton, setShowSubscribeButton] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.add('landing-white'); // Custom background for students
        setIsMicrosoftTeams(SessionManager.getIfMicrosoftTeams());
        setShowSubscribeButton(
            roles === null ||
            typeof roles === 'undefined' ||
            roles.length === 0 ||
            roles.indexOf(Settings.ROLE_TEACHER_STR) === -1
        );
    }, []);

    return (
        <div>
            <div className="card-white container-padding  animated delay-05s fadeIn">
                <div className="container-padding  animated delay-05s fadeIn">
                    <Container className="container-wrapper" >
                        <PlatformDownloadList/>
                    </Container>
                </div>
            </div>
            <LiveChat license={Settings.CHAT_LIVE_CHAT_LICENSE} />
        </div>
    );
};

export default withLocalize(DownloadPage);