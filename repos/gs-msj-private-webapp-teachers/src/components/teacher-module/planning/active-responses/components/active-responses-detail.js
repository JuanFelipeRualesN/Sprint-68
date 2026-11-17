import React, { useEffect, useState } from "react";
import useGetActiveResponseTypes from "../hooks/use-get-active-responses-types";
import SessionManager from "../../../../../custom-classes/session-manager";
import moment from "moment";
import { Translate } from "react-localize-redux";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import LanguageManager from "../../../../../custom-classes/language-manager";
import { translate } from "../../shared/utils";

export default function ActiveResponsesDetail({ activeResponse, onChange = () => { } }) {
  const planning = SessionManager.getSelectedPlanning();
  let { getActiveResponseTypes, types } = useGetActiveResponseTypes();
  const [activeResponseState, setActiveResponseState] = useState(() => ({
    ...activeResponse,
    activeResponseTypeIds: activeResponse.activeResponseTypeIds || [],
    includesStudentActiveResponse: activeResponse.includesStudentActiveResponse || 0,
    includesLearningCheckActivity: activeResponse.includesLearningCheckActivity || 0
  }));

  useEffect(() => {
    getActiveResponseTypes();
  }, [LanguageManager.getLanguage()]);

  const handleChange = (name, value) => {
    const newState = { ...activeResponseState, [name]: value };
    setActiveResponseState(newState);
    onChange(newState);
  };
  
  useEffect(() => {
    getActiveResponseTypes();
  }, []);


  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={activeResponseState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />


      <Heading as="h3" className="mb-0">
        <Translate id="active-response.content" />
      </Heading>

      <Space variant="regular" />

      <FormCheckboxGroup
        name="activeResponseTypeIds"
        options={types?.map((type) => ({
          value: type.id,
          label: type.description,
        }))}
        values={activeResponseState.activeResponseTypeIds}
        onChange={(values) => handleChange("activeResponseTypeIds", values)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="active-response.includes" />
      </Heading>

      <Space variant="small" />

      <div className="checkbox-container">
        <FormCheckbox
          name="includesStudentActiveResponse"
          label={translate("active-response.includes-active")}
          checked={!!activeResponseState.includesStudentActiveResponse}
          onChange={(checked) =>
            handleChange("includesStudentActiveResponse", checked ? 1 : 0)
          }
        />

        <FormCheckbox
          name="includesLearningCheckActivity"
          label={translate("active-response.includes-check")}
          checked={!!activeResponseState.includesLearningCheckActivity}
          onChange={(checked) =>
            handleChange("includesLearningCheckActivity", checked ? 1 : 0)
          }
        />
      </div>
    </>
  );
}
