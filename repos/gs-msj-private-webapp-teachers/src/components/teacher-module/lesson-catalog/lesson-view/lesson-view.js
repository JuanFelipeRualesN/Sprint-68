import React from 'react'
import {Translate, withLocalize} from "react-localize-redux";
import DefaultImage from '../../../../assets/images/common-icons/Catalog-icon-Default.jpg'
import { Visibility, NoteAdd, SaveAlt, Lock} from "@material-ui/icons";
import { Tooltip } from '@material-ui/core';
import { getLessonPDF } from '../../../../api/teachers';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faTimes, faTable   } from '@fortawesome/free-solid-svg-icons';
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";
import { SendAccessLog } from '../../../../utils/access_logger';
import SessionManager from '../../../../custom-classes/session-manager';


// Component to display the lesson card
const LessonView = ({
  lesson,
  lessonImage,
  lessonTitle,
  lessonCode,
  handleOpenLessonDetailsModal,
  setLoading,
  getPlannings,
  handleAssignAvanzaLesson,
  isAvailable,
  availableAssignAvanza,
  availableAssignPlanning,
}) => {
  const sessionDetails = SessionManager.getSessionDetails();

  // Check if is Avanza lesson (N or C level)
  const isAvanzaLesson = /^[cn][1-4]$/i.test(lesson.LessonModel.LevelCode);

  // Initialize translations based on the current version
  useVersionedTranslations();

  // Download lesson PDF
  const printLesson = (isAvailable) => {
    if (!isAvailable) return;

    setLoading(true);
    getLessonPDF(lesson.LessonModel.Id).then((response) => {
      setLoading(false);
      try {
        SendAccessLog({
          UserName: sessionDetails.Username,
          InvitationCode: sessionDetails.InvitationCode,
          Role: sessionDetails.Roles[0],
          AreaId: "14",
          Details: "Downloaded lesson PDF in catalog",
          LessonId: lesson.LessonModel.Id ?? null,
          LessonTitle: lesson.LessonModel.LessonTitle ?? null,
          Subject: lesson.LessonModel.SubjectCode ?? null,
          Grade: lesson.LessonModel.LevelCode ?? null,
        });
      } catch (error) {}
      window.open(response.data);
    });
  };

  const lessonTooltipText = (
    <div style={{ padding: "6px", fontSize: "12px" }}>
      <Translate
        id="lesson-catalog.unavailable-course"
        options={{ renderInnerHtml: false }}
      />
    </div>
  );

  const phaseTooltipText = (
    <div style={{ padding: "6px", fontSize: "12px" }}>
      <Translate
        id="lesson-catalog.unavailable-course-phase"
        options={{ renderInnerHtml: false }}
      />
    </div>
  );

  return (
    <div className="lesson-card">
      <img
        src={lessonImage != null ? lessonImage : DefaultImage}
        alt="Lesson Image"
        style={{ width: "100%", height: "150px", objectFit: "cover" }}
      />
      <div>
        <div className="lesson-title">{lessonTitle}</div>
      </div>
      <div>
        {lessonCode && (
          <div style={{ display: "flex", flexDirection: "row", gap: "10px" }}>
            <div
              style={{ fontSize: "12px", fontWeight: 600, marginBottom: "7px" }}
            >
              <Translate
                id="lesson-catalog.code"
                options={{ renderInnerHtml: false }}
              />
              :
            </div>
            <div
              style={{ fontSize: "12px", fontWeight: 600, color: "#A9A9A9" }}
            >
              {lessonCode}
            </div>
          </div>
        )}
        {isAvailable ? (
          <div
            className="lesson-card-actions"
            onClick={() => handleOpenLessonDetailsModal(lesson)}
          >
            <Visibility
              fontSize="small"
              style={{ marginRight: "5px", width: "15px" }}
            />
            <span>
              <Translate
                id="lesson-catalog.see-details"
                options={{ renderInnerHtml: false }}
              />
            </span>
          </div>
        ) : (
          <Tooltip title={lessonTooltipText} placement="top" arrow>
            <div
              className="lesson-card-actions"
              style={{
                cursor: "not-allowed",
                color: "#A9A9A9",
                opacity: 0.7,
              }}
            >
              <Lock
                fontSize="small"
                style={{ marginRight: "5px", width: "15px", color: "#616161" }}
              />
              <Translate
                id="lesson-catalog.see-details"
                options={{ renderInnerHtml: false }}
              />
            </div>
          </Tooltip>
        )}
        {isAvailable ? (
          <div
            className="lesson-card-actions"
            onClick={() => {
              printLesson(isAvailable);
            }}
          >
            <SaveAlt
              fontSize="small"
              style={{ marginRight: "5px", width: "15px" }}
            />
            <Translate
              id="lesson-catalog.download-pdf"
              options={{ renderInnerHtml: false }}
            />
          </div>
        ) : (
          <Tooltip title={lessonTooltipText} placement="top" arrow>
            <div
              className="lesson-card-actions"
              style={{
                cursor: "not-allowed",
                color: "#A9A9A9",
                opacity: 0.7,
              }}
            >
              <Lock
                fontSize="small"
                style={{ marginRight: "5px", width: "15px", color: "#616161" }}
              />
              <Translate
                id="lesson-catalog.download-pdf"
                options={{ renderInnerHtml: false }}
              />
            </div>
          </Tooltip>
        )}
        {availableAssignPlanning ? (
          <>
            {isAvailable ? (
              <div className="lesson-card-actions" onClick={getPlannings}>
                <NoteAdd
                  fontSize="small"
                  style={{ marginRight: "5px", width: "15px" }}
                />
                <Translate
                  id="lesson-catalog.assign-to-plan"
                  options={{ renderInnerHtml: false }}
                />
              </div>
            ) : (
              <Tooltip title={lessonTooltipText} placement="top" arrow>
                <div
                  className="lesson-card-actions"
                  style={{
                    cursor: "not-allowed",
                    color: "#A9A9A9",
                    opacity: 0.7,
                  }}
                >
                  <Lock
                    fontSize="small"
                    style={{
                      marginRight: "5px",
                      width: "15px",
                      color: "#616161",
                    }}
                  />
                  <Translate
                    id="lesson-catalog.assign-to-plan"
                    options={{ renderInnerHtml: false }}
                  />
                </div>
              </Tooltip>
            )}
          </>
        ) : null}
        {availableAssignPlanning ? (
          <>
            {isAvanzaLesson ? (
              isAvailable ? (
                <div
                  onClick={handleAssignAvanzaLesson}
                  className="lesson-card-actions"
                >
                  <NoteAdd
                    fontSize="small"
                    style={{ marginRight: "5px", width: "15px" }}
                  />
                  <Translate
                    id="lesson-catalog.assign-to-avanza"
                    options={{ renderInnerHtml: false }}
                  />
                </div>
              ) : (
                <Tooltip title={phaseTooltipText} placement="top" arrow>
                  <div
                    className="lesson-card-actions"
                    style={{
                      cursor: "not-allowed",
                      color: "#A9A9A9",
                      opacity: 0.7,
                    }}
                  >
                    <Lock
                      fontSize="small"
                      style={{
                        marginRight: "5px",
                        width: "15px",
                        color: "#616161",
                      }}
                    />
                    <Translate
                      id="lesson-catalog.assign-to-avanza"
                      options={{ renderInnerHtml: false }}
                    />
                  </div>
                </Tooltip>
              )
            ) : null}{" "}
          </>
        ) : null}
        <div
          className="lesson-card-actions"
          style={{ marginTop: "10px", pointerEvents: "none" }}
        >
          <FontAwesomeIcon
            icon={faTable}
            style={{ marginRight: "5px", width: "15px" }}
          />
          <span style={{ marginRight: "10px", alignItems: "center" }}>
            Blueprint
          </span>
          <FontAwesomeIcon
            icon={
              lesson?.LessonModel?.Blueprint === null ||
              lesson?.LessonModel?.Blueprint == 0
                ? faTimes
                : faCheck
            }
            style={{
              color:
                lesson?.LessonModel?.Blueprint === null ||
                lesson?.LessonModel?.Blueprint == 0
                  ? "red"
                  : "green",
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default withLocalize(LessonView);