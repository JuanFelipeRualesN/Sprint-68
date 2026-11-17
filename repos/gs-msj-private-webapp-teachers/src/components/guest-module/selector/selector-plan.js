import React, { useState, useEffect } from 'react';
import {getCommercialPlans} from '../../../api/subscriptions';
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of plans
 */
const SelectorPlan = ({ disabled = false, selectedPlan, callPromo, updateParent, isCommercial = false }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]); // The options to fill the selection
    const [subjectTranslation] = useState(
        isCommercial ? LanguageManager.getTranslation("commercial.select-the-plan", Settings.LANGUAGES) :
            LanguageManager.getTranslation("subject.subject-and-grade", Settings.LANGUAGES));
    const [selectedPlanState, setSelectedPlanState] = useState(selectedPlan ?? null);
    const [callPromoState] = useState(callPromo ?? false);
    console.log("Selector disabled", disabled);

    useEffect(() => {
        setLoading(true);

        let apiCall = getCommercialPlans(callPromoState);
        let arrOptions = [];

        apiCall.then((response) => {
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({
                    value: response.data[i].Id, 
                    label: response.data[i].PlanDescriptionByLang[0].Name, 
                    interval: response.data[i].Interval
                });
            }
        }).then(() => {
            setOptions(arrOptions);
            setLoading(false);
        });
    }, [callPromoState]);

    // Handles the change event to get the selected value. Do not change this function

    const handleChange = (selectedOption) => {
        setSelectedPlanState(selectedOption.value);
        updateParent(selectedOption);
        return selectedOption;
    };

    if (loading && options.length === 0) {
        return(<BeatLoader
            size={10}
            color={"#6bc4ff"}
        />);
    }

    return (
        (options !== null && options.length > 0) ? (
        <Select
            isDisabled={disabled}
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={subjectTranslation}
            onChange={handleChange}
            value={options.filter(option => option.value == selectedPlanState)}
            options={options}
        />) : (
           <></>
        )
    );
};

export default SelectorPlan;