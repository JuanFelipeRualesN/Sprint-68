import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import {getCountries} from "../../../api/gsweb";
import { Translate } from "react-localize-redux";

/**
 * Handles the selection of products
 */
const SelectorCountry = ({disabled, value, updateParent}) => {
    const [disabledState, setDisabledState] = useState(disabled);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [valueState, setValueState] = useState(value);

    useEffect(() => {
        setDisabledState(disabled);
        setValueState(value);
    }, [disabled, value]);

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        callData();
    }, []);

    // Call the data API
    const callData = () => {
        let apiCall = getCountries();
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
        let dataReturned = response.data;
        // Now fill the selections
        for (let i = 0; i < dataReturned.length; i++) {
            arrOptions.push({ value: dataReturned[i].IsoCode2, label: dataReturned[i].CountryName });
        }
        }).then(() => {
            setOptions(arrOptions);
        });
    };

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        setValueState(object.value);
        updateParent(object.value);
    };
    
    // Renders the item list
    return (
            <div style={{color:'black'}}>
                {
            options.length > 0 ?
            <Select
                classNamePrefix="mantine-like"
                className="mantine-like__control"
                value={options.filter(option => option.value === value)[0]}
                isDisabled={disabled} options={options} name="countries"
                onChange={handleChange} placeholder=
                    {
                        <Translate id="register.select-country" options={{renderInnerHtml: false}}/>
                    }
            /> : null}
            </div>
    )
};

export default SelectorCountry;