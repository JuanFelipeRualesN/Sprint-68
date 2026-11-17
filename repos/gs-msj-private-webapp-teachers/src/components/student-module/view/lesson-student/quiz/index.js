import React, { useEffect, useState } from "react";
import "./style.css";
import { getQuizStudent } from "../../../../../api/athenas";
import { useParams, useLocation} from "react-router-dom";
import { Loader } from "../common/styled-components/loader";
import QuizMenu from "./quiz-menu";
import Pairing from "./pairing";
import Selection from "./selection";
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from "react-dom/server";
import useVersionedTranslations from "../../../../../hooks/useVersionedTranslations";

const QuizStudent = () => {
  const { quizId } = useParams();
  const location = useLocation();

  const [quiz, setQuiz] = useState(null);
  const [loading, setLoading] = useState(false);

  // Extract query parameters
  const query = new URLSearchParams(location.search);
  const fromLearningGap = query.get("fromLearningGap");
  const studentId = query.get("studentId");
  const studentName = query.get("studentName");

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    getQuiz(quizId);
  }, []);

  const getQuiz = async (quizId, response) => {
    setLoading(true);
    try {
      const res = await getQuizStudent(quizId, response);
      const data = res.data.QuizRequestModel;
      setQuiz(data);
    } finally {
      setLoading(false);
    }
  };

  const getResponseQuiz = (isExam) => {
    if (isExam === "0") {
      const response = 1;
      getQuiz(quizId, response);
    }
  };

  return (
    <>
      <QuizMenu
          quiz={quiz?.QuizModel}
          fromLearningGap={fromLearningGap}
          studentId={studentId}
          studentName={studentName}
      />
      {quiz && quiz.QuizModel.QuizTypeId === "1" && <Selection quiz={quiz} subject={quiz.SubjectCode} level={quiz.LevelCode}
                                                               getResponseQuiz={getResponseQuiz} fromLearningGap={fromLearningGap}
      studentId={studentId} studentName={studentName}/>}
      {quiz && quiz.QuizModel.QuizTypeId === "2" && <Pairing quiz={quiz}
                                                             subject={quiz.SubjectCode} level={quiz.LevelCode}
                                                             fromLearningGap={fromLearningGap}
                                                                studentId={studentId} studentName={studentName}

      />}
      {loading && <Loader />}
    </>
  );
};

export default withLocalize(QuizStudent);
