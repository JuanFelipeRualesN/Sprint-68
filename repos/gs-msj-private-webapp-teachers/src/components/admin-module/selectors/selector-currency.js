/**
 * Created by portiz 2019/01/23.
 */
import React, { useState, useEffect } from 'react';
import {getCurrencies} from "../../../api/subscriptions";
import Select from 'react-select';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Handles the selection of the available currencies
 */
const SelectorCurrency = ({ disabled, value, updateParent }) => {
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [valueState, setValueState] = useState(null);

    useEffect(() => {
        setValueState(value);
    }, [value]);

    // Call the data API
    const callData = () => {
        // This is promise
        let apiCall = getCurrencies();
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            // Now fill the selections
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({ 
                    value: response.data[i].IsoCode,
                    label: response.data[i].IsoCode 
                });
            }
        }).then(() => {
            setOptions(arrOptions);
        });
    };

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        callData();
    }, []);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        const newValue = object.value === "0" ? null : object.value;
        setValueState(newValue);
        updateParent(newValue);
    };

    // Renders the item list
    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            value={options.filter(option => option.value === valueState)} 
            isDisabled={disabled} 
            options={options} 
            name="plans" 
            onChange={handleChange} 
            placeholder={
                <Translate id="currency.select-currency" options={{renderInnerHtml: false}}/>
            }
        />
    );
};

export default SelectorCurrency;