import React, { useState } from "react";
import { translate } from "../../shared/utils";

export const Item = ({ item, index, onChange, isDisabled }) => {
  const handleChange = (e) => {
    const value = e.target.value;
    onChange(item.QuizQuestionModel.Id, value);
  };

  return (
    <>
      <div className="modalCorrections--Body_premisa">
        <div className="modalCorrections--Body_premisa--question d-flex">
          <span
            dangerouslySetInnerHTML={{
              __html: item.QuizQuestionModel.Desc,
            }}
          />
          <span className="qscore">
            (
            {`${item.QuizQuestionModel.CustomValue} ${translate(
              "assignment.preview-progress-points"
            )}`}
            )
          </span>
        </div>
        <div className="modalCorrections--Body_premisa--answer">
          <div className="premisa--answer-FormGroup">
            <textarea
              placeholder={translate("assignment.preview-response-student")}
              disabled={isDisabled}
              name={`text${index}`}
              onChange={handleChange}
            ></textarea>
          </div>
        </div>
      </div>
    </>
  );
};

export default function OpenQuestion({ quiz }) {
  const [values, setValues] = useState({});

  const handleFieldChange = (fieldId, value) => {
    setValues({ ...values, [fieldId]: value });
  };

  return (
    <>
      <div className="wrapper-iframe-form">
        <h1>{quiz.Title}</h1>
        <p className="instruction">
          <span
            dangerouslySetInnerHTML={{
              __html: quiz.Instruction,
            }}
          />
        </p>
        {quiz.QuizExtraInfoModel && (
          <div
            dangerouslySetInnerHTML={{
              __html: quiz.QuizExtraInfoModel.ExtraInfo,
            }}
          />
        )}

        {quiz.QuestionRequestModelList.map((item, i) => (
          <Item
            key={i}
            item={item}
            index={i}
            isDisabled={true}
            onChange={handleFieldChange}
          />
        ))}

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
