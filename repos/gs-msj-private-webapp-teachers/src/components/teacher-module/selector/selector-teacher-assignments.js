/**
 * Created by portiz on 05/11/2020.
 */
import React, { useState, useEffect } from 'react';
import {getTeacherQuizzes} from "../../../api/teachers";
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of teacher assignments
 */
const SelectorTeacherAssignments = ({ value, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [assignmentTranslation] = useState(LanguageManager.getTranslation("planning.select-assignment", Settings.LANGUAGES));

    // Call the webservices for the subscription and fill those selects
    useEffect(() => {
        setLoading(true);
        // This is promise
        let apiCall = getTeacherQuizzes(true);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            // Now fill the selections
            for (let i = 0; i < response.data.QuizModelList.length; i++) {
                arrOptions.push({
                    value: response.data.QuizModelList[i].Id, 
                    label: response.data.QuizModelList[i].Title
                });
            }
        }).then(() => {
            // Then, set the state
            setOptions(arrOptions);
            setLoading(false);
        });
    }, []);

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

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={assignmentTranslation}
            onChange={handleChange}
            value={value}
            options={options}
        />
    );
};

export default SelectorTeacherAssignments;