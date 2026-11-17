/**
 * Created by portiz on 8/15/2018.
 */
 import React, { useState, useEffect } from 'react';import {getProfileProducts} from "../../../api/subscriptions";
 import Select from 'react-select';
 import {BeatLoader} from "react-spinners";
 import Settings from "../../../custom-classes/settings";
 /**
  * Handles the selection of products
  */
const SelectorStudentProduct = ({data, value, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [studentId, setStudentId] = useState(data.studentId.UserId != null ? data.studentId.UserId : null);
    const [valueState, setValueState] = useState(null);

    useEffect(() => {
        setLoading(false);
        setStudentId(data.studentId.UserId);
        mountData();
    }, [data.studentId.UserId]);

    const mountData = () => {
        setLoading(true);

        let apiCall = getProfileProducts(studentId);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            for (let i = 0; i < response.data.length; i++)
            {
                arrOptions.push(
                    {
                        value: response.data[i].ItemId,
                        label: response.data[i].Name,
                        subjectCode: response.data[i].SubscriptionProductModel?.Subject,
                        levelCode: response.data[i].SubscriptionProductModel?.Level
                    }
                );
            }
        }).then(() => {
            setOptions(arrOptions);
            setLoading(false);
        });
    };

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        setValueState(selectedOption);
        updateParent(selectedOption);
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
        <Select   classNamePrefix="mantine-like"
                  className="mantine-like__control"
                  onChange={handleChange} value={value} options={options}/>
    )
};
 
 export default SelectorStudentProduct;