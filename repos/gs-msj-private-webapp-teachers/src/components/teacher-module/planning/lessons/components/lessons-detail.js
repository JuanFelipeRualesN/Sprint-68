import React, { useEffect, useState } from "react";
import useGetAvailableSubjectsAndGrades from "../../shared/hooks/useGetAvailableSubjectsAndGrades";
import useGetLessons from "../hooks/useGetLessons";
import useGetLessonDetails from "../hooks/useGetLessonDetails";
import moment from "moment";
import FormDatePicker from "../../shared/components/form/form-date-picker";
import FormSelect from "../../shared/components/form/form-select";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import Accordion from "../../shared/components/accordion";
import DateSelections from "../../shared/components/date-selections";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";
import ModalViewLesson from "../../../modal/modal-view-lesson";
import LanguageManager from "../../../../../custom-classes/language-manager";
import { SendAccessLog } from "../../../../../utils/access_logger";

export default function LessonsDetail({
  lesson: lessonForEdit,
  onChange = (model) => {},
  countTowardGrade = false,
  setCountTowardGrade = (isCheked) => {},
  setQuizzes = () => {},
  setLessonDetailsState = () => {},
}) {
  const planning = SessionManager.getSelectedPlanning();
  const [selectedLesson, setSelectedLesson] = useState();
  const [selectedSubjectAndGrade, setSelectedSubjectAndGrade] = useState();
  const [
    createAttachPlanningLessonModel,
    setCreateAttachPlanningLessonModel,
  ] = useState({
    PlanningId: planning.planningId,
    ForGrading: "0",
    TriesQuantity: "1",
    PrePostRequired: "0",
  });

  const {
    availableSubjectsAndGrades,
    getAvailableSubjectsAndGrades,
  } = useGetAvailableSubjectsAndGrades();
  const { lessons, getLessons } = useGetLessons();
  const { lessonDetail, getLessonHtmlDetail } = useGetLessonDetails();
  const [showModalViewLesson, setShowModalViewLesson] = useState(false);
  const [selectedDueDate, setSelectedDueDate] = useState();
  const sessionDetails = SessionManager.getSessionDetails();
  const { planningId, planningName } = SessionManager.getSelectedPlanning();

  const handleChange = (name, value) => {
    let newState = { ...createAttachPlanningLessonModel, [name]: value };

    if (name === "TeacherPlanLessonDateModelList") {
      newState[name] = value.map((item) => ({
        ...item,
        ImplementationDate: item.Date,
      }));
    }
    setCreateAttachPlanningLessonModel(newState);
    onChange(newState);
  };

  const handleChangeSubjectAndGrade = (value) => {
    setSelectedLesson(null);
    handleChange("LessonId", null);
    const subjectAndGrade = availableSubjectsAndGrades.find(
      (subjectAndGrade) => subjectAndGrade.ProductId === value,
    );
    if (subjectAndGrade) {
      setSelectedSubjectAndGrade(subjectAndGrade);
      getLessons(subjectAndGrade.Subject, subjectAndGrade.Level);
    }
  };

  const handleChangeLesson = (value) => {
    if (value) {
      const lesson = lessons.find((lesson) => lesson.Id === value);
      setLessonDetailsState(lesson);
      setSelectedLesson(lesson);
      setQuizzes([]);
      handleChange("LessonId", lesson?.Id);
    }
    if (!selectedLesson) {
      setShowModalViewLesson(false);
    }
  };

  const handleDate = (name, value) => {
    if (name === "TeacherPlanLessonDateModelList") {
      handleChange(
        name,
        value.map((x) => ({
          Date: moment(x).format("YYYY-MM-DD"),
        })),
      );
    } else {
      handleChange(name, `${moment(value).format("YYYY-MM-DD")} 00:00:00`);
      // If name is DueDate, set the selectedDueDate state
      if (name === "DueDate") {
        setSelectedDueDate(`${moment(value).format("YYYY-MM-DD")} 00:00:00`);
      }
    }
  };

  const toggleModalViewLesson = () => {
    setShowModalViewLesson(!showModalViewLesson);
  };

  const handleCheckChanges = (value) => {
    setCountTowardGrade(value);
    handleChange("ForGrading", value ? "1" : "0");
  };

  useEffect(() => {
    getAvailableSubjectsAndGrades();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getAvailableSubjectsAndGrades();
  }, []);

  useEffect(() => {
    if (lessonForEdit) {
      setCreateAttachPlanningLessonModel({
        DueDate: `${moment(lessonForEdit?.DueDate).format(
          "YYYY-MM-DD",
        )} 00:00:00`,
        PlanningId: planning.planningId,
        ForGrading: lessonForEdit.ForGrading,
        TriesQuantity: "1",
        Id: lessonForEdit.Id,
        PrePostRequired: lessonForEdit.PrePostRequired,
        LessonId: lessonForEdit.LessonId,
        TeacherPlanLessonDateModelList:
          lessonForEdit.TeacherPlanLessonDateModelList,
        TeacherPlanLessonDateModels: lessonForEdit.TeacherPlanLessonDateModels,
      });
    }
  }, [lessonForEdit]);

  useEffect(() => {
    if (showModalViewLesson) {
      try {
        SendAccessLog({
          UserName: sessionDetails.Username,
          InvitationCode: sessionDetails.InvitationCode,
          Role: sessionDetails.Roles[0],
          AreaId: "14",
          PlanId: planningId ?? null,
          PlanName: planningName ?? null,
          Details: "Previewed Lesson in planning module",
          LessonId: selectedLesson ? selectedLesson?.Id : lessonForEdit?.LessonId ?? null,
          LessonTitle: selectedLesson ? selectedLesson.LessonTitle : lessonForEdit?.LessonName ?? null,
          Subject: selectedLesson ? selectedLesson.SubjectCode : lessonForEdit?.SubjectCode ?? null,
          Grade: selectedLesson ? selectedLesson.LevelCode : lessonForEdit?.LevelCode ?? null,
        });
      } catch (error) {
        // Silently catch errors to avoid disrupting the user experience
      }
    }
  }, [showModalViewLesson]);

  return (
    <>
      <div className="wrapper-halfCol">
        <div className="contentForms">
          <DateSelections
            fromDate={moment(planning?.openDate).toDate()}
            toDate={moment(planning?.closeDate).toDate()}
            onChange={(values) =>
              handleDate("TeacherPlanLessonDateModelList", values)
            }
            values={lessonForEdit?.implementationDates?.map((x) =>
              moment(x).toDate(),
            )}
            labelClassName="forms-label"
          />
          <FormDatePicker
            name="DueDate"
            label={<Translate id="lesson.select-starting-date" />}
            onChange={(values) => handleDate("DueDate", values)}
            value={
              lessonForEdit?.DueDate
                ? moment(lessonForEdit?.DueDate).toDate()
                : selectedDueDate
            }
            minDate={moment(planning?.openDate).toDate()}
            maxDate={moment(planning?.closeDate).toDate()}
            toggleModalView={toggleModalViewLesson}
            showPreviewButton={lessonForEdit}
            labelClassName="forms-label"
          />
        </div>
      </div>
      <div className="wrapperFlex">
        <div className="wrapperFlex contentSize-md contentForms-inline">
          {!lessonForEdit && (
            <div className="col pl-0 col-sm">
              <FormSelect
                name="grade"
                label={<Translate id="planning.subject-grade" />}
                placeholder={translate("general.select")}
                options={availableSubjectsAndGrades.map((subjectsAndGrade) => ({
                  value: subjectsAndGrade.ProductId,
                  label: `${subjectsAndGrade.SubjectName} ${subjectsAndGrade.Level}`,
                }))}
                onChange={handleChangeSubjectAndGrade}
                searchable
              />
            </div>
          )}
          {(selectedSubjectAndGrade || lessonForEdit) && (
            <div className="col pl-0 col-sm">
              {selectedSubjectAndGrade && (
                <FormSelect
                  name="lesson"
                  label={<Translate id="lesson.select-lesson" />}
                  placeholder={translate("lesson.select-lesson")}
                  options={lessons.map((lesson) => ({
                    value: lesson.Id,
                    label: lesson.LessonTitle,
                  }))}
                  onChange={handleChangeLesson}
                  value={createAttachPlanningLessonModel.LessonId}
                  searchable
                  disabledPreviewIcon={
                    !createAttachPlanningLessonModel.LessonId
                  }
                  toggleModalView={toggleModalViewLesson}
                  showPreviewButton={true}
                  className="align-vertical-down"
                />
              )}
              <FormCheckbox
                name="isRequiredPreAndPostTest"
                label={<Translate id="lesson.require-pre-post-test" />}
                onChange={(value) =>
                  handleChange("PrePostRequired", value === true ? 1 : 0)
                }
                checked={
                  createAttachPlanningLessonModel.PrePostRequired === "1"
                }
                value={
                  createAttachPlanningLessonModel.PrePostRequired === "1"
                    ? "true"
                    : "false"
                }
              />
              <FormCheckbox
                className="count-for-grade"
                name={`${lessons.Id}-countForGradeMessage`}
                label={<Translate id="quiz.count-for-grade-message" />}
                checked={countTowardGrade}
                onChange={handleCheckChanges}
              />
              <br />
            </div>
          )}
        </div>
        {showModalViewLesson ? (
          <ModalViewLesson
            visible={showModalViewLesson}
            lessonId={createAttachPlanningLessonModel.LessonId}
            updateParent={toggleModalViewLesson}
          />
        ) : null}
      </div>
    </>
  );
}
