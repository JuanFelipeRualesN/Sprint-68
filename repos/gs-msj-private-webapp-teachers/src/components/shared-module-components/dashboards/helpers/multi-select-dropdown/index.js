import React, {useState} from "react";
import {Dropdown, DropdownItem, DropdownMenu, DropdownToggle, FormGroup, Input, Label} from "reactstrap";

export const MultiSelectDropdown = ({ options, selectedValues, setSelectedValues, label, placeholder, disabled, emptyMessage }) => {
    const isEmpty = options.length === 0;

    const [dropdownOpen, setDropdownOpen] = useState(false);
    const toggle = () => {
        if (!disabled) {
            setDropdownOpen(prevState => !prevState);
        }
    };

    const handleCheckboxChange = (value) => {
        if (disabled) return;

        setSelectedValues(prev => {
            if (prev.includes(value)) {
                return prev.filter(v => v !== value);
            } else {
                return [...prev, value];
            }
        });
    };

    const getDisplayValue = () => {
        if (isEmpty && emptyMessage) {
            return emptyMessage;
        }
        if (selectedValues.length === 0) {
            return placeholder;
        }
        if (selectedValues.length === 1) {
            const match = options.find(o => o.value === selectedValues[0]);
            return match ? match.label : selectedValues[0];
        }
        return `${selectedValues.length} ${label}(s) seleccionado(s)`;
    };

    return (
        <Dropdown isOpen={dropdownOpen} toggle={toggle} className={"w-100"} disabled={disabled}>
            <DropdownToggle 
                caret 
                color="white"
                style={{cursor: disabled ? 'not-allowed' : 'pointer'}}                          
                className={"w-100 text-left border bg-white form-control-sm text-dark d-flex justify-content-between align-items-center"}
            >
                <span className="text-truncate" style={{ flexGrow: 1 }}>{getDisplayValue()}</span>
            </DropdownToggle>
            <DropdownMenu style={{ maxHeight: '200px', overflowY: 'auto', width: '100%' }}>
                <DropdownItem header>{label}</DropdownItem>
                <DropdownItem divider />
                {isEmpty && emptyMessage && (
                    <DropdownItem style={{color: 'black'}} disabled>{emptyMessage}</DropdownItem>
                )}
                {options.map((option, index) => (
                    <div key={index} className="px-3 py-1 dropdown-item-custom">
                        <FormGroup check className="mb-0">
                            <Label check className="d-flex align-items-center cursor-pointer">
                                <Input
                                    type="checkbox"
                                    checked={selectedValues.includes(option.value)}
                                    onChange={() => handleCheckboxChange(option.value)}
                                    disabled={disabled}
                                    style={{ marginRight: '8px' }}
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