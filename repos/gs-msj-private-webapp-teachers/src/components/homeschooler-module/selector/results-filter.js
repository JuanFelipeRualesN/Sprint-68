import React, {useState} from 'react'
import {Translate} from 'react-localize-redux'
import {
    Typography,
    Button,
    Select
} from "@material-ui/core";
import {Search as SearchIcon} from "@material-ui/icons";

import {BeatLoader} from "react-spinners";
import MenuItem from "@material-ui/core/MenuItem";
import LocalizedDatePicker from '../../../utils/datepicker-locale';
import {styles} from './results-filter-styles';

const ResultFilters = ({
                           grades, // Only grades are passed as a filter option
                           subjects,
                           filters, // Current filter state
                           setFilters, // Function to update filter state
                           setInputValues, // Function to update input field values
                           filterResults, // Function to trigger filtering (renamed from filterLessons)
                           clearAndFetch, // Function to clear filters and re-fetch results
                       }) => {
    const classes = styles(); // Assuming styles hook from Material-UI makeStyles
    const [loading, setLoading] = useState(false);

    const handleGradeSelectChange = (event, category) => {
        // If category is 'grades'
        const { value } = event.target;
        if (category === 'grades') {
            setFilters(prev => ({
                ...prev,
                grades: value ? [value] : [] // Set to an array with the selected value or empty array
            }));
        } else if (category === 'subjects') {
            setFilters(prev => ({
                ...prev,
                subjects: value ? [value] : [] // Set to an array with the selected value or empty array
            }));
        }
    };

    const handleFromDateFilterChange = (date) => {
        setFilters(prev => ({
            ...prev,
            dateFrom: date
        }));
    }

    const handleToDateFilterChange = (date) => {
        setFilters(prev => ({
            ...prev,
            dateTo: date
        }));
    }

    const clearFilters = () => {
        setFilters({
            grades: [],
            subjects: []
        });
    }

    // --- Accordion Data (simplified for only grades) ---
    const accordionData = [
        {
            title: <Translate id="homeschool.grade" options={{renderInnerHtml: false}}/>,
            category: "grades",
            data: grades
        },
        {
            title: <Translate id="homeschool.subject" options={{renderInnerHtml: false}}/>,
            category: "subjects",
            data: subjects
        },
    ];

    return (
        <div className="filters-container">
            <div style={{
                paddingLeft: '16px',
                paddingRight: '16px',
                paddingBottom: '10px',
                marginTop: '10px',
                color: '#456083'
            }}>
                <Typography classes={{root: classes.titles}}>
                    <Translate id="homeschool.filters" options={{renderInnerHtml: false}}/>
                </Typography>
            </div>

            {accordionData.map((item) => {
                return (
                    <div
                        key={item.category}
                        style={{paddingLeft: 20, paddingRight: 20}}
                    >
                        <span style={{fontWeight: "600", fontSize: "15px"}}>{item.title}</span>
                        {!loading[item.category] ? (
                            <div className={classes.scrollableContainer}>
                                {item.data?.length > 0 ? (
                                    <Select
                                        labelId={`${item.category}-select-label`}
                                        id={`${item.category}-select`}
                                        value={
                                            filters && filters[item.category] && Array.isArray(filters[item.category]) && filters[item.category].length > 0
                                                ? filters[item.category][0]
                                                : ''
                                        }
                                        onChange={(e) => handleGradeSelectChange(e, item.category)}
                                        style={{width: "100%"}}
                                        classNamePrefix="mantine-like"
                                        className="mantine-like__control"
                                        label={<Translate id={`homeschool.${item.category}`}
                                                          options={{renderInnerHtml: false}}/>}
                                    >
                                        {item.data.map((option) => (
                                            <MenuItem key={option.code} name={item.category} value={option.code}>
                                                {option.label}
                                            </MenuItem>
                                        ))}
                                    </Select>) : null
                                }
                            </div>
                        ) : (
                            <div style={{marginLeft: '15px'}}>
                                <BeatLoader size={10} color={"#6bc4ff"}/>
                            </div>
                        )}
                    </div>
                );
            })}

            <div
                style={{paddingLeft: 20, paddingRight: 20}}
            >
                <div className="form-group-fields form-group-fields__datapicker">
                    <span style={{fontWeight: "600", fontSize: "15px"}}>
                         <Translate id="homeschool.date-from" options={{renderInnerHtml: false}}/>
                    </span>
                    <br/>
                    <LocalizedDatePicker
                        inline
                        onChange={handleFromDateFilterChange}
                        selected={filters != null && filters.dateFrom != null ? filters.dateFrom : null}
                    />
                </div>
            </div>
            <div
                style={{paddingLeft: 20, paddingRight: 20}}
            >
                <div className="form-group-fields form-group-fields__datapicker">
                    <span style={{fontWeight: "600", fontSize: "15px"}}>
                         <Translate id="homeschool.date-to" options={{renderInnerHtml: false}}/>
                    </span>
                    <br/>
                    <LocalizedDatePicker
                        inline
                        onChange={handleToDateFilterChange}
                        selected={filters != null && filters.dateTo != null ? filters.dateTo : null}

                    />
                </div>
            </div>

            <div className={classes.searchContainer}>
                <div style={{display: 'flex', flexDirection: 'row', gap: '20px'}}>
                    <Button onClick={filterResults}>
                        <div style={{textTransform: 'capitalize', marginRight: '7px', color: '#456083'}}>
                            <Typography classes={{root: classes.titles}}>
                                <Translate id="homeschool.search" options={{renderInnerHtml: false}}/>
                            </Typography>
                        </div>
                        <SearchIcon style={{color: '#456083'}}/>
                    </Button>

                    <Button onClick={clearFilters}>
                        <div style={{fontSize: '15px', fontWeight: 600, textTransform: 'capitalize', color: '#456083'}}>
                            <Typography classes={{root: classes.titles}}>
                                <Translate id="homeschool.clear-filters" options={{renderInnerHtml: false}}/>
                            </Typography>
                        </div>
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default ResultFilters;