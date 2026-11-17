import React, { useState } from "react";
import { Item as SelectionItem } from "./selection";
import { Item as TrueOrFalseItem } from "./true-or-false";
import { Item as OpenQuestionItem } from "./open-question";
import MatchQuestionPreview from "./match-preview";
import { translate } from "../../shared/utils";

export default function Mixed({ quiz }) {
  const [values, setValues] = useState({});

  const handleFieldChange = (fieldId, value) => {
    setValues({ ...values, [fieldId]: value });
  };

  const score = quiz.QuestionRequestModelList.reduce((acc, item) => {
    const questionScore = item.QuizQuestionModel.CustomValue;

    if (!isNaN(Number(questionScore)) && questionScore !== "") {
      return acc + parseFloat(questionScore);
    }
    return isNaN(acc) ? 0 : acc;
  }, 0);

  return (
    <>
      <div className="wrapper-modalCorrections">
        <div className="modalCorrections--Title">
          {quiz.Title}
          <span className="qscore">
            ({`${translate("assignment.preview-progress-points")} ${score}`})
          </span>
        </div>

        <div className="modalCorrections--Desc">
          <div className="modalCorrections--Desc_desc">
            <span dangerouslySetInnerHTML={{ __html: quiz.Instruction }} />
          </div>
        </div>

        {quiz.QuizExtraInfoModel && (
          <div
            dangerouslySetInnerHTML={{
              __html: quiz.QuizExtraInfoModel.ExtraInfo,
            }}
          />
        )}
        <div className="modalCorrections--Body">
          {quiz.QuestionRequestModelList.map((item, i) => (
            <div key={i}>
              {quiz.QuizTypeId === "1" && (
                <SelectionItem
                  item={item}
                  index={i}
                  isDisabled={true}
                  onChange={handleFieldChange}
                />
              )}
              {quiz.QuizTypeId === "2" && (
                <MatchQuestionPreview quiz={quiz} />
              )}
              {quiz.QuizTypeId === "3" && (
                <OpenQuestionItem
                  item={item}
                  index={i}
                  isDisabled={true}
                  onChange={handleFieldChange}
                />
              )}
              {quiz.QuizTypeId === "4" && (
                <TrueOrFalseItem
                  item={item}
                  index={i}
                  isDisabled={true}
                  onChange={handleFieldChange}
                />
              )}
            </div>
          ))}
        </div>

        <div className="preview-docs-message">
          {quiz.QuizAttachmentModelList &&
            quiz.QuizAttachmentModelList.length !== 0 &&
            quiz.QuizAttachmentModelList.map((item, i) => (
              <div key={i}>
                <a href={item.AttachmentUrl} download>
                  {`${translate("assignment.preview-attachment")}: ${item.Id}`}
                </a>
                <br />
              </div>
            ))}
        </div>
      </div>
    </>
  );
}
