import React, { useEffect, useState } from "react";
import useGetSubjectsAndGrades from "../../shared/hooks/useGetSubjectsAndGrades";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";
import moment from "moment";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function IntegrationsDetail({
  integration,
  onChange = () => {},
}) {
  const { getSubjectsAndGrades, subjectsAndGrades } = useGetSubjectsAndGrades();
  const planning = SessionManager.getSelectedPlanning();
  const [integrationState, setIntegrationState] = useState(integration);

  const handleChange = (name, value) => {
    const newState = { ...integrationState, [name]: value };
    setIntegrationState(newState);
    onChange(newState);
  };
  
  useEffect(() => {
    getSubjectsAndGrades();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getSubjectsAndGrades();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={integrationState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="integration.content" />
      </Heading>

      <Space variant="regular" />

      <FormCheckboxGroup
        options={subjectsAndGrades?.map((subject) => {
          return {
            label: subject.name,
            value: subject.id,
          };
        })}
        values={integrationState.subjects}
        onChange={(values) => handleChange("subjects", values)}
      />
    </>
  );
}
