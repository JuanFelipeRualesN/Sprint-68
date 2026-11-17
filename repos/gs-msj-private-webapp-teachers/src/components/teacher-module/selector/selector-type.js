import React, { useState } from "react";
import Select from 'react-select';

/**
 * Handles the selection type of lesson
 */
const SelectorType = ({updateParent, typeCode: typeCodeProp}) => {
  const [options] = useState([
    { value: "1", label: "Cierre de brechas" },
    { value: "0", label: "Regular" },
  ]);
  const [value, setValue] = useState(null);
  const [typeCode, setTypeCode] = useState(typeCodeProp ?? null);

  // Handle the change event to get the selected value. Do not change this function
  const handleChange = (selectedOption) => {
    setValue(selectedOption);
    setTypeCode(selectedOption.typeCode);
    updateParent(selectedOption);
    return selectedOption;
  };

  return (
    <div>
      <Select
          classNamePrefix="mantine-like"
          className="mantine-like__control"
        value={value}
        onChange={handleChange}
        options={options}
        placeholder=""
      />
    </div>
  );
};

export default SelectorType;
