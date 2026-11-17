import React from "react";
import "./style.css";
import { useSelector } from "react-redux";
import LessonItem from "../lesson-item";
import styled from "styled-components";
import {Translate} from "react-localize-redux";
import { SkeletonStyled } from "../../../../student-module/view/lesson-student/common/styled-components/skeleton";

const LessonEmptyStyled = styled.p`
  color: #ccc;
  display: block
  justify-content: center;
  text-align: center;
  margin: 3em auto;

  & i {
    display: block;
    margin: 2em 0 0.25em 0;
    font-size: 45px;
    opacity: .5;
  }
`;

const LessonList = () => {
  const { subject, lessons, loading } = useSelector((state) => ({
    subject: state.subject,
    lessons: state.lessons,
    loading: state.loading,
  }));

  let component;
  if (!lessons?.length) {
    component = (
      <LessonEmptyStyled>
        <i className="fas fa-book-open"></i>
        <Translate id="student-platform.empty-subject" options={{ renderInnerHtml: true }} />
      </LessonEmptyStyled>
    );
  } else {
    component = (
      <div className="wrapper-lessons-progress">
        {lessons.map((lesson, i) => (
          <LessonItem key={i} lesson={lesson} />
        ))}
      </div>
    );
  }

  return (
    <>
      <div className="total-wrapper_lesson-progress">
        <h3><Translate id="student-platform.select-lesson" options={{ renderInnerHtml: true }} /></h3>
        {lessons?.length ? <h4>{subject.SubjectName}{" "}{subject.Level}</h4> : null}
        {loading ? (
          <SkeletonStyled>
            <div className="area"></div>
            <div className="area"></div>
            <div className="area"></div>
            <div className="area"></div>
            <div className="area"></div>
            <div className="area"></div>
          </SkeletonStyled>
        ) : (
          component
        )}
      </div>
    </>
  );
};

export default LessonList;