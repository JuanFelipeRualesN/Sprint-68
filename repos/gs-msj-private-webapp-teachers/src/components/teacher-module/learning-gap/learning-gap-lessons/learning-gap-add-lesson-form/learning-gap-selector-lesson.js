import React, { useState, useEffect } from 'react';
import { getLessonsTeachers } from "../../../../../api/teachers";
import Select from 'react-select';
import LanguageManager from "../../../../../custom-classes/language-manager";
import { Translate } from "react-localize-redux";
import { BeatLoader } from "react-spinners";
import Settings from "../../../../../custom-classes/settings";

/**
 * To handle the selection of lessons for Closing gaps.
 * @param subjectCode The subject code.
 * @param levelCode The level code.
 * @param updateParent The function to update the parent component.
 * @returns {Element} The SelectorLesson component.
 * @constructor
 */
const SelectorLesson = ({ subjectCode, levelCode, updateParent }) => {
    // State management with useState
    const [languages] = useState(Settings.LANGUAGES);
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [lessonTranslation] = useState(
        LanguageManager.getTranslation("learning-gap.select-lesson", languages)
    );

    // Effect to load data on component mount
    useEffect(() => {
        const mountData = async () => {
            setLoading(true);
            try {
                const response = await getLessonsTeachers(subjectCode.toLowerCase(),
                    levelCode.toLowerCase(), "1");
                const arr = response.data;
                const arrOptions = arr.map(item => ({
                    value: item.Id,
                    label: item.LessonTitle,
                }));

                setOptions(arrOptions);
            } catch {
                // Handle errors if necessary
            } finally {
                setLoading(false);
            }
        };

        mountData();
    }, [subjectCode, levelCode]); // Dependencies ensure this effect runs when these values change

    const handleChange = (selectedOption) => {
        updateParent(selectedOption);
    };

    // Render loading state
    if (loading && options.length === 0) {
        return <BeatLoader size={10} color={"#6bc4ff"} />;
    }

    // Render no lessons found state
    if (options.length === 0) {
        return <Translate id="learning-gap.no-lessons-found" />;
    }

    // Render the Select component
    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={lessonTranslation}
            onChange={handleChange}
            options={options}
        />
    );
};

export default SelectorLesson;
