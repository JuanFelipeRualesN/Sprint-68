import React, { useEffect, useState } from "react";
import useGetAccommodationTypes from "../hooks/use-get-accommodation-types";
import DateSelections from "../../shared/components/date-selections";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../../custom-classes/session-manager";
import moment from "moment";
import { translate } from "../../shared/utils";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function AccommodationsDetail({
  accommodation,
  onChange = () => {},
}) {
  const planning = SessionManager.getSelectedPlanning();
  const { getAccommodationTypes, types } = useGetAccommodationTypes();
  const [isSelectOthersEnabled, setIsSelectOthersEnabled] = useState(accommodation.customs?.length > 0);
  const [accommodationState, setAccommodationState] = useState(accommodation);

  const handleChange = (name, value) => {
    const newState = { ...accommodationState, [name]: value };
    setAccommodationState(newState);
    onChange(newState);
  };

  useEffect(() => {
    getAccommodationTypes();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getAccommodationTypes();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={accommodationState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="accommodation.content" />
      </Heading>

      <Space variant="regular" />

      <FormCheckboxGroup
        name="accommodations"
        options={types?.defaults?.map((type) => ({
          value: type.id,
          label: type.name,
        }))}
        values={accommodationState.defaults}
        onChange={(values) => handleChange("defaults", values)}
      />

      <Space variant="regular" />

      <div className="wrapperFlex FlexAlingEnd w-100 mb-2 align-items-baseline">
        <FormCheckbox
          name="otherAccommodationTypeCheck"
          label={translate("general.other")}
          checked={isSelectOthersEnabled}
          onChange={setIsSelectOthersEnabled}
          disabled={accommodationState.customs?.length > 0}
        />
        {isSelectOthersEnabled && (
          <div className="col">
            <FormCreatableSelect
              name="otherAccommodationType"
              placeholder={translate("general.select")}
              options={types?.customs?.map((type) => ({
                value: type.id,
                label: type.description,
              }))}
              values={accommodationState.customs}
              onChange={(values) => handleChange("customs", values)}
            />
          </div>
        )}
      </div>
    </>
  );
}
