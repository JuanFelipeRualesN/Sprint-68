import React, { useEffect, useState } from "react";
import useGetReflectionsNotAchievingReasons from "../hooks/use-get-observations";
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

export default function ObservationsDetail({ observation, onChange = () => { } }) {
  const planning = SessionManager.getSelectedPlanning();
  const [observationState, setObservationState] = useState(observation);

  const handleChange = (name, value) => {
    const newState = { ...observationState, [name]: value };
    setObservationState(newState);
    onChange(newState);
  };

  useEffect(() => {
    if (observation.achieved === undefined) handleChange("achieved", true);

  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={observationState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="observation.content" />
      </Heading>

      <div className="wrapperFlex FlexAlingEnd w-75 mb-2">
        <div className="col detail pl-0">
          <FormTextarea
            name="detail"
            label={<Translate id="observation.detail" />}
            rows={3}
            value={observationState.detail}
            onChange={(event) => handleChange("detail", event.target.value)}
          />
        </div>
      </div>
    </>
  );
}
