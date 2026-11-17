import React, { useState, useEffect } from 'react';
import {getTeacherPlansSameSchool} from "../../../api/teachers";
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faExclamationTriangle} from "@fortawesome/free-solid-svg-icons/faExclamationTriangle";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/** End of Multiple Language imports **/
/**
 * Handles the selection of teachers planning
 */
const SelectorTeacherPlanning = ({ teacherUserId, filterByTeacher, value, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [selectorPlanningTranslation] = useState(LanguageManager.getTranslation("progress.select-planning", Settings.LANGUAGES));
    const [options, setOptions] = useState([]); // The options to fill the selection

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Call the webservices for the subscription and fill those selects
    useEffect(() => {
        setLoading(true);
        // This is promise
        let apiCall = getTeacherPlansSameSchool(teacherUserId);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            // Now fill the selections
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({
                    value: response.data[i].PlanId,
                    label: response.data[i].PlanName,
                    students: response.data[i].PlanId
                });
            }
        }).then(() => {
            // Then, set the state
            setOptions(arrOptions);
            setLoading(false);
        });
    }, [teacherUserId]);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        updateParent(selectedOption);
    };

    if (loading && options.length === 0) {
        return (<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />)
    }

    if (options.length === 0) {
        return (
            <div className="alert-message">
                <FontAwesomeIcon icon={faExclamationTriangle} />{' '}
                <Translate id="calendar.no-calendar"
                           options={{renderInnerHtml: false}}/>
            </div>
        )
    }

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={selectorPlanningTranslation}
            styles={{
                // Fixes the overlapping problem of the component
                menu: provided => ({ ...provided, zIndex: 9999 })
            }}
            onChange={handleChange}
            value={value}
            options={options}
        />
    );
};

export default withLocalize(SelectorTeacherPlanning);