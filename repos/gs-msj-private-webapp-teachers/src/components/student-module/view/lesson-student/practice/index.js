import React, { useState, useEffect } from "react";
import "./style.css";
import styled from "styled-components";
import { useHistory } from "react-router-dom";
import LessonTitle from "../lesson-title";
import { getLessonByIdPractices } from "../../../../../api/gsweb";
import {Translate} from "react-localize-redux";
import pdfIcon from "../../../../../assets/images/common-icons/Avanza-icon-PdfIcon.png";
import {getMaterialOffline} from "../../../../../api/athenas";
import docIcon from "../../../../../assets/images/common-icons/Avanza-icon-DocIcon.png";
import LoadingFullWindow from "../../../../shared-module-components/loader-windows/loader-full-window";

const EmptyListStyled = styled.p`
  color: #ccc;
  display: block;
  justify-content: center;
  text-align: center;
  margin: 3em auto;
`;

const PracticeItem = ({ quiz, fromLearningGap, studentId, studentName, lessonId }) => {
  const history = useHistory();
  const [loading, setLoading] = useState(false);

  const goTo = (path) => {
    history.push(path);
  };

  const getMaterialOfflineByLesson = async (format) => {
    setLoading(true);
    try {
      const res = await getMaterialOffline(lessonId, quiz.Id, format);
      const data = res.data;
      if (data) {
        window.open(data, "_blank");
      }
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  return (
    <div className="student-container-quiz-lesson">
      <div className="practice-item" style={{cursor: quiz.LevelOfKnowledge <= 2 ? 'pointer' : 'default'}}>
        {parseInt(quiz.LevelOfKnowledge) <= 2 ?
            <>
              <a>
                <div className="student-col-left--quiz-lesson">{quiz.Title}</div>
                <div className="student-col-right--quiz-lesson">
                  <a
                      className="student-button-generic"
                      onClick={() => {
                        const params = new URLSearchParams();

                        if (fromLearningGap != null && fromLearningGap !== false) params.append("fromLearningGap", fromLearningGap);
                        if (studentId != null) params.append("studentId", studentId);
                        if (studentName != null) params.append("studentName", studentName);

                        goTo(`/students/lesson/quiz/${quiz.Id}?${params.toString()}`);
                      }}
                  >
                    <Translate id="student.start-practice"
                               options={{renderInnerHtml: true}}/>
                  </a>
                </div>
              </a>
            </> :
            <>
              <a>
                <div className="student-col-left--quiz-lesson">{quiz.Title}</div>
                <div className="student-col-right--quiz-lesson" style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                  <img
                      src={pdfIcon}
                      className="download-icons"
                      alt="pdf icon"
                      style={{ cursor: 'pointer' }}
                      onClick={() => getMaterialOfflineByLesson("pdf")}
                  />
                  <img
                      src={docIcon}
                      className="download-icons"
                      alt="doc icon"
                      style={{ cursor: 'pointer' }}
                      onClick={() => getMaterialOfflineByLesson("doc")}
                  />
                </div>
              </a>
            </>
        }
      </div> {loading?
      <LoadingFullWindow /> : null}
    </div>
  );
};

const Practice = ({lesson, kids, subject, lessonId, setLoading, fromLearningGap, studentId, studentName}) => {
  const [practices, setPractices] = useState([]);

  useEffect(() => {
    getPracticesByLesson();
  }, []);

  const getPracticesByLesson = async () => {
    setLoading(true);
    try {
      const res = await getLessonByIdPractices(lessonId);
      const data = res.data;
      setPractices(data);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  let className = "student-wrapper-iframe";
  if (kids) {
    className = "student-wrapper-iframe-kids";
  }

  return (
    <div className="student-wrapper-student-content">
      <div className="student-center-container">
        <div className={className}>
          {lesson && <LessonTitle subject={subject} lesson={lesson} />}
          <div className="student-wrapper-quiz-list">
            <h3> <Translate id="student.select-practice"
                            options={{renderInnerHtml: true}}/></h3>
            <div className="wrapper-lessons-progress">
              {practices.length > 0 &&
                practices.filter(
                  (q) => q.IsExam === "0"
                ).map((quiz, i) => <PracticeItem key={i} quiz={quiz}
                                                 lessonId={lessonId}
                                                 fromLearningGap={fromLearningGap}
                                                 studentName={studentName}
                                                 studentId={studentId} />)}
            </div>
            {practices.length === 0 && (
              <EmptyListStyled> <Translate id="student.empty-practice"
                                           options={{renderInnerHtml: true}}/></EmptyListStyled>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Practice;
