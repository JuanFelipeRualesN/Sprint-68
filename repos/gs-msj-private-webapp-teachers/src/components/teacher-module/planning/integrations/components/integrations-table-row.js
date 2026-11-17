import React, { useState } from "react";
import IntegrationsDetail from "./integrations-detail";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteIntegration from "../hooks/use-delete-integration";
import useUpdateIntegration from "../hooks/use-update-integration";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import SessionManager from "../../../../../custom-classes/session-manager";

export default function IntegrationsTableRow({
  integration,
  headers,
  onChange,
}) {
  const { minDate, maxDate } = getMinAndMaxDates(
    integration.implementationDates
  );
  const { deleteIntegration, isLoading: isDeleting } = useDeleteIntegration();
  const { updateIntegration, isLoading: isUpdating } = useUpdateIntegration();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(integration);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]} className="w-50">
        {integration.title}
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
            updateIntegration({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <IntegrationsDetail
            integration={integration}
            onChange={handleChange}
          />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"integration.modal-delete-title"}
          description={"integration.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteIntegration(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
