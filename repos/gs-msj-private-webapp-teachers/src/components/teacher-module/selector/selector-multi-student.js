import React, { useEffect, useState } from "react";
import Select from 'react-select';
import LanguageManager from "../../../custom-classes/language-manager";
import {BeatLoader} from "react-spinners";
import {callUsersThatCanBeHandled} from "../../../api/users";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the selection of students
 */
const SelectorMultiStudent = ({updateParent, value}) => {
  const [loading, setLoading] = useState(false);
  const [selectorStudentTranslation] = useState(LanguageManager.getTranslation("messages.select-students", Settings.LANGUAGES));
  const [options, setOptions] = useState([]); // The options to fill the selection

  useEffect(() => {
    setLoading(true);
    let apiCall = callUsersThatCanBeHandled();
    let arrOptions = [];
    
    // After the api call, fill the data
    apiCall.then((response) => {
        // Now fill the selections
        for (let i = 0; i < response.data.length; i++) {
          arrOptions.push({
            value: response.data[i].UserId,
            label: response.data[i].FirstName + " " + response.data[i].LastName,
          });
        }
      })
      .then(() => {
        // Then, set the state
        setOptions(arrOptions);
        setLoading(false);
      });
  }, []);

  // Handles the change event to get the selected value. Do not change this function
  const handleChange = (selectedOption) => {
    updateParent(selectedOption);
  };

  if (loading && options.length === 0) {
    return <BeatLoader size={10} color={"#6bc4ff"} />;
  }

  return (
    <Select
        classNamePrefix="mantine-like"
        className="mantine-like__control"
      placeholder={selectorStudentTranslation}
      isMulti
      onChange={handleChange}
      value={value}
      options={options}
    />
  );
};

export default SelectorMultiStudent;