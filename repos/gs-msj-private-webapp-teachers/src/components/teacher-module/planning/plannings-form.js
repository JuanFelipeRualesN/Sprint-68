import React, { useEffect, useState } from "react";
import moment from "moment";
import useGetStudentsGroup from "./shared/hooks/useGetStudentsGroup";
import useGetActiveSubscription from "./shared/hooks/useGetActiveSubscription";
import FormSelect from "./shared/components/form/form-select";
import Heading from "./shared/components/heading";
import Text from "./shared/components/text";
import FormInput from "./shared/components/form/form-field";
import FormDatePicker from "./shared/components/form/form-date-picker";
import { translate } from "./shared/utils";
import { Translate } from "react-localize-redux";
import FormMultiSelect from "./shared/components/form/form-multi-select";
import {Alert} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faExclamationCircle} from "@fortawesome/free-solid-svg-icons";

export default function PlanningsForm({
    disabled = false,
  planning,
  isLoading = false,
  isForEdit,
  onChange = (createPlanningModel) => {},
}) {
  const { studentsGroup, getStudentsGroup } = useGetStudentsGroup();
  const {
    subjectsAndGrades,
    academicPeriods,
    getActiveSubscription,
  } = useGetActiveSubscription();
  const [createPlanningModel, setCreatePlanningModel] = useState({});
  const [minDate, setMinDate] = useState(new Date());
  const [maxDate, setMaxDate] = useState(new Date());
  const [subjectAndGradeId, setSubjectAndGradeId] = useState("");
  const [disableForm, setDisableForm] = useState(disabled);
  const [periodId, setPeriodId] = useState(planning?.periodId || null);
  const [openDate, setOpenDate] = useState(planning?.openDate || null);
  const [closeDate, setCloseDate] = useState(planning?.closeDate || null);
  const [weekNumber, setWeekNumber] = useState(planning?.weekNumber || 1);
  const [showOpenDateError, setShowOpenDateError] = useState(false);
  const [showCloseDateError, setShowCloseDateError] = useState(false);
  const [defaultDateSet, setDefaultDateSet] = useState(false);
  const [periodExpired, setPeriodExpired] = useState(false);
  const [dateWarnings, setDateWarnings] = useState({
    openDatePast: false,
    closeDatePast: false,
    rangeInvalid: false,
  });

  // Disable date fields if there's no period selected or period has expired
  const datesLocked = !periodId || periodExpired;

  useEffect(() => {
    // Set default week number to 1 if not provided
    if(!isForEdit) {
      setCreatePlanningModel((prev) => ({
        ...prev,
        WeekNumber: weekNumber,
      }));
    }
  }, [weekNumber]);

  useEffect(() => {
    if (createPlanningModel?.PeriodId) {
      changeDatesByPeriod(createPlanningModel.PeriodId);
    }
  }, [createPlanningModel?.PeriodId]);

  const handleChange = (name, value, extra = {}, options = {}) => {

    if (name === "PeriodId") {
        setPeriodId(value);
    }

    if(name === "WeekNumber") {
      setWeekNumber(value);
    }

    if(name === "OpenDate" && !options.skipDateState) {
        setOpenDate(value);
    }

    if(name === "CloseDate" && !options.skipDateState) {
        setCloseDate(value);
    }

    setCreatePlanningModel((prev) => {
      // Keep the previous closing date when the period is unknown, otherwise a
      // cloned planning loses the value updateForm read from planning.closure
      const periodCloseDate = name === "PeriodId"
        ? academicPeriods.find((x) => x.Id === value)?.PeriodTo ?? prev.PeriodCloseDate
        : prev.PeriodCloseDate;

      const newPlanning = {
        ...prev,
        [name]: value,
        PeriodCloseDate: periodCloseDate,
        ...extra,
      };

      onChange(newPlanning);
      return newPlanning;
    });
  };

  const handlePeriod = (selectedValue) => {
    // Picking an active period clears the warnings raised by a cloned planning
    setPeriodExpired(false);
    setDateWarnings({ openDatePast: false, closeDatePast: false, rangeInvalid: false });
    handleChange("PeriodId", selectedValue, { HasInvalidDates: false });
    setTimeout(() => changeDatesByPeriod(selectedValue), 0);
  };

  const changeDatesByPeriod = (periodId) => {
    const academicPeriod = academicPeriods.find((x) => x.Id === periodId);
    const today = new Date();

    let fromDate = today;
    let toDate = today;

    if (academicPeriod) {
      const periodFrom = moment(academicPeriod.PeriodFrom).toDate();
      const periodTo = moment(academicPeriod.PeriodTo).toDate();

      fromDate = periodFrom < today ? today : periodFrom;
      toDate = periodTo;
    }

    const formattedFromDate = moment(fromDate).format("YYYY-MM-DD");
    const formattedToDate = moment(toDate).format("YYYY-MM-DD");
    const calculatedWeekNumber = calculateNumberOfWeeks(fromDate, toDate);

    const isCloningSamePeriod = (
        createPlanningModel &&
        createPlanningModel.PeriodId === periodId &&
        !isForEdit &&
        createPlanningModel.Id != null &&
            !defaultDateSet
    );

    if (isCloningSamePeriod) {
      const open = planning.openDate
          ? new Date(planning.openDate)
          : new Date();
      const close = planning.closeDate
          ? new Date(planning.closeDate)
          : new Date();

      setOpenDate(open);
      setCloseDate(close);
      const warnings = validateDateRange(open, close);
      setPeriodId(periodId);

      // Format dates for the new planning model
        const formattedOpenDate = moment(open).format("YYYY-MM-DD");
        const formattedCloseDate = moment(close).format("YYYY-MM-DD");

      // Also update planning.periodId if it exists
        if (planning.periodId) {
            planning.periodId = periodId;
        }

      setCreatePlanningModel((prev) => {
        const newPlanning = {
          ...prev,
          PeriodId: periodId,
          OpenDate: formattedOpenDate,
          CloseDate: formattedCloseDate,
          WeekNumber: planning.weekNumber,
          PeriodCloseDate: formattedToDate,
          HasInvalidDates:
            !academicPeriod ||
            warnings.openDatePast ||
            warnings.closeDatePast ||
            warnings.rangeInvalid,
        };

        onChange(newPlanning);
        return newPlanning;
      });
      setDefaultDateSet(true);
      return;
    }

    // Not cloning — update min/max limits
    setMinDate(fromDate);
    setMaxDate(toDate);

    if (isForEdit) return;

    // Default closeDate: +6 days or max period end
    const calculatedCloseDate = new Date(fromDate.getTime());
    calculatedCloseDate.setDate(calculatedCloseDate.getDate() + 6);
    const finalCloseDate = calculatedCloseDate > toDate ? toDate : calculatedCloseDate;

    const formattedCloseDate = moment(finalCloseDate).format("YYYY-MM-DD");

    setOpenDate(fromDate);
    setCloseDate(finalCloseDate);
    const warnings = validateDateRange(fromDate, finalCloseDate);
    setWeekNumber(calculatedWeekNumber);
    setShowOpenDateError(false); // Since closeDate hasn't been edited yet
    setShowCloseDateError(false);

    // Also update planning.periodId if it exists
    if (planning.periodId) {
      planning.periodId = periodId;
    }

    setCreatePlanningModel((prev) => {
      const newPlanning = {
        ...prev,
        PeriodId: periodId,
        OpenDate: formattedFromDate,
        CloseDate: formattedCloseDate,
        WeekNumber: calculatedWeekNumber,
        PeriodCloseDate: formattedToDate,
        HasInvalidDates:
          !academicPeriod ||
          warnings.openDatePast ||
          warnings.closeDatePast ||
          warnings.rangeInvalid,
      };

      onChange(newPlanning);
      return newPlanning;
    });
  };

  const extractSubjectName = (productName) => {
    return productName.replace(/\s+([K\d]+)$/, '').trim();
  };

  const handleGradeChange = (selectedValue) => {
    const subjectAndGrade = subjectsAndGrades.find(
      (subAndGrade) => subAndGrade.Id === selectedValue
    );
    const newPlanning = {
      ...createPlanningModel,
      SubjectName: extractSubjectName(subjectAndGrade?.SubjectName),
      LevelCode: subjectAndGrade?.Level,
      SubjectCode: subjectAndGrade?.Subject,
    };
    setCreatePlanningModel(newPlanning);
    onChange(newPlanning);
    setSubjectAndGradeId(subjectAndGrade?.Id);
  };

  const handleGroupsChange = (selected) => {
    const planningGroups = selected.map((x) => ({ GroupId: x }));
    handleChange("Groups", planningGroups);
  };

  const handleDate = (name, value, refOpenDate = openDate, refCloseDate = closeDate) => {
    setShowOpenDateError(false);
    setShowCloseDateError(false);

    const formattedDate = moment(value).format("YYYY-MM-DD");

    // Compare dates as moment objects to avoid issues
    const asMoment = (date) => (date ? moment(date).startOf("day") : null);
    const selected = asMoment(value);
    const openRef = asMoment(refOpenDate);
    const closeRef = asMoment(refCloseDate);

    let warnings;

    if (name === "OpenDate") {
      setOpenDate(value);
      setShowOpenDateError(!!closeRef && selected.isAfter(closeRef));
      warnings = validateDateRange(value, refCloseDate);
    }

    if (name === "CloseDate") {
      setCloseDate(value);
      setShowCloseDateError(!!openRef && selected.isBefore(openRef));
      warnings = validateDateRange(refOpenDate, value);
    }

    // Editing a date re-evaluates whether the planning can still be created
    const hasInvalidDates = periodExpired || !!(
      warnings && (warnings.openDatePast || warnings.closeDatePast || warnings.rangeInvalid)
    );

    handleChange(name, formattedDate, { HasInvalidDates: hasInvalidDates }, { skipDateState: true });
  };

  const updateForm = (planning) => {
    const subjectAndGrade = subjectsAndGrades.find(
      (s) =>
        s.Level === planning.levelCode && s.Subject === planning.subjectCode
    );

    setSubjectAndGradeId(subjectAndGrade?.Id);
    changeDatesByPeriod(planning.periodId || "");

    const planningPeriodId = planning.periodId || null;
    const today = moment().startOf('day');

    // When the cloned planning period has expired, no value is selected in the period dropdown, which triggers the warning
    const isExpired = !!planning?.periodTo && moment(planning.periodTo).startOf('day').isBefore(today);
    setPeriodExpired(isExpired);

    const openDate = planning.openDate ? new Date(planning.openDate) : null;
    const closeDate = planning.closeDate ? new Date(planning.closeDate) : null;

    setOpenDate(openDate);
    setCloseDate(closeDate);
    const { openDatePast, closeDatePast, rangeInvalid } = validateDateRange(openDate, closeDate);

    // Determine if the same period is being used to set week number when cloning a planning
    const currentPeriod = createPlanningModel?.PeriodId ?? periodId;
    const samePeriod = currentPeriod && planningPeriodId && currentPeriod === planningPeriodId;

    let weeks = calculateWeeksForPeriod(planningPeriodId);

    if (samePeriod) {
      const selectedWeeks = createPlanningModel?.WeekNumber || weekNumber;
      if (selectedWeeks > 0) 
        weeks = selectedWeeks;
    }

    setWeekNumber(weeks);
    setPeriodId(planningPeriodId);

    setCreatePlanningModel((prev) => {
      const updatedPlanning = {
        ...prev,
        Id: planning.id,
        PeriodId: planning.periodId,
        Name: planning.name,
        LevelCode: planning.levelCode,
        SubjectCode: planning.subjectCode,
        Groups: planning.planningGroups,
        CloseDate: isForEdit
          ? moment(planning.closeDate).format("YYYY-MM-DD")
          : closeDate
          ? moment(closeDate).format("YYYY-MM-DD")
          : prev.CloseDate,
        OpenDate: isForEdit
          ? moment(planning.openDate).format("YYYY-MM-DD")
          : openDate
          ? moment(openDate).format("YYYY-MM-DD")
          : prev.OpenDate,
        WeekNumber: weeks,
        subjectAndGradeId: subjectAndGrade?.Id,
        PeriodCloseDate: planning.closure,
        HasInvalidDates: isExpired || openDatePast || closeDatePast || rangeInvalid,
      };

      onChange(updatedPlanning);
      return updatedPlanning;
    });
  };

  useEffect(() => {
    getStudentsGroup();
     getActiveSubscription();
  }, []);

  useEffect(() => {
    if (planning && !isForEdit) {
      updateForm(planning);
    }
  }, [planning]);

  useEffect(() => {
    if (
      isForEdit &&
      subjectsAndGrades.length > 0 &&
      academicPeriods.length > 0
    ) {
      updateForm(planning);
    }
  }, [subjectsAndGrades, academicPeriods]);

  const calculateNumberOfWeeks = (fromDate, toDate) => {
    const startMillis = fromDate.getTime();
    const endMillis = toDate.getTime();

    // Calculate the difference in milliseconds
    const differenceMillis = endMillis - startMillis;

    // Calculate the number of milliseconds in a week
    const millisecondsInWeek = 1000 * 60 * 60 * 24 * 7;

    // Calculate the number of weeks
    const weeks = differenceMillis / millisecondsInWeek;

    // Return the number of weeks rounded down to the nearest integer
    return Math.floor(weeks);
  };

  const deduplicatedData = subjectsAndGrades.filter(
    (subjectAndGrade, index, self) => index === self.findIndex(item => item.Id === subjectAndGrade.Id)
  );

  // Check when selecting an existing planning whether the opening and closing dates have already passed
  const validateDateRange = (openDate, closeDate) => {
    const today = moment().startOf('day');
    const openDateFormatted = openDate ? moment(openDate).startOf('day') : null;
    const closeDateFormatted = closeDate ? moment(closeDate).startOf('day') : null;

    const openDatePast = openDateFormatted?.isBefore(today) ?? false;
    const closeDatePast = closeDateFormatted?.isBefore(today) ?? false;
    const rangeInvalid = (openDateFormatted && closeDateFormatted) ? openDateFormatted.isAfter(closeDateFormatted) : false;

    const warnings = { openDatePast, closeDatePast, rangeInvalid };
    setDateWarnings(warnings);

    return warnings;
  };

  // Calculate weeks for a given academic period
  const calculateWeeksForPeriod = (periodId) => {
    const period = academicPeriods.find(period => period.Id === periodId);

    // Return 1 week by default if period not found
    if (!period) return 1;

    const today = moment().startOf('day');

    const fromDate = moment(period.PeriodFrom).isBefore(today) ? today.toDate() : moment(period.PeriodFrom).toDate();
    const toDate = moment(period.PeriodTo).toDate();

    return calculateNumberOfWeeks(fromDate, toDate);
  };

  return (
    <div className="wrapper-halfCol">
      <div className="contentForms">
        <div className="form-group-fields inputWidth-lg">
          <FormSelect
            name="period"
            label={
              <span>
                {!isForEdit && <span className="required">* </span>}
                <Translate id="planning.period" />
              </span>
            }
            placeholder={translate("general.select")}
            options={academicPeriods.map((period) => ({
              value: period.Id,
              label: `${period.Id}: ${period.PeriodFrom.split(" ")[0]} - ${
                period.PeriodTo.split(" ")[0]
              } `,
            }))}
            onChange={handlePeriod}
            value={planning.periodId}
            disabled={isLoading || isForEdit || disableForm}
            searchable
          />
          {periodExpired && (
            <Alert color="danger" className="mt-2">
              <FontAwesomeIcon icon={faExclamationCircle} />{" "}
              <span style={{ fontSize: "13px" }}>
                <Translate id="planning.select-an-active-period" />
              </span>
            </Alert>
          )}
        </div>
        <FormInput
          name="name"
          label={
            <span>
                {!isForEdit && <span className="required">* </span>}
                <Translate id="planning.planning-name" />
            </span>
          }
          placeholder={translate("planning.planning-name")}
          onChange={(e) => handleChange("Name", e?.target.value)}
          value={createPlanningModel.Name}
          disabled={isLoading || disableForm}
        />
        <Text type="hint">
          <span>
            <Translate id="planning.add-name-message" />{" "}
            <strong>
              <Translate id="planning.name-example" />
            </strong>
          </span>
        </Text>
        {isForEdit || (!isForEdit && periodId) ?
            <>
              <Heading as="h5" className="mt-4">
          <Translate id="planning.period-message" />
        </Heading>
              <div className="wrapperFlex contentSize-lg contentForms-inline">
          <div className="col pl-0">
            <FormDatePicker
              name="openDate"
              label={
            <span>
                {!isForEdit && <span className="required">* </span>}
                <Translate id="planning.start-date" />
            </span>
          }
              onChange={(e) => handleDate("OpenDate", e)}
              minDate={minDate}
              maxDate={maxDate}
              value={openDate}
              disabled={isLoading || disableForm || isForEdit || datesLocked}
            />
            {showOpenDateError && (
                <Alert color="warning">
                  <FontAwesomeIcon icon={faExclamationCircle} />{' '}
                  <span style={{ fontSize: "13px" }}>
                    <Translate id="planning.open-date-error" />
                  </span>
                </Alert>
                )}
          </div>

          <div className="col">
            <FormDatePicker
              name="closeDate"
              label={
                <span>
                    {!isForEdit && <span className="required">* </span>}
                    <Translate id="planning.end-date" />
                </span>
              }
              onChange={(e) => handleDate("CloseDate", e)}
              minDate={minDate}
              maxDate={maxDate}
              value={closeDate}
              disabled={isLoading || disableForm || isForEdit || datesLocked}
            />

            {showCloseDateError && (
                <Alert color="warning">
                  <FontAwesomeIcon icon={faExclamationCircle} />{' '}
                  <span style={{ fontSize: "13px" }}>
                    <Translate id="planning.close-date-error" />
                  </span>
                </Alert>
                )}
          </div>

          <div className="col pr-0">
            <FormInput
              name="weekNumber"
              label={
                <span>
                  {!isForEdit && <span className="required">* </span>}
                  <Translate id="planning.week-number" />
                </span>
              }
              placeholder={translate("planning.week-number")}
              onChange={(e) =>
                handleChange("WeekNumber", parseInt(e?.target.value || "0"))
              }
              value={weekNumber}
              disabled={isLoading || disableForm}
            />
          </div>
          {!dateWarnings.rangeInvalid && (dateWarnings.openDatePast || dateWarnings.closeDatePast) && !periodExpired && (
            <Alert color="warning">
              <FontAwesomeIcon icon={faExclamationCircle} />{" "}
              <span style={{ fontSize: "13px" }}>
                <Translate id="planning.cloned-dates-outdated-warning" />
              </span>
            </Alert>
          )}
        </div>
              {subjectsAndGrades && subjectsAndGrades.length > 0 && (
                <FormSelect
                  name="subject-grade"
                  label={<span>
                    {!isForEdit && <span className="required">* </span>}
                    <Translate id="planning.subject-grade" />
                  </span>}
                  placeholder={translate("general.select")}
                  options={deduplicatedData.map((subjectsAndGrade) => ({
                    value: subjectsAndGrade.Id,
                    label: subjectsAndGrade.SubjectName,
                  }))}
                  onChange={handleGradeChange}
                  value={subjectAndGradeId}
                  disabled={isLoading || disableForm}
                  searchable
                />
              )}
        
            {studentsGroup && studentsGroup.length > 0 && (
                <FormMultiSelect
              name="studentGroups"
              label={<Translate id="planning.student-group" />}
              placeholder={translate("general.select")}
              options={studentsGroup.map((group) => ({
                value: group.id,
                label: group.name,
              }))}
              onChange={handleGroupsChange}
              values={createPlanningModel.Groups &&
                Object.values(
                  createPlanningModel.Groups.reduce((acc, item) => {
                    acc[item.GroupId] = item;
                    return acc;
                  }, {})
                ).map((group) => ({
                  label: group.GroupName,
                  value: group.GroupId,
                }))}
              disabled={isLoading || disableForm}
            />)}
            </> : null}
      </div>
    </div>
  );
}