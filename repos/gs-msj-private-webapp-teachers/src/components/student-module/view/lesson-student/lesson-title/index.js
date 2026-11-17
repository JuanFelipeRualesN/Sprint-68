import React from "react";
import "./style.css";
import styled from "styled-components";
import { assignColor } from "../utils/lesson";
import { getAsset } from "../../../../../utils/getAssets";

const LessonTitleStyled = styled.h4`
  background-color: #e8eff3;
  font-size: 16px;
  padding: 0.7em;
  color: #253084;
`;

const LessonTitle = ({ subject, lesson }) => {
  const assignIcon = (subject) => {
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
  const iconName = assignIcon(subject.SubjectCode);
  const imageLoad = getAsset(iconName);

  return (
    <LessonTitleStyled>
      <div className="student-lesson-title-subject">
        <div className="student-lesson-title-subject__left">
          <img src={imageLoad} />
        </div>

        <div className="student-lesson-title-subject__right">
          <span
            className="student-lesson-title-subject__subject"
            style={{ color: `${assignColor(subject)}` }}
          >
          </span>
          <span className="student-lesson-title-subject__lesson">
            {" "}
            {lesson.LessonTitle}{" "}
          </span>
        </div>
      </div>
    </LessonTitleStyled>
  );
};

export default LessonTitle;
