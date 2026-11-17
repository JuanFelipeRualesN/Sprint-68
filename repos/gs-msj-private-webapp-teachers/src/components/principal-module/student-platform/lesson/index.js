import React, { useEffect } from "react";
import "./style.css";
import { getSubjectsAndCoursesThuk } from "../../../../redux-students-area/actions/subjects";
import { useSelector, useDispatch } from "react-redux";
import {Translate} from "react-localize-redux";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import LessonList from "./lessons";
import Subjects from "./subjects";
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";

const Lesson = () => {
  const dispatch = useDispatch();
  const { subjects } = useSelector((state) => ({
    subjects: state.subjects,
  }));
  
  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    dispatch(getSubjectsAndCoursesThuk());
  }, []);

  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <div className="wrapper-message-tabs content-max-width">
            <div className="message-tabs">
              <label><Translate id="student-platform.lessons" options={{ renderInnerHtml: true }} /></label>
              <div className="message-tabs--content">
                <div className="wrapper-lesson-tabs">
                      <div className="container_two-col">
                        <Subjects subjects={subjects} />
                      </div>
                      <div className="container_two-col">
                        <LessonList />
                      </div>
                    </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default withLocalize(Lesson);