import { useState } from "react";
import * as preview from "../services/preview.service";

const mapContent = (title, data) => {
  return data?.map((item) => ({
    id: item.Id,
    lessonId: item.LessonId,
    type: "default",
    title,
    description: item.Desc,
  }));
};

const mapConstructedCodeToContent = (lesson, data) => {
  return data?.map((item) => ({
    id: item.Id,
    lessonId: lesson.Id,
    type: "default",
    title: lesson.LessonTitle,
    description: item.CodeDescription,
  }));
};

const useGetLessonDetail = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getManyLessonDetail = async (lessonsId) => {
    setIsLoading(true);

    try {
      const response = await preview.getManyLessonDetail(lessonsId);
      const results = response.map((detail) => {
        const {
          LessonModifierRequestModel: {
            LessonModel,
            ConstructedCodeHandleModelList,
            LessonObjectiveModelList,
            LessonStrategyModelList,
            LessonTransversalThemeModelList,
            LessonPerformanceTaskModelList
          },
        } = detail;
        const constructedCodes = ConstructedCodeHandleModelList.flatMap(
          (codes) => codes.ConstructedCodeCodeModels
        );

        const uniqueStandardCodes = removeDuplicates(
          constructedCodes.filter((code) => code.CodeTypeId === "1"),
          'CodeDescription'
        );

        const uniqueExpectationCodes = removeDuplicates(
          constructedCodes.filter((code) => code.CodeTypeId === "7"),
          'CodeDescription'
        );

        return {
          standards: mapConstructedCodeToContent(
            LessonModel,
            uniqueStandardCodes
          ),
          expectations: mapConstructedCodeToContent(
            LessonModel,
            uniqueExpectationCodes
          ),
          goals: mapContent(LessonModel?.LessonTitle, LessonObjectiveModelList),
          strategies: mapContent(
            LessonModel?.LessonTitle,
            LessonStrategyModelList
          ),
          topics: mapContent(
            LessonModel?.LessonTitle,
            LessonTransversalThemeModelList
          ),
          units: mapContent(
            LessonModel?.LessonTitle,
            LessonPerformanceTaskModelList
          ),
        };
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
      setMessage("An error occur while getting lesson detail");
    }

    setIsLoading(false);
  };

  const removeDuplicates = (arr, propertyName) => {
    const seen = new Set();
    return arr.filter(item => {
      const value = item[propertyName];
      if (seen.has(value)) {
        return false;
      }
      seen.add(value);
      return true;
    });
  };

  return {
    getManyLessonDetail,
    isLoading,
    succeeded,
    message,
    lessonsDetail: data,
  };
};

export default useGetLessonDetail;
