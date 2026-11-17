/*
*   Create By: Mario Cancel
*/
import {FormGroup, Input } from 'reactstrap';
import React from 'react';
import LanguageManager from '../../../custom-classes/language-manager';
import './dropdowns.css';

//The component for create Dropdowns
const Dropdowns = ({optionObject, onValueChange, selectedValue, disabled}) => {

    return (
        <FormGroup>
            <Input
                disabled={disabled}
                type="select"
                name="select"
                id="exampleSelect"
                onChange={event=> onValueChange(event.currentTarget.value)}
                value={selectedValue}
                className="dropdown-custom" >
                {optionObject.map(obj=>
                    <option
                        key={obj.Id}
                        value={obj.Id}>{obj.Name.split("/")[LanguageManager.getLanguage() !== 'en' ? 1 : 0]}
                    </option>
                )}
            </Input>
        </FormGroup>
    );
};

Dropdowns.defaultProps = {
    selectedValue: null,
    disabled: false,
    optionObject: []
};

export default Dropdowns;