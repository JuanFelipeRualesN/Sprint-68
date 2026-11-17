import  {useState, useEffect, React } from 'react';
import {getTeacherGroups} from "../../../api/teachers";
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import {BeatLoader} from "react-spinners";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of groups
 */
const SelectorMultiGroups = ({ value, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const selectorGroupTranslation = LanguageManager.getTranslation("messages.select-group", Settings.LANGUAGES);
    const noOptionsTranslation = LanguageManager.getTranslation("messages.no-options", Settings.LANGUAGES); // Add this line

    useEffect(() => {
        setLoading(true);
        getTeacherGroups(50, 0).then((response) => {
            const arrOptions = response.data.TeacherGroupModelList.map(group => ({
                value: group.Id,
                label: group.Name,
            }));
            setOptions(arrOptions);
            setLoading(false);
        });
    }, []);

    const handleChange = (selectedOption) => {
        updateParent(selectedOption);
    };

    if (loading) {
        return <BeatLoader size={10} color={"#6bc4ff"} />;
    }

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            placeholder={selectorGroupTranslation}
            isMulti
            onChange={handleChange}
            value={value}
            options={options}
            noOptionsMessage={() => noOptionsTranslation}
        />
    );
};

export default SelectorMultiGroups;