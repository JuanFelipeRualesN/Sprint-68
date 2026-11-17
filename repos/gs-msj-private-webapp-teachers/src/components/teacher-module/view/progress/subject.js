import React from "react";
import { getAsset } from "../../../../utils/getAssets";

const Subject = ({
  subject,
  hadleSubjectSelected,
  subjectSelected,
}) => {

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
  const iconName = assignIcon(subject.SubjectCode);
  const imageLoad = getAsset(iconName);

  return (
    <>
      <div className="general-subject__mod-lessons">
        <a
          className={
            subjectSelected &&
              subjectSelected.LevelCode === subject.LevelCode &&
              subjectSelected.SubjectCode === subject.SubjectCode
              ? "active"
              : ""
          }
          onClick={() => hadleSubjectSelected(subject)}
        >
          <div className="mod-lessons--left">
            <img src={imageLoad} />{" "}
            <span className="mod-lessons--title-lesson">{subject.SubjectName}{" "}{subject.LevelCode}</span>
          </div>
          <div className="mod-lessons--right bg-average">
            <span className="mod-lessons--lesson-grade">
              <span className="average txt-average">
                {subject.Average}%{"  "}{subject.AcademicGrade}
              </span>
            </span>
          </div>
        </a>
      </div>
    </>
  );
};

export default Subject;
