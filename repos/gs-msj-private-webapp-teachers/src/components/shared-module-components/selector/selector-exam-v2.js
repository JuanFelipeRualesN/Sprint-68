import React, {useEffect, useState} from 'react';
import Select from 'react-select';
import {getLessonExams} from "../../../api/athenas";
import { Translate } from "react-localize-redux";

/**
 * Handles the selection of exams for version 2 (after 05/21/2020)
 */
const SelectorExamV2 = ({ lessonId, updateParent }) => {
    const [options, setOptions] = useState([]);

    useEffect(() => {
        if (lessonId) {
            callData(lessonId);
        }
    }, [lessonId]);

    const callData = (lessonId) => {
        getLessonExams(lessonId).then((response) => {
            const dataReturned = response.data.Quizzes;
            const arrOptions = [];

            dataReturned.forEach((quiz) => {
                if (quiz.IsExam === "1") {
                    arrOptions.push({
                        value: quiz.Id,
                        label: quiz.LevelOfKnowledge + ') ' + quiz.Title
                    });
                }
            });

            setOptions(arrOptions);
        });
    };

    const handleChange = (object) => {
        updateParent(object.value);
    };

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            options={options}
            name="exams"
            onChange={handleChange}
            placeholder={<Translate id="report-student-average.select-exam" options={{ renderInnerHtml: false }} />}
        />
    );
};

export default SelectorExamV2;