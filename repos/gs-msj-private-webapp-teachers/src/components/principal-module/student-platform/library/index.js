import React, { useState, useEffect } from "react";
import "./style.css";
import { Loader } from "../../../student-module/view/lesson-student/common/styled-components/loader";
import Select from "react-select";
import styled from "styled-components";
import {Translate} from "react-localize-redux";
import { currentGrade } from "../../../../redux-students-area/actions/grade";
import { getLessonsSuccess } from "../../../../redux-students-area/actions/lessons";
import { getSubjectsAndCoursesThuk } from "../../../../redux-students-area/actions/subjects";
import { useSelector, useDispatch } from "react-redux";
import { getLanguage } from "../../../student-module/view/lesson-student/utils/utils";
import { getGradesOrdinals } from "../../../student-module/view/lesson-student/utils/lesson";
import BookItem from "./book-item";
import { getLibraries } from "../../../../api/athenas";
import Pagination from "@material-ui/lab/Pagination";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";

const EmptyListStyled = styled.p`
  color: #ccc;
  display: block
  justify-content: center;
  text-align: center;
  margin: 3em auto;

  & img {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }

  & i {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }
`;

const SelectGrade = {
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

const Library = () => {
  const dispatch = useDispatch();
  const { grade, subjects } = useSelector((state) => ({
    grade: state.grade,
    subjects: state.subjects,
  }));
  const [library, setLibrary] = useState([]);
  const [loading, setLoading] = useState(false);
  const [gradeSelected, setGradeSelected] = useState(null);
  const [grades, setGrades] = useState([]);
  const [matterSelected, setMatterSelected] = useState(null);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [count, setCount] = useState(0);

  // Initialize translations based on the current version
  useVersionedTranslations();


  useEffect(() => {
    dispatch(getSubjectsAndCoursesThuk());
    loadLibraries(grade, undefined, page, pageSize);
  }, []);

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

  const loadLibraries = async (grade, subject, page, pageSize) => {
    page = page - 1;
    const req = {
      SubjectCodes: subject ? [subject] : [],
      LevelCodes: [grade],
      FromPage: page,
      Limit: pageSize,
    };
    setLoading(true);
    try {
      const res = await getLibraries(req);
      const data = res.data;
      setLibrary(data.AdditionalMaterials);
      setCount(data.TotalPages);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const handleChangeGrade = (selected) => {
    setPage(1);
    setGradeSelected(selected);
    dispatch(currentGrade(selected.value));
    dispatch(getLessonsSuccess([]));
    setMatterSelected(null);
    loadLibraries(selected.value, undefined, 1, pageSize);
  };

  const handleChangeSubject = (selected) => {
    setPage(1);
    setMatterSelected(selected);
    loadLibraries(grade, selected.value, 1, pageSize);
  };

  const handleChangePaginate = (event, value) => {
    setPage(value);
    loadLibraries(
      grade,
      matterSelected ? matterSelected.value : undefined,
      value,
      pageSize
    );
  };

  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <div className="wrapper-library">
            <h2><Translate id="student-platform.virtual-library" options={{ renderInnerHtml: true }} /></h2>
            <div className="content-list-library">
              <h3><Translate id="student-platform.select-grade-subject" options={{ renderInnerHtml: true }} /></h3>
              <div className="wrapper-col">
                <div className="two-col">
                  <div className="general-module">
                    <div className="selectTrelloGrade">
                      <Select
                        styles={SelectGrade}
                        value={gradeSelected}
                        classNamePrefix="mantine-like"
                        className="mantine-like__control"
                        onChange={handleChangeGrade}
                        options={grades}
                        placeholder={<Translate id="student-platform.select-grade" options={{ renderInnerHtml: true }} />}
                      />
                    </div>
                  </div>
                </div>
                <div className="two-col">
                  <div className="general-module">
                    <div className="selectTrelloGrade">
                      <Select
                        styles={SelectGrade}
                        value={matterSelected}
                        classNamePrefix="mantine-like"
                        className="mantine-like__control"
                        onChange={handleChangeSubject}
                        options={subjects
                          .filter((course) => course.Level === grade)
                          .map((item) => ({
                            value: item.Subject,
                            label: item.SubjectName,
                          }))
                          .sort((x, y) => {
                            let a = x.label.toUpperCase(),
                              b = y.label.toUpperCase();
                            return a == b ? 0 : a > b ? 1 : -1;
                          })}
                        placeholder={<Translate id="student-platform.select-subject" options={{ renderInnerHtml: true }} />}
                      />
                    </div>
                  </div>
                </div>
              </div>

              <div className="wrapper-col-library">
                {library.map((item, i) => (
                  <BookItem key={i} book={item.AdditionalMaterialModel} />
                ))}
              </div>

              {!loading && !library.length && (
                <EmptyListStyled>
                  <i className="fas fa-book-reader"></i> <Translate id="student-platform.empty-library" options={{ renderInnerHtml: true }} />
                </EmptyListStyled>
              )}
              {count > 1 && (
                <Pagination
                  count={count}
                  page={page}
                  onChange={handleChangePaginate}
                />
              )}
            </div>
          </div>
          {loading && <Loader />}
        </div>
      </div>
    </>
  );
};

export default withLocalize(Library);