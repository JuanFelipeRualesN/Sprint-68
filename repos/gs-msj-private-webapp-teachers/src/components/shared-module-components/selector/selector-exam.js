/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import {getLessonExams} from "../../../api/athenas";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Handles the selection of products
 */
const SelectorExam = ({data, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [value, setValue] = useState(data.lessonId.value);

    // Handle the updates
    // We cannot find a concrete response why this is needed here meanwhile the other selector components are not needed.
    useEffect(() => {
        setValue(data.lessonId.value);
        callData();
    }, [data.lessonId.value]);

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        callData();
    }, []);

    // Call the data API
    const callData = () => {
        setLoading(true);
        // This is promise
        let apiCall = getLessonExams(value);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            let dataReturned = response.data.Quizzes;

            // Translate the first selection
            arrOptions.push(
                <option key={"0"} value={"0"}>
                    <Translate id="report-student-average.select-exam" options={{renderInnerHtml: false}}/>
                </option>);

            // Now fill the selections
            for (let i = 0; i < dataReturned.length; i++) {
                arrOptions.push({
                        value: dataReturned[i].Id,
                        label: dataReturned[i].LevelOfKnowledge +') ' +  dataReturned[i].Title
                    }
                );
            }
        }).then(() => {
            setOptions(arrOptions);
            setLoading(false);
        });
    };

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        setValue(object.value);
        updateParent(object.value);
    };

    // Renders the item list
    if(loading && options.length === 0)
    {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />)
    }

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            isDisabled={loading} options={options} name="exams"
            onChange={handleChange} placeholder=
                {
                    <Translate id="report-student-average.select-exam" options={{renderInnerHtml: false}}/>
                }
        />
    )
};

export default SelectorExam;