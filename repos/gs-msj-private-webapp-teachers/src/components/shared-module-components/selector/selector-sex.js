import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { Translate } from "react-localize-redux";


/**
 * Handles the selection of products
 */
const SelectorSex = ({data, value, updateParent}) => {
    const [options] = useState([
        {value: 'F', label: <Translate id="profile-register.female" options={{renderInnerHtml: false}}/>},
        {value: 'M', label: <Translate id="profile-register.male" options={{renderInnerHtml: false}}/>},
        {value: 'O', label: <Translate id="profile-register.other" options={{renderInnerHtml: false}}/>},
        {value: 'U', label: <Translate id="profile-register.unknown" options={{renderInnerHtml: false}}/>}
    ]); // The options to fill the selection
    const [valueState, setValueState] = useState(data.sex);

    useEffect(() => {
        setValueState(value);
    }, [value]);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        setValueState(object.value);
        updateParent(object.value);
    };

    // Renders the item list
    return (
        <div>

            {
                options.length > 0 ?
                    <Select
                        classNamePrefix="mantine-like"
                        className="mantine-like__control"
                        value={options.filter(option => option.value === valueState)[0]}
                        isDisabled={false} options={options} name="countries"
                        onChange={handleChange} placeholder=
                            {
                                <Translate id="profile-register.select-sex" options={{renderInnerHtml: false}}/>
                            }
                    /> : null}
        </div>
    )
};

export default SelectorSex;