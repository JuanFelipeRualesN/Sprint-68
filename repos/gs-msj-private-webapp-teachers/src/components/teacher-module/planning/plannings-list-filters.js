import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import FormDateRangePicker from "./shared/components/form/form-date-range-picker";
import FormField from "./shared/components/form/form-field";
import FormSelect from "./shared/components/form/form-select";
import moment from "moment";
import {
  faSearch,
  faTimesCircle,
} from "@fortawesome/free-solid-svg-icons";
import Button from "./shared/components/button";
import { Translate } from "react-localize-redux";
import useGetSubjectsAndGrades from "./shared/hooks/useGetSubjectsAndGrades";
import useGetStudentsGroup from "./shared/hooks/useGetStudentsGroup";
import { translate } from "./shared/utils";

export default function PlanningsListFilters({ initialValues, onChange }) {
  const [name, setName] = useState();
  const [subjectsAndGradesOptions, setSubjectsAndGradesOptions] = useState([]);
  const [studentsGroupOptions, setStudentsGroupOptions] = useState([]);
  const [filters, setFilters] = useState(initialValues);
  const { studentsGroup, getStudentsGroup } = useGetStudentsGroup();
  const { subjectsAndGrades, getSubjectsAndGrades } = useGetSubjectsAndGrades();
  const [selectedGroup, setSelectedGroup] = useState(null);
  const [selectedSubject, setSelectedSubject] = useState(null);
  const [selectedDates, setSelectedDates] = useState([null, null]);

  const handleFiltersChanges = (filter) => {
    const changes = {
      ...filters,
      ...filter,
    };

    setFilters(changes);
  };

  const handleDatesChanges = (values) => {
    if (values[0] && !values[1]) return;

    const [fromDate, toDate] = values.map((date) =>
      date ? moment(date).format("YYYY-MM-DD") : null
    );
    handleFiltersChanges({ fromDate, toDate });
  };

  const resetFilters = () => {
    setName("");
    setSelectedGroup(null);
    setSelectedSubject(null);
    setSelectedDates([null, null]);
    onChange({});
    setFilters({});
  };

  useEffect(() => {
    getStudentsGroup();
    getSubjectsAndGrades();
  }, []);

  useEffect(() => {
    if (name === undefined) return;
    const timeout = setTimeout(() => {
      handleFiltersChanges({ name });
    }, 400);
    return () => clearTimeout(timeout);
  }, [name]);

  useEffect(() => {
    if (!subjectsAndGrades || subjectsAndGrades.length <= 0) return;

    const options = subjectsAndGrades.map((subject) => {
      return {
        label: subject.name,
        value: subject.code,
      };
    });

    setSubjectsAndGradesOptions(options);
  }, [subjectsAndGrades]);

  useEffect(() => {
    if (!studentsGroup || studentsGroup.length <= 0) return;

    const options = studentsGroup.map((group) => {
      return {
        label: group.name,
        value: group.id,
      };
    });

    setStudentsGroupOptions(options);
  }, [studentsGroup]);

  return (
    <div className="wrapperFlex FlexAlingEnd">
      <div className="col pl-0">
        <FormField
          label={<Translate id="planning.topic" />}
          name="name"
          value={name}
          onChange={(event) => {
            setName(event.target.value);
          }}
          onEnter={(name) => handleFiltersChanges({ name })}
        />
      </div>
      <div className="col">
        <FormSelect
          label={translate("group-management.group-students")}
          name="studentsGroup"
          options={studentsGroupOptions}
          placeholder={translate("general.select")}
          value={selectedGroup}
          clearable={true}
          onChange={(value) => {
            setSelectedGroup(value);
            handleFiltersChanges({ teacherGroup: value });
          }}
          searchable
        />
      </div>
      <div className="col">
        <FormSelect
          label={translate("planning.subject")}
          name="subjectAndGrade"
          clearable={true}
          options={subjectsAndGradesOptions}
          placeholder={translate("general.select")}
          value={selectedSubject}
          onChange={(value) => {
            setSelectedSubject(value);
            handleFiltersChanges({ subjectCode: value });
          }}
          searchable
        />
      </div>
      <div className="col">
        <FormDateRangePicker
          label={<Translate id="planning.dates-from-to" />}
          name="dates"
          value={selectedDates}
          onChange={(values) => {
            setSelectedDates(values);
            handleDatesChanges(values);
          }}
        />
      </div>
      <div className="col ms-4 d-flex">
        <Button
          variant="simple"
          icon={<FontAwesomeIcon icon={faSearch} />}
          onClick={() => onChange(filters)}
        >
          <Translate id="general.filter" />
        </Button>
        <Button
          className="ml-2"
          icon={<FontAwesomeIcon icon={faTimesCircle} />}
          onClick={() => resetFilters()}
        >
          <Translate id="general.clear" />
        </Button>
      </div>
    </div>
  );
}
