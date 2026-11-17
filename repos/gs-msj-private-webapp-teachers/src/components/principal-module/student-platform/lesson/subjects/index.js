import React, { useState, useEffect } from "react";
import "./style.css";
import SubjectItem from "../subject-item";
import Select from "react-select";
import styled from "styled-components";
import { currentGrade } from "../../../../../redux-students-area/actions/grade";
import { getLessonsSuccess } from "../../../../../redux-students-area/actions/lessons";
import { useSelector, useDispatch } from "react-redux";
import { getLanguage } from "../../../../student-module/view/lesson-student/utils/utils";
import {Translate} from "react-localize-redux";
import { getGradesOrdinals } from "../../../../student-module/view/lesson-student/utils/lesson";

const SubjectEmptyStyled = styled.p`
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

const SelectTrelloGrade = {
  option: (provided, state) => ({
    ...provided,
    borderBottom: "1px solid #E9E9E9",
    color: state.isSelected ? "#FFF" : "#6F6F6F",
    padding: 10,
    fontSize: 12,
  }),

  singleValue: (provided, state) => {
    const opacity = state.isDisabled ? 0.5 : 1;
    const transition = "opacity 300ms";

    return { ...provided, opacity, transition };
  },
};

const Subjects = ({ subjects }) => {
  const dispatch = useDispatch();
  const { grade } = useSelector((state) => ({
    grade: state.grade,
  }));

  const [gradeSelected, setGradeSelected] = useState(null);
  const [grades, setGrades] = useState([]);

  useEffect(() => {
    const grades = Array.from(
      new Set(subjects.map((subject) => subject.Level))
    ).sort((a, b) => {
      return a - b;
    });
    const lists = grades.map((grade) => ({
      value: grade,
      label: getGradesOrdinals(grade, getLanguage()),
    }));
    setGrades(lists);
    setGradeSelected(
      grade
        ? { value: grade, label: getGradesOrdinals(grade, getLanguage()) }
        : {
            value: grades[0],
            label: getGradesOrdinals(grades[0], getLanguage()),
          }
    );
    if (!grade) {
      dispatch(currentGrade(grades.length ? grades[0] : null));
    }
  }, [subjects]);

  const handleChange = (selected) => {
    setGradeSelected(selected);
    dispatch(currentGrade(selected.value));
    dispatch(getLessonsSuccess([]));
  };

  return (
    <>
      <div className="total-wrapper_lessons">
        <div className="module-lesson-title">
          <div className="col-left--module-lesson-title">
            <Translate id="student-platform.select-subject" options={{ renderInnerHtml: true }} />
          </div>
          <div className="col-right--module-lesson-title">
            <div className="selectTrelloGrade">
              <Select
                styles={SelectTrelloGrade}
                value={gradeSelected}
                classNamePrefix="mantine-like"
                className="mantine-like__control"
                onChange={handleChange}
                options={grades}
                placeholder={<Translate id="student-platform.select-grade" options={{ renderInnerHtml: true }} />}
              />
            </div>
          </div>
        </div>
        {!subjects.filter((course) => course.Level === grade).length && (
          <SubjectEmptyStyled>
            <i className="fas fa-book-open"></i><Translate id="student-platform.empty-grade" options={{ renderInnerHtml: true }} />
          </SubjectEmptyStyled>
        )}
        {subjects
          .filter((course) => course.Level === grade)
          .map((subject, i) => (
            <SubjectItem key={i} subject={subject} />
          ))}
      </div>
    </>
  );
};

export default Subjects;