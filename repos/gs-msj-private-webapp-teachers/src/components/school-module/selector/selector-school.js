/**
 * Created by portiz on 4/30/2020.
 */
import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import {getSchoolsForSimpleSelect} from '../../../api/schools';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Handles the selection of school-module
 */
const SelectorSchool = (props) => {
    const [city] = useState(props.city);
    const [country] = useState(props.country);
    const [disabled] = useState(props.disabled);
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [privateOrPublic] = useState(props.privateOrPublic);
    const [stateValue] = useState(props.state);
    const [value, setValue] = useState(props.value);

    useEffect(() => {
        callData();
    }, [props.redoSearch]);

    // Call the data API
    const callData = () => {
        setOptions([]);
        setLoading(true);

        let apiCall = getSchoolsForSimpleSelect(privateOrPublic, stateValue, country, city)
        let arrOptions = [];

        apiCall.then((response) => {
            for (let i = 0; i < response.data.SchoolEntities.length; i++) {
                arrOptions.push({value: response.data.SchoolEntities[i].EntityId, label: response.data.SchoolEntities[i].EntityName});
            }
            arrOptions.push({value: "", label: "Other/Otro"});
        }).then(() => {
            setLoading(false);
            setOptions(arrOptions);
        });
    };

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (object) => {
        setValue(object.value);
        props.updateParent(object.value);
        return object.value;
    };

    if (loading) {
        return <BeatLoader size={10} color={"#6bc4ff"} />;
    }

    return (
            <div style={{color:'black'}}>
                {
                    options.length > 0 ?
                        <Select
                            classNamePrefix="mantine-like"
                            className="mantine-like__control"
                            value={options.filter(option => option.value === value)[0]}
                            isDisabled={disabled} options={options} name="schools"
                            onChange={handleChange}
                            placeholder={<Translate id="register-teacher.select-school" options={{renderInnerHtml: false}}/>}
                        /> : 
                        null
                }
            </div>
        )
};


export default SelectorSchool;