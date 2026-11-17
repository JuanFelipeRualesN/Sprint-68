import React, { useState, useEffect } from "react";
import "./style.css";
import styled from "styled-components";
import LessonTitle from "../lesson-title";
import FontSizeChanger from "react-font-size-changer";
import {getLessonByIdDefinitions } from "../../../../../api/gsweb";
import WrapperOverlappingMedia from "../../../../teacher-module/wrapper-overlapping-media";

const ContainerIframe = styled.div`
  position: relative;
  width: 100%;
  overflow: hidden;
  padding-top: 56.25%;

  @media (max-width: 450px) {
    padding-top: 100%;
  }
`;

const IFrameTemplateLesson = styled.iframe`
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  width: 100%;
  height: 100%;
  border: none;
`;

const VocabularyInnerHTML = ({ name, desc }) => {
  return (
    <>
      {name && <div className="student-vocabulary-title">{name}</div>}
      <WrapperOverlappingMedia section={desc}>
        <div className="only-image-inline"
          dangerouslySetInnerHTML={{
            __html: desc,
          }}
        />
      </WrapperOverlappingMedia>
    </>
  );
};

const IFrame = ({ lesson, kids, subject, definition }) => {
  let className = "student-wrapper-iframe--FullWidth";
  if (kids) {
    className = "student-wrapper-iframe-kids--FullWidth";
  }
  return (
    <div className="student-wrapper-student-content">
      <div className="student-center-container">
        <div className={className}>
          {lesson && <LessonTitle subject={subject} lesson={lesson} />}
          {definition &&
            definition.LessonDefinitionModelList.map((def, i) => (
              <ContainerIframe key={i}>
                <IFrameTemplateLesson src={def.Desc} />
              </ContainerIframe>
            ))}
        </div>
      </div>
    </div>
  );
};

const Vocabulary = ({ lesson, kids, subject, lessonId, setLoading, loading }) => {
  const [definition, setDefinition] = useState(null);

  useEffect(() => {
    getDefinitionByLesson();
  }, []);

  const getDefinitionByLesson = async () => {
    setLoading(true);
    try {
      const res = await getLessonByIdDefinitions(lessonId);
      const data = res.data;
      setDefinition(data);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  if (definition && definition.IsDefinitionALink) {
    return <IFrame lesson={lesson} kids={kids} subject={subject} definition={definition} />;
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
                {definition &&
                  definition.LessonDefinitionModelList.map((def, i) => (
                    <VocabularyInnerHTML
                      key={i}
                      name={def.Name}
                      desc={def.Desc}
                    />
                  ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Vocabulary;
