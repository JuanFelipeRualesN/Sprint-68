/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';import Select from 'react-select';
import {BeatLoader} from "react-spinners";
import {getItemProductCourseInterface} from "../../../api/subscriptions";
import {getLessons} from "../../../api/athenas";
import Settings from "../../../custom-classes/settings";
/**
 * Handles the selection of products
 */
const SelectorLesson = ({levelCode, subjectCode, data, value, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [levelCodeState, setLevelCodeState] = useState(levelCode);
    const [subjectCodeState, setSubjectCodeState] = useState(subjectCode);
    const [valueState, setValueState] = useState(value);

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
       mountData();
       setValueState(null);
    }, [data.productId.value]);

    const mountData = () => {
        setLoading(true);

        if(typeof subjectCodeState !== 'undefined' && subjectCodeState !== null && typeof levelCodeState !== 'undefined' && levelCodeState !== null){
            let apiCallLessons = getLessons(subjectCodeState, levelCodeState);
            let arrOptions = [];
            // After the api call, fill the data
            apiCallLessons.then((response)=>{
                if(response.data.LessonModelList !== null) {
                    let dataReturned = response.data.LessonModelList;

                    // Now fill the selections
                    for (let i = 0; i < dataReturned.length; i++) {
                        arrOptions.push({
                            value: dataReturned[i].Id, label: dataReturned[i].LessonTitle
                        });
                    }
                }
            }).then(() =>{
                setOptions(arrOptions);
                setLoading(false);
            });
        }
        else {
            let apiCallInterface = getItemProductCourseInterface(data.productId.value);
            apiCallInterface.then((response) => {

                // Call Athenas Lessons
                let apiCallLessons = getLessons(response.data.Subject, response.data.Level);
                let arrOptions = [];
                // After the api call, fill the data
                apiCallLessons.then((response) => {
                    if (response.data.LessonModelList !== null) {
                        let dataReturned = response.data.LessonModelList;

                        // Now fill the selections
                        for (let i = 0; i < dataReturned.length; i++) {
                            arrOptions.push({
                                value: dataReturned[i].Id, label: dataReturned[i].LessonTitle
                            });
                        }
                    }
                }).then(() => {
                    setOptions(arrOptions);
                    setLoading(false);
                });

            });
        }
    };

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        setValueState(selectedOption);
        updateParent(selectedOption);
    };

    // Renders the item list
    if (loading && options.length === 0)
    {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />)
    }

    return (
        <Select  classNamePrefix="mantine-like"
                 className="mantine-like__control" onChange={handleChange} value={valueState} options={options}/>
    )
};

export default SelectorLesson;