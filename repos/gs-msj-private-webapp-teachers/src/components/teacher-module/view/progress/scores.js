import React, { useState, useEffect } from "react";
import Select from "react-select";
import {
  getAcademicYear,
  getAcademicYearGrades,
  getReportAcademicYearGrades
} from "../../../../api/subscriptions";
import { removeDuplicates } from "../../../../utils/utils";
import moment from 'moment';
import styled from "styled-components";
import { Translate } from "react-localize-redux";

const EmptyListStyled = styled.p`
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

const Scores = ({ studentId, setLoading }) => {
  const [academicYearsList, setAcademicYearsList] = useState([]);
  const [academicYearSelected, setAcademicYearSelected] = useState(null);
  const [grades, setGrades] = useState([]);
  const [cummulative, setCummulative] = useState([]);
  const [averagePerPeriod, setAveragePerPeriod] = useState([]);

  useEffect(() => {
    getAcademicYearUser(studentId);
  }, []);

  const getAcademicYearUser = async (studentId) => {
    setLoading(true);
    try {
      const res = await getAcademicYear(studentId);
      const data = res.data;
      const options = data.map(item => {
        return {
          value: item.Id,
          label: moment(item.FullTermStartDate, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY") + " - " + moment(item.FullTermEndDate, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY")
        };
      });
      setAcademicYearsList(options);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const handleChangeAcademicYear = (selected) => {
    setAcademicYearSelected(selected);
    getGradesByAcademicYear(selected.value, studentId);
  };

  const getGradesByAcademicYear = async (academicYearId, studentId) => {
    setLoading(true);
    try {
      const res = await getAcademicYearGrades(academicYearId, studentId);
      const data = res.data;

      setCummulative(data.cummulative);

      const avgpp = data.average_per_period.map(item => {
        return {
          score: item.score.toFixed(2),
          grade: item.grade,
          period_no: item.period_no
        };
      });
      setAveragePerPeriod(avgpp);

      const subjectCodes = removeDuplicates(data.period_details.map(item => ({ subject_code: item.subject_code, level_code: item.level_code, subject_name: item.subject_name })));

      let subjectGrades = [];
      const formatted = data.period_details.map(item => {
        return {
          subject_code: item.subject_code,
          grade: item.grade,
          score: item.score.toFixed(2),
          level_code: item.level_code,
          subject_name: item.subject_name,
        };
      });

      subjectCodes.forEach(element => {
        let filtered = formatted.filter(item => item.subject_code === element.subject_code && item.level_code === element.level_code);
        subjectGrades.push({
          subject_code: element.subject_code,
          level_code: element.level_code,
          subject_name: element.subject_name,
          grades: filtered,
        });
      });
      setGrades(subjectGrades);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const downloadPDF = async (academicYearId, studentId) => {
    setLoading(true);
    try {
      const res = await getReportAcademicYearGrades(academicYearId, studentId);
      const url = res.data;
      if (url) {
        window.open(url, "_blank");
      }
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  return (
    <>
      <div className="wrapper-col space-between">
        <h1 className="title-table-progress"><Translate id="progress-average.academic-year" /></h1>
        <div className="title-search__col-right">
          <Select
              classNamePrefix="mantine-like"
              className="mantine-like__control"
            onChange={handleChangeAcademicYear}
            options={academicYearsList}
            placeholder={<Translate id="progress-average.select-year" />}
          />
        </div>
      </div>
      {averagePerPeriod.length > 0 && <div className="wrapper-table-homework">
        <div className="wrapper-download">
          <button type="submit" className="button-primary" onClick={() => downloadPDF(academicYearSelected.value, studentId)}>
            <Translate id="progress-average.download" />
          </button>
        </div>

        <table className="content-table-homework content-table-progress">
          <thead>
            <tr>
              <th><Translate id="progress-average.subjects" /></th>
              {averagePerPeriod.map((item, i) => (
                <th key={i}><Translate id="progress-average.period-colum" /> {item.period_no}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {grades.map((grade, i) => (
              <tr key={i}>
                <td className="table-progress--title">{grade.subject_name} {" "} {grade.level_code}</td>
                {grade.grades.map((x, j) =>
                  <td key={j}>
                    {<span><span className="letter-progress">{x && x.score}%</span> {" "} {x && x.grade}</span>}
                  </td>
                )}
              </tr>
            ))}

            <tr className="footer-table-progress">
              <td className="table-progress--title"><Translate id="progress-average.average-footer" /></td>
              {averagePerPeriod.map((item, i) => (
                <td key={i}>
                  {<span><span className="letter-progress">{item && item.score}%</span> {" "} {item && item.grade}</span>}
                </td>
              ))}
            </tr>
          </tbody>
        </table>
      </div>}

      {cummulative.length > 0 && (<>
        <div className="wrapper-col space-between mt-3">
          <h1 className="title-table-progress"><Translate id="progress-average.cumulative" /></h1>
        </div>
        <div className="wrapper-table-homework">
          <table className="content-table-homework content-table-progress">
            <thead>
              <tr>
                <th><Translate id="progress-average.subjects" /></th>
                <th><Translate id="progress-average.cumulative-colum" /></th>
              </tr>
            </thead>
            <tbody>
              {cummulative.map((item, i) => (
                <tr key={i}>
                  <td className="table-progress--title">{item.subject_name} {" "} {item.level_code}</td>
                  <td>{<span><span className="letter-progress">{item && item.score}%</span> {" "} {item && item.grade}</span>}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div></>)}
      {!academicYearSelected && (
        <EmptyListStyled>
          <i className="fas fa-book-open"></i>
          <Translate id="progress-average.select-year-academic" />
        </EmptyListStyled>
      )}
      {academicYearSelected && !averagePerPeriod.length && (
        <EmptyListStyled>
          <i className="fas fa-book-open"></i>
          <Translate id="progress-average.year-academic-empty" />
        </EmptyListStyled>
      )}
    </>
  );
};

export default Scores;
