import React, { useState, useEffect } from 'react';
import Select from 'react-select';

const SelectorTermNumber = ({ value, optionsNumber, updateParent, disabled }) => {

    const [options, setOptions] = useState([]);

    useEffect(() => {
        let options = [];
        for (var i = 1; i <= optionsNumber; i++) {
            options.push({ value: i, label: i });
        }
        setOptions(options);
    }, [optionsNumber]);

    const onChange = (option) => {
        updateParent(option.value);
    };

    return (
        <>
            <Select
                classNamePrefix="mantine-like"
                className="mantine-like__control"
                onChange={onChange}
                value={{ value: value, label: value }}
                options={options}
                isDisabled={disabled} />
        </>
    );
};

export default SelectorTermNumber;