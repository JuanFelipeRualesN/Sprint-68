import React, {useState} from "react";
import {useMsal} from "@azure/msal-react";
import {Translate} from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCheck, faSpinner} from "@fortawesome/free-solid-svg-icons";
import useDisableScroll from "../../../../../hooks/useDisableScroll";

const CalendarCreateDialog = ({isOpen, onClose, positionDialog}) => {

    const {instance, accounts} = useMsal();
    const [meetingUrl, setMeetingUrl] = useState(null)
    const [meetingName, setMeetingName] = useState('');
    const [isInputDisabled, setInputDisabled] = useState(false);
    const [isInputError, setInputError] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [isLinkCopied, setIsLinkCopied] = useState(false);

    // disable scroll when modal is open
    useDisableScroll(isOpen);

    if (!isOpen) return null;

    const createMeeting = async () => {

        const now = new Date();
        const oneHourLater = new Date(now.getTime() + 60 * 60 * 1000);

        const meetingDetails = {
            startDateTime: now.toISOString(),
            endDateTime: oneHourLater.toISOString(),
            subject: meetingName
        };

        const token = await getAccessToken();
        const response = await fetch('https://graph.microsoft.com/v1.0/me/onlineMeetings', {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(meetingDetails)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        setMeetingUrl(data.joinWebUrl)
        return data.joinWebUrl;

    }


    const createMeetingAndCopyLink = async () => {
        if (!onSubmit()) return
        if (!meetingUrl) {
            const joinWebUrl = await createMeeting()
            await navigator.clipboard.writeText(joinWebUrl)
        } else
            await navigator.clipboard.writeText(meetingUrl)
        setIsLinkCopied(true);
        setTimeout(() => {
            setIsLinkCopied(false);
            setIsLoading(false);
        }, 2000);
    };

    const createMeetingAndOpenNewTab = async () => {
        if (!onSubmit()) return
        if (!meetingUrl) {
            const joinWebUrl = await createMeeting()
            window.open(joinWebUrl, '_blank');
        } else
            window.open(meetingUrl, '_blank');
        setIsLoading(false);
        onCloseModal()

    }

    // handle close modal and reset state
    const onCloseModal = () => {
        onClose();
        setInputError(false);
        setMeetingUrl(null)
        setInputDisabled(false);
        setIsLoading(false);
        setMeetingName('');
    }

    const handleInputChange = (event) => {
        setMeetingName(event.target.value);
        if (event.target.value.trim().length > 0) {
            setInputError(false);
        }
    };

    const onSubmit = () => {
        if (meetingName.trim().length === 0) {
            setInputError(true);
            return false;
        }
        setIsLoading(true);
        setInputDisabled(true);
        return true;
    }


    const getAccessToken = async () => {
        let token;
        try {
            token = await instance.acquireTokenSilent({
                scopes: ["Calendars.ReadWrite"],
            });
        } catch (ex) {
            token = await instance.acquireTokenPopup({
                scopes: ["Calendars.ReadWrite"],
            });
        }
        return token.accessToken;
    }

    return (
        <>
            <div className="modal-overlay" onClick={onCloseModal}></div>
            <div className="modalContent-MeetNow ForceWitdht-OnlyTest dialog-teams" style={positionDialog}>
                {isLoading ? (
                    <div className='modal-is-loading'>
                        {isLinkCopied ?
                            <>
                                <FontAwesomeIcon icon={faCheck} className={"text-color-green"} size={'lg'}/>{' '}
                                <Translate id="teams.link-copied" options={{renderInnerHtml: true}}/>
                            </>
                            : <>
                                <FontAwesomeIcon icon={faSpinner} spin size={'lg'}/>{' '}
                                <Translate id="teams.Loading" options={{renderInnerHtml: true}}/>
                            </>
                        }
                    </div>
                ) : null}
                <div className="form-wizard-fields">
                    <label>* <Translate
                        id="teams.meeting-name"
                        options={{renderInnerHtml: true}}/>
                    </label>
                    <input
                        type="text"
                        className="form-wizard-fields__input"
                        name="meetingName"
                        value={meetingName}
                        onChange={handleInputChange}
                        disabled={isInputDisabled}
                        autoComplete={"off"}
                    />
                    {isInputError ? (
                        <p className="content-meet--helptext text-color-red">
                            <Translate id="teams.meeting-name-required" options={{renderInnerHtml: true}}/>
                        </p>
                    ) : null}
                </div>

                <div>
                    <button className="buttonTeams-Default" onClick={createMeetingAndCopyLink} >
                        <Translate id="teams.get-link" options={{renderInnerHtml: true}}/>
                    </button>
                    <button className="buttonTeams-Primary" onClick={createMeetingAndOpenNewTab}>
                        <Translate id="teams.start-meeting" options={{renderInnerHtml: true}}/>
                    </button>
                </div>
            </div>
        </>
    );


}



export default CalendarCreateDialog;
