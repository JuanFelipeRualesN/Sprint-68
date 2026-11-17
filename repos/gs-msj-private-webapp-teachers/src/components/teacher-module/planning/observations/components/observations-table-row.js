import React, { useState } from "react";
import ObservationsDetail from "./observations-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteObservations from "../hooks/use-delete-observations";
import useUpdateObservations from "../hooks/use-update-observations";
import SessionManager from '../../../../../custom-classes/session-manager';

export default function ObservationsTableRow({ observation, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(
    observation.implementationDates
  );
  const { deleteObservation, isLoading: isDeleting } = useDeleteObservations();
  const { updateObservation, isLoading: isUpdating } = useUpdateObservations();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(observation);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>{observation.detail}</td>
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
            updateObservation({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <ObservationsDetail observation={observation} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"observation.modal-delete-title"}
          description={"observation.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteObservation(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
