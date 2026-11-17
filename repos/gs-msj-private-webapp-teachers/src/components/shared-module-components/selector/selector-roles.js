import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import {getRoles} from "../../../api/users";
import { Translate } from "react-localize-redux";
import {BeatLoader} from "react-spinners";
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of the roles
 */
const SelectorRoles = ({roleId, disabled, redoSearch, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [value, setValue] = useState( roleId ?? null);
    const [isDirector, setIsDirector] = useState(SessionManager.getSessionDetails().Roles.indexOf(Settings.ROLE_DIRECTOR_STR) > - 1);

    // Call the webservices for the subscription and fill those selects
    // Component did mount is required for selects with API calls.
    useEffect(() => {
        callData();
    }, [redoSearch]);

    useEffect(() => {
        setValue(roleId);
    }, [roleId]);

    // Call the data API
    const callData = () => {
        setLoading(true);
        // This is promise
        let apiCall = getRoles();
        let arrOptions = [];

        // After the api call, fill the data
        apiCall.then((response)=>{
            // Translate the first selection
            arrOptions.push(
                <option key={"0"} value={"0"}>
                    <Translate id="roles.select-role" options={{renderInnerHtml: false}}/>
                </option>);

            // Now fill the selections
            for (let i = 0; i < response.data.length; i++) {
                if(isDirector){
                    if(Settings.DIRECTOR_ALLOWED_ROLES.indexOf(response.data[i].Name) > -1)
                    {
                        arrOptions.push({
                                value: response.data[i].Id,
                                label: response.data[i].Desc
                            }
                        );
                    }
                }
                else
                {
                    arrOptions.push({
                            value: response.data[i].Id,
                            label: response.data[i].Desc
                        }
                    );
                }
            }
        }).then(() =>{
            setOptions(arrOptions);
            setLoading(false);
        });
    };

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        setValue(object.value);
        updateParent(object.value);
    };

    if (loading)
    {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />)
    }

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            value={options.filter(option => option.value === value)}
            isDisabled={disabled} options={options} name="roles"
            onChange={handleChange} placeholder=
                {
                    <Translate id="roles.select-role" options={{renderInnerHtml: false}}/>
                }
        />
    )
};

export default SelectorRoles;