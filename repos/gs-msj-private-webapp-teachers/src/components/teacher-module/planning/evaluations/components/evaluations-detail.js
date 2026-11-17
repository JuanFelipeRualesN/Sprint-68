import React, { useEffect, useState } from "react";
import useGetEvaluationTypes from "../hooks/use-get-evaluation-categories";
import SessionManager from "../../../../../custom-classes/session-manager";
import moment from "moment";
import { Translate } from "react-localize-redux";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import EvaluationsItem from "./evaluations-item";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function EvaluationsDetail({ evaluation, onChange = () => { } }) {
  const planning = SessionManager.getSelectedPlanning();
  let { getEvaluationCategories, categories } = useGetEvaluationTypes();
  const [evaluationState, setEvaluationState] = useState(evaluation);

  useEffect(() => {
    getEvaluationCategories();
  }, [LanguageManager.getLanguage()]);

  const handleChange = (name, value) => {
    const newState = { ...evaluationState, [name]: value };
    setEvaluationState(newState);
    onChange(newState);
  };

  const handleCategoryChange = (code, name, value) => {
    const isCustoms = name === "customs";
    const currentState = evaluationState[code] || {};

    const updatedState = {
      ...(isCustoms ? { defaults: currentState.defaults } : { customs: currentState.customs }),
      [name]: value,
    };

    handleChange(code, updatedState);
  };

  useEffect(() => {
    getEvaluationCategories();
  }, []);

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={evaluationState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      {categories?.map((category) => {
        return (
          <>
            <Heading as="h3" className="mb-0">
              <Translate id="evaluation.content" /> {`(${category.name})`}
            </Heading>

            <Space variant="regular" />

            <FormCheckboxGroup
              options={category?.defaults?.map((type) => ({
                value: type.id,
                label: type.description,
              }))}
              values={evaluationState[category.code]?.defaults}
              onChange={(values) =>
                handleCategoryChange(category.code, "defaults", values)
              }
            />

            <Space variant="regular" />

            <EvaluationsItem
              code={category.code}
              values={evaluationState[category.code]?.customs}
              options={category?.customs}
              handleChange={handleCategoryChange}
            />
          </>
        );
      })}
    </>
  );
}
