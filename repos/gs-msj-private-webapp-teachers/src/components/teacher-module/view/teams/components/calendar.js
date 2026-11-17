import React, {useEffect, useState} from 'react'
import {Calendar, momentLocalizer} from "react-big-calendar";
import {useHistory} from "react-router-dom";
import {useMsal} from "@azure/msal-react";
import {Translate} from "react-localize-redux";
import {faPlus, faVideo} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {DefaultButton, PrimaryButton, Stack, StackItem} from "office-ui-fabric-react";
import moment from "moment";
import {getTimeZone} from "../../../../../utils/utils";
import Header from "./header";
import {callMsGraphGetCalendar, callMsGraphGetUser} from "../../../../../graph";
import CalendarCreateDialog from "./calendar-create-dialog";
import CalendarDetailDialog from "./calendar-detail-dialog";
import SessionManager from "../../../../../custom-classes/session-manager";
import Settings from "../../../../../custom-classes/settings";
import LanguageManager from '../../../../../custom-classes/language-manager';
import "moment/locale/es";

const localizer = momentLocalizer(moment);

const CalendarEvent = () => {
  const [eventsData, setEventsData] = useState([]);
  const {instance, accounts} = useMsal();
  const [isDialogDetailVisible, setIsDialogDetailVisible] = useState(false);
  const [isDialogCreateMeetingVisible, setIsDialogCreateMeetingVisible] = useState(false);
  const [ isTeacher, setIsTeacher ] = useState(SessionManager.getUserRole() === Settings.ROLE_TEACHER_STR ||
      SessionManager.getUserRole() === Settings.ROLE_CLIENT_STR);
  const [positionDialog, setPositionDialog] = useState({
    top: 0,
    left: 0,
  });//{ top: 0, left: 0 }
  const [eventToDialog, setEventToDialog] = useState({
    // title: '',
    // start: '',
  });
  const history = useHistory();

    const appLocale = LanguageManager.getLanguage(); 
    const locale = ["en", "es"].includes(appLocale) ? appLocale : "en";
    moment.locale(locale);
  
  useEffect(() => {
    const startDateTime = moment()
      .startOf("week")
      .toISOString();
    const endDateTime = moment()
      .endOf("week")
      .toISOString();
    getCaledarEvents(startDateTime, endDateTime);
  }, []);

  
  const handleCloseModalDetail = () => {
    setIsDialogDetailVisible(false);
  }
  
  const handleCloseModalCreateMeeting = () => {
    setIsDialogCreateMeetingVisible(false);
  }
  
  
  const getCaledarEvents = (startDateTime, endDateTime) => {
    instance
      .acquireTokenSilent({
        scopes: ["Calendars.ReadWrite"],
      })
      .then((response) => {
        callMsGraphGetCalendar(
          response.accessToken,
          startDateTime,
          endDateTime
        ).then((response) => {
          const {value} = response;

          const events = value
            .filter(item => !item.isCancelled)
            .map((item) => {
            return ({
              id: item.id,
              start: new Date(
                moment
                  .utc(item.start.dateTime)
                  .tz(getTimeZone())
                  .toISOString()
              ),
              end: new Date(
                moment
                  .utc(item.end.dateTime)
                  .tz(getTimeZone())
                  .toISOString()
              ),
              isOrganizer: item.isOrganizer,
              title: item.subject,
              joinUrl: item.onlineMeeting && item.onlineMeeting.joinUrl,
              organizer: item.organizer && item.organizer.emailAddress.name,
              attendees: item.attendees && item.attendees
            })
          });
          setEventsData(events);
        });
      });
  };
  
  const handleRangeChange = (rangeDate) => {
    const startDateTime = moment(rangeDate[0]).toISOString();
    const endDateTime = moment(rangeDate[4]).toISOString();
    getCaledarEvents(startDateTime, endDateTime);
  };
  
  const calculatePosition = (e) => {
    const {clientX, clientY} = e;
    const {innerWidth, innerHeight} = window;
    
    let position = {};
    if (clientX > innerWidth / 2) {
      position.right = `${innerWidth - clientX}px`;
    } else {
      position.left = `${clientX}px`;
    }
    
    if (clientY > innerHeight / 2) {
      position.bottom = `${innerHeight - clientY}px`;
    } else {
      position.top = `${clientY}px`;
    }
    
    return position;
  };
  
  const handleDetailMeet = (event, e) => {
    const position = calculatePosition(e);
    
    setPositionDialog(position);
    setEventToDialog(event)
    setIsDialogDetailVisible(true)
  }
  
  const handleCreteMeetNow = (e) => {
    e.preventDefault()
    
    const position = calculatePosition(e);
    
    setPositionDialog(position);
    setIsDialogCreateMeetingVisible(true)
  }
  
  const formats = {
    timeGutterFormat: (date, culture, localizer) =>
      localizer.format(date, 'h:mm A', culture),
    
    eventTimeRangeFormat: ({ start, end }, culture, local) => {
      let s = local.format(start, 'h:mm A', culture);
      let e = local.format(end, 'h:mm A', culture);
      return `${s} - ${e}`;
    },
    
    agendaTimeRangeFormat: ({ start, end }, culture, local) =>
      `${local.format(start, 'h:mm A', culture)} - ${local.format(end, 'h:mm A', culture)}`
  }
  
  return (
    <>
      <Header />
      <div className="newMeetingContainer">
        <div className="container-teams">
          <div className="content-button-header-calendar">
            {isTeacher ?
              <Stack horizontal tokens={{childrenGap: 15}}>
                <StackItem align="end" className="newMeetingButtons">
                  <Stack horizontal tokens={{childrenGap: 10}}>
                    <PrimaryButton
                      className="teamsButton"
                      ariaLabel={"Create Meeting"}
                      onClick={ handleCreteMeetNow}
                    >
                      
                      <FontAwesomeIcon icon={faVideo} />{' '}
                      <Translate id='teams.Meet-now' options={{renderInnerHtml: true}}/>
                    </PrimaryButton>
                    <DefaultButton
                      className="teamsButtonInverted"
                      ariaLabel={"Cancel"}
                      onClick={() => history.push("/create-meeting")}
                    >
                      
                      <FontAwesomeIcon icon={faPlus} />{' '}
                      <Translate id='teams.Schedule-Meeting' options={{renderInnerHtml: true}}/>
                    </DefaultButton>
                  </Stack>
                </StackItem>
              </Stack>
              : null
            }
          </div>
          <Calendar
            // views={["day", "agenda", "work_week", "month"]}
            views={["work_week"]}
            selectable
            localizer={localizer}
            defaultDate={new Date()}
            defaultView="work_week"
            events={eventsData}
            formats={formats}
            // style={{ height: 500 }}
            className="calendarTeams-Content"
            onSelectEvent={handleDetailMeet}
            // onSelectEvent={(event) => alert(event.joinUrl ? event.joinUrl : event.title)}
            onRangeChange={handleRangeChange}
            // onSelectSlot={handleSelect}
          />
          
          <CalendarDetailDialog
            isVisible={isDialogDetailVisible}
            event={eventToDialog}
            positionDialog={positionDialog}
            onCloseModal={handleCloseModalDetail}
            isTeacher={isTeacher}
          />
          <CalendarCreateDialog
            onClose={handleCloseModalCreateMeeting}
            isOpen={isDialogCreateMeetingVisible}
            positionDialog={positionDialog}
          />
        </div>
      </div>
    </>
  );
};

export default CalendarEvent;
