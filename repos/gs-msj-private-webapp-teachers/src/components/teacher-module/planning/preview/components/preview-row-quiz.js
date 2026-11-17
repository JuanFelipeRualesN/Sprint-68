import React, { useContext } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PreviewRowAction from "./preview-row-action";
import PreviewRowTitle from "./preview-row-title";
import { PreviewTableContext } from "../contexts/preview-table-provider";
import { faPenSquare } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";

export const QuizRowCell = ({ day, quizzes, viewAsPDF }) => {
  const quizzesForDay = quizzes.filter((l) => l.date === day.date);

  const handleEditQuiz = (quizId) => {
    console.log(quizId);
  };

  return !quizzesForDay || quizzesForDay.length < 1 ? null : (
    <div className="tablePDF__container">
      <ul className="tablePDF__list">
        {quizzesForDay.map((quiz, index) => {
          return (
            <li
              key={`${day.day}-quiz-item-${index}`}
              className="tablePDF__list--item"
            >
              {!viewAsPDF && (
                <button
                  className={`btn-base btn-onlyPDF ${index !== 0 &&
                    "borderTop"}`}
                  onClick={() => handleEditQuiz(quiz.id)}
                >
                  <FontAwesomeIcon icon={faPenSquare} />
                </button>
              )}
              <div
                className={`tablePDF__list--item_desc ${viewAsPDF &&
                  "py-2"} ${index !== 0 && viewAsPDF && "borderTop"}`}
              >
                <p className="list--item_desc--title list--item_firstTitle">
                  {quiz.title}
                </p>
                <p className="list--item_desc--desc_quiz">{quiz.description}</p>
                <p className="list--item_desc--desc_quiz-date">{quiz.time}</p>
              </div>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

export default function PreviewRowQuiz({ quizzes }) {
  const { headers, viewAsPDF } = useContext(PreviewTableContext);

  const handleAddQuiz = (date) => {
    console.log(date);
  };

  const hasPreviewToDQuizay = quizzes && quizzes.length > 0;

  return !viewAsPDF ? (
    <>
      <PreviewRowAction
        title={<Translate id="preview.table-add" />}
        onClick={handleAddQuiz}
      >
        <PreviewRowTitle
          rowSpan={!hasPreviewToDQuizay ? 1 : 2}
          title={<Translate id="preview.table-quizzes" />}
          disclaimer={<Translate id="preview.table-disclaimer" />}
        />
      </PreviewRowAction>

      {hasPreviewToDQuizay ? (
        <tr>
          {headers.map((header, index) => (
            <td key={`quiz-cell-${index}`}>
              <QuizRowCell
                day={header}
                quizzes={quizzes}
                viewAsPDF={viewAsPDF}
              />
            </td>
          ))}
        </tr>
      ) : null}
    </>
  ) : null;
}
