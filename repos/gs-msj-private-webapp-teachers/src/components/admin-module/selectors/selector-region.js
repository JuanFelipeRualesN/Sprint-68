/**
 * Created by portiz on 11/20/2020.
 */
import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import {getRegions} from "../../../api/schools";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Handles the selection of regions
 */
const SelectorRegion = ({ disabled, value, countryIso, redoSearchRegion, updateParent }) => {
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [valueState, setValueState] = useState(value);

    useEffect(() => {
        setValueState(value);
    }, [value]);

    // Call the data API
    const callData = () => {
        // This is promise
        let apiCall = getRegions(countryIso);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            // Now fill the selections
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({
                    value: response.data[i].Id, 
                    label: response.data[i].Name
                });
            }
        }).then(() => {
            setOptions(arrOptions);
        });
    };

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        if (countryIso) {
            callData();
        }
    }, [countryIso, redoSearchRegion]);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        setValueState(object.value);
        updateParent(object.value);
    };

    // Renders the item list
    return (
        <div style={{color:'black'}}>
            {options.length > 0 ?
                <Select
                    classNamePrefix="mantine-like"
                    className="mantine-like__control"
                    value={options.filter(option => option.value === valueState)[0]}
                    isDisabled={disabled} 
                    options={options} 
                    name="region"
                    onChange={handleChange} 
                    placeholder={
                        <Translate id="register.select-region" options={{renderInnerHtml: false}}/>
                    }
                /> : null
            }
        </div>
    );
};

export default SelectorRegion;