import React, { useState, useEffect } from "react";
import "./style.css";
import { useHistory } from "react-router-dom";
import {Translate} from "react-localize-redux";
import WrapperOverlappingMedia from "../../../../../teacher-module/wrapper-overlapping-media";


const alphabet = {
  0: "a",
  1: "b",
  2: "c",
  3: "d",
  4: "e",
  5: "f",
  6: "g",
  7: "h",
  8: "i",
  9: "j",
  10: "k",
  11: "l",
  12: "m",
  13: "n",
  14: "o",
  15: "p",
  16: "q",
  17: "r",
  18: "s",
  19: "t",
  20: "u",
  21: "v",
  22: "w",
  23: "x",
  24: "y",
  25: "z",
};

const Pairing = ({ quiz, subject, level }) => {

  const history = useHistory();

  const [values, setValues] = useState({});
  const [open, setOpen] = useState(false);
  const [submited, setSubmited] = useState(null);
  const [questionListRandom, setQuestionListRandom] = useState([]);

  useEffect(() => {
    const clone = [...quiz.QuestionRequestModelList];
    setQuestionListRandom(shuffle(clone));
  }, [quiz]);

  const handleFieldChange = (fieldId, value) => {
    setValues({ ...values, [fieldId]: value });
  };


  const goBack = () => {
    const params = new URLSearchParams();

    if (fromLearningGap != null) params.append("fromLearningGap", fromLearningGap);
    if (studentId != null) params.append("studentId", studentId);
    if (studentName != null) params.append("studentName", studentName);

    const queryString = params.toString();
    const basePath =
        quiz?.QuizModel?.IsExam === "0"
            ? `/students/lesson/${quiz.QuizModel.LessonId}?tab=practice`
            : quiz?.QuizModel?.IsExam === "1"
                ? `/students/lesson/${quiz.QuizModel.LessonId}?tab=challenge`
                : null;

    if (basePath) {
      history.push(queryString ? `${basePath}&${queryString}` : basePath);
    } else {
      history.goBack();
    }
  };

  function shuffle(array) {
    var tmp,
      current,
      top = array.length;

    if (top)
      while (--top) {
        current = Math.floor(Math.random() * (top + 1));
        tmp = array[current];
        array[current] = array[top];
        array[top] = tmp;
      }

    return array;
  }

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <WrapperOverlappingMedia section={quiz}>
            <div className="student-wrapper-iframe-form">
              <h4 className="student-title-page">{quiz.QuizModel.Title}</h4>

              <h1>{quiz.QuizModel.Title}</h1>
              <p className="student-instruction">
                <span
                  dangerouslySetInnerHTML={{
                    __html: quiz.QuizModel.Instruction,
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

              <form className="student-content-pareo">
                <div className="student-pareo-col-left">
                  <ul className="student-list-form">
                    {quiz.QuestionRequestModelList.map((item, i) => (
                      <li key={i}>
                        {i + 1}. ({" "}
                        <input
                          type="text"
                          name={`user${i}`}
                          disabled={submited}
                          placeholder="Ej. a"
                          onChange={(e) =>
                            handleFieldChange(
                              item.QuizQuestionModel.Id,
                              e.target.value.replace(/[^a-zA-Z\s]/g, '').substring(0, 1).toLowerCase()
                            )
                          }
                        />{" "}
                        ){" "}
                        <span
                          dangerouslySetInnerHTML={{
                            __html: item.QuizQuestionModel.Desc,
                          }}
                        />
                      </li>
                    ))}
                  </ul>
                </div>

                <div className="student-pareo-col-right">
                  <ul className="list-student-question">
                    {questionListRandom.map((item, i) => (
                      <li key={i}>
                        {alphabet[`${i}`]}.{" "}
                        <span
                          dangerouslySetInnerHTML={{
                            __html:
                              item.QuizQuestionResponseModelList[0].Response,
                          }}
                        />
                      </li>
                    ))}
                  </ul>
                </div>
              </form>
                <div>
                  <button
                    className="student-button-quiz-back"
                    type="button"
                    onClick={goBack}
                  >
                    <Translate id="student.back"  options={{renderInnerHtml: true}}/>
                  </button>
                </div>

            </div>
          </WrapperOverlappingMedia>
        </div>
      </div>
    </>
  );
};

export default Pairing;
