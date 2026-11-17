import React, { useEffect, useState } from "react";
import useGetProjectTypes from "../hooks/use-get-project-types";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import SessionManager from "../../../../../custom-classes/session-manager";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";
import moment from "moment";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function ProjectsDetail({ project, onChange = () => {} }) {
  const planning = SessionManager.getSelectedPlanning();
  const { getProjectTypes, types } = useGetProjectTypes();
  const [isSelectOthersEnabled, setIsSelectOthersEnabled] = useState(project.customs?.length > 0);
  const [projectState, setProjectState] = useState(project);

  const handleChange = (name, value) => {
    const newState = { ...projectState, [name]: value };
    setProjectState(newState);
    onChange(newState);
  };

  useEffect(() => {
    getProjectTypes();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getProjectTypes();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={projectState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="project.content" />
      </Heading>

      <Space variant="regular" />

      <FormCheckboxGroup
        name="projects"
        options={types?.defaults?.map((type) => ({
          value: type.id,
          label: type.description,
        }))}
        values={projectState.defaults}
        onChange={(values) => handleChange("defaults", values)}
      />

      <Space variant="regular" />

      <div className="wrapperFlex FlexAlingEnd w-100 mb-2 align-items-baseline">
        <FormCheckbox
          name="otherProjectTypeCheck"
          label={translate("general.other")}
          checked={isSelectOthersEnabled}
          onChange={setIsSelectOthersEnabled}
          disabled={projectState.customs?.length > 0}
        />
        {isSelectOthersEnabled && (
          <div className="col">
            <FormCreatableSelect
              name="otherProjectType"
              placeholder={translate("general.select")}
              options={types?.customs?.map((type) => ({
                value: type.id,
                label: type.description,
              }))}
              values={projectState.customs}
              onChange={(values) => handleChange("customs", values)}
            />
          </div>
        )}
      </div>
    </>
  );
}
