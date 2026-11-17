import React, { useState } from "react";
import { Dropdown, DropdownItem, DropdownMenu, DropdownToggle, FormGroup, Input, Label } from "reactstrap";
import "./style.css";

// Generic single-select dropdown component used for regions
export const SingleSelectDropdown = ({ options = [], selectedValue, onSelect, label, placeholder, disabled }) => {
    const [dropdownOpen, setDropdownOpen] = useState(false);

    const toggle = () => {
        if (!disabled) {
            setDropdownOpen((prev) => !prev);
        }
    };

    const handleSelect = (option) => {
        if (disabled) return;
        if (String(selectedValue) === String(option.value)) {
            onSelect(null, null);
        } else {
            onSelect(option.value, option.label);
            setDropdownOpen(false);
        }
    };

    const getDisplayValue = () => {
        if (selectedValue == null) return placeholder;
        const match = options.find((o) => String(o.value) === String(selectedValue));
        return match ? match.label : placeholder;
    };

    return (
        <Dropdown isOpen={dropdownOpen} toggle={toggle} className="ssd-dropdown" disabled={disabled}>
            <DropdownToggle
                caret
                color="white"
                className={`ssd-toggle form-control-sm${disabled ? " ssd-disabled" : ""}`}
            >
                <span className="ssd-toggle-label">{getDisplayValue()}</span>
            </DropdownToggle>
            <DropdownMenu className="ssd-menu">
                <DropdownItem header>{label}</DropdownItem>
                <DropdownItem divider />
                {options.map((option, index) => (
                    <div key={`${option.value}-${index}`} className="ssd-option dropdown-item-custom">
                        <FormGroup check className="mb-0">
                            <Label check className="ssd-option-label">
                                <Input
                                    type="checkbox"
                                    className="ssd-checkbox"
                                    checked={String(selectedValue) === String(option.value)}
                                    onChange={() => handleSelect(option)}
                                    disabled={disabled}
                                />
                                {option.label}
                            </Label>
                        </FormGroup>
                    </div>
                ))}
            </DropdownMenu>
        </Dropdown>
    );
};
