import React, { useState, useEffect } from "react";
import "./style.css";
import LessonTitle from "../lesson-title";
import FontSizeChanger from "react-font-size-changer";
import { getLessonByIdConcept } from "../../../../../api/gsweb";
import WrapperOverlappingMedia from "../../../../teacher-module/wrapper-overlapping-media";

const Concept = ({ lesson, kids, subject, lessonId, setLoading, loading }) => {
  const [concept, setConcept] = useState(null);

  useEffect(() => {
    getConceptByLesson();
  }, []);

  const getConceptByLesson = async () => {
    setLoading(true);
    try {
      const res = await getLessonByIdConcept(lessonId);
      const data = res.data;
      setConcept(data);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const removeDonwloadControlFromVideos = (htmlContent) => {
    if (htmlContent.includes("<video")) {
      htmlContent = htmlContent.replace("<video", "<video controlsList=\"nodownload\"");
    }
    return htmlContent;
  }

  let className = "student-wrapper-iframe";
  if (kids) {
    className = "student-wrapper-iframe-kids";
  }

  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <div className={className}>
            {lesson && <LessonTitle subject={subject} lesson={lesson} />}
            <div className="font-size-changer-location">
            <FontSizeChanger
              targets={["#student-target-one .student-content"]}
              options={{
                stepSize: 2,
                range: 3,
              }}
            />
            </div>
            <div id="student-target-one">
              <div className="student-content">
                <div className="student-student-content-student-font-size-changer"></div>
                <WrapperOverlappingMedia section={concept}>
                  <div className="only-image-inline"
                    onContextMenu={e => e.preventDefault()}
                    dangerouslySetInnerHTML={{
                      __html: concept ? removeDonwloadControlFromVideos(concept.Concept) : "",
                    }}
                  />
                </WrapperOverlappingMedia>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Concept;
