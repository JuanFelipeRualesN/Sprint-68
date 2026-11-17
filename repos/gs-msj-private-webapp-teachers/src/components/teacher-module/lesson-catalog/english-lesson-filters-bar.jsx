import React, { useState } from 'react';
import { Translate } from 'react-localize-redux';
import {
  Popover,
  Checkbox,
  FormControlLabel,
  Typography,
  Button,
  Chip,
  InputBase,
  Grid,
} from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import {
  ExpandMore,
  Search as SearchIcon,
  Close as CloseIcon,
  TuneRounded,
} from '@material-ui/icons';
import { BeatLoader } from 'react-spinners';
import { getAthenasStandards, getAthenasExpectations } from '../../../api/teachers';

const INK = '#16233A';
const SLATE = '#456083';
const MARIGOLD = '#F2A03D';
const MARIGOLD_SOFT = '#FDF1DF';
const CANVAS = '#F5F7FA';
const LINE = '#E3E8EF';

const useStyles = makeStyles({
  bar: {
    position: 'sticky',
    top: 0,
    zIndex: 20,
    backgroundColor: '#FFFFFF',
    borderBottom: `1px solid ${LINE}`,
    padding: '14px 24px 12px',
    marginBottom: '24px',
  },
  row: {
    display: 'flex',
    flexWrap: 'wrap',
    alignItems: 'center',
    gap: '10px',
  },
  barLabel: {
    display: 'flex',
    alignItems: 'center',
    gap: '6px',
    color: SLATE,
    fontSize: '12px',
    fontWeight: 700,
    letterSpacing: '0.09em',
    textTransform: 'uppercase',
    marginRight: '4px',
  },
  picker: {
    textTransform: 'none',
    borderRadius: '999px',
    border: `1px solid ${LINE}`,
    padding: '5px 12px 5px 16px',
    backgroundColor: '#FFFFFF',
    color: INK,
    fontSize: '14px',
    fontWeight: 600,
    minHeight: '38px',
    '&:hover': { backgroundColor: CANVAS, borderColor: '#CBD5E1' },
  },
  pickerActive: {
    borderColor: MARIGOLD,
    backgroundColor: MARIGOLD_SOFT,
    '&:hover': { backgroundColor: MARIGOLD_SOFT, borderColor: MARIGOLD },
  },
  pickerCount: {
    marginLeft: '8px',
    minWidth: '20px',
    height: '20px',
    lineHeight: '20px',
    borderRadius: '10px',
    backgroundColor: MARIGOLD,
    color: '#FFFFFF',
    fontSize: '11px',
    fontWeight: 700,
    padding: '0 6px',
  },
  searchWrap: {
    display: 'flex',
    alignItems: 'center',
    gap: '8px',
    flex: '1 1 240px',
    minWidth: '220px',
    height: '38px',
    padding: '0 14px',
    borderRadius: '999px',
    backgroundColor: CANVAS,
    border: `1px solid transparent`,
    '&:focus-within': { borderColor: MARIGOLD, backgroundColor: '#FFFFFF' },
  },
  searchInput: {
    flex: 1,
    fontSize: '14px',
    color: INK,
  },
  codeWrap: {
    flex: '0 1 150px',
    minWidth: '120px',
  },
  goBtn: {
    textTransform: 'none',
    borderRadius: '999px',
    minHeight: '38px',
    padding: '0 22px',
    backgroundColor: INK,
    color: '#FFFFFF',
    fontWeight: 700,
    fontSize: '14px',
    '&:hover': { backgroundColor: '#0F1929' },
  },
  chipRow: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: '8px',
    marginTop: '12px',
    alignItems: 'center',
  },
  chip: {
    backgroundColor: MARIGOLD_SOFT,
    color: '#8A5A12',
    fontWeight: 600,
    fontSize: '12px',
    height: '26px',
    borderRadius: '6px',
    '& .MuiChip-deleteIcon': { color: '#B98430', width: '15px' },
  },
  clearAll: {
    textTransform: 'none',
    fontSize: '12px',
    fontWeight: 700,
    color: SLATE,
    padding: '2px 8px',
    minWidth: 0,
    textDecoration: 'underline',
  },
  panel: {
    width: '420px',
    maxWidth: '92vw',
    padding: '14px 16px 8px',
    borderRadius: '14px',
  },
  panelSearch: {
    display: 'flex',
    alignItems: 'center',
    gap: '8px',
    height: '34px',
    padding: '0 12px',
    borderRadius: '8px',
    backgroundColor: CANVAS,
    marginBottom: '10px',
  },
  optionList: {
    maxHeight: '280px',
    overflowY: 'auto',
    paddingRight: '4px',
  },
  optionLabel: {
    fontSize: '13px',
    color: INK,
    lineHeight: 1.3,
    display: '-webkit-box',
    WebkitLineClamp: 2,
    WebkitBoxOrient: 'vertical',
    overflow: 'hidden',
  },
  checkbox: {
    color: '#B6C2D2',
    padding: '5px',
    '&.Mui-checked': { color: MARIGOLD },
  },
  showMore: {
    textTransform: 'none',
    fontSize: '13px',
    fontWeight: 700,
    color: SLATE,
    marginTop: '4px',
  },
  empty: {
    fontSize: '13px',
    color: '#8A97A8',
    padding: '10px 4px',
  },
});

