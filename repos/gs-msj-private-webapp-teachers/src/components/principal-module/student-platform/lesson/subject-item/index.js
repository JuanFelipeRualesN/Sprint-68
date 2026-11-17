import React from "react";
import "./style.css";
import currentSubject from "../../../../../redux-students-area/actions/subject";
import { getLessonsBySubjectsAndGradesThuk } from "../../../../../redux-students-area/actions/lessons";
import { useDispatch, useSelector } from "react-redux";
import { assignColor } from "../../../../student-module/view/lesson-student/utils/lesson";
import { getAsset } from "../../../../../utils/getAssets";

const SubjectItem = ({ subject }) => {
  const dispatch = useDispatch();
  const item = useSelector((state) => state.subject);

  const hadleSubjectSelected = () => {
    dispatch(currentSubject(subject));
    dispatch(getLessonsBySubjectsAndGradesThuk(subject.Subject, subject.Level));
  };

  function assignIcon(subject) {
    switch (subject) {
      case "en":
        return "LessonEnglish-icon-Logo";
      case "sp":
        return "LessonSpanish-icon-Logo";
      case "mat-en":
        return "LessonMath-icon-Logo";
      case "sci-en":
        return "LessonScience-icon-Logo";
      case "mat-sp":
        return "LessonMath-icon-Logo";
      case "sci-sp":
        return "LessonScience-icon-Logo";
      case "fi-sp":
        return "LessonScience-icon-Logo";
      case "ca-sp":
        return "LessonScience-icon-Logo";
      case "bi-sp":
        return "LessonScience-icon-Logo";
      case "es-sp":
        return "LessonMath-icon-Logo";
      case "fpc-sp":
        return "LessonMath-icon-Logo";
      case "maa-sp":
        return "LessonMath-icon-Logo";
      case "pa-sp":
        return "LessonMath-icon-Logo";
      case "a1-sp":
        return "LessonMath-icon-Logo";
      case "a2":
        return "LessonMath-icon-Logo";
      case "geo-sp":
        return "LessonMath-icon-Logo";
      case "tr-sp":
        return "LessonMath-icon-Logo";
      case "pc-en":
        return "LessonMath-icon-Logo";
      case "qu-sp":
        return "LessonScience-icon-Logo";
      case "ci-sp":
        return "LessonScience-icon-Logo";
      case "ct-sp":
        return "LessonScience-icon-Logo";
      case "sci-so":
        return "LessonSocialStudies-icon-Logo";
      case "edf-sp":
        return "LessonEducacionFisica-icon-Logo";
      case "art-sp":
        return "LessonBellasArtes-icon-Logo";
      case "sld-sp":
        return "LessonSalud-icon-Logo";
      default:
        return "LessonDefault-icon-Logo";
    }
  }

  const iconName = assignIcon(subject.Subject);
  const imageLoad = getAsset(iconName);

  return (
    <>
      <div className="wrapper-lessons">
        <a
          className={
            item.Level === subject.Level && item.Subject === subject.Subject
              ? "item-active"
              : ""
          }
          onClick={hadleSubjectSelected}
        >
          <div className="col-left">
            <img src={imageLoad} />{" "}
            <span className="title-lesson">{subject.SubjectName}</span>
          </div>
          <div
            className="col-right"
            style={{ backgroundColor: `${assignColor(subject.Subject)}` }}
          >
            <svg viewBox="0 0 57 105.6">
              <path
                className="arrow-right"
                style={{ fill: `${assignColor(subject.Subject)}` }}
                d="M56.6,105.6V0.1h-6.4L5.6,44.7C1,49.2,1,56.6,5.6,61.1L50,105.6H56.6z"
              />
            </svg>
            <span className="lesson-grade">{subject.Level}</span>
          </div>
        </a>
      </div>
    </>
  );
};

export default SubjectItem;