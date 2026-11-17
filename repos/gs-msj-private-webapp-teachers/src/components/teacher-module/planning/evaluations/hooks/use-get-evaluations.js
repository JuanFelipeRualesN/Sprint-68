import { useState } from "react";
import * as evaluations from "../services/evaluations.service";
import {
  getCategoryCode,
  getCategoryName,
} from "./use-get-evaluation-categories";

const useGetEvaluations = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getEvaluationTypes = (types, custom) => {
    return types?.filter(
      (p) => p.EvaluationType?.Custom === custom
    );
  };

  const getEvaluationTitle = (types) => {
    return types
      ?.map((type) => type.EvaluationType?.Description)
      ?.join(", ");
  };

  const groupByCategories = (array) => {
    return array.reduce((result, currentValue) => {
      const groupKey = currentValue.EvaluationType.Type;

      if (!result[groupKey]) {
        result[groupKey] = [];
      }

      result[groupKey].push(currentValue);

      return result;
    }, {});
  };

  const getCategoriesFromEvaluation = (types) => {
    const categories = groupByCategories(types);
    const results = Object.keys(categories).map((category) => {
      const defaults = getEvaluationTypes(categories[category], 0);
      const customs = getEvaluationTypes(categories[category], 1);

      return {
        name: getCategoryName(category),
        code: getCategoryCode(category),
        title: getEvaluationTitle(defaults),
        subtitle: getEvaluationTitle(customs),
        defaults: defaults?.map((type) => type.EvaluationTypeId),
        customs: customs?.map((type) => ({
          value: type.EvaluationTypeId,
          label: type.EvaluationType?.Description,
        })),
      };
    });

    return results;
  };

  const getEvaluations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await evaluations.getEvaluations(planningId);
      const results = response.map((evaluation) => {
        const categories = getCategoriesFromEvaluation(
          evaluation.TeacherPlanEvaluationTypes
        );
        const transformedCategories = categories?.reduce((acc, category) => {
          return { ...acc, [category.code]: { defaults: category.defaults, customs: category.customs } };
        }, {});

        return {
          id: evaluation.Id,
          categories,
          ...transformedCategories,
          implementationDates: evaluation.TeacherPlanEvaluationsDates.map(
            (i) => new Date(i.ImplementationDate)
          ),
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
    getEvaluations,
    isLoading,
    succeeded,
    evaluations: data,
  };
};

export default useGetEvaluations;
