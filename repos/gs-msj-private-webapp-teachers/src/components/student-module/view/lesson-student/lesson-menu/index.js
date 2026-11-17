import React from "react";
import "./style.css";
import { useHistory } from "react-router-dom";
import { Translate } from "react-localize-redux";
import { hasRole, getUser } from "../utils/utils";
import Settings from "../../../../../custom-classes/settings";
import { getAsset } from "../../../../../utils/getAssets";

const IconConcept = getAsset('LessonsMenu-icon-Concept');
const IconVocabulary = getAsset('LessonsMenu-icon-Vocabulary');
const IconFinish = getAsset('LessonsMenu-icon-Finish');
const IconDownload = getAsset('LessonsMenu-icon-Download');
const IconAditionalMaterial = getAsset('LessonsMenu-icon-AdditionalMaterial');
const IconChallenge = getAsset('LessonsMenu-icon-Challenge');
const IconPractice = getAsset('LessonsMenu-icon-Practice');
const IconExample = getAsset('LessonsMenu-icon-Examples');
const IconLessonMark = getAsset('Homework-icon-LessonMark');

const LessonMenu = ({ tabActive, setTabActive, fromLearningGap, studentId, studentName }) => {

  const history = useHistory();
  const userCurrent = getUser();

  const goBack = () => {
    if(fromLearningGap) {
      let urlBack = `/learning-gap?tab=2`;
        if(typeof studentId !== 'undefined' && studentId !== null) {
          urlBack += `&studentId=${studentId}`;
        }
        if(typeof studentName !== 'undefined' && studentName !== null) {
            urlBack += `&studentName=${studentName}`;
        }
      history.push(urlBack);
    }
    else if (hasRole(userCurrent, [Settings.ROLE_DIRECTOR_STR])) {
      history.push("/students/lesson");
    }
    else
      history.push("/v2/plannings-detail?page=lessons");
  };

  return (
    <>
      <div className="student-wrapper-menu">
        <ul className="student-navigation-menu">
          <li className={tabActive === "concept" ? "active" : ""}>
            <a onClick={() => setTabActive("concept")}>
              {" "}
              <img src={IconConcept} />{" "}
              <span className="student-navigation-menu--item">
              <Translate id="student.concept"
                         options={{ renderInnerHtml: true }}
              />
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className={tabActive === "vocabulary" ? "active" : ""}>
            <a onClick={() => setTabActive("vocabulary")}>
              {" "}
              <img src={IconVocabulary} />
              <span className="student-navigation-menu--item">
               <Translate id="student.vocabulary"
                          options={{ renderInnerHtml: true }}
               />
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className={tabActive === "example" ? "active" : ""}>
            <a onClick={() => setTabActive("example")}>
              {" "}
              <img src={IconExample} />
              <span className="student-navigation-menu--item">
               <Translate id="student.examples"
                          options={{ renderInnerHtml: true }}
               />
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className={tabActive === "practice" ? "active" : ""}>
            <a onClick={() => setTabActive("practice")}>
              {" "}
              <img src={IconPractice} />
              <span className="student-navigation-menu--item">
               <Translate id="student.practices"
                          options={{ renderInnerHtml: true }}
               />
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className={tabActive === "challenge" ? "active" : ""}>
            <a onClick={() => setTabActive("challenge")}>
              {" "}
              <img src={IconChallenge} />
              <span className="student-navigation-menu--item">  <Translate id="student.exercises"
                                                                           options={{ renderInnerHtml: true }}
              /> </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className={tabActive === "additional" ? "active" : ""}>
            <a onClick={() => setTabActive("additional")}>
              {" "}
              <img src={IconAditionalMaterial} />
              <span className="student-navigation-menu--item">
              <Translate id="student.additional-materials"
                         options={{ renderInnerHtml: true }}
              />
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className={tabActive === "download" ? "active" : ""}>
            <a onClick={() => setTabActive("download")}>
              {" "}
              <img src={IconDownload} />
              <span className="student-navigation-menu--item">
                {fromLearningGap ? <Translate id="student.download-phase" /> :
              <Translate id="student.download-lesson"
                         options={{ renderInnerHtml: true }}
              />  }
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
          <li className="highlight-item">
            <a onClick={goBack}>
              {" "}
              <img src={IconFinish} />
              <span className="student-navigation-menu--item">
              <Translate id="student.finish"
                         options={{ renderInnerHtml: true }}
              />
              </span>
            </a>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>
        </ul>
      </div>
    </>
  );
};

export default LessonMenu;
