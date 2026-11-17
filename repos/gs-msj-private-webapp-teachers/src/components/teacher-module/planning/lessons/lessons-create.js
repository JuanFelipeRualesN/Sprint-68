import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import LessonsDetail from "./components/lessons-detail";
import QuizzesDetail from "../quizzes/components/quizzes-detail";
import useCreateLesson from "./hooks/use-create-lesson";
import useGetLessonQuizzes from "../quizzes/hooks/useGetLessonQuizzes";
import useGetPlanningDetails from "../shared/hooks/useGetPlanningDetails";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";
import { SendAccessLog } from "../../../../utils/access_logger";

export default function CreateLesson() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createLesson, isLoading } = useCreateLesson();
  const { quizzes: lessonQuizzes, getLessonQuizzes } = useGetLessonQuizzes();
  const {
    planning,
    getPlanning,
    isLoading: isLoadingPlanning,
  } = useGetPlanningDetails();
  const { planningId, planningName } = SessionManager.getSelectedPlanning();
  const sessionDetails = SessionManager.getSessionDetails();
  const [lesson, setLesson] = useState();
  const [lessonDetailsState, setLessonDetailsState] = useState();
  const [quizzes, setQuizzes] = useState([]);
  const [countTowardGrade, setCountTowardGrade] = useState(false);
  const [quizFieldCompleted, setQuizFieldCompleted] = useState(false);

  const handleCreate = async () => {
    try {
      const lessonCreated = await createLesson({
        ...lesson,
        PlanningId: planningId,
        TeacherPlanLessonQuizModelList: quizzes,
      });
      if (!isLoading) {
        if (lessonCreated) {
          try {
            SendAccessLog({
              UserName: sessionDetails.Username,
              InvitationCode: sessionDetails.InvitationCode,
              Role: sessionDetails.Roles[0],
              AreaId: "14",
              PlanId: planningId ?? null,
              PlanName: planningName ?? null,
              Details: "Created Lesson in planning",
              LessonId: lessonDetailsState?.Id ?? null,
              LessonTitle: lessonDetailsState.LessonTitle ?? null,
              Subject: lessonDetailsState.SubjectCode ?? null,
              Grade: lessonDetailsState.LevelCode ?? null,
            });
          } catch (error) {}
          return true;
        }
      }
    } catch (error) {
      console.error("Error creating lesson:", error);
      return false;
    }
  };

  const handleQuizChange = (changes) => {
    const updatedQuizzes = quizzes.map((quiz) =>
      quiz.quizId === changes.quizId ? { ...quiz, ...changes } : quiz,
    );
    setQuizzes(updatedQuizzes);
    setQuizFieldCompleted(
      updatedQuizzes.every(
        (quiz) => quiz.date && quiz.time && quiz.duration && quiz.timezone,
      ),
    );
  };

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-genial"),
      name: translate("lesson.title"),
      url: "?page=lessons",
    });
    setPath({ name: translate("general.create-new") });
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getPlanning(planningId);
  }, []);

  useEffect(() => {
    if (!lesson || !lesson.LessonId) {
      setQuizzes([]);
      return;
    }
    getLessonQuizzes(lesson.LessonId);
  }, [lesson?.LessonId]);

  useEffect(() => {
    if (!lessonQuizzes || lessonQuizzes.length <= 0) return;

    const updatedQuizzes = lessonQuizzes.map((quiz) => ({
      ...quiz,
      lessonDueDate: lesson.DueDate,
    }));
    setQuizzes(updatedQuizzes);
  }, [lessonQuizzes, lesson?.DueDate]);

  const isValid = () => {
    return Boolean(lesson && lesson.DueDate && lesson.LessonId);
  };

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="general.create-new" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="lesson.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=lessons")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={!isValid() || isLoading || !quizFieldCompleted}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=lessons");
          }}
          isLoading={isLoading}
        />
      </Border>

      <LessonsDetail
        onChange={setLesson}
        countTowardGrade={countTowardGrade}
        setCountTowardGrade={setCountTowardGrade}
        setQuizzes={setQuizzes}
        setLessonDetailsState={setLessonDetailsState}
      />
      <>
        <Heading as="h5" className="mt-4">
          <Translate id="quiz.select-lesson-message" />
        </Heading>

        {quizzes.map((quiz, index) => {
          return (
            <QuizzesDetail
              quiz={quiz}
              key={`quiz-${quiz.quizId}-${quiz.date}`}
              onChange={handleQuizChange}
            />
          );
        })}
      </>
    </>
  );
}
