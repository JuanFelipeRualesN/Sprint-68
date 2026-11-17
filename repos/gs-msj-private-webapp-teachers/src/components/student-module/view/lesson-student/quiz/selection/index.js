import React, { useState, useEffect } from "react";
import { useHistory, useLocation } from "react-router-dom";
import "./style.css";
import moment from "moment";
import "moment-timezone";
import { getTimeZone } from "../../../../../../utils/utils";
import {Translate} from "react-localize-redux";
import WrapperOverlappingMedia from "../../../../../teacher-module/wrapper-overlapping-media";


const Item = ({ item, index, onChange, submited, isExam, responsesSend }) => {

  const handleChange = (e) => {
    const value = e.target.value;
    onChange(item.QuizQuestionModel.Id, value);
  };

  const mark = responsesSend.find(elem => elem.QuestionId === item.QuizQuestionModel.Id);

  return (
    <>
      <form className="student-content-form">
        <div
          className="quiz-title only-image-inline"
          dangerouslySetInnerHTML={{
            __html: item.QuizQuestionModel.Desc,
          }}
        />
        {item.QuizQuestionResponseModelList.map((res, i) => (
          <span className="student-quiz_radio-button" key={i}>
            {!submited ? (<input
              type="radio"
              value={res.Id}
              name={`radio${index}`}
              onChange={handleChange}
            />) : (<input
              type="radio"
              className={
                res.Correct === "1" && isExam === "0" ? "approved" : ""
              }
              disabled={submited}
              value={res.Id}
              name={`radio${index}`}
              checked={mark && mark.ResponseId === res.Id}
            />
            )}
            <label htmlFor={`res${i}`}>
              <div className="only-image-inline"
                dangerouslySetInnerHTML={{
                  __html: res.Response,
                }}
              />
            </label>
          </span>
        ))}
      </form>
    </>
  );
};

let timeLimitTimeout;
let timeLimitWarnTimeout;
let timeLimitDangerTimeout;

