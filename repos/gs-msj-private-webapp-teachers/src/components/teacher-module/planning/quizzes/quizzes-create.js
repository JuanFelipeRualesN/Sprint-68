import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import QuizzesDetail from "./components/quizzes-detail";
import useCreateQuiz from "./hooks/useCreateQuiz";
import useGetLessonQuizzes from "./hooks/useGetLessonQuizzes";
import useGetPlanningDetails from "../shared/hooks/useGetPlanningDetails";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import "../shared/styles/quizzes.css";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import SessionManager from "../../../../custom-classes/session-manager";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import FormSelect from "../shared/components/form/form-select";

export default function QuizzesCreate() {
  const navigate = useHistory();
  const { setPath } = useContext(BreadcrumbContext);
  const { planningId } = SessionManager.getSelectedPlanning();
  const { createQuiz, isLoading } = useCreateQuiz();
  const { planning, getPlanning } = useGetPlanningDetails();
  const { quizzes: lessonQuizzes, getLessonQuizzes } = useGetLessonQuizzes();
  const [currentLesson, setCurrentLesson] = useState("");
  const [quizzes, setQuizzes] = useState([]);

  const getCurrentLesson = () => {
    return planning?.lessons?.find((l) => l.lessonId === currentLesson);
  };

  const handleCreate = () => {
    const lesson = getCurrentLesson();
    return createQuiz({
      planningId,
      lessonId: lesson.lessonId,
      dueDate: lesson.dueDate,
      quizzes,
    });
  };

  const handleQuizChange = (quizId, changes) => {
    const updatedQuizzes = quizzes.map((quiz) =>
      quiz.quizId === quizId ? { ...quiz, ...changes } : quiz
    );
    setQuizzes(updatedQuizzes);
  };

  useEffect(() => {
    setPath({ name: translate("general.create-new") });
    getPlanning(planningId);
  }, []);

  useEffect(() => {
    if (!lessonQuizzes || lessonQuizzes.length <= 0) return;

    const lesson = getCurrentLesson();
    const updatedQuizzes = lessonQuizzes.map((quiz) => ({
      ...quiz,
      planLessonId: lesson.id,
      lessonDueDate: lesson.dueDate,
    }));

    setQuizzes(updatedQuizzes);
  }, [lessonQuizzes]);

  const isActive = !quizzes.some(q => q.isActive === true);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="general.create-new" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="quiz.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=quizzes")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=quizzes");
          }}
          isLoading={isLoading}
          // disabled={isActive}
        />
      </Border>

      <div className="wrapperFlex FlexAlingEnd">
        <div className="col mt-4 pl-0">
          <Heading as="h3">
            <Translate id="quiz.header-select-lesson" />
          </Heading>
          <FormSelect
            className="m-0 w-25"
            name="lesson"
            label=""
            placeholder={translate("general.select")}
            options={planning?.lessons?.map((lesson) => ({
              label: lesson.title,
              value: lesson.lessonId,
            }))}
            onChange={(value) => {
              getLessonQuizzes(value);
              setCurrentLesson(value);
            }}
            searchable
          />
        </div>
      </div>

      {currentLesson && quizzes && (
        <>
          <Heading as="h5" className="mt-4">
            <Translate id="quiz.select-lesson-message" />
          </Heading>

          {quizzes.map((quiz, index) => {
            return (
              <QuizzesDetail
                quiz={quiz}
                key={`quizzes-${index}`}
                onChange={(changes) =>
                  handleQuizChange(changes.quizId, changes)
                }
              />
            );
          })}
        </>
      )}
    </>
  );
}
