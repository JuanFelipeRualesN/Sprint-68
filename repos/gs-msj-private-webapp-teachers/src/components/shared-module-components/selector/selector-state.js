import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { getStateProvinces } from "../../../api/address";
import { Translate } from "react-localize-redux";
import { BeatLoader } from "react-spinners";

const SelectorState = ({ data, value: valueProp, disabled = false, updateParent }) => {
    const country = data?.country;
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [value, setValue] = useState(valueProp ?? data?.stateRegion ?? null);

    // Keep the selected value in sync with the parent
    useEffect(() => {
        setValue(valueProp ?? data?.stateRegion ?? null);
    }, [valueProp, data?.stateRegion]);

    // Fetch states whenever the country changes
    useEffect(() => {
        if (!country || country === "0") {
            setOptions([]);
            return;
        }

        let cancelled = false;
        setLoading(true);

        getStateProvinces(country)
            .then((response) => {
                if (cancelled) return;
                const list = Array.isArray(response.data) ? response.data : [];
                setOptions(list.map(s => ({ value: s.IsoCode, label: s.Name })));
            })
            .catch((err) => {
                if (!cancelled) {
                    console.error("Failed to load states", err);
                    setOptions([]);
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => { cancelled = true; };
    }, [country]);

    const handleChange = (object) => {
        setValue(object.value);
        updateParent(object.value);
    };

    if (loading) {
        return <BeatLoader size={10} color={"#6bc4ff"} />;
    }

    if (options.length === 0) return null;

    return (
        <div style={{ color: 'black' }}>
            <Select
                classNamePrefix="mantine-like"
                className="mantine-like__control"
                value={options.find(o => o.value === value) ?? null}
                isDisabled={disabled}
                options={options}
                name="states"
                onChange={handleChange}
                placeholder={<Translate id="register.select-state" options={{ renderInnerHtml: false }} />}
            />
        </div>
    );
};

export default SelectorState;