// Horizontal filter bar for the English catalog.
// Subjects arriving here are already restricted to the '-en' set by the parent.
export const EnglishLessonFiltersBar = ({
  subjects,
  grades,
  standards,
  expectations,
  filters,
  setFilters,
  inputValues,
  setInputValues,
  filterLessons,
  filterByCheckbox,
  clearAndFetch,
  setStandards,
  setExpectations,
}) => {
  const classes = useStyles();

  const [anchor, setAnchor] = useState({ el: null, category: null });
  const [optionSearch, setOptionSearch] = useState('');
  const [standardsPage, setStandardsPage] = useState(0);
  const [expectationsPage, setExpectationsPage] = useState(0);
  const [standardsTotalPages, setStandardsTotalPages] = useState(null);
  const [expectationsTotalPages, setExpectationsTotalPages] = useState(null);
  const [limiter] = useState(10);
  const [loading, setLoading] = useState({ standards: false, expectations: false });

  const fetchMoreStandards = async () => {
    if (standardsTotalPages !== null && standardsPage >= standardsTotalPages - 1) return;
    try {
      setLoading(prev => ({ ...prev, standards: true }));
      const nextPage = standardsPage + 1;
      const response = await getAthenasStandards(nextPage, limiter);
      if (response.data?.TotalPages) setStandardsTotalPages(response.data.TotalPages);
      if (response.data?.Items) {
        const formatted = response.data.Items
          .filter(standard => standard.Description !== '')
          .map(standard => ({
            label: standard.Description,
            code: standard.Code,
            codeId: standard.Id,
          }));
        setStandards(prev => [...prev, ...formatted]);
        setStandardsPage(nextPage);
      }
    } catch (error) {
      console.error('Error fetching more standards:', error);
    } finally {
      setLoading(prev => ({ ...prev, standards: false }));
    }
  };

  const fetchMoreExpectations = async () => {
    if (expectationsTotalPages !== null && expectationsPage >= expectationsTotalPages - 1) return;
    try {
      setLoading(prev => ({ ...prev, expectations: true }));
      const nextPage = expectationsPage + 1;
      const response = await getAthenasExpectations(nextPage, limiter);
      if (response.data?.TotalPages) setExpectationsTotalPages(response.data.TotalPages);
      if (response.data?.Items) {
        const formatted = response.data.Items.map(expectation => ({
          label: expectation.Description,
          code: expectation.Code,
          codeId: expectation.Id,
        }));
        setExpectations(prev => [...prev, ...formatted]);
        setExpectationsPage(nextPage);
      }
    } catch (error) {
      console.error('Error fetching more expectations:', error);
    } finally {
      setLoading(prev => ({ ...prev, expectations: false }));
    }
  };

  const categories = [
    { key: 'subjects', labelId: 'lesson-catalog.subjects', data: subjects },
    { key: 'grades', labelId: 'lesson-catalog.grades', data: grades },
    {
      key: 'standards',
      labelId: 'lesson-catalog.standards',
      data: standards,
      onShowMore: fetchMoreStandards,
      hasMore: standardsTotalPages === null || standardsPage < standardsTotalPages - 1,
    },
    {
      key: 'expectations',
      labelId: 'lesson-catalog.expectations',
      data: expectations,
      onShowMore: fetchMoreExpectations,
      hasMore: expectationsTotalPages === null || expectationsPage < expectationsTotalPages - 1,
    },
  ];

  const isObjectCategory = key => key === 'standards' || key === 'expectations';

  const isOptionChecked = (key, option) =>
    isObjectCategory(key)
      ? (filters[key] || []).some(item => item.Code === option.code && item.CodeId === option.codeId)
      : (filters[key] || []).includes(option.code);

  const handleCheckboxChange = (key, option) => event => {
    const isChecked = event.target.checked;
    const newFilters = { ...filters };

    if (isObjectCategory(key)) {
      newFilters[key] = isChecked
        ? [...newFilters[key], { Code: option.code, CodeId: option.codeId }]
        : newFilters[key].filter(item => !(item.Code === option.code && item.CodeId === option.codeId));
    } else {
      newFilters[key] = isChecked
        ? [...newFilters[key], option.code]
        : newFilters[key].filter(item => item !== option.code);
    }

    setFilters(newFilters);
    filterByCheckbox(newFilters);
  };

  // Remove a single value from the chip row
  const removeChip = (key, value) => {
    const newFilters = { ...filters };
    newFilters[key] = isObjectCategory(key)
      ? newFilters[key].filter(item => !(item.Code === value.Code && item.CodeId === value.CodeId))
      : newFilters[key].filter(item => item !== value);
    setFilters(newFilters);
    filterByCheckbox(newFilters);
  };

  const handleInputChange = e => {
    const { name, value } = e.target;
    setInputValues({ ...inputValues, [name]: value });
  };

  const openPanel = (event, category) => {
    setOptionSearch('');
    setAnchor({ el: event.currentTarget, category });
  };

  const closePanel = () => setAnchor({ el: null, category: null });

  const activeCategory = categories.find(c => c.key === anchor.category);

  const visibleOptions = (activeCategory?.data || []).filter(option =>
    optionSearch.trim() === ''
      ? true
      : `${option.label} ${option.code}`.toLowerCase().includes(optionSearch.trim().toLowerCase())
  );

  // Human-readable text for a chip
  const chipLabel = (key, value) => {
    const list = categories.find(c => c.key === key)?.data || [];
    if (isObjectCategory(key)) {
      const match = list.find(o => o.code === value.Code && o.codeId === value.CodeId);
      return match ? match.label : value.Code;
    }
    const match = list.find(o => o.code === value);
    return match ? match.label : value;
  };

  const activeChips = categories.flatMap(category =>
    (filters[category.key] || []).map(value => ({ key: category.key, value }))
  );

  return (
    <div className={classes.bar}>
      <div className={classes.row}>
        <span className={classes.barLabel}>
          <TuneRounded style={{ fontSize: '17px' }} />
          <Translate id="lesson-catalog.filters" options={{ renderInnerHtml: false }} />
        </span>

        {categories.map(category => {
          const count = (filters[category.key] || []).length;
          return (
            <Button
              key={category.key}
              onClick={event => openPanel(event, category.key)}
              className={`${classes.picker} ${count > 0 ? classes.pickerActive : ''}`}
              endIcon={<ExpandMore style={{ fontSize: '19px', color: SLATE }} />}
            >
              <Translate id={category.labelId} options={{ renderInnerHtml: false }} />
              {count > 0 && <span className={classes.pickerCount}>{count}</span>}
            </Button>
          );
        })}

        <div className={classes.searchWrap}>
          <SearchIcon style={{ fontSize: '19px', color: SLATE }} />
          <InputBase
            name="content"
            value={inputValues.content}
            onChange={handleInputChange}
            onKeyPress={e => e.key === 'Enter' && filterLessons()}
            className={classes.searchInput}
            inputProps={{ 'aria-label': 'Search lessons' }}
          />
        </div>

        <div className={`${classes.searchWrap} ${classes.codeWrap}`}>
          <Translate>
            {({ translate }) => 
            <InputBase
              name="code"
              value={inputValues.code}
              onChange={handleInputChange}
              onKeyPress={e => e.key === 'Enter' && filterLessons()}
              className={classes.searchInput}
              placeholder={translate('lesson-catalog.code-placeholder', null, { renderInnerHtml: false })}
            />}
          </Translate>
        </div>

        <Button className={classes.goBtn} onClick={filterLessons}>
          <Translate id="lesson-catalog.search" options={{ renderInnerHtml: false }} />
        </Button>
      </div>

      {activeChips.length > 0 && (
        <div className={classes.chipRow}>
          {activeChips.map(({ key, value }) => (
            <Chip
              key={`${key}-${isObjectCategory(key) ? `${value.Code}-${value.CodeId}` : value}`}
              label={chipLabel(key, value)}
              onDelete={() => removeChip(key, value)}
              deleteIcon={<CloseIcon />}
              className={classes.chip}
            />
          ))}
          <Button className={classes.clearAll} onClick={clearAndFetch}>
            <Translate id="lesson-catalog.clear-filters" options={{ renderInnerHtml: false }} />
          </Button>
        </div>
      )}

      <Popover
        open={Boolean(anchor.el)}
        anchorEl={anchor.el}
        onClose={closePanel}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
        transformOrigin={{ vertical: 'top', horizontal: 'left' }}
        PaperProps={{ className: classes.panel, elevation: 6 }}
      >
        {activeCategory && (
          <>
            <div className={classes.panelSearch}>
              <SearchIcon style={{ fontSize: '18px', color: SLATE }} />
              <InputBase
                autoFocus
                value={optionSearch}
                onChange={e => setOptionSearch(e.target.value)}
                className={classes.searchInput}
              />
            </div>

            {loading[activeCategory.key] && visibleOptions.length === 0 ? (
              <div style={{ padding: '14px 4px' }}>
                <BeatLoader size={9} color={MARIGOLD} />
              </div>
            ) : visibleOptions.length === 0 ? (
              <Typography className={classes.empty}>
                <Translate id="lesson-catalog.no-lessons-found" options={{ renderInnerHtml: false }} />
              </Typography>
            ) : (
              <div className={classes.optionList}>
                <Grid container>
                  {visibleOptions.map(option => (
                    <Grid item xs={12} sm={6} key={option.codeId ?? option.code}>
                      <FormControlLabel
                        control={
                          <Checkbox
                            size="small"
                            className={classes.checkbox}
                            checked={isOptionChecked(activeCategory.key, option)}
                            onChange={handleCheckboxChange(activeCategory.key, option)}
                          />
                        }
                        label={
                          <Typography className={classes.optionLabel} title={option.label}>
                            {option.label}
                          </Typography>
                        }
                      />
                    </Grid>
                  ))}
                </Grid>
              </div>
            )}

            {activeCategory.onShowMore && activeCategory.hasMore && (
              <Button
                className={classes.showMore}
                onClick={activeCategory.onShowMore}
                disabled={loading[activeCategory.key]}
              >
                {loading[activeCategory.key] ? (
                  <Translate id="lesson-catalog.loading" options={{ renderInnerHtml: false }} />
                ) : (
                  <Translate id="lesson-catalog.show-more" options={{ renderInnerHtml: false }} />
                )}
              </Button>
            )}
          </>
        )}
      </Popover>
    </div>
  );
};
