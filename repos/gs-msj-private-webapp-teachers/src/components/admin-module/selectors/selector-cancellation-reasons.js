/**
 * Added by ET 2021-09-30 cancellation reasons
 */
 import React, { useState, useEffect } from 'react';
 import Select from 'react-select';
 import {getCancellationReasons} from '../../../api/subscriptions';
 /**
  * Handles the selection of cancellationReasons
  */
 const SelectorCancellationReasons = ({ isDisabled, reasonId, setReasonId }) => {
    const [reasons, setReasons] = useState([]);
    const [selectedReason, setSelectedReason] = useState(null);

    const onChange = (option) => {
        setReasonId(option.value);
        setSelectedReason(option);
    };

    useEffect(() => {
        getCancellationReasons().then((response) => {
            var reasons = response.data,
                reasonsOptions = reasons.map((item) => {
                    return {label: item.Reason, value: item.Id};
                });
            setReasons(reasonsOptions);
            if(reasonId != null) {
                setSelectedReason(reasonsOptions.find(e => e.value === reasonId));
            }
        });
    }, [reasonId]);

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            onChange={onChange} 
            value={selectedReason} 
            options={reasons}
            isDisabled={isDisabled}
        />
    );
 };
 
 export default SelectorCancellationReasons;