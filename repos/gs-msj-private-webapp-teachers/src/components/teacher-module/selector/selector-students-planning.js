import React, { useState, useEffect } from "react";
import { getStudentPlannings } from "../../../api/teachers";
import Select from "react-select";
import LanguageManager from "../../../custom-classes/language-manager";
/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import { BeatLoader } from "react-spinners";
import Settings from "../../../custom-classes/settings";
import { removeDuplicates } from "../../../utils/utils";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
/** End of Multiple Language imports **/
/**
 * Handles the selection of plannings of students
 */
const SelectorStudentsPlanning = ({studentId, filterByTeacher, value, updateParent}) => {
    const [loading, setLoading] = useState(false);
    const [selectorPlanningTranslation] = useState(
        LanguageManager.getTranslation("progress.select-planning", Settings.LANGUAGES)
    );
    const [options, setOptions] = useState([]); // The options to fill the selection

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Call the webservices for the subscription and fill those selects
    useEffect(() => {
        const loadPlannings = async () => {
            setLoading(true);
            try {
                const response = await getStudentPlannings(studentId, filterByTeacher);
                let arrOptions = [];
                for (let i = 0; i < response.data.length; i++) {
                    arrOptions.push({
                        value: response.data[i].PlanId,
                        label: response.data[i].PlanName,
                    });
                }
                setOptions(removeDuplicates(arrOptions));
                setLoading(false);
            } catch (error) {
                setLoading(false);
            }
        };

        loadPlannings();
    }, [studentId, filterByTeacher]);

    // Handles the change event to get the selected value. Do not change this function
    const handleChange = (selectedOption) => {
        updateParent(selectedOption);
    };

    if (loading && options.length === 0) {
        return <BeatLoader size={10} color={"#6bc4ff"} />;
    }

    if (options.length === 0) {
        return (
            <div>
                <Translate
                    id="progress.no-student-planning"
                    options={{ renderInnerHtml: false }}
                />
            </div>
        );
    }

    return (
        <>
            <Translate id="progress.select-instructional-active-plan" />
            <Select
                classNamePrefix="mantine-like"
                className="mantine-like__control"
                placeholder={selectorPlanningTranslation}
                onChange={handleChange}
                value={value}
                options={options}
            />
        </>
    );
};

export default withLocalize(SelectorStudentsPlanning);
