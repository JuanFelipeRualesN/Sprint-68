import React, {useEffect, useState} from "react";
import Select from "react-select";
import {BeatLoader} from "react-spinners";
import {getTeacherGroups} from "../../../../../api/teachers";
import {getProfileConnectionsByPagination} from "../../../../../api/users";

export const InputSelectGroup = ({updateParent}) => {
  const [isLoading, setIsLoading] = useState(false);
  const [value, setValue] = useState(null);
  
  const [options, setOptions] = useState([]); // The options to fill the selection
  /**
   * Call the webservices for the subscription and fill those select
   * Component did mount is required for selects with API calls.
   */
  
  useEffect(() => {
    setIsLoading(true);
    // This is promise
    let apiCall = getTeacherGroups(50, 0);
    let arrOptions = [];
    
    // After the api call, fill the data
    apiCall.then((response) => {
      
      // Now fill the selections
      for (let i = 0; i < response.data.TeacherGroupModelList.length; i++) {
        arrOptions.push({
          ...response.data.TeacherGroupModelList[i],
          value: response.data.TeacherGroupModelList[i].Id,
          label: response.data.TeacherGroupModelList[i].Name,
        });
      }
    }).then(() => {
      // Then, set the state
      setOptions(arrOptions);
      setIsLoading(false);
    });
  }, []);
  
  /**
   * Handles the change event to get the selected value. Do not change this function
   */
  const handleChange = (selectedOption) => {
    setValue(selectedOption);
    updateParent(selectedOption);
  };
  
  /**
   * Renders the item list
   */
  if (isLoading) {
    return (<BeatLoader
      size={10}
      color={"#6bc4ff"}
    />)
  }
  
  return (
    <div style={{marginBottom: '1rem'}}>
      <Select
          classNamePrefix="mantine-like"
          className="mantine-like__control"
        isMulti
        onChange={handleChange}
        value={value}
        options={options}/>
    </div>
  )
}

export const InputSelectStudent = (props) => {
  const [isLoading, setIsLoading] = useState(false);
  const [value, setValue] = useState(null);
  
  const [options, setOptions] = useState([]); // The options to fill the selection
  
  
  useEffect(() => {
    
    let apiCall = getProfileConnectionsByPagination(0, 250);
    
   let arrOptions = [];
    // After the api call, fill the data
    apiCall.then((response) => {
      // Now fill the selections
      
       arrOptions = response.data.UserInfo
        .filter((user, index, self) => user.Email !== null && user.Email !== undefined && index === self.findIndex((t) => t.UserId === user.UserId))
        .map((user) => ({
          email: user.Email,
          value: user.UserId,
          label: `${user.GivenName} ${user.Surname}`,
        }));
      
    }).then(() => {
      // Then, set the state
      setOptions(arrOptions);
      if (props.value) {
        setValue(arrOptions.filter((item) => props.value.some((item2) => item2.email === item.email)));
        // console.log(arrOptions.filter((item) => props.value.some((item2) => item2.email === item.email))
      }
      
      setIsLoading(false);
    });
  }, []);
  
  /**
   * Handles the change event to get the selected value. Do not change this function
   */
  const handleChange = (selectedOption) => {
    setValue(selectedOption)
    props.updateParent(selectedOption);
  };
  
  /**
   * Renders the item list
   */
  if (isLoading && options.length === 0) {
    return (<BeatLoader
      size={10}
      color={"#6bc4ff"}
    />)
  }
  
  return (
    <div style={{marginBottom: '1rem'}}>
      <Select
          classNamePrefix="mantine-like"
          className="mantine-like__control"
        isMulti
        defaultValue={props.value}
        onChange={handleChange}
        value={value}
        options={options}/>
    </div>
  )
}

