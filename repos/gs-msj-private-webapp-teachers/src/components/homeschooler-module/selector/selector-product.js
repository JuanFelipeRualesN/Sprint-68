/**
 * Created by portiz on 8/15/2018.
 */
import React, { useState, useEffect } from 'react';
import {getUserSubscribedItems} from '../../../api/gsweb';
import Select from 'react-select';
import { Redirect} from "react-router-dom";
import {BeatLoader} from "react-spinners";
/**
 * Handles the selection of products
 */
const SelectorProduct = ({ value, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [sessionValid, setSessionValid] = useState(true);
    const [selectedValue, setSelectedValue] = useState(null);

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        setLoading(true);

        // This is promise
        let apiCall = getUserSubscribedItems();
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            for (let i = 0; i < response.data.Items.length; i++) {
                arrOptions.push({value: response.data.Items[i].ItemId, label: response.data.Items[i].Name});
            }
        }).then(() => {
            setOptions(arrOptions);
            setLoading(false);
        });
    }, []);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        setSelectedValue(selectedOption);
        updateParent(selectedOption);
        return selectedOption;
    };

    if(!sessionValid) {
        return (<Redirect to={{
            pathname: "/"
        }}/>);
    }

    if (loading && options.length === 0) {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />);
    }

    return (
        <Select    classNamePrefix="mantine-like"
                   className="mantine-like__control" onChange={handleChange} value={value} options={options}/>
    );
};

export default SelectorProduct;