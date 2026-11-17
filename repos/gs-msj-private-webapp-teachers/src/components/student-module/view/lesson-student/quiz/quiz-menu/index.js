import React from "react";
import "./style.css";
import { useHistory } from "react-router-dom";
import {Translate} from "react-localize-redux";
import { getAsset } from "../../../../../../utils/getAssets";

const IconPractice = getAsset('LessonsMenu-icon-Practice');
const IconChallenge = getAsset('LessonsMenu-icon-Challenge');
const IconBack = getAsset('LessonsMenu-icon-Back');
const IconLessonMark = getAsset('Homework-icon-LessonMark');

const QuizMenu = ({ quiz, fromLearningGap, studentId, studentName }) => {
  const history = useHistory();

  const goBack = () => {
    const params = new URLSearchParams();

    if (fromLearningGap != null) params.append("fromLearningGap", fromLearningGap);
    if (studentId != null) params.append("studentId", studentId);
    if (studentName != null) params.append("studentName", studentName);

    const queryString = params.toString();
    const basePath =
        quiz?.IsExam === "0"
            ? `/students/lesson/${quiz.LessonId}?tab=practice`
            : quiz?.IsExam === "1"
                ? `/students/lesson/${quiz.LessonId}?tab=challenge`
                : null;

    if (basePath) {
      history.push(queryString ? `${basePath}&${queryString}` : basePath);
    } else {
      history.goBack();
    }
  };

  return (
      <div className="student-wrapper-menu">
        <ul className="student-navigation-menu">
          {quiz?.IsExam === "0" && (
              <li className="active">
                <a>
                  <img src={IconPractice} />
                  <span className="student-navigation-menu--item">
                <Translate id="student.practices" options={{ renderInnerHtml: true }} />
              </span>
                </a>
                <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
              </li>
          )}
          {quiz?.IsExam === "1" && (
              <li className="active">
                <a>
                  <img src={IconChallenge} />
                  <span className="student-navigation-menu--item">
                <Translate id="student.exam" options={{ renderInnerHtml: true }} />
              </span>
                </a>
                <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
              </li>
          )}
          <li className="highlight-item">
            <a onClick={goBack}>
              <img src={IconBack} />
              <span className="student-navigation-menu--item">
              <Translate id="student.back" options={{ renderInnerHtml: true }} />
            </span>
            </a>
          </li>
        </ul>
      </div>
  );
};

export default QuizMenu;

