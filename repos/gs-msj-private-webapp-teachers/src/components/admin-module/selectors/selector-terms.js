import React, { useState, useEffect } from 'react';
import { getTerms } from "../../../api/subscriptions";
import Select from 'react-select';
import { Translate } from "react-localize-redux";

const SelectorTerms = ({ disabled, value, updateParent }) => {
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [valueState, setValueState] = useState(null);

    useEffect(() => {
        setValueState(value);
    }, [value]);

    const callData = () => {
        let apiCall = getTerms();
        let arrOptions = [];

        apiCall.then((response) => {
            response.data.forEach(term => {
                arrOptions.push({
                    value: term.Id,
                    label: term.SchTerm
                });
            });
        }).then(() => {
            setOptions(arrOptions);
        });
    };

    useEffect(() => {
        callData();
    }, []);

    const handleChange = (object) => {
        const newValue = object.value === "0" ? null : object.value;
        setValueState(newValue);
        updateParent(newValue);
    };

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
                <Translate id="terms.select-terms" options={{ renderInnerHtml: false }} />
            }
        />
    );
};

export default SelectorTerms;