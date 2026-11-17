import React from "react";
import "./style.css";
import { Link } from "react-router-dom";
import {useLocation } from "react-router-dom";
import {Translate} from "react-localize-redux";
import { useSelector } from "react-redux";
import { hasRole, getUser } from "../../../student-module/view/lesson-student/utils/utils";
import Settings from "../../../../custom-classes/settings";
import { useHistory } from "react-router-dom";
import { getAsset } from "../../../../utils/getAssets";

const IconBack = getAsset('LessonsMenu-icon-Back');
const IconLessonMark = getAsset('Homework-icon-LessonMark');
const IconKinder = getAsset('NavMenu-icon-Kinder');
const IconLessons = getAsset('NavMenu-icon-Lessons');
const IconLibrary = getAsset('NavMenu-icon-Library');

const Menu = () => {
  const location = useLocation();
  const userCurrent = getUser();
  const { grade, user, parameters } = useSelector((state) => ({
    grade: state.grade,
    user: state.user,
    parameters: state.parameters,
  }));

  return (
    <>
      <div className="wrapper-menu wrapper-menu-director">
        <ul className="navigation-menu">
          {hasRole(userCurrent, [Settings.ROLE_STUDENT_STR, Settings.ROLE_TEACHER_STR, Settings.ROLE_DIRECTOR_STR]) && <li
            className={
              location.pathname.includes("/students/lesson") ? "active" : ""
            }
          >
            <Link to="/students/lesson">
              <img src={IconLessons} />
              <span className="navigation-menu--item"> <Translate id="student-courses.lessons" options={{ renderInnerHtml: true }} /> </span>
            </Link>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>}
          {hasRole(userCurrent, [Settings.ROLE_STUDENT_STR, Settings.ROLE_DIRECTOR_STR]) && <li
            className={
              location.pathname.includes("/students/library") ? "active" : ""
            }
          >
            <Link to="/students/library">
              <img src={IconLibrary} />
              <span className="navigation-menu--item"> <Translate id="student-courses.library" options={{ renderInnerHtml: true }} /> </span>
            </Link>
            <span className="lesson-mark">
              <img src={IconLessonMark} />
            </span>
          </li>}
          {hasRole(userCurrent, [Settings.ROLE_STUDENT_STR, Settings.ROLE_DIRECTOR_STR]) && grade === "k" && (
            <li
              className={
                location.pathname.includes("/students/kinder-interactive")
                  ? "active"
                  : ""
              }
            >
              <Link to="/students/kinder-interactive" className="nav-kinder">
                <img src={IconKinder} />
                <span className="navigation-menu--item"> Kinder </span>
              </Link>
              <span className="lesson-mark">
                <img src={IconLessonMark} />
              </span>
            </li>
          )}
          <li>
            <Link to="/academic-content-select">
              <img src={IconBack} />
              <span className="navigation-menu--item"><Translate id="general.back-action" options={{ renderInnerHtml: true }} /></span>
            </Link>
          </li>
        </ul>
      </div>
    </>
  );
};

export default Menu;