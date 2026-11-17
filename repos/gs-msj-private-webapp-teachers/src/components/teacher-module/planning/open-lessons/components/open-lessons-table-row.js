import React, { useState } from "react";
import OpenLessonsDetail from "./open-lessons-detail";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteOpenLesson from "../hooks/use-delete-open-lesson";
import useUpdateOpenLesson from "../hooks/use-update-open-lesson";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import SessionManager from "../../../../../custom-classes/session-manager";

export default function OpenLessonsTableRow({
  lesson,
  headers,
  onChange,
}) {
  const { minDate, maxDate } = getMinAndMaxDates(
    lesson.implementationDates
  );
  const { deleteOpenLesson, isLoading: isDeleting } = useDeleteOpenLesson();
  const { updateOpenLesson, isLoading: isUpdating } = useUpdateOpenLesson();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(lesson);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]} className="w-50">
        {lesson.title}
      </td>
      <td data-th={headers[1]} className="text-capitalize">
        {formatWithDayName(minDate)}
      </td>
      <td data-th={headers[2]} className="text-capitalize">
        {formatWithDayName(maxDate)}
      </td>
      <td className="table-ppal--td-actions">
        <EditModal
          editButtonText={"general.viewOrEdit"}
          title={"general.viewOrEdit"}
          confirmButtonText={"confirm-dialog.save"}
          cancelButtonText={"confirm-dialog.button-cancel"}
          onConfirmation={() =>
            updateOpenLesson({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <OpenLessonsDetail
            lesson={lesson}
            onChange={handleChange}
          />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"lesson.modal-delete-title"}
          description={"lesson.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteOpenLesson(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
