import React, { useState } from "react";
import DifferentiationStrategiesDetail from "./differentiation-strategies-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteDifferentiationStrategy from "../hooks/use-delete-differentiation-strategies";
import useUpdateDifferentiationStrategy from "../hooks/use-update-differentiation-strategies";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from 'react-localize-redux';

export default function DifferentiationStrategiesTableRow({ differentiationStrategy, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(differentiationStrategy.implementationDates);
  const { deleteDifferentiationStrategy, isLoading: isDeleting } = useDeleteDifferentiationStrategy();
  const { updateDifferentiationStrategy, isLoading: isUpdating } = useUpdateDifferentiationStrategy();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(differentiationStrategy);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>{differentiationStrategy.category}</td>
      <td data-th={headers[1]} className="w-50">
        {differentiationStrategy.title}
        {differentiationStrategy.subtitle && (
          <div>
            <strong>
              <Translate id="general.other" />:{" "}
            </strong>
            {differentiationStrategy.subtitle}
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
            updateDifferentiationStrategy({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <DifferentiationStrategiesDetail differentiationStrategy={differentiationStrategy} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"strategy.modal-delete-title"}
          description={"strategy.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteDifferentiationStrategy(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
