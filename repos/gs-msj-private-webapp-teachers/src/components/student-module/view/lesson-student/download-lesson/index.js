import React, { useState } from "react";
import "./style.css";
import { getMaterialOffline } from "../../../../../api/athenas";
import LessonTitle from "../lesson-title";
import OfflineIcon from "../../../../../assets/images/common-icons/Lesson-icon-Offline.svg";
import {Translate} from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faFilePdf} from "@fortawesome/free-solid-svg-icons";
import { getAsset } from "../../../../../utils/getAssets";

const IconAvatarOffline = getAsset("Lesson-icon-AvatarOffline");

const DownloadLesson = ({
  lesson,
  kids,
  subject,
  lessonId,
  setLoading,
  loading,
}) => {

  const [materials, setMaterials] = useState(null);

  const getMaterialOfflineByLesson = async (format) => {
    setLoading(true);
    try {
      const res = await getMaterialOffline(lessonId, -1, format);
      /** Modified by: Pedro Ortiz on 01/20/2021
       * The endpoint now just return the URL of the PDF (before was res.data.Location)
       * **/
      const data = res.data;
      if (data) {
        window.open(data, "_blank");
      }
      setMaterials(data);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  let className = "student-wrapper-iframe";
  if (kids) {
    className = "student-wrapper-iframe-kids";
  }
  /** Modified by: Pedro Ortiz on 01/20/2021
   * before materials was an array, now is a simple string
   * **/
  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <div className={className}>
            {lesson && <LessonTitle subject={subject} lesson={lesson} />}
            <div className="student-wrapper-student-list-w-icon">
              <div className="student-list-w-icon">
                <div className="student-list-w-icon__right">
                  <img
                    src={OfflineIcon}
                    alt="icon"
                  />
                </div>
                <div className="student-list-w-icon__left">
                  <p className="student-list-w-icon__txt-ppal">
                    <Translate id="student.title-download-lesson"
                               options={{renderInnerHtml: true}}/>
                  </p>
                  <p>
                    <Translate id="student.subtitle-download-lesson"
                               options={{renderInnerHtml: true}}/>
                    <div className="student-quiz-lesson__download--btn">
                      <div style={{cursor: 'pointer'}} onClick={() => getMaterialOfflineByLesson("pdf")}>
                        <FontAwesomeIcon icon={faFilePdf} className="student-quiz-lesson__icon-file-download__pdf" />{""}
                        <span className="student-quiz-lesson__download-txt student-quiz-lesson__icon-file-download__pdf">PDF</span>
                      </div>
                      {/* <div style={{cursor: 'pointer'}} onClick={() => getMaterialOfflineByLesson("doc")}><i className="student-quiz-lesson__icon-file-download fas fa-file-word student-quiz-lesson__icon-file-download__word"></i><span className="student-quiz-lesson__download-txt student-quiz-lesson__icon-file-download__word">Word</span></div> */}
                    </div>
                  </p>
                </div>
              </div>

              <div className="student-bg-list-offline">
                {" "}
                <img
                  src={IconAvatarOffline}
                />{" "}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default DownloadLesson;
