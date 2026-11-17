import React, { useEffect, useState } from "react";
import useGetMaterialTypes from "../hooks/use-get-material-types";
import SessionManager from "../../../../../custom-classes/session-manager";
import moment from "moment";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function MaterialsDetail({ material, onChange = () => {} }) {
  const planning = SessionManager.getSelectedPlanning();
  const { getMaterialTypes, types } = useGetMaterialTypes();
  const [isSelectOthersEnabled, setIsSelectOthersEnabled] = useState(
    material.customs?.length > 0
  );
  const [materialState, setMaterialState] = useState(material);

  const handleChange = (name, value) => {
    const newState = { ...materialState, [name]: value };
    setMaterialState(newState);
    onChange(newState);
  };

  useEffect(() => {
    getMaterialTypes();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getMaterialTypes();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={materialState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="material.content" />
      </Heading>

      <Space variant="regular" />

      <FormCheckboxGroup
        name="materials"
        options={types?.defaults?.map((type) => ({
          value: type.id,
          label: type.name,
        }))}
        values={materialState.defaults}
        onChange={(values) => handleChange("defaults", values)}
      />

      <Space variant="regular" />

      <div className="wrapperFlex FlexAlingEnd w-100 mb-2 align-items-baseline">
        <FormCheckbox
          name="otherMaterialTypeCheck"
          label={translate("general.other")}
          checked={isSelectOthersEnabled}
          onChange={setIsSelectOthersEnabled}
          disabled={materialState.customs?.length > 0}
        />
        {isSelectOthersEnabled && (
          <div className="col">
            <FormCreatableSelect
              name="otherMaterialType"
              placeholder={translate("general.select")}
              options={types?.customs?.map((type) => ({
                value: type.id,
                label: type.description,
              }))}
              values={materialState.customs}
              onChange={(values) => handleChange("customs", values)}
            />
          </div>
        )}
      </div>
    </>
  );
}
