import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";
import {getProducts} from "../../../api/subscriptions";

/**
 * Handles the selection of products
 */
const SelectorProduct = ({ disabled, resetBox, redoSearch, value, updateParent, isCommercial = false }) => {
    const [loading, setLoading] = useState(false);
    const [placeholder] = useState(LanguageManager.getTranslation("product.select", Settings.LANGUAGES));
    const [options, setOptions] = useState([]); // The options to fill the selection

    const callData = () => {
        setLoading(true);
        // This is promise
        let apiCall = getProducts(false, false, isCommercial);
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response) => {
            // Now fill the selections
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({
                    value: response.data[i].Id,
                    label: response.data[i].Name,
                });
            }
        }).then(() => {
            // Then, set the state
            setOptions(arrOptions);
            setLoading(false);
        });
    };

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        callData();
    }, [redoSearch]);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        updateParent(selectedOption);
    };

    if (loading) {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />)
    }

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={placeholder}
            onChange={handleChange}
            value={resetBox ? null : value}
            options={options}
            isDisabled={disabled}
        />
    );
};

export default SelectorProduct;