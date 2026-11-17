/**
 * Created by portiz on 05/08/2020.
 */
import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import { BeatLoader } from "react-spinners";
import { getSubscriptionCourses } from "../../../api/subscriptions";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of subjects
 */
const SelectorSubject = ({ subjectCode, levelCode, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [subjectTranslation, setSubjectTranslation] = useState("");
    const [subjectCodeState, setSubjectCodeState] = useState(subjectCode ?? null);
    const [levelCodeState, setLevelCodeState] = useState(levelCode ?? null);

    // Call the webservices for the subscription and fill those selects
    useEffect(() => {
        setLoading(true);

        let arrOptions = [];

        let apiCall2 = getSubscriptionCourses();
        apiCall2.then((response) => {
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({
                    subject: response.data[i].Subject,
                    label: response.data[i].SubjectName + ' ' + response.data[i].Level,
                    level: response.data[i].Level,
                    value: i
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
        setSubjectCodeState(selectedOption.subjectCode);
        setLevelCodeState(selectedOption.levelCode);
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
            value={options.filter(option => option.subject === subjectCodeState && option.level === levelCodeState)[0]}
            placeholder={subjectTranslation}
            onChange={handleChange}
            options={options} />
    )
};

export default SelectorSubject;