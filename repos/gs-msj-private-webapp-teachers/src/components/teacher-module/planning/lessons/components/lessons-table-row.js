import React, { useState, useEffect } from "react";
import LessonsDetail from "./lessons-detail";
import QuizzesDetail from "../../quizzes/components/quizzes-detail";
import EditModal from "../../shared/components/edit-modal";
import DeleteModal from "../../shared/components/delete-modal";
import useDeleteLesson from "../hooks/use-delete-lesson";
import useUpdateLesson from "../hooks/use-update-lesson";
import useDeleteQuiz from "../../quizzes/hooks/useDeleteQuiz";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import SessionManager from "../../../../../custom-classes/session-manager";
import { getPlanLessonDetail } from "../../../../../api/teachers";
import moment from "moment";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import Button from "../../shared/components/button";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChalkboard} from "@fortawesome/free-solid-svg-icons/faChalkboard";
import {Translate} from "react-localize-redux";
import {useHistory} from "react-router-dom";

export default function   LessonsTableRow({ lesson, disabled, headers, onChange }) {
  const { isLoading: isDeleting, deleteLesson } = useDeleteLesson();
  const { isLoading: isDeletingQuiz, deleteQuiz } = useDeleteQuiz();
  const { isLoading: isUpdating, updateLesson } = useUpdateLesson();
  const history = useHistory();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { minDate, maxDate } = getMinAndMaxDates(lesson.implementationDates);
  const [quizzesData, setQuizzesData] = useState([]);
  const [lessonData, setLessonData] = useState(lesson);
  const [countTowardGrade, setCountTowardGrade] = useState(false);
  const [isForGrading, setIsForGrading] = useState(false); // initial value for isForGrading
  const [disabledSaveButton, setDisabledSaveButton] = useState(false);
  const [showErrorDateMessage, setShowErrorDateMessage] = useState(false);
  const [showErrorQuizMessage, setShowErrorQuizMessage] = useState(false);

  useEffect(() => {
    setCountTowardGrade(lesson?.ForGrading === "1" ? true : false);
  }, [lesson]);

  const handleGetDetailsForQuiz = async () => {
    try {
      const response = await getPlanLessonDetail(lesson.Id);
      const data = response.data;

      const exams = data.TeacherPlanLessonQuizModels.map(item => {
        return {
          id: item.Id,
          lessonId: data.TeacherPlanLessonModel.LessonId,
          quizId: item.QuizId,
          name: item.QuizTitle,
          date: item.DueDate ? new Date(moment(item.DueDate, "MM/DD/YYYY HH:mm:ss").format("YYYY/MM/DD")) : "",
          time: item.DueDate ? moment(item.DueDate, "MM/DD/YYYY HH:mm:ss").format("hh:mm A") : "",
          duration: item.Duration,
          timezone: item.DueDateTimezone,
          planLessonId: item.PlanLessonId,
          lessonDueDate: data.TeacherPlanLessonModel.DueDate,
        };
      });
      setQuizzesData(exams);
    } catch (error) {
      console.error("Error fetching quiz details:", error);
      return [];
    }
  }

  useEffect(() => {
    setDisabledSaveButton(false);
    setShowErrorDateMessage(false);
    setShowErrorQuizMessage(false);
    if (!lessonData?.DueDate || !Array.isArray(quizzesData)) return;

    const dueDate = new Date(lessonData.DueDate);

    const hasInvalidQuiz = quizzesData.some((quiz) => {
      if (!quiz.date) return false; // skip if no quiz date

      const quizDate = new Date(quiz.date);
      const diffInTime = quizDate.getTime() - dueDate.getTime();
      const diffInDays = diffInTime / (1000 * 3600 * 24);

      return diffInDays < 1; // Must be at least 1 full day after DueDate
    });

    // Check if all quizzes have required fields completed
    const isQuizCompleted = quizzesData.every(quiz => quiz.date && quiz.time && quiz.duration && quiz.timezone);

    if (hasInvalidQuiz) {
      setShowErrorDateMessage(true);
      setDisabledSaveButton(true);
    }
    if (!isQuizCompleted){
      setShowErrorQuizMessage(true);
      setDisabledSaveButton(true);
    }

  }, [lessonData, quizzesData]);
  
  const handleQuizChange = (changes) => {
    const updatedQuizzes = quizzesData.map((quiz) =>
        quiz.quizId === changes.quizId ? { ...quiz, ...changes } : quiz
    );
    setQuizzesData(updatedQuizzes);
  }

  const handleUpdateLessonWithQuizzes = async () => {
    try {
      await updateLesson({
        ...lessonData,
        PlanningId: planningId,
        TeacherPlanLessonQuizModelList: quizzesData,
      });
      if (!isUpdating) {
          return true;
      }
      return true;
    } catch (error) {
      console.error("Error updating lesson or quiz:", error);
      return false;
    }
  }

  useEffect(() => {
    setIsForGrading(lesson?.ForGrading === "1" ? true : false);
  },[])

  const handleDelete = async () => {
    try {
      await deleteLesson(planningId, lesson.Id);
      if (!isDeleting) {
        if (quizzesData.length === 0) return true;
        await Promise.all(  
        quizzesData.map(async (quiz) => {
          await deleteQuiz(planningId, quiz.quizId);
        })
      );        
      return true;
      }
    } catch (error) {
      console.error("Error deleting lesson:", error);
      return false;
    }
  }

    // Combine error messages if both are active, otherwise show the relevant one
    let customErrorMessage = null;
    if (showErrorDateMessage && showErrorQuizMessage) {
      customErrorMessage = "planning.error-lesson-quiz-dates-and-fields";
    } else if (showErrorDateMessage) {
      customErrorMessage = "planning.error-lesson-quiz-dates";
    } else if (showErrorQuizMessage) {
      customErrorMessage = "planning.error-lesson-quiz-fields"
    }

  const presentLesson  = (() =>{
      console.log("Lesson", lesson);
      history.push(`/students/lesson/${lesson.LessonId}`);
    })

  return (
      <tr>
        <td data-th={headers[0]}>{lesson.LessonName}</td>
        <td data-th={headers[1]}>{formatWithDayName(minDate)}</td>
        <td data-th={headers[2]}>{formatWithDayName(maxDate)}</td>
        <td data-th={headers[3]}> <FormCheckbox
            name=""
            checked={lesson.ForGrading === "1"}
            disabled={true}
        />
        </td>
        <td data-th={headers[4]}>{formatWithDayName(lesson.DueDate)}</td>

        <td className="table-ppal--td-actions">
          <Button variant="secondary"
          onClick={presentLesson} style={{marginBottom: 5}}
          >
            <FontAwesomeIcon icon={faChalkboard}/>{' '}
            <Translate id={"general.present"} /></Button>
          {' '}
          <EditModal
              disabled={disabled}
              editButtonText={"general.viewOrEdit"}
              title={"general.viewOrEdit"}
              confirmButtonText={"confirm-dialog.save"}
              cancelButtonText={"confirm-dialog.button-cancel"}
              onConfirmation={handleUpdateLessonWithQuizzes}
              onCompleted={onChange}
              isLoading={isUpdating}
              isForGrading={isForGrading}
              setCountTowardGrade={setCountTowardGrade}
              handleGetDetailsForQuiz={handleGetDetailsForQuiz}
              customErrorMessage={customErrorMessage}
              disableSaveButton={disabledSaveButton}
          >
            <LessonsDetail
                lesson={lessonData}
                onChange={changes => setLessonData(prev => ({...prev, ...changes}))}
                countTowardGrade={countTowardGrade}
                setCountTowardGrade={setCountTowardGrade}
            />
            <>
              {quizzesData.map((quiz, quizIndex) => (
                  <QuizzesDetail
                      quiz={quiz}
                      key={`q-${quizIndex}`}
                      onChange={handleQuizChange}
                      isForEdit={true}
                  />
              ))}
            </>
          </EditModal>
        </td>
        <td className="table-ppal--td-actions">
          <DeleteModal
              disabled={disabled}
              title="lesson.modal-delete-title"
              description="lesson.modal-delete-description"
              confirmButtonText={"general.modal-delete-confirmation"}
              cancelButtonText={"general.modal-delete-cancel"}
              onConfirmation={handleDelete}
              onCompleted={onChange}
              isLoading={isDeletingQuiz || isDeleting}
          />

        </td>
      </tr>
  );
}
