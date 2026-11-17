import React, { useState } from "react";
import StrategiesDetail from "./strategies-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteStrategy from "../hooks/use-delete-strategy";
import useUpdateStrategy from "../hooks/use-update-strategy";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from 'react-localize-redux';

export default function StrategiesTableRow({ strategy, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(strategy.implementationDates);
  const { deleteStrategy, isLoading: isDeleting } = useDeleteStrategy();
  const { updateStrategy, isLoading: isUpdating } = useUpdateStrategy();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(strategy);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>{strategy.category}</td>
      <td data-th={headers[1]} className="w-50">
        {strategy.title}
        {strategy.subtitle && (
          <div>
            <strong>
              <Translate id="general.other" />:{" "}
            </strong>
            {strategy.subtitle}
          </div>
        )}
      </td>
      <td data-th={headers[2]} className="text-capitalize">
        {formatWithDayName(minDate)}
      </td>
      <td data-th={headers[3]} className="text-capitalize">
        {formatWithDayName(maxDate)}
      </td>
      <td className="table-ppal--td-actions">
        <EditModal
          editButtonText={"general.viewOrEdit"}
          title={"general.viewOrEdit"}
          confirmButtonText={"confirm-dialog.save"}
          cancelButtonText={"confirm-dialog.button-cancel"}
          onConfirmation={() =>
            updateStrategy({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <StrategiesDetail strategy={strategy} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"strategy.modal-delete-title"}
          description={"strategy.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteStrategy(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
