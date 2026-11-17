import React, { useState } from "react";
import QuizzesDetail from "./quizzes-detail";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import CheckBox from "../../shared/components/form/form-checkbox";
import Text from "../../shared/components/text";
import useDeleteQuiz from "../hooks/useDeleteQuiz";
import useUpdateQuiz from "../hooks/useUpdateQuiz";
import { formatWithDayName } from "../../shared/utils";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";

export default function QuizzesTableRow({ quiz, headers, onChange }) {
  const { deleteQuiz, isLoading: isDeleting } = useDeleteQuiz();
  const { updateQuiz, isLoading: isUpdating } = useUpdateQuiz();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(quiz);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>{data.lesson}</td>
      <td data-th={headers[1]}>{data.name}</td>
      <td data-th={headers[2]} className="text-capitalize">
        {formatWithDayName(data.date)}
      </td>
      <td data-th={headers[3]}>
        <CheckBox
          className="count-for-grade"
          name={`countForGrade-${data.id}`}
          checked={data.countForGrade}
          disabled
        />
      </td>
      <td className="table-ppal--td-actions">
        <EditModal
          editButtonText={"general.viewOrEdit"}
          title={"general.viewOrEdit"}
          confirmButtonText={"confirm-dialog.save"}
          cancelButtonText={"confirm-dialog.button-cancel"}
          onConfirmation={() =>
            updateQuiz({
              planningId,
              lessonId: data.lessonId,
              dueDate: data.lessonDueDate,
              quizzes: [{ ...data }],
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <Text type="regular">
            <span className="fs-4">
              {<Translate id="quiz.modal-lesson-message" />}:{" "}
              <strong>{data.lesson}</strong>
            </span>
          </Text>
          <QuizzesDetail
            quiz={{ ...data, isActive: true }}
            isForEdit={true}
            onChange={handleChange}
          />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"quiz.modal-delete-title"}
          description={"quiz.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteQuiz(planningId, data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
