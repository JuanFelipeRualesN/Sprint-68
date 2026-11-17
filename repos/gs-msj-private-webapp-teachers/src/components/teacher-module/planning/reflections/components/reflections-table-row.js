import React, { useState } from "react";
import ReflectionsDetail from "./reflections-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteReflection from "../hooks/use-delete-reflection";
import useUpdateReflection from "../hooks/use-update-reflection";
import SessionManager from '../../../../../custom-classes/session-manager';

export default function ReflectionsTableRow({ reflection, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(
    reflection.implementationDates
  );
  const { deleteReflection, isLoading: isDeleting } = useDeleteReflection();
  const { updateReflection, isLoading: isUpdating } = useUpdateReflection();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(reflection);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>{reflection.detail}</td>
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
            updateReflection({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <ReflectionsDetail reflection={reflection} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"reflection.modal-delete-title"}
          description={"reflection.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteReflection(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
