import { useState } from "react";
import * as evaluations from "../services/evaluations.service";
import { translate } from "../../shared/utils";

export const getCategoryName = (type) => {
  switch (type) {
    case "2":
      return translate("evaluation.formative");
    case "3":
      return translate("evaluation.summative");
    default:
      return translate("evaluation.diagnostic");
  }
};

export const getCategoryCode = (type) => {
  switch (type) {
    case "2":
      return "formative";
    case "3":
      return "summative";
    default:
      return "diagnostic";
  }
};

export const groupByCategories = (array) => {
  return array.reduce((result, currentValue) => {
    const groupKey = currentValue.Type;

    if (!result[groupKey]) {
      result[groupKey] = [];
    }

    result[groupKey].push(currentValue);

    return result;
  }, {});
};

const useGetEvaluationCategories = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getEvaluationCategories = async () => {
    setIsLoading(true);

    try {
      const response = await evaluations.getCategories();
      const categories = groupByCategories(response);

      const results = Object.keys(categories).map((category) => {
        return {
          name: getCategoryName(category),
          code: getCategoryCode(category),
          defaults: categories[category]?.filter(c => c.Custom === 0).map((type) => ({
            id: type.Id,
            description: type.Description,
          })),
          customs: categories[category]?.filter(c => c.Custom === 1).map((type) => ({
            id: type.Id,
            description: type.Description,
          })),
        };
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getEvaluationCategories,
    isLoading,
    succeeded,
    categories: data,
  };
};

export default useGetEvaluationCategories;
