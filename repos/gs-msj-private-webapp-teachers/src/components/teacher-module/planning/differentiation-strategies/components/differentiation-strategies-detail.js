import React, { useEffect, useState } from "react";
import useGetDifferentiationStrategyTypes from "../hooks/use-get-differentiation-strategies-types";
import useGetDifferentiationStrategyCategories from "../hooks/use-get-differentiation-strategies-categories";
import DateSelections from "../../shared/components/date-selections";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";
import FormSelect from "../../shared/components/form/form-select";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../../custom-classes/session-manager";
import LanguageManager from "../../../../../custom-classes/language-manager";
import moment from "moment";
import { translate } from "../../shared/utils";

const normalizeCategoryIds = (value) => {
    if (!value) return [];

    if (Array.isArray(value)) {
      return value.filter(Boolean);
    }


    return String(value)
      .split(",")
      .map((id) => id.trim())
      .filter(Boolean);
};

export default function DifferentiationStrategiesDetail({
  differentiationStrategy,
  onChange = () => {},
}) {
  const planning = SessionManager.getSelectedPlanning();
  const { getDifferentiationStrategyCategories, categories } = useGetDifferentiationStrategyCategories();
  const { getDifferentiationStrategyTypes, types } = useGetDifferentiationStrategyTypes();
  const [isSelectOthersEnabled, setIsSelectOthersEnabled] = useState(differentiationStrategy.customs?.length > 0);
  const [differentiationStrategyState, setDifferentiationStrategyState] = useState(() => ({
    ...differentiationStrategy,
    categoryId: normalizeCategoryIds(differentiationStrategy.categoryId),
  }));


  const [currentTypes, setCurrentTypes] = useState();


  const getTypesByCategory = (categoryIdOrIds) => {
    if (!types) {
      return { defaults: [], customs: [] };
    }

    // Normalize to array
    const categoryIds = Array.isArray(categoryIdOrIds)
      ? categoryIdOrIds.filter(Boolean)
      : categoryIdOrIds
      ? [categoryIdOrIds]
      : [];

    if (categoryIds.length === 0) {
      return { defaults: [], customs: [] };
    }

    const filterByCategories = (list = []) =>
      list.filter((item) => categoryIds.includes(item.categoryId));

    const uniqueById = (list = []) => {
      const seen = {};
      return list.filter((item) => {
        if (seen[item.id]) return false;
        seen[item.id] = true;
        return true;
      });
    };

    const defaults = uniqueById(filterByCategories(types.defaults || []));
    const customs = uniqueById(filterByCategories(types.customs || []));

    return { defaults, customs };
  };


  const handleChange = (name, value) => {
    const newState = { ...differentiationStrategyState, [name]: value };
    setDifferentiationStrategyState(newState);
    onChange(newState);
  };

  const handleCategoryChange = (value) => {
    const data = getTypesByCategory(value);
    setCurrentTypes(data);
    
    const newDifferentiationStrategy = {
      ...differentiationStrategyState,
      categoryId: value,
      defaults: [],
      customs: [],
    };
    setIsSelectOthersEnabled(false);
    setDifferentiationStrategyState(newDifferentiationStrategy);
    onChange(newDifferentiationStrategy);
  };

  useEffect(() => {
    const data = getTypesByCategory(normalizeCategoryIds(differentiationStrategy.categoryId));
    setCurrentTypes(data);
  }, [types]);

  useEffect(() => {
    getDifferentiationStrategyCategories();
    getDifferentiationStrategyTypes();
  }, []);

  useEffect(() => {
    getDifferentiationStrategyCategories();
    getDifferentiationStrategyTypes();
  }, [LanguageManager.getLanguage()]);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={differentiationStrategyState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <div className="wrapperFlex FlexAlingEnd">
        <div className="col pl-0 mt-4">
          <Heading as="h3">
            <Translate id="accommodation.content" />
          </Heading>
          <FormCheckboxGroup
            className="m-0 w-25"
            name="categories"
            label=""
            placeholder={translate("general.select")}
            options={categories}
            values={differentiationStrategyState.categoryId}
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
              <Translate id="accommodation.content" />
            </Heading>

            <Space variant="regular" />

            <FormCheckboxGroup
              name="strategies"
              options={currentTypes?.defaults?.map((type) => ({
                value: type.id,
                label: type.name,
              }))}
              values={differentiationStrategyState.defaults}
              onChange={(values) => handleChange("defaults", values)}
            />

            <Space variant="regular" />

            <div className="wrapperFlex FlexAlingEnd w-100 mb-2 align-items-baseline">
              <FormCheckbox
                name="otherStrategyTypeCheck"
                label={translate("general.other")}
                checked={isSelectOthersEnabled}
                onChange={setIsSelectOthersEnabled}
                disabled={differentiationStrategyState.customs?.length > 0}
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
                    values={differentiationStrategyState.customs}
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
