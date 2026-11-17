import React, { useState, useEffect, useCallback, useRef } from "react";
import { Dropdown, DropdownItem, DropdownMenu, DropdownToggle, FormGroup, Input, Label } from "reactstrap";
import { BeatLoader } from "react-spinners";
import { getEntitiesSchool } from "../../../../../api/schools";
import "./style.css";

// Single-select school component to fetch and select a school.
export const SchoolSelectDropdown = ({ selectedId, selectedName, onSelect, label, placeholder, disabled, translate }) => {
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [options, setOptions] = useState([]);
    const [search, setSearch] = useState("");
    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);
    const [loading, setLoading] = useState(false);
    const limiter = 10

    const searchRef = useRef("");

    const toggle = () => {
        if (!disabled) {
            setDropdownOpen((prev) => !prev);
        }
    };

    // Fetch a page of schools
    const fetchSchools = useCallback(async (pageToLoad, append) => {
        setLoading(true);
        try {
            const name = searchRef.current.trim() === "" ? null : searchRef.current.trim();
            const res = await getEntitiesSchool(null, name, null, null, null, pageToLoad, limiter);
            const entities = res.data?.SchoolEntities ?? [];
            const totalPages = res.data?.PageTotals ?? 0;

            const mapped = entities.map((entity) => ({ id: entity.EntityId, name: entity.EntityName }));

            setOptions((prev) => (append ? [...prev, ...mapped] : mapped));
            setPage(pageToLoad);
            setHasMore(pageToLoad + 1 < totalPages);
        } catch (e) {
            console.error("error fetching schools:", e);
            if (!append) setOptions([]);
            setHasMore(false);
        } finally {
            setLoading(false);
        }
    }, []);

    // Load first page when the dropdown is first opened.
    useEffect(() => {
        if (dropdownOpen && options.length === 0) {
            fetchSchools(0, false);
        }
    }, [dropdownOpen]);

    // search from page 0 whenever the search changes.
    useEffect(() => {
        if (!dropdownOpen) return;
        searchRef.current = search;
        const timeout = setTimeout(() => {
            fetchSchools(0, false);
        }, 400);
        return () => clearTimeout(timeout);
    }, [search]);

    const handleSelect = (option) => {
        if (disabled) return;
        if (String(selectedId) === String(option.id)) {
            onSelect(null, null);
        } else {
            onSelect(option.id, option.name);
            setDropdownOpen(false);
        }
    };

    const getDisplayValue = () => (selectedId != null && selectedName ? selectedName : placeholder);

    return (
        <Dropdown isOpen={dropdownOpen} toggle={toggle} className="scd-dropdown" disabled={disabled}>
            <DropdownToggle
                caret
                color="white"
                className={`scd-toggle form-control-sm${disabled ? " scd-disabled" : ""}`}
            >
                <span className="scd-toggle-label">{getDisplayValue()}</span>
            </DropdownToggle>
            <DropdownMenu className="scd-menu">
                <DropdownItem header>{label}</DropdownItem>

                <div className="scd-search">
                    <Input
                        type="text"
                        bsSize="sm"
                        value={search}
                        placeholder={translate ? translate("dashboards.search") : "Search..."}
                        onChange={(e) => setSearch(e.target.value)}
                        onClick={(e) => e.stopPropagation()}
                    />
                </div>
                <DropdownItem divider />

                {options.map((option, index) => (
                    <div key={`${option.id}-${index}`} className="scd-option dropdown-item-custom">
                        <FormGroup check className="mb-0">
                            <Label check className="scd-option-label">
                                <Input
                                    type="checkbox"
                                    className="scd-checkbox"
                                    checked={String(selectedId) === String(option.id)}
                                    onChange={() => handleSelect(option)}
                                    disabled={disabled}
                                />
                                {option.name}
                            </Label>
                        </FormGroup>
                    </div>
                ))}

                {loading && (
                    <div className="scd-state">
                        <BeatLoader size={8} color="#395a8f" />
                    </div>
                )}

                {!loading && options.length === 0 && (
                    <div className="scd-state scd-state-muted">
                        {translate ? translate("dashboards.no-results") : "No results"}
                    </div>
                )}

                {hasMore && !loading && (
                    <>
                        <DropdownItem divider />
                        <div className="scd-load-more">
                            <button
                                type="button"
                                className="btn btn-link btn-sm scd-load-more-btn"
                                onClick={(e) => {
                                    e.stopPropagation();
                                    fetchSchools(page + 1, true);
                                }}
                            >
                                {translate ? translate("dashboards.load-more") : "Load more"}
                            </button>
                        </div>
                    </>
                )}
            </DropdownMenu>
        </Dropdown>
    );
};
