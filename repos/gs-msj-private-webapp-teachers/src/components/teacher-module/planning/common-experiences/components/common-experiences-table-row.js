import React, { useState } from "react";
import CommonExperiencesDetail from "./common-experiences-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteCommonExperience from "../hooks/use-delete-common-experiences";
import useUpdateCommonExperience from "../hooks/use-update-common-experiences";
import SessionManager from '../../../../../custom-classes/session-manager';

export default function CommonExperiencesTableRow({ commonExperience, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(
    commonExperience.implementationDates
  );
  const { deleteCommonExperience, isLoading: isDeleting } = useDeleteCommonExperience();
  const { updateCommonExperience, isLoading: isUpdating } = useUpdateCommonExperience();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(commonExperience);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>{commonExperience.detail}</td>
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
            updateCommonExperience({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <CommonExperiencesDetail commonExperience={commonExperience} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"common-experience.modal-delete-title"}
          description={"common-experience.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteCommonExperience(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