const Selection = ({ quiz, subject, level, getResponseQuiz, fromLearningGap, studentId, studentName }) => {
  const history = useHistory();
  let query = new URLSearchParams(useLocation().search);
  let h = query.get("h");
  const dataTimeIso = h ? moment(h).tz(getTimeZone())
    .format("MM/DD/YYYY hh:mm:ss a") : "";
  const [values, setValues] = useState({});
  const [open, setOpen] = useState(false);
  const [result, setResult] = useState(null);
  const [submited, setSubmited] = useState(null);
  const [lock, setLock] = useState(false);
  const [openSorry, setOpenSorry] = useState(false);
  const [classTimeLimit, setClassTimeLimit] = useState("status-time-test--process");
  const [responsesSend, setResponsesSend] = useState([]);

  const handleFieldChange = (fieldId, value) => {
    setValues({ ...values, [fieldId]: value });
  };

  useEffect(() => {
    if (h && quiz.QuizModel.IsExam === "1") {
      const minutesDiff = moment(h).diff(moment(new Date().toISOString()), 'minutes');
      const seconds = minutesDiff * 60;
      setClassTimeLimit(getStyleWarning(minutesDiff));
      if (minutesDiff >= 0) {
        const hour = moment(h).tz(getTimeZone())
          .format("hh:mm:ss a");
        if (minutesDiff >= 15) {
          const minutesWarn = moment(h).subtract(15, 'minutes').diff(moment(new Date().toISOString()), 'minutes');
          const secondsWarn = minutesWarn * 60;
          timeLimitWarnTimeout = setTimeout(() => {
            setClassTimeLimit("status-time-test--pending");
          }, secondsWarn * 1000);
        }
        if (minutesDiff >= 5) {
          const minutesDanger = moment(h).subtract(5, 'minutes').diff(moment(new Date().toISOString()), 'minutes');
          const secondsDanger = minutesDanger * 60;
          timeLimitDangerTimeout = setTimeout(() => {
            setClassTimeLimit("status-time-test--alert");
          }, secondsDanger * 1000);
        }
        timeLimitTimeout = setTimeout(() => {
          handleClickOpenSorry();
          setClassTimeLimit("status-time-test--alert");
        }, seconds * 1000);
      } else {
        handleClickOpenSorry();
      }
    }
    return () => {
      if (timeLimitTimeout) {
        clearTimeout(timeLimitTimeout);
      }
      if (timeLimitWarnTimeout) {
        clearTimeout(timeLimitWarnTimeout);
      }
      if (timeLimitDangerTimeout) {
        clearTimeout(timeLimitDangerTimeout);
      }
    };
  }, []);

  const getStyleWarning = (minutesDiff) => {
    if (minutesDiff >= 15) {
      return "status-time-test--process";
    } else if (minutesDiff >= 5 && minutesDiff < 15) {
      return "status-time-test--pending";
    } else {
      return "status-time-test--alert";
    }
  };

  const handleSubmit = async () => {
    const request = Object.entries(values).map((value) => ({
      QuizId: quiz.QuizModel.Id,
      QuestionId: value[0],
      ResponseId: value[1],
      SubjectCode: subject,
      LevelCode: level,
    }));
    setLock(true);
    try {
      handleClickOpen();
      setSubmited(true);
      getResponseQuiz(quiz.QuizModel.IsExam);
      setResponsesSend(request);
    } catch {
    } finally {
      setLock(false);
    }
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

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleClickOpenSorry = () => {
    setOpenSorry(true);
  };

  const handleCloseSorry = () => {
    setOpenSorry(false);
  };

  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <WrapperOverlappingMedia section={quiz}>
            <div className="student-wrapper-iframe-form">
              <h4 className="student-title-page">{quiz.QuizModel.Title}</h4>
              {result && quiz.QuizModel.IsExam === "2" || result && quiz.QuizModel.IsExam === "3" ? null : <><h1>{quiz.QuizModel.Title}</h1>
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

                {quiz.QuestionRequestModelList.map((item, i) => (
                  <Item
                    key={i}
                    item={item}
                    index={i}
                    submited={submited}
                    isExam={quiz.QuizModel.IsExam}
                    onChange={handleFieldChange}
                    responsesSend={responsesSend}
                  />
                ))}

                  <div>
                    <button
                      className="student-button-quiz"
                      type="button"
                      disabled={true}
                      onClick={handleSubmit}
                    >
                      <Translate id="student.submit"
                                options={{renderInnerHtml: true}}/>
                    </button>
                    <button
                      className="student-button-quiz-back"
                      type="button"
                      onClick={goBack}
                    >
                      <Translate id="student.back"
                                options={{renderInnerHtml: true}}/>
                    </button>
                  </div>
                  </>}

              {result && quiz.QuizModel.IsExam === "2" ? (<div className="container-message">
                <p><i className="icon-message-test fas fa-check-circle"></i></p>
                <p><strong>
                  <Translate id="student.end-quiz"
                            options={{renderInnerHtml: true}}/>
                </strong></p>
                <p> <Translate id="student.ready-quiz"
                              options={{renderInnerHtml: true}}/></p>
                <button
                  className="student-button-quiz"
                  type="button"
                  onClick={goBack}
                >
                  <Translate id="student.continue-to-lesson"
                            options={{renderInnerHtml: true}}/>
                </button>
              </div>) : null}

              {result && quiz.QuizModel.IsExam === "3" ? (<div className="container-message">
                <p><i className="icon-message-test fas fa-check-circle"></i></p>
                <p><strong>
                  <Translate id="student.end-quiz" />
                  </strong></p>
                <p> <Translate id="student.ready-quiz" /></p>
                <button
                  className="student-button-quiz"
                  type="button"
                  onClick={goBack}
                >
                  <Translate id="student.back" />
                </button>
              </div>) : null}
            </div>
          </WrapperOverlappingMedia>
        </div>
      </div>
    </>
  );
};

export default Selection;

