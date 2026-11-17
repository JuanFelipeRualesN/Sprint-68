import React from 'react'
import {Translate} from "react-localize-redux";
import {getDate} from "../../../../../utils/utils";
import {useHistory} from "react-router-dom";
import useDisableScroll from "../../../../../hooks/useDisableScroll";

const CalendarDetailDialog = ({isVisible, event, positionDialog, onCloseModal, isTeacher}) => {


    // disable scroll when modal is open
    useDisableScroll(isVisible);
    const history = useHistory();


    if (!isVisible) return null

    // open event.joinUrl on new tab
    const handleJoinMeeting = (e) => {
        e.preventDefault()
        window.open(event.joinUrl, '_blank')
    }


    // go to edit event page
    const handleEditMeeting = (e) => {
        e.preventDefault()
        history.push(`/create-meeting?eventId=${event.id}`)
    }

    return (
        <>
            <div className="modal-overlay" onClick={onCloseModal}>
            </div>

            <div className="modalContent-MeetDetail ForceWitdht-OnlyTest dialog-teams" style={positionDialog}>
                <div className="meetDetail-content">
                    <div className="meetDetail-content--title">{event.title}</div>
                    <p className="meetDetail-content--date">{getDate(event.start, event.end)}</p>
                </div>
                <div>
                    <button className="buttonTeams-Primary mr-2" onClick={handleJoinMeeting}>
                        <Translate id='teams.join' options={{renderInnerHtml: true}}/>
                    </button>

                    {(event.isOrganizer && isTeacher)
                        ? <button className="buttonTeams-Default" onClick={handleEditMeeting}>
                            <Translate id='teams.edit' options={{renderInnerHtml: true}}/>
                        </button>
                        : null
                    }
                </div>

                <div className="meetDetail-aditionalInfo">
                    <ul className="aditionalInfo-Organizer">
                        <li>{event.organizer}</li>
                        <li> {" "}
                            <Translate id='teams.Organizer' options={{renderInnerHtml: true}}/>
                        </li>
                    </ul>

                    {Boolean(event.attendees.length) ?

                        <ul className="aditionalInfo-Guest">

                            <li className="aditionalInfo-Guest--title">
                                <Translate id='teams.Guests' options={{renderInnerHtml: true}}/>
                            </li>
                            {event.attendees.map((attendee, index) => (
                                <li key={index}>{attendee.emailAddress.name}</li>
                            ))}
                        </ul>
                        : null
                    }

                </div>
            </div>
        </>
    )
}


export default CalendarDetailDialog
