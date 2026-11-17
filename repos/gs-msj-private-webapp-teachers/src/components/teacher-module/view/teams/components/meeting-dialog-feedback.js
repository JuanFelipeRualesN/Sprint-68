import React from 'react';
import {Translate} from "react-localize-redux";
import iconSuccessful from "../../../../../assets/images/common-icons/Feedback-icon-Success.svg";
import {faExclamationTriangle, faSpinner, faTimes    } from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useHistory} from "react-router-dom";

const MeetingDialogFeedback = ({state = 'warning', isOpen, onClose, cancelMeeting}) => {


    const history = useHistory();
    const goToCalendar = () => {
        history.push('/teams-calendar')
    }

    const handleOnClose = () => {
        if (state === 'warning') {
            onClose()
        }
        if (state === 'error') {
            onClose()
        }
    }

    if (!isOpen) return null;

    return (
        <>
            <div className="modal-overlay with-backdrop" onClick={handleOnClose}/>
            <div
                className="modalContent-Successful ForceWitdht-OnlyTest dialog-teams"
                style={{top: '50%', left: '50%', transform: 'translate(-50%, -50%)'}}
            >
                {state === 'success' ?
                    <div className="modalContent-Successful--icon">
                        <img src={iconSuccessful} alt={'icon-success'}/>
                    </div>
                    : null
                }
                {state === 'warning' ?
                    <div className="modalContent-Successful--icon">
                        <FontAwesomeIcon icon={faExclamationTriangle} className={"text-color-orange"} size={'3x'}/>
                    </div>
                    : null
                }

                {state === 'loading' ?
                    <div className="modalContent-Successful--icon">
                        <FontAwesomeIcon icon={faSpinner} spin size={'3x'}/>
                    </div>
                    : null
                }
                { state === 'error' ?
                    <div className="modalContent-Successful--icon">
                        <FontAwesomeIcon icon={faTimes} className={"text-color-red"} size={'3x'}/>
                    </div>
                    : null
                }

                <div className="modalContent-Successful--desc">
                    {state === 'success' ?
                        <Translate id="teams.meeting-created-success" options={{renderInnerHtml: true}}/>
                        : null
                    }
                    {state === 'warning' ?
                        <Translate id="teams.delete-meeting-question" options={{renderInnerHtml: true}}/>
                        : null
                    }
                    {state === 'loading' ?
                        <Translate id="teams.Loading" options={{renderInnerHtml: true}}/>
                        : null
                    }
                    { state === 'error' ?
                        <Translate id="teams.error-creating-meeting" options={{renderInnerHtml: true}}/>
                        : null
                    }


                </div>
                <div className="modalContent-Successful--button" style={{textAlign: 'center'}}>
                    {state === 'success' ?
                        <button className="buttonTeams-Primary" onClick={goToCalendar}>
                            <Translate id="teams.accept" options={{renderInnerHtml: true}}/>
                        </button> : null}
                    {state === 'warning' ?
                        <>
                            <button className="buttonTeams-Primary mr-2" onClick={cancelMeeting}>
                                <Translate id="teams.delete" options={{renderInnerHtml: true}}/>
                            </button>
                            <button className="buttonTeams-Default" onClick={onClose}>
                                <Translate id="teams.cancel" options={{renderInnerHtml: true}}/>
                            </button>
                        </>
                        : null}

                    {state === 'error' ?
                        <button className="buttonTeams-Primary" onClick={onClose}>
                            <Translate id="teams.accept" options={{renderInnerHtml: true}}/>
                        </button> : null}

                </div>
            </div>
        </>
    )
}

export default MeetingDialogFeedback;