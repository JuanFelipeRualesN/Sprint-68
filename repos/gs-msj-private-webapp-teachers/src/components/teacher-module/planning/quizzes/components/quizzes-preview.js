import React, { useEffect } from "react";
import Accordion from "../../shared/components/accordion";
import useGetQuizPreview from "../hooks/useGetQuizPreview";
import { Translate } from "react-localize-redux";
import { Provider } from 'react-redux';
import { LocalizeProvider } from 'react-localize-redux';
import store from "../../../../../redux/store";
import { getLessonQuizzesDetails} from "../../../../../api/teachers"
import LearningGapLessonView, { QuizzesDropdown } from "../../../../../components/teacher-module/learning-gap/learning-gap-lessons/learning-gap-lesson-view/learning-gap-lesson-view";

export default function QuizzesPreview({ quizId, lessonId }) {


  const lessonDetailAPI=[
    {
        accordionId: "11",
        titleId: "learning-gap.exams",
        state: "exams",
        apiMethod: getLessonQuizzesDetails,
        examType: "1",
        typeDispatch: "FETCH_EXAMS_DETAIL",
        render: quiz => <QuizzesDropdown quizzes={quiz}/>
    },
  ]

  return (
    <>
      <Accordion
        id={`quiz-preview-${quizId}`}
        title={<Translate id="quiz.preview" />}
      >
      <Provider store={store}>
          <LocalizeProvider store={store}>
              {typeof lessonId !== undefined && lessonId !== null && lessonId !== '' ?
                      <LearningGapLessonView lessonId={lessonId} lessonDetailsAPI={lessonDetailAPI} /> : null}
          </LocalizeProvider>
      </Provider>
      </Accordion>
    </>
  );
}
