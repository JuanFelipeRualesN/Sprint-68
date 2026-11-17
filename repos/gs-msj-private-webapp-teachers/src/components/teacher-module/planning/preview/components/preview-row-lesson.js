import React, { useContext } from "react";
import PreviewRowAction from "./preview-row-action";
import PreviewRowTitle from "./preview-row-title";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PreviewTableContext } from "../contexts/preview-table-provider";
import { faPenSquare } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import { format, translate } from "../../shared/utils";

export default function PreviewRowLesson({ lessons, customLessons }) {
  const { headers, viewAsPDF } = useContext(PreviewTableContext);

  const handleEditLesson = (lessonId) => {
    console.log(lessonId);
  };

  const handleAddLesson = (date) => {
    console.log(date);
  };

  // Handle custom lessons
  const handleCustomLessons = () => {
    if (!customLessons?.length) return [];
    
    return customLessons.flatMap(lesson => 
      (lesson.TeacherPlanCustomLessonDates).map(dateObj => {
        const dateStr = dateObj.ImplementationDate;
        
        let formattedDate;
        try {
          const date = new Date(dateStr);
          formattedDate = format(date);
        } catch (error) {
          console.error('Invalid format:', dateStr);
        }
        
        return {
          id: lesson.Id,
          type: "custom",
          title: translate("planning.menu-open"),
          description: lesson.Title,
          date: formattedDate
        };
      })
    );
  };
  const formattedCustomLessons = handleCustomLessons();

  // Combine genial lessons and custom lessons
  const allLessons = [...(lessons || []), ...(formattedCustomLessons || [])];

  const LessonRowCell = ({ day }) => {
    const lessonsForDay = allLessons.filter((l) => l.date === day.date);

    return !lessonsForDay || lessonsForDay.length < 1 ? null : (
      <div className="tablePDF__container">
        <ul className="tablePDF__list">
          {lessonsForDay.map((lesson, index) => {
            return (
              <li
                key={`${day.day}-lesson-item-${index}`}
                className="tablePDF__list--item"
              >
                {!viewAsPDF && (
                  <button
                    className={`btn-base btn-onlyPDF ${index !== 0 &&
                      "borderTop"}`}
                    onClick={() => handleEditLesson(lesson.id)}
                  >
                    <FontAwesomeIcon icon={faPenSquare} />
                  </button>
                )}
                <div
                  className={`tablePDF__list--item_desc ${viewAsPDF &&
                    "py-2"} ${index !== 0 && viewAsPDF && "borderTop"}`}
                >
                  <p
                    className={`list--item_desc--title list--item_${
                      lesson.type === "genial" ? "firstTitle" : "secondTitle"
                    }`}
                  >
                    {lesson.title}
                  </p>
                  <p className="list--item_desc--desc">{lesson.description}</p>
                </div>
              </li>
            );
          })}
        </ul>
      </div>
    );
  };

  const hasLessonsToDisplay = allLessons && allLessons.length > 0;

  return (
    <>
      {!viewAsPDF ? (
        <PreviewRowAction
          title={<Translate id="preview.table-add-lessons" />}
          onClick={handleAddLesson}
        >
          <PreviewRowTitle
            rowSpan={!hasLessonsToDisplay ? 1 : 2}
            title={<Translate id="preview.table-lessons" />}
          />
        </PreviewRowAction>
      ) : (
        (
        <tr>
          <PreviewRowTitle
            rowSpan={2}
            title={<Translate id="preview.table-lessons" />}
          />
        </tr>
        )
      )}

      {hasLessonsToDisplay ? (
        <tr>
          {headers.map((header, index) => (
            <td key={`lesson-cell-${index}`}>
              <LessonRowCell day={header} />
            </td>
          ))}
        </tr>
      ) : null}
    </>
  );
}
