import React, {useEffect, useState} from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar'
import moment from 'moment'
import LanguageManager from "../../../custom-classes/language-manager";
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import {getTeacherCalendar} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import 'react-big-calendar/lib/css/react-big-calendar.css';

const PlanningCalendar = ({ teacherPlanId, updateParent }) => {
    const [options, setOptions] = useState([]);
    const [loading, setLoading] = useState(false);
    const [value, setValue] = useState(null);

    const  localizer = momentLocalizer(moment);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        //  let languageCode =  LanguageManager.getLanguage();
        // require('moment/locale/'+ languageCode +'.js');
        setLoading(true);

        // This is promise
        let apiCall = getTeacherCalendar(teacherPlanId);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            // Now fill the selections
            for (let i = 0; i < response.data.LessonAssignmentModels.length; i++)
            {
                arrOptions.push({
                    title: response.data.LessonAssignmentModels[i].Title,
                    start: moment(response.data.LessonAssignmentModels[i].DueDate).format('MM/DD/YYYY h:mm:ss'),
                    end: moment(response.data.LessonAssignmentModels[i].DueDate).format('MM/DD/YYYY  h:mm:ss'),
                    allDay: true,
                });
            }

            for (let i = 0; i < response.data.HomeworkAssignmentModels.length; i++)
            {
                arrOptions.push({
                    title: response.data.HomeworkAssignmentModels[i].Title,
                    start: moment(response.data.HomeworkAssignmentModels[i].DueDate).format('MM/DD/YYYY h:mm:ss'),
                    end: moment(response.data.HomeworkAssignmentModels[i].DueDate).format('MM/DD/YYYY  h:mm:ss'),
                    allDay: true,
                });
            }

        }).then(()=>{
            // Then, set the state
            setOptions(arrOptions);
            setLoading(false);
        });
    }, [teacherPlanId]);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        setValue(selectedOption);
        updateParent(selectedOption);
    };

    if (loading && options.length === 0)
    {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />)
    }

    if (options.length === 0)
    {
        return(
            <div>
                <Translate id="calendar.no-details"
                           options={{renderInnerHtml: false}}/>
            </div>
        )
    }

    return (
        <Calendar
            messages={
                {
                    next: LanguageManager.getTranslation("calendar.next", Settings.LANGUAGES),
                    previous:LanguageManager.getTranslation("calendar.previous", Settings.LANGUAGES),
                    today:LanguageManager.getTranslation("calendar.today", Settings.LANGUAGES),
                    month: LanguageManager.getTranslation("calendar.month", Settings.LANGUAGES),
                    week: LanguageManager.getTranslation("calendar.week", Settings.LANGUAGES),
                    day: LanguageManager.getTranslation("calendar.day", Settings.LANGUAGES),
                    date: LanguageManager.getTranslation("calendar.date", Settings.LANGUAGES),
                    time: LanguageManager.getTranslation("calendar.time", Settings.LANGUAGES),
                    event: LanguageManager.getTranslation("calendar.event", Settings.LANGUAGES),
                    allday: LanguageManager.getTranslation("calendar.allday", Settings.LANGUAGES)
                    }}
            localizer={localizer}
            events={options}
            startAccessor="start"
            endAccessor="end"
            style={{ height: 500 }}
        />
    )
};

export default withLocalize(PlanningCalendar);