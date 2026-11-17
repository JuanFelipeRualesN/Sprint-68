import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { Translate } from 'react-localize-redux';

// Component that display the planning selector to assign a lesson
const SelectorPlanning = ({plannings, onPlanningSelect}) => {
  const [value, setValue] = useState(null);
  const [options, setOptions] = useState([]);

  useEffect(() => {
    if (plannings && plannings.length > 0) {
      const formattedOptions = plannings.map(planning => ({
        value: planning.PlanId,
        label: planning.PlanName
      }));
      setOptions(formattedOptions);
    }
  }, [plannings]);

  const handleChange = (selectedOption) => {
    setValue(selectedOption);
    if (onPlanningSelect && selectedOption) {
      onPlanningSelect(selectedOption.value);
    }
  }

  return (
    <>
      <span style={{fontWeight: 'bold', fontSize: '.95rem', color: '#2a426d'}}>
        <Translate id="lesson-catalog.select-instructional-plan" options={{ renderInnerHtml: false }} />
      </span>
      <div style={{width: '250px', maxWidth: "100%", marginTop: '10px'}}>
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
          value={value}
          onChange={handleChange}
          options={options}
          placeholder=""
        />
      </div>
    </>
    
  )
}

export default SelectorPlanning;

