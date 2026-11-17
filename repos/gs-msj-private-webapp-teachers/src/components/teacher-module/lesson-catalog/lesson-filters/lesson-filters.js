import React, { useState } from 'react'
import { Translate } from 'react-localize-redux'
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Checkbox,
  FormControlLabel,
  Typography,
  Grid,
  Button,
} from "@material-ui/core";
import { Input } from 'reactstrap';
import { styles } from './lesson-filters-styles';
import { ExpandMore, Search as SearchIcon } from "@material-ui/icons";
import { getAthenasStandards, getAthenasExpectations } from '../../../../api/teachers';
import { BeatLoader } from 'react-spinners';
import {isTablet} from "../../../../utils/utils";

// Component to filter lessons
const LessonFilters = ({subjects, grades, standards, expectations, filters, setFilters,
                         setInputValues, inputValues, filterLessons, setStandards, setExpectations, clearAndFetch, filterByCheckbox }) => {

  console.log("Subjects", subjects);
  const [standardsPage, setStandardsPage] = useState(0);
  const [expectationsPage, setExpectationsPage] = useState(0);
  const [standardsTotalPages, setStandardsTotalPages] = useState(null);
  const [expectationsTotalPages, setExpectationsTotalPages] = useState(null);
  const [limiter] = useState(10);
  const [loading, setLoading] = useState({
    standards: false,
    expectations: false
  });

  const classes = styles();

  // Fetch the standards from the next page
  const fetchMoreStandards = async () => {

    // Do not fetch if we already fetched the last page
    if (standardsTotalPages !== null && standardsPage >= standardsTotalPages - 1) {
      return;
    }

    try {
      setLoading(prev => ({ ...prev, standards: true }));
      const nextPage = standardsPage + 1;
      const response = await getAthenasStandards(nextPage, limiter);

      if (response.data?.TotalPages ) {
        setStandardsTotalPages(response.data.TotalPages);
      }

      if (response.data?.Items) {
        const formattedStandards = response.data.Items
            .filter(standard => standard.Description !== "")
            .map(standard => ({
              label: standard.Description,
              code: standard.Code,
              codeId: standard.Id
            }));

        setStandards(prevStandards => [...prevStandards, ...formattedStandards]);
        setStandardsPage(nextPage);
      }
    } catch (error) {
      console.error("Error fetching more standards:", error);
    } finally {
      setLoading(prev => ({ ...prev, standards: false }));
    }
  }

  // Fetch the expectations from the next page
  const fetchMoreExpectations = async () => {

    // Do not fetch if we already fetched the last page
    if (expectationsTotalPages !== null && expectationsPage >= expectationsTotalPages - 1) {
      return;
    }

    try {
      setLoading(prev => ({ ...prev, expectations: true }));
      const nextPage = expectationsPage + 1;
      const response = await getAthenasExpectations(nextPage, limiter);

      if (response.data?.TotalPages ) {
        setExpectationsTotalPages(response.data.TotalPages);
      }

      if (response.data.Items) {
        const formattedExpectations = response.data.Items
            .map(expectation => ({
              label: expectation.Description,
              code: expectation.Code,
              codeId: expectation.Id
            }));

        setExpectations(prevExpectations => [...prevExpectations, ...formattedExpectations]);
        setExpectationsPage(nextPage);
      }
    } catch (error) {
      console.error("Error fetching more expectations:", error);
    } finally {
      setLoading(prev => ({ ...prev, expectations: false }));
    }
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setInputValues({
      ...inputValues,
      [name]: value
    });
  };

  const handleCheckboxChange = (category, option) => (event) => {
    const isChecked = event.target.checked;
    const code = option.code;
    const codeId = option.codeId;

    // Create a copy of the filters object
    let newFilters = { ...filters };

    if (category === "expectations") {
      // Compare and set description in expectations filter
      if (isChecked) {
        newFilters.expectations = [
          ...newFilters.expectations,
          {
            Code: code,
            CodeId: codeId
          }
        ];
      } else {
        newFilters.expectations = newFilters.expectations.filter(item => !(item.Code === code && item.CodeId === codeId));
      }
    } else if (category === "standards") {
      if (isChecked) {
        newFilters.standards = [
          ...newFilters.standards,
          {
            Code: code,
            CodeId: codeId
          }
        ];
      } else {
        newFilters.standards = newFilters.standards.filter(item => !(item.Code === code && item.CodeId === codeId));
      }
    } else {
      if (isChecked) {
        newFilters[category] = [...newFilters[category], code];
      } else {
        newFilters[category] = newFilters[category].filter(item => item !== code);
      }
    }
    setFilters(newFilters);
    filterByCheckbox(newFilters);
  };

  // Subjects taught in English, identified by the '-en' suffix in their code (e.g. mat-en)
  const englishSubjects = (subjects || []).filter(subject =>
      subject.code?.toLowerCase().includes('-en') || subject.code?.toLowerCase() === 'en'
  );

  // Checked only when every english subject is currently selected
  const isEnglishContentAreaChecked = englishSubjects.length > 0 &&
      englishSubjects.every(subject => filters.subjects.includes(subject.code));

  // Select or deselect all english subjects at once
  const handleEnglishContentAreaChange = (event) => {
    const isChecked = event.target.checked;
    const englishCodes = englishSubjects.map(subject => subject.code);

    let newFilters = { ...filters };

    if (isChecked) {
      newFilters.subjects = [
        ...newFilters.subjects,
        ...englishCodes.filter(code => !newFilters.subjects.includes(code))
      ];
    } else {
      newFilters.subjects = newFilters.subjects.filter(code => !englishCodes.includes(code));
    }

    setFilters(newFilters);
    filterByCheckbox(newFilters);
  };

  // Clear all filters and fetch lessons
  const clearFilters = () => {
    clearAndFetch();
  }

  const accordionData = [
    {
      title: <Translate id="lesson-catalog.subjects" options={{ renderInnerHtml: false }} />,
      category: "subjects",
      data: subjects
    },
    {
      title: <Translate id="lesson-catalog.grades" options={{ renderInnerHtml: false }} />,
      category: "grades",
      data: grades
    },
    {
      title: <Translate id="lesson-catalog.standards" options={{ renderInnerHtml: false }} />,
      category: "standards",
      data: standards,
      onShowMore: fetchMoreStandards
    },
    {
      title: <Translate id="lesson-catalog.expectations" options={{ renderInnerHtml: false }} />,
      category: "expectations",
      data: expectations,
      onShowMore: fetchMoreExpectations
    }
  ];

  const tablet = isTablet();
  return (
      <div className="filters-container">
        <div style={{paddingLeft: '16px', paddingRight: '16px', paddingBottom: '10px', marginTop: '10px', color: '#456083'}}>
          <Typography classes={{root: classes.titles}}>
            <Translate id="lesson-catalog.filters" options={{ renderInnerHtml: false }} />
          </Typography>
        </div>

        {/* English Content Area: selects every subject whose code ends in '-en' */}
        <div style={{paddingLeft: '16px', paddingRight: '16px', paddingBottom: '4px'}}>
          <FormControlLabel
              control={
                <Checkbox
                    checked={isEnglishContentAreaChecked}
                    onChange={handleEnglishContentAreaChange}
                    disabled={englishSubjects.length === 0}
                    className={classes.customCheckbox}
                    size="small"
                />
              }
              label={
                <Typography classes={{root: classes.titles}}>
                  <Translate id="lesson-catalog.english-content-area" options={{ renderInnerHtml: false }} />
                </Typography>
              }
          />
        </div>

        {accordionData.map((item) => (
            <Accordion key={item.category} classes={{ root: classes.accordion }} elevation={0}>
              <AccordionSummary classes={{ root: classes.accordionSummary }} expandIcon={<ExpandMore />}>
                <Typography classes={{root: classes.titles}}>{item.title}</Typography>
              </AccordionSummary>
              {!loading[item.category] ?
                  <AccordionDetails classes={{ root: classes.accordionDetails }}>
                    <div className={classes.scrollableContainer}>
                      {item.data?.length > 0 ? (
                          <Grid container>
                            {item.data.map((option) => {
                              // Handle expectations where the code and description are both needed to handle the checkbox
                              const isChecked = item.category === "expectations" ?
                                  filters.expectations.some(expectation => expectation.Code === option.code && expectation.CodeId === option.codeId)
                                  : item.category === "standards" ? filters.standards.some(standard => standard.Code === option.code && standard.CodeId === option.codeId)
                                      : filters[item.category].includes(option.code);

                              return <Grid item xs={6} key={option.codeId ?? option.code}>
                                <FormControlLabel
                                    control={
                                      <Checkbox
                                          checked={isChecked}
                                          onChange={handleCheckboxChange(item.category, option)}
                                          className={classes.customCheckbox}
                                          size="small"
                                      />
                                    }
                                    label={
                                      <Typography
                                          className={tablet ? classes.truncatedLabelTablet : classes.truncatedLabel}
                                          title={option.label}
                                      >
                                        {option.label}
                                      </Typography>
                                    }
                                    {...(item.category === "expectations" && !tablet ?  { className: classes.expectationsCheckboxLabel } : item.category === "expectations" && tablet && { className: classes.expectationsCheckboxLabelTablet })}
                                />
                              </Grid>
                            })}
                          </Grid>
                      ) : (
                          <Typography>
                            <Translate id="lesson-catalog.loading" options={{ renderInnerHtml: false }} />
                          </Typography>
                      )}
                    </div>
                  </AccordionDetails>
                  :
                  <div style={{marginLeft: '15px'}}>
                    <BeatLoader size={10} color={"#6bc4ff"} />
                  </div>}

              {/* Show more button for standards and expectations */}
              {(item.category === 'standards' || item.category === 'expectations') &&
                  ((item.category === 'standards' && (standardsTotalPages === null || standardsPage < standardsTotalPages - 1)) ||
                      (item.category === 'expectations' && (expectationsTotalPages === null || expectationsPage < expectationsTotalPages - 1))) && (
                      <Button
                          onClick={item.onShowMore}
                          disabled={loading[item.category]}
                          style={{paddingLeft: '16px', textTransform: 'none', backgroundColor: 'transparent'}}
                      >
                        <Typography style={{fontSize: '13px', fontWeight: 600, color: '#456083', textTransform: 'capitalize'}}>
                          {loading[item.category] ?
                              <Translate id="lesson-catalog.loading" options={{ renderInnerHtml: false }} />
                              :
                              <Translate id="lesson-catalog.show-more" options={{ renderInnerHtml: false }} />
                          }
                          {" "}
                          <Translate id={`lesson-catalog.${item.category === 'expectations' ? 'expectations': 'standards'}`} options={{ renderInnerHtml: false }} />
                        </Typography>
                      </Button>
                  )}
            </Accordion>
        ))}
        <div className={classes.searchContainer}>
          <div className={classes.inputRow}>
            <Typography classes={{root: classes.titles}}>
              <Translate id="lesson-catalog.code" options={{ renderInnerHtml: false }} />
            </Typography>
            <div className='form-group-fields' style={{width: '100%', marginTop: '5px'}}>
              <Input
                  name="code"
                  type="text"
                  value={inputValues.code}
                  onChange={handleInputChange}
                  style={{width: '100%'}}
              />
            </div>
          </div>
          <div className={classes.inputRow}>
            <Typography classes={{root: classes.titles}}>
              <Translate id="lesson-catalog.text" options={{ renderInnerHtml: false }} />
            </Typography>
            <div className='form-group-fields' style={{width: '100%', marginTop: '5px'}}>
              <Input
                  name="content"
                  type="text"
                  value={inputValues.content}
                  onChange={handleInputChange}
                  style={{width: '100%'}}
              />
            </div>
          </div>
          <div style={{display: 'flex', flexDirection: 'row', gap: '20px'}}>
            <Button onClick={filterLessons}>
              <div style={{textTransform: 'capitalize', marginRight: '7px'}} className='search-btn'>
                <Typography classes={{root: classes.titles}}>
                  <Translate id="lesson-catalog.search" options={{ renderInnerHtml: false }} />
                </Typography>
              </div>
              <SearchIcon style={{color: '#456083'}}/>
            </Button>
            <Button onClick={clearFilters}>
              <div style={{fontSize:'15px', fontWeight: 600, textTransform: 'capitalize'}} className='clear-filters-btn'>
                <Typography classes={{root: classes.titles}}>
                  <Translate id="lesson-catalog.clear-filters" options={{ renderInnerHtml: false }} />
                </Typography>
              </div>
            </Button>
          </div>
        </div>
      </div>
  )
}

export default LessonFilters
