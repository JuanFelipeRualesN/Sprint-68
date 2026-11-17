/**
 * Created by portiz on 05/11/2020.
 */
import React, { useState, useEffect } from 'react';
import { getLessons } from "../../../api/athenas";
import { getLessonsTeachers } from "../../../api/teachers";
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import { Translate } from "react-localize-redux";
import { BeatLoader } from "react-spinners";
import Settings from "../../../custom-classes/settings";
/**
 * Handles the selection of lessons
 */
const SelectorLesson = ({subjectCode, levelCode, isGapClosing, value, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [subjectCodeState, setSubjectCodeState] = useState(subjectCode);
    const [levelCodeState, setLevelCodeState] = useState(levelCode);
    const [isGapClosingState, setIsGapClosingState] = useState(isGapClosing);
    const [lessonTranslation, setLessonTranslation] = useState(LanguageManager.getTranslation("planning.select-lesson", Settings.LANGUAGES));
    const [valueState, setValueState] = useState(null);

    useEffect(() => {
        mountData();
    }, []);

    const mountData = async () => {
        setLoading(true);
        try {
            const response = await getLessonsTeachers(subjectCodeState, levelCodeState, isGapClosingState);
            const arr = response.data;

            let arrOptions = arr.map(item => {
                return {
                    value: item.Id,
                    label: item.LessonTitle
                };
            });
            setOptions(arrOptions);
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const handleChange = (selectedOption) => {
        setValueState(selectedOption);
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
            <Translate id="planning.lessons-not-found" />
        )
    }

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={lessonTranslation}
            onChange={handleChange}
            value={value}
            options={options} />
    )
};

export default SelectorLesson;
