/**
 * Created by portiz on 05/07/20.
 */
import React, { useState, useEffect } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Button, Input } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import DataTable from '../../shared-module-components/datatables/datatable';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import ModalPlanningItemArchive from "../modal/modal-planning-item-archive";
import ModalPlanningItemEdit from "../modal/modal-planning-item-edit";
import ModalViewLesson from "../modal/modal-view-lesson";
import ModalExamPreview from "../modal/modal-exam-preview";
import {
    faTrash,
    faEye, faChalkboardTeacher, faEdit, faFlag,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { getTimeZone } from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";
import { withRouter } from 'react-router-dom';
/**
 * Data table to handle the planning details
 */
const PlanningDetailAttachedItemsDataTable = ({data: initialData, planningId, planningName, lessonOrQuiz, disabled = false, updateParent, updateLesson, updateQuizz}) => {
  const [data, setData] = useState(initialData ?? []);
  const [lessonId, setLessonId] = useState(null);
  const [planningItemId, setPlanningItemId] = useState(null);
  const [showModalArchive, setShowModalArchive] = useState(false);
  const [showModalViewLesson, setShowModalViewLesson] = useState(false);
  const [showModalExamPreview, setShowModalExamPreview] = useState(false);
  const [showModalEdit, setShowModalEdit] = useState(false);
  const [quizId, setQuizId] = useState(null);
  const [editQuizState, setEditQuizState] = useState({
      lessonOrQuizId: 0,
      dueDate: new Date(),
      forGrading: false,
      triesQuantity: 0,
      id: 0
  });
  const headerOptions = [
      { key: "4", name: "" },
      { key: "0", name: LanguageManager.getTranslation("planning.title", Settings.LANGUAGES) },
      { key: "1", name: LanguageManager.getTranslation("planning.due-date", Settings.LANGUAGES) },
      { key: "2", name: "" },
      { key: "3", name: "" },
      { key: "5", name: LanguageManager.getTranslation("planning.for-grading", Settings.LANGUAGES) },
      { key: "6", name: "" },
      { key: "7", name: "" }
  ]
  const history = useHistory();

  useEffect(() => {
      convertDataToDataTable(initialData);
    }, [initialData]);

  const handleViewClick = (newValue) => {
    if (lessonOrQuiz === "LESSON") {
      setLessonId(newValue);
      setShowModalViewLesson(true);
    } else if (lessonOrQuiz === "QUIZ") {
      setShowModalExamPreview(true);
      setQuizId(newValue);
    }
  };

  const handleEditClick = (newValue) => {
    setShowModalEdit(true);
    setEditQuizState({
      lessonOrQuizId: newValue.ItemId,
      dueDate: newValue.DueDate,
      forGrading: newValue.ForGrading,
      triesQuantity: newValue.TriesQuantity,
      id: newValue.Id
    });
  };

  const handleArchiveClick = (newValue) => {
    setShowModalArchive(true);
    setPlanningItemId(newValue);
  };

  const handleStudentPresentation = (newValue) => {
    SessionManager.setSelectedLessonToShow(newValue, planningId, planningName);
    goToSiteStudentsDetailLesson(newValue);
  };

  const goToSiteStudentsDetailLesson = (lessonId) => {
    history.push(`/students/lesson/${lessonId}`);
  };

  const toggleModalArchive = () => {
    setShowModalArchive(false);
    updateParent(true);
  };

  const handleOnChangeForGrading = (event) => {
    const forGrading = event.currentTarget.checked;
    const QuizzOrLessonId = event.currentTarget.value;
    if (lessonOrQuiz === "LESSON") {
      updateLesson(QuizzOrLessonId, forGrading);
    } else {
      updateQuizz(QuizzOrLessonId, forGrading);
    }
  };

  const toggleModalPreview = () => {
    setShowModalExamPreview(false);
  };

  const toggleModalViewLesson = () => {
    setShowModalViewLesson(false);
  };

    // const postCrossDomainMessage = (lessonId) => {
  //   const sessionDetails = SessionManager.getSessionDetails();
  //   let iframe = document.createElement('iframe');
  //   iframe.src = Settings.URL_SITE_STUDENTS;
  //   iframe.loading = 'lazy';
  //   iframe.style = 'visibility: hidden;';
  //   document.body.appendChild(iframe);
  //   iframe.onload = () => {
  //     let win = iframe.contentWindow;
  //     win.postMessage({ type: 'login', body: sessionDetails }, Settings.URL_SITE_STUDENTS);
  //     const url = `${Settings.URL_SITE_STUDENTS}/students/lesson/${lessonId}`;
  //     window.location.href = url;
  //   }
  // };

    const convertDataToDataTable = (dataToConvert) => {
    if (!dataToConvert?.length) {
        setData([]);
        return;
    }
  
    const newArray = dataToConvert.map(lesson => ({
      isGapClosing: lesson.isGapClosing === '1' ? 
      <FontAwesomeIcon icon={faFlag} style={{color: "#4ca7a2"}} /> : null,
      Title: lesson.Title,
      DueDate: lesson.DueDate ? 
      moment.utc(lesson.DueDate, "MM/DD/YYYY HH:mm:ss")
          .tz(getTimeZone())
          .format("MM/DD/YYYY hh:mm a") : "",
      Presentation: lessonOrQuiz === "LESSON" ? (
      <Button 
          className="btn btn-success" 
          onClick={() => handleStudentPresentation(lesson.ItemId)} 
          value={lesson.ItemId}
      >
          <FontAwesomeIcon icon={faChalkboardTeacher} />{' '}
          <Translate id="planning.presentation" options={{ renderInnerHtml: true }} />
      </Button>
      ) : null,
      View: (
      <button 
          className="btn-default button-secondary-custom"
          onClick={() => handleViewClick(lesson.ItemId)}
      >
          <FontAwesomeIcon icon={faEye} />{' '}
          <Translate id="planning.view-only" options={{ renderInnerHtml: true }} />
      </button>
      ),
      ForGrading: (
      <Input 
          type="checkbox" 
          checked={lesson.ForGrading === "1"} 
          value={lesson.Id} 
          onChange={handleOnChangeForGrading}
      />
      ),
      Edit: (
      <Button 
          className="btn only-icon" 
          onClick={() => handleEditClick(lesson)} 
          value={lesson.Id} 
          name={lesson.ItemId} 
          disabled={disabled}
      >
          <FontAwesomeIcon icon={faEdit} />
          {/* <Translate id="planning.edit" options={{renderInnerHtml: true}}/> */}
      </Button>
      ),
      Available: (
      <Button 
          className="btn btn-danger" 
          onClick={() => handleArchiveClick(lesson.Id)} 
          value={lesson.Id} 
          disabled={disabled}
      >
          <FontAwesomeIcon icon={faTrash} />
          {/* <Translate id="planning.archive" options={{renderInnerHtml: true}}/> */}
      </Button>
      ),
      value: lesson.Id,
    }));
    setData(newArray);
  };

  return (
    <div>
      <DataTable headerOptions={headerOptions} data={data} />
      {showModalArchive && (
        <ModalPlanningItemArchive
          lessonOrQuiz={lessonOrQuiz}
          planningItemId={planningItemId}
          planningId={planningId}
          visible={showModalArchive}
          toggle={toggleModalArchive}
          updateParent={toggleModalArchive}
        />
      )}
      {showModalEdit && (
        <ModalPlanningItemEdit
          lessonOrQuiz={lessonOrQuiz}
          lessonOrQuizId={planningItemId}
          dueDate={editQuizState.dueDate}
          id={editQuizState.id}
          forGrading={editQuizState.forGrading}
          planningId={planningId}
          visible={showModalEdit}
          toggle={toggleModalArchive}
          updateParent={toggleModalArchive}
        />
      )}
      {showModalViewLesson && (
        <ModalViewLesson 
          visible={showModalViewLesson} 
          lessonId={lessonId} 
          updateParent={toggleModalViewLesson} 
        />
      )}
      {showModalExamPreview && (
        <ModalExamPreview
          quizId={quizId}
          visible={showModalExamPreview}
          preview={true}
          toggle={toggleModalPreview}
          updateParent={toggleModalPreview}
        />
      )}
    </div>
  );
};

export default withRouter(PlanningDetailAttachedItemsDataTable);
