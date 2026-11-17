import React, { useState, useEffect } from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import {getTaxes} from "../../../api/subscriptions";
import ListItem from '@material-ui/core/ListItem';
import {Col, Row} from 'reactstrap';

/**
 * Handle the checkboxes of the taxes
 */
const CheckboxTaxes = ({ disabled, selected, updateParent }) => {

    const [checkboxes, setCheckboxes] = useState([]);
    const [checkedItems, setCheckedItems] = useState(new Map());
    const [checkedValues, setCheckedValues] = useState(
        typeof selected !== 'undefined' && selected !== null ? selected : []
    );

    useEffect(() => {
        setCheckedValues(
            selected !== null && typeof selected !== 'undefined' ? selected : []
        );
    }, [selected]);

    // Mounts the data. Call the data from the server and sets the data to the list
    const mountData = () => {
        // This is a promise
        let apiCall = getTaxes();

        // After the api call, fill the data
        apiCall.then((response) => {
            let options = [];
            let dataReturned = response.data;

            // Now fill the selections
            for (let i = 0; i < dataReturned.length; i++) {
                let name = dataReturned[i].TaxName;
                if (dataReturned[i].StateIso === null) // State is optional.
                    name += ' (' + dataReturned[i].CountryIso + ')';
                else
                    name += ' (' + dataReturned[i].StateIso + ', ' + dataReturned[i].CountryIso + ')';

                options.push({
                    name: name,
                    key: dataReturned[i].Id,
                    label: dataReturned[i].Id,
                    checked: checkedValues.indexOf(dataReturned[i].Id) > - 1
                });
            }
            return options;
        }).then((options) => {
            setCheckboxes(options);
        });
    };

    // Action when the component will mount
    useEffect(() => {
        mountData();
    }, []);

    // Removes a value from array
    const removeFromArray = (array, xToRemove) => {
        return array.filter(item => item !== xToRemove);
    };

    // Add a value to an array
    const insertInArrayIfNotExists = (array, xToAdd) => {
        if (array.indexOf(xToAdd) > -1)
            return array;
        else {
            return [...array, xToAdd];
        }
    };

    // Action when the checkbox changes
    const handleChange = (e) => {
        const isChecked = e.currentTarget.checked;
        const value = e.currentTarget.value;

        const newCheckedItems = new Map(checkedItems);
        newCheckedItems.set(value, isChecked);
        setCheckedItems(newCheckedItems);

        let newArray = [];
        if (isChecked) {
            newArray = insertInArrayIfNotExists(checkedValues, value);
        } else {
            newArray = removeFromArray(checkedValues, value);
        }

        setCheckedValues(newArray);
        updateParent(newArray);
    };

    return (
        <Row>
            {
                checkboxes.map(item => (
                    <Col className="tax-space" sm={{size: 6}} key={item.key}>
                        <ListItem key={item.key} dense button>
                            <Checkbox name={item.name}
                                      value={item.key}
                                      checked={checkedValues.indexOf(item.key) > -1}
                                      onChange={handleChange}
                                      disabled={disabled}
                            />
                            <small><label key={item.key}> {item.name} </label></small>
                        </ListItem>
                    </Col>
                ))
            }
        </Row>
    );
};

export default CheckboxTaxes;