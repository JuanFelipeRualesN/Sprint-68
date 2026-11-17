import React, {useEffect, useState} from "react";
import "./style.css";
import styled from "styled-components";
import { useHistory } from "react-router-dom";
import LessonTitle from "../lesson-title";
import {getLessonExams, getMaterialOffline} from "../../../../../api/athenas";
import { getTimeZone } from "../../../../../utils/utils";
import moment from "moment";
import "moment-timezone";
import {Translate} from "react-localize-redux";

const EmptyListStyled = styled.p`
  color: #ccc;
  display: block;
  justify-content: center;
  text-align: center;
  margin: 3em auto;
`;

const ChallengeItem = ({ quiz, lessonId, setLoading, fromLearningGap, studentId, studentName }) => {
  const history = useHistory();
  const startDateTime = quiz.DateAvailable ? moment(quiz.DateAvailable, "MM/DD/YYYY HH:mm:ss")
    .tz(getTimeZone())
    .format("MM/DD/YYYY hh:mm a") : "";
  const endDateTime = quiz.DateAvailable ? moment(quiz.DateAvailable, "MM/DD/YYYY HH:mm:ss").add(quiz.Duration, 'minutes')
    .tz(getTimeZone())
    .format("MM/DD/YYYY hh:mm a") : "";

  const goTo = (path) => {
    if (endDateTime && quiz.IsExam === "1") {
      history.push(`${path}?h=${new Date(endDateTime).toISOString()}`);
    } else {
      history.push(path);
    }
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

  const downloadQuiz = async (quizId, format) => {
    setLoading(true);
    try {
      const res = await getMaterialOffline(lessonId, quizId, format);
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
    <div className="student-container-quiz-lesson">
      <a>
        <div className="student-col-left--quiz-lesson">
          <span>{quiz.Title}</span>
          {<p className="student-quiz-lesson__download">
            <div className="student-quiz-lesson__download--btn">
              <div onClick={() => downloadQuiz(quiz.Id, "pdf")}><i className="student-quiz-lesson__icon-file-download fas fa-file-pdf student-quiz-lesson__icon-file-download__pdf"></i><span className="student-quiz-lesson__download-txt student-quiz-lesson__icon-file-download__pdf">PDF</span></div>
              <div onClick={() => downloadQuiz(quiz.Id, "doc")}><i className="student-quiz-lesson__icon-file-download fas fa-file-word student-quiz-lesson__icon-file-download__word"></i><span className="student-quiz-lesson__download-txt student-quiz-lesson__icon-file-download__word">Word</span></div>
            </div>
          </p>}
          <div className="student-col-right--quiz-lesson">
            {parseInt(quiz.LevelOfKnowledge) <= 2 ?
                <>
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
                    {quiz.IsExam === "2" ?  <Translate id="student.start-pretest" /> :
                        (quiz.IsExam === "3" ?<Translate id="student.start-posttest" /> :
                            <Translate id="student.start-test" /> )}
                  </a>
                </> : null
            }
          </div>
        </div>

        <div className="student-col-left--quiz-lesson student-quiz-lesson__date" onClick={ () => goTo(`/students/lesson/quiz/${quiz.Id}`)}>{`${startDateTime} - ${endDateTime}`}</div>

        <div className="col-center--quiz-lesson">
          {quiz.IsExam === "3" && <div className="col-with-fix--quiz-lesson"><i className="student-quiz-lesson__icon-waiting fas fa-clock"></i></div>}
          {quiz.IsExam === "2" && <div className="col-with-fix--quiz-lesson"><i className="student-quiz-lesson__icon-finish fas fa-check-circle"></i></div>}
          {/* {quiz.IsExam === "1" && <><span>{quiz.TotalScore}%</span>
            <div className="progress">
              <div className="bar" style={{ width: `${quiz.TotalScore}%` }}></div>
            </div></>} */}
        </div>
      </a>
    </div>
  );
};

const Challenge = ({ lesson, kids, subject, lessonId, setLoading, loading, studentName, studentId, fromLearningGap }) => {


  const [exams, setExams] = useState([]);
  const [examsPrePost, setExamsPrePost] = useState([]);

  useEffect(() => {
     // getExamsByLessonHomeSchooler(lessonId);
      getExamsByLessonSchool(lessonId);

  }, []);

  const getExamsByLessonSchool = async (lessonId) => {
    setLoading(true);
    try {
      const res = await getLessonExams(lessonId);
      const data = res.data.Quizzes;
      let prepostList = [];
      let examsList = [];

      if (data.length > 0) {
        prepostList = data.filter(quiz => quiz.IsExam === "2" || quiz.IsExam === "3");
        examsList = data.filter(quiz => quiz.IsExam === "1");
      }
      setExams(examsList);
      setExamsPrePost(prepostList);
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
            <h3><Translate id="student.select-exercise"/></h3>
            <div className="wrapper-lessons-progress">
              {examsPrePost.length > 0 &&
                examsPrePost.map((quiz, i) => <ChallengeItem key={i} quiz={quiz} lessonId={lessonId} setLoading={setLoading} />)}
            </div>
            <div className="wrapper-lessons-progress" style={{ marginTop: '2rem' }}>
              {exams.length > 0 &&
                exams.map((quiz, i) => <ChallengeItem key={i} quiz={quiz} lessonId={lessonId} setLoading={setLoading} fromLearningGap={fromLearningGap} studentName={studentName} studentId={studentId} />)}
            </div>
            {exams.length === 0 && (
              <EmptyListStyled>
                <Translate id="student.challenge-not-available" />
               </EmptyListStyled>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Challenge;
