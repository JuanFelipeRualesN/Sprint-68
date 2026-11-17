/**
 * Created by portiz on 1/28/19.
 */
import React, { useState, useEffect } from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import {getCountryFullDetails} from "../../../api/address";
import {Col, Row, Input} from 'reactstrap';
import {Translate} from "react-localize-redux";

/**
 * Handle the checkboxes of the countries isos
 */
const CheckboxCountries = ({ checkedRegionsValues,  checkedValues,  disabled,  planAvailableIsos,  selectAll,  updateParent}) => {
    const [checkboxes, setCheckboxes] = useState([]);
    const [checkedItems] = useState(new Map());
    const [currentCheckedRegionsValues] = useState(checkedRegionsValues ?? []);
    const [currentCheckedValues, setCurrentCheckedValues] = useState(checkedValues ?? []);
    const [currentDisabled] = useState(disabled);
    const [mounted, setMounted] = useState(false);
    const [currentPlanAvailableIsos] = useState(planAvailableIsos ?? []);
    const [currentSelectAll, setCurrentSelectAll] = useState(selectAll ?? false);
    const [showAll, setShowAll] = useState(false);

    //Sets the component to receive props
    useEffect(() => {
        let totalOfRegions = 0;
        let totalOfRegionsChecked = 0;
        let totalOfCountries = 0;
        let totalOfCountriesChecked = 0;

        // This is promise
        let apiCall = getCountryFullDetails(true, true);

        // After the api call, fill the data
        apiCall.then((res) => {
        let options = [];
        let dataReturned = res.data;

        // Now fill the selections
        try {
            for (let i = 0; i < dataReturned.length; i++) {
            totalOfCountries += 1; // Sum this total, only to compare if all the countries are selected
            let countrySelected = false; // To evaluate if the country was selected
            if (currentCheckedValues.indexOf(dataReturned[i].IsoCode1) > -1
                || currentSelectAll) // We check if the country was selected or the selected all checkmark is selected
            {
                totalOfCountriesChecked += 1; // Sum this total of selected contries to compare if all the countries were selected
                countrySelected = true; // Sets true as the country was selected.
                currentCheckedValues.push(dataReturned[i].IsoCode1); // Add to the checked values the country added.
                checkedItems.set(dataReturned[i].IsoCode1, true); // This is to mark the country as checked (UI)
            }

            let optionToAdd = {
                name: dataReturned[i].CountryNameTrans[0].Desc,
                key: dataReturned[i].IsoCode1,
                label: dataReturned[i].IsoCode1,
                checked: countrySelected,
                regions: []
            };

            // Now evaluate the regions and if any of them were marked
            let optionsRegions = [];
            for (let j = 0; j < dataReturned[i].Regions.length; j++) {
                totalOfRegions += 1; // Sum the total of regions
                if (currentCheckedRegionsValues.indexOf(dataReturned[i].Regions[j].IsoCode + '|' + dataReturned[i].IsoCode1) > -1
                || currentSelectAll) // Check if we have to add the region as selected.
                {
                totalOfRegionsChecked += 1; // Sum to the total of selected regions
                optionsRegions.push({
                    name: dataReturned[i].Regions[j].Name,
                    key: dataReturned[i].Regions[j].IsoCode + '|' + dataReturned[i].IsoCode1,
                    label: dataReturned[i].Regions[j].IsoCode + '|' + dataReturned[i].IsoCode1,
                    checked: true
                });
                } else {
                optionsRegions.push({
                    name: dataReturned[i].Regions[j].Name,
                    key: dataReturned[i].Regions[j].IsoCode + '|' + dataReturned[i].IsoCode1,
                    label: dataReturned[i].Regions[j].IsoCode + '|' + dataReturned[i].IsoCode1,
                    checked: false
                });
                }
                optionToAdd.regions = optionsRegions;
            }
            options.push(optionToAdd); // Push it, so it does exists in the array already (don't change this position)
            }
        }
        catch (e) {
            // Ignore
        }
            setCheckboxes(options);
            setCurrentSelectAll(totalOfRegions === totalOfRegionsChecked && totalOfCountries === totalOfCountriesChecked);
            updateParent({
                checkboxes: options,
                checkedItems,
                checkedRegionsValues: currentCheckedRegionsValues,
                checkedValues: currentCheckedValues,
                disabled: currentDisabled,
                mounted: true,
                planAvailableIsos: currentPlanAvailableIsos,
                selectAll: totalOfRegions === totalOfRegionsChecked && totalOfCountries === totalOfCountriesChecked,
                showAll: false
            });
        });
    }, []);

    // Action when the checkbox changes
    const handleChange = (e) => {
        addCountry(e.currentTarget.value, e.currentTarget.checked);
    };

    // To remove multiple regions when a country is selected
    const removeRegions = (countryIso) => {
        for(let i=0; i < checkboxes.length;i++)
        {
        if(checkboxes[i].key === countryIso) {
            for (let j = 0; j < checkboxes[i].regions.length; j++) {
            changeRegion(checkboxes, checkboxes[i].regions[j].key, false);
            }
            break;
        }
        }
        updateParent({
            checkboxes,
            checkedItems,
            checkedRegionsValues: currentCheckedRegionsValues,
            checkedValues: currentCheckedValues,
            disabled: currentDisabled,
            mounted,
            planAvailableIsos: currentPlanAvailableIsos,
            selectAll: currentSelectAll,
            showAll
        });
    };

    // Adds multiple regions when country is selected
    const addRegions = (countryIso) => {
        for(let i=0; i < checkboxes.length;i++)
        {
        if(checkboxes[i].key === countryIso) {
            for (let j = 0; j < checkboxes[i].regions.length; j++) {
            changeRegion(checkboxes, checkboxes[i].regions[j].key, true);
            }
            break;
        }
        }
        updateParent({
            checkboxes,
            checkedItems,
            checkedRegionsValues: currentCheckedRegionsValues,
            checkedValues: currentCheckedValues,
            disabled: currentDisabled,
            mounted,
            planAvailableIsos: currentPlanAvailableIsos,
            selectAll: currentSelectAll,
            showAll
        });
    };

    // Adds the country when the value is checked
    const addCountry = (value, isChecked) => {
        // First, we set the checked items to true or false
        checkedItems.set(value, isChecked);

        // Second, in the checkboxes array, update the value
        for(let i=0; i < checkboxes.length; i++)
        {
        if(checkboxes[i].key === value)
        {
            if(isChecked)
            {
            // Add it to the array if doesn't exist
            currentCheckedValues.push(value);
            // Add the regions
            addRegions(value);
            }
            else
            {
            // Remove
            let index = currentCheckedValues.indexOf(value);
            if (index > -1)
                currentCheckedValues.splice(index, 1);
            removeRegions(value);
            }

            // Finally, update the checkbox checked value
            checkboxes[i].checked = isChecked;
        }
        }
        setCheckboxes([...checkboxes]); // Force
        updateParent({
            checkboxes,
            checkedItems,
            checkedRegionsValues: currentCheckedRegionsValues,
            checkedValues: currentCheckedValues,
            disabled: currentDisabled,
            mounted,
            planAvailableIsos: currentPlanAvailableIsos,
            selectAll: currentSelectAll,
            showAll
        });
    };

     //Handles the show all checkbox
    const handleShowAll = (e) => {
        setShowAll(e.currentTarget.checked);
    };

     // Generic function when a function is selected or unselected.
    const changeRegion = (parentArray, value, isChecked) => {
        let countryCode = value.substr(value.indexOf("|") + 1);  // Gets the text part
        for(let i=0; i < parentArray.length; i++)
        {
        if(parentArray[i].key === countryCode){
            for(let j=0; j < parentArray[i].regions.length;j++)
            {
            if(parentArray[i].regions[j].key === value)
            {
                if(isChecked)
                currentCheckedRegionsValues.push(value);
                else
                {
                // remove it from index
                let index = currentCheckedRegionsValues.indexOf(value);
                if (index > -1) {
                    currentCheckedRegionsValues.splice(index, 1);
                }
                }
                parentArray[i].regions[j].checked = isChecked;
                break;
            }
            }
            break;
        }
        }
        setCheckboxes([...parentArray]); // Force a replace
        updateParent({
            checkboxes: parentArray,
            checkedItems,
            checkedRegionsValues: currentCheckedRegionsValues,
            checkedValues: currentCheckedValues,
            disabled: currentDisabled,
            mounted,
            planAvailableIsos: currentPlanAvailableIsos,
            selectAll: currentSelectAll,
            showAll
        });
    };


    // Handles when a region is selected
    const handleChangeRegion = (e) => {
        changeRegion(checkboxes, e.currentTarget.value, e.currentTarget.checked);
    };

    return (
        <div>
        <Row>
            <Col sm={{size:10, offset:1}} md={{size:10, offset:1}} lg={{size:10, offset:1}}>
            <Col sm={{size:12}} md={{size:12}} lg={{size:12}}>
            <Input type="checkbox" onChange={handleShowAll} checked={showAll}/>
            <Translate id="iso-codes-applier.show-all" />
            </Col>
            </Col>
        </Row>
        {
            showAll ?
            <Row>
            {
                checkboxes.map(item => (
                <Col sm={{size: 12}} md={{size:12}} lg={{size:12}} key={item.key} id='space-checkbox-country' >
                        <Checkbox name={item.name}
                                    value={item.key}
                                    disabled={currentDisabled}
                                    checked={checkedItems.get(item.key)}
                                    onChange={handleChange} />
                                <small><label key={item.key + item.name}> {item.name} </label></small>
                        {
                            item.regions.length > 0 ?
                            item.regions.map(itemRegions => (
                                    <Col xs={{size: 10, offset: 1}}
                                        sm={{size: 10, offset: 1}}
                                        md={{size: 10, offset: 1}}
                                        lg={{size: 10, offset: 1}}
                                        key={itemRegions.key}>
                                        <Checkbox name={itemRegions.name}
                                                    value={itemRegions.key}
                                                    checked={itemRegions.checked}
                                                    onChange={handleChangeRegion}
                                                    disabled={currentDisabled}
                                        />
                                        <small>
                                            <label key={itemRegions.key + itemRegions.name}>
                                                {itemRegions.name}
                                            </label>
                                        </small>
                                    </Col>
                                ))
                                : null
                        }
                </Col>
                ))
            }
            </Row> : null }
        </div>
    );
};


export default CheckboxCountries;