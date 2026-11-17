import React, { useEffect, useState } from "react";
import useGetReflectionsNotAchievingReasons from "../hooks/use-get-reflection-reasons";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import FormTextarea from "../../shared/components/form/form-text-area";
import FormRadioButtom from "../../shared/components/form/form-radio-button";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";
import moment from "moment";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function ReflectionsDetail({ reflection, onChange = () => { } }) {
  const planning = SessionManager.getSelectedPlanning();
  const {
    getReflectionsNotAchievingReasons,
    reasons,
  } = useGetReflectionsNotAchievingReasons();
  const [reflectionState, setReflectionState] = useState(reflection);

  const handleChange = (name, value) => {
    const newState = { ...reflectionState, [name]: value };
    setReflectionState(newState);
    onChange(newState);
  };

  useEffect(() => {
    if (reflection.achieved === undefined) handleChange("achieved", true);
    getReflectionsNotAchievingReasons();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    if (reflection.achieved === undefined) handleChange("achieved", true);

    getReflectionsNotAchievingReasons();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={reflectionState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="reflection.content" />
      </Heading>

      <FormRadioButtom
        name="achieved"
        label=""
        options={[
          { label: translate("general.yes"), value: "true" },
          { label: translate("general.no"), value: "false" },
        ]}
        value={reflectionState.achieved === undefined ? "true" : reflectionState.achieved.toString()}
        onChange={(value) => handleChange("achieved", value === "true")}
      />

      <Space variant="regular" />

      {reflectionState.achieved === false && (
        <>
          <Heading as="h3" className="mb-0">
            <Translate id="reflection.select-reason" />
          </Heading>

          <Space variant="sm" />

          <FormCheckboxGroup
            name="reasons"
            options={reasons}
            values={reflectionState.reasons}
            onChange={(values) => handleChange("reasons", values)}
          />

          <Space variant="regular" />
        </>
      )}
      <div className="wrapperFlex FlexAlingEnd w-75 mb-2">
        <div className="col detail pl-0">
          <FormTextarea
            name="detail"
            label={<Translate id="reflection.detail" />}
            rows={3}
            value={reflectionState.detail}
            onChange={(event) => handleChange("detail", event.target.value)}
          />
        </div>
      </div>
    </>
  );
}
