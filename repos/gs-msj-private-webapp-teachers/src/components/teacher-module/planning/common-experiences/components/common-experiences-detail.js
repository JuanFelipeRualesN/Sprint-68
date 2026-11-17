import React, { useEffect, useState } from "react";
import useGetReflectionsNotAchievingReasons from "../hooks/use-get-common-experiences";
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

export default function CommonExperienceDetail({ commonExperience, onChange = () => { } }) {
  const planning = SessionManager.getSelectedPlanning();
  const [commonExperienceState, setCommonExperienceState] = useState(commonExperience);

  const handleChange = (name, value) => {
    const newState = { ...commonExperienceState, [name]: value };
    setCommonExperienceState(newState);
    onChange(newState);
  };

  useEffect(() => {
    if (commonExperience.achieved === undefined) handleChange("achieved", true);
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={commonExperienceState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="common-experience.content" />
      </Heading>

      <div className="wrapperFlex FlexAlingEnd w-75 mb-2">
        <div className="col detail pl-0">
          <FormTextarea
            name="detail"
            label={<Translate id="common-experience.detail" />}
            rows={3}
            value={commonExperienceState.detail}
            onChange={(event) => handleChange("detail", event.target.value)}
          />
        </div>
      </div>
    </>
  );
}
