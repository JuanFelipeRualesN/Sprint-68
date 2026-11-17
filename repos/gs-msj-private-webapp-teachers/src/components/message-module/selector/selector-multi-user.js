import React, { useState, useEffect } from "react";
import {getProfileConnectionsByPagination} from "../../../api/users";
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of students
 */
const SelectorMultiUser = ({ selectedDefault = [], updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [selectedValues, setSelectedValues] = useState([]);

    useEffect(() => {
        setLoading(true);
        getProfileConnectionsByPagination(0, 160).then((response) => {
            let fetchedOptions = response.data.UserInfo
                .map(user => ({
                    value: user.UserId,
                    label: `${user.GivenName} ${user.Surname}`
                }))
                .sort((a, b) => a.label.localeCompare(b.label));
            setOptions(fetchedOptions);

            // Pre-select default values
            let defaultSelected = fetchedOptions.filter(option => selectedDefault.includes(option.value));
            setSelectedValues(defaultSelected);
            setLoading(false);
        }).then(() => {
            setLoading(false);
        }).catch(() => {
            setLoading(false);
        });
    }, []); // Runs once on mount

    const handleChange = (selectedOptions) => {
        if (selectedOptions.some(option => option.value === "select_all")) {
            if (selectedValues.length !== options.length - 1) {
                // Select all except "Select All"
                setSelectedValues(options.slice(1));
                updateParent(options.slice(1));
            } else {
                // Clear all selections
                setSelectedValues([]);
                updateParent([]);
            }
        } else {
            setSelectedValues(selectedOptions);
            updateParent(selectedOptions);
        }
    };

    if (loading) {
        return <BeatLoader size={10} color={"#6bc4ff"} />;
    }

    return (
        <Select
            placeholder={LanguageManager.getTranslation("messages.select-students", Settings.LANGUAGES)}
            isMulti
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            name="contacts"
            options={options}
            value={selectedValues}
            onChange={handleChange}
        />
    );
};

export default SelectorMultiUser;