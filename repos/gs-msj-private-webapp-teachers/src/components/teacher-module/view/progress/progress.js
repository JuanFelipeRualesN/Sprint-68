import React, { useEffect, useState } from "react";
import Select from "react-select";
import styled from "styled-components";
import moment from 'moment';
import { getSubscriptionsActivePeriods, getReportAverageByPeriod } from "../../../../api/subscriptions";
import Subject from "./subject";
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

const Progress = ({ studentId, setLoading }) => {
    const [periodSelected, setPeriodSelected] = useState(null);
    const [periods, setPeriods] = useState([]);
    const [averages, setAverages] = useState([]);
    const [subjectSelected, setSubjectSelected] = useState(null);
    const [homeworks, setHomeworks] = useState([]);

    useEffect(() => {
        getSubscriptionsActivePeriodsUser(studentId);
    }, []);

    const getSubscriptionsActivePeriodsUser = async (studentId) => {
        setLoading(true);
        try {
            const res = await getSubscriptionsActivePeriods(studentId);
            const data = res.data;
            const options = data.map(item => {
                return {
                    value: item.Id,
                    label: item.PeriodNumber + ": " + moment(item.PeriodFrom, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY") + " - " + moment(item.PeriodTo, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY")
                };
            });
            setPeriods(options);
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const getAveragesSubjects = async (periodId, studentId) => {
        setLoading(true);
        try {
            const res = await getReportAverageByPeriod(periodId, studentId);
            const data = res.data;
            setAverages(data);
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const getDetailsSubjectAndPeriod = async (periodId, studentId, subjectCode, levelCode) => {
        setLoading(true);
        try {
            const res = await getReportAverageByPeriod(periodId, studentId, subjectCode, levelCode);
            const data = res.data[0].Details;
            setHomeworks(data);
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const handleChangePeriods = (selected) => {
        setPeriodSelected(selected);
        getAveragesSubjects(selected.value, studentId);
        setHomeworks([]);
    };

    const hadleSubjectSelected = (subject) => {
        setSubjectSelected(subject);
        getDetailsSubjectAndPeriod(periodSelected.value, studentId, subject.SubjectCode, subject.LevelCode);
    };

    return (
        <>
            <div className="wrapper-lesson-tabs">
                <div className="container_two-col">
                    <div className="wrapper-student-progress">
                        <div className="general-subject">
                            <div className="general-subject__title-search">
                                <div className="title-search__col-left">
                                    <Translate id="progress-average.period" />
                                </div>
                                <div className="title-search__col-right">
                                    <Select
                                        classNamePrefix="mantine-like"
                                        className="mantine-like__control"
                                        value={periodSelected}
                                        onChange={handleChangePeriods}
                                        options={periods}
                                        placeholder={<Translate id="progress-average.select-period" />}
                                    />
                                </div>
                            </div>

                            <p><small><Translate id="progress-average.teacher-note" /></small></p>

                            {averages.length > 0 && <p><small><strong><Translate id="progress-average.select-matter" /></strong></small></p>}

                            {averages.length > 0 &&
                                averages.map((item, i) => (
                                    <Subject
                                        key={i}
                                        subject={item}
                                        hadleSubjectSelected={hadleSubjectSelected}
                                        subjectSelected={subjectSelected}
                                    />
                                ))}
                        </div>
                    </div>
                </div>

                <div className="container_two-col">
                    <div className="wrapper-detail-average">
                        <h3><Translate id="progress-average.title-table-assignments" /></h3>
                        {subjectSelected && <span className="wrapper-title-subject">{subjectSelected.SubjectName}{" "}{subjectSelected.LevelCode}</span>}
                        {homeworks.length > 0 && (
                            <table className="detail-average__table">
                                <thead>
                                    <tr>
                                        <th><Translate id="progress-average.table-colum-description" /></th>
                                        <th><Translate id="progress-average.table-colum-average" /></th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {homeworks.map((item, i) => (
                                        <tr key={i}>
                                            <td>
                                                {item.Title}
                                            </td>
                                            <td>{item.Average}%{" "}{item.AcademicGrade}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        )}
                        {!subjectSelected && (
                            <EmptyListStyled>
                                <i className="fas fa-book-open"></i>
                                <Translate id="progress-average.select-subject" />
                            </EmptyListStyled>
                        )}
                        {subjectSelected && homeworks.length === 0 && (
                            <EmptyListStyled>
                                <i className="fas fa-book-open"></i>
                                <Translate id="progress-average.task-empty" />
                            </EmptyListStyled>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
};

export default Progress;
