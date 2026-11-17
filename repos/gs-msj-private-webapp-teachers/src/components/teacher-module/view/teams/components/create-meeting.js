import React, {useEffect, useState} from 'react';
import Header from "./header";
import {DefaultButton, FontWeights, PrimaryButton, Stack, StackItem, Text} from "office-ui-fabric-react";
import {Translate} from "react-localize-redux";
import {faArrowLeft, faArrowRight} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useHistory, useLocation, useParams} from "react-router-dom";
import {useForm} from "react-hook-form";
import {InputSelectGroup, InputSelectStudent} from "./inputs-select";
import DatePicker from "../../../../shared-module-components/date-picker/date-picker-recent";

import events from "./events"
import moment from "moment";
import Select from "react-select";
import useTimeZones from "../../../../../hooks/useTimeZones";
import styled from "styled-components";
import {getGroupDetails} from "../../../../../api/teachers";
import axios from "axios";
import {useMsal} from "@azure/msal-react";
import {convertTo24Hour, getTimeZone} from "../../../../../utils/utils";
import MeetingDialogFeedback from "./meeting-dialog-feedback";
import Settings from "../../../../../custom-classes/settings";
import SessionManager from "../../../../../custom-classes/session-manager";

const CreateMeeting = () => {
  
  const {instance, accounts} = useMsal();
  const {
    handleSubmit,
    register,
    setValue,
    formState: {errors}
    
  } = useForm();
  const history = useHistory();
  const options = events.map(item => ({label: item, value: item}));
  const timeZones = useTimeZones()
  const userRole = SessionManager.getUserRole();
  
  // states for groups logic
  const [attendersByGroup, setAttendersByGroup] = useState([]); // [{groupId: 1, attenders: []}
  const [groups, setGroups] = useState([]); // [{groupId: 1, attenders: []}
  const [groupsMap, setGroupsMap] = useState(new Map());
  // .state for students logic
  
  const [attendersByStudents, setAttendersByStudents] = useState([]); // [{studentId: 1, attenders: []}
  const [startDate, setStartDate] = useState(moment().format("YYYY-MM-DD"));
  const [endDate, setEndDate] = useState(moment().format("YYYY-MM-DD"));
  const [hourStart, setHourStart] = useState("08:00 am");
  const [hourEnd, setHourEnd] = useState("08:00 am");
  const [selectedTimezone, setSelectedTimezone] = useState("");
  const [isLoaded, setIsLoaded] = useState(false);
  
  // get query param evenId with react router dom
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const eventId = searchParams.get('eventId');
  
  // modal feedback
  const [isModalFeedbackVisible, setIsModalFeedbackVisible] = useState(false);
  const [modalFeedbackState, setModalFeedbackState] = useState('loading');
  
  // manage error date
  const [errorDate, setErrorDate] = useState(null);
  
  function removeExtraContent(text) {
    // Divide el texto en un array utilizando "........................................................................................................................................." como separador
    const parts = text.split(".........................................................................................................................................");
    // Devuelve solo la primera parte, que es el texto antes de "........................................................................................................................................."
    return parts[0];
  }
  
  // get event details from graph.microsoft.com
  useEffect(() => {
    const getEventDetails = async () => {
      if (eventId) {
        
        try {
          const token = await getToken();
          
          const response = await axios.get(`https://graph.microsoft.com/v1.0/me/events/${eventId}`, {
            headers: {Authorization: `Bearer ${token.accessToken}`},
          });
          
          const data = response.data;
          
          setValue('name', data.subject);
          setValue('description', removeExtraContent(data.bodyPreview));
          setStartDate(moment.utc(data.start.dateTime).tz(getTimeZone()).format("YYYY-MM-DD"));
          setEndDate(moment.utc(data.end.dateTime).tz(getTimeZone()).format("YYYY-MM-DD"));
          setHourStart(moment.utc(data.start.dateTime).tz(getTimeZone()).format("hh:mm a"));
          setHourEnd(moment.utc(data.end.dateTime).tz(getTimeZone()).format("hh:mm a"));
          setAttendersByStudents(data.attendees.map(item => ({email: item.emailAddress.address})));
          
        } catch (error) {
          console.error(error);
        }
        
      }
      setIsLoaded(true);
    }
    getEventDetails();
    
  }, []);
  
  const handleCancelMeeeting = () => {
    setModalFeedbackState('warning');
    setIsModalFeedbackVisible(true);
  }
  
  // cancel meeting
  const cancelMeeting = async () => {
    setModalFeedbackState('loading');
    
    const token = await getToken();
    
    const config = {
      headers: {
        'Authorization': `Bearer ${token.accessToken}`,
        'Content-Type': 'application/json'
      }
    }
    
    axios.delete(
      `https://graph.microsoft.com/v1.0/me/events/${eventId}`,
      config
    ).then((response) => {
    }).catch((error) => {
      console.error(error);
    });
    
    setModalFeedbackState('success');
    
  }
  
  useEffect(() => {
    const getGuessTimeZone = async () => {
      const timezone1 = Intl.DateTimeFormat().resolvedOptions().timeZone
      setSelectedTimezone({value: timezone1, label: timezone1})
    }
    getGuessTimeZone();
  }, [])
  
  
  const getMembersByGroup = async (groupId) => {
    // Tu lógica para obtener los miembros por grupo
    let recipients = [];
    try {
      const response = await getGroupDetails(groupId);
      const data = response.data;
      if (data.StudentDetailModelList && data.StudentDetailModelList.length > 0) {
        recipients = data.StudentDetailModelList;
      }
    } catch (error) {
      console.error(error);
    }
    return recipients;
  }
  
  const setSelectedGroups = async (selectedGroups) => {
    setGroups(selectedGroups);
    
    if (selectedGroups.length > 0) {
      const selectedIds = selectedGroups.map(item => item.value);
      const newGroupAdded = selectedGroups.length > groups.length;
      let groupId;
      
      if (newGroupAdded) {
        groupId = selectedGroups[selectedGroups.length - 1].value;
        if (!groupsMap.has(groupId)) {
          const recipients = await getMembersByGroup(groupId);
          groupsMap.set(groupId, recipients);
        }
      } else {
        groupId = groups.find(item => !selectedIds.includes(item.value)).value;
        if (groupsMap.has(groupId)) {
          groupsMap.delete(groupId);
        }
      }
      
      const arrayValues = [].concat(...groupsMap.values());
      setAttendersByGroup(arrayValues);
    } else {
      setGroups([]);
      setAttendersByGroup([]);
      setGroupsMap(new Map());
    }
    
  }
  
  const setSelectedStudents = (selectedStudents) => {
    setAttendersByStudents(selectedStudents);
  }
  
  const getToken = async () => {
    let token;
    try {
      token = await instance.acquireTokenSilent({
        scopes: ["OnlineMeetings.ReadWrite"],
      });
    } catch (ex) {
      token = await instance.acquireTokenPopup({
        scopes: ["OnlineMeetings.ReadWrite"],
      });
    }
    return token;
  }
  
  const onSubmit = async (data) => {
    const {name, description} = data;
    const submit_data = {
      name,
      description,
      attendersByGroup,
      attendersByStudents,
      startDate,
      endDate,
      hourStart,
      hourEnd,
      selectedTimezone
    }
    
    // validate if start date is less than end date
    const startDateValue = new Date(`${moment(startDate).format('MM/DD/YYYY')} ${convertTo24Hour(hourStart)}:00`);
    const endDateValue = new Date(`${moment(endDate).format('MM/DD/YYYY')} ${convertTo24Hour(hourEnd)}:00`);
    if (startDateValue >= endDateValue) {
      setErrorDate("error-meeting-date")
      return
    }
    
    // validate if start date is less than current date with validation for selected time zone
    const currentDate = new Date();
    const currentDateWithTimeZone = new Date(currentDate.toLocaleString('en-US', {timeZone: selectedTimezone.value}));
    if (startDateValue < currentDateWithTimeZone) {
      setErrorDate("error-meeting-date-end")
      return
    }
    
    // create meeting
    setModalFeedbackState('loading');
    setIsModalFeedbackVisible(true);
    
    const token = await getToken();
    
    const config = {
      headers: {
        'Authorization': `Bearer ${token.accessToken}`,
        'Content-Type': 'application/json'
      }
    }
    
    const startDateString = startDateValue.toISOString();
    const endDateString = endDateValue.toISOString();
    
    const bodyParameters = {
      subject: name,
      body: {
        content: description ? description : '',
      },
      start: {
        dateTime: startDateString,
        timeZone: selectedTimezone.value
      },
      end: {
        dateTime: endDateString,
        timeZone: selectedTimezone.value
      },
      attendees: [
        ...attendersByGroup.map(person => ({emailAddress: {address: person.Email}})),
        ...attendersByStudents.map(person => ({emailAddress: {address: person.email}}))
      ],
      isOnlineMeeting: true,
      onlineMeetingProvider: "teamsForBusiness"
    }
    
    const url = eventId !== null ? `https://graph.microsoft.com/v1.0/me/events/${eventId}` : 'https://graph.microsoft.com/v1.0/me/events';
    const method = eventId !== null ? 'patch' : 'post';
    axios[method](
      url,
      bodyParameters,
      config
    ).then((response) => {
      setModalFeedbackState('success');
    }).catch((error) => {
      console.error(error);
      setModalFeedbackState('error')
    });
    
  }
  
  const handleOnClose = () => {
    if (modalFeedbackState === 'warning') {
      setIsModalFeedbackVisible(false);
    }
    if (modalFeedbackState === 'error') {
      setIsModalFeedbackVisible(false);
    }
  }
  
  return (
    <>
      <Header/>
      <div className='newMeetingContainer'>
        <Stack className='container-teams' verticalFill>
          <div className="content-button-back">
            <Stack horizontal tokens={{childrenGap: 15}}>
              <StackItem align="end" className="newMeetingButtons">
                <Stack horizontal tokens={{childrenGap: 10}}>
                  <a className="btn-back" onClick={() => history.goBack()}>
                    <FontAwesomeIcon icon={faArrowLeft}/>{" "}
                    <Translate
                      id="general.back-action"
                      options={{renderInnerHtml: false}}
                    />
                  </a>
                </Stack>
              </StackItem>
            </Stack>
          </div>
          <Stack horizontal tokens={{childrenGap: 15}}>
            <StackItem grow>
              {/* <FontIcon iconName="Calendar" className={meetingIconClass} /> */}
              <Text variant="xLarge" styles={boldStyle}>
                {eventId !== null
                  ? <Translate id='teams.schedule-meeting'/> :
                  <Translate id='teams.new-meeting'/>
                }
              </Text>
            </StackItem>
          </Stack>
          
          {/*form*/}
          <div className="wrapper-content-meet mt-4">
            <form onSubmit={handleSubmit(onSubmit)}>
              {/* nane */}
              <div className="form-wizard-fields">
                <label>* <Translate id='teams.meeting-name-title' options={{renderInnerHtml: false}}/>
                </label>
                <input
                  type="text"
                  autoComplete={'off'}
                  className="form-wizard-fields__input"
                  {...register(
                    'name',
                    {
                      required: {
                        value: true,
                        message: <Translate id="ms-teams.subject-title-required"
                                            options={{renderInnerHtml: true}}/>,
                      }
                    })}
                />
                {errors.name
                  ? <p className="content-meet--helptext text-color-red"> {errors.name.message} </p>
                  : <p className="content-meet--helptext">
                    <Translate id={"teams.enter-title-meeting"} options={{renderInnerHtml: true}}/>
                  </p>
                }
              </div>
              
              {/*required attenders per group*/}
              {userRole === Settings.ROLE_TEACHER_STR ? (
              <div className="form-wizard-fields">
                <label><Translate id='teams.add-required-attendees'
                                    options={{renderInnerHtml: false}}/> </label>
                <InputSelectGroup label="* Add required attendees per group"
                                  updateParent={setSelectedGroups}/>
                <p className="content-meet--helptext">
                  <Translate id={"teams.add-student-group"} options={{renderInnerHtml: true}}/>
                </p>
              </div>) : null}
              
              {/*required attenders per student*/}
              <div className="form-wizard-fields">
                <label><Translate id='teams.add-required-attendees-person'
                                    options={{renderInnerHtml: false}}/> </label>
                {isLoaded ? (
                  <InputSelectStudent
                    label="* Add required attendees per student"
                    updateParent={setSelectedStudents}
                    value={attendersByStudents}
                  
                  />
                ) : null
                }
                <p className="content-meet--helptext">
                  {userRole === Settings.ROLE_TEACHER_STR ? (
                  <Translate id={"teams.add-any-students"} options={{renderInnerHtml: true}}/>) :
                      (<Translate id={"teams.teams-add-person-requirements"} options={{renderInnerHtml: true}}/>)
                  }
                </p>
              </div>
              
              {/*date*/}
              <div className="wrapper-content-meet--col">
                <p className="label-dateMeet"> *
                  <Translate id='teams.add-time-from-to' options={{renderInnerHtml: false}}/>
                </p>
                <div className="content-meet--col">
                  <div className="form-wizard-fields">
                    <DatePicker
                      date={startDate ? moment(startDate, "YYYY-MM-DD") : null}
                      updateParent={(value) => setStartDate(value.date)}
                    />
                  </div>
                  
                  <div className="form-wizard-fields">
                    <Select   classNamePrefix="mantine-like"
                              className="mantine-like__control" options={options} onChange={(selected) => setHourStart(selected.value)}
                            value={{value: hourStart, label: hourStart}}/>
                  </div>
                  
                  <div className="date-contentIcon">
                    <FontAwesomeIcon icon={faArrowRight}/>{' '}
                  </div>
                  
                  <div className="form-wizard-fields">
                    <DatePicker
                      date={endDate ? moment(endDate, "YYYY-MM-DD") : null}
                      updateParent={(value) => setEndDate(value.date)}
                    />
                  </div>
                  
                  <div className="form-wizard-fields">
                    <Select  classNamePrefix="mantine-like"
                             className="mantine-like__control" options={options} onChange={(selected) => setHourEnd(selected.value)}
                            value={{value: hourEnd, label: hourEnd}}/>
                  </div>
                
                </div>
                
                <p className={`content-meet--helptext mb-4 ${errorDate ? 'text-color-red' : ''}`}>
                  {errorDate ? <Translate id={`teams.${errorDate}`} options={{renderInnerHtml: true}}/>
                    : <Translate id={"teams.add-date-time-meeting"} options={{renderInnerHtml: true}}/>}
                </p>
              </div>
              
              {/*time-zone*/}
              <div className="form-wizard-fields input-md">
                <label>* <Translate id='teams.add-timezone-meeting' options={{renderInnerHtml: false}}/>
                </label>
                {(timeZones.length > 0 && selectedTimezone !== null) ? (
                  <Select  classNamePrefix="mantine-like"
                           className="mantine-like__control" options={timeZones} onChange={(selected) => setSelectedTimezone(selected)}
                          value={{value: selectedTimezone.label, label: selectedTimezone.label}}
                  />
                
                ) : null}
                <p className="content-meet--helptext mt-1">
                  <Translate id={"teams.add-timezone-meeting"} options={{renderInnerHtml: true}}/>
                </p>
              </div>
              
              {/*description*/}
              <div className="form-wizard-fields fix-textarea">
                <label>
                  <Translate id='teams.meeting-content' options={{renderInnerHtml: false}}/>
                </label>
                <TextAreaTrelloStyled
                  className="form-wizard-fields__input"
                  {...register("description", {
                    required: {
                      value: false,
                    },
                  })}
                />
                <p className="content-meet--helptext">
                  <Translate id={"teams.add-description-meeting-content"}
                             options={{renderInnerHtml: true}}/>
                </p>
              </div>
              
              {/*buttons */}
              
              <div className="content-button-footer-create-meet">
                <Stack horizontal tokens={{childrenGap: 10}}>
                  {eventId !== null
                    ?
                    <DefaultButton
                      className="teamsButtonInverted  btn-outline-danger"
                      onClick={handleCancelMeeeting}
                    
                    >
                      <Translate id="teams.cancel-meeting" options={{renderInnerHtml: true}}/>
                    </DefaultButton>
                    : null
                  }
                  <PrimaryButton className="teamsButton" type="submit">
                    {eventId !== null
                      ? <Translate id='teams.send-update'
                                   options={{renderInnerHtml: true}}/> :
                      
                      <Translate id="teams.create-meeting-button-primary"
                                 options={{renderInnerHtml: true}}/>}
                  </PrimaryButton>
                  
                  <DefaultButton
                    className="teamsButtonInverted"
                    onClick={() => history.goBack()}
                  >
                    <Translate id="teams.cancel" options={{renderInnerHtml: true}}/>
                  </DefaultButton>
                </Stack>
              </div>
            
            </form>
          </div>
        
        </Stack>
        <MeetingDialogFeedback
          isOpen={isModalFeedbackVisible}
          onClose={handleOnClose}
          state={modalFeedbackState}
          cancelMeeting={cancelMeeting}
        
        />
      </div>
    </>
  );
}

const boldStyle = {root: {fontWeight: FontWeights.semibold}};


const TextAreaTrelloStyled = styled.textarea`
    font-size: 13px;
    height: 60px;
    border: 1px solid #d3d5db;
    font-family: "Roboto", Arial, Helvetica, sans-serif;
    padding: 0.5em;
    box-sizing: border-box;
    width: ${(props) => props.width || "100%"};
    margin: 0.5em 0;
    margin-top: 1em;
    margin-bottom: 2.5em;
`;

export default CreateMeeting;