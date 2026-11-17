import React from "react";
import "./style.css";
import { useHistory } from "react-router-dom";

const LessonItem = ({ lesson }) => {
  const history = useHistory();

  const goTo = (path) => {
    history.push(path);
  };

  return (
    <>
      
        <div className="container-items-lesson">
          <a onClick={() => goTo(`/students/lesson/${lesson.Id}`)}>
            
            <div className="col-right--items-lesson">
              <i className="icon-finished-lesson fas fa-check-circle"></i>
            </div>
            <div className="col-left--items-lesson">{lesson.LessonTitle}</div>
          </a>
        </div>
      
    </>
  );
};

export default LessonItem;