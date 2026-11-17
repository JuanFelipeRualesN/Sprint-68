import React, { useState } from "react";
import MaterialsDetail from "./materials-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteMaterial from "../hooks/use-delete-material";
import useUpdateMaterial from "../hooks/use-update-material";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from 'react-localize-redux';

export default function MaterialsTableRow({ material, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(material.implementationDates);
  const { deleteMaterial, isLoading: isDeleting } = useDeleteMaterial();
  const { updateMaterial, isLoading: isUpdating } = useUpdateMaterial();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(material);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]} className="w-50">
        {material.title}
        {material.subtitle && (
          <div>
            <strong>
              <Translate id="general.other" />:{" "}
            </strong>
            {material.subtitle}
          </div>
        )}
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
            updateMaterial({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <MaterialsDetail material={material} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"material.modal-delete-title"}
          description={"material.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteMaterial(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
