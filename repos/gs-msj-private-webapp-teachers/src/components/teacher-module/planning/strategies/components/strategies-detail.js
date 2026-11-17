import React, { useEffect, useState } from "react";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import useGetStrategyTypes from "../hooks/use-get-strategy-types";
import useGetStrategyCategories from "../hooks/use-get-strategy-categories";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import FormSelect from "../../shared/components/form/form-select";
import moment from "moment";
import SessionManager from "../../../../../custom-classes/session-manager";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function StrategiesDetail({ strategy, onChange = () => {} }) {
  const planning = SessionManager.getSelectedPlanning();
  const { getStrategyCategories, categories } = useGetStrategyCategories();
  const { getStrategyTypes, types } = useGetStrategyTypes();
  const [isSelectOthersEnabled, setIsSelectOthersEnabled] = useState(
    strategy.customs?.length > 0
  );
  const [currentTypes, setCurrentTypes] = useState();
  const [strategyState, setStrategyState] = useState(strategy);

  const getTypesByCategory = (categoryId) => {
    const defaults = types?.defaults
      ? types.defaults.filter((t) => t.categoryId === categoryId)
      : [];
    const customs = types?.customs
      ? types.customs.filter((t) => t.categoryId === categoryId)
      : [];

    return { defaults, customs };
  };

  const handleChange = (name, value) => {
    const newState = { ...strategyState, [name]: value };
    setStrategyState(newState);
    onChange(newState);
  };

  const handleCategoryChange = (value) => {
    const data = getTypesByCategory(value);
    setCurrentTypes(data);
    
    const newStrategy = {
      ...strategyState,
      categoryId: value,
      defaults: [],
      customs: [],
    };
    setIsSelectOthersEnabled(false);
    setStrategyState(newStrategy);
    onChange(newStrategy);
  };

  useEffect(() => {
    const data = getTypesByCategory(strategy.categoryId);
    setCurrentTypes(data);
  }, [types]);

  useEffect(() => {
    getStrategyCategories();
    getStrategyTypes();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getStrategyCategories();
    getStrategyTypes();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={strategyState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <div className="wrapperFlex FlexAlingEnd">
        <div className="col pl-0 mt-4">
          <Heading as="h3">
            <Translate id="accommodation.select-type" />
          </Heading>
          <FormSelect
            className="m-0 w-25"
            name="categories"
            label=""
            placeholder={translate("general.select")}
            options={categories}
            value={strategyState.categoryId}
            onChange={handleCategoryChange}
            searchable
          />
        </div>
      </div>

      <Space variant="regular" />

      {currentTypes &&
        (currentTypes.defaults?.length > 0 ||
          currentTypes.customs?.length > 0) && (
          <>
            <Heading as="h3" className="mb-0">
              <Translate id="strategy.content" />
            </Heading>

            <Space variant="regular" />

            <FormCheckboxGroup
              name="strategies"
              options={currentTypes?.defaults?.map((type) => ({
                value: type.id,
                label: type.name,
              }))}
              values={strategyState.defaults}
              onChange={(values) => handleChange("defaults", values)}
            />

            <Space variant="regular" />

            <div className="wrapperFlex FlexAlingEnd w-100 mb-2 align-items-baseline">
              <FormCheckbox
                name="otherStrategyTypeCheck"
                label={translate("general.other")}
                checked={isSelectOthersEnabled}
                onChange={setIsSelectOthersEnabled}
                disabled={strategyState.customs?.length > 0}
              />
              {isSelectOthersEnabled && (
                <div className="col">
                  <FormCreatableSelect
                    name="otherStrategyType"
                    placeholder={translate("general.select")}
                    options={currentTypes?.customs?.map((type) => ({
                      value: type.id,
                      label: type.description,
                    }))}
                    values={strategyState.customs}
                    onChange={(values) => handleChange("customs", values)}
                  />
                </div>
              )}
            </div>
          </>
        )}
    </>
  );
}